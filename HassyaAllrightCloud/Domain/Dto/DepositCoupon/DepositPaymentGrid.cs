using HassyaAllrightCloud.Commons.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto.DepositCoupon
{
    public class DepositPaymentGrid
    {
        public int CountNumber { get; set; }
        public long TotalAllCash { get; set; } = 0;

        public long TotalAllTransfer { get; set; } = 0;

        public long TotalAllTransferFee { get; set; } = 0;

        public long TotalAllTransferSupport { get; set; } = 0;

        public long TotalAllCard { get; set; } = 0;

        public long TotalAllCommercialPaper { get; set; } = 0;

        public long TotalAllOffset { get; set; } = 0;

        public long TotalAllAdjustment { get; set; } = 0;

        public long TotalAllOther1 { get; set; } = 0;

        public long TotalAllOther2 { get; set; } = 0;

        public long TotalAllTotalDeposit { get; set; } = 0;

        public long TotalAllCouponAppliedAmount { get; set; } = 0;
        public string UkeNo { get; set; }
        public int NyuSihRen { get; set; }
        public byte NyuSihKbn { get; set; }
        public byte SeiFutSyu { get; set; }
        public short UnkRen { get; set; }
        public int YouTblSeq { get; set; }
        public int KesG { get; set; }
        public int FurKesG { get; set; }
        public int KyoKesG { get; set; }
        public short FutTumRen { get; set; }
        public short NyuSihCouRen { get; set; }
        public int NyuSihTblSeq { get; set; }
        public int CouTblSeq { get; set; }
        public byte SiyoKbn { get; set; }
        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
        public int NS_NyuSihTblSeq { get; set; }
        public int NS_NyuSihKbn { get; set; }
        public int NS_NyuSihSyu { get; set; }
        public string NS_CardSyo { get; set; }
        public string NS_CardDen { get; set; }
        public int NS_NyuSihG { get; set; }
        public int NS_FuriTes { get; set; }
        public int NS_KyoRyoKin { get; set; }
        public string NS_BankCd { get; set; }
        public string NS_BankSitCd { get; set; }
        public int NS_YokinSyu { get; set; }
        public string NS_TegataYmd { get; set; }
        public string NS_TegataNo { get; set; }
        public string NS_EtcSyo1 { get; set; }
        public string NS_EtcSyo2 { get; set; }
        public string NS_UpdYmd { get; set; }
        public string NS_UpdTime { get; set; }
        public string NyuSihHakoYmd { get; set; }
        public int NyuSihEigSeq { get; set; }
        public int NyuSihEigCd { get; set; }
        public string NyuSihEigNm { get; set; }
        public string BankNm { get; set; }
        public string BankStNm { get; set; }
        public string YokinSyuNm { get; set; }
        public byte NyuKesiKbn { get; set; }
        public int CouKesG { get; set; }
        public string NSC_UpdYmd { get; set; }
        public string NSC_UpdTime { get; set; }
        public string CouNo { get; set; }
        public int CouGkin { get; set; }
        public string COU_UpdYmd { get; set; }
        public string COU_UpdTime { get; set; }
        public int COU_NyuSihRen { get; set; }
    }

    public class DepositPaymentFilter
    {
        public string UkeNo { get; set; }
        public short FutuUnkRen { get; set; }
        public byte SeiFutSyu { get; set; }
        public short FutTumRen { get; set; }
        public int Offset { get; set; } = 0;
        public int Limit { get; set; } = Common.LimitPage;
    }

    public class DepositPaymentHaitaCheck {
        public string tkdNyuSihUpdYmdTime { get; set; }
        public string tkdNyShmiUpdYmdTime { get; set; }
        public string tkdCouponUpdYmdTime { get; set; }
        public string tkdNyShCuUpdYmdTime { get; set; }
        public string tkdMishumUpdYmdTime { get; set; }
        public string tkdYykshoUpdYmdTime { get; set; }
        public string tkdFutTumUpdYmdTime { get; set; }
        public string UkeNo { get; set; }
        public short MisyuRen { get; set; }
        public int TenantCdSeq { get; set; }
        public short UnkRen { get; set; }
        public short FutTumRen { get; set; }
        public byte FutTumKbn { get; set; }

    }
}
