using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class BranchChartData
    {
        public int EigyoCdSeq { get; set; }
        public int EigyoCd { get; set; }
        public string RyakuNm { get; set; } = "";
        public string Com_RyakuNm { get; set; } = "";
        public int CompanyCdSeq { get; set; }
        public string CompanyNm { get; set; } = "";
        public int CompanyCd { get; set; }
        public string Text => EigyoCdSeq == 0 ? string.Empty:$"{Com_RyakuNm}－{RyakuNm}";
        public string IDText => EigyoCdSeq == 0 ? "すべて" : $"{CompanyCd.ToString("D4")}：{Com_RyakuNm}　{EigyoCd.ToString("D4")}：{RyakuNm}";
        public string TextNull => EigyoCdSeq==0 ? "  ": $"{EigyoCd.ToString("D4")}：{RyakuNm}";
         public string TextNullbus => EigyoCdSeq==0 ? "すべて": $"{EigyoCd.ToString("D4")}：{RyakuNm}";
        public string TextNullHaiSha => EigyoCdSeq == 0 ? string.Empty : $"{EigyoCd.ToString("D4")}：{RyakuNm}";
    }
}
