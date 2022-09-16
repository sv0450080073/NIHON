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
    public class PostTkdYykSyuCommand : IRequest<bool>
    {
        public TkdYykSyu TkdYykSyu;
        class Handler : IRequestHandler<PostTkdYykSyuCommand, bool>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(PostTkdYykSyuCommand request, CancellationToken cancellationToken)
            {
                _context.TkdYykSyu.Add(request.TkdYykSyu);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (TkdYykSyuExists(request.TkdYykSyu.UkeNo))
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
