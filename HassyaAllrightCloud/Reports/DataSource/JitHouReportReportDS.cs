using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.IService;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Reports.DataSource
{
    public class JitHouReportReportDS
    {
        public List<JitHouReports> _data { get; set; }
        public JitHouReportReportDS(List<JitHouReports> data)
        {
            _data = data;
        }
    }
}
