using HassyaAllrightCloud.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace HassyaAllrightCloud.Reports.DataSource
{
    public class DepositListReportDS
    {
        public List<DepositsListReportDataModel> _data { get; set; }
        public DepositListReportDS(List<DepositsListReportDataModel> data)
        {
            _data = data;
        }
    }
}
