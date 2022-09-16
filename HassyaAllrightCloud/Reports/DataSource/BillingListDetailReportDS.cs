using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.BillingList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Reports.DataSource
{
    public class BillingListDetailReportDS
    {
        public List<BillingListDetailReport> _data { get; set; }
        public BillingListDetailReportDS(List<BillingListDetailReport> data)
        {
            _data = data;
        }
    }
}
