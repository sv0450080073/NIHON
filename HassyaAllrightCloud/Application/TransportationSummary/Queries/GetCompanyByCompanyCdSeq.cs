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
    public class GetCompanyByCompanyCdSeq : IRequest<IEnumerable<CompanyListItem>>
    {
        public int CompanyCdSeq { get; set; }
        public class Handler : IRequestHandler<GetCompanyByCompanyCdSeq, IEnumerable<CompanyListItem>>
        {
            private readonly KobodbContext _kobodbContext;
            public Handler(KobodbContext kobodbContext) => _kobodbContext = kobodbContext;

            /// <summary>
            /// Get Company list to fill in Company dropdown
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<IEnumerable<CompanyListItem>> Handle(GetCompanyByCompanyCdSeq request, CancellationToken cancellationToken)
            {
                var query = (from c in _kobodbContext.VpmCompny.Where(e => e.SiyoKbn == 1 && (request.CompanyCdSeq == 0 || request.CompanyCdSeq == e.CompanyCdSeq) && e.TenantCdSeq == new ClaimModel().TenantID)
                             join t in _kobodbContext.VpmTenant.Where(e => e.SiyoKbn == 1)
                             on c.TenantCdSeq equals t.TenantCdSeq
                             select new CompanyListItem()
                             {
                                 CompanyCd = c.CompanyCd,
                                 RyakuNm = c.RyakuNm,
                                 CompanyCdSeq = c.CompanyCdSeq,
                                 TenantCdSeq = c.TenantCdSeq
                             }).OrderBy(c => c.CompanyCd);
                return await query.ToListAsync(cancellationToken);
            }
        }
    }
}
