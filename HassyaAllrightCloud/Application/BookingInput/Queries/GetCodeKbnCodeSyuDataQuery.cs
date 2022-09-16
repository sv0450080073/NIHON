using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DevExpress.Blazor.Internal;
using HassyaAllrightCloud.IService;

namespace HassyaAllrightCloud.Application.BookingInput.Queries
{
    public class GetCodeKbnCodeSyuDataQuery : IRequest<IEnumerable<TPM_CodeKbCodeSyuData>>
    {
        private readonly string _codeSyu;
        private readonly int _tenantId;

        public GetCodeKbnCodeSyuDataQuery(string codeSyu, int tenantId)
        {
            _codeSyu = codeSyu;
            _tenantId = tenantId;
        }

        public class Handler : IRequestHandler<GetCodeKbnCodeSyuDataQuery, IEnumerable<TPM_CodeKbCodeSyuData>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetCodeKbnCodeSyuDataQuery> _logger;
            private readonly ITPM_CodeSyService _codeSyuService;

            public Handler(KobodbContext context, ILogger<GetCodeKbnCodeSyuDataQuery> logger, ITPM_CodeSyService codeSyuService)
            {
                _context = context;
                _logger = logger;
                _codeSyuService = codeSyuService ?? throw new ArgumentNullException(nameof(codeSyuService));
            }

            public async Task<IEnumerable<TPM_CodeKbCodeSyuData>> Handle(GetCodeKbnCodeSyuDataQuery request, CancellationToken cancellationToken)
            {
                var result = new List<TPM_CodeKbCodeSyuData>();
                try
                {
                    result = await
                        _codeSyuService.FilterTenantIdByCodeSyu((tenantId, codeSyu) =>
                        {
                            return 
                                (from s in _context.VpmCodeKb
                                 where s.CodeSyu == codeSyu && s.TenantCdSeq == tenantId && s.SiyoKbn == 1
                                 select new TPM_CodeKbCodeSyuData
                                 {
                                     CodeKbn = s.CodeKbn,
                                     RyakuNm = s.RyakuNm
                                 }).ToListAsync();
                        }, request._tenantId, request._codeSyu);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return null;
                }
                return result;
            }
        }
    }
}
