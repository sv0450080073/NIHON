using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class HaiinDataModel
    {
        public string UkeNo { get; set; }
        public int UkeCd { get; set; }
        public int UnkRen { get; set; }
        public int TeiDanNo { get; set; }
        public int BunkRen { get; set; }
        public byte HaiInRen { get; set; }
        public string GoSya { get; set; }
        public string SyaSyuNm { get; set; }
        public byte TeiCnt { get; set; }
        public string Title { get; set; }
        public string SyuKoYmd { get; set; }
        public string SyuKoTime { get; set; }
        public string KikYmd { get; set; }
        public string KikTime { get; set; }
        public int SyainCdSeq { get; set; }
        public string SyainCd { get; set; }
        public string SyainNm { get; set; }
        public string HaiSNm { get; set; }
        public string HaiSTime { get; set; }
        public string TouNm { get; set; }
        public string TouChTime { get; set; }
        public string DanTaNm { get; set; }
        public string IkNm { get; set; }
        public byte SchReadKbn { get; set; }
    }
}
