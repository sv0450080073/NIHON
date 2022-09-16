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

namespace HassyaAllrightCloud.Application.CodeKb.Queries
{
    public class GetCodeKbByCodeSyuAndListCodeKbnQuery : IRequest<List<TPM_CodeKbData>>
    {
        private readonly List<string> _codeKbn;
        private readonly string _codeSyu;
        private readonly int _tenantId;
        private readonly ITPM_CodeSyService _codeSyService;

        public GetCodeKbByCodeSyuAndListCodeKbnQuery(List<string> codeKbn, string codeSyu, int tenantId, ITPM_CodeSyService codeSyService)
        {
            _codeKbn = codeKbn ?? throw new ArgumentNullException(nameof(codeKbn));
            _codeSyu = codeSyu ?? throw new ArgumentNullException(nameof(codeSyu));
            _tenantId = tenantId;
            _codeSyService = codeSyService ?? throw new ArgumentNullException(nameof(codeSyService));
        }

        public class Handler : IRequestHandler<GetCodeKbByCodeSyuAndListCodeKbnQuery, List<TPM_CodeKbData>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetCodeKbByCodeSyuAndListCodeKbnQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetCodeKbByCodeSyuAndListCodeKbnQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<List<TPM_CodeKbData>> Handle(GetCodeKbByCodeSyuAndListCodeKbnQuery request, CancellationToken cancellationToken)
            {
                return await request._codeSyService.FilterTenantIdByCodeSyu(async (tenantId, code) =>
                {
                    return await
                        (from codeKb in _context.VpmCodeKb
                         where codeKb.SiyoKbn == 1 &&
                               request._codeKbn.Contains(codeKb.CodeKbn) &&
                               codeKb.CodeSyu == request._codeSyu &&
                               codeKb.TenantCdSeq == tenantId
                         select new TPM_CodeKbData
                         {
                             CodeKb_CodeKbn = codeKb.CodeKbn,
                             CodeKb_CodeKbnSeq = codeKb.CodeKbnSeq,
                             CodeKb_CodeSyu = codeKb.CodeSyu,
                             CodeKb_RyakuNm = codeKb.RyakuNm,
                             CodeKbnName = codeKb.CodeKbnNm
                         }
                        ).ToListAsync();
                }, request._tenantId, request._codeSyu);
            }
        }
    }
}
