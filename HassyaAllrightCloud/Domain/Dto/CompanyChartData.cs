using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class CompanyChartData
    {
        public int CompanyCdSeq { get; set; }
        public int CompanyCd { get; set; }
        public string CompanyNm { get; set; }
        public string RyakuNm { get; set; }
        public int EigyoCdSeq { get; set; }
        public string Text => CompanyCdSeq == 0 ? string.Empty: $"{CompanyCd.ToString("D4")}：{RyakuNm}";
        public string TextReport => CompanyCdSeq == 0 ? "すべて" : $"{CompanyCd.ToString("D4")}：{RyakuNm}";
    }
}
