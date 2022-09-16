using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.TkdYykshoes.Commands
{
    public class DeleteTkdYykshoCommand : IRequest<TkdYyksho>
    {
        public int Id;
        class Handler : IRequestHandler<DeleteTkdYykshoCommand, TkdYyksho>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<TkdYyksho> Handle(DeleteTkdYykshoCommand request, CancellationToken cancellationToken)
            {
                var tkdYyksho = await _context.TkdYyksho.FindAsync(request.Id);
                if (tkdYyksho == null)
                {
                    return null;
                }

                _context.TkdYyksho.Remove(tkdYyksho);
                await _context.SaveChangesAsync();
                return tkdYyksho;
            }
        }
    }
}
