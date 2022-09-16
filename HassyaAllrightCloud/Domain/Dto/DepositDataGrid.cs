using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class DepositDataGrid
    {
        public int No { get; set; }
        public string PaymentDate { get; set; }
        public string UkeNo { get; set; }
        public string UkeNoFullString { get; set; }
        public string ReceptionOffice { get; set; }
        public string UkeNoAndReceptionOffice { get; set; }
        public string CustomerName { get; set; }
        public string OperatingSerialNumber { get; set; }
        public string GroupName { get; set; }
        public string DestinationName { get; set; }
        public string GroupNameAndDestinationName { get; set; }
        public string DeliveryDate { get; set; }
        public string ArrivalDate { get; set; }
        public string DeliveryDateAndArrivalDate { get; set; }
        public string BillingType { get; set; }
        public string LoadingGoodName { get; set; }
        public string PaymenMethod { get; set; }
        public int Amount { get; set; }
        public int TransferFee { get; set; }
        public int AmountAndTransferFee { get; set; }
        public int CumulativePayment { get; set; }
        public string CouponFaceValue { get; set; }
        public string CouponNo { get; set; }
        public string CouponNoAndCouponFaceValue { get; set; }
        public int PreviousReceiveAmount { get; set; }
        public int CumulativePaymentAndPreviousReceiveAmount { get; set; }
        public string Transferbank { get; set; }
        public string CardApprovalNumber { get; set; }
        public string CardSlipNumber { get; set; }
        public string PaperDueDate { get; set; }
        public string PaperNumber { get; set; }
        public string Other11 { get; set; }
        public string Other12 { get; set; }
        public string Other21 { get; set; }
        public string Other22 { get; set; }
        public string FutTumNm { get; set; }
        public int Cash { get; set; }
        public int Another { get; set; }
        public int AdjustMoney { get; set; }
        public int Bill { get; set; }
        public int Compensation { get; set; }
        public int TransferAmount { get; set; }
        public string ServiceDate { get; set; }
        public string SaleOffice { get; set; }
        public string Division { get; set; }
        public string BankNm { get; set; }
        public string BankSitNm { get; set; }
        public byte YouKbn { get; set; }
        public short FutTumRen { get; set; }
        public short FutuUnkRen { get; set; }
        public byte SeiFutSyu { get; set; }
    }
}
