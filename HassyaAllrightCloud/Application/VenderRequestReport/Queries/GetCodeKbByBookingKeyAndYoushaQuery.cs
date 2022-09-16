using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.IService;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.VenderRequestReport.Queries
{
    public class GetCodeKbByBookingKeyAndYoushaQuery : IRequest<List<VenderRequestReportBusLoanInfo>>
    {
        private readonly int _youTblSeq;
        private readonly string _ukeNo;
        private readonly int _unkRen;
        private readonly int _tenantId;
        private readonly ITPM_CodeSyService _codeSyuService;

        public GetCodeKbByBookingKeyAndYoushaQuery( 
            int youTblSeq, string ukeNo, int unkRen, int tenantId, 
            ITPM_CodeSyService codeSyuService)
        {
            _youTblSeq = youTblSeq;
            _ukeNo = ukeNo ?? throw new ArgumentNullException(nameof(ukeNo));
            _unkRen = unkRen;
            _tenantId = tenantId;
            _codeSyuService = codeSyuService ?? throw new ArgumentNullException(nameof(codeSyuService));
        }

        public class Handler : IRequestHandler<GetCodeKbByBookingKeyAndYoushaQuery, List<VenderRequestReportBusLoanInfo>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetCodeKbByBookingKeyAndYoushaQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetCodeKbByBookingKeyAndYoushaQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<List<VenderRequestReportBusLoanInfo>> Handle(GetCodeKbByBookingKeyAndYoushaQuery request, CancellationToken cancellationToken)
            {
                return await request._codeSyuService.FilterTenantIdByCodeSyu(async (tenantId, code) =>
                {
                    return await
                        (from youSyu in _context.TkdYouSyu
                         join codeKb in _context.VpmCodeKb
                         on new { CodeKbn = Convert.ToInt32(youSyu.YouKataKbn), CodeSyu = code, TenantCdSeq = tenantId } equals
                            new {CodeKbn = Convert.ToInt32(codeKb.CodeKbn), codeKb.CodeSyu, codeKb.TenantCdSeq } into gryc
                         from subyc in gryc.DefaultIfEmpty()
                         where youSyu.UkeNo == request._ukeNo &&
                               youSyu.UnkRen == request._unkRen &&
                               youSyu.YouTblSeq == request._youTblSeq &&
                               youSyu.SiyoKbn == 1
                         select new VenderRequestReportBusLoanInfo
                         {
                             IsMainReport = true,
                             RyakuNm = subyc.RyakuNm,
                             SyaRyoUnc = youSyu.SyaRyoUnc.ToString(),
                             SyaSyuDai = youSyu.SyaSyuDai.ToString(),
                             SyaSyuTan = youSyu.SyaSyuTan.ToString(),
                         }
                         ).ToListAsync();
                }, request._tenantId, "KATAKBN");
            }
        }
    }
}
