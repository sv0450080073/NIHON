using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto.DepositCoupon
{
    public class DepositCouponTotal
    {
        public long TotalPageSalesAmount { get; set; }
        public long TotalPageTaxAmount { get; set; }
        public long TotalPageTaxIncluded { get; set; }
        public long TotalPageCommissionAmount { get; set; }
        public long TotalPageBillAmount { get; set; }
        public long TotalPageCumulativeDeposit { get; set; }
        public long TotalPageUnpaidAmount { get; set; }
        public long TotalAllSalesAmount { get; set; }
        public long TotalAllTaxAmount { get; set; }
        public long TotalAllTaxIncluded { get; set; }
        public long TotalAllCommissionAmount { get; set; }
        public long TotalAllBillAmount { get; set; }
        public long TotalAllCumulativeDeposit { get; set; }
        public long TotalAllUnpaidAmount { get; set; }
    }

    public class DepositCouponItemTotal
    {
        public string ItemFare { get; set; }
        public long ItemFareNum { get; set; }
        public string ItemIncidental { get; set; }
        public long ItemIncidentalNum { get; set; }
        public string ItemTollFee { get; set; }
        public long ItemTollFeeNum { get; set; }
        public string ItemArrangementFee { get; set; }
        public long ItemArrangementFeeNum { get; set; }
        public string ItemGuideFee { get; set; }
        public long ItemGuideFeeNum { get; set; }
        public string ItemLoaded { get; set; }
        public long ItemLoadedNum { get; set; }
        public string ItemCancellation { get; set; }
        public long ItemCancellationNum { get; set; }
        public string Total { get; set; }
        public long TotalNum { get; set; }
        public string StatiticsDeposit { get; set; }
        public long StatiticsDepositNum { get; set; }
    }
}
