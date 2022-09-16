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
    public class GetKasSetByIdQuery : IRequest<TkmKasSet>
    {
        public int Id { get; set; }

        public class Handler : IRequestHandler<GetKasSetByIdQuery, TkmKasSet>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<TkmKasSet> Handle(GetKasSetByIdQuery request, CancellationToken cancellationToken)
            {
                return await _context.TkmKasSet.FindAsync(request.Id);
            }
        }
    }
}
