using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BusAllocation.Queries
{
    public class GetBusAllocationHaitaCheckQuery : IRequest<BusAllocationHaitaCheck>
    {
        public string UkeNo { get; set; }
        public class Handler : IRequestHandler<GetBusAllocationHaitaCheckQuery, BusAllocationHaitaCheck>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetBusAllocationHaitaCheckQuery> _logger;
            public Handler(KobodbContext context, ILogger<GetBusAllocationHaitaCheckQuery> logger)
            {
                _context = context;
                _logger = logger;
            }
            public async Task<BusAllocationHaitaCheck> Handle(GetBusAllocationHaitaCheckQuery request, CancellationToken cancellationToken)
            {
                var tkdYyksho = await _context.TkdYyksho.Where(x => x.UkeNo == request.UkeNo).FirstOrDefaultAsync();
                var tkdUnkobiUpdYmdTime = await _context.TkdUnkobi.Where(x => x.UkeNo == request.UkeNo).MaxAsync(x => x.UpdYmd + x.UpdTime);
                var tkdHaishaUpdYmdTime = await _context.TkdHaisha.Where(x => x.UkeNo == request.UkeNo).MaxAsync(x => x.UpdYmd + x.UpdTime);
                var tkdKobanUpdYmdTime = await _context.TkdKoban.Where(x => x.UkeNo == request.UkeNo).MaxAsync(x => x.UpdYmd + x.UpdTime);
                var tkdHaiinUpdYmdTime = await _context.TkdHaiin.Where(x => x.UkeNo == request.UkeNo).MaxAsync(x => x.UpdYmd + x.UpdTime);

                return new BusAllocationHaitaCheck()
                {
                    UkeNo = request.UkeNo,
                    YYKSHO_UpdYmdTime = tkdYyksho?.UpdYmd + tkdYyksho?.UpdTime,
                    UNKOBI_UpdYmdTime = tkdUnkobiUpdYmdTime,
                    HAISHA_UpYmdTime = tkdHaishaUpdYmdTime,
                    KOBAN_UpdYmdTime = tkdKobanUpdYmdTime,
                    HAIIN_UpdYmdTime = tkdHaiinUpdYmdTime
                };
            }
        }
    }
}
