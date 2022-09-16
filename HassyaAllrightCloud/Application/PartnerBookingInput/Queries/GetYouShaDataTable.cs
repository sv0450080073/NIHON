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
    public class GetYouShaDataTable : IRequest<List<YouShaDataTable>>
    {
        public string Ukeno { get; set; } = "";
        public string Unkren { get; set; } = "";
        public int TenantCdSeq { get; set; } = 0;
        public class Handler : IRequestHandler<GetYouShaDataTable, List<YouShaDataTable>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<List<YouShaDataTable>> Handle(GetYouShaDataTable request, CancellationToken cancellationToken)
            {
                var result = new List<YouShaDataTable>();
                try
                {
                    result = (from YOUSHA in _context.TkdYousha
                              join YOUSHASAKI in _context.VpmTokisk
                              on new { Yo1 = YOUSHA.YouCdSeq, Yo = request.TenantCdSeq }
                              equals new { Yo1 = YOUSHASAKI.TokuiSeq, Yo = YOUSHASAKI.TenantCdSeq }
                              into YOUSHASAKI_join
                              from YOUSHASAKI in YOUSHASAKI_join.DefaultIfEmpty()
                              join YOUSHASAKISITEN in _context.VpmTokiSt
                              on new { Yo1 = YOUSHA.YouCdSeq, Yo2 = YOUSHA.YouSitCdSeq }
                              equals new { Yo1 = YOUSHASAKISITEN.TokuiSeq, Yo2 = YOUSHASAKISITEN.SitenCdSeq }
                              into YOUSHASAKISITEN_join
                              from YOUSHASAKISITEN in YOUSHASAKISITEN_join.DefaultIfEmpty()
                              join GYOSYA in _context.VpmGyosya
                              on new { YOUSHASAKI.GyosyaCdSeq, YOUSHASAKI.TenantCdSeq } equals new { GYOSYA.GyosyaCdSeq, GYOSYA.TenantCdSeq }
                              into GYOSYA_join
                              from GYOSYA in GYOSYA_join.DefaultIfEmpty()
                              where YOUSHA.UkeNo == request.Ukeno
                              && YOUSHA.UnkRen.ToString() == request.Unkren
                              && YOUSHA.SiyoKbn == 1
                              select new YouShaDataTable()
                              {
                                  YOUSHA_UkeNo = YOUSHA.UkeNo,
                                  YOUSHA_UnkRen = YOUSHA.UnkRen,
                                  YOUSHA_YouTblSeq = YOUSHA.YouTblSeq,
                                  GYOSYA_GyosyaCdSeq = GYOSYA.GyosyaCdSeq,
                                  GYOSYA_GyosyaCd = GYOSYA.GyosyaCd,
                                  YOUSHA_YouCdSeq = YOUSHA.YouCdSeq,
                                  YOUSHASAKI_TokuiCd = YOUSHASAKI.TokuiCd,
                                  YOUSHASAKI_RyakuNm = YOUSHASAKI.RyakuNm,
                                  YOUSHA_YouSitCdSeq = YOUSHA.YouSitCdSeq,
                                  YOUSHASAKISITEN_SitenCd = YOUSHASAKISITEN.SitenCd,
                                  YOUSHASAKISITEN_RyakuNm = YOUSHASAKISITEN.RyakuNm,
                                  YOUSHA_SyaRyo_SyaRyoSyo = YOUSHA.SyaRyoUnc + YOUSHA.SyaRyoSyo,
                                  YOUSHA_SyaRyoUnc = YOUSHA.SyaRyoUnc,
                                  YOUSHA_SyaRyoSyo = YOUSHA.SyaRyoSyo,
                                  YOUSHA_TesuRitu = YOUSHA.TesuRitu,
                                  YOUSHA_SyaRyoTes = YOUSHA.SyaRyoTes,
                                  YOUSHA_SihYotYmd = YOUSHA.SihYotYmd
                              }).ToList();
                    if (result != null && result.Count > 0)
                    {
                        foreach (var item in result)
                        {
                            int sumSyaSyuDai = _context.TkdYouSyu.Where(x => x.UkeNo == item.YOUSHA_UkeNo
                            && x.UnkRen == item.YOUSHA_UnkRen
                            && x.YouTblSeq == item.YOUSHA_YouTblSeq
                            && x.SiyoKbn == 1).Count();
                            item.Sum_SyaSyuDai = sumSyaSyuDai;
                        }
                    }
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
