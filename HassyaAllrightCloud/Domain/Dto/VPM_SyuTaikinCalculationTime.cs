using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class VPM_SyuTaikinCalculationTime
    {
        public int CompanyCdSeq { get; set; }
        public byte SyugyoKbn { get; set; }
        public byte KouZokPtnKbn { get; set; }
        public int SyukinCalculationTimeMinutes { get; set; }
        public int TaikinCalculationTimeMinutes { get; set; }
    }
}
