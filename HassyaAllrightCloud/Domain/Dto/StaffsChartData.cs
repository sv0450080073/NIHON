using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class StaffsChartData
    {
        public class SearchData
        {
            public string DepartureDateStart { get; set; }
            public string DepartureTimeStart { get; set; } = "0000";
            public string DepartureDateEnd { get; set; }
            public string DepartureTimeEnd { get; set; } = "2359";
            public string ArrivalDateStart { get; set; }
            public string ArrivalTimeStart { get; set; }= "0000";
            public string ArrivalDateEnd { get; set; }
            public string ArrivalTimeEnd { get; set; } = "2359";
            public string DeliveryDateStart { get; set; }
            public string DeliveryTimeStart { get; set; }= "0000";
            public string DeliveryDateEnd { get; set; }
            public string DeliveryTimeEnd { get; set; } = "2359";
            public string ReturnDateStart { get; set; }
            public string ReturnTimeStart { get; set; }= "0000";
            public string ReturnDateEnd { get; set; }
            public string ReturnTimeEnd { get; set; }= "2359";
        }
    }
}
