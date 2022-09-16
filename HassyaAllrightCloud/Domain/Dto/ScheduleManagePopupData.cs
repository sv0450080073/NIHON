using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class ScheduleManagePopupData
    {
        public Staffs Staff { get; set; }
        public Branch Branch { get; set; }
        public string UpdateTime { get; set; }
        public string ScheduleType { get; set; }
        public string EventType { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public Staffs ScheduleStaff { get; set; }
        public string DateStartEvent { get; set; }
        public string TimeStartEvent { get; set; }
    }
}
