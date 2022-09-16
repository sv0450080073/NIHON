using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class DepositListModel
    {
        public int No { get; set; }
        public string NyuSihYmd { get; set; }
        public string UkeNo { get; set; }
        public string UkeRyakuNm { get; set; }
        public string TokRyakuNm { get; set; }
        public string SitRyakuNm { get; set; }
        public short UnkRen { get; set; }
        public string UnkRenS { get; set; }
        public string DantaNm { get; set; }
        public string IkNm { get; set; }
        public string HaiSYmd { get; set; }
        public string TouYmd { get; set; }
        public string SeiFutSyuNm { get; set; }
        public string FutTumNm { get; set; }
        public string NyuKinTejNm { get; set; }
        public dynamic CouGkin { get; set; }
        public string CouNo { get; set; }
        public string BankCd { get; set; }
        public string BankRyak { get; set; }
        public string BankSitCd { get; set; }
        public string BankSitRyak { get; set; }
        public string NyuSihSyuS { get; set; }
        public string CardSyo { get; set; }
        public string CardDen { get; set; }
        public string TegataYmd { get; set; }
        public string TegataNo { get; set; }
        public byte NyuSihSyu { get; set; }
        public int KesG { get; set; }
        public int FurKesG { get; set; }
        public string EtcSyo1 { get; set; }
        public string EtcSyo2 { get; set; }
        public string Orther11 { get; set; }
        public string Orther12 { get; set; }
        public string Orther21 { get; set; }
        public string Orther22 { get; set; }
        public int Amount { get; set; }
        public int TransferFee { get; set; }
        public int CumulativePayment { get; set; }
        public int PreviousReceiveAmount { get; set; }
        public string SeikYm { get; set; }
        public string SeiTaiYmd { get; set; }
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
        public string UkeEigCd { get; set; }
        public string UkeEigyoNm { get; set; }
        public string GyosyaCd { get; set; }
        public string TokuiCd { get; set; }
        public string SitenCd { get; set; }
        public string GyosyaNm { get; set; }
        public string TokuiNm { get; set; }
        public string SitenNm { get; set; }
        public byte SeiFutSyu { get; set; }
        public string BankNm { get; set; }
        public string BankSitNm { get; set; }
        public string TSiyoStaYmd { get; set; }
        public string TSiyoEndYmd { get; set; }
        public string SSiyoStaYmd { get; set; }
        public string SSiyoEndYmd { get; set; }
        public byte YouKbn { get; set; }
        public int Cash { get; set; }
        public int Another { get; set; }
        public int AdjustMoney { get; set; }
        public int Bill { get; set; }
        public int Compensation { get; set; }
        public int TransferAmount { get; set; }
        public short FutTumRen { get; set; }
    }
}
