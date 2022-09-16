using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.TkdYykshoes.Commands
{
    public class PutTkdYykshoCommand : IRequest<bool>
    {
        public TkdYyksho TkdYyksho;
        public string Id;
        class Handler : IRequestHandler<PutTkdYykshoCommand, bool>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(PutTkdYykshoCommand request, CancellationToken cancellationToken)
            {
                _context.Entry(request.TkdYyksho).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TkdYykshoExists(request.Id))
                    {
                        return false;
                    }
                    else
                    {
                        throw;
                    }
                }
                return true;
            }
            private bool TkdYykshoExists(string id)
            {
                return _context.TkdYyksho.Any(e => e.UkeNo == id);
            }
        }
    }
}
