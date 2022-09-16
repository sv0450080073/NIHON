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
    public class HolidayDataItem
    {
        public int SyainCdSeq { get; set; }
        public int KinKyuCdSeq { get; set; }
        public short KinKyuCdKinKyuCd { get; set; }
        public string KinKyuCdKinKyuNm { get; set; }
        public string KinKyuCdRyakuNm { get; set; }
        public byte KinKyuCdKinKyuKbn { get; set; }
    }
    public class GetHolidayData : IRequest<List<HolidayDataItem>>
    {
        public AttendanceReportModel Model { get; set; }
        public class Handler : IRequestHandler<GetHolidayData, List<HolidayDataItem>>
        {
            public KobodbContext _context { get; set; }
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<List<HolidayDataItem>> Handle(GetHolidayData request, CancellationToken cancellationToken)
            {
                var result = new List<HolidayDataItem>();
                if (request == null || request.Model == null) throw new Exception($"Fail to execute {nameof(GetHolidayData)}!!! Model is null");
                var proc = _context.LoadStoredProc("PK_dGetHolidayData_R")
                    .AddParam("@ProcessingDate", request.Model.ProcessingDate)
                    .AddParam("@EigyoCdFrom", request.Model.EigyoFrom)
                    .AddParam("@EigyoCdTo", request.Model.EigyoTo)
                    .AddParam("@CompanyCdSeq", request.Model.CompanyCdSeq);
                await proc.ExecAsync(async r => result = await r.ToListAsync<HolidayDataItem>(cancellationToken));
                return result;
            }
        }
    }
}
