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
    public class GetCodeKATAKBNByYoushaCodeKbnQuery : IRequest<List<TPM_CodeKbData>>
    {
        private string _youShaCodeKbn;
        private int _tenantId;
        private readonly ITPM_CodeSyService _codeSyuService;

        public GetCodeKATAKBNByYoushaCodeKbnQuery(int tenantId, string youShaCodeKbn,
            ITPM_CodeSyService codeSyuService)
        {
            _tenantId = tenantId;
            _youShaCodeKbn = youShaCodeKbn ?? throw new ArgumentNullException(nameof(youShaCodeKbn));
            _codeSyuService = codeSyuService ?? throw new ArgumentNullException(nameof(codeSyuService));
        }

        public class Handler : IRequestHandler<GetCodeKATAKBNByYoushaCodeKbnQuery, List<TPM_CodeKbData>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetCodeKATAKBNByYoushaCodeKbnQuery> _logger;
            public Handler(KobodbContext context,
                ILogger<GetCodeKATAKBNByYoushaCodeKbnQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<List<TPM_CodeKbData>> Handle(GetCodeKATAKBNByYoushaCodeKbnQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    return await 
                        request._codeSyuService.FilterTenantIdByCodeSyu(async (tenantId, code) => {
                        return await
                            (from s in _context.VpmCodeKb
                             where Convert.ToInt32(s.CodeKbn) == Convert.ToInt32(request._youShaCodeKbn) &&
                                   s.CodeSyu == code &&
                                   s.SiyoKbn == 1 &&
                                   s.TenantCdSeq == tenantId
                             select new TPM_CodeKbData
                             {
                                 CodeKb_CodeKbnSeq = Convert.ToInt32(s.CodeKbnSeq),
                                 CodeKb_CodeKbn = s.CodeKbn,
                                 CodeKb_RyakuNm = s.RyakuNm,
                             }).ToListAsync();
                    }, request._tenantId, "KATAKBN");
                }
                catch (Exception ex)
                {
                    _logger.LogTrace(ex.ToString());

                    return new List<TPM_CodeKbData>();
                }


            }
        }
    }
}
