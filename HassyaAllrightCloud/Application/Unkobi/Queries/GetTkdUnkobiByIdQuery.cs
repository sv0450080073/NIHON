using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.Unkobi.Queries
{
    public class GetTkdUnkobiByIdQuery : IRequest<TkdUnkobi>
    {
        public string Id { get; set; }
        public short UnkRen { get; set; }

        public class Handler : IRequestHandler<GetTkdUnkobiByIdQuery, TkdUnkobi>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<TkdUnkobi> Handle(GetTkdUnkobiByIdQuery request, CancellationToken cancellationToken)
            {
                return await _context.TkdUnkobi.FirstOrDefaultAsync(x=> x.UkeNo == request.Id && x.UnkRen == request.UnkRen);
            }
        }
    }
}
