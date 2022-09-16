using HassyaAllrightCloud.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Reports.DataSource
{
    public class AdvancePaymentDetailsReportDS
    {
        public List<AdvancePaymentDetailsReportPDF> _data { get; set; }
        public AdvancePaymentDetailsReportDS(List<AdvancePaymentDetailsReportPDF> data)
        {
            _data = data;
        }
    }
}
