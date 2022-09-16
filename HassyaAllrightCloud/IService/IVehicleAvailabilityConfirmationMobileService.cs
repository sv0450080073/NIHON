using HassyaAllrightCloud.Application.VehicleAvailabilityConfirmationMobile.Queries;
using HassyaAllrightCloud.Domain.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IVehicleAvailabilityConfirmationMobileService
    {
        Task<IEnumerable<BusType>> GetBusTypeListItems();
        Task<BusAllocationDatas> CaculateBusInfo(DateTime dateSeleccted, List<BusAllocation> busAllocations, List<BusData> busData, List<ShuriData> repairData);
    }
    public class VehicleAvailabilityConfirmationMobileService : IVehicleAvailabilityConfirmationMobileService
    {
        private readonly IMediator _mediator;
        public VehicleAvailabilityConfirmationMobileService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<BusType>> GetBusTypeListItems()
        {
            return await _mediator.Send(new GetBusTypeListItems());
        }

        public async Task<BusAllocationDatas> CaculateBusInfo(DateTime dateSelected, List<BusAllocation> busAllocations, List<BusData> busData, List<ShuriData> repairData)
        {
            return await _mediator.Send(new CaculateBusInfo() { DateSeleccted = dateSelected, BusAllocations = busAllocations, BusData = busData, RepairData = repairData });
        }
    }
}
