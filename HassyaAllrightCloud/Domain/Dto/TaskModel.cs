using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class TaskModel
    {
        public short SyokumuCd { get; set; }
        public string SyokumuNm { get; set; }
        public string Text => SyokumuCd == -1 ? "" : $"{SyokumuCd.ToString("D2")} : {SyokumuNm}";
    }
}
