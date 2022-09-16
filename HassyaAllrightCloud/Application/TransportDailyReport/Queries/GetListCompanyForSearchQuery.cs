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
    public class GetListCompanyForSearchQuery : IRequest<List<CompanySearchData>>
    {
        public int CompanyCdSeq { get; set; }
        public class Handler : IRequestHandler<GetListCompanyForSearchQuery, List<CompanySearchData>>
        {
            private KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<List<CompanySearchData>> Handle(GetListCompanyForSearchQuery request, CancellationToken cancellationToken)
            {
                var result = await (from c in _context.VpmCompny.Where(_ => _.CompanyCdSeq == request.CompanyCdSeq && _.SiyoKbn == CommonConstants.SiyoKbn)
                                    join t in _context.VpmTenant.Where(_ => _.SiyoKbn == CommonConstants.SiyoKbn)
                                    on c.TenantCdSeq equals t.TenantCdSeq
                                    select new CompanySearchData()
                                    {
                                        CompanyCdSeq = c.CompanyCdSeq,
                                        CompanyCd = c.CompanyCd,
                                        RyakuNm = c.RyakuNm,
                                        TenantCdSeq = c.TenantCdSeq
                                    }).ToListAsync();

                return result;
            }
        }
    }
}
