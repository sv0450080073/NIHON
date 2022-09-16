using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BusAllocation.Queries
{
    public class GetBranchChartDataQuery : IRequest<List<BranchChartData>>
    {
        public int TenantCdSeq { get; set; } = 0;
        public class Handler : IRequestHandler<GetBranchChartDataQuery, List<BranchChartData>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<List<BranchChartData>> Handle(GetBranchChartDataQuery request, CancellationToken cancellationToken)
            {
                var result = new List<BranchChartData>();
                try
                {
                    result = (from EIGYOUS in _context.VpmEigyos
                              join COMPNY in _context.VpmCompny
                              on new { C1 = EIGYOUS.CompanyCdSeq, C2 = request.TenantCdSeq }
                              equals new { C1 = COMPNY.CompanyCdSeq, C2 = COMPNY.TenantCdSeq }
                              where COMPNY.SiyoKbn == 1
                              && EIGYOUS.SiyoKbn == 1
                              select new BranchChartData()
                              {
                                  CompanyCdSeq = COMPNY.CompanyCdSeq,
                                  CompanyCd = COMPNY.CompanyCd,
                                  Com_RyakuNm = COMPNY.RyakuNm,
                                  EigyoCdSeq = EIGYOUS.EigyoCdSeq,
                                  EigyoCd = EIGYOUS.EigyoCd,
                                  RyakuNm = EIGYOUS.RyakuNm
                              }).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    return result;
                }
            }
        }
    }

}
