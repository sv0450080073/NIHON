using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.TenantGroup.Queries
{
    public class GetTenantNameQuery : IRequest<string>
    {
        private readonly int _tenantCdSeq;
        public GetTenantNameQuery(int tenantCdSeq)
        {
            _tenantCdSeq = tenantCdSeq;
        }
        public class Handler : IRequestHandler<GetTenantNameQuery, string>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<string> Handle(GetTenantNameQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    string result = string.Empty;
                        result = (await _context.VpmTenant
                            .SingleOrDefaultAsync(y => y.TenantCdSeq == request._tenantCdSeq))
                            .TenantCompanyName;

                    return result;
                }
                catch (Exception)
                {
                    throw;
                }

            }
        }
    }
}
