using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.TkdYykSyus.Queries
{
    public class GetTkdYykSyuQuery : IRequest<TkdYykSyu>
    {
        public int Id { get; set; }
        public class Handler : IRequestHandler<GetTkdYykSyuQuery, TkdYykSyu>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<TkdYykSyu> Handle(GetTkdYykSyuQuery request, CancellationToken cancellationToken)
            {
                return await _context.TkdYykSyu.FindAsync(request.Id);
            }
        }
    }
}
