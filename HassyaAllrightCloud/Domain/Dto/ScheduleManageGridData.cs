using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class ScheduleManageGridData
    {
        public int No { get; set; }
        public string SyainCd { get; set; }
        public string SyainNm { get; set; }
        public string EigyoNm { get; set; }
        public string YoteiTypeNm { get; set; }
        public string KinkyuNm { get; set; }
        public string TukiLabKbn { get; set; }
        public string Title { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string YoteiShoKbn { get; set; }
        public string ShoSyainCd { get; set; }
        public string ShoSyainNm { get; set; }
        public string UpdateTime { get; set; }
        public int ScheduleId { get; set; }
        public byte AllDayKbn { get; set; }
    }
}
