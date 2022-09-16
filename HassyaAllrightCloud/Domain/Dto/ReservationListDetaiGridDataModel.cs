using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class ReservationListDetaiGridDataModel
    {
        public string No { get; set; }
        public string ReceiptNumber { get; set; }
        public string ReceptionOffice { get; set; }
        public string CustomerName { get; set; }
        public string BillingDate { get; set; }
        public string OperationScheduleSerialNumber { get; set; }
        public string GroupName { get; set; }
        public string DestinationName { get; set; }
        public string DeliveryDate { get; set; }
        public string ArrivalDate { get; set; }
        public string BillingType { get; set; }
        public string LodingGoodsName { get; set; }
        public string PaymentName { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public int SalesAmount { get; set; }
        public string TaxRate { get; set; }
        public int TaxAmount { get; set; }
        public string FeeRate { get; set; }
        public int FeeAmount { get; set; }
        public int BillingAmount { get; set; }
        public int DepositAmount { get; set; }
        public int UnpaidAmount { get; set; }
        public int CouponAmount { get; set; }
        public string SeiRyakuNm { get; set; }
        public string SeiSitRyakuNm { get; set; }
        public int NyuKinKbn { get; set; }
        public int NCouKbn { get; set; }
        public int CumulativeDeposit { get; set; }
        public byte YouKbn { get; set; }
        public short FutTumRen { get; set; }
        public short FutuUnkRen { get; set; }
        public byte SeiFutSyu { get; set; }
        public string UkeNo { get; set; }
    }
}
