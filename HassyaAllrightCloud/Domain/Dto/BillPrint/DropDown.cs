using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto.BillPrint
{
    public class DropDown
    {
        public string CodeText { get; set; }
        public string Text => $"{CodeText}";
        public string Code { get; set; }
        public string Name { get; set; }
        public int SeiCdSeq { get; set; }
        public int SeiSitCdSeq { get; set; }
        public byte CodeNumber { get; set; }
    }

    public class CheckBoxFilter
    {
        public string Id { get; set; }
        public bool IsChecked { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public bool IsBillingType { get; set; } = false;
    }
}
