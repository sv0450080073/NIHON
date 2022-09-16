using System;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class ReceiptOutputFormSeachModel
    {
        public BillOfficeReceipt BillOffice { get; set; }
        public BillAddressReceiptFromTo BillAddressFrom { get; set; }
        public BillAddressReceiptFromTo BillAddressTo { get; set; }
        public BillAddressReceipt BillAddressReceipt { get; set; }
        public DateTime? StaInvoicingDate { get; set; }
        public DateTime? EndInvoicingDate { get; set; }
        public DateTime? InvoiceYearMonth { get; set; }
        public string StaInvoiceOutNum { get; set; }
        public string StaInvoiceSerNum { get; set; }
        public string EndInvoiceOutNum { get; set; }
        public string EndInvoiceSerNum { get; set; }
        public DateTime? IssueDate { get; set; } = DateTime.Now;
        public string SeiOutSeqSeiRen { get; set; }
        public int ActiveV { get; set; }
        public CustomerModel CustomerModelFrom { get; set; }
        public CustomerModel CustomerModelTo { get; set; }
    }
    public class BillOfficeReceipt
    {
        public int EigyoCdSeq { get; set; }
        public int EigyoCd { get; set; }
        public int CompanyCd { get; set; }
        public string RyakuNm { get; set; }
        public string Text => $"{EigyoCd.ToString("D5")} : {RyakuNm}";
    }
    public class BillAddressReceiptFromTo
    {
        public int GyosyaCd { get; set; }
        public int TokuiCd { get; set; }
        public int TokuiCdSeq { get; set; }
        public int SitenCd { get; set; }
        public int SitenCdSeq { get; set; }
        public string RyakuNm { get; set; }
        public string SitenNm { get; set; }
        public string Text => (TokuiCd > 0 && SitenCd > 0) ? $"{string.Format("{0:D4}", TokuiCd)}：{RyakuNm}　{string.Format("{0:D4}", SitenCd)}：{SitenNm}" : "";
        public string Code => $"{GyosyaCd.ToString("D3")}{TokuiCd.ToString("D4")}{SitenCd.ToString("D4")}";
    }
    public class BillAddressReceipt
    {
        public short GyosyaCd { get; set; }
        public short TokuiCd { get; set; }
        public short SitenCd { get; set; }
        public string TokuiNm { get; set; }
        public string SitenNm { get; set; }
        public string GyosyaNm { get; set; }
        public int TokuiSeq { get; set; }
        public short SitenCdSeq { get; set; }
        public string RyakuNm { get; set; }

        public string Text => $"{string.Format("{0:D3}", GyosyaCd)}-{string.Format("{0:D4}", TokuiCd)}-{string.Format("{0:D4}", SitenCd)}：" +
                              $"{GyosyaNm}-{TokuiNm}-{SitenNm}";
        public string Code => $"{GyosyaCd}{TokuiCd}{SitenCd}";
    }
    public class ReceiptOuputCommonItems
    {
        public List<BillOfficeReceipt> BillOfficeReceipts { get; set; }
        public List<BillAddressReceiptFromTo> BillAddressReceiptFromTos { get; set; }
        public List<BillAddressReceipt> BillAddressReceipts { get; set; }
    }
    public class Invoice
    {
        public int ListNo { get; set; }
        public string ListInvoiceNo { get; set; }
        public string ListBillingOffice { get; set; }
        public string ListBillingAddress { get; set; }
        public string ListInvoiceYearMonth { get; set; }
        public string PreviousCarryAmount { get; set; }
        public string ThisAmount { get; set; }
        public string ThisTaxAmount { get; set; }
        public string ThisFeeAmount { get; set; }
        public string ThisDeposit { get; set; }
        public string ThisBillingAmount { get; set; }
        public string InvoiceDate { get; set; }
        public bool Checked { get; set; }
        public int SeiOutSeq { get; set; }
        public short SeiRen { get; set; }
    }
    public class InvoicesListResult
    {
        public int SeiOutSeq { get; set; }
        public short SeiRen { get; set; }
        public string SeikYm { get; set; }
        public int ZenKurG { get; set; }
        public int KonUriG { get; set; }
        public int KonSyoG { get; set; }
        public int KonTesG { get; set; }
        public int KonNyuG { get; set; }
        public int KonSeiG { get; set; }
        public string TokuiNm { get; set; }
        public string SitenNm { get; set; }
        public string SeiEigEigyoNm { get; set; }
        public string TokuiTanNm { get; set; }
        public string SeiEigCompanyNm { get; set; }
        public string SeiHatYmd { get; set; }
    }
    public class ReceiptDetailReport
    {
        public string RyoHatYmd { get; set; }
        public string RyoOutSeq { get; set; }
        public string RyoRen { get; set; }
        public string ZipCd { get; set; }
        public string Jyus1 { get; set; }
        public string Jyus2 { get; set; }
        public string TokuiNm { get; set; }
        public string SitenNm { get; set; }
        public string SeiEigZipCd { get; set; }
        public string SeiEigJyus1 { get; set; }
        public string SeiEigJyus2 { get; set; }
        public string SeiEigEigyoNm { get; set; }
        public string SeiEigCompanyNm { get; set; }
        public string SeiEigTelNo { get; set; }
        public string SeiEigFaxNo { get; set; }
        public string KonSeiG { get; set; }
        public string HasYmd { get; set; }
        public string BreakdownDetails { get; set; }
        public string ReducedTaxRateClassification { get; set; }
        public string SyaSyuNm { get; set; }
        public string Suryo { get; set; }
        public string TanKa { get; set; }
        public string UriGakKin { get; set; }
        public string SyaRyoSyo { get; set; }
        public string SeiKin { get; set; }
        public string NyuKinRui { get; set; }
        public string BikoNm { get; set; }
        public string SalesPageTotal { get; set; }
        public string ConsumptionTaxPageTotal { get; set; }
        public string SubtotalPageTotal { get; set; }
        public string TargetTotal10 { get; set; }
        public string TargetTotal8 { get; set; }
        public string ConsumptionTax10 { get; set; }
        public string ConsumptionTax8 { get; set; }
        public string KonSyoG { get; set; }
    }
}
