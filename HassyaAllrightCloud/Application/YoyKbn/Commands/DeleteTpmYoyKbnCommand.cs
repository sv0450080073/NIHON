using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.YoyKbn.Commands
{
    public class DeleteVpmYoyKbnCommand : IRequest<VpmYoyKbn>
    {
        public int Id { get; set; }

        public class Handler : IRequestHandler<DeleteVpmYoyKbnCommand, VpmYoyKbn>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<VpmYoyKbn> Handle(DeleteVpmYoyKbnCommand request, CancellationToken cancellationToken)
            {
                var VpmYoyKbn = await _context.VpmYoyKbn.FindAsync(request.Id);
                _context.VpmYoyKbn.Remove(VpmYoyKbn);
                await _context.SaveChangesAsync();
                return VpmYoyKbn;
            }
        }
    }
}
