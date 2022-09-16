using System.Collections.Generic;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class BusscheduleData
    {
        public LoadCustomerList Customerlst { get; set; }
        public List<ItemBus> Itembus { get; set; }
        public int Loginuser { get; set; }
        public int BranchID { get; set; }
        public int CompanyID { get; set; }
        public string BookingName { get; set; }
        public int BusdriverNum { get; set; }
        public int BusGuideNum { get; set; }
    }
}
