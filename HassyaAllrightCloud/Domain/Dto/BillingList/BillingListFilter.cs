using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto.BillingList
{
    public class BillingListFilter
    {
        public string Code { get; set; }
        public DateTime? BillDate { get; set; } = DateTime.Now;
        public byte? CloseDate { get; set; }
        public BillOfficeData BillOffice { get; set; }
        public long? StartReceiptNumber { get; set; }
        public long? EndReceiptNumber { get; set; }
        // 予約区分
        public List<int> BillTypes { get; set; } = new List<int>();

        public ReservationClassComponentData StartReservationClassification { get; set; }
        public ReservationClassComponentData EndReservationClassification { get; set; }
        public InvoiceType StartBillClassification { get; set; }
        public InvoiceType EndBillClassification { get; set; }
        public ComboboxFixField BillIssuedClassification { get; set; } = new ComboboxFixField();
        public ComboboxFixField BillTypeOrder { get; set; } = new ComboboxFixField();
        public CustomerComponentGyosyaData startCustomerComponentGyosyaData { get; set; }
        public CustomerComponentTokiskData startCustomerComponentTokiskData { get; set; }
        public CustomerComponentTokiStData startCustomerComponentTokiStData { get; set; }
        public CustomerComponentGyosyaData endCustomerComponentGyosyaData { get; set; }
        public CustomerComponentTokiskData endCustomerComponentTokiskData { get; set; }
        public CustomerComponentTokiStData endCustomerComponentTokiStData { get; set; }
        public TransferAmountOutputClassification TransferAmountOutputClassification { get; set; }
        public bool itemFare { get; set; }
        public bool itemIncidental { get; set; }
        public bool itemTollFee { get; set; }
        public bool itemArrangementFee { get; set; }
        public bool itemGuideFee { get; set; }
        public bool itemLoaded { get; set; }
        public bool itemCancellationCharge { get; set; }
        public bool isListMode { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; } = Common.LimitPage;
        public int ActiveV { get; set; }
        public List<int> lstActiveTypeTotal { get; set; } = new List<int> { 1, 2, 3 };

        // item for report button
        public OutputReportType OutputType { get; set; } = OutputReportType.Preview;
        public ComboboxFixField PageSize { get; set; } = BillTypePagePrintList.BillTypePagePrintData[0];
        public ComboboxFixField ActiveHeaderOption { get; set; } = ShowHeaderOptions.ShowHeaderOptionData[0];
        public ComboboxFixField GroupType { get; set; } = GroupTypes.GroupTypeData[0];
        public ComboboxFixField DelimiterType { get; set; } = DelimiterTypes.DelimiterTypeData[2];
    }

    public class TransferAmountOutputClassification
    {
        public string Text => $"{Name}";
        public int Code { get; set; }
        public string Name { get; set; }
    }

    public class BillingListCode
    {
        public short GyosyaCd { get; set; }
        public short TokuiCd { get; set; }
        public short SitenCd { get; set; }
        public string TokuiNm { get; set; }
        public string Code => $"{GyosyaCd.ToString("D3")}-{TokuiCd.ToString("D4")}-{SitenCd.ToString("D4")} : {TokuiNm}";
    }

    public class BillingListFilterParam
    {
        public string BillDate { get; set; }
        public byte CloseDate { get; set; }
        public int TenantCdSeq { get; set; }
        public string StartBillAddress { get; set; }
        public string EndBillAddress { get; set; }
        public int BillOffice { get; set; }
        public int StartReservationClassification { get; set; }
        public int EndReservationClassification { get; set; }
        public string StartReceiptNumber { get; set; }
        public string EndReceiptNumber { get; set; }
        public long StartBillClassification { get; set; }
        public long EndBillClassification { get; set; }
        public string BillTypes { get; set; }
        public byte BillIssuedClassification { get; set; }
        public string BillTypeOrder { get; set; }
        public short GyosyaCd { get; set; }
        public short TokuiCd { get; set; }
        public short SitenCd { get; set; }
        public string TokuiNm { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; } = Common.LimitPage;
    }
}
