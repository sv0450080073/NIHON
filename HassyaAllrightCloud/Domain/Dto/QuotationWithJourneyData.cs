using System;
using System.Collections.Generic;
using HassyaAllrightCloud.Commons.Constants;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class QuotationWithJourneyData
    {
        public DateTime StartPickupDate { get; set; } = DateTime.Today;
        public DateTime EndPickupDate { get; set; } = DateTime.Today;
        public DateTime StartArrivalDate { get; set; } = DateTime.Today;
        public DateTime EndArrivalDate { get; set; } = DateTime.Today;
        public List<ReservationData> BookingTypes { get; set; }
        public int _ukeCdFrom { get; set; }
        public string UkeCdFrom
        {
            get
            {
                return _ukeCdFrom.ToString("D10");
            }
            set
            {
                int newValue;
                bool isValid = int.TryParse(value, out newValue);

                if (isValid)
                    _ukeCdFrom = newValue;
            }
        }
        public int _ukeCdTo { get; set; }
        public string UkeCdTo
        {
            get
            {
                return _ukeCdTo.ToString("D10");
            }
            set
            {
                int newValue;
                bool isValid = int.TryParse(value, out newValue);

                if (isValid)
                    _ukeCdTo = newValue;
            }
        }
        public LoadCustomerList CustomerStart { get; set; }
        public LoadCustomerList CustomerEnd { get; set; }
        public LoadSaleBranch BranchStart { get; set; }
        public LoadSaleBranch BranchEnd { get; set; }
        public bool Fare { get; set; } = false;
        public OutputOrientation OutputOrientation { get; set; }
    }
}
