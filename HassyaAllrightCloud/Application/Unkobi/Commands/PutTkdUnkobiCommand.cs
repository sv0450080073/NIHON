using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.Unkobi.Commands
{
    public class PutTkdUnkobiCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public TkdUnkobi TkdUnkobi { get; set; }

        public class Handler : IRequestHandler<PutTkdUnkobiCommand, bool>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(PutTkdUnkobiCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    _context.Entry(request.TkdUnkobi).State = EntityState.Modified;

                    await _context.SaveChangesAsync(); ;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TkdUnkobiExists(request.Id))
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
