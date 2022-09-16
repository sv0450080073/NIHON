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

    public class VehicleRepairDataItem
    {
        public int ShuriCdSeq { get; set; }
        public int SyaRyoCdSeq { get; set; }
        public string SyuRiCdCodeKbn { get; set; }
        public string SyuRiCdCodeKbnNm { get; set; }
        public string SyuRiCdRyakuNm { get; set; }
        public int EigyoCdSeq { get; set; }
        public int EigyosEigyoCd { get; set; }
        public string EigyosEigyoNm { get; set; }
        public string EigyosRyakuNm { get; set; }
    }
    public class GetListVehicleRepair : IRequest<List<VehicleRepairDataItem>>
    {
        public AttendanceReportModel Model { get; set; }
        public class Handler : IRequestHandler<GetListVehicleRepair, List<VehicleRepairDataItem>>
        {
            public KobodbContext _context { get; set; }
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<List<VehicleRepairDataItem>> Handle(GetListVehicleRepair request, CancellationToken cancellationToken)
            {
                var result = new List<VehicleRepairDataItem>();
                if (request == null || request.Model == null) throw new Exception($"Fail to execute {nameof(GetListVehicleRepair)}!!! Model is null");
                var proc = _context.LoadStoredProc("PK_dGetListVehicleRepairData_R")
                    .AddParam("@ProcessingDate", request.Model.ProcessingDate)
                    .AddParam("@EigyoCdFrom", request.Model.EigyoFrom)
                    .AddParam("@EigyoCdTo", request.Model.EigyoTo)
                    .AddParam("@CompanyCdSeq", request.Model.CompanyCdSeq);
                await proc.ExecAsync(async r => result = await r.ToListAsync<VehicleRepairDataItem>(cancellationToken));
                return result;
            }
        }
    }
}
