using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.Staff.Queries
{
    public class GetHaitaCheckDeleteQuery : IRequest<string>
    {
        public int SyainCdSeq { get; set; }
        public string UnkYmd { get; set; }
        
        public class Handler : IRequestHandler<GetHaitaCheckDeleteQuery, string>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<string> Handle(GetHaitaCheckDeleteQuery request, CancellationToken cancellationToken)
            {
                var koban = await  _context.TkdKoban.OrderByDescending(_ => _.UpdYmd).ThenByDescending(_ => _.UpdTime)
                                                    .FirstOrDefaultAsync(_ => _.KinKyuTblCdSeq > 0 && _.SyainCdSeq == request.SyainCdSeq && _.UnkYmd == request.UnkYmd);

                string result = koban == null ? string.Empty : koban.UpdYmd + koban.UpdTime;

                return result;
            }
        }
    }
}
