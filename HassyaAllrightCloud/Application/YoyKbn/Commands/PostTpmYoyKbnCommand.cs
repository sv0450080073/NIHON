using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.YoyKbn.Commands
{
    public class PostVpmYoyKbnCommand : IRequest<Unit>
    {
        public VpmYoyKbn VpmYoyKbn { get; set; }
        public class Handler : IRequestHandler<PostVpmYoyKbnCommand, Unit>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(PostVpmYoyKbnCommand request, CancellationToken cancellationToken)
            {
                _context.VpmYoyKbn.Add(request.VpmYoyKbn);
                await _context.SaveChangesAsync();
                return Unit.Value;
            }
        }
    }
}
