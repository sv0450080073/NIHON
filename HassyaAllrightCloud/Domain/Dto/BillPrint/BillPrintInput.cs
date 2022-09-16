using HassyaAllrightCloud.Domain.Dto.BillPrint;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class BillPrintInput
    {
        public int? InvoiceOutNum { get; set; }
        public int? InvoiceSerNum { get; set; }
        public DateTime? InvoiceYm { get; set; } = null;
        public long? StartRcpNum { get; set; }
        public long? EndRcpNum { get; set; }
        public int StartRsrCat { get; set; }
        public ReservationClassComponentData StartRsrCatDropDown { get; set; }
        public int EndRsrCat { get; set; }
        public ReservationClassComponentData EndRsrCatDropDown { get; set; }
        public int BillingOffice { get; set; }
        public DropDown BillingOfficeDropDown { get; set; }
        public string BillingAddress { get; set; }
        public DropDown BillingAddressDropDown { get; set; }
        public long StartBillAdd { get; set; }
        public int StaSeiCdSeq { get; set; }
        public int StaSeiSitCdSeq { get; set; }
        public CustomerComponentGyosyaData startCustomerComponentGyosyaData { get; set; }
        public CustomerComponentTokiskData startCustomerComponentTokiskData { get; set; }
        public CustomerComponentTokiStData startCustomerComponentTokiStData { get; set; }
        public CustomerComponentGyosyaData endCustomerComponentGyosyaData { get; set; }
        public CustomerComponentTokiskData endCustomerComponentTokiskData { get; set; }
        public CustomerComponentTokiStData endCustomerComponentTokiStData { get; set; }
        public long EndBillAdd { get; set; }
        public int EndSeiCdSeq { get; set; }
        public int EndSeiSitCdSeq { get; set; }
        public int? ClosingDate { get; set; }
        public DateTime IssueYmd { get; set; }
        public string HandlingCharPrt { get; set; }
        public DropDown HandlingCharPrtDropDown { get; set; }
        public string BillingType { get; set; }
        public string ZipCode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string CustomerNm { get; set; }
        public string CustomerBrchNm { get; set; }
        public int PrintMode { get; set; }
        public byte FareBilTyp { get; set; } = 2;
        public byte FutaiBilTyp { get; set; } = 2;
        public byte TollFeeBilTyp { get; set; } = 2;
        public byte ArrangementFeeBilTyp { get; set; } = 2;
        public byte GuideFeeBilTyp { get; set; } = 2;
        public byte LoadedItemBilTyp { get; set; } = 2;
        public byte CancelFeeBilTyp { get; set; } = 2;
        public List<OutDataTable> OutDataTables { get; set; }
        public bool IsPreview { get; set; }
        public List<CheckBoxFilter> checkBoxFilters { get; set; } = new List<CheckBoxFilter>();
    }

    public class PaymentRequestInputData
    {
        public string SeikYm { get; set; }
        public string SeiHatYmd { get; set; }
        public string SeiOutTime { get; set; }
        public int InTanCdSeq { get; set; }
        public byte SeiOutSyKbn { get; set; }
        public byte SeiGenFlg { get; set; }
        public byte RStaUkeNo { get; set; }
        public byte REndUkeNo { get; set; }
        public string StaUkeNo { get; set; }
        public string EndUkeNo { get; set; }
        public int StaYoyaKbn { get; set; }
        public int EndYoyaKbn { get; set; }
        public int SeiEigCdSeq { get; set; }
        public byte SeiSitKbn { get; set; }
        public int StaSeiCdSeq { get; set; }
        public int StaSeiSitCdSeq { get; set; }
        public int EndSeiCdSeq { get; set; }
        public int EndSeiSitCdSeq { get; set; }
        public byte SimeD { get; set; }
        public byte PrnCpys { get; set; }
        public byte PrnCpysTan { get; set; }
        public byte SeiFutUncKbn { get; set; }
        public byte SeiFutFutKbn { get; set; }
        public byte SeiFutTukKbn { get; set; }
        public byte SeiFutTehKbn { get; set; }
        public byte SeiFutGuiKbn { get; set; }
        public byte SeiFutTumKbn { get; set; }
        public byte SeiFutCanKbn { get; set; }
        public string ZipCd { get; set; }
        public string Jyus1 { get; set; }
        public string Jyus2 { get; set; }
        public string TokuiNm { get; set; }
        public string SitenNm { get; set; }
        public byte SiyoKbn { get; set; }
        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
        public int UpdSyainCd { get; set; }
        public string UpdPrgID { get; set; }
        public byte KuriSyoriKbn { get; set; }
        public int TenantCdSeq { get; set; }
        public string StartBillAdd { get; set; }
        public string EndBillAdd { get; set; }
        public int InvoiceOutNum { get; set; }
        public short InvoiceSerNum { get; set; }
        public string BillingType { get; set; }
        public string OutDataTable { get; set; }
        public byte TesPrnKbn { get; set; }
    }
}
