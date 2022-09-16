using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.Tehai.Queries
{
    public class GetHaitaCheckQuery : IRequest<string>
    {
        public string UkeNo { get; set; }
        public short UnkRen { get; set; }
        public short TeiDanNo { get; set; }
        public short BunkRen { get; set; }
        public class Handler : IRequestHandler<GetHaitaCheckQuery, string>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<string> Handle(GetHaitaCheckQuery request, CancellationToken cancellationToken)
            {
                TkdTehai tehai = null;

                if(request.TeiDanNo == 0)
                    tehai = await _context.TkdTehai.OrderByDescending(_ => _.UpdYmd).ThenByDescending(_ => _.UpdTime)
                                                   .FirstOrDefaultAsync(_ => _.UkeNo == request.UkeNo && _.TeiDanNo == 0);
                else
                    tehai = await _context.TkdTehai.OrderByDescending(_ => _.UpdYmd).ThenByDescending(_ => _.UpdTime)
                                                   .FirstOrDefaultAsync(_ => _.UkeNo == request.UkeNo && _.UnkRen == request.UnkRen && _.TeiDanNo == request.TeiDanNo && _.BunkRen == request.BunkRen);

                return tehai == null ? string.Empty : tehai.UpdYmd + tehai.UpdTime;
            }
        }
    }
}
