using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class CheckHaiinData
    {
        public string Haisha_UkeNo { get; set; }
        public short Haisha_UnkRen { get; set; }
        public short Haisha_TeiDanNo { get; set; }
        public short Haisha_BunkRen { get; set; }
        public int Haisha_DrvJin { get; set; }
        public int Haisha_GuiSu { get; set; }
        public int Haiin_CountDriver { get; set; }
        public int Haiin_CountGuide { get; set; }
    }
}
