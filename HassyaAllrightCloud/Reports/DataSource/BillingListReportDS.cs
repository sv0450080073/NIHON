using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.BillingList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Reports.DataSource
{
    public class BillingListReportDS
    {
        public List<BillingListReport> _data { get; set; }
        public BillingListReportDS(List<BillingListReport> data)
        {
            _data = data;
        }
    }
}
