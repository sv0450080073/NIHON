using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class DepositsListReportDataModel
    {
        public string OutputDate { get; set; }
        public string Page { get; set; }
        public string BillingOffice { get; set; }
        public string BillingCode { get; set; }
        public string PaymentDate { get; set; }
        public string BillingCodeRange { get; set; }
        public string CompanyCode { get; set; }
        public string SalesOfficeCodeRange { get; set; }
        public string TransferBankHeader { get; set; }
        public List<DepositDataGrid> DepositDatas { get; set; }
        public string AmountOfPage { get; set; } = string.Empty;
        public string PageTotalOfTransferFee { get; set; } = string.Empty;
        public string PageTotalOfDeposit { get; set; } = string.Empty;
        public string DownPaymentPageTotal { get; set; } = string.Empty;
        public string CumulativeAmountOfMoney { get; set; } = string.Empty;
        public string CumulativeTransferFees { get; set; } = string.Empty;
        public string AccumulatedDeposit { get; set; } = string.Empty;
        public string TotalAmountOfMoney { get; set; } = string.Empty;
        public string TotalTransferFee { get; set; } = string.Empty;
        public string TotalDeposit { get; set; } = string.Empty;
        public string TotalDepositReceived { get; set; } = string.Empty;
        public string OutputPersonCode { get; set; }
        public string OutputPersonName { get; set; }
        public string NyuSEtcNm1 { get; set; }
        public string NyuSEtcNm2 { get; set; }
        public string NyuSyoNm11 { get; set; }
        public string NyuSyoNm12 { get; set; }
        public string NyuSyoNm21 { get; set; }
        public string NyuSyoNm22 { get; set; }
    }
}
