using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.AttendanceReport.Queries
{
    public class VehicleAllocationDataItem
    {
        public int HaiSSryCdSeq { get; set; }
        public string SyuKoYmd { get; set; }
        public string SyuKoTime { get; set; }
        public string KikYmd { get; set; }
        public string KikTime { get; set; }
    }
    public class GetListVehicleAllocation : IRequest<List<VehicleAllocationDataItem>>
    {
        public AttendanceReportModel Model { get; set; }
        public class Handler : IRequestHandler<GetListVehicleAllocation, List<VehicleAllocationDataItem>>
        {
            public KobodbContext _context { get; set; }
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<List<VehicleAllocationDataItem>> Handle(GetListVehicleAllocation request, CancellationToken cancellationToken)
            {
                var result = new List<VehicleAllocationDataItem>();
                if (request == null || request.Model == null) throw new Exception($"Fail to execute {nameof(GetListVehicleAllocation)}!!! Model is null");
                var proc = _context.LoadStoredProc("PK_dGetVehicleAllocaltionData_R")
                    .AddParam("@ProcessingDate", request.Model.ProcessingDate)
                    .AddParam("@EigyoCdFrom", request.Model.EigyoFrom)
                    .AddParam("@EigyoCdTo", request.Model.EigyoTo)
                    .AddParam("@CompanyCdSeq", request.Model.CompanyCdSeq)
                    .AddParam("@YoyaKbnSortFrom", request.Model.RegistrationTypeSortFrom)
                    .AddParam("@YoyaKbnSortTo", request.Model.RegistrationTypeSortTo)
                    .AddParam("@TenantCdSeq", request.Model.TenantId);
                await proc.ExecAsync(async r => result = await r.ToListAsync<VehicleAllocationDataItem>(cancellationToken));
                return result;
            }
        }
    }
}
