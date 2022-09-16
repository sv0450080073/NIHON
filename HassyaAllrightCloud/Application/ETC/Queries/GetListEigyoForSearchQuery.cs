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
    public class GetListEigyoForSearchQuery : IRequest<List<ETCEigyo>>
    {
        public int TenantCdSeq { get; set; }
        public class Handler : IRequestHandler<GetListEigyoForSearchQuery, List<ETCEigyo>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<ETCEigyo>> Handle(GetListEigyoForSearchQuery request, CancellationToken cancellationToken)
            {
                var listEigyo = await (from e in _context.VpmEigyos.Where(_ => _.SiyoKbn == Constants.SiyoKbn)
                                       join c in _context.VpmCompny.Where(_ => _.SiyoKbn == Constants.SiyoKbn)
                                       on e.CompanyCdSeq equals c.CompanyCdSeq
                                       join t in _context.VpmTenant.Where(_ => _.SiyoKbn == Constants.SiyoKbn && _.TenantCdSeq == request.TenantCdSeq)
                                       on c.TenantCdSeq equals t.TenantCdSeq
                                       select new ETCEigyo()
                                       {
                                           CompanyCdSeq = c.CompanyCdSeq,
                                           EigyoCdSeq = e.EigyoCdSeq,
                                           EigyoCd = e.EigyoCd,
                                           RyakuMn = e.RyakuNm
                                       }).ToListAsync();

                return listEigyo;
            }
        }
    }
}
