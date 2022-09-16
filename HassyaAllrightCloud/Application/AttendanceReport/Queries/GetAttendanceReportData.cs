using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.AttendanceReport.Queries
{
    public class GetAttendanceReportData : IRequest<IEnumerable<AttendanceReportSPModel>>
    {
        public AttendanceReportModel Model { get; set; }
        public class Handler : IRequestHandler<GetAttendanceReportData, IEnumerable<AttendanceReportSPModel>>
        {
            private readonly KobodbContext _kobodbContext;
            public Handler(KobodbContext kobodbContext)
            {
                _kobodbContext = kobodbContext;
            }

            public async Task<IEnumerable<AttendanceReportSPModel>> Handle(GetAttendanceReportData request, CancellationToken cancellationToken)
            {
                var result = new List<AttendanceReportSPModel>();
               

                return result;
            }
        }
    }
}
