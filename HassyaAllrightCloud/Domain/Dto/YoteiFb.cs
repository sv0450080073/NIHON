using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class YoteiFb
    {
        public int YoteiSeq { get; set; }
        public byte KuriKbn { get; set; }
        public string YoteiSYmd { get; set; }
        public string YoteiSTime { get; set; }
        public string EventParticipant { get; set; }
    }
}
