using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class LeaveData
    {
        public string KinKyuTblCdSeq { get; set; }
        public string TukiLabel { get; set; }
        public string Title { get; set; }
        public string KinKyuSYmd { get; set; }
        public string KinKyuSTime { get; set; }
        public string KinKyuEYmd { get; set; }
        public string KinKyuETime { get; set; }
        public string BikoNm { get; set; }
        public string SyainCd { get; set; }
        public string SyainNm { get; set; }
        public string CodeKbnNm { get; set; }
        public string KinKyuNm { get; set; }
        public byte? KinKyuKbn { get; set; }
    }
}
