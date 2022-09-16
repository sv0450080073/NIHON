using HassyaAllrightCloud.Domain.Dto;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Reports.DataSource
{
    public class AttendanceReportDataSource
    {
        public List<AttendanceReportPage> DataSource { get; set; }
        public AttendanceReportDataSource(List<AttendanceReportPage> dataSource)
        {
            DataSource = dataSource;
        }
    }
}
