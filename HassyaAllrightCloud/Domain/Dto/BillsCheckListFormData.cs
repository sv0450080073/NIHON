using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class BillsCheckListFormData
    {
        public DateTime? BillPeriodFrom { get; set; }
        public DateTime? BillPeriodTo { get; set; }
        public BillOfficeData BillOffice { get; set; }
        // 請求先
        public CustomerComponentGyosyaData GyosyaTokuiSakiFrom { get; set; }
        public CustomerComponentGyosyaData GyosyaTokuiSakiTo { get; set; }
        public CustomerComponentTokiskData TokiskTokuiSakiFrom { get; set; }
        public CustomerComponentTokiskData TokiskTokuiSakiTo { get; set; }
        public CustomerComponentTokiStData TokiStTokuiSakiFrom { get; set; }
        public CustomerComponentTokiStData TokiStTokuiSakiTo { get; set; }

        public BillAddress StartBillAddress { get; set; } // Get from TPM_Tokisk AND TPM_TokiSt
        public BillAddress EndBillAddress { get; set; }
        // 受付番号
        public string StartReceiptNumber { get; set; }
        public string EndReceiptNumber { get; set; }
        // 予約区分
        // New
        public ReservationClassComponentData YoyakuFrom { get; set; }
        public ReservationClassComponentData YoyakuTo { get; set; }
        public ReservationData StartReservationClassification { get; set; }
        public ReservationData EndReservationClassification { get; set; }
        //
        public InvoiceType StartBillClassification { get; set; }
        public InvoiceType EndBillClassification { get; set; }
        //
        public ComboboxFixField BillIssuedClassification { get; set; } = new ComboboxFixField();
        public ComboboxFixField BillTypeOrder { get; set; } = new ComboboxFixField();
        public BillAddress BillAdress { get; set; }
        public bool itemFare { get; set; }
        public bool itemIncidental { get; set; }
        public bool itemTollFee { get; set; }
        public bool itemArrangementFee { get; set; }
        public bool itemGuideFee { get; set; }
        public bool itemShippingCharge { get; set; }
        public bool itemCancellationCharge { get; set; }
        // item for report button
        public OutputReportType OutputType { get; set; } = OutputReportType.Preview;
        public ComboboxFixField PageSize { get; set; } = BillTypePagePrintList.BillTypePagePrintData[0];
        public ComboboxFixField ActiveHeaderOption { get; set; } = ShowHeaderOptions.ShowHeaderOptionData[0];
        public ComboboxFixField GroupType { get; set; } = GroupTypes.GroupTypeData[0];
        public ComboboxFixField DelimiterType { get; set; } = DelimiterTypes.DelimiterTypeData[2];
        // item for save button status
        public int ActiveV { get; set; }
        public List<int> lstActiveTypeTotal { get; set; } = new List<int> { 1, 2, 3 };
        public OutputReportType typePrint { get; set; }
    }
}
