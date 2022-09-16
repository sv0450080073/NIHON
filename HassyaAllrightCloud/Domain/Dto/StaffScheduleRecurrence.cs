using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class StaffScheduleRecurrence
    {
        public int targetStartDate { get; set; }
        public int targetEndDate { get; set; }
        public StaffScheduleData staffData { get; set; }
        public ScheduleDataModel staffGroupData { get; set; }
    }
}
