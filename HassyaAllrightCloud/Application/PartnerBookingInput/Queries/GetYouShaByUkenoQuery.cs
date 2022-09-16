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
    public class GetYouShaByUkenoQuery : IRequest<TkdYousha>
    {
        public string Ukeno { get; set; }
        public YouShaDataInsert YouShaDataInsert { get; set; }

        public class Handler : IRequestHandler<GetYouShaByUkenoQuery, TkdYousha>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetYouShaByUkenoQuery> _logger;
            public Handler(KobodbContext context, ILogger<GetYouShaByUkenoQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public Task<TkdYousha> Handle(GetYouShaByUkenoQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    TkdYousha tkdYousha = _context.TkdYousha.FirstOrDefault(e => e.UkeNo == request.Ukeno
                                                                            && e.UnkRen == request.YouShaDataInsert.YouShaDataPopup.YOUSHA_UnkRen
                                                                            && e.YouTblSeq == request.YouShaDataInsert.YouShaDataPopup.YOUSHA_YouTblSeq);
                    SetTkdYoushaData(ref tkdYousha, request.YouShaDataInsert);
                    return Task.FromResult(tkdYousha);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            private void SetTkdYoushaData(ref TkdYousha tkdYousha, YouShaDataInsert youShaDataInsert)
            {
                if (tkdYousha != null && youShaDataInsert != null)
                {
                    tkdYousha.HenKai++;
                    tkdYousha.YouCdSeq = youShaDataInsert.TokistData.TOKISK_TokuiSeq;
                    tkdYousha.YouSitCdSeq = youShaDataInsert.TokistData.TOKIST_SitenCdSeq;

                    tkdYousha.SihYotYmd = youShaDataInsert.YouShaDataPopup.YOUSHA_SihYotYmd;
                    tkdYousha.SihYm = youShaDataInsert.YouShaDataPopup.YOUSHA_SihYotYmd.Substring(0, 6);
                    
                    tkdYousha.ZeiKbn = (byte)Int16.Parse(youShaDataInsert.CodeKbnDataPopup.CodeKbn);
                    tkdYousha.Zeiritsu = youShaDataInsert.YouShaDataPopup.YOUSHA_Zeiritsu;              
                    tkdYousha.TesuRitu = youShaDataInsert.YouShaDataPopup.YOUSHA_TesuRitu;

                    tkdYousha.SyaRyoTes = youShaDataInsert.YouShaDataPopup.YOUSHA_SyaRyoTes;
                    tkdYousha.SyaRyoSyo = youShaDataInsert.YouShaDataPopup.YOUSHA_SyaRyoSyo;
                    tkdYousha.SyaRyoUnc = youShaDataInsert.Sum_YOUSYU_SyaSyuTan;
                    tkdYousha.SyaRyoUnc = youShaDataInsert.CodeKbnDataPopup.CodeKbnNm == "内税" ? youShaDataInsert.Sum_YOUSYU_SyaSyuTan - tkdYousha.SyaRyoSyo: youShaDataInsert.Sum_YOUSYU_SyaSyuTan;
                    tkdYousha.JitaFlg = 0;
                    tkdYousha.CompanyCdSeq = new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID;
                    tkdYousha.SihKbn = 1;
                    tkdYousha.ScouKbn = 1;
                    tkdYousha.SiyoKbn = 1;
                    tkdYousha.UpdYmd = DateTime.Now.ToString(CommonConstants.FormatUpdateDbDate);
                    tkdYousha.UpdTime = DateTime.Now.ToString(CommonConstants.FormatUpdateDbTime);
                    tkdYousha.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    tkdYousha.UpdPrgId = ScreenCode.PartnerBookingInputUpdPrgId;
                }
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