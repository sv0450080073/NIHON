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
    public class GetCodeKbByCodeSyuAndCodeKbnQuery : IRequest<List<TPM_CodeKbData>>
    {
        private readonly string _codeKbn;
        private readonly string _codeSyu;
        private readonly int _tenantId;
        private readonly ITPM_CodeSyService _codeSyService;

        public GetCodeKbByCodeSyuAndCodeKbnQuery(string codeKbn, string codeSyu, int tenantId, ITPM_CodeSyService codeSyService)
        {
            _codeKbn = codeKbn;
            _codeSyu = codeSyu;
            _tenantId = tenantId;
            _codeSyService = codeSyService;
        }

        public class Handler : IRequestHandler<GetCodeKbByCodeSyuAndCodeKbnQuery, List<TPM_CodeKbData>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetCodeKbByCodeSyuAndCodeKbnQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetCodeKbByCodeSyuAndCodeKbnQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<List<TPM_CodeKbData>> Handle(GetCodeKbByCodeSyuAndCodeKbnQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    return await request._codeSyService.FilterTenantIdByCodeSyu(async (tenantId, code) =>
                    {
                        return await
                            (from codeKb in _context.VpmCodeKb
                             where codeKb.SiyoKbn == 1 &&
                                   Convert.ToInt32(codeKb.CodeKbn) == Convert.ToInt32(request._codeKbn) &&
                                   codeKb.CodeSyu == code &&
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
                catch (System.Exception ex)
                {
                    _logger.LogError(ex, ex.Message);

                    return new List<TPM_CodeKbData>();
                }
                
            }
        }
    }
}
