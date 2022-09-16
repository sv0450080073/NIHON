using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.Haisha.Commands
{
    public class PostTkdHaishaCommand : IRequest<bool>
    {
        public TkdHaisha tkdHaisha { get; set; }

        public class Handler : IRequestHandler<PostTkdHaishaCommand, bool>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<bool> Handle(PostTkdHaishaCommand request, CancellationToken cancellationToken)
            {
                _context.TkdHaisha.Add(request.tkdHaisha);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (TkdHaishaExists(request.tkdHaisha.UkeNo))
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
            private bool TkdHaishaExists(string id)
            {
                return _context.TkdHaisha.Any(e => e.UkeNo == id);
            }
        }
    }
}
