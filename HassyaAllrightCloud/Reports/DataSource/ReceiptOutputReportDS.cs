using HassyaAllrightCloud.Domain.Dto;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Reports.DataSource
{
    public class ReceiptOutputReportDS
    {
        public List<ReceiptOutputReport> _data { get; set; }
        public ReceiptOutputReportDS(List<ReceiptOutputReport> data)
        {
            _data = data;
        }
    }
}
