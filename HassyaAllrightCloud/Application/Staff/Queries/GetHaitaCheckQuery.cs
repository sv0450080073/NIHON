using HassyaAllrightCloud.Domain.Dto;
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
    public class GetHaitaCheckQuery : IRequest<StaffHaitaCheck>
    {
        public string UkeNo { get; set; }
        public short UnkRen { get; set; }
        public short TeiDanNo { get; set; }
        public short BunkRen { get; set; }

        public class Handler : IRequestHandler<GetHaitaCheckQuery, StaffHaitaCheck>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<StaffHaitaCheck> Handle(GetHaitaCheckQuery request, CancellationToken cancellationToken)
            {
                StaffHaitaCheck result = new StaffHaitaCheck();

                var haisha = await _context.TkdHaisha.FirstOrDefaultAsync(_ => _.UkeNo == request.UkeNo && _.UnkRen == request.UnkRen
                                                                            && _.TeiDanNo == request.TeiDanNo && _.BunkRen == request.BunkRen);

                var haiin = await _context.TkdHaiin.OrderByDescending(_ => _.UpdYmd).ThenByDescending(_ => _.UpdTime)
                                                   .FirstOrDefaultAsync(_ => _.UkeNo == request.UkeNo && _.UnkRen == request.UnkRen
                                                                          && _.TeiDanNo == request.TeiDanNo && _.BunkRen == request.BunkRen);

                result.HaishaUpdYmd = haisha?.UpdYmd ?? string.Empty;
                result.HaishaUpdTime = haisha?.UpdTime ?? string.Empty;
                result.HaiinUpdYmdTime = haiin == null ? string.Empty : haiin.UpdYmd + haiin.UpdTime;

                return result;
            }
        }
    }
}
