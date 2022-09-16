using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class WorkHourModel
    {
        public int SyainCdSeq { get; set; }
        public string UnkYmd { get; set; }
        public string SyukinTime { get; set; }
        public string TaiknTime { get; set; }
        public string KousTime { get; set; }
        public DateTime StartWorkHour { get; set; }
        public DateTime EndWorkHour { get; set; }
    }
}
