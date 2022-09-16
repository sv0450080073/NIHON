using System;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class LockBookingData
    {
        public LockBookingData()
        {
            ProcessingDate = DateTime.Today;
        }

        public DateTime ProcessingDate { get; set; }
        public DepartureOfficeData SalesOffice { get; set; }
    }

    public class LockBookingDetailData
    {
        public string SalesOfficeCode { get; set; }
        public string SalesOfficeName { get; set; }
        public DateTime LockDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public DateTime LastUpdatedTime { get; set; }
        public string LastUpdatedPerson { get; set; }
    }

}
