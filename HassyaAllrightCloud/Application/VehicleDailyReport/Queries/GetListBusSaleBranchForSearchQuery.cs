using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HassyaAllrightCloud.Commons.Constants;

namespace HassyaAllrightCloud.Application.VehicleDailyReport.Queries
{
    public class GetListBusSaleBranchForSearchQuery : IRequest<List<BusSaleBranchSearch>>
    {
        public int TenantCdSeq { get; set; }
        public class Handler : IRequestHandler<GetListBusSaleBranchForSearchQuery, List<BusSaleBranchSearch>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            /// <summary>
            /// Get list bus sale for search
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<List<BusSaleBranchSearch>> Handle(GetListBusSaleBranchForSearchQuery request, CancellationToken cancellationToken)
            {
                var result = await (from e in _context.VpmEigyos.Where(_ => _.SiyoKbn == CommonConstants.SiyoKbn)
                                    join c in  _context.VpmCompny.Where(_ => _.SiyoKbn == CommonConstants.SiyoKbn && _.TenantCdSeq == request.TenantCdSeq)
                                    on e.CompanyCdSeq equals c.CompanyCdSeq
                                    select new BusSaleBranchSearch()
                                    {
                                        EigyoCdSeq = e.EigyoCdSeq,
                                        EigyoCd = e.EigyoCd,
                                        RyakuNm = e.RyakuNm
                                    }).OrderBy(_ => _.EigyoCd).ToListAsync();
                return result;
            }
        }
    }
}
