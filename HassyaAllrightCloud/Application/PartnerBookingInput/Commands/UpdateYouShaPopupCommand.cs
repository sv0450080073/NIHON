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
    public class UpdateYouShaPopupCommand : IRequest<IActionResult>
    {
        public string Ukeno { get; set; }
        public TkdYousha TkdYousha { get; set; }
        public TkdMihrim TkdMihrim { get; set; }
        public Dictionary<CommandMode, List<TkdYouSyu>> YouSyu { get; set; }
        public YouShaDataInsert YouShaDataInsert { get; set; }

        public class Handler : IRequestHandler<UpdateYouShaPopupCommand, IActionResult>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<UpdateYouShaPopupCommand> _logger;
            public Handler(KobodbContext context, ILogger<UpdateYouShaPopupCommand> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<IActionResult> Handle(UpdateYouShaPopupCommand command, CancellationToken cancellationToken)
            {

                List<TkdHaisha> tkdHaishaList = CollectDataTkdHaisha(command.YouShaDataInsert);
                TkdYyksho tkdYyksho = new TkdYyksho();
                using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbTran = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.TkdYousha.Update(command.TkdYousha);
                        _context.TkdMihrim.Update(command.TkdMihrim);
                        if (command.YouSyu[CommandMode.Delete].Count > 0)
                        {
                            _context.TkdYouSyu.RemoveRange(command.YouSyu[CommandMode.Delete]);
                        }
                        if (command.YouSyu[CommandMode.Create].Count > 0)
                        {
                            _context.TkdYouSyu.AddRange(command.YouSyu[CommandMode.Create]);
                        }
                        if (command.YouSyu[CommandMode.Update].Count > 0)
                        {
                            _context.TkdYouSyu.UpdateRange(command.YouSyu[CommandMode.Update]);
                        }
                        var youTblUpdate = command.TkdYousha.YouTblSeq;
                        foreach (var item in tkdHaishaList)
                        {
                            item.YouTblSeq = youTblUpdate;
                            _context.TkdHaisha.Update(item);
                            await _context.SaveChangesAsync();
                        }
                        List<TkdUnkobi> tkdUnkobiList = CollectDataTkdUnkobi(command.YouShaDataInsert);
                        foreach (TkdUnkobi item in tkdUnkobiList)
                        {
                            _context.TkdUnkobi.Update(item);
                            await _context.SaveChangesAsync();
                        }
                        tkdYyksho = CollectDataTkdYyksho(command.YouShaDataInsert);
                        _context.TkdYyksho.Update(tkdYyksho);
                        await _context.SaveChangesAsync();
                        List<TkdHaiin> tkdHaiinList = CollectDataTkdHaiin(command.YouShaDataInsert);
                        foreach (TkdHaiin item in tkdHaiinList)
                        {
                            _context.TkdHaiin.Update(item);
                        }
                        List<TkdKoban> tkdKobanList = CollectDataTkdKoban(command.YouShaDataInsert);
                        foreach (TkdKoban item in tkdKobanList)
                        {
                            _context.Remove(item);
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
                return new ContentResult { Content = command.YouShaDataInsert.YouShaDataPopup.YOUSHA_UkeNo};
            }
            private List<TkdHaisha> CollectDataTkdHaisha(YouShaDataInsert youShaDataInsert)
            {
                List<TkdHaisha> listTkdHaiSha = new List<TkdHaisha>();
                foreach (var item in youShaDataInsert.HaiShaDataTableList)
                {
                    var tkdHaiShaData = _context.TkdHaisha.Where(x => x.UkeNo == item.HAISHA_UkeNo
                    && x.UnkRen == item.HAISHA_UnkRen
                    && x.TeiDanNo == item.HAISHA_TeiDanNo
                    && x.BunkRen == item.HAISHA_BunkRen
                    && x.SyaSyuRen == item.HAISHA_SyaSyuRen).First();
                    if (tkdHaiShaData != null)
                    {
                        var YykSyuItem = youShaDataInsert.YyKSyuDataPopups.Where(x => x.YYKSYU_UkeNo == item.HAISHA_UkeNo
                        && x.YYKSYU_UnkRen == item.HAISHA_UnkRen
                        && x.YYKSYU_SyaSyuRen == item.HAISHA_SyaSyuRen).FirstOrDefault();
                        tkdHaiShaData.HenKai++;
                        tkdHaiShaData.SyuEigCdSeq = 0;
                        tkdHaiShaData.KikEigSeq = 0;
                        tkdHaiShaData.HaiSsryCdSeq = 0;
                        tkdHaiShaData.KssyaRseq = 0;
                        tkdHaiShaData.Kskbn = 1;
                        tkdHaiShaData.HaiSkbn = 1;
                        tkdHaiShaData.HaiIkbn = 1;
                        tkdHaiShaData.YouTblSeq = -1;
                        tkdHaiShaData.YouKataKbn = YykSyuItem.YOUSYU_YouKataKbn;
                        var YykSyuItems = youShaDataInsert.YyKSyuDataPopups.Where(x => x.YYKSYU_UkeNo == item.HAISHA_UkeNo
                           && x.YYKSYU_UnkRen == item.HAISHA_UnkRen
                           && x.YYKSYU_SyaSyuRen == item.HAISHA_SyaSyuRen).ToList();
                        if (YykSyuItems.Count() == 1)
                        {
                           
                            tkdHaiShaData.YoushaSyo = CaculateVATMoney(YykSyuItem.YOUSYU_SyaSyuTan,
          youShaDataInsert.YouShaDataPopup.YOUSHA_Zeiritsu, youShaDataInsert.CodeKbnDataPopup.CodeKbnNm, youShaDataInsert.TKM_KasSetDataList);
                            tkdHaiShaData.YoushaTes = CaculateVATMoneyCustomer(YykSyuItem.YOUSYU_SyaSyuTan,
        youShaDataInsert.YouShaDataPopup.YOUSHA_TesuRitu, youShaDataInsert.CodeKbnDataPopup.CodeKbnNm, tkdHaiShaData.YoushaSyo, youShaDataInsert.TKM_KasSetDataList);
                            tkdHaiShaData.YoushaUnc = youShaDataInsert.CodeKbnDataPopup.CodeKbnNm == "内税" ? YykSyuItem.YOUSYU_SyaSyuTan- tkdHaiShaData.YoushaSyo : YykSyuItem.YOUSYU_SyaSyuTan;
                        }
                        else
                        {
                            tkdHaiShaData.YoushaUnc = YykSyuItem.YOUSYU_SyaSyuTan / YykSyuItems.Count();
                            if (tkdHaiShaData.TeiDanNo == youShaDataInsert.HaiShaDataTableList.Where(t => t.HAISHA_SyaSyuRen == tkdHaiShaData.SyaSyuRen).Select(t => t.HAISHA_TeiDanNo).Min() &&
                                tkdHaiShaData.BunkRen == youShaDataInsert.HaiShaDataTableList.Where(t => t.HAISHA_SyaSyuRen == tkdHaiShaData.SyaSyuRen).Select(t => t.HAISHA_BunkRen).Min())
                            {
                                tkdHaiShaData.YoushaUnc += YykSyuItem.YOUSYU_SyaSyuTan % YykSyuItems.Count();
                            }
                            tkdHaiShaData.YoushaSyo = CaculateVATMoney(tkdHaiShaData.YoushaUnc,
          youShaDataInsert.YouShaDataPopup.YOUSHA_Zeiritsu, youShaDataInsert.CodeKbnDataPopup.CodeKbnNm, youShaDataInsert.TKM_KasSetDataList);
                            tkdHaiShaData.YoushaTes = CaculateVATMoneyCustomer(tkdHaiShaData.YoushaUnc,
        youShaDataInsert.YouShaDataPopup.YOUSHA_TesuRitu, youShaDataInsert.CodeKbnDataPopup.CodeKbnNm, tkdHaiShaData.YoushaSyo, youShaDataInsert.TKM_KasSetDataList);
                            tkdHaiShaData.YoushaUnc = youShaDataInsert.CodeKbnDataPopup.CodeKbnNm == "内税" ? YykSyuItem.YOUSYU_SyaSyuTan - tkdHaiShaData.YoushaSyo : YykSyuItem.YOUSYU_SyaSyuTan;
                        }
                        tkdHaiShaData.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                        tkdHaiShaData.UpdTime = DateTime.Now.ToString("hhmmss");
                        tkdHaiShaData.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                        tkdHaiShaData.UpdPrgId = ScreenCode.PartnerBookingInputUpdPrgId;
                    }
                    listTkdHaiSha.Add(tkdHaiShaData);
                }
                return listTkdHaiSha;
            }
            private int CaculateVATMoney(decimal sumMoney, decimal VAT, string typeVAT, List<TKM_KasSetData> tkm_KasSetDataList)
            {
                var dataKaset = tkm_KasSetDataList.FirstOrDefault();
                decimal vatMoney = 0;
                int result = 0;
                if (typeVAT == "外税")
                {
                    vatMoney = sumMoney * (VAT / 100);
                }
                else if (typeVAT == "内税")
                {
                    vatMoney = sumMoney * VAT / (100 + VAT);
                }
                else if (typeVAT == "非課税")
                {
                    vatMoney = 0;
                }
                if (dataKaset != null)
                {
                    if (dataKaset.SyohiHasu == 1)
                    {
                        result = (int)Math.Ceiling(vatMoney);
                    }
                    else if (dataKaset.SyohiHasu == 2)
                    {
                        result = (int)Math.Floor(vatMoney);
                    }
                    else
                    {
                        result = (int)Math.Round(vatMoney, 0, MidpointRounding.AwayFromZero);
                    }
                }
                return result;
            }
            private int CaculateVATMoneyCustomer(decimal sumMoney, decimal VAT, string typeVAT, decimal tax, List<TKM_KasSetData> tkm_KasSetDataList)
            {
                var dataKaset = tkm_KasSetDataList.FirstOrDefault();
                decimal vatMoney = 0;
                int result = 0;
                if (typeVAT == "外税")
                {
                    vatMoney = (sumMoney + tax) * (VAT / 100);
                }
                else
                { vatMoney = sumMoney * (VAT / 100); }

                if (dataKaset != null)
                {
                    if (dataKaset.TesuHasu == 1)
                    {
                        result = (int)Math.Ceiling(vatMoney);
                    }
                    else if (dataKaset.TesuHasu == 2)
                    {
                        result = (int)Math.Floor(vatMoney);
                    }
                    else
                    {
                        result = (int)Math.Round(vatMoney, 0, MidpointRounding.AwayFromZero);
                    }
                }
                return result;
            }
            private List<TkdUnkobi> CollectDataTkdUnkobi(YouShaDataInsert youShaDataInsert)
            {
                List<TkdUnkobi> listTkdUnkobi = new List<TkdUnkobi>();
                if(youShaDataInsert !=null)
                {
                    var dataParam = youShaDataInsert.YyKSyuDataPopups.FirstOrDefault();
                    var tkdUnkobiData = _context.TkdUnkobi.Where(x => x.UkeNo == dataParam.YYKSYU_UkeNo && x.UnkRen == dataParam.YYKSYU_UnkRen).FirstOrDefault();
                    if (tkdUnkobiData != null)
                    {
                        tkdUnkobiData.HenKai = (short)(tkdUnkobiData.HenKai + 1);
                        tkdUnkobiData.Kskbn = calculateValueUnkobi(dataParam.YYKSYU_UkeNo, dataParam.YYKSYU_UnkRen, "Kskbn");
                        tkdUnkobiData.HaiSkbn = calculateValueUnkobi(dataParam.YYKSYU_UkeNo, dataParam.YYKSYU_UnkRen, "HaiSkbn");
                        tkdUnkobiData.HaiIkbn = calculateValueUnkobi(dataParam.YYKSYU_UkeNo, dataParam.YYKSYU_UnkRen, "HaiIkbn");
                        tkdUnkobiData.YouKbn = 2;
                        tkdUnkobiData.UpdYmd = DateTime.Now.ToString(CommonConstants.FormatUpdateDbDate);
                        tkdUnkobiData.UpdTime = DateTime.Now.ToString(CommonConstants.FormatUpdateDbTime);
                        tkdUnkobiData.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                        tkdUnkobiData.UpdPrgId = ScreenCode.PartnerBookingInputUpdPrgId;
                        listTkdUnkobi.Add(tkdUnkobiData);
                    }
                }
                return listTkdUnkobi;
            }
            private TkdYyksho CollectDataTkdYyksho(YouShaDataInsert youShaDataInsert)
            {
                var tenantCdSeq = new ClaimModel().TenantID;
                TkdYyksho tkdYyksho = new TkdYyksho();
                var tkdYykshoData = _context.TkdYyksho.Where(x => x.UkeNo == youShaDataInsert.YyKSyuDataPopups.First().YYKSYU_UkeNo && x.TenantCdSeq == tenantCdSeq).First();
                if (tkdYykshoData != null)
                {
                    tkdYykshoData.HenKai++;
                    tkdYykshoData.Kskbn = calculateValueYykSho(tkdYykshoData.UkeNo, "Kskbn");
                    tkdYykshoData.HaiSkbn = calculateValueYykSho(tkdYykshoData.UkeNo, "HaiSkbn");
                    tkdYykshoData.HaiIkbn = calculateValueYykSho(tkdYykshoData.UkeNo, "HaiIkbn");
                    tkdYykshoData.YouKbn = 2;
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
            private List<TkdHaiin> CollectDataTkdHaiin(YouShaDataInsert youShaDataInsert)
            {
                List<TkdHaiin> listTkdHaiin = new List<TkdHaiin>();
                foreach (var item in youShaDataInsert.HaiShaDataTableList)
                {
                    var tkdHaiShaData = _context.TkdHaisha.Where(x => x.UkeNo == item.HAISHA_UkeNo
                    && x.UnkRen == item.HAISHA_UnkRen
                    && x.TeiDanNo == item.HAISHA_TeiDanNo
                    && x.BunkRen == item.HAISHA_BunkRen
                    && x.SyaSyuRen == item.HAISHA_SyaSyuRen).First();
                    if (tkdHaiShaData != null && tkdHaiShaData.HaiIkbn != 1)
                    {
                        var tkdHaiinData = _context.TkdHaiin.Where(x => x.UkeNo == item.HAISHA_UkeNo
                             && x.UnkRen == item.HAISHA_UnkRen
                             && x.TeiDanNo == item.HAISHA_TeiDanNo
                             && x.BunkRen == item.HAISHA_BunkRen).First();
                        if (tkdHaiinData != null)
                        {
                            tkdHaiinData.SiyoKbn = 2;
                            tkdHaiinData.HenKai = (short)(tkdHaiinData.HenKai + 1);
                            tkdHaiinData.UpdYmd = DateTime.Now.ToString(CommonConstants.FormatUpdateDbDate);
                            tkdHaiinData.UpdTime = DateTime.Now.ToString(CommonConstants.FormatUpdateDbTime);
                            tkdHaiinData.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                            tkdHaiinData.UpdPrgId = ScreenCode.PartnerBookingInputUpdPrgId;
                            listTkdHaiin.Add(tkdHaiinData);
                        }
                    }
                }
                return listTkdHaiin;
            }
            private List<TkdKoban> CollectDataTkdKoban(YouShaDataInsert youShaDataInsert)
            {
                List<TkdKoban> listTkdKoban = new List<TkdKoban>();
                foreach (var item in youShaDataInsert.HaiShaDataTableList)
                {
                    var tkdHaiShaData = _context.TkdHaisha.Where(x => x.UkeNo == item.HAISHA_UkeNo
                    && x.UnkRen == item.HAISHA_UnkRen
                    && x.SyaSyuRen == item.HAISHA_SyaSyuRen
                    && x.TeiDanNo == item.HAISHA_TeiDanNo
                    && x.BunkRen == item.HAISHA_BunkRen
                    ).First();
                    if (tkdHaiShaData != null && tkdHaiShaData.HaiIkbn != 1)
                    {
                        var tkdKobanData = _context.TkdKoban.Where(x => x.UkeNo == item.HAISHA_UkeNo
                                && x.UnkRen == item.HAISHA_UnkRen
                                && x.TeiDanNo == item.HAISHA_TeiDanNo
                                && x.BunkRen == item.HAISHA_BunkRen
                                && x.SyaSyuRen == item.HAISHA_SyaSyuRen).First();
                        if (tkdKobanData != null)
                        {
                            listTkdKoban.Add(tkdKobanData);
                        }
                    }
                }
                return listTkdKoban;
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
        }
    }
}
