using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.SaleCompany.Queries
{
    public class GetCompanyByTenantIdQuery : IRequest<List<CompanyData>>
    {
        public int TenantId { get; set; }

        public GetCompanyByTenantIdQuery(int tenantId)
        {
            TenantId = tenantId;
        }

        public class Handler : IRequestHandler<GetCompanyByTenantIdQuery, List<CompanyData>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetCompanyByTenantIdQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetCompanyByTenantIdQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<List<CompanyData>> Handle(GetCompanyByTenantIdQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    return await (from s in _context.VpmCompny
                                  where s.SiyoKbn == 1 && s.TenantCdSeq == request.TenantId
                                  orderby s.CompanyCd ascending
                                  select new CompanyData()
                                  {
                                      CompanyCdSeq = s.CompanyCdSeq,
                                      CompanyCd = s.CompanyCd,
                                      CompanyNm = s.CompanyNm,
                                      RyakuNm = s.RyakuNm
                                  }).ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogTrace(ex.ToString());

                    return new List<CompanyData>();
                }
            }
        }
    }
}
