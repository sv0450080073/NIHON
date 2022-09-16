using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class BusinessPlanReceivableListReportDataModel
    {
        public DateTime OutputDate { get; set; }
        public string Page { get; set; }
        public string BillingOffice { get; set; }
        public string BillingCode { get; set; }
        public string BillingPeriod { get; set; }
        public string UnpaidDesignation { get; set; }
        public string SpecifyPayment { get; set; }
        public string ReservationClassification { get; set; }
        public string CompanyCode { get; set; }
        public string OfficeCode { get; set; }
        public List<BussinesPlanReceivableGridDataModel> bussinesPlanReceivableGridDatas { get; set; }
        public string EmployeeCodeOfOutputPerson { get; set; }
        public string OutputPersonEmployeeName { get; set; }
    }
}
