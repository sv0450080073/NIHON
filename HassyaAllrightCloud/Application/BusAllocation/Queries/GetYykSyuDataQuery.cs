using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.IService;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BusAllocation.Queries
{
    public class GetYykSyuDataQuery : IRequest<List<CarTypePopup>>
    {
        public string Ukeno { get; set; } = "";
        public int TenantCdSeq { get; set; } = 0;

        public class Handler : IRequestHandler<GetYykSyuDataQuery, List<CarTypePopup>>
        {
            private readonly KobodbContext _context;
            private readonly ITPM_CodeSyService _codeSyuService;
            public Handler(KobodbContext context, ITPM_CodeSyService codeSyuService)
            {
                _context = context;
                _codeSyuService = codeSyuService;
            }

            public async Task<List<CarTypePopup>> Handle(GetYykSyuDataQuery request, CancellationToken cancellationToken)
            {
                var result = new List<CarTypePopup>();
                try
                {
                    string codeSyuKATAKBN = "KATAKBN";
                    int tenantKATAKBN = await _codeSyuService.CheckTenantByKanriKbnAsync(request.TenantCdSeq, codeSyuKATAKBN);
                    result = (from YYKSYU in _context.TkdYykSyu
                              join SYASYU in _context.VpmSyaSyu
                              on new { H1 = YYKSYU.SyaSyuCdSeq, H2 = request.TenantCdSeq }
                               equals new { H1 = SYASYU.SyaSyuCdSeq, H2 = SYASYU.TenantCdSeq }
                               into SYASYU_join
                              from SYASYU in SYASYU_join.DefaultIfEmpty()
                              join KATAKBN in _context.VpmCodeKb
                              on new { H1 = YYKSYU.KataKbn.ToString(), H2 = tenantKATAKBN, H3 = codeSyuKATAKBN }
                              equals new { H1 = KATAKBN.CodeKbn, H2 = KATAKBN.TenantCdSeq, H3 = KATAKBN.CodeSyu }
                              into KATAKBN_join
                              from KATAKBN in KATAKBN_join.DefaultIfEmpty()
                              where YYKSYU.UkeNo == request.Ukeno
                              && YYKSYU.UnkRen == 1
                              && YYKSYU.SiyoKbn == 1
                              select new CarTypePopup()
                              {
                                  YYKSYU_UkeNo = YYKSYU.UkeNo,
                                  YYKSYU_UnkRen = YYKSYU.UnkRen,
                                  YYKSYU_SyaSyuRen = YYKSYU.SyaSyuRen,
                                  YYKSYU_SyaSyuCdSeq = YYKSYU.SyaSyuCdSeq,
                                  SYASYU_SyaSyuNm = SYASYU.SyaSyuNm == null ? "指定なし" : SYASYU.SyaSyuNm,
                                  YYKSYU_KataKbn = YYKSYU.KataKbn,
                                  KATAKBN_RyakuNm = KATAKBN.RyakuNm,
                                  YYKSYU_SyaSyuDai = YYKSYU.SyaSyuDai
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
