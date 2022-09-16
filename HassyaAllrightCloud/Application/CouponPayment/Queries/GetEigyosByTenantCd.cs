using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.CouponPayment.Queries
{
    public class GetEigyosByTenantCd : IRequest<List<EigyoListItem>>
    {
        public int TenantCdSeq { get; set; }
        public class Handler : IRequestHandler<GetEigyosByTenantCd, List<EigyoListItem>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<List<EigyoListItem>> Handle(GetEigyosByTenantCd request, CancellationToken cancellationToken)
            {
                var query = from ei in _context.VpmEigyos.Where(e => e.SiyoKbn == 1)
                            join c in _context.VpmCompny.Where(e => e.SiyoKbn == 1)
                            on ei.CompanyCdSeq equals c.CompanyCdSeq
                            join t in _context.VpmTenant.Where(e => e.SiyoKbn == 1 && e.TenantCdSeq == request.TenantCdSeq)
                            on c.TenantCdSeq equals t.TenantCdSeq
                            select new EigyoListItem()
                            {
                                EigyoCd = ei.EigyoCd,
                                EigyoCdSeq = ei.EigyoCdSeq,
                                RyakuNm = ei.RyakuNm
                            };
                return await query.ToListAsync(cancellationToken);
            }
        }
    }
}
