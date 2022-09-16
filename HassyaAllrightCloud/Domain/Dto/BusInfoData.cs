using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class BusInfoData
    {
        public int EigyoCdSeq { get; set; }
        public int EigyoCd { get; set; }
        public string RyakuNm { get; set; } = "";
        public int SyaRyoCdSeq { get; set; }
        public int SyaRyoCd { get; set; }
        public string SyaRyoNm { get; set; } = "";
        public int NinkaKbn { get; set; }
        public string KariSyaRyoNm { get; set; } = "";
        public int SyaSyuCdSeq { get; set; }
        public int SyaSyuCd { get; set; }
        public string EigyoNm { get; set; } = "";
        public string SyaSyuNm { get; set; } = "";
        public int CompanyCdSeq { get; set; }
        public int CompanyCd { get; set; }
        public string CompanyNm { get; set; } = "";
        public byte KataKbn { get; set; }
        public string TenkoNo { get; set; } = "";
        public string StaYmd { get; set; } = "";
        public string EndYmd { get; set; } = "";
        public int id { get; set; }
        public int TeiCnt { get; set; }
        public string Text => $"{SyaRyoNm}　{SyaSyuNm}　{EigyoNm}";
        public string Textforcbb =>  EigyoCd == 0 ? string.Empty:$"{EigyoCd.ToString("D5")}：{RyakuNm}　{SyaRyoCd.ToString("D5")}：{SyaRyoNm}";
        public string LockYmd { get; set; }
        public int SYARYO_SyainCdSeq { get; set; }
    }
}
