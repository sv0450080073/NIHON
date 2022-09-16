using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.VehicalData.Queries
{
    public class GetVehicleByBranchIdQuery : IRequest<IEnumerable<Vehical>>
    {
        private readonly int _branchId;
        private readonly DateTime _leaveGarageTime;
        private readonly DateTime _returnGarageTime;

        public GetVehicleByBranchIdQuery(int branchId, DateTime leaveGarageTime, DateTime returnGarageTime)
        {
            if (leaveGarageTime.CompareTo(returnGarageTime) >= 0)
                throw new ArgumentException("Make sure leave time earlier than return time.");

            _branchId = branchId;
            _leaveGarageTime = leaveGarageTime;
            _returnGarageTime = returnGarageTime;
        }

        public class Handler : IRequestHandler<GetVehicleByBranchIdQuery, IEnumerable<Vehical>>
        {
            private readonly KobodbContext _context;
            
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<Vehical>> Handle(GetVehicleByBranchIdQuery request, CancellationToken cancellationToken)
            {
                var VPM_HenSyas = _context.VpmHenSya;
                var VPM_SyaSyus = _context.VpmSyaSyu;
                var VPM_SyaRyos = _context.VpmSyaRyo;

                string strLeaveGaraTime = request._leaveGarageTime.ToString("yyyyMMdd");
                string strReturnGaraTime = request._returnGarageTime.ToString("yyyyMMdd");

                return await 
                        (from hs in VPM_HenSyas
                        join sr in VPM_SyaRyos on hs.SyaRyoCdSeq equals sr.SyaRyoCdSeq
                        join ss in VPM_SyaSyus on sr.SyaSyuCdSeq equals ss.SyaSyuCdSeq
                        where (request._branchId == hs.EigyoCdSeq)
                               && (ss.SiyoKbn == 1)
                               && (strLeaveGaraTime.CompareTo(hs.StaYmd) >= 0) && (strLeaveGaraTime.CompareTo(hs.EndYmd) < 0)
                               && (strReturnGaraTime.CompareTo(hs.StaYmd) >= 0) && (strReturnGaraTime.CompareTo(hs.EndYmd) < 0)
                        orderby ss.KataKbn, ss.SyaSyuCd, sr.SyaRyoCd
                        select new Vehical { EigyoCdSeq = hs.EigyoCdSeq, KataKbn = ss.KataKbn, VehicleModel = sr }).ToListAsync();
            }
        }
    }
}
