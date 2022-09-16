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
    public class GetListFutaiForSearchQuery : IRequest<List<ETCFutai>>
    {
        public int TenantCdSeq { get; set; }
        public class Handler : IRequestHandler<GetListFutaiForSearchQuery, List<ETCFutai>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<ETCFutai>> Handle(GetListFutaiForSearchQuery request, CancellationToken cancellationToken)
            {
                var listFutai = await (from f in _context.VpmFutai.Where(_ => _.SiyoKbn == Constants.SiyoKbn)
                                       join t in _context.VpmTenant.Where(_ => _.SiyoKbn == Constants.SiyoKbn && _.TenantCdSeq == request.TenantCdSeq)
                                       on f.TenantCdSeq equals t.TenantCdSeq
                                       select new ETCFutai()
                                       {
                                           FutaiCdSeq = f.FutaiCdSeq,
                                           FutaiCd = f.FutaiCd,
                                           FutaiNm = f.FutaiNm,
                                           ZeiHyoKbn = f.ZeiHyoKbn
                                       }).ToListAsync();

                return listFutai;
            }
        }
    }
}
