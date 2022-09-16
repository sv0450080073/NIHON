using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.IService;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.PartnerBookingInput.Queries
{
    public class GetYyKSyuDataPopup : IRequest<List<YyKSyuDataPopup>>
    {
        public string Ukeno { get; set; } = "";
        public string Unkren { get; set; } = "";
        public int TenantCdSeq { get; set; } = 0;
        public int YouTblSeq { get; set; } = 0;
        public class Handler : IRequestHandler<GetYyKSyuDataPopup, List<YyKSyuDataPopup>>
        {
            private readonly KobodbContext _context;
            private readonly ITPM_CodeSyService _codeSyuService;
            public Handler(KobodbContext context, ITPM_CodeSyService codeSyuService)
            {
                _context = context;
                _codeSyuService = codeSyuService;
            }
            public async Task<List<YyKSyuDataPopup>> Handle(GetYyKSyuDataPopup request, CancellationToken cancellationToken)
            {
                var result = new List<YyKSyuDataPopup>();
                try
                {
                    string codeSyuKATAKBN = "KATAKBN";
                    int tenantKATAKBN = await _codeSyuService.CheckTenantByKanriKbnAsync(request.TenantCdSeq, codeSyuKATAKBN);
                    result = (from YYKSYU in _context.TkdYykSyu
                              join YYKSYU_SYASYU in _context.VpmSyaSyu
                              on new { Y1 = YYKSYU.SyaSyuCdSeq, Y2 = request.TenantCdSeq }
                              equals new { Y1 = YYKSYU_SYASYU.SyaSyuCdSeq, Y2 = YYKSYU_SYASYU.TenantCdSeq }
                              into YYKSYU_SYASYU_join
                              from YYKSYU_SYASYU in YYKSYU_SYASYU_join.DefaultIfEmpty()
                              join YYKSYU_KATAKBN in _context.VpmCodeKb
                              on new { Y1 = YYKSYU.KataKbn.ToString(), Y2 = codeSyuKATAKBN, Y3 = tenantKATAKBN }
                              equals new { Y1 = YYKSYU_KATAKBN.CodeKbn, Y2 = YYKSYU_KATAKBN.CodeSyu, Y3 = YYKSYU_KATAKBN.TenantCdSeq }
                              into YYKSYU_KATAKBN_join
                              from YYKSYU_KATAKBN in YYKSYU_KATAKBN_join.DefaultIfEmpty()
                              join YOUSYU in _context.TkdYouSyu
                              on new { Y1 = YYKSYU.UkeNo, Y2 = YYKSYU.UnkRen, Y3 = YYKSYU.SyaSyuRen, Y4 = 1, Y5=request.YouTblSeq }
                              equals new { Y1 = YOUSYU.UkeNo, Y2 = YOUSYU.UnkRen, Y3 = YOUSYU.SyaSyuRen, Y4 = (int)YOUSYU.SiyoKbn, Y5=YOUSYU.YouTblSeq }
                              into YOUSYU_join
                              from YOUSYU in YOUSYU_join.DefaultIfEmpty()
                              where YYKSYU.UkeNo == request.Ukeno
                              && YYKSYU.UnkRen.ToString() == request.Unkren
                              && YYKSYU.SiyoKbn == 1
                              select new YyKSyuDataPopup()
                              {
                                  YYKSYU_UkeNo = YYKSYU.UkeNo,
                                  YYKSYU_UnkRen = YYKSYU.UnkRen,
                                  YYKSYU_SyaSyuRen = YYKSYU.SyaSyuRen,
                                  YYKSYU_SyaSyuCdSeq = YYKSYU.SyaSyuCdSeq,
                                  YYKSYU_KataKbn = YYKSYU.KataKbn,
                                  YYKSYU_SyaSyuDai = YYKSYU.SyaSyuDai,
                                  YYKSYU_SyaSyuTan = YYKSYU.SyaSyuTan,
                                  YYKSYU_SyaRyoUnc = YYKSYU.SyaRyoUnc,
                                  YYKSYU_UnitBusPrice = YYKSYU.UnitBusPrice,
                                  YYKSYU_SYASYU_SyaSyuNm = YYKSYU_SYASYU.SyaSyuNm ==null? "指定なし" : YYKSYU_SYASYU.SyaSyuNm,
                                  YYKSYU_KATAKBN_CodeKbnNm = YYKSYU_KATAKBN.CodeKbnNm,
                                  YOUSYU_UkeNo = YOUSYU.UkeNo,
                                  YOUSYU_UnkRen = YOUSYU.UnkRen,
                                  YOUSYU_YouTblSeq = YOUSYU.YouTblSeq,
                                  YOUSYU_SyaSyuRen = YOUSYU.SyaSyuRen,
                                  YOUSYU_YouKataKbn = YOUSYU.YouKataKbn,
                                  YOUSYU_SyaSyuDai = YOUSYU.SyaSyuDai,
                                  YOUSYU_SyaSyuTan = YOUSYU.SyaSyuTan,
                                  YOUSYU_SyaRyoUnc = YOUSYU.SyaRyoUnc
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
