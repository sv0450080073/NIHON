using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.TransportDailyReport.Queries
{
    public class GetListEigyoForSearchQuery : IRequest<List<EigyoSearchData>>
    {
        public int TenantCdSeq { get; set; }
        public int CompanyCdSeq { get; set; }
        public class Hanlder : IRequestHandler<GetListEigyoForSearchQuery, List<EigyoSearchData>>
        {
            private KobodbContext _context;
            public Hanlder(KobodbContext context)
            {
                _context = context;
            }
            public async Task<List<EigyoSearchData>> Handle(GetListEigyoForSearchQuery request, CancellationToken cancellationToken)
            {
                var result = await (from e in _context.VpmEigyos.Where(_ => _.SiyoKbn == CommonConstants.SiyoKbn && _.CompanyCdSeq == request.CompanyCdSeq)
                                    join c in _context.VpmCompny.Where(_ => _.SiyoKbn == CommonConstants.SiyoKbn)
                                    on e.CompanyCdSeq equals c.CompanyCdSeq
                                    join t in _context.VpmTenant.Where(_ => _.SiyoKbn == CommonConstants.SiyoKbn && _.TenantCdSeq == request.TenantCdSeq)
                                    on c.TenantCdSeq equals t.TenantCdSeq
                                    select new EigyoSearchData() { 
                                        EigyoCd = e.EigyoCd,
                                        EigyoCdSeq = e.EigyoCdSeq,
                                        RyakuNm = e.RyakuNm
                                    }).ToListAsync();

                return result;
            }
        }
    }
}
