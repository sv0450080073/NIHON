using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.VehicleDailyReport.Queries
{
    public class GetHaitaCheckQuery : IRequest<HaitaCheckVehicleDailyReport>
    {
        public string UkeNo { get; set; }
        public short UnkRen { get; set; }
        public short TeiDanNo { get; set; }
        public short BunkRen { get; set; }
        public string UnkYmd { get; set; }
        public class Handler : IRequestHandler<GetHaitaCheckQuery, HaitaCheckVehicleDailyReport>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<HaitaCheckVehicleDailyReport> Handle(GetHaitaCheckQuery request, CancellationToken cancellationToken)
            {
                HaitaCheckVehicleDailyReport result = new HaitaCheckVehicleDailyReport();

                var haisha = _context.TkdHaisha.FirstOrDefault(_ => _.UkeNo == request.UkeNo && _.UnkRen == request.UnkRen
                                                                 && _.TeiDanNo == request.TeiDanNo && _.BunkRen == request.BunkRen);

                var shabni = _context.TkdShabni.FirstOrDefault(_ => _.UkeNo == request.UkeNo && _.UnkRen == request.UnkRen
                                                                 && _.TeiDanNo == request.TeiDanNo && _.BunkRen == request.BunkRen && _.UnkYmd == request.UnkYmd);

                result.UpdYmd = shabni?.UpdYmd ?? string.Empty;
                result.UpdTime = shabni?.UpdTime ?? string.Empty;
                result.HaishaUpdYmd = haisha?.UpdYmd ?? string.Empty;
                result.HaishaUpdTime = haisha?.UpdTime ?? string.Empty;

                return result;
            }
        }
    }
}
