using HassyaAllrightCloud.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Reports.DataSource
{
    public class TransportDailyReportDS
    {
        public List<TransportDailyReportPDF> _data { get; set; }
        public TransportDailyReportDS(List<TransportDailyReportPDF> data)
        {
            _data = data;
        }
    }
}
