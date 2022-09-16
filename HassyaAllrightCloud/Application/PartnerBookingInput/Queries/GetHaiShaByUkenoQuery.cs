using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.PartnerBookingInput.Queries
{
    public class GetHaiShaByUkenoQuery : IRequest<List<TkdHaisha>>
    {
        public string Ukeno { get; set; }
        public YouShaDataInsert YouShaDataInsert { get; set; }
        public class Handler : IRequestHandler<GetHaiShaByUkenoQuery, List<TkdHaisha>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetHaiShaByUkenoQuery> _logger;
            public Handler(KobodbContext context, ILogger<GetHaiShaByUkenoQuery> logger)
            {
                _context = context;
                _logger = logger;
            }
            public Task<List<TkdHaisha>> Handle(GetHaiShaByUkenoQuery request, CancellationToken cancellationToken)
            {
               var  youShaDataInsert = request.YouShaDataInsert;
                try
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
                                tkdHaiShaData.YoushaUnc = YykSyuItem.YOUSYU_SyaSyuTan;
                                tkdHaiShaData.YoushaSyo = CaculateVATMoney(YykSyuItem.YOUSYU_SyaSyuTan,
              youShaDataInsert.YouShaDataPopup.YOUSHA_Zeiritsu, youShaDataInsert.CodeKbnDataPopup.CodeKbnNm, youShaDataInsert.TKM_KasSetDataList);
                                tkdHaiShaData.YoushaTes = CaculateVATMoneyCustomer(YykSyuItem.YOUSYU_SyaSyuTan,
            youShaDataInsert.YouShaDataPopup.YOUSHA_TesuRitu, youShaDataInsert.CodeKbnDataPopup.CodeKbnNm, tkdHaiShaData.YoushaSyo, youShaDataInsert.TKM_KasSetDataList);
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
                            }
                            tkdHaiShaData.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                            tkdHaiShaData.UpdTime = DateTime.Now.ToString("hhmmss");
                            tkdHaiShaData.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                            tkdHaiShaData.UpdPrgId = ScreenCode.PartnerBookingInputUpdPrgId;
                        }
                        listTkdHaiSha.Add(tkdHaiShaData);
                    }
                    return Task.FromResult(listTkdHaiSha);
                }
                catch(Exception ex)
                {
                    throw ex;
                }
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
        }
    }
}
