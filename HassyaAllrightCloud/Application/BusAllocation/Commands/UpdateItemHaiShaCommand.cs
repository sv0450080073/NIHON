using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.IService;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BusAllocation.Commands
{
    public class UpdateItemHaiShaCommand : IRequest<IActionResult>
    {
        public TkdHaisha TkdHaisha { get; set; }
        public TkdUnkobi TkdUnkobi { get; set; }
        public TkdYyksho TkdYyksho { get; set; }
        public Dictionary<CommandMode, List<TkdKoban>> TkdKoban { get; set; }
        public BusAllocationDataUpdate BusAllocationDataUpdate { get; set; }

        public class Handler : IRequestHandler<UpdateItemHaiShaCommand, IActionResult>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<UpdateItemHaiShaCommand> _logger;
            private readonly ITKD_HaishaDataListService _TKD_HaishaDataService;
           
            public Handler(KobodbContext context, ILogger<UpdateItemHaiShaCommand> logger, ITKD_HaishaDataListService TKD_HaishaDataService)
            {
                _context = context;
                _logger = logger;
                _TKD_HaishaDataService = TKD_HaishaDataService;
            }
            public async Task<IActionResult> Handle(UpdateItemHaiShaCommand command, CancellationToken cancellationToken)
            {
                //Add custom field to HaiSha
                if (command.BusAllocationDataUpdate.CustomData.Count() > 0)
                {
                    foreach (var fieldValue in command.BusAllocationDataUpdate.CustomData)
                    {
                        _context.Entry(command.TkdHaisha).Property($"CustomItems{fieldValue.Key}").CurrentValue = fieldValue.Value;
                    }
                }
                using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbTran = _context.Database.BeginTransaction())
                {
                    try
                    {
                        await _TKD_HaishaDataService.updateHaiindata(command.BusAllocationDataUpdate, command.BusAllocationDataUpdate.CheckDeleteDriver);
                        _context.TkdHaisha.Update(command.TkdHaisha);
                        //await _context.SaveChangesAsync();
                        /*Update Haiskbn...*/
                        _context.TkdUnkobi.Update(command.TkdUnkobi);
                        //await _context.SaveChangesAsync();
                        _context.TkdYyksho.Update(command.TkdYyksho);
                        //await _context.SaveChangesAsync();
                        if (command.TkdKoban[CommandMode.Delete].Count > 0)
                        {
                            _context.TkdKoban.RemoveRange(command.TkdKoban[CommandMode.Delete]);
                        }
                        if (command.TkdKoban[CommandMode.Create].Count > 0)
                        {
                            _context.TkdKoban.AddRange(command.TkdKoban[CommandMode.Create]);
                        }
                        if (command.TkdKoban[CommandMode.Update].Count > 0)
                        {
                            _context.TkdKoban.UpdateRange(command.TkdKoban[CommandMode.Update]);
                        }
                        //await _context.SaveChangesAsync();
                        //Update KouBnRen                       
                        Dictionary<CommandMode, List<TkdKoban>> tkdKobanUpdate = new Dictionary<CommandMode, List<TkdKoban>>();
                        tkdKobanUpdate = UpdateKouBnRen(command.TkdHaisha);
                        if (tkdKobanUpdate[CommandMode.Delete].Count > 0)
                        {
                            _context.TkdKoban.RemoveRange(tkdKobanUpdate[CommandMode.Delete]);
                        }
                        if (tkdKobanUpdate[CommandMode.Create].Count > 0)
                        {
                            _context.TkdKoban.AddRange(tkdKobanUpdate[CommandMode.Create]);
                        }
                        await _TKD_HaishaDataService.UpdateTKD_KobanBusallocation(command.BusAllocationDataUpdate);
                        await _context.SaveChangesAsync();
                        dbTran.Commit();
                    }
                    catch (DbUpdateConcurrencyException Ex)
                    {
                        _logger.LogError(Ex, Ex.Message);
                        dbTran.Rollback();
                        throw;
                    }
                    catch (Exception ex)
                    {
                        //Rollback transaction if exception occurs  
                        _logger.LogTrace(ex.ToString());
                        dbTran.Rollback();
                        throw;
                        //return new BadRequestResult();
                    }
                }
                return new ContentResult { Content = command.TkdHaisha.UkeNo };
            }

            public Dictionary<CommandMode, List<TkdKoban>> UpdateKouBnRen(TkdHaisha tkdHaishaUpdate)
            {
                try
                {
                    Dictionary<CommandMode, List<TkdKoban>> result = new Dictionary<CommandMode, List<TkdKoban>>();
                    List<TkdKoban> addNewKobanList = new List<TkdKoban>();
                    List<TkdKoban> removeKobanList = new List<TkdKoban>();
                    List<TkdKoban> updateKobanList = new List<TkdKoban>();
                    var kobanDataList = _context.TkdKoban.Where(x => x.UkeNo == tkdHaishaUpdate.UkeNo
                                                      && x.UnkRen == tkdHaishaUpdate.UnkRen
                                                      && x.TeiDanNo == tkdHaishaUpdate.TeiDanNo
                                                      && x.BunkRen == tkdHaishaUpdate.BunkRen).ToList();
                    if (kobanDataList.Count != 0)
                    {
                        DateTime startdate;
                        DateTime.TryParseExact(tkdHaishaUpdate.SyuKoYmd,
                                       "yyyyMMdd",
                                       CultureInfo.CurrentCulture,
                                       DateTimeStyles.None,
                                       out startdate);

                        DateTime enddate;
                        DateTime.TryParseExact(tkdHaishaUpdate.KikYmd,
                                       "yyyyMMdd",
                                       CultureInfo.CurrentCulture,
                                       DateTimeStyles.None,
                                       out enddate);
                        var employeeCode = kobanDataList.GroupBy(x => new { x.SyainCdSeq, x.UkeNo, x.UnkRen, x.BunkRen, x.TeiDanNo }).Select(x => x.First()).ToList();
                        foreach (var data in employeeCode)
                        {
                            for (DateTime date = startdate; date <= enddate; date = date.AddDays(1))
                            {
                                var kobanDataByEmployeeList = _context.TkdKoban.OrderBy(t => t.SyukinYmd).ThenBy(t => t.SyukinTime)
                                    .Where(x => x.SyainCdSeq == data.SyainCdSeq && x.UnkYmd == date.ToString("yyyyMMdd")).ToList();

                                var tempRemoveList = kobanDataByEmployeeList;
                                removeKobanList.AddRange(kobanDataByEmployeeList);
                                if (kobanDataByEmployeeList.Count != 0)
                                {                                    
                                    var kobanNewList = SetKouBnRen(kobanDataByEmployeeList);
                                    addNewKobanList.AddRange(kobanNewList);
                                }
                                //removeKobanList.AddRange(tempRemoveList);
                            }
                        }
                        addNewKobanList = addNewKobanList.OrderBy(x => x.UnkYmd).ToList();
                    }
                    result.Add(CommandMode.Create, addNewKobanList);
                    result.Add(CommandMode.Update, updateKobanList);
                    result.Add(CommandMode.Delete, removeKobanList);
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public List<TkdKoban> SetKouBnRen(List<TkdKoban> tkdKobanList)
            {
                List<TkdKoban> result = new List<TkdKoban>();
                int i = 1;
                foreach (var item in tkdKobanList)
                {                   
                    TkdKoban newitem = new TkdKoban();
                    newitem.UnkYmd = item.UnkYmd;
                    newitem.SyainCdSeq = item.SyainCdSeq;                 
                    newitem.KouBnRen = (short)i++;
                    newitem.HenKai = item.HenKai;
                    newitem.SyugyoKbn = item.SyugyoKbn;
                    newitem.KinKyuTblCdSeq = item.KinKyuTblCdSeq;
                    newitem.UkeNo = item.UkeNo;
                    newitem.UnkRen = item.UnkRen;
                    newitem.SyaSyuRen = item.SyaSyuRen;
                    newitem.TeiDanNo = item.TeiDanNo;
                    newitem.BunkRen = item.BunkRen;
                    newitem.RotCdSeq = item.RotCdSeq;
                    newitem.RenEigCd = item.RenEigCd;
                    newitem.SigySyu = item.SigySyu;
                    newitem.KitYmd = item.KitYmd;
                    newitem.SigyKbn = item.SigyKbn;
                    newitem.SiyoKbn = item.SiyoKbn;
                    newitem.SigyCd = item.SigyCd;
                    newitem.KouZokPtnKbn = item.KouZokPtnKbn;
                    newitem.SyukinYmd = item.SyukinYmd;
                    newitem.TaikinYmd = item.TaikinYmd;
                    newitem.SyukinTime = item.SyukinTime;
                    newitem.TaiknTime = item.TaiknTime;
                    newitem.FuriYmd = item.FuriYmd;
                    newitem.RouTime = item.RouTime;
                    newitem.KouStime = item.KouStime;
                    newitem.TaikTime = item.TaikTime;
                    newitem.KyuKtime = item.KyuKtime;
                    newitem.JitdTime = item.JitdTime;
                    newitem.ZangTime = item.ZangTime;
                    newitem.UsinyTime = item.UsinyTime;
                    newitem.Syukinbasy = item.Syukinbasy;
                    newitem.SsinTime = item.SsinTime;
                    newitem.BikoNm = item.BikoNm;
                    newitem.TaiknBasy = item.TaiknBasy;
                    newitem.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    newitem.UpdTime = DateTime.Now.ToString("HHmm");
                    newitem.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    newitem.UpdPrgId = "KU0600";
                    result.Add(newitem);
                }
                return result;
            }

            public int GetSyokumuCdSeq(string date, int syainCdSeq)
            {
                return (from KYOSHE in _context.VpmKyoShe
                        join VPMSyokum in _context.VpmSyokum on 
                        new { C1 = KYOSHE.SyokumuCdSeq, C2 = new ClaimModel().TenantID } equals new { C1= VPMSyokum.SyokumuCdSeq, C2 = VPMSyokum.TenantCdSeq }   
                    where string.Compare(KYOSHE.StaYmd, date) <= 0 &&
                                                             string.Compare(KYOSHE.EndYmd, date) >= 0 &&
                                                             KYOSHE.SyainCdSeq == syainCdSeq
                        select VPMSyokum.SyokumuKbn).First();
            }
        }
    }
}
