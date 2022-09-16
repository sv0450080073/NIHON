using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto.DepositCoupon
{
    public class DepositCouponPayment
    {
        //lump
        public string StatiticsDeposit { get; set; }
        //cardPayment 現金 01
        public string DepositMethod { get; set; }
        public DateTime? DepositDate { get; set; } = DateTime.Now;
        public DepositOffice DepositOffice { get; set; }
        public int? DepositAmount { get; set; } = 0;
        public string BillAmount { get; set; } = "0";
        public string CumulativeDeposit { get; set; } = "0";
        public string CurrentDeposit { get; set; } = "0";
        public string TotalDepositCouponAmount { get; set; } = "0";
        public string UnpaidAmount { get; set; } = "0";
        public string NumberOfCoupons { get; set; } = "0";
        public string SumCouponsApplied { get; set; } = "0";
        public string CurrentApplied { get; set; } = "0";
        public string CumulativeCouponsApplied { get; set; } = "0";
        //transferPayment 振込 02
        public DepositTransferBank DepositTransferBank { get; set; }
        public byte DepositType { get; set; }
        public int TransferFee { get; set; } = 0;
        public int SponsorshipFund { get; set; } = 0;
        //cardPayment カード 03
        public string CardApprovalNumber { get; set; }
        public string CardSlipNumber { get; set; }
        //billPayment 手形 04
        public DateTime? BillDate { get; set; } = DateTime.Now;
        public string BillNo { get; set; }
        //offsetPayment 相殺 05
        //AdjustmentMoneyPayment 調整金 06
        //CouponPayment クーポン 07
        public List<OffsetPaymentTable> OffsetPaymentTables { get; set; }
        public bool IsEditOffsetTable { get; set; }
        public long TotalApplicationAmount { get; set; }
        public long TotalFaceValue { get; set; }
        public bool IsChangeOrDeleteCoupon { get; set; } = true;
        public bool IsCouponBalances { get; set; }
        //Other1Payment その他１ 91
        public string DetailedNameOfDepositMeans11 { get; set; }
        public string DetailedNameOfDepositMeans12 { get; set; }
        //Other2Payment その他2 92
        public string DetailedNameOfDepositMeans21 { get; set; }
        public string DetailedNameOfDepositMeans22 { get; set; }
    }

    public class DepositPaymentTotal
    {
        public long TotalPageCash { get; set; } = 0;
        public long TotalPageTransfer { get; set; } = 0;
        public long TotalPageTransferFee { get; set; } = 0;
        public long TotalPageTransferSupport { get; set; } = 0;

        public long TotalPageCard { get; set; } = 0;

        public long TotalPageCommercialPaper { get; set; } = 0;

        public long TotalPageOffset { get; set; } = 0;

        public long TotalPageAdjustment { get; set; } = 0;

        public long TotalPageOther1 { get; set; } = 0;

        public long TotalPageOther2 { get; set; } = 0;

        public long TotalPageTotalDeposit { get; set; } = 0;

        public long TotalPageCouponAppliedAmount { get; set; } = 0;

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

    }

    public class OffsetPaymentTable
    {
        public DateTime? DateOfIssue { get; set; } = DateTime.Now;
        public string CouponNo { get; set; }
        public int? ApplicationAmount { get; set; }
        public int? FaceValue { get; set; } = 0;
        //only for validation
        public string BillAmount { get; set; } = "0";
        public string SumCouponsApplied { get; set; } = "0";
        //only for lump process
        public int IndexPayment { get; set; }

    }

    public class CouponBalanceGrid
    {
        public string HakoYmd { get; set; }
        public string CouNo { get; set; }
        public int CouGkin { get; set; }
        public int CouZan { get; set; }
    }

    public class ChaterInquiryGrid
    {
        public short GyosyaCd { get; set; }
        public short TokuiCd { get; set; }
        public short SitenCd { get; set; }
        public string TokuiRyak { get; set; }
        public string SitenRyak { get; set; }
        public int SyaSyuDai { get; set; }
        public int HaseiKin { get; set; }
        public int SyaRyoSyo { get; set; }
        public int SyaRyoTes { get; set; }
        public decimal Zeiritsu { get; set; }
        public decimal TesuRitu { get; set; }
        public int YoushaGak { get; set; }
        public decimal SihRaiRui { get; set; }
    }
}
