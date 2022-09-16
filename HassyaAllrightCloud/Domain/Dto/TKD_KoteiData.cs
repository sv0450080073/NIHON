using System.Collections.Generic;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class TKD_KoteiData
    {
        public string Kotei_UkeNo { get; set; }
        public short Kotei_UnkRen { get; set; }
        public short Kotei_TeiDanNo { get; set; }
        public short Kotei_BunkRen { get; set; }
        public short Kotei_TomKbn { get; set; }
        public short Kotei_Nittei { get; set; }
        public short Kotei_KouRen { get; set; }
        public string Kotei_Koutei { get; set; }
        public string Koteik_SyuPaTime { get; set; }
        public string Koteik_TouChTime { get; set; }
        public string Koteik_SyukoTime { get; set; }
        public string Koteik_HaiSTime { get; set; }
        public string Koteik_KikTime { get; set; }
        public decimal Koteik_JisaIPKm { get; set; }
        public decimal Koteik_JisaKSKm { get; set; }
        public decimal Koteik_KisoIPkm { get; set; }
        public decimal Koteik_KisoKOKm { get; set; }
    }

    public class LatestUpdYmdTime
    {
        public long MaxKoteikYmdTime { get; set; }
        public long MaxKoteiYmdTime { get; set; }
    }
}
