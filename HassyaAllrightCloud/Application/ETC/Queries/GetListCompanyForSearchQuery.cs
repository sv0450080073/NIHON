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

namespace HassyaAllrightCloud.Application.ETC.Queries
{
    public class GetListCompanyForSearchQuery : IRequest<List<ETCCompany>>
    {
        public int TenantCdSeq { get; set; }
        public class Handler : IRequestHandler<GetListCompanyForSearchQuery, List<ETCCompany>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<ETCCompany>> Handle(GetListCompanyForSearchQuery request, CancellationToken cancellationToken)
            {
                var listCompany = await (from c in _context.VpmCompny.Where(_ => _.SiyoKbn == Constants.SiyoKbn && _.CompanyCdSeq == new ClaimModel().CompanyID && _.TenantCdSeq == request.TenantCdSeq)
                                         join t in _context.VpmTenant.Where(_ => _.SiyoKbn == Constants.SiyoKbn)
                                         on c.TenantCdSeq equals t.TenantCdSeq
                                         select new ETCCompany()
                                         {
                                             CompanyCdSeq = c.CompanyCdSeq,
                                             CompanyCd = c.CompanyCd,
                                             RyakuMn = c.RyakuNm,
                                             TenantCdSeq = c.TenantCdSeq
                                         }).ToListAsync();

                return listCompany;
            }
        }
    }
}
