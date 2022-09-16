using HassyaAllrightCloud.Domain.Dto;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Reports.DataSource
{
    public class VenderRequestReportDataSource
    {
        public List<VenderRequestReportData> _data { get; set; }
        public VenderRequestReportDataSource(List<VenderRequestReportData> data)
        {
            _data = data;
        }
    }
}
