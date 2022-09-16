using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto.SubContractorStatus
{
    public class SubContractorStatusReportSearchParams
    {
        public int TenantId { get; set; }
        public int UserLoginId { get; set; }
        public SubContractorStatusData SearchCondition { get; set; }
    }
}
