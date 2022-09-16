using HassyaAllrightCloud.Application.VehicleSchedulerMobile.Queries;
using HassyaAllrightCloud.Domain.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IVehicleSchedulerMobileService
    {
        Task<VehicleSchedulerEigyoData> GetEigyoData(int syaRyoCdSeq);
        Task<List<VehicleSchedulerSyaRyoData>> GetListSyaRyoData(int eigyoCdSeq);
        Task<List<VehicleAllocationData>> GetListVehicleAllocation(string startYmd, string endYmd, int syaRyoCdSeq, int tenantCdSeq);
        Task<List<VehicleRepairData>> GetListVehicleRepair(string startYmd, string endYmd, int syaRyoCdSeq);
    }

    public class VehicleSchedulerMobileService : IVehicleSchedulerMobileService
    {
        private readonly IMediator _mediator;
        public VehicleSchedulerMobileService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<VehicleSchedulerEigyoData> GetEigyoData(int syaRyoCdSeq)
        {
            return await _mediator.Send(new GetEigyoDataQuery() { SyaRyoCdSeq = syaRyoCdSeq });
        }

        public async Task<List<VehicleSchedulerSyaRyoData>> GetListSyaRyoData(int eigyoCdSeq)
        {
            return await _mediator.Send(new GetListSyaRyoQuery() { EigyoCdSeq = eigyoCdSeq });
        }

        public async Task<List<VehicleAllocationData>> GetListVehicleAllocation(string startYmd, string endYmd, int syaRyoCdSeq, int tenantCdSeq)
        {
            return await _mediator.Send(new GetListVehicleAllocationQuery() { StartYmd = startYmd, EndYmd = endYmd, SyaRyoCdSeq = syaRyoCdSeq, TenantCdSeq = tenantCdSeq });
        }

        public async Task<List<VehicleRepairData>> GetListVehicleRepair(string startYmd, string endYmd, int syaRyoCdSeq)
        {
            return await _mediator.Send(new GetListVehicleRepairQuery() { StartYmd = startYmd, EndYmd = endYmd, SyaRyoCdSeq = syaRyoCdSeq });
        }
    }
}
