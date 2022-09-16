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

namespace HassyaAllrightCloud.Application.CarCooperation.Queries
{
    public class GetCarCooperation : IRequest<CarCooperationData>
    {
        private readonly string _ukeNo;
        private readonly short _unkRen;
        private readonly int _youTblSeq;
        public GetCarCooperation(string ukeNo, short unkRen, int youTblSeq)
        {
            _ukeNo = ukeNo;
            _unkRen = unkRen;
            _youTblSeq = youTblSeq;
        }
        public class Handler : IRequestHandler<GetCarCooperation,CarCooperationData>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetCarCooperation> _logger;

            public Handler(KobodbContext context, ILogger<GetCarCooperation> logger)
            {
                _context = context;
                _logger = logger;
            }
            public async Task<CarCooperationData> Handle(GetCarCooperation request, CancellationToken cancellationToken)
            {
                return await (from YOUSHA in _context.TkdYousha
                              join UNKOBI in _context.TkdUnkobi
                                    on new { YOUSHA.UkeNo, YOUSHA.UnkRen, SiyoKbn = 1 }
                                equals new { UNKOBI.UkeNo, UNKOBI.UnkRen, SiyoKbn = (int)UNKOBI.SiyoKbn } into UNKOBI_join
                              from UNKOBI in UNKOBI_join.DefaultIfEmpty()
                              join YYKSHO in _context.TkdYyksho
                                    on new { YOUSHA.UkeNo, SiyoKbn = 1, TenantCdSeq = new ClaimModel().TenantID }
                                equals new { YYKSHO.UkeNo, SiyoKbn = (int)YYKSHO.SiyoKbn, YYKSHO.TenantCdSeq } into YYKSHO_join
                              from YYKSHO in YYKSHO_join.DefaultIfEmpty()
                              join TOKISK in _context.VpmTokisk
                                    on new { TokuiSeq = YOUSHA.YouCdSeq, TenantCdSeq = new ClaimModel().TenantID }
                                equals new { TOKISK.TokuiSeq, TOKISK.TenantCdSeq } into TOKISK_join
                              from TOKISK in TOKISK_join.DefaultIfEmpty()
                              join TOKIST in _context.VpmTokiSt
                                    on new { TokuiSeq = YOUSHA.YouCdSeq, SitenCdSeq = YOUSHA.YouSitCdSeq }
                                equals new { TOKIST.TokuiSeq, TOKIST.SitenCdSeq } into TOKIST_join
                              from TOKIST in TOKIST_join.DefaultIfEmpty()
                              where
                                YOUSHA.UkeNo == request._ukeNo &&
                                YOUSHA.UnkRen == request._unkRen &&
                                YOUSHA.YouTblSeq == request._youTblSeq &&
                                YOUSHA.SiyoKbn == 1 &&
                                string.Compare(TOKISK.SiyoStaYmd, UNKOBI.HaiSymd) <= 0 &&
                                string.Compare(TOKISK.SiyoEndYmd, UNKOBI.HaiSymd) >= 0 &&
                                string.Compare(TOKIST.SiyoStaYmd, UNKOBI.HaiSymd) <= 0 &&
                                string.Compare(TOKIST.SiyoEndYmd, UNKOBI.HaiSymd) >= 0
                              select new CarCooperationData()
                              {
                                  TokuiSeq=TOKIST.TokuiSeq,
                                  SitenCdSeq=TOKIST.SitenCdSeq,
                                  UkeNo = YOUSHA.UkeNo,
                                  UnkRen = YOUSHA.UnkRen,
                                  YouTblSeq = YOUSHA.YouTblSeq,
                                  YouCdSeq = YOUSHA.YouCdSeq,
                                  TOKISK_RyakuNm = TOKISK.RyakuNm,
                                  YouSitCdSeq = YOUSHA.YouSitCdSeq,
                                  TOKIST_RyakuNm = TOKIST.RyakuNm,
                                  UkeCd = YYKSHO.UkeCd,
                                  DanTaNm = UNKOBI.DanTaNm,
                                  TokuiMail=TOKIST.TokuiMail,
                                  MinDate =
                                  (from TKD_Haisha in _context.TkdHaisha
                                   where
       TKD_Haisha.UkeNo == YOUSHA.UkeNo &&
       TKD_Haisha.UnkRen == YOUSHA.UnkRen &&
       TKD_Haisha.YouTblSeq == YOUSHA.YouTblSeq &&
       TKD_Haisha.SiyoKbn == 1
                                   select new
                                   {
                                       TKD_Haisha.HaiSymd
                                   }).Min(p => p.HaiSymd),
                                  MaxDate =
                                  (from TKD_Haisha in _context.TkdHaisha
                                   where
       TKD_Haisha.UkeNo == YOUSHA.UkeNo &&
       TKD_Haisha.UnkRen == YOUSHA.UnkRen &&
       TKD_Haisha.YouTblSeq == YOUSHA.YouTblSeq &&
       TKD_Haisha.SiyoKbn == 1
                                   select new
                                   {
                                       TKD_Haisha.TouYmd
                                   }).Max(p => p.TouYmd),

                                  MinTime =
                                  (from TKD_Haisha in _context.TkdHaisha
                                   where
       TKD_Haisha.UkeNo == YOUSHA.UkeNo &&
       TKD_Haisha.UnkRen == YOUSHA.UnkRen &&
       TKD_Haisha.YouTblSeq == YOUSHA.YouTblSeq &&
       TKD_Haisha.SiyoKbn == 1
                                   select new
                                   {
                                       TKD_Haisha.HaiSymd,
                                       TKD_Haisha.HaiStime

                                   }).Min(p => p.HaiStime),
                                  MaxTime =
                                  (from TKD_Haisha in _context.TkdHaisha
                                   where
       TKD_Haisha.UkeNo == YOUSHA.UkeNo &&
       TKD_Haisha.UnkRen == YOUSHA.UnkRen &&
       TKD_Haisha.YouTblSeq == YOUSHA.YouTblSeq &&
       TKD_Haisha.SiyoKbn == 1
                                   select new
                                   {
                                       TKD_Haisha.TouYmd,
                                       TKD_Haisha.TouChTime
                                   }).Max(p => p.TouChTime),
                                  LargeCount =
                                  (from TKD_YouSyu in _context.TkdYouSyu
                                   where
       TKD_YouSyu.UkeNo == YOUSHA.UkeNo &&
       TKD_YouSyu.UnkRen == YOUSHA.UnkRen &&
       TKD_YouSyu.YouTblSeq == YOUSHA.YouTblSeq &&
       TKD_YouSyu.SiyoKbn == 1 &&
       TKD_YouSyu.YouKataKbn == 1
                                   select new
                                   {
                                       TKD_YouSyu.SyaSyuDai
                                   }).Sum(t => t.SyaSyuDai),
                                  MediumCount =
                                  (from TKD_YouSyu in _context.TkdYouSyu
                                   where
       TKD_YouSyu.UkeNo == YOUSHA.UkeNo &&
       TKD_YouSyu.UnkRen == YOUSHA.UnkRen &&
       TKD_YouSyu.YouTblSeq == YOUSHA.YouTblSeq &&
       TKD_YouSyu.SiyoKbn == 1 &&
       TKD_YouSyu.YouKataKbn == 2
                                   select new
                                   {
                                       TKD_YouSyu.SyaSyuDai
                                   }).Sum(t => t.SyaSyuDai),
                                  SmallCount =
                                  (from TKD_YouSyu in _context.TkdYouSyu
                                   where
       TKD_YouSyu.UkeNo == YOUSHA.UkeNo &&
       TKD_YouSyu.UnkRen == YOUSHA.UnkRen &&
       TKD_YouSyu.YouTblSeq == YOUSHA.YouTblSeq &&
       TKD_YouSyu.SiyoKbn == 1 &&
       TKD_YouSyu.YouKataKbn == 3
                                   select new
                                   {
                                       TKD_YouSyu.SyaSyuDai
                                   }).Sum(t=>t.SyaSyuDai)
                              }).FirstOrDefaultAsync();
            }

        }
    }
}
