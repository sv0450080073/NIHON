using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto.BillingList
{
    public class BillingListDetailGrid
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
        public short FutuUnkRen { get; set; }
        public short FutTumRen { get; set; }
        public byte SiyoKbn { get; set; }
        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
        public int UpdSyainCd { get; set; }
        public string UpdPrgId { get; set; }
        public int SeiEigyoCdSeq { get; set; }
        public int SeiEigyoCd { get; set; }
        public string SeiEigyoNm { get; set; }
        public string SeiEigyoRyak { get; set; }
        public short SeiGyosyaCd { get; set; }
        public short SeiCd { get; set; }
        public string SeiCdNm { get; set; }
        public string SeiRyakuNm { get; set; }
        public short SeiSitenCd { get; set; }
        public string SeiSitenCdNm { get; set; }
        public string SeiSitRyakuNm { get; set; }
        public string SeiGyosyaCdNm { get; set; }
        public string SeiTaiYmd { get; set; }
        public int UkeEigyoCd { get; set; }
        public string UkeEigyoNm { get; set; }
        public string UkeRyakuNm { get; set; }
        public string HaiSYmd { get; set; }
        public string TouYmd { get; set; }
        public string IkNm { get; set; }
        public string DanTaNm { get; set; }
        public decimal TesuRitu { get; set; }
        public string FutTumNm { get; set; }
        public string SeisanNm { get; set; }
        public string FutTumKbn { get; set; }
        public string Suryo { get; set; }
        public string TanKa { get; set; }
        public string SeisanCd { get; set; }
        public string SeiFutSyuNm { get; set; }
        public string NyuKinYmd { get; set; }
        public string HasYmd { get; set; }
        public string SeiHatYmd { get; set; }
        public byte NyuKinKbn { get; set; }
        public byte NCouKbn { get; set; }
        public decimal MisyuG { get; set; }
        public string TSiyoStaYmd { get; set; }
        public string TSiyoEndYmd { get; set; }
        public string SSiyoStaYmd { get; set; }
        public string SSiyoEndYmd { get; set; }
        public int SumSyaSyuDai { get; set; }
        public int SumSyaSyuTan { get; set; }
        public string DetailBusType { get; set; }
        public bool Checked { get; set; }
        public long No { get; set; }
    }

    public class BillingListBusType
    {
        public string UkeNo { get; set; }
        public string SyaSyuCd_SyaSyuNm { get; set; }
        public string KataKbn_CodeKbnNm { get; set; }
        public string KataKbn_RyakuNm { get; set; }
    }

    public class BillingListDetailGridCsvData
    {
        public int SeiEigyoCd { get; set; }
        public string SeiEigyoNm { get; set; }
        public string SeiEigyoRyak { get; set; }
        public short SeiGyosyaCd { get; set; }
        public short SeiCd { get; set; }
        public short SeiSitenCd { get; set; }
        public string SeiGyosyaCdNm { get; set; }
        public string SeiCdNm { get; set; }
        public string SeiSitenCdNm { get; set; }
        public string SeiRyakuNm { get; set; }
        public string SeiSitRyakuNm { get; set; }
        public string SeiTaiYmd { get; set; }
        public string UkeNo { get; set; }
        public int UkeEigyoCd { get; set; }
        public string UkeEigyoNm { get; set; }
        public string UkeRyakuNm { get; set; }
        public string DanTaNm { get; set; }
        public string IkNm { get; set; }
        public string HaiSYmd { get; set; }
        public string TouYmd { get; set; }
        public byte SeiFutSyu { get; set; }
        public string SeiFutSyuNm { get; set; }
        public string FutTumNm { get; set; }
        public string SeisanCd { get; set; }
        public string SeisanNm { get; set; }
        public string Suryo { get; set; }
        public string TanKa { get; set; }
        public int SeiKin { get; set; }
        public string NyuKinYmd { get; set; }
        public decimal NyuKinRui { get; set; }
        public decimal MisyuG { get; set; }
        public int UriGakKin { get; set; }
        public int SyaRyoSyo { get; set; }
        public decimal TesuRitu { get; set; }
        public int SyaRyoTes { get; set; }
        public string HasYmd { get; set; }
        public string SeiHatYmd { get; set; }
        public string TSiyoStaYmd { get; set; }
        public string TSiyoEndYmd { get; set; }
        public string SSiyoStaYmd { get; set; }
        public string SSiyoEndYmd { get; set; }
        public int Sum_SyaSyuDai { get; set; }
        public int Sum_SyaSyuTan { get; set; }
    }

    public class BillingListTotal
    {
        public int Type { get; set; }
        public string Text { get; set; }

        public byte SeiFutSyu { get; set; }
        // 請求額
        public long BillAmount { get; set; }
        // 入金合計
        public long DepositAmount { get; set; }
        // 未収額
        public long UnpaidAmount { get; set; }
        // 売上額
        public long SalesAmount { get; set; }
        // 消費税額
        public long TaxAmount { get; set; }
        // 手数料額
        public long CommissionAmount { get; set; }
    }

    public class BillingListDetailResult {
        public List<BillingListDetailGrid> billingListDetailGrids { get; set; } = new List<BillingListDetailGrid>();
        public List<BillingListTotal> billingListTotals { get; set; } = new List<BillingListTotal>();
        public int CountNumber { get; set; }
    }
}
