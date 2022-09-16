using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HassyaAllrightCloud.Application.LoanBookingIncidental.Queries
{
    public class GetLoadYRyokinQuery : IRequest<List<LoadYRyokin>>
    {
        public class Handler : IRequestHandler<GetLoadYRyokinQuery, List<LoadYRyokin>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<LoadYRyokin>> Handle(GetLoadYRyokinQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await _context.VpmRyokin
                        .OrderBy(c => c.RyokinTikuCd)
                        .ThenBy(c => c.RyokinCd)
                        .Select(c => new LoadYRyokin()
                        {
                            RyoKinCd = Convert.ToInt16(c.RyokinCd),
                            RyoKinNm = c.RyokinNm,
                            RyoKinTikuCd = Convert.ToByte(c.RyokinTikuCd),
                            SiyoStaYmd = c.SiyoStaYmd,
                            SiyoEndYmd = c.SiyoEndYmd
                        })
                        .ToListAsync();
                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
