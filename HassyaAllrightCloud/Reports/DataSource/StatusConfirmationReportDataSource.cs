using HassyaAllrightCloud.Domain.Dto;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Reports.DataSource
{
    public class StatusConfirmationReportDataSource
    {
        public List<StatusConfirmationReportPaged> _data { get; set; }
        public StatusConfirmationReportDataSource(List<StatusConfirmationReportPaged> data)
        {
            _data = data;
        }
    }
}
