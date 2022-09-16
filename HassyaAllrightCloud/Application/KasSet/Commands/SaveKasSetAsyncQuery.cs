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
    public class SaveKasSetAsyncQuery : IRequest<TkmKasSet>
    {
        public TkmKasSet tkmKasSet {get; set;}
        public class Handler : IRequestHandler<SaveKasSetAsyncQuery, TkmKasSet>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<TkmKasSet> Handle(SaveKasSetAsyncQuery request, CancellationToken cancellationToken)
            {
                var result = await _context.AddAsync(request.tkmKasSet);
                await _context.SaveChangesAsync();
                return result.Entity;
            }
        }
    }
}
