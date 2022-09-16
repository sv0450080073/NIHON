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

namespace HassyaAllrightCloud.Application.BusAllocation.Queries
{
    public class UpdateListHaiShaCommand : IRequest<IActionResult>
    {
        public List<TkdHaisha> TkdHaishaList { get; set; }
        public TkdUnkobi TkdUnkobi { get; set; }
        public TkdYyksho TkdYyksho { get; set; }
        public Dictionary<CommandMode, List<TkdKoban>> TkdKoban { get; set; }
        public bool IsUpdateAll { get; set; } = true;
        public BusAllocationDataUpdateAll BusAllocationDataUpdateAll { get; set; }
        public BusAllocationDataGrid DataSourceItem { get; set; }
        public class Handler : IRequestHandler<UpdateListHaiShaCommand, IActionResult>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<UpdateListHaiShaCommand> _logger;
            private readonly IBusBookingDataListService _BusBookingDataService;
            public Handler(KobodbContext context, ILogger<UpdateListHaiShaCommand> logger, IBusBookingDataListService BusBookingDataService)
            {
                _context = context;
                _logger = logger;
                _BusBookingDataService = BusBookingDataService;
            }

            public async Task<IActionResult> Handle(UpdateListHaiShaCommand command, CancellationToken cancellationToken)
            {
                /*CustomItem HaiSha*/
                if (command.TkdHaishaList?.Count() > 0)
                {
                    foreach (var item in command.TkdHaishaList)
                    {
                        if (command.IsUpdateAll)
                        {
                            if (command.BusAllocationDataUpdateAll.CustomData.Count() > 0)
                            {
                                foreach (var fieldValue in command.BusAllocationDataUpdateAll.CustomData)
                                {
                                    if (fieldValue.Value != "")
                                    {
                                        _context.Entry(item).Property($"CustomItems{fieldValue.Key}").CurrentValue = fieldValue.Value;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (command.DataSourceItem.CustomData.Count() > 0)
                            {
                                foreach (var fieldValue in command.DataSourceItem.CustomData)
                                {
                                    _context.Entry(item).Property($"CustomItems{fieldValue.Key}").CurrentValue = fieldValue.Value;
                                }
                            }
                        }

                    }
                }
                using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbTran = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (command.TkdHaishaList.Count() > 0)
                        {
                            foreach (var item in command.TkdHaishaList)
                            {
                                item.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                                item.UpdTime = DateTime.Now.ToString("HHmmss");
                                _context.TkdHaisha.Update(item);
                            }
                        }
                        //await _context.SaveChangesAsync();
                        //command.TkdUnkobi = SetTkdUnkobiData(command.TkdUnkobi, command.TkdUnkobi.UkeNo, command.TkdUnkobi.UnkRen);
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
                        if (command.TkdKoban[CommandMode.Delete].Count > 0 && command.TkdKoban[CommandMode.Create].Count > 0)
                        {
                            Dictionary<CommandMode, List<TkdKoban>> tkdKobanUpdate = UpdateKouBnRen(command.TkdHaishaList);

                            if (tkdKobanUpdate[CommandMode.Delete].Count > 0)
                            {
                                _context.TkdKoban.RemoveRange(tkdKobanUpdate[CommandMode.Delete]);
                            }
                            if (tkdKobanUpdate[CommandMode.Create].Count > 0)
                            {
                                _context.TkdKoban.AddRange(tkdKobanUpdate[CommandMode.Create]);
                            }
                        }
                        //await _context.SaveChangesAsync();
                        if (command.IsUpdateAll)
                        {
                            _BusBookingDataService.UpdateUnkobi(command.TkdYyksho.UkeNo, command.BusAllocationDataUpdateAll.HAISHA_UnkRen, "KU0600");
                            _BusBookingDataService.UpdateYyksho(command.TkdYyksho.UkeNo, new HassyaAllrightCloud.Domain.Dto.ClaimModel().TenantID, "KU0600");
                        }
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
                        return new BadRequestResult();
                    }
                } 
                return new ContentResult { Content = command.TkdUnkobi.UkeNo };

            }
            public Dictionary<CommandMode, List<TkdKoban>> UpdateKouBnRen(List<TkdHaisha> tkdHaishaUpdateList)
            {
                try
                {
                    Dictionary<CommandMode, List<TkdKoban>> result = new Dictionary<CommandMode, List<TkdKoban>>();
                    List<TkdKoban> addNewKobanList = new List<TkdKoban>();
                    List<TkdKoban> removeKobanList = new List<TkdKoban>();
                    List<TkdKoban> updateKobanList = new List<TkdKoban>();
                    foreach (var item in tkdHaishaUpdateList)
                    {
                        var kobanDataList = _context.TkdKoban.Where(x => x.UkeNo == item.UkeNo
                                                        && x.UnkRen == item.UnkRen
                                                        && x.TeiDanNo == item.TeiDanNo
                                                        && x.BunkRen == item.BunkRen).ToList();
                        if (kobanDataList.Count != 0)
                        {
                            DateTime startdate;
                            DateTime.TryParseExact(item.SyuKoYmd,
                                           "yyyyMMdd",
                                           CultureInfo.CurrentCulture,
                                           DateTimeStyles.None,
                                           out startdate);

                            DateTime enddate;
                            DateTime.TryParseExact(item.KikYmd,
                                           "yyyyMMdd",
                                           CultureInfo.CurrentCulture,
                                           DateTimeStyles.None,
                                           out enddate);
                            List<int> employeeCode = kobanDataList.Distinct().Select(x => x.SyainCdSeq).ToList();

                            for (int i = 0; i < employeeCode.Count; i++)
                            {
                                for (DateTime date = startdate; date <= enddate; date = date.AddDays(1))
                                {
                                    var kobanDataByEmployeeList = _context.TkdKoban.OrderBy(t => t.SyukinYmd).ThenBy(t => t.SyukinTime)
                                        .Where(x => x.SyainCdSeq == employeeCode[i] && x.UnkYmd == date.ToString("yyyyMMdd")).ToList();
                                    removeKobanList.AddRange(kobanDataByEmployeeList);
                                    if (kobanDataByEmployeeList.Count != 0)
                                    {
                                        var kobanNewList = SetKouBnRen(kobanDataByEmployeeList);
                                        addNewKobanList.AddRange(kobanNewList);
                                    }
                                }
                            }
                        }
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
            private TkdUnkobi SetTkdUnkobiData(TkdUnkobi tkdUnkobi, string ukeno, short unkRen)
            {
                if (tkdUnkobi != null && ukeno != "" && unkRen > 0)
                {
                    tkdUnkobi.HenKai++;
                    tkdUnkobi.Kskbn = CalculateValueUnkobi(ukeno, unkRen, "HaiSkbn");
                    tkdUnkobi.Kskbn = CalculateValueUnkobi(ukeno, unkRen, "Kskbn");
                    tkdUnkobi.HaiIkbn = CalculateValueUnkobi(ukeno, unkRen, "HaiIkbn");
                    tkdUnkobi.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    tkdUnkobi.UpdTime = DateTime.Now.ToString("hhmm");
                    tkdUnkobi.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    tkdUnkobi.UpdPrgId = "KU0600";
                }
                return tkdUnkobi;
            }
            private byte CalculateValueUnkobi(string ukeNo, short unkRen, string Option)
            {
                int countNumberOne = 0;
                int countNumberTwo = 0;
                int countAll = 0;
                var HaiShaList = new List<TkdHaisha>();
                HaiShaList = _context.TkdHaisha.Where(x => x.UkeNo == ukeNo && x.UnkRen == unkRen).ToList();
                if (HaiShaList != null && HaiShaList.Count > 0)
                {
                    countAll = HaiShaList.Count();
                    if (Option == "HaiSkbn")
                    {
                        countNumberOne = HaiShaList.Where(x => x.HaiSkbn == 1).Count();
                        countNumberTwo = HaiShaList.Where(x => x.HaiSkbn == 2).Count();
                    }
                    else if (Option == "Kskbn")
                    {
                        countNumberOne = HaiShaList.Where(x => x.Kskbn == 1).Count();
                        countNumberTwo = HaiShaList.Where(x => x.Kskbn == 2).Count();
                    }
                    else
                    {
                        countNumberOne = HaiShaList.Where(x => x.HaiIkbn == 1).Count();
                        countNumberTwo = HaiShaList.Where(x => x.HaiIkbn == 2).Count();
                    }
                    if (countNumberOne == countAll)
                    {
                        return 1;
                    }
                    else if (countNumberTwo == countAll)
                    {
                        return 2;
                    }
                }
                return 3;
            }





        }
    }
}
