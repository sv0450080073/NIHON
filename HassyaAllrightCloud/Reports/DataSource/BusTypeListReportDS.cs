using HassyaAllrightCloud.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Reports.DataSource
{
    public class BusTypeListReportDS
    {

        public List<BusTypeListReportPDF> _data { get; set; }
        public BusTypeListReportDS(List<BusTypeListReportPDF> data)
        {
            _data = data;
        }
    }
}
