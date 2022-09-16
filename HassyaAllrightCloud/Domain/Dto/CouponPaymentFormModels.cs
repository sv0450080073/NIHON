using System;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class CouponPaymentFormGridItem
    {
        public string UkeNo { get; set; }
        public short NyuSihRen { get; set; }
        public byte NSNyuSihSyu { get; set; }
        public int KesG { get; set; }
        public string BankNm { get; set; }
        public string BankStNm { get; set; }
        public long No { get; set; }
        public string NyuSihHakoYmd { get; set; }
        public string NyuSihEigNm { get; set; }
        public int GridCash { get => NSNyuSihSyu == 1 ? KesG : 0; }
        public int GridTransfer { get => NSNyuSihSyu == 2 ? KesG : 0; }
        public int FurKesG { get; set; }
        public int KyoKesG { get; set; }
        public int GridCard { get => NSNyuSihSyu == 3 ? KesG : 0; }
        public int GridCommercialPaper { get => NSNyuSihSyu == 4 ? KesG : 0; }
        public int GridOffset { get => NSNyuSihSyu == 5 ? KesG : 0; }
        public int GridAdjustment { get => NSNyuSihSyu == 6 ? KesG : 0; }
        public int GridOther1 { get => NSNyuSihSyu == 91 ? KesG : 0; }
        public int GridOther2 { get => NSNyuSihSyu == 92 ? KesG : 0; }
        public int GridTotalDeposit { get => KesG + FurKesG + KyoKesG; }
        public string GridBank { get => BankNm + BankStNm; }
        public string YokinSyuNm { get; set; }
        public string NSCardSyo { get; set; }
        public string NSCardDen { get; set; }
        public string NSTegataNo { get; set; }
        public string NSBankCd { get; set; }
        public string NSBankSitCd { get; set; }
        public byte NSYokinSyu { get; set; }
        public string NSTegataYmd { get; set; }
        public string NSEtcSyo1 { get; set; }
        public string NSEtcSyo2 { get; set; }
        public int CouTblSeq { get; set; }
        public byte NyuKesiKbn { get; set; }
        public int NyuSihEigSeq { get; set; }
        public int NyuSihTblSeq { get; set; }
    }

    public class CouponPaymentFormSummaryItem
    {
        public string TotalType { get; set; }
        public string TotalCash { get; set; }
        public string TotalTransfer { get; set; }
        public string TotalTransferFee { get; set; }
        public string TotalTransferSupport { get; set; }
        public string TotalCard { get; set; }
        public string TotalCommercialPaper { get; set; }
        public string TotalOffset { get; set; }
        public string TotalAdjustment { get; set; }
        public string TotalOther1 { get; set; }
        public string TotalOther2 { get; set; }
        public string TotalTotalDeposit { get; set; }
    }

    public class CouponPaymentPopupFormModel
    {
        public int NyuSihTblSeq { get; set; }
        public short NyuSihRen { get; set; }
        public string UkeNo { get; set; }
        public CouponPaymentGridItem SelectedItem { get; set; }
        public int DepositAmount { get; set; }
        public EigyoListItem? DepositOffice { get; set; }
        public DateTime DepositDate { get; set; }
        public DepositMethodEnum DepositMethod { get; set; }
        public BankTransferItem BankTransfer { get; set; }
        public DepositTypeEnum DepositType { get; set; }
        public DateTime TegataYmd { get; set; }
        public int SponsorshipFee { get; set; }
        public int TransferFee { get; set; }
        public string CardDen { get; set; }
        public string CardSyo { get; set; }
        public string TegataNo { get; set; }
        public string EtcSyo1 { get; set; }
        public string EtcSyo2 { get; set; }
    }
    public class BankTransferItem
    {
        public string DisplayName
        {
            get
            {
                return $"{BankCd,4}:{BankRyakuNm} {BankSitCd,4}:{BankStRyakuNm}";
            }
        }
        public string BankCd { get; set; }
        public string BankRyakuNm { get; set; }
        public string BankSitCd { get; set; }
        public string BankStRyakuNm { get; set; }
    }
    public enum DepositMethodEnum
    {
        Cash = 1,
        Transfer = 2,
        Card = 3,
        Bill = 4,
        Offset = 5,
        AdjustmentMoney = 6,
        DepositorAndOther1 = 91,
        DepositorAndOther2 = 92
    }
    public enum DepositTypeEnum
    {
        Normal = 1,
        Current = 2
    }
}
