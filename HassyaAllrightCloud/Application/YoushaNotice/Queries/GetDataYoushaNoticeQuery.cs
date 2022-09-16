using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.IService;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.YoushaNotice.Queries
{
    public class GetDataYoushaNoticeQuery: IRequest<List<YoushaNoticeData>>
    {
        private readonly int _tenantCdSeq;
        public GetDataYoushaNoticeQuery(int tenantCdSeq)
        {
            _tenantCdSeq = tenantCdSeq;
        }
        public class Handler : IRequestHandler<GetDataYoushaNoticeQuery, List<YoushaNoticeData>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetDataYoushaNoticeQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetDataYoushaNoticeQuery> logger)
            {
                _context = context;
                _logger = logger;
            }
            public async Task<List<YoushaNoticeData>> Handle(GetDataYoushaNoticeQuery request, CancellationToken cancellationToken)
            {
                return await (from YNOTICE in _context.TkdYoushaNotice
                              join TOKISK in _context.VpmTokisk on new { TokuiSeq = (int)YNOTICE.MotoTokuiSeq } equals new { TokuiSeq = TOKISK.TokuiSeq } into TOKISK_join
                              from TOKISK in TOKISK_join.DefaultIfEmpty()
                              join TOKIST in _context.VpmTokiSt
                                    on new { TokuiSeq = (int)YNOTICE.MotoTokuiSeq, SitenCdSeq = (int)YNOTICE.MotoSitenCdSeq }
                                equals new { TOKIST.TokuiSeq, TOKIST.SitenCdSeq } into TOKIST_join
                              from TOKIST in TOKIST_join.DefaultIfEmpty()
                              where
                                YNOTICE.SiyoKbn == 1 &&
                                YNOTICE.UkeTenantCdSeq == request._tenantCdSeq
                              orderby
                                YNOTICE.UpdYmd descending,
                                YNOTICE.UpdTime descending
                              select new YoushaNoticeData
                              {
                                  MotoTenantCdSeq=YNOTICE.MotoTenantCdSeq,
                                  MotoYouTblSeq=YNOTICE.MotoYouTblSeq,
                                  MotoUkeNo = YNOTICE.MotoUkeNo,
                                  MotoUnKRen = YNOTICE.MotoUnkRen,
                                  MotoTokuiSeq = (int)YNOTICE.MotoTokuiSeq,
                                  MotoSitenCdSeq = (int)YNOTICE.MotoSitenCdSeq,
                                  UkeTenantCdSeq = (int)YNOTICE.UkeTenantCdSeq,
                                  TOKISK_RyakuNm=TOKISK.RyakuNm,
                                  TOKIST_RyakuNm=TOKIST.RyakuNm,
                                  DanTaNm=YNOTICE.DanTaNm,
                                  HaiSYmd=YNOTICE.HaiSYmd,
                                  HaiSTime=YNOTICE.HaiSTime,
                                  TouYmd=YNOTICE.TouYmd,
                                  TouChTime=YNOTICE.TouChTime,
                                  BigtypeNum=(short)YNOTICE.BigtypeNum,
                                  MediumtypeNum=(short)YNOTICE.MediumtypeNum,
                                  SmalltypeNum=(short)YNOTICE.SmalltypeNum,
                                  UnReadKbn=(byte)YNOTICE.UnReadKbn,
                                  RegiterKbn=(byte)YNOTICE.RegiterKbn,
                                  UpdYmd=YNOTICE.UpdYmd,
                                  UpdTime=YNOTICE.UpdTime,
                                  TypeNoti=1
                              }).ToListAsync();
            }
        }
    }
}
