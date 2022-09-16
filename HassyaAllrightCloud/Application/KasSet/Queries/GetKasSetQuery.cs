using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.KasSet.Queries
{
    public class GetKasSetQuery : IRequest<IEnumerable<TkmKasSet>>
    {
        public class Handler : IRequestHandler<GetKasSetQuery, IEnumerable<TkmKasSet>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<TkmKasSet>> Handle(GetKasSetQuery request, CancellationToken cancellationToken)
            {
                return await _context.TkmKasSet.ToListAsync();
            }
        }
    }
}
