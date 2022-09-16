using HassyaAllrightCloud.Domain.Dto;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Reports.DataSource
{
    public class CancelListReportDataSource
    {
        public CancelListReportDataSource(List<CancelListReportPagedData> data)
        {
            _data = data;
        }

        public List<CancelListReportPagedData> _data { get; set; }
    }
}
