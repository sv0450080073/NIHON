using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto.SubContractorStatus
{
    public class SubContractorStatusReportPagedData
    {
        public bool IsNormalPagging { get; set; }

        public string DateType { get; set; }
        public string DateInfo { get; set; }
        public string CustomerInfo { get; set; }
        public string UkeCdInfo { get; set; }
        public string BranchInfo { get; set; }
        public string OutputInfo { get; set; }
        public string StaffInfo { get; set; }

        public List<SubContractorStatusReportData> ReportDatas { get; set; } = new List<SubContractorStatusReportData>();

        public string PrintedStaffCD { get; set; }
        public string PrintedStaffName { get; set; }
    }
}
