using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.Unkobi.Commands
{
    public class PostTkdUnkobiCommand : IRequest<bool>
    {
        public TkdUnkobi TkdUnkobi { get; set; }
        public class Handler : IRequestHandler<PostTkdUnkobiCommand, bool>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(PostTkdUnkobiCommand request, CancellationToken cancellationToken)
            {
                _context.TkdUnkobi.Add(request.TkdUnkobi);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if(TkdUnkobiExists(request.TkdUnkobi.UkeNo))
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
            private bool TkdUnkobiExists(string id)
            {
                return _context.TkdUnkobi.Any(e => e.UkeNo == id);
            }
        }
    }
}
