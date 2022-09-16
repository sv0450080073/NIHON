using HassyaAllrightCloud.Commons;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.VehicleAvailabilityConfirmationMobile.Queries
{
    public class CaculateBusInfo : IRequest<BusAllocationDatas>
    {
        public DateTime DateSeleccted;
        public List<Domain.Dto.BusAllocation> BusAllocations;
        public List<BusData> BusData;
        public List<ShuriData> RepairData;

        public class Handler : IRequestHandler<CaculateBusInfo, BusAllocationDatas>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context) => _context = context;

            public async Task<BusAllocationDatas> Handle(CaculateBusInfo request, CancellationToken cancellationToken)
            {
                var daySelected = request.DateSeleccted.ToString(DateTimeFormat.yyyyMMdd);
                var busAllocationsSeq = request.BusAllocations.Where(e => e.SyuKoYmd.CompareTo(daySelected) <= 0 && e.KikYmd.CompareTo(daySelected) >= 0).DistinctBy(e => e.HaiSSryCdSeq).Select(_ => _.HaiSSryCdSeq);
                var busRepair = request.RepairData.Where(e => e.ShuriSYmd.CompareTo(daySelected) <= 0 && e.ShuriEYmd.CompareTo(daySelected) >= 0).Select(e => e.SyaRyoCdSeq);
                var busData = request.BusData.Where(e => e.StaYmd.CompareTo(daySelected) <= 0 && e.EndYmd.CompareTo(daySelected) >= 0 && !busRepair.Contains(e.SyaRyoCdSeq));

                return new BusAllocationDatas()
                {
                    BusAllocationsSeqs = busAllocationsSeq,
                    BusDatas = busData,
                    DateSelected = request.DateSeleccted
                };
            }
        }
    }
}
