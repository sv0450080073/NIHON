using HassyaAllrightCloud.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Reports.DataSource
{
    public class BillCheckListReportDS
    {
        public List<BillCheckListReportPDF> _data { get; set; }
        public BillCheckListReportDS(List<BillCheckListReportPDF> data)
        {
            _data = data;
        }
    }
}
