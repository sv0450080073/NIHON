using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
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
    public class GetCodeKbByCodeSyu : IRequest<List<VpmCodeKb>>
    {
        private string _codeSyu;
        private int _tenantId;

        public GetCodeKbByCodeSyu(int tenantId, string codeSyu)
        {
            _tenantId = tenantId;
            _codeSyu = codeSyu ?? throw new ArgumentNullException(nameof(codeSyu));
        }

        public class Handler : IRequestHandler<GetCodeKbByCodeSyu, List<VpmCodeKb>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetCodeKbByCodeSyu> _logger;
            public Handler(KobodbContext context,
                ILogger<GetCodeKbByCodeSyu> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<List<VpmCodeKb>> Handle(GetCodeKbByCodeSyu request, CancellationToken cancellationToken)
            {
                try
                {
                    return await _context.VpmCodeKb.Where(_ => _.TenantCdSeq == request._tenantId &&
                                                               _.SiyoKbn == 1 &&
                                                               _.CodeSyu == request._codeSyu)
                                                   .ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogTrace(ex.ToString());

                    return new List<VpmCodeKb>();
                }


            }
        }
    }
}
