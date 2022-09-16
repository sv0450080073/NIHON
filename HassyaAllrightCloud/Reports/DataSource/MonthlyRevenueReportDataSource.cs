using HassyaAllrightCloud.Domain.Dto;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Reports.DataSource
{
    public class MonthlyRevenueReportDataSource
    {
        public List<MonthlyRevenueReportData> DataSource { get; set; }
        public MonthlyRevenueReportDataSource(List<MonthlyRevenueReportData> dataSource)
        {
            DataSource = dataSource;
        }
    }
}
