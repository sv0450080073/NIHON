using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.TkdYykshoes.Queries
{
    public class GetTkdYykshoesQuery : IRequest<IEnumerable<TkdYyksho>>
    {
        public class Handler : IRequestHandler<GetTkdYykshoesQuery, IEnumerable<TkdYyksho>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<IEnumerable<TkdYyksho>> Handle(GetTkdYykshoesQuery request, CancellationToken cancellationToken)
            {
                return await _context.TkdYyksho.ToListAsync();
            }
        }
    }
}
