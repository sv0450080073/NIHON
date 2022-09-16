using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.TransportationSummary.Queries
{
    public class GetEigyosByCompanyCdSeq: IRequest<IEnumerable<EigyoListItem>>
    {
        public int CompanyCdSeq { get; set; }
        public int TenantCdSeq { get; set; }

        public class Handler : IRequestHandler<GetEigyosByCompanyCdSeq, IEnumerable<EigyoListItem>>
        {
            private readonly KobodbContext _kobodbContext;
            public Handler(KobodbContext kobodbContext)
            {
                _kobodbContext = kobodbContext;
            }

            /// <summary>
            /// Get Eigyo list by CompanyCdSeq to fill in Eigyo dropdown
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<IEnumerable<EigyoListItem>> Handle(GetEigyosByCompanyCdSeq request, CancellationToken cancellationToken)
            {
                var query = from ei in _kobodbContext.VpmEigyos.Where(e => e.SiyoKbn == 1)
                            join c in _kobodbContext.VpmCompny.Where(e => e.SiyoKbn == 1 && e.CompanyCdSeq == request.CompanyCdSeq)
                            on ei.CompanyCdSeq equals c.CompanyCdSeq
                            join t in _kobodbContext.VpmTenant.Where(e => e.SiyoKbn == 1 && e.TenantCdSeq == request.TenantCdSeq)
                            on c.TenantCdSeq equals t.TenantCdSeq
                            orderby ei.EigyoCd
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
