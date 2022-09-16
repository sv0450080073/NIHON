using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.Mishum.Commands
{
    public class PostTkdMishumCommand : IRequest<bool>
    {
        public TkdMishum tkdMishum { get; set; }

        public class Handler : IRequestHandler<PostTkdMishumCommand, bool>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(PostTkdMishumCommand request, CancellationToken cancellationToken)
            {
                _context.TkdMishum.Add(request.tkdMishum);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (TkdMishunExists(request.tkdMishum.UkeNo))
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

            private bool TkdMishunExists(string id)
            {
                return _context.TkdMishum.Any(e => e.UkeNo == id);
            }
        }
    }
}
