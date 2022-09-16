using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.YoyKbn.Queries
{
    public class GetVpmYoyKbnQuery : IRequest<IEnumerable<VpmYoyKbn>>
    {
        public class Handler : IRequestHandler<GetVpmYoyKbnQuery, IEnumerable<VpmYoyKbn>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<VpmYoyKbn>> Handle(GetVpmYoyKbnQuery request, CancellationToken cancellationToken)
            {
                return await _context.VpmYoyKbn.ToListAsync();
            }
        }
    }
}
