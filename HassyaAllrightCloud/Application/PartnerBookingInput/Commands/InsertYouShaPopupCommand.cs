using HassyaAllrightCloud.Commons;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.PartnerBookingInput.Commands
{
    public class InsertYouShaPopupCommand : IRequest<string>
    {
        private readonly YouShaDataInsert _youShaDataInsert;
        public InsertYouShaPopupCommand(YouShaDataInsert youShaDataInsert)
        {
            _youShaDataInsert = youShaDataInsert;
        }
        public class Handler : IRequestHandler<InsertYouShaPopupCommand, string>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<InsertYouShaPopupCommand> _logger;
            private readonly string _newYouSha;
            public Handler(KobodbContext context, ILogger<InsertYouShaPopupCommand> logger)
            {
                _context = context;
                _logger = logger;
            }
            public async Task<string> Handle(InsertYouShaPopupCommand request, CancellationToken cancellationToken)
            {
                TkdYousha tkdYousha = CollectDataTkdYouSha(request._youShaDataInsert);
                List<TkdYouSyu> tkdYouSyuList = CollectDataTkdYouSyu(request._youShaDataInsert);
                TkdMihrim tkdMihrim = CollectDataTkdMihrim(request._youShaDataInsert);
                List<TkdHaisha> tkdHaishaList = CollectDataTkdHaisha(request._youShaDataInsert);
                TkdYyksho tkdYyksho = new TkdYyksho();
                using (var dbTran = _context.Database.BeginTransaction())
                {
                    try
                    {
                        await _context.TkdYousha.AddAsync(tkdYousha);
                        await _context.SaveChangesAsync();
                        var youTblSeqNew = tkdYousha.YouTblSeq;
                        foreach (TkdYouSyu item in tkdYouSyuList)
                        {
                            item.YouTblSeq = youTblSeqNew;
                            await _context.TkdYouSyu.AddAsync(item);
                        }
                        await _context.SaveChangesAsync();
                        tkdMihrim.YouTblSeq = youTblSeqNew;
                        await _context.TkdMihrim.AddAsync(tkdMihrim);
                        await _context.SaveChangesAsync();
                        foreach (TkdHaisha item in tkdHaishaList)
                        {
                            item.YouTblSeq = youTblSeqNew;
                            item.UpdYmd = DateTime.Now.ToString(DateTimeFormat.yyyyMMdd);
                            item.UpdTime = DateTime.Now.ToString(DateTimeFormat.HHmmss);
                            _context.TkdHaisha.Update(item);
                            await _context.SaveChangesAsync();
                        }
                        List<TkdUnkobi> tkdUnkobiList = CollectDataTkdUnkobi(request._youShaDataInsert);
                        foreach (TkdUnkobi item in tkdUnkobiList)
                        {
                            _context.TkdUnkobi.Update(item);
                            await _context.SaveChangesAsync();
                        }
                        tkdYyksho = CollectDataTkdYyksho(request._youShaDataInsert);
                        _context.TkdYyksho.Update(tkdYyksho);
                        await _context.SaveChangesAsync();
                        List<TkdHaiin> tkdHaiinList = CollectDataTkdHaiin(request._youShaDataInsert);
                        foreach (TkdHaiin item in tkdHaiinList)
                        {
                            _context.TkdHaiin.Update(item);
                        }
                        List<TkdKoban> tkdKobanList = CollectDataTkdKoban(request._youShaDataInsert);
                        foreach (TkdKoban item in tkdKobanList)
                        {
                            _context.Remove(item);
                        }
                        await _context.SaveChangesAsync();
                        dbTran.Commit();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        dbTran.Rollback();
                        return "-1";
                    }
                }
                return tkdYyksho.UkeCd.ToString();
            }
            private TkdYousha CollectDataTkdYouSha(YouShaDataInsert youShaDataInsert)
            {
                TkdYousha tkdYousha = new TkdYousha();
                tkdYousha.UkeNo = youShaDataInsert.YyKSyuDataPopups.First().YYKSYU_UkeNo;
                tkdYousha.UnkRen = youShaDataInsert.YyKSyuDataPopups.First().YYKSYU_UnkRen;
                tkdYousha.YouTblSeq = getYouTblSeq(youShaDataInsert.YyKSyuDataPopups.First().YYKSYU_UkeNo, youShaDataInsert.YyKSyuDataPopups.First().YYKSYU_UnkRen);
                tkdYousha.HenKai = 0;
                tkdYousha.YouCdSeq = youShaDataInsert.TokistData.TOKISK_TokuiSeq;
                tkdYousha.YouSitCdSeq = youShaDataInsert.TokistData.TOKIST_SitenCdSeq;
                tkdYousha.HasYmd = getUriKbn(new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID) == 1 ? youShaDataInsert.HaiShaDataTableList.First().HAISHA_HaiSYmd
                    : youShaDataInsert.HaiShaDataTableList.First().HAISHA_TouYmd;
                tkdYousha.SihYotYmd = youShaDataInsert.YouShaDataPopup.YOUSHA_SihYotYmd;
                tkdYousha.SihYm = youShaDataInsert.YouShaDataPopup.YOUSHA_SihYotYmd.Substring(0, 6);             
                tkdYousha.ZeiKbn = (byte)Int16.Parse(youShaDataInsert.CodeKbnDataPopup.CodeKbn);
                tkdYousha.Zeiritsu = youShaDataInsert.YouShaDataPopup.YOUSHA_Zeiritsu;             
                tkdYousha.SyaRyoSyo = youShaDataInsert.YouShaDataPopup.YOUSHA_SyaRyoSyo;
                tkdYousha.SyaRyoTes = youShaDataInsert.YouShaDataPopup.YOUSHA_SyaRyoTes;
                tkdYousha.SyaRyoUnc = youShaDataInsert.CodeKbnDataPopup.CodeKbnNm == "内税"? youShaDataInsert.Sum_YOUSYU_SyaSyuTan - tkdYousha.SyaRyoSyo : youShaDataInsert.Sum_YOUSYU_SyaSyuTan;
                tkdYousha.TesuRitu = youShaDataInsert.YouShaDataPopup.YOUSHA_TesuRitu;        
                tkdYousha.JitaFlg = 0;
                tkdYousha.CompanyCdSeq = new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID;
                tkdYousha.SihKbn = 1;
                tkdYousha.ScouKbn = 1;
                tkdYousha.SiyoKbn = 1;
                tkdYousha.UpdYmd = DateTime.Now.ToString(CommonConstants.FormatUpdateDbDate);
                tkdYousha.UpdTime = DateTime.Now.ToString(CommonConstants.FormatUpdateDbTime);
                tkdYousha.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                tkdYousha.UpdPrgId = ScreenCode.PartnerBookingInputUpdPrgId;
                return tkdYousha;
            }
            private List<TkdYouSyu> CollectDataTkdYouSyu(YouShaDataInsert youShaDataInsert)
            {
                List<TkdYouSyu> listTkdYouSyu = new List<TkdYouSyu>();
                foreach (var item in youShaDataInsert.YyKSyuDataPopups)
                {
                    TkdYouSyu tkdYouSyu = new TkdYouSyu();
                    tkdYouSyu.UkeNo = item.YYKSYU_UkeNo;
                    tkdYouSyu.UnkRen = item.YYKSYU_UnkRen;
                    // tkdYouSyu.YouTblSeq = -1;
                    tkdYouSyu.SyaSyuRen = item.YYKSYU_SyaSyuRen;
                    tkdYouSyu.HenKai = 0;
                    tkdYouSyu.YouKataKbn = (byte)item.BusTypeDataPartner.Id;
                    tkdYouSyu.SyaSyuDai = item.YOUSYU_SyaSyuDai;
                    tkdYouSyu.SyaSyuTan = item.YOUSYU_SyaSyuTan;
                    tkdYouSyu.SyaRyoUnc = item.YOUSYU_SyaRyoUnc;
                    tkdYouSyu.SiyoKbn = 1;
                    tkdYouSyu.UpdYmd = DateTime.Now.ToString(CommonConstants.FormatUpdateDbDate);
                    tkdYouSyu.UpdTime = DateTime.Now.ToString(CommonConstants.FormatUpdateDbTime);
                    tkdYouSyu.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    tkdYouSyu.UpdPrgId = ScreenCode.PartnerBookingInputUpdPrgId;
                    listTkdYouSyu.Add(tkdYouSyu);
                }
                return listTkdYouSyu;
            }
            private TkdMihrim CollectDataTkdMihrim(YouShaDataInsert youShaDataInsert)
            {
                TkdMihrim tkdMihrim = new TkdMihrim();
                tkdMihrim.UkeNo = youShaDataInsert.YyKSyuDataPopups.First().YYKSYU_UkeNo;
                tkdMihrim.MihRen = (short)getMihRen(youShaDataInsert.YyKSyuDataPopups.First().YYKSYU_UkeNo);
                tkdMihrim.HenKai = 0;
                tkdMihrim.SihFutSyu = 1;
                tkdMihrim.UnkRen = youShaDataInsert.YyKSyuDataPopups.First().YYKSYU_UnkRen;
                tkdMihrim.YouTblSeq = -1;
                tkdMihrim.HaseiKin = youShaDataInsert.Sum_YOUSYU_SyaSyuTan;
                tkdMihrim.SyaRyoSyo = youShaDataInsert.YouShaDataPopup.YOUSHA_SyaRyoSyo;
                tkdMihrim.SyaRyoTes = youShaDataInsert.YouShaDataPopup.YOUSHA_SyaRyoTes;
                tkdMihrim.YoushaGak = youShaDataInsert.YouShaDataPopup.Sum_MoneyAllShow;
                tkdMihrim.SihRaiRui = 0;
                tkdMihrim.CouKesRui = 0;
                tkdMihrim.YouFutTumRen = 0;
                tkdMihrim.SiyoKbn = 1;
                tkdMihrim.UpdYmd = DateTime.Now.ToString(CommonConstants.FormatUpdateDbDate);
                tkdMihrim.UpdTime = DateTime.Now.ToString(CommonConstants.FormatUpdateDbTime);
                tkdMihrim.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                tkdMihrim.UpdPrgId = ScreenCode.PartnerBookingInputUpdPrgId;
                return tkdMihrim;
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
                    && x.SyaSyuRen == item.HAISHA_SyaSyuRen).FirstOrDefault();
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
                        tkdHaiShaData.YouKataKbn = (byte)YykSyuItem.BusTypeDataPartner.Id;
                        var YykSyuItems = youShaDataInsert.YyKSyuDataPopups.Where(x => x.YYKSYU_UkeNo == item.HAISHA_UkeNo
                          && x.YYKSYU_UnkRen == item.HAISHA_UnkRen
                          && x.YYKSYU_SyaSyuRen == item.HAISHA_SyaSyuRen).ToList();
                        if(YykSyuItems.Count()==1)
                        {                       
                            tkdHaiShaData.YoushaSyo = CaculateVATMoney(YykSyuItem.YOUSYU_SyaSyuTan,
          youShaDataInsert.YouShaDataPopup.YOUSHA_Zeiritsu, youShaDataInsert.CodeKbnDataPopup.CodeKbnNm,youShaDataInsert.TKM_KasSetDataList);
                            tkdHaiShaData.YoushaTes = CaculateVATMoneyCustomer(YykSyuItem.YOUSYU_SyaSyuTan,
        youShaDataInsert.YouShaDataPopup.YOUSHA_TesuRitu, youShaDataInsert.CodeKbnDataPopup.CodeKbnNm, tkdHaiShaData.YoushaSyo,youShaDataInsert.TKM_KasSetDataList);
                            tkdHaiShaData.YoushaUnc = youShaDataInsert.CodeKbnDataPopup.CodeKbnNm == "内税" ? YykSyuItem.YOUSYU_SyaSyuTan - tkdHaiShaData.YoushaSyo : YykSyuItem.YOUSYU_SyaSyuTan;
                        }
                        else
                        {
                            tkdHaiShaData.YoushaUnc = YykSyuItem.YOUSYU_SyaSyuTan/YykSyuItems.Count();//15
                            if(tkdHaiShaData.TeiDanNo==youShaDataInsert.HaiShaDataTableList.Where(t=>t.HAISHA_SyaSyuRen==tkdHaiShaData.SyaSyuRen).Select(t=>t.HAISHA_TeiDanNo).Min() && 
                                tkdHaiShaData.BunkRen==youShaDataInsert.HaiShaDataTableList.Where(t=>t.HAISHA_SyaSyuRen==tkdHaiShaData.SyaSyuRen).Select(t=>t.HAISHA_BunkRen).Min() )
                            { 
                                tkdHaiShaData.YoushaUnc += YykSyuItem.YOUSYU_SyaSyuTan%YykSyuItems.Count();
                            }
                            tkdHaiShaData.YoushaSyo = CaculateVATMoney(tkdHaiShaData.YoushaUnc,
          youShaDataInsert.YouShaDataPopup.YOUSHA_Zeiritsu, youShaDataInsert.CodeKbnDataPopup.CodeKbnNm, youShaDataInsert.TKM_KasSetDataList);
                            tkdHaiShaData.YoushaTes = CaculateVATMoneyCustomer(tkdHaiShaData.YoushaUnc,
        youShaDataInsert.YouShaDataPopup.YOUSHA_TesuRitu, youShaDataInsert.CodeKbnDataPopup.CodeKbnNm, tkdHaiShaData.YoushaSyo, youShaDataInsert.TKM_KasSetDataList);
                            tkdHaiShaData.YoushaUnc = youShaDataInsert.CodeKbnDataPopup.CodeKbnNm == "内税" ? tkdHaiShaData.YoushaUnc - tkdHaiShaData.YoushaSyo : tkdHaiShaData.YoushaUnc;
                        }
                        tkdHaiShaData.UpdYmd = DateTime.Now.ToString(CommonConstants.FormatUpdateDbDate);
                        tkdHaiShaData.UpdTime = DateTime.Now.ToString(CommonConstants.FormatUpdateDbTime);
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
                {
                    vatMoney = sumMoney * (VAT / 100); 
                }

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
                if (youShaDataInsert != null)
                {
                    var dataParam = youShaDataInsert.YyKSyuDataPopups.FirstOrDefault();
                    var tkdUnkobiData = _context.TkdUnkobi.Where(x => x.UkeNo == dataParam.YYKSYU_UkeNo && x.UnkRen == dataParam.YYKSYU_UnkRen).First();
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
                TkdYyksho tkdYykshoData = new TkdYyksho();
                if (youShaDataInsert != null)
                {
                    var dataParam = youShaDataInsert.YyKSyuDataPopups.FirstOrDefault();
                    tkdYykshoData = _context.TkdYyksho.Where(x => x.UkeNo == dataParam.YYKSYU_UkeNo && x.TenantCdSeq == tenantCdSeq).First();
                    if (tkdYykshoData != null)
                    {
                        tkdYykshoData.HenKai = (short)(tkdYykshoData.HenKai + 1);
                        tkdYykshoData.Kskbn = calculateValueYykSho(tkdYykshoData.UkeNo, "Kskbn");
                        tkdYykshoData.HaiSkbn = calculateValueYykSho(tkdYykshoData.UkeNo, "HaiSkbn");
                        tkdYykshoData.HaiIkbn = calculateValueYykSho(tkdYykshoData.UkeNo, "HaiIkbn");
                        tkdYykshoData.YouKbn = 2;
                        tkdYykshoData.UpdYmd = DateTime.Now.ToString(CommonConstants.FormatUpdateDbDate);
                        tkdYykshoData.UpdTime = DateTime.Now.ToString(CommonConstants.FormatUpdateDbTime);
                        tkdYykshoData.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                        tkdYykshoData.UpdPrgId = ScreenCode.PartnerBookingInputUpdPrgId;
                    }
                }

                return tkdYykshoData;
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
            private int getUriKbn(int companyCdSeq)
            {
                var vpmKasSetData = new TkmKasSet();
                try
                {
                    vpmKasSetData = _context.TkmKasSet.Where(x => x.CompanyCdSeq == companyCdSeq).First();
                    return vpmKasSetData.UriKbn;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
            private int getYouTblSeq(string ukeno, short unkren)
            {
                var tkdYousha = new List<TkdYousha>();
                try
                {
                    tkdYousha = _context.TkdYousha.Where(x => x.UkeNo == ukeno && x.UnkRen == unkren).ToList();
                    if (tkdYousha != null && tkdYousha.Count > 0)
                    {
                        return tkdYousha.Max(x => x.YouTblSeq) + 1;
                    }
                    else
                    {
                        return 1;
                    }
                }
                catch (Exception ex)
                {
                    return 0;
                }

            }
            private int getMihRen(string ukeno)
            {
                var tkdMihrimList = new List<TkdMihrim>();
                try
                {
                    tkdMihrimList = _context.TkdMihrim.Where(x => x.UkeNo == ukeno).ToList();
                    if (tkdMihrimList != null && tkdMihrimList.Count() > 0)
                    {
                        return tkdMihrimList.Max(x => x.MihRen) + 1;
                    }
                    else
                    {
                        return 1;
                    }
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }
    }
}
