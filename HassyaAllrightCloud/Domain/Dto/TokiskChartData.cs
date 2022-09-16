using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class TokiskChartData
    {
        public int Tokisk_TokuiSeq { get; set; }
        public int Tokisk_TokuiCd { get; set; }
        public string Tokisk_TokuiNm { get; set; }
        public string Tokisk_RyakuNm { get; set; }
        public int TokiSt_SitenCdSeq { get; set; }
        public int TokiSt_SitenCd { get; set; }
        public string TokiSt_RyakuNm { get; set; }
        public string TokiSt_TelNo { get; set; }
        public string TokiSt_FaxNo { get; set; }
        public string TokiSt_TokuiTanNm { get; set; }
        public string TokiSt_TokuiMail { get; set; }
        public decimal TokiSt_TesuRitu { get; set; }
    }
}
