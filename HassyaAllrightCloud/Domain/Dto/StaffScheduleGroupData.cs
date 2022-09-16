using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class StaffScheduleGroupData
    {
        public string EmployeeId { get; set; }
        public string id { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string Description { get; set; }
        public string EmployeeName { get; set; }
        public string ScheduleType { get; set; }
        public string LeaveType { get; set; }
        public string color { get; set; }
        public string Creator { get; set; }
        public string Participant { get; set; }
        public string PlanComment { get; set; }
        public string Destination { get; set; }
        public string IsPublic { get; set; }
        public string scheduleId { get; set; }
        public string recurrenceRule { get; set; }
        public string label { get; set; }
        public string plantype { get; set; }
        public string LeaveName { get; set; }
        public bool isAllDay { get; set; }
    }
}
