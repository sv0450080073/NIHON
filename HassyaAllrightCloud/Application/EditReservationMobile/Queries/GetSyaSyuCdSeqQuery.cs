using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.EditReservationMobile.Queries
{
    public class GetSyaSyuCdSeqQuery : IRequest<int>
    {
        public int SyaRyoCdSeq { get; set; }
        public class Handler : IRequestHandler<GetSyaSyuCdSeqQuery, int>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(GetSyaSyuCdSeqQuery request, CancellationToken cancellationToken)
            {
                var result = await _context.VpmSyaRyo.FirstOrDefaultAsync(_ => _.SyaRyoCdSeq == request.SyaRyoCdSeq);
                if(result != null)
                {
                    return result.SyaSyuCdSeq;
                }
                return 0;
            }
        }
    }
}
