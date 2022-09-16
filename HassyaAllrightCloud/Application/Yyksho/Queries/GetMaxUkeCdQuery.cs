using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.Yyksho.Queries
{
    public class GetMaxUkeCdQuery : IRequest<int>
    {
        private readonly int _tenantId;

        public GetMaxUkeCdQuery(int tenantId)
        {
            _tenantId = tenantId;
        }

        public class Handler : IRequestHandler<GetMaxUkeCdQuery, int>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public Task<int> Handle(GetMaxUkeCdQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    int result = _context.TkdYyksho.Where(_ => _.TenantCdSeq == request._tenantId).Count() > 0 ? _context.TkdYyksho.Where(_ => _.TenantCdSeq == request._tenantId).Max(u => u.UkeCd) : 0;
                    return Task.FromResult(result);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
