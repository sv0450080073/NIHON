using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class VpmKinKyuData
    {
        public int KinKyuCdSeq { get; set; }
        public short KinKyuCd { get; set; }
        public string KinKyuNm { get; set; }
        public string RyakuNm { get; set; }
        public byte KinKyuKbn { get; set; }
        public string ColKinKyu { get; set; }
        public string KyuSyukinNm { get; set; }
        public string KyuSyukinRyaku { get; set; }
        public string DefaultSyukinTime { get; set; }
        public string DefaultTaiknTime { get; set; }
        public byte KyusyutsuKbn { get; set; }
        public string Text => $"{KinKyuCd}：{KinKyuNm}"; 
    }
}
