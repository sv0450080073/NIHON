using HassyaAllrightCloud.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Reports.DataSource
{
    public class UnkoushijishoReportDS
    {
        public List<OperatingInstructionReportPDF> _data { get; set; }
        public UnkoushijishoReportDS(List<OperatingInstructionReportPDF> data)
        {
            _data = data;
        }
    }
}
