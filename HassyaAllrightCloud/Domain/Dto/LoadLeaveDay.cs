using System;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class LoadLeaveDay
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Type { get; set; }
        public string TypeName { get; set; }
        public string EmployeeName { get; set; }
        public bool IsLeave { get; set; }
        public string Remark { get; set; }
    }
}
