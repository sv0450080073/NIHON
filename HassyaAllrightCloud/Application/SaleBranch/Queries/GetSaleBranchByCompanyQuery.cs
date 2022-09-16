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
    public class GetSaleBranchByCompanyQuery : IRequest<List<LoadSaleBranch>>
    {
        private readonly int _tenantId;
        private readonly int _companyId;

        public GetSaleBranchByCompanyQuery(int companyId, int tenantId)
        {
            _tenantId = tenantId;
            _companyId = companyId;
        }

        public class Handler : IRequestHandler<GetSaleBranchByCompanyQuery, List<LoadSaleBranch>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetSaleBranchByCompanyQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetSaleBranchByCompanyQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<List<LoadSaleBranch>> Handle(GetSaleBranchByCompanyQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    return await (
                                  from e in _context.VpmEigyos
                                  join c in _context.VpmCompny on new { e.CompanyCdSeq, TenantId = request._tenantId } equals new { c.CompanyCdSeq, TenantId = c.TenantCdSeq } into ec
                                  from subEC in ec.DefaultIfEmpty()
                                  where e.SiyoKbn == 1 && subEC.SiyoKbn == 1 && subEC.TenantCdSeq == request._tenantId && subEC.CompanyCdSeq == request._companyId
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
