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
    public class PostTkdYykshoCommand : IRequest<Unit>
    {
        public TkdYyksho TkdYyksho;
        class Handler : IRequestHandler<PostTkdYykshoCommand, Unit>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(PostTkdYykshoCommand request, CancellationToken cancellationToken)
            {
                _context.TkdYyksho.Add(request.TkdYyksho);
                await _context.SaveChangesAsync();
                return Unit.Value;
            }
        }
    }
}
