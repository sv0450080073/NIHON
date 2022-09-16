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
    public class GetTokiskData : IRequest<List<TokistData>>
    {
        public string Ukeno { get; set; } = "";
        public string Unkren { get; set; } = "";
        public string DateParam { get; set; } = "";
         public int TenantCdSeq { get; set; } =0;
        public class Handler : IRequestHandler<GetTokiskData, List<TokistData>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<List<TokistData>> Handle(GetTokiskData request, CancellationToken cancellationToken)
            {
                var result = new List<TokistData>();
                try
                {
                    result = (from TOKISK in _context.VpmTokisk
                              join TOKIST in _context.VpmTokiSt
                              on new { T1 = TOKISK.TokuiSeq}
                              equals new { T1 = TOKIST.TokuiSeq}
                              into TOKIST_join
                              from TOKIST in TOKIST_join.DefaultIfEmpty()
                              where TOKISK.SiyoStaYmd.CompareTo(request.DateParam) <= 0
                              && TOKISK.SiyoEndYmd.CompareTo(request.DateParam) >= 0
                              && TOKIST.SiyoStaYmd.CompareTo(request.DateParam) <= 0
                              && TOKIST.SiyoEndYmd.CompareTo(request.DateParam) >= 0
                              && TOKISK.TenantCdSeq==request.TenantCdSeq
                              select new TokistData()
                              {
                                  TOKISK_TokuiSeq= TOKISK.TokuiSeq,
                                  TOKISK_TokuiCd= TOKISK.TokuiCd,
                                  TOKISK_RyakuNm= TOKISK.RyakuNm,
                                  TOKIST_SitenCdSeq= TOKIST.SitenCdSeq,
                                  TOKIST_SitenCd= TOKIST.SitenCd,
                                  TOKIST_RyakuNm= TOKIST.RyakuNm,
                                  TOKIST_TesuRitu = TOKIST.TesuRitu
                              }).ToList();
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
