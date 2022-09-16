using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.CouponPayment.Queries
{
    public class GetYoyKbn : IRequest<List<YoyaKbnItem>>
    {
        public int TenantCdSeq { get; set; }
        public class Handler : IRequestHandler<GetYoyKbn, List<YoyaKbnItem>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<List<YoyaKbnItem>> Handle(GetYoyKbn request, CancellationToken cancellationToken)
            {
                var query = _context.VpmYoyKbn.Where(e => e.SiyoKbn == 1).OrderBy(e => e.YoyaKbn).Select(e => new YoyaKbnItem() {
                    YoyaKbn = e.YoyaKbn,
                    YoyaKbnNm = e.YoyaKbnNm
                });
                return await query.ToListAsync(cancellationToken);
            }
        }
    }
}
