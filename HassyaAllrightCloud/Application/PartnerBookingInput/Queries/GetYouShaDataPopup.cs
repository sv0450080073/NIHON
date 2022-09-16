using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.PartnerBookingInput.Queries
{
    public class GetYouShaDataPopup : IRequest<YouShaDataPopup>
    {
        public string Ukeno { get; set; } = "";
        public string Unkren { get; set; } = "";
        public int TenantCdSeq { get; set; } = 0;
        public int YouTblSeq { get; set; } = 0;
        public class Handler : IRequestHandler<GetYouShaDataPopup, YouShaDataPopup>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<YouShaDataPopup> Handle(GetYouShaDataPopup request, CancellationToken cancellationToken)
            {
                var result = new YouShaDataPopup();
                try
                {
                    result = (from YOUSHA in _context.TkdYousha
                              join TOKISK in _context.VpmTokisk
                              on new { T1 = YOUSHA.YouCdSeq, T2 = request.TenantCdSeq }
                              equals new { T1 = TOKISK.TokuiSeq, T2 = TOKISK.TenantCdSeq }
                              into TOKISK_join
                              from TOKISK in TOKISK_join.DefaultIfEmpty()
                              join GYOUSYA in _context.VpmGyosya
                              on new { TOKISK.TenantCdSeq, TOKISK.GyosyaCdSeq } equals new { GYOUSYA.TenantCdSeq, GYOUSYA.GyosyaCdSeq }
                              into GYOUSYA_join
                              from GYOUSYA in GYOUSYA_join.DefaultIfEmpty()
                              join TOKIST in _context.VpmTokiSt
                              on new { T1 = YOUSHA.YouCdSeq, T2 = YOUSHA.YouSitCdSeq }
                              equals new { T1 = TOKIST.TokuiSeq, T2 = TOKIST.SitenCdSeq }
                              into TOKIST_join
                              from TOKIST in TOKIST_join.DefaultIfEmpty()
                              where YOUSHA.UkeNo == request.Ukeno
                              && YOUSHA.UnkRen.ToString() == request.Unkren
                              && YOUSHA.YouTblSeq == request.YouTblSeq
                              && YOUSHA.SiyoKbn == 1
                              select new YouShaDataPopup()
                              {
                                  YOUSHA_UkeNo = YOUSHA.UkeNo,
                                  YOUSHA_UnkRen = YOUSHA.UnkRen,
                                  YOUSHA_YouTblSeq = YOUSHA.YouTblSeq,
                                  YOUSHA_YouCdSeq = YOUSHA.YouCdSeq,
                                  YOUSHA_YouSitCdSeq = YOUSHA.YouSitCdSeq,
                                  TOKISK_RyakuNm = TOKISK.RyakuNm,
                                  TOKIST_RyakuNm = TOKIST.RyakuNm,
                                  YOUSHA_SyaRyoUnc = YOUSHA.SyaRyoUnc,
                                  YOUSHA_ZeiKbn = YOUSHA.ZeiKbn,
                                  YOUSHA_Zeiritsu = YOUSHA.Zeiritsu,
                                  YOUSHA_SyaRyoSyo = YOUSHA.SyaRyoSyo,
                                  YOUSHA_TesuRitu = YOUSHA.TesuRitu,
                                  YOUSHA_SyaRyoTes = YOUSHA.SyaRyoTes,
                                  YOUSHA_SihYotYmd = YOUSHA.SihYotYmd,
                                  YOUSHA_HasYmd = YOUSHA.HasYmd,
                                  GYOUSYA_GyosyaCdSeq = GYOUSYA.GyosyaCdSeq,
                                  GYOUSYA_GyosyaCd = GYOUSYA.GyosyaCd,
                                  GYOUSYA_GyosyaNm = GYOUSYA.GyosyaNm,
                                  TOKISK_TokuiCdSeq = TOKISK.TokuiSeq,
                                  TOKISK_TokuiCd = TOKISK.TokuiCd,
                                  TOKIST_SitenCdSeq = TOKIST.SitenCdSeq,
                                  TOKIST_SitenCd = TOKIST.SitenCd,
                              }).FirstOrDefault();
                    return result;
                }
                catch (Exception ex)
                {
                    return result;
                }
            }
        }
    }
}
