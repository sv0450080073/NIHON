using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class JourneyData
    {
        public string Title { get; set; }
        public string SyuKoYmd { get; set; }
        public string SyuKoTime { get; set; }
        public string KikYmd { get; set; }
        public string KikTime { get; set; }
        public string SyainCdSeq { get; set; }
        public string SyainNm { get; set; }
        public string HaiSNm { get; set; }
        public string HaiSTime { get; set; }
        public string TouNm { get; set; }
        public string TouChTime { get; set; }
        public string DanTaNm { get; set; }
        public string IkNm { get; set; }
    }
}
