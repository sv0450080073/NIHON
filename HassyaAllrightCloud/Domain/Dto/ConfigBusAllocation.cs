using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class ConfigBusAllocation
    {
        public int DateType { get; set; }
        public int ReservationType { get; set; }
        public int SortOrder { get; set; }
        public int Size { get; set; }
    }
}
