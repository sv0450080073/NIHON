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
    public class GetSaleBranchByTenantQuery : IRequest<IEnumerable<LoadSaleBranch>>
    {
        private readonly int _tenantId;
        public GetSaleBranchByTenantQuery(int tenantId)
        {
            _tenantId = tenantId;
        }

        public class Handler : IRequestHandler<GetSaleBranchByTenantQuery, IEnumerable<LoadSaleBranch>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetSaleBranchByTenantQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetSaleBranchByTenantQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            /// <summary>
            /// #7188 fix bug only load sale branch where siyokbn = 1
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<IEnumerable<LoadSaleBranch>> Handle(GetSaleBranchByTenantQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    return await (from t in _context.VpmTenant
                                        join c in _context.VpmCompny
                                           on t.TenantCdSeq equals c.TenantCdSeq into grtc
                                        from tc in grtc.DefaultIfEmpty()
                                        join e in _context.VpmEigyos
                                            on tc.CompanyCdSeq equals e.CompanyCdSeq
                                        where t.TenantCdSeq == request._tenantId && e.SiyoKbn == 1 && tc.SiyoKbn == 1
                                        select new LoadSaleBranch()
                                        {
                                            CompanyCdSeq = tc.CompanyCdSeq,
                                            CompanyName = tc.CompanyNm,
                                            CompanyCd = tc.CompanyCd,
                                            CompanyRyakuNm = tc.RyakuNm,
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
