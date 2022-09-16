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
    public class VehicleDataItem
    {
        public int SyaRyoCdSeq { get; set; }
        public int SyaRyoSyaRyoCd { get; set; }
        public string SyaRyoSyaRyoNm { get; set; }
        public int SyaRyoSyaSyuCdSeq { get; set; }
        public short SyaSyuSyaSyuCd { get; set; }
        public string SyaSyuSyaSyuNm { get; set; }
        public int EigyosEigyoCd { get; set; }
        public string EigyosEigyoNm { get; set; }
        public string EigyosRyakuNm { get; set; }
    }
    public class GetListVehicle: IRequest<List<VehicleDataItem>>
    {
        public AttendanceReportModel Model { get; set; }
        public class Handler : IRequestHandler<GetListVehicle, List<VehicleDataItem>>
        {
            public KobodbContext _context { get; set; }
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<List<VehicleDataItem>> Handle(GetListVehicle request, CancellationToken cancellationToken)
            {
                var result = new List<VehicleDataItem>();
                if (request == null || request.Model == null) throw new Exception($"Fail to execute {nameof(GetListVehicle)}!!! Model is null");
                var proc = _context.LoadStoredProc("PK_dGetVehicleData_R")
                    .AddParam("@ProcessingDate", request.Model.ProcessingDate)
                    .AddParam("@EigyoCdFrom", request.Model.EigyoFrom)
                    .AddParam("@EigyoCdTo", request.Model.EigyoTo)
                    .AddParam("@CompanyCdSeq", request.Model.CompanyCdSeq)
                    .AddParam("@TenantCdSeq", request.Model.CurrentTenantId);
                await proc.ExecAsync(async r => result = await r.ToListAsync<VehicleDataItem>(cancellationToken));
                return result;
            }
        }
    }
}
