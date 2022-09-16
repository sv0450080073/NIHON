using System;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class JobData
    {
        public int JobID { get; set; }
        public string JobName { get; set; }
        public int StaffID { get; set; }
        public bool IsDoing { get; set; }
        public string CCSStyle { get; set; }
        public string ColorLine { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Top { get; set; }
        public double Left { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int TimeStart { get; set; }
        public int TimeEnd { get; set; }
        public double TimeStartString => double.Parse(StartDate + TimeStart.ToString("D4"));
        public double TimeEndString => double.Parse(EndDate + TimeEnd.ToString("D4"));
    }
}
