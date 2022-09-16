using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BusAllocation.Queries
{
    public class SyainIsAvailableQuery : IRequest<bool>
    {
        private readonly int _syainId;
        private readonly int _tenantId;
        private readonly string _date;

        public SyainIsAvailableQuery(int syainId, int tenantId, string date)
        {
            _syainId = syainId;
            _tenantId = tenantId;
            _date = date;
        }

        public class Handler : IRequestHandler<SyainIsAvailableQuery, bool>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(SyainIsAvailableQuery request, CancellationToken cancellationToken)
            {
                return await
                    (from she in _context.VpmKyoShe
                     join ei in _context.VpmEigyos on she.EigyoCdSeq equals ei.EigyoCdSeq
                     join cmp in _context.VpmCompny on ei.CompanyCdSeq equals cmp.CompanyCdSeq
                     where she.SyainCdSeq == request._syainId
                     && String.Compare(she.StaYmd, request._date) <= 0
                     && String.Compare(she.EndYmd, request._date) >= 0
                     && cmp.TenantCdSeq == request._tenantId
                     select new { }
                    ).AnyAsync();
            }
        }
    }
}
