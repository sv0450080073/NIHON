using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class BusinessPlanReceivableCSVDataModel
    {
        public string EigyoCd { get; set; }
        public string EigyoNm { get; set; }
        public string EigyoRyak { get; set; }
        public string SeiGyosyaCd { get; set; }
        public string SeiCd { get; set; }
        public string SeiSitenCd { get; set; }
        public string SeiGyosyaCdNm { get; set; }
        public string SeiCdNm { get; set; }
        public string SeiSitenCdNm { get; set; }
        public string SeiRyakuNm { get; set; }
        public string SeiSitRyakuNm { get; set; }
        public long UnUriGakKin { get; set; }
        public long UnSyaRyoSyo { get; set; }
        public long UnSyaRyoTes { get; set; }
        public long UnNyukinG { get; set; }
        public long UnMisyuG { get; set; }
        public long GaUriGakKin { get; set; }
        public long GaSyaRyoSyo { get; set; }
        public long GaSyaRyoTes { get; set; }
        public long GaNyukinG { get; set; }
        public long GaMisyuG { get; set; }
        public long SoUriGakKin { get; set; }
        public long SoSyaRyoSyo { get; set; }
        public long SoSyaRyoTes { get; set; }
        public long SoNyukinG { get; set; }
        public long SoMisyuG { get; set; }
        public long CaUriGakKin { get; set; }
        public long CaSyaRyoSyo { get; set; }
        public long CaNyukinG { get; set; }
        public long CaMisyuG { get; set; }
        public long MisyuG { get; set; }
    }
}
