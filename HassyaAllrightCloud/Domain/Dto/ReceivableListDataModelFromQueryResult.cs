using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class ReceivableListDataModelFromQueryResult
    {
        public string UkeNo { get; set; }
        public short MisyuRen { get; set; }
        public short HenKai { get; set; }
        public byte SeiFutSyu { get; set; }
        public int UriGakKin { get; set; }
        public int SyaRyoSyo { get; set; }
        public int SyaRyoTes { get; set; }
        public int SeiKin { get; set; }
        public decimal NyuKinRui { get; set; }
        public int CouKesRui { get; set; }
        public string FutuUnkRenChar { get; set; }
        public byte SiyoKbn { get; set; }
        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
        public string UpdSyainCd { get; set; }
        public string UpdPrgID { get; set; }
        public string EigyoCd { get; set; }
        public string EigyoCdSeq { get; set; }
        public string EigyoNm { get; set; }
        public string EigyoRyak { get; set; }
        public string SeiGyosyaCd { get; set; }
        public string SeiCd { get; set; }
        public string SeiCdNm { get; set; }
        public string SeiRyakuNm { get; set; }
        public string SeiSitenCd { get; set; }
        public string SeiSitenCdNm { get; set; }
        public string SeiSitRyakuNm { get; set; }
        public string SeiGyosyaCdNm { get; set; }
        public string SeiTaiYmd { get; set; }
        public string UkeEigyoCd { get; set; }
        public string UkeEigyoNm { get; set; }
        public string UkeRyakuNm { get; set; }
        public string TokuiCd { get; set; }
        public string TokuiNm { get; set; }
        public string TokRyakuNm { get; set; }
        public string SitenCd { get; set; }
        public string SitenNm { get; set; }
        public string SitRyakuNm { get; set; }
        public string GyosyaCd { get; set; }
        public string GyosyaNm { get; set; }
        public string HaiSYmd { get; set; }
        public string TouYmd { get; set; }
        public string IkNm { get; set; }
        public string DanTaNm { get; set; }
        public string ZeiKbn { get; set; }
        public int Zeiritsu { get; set; }
        public int TesuRitu { get; set; }
        public string FutTumNm { get; set; }
        public string FutTumKbn { get; set; }
        public string SeisanNm { get; set; }
        public int Suryo { get; set; }
        public int TanKa { get; set; }
        public string SeisanCd { get; set; }
        public string SeiFutSyuNm { get; set; }
        public string ZeiKbnNm { get; set; }
        public int NyukinG { get; set; }
        public int FuriTes { get; set; }
        public string NyuKinKbn { get; set; }
        public string NCouKbn { get; set; }
        public string NyukinRuiG { get; set; }
        public int MisyuG { get; set; }
        public string TSiyoStaYmd { get; set; }
        public string TSiyoEndYmd { get; set; }
        public string SSiyoStaYmd { get; set; }
        public string SSiyoEndYmd { get; set; }
        public byte YouKbn { get; set; }
        public short FutTumRen { get; set; }
        public short FutuUnkRen { get; set; }
    }

    public class ReceivablePaymentSummary
    {
        public long TotalAllSaleAmount { get; set; } = 0;
        public long TotalAllFeeAmount { get; set; } = 0;
        public long TotalAllBillingAmount { get; set; } = 0;
        public long TotalAllDepositAmount { get; set; } = 0;
        public long TotalAllUnpaidAmount { get; set; } = 0;
        public long TotalAllCouponAmount { get; set; } = 0;
        public long TotalAllTaxAmount { get; set; } = 0;
        public long TotalPageSaleAmount { get; set; } = 0;
        public long TotalPageFeeAmount { get; set; } = 0;
        public long TotalPageBillingAmount { get; set; } = 0;
        public long TotalPageDepositAmount { get; set; } = 0;
        public long TotalPageUnpaidAmount { get; set; } = 0;
        public long TotalPageCouponAmount { get; set; } = 0;
        public long TotalPageTaxAmount { get; set; } = 0;
    }

    public class BusinessPlanReceivablePaymentSummary
    {
        public long TotalFareSalesAmount { get; set; } = 0;
        public long TotalFareTaxAmount { get; set; } = 0;
        public long TotalFareFeeAmount { get; set; } = 0;
        public long TotalFareDepositAmount { get; set; } = 0;
        public long TotalFareUnpaidAmount { get; set; } = 0;

        public long TotalGuideSalesAmount { get; set; } = 0;
        public long TotalGuideTaxAmount { get; set; } = 0;
        public long TotalGuideFeeAmount { get; set; } = 0;
        public long TotalGuideDepositAmount { get; set; } = 0;
        public long TotalGuideUnpaidAmount { get; set; } = 0;


        public long TotalOtherSalesAmount { get; set; } = 0;
        public long TotalOtherTaxAmount { get; set; } = 0;
        public long TotalOtherFeeAmount { get; set; } = 0;
        public long TotalOtherDepositAmount { get; set; } = 0;
        public long TotalOtherUnpaidAmount { get; set; } = 0;

        public long TotalCancelSalesAmount { get; set; } = 0;
        public long TotalCancelTaxAmount { get; set; } = 0;
        public long TotalCancelFeeAmount { get; set; } = 0;
        public long TotalCancelUnpaidAmount { get; set; } = 0;
        public long TotalUnpaidSubTotal { get; set; } = 0;



        public long PageFareSalesAmount { get; set; } = 0;
        public long PageFareTaxAmount { get; set; } = 0;
        public long PageFareFeeAmount { get; set; } = 0;
        public long PageFareDepositAmount { get; set; } = 0;
        public long PageFareUnpaidAmount { get; set; } = 0;

        public long PageGuideSalesAmount { get; set; } = 0;
        public long PageGuideTaxAmount { get; set; } = 0;
        public long PageGuideFeeAmount { get; set; } = 0;
        public long PageGuideDepositAmount { get; set; } = 0;
        public long PageGuideUnpaidAmount { get; set; } = 0;


        public long PageOtherSalesAmount { get; set; } = 0;
        public long PageOtherTaxAmount { get; set; } = 0;
        public long PageOtherFeeAmount { get; set; } = 0;
        public long PageOtherDepositAmount { get; set; } = 0;
        public long PageOtherUnpaidAmount { get; set; } = 0;

        public long PageCancelSalesAmount { get; set; } = 0;
        public long PageCancelTaxAmount { get; set; } = 0;
        public long PageCancelFeeAmount { get; set; } = 0;
        public long PageCancelUnpaidAmount { get; set; } = 0;
        public long PageUnpaidSubTotal { get; set; } = 0;
    }
}
