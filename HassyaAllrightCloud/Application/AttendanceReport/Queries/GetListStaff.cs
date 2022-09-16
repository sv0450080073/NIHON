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
    public class StaffDataItem
    {
        public int SyainCdSeq { get; set; }
    }
    public class GetListStaff : IRequest<List<StaffDataItem>>
    {
        public AttendanceReportModel Model { get; set; }
        public class Handler : IRequestHandler<GetListStaff, List<StaffDataItem>>
        {
            public KobodbContext _context { get; set; }
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<List<StaffDataItem>> Handle(GetListStaff request, CancellationToken cancellationToken)
            {
                var result = new List<StaffDataItem>();
                if (request == null || request.Model == null) throw new Exception($"Fail to execute {nameof(GetListStaff)}!!! Model is null");
                var proc = _context.LoadStoredProc("PK_dGetListStaff_R")
                    .AddParam("@ProcessingDate", request.Model.ProcessingDate)
                    .AddParam("@EigyoCdFrom", request.Model.EigyoFrom)
                    .AddParam("@EigyoCdTo", request.Model.EigyoTo)
                    .AddParam("@YoyaKbnSortFrom", request.Model.RegistrationTypeSortFrom)
                    .AddParam("@YoyaKbnSortTo", request.Model.RegistrationTypeSortTo)
                    .AddParam("@CompanyCdSeq", request.Model.CompanyCdSeq)
                    .AddParam("@TenantCdSeq", request.Model.TenantId);
                await proc.ExecAsync(async r => result = await r.ToListAsync<StaffDataItem>(cancellationToken));
                return result;
            }
        }
    }
}
