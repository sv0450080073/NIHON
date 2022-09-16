using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.TkdYykSyus.Queries
{
    public class GetTkdYykSyusQuery : IRequest<IEnumerable<TkdYykSyu>>
    {
        public class Handler : IRequestHandler<GetTkdYykSyusQuery, IEnumerable<TkdYykSyu>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<IEnumerable<TkdYykSyu>> Handle(GetTkdYykSyusQuery request, CancellationToken cancellationToken)
            {
                return await _context.TkdYykSyu.ToListAsync();
            }
        }
    }
}
