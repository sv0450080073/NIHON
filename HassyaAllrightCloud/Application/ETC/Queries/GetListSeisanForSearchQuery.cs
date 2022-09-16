using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.ETC.Queries
{
    public class GetListSeisanForSearchQuery : IRequest<List<ETCSeisan>>
    {
        public class Handler : IRequestHandler<GetListSeisanForSearchQuery, List<ETCSeisan>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public Task<List<ETCSeisan>> Handle(GetListSeisanForSearchQuery request, CancellationToken cancellationToken)
            {
                var currentTenant = new ClaimModel().TenantID;
                return _context.VpmSeisan.Where(e => e.SiyoKbn == Constants.SiyoKbn && e.TenantCdSeq == currentTenant).Select(e => new ETCSeisan()
                {
                    SeisanCdSeq = e.SeisanCdSeq,
                    SeisanCd = e.SeisanCd,
                    SeisanKbn = e.SeisanKbn,
                    SeisanNm = e.SeisanNm
                }).ToListAsync();
            }
        }
    }
}
