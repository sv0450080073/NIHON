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
    public class GetItemYoushaNoticeQuery : IRequest<YoushaNoticeData>
    {
        private readonly int _motoTenantCdSeq;
        private readonly string _motoUkeNo;
        private readonly short _motoUnkRen;
        private readonly int _motoYouTblSeq;
        public GetItemYoushaNoticeQuery(int motoTenantCdSeq,string motoUkeNo,short motoUnkRen,int motoYouTblSeq)
        {
            _motoTenantCdSeq = motoTenantCdSeq;
            _motoUkeNo = motoUkeNo;
            _motoUnkRen = motoUnkRen;
            _motoYouTblSeq = motoYouTblSeq;
        }
        public class Handler : IRequestHandler<GetItemYoushaNoticeQuery, YoushaNoticeData>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetItemYoushaNoticeQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetItemYoushaNoticeQuery> logger)
            {
                _context = context;
                _logger = logger;
            }
            public async Task<YoushaNoticeData> Handle(GetItemYoushaNoticeQuery request, CancellationToken cancellationToken)
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
                                YNOTICE.MotoTenantCdSeq == request._motoTenantCdSeq &&
                                YNOTICE.MotoUkeNo == request._motoUkeNo &&
                                YNOTICE.MotoUnkRen == request._motoUnkRen &&
                                YNOTICE.MotoYouTblSeq == request._motoYouTblSeq &&
                                YNOTICE.UkeTenantCdSeq==new ClaimModel().TenantID

                              orderby
                                YNOTICE.UpdYmd descending,
                                YNOTICE.UpdTime descending
                              select new YoushaNoticeData
                              {
                                  MotoTenantCdSeq = YNOTICE.MotoTenantCdSeq,
                                  MotoYouTblSeq = YNOTICE.MotoYouTblSeq,
                                  MotoUkeNo = YNOTICE.MotoUkeNo,
                                  MotoUnKRen = YNOTICE.MotoUnkRen,
                                  MotoTokuiSeq = (int)YNOTICE.MotoTokuiSeq,
                                  MotoSitenCdSeq = (int)YNOTICE.MotoSitenCdSeq,
                                  UkeTenantCdSeq = (int)YNOTICE.UkeTenantCdSeq,
                                  TOKISK_RyakuNm = TOKISK.RyakuNm,
                                  TOKIST_RyakuNm = TOKIST.RyakuNm,
                                  DanTaNm = YNOTICE.DanTaNm,
                                  HaiSYmd = YNOTICE.HaiSYmd,
                                  HaiSTime = YNOTICE.HaiSTime,
                                  TouYmd = YNOTICE.TouYmd,
                                  TouChTime = YNOTICE.TouChTime,
                                  BigtypeNum = (short)YNOTICE.BigtypeNum,
                                  MediumtypeNum = (short)YNOTICE.MediumtypeNum,
                                  SmalltypeNum = (short)YNOTICE.SmalltypeNum,
                                  UnReadKbn = (byte)YNOTICE.UnReadKbn,
                                  RegiterKbn = (byte)YNOTICE.RegiterKbn,
                                  UpdYmd = YNOTICE.UpdYmd,
                                  UpdTime = YNOTICE.UpdTime,
                                  TypeNoti = 1
                              }).FirstOrDefaultAsync();
            }
        }
    }
}
