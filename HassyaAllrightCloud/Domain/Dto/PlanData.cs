using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class PlanData
    {
        public int YoteiSeq { get; set; }
        public string RyakuNm { get; set; }
        public string TukiLabKbn { get; set; }
        public string Title { get; set; }
        public string YoteiSYmd { get; set; }
        public string YoteiSTime { get; set; }
        public string YoteiEYmd { get; set; }
        public string YoteiETime { get; set; }
        public byte AllDayKbn { get; set; }
        public string KuriRule { get; set; }
        public string KuriReg { get; set; }
        public byte GaiKkKbn { get; set; }
        public string SyainCdSeq { get; set; }
        public string SyainCd { get; set; }
        public string SyainNm { get; set; }
        public string EventParticipant { get; set; }
        public string YoteiBiko { get; set; }
    }
}
