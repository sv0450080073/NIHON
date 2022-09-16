using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class Unkobidatafile
    {
        public string UkeNo { get; set; }
        public short UnkRen { get; set; }
        public string HaiSYmd { get; set; }
        public string HaiSTime { get; set; }
        public string TouYmd { get; set; }
        public string TouChTime { get; set; }
        public string SyukoYmd { get; set; }
        public string SyuKoTime { get; set; }
        public string KikYmd { get; set; }
        public string KikTime { get; set; }
        public string Text => UnkRen.ToString("D2") +"："+DateTime.ParseExact(SyukoYmd, "yyyyMMdd", null).ToString("yyyy/MM/dd")+"～"+DateTime.ParseExact(KikYmd, "yyyyMMdd", null).ToString("yyyy/MM/dd");
    }
}
