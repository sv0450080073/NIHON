using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class KinkyuDataModel
    {
        public int KinKyuTblCdSeq { get; set; }
        public string TukiLabel { get; set; }
        public string Title { get; set; }
        public string KinKyuSYmd { get; set; }
        public string KinKyuSTime { get; set; }
        public string KinKyuEYmd { get; set; }
        public string KinKyuETime { get; set; }
        public string BikoNm { get; set; }
        public int SyainCdSeq { get; set; }
        public string SyainCd { get; set; }
        public string SyainNm { get; set; }
        public byte KinKyuKbn { get; set; }
        public string KinKyuKbnNm { get; set; }
        public string KinKyuNm { get; set; }
        public int KinKyuCdSeq { get; set; }
        public byte SchReadKbn { get; set; }
    }
}
