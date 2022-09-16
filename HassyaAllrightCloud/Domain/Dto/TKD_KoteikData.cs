using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class TKD_KoteikData
    {
        public bool isZenHaFlg { get; set; } = false;
        public bool isKhakFlg { get; set; } = false;
        public bool isCommom { get; set; } = true;
        public string UkeNo { get; set; }
        public short UnkRen { get; set; }
        public short TeiDanNo { get; set; }
        public short BunkRen { get; set; }
        public short TomKbn { get; set; }
        public short Nittei { get; set; }
        public string SyuPaTime { get; set; }
        public string TouChTime { get; set; }
        public string SyukoTime { get; set; }
        public string HaiSTime { get; set; }
        public string KikTime { get; set; }
        public decimal JisaIPKm { get; set; }
        public decimal JisaKSKm { get; set; }
        public decimal KisoIPkm { get; set; }
        public decimal KisoKOKm { get; set; }
        public List<TKD_KoteiDataInsert> JourneyLst { get; set; }
    }
}
