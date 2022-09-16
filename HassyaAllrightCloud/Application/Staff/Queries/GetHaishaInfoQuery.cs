using HassyaAllrightCloud.Commons.Constants;
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
    public class GetHaishaInfoQuery : IRequest<HaishaStaffItem>
    {
        public string UkeNo { get; set; }
        public short UnkRen { get; set; }
        public short TeiDanNo { get; set; }
        public short BunkRen { get; set; }
        public class Handler : IRequestHandler<GetHaishaInfoQuery, HaishaStaffItem>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<HaishaStaffItem> Handle(GetHaishaInfoQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    HaishaStaffItem result = await _context.TkdHaisha.Where(_ => _.UkeNo == request.UkeNo && _.UnkRen == request.UnkRen
                                                                              && _.TeiDanNo == request.TeiDanNo && _.BunkRen == request.BunkRen && _.SiyoKbn == CommonConstants.SiyoKbn)
                                                                     .Select(_ => new HaishaStaffItem()
                                                                     {
                                                                         UkeNo = _.UkeNo,
                                                                         UnkRen = _.UnkRen,
                                                                         TeiDanNo = _.TeiDanNo,
                                                                         BunkRen = _.BunkRen,
                                                                         SyaSyuRen = _.SyaSyuRen,
                                                                         SyuKoYmd = _.SyuKoYmd,
                                                                         SyuKoTime = _.SyuKoTime,
                                                                         KikYmd = _.KikYmd,
                                                                         KikTime = _.KikTime
                                                                     }).FirstOrDefaultAsync();

                    return result;
                }
                catch(Exception ex)
                {
                    // TODO: write log
                    throw ex;
                }
            }
        }
    }
}
