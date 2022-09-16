using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.YoyKbn.Queries
{
    public class GetVpmYoyKbnByIdQuery : IRequest<VpmYoyKbn>
    {
        public int Id { get; set; }
        public class Handler : IRequestHandler<GetVpmYoyKbnByIdQuery, VpmYoyKbn>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<VpmYoyKbn> Handle(GetVpmYoyKbnByIdQuery request, CancellationToken cancellationToken)
            {
                return await _context.VpmYoyKbn.FindAsync(request.Id);
            }
        }

    }
}
