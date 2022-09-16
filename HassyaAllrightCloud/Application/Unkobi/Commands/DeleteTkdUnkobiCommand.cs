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
    public class DeleteTkdUnkobiCommand : IRequest<TkdUnkobi>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<DeleteTkdUnkobiCommand, TkdUnkobi>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<TkdUnkobi> Handle(DeleteTkdUnkobiCommand request, CancellationToken cancellationToken)
            {
                var tkdUnkobi = await _context.TkdUnkobi.FirstOrDefaultAsync(x => x.UkeNo == request.Id);
                _context.TkdUnkobi.Remove(tkdUnkobi);
                await _context.SaveChangesAsync();
                return tkdUnkobi;
            }
        }
    }
}
