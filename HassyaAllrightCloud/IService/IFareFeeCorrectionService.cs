using HassyaAllrightCloud.Application.FareFeeCorrection.Commands;
using HassyaAllrightCloud.Application.FareFeeCorrection.Queries;
using HassyaAllrightCloud.Commons;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IFareFeeCorrectionService
    {
        Task<List<CompanyValidate>> GetCompanyValidate();
        Task<List<Reservation>> GetReservationList(string tenantCdSeq, string ukeNo);
        Task<List<VehicleAllocation>> GetVehicleAllocationList(string tenantCdSeq, string ukeNo);
        Task<List<Vehicle>> GetVehicle(string tenantCdSeq, string ukeNo);
        Task<bool> SaveOrUpdateVehicleAllocation(ReservationGrid reservationGrid);
        Task<HaitaCheckModel> GetHaitaCheck(string ukeNo);
    }
    public class FareFeeCorrectionService : IFareFeeCorrectionService
    {
        private readonly IMediator _mediator;
        private readonly KobodbContext _context;
        private readonly int lockTime;

        public FareFeeCorrectionService(IMediator mediator, KobodbContext context, IConfiguration config)
        {
            if (int.TryParse(config["MySettings:UkeNoLockTime"], out int temp))
            {
                lockTime = temp;
            }
            else lockTime = 60;
            _mediator = mediator;
            _context = context;
        }

        public async Task<List<CompanyValidate>> GetCompanyValidate()
        {
            return await _mediator.Send(new GetCompanyValidate());
        }

        public async Task<List<Reservation>> GetReservationList(string tenantCdSeq, string ukeNo)
        {
            return await _mediator.Send(new GetReservationList { TenantCdSeq = tenantCdSeq, UkeNo = ukeNo });
        }

        public async Task<List<VehicleAllocation>> GetVehicleAllocationList(string tenantCdSeq, string ukeNo)
        {
            return await _mediator.Send(new GetVehicleAllocationList { TenantCdSeq = tenantCdSeq, UkeNo = ukeNo });
        }

        public async Task<List<Vehicle>> GetVehicle(string tenantCdSeq, string ukeNo)
        {
            return await _mediator.Send(new GetVehicle { TenantCdSeq = tenantCdSeq, UkeNo = ukeNo });
        }

        public async Task<bool> SaveOrUpdateVehicleAllocation(ReservationGrid reservationGrid)
        {
            return await _mediator.Send(new SaveOrUpdateVehicleAllocation { ReservationItem = reservationGrid });
        }

        public async Task<HaitaCheckModel> GetHaitaCheck(string ukeNo)
        {
            return await _mediator.Send(new GetHaitaCheck { UkeNo = ukeNo });
        }
    }
}
