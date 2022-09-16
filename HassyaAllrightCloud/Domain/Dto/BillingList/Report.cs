using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto.BillingList
{
    public class BillingListReport
    {
        public MainInfo mainInfo { get; set; }
        public List<BillingListGrid> billingListGrids { get; set; }
    }

    public class BillingListDetailReport
    {
        public MainInfo mainInfo { get; set; }
        public List<BillingListDetailGrid> billingListDetailGrids { get; set; }
    }

    public class MainInfo
    {
        public string OutputOrder { get; set; }
        public string BillOffice { get; set; }
        public string BillAddress { get; set; }
        public string BillDate { get; set; }
        public byte CloseDate { get; set; }
        public string BillAddressCode { get; set; }
        public string BillOfficeCode { get; set; }
        public string BillIssuedClassification { get; set; }
        public string TransferAmountOutputClassification { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string CurrentDate { get; set; }
    }

    public class BillingListDetailTableInfo
    {
        public string DetailBillDate { get; set; }
        public string DetailReceiptNumber { get; set; }
        public string DetailOffice { get; set; }
        public string DetailBillAddress { get; set; }
        public string GridBillAddressCode { get; set; }
        public string DetailGroupName { get; set; }
        public string DetailDestinationName { get; set; }
        public string DetailDispatchDate { get; set; }
        public string DetailArrivalDate { get; set; }
        public string DetailIssuedDate { get; set; }
        public string DetailBillIncidentTypeName { get; set; }
        public string DetailIncidentLoadingGoodsName { get; set; }
        public string DetailPaymentName { get; set; }
        public string DetailOccurrenceDate { get; set; }
        public string DetailQuantity { get; set; }
        public string DetailPrice { get; set; }
        public string DetailSalesAmount { get; set; }
        public string DetailTaxAmount { get; set; }
        public string DetailCommissionRate { get; set; }
        public string DetailCommissionAmount { get; set; }
        public string DetailBillAmount { get; set; }
        public string DetailDepositDate { get; set; }
        public string DetailDepositAmount { get; set; }
        public string DetailUnpaidAmount { get; set; }
        public string TotalPage { get; set; }
        public string TotalPageNumber { get; set; }
        public string TotalPageQuantity { get; set; }
        public string TotalPageDetailSalesAmount { get; set; }
        public string TotalPageDetailTaxAmount { get; set; }
        public string TotalPageDetailCommissionAmount { get; set; }
        public string TotalPageDetailBillAmount { get; set; }
        public string TotalPageDetailDepositAmount { get; set; }
        public string TotalPageDetailUnpaidAmount { get; set; }
        public string TotalAll { get; set; }
        public string TotalAllNumber { get; set; }
        public string TotalAllQuantity { get; set; }
        public string TotalAllDetailSalesAmount { get; set; }
        public string TotalAllDetailTaxAmount { get; set; }
        public string TotalAllDetailCommissionAmount { get; set; }
        public string TotalAllDetailBillAmount { get; set; }
        public string TotalAllDetailDepositAmount { get; set; }
        public string TotalAllDetailUnpaidAmount { get; set; }
    }

    public class BillingListTableInfo
    {
        public string ListBillAddress { get; set; }
        public string ListSaleAmount { get; set; }
        public string ListSaleTax { get; set; }
        public string ListSaleFeeAmount { get; set; }
        public string ListSaleBillAmount { get; set; }
        public string ListSaleDeposit { get; set; }
        public string ListSaleUnpaidAmount { get; set; }
        public string ListSaleAdvancePayment { get; set; }
        public string ListGuideAmount { get; set; }
        public string ListGuideTax { get; set; }
        public string ListGuideFeeAmount { get; set; }
        public string ListGuideBillAmount { get; set; }
        public string ListGuideDeposit { get; set; }
        public string ListGuideUnpaidAmount { get; set; }
        public string ListGuideAdvancePayment { get; set; }
        public string ListOtherAmount { get; set; }
        public string ListOtherTax { get; set; }
        public string ListOtherFeeAmount { get; set; }
        public string ListOtherBillAmount { get; set; }
        public string ListOtherDeposit { get; set; }
        public string ListOtherUnpaidAmount { get; set; }
        public string ListOtherAdvancePayment { get; set; }
        public string ListCancelAmount { get; set; }
        public string ListCancelTax { get; set; }
        public string ListCancelBillAmount { get; set; }
        public string ListCancelDeposit { get; set; }
        public string ListCancelUnpaidAmount { get; set; }
        public string ListCancelAdvancePayment { get; set; }
        public string ListAmount { get; set; }
        public string ListTax { get; set; }
        public string ListFeeAmount { get; set; }
        public string ListBillAmount { get; set; }
        public string ListDeposit { get; set; }
        public string ListUnpaidAmount { get; set; }
        public string ListAdvancePayment { get; set; }
        public string ListTotalPage { get; set; }
        public string ListTotalAll { get; set; }
    }
}
