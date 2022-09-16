using HassyaAllrightCloud.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Reports.DataSource
{
    public class BusTypeListReportGroupDS
    {
        public List<BusTypeListReportGroupPDF> _data { get; set; }
        public BusTypeListReportGroupDS(List<BusTypeListReportGroupPDF> data)
        {
            _data = data;
        }
    }
}
