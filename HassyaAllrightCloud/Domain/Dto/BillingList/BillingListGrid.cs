using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto.BillingList
{
    public class BillingListGrid
    {
        public int EigyoCd { get; set; }
        public string EigyoNm { get; set; }
        public string EigyoRyak { get; set; }
        public short SeiGyosyaCd { get; set; }
        public short SeiCd { get; set; }
        public int SeiCdSeq { get; set; }
        public string SeiCdNm { get; set; }
        public string SeiRyakuNm { get; set; }
        public short SeiSitenCd { get; set; }
        public int SeiSitenCdSeq { get; set; }
        public string SeiSitenCdNm { get; set; }
        public string SeiSitRyakuNm { get; set; }
        public string SeiGyosyaCdNm { get; set; }
        public long UriZenZan { get; set; }
        public long UriUriGakKin { get; set; }
        public long UriSyaRyoSyo { get; set; }
        public long UriSyaRyoTes { get; set; }
        public long UriSeiKin { get; set; }
        public long UriNyuKinRui { get; set; }
        public long UriMisyuKin { get; set; }
        public long UriMaeuke { get; set; }
        public long GaiZenZan { get; set; }
        public long GaiGaiGakKin { get; set; }
        public long GaiSyaRyoSyo { get; set; }
        public long GaiSyaRyoTes { get; set; }
        public long GaiSeiKin { get; set; }
        public long GaiNyuKinRui { get; set; }
        public long GaiMisyuKin { get; set; }
        public long GaiMaeuke { get; set; }
        public long EtcZenZan { get; set; }
        public long EtcEtcGakKin { get; set; }
        public long EtcSyaRyoSyo { get; set; }
        public long EtcSyaRyoTes { get; set; }
        public long EtcSeiKin { get; set; }
        public long EtcNyuKinRui { get; set; }
        public long EtcMisyuKin { get; set; }
        public long EtcMaeuke { get; set; }
        public long CanZenZan { get; set; }
        public long CanCanGakKin { get; set; }
        public long CanSyaRyoSyo { get; set; }
        public long CanSyaRyoTes { get; set; }
        public long CanSeiKin { get; set; }
        public long CanNyuKinRui { get; set; }
        public long CanMisyuKin { get; set; }
        public long CanMaeuke { get; set; }
        public byte UriTesKbn { get; set; }
        public byte GaiTesKbn { get; set; }
        public byte EtcTesKbn { get; set; }
        public string UriTesKbnNm { get; set; }
        public string GaiTesKbnNm { get; set; }
        public string EtcTesKbnNm { get; set; }
        public long No { get; set; }
    }

    public class BillingListGridCsvData
    {
        public int EigyoCd { get; set; }
        public string EigyoNm { get; set; }
        public string EigyoRyak { get; set; }
        public short SeiGyosyaCd { get; set; }
        public short SeiCd { get; set; }
        public short SeiSitenCd { get; set; }
        public int SeiCdSeq { get; set; }
        public int SeiSitenCdSeq { get; set; }
        public string SeiGyosyaCdNm { get; set; }
        public string SeiCdNm { get; set; }
        public string SeiSitenCdNm { get; set; }
        public string SeiRyakuNm { get; set; }
        public string SeiSitRyakuNm { get; set; }
        public long UriZenZan { get; set; }
        public long UriUriGakKin { get; set; }
        public long UriSyaRyoSyo { get; set; }
        public byte UriTesKbn { get; set; }
        public string UriTesKbnNm { get; set; }
        public long UriSyaRyoTes { get; set; }
        public long UriSeiKin { get; set; }
        public long UriNyuKinRui { get; set; }
        public long UriMisyuKin { get; set; }
        public long UriMaeuke { get; set; }
        public long GaiZenZan { get; set; }
        public long GaiGaiGakKin { get; set; }
        public long GaiSyaRyoSyo { get; set; }
        public byte GaiTesKbn { get; set; }
        public string GaiTesKbnNm { get; set; }
        public long GaiSyaRyoTes { get; set; }
        public long GaiSeiKin { get; set; }
        public long GaiNyuKinRui { get; set; }
        public long GaiMisyuKin { get; set; }
        public long GaiMaeuke { get; set; }
        public long EtcZenZan { get; set; }
        public long EtcEtcGakKin { get; set; }
        public long EtcSyaRyoSyo { get; set; }
        public byte EtcTesKbn { get; set; }
        public string EtcTesKbnNm { get; set; }
        public long EtcSyaRyoTes { get; set; }
        public long EtcSeiKin { get; set; }
        public long EtcNyuKinRui { get; set; }
        public long EtcMisyuKin { get; set; }
        public long EtcMaeuke { get; set; }
        public long CanZenZan { get; set; }
        public long CanCanGakKin { get; set; }
        public long CanSyaRyoSyo { get; set; }
        public long CanSeiKin { get; set; }
        public long CanNyuKinRui { get; set; }
        public long CanMisyuKin { get; set; }
        public long CanMaeuke { get; set; }
        public long UGECZenZan { get; set; }
        public long UGECGakKin { get; set; }
        public long UGECSyaRyoSyo { get; set; }
        public long UGECSyaRyoTes { get; set; }
        public long UGECSeiKin { get; set; }
        public long UGECNyuKinRui { get; set; }
        public long UGECMisyuKin { get; set; }
        public long UGECMaeuke { get; set; }
        public long UGCZenZan { get; set; }
        public long UGCGakKin { get; set; }
        public long UGCSyaRyoSyo { get; set; }
        public long UGCSyaRyoTes { get; set; }
        public long UGCSeiKin { get; set; }
        public long UGCNyuKinRui { get; set; }
        public long UGCMisyuKin { get; set; }
        public long UGCMaeuke { get; set; }
    }

    public class BillingListSum {
        public long UriUriGakKinSum { get; set; }
        public long UriSyaRyoSyoSum { get; set; }
        public long UriSyaRyoTesSum { get; set; }
        public long UriSeiKinSum { get; set; }
        public long UriNyuKinRuiSum { get; set; }
        public long UriSum { get; set; }
        public long UriMaeukeSum { get; set; }
        public long GaiGaiGakKinSum { get; set; }
        public long GaiSyaRyoSyoSum { get; set; }
        public long GaiSyaRyoTesSum { get; set; }
        public long GaiSeiKinSum { get; set; }
        public long GaiNyuKinRuiSum { get; set; }
        public long GaiSum { get; set; }
        public long GaiMaeukeSum { get; set; }
        public long EtcEtcGakKinSum { get; set; }
        public long EtcSyaRyoSyoSum { get; set; }
        public long EtcSyaRyoTesSum { get; set; }
        public long EtcSeiKinSum { get; set; }
        public long EtcNyuKinRuiSum { get; set; }
        public long EtcSum { get; set; }
        public long EtcMaeukeSum { get; set; }
        public long CanCanGakKinSum { get; set; }
        public long CanSyaRyoSyoSum { get; set; }
        public long CanSeiKinSum { get; set; }
        public long CanNyuKinRuiSum { get; set; }
        public long CanSum { get; set; }
        public long CanMaeukeSum { get; set; }
    }

    public class BillingListResult
    {
        public List<BillingListGrid> billingListGrids { get; set; } = new List<BillingListGrid>();
        public int CountNumber { get; set; }
        public BillingListSum billingListSum { get; set; } = new BillingListSum();
    }
}
