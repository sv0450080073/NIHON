using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class DailyBatchCopySearchModel
    {
        public byte Vehicle { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public bool IsProcess { get; set; } = true;
        public bool IsArrangement { get; set; } = true;
        public bool IsLoadedGoods { get; set; } = false;
        public bool IsIncidental { get; set; } = false;
        public bool IsMonday { get; set; }
        public bool IsTuesday { get; set; }
        public bool IsWebnesday { get; set; }
        public bool IsThursday { get; set; }
        public bool IsFriday { get; set; }
        public bool IsSaturday { get; set; }
        public bool IsSunday { get; set; }
        public DateTime RepeatEnd { get; set; } = DateTime.Now;
        public bool IsDayOfWeek { get; set; }
    }

    public class DailyBatchCopyData
    {
        public string UkeNo { get; set; }
        public string HaiSYmd { get; set; }
        public string TouYmd { get; set; }
        public string TokisakiNm { get; set; }
        public string TokisitenNm { get; set; }
        public string DanTaNm { get; set; }
        public string IkNm { get; set; }
        public int DriverNum { get; set; }
        public int GuiderNum { get; set; }
        public int Unchin { get; set; }
        public int Ryokin { get; set; }
        public int SyaSyuTan { get; set; }
        public int SyaSyuDai { get; set; }
        public int SyaRyoUnc { get; set; }
        public int UnitGuiderPrice { get; set; }
        public int UnitGuiderFee { get; set; }
    }
}
