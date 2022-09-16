using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class ETCFormModel
    {
        public ETCSyaRyo SyaRyo { get; set; }
        public DateTime? UnkYmd { get; set; }
        public string UnkTime { get; set; }
        public ETCFutai Futai { get; set; }
        public RyokinDataItem IriRyaku { get; set; }
        public RyokinDataItem DeRyaku { get; set; }
        public ETCSeisan Seisan { get; set; }
        public byte SeisanWay { get; set; }
        public short Suryo { get; set; }
        public int Tanka { get; set; }
        public int UriGakKin { get; set; }
        public string ZeiKbnNm { get; set; }
        public decimal ZeiRitu { get; set; }
        public int SyaRyoSyo { get; set; }
        public decimal TesuRitu { get; set; }
        public int SyaRyoTes { get; set; }
        public string BikoNm { get; set; }
        public string UkeNo { get; set; }
        public string DanNm { get; set; }
        public ETCYoyakuData ETCYoyaku { get; set; }
    }

    public class RyokinDataItem
    {
        public string RyokinTikuCd { get; set; }
        public string RyokinCd { get; set; }
        public string RyokinNm { get; set; }
        public string DisplayName
        {
            get
            {
                return $"{RyokinTikuCd?.Trim().PadLeft(2, '0')} - {RyokinCd?.Trim().PadLeft(3, '0')} : {RyokinNm}";
            }
        }
    }

    public enum ETCFormTypeEnum
    {
        Create,
        Update,
        View
    }
}
