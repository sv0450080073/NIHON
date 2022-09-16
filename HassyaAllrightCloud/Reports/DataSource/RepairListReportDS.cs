using HassyaAllrightCloud.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Reports.DataSource
{
    public class RepairListReportDS
    {
        public List<RepairListReportPDF> _data { get; set; }
        public RepairListReportDS(List<RepairListReportPDF> data)
        {
            _data = data;
        }
    }
}
