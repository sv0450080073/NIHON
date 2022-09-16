using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto.DepositCoupon
{
    public class DepositTransferBank
    {
        public string BankCd { get; set; }
        public string BankRyakuNm { get; set; }
        public string BankSitCd { get; set; }
        public string BankStRyakuNm { get; set; }
        public string Text => $"{BankCd} : {BankRyakuNm}  {BankSitCd} : {BankStRyakuNm}";
    }
}
