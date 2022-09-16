using HassyaAllrightCloud.Commons.Constants;
using System;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class BusReportData
    {
        public DateTime OperationDate { get; set; }
        public List<CompanyChartData> CompanyChartData { get; set; }
        public DepartureOfficeData VehicleDispatchOffice1 { get; set; } = new DepartureOfficeData();
        public DepartureOfficeData VehicleDispatchOffice2 { get; set; } = new DepartureOfficeData();
        public List<ReservationData> ReservationList { get; set; } = new List<ReservationData>();
        public string Undelivered { get; set; }
        public string TemporaryCar { get; set; }
        public OutputOrderData OutputOrder { get; set; } = new OutputOrderData();
        public string SizeOfPaper { get; set; }
        public int TenantCdSeq { get; set; } = 0;
        public string SyainNm { get; set; } = "";
        public string SyainCd { get; set; } = "";
        public string DateTimeFooter { get; set; } = DateTime.Now.ToString("yyyy/MM/dd  HH:mm");
        public string Uri { get; set; } = "";
        public OutputInstruction OutputSetting { get; set; }
        public ReservationData BookingFrom { get; set; } = new ReservationData();
        public ReservationData BookingTo { get; set; } = new ReservationData();
    }
    public class BusReportDataUri
    {
        public string Uri { get; set; } = "";
    }
}
