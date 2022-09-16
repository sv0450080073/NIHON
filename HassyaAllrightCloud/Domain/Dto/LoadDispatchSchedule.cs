using System;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class LoadDispatchSchedule
    {
        public int StaffCdSeq { get; set; }
        public DateTime StockOut { get; set; }
        public DateTime Arrival { get; set; }
        public string Destination { get; set; }
    }
}
