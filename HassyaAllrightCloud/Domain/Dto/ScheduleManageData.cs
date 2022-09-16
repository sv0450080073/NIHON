using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class ScheduleManageData
    {
        public int YoteiSeq { get; set; }
        public string SyainCd { get; set; }
        public string SyainNm { get; set; }
        public string EigyoNm { get; set; }
        public string YoteiTypeNm { get; set; }
        public string KinkyuNm { get; set; }
        public string TukiLabKbn { get; set; }
        public string Title { get; set; }
        public byte YoteiShoKbn { get; set; }
        public string ShoSyainCd { get; set; }
        public string ShoSyainNm { get; set; }
        public string YoteiSYmd { get; set; }
        public string YoteiSTime { get; set; }
        public string YoteiEYmd { get; set; }
        public string YoteiETime { get; set; }
        public string ShoUpdYmd { get; set; }
        public string ShoUpdTime { get; set; }
        public byte AllDayKbn { get; set; }
    }
}
