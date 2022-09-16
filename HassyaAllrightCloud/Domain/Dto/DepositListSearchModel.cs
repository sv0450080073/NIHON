using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class DepositListSearchModel : Paging
    {
        public CompanyData CompanyData { get; set; }
        public CustomerComponentGyosyaData startCustomerComponentGyosyaData { get; set; }
        public CustomerComponentTokiskData startCustomerComponentTokiskData { get; set; }
        public CustomerComponentTokiStData startCustomerComponentTokiStData { get; set; }
        public CustomerComponentGyosyaData endCustomerComponentGyosyaData { get; set; }
        public CustomerComponentTokiskData endCustomerComponentTokiskData { get; set; }
        public CustomerComponentTokiStData endCustomerComponentTokiStData { get; set; }
        public ReservationClassComponentData StartReservationClassification { get; set; }
        public ReservationClassComponentData EndReservationClassification { get; set; }
        public LoadSaleBranchList StartSaleBranchList { get; set; }
        public LoadSaleBranchList EndSaleBranchList { get; set; }
        public SaleOfficeModel SaleOfficeType { get; set; }
        public DateTime? StartPaymentDate { get; set; }
        public DateTime? EndPaymentDate { get; set; }
        public string StartReceiptNumber { get; set; }
        public string EndReceiptNumber { get; set; }
        public string BillingType { get; set; }
        public TransferBankModel StartTransferBank { get; set; }
        public TransferBankModel EndTransferBank { get; set; }
        public string PaymentMethod { get; set; }
        public bool IsUseReport { get; set; }
        public bool IsUseCSV { get; set; }
        public DepositOutputClass DepositOutputTemplate { get; set; }
        public SelectedSaleBranchModel SelectedSaleBranchPayment { get; set; }
        public SelectedPaymentAddressModel SelectedBillingAddressPayment { get; set;}
        public byte FareBilTyp { get; set; } = 2;
        public byte FutaiBilTyp { get; set; } = 2;
        public byte TollFeeBilTyp { get; set; } = 2;
        public byte ArrangementFeeBilTyp { get; set; } = 2;
        public byte GuideFeeBilTyp { get; set; } = 2;
        public byte LoadedItemBilTyp { get; set; } = 2;
        public byte CancelFeeBilTyp { get; set; } = 2;
        public OutputReportType OutputType { get; set; } = OutputReportType.Preview;
        public ComboboxFixField PageSizeReport { get; set; } = BillTypePagePrintList.BillTypePagePrintData[0];
        public ComboboxFixField ActiveHeaderOption { get; set; } = ShowHeaderOptions.ShowHeaderOptionData[0];
        public ComboboxFixField GroupType { get; set; } = GroupTypes.GroupTypeData[0];
        public ComboboxFixField DelimiterType { get; set; } = DelimiterTypes.DelimiterTypeData[2];
        public bool IsGetAll { get; set; }
    }

    public class DepositListSummary
    {
        public int PageAmount { get; set; }
        public int PageTransferFee { get; set; }
        public int PageCumulativePayment { get; set; }
        public int PagePreviousReceiveAmount { get; set; }
        public int TotalAmount { get; set; }
        public int TotalTransferFee { get; set; }
        public int TotalCumulativePayment { get; set; }
        public int TotalPreviousReceiveAmount { get; set; }
    }
}
