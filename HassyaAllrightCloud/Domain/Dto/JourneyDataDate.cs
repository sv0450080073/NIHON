using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class JourneyDataDate
    {
        public DateTime date { get; set; }
        public bool isZenHaFlg { get; set; } = false;
        public bool isKhakFlg { get; set; } = false;
        public bool isCommom { get; set; } = true;
        public Byte tomKbn { get; set; } = 1;
        public string ukeNo { get; set; }
        public short teiDanNo { get; set; }
        public short bunkRen { get; set; }
        public short unkRen { get; set; }
        public short nittei { get; set; }
        public TimeSpan form { get; set; }
        public TimeSpan to { get; set; }
        public TimeSpan SyukoTime { get; set; }
        public TimeSpan HaiSTime { get; set; }
        public TimeSpan KikTime { get; set; }
        public string timeStartZenHaFlg { get; set; } = "";
        public string timeEndKhakFlg { get; set; } = "";
        public string Textbun => isZenHaFlg ? $"前泊" : isKhakFlg ? $"後泊" : "通常";
        public string Text => Textbun + $"{nittei}日目：{date.ToString("yyyy/MM/dd (ddd)")}";
    }
}
