using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using System;

namespace HassyaAllrightCloud.Domain.Dto.InvoiceIssueRelease
{
    public class InvoiceIssueFilter
    {
        public int? BillOutputSeq { get; set; }
        public int? BillSerialNumber { get; set; }
        public DateTime? StartBillIssuedDate { get; set; }
        public DateTime? EndBillIssuedDate { get; set; }
        //public LoadCustomerList StartBillAddress { get; set; }
        //public LoadCustomerList EndBillAddress { get; set; }

        public CustomerComponentGyosyaData SelectedGyosyaFrom { get; set; }
        public CustomerComponentTokiskData SelectedTokiskFrom { get; set; }
        public CustomerComponentTokiStData SelectedTokiStFrom { get; set; }
        public CustomerComponentGyosyaData SelectedGyosyaTo { get; set; }
        public CustomerComponentTokiskData SelectedTokiskTo { get; set; }
        public CustomerComponentTokiStData SelectedTokiStTo { get; set; }
        //for filter
        public string StartBillIssuedDateString { get; set; }
        public string EndBillIssuedDateString { get; set; }
        public string StartBillAddressString { get; set; }
        public string EndBillAddressString { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
        public int ActiveV { get; set; }
    }
}
