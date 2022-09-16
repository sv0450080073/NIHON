using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto.InvoiceIssueRelease
{
    public class InvoiceIssueGrid
    {
        public int CountNumber { get; set; }
        public int SeiOutSeq { get; set; }
        public short SeiRen { get; set; }
        public int TokuiSeq { get; set; }
        public int SitenCdSeq { get; set; }
        public string SiyoEndYmd { get; set; }
        public string SeikYm { get; set; }
        public int ZenKurG { get; set; }
        public int KonUriG { get; set; }
        public int KonSyoG { get; set; }
        public int KonTesG { get; set; }
        public int KonNyuG { get; set; }
        public int KonSeiG { get; set; }
        public byte SeikyuSiyoKbn { get; set; }
        public string SeikyuUpdYmd { get; set; }
        public string SeikyuUpdTime { get; set; }
        public int SeikyuUpdSyainCd { get; set; }
        public string SeikyuUpdPrgId { get; set; }
        public int SeikyuSeiOutSeq { get; set; }
        public string SeikyuSeikYm { get; set; }
        public string SeikyuSeiHatYmd { get; set; }
        public string SeiOutTime { get; set; }
        public int InTanCdSeq { get; set; }
        public byte SeiOutSyKbn { get; set; }
        public byte SeiGenFlg { get; set; }
        public string StaUkeNo { get; set; }
        public string EndUkeNo { get; set; }
        public byte StaYoyaKbn { get; set; }
        public byte EndYoyaKbn { get; set; }
        public int SeiEigCdSeq { get; set; }
        public byte SeiSitKbn { get; set; }
        public int StaSeiCdSeq { get; set; }
        public int StaSeiSitCdSeq { get; set; }
        public int EndSeiCdSeq { get; set; }
        public int EndSeiSitCdSeq { get; set; }
        public byte SimeD { get; set; }
        public byte PrnCpys { get; set; }
        public byte PrnCpysTan { get; set; }
        public byte TesPrnKbn { get; set; }
        public byte SeiFutUncKbn { get; set; }
        public byte SeiFutFutKbn { get; set; }
        public byte SeiFutTukKbn { get; set; }
        public byte SeiFutTehKbn { get; set; }
        public byte SeiFutGuiKbn { get; set; }
        public byte SeiFutTumKbn { get; set; }
        public byte SeiFutCanKbn { get; set; }
        public string ZipCd { get; set; }
        public string Jyus1 { get; set; }
        public string Jyus2 { get; set; }
        public string TokuiNm { get; set; }
        public string SitenNm { get; set; }
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
        public string SeiGyosyaCdNm { get; set; }
        public short SeiCd { get; set; }
        public string SeiCdNm { get; set; }
        public string SeiRyakuNm { get; set; }
        public short SeiSitenCd { get; set; }
        public string SeiSitenCdNm { get; set; }
        public string SeiSitRyakuNm { get; set; }
        public string MinSeiTaiYmd { get; set; }
        public string MaxSeiTaiYmd { get; set; }
        public string SeiHatYmd { get; set; }
        public string TSiyoStaYmd { get; set; }
        public string TSiyoEndYmd { get; set; }
        public string SSiyoStaYmd { get; set; }
        public string SSiyoEndYmd { get; set; }
        public bool Checked { get; set; }
    }
}
