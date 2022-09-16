using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class StaffDayOff
    {
        public int Kikyuj_KinKyuTblCdSeq { get; set; }
        public int Kikyuj_SyainCdSeq { get; set; }
        public string Kikyuj_KinKyuSYmd { get; set; }
        public string Kikyuj_KinKyuSTime { get; set; }
        public string Kikyuj_KinKyuEYmd { get; set; }
        public string Kikyuj_KinKyuETime { get; set; }
        public int Kikyuj_KinKyuCdSeq { get; set; }
        public int KinKyu_KinKyuCdSeq { get; set; }
        public string KinKyu_KinKyuNm { get; set; }
        public byte KinKyu_KinKyuKbn { get; set; }
        public string KinKyu_ColKinKyu { get; set; }
        public int Eigyos_EigyoCdSeq { get; set; }
        public int Compny_CompanyCdSeq { get; set; }
        public string FuriYmd { get; set; }
    }
}
