using HassyaAllrightCloud.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Reports.DataSource
{
    public class SimpleQuotationReportDataSource
    {
        public SimpleQuotationReportDataSource(List<SimpleQuotationDataReport> data)
        {
            _data = data;
        }
        public List<SimpleQuotationDataReport> _data { get; set; }

    }


}
