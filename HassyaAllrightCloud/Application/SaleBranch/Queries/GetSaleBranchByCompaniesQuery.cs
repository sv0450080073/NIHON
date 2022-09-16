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

namespace HassyaAllrightCloud.Application.SaleBranch.Queries
{
    public class GetSaleBranchByCompaniesQuery : IRequest<List<LoadSaleBranch>>
    {
        private readonly int _tenantId;
        private readonly List<int> _companyIds;

        public GetSaleBranchByCompaniesQuery(int tenantId, List<int> companyIds)
        {
            _tenantId = tenantId;
            _companyIds = companyIds ?? throw new ArgumentNullException(nameof(companyIds));
        }

        public class Handler : IRequestHandler<GetSaleBranchByCompaniesQuery, List<LoadSaleBranch>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetSaleBranchByCompaniesQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetSaleBranchByCompaniesQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<List<LoadSaleBranch>> Handle(GetSaleBranchByCompaniesQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    return await (
                                  from e in _context.VpmEigyos
                                  join c in _context.VpmCompny on new { e.CompanyCdSeq, TenantId = request._tenantId } equals new { c.CompanyCdSeq, TenantId = c.TenantCdSeq } into ec
                                  from subEC in ec.DefaultIfEmpty()
                                  where e.SiyoKbn == 1 && subEC.SiyoKbn == 1 && subEC.TenantCdSeq == request._tenantId && request._companyIds.Any(_ => _ == subEC.CompanyCdSeq)
                                  select new LoadSaleBranch()
                                  {
                                      CompanyCdSeq = subEC.CompanyCdSeq,
                                      CompanyName = subEC.CompanyNm,
                                      CompanyCd = subEC.CompanyCd,
                                      CompanyRyakuNm = subEC.RyakuNm,
                                      EigyoCdSeq = e.EigyoCdSeq,
                                      EigyoCd = e.EigyoCd,
                                      EigyoName = e.EigyoNm,
                                      TransportationPlaceCodeSeq = e.TransportationPlaceCodeSeq
                                  }).OrderBy(s => s.CompanyCd)
                                        .ThenBy(s => s.EigyoCd)
                                        .ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);

                    return new List<LoadSaleBranch>();
                }
            }
        }
    }
}
