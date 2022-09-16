using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.TkdYykSyus.Commands
{
    public class PutTkdYykSyuCommand : IRequest<bool>
    {
        public TkdYykSyu TkdYykSyu;
        public string Id;
        class Handler : IRequestHandler<PutTkdYykSyuCommand, bool>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(PutTkdYykSyuCommand request, CancellationToken cancellationToken)
            {
                _context.Entry(request.TkdYykSyu).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TkdYykSyuExists(request.Id))
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
            private bool TkdYykSyuExists(string id)
            {
                return _context.TkdYykSyu.Any(e => e.UkeNo == id);
            }
        }
    }
}
