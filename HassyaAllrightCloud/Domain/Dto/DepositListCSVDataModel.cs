using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class DepositListCSVDataModel
    {
        public string EigyoCd { get; set; }
        public string EigyoNm { get; set; }
        public string EigyoRyak { get; set; }
        public string SeiGyosyaCd { get; set; }
        public string SeiCd { get; set; }
        public string SeiSitenCd { get; set; }
        public string SeiGyosyaCdNm { get; set; }
        public string SeiCdNm { get; set; }
        public string SeiSitenCdNm { get; set; }
        public string SeiRyakuNm { get; set; }
        public string SeiSitRyakuNm { get; set; }
        public string NyuSihYmd { get; set; }
        public string UkeNo { get; set; }
        public string UkeEigCd { get; set; }
        public string UkeEigyoNm { get; set; }
        public string UkeRyakuNm { get; set; }
        public string GyosyaCd { get; set; }
        public string TokuiCd { get; set; }
        public string SitenCd { get; set; }
        public string GyosyaNm { get; set; }
        public string TokuiNm { get; set; }
        public string SitenNm { get; set; }
        public string TokRyakuNm { get; set; }
        public string SitRyakuNm { get; set; }
        public string UnkRen { get; set; }
        public string DanTaNm { get; set; }
        public string IkNm { get; set; }
        public string HaiSYmd { get; set; }
        public string TouYmd { get; set; }
        public byte SeiFutSyu { get; set; }
        public string SeiFutSyuNm { get; set; }
        public string FutTumNm { get; set; }
        public string NyuSihSyu { get; set; }
        public string NyuKinTejNm { get; set; }
        public int Amount { get; set; }
        public int TransferFee { get; set; }
        public int CumulativeDeposit { get; set; }
        public string CouGkin { get; set; }
        public string CouNo { get; set; }
        public int PreviousPayment { get; set; }
        public string BankCd { get; set; }
        public string BankSitCd { get; set; }
        public string BankNm { get; set; }
        public string BankSitNm { get; set; }
        public string BankRyak { get; set; }
        public string BankSitRyak { get; set; }
        public string CardSyo { get; set; }
        public string CardDen { get; set; }
        public string TegataYmd { get; set; }
        public string TegataNo { get; set; }
        public string NyuSEtcNm1 { get; set; }
        public string Other11 { get; set; }
        public string Other12 { get; set; }
        public string NyuSEtcNm2 { get; set; }
        public string Other21 { get; set; }
        public string Other22 { get; set; }
        public string TSiyoStaYmd { get; set; }
        public string TSiyoEndYmd { get; set; }
        public string SSiyoStaYmd { get; set; }
        public string SSiyoEndYmd { get; set; }
        public int Cash { get; set; }
        public int Another { get; set; }
        public int AdjustMoney { get; set; }
        public int Bill { get; set; }
        public int Compensation { get; set; }
        public int TransferAmount { get; set; }
    }
}
