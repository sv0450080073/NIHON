using System;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class ConfigBusSchedule
    {
        public int ActiveS1 { get; set; }
        public int ActiveS2 { get; set; }
        public int ActiveG { get; set; }
        public int ActiveV { get; set; }
        public int ActiveL { get; set; }
        public int ActiveP { get; set; }
        public int ActiveCPT { get; set; }
        public int ActiveS3 { get; set; }
        public int ActiveR { get; set; }
        public int Number_of_days { get; set; }
        public int Mode { get; set; }
        public bool IsDivided { get; set; } = false;
        public List<List<DateTime>> ListDate { get; set; } = new List<List<DateTime>>();
        public int TotalDays { get; set; }
    }
}
