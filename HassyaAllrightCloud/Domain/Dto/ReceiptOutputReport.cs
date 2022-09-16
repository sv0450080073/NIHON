using System.Collections.Generic;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class HeaderResultOutput
    {
        public int RyoOutSeq { get; set; }
        public short RyoRen { get; set; }
        public int SeiOutSeq { get; set; }
        public short SeiRen { get; set; }
        public string SeiOutSeqSeiRen => $"{RyoOutSeq.ToString("D8")} - {RyoRen.ToString("D4")}";
        public string SeikYm { get; set; }
        public int ZenKurG { get; set; }
        public int KonUriG { get; set; }
        public int KonSyoG { get; set; }
        public int KonTesG { get; set; }
        public int KonNyuG { get; set; }
        public int KonSeiG { get; set; }
        public string KonSeiGFormat => $"¥{KonSeiG.ToString("#,##0").ToString()}-         ( 税 込 )";
        public string ZipCd { get; set; }
        public string Jyus1 { get; set; }
        public string Jyus2 { get; set; }
        public string TokuiNm { get; set; }
        public string SitenNm { get; set; }
        public string SeiEigZipCd { get; set; }
        public string SeiEigJyus1 { get; set; }
        public string SeiEigJyus2 { get; set; }
        public string SeiEigEigyoNm { get; set; }
        public string TokuiTanNm { get; set; }
        public string SeiEigTelNo { get; set; }
        public string SeiEigFaxNo { get; set; }
        public string MeisaiKensu { get; set; }
        public string SeiHatYmd { get; set; }
        public string SeiEigCompanyNm { get; set; }
        public string TekaSeikyuTouNo { get; set; }
        public int PageReceipt { get; set; }
        public int NoPageReceipt { get; set; }
        public int PageSize { get; set; }
    }

    public class DetailedResultOutput
    {
        public int SeiOutSeq { get; set; }
        public short SeiRen { get; set; }
        public short SeiMeiRen { get; set; }
        public short SeiUchRen { get; set; }
        public string HasYmd { get; set; }
        public string YoyaNm { get; set; }
        public string FutTumNm { get; set; }
        public string HaiSYmd { get; set; }
        public string TouYmd { get; set; }
        public short Suryo { get; set; }
        public int TanKa { get; set; }
        public string SyaSyuNm { get; set; }
        public int UriGakKin { get; set; }
        public int SyaRyoSyo { get; set; }
        public int SyaRyoTes { get; set; }
        public int SeiKin { get; set; }
        public decimal NyuKinRui { get; set; }
        public string BikoNm { get; set; }
        public byte SeiSaHKbn { get; set; }
        public string UkeNo { get; set; }
        public string IriRyoNm { get; set; }
        public string DeRyoNm { get; set; }
        public byte SeiFutSyu { get; set; }
        public short FutaiCd { get; set; }
        public decimal Zeiritsu { get; set; }
        public string MeisaiUchiwake { get; set; }
        public string SyaSyuNmDisplay { get; set; }
    }

    public class ReceiptOutputReport
    {
        public int RyoOutSeq { get; set; }
        public HeaderResultOutput HeaderResultOutput { get; set; }
        public List<DetailedResultOutput> DetailedResultOutput { get; set; }
    }
}
