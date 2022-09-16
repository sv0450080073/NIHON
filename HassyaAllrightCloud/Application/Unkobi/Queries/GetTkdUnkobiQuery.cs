using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.Unkobi.Queries
{
    public class GetTkdUnkobiQuery : IRequest<IEnumerable<TkdUnkobi>>
    {
        public class Handler : IRequestHandler<GetTkdUnkobiQuery, IEnumerable<TkdUnkobi>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<TkdUnkobi>> Handle(GetTkdUnkobiQuery request, CancellationToken cancellationToken)
            {
                return await _context.TkdUnkobi.ToListAsync();
            }
        }
    }
}
