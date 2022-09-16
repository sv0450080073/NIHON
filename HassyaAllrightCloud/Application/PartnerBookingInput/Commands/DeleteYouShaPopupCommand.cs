using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.PartnerBookingInput.Commands
{
    public class DeleteYouShaPopupCommand : IRequest<IActionResult>
    {
        public string Ukeno { get; set; }
        public TkdYousha TkdYousha { get; set; }
        public TkdMihrim TkdMihrim { get; set; }
        public Dictionary<CommandMode, List<TkdYouSyu>> YouSyu { get; set; }
        public YouShaDataTable YouShaItemUpdate { get; set; }
        public class Handler : IRequestHandler<DeleteYouShaPopupCommand, IActionResult>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<DeleteYouShaPopupCommand> _logger;
            public Handler(KobodbContext context, ILogger<DeleteYouShaPopupCommand> logger)
            {
                _context = context;
                _logger = logger;
            }
            public async Task<IActionResult> Handle(DeleteYouShaPopupCommand command, CancellationToken cancellationToken)
            {
                List<TkdHaisha> tkdHaishaList = CollectDataTkdHaisha(command.YouSyu[CommandMode.Update]);
                TkdYyksho tkdYyksho = new TkdYyksho();
                using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbTran = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.TkdYousha.Update(command.TkdYousha);
                        _context.TkdMihrim.Update(command.TkdMihrim);
                        if (command.YouSyu[CommandMode.Update].Count > 0)
                        {
                            _context.TkdYouSyu.UpdateRange(command.YouSyu[CommandMode.Update]);
                        }
                        foreach (var item in tkdHaishaList)
                        {

                            _context.TkdHaisha.Update(item);
                            await _context.SaveChangesAsync();
                        }
                        List<TkdUnkobi> tkdUnkobiList = CollectDataTkdUnkobi(command.YouShaItemUpdate);
                        foreach (TkdUnkobi item in tkdUnkobiList)
                        {
                            _context.TkdUnkobi.Update(item);
                            await _context.SaveChangesAsync();
                        }
                        tkdYyksho = CollectDataTkdYyksho(command.YouShaItemUpdate);
                        _context.TkdYyksho.Update(tkdYyksho);
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
                        _logger.LogTrace(ex.ToString());
                        dbTran.Rollback();
                        return new BadRequestResult();
                    }
                }
                return new ContentResult { Content = command.YouShaItemUpdate.YOUSHA_UkeNo };
            }
            private List<TkdHaisha> CollectDataTkdHaisha(List<TkdYouSyu> YouSyuList)
            {
                List<TkdHaisha> listTkdHaiSha = new List<TkdHaisha>();
                if (YouSyuList != null && YouSyuList.Count > 0)
                {
                    foreach (var item in YouSyuList)
                    {
                        var tkdHaiShaDataList = _context.TkdHaisha.Where(x => x.UkeNo == item.UkeNo
                                            && x.UnkRen == item.UnkRen
                                            && x.SyaSyuRen == item.SyaSyuRen
                                            && x.YouTblSeq == item.YouTblSeq).ToList();
                        foreach (var data in tkdHaiShaDataList)
                        {
                            data.YouKataKbn = 9;
                            data.YoushaUnc = 0;
                            data.YoushaSyo = 0;
                            data.YouTblSeq = 0;
                            data.HenKai++;
                            data.UpdYmd = DateTime.Now.ToString(CommonConstants.FormatUpdateDbDate);
                            data.UpdTime = DateTime.Now.ToString(CommonConstants.FormatUpdateDbTime);
                            data.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                            data.UpdPrgId = ScreenCode.PartnerBookingInputUpdPrgId;
                            listTkdHaiSha.Add(data);
                        }
                    }
                }
                return listTkdHaiSha;
            }
            private List<TkdUnkobi> CollectDataTkdUnkobi(YouShaDataTable YouShaItemUpdate)
            {
                List<TkdUnkobi> listTkdUnkobi = new List<TkdUnkobi>();

                var tkdUnkobiData = _context.TkdUnkobi.Where(x => x.UkeNo == YouShaItemUpdate.YOUSHA_UkeNo && x.UnkRen == YouShaItemUpdate.YOUSHA_UnkRen).FirstOrDefault();
                if (tkdUnkobiData != null)
                {
                    tkdUnkobiData.HenKai++;
                    //tkdUnkobiData.Kskbn = calculateValueUnkobi(YouShaItemUpdate.YOUSHA_UkeNo, YouShaItemUpdate.YOUSHA_UnkRen, "Kskbn");    
                    tkdUnkobiData.YouKbn = calculateYouKbnValue(YouShaItemUpdate);
                    tkdUnkobiData.UpdYmd = DateTime.Now.ToString(CommonConstants.FormatUpdateDbDate);
                    tkdUnkobiData.UpdTime = DateTime.Now.ToString(CommonConstants.FormatUpdateDbTime);
                    tkdUnkobiData.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    tkdUnkobiData.UpdPrgId = ScreenCode.PartnerBookingInputUpdPrgId;
                    listTkdUnkobi.Add(tkdUnkobiData);
                }
                return listTkdUnkobi;
            }
            private TkdYyksho CollectDataTkdYyksho(YouShaDataTable YouShaItemUpdate)
            {
                var tenantCdSeq = new ClaimModel().TenantID;
                TkdYyksho tkdYyksho = new TkdYyksho();
                var tkdYykshoData = _context.TkdYyksho.Where(x => x.UkeNo == YouShaItemUpdate.YOUSHA_UkeNo && x.TenantCdSeq == tenantCdSeq).First();
                if (tkdYykshoData != null)
                {
                    tkdYykshoData.HenKai++;
                   // tkdYykshoData.Kskbn = calculateValueYykSho(tkdYykshoData.UkeNo, "Kskbn");
                    tkdYykshoData.YouKbn = calculateYouKbnValue(YouShaItemUpdate);
                    tkdYykshoData.UpdYmd = DateTime.Now.ToString(CommonConstants.FormatUpdateDbDate);
                    tkdYykshoData.UpdTime = DateTime.Now.ToString(CommonConstants.FormatUpdateDbTime);
                    tkdYykshoData.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    tkdYykshoData.UpdPrgId = ScreenCode.PartnerBookingInputUpdPrgId;
                }
                return tkdYykshoData;
            }
            private byte calculateValueUnkobi(string ukeNo, short unkRen, string Option)
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
            private byte calculateValueYykSho(string ukeNo, string Option)
            {
                int countNumberOne = 0;
                int countNumberTwo = 0;
                int countAll = 0;
                var UnkobiList = new List<TkdUnkobi>();
                UnkobiList = _context.TkdUnkobi.Where(x => x.UkeNo == ukeNo).ToList();
                if (UnkobiList != null && UnkobiList.Count > 0)
                {
                    countAll = UnkobiList.Count();
                    if (Option == "HaiSkbn")
                    {
                        countNumberOne = UnkobiList.Where(x => x.HaiSkbn == 1).Count();
                        countNumberTwo = UnkobiList.Where(x => x.HaiSkbn == 2).Count();
                    }
                    else if (Option == "Kskbn")
                    {
                        countNumberOne = UnkobiList.Where(x => x.Kskbn == 1).Count();
                        countNumberTwo = UnkobiList.Where(x => x.Kskbn == 2).Count();
                    }
                    else
                    {
                        countNumberOne = UnkobiList.Where(x => x.HaiIkbn == 1).Count();
                        countNumberTwo = UnkobiList.Where(x => x.HaiIkbn == 2).Count();
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
            private byte calculateYouKbnValue(YouShaDataTable YouShaItemUpdate)
            {
                byte result = 1;
                var data = _context.TkdYousha.Where(x => x.UkeNo == YouShaItemUpdate.YOUSHA_UkeNo
                && x.UnkRen == YouShaItemUpdate.YOUSHA_UnkRen
                && x.SiyoKbn == 1).ToList();
                if(data !=null && data.Count >0)
                {
                    result = 2;
                }
                return result;
            }
        }
    }
}
