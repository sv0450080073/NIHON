using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class ReceivableFilterModel : Paging
    {
        public CompanyData CompanyData { get; set; }
        public LoadSaleBranchList StartSaleBranchList { get; set; }
        public LoadSaleBranchList EndSaleBranchList { get; set; }
        public SaleOfficeModel SaleOfficeType { get; set; }
        public DateTime? StartPaymentDate { get; set; }
        public DateTime? EndPaymentDate { get; set; }       
        public string StartReceiptNumber { get; set; }
        public string EndReceiptNumber { get; set; }
        public CustomerComponentGyosyaData startCustomerComponentGyosyaData { get; set; }
        public CustomerComponentTokiskData startCustomerComponentTokiskData { get; set; }
        public CustomerComponentTokiStData startCustomerComponentTokiStData { get; set; }
        public CustomerComponentGyosyaData endCustomerComponentGyosyaData { get; set; }
        public CustomerComponentTokiskData endCustomerComponentTokiskData { get; set; }
        public CustomerComponentTokiStData endCustomerComponentTokiStData { get; set; }
        public ReservationClassComponentData StartReservationClassification { get; set; }
        public ReservationClassComponentData EndReservationClassification { get; set; }
        public string BillingType { get; set; }
        public ComboboxFixField UnpaidSelection { get; set; } = ReceivableUnpaidList.ReceivableUnpaids[0];
        public DateTime? PaymentDate { get; set; }
        public string GridDisplayType { get; set; }
        public SelectedSaleBranchModel SelectedSaleBranchPayment { get; set; }
        public SelectedPaymentAddressModel SelectedBillingAddressPayment { get; set; }
        public byte FareBilTyp { get; set; } = 2;
        public byte FutaiBilTyp { get; set; } = 2;
        public byte TollFeeBilTyp { get; set; } = 2;
        public byte ArrangementFeeBilTyp { get; set; } = 2;
        public byte GuideFeeBilTyp { get; set; } = 2;
        public byte LoadedItemBilTyp { get; set; } = 2;
        public byte CancelFeeBilTyp { get; set; } = 2;
        public OutputReportType OutputType { get; set; } = OutputReportType.Preview;
        public ComboboxFixField ReportPageSize { get; set; } = BillTypePagePrintList.BillTypePagePrintData[0];
        public ComboboxFixField ActiveHeaderOption { get; set; } = ShowHeaderOptions.ShowHeaderOptionData[0];
        public ComboboxFixField GroupType { get; set; } = GroupTypes.GroupTypeData[0];
        public ComboboxFixField DelimiterType { get; set; } = DelimiterTypes.DelimiterTypeData[2];
        public int PageNumBP { get; set; }
        public int PageSizeBP { get; set; }
        public int ReceivableReport { get; set; }
    }
}
