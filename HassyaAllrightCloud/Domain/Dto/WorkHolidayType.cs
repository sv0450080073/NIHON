using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class WorkHolidayType
    {
        public byte JisKinKyuCd { get; set; } = 0;
        public int KinKyuCdSeq { get; set; }
        public short KinKyuCd { get; set; }
        public string KinKyuNm { get; set; }
        public string CodeKbnNm { get; set; }
    }
}
