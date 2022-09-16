using HassyaAllrightCloud.Domain.Dto;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Reports.DataSource
{
    public class DailyRevenueReportDataSource
    {
        public List<DailyRevenueReportData> DataSource { get; set; }
        public DailyRevenueReportDataSource(List<DailyRevenueReportData> dataSource)
        {
            DataSource = dataSource;
        }
    }
}
