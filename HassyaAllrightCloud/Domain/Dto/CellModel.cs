using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class CellModel
    {
        public int Row { get; set; }
        public int Cell { get; set; }
        public int SyainCdSeq { get; set; }
        public string Date { get; set; }
        public DateTime DateTime { get; set; }
    }
}
