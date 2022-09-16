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
    public class AttendanceReportCommonDataItem
    {
        public int SyainSyainCdSeq { get; set; }
        public string SyainSyainCd { get; set; }
        public string SyainSyainNm { get; set; }
        public byte SyokumSyokumuKbn { get; set; }
        public int EigyosEigyoCdSeq { get; set; }
        public int EigyosEigyoCd { get; set; }
        public string EigyosEigyoNm { get; set; }
        public string EigyosRyakuNm { get; set; }
    }
    public class GetCommonData : IRequest<List<AttendanceReportCommonDataItem>>
    {
        public AttendanceReportModel Model { get; set; }
        public class Handler : IRequestHandler<GetCommonData, List<AttendanceReportCommonDataItem>>
        {
            public KobodbContext _context { get; set; }
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<List<AttendanceReportCommonDataItem>> Handle(GetCommonData request, CancellationToken cancellationToken)
            {
                var result = new List<AttendanceReportCommonDataItem>();
                if (request == null || request.Model == null) throw new Exception($"Fail to execute {nameof(GetCommonData)}!!! Model is null");
                var proc = _context.LoadStoredProc("PK_dGetAttendanceReportCommonData_R")
                    .AddParam("@ProcessingDate", request.Model.ProcessingDate)
                    .AddParam("@EigyoCdFrom", request.Model.EigyoFrom)
                    .AddParam("@EigyoCdTo", request.Model.EigyoTo)
                    .AddParam("@CompanyCdSeq", request.Model.CompanyCdSeq);
                await proc.ExecAsync(async r => result = await r.ToListAsync<AttendanceReportCommonDataItem>(cancellationToken));
                return result;
            }
        }
    }
}
