using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class ReceivableListReportDataModel
    {
        public DateTime OutputDate { get; set; }
        public string Page { get; set; }
        public string BillingOffice { get; set; }
        public string BillingCode { get; set; }
        public string BillingPeriod { get; set; }
        public string BillingCodeRange { get; set; }
        public string SalesOfficeCodeRange { get; set; }
        public string UnpaidDesignation { get; set; }
        public string SpecifyPayment { get; set; }
        public string ReservationClassification { get; set; }
        public List<ReservationListDetaiGridDataModel> reservationListDetaiGridDatas { get; set; }
        public string EmployeeCodeOfOutputPerson { get; set; }
        public string OutputPersonEmployeeName { get; set; }
    }
}
