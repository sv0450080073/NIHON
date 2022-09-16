using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;

namespace HassyaAllrightCloud.Reports.DataSource
{
    public class ReceivableListReportDS
    {
        public List<ReceivableListReportDataModel> _data { get; set; }
        public ReceivableListReportDS(List<ReceivableListReportDataModel> data)
        {
            _data = data;
        }
    }
}
