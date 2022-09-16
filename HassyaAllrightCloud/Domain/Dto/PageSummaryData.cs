using System;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class PageSummaryData
    {
        public long _sumBusFee = 0;
        public string SumBusFee
        {
            get => String.Format("{0:C}", _sumBusFee);
            set => long.TryParse(value, out _sumBusFee);
        }
        public long _sumBusTax = 0;
        public string SumBusTax
        {
            get => String.Format("{0:C}", _sumBusTax);
            set => long.TryParse(value, out _sumBusTax);
        }
        public long _sumBusCharge = 0;
        public string SumBusCharge
        {
            get => String.Format("{0:C}", _sumBusCharge);
            set => long.TryParse(value, out _sumBusCharge);
        }
        public long _sumGuideFee = 0;
        public string SumGuideFee
        {
            get => String.Format("{0:C}", _sumGuideFee);
            set => long.TryParse(value, out _sumGuideFee);
        }
        public long _sumGuideTax = 0;
        public string SumGuideTax
        {
            get => String.Format("{0:C}", _sumGuideTax);
            set => long.TryParse(value, out _sumGuideTax);
        }
        public long _sumGuideCharge = 0;
        public string SumGuideCharge
        {
            get => String.Format("{0:C}", _sumGuideCharge);
            set => long.TryParse(value, out _sumGuideCharge);
        }
        public long _sumTaxIncludeFee = 0;
        public string SumTaxIncludeFee
        {
            get => String.Format("{0:C}", _sumTaxIncludeFee);
            set => long.TryParse(value, out _sumTaxIncludeFee);
        }
        public long _sumCancelFee = 0;
        public string SumCancelFee
        {
            get => String.Format("{0:C}", _sumCancelFee);
            set => long.TryParse(value, out _sumCancelFee);
        }
        public long _sumCancelTax = 0;
        public string SumCancelTax
        {
            get => String.Format("{0:C}", _sumCancelTax);
            set => long.TryParse(value, out _sumCancelTax);
        }
        public long _sumTaxIncludedCancelFee = 0;
        public string SumTaxIncludedCancelFee
        {
            get => String.Format("{0:C}", _sumTaxIncludedCancelFee);
            set => long.TryParse(value, out _sumTaxIncludedCancelFee);
        }
        public int _sumCancelCase = 0;
        public string SumCancelCase
        {
            get => _sumCancelCase.ToString();
            set => int.TryParse(value, out _sumCancelCase);
        }
    }
}
