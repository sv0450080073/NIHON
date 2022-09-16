using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class Driverlst
    {
        public int index { get; set; }
        public int HaiInRen { get; set; }
        public int SyainCdSeq { get; set; }
        public string DriverName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string StartComment { get; set; }
        public string EndComment { get; set; }
        public string StartTimestr { get; set; }
        public string EndTimestr { get; set; }
        public int SiyoKbn { get; set; }
        public int Syokum_SyokumuCdSeq { get; set; }
        public int CompanyCdSeq { get; set; }
        public byte SyokumuKbn { get; set; }
    }
}
