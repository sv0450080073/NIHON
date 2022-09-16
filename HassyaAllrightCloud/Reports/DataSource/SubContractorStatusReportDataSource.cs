using HassyaAllrightCloud.Domain.Dto.SubContractorStatus;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Reports.DataSource
{
    public class SubContractorStatusReportDataSource
    {
        public SubContractorStatusReportDataSource(List<SubContractorStatusReportPagedData> data)
        {
            _data = data;
        }

        public List<SubContractorStatusReportPagedData> _data { get; set; }
    }
}
