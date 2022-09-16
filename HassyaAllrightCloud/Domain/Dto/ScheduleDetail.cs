using HassyaAllrightCloud.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class ScheduleDetail
    {
        public int YoteiSeq { get; set; }
        public string SyainCd { get; set; }
        public string SyainNm { get; set; }
        public string EigyoNm { get; set; }
        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
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
        public string ShoRejBiko { get; set; }
        public string ToteiBiko { get; set; }
        public string YoteiBiko { get; set; }
        public Status ApprovalStatus { get; set; }
        public int SyainCdSeq { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
