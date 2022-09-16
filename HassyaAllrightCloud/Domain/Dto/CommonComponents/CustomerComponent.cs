using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto.CommonComponents
{
    public class CustomerComponentGyosyaData
    {
        public int? GyosyaCdSeq { get; set; }
        public byte GyosyaKbn { get; set; }
        public short GyosyaCd { get; set; }
        public string GyosyaNm { get; set; }
        public string Text => $"{GyosyaCd:000} : {GyosyaNm}";
        public bool IsSelect { get; set; }
    }

    public class CustomerComponentTokiskData
    {
        public int TokuiSeq { get; set; }
        public int GyosyaCdSeq { get; set; }
        public short TokuiCd { get; set; }
        public string RyakuNm { get; set; }
        public string TokuiNm { get; set; }
        public string Text => $"{TokuiCd:0000} : {RyakuNm}";
        public bool IsSelect { get; set; }
    }

    public class CustomerComponentTokiStData
    {
        public int SitenCdSeq { get; set; }
        public int TokuiSeq { get; set; }
        public short SitenCd { get; set; }
        public string RyakuNm { get; set; }
        //booking input
        public decimal TesuRituGui { get; set; }
        public byte SimeD { get; set; }
        public byte TesKbn { get; set; }
        public string Text => $"{SitenCd:0000} : {RyakuNm}";

        // Additional properties
        public string TelNo { get; set; }
        public string TokuiTanNm { get; set; }
        public string FaxNo { get; set; }
        public string TokuiMail { get; set; }
        public decimal TesuRitu { get; set; }
        public string SitenNm { get; set; }
        public bool IsSelect { get; set; }
    }
}
