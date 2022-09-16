using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto.DepositCoupon
{
    public class DepositCouponGrid
    {
        public string Code { get; set; }
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
        public string UpdPrgID { get; set; }
        public int EigyoCd { get; set; }
        public int EigyoCdSeq { get; set; }
        public string EigyoNm { get; set; }
        public string EigyoRyak { get; set; }
        public short SeiGyosyaCd { get; set; }
        public short SeiCd { get; set; }
        public string SeiCdNm { get; set; }
        public string SeiRyakuNm { get; set; }
        public short SeiSitenCd { get; set; }
        public string SeiSitenCdNm { get; set; }
        public string SeiSitRyakuNm { get; set; }
        public string SeiGyosyaCdNm { get; set; }
        public string SeiTaiYmd { get; set; }
        public string CanYmd { get; set; }
        public int UkeEigCd { get; set; }
        public string UkeEigyoNm { get; set; }
        public string UkeRyakuNm { get; set; }
        public short TokuiCd { get; set; }
        public string TokuiNm { get; set; }
        public string TokRyakuNm { get; set; }
        public short SitenCd { get; set; }
        public string SitenNm { get; set; }
        public string SitRyakuNm { get; set; }
        public short GyosyaCd { get; set; }
        public string GyosyaNm { get; set; }
        public string HaiSYmd { get; set; }
        public string HaiSTime { get; set; }
        public string TouYmd { get; set; }
        public string TouChTime { get; set; }
        public string IkNm { get; set; }
        public string DanTaNm { get; set; }
        public byte ZeiKbn { get; set; }
        public decimal Zeiritsu { get; set; }
        public decimal TesuRitu { get; set; }
        public string FutTumNm { get; set; }
        public string FutTumKbn { get; set; }
        public string SeisanNm { get; set; }
        public string Suryo { get; set; }
        public string TanKa { get; set; }
        public string SeiFutSyuNm { get; set; }
        public string ZeiKbnNm { get; set; }
        public byte YoyaKbn { get; set; }
        public string YoyaKbnNm { get; set; }
        public string LastNyuYmd { get; set; }
        public string LastCouNo { get; set; }
        public byte YouKbn { get; set; }
        public byte NyuKinKbn { get; set; }
        public byte NCouKbn { get; set; }
        public bool Checked { get; set; }
        public string HaiSYmdString { get; set; }
        public string HaiSTimeString { get; set; }
        public string TouYmdString { get; set; }
        public string TouChTimeString { get; set; }
    }

    public class DepositCouponPageTotal
    {
        public int CountNumber { get; set; }
        public long TotalSaleAmount { get; set; }
        public long TotalTaxAmount { get; set; }
        public long TotalTaxIncluded { get; set; }
        public long TotalCommissionAmount { get; set; }
        public long TotalBillAmount { get; set; }
        public long TotalCumulativeDeposit { get; set; }
        public long TotalUnpaidAmount { get; set; }
    }

    public class DepositCouponResult
    {
        public List<DepositCouponGrid> depositCouponGrids { get; set; } = new List<DepositCouponGrid>();
        public DepositCouponPageTotal depositCouponTotal { get; set; } = new DepositCouponPageTotal();
    }
}
