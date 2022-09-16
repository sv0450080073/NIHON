using HassyaAllrightCloud.Domain.Dto;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Reports.DataSource
{
    public class AccessoryFeeListReportDataSource
    {
        public AccessoryFeeListReportDataSource(List<AccessoryFeeListReportPagedData> data)
        {
            _data = data;
        }

        public List<AccessoryFeeListReportPagedData> _data { get; set; }
    }
}
