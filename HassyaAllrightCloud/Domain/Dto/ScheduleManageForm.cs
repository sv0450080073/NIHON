using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class ScheduleManageForm : Paging
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Status ApprovalStatus { get; set; }
        public Staffs Staff { get; set; }
        public Branch Branch { get; set; }
        public CustomGroup CustomGroup { get; set; }

    }
}
