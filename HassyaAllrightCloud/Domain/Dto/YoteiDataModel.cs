using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class YoteiDataModel
    {
        public int YoteiSeq { get; set; }
        public int YoteiType { get; set; }
        public string RyakuNm { get; set; }
        public int KinKyuCdSeq { get; set; }
        public int CalendarSeq { get; set; }
        public int SyainCdSeq { get; set; }
        public string SyainCd { get; set; }
        public string SyainNm { get; set; }
        public int KinKyuTblCdSeq { get; set; }
        public string Title { get; set; }
        public string YoteiSYmd { get; set; }
        public string YoteiSTime { get; set; }
        public string YoteiEYmd { get; set; }
        public string YoteiETime { get; set; }
        public string TukiLabKbn { get; set; }
        public byte AllDayKbn { get; set; }
        public byte KuriKbn { get; set; }
        public string KuriRule { get; set; }
        public string KuriReg { get; set; }
        public byte GaiKkKbn { get; set; }
        public string KuriEndYmd { get; set; }
        public string YoteiBiko { get; set; }
        public byte YoteiShoKbn { get; set; }
        public int ShoSyainCdSeq { get; set; }
        public string ShoSyainCd { get; set; }
        public string ShoSyainNm { get; set; }
        public string ShoUpdYmd { get; set; }
        public string ShoUpdTime { get; set; }
        public string ShoRejBiko { get; set; }
        public string YotKSya { get; set; }
    }
}
