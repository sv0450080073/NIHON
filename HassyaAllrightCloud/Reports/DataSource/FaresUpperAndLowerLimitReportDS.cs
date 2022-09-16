using HassyaAllrightCloud.Domain.Dto;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Reports.DataSource
{
    public class FaresUpperAndLowerLimitReportDS
    {
        public FaresUpperAndLowerLimitReport _data { get; set; }
        public FaresUpperAndLowerLimitReportDS(FaresUpperAndLowerLimitReport data)
        {
            _data = data;
        }
    }
}
