using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.TkdYykSyus.Commands
{
    public class DeleteTkdYykSyuCommand : IRequest<TkdYykSyu>
    {
        public int Id;
        class Handler : IRequestHandler<DeleteTkdYykSyuCommand, TkdYykSyu>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<TkdYykSyu> Handle(DeleteTkdYykSyuCommand request, CancellationToken cancellationToken)
            {
                var tkdYykSyu = await _context.TkdYykSyu.FindAsync(request.Id);
                if (tkdYykSyu == null)
                {
                    return null;
                }

                _context.TkdYykSyu.Remove(tkdYykSyu);
                await _context.SaveChangesAsync();
                return tkdYykSyu;
            }
        }
    }
}
