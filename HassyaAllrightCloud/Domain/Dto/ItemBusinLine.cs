using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class ItemBusinLine
    {
        public string lineID { get; set; }
        public double left { get; set; }
        public double width { get; set; }
        public double minleft { get; set; }
        public double maxwidth { get; set; }
    }

    public class UpYmdTime
    {
        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
        public string UkeNo { get; set; }
    }

    public class ParamHaiTaCheck
    {
        public string UkeNo { get; set; }
        public short UnkRen { get; set; }
        public short TeiDanNo { get; set; }
        public short BunkRen { get; set; }
        public int SyaSyuRen { get; set; }
    }

    public class ResponseHaiTaCheck
    {
        public List<UpYmdTime> UpYmdTimes { get; set; }
        public short UnkRen { get; set; }
        public short TeiDanNo { get; set; }
        public short BunkRen { get; set; }
        public string UkeNo { get; set; }
    }

    public class DataInit
    {
        public string UkeNo { get; set; }
        public List<ResponseHaiTaCheck> ResponseHaiTaCheckInit { get; set; }
        public bool IsValid { get; set; }
        public ActionBusSchedule ActionBusSchedule { get; set; }
    }
    public enum ActionBusSchedule
    {
        Move,
        Cut,
        Merge,
        Delete,
        SimpleDispatch,
        Undo,
        None
    }
}
