using HassyaAllrightCloud.Commons.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class ETCData
    {
        /// <summary>
        /// ファイル名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// カード番号
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// ＥＴＣ明細連番
        /// </summary>
        public short EtcRen { get; set; }
        /// <summary>
        /// 運行年月日
        /// </summary>
        public string UnkYmd { get; set; }
        /// <summary>
        /// 車輌コード
        /// </summary>
        public int SyaRyoCd { get; set; }
        /// <summary>
        /// 受付番号
        /// </summary>
        public string UkeNo { get; set; }
        /// <summary>
        /// 運行日連番
        /// </summary>
        public short UnkRen { get; set; }
        /// <summary>
        /// 悌団番号
        /// </summary>
        public short TeiDanNo { get; set; }
        /// <summary>
        /// 分割連番
        /// </summary>
        public short BunkRen { get; set; }
        /// <summary>
        /// 運行時間
        /// </summary>
        public string UnkTime { get; set; }
        /// <summary>
        /// 変更回数
        /// </summary>
        public short HenKai { get; set; }
        /// <summary>
        /// 付帯積込品コードＳＥＱ
        /// </summary>
        public int FutTumCdSeq { get; set; }
        /// <summary>
        /// 付帯積込品名
        /// </summary>
        public string FutTumNm { get; set; }
        /// <summary>
        /// 入料金所地区コード
        /// </summary>
        public byte IriRyoChiCd { get; set; }
        /// <summary>
        /// 入料金所コード
        /// </summary>
        public string IriRyoCd { get; set; }
        /// <summary>
        /// 出料金所地区コード
        /// </summary>
        public byte DeRyoChiCd { get; set; }
        /// <summary>
        /// 出料金所コード
        /// </summary>
        public string DeRyoCd { get; set; }
        /// <summary>
        /// 精算コードＳＥＱ
        /// </summary>
        public int SeisanCdSeq { get; set; }
        /// <summary>
        /// 精算名
        /// </summary>
        public string SeisanNm { get; set; }
        /// <summary>
        /// 精算区分
        /// </summary>
        public byte SeisanKbn { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public short Suryo { get; set; }
        /// <summary>
        /// 単価
        /// </summary>
        public int TanKa { get; set; }
        /// <summary>
        /// 手数料率
        /// </summary>
        public decimal TesuRitu { get; set; }
        /// <summary>
        /// 手数料額
        /// </summary>
        public int SyaRyoTes { get; set; }
        /// <summary>
        /// 転送区分
        /// </summary>
        public byte TensoKbn { get; set; }
        /// <summary>
        /// 取込単価
        /// </summary>
        public int ImportTanka { get; set; }
        /// <summary>
        /// 備考
        /// </summary>
        public string BikoNm { get; set; }
        /// <summary>
        /// 拡張項目
        /// </summary>
        public string ExpItem { get; set; }
        /// <summary>
        /// 使用区分
        /// </summary>
        public byte SiyoKbn { get; set; }
        /// <summary>
        /// 最終更新年月日
        /// </summary>
        public string UpdYmd { get; set; }
        /// <summary>
        /// 最終更新時間
        /// </summary>
        public string UpdTime { get; set; }
        /// <summary>
        /// 最終更新社員コードＳＥＱ
        /// </summary>
        public int UpdSyainCd { get; set; }
        /// <summary>
        /// 最終更新プログラムＩＤ
        /// </summary>
        public string UpdPrgId { get; set; }

        public string SyaRyoNm { get; set; }
        public string IriDouroNm { get; set; }
        public string IriRyokinNm { get; set; }
        public string IriRyakuNm { get; set; }
        public string DeDouroNm { get; set; }
        public string DeRyokinNm { get; set; }
        public string DeRyakuNm { get; set; }
        public int FutaiCd { get; set; }
        public int SeisanCd { get; set; }
        public string JM_FutTumNm { get; set; }
        public string JM_SeisanNm { get; set; }
        public string SyaRyoEigyoNm { get; set; }
        public string UpdSyainCd_SyainCd { get; set; }
        public string UpdSyainCd_SyainNm { get; set; }

        public byte ZeiKbn { get; set; }
        public int UriGakKin { get; set; }
        public long ZeikomiKin { get; set; }
        public decimal ZeiRitu { get; set; }
        public int SyaRyoSyo { get; set; }
        public string ZeiKbnNm { get; set; }
        public bool Torikomi { get; set; }
        public int MFutCount { get; set; }
        public string DantaiNm { get; set; }
        public string MstTokuiNm { get; set; }
        public string MstSitenNm { get; set; }
        public int TesuKbn { get; set; }
        public ETCFutai selectedFutai { get; set; }
        public ETCSeisan selectedSeisan { get; set; }

        public byte TomKbn { get; set; }
        public short Nittei { get; set; }
        public byte FutGuiKbn { get; set; }

        public List<ETCYoyakuData> listYoyaku { get; set; } = new List<ETCYoyakuData>();
    }

    public class TkmKasSetModel
    {
        public int SeisanCdSeq { get; internal set; }
        public int FutTumCdSeq { get; internal set; }
        public int CompanyCdSeq { get; internal set; }
    }

    public class ETCYoyakuData
    {
        public string UkeNo { get; set; }
        public short UnkRen { get; set; }
        public short TeiDanNo { get; set; }
        public short BunkRen { get; set; }
        public string DanTaNm { get; set; }
        public int SyaRyoCd { get; set; }
        public string SyaRyoNm { get; set; }
        public string DantaNm1 { get; set; }
        public int SyaryoCd1 { get; set; }
        public decimal TesuRituFut { get; set; }
        public decimal TesuRituGui { get; set; }
        public string SitenNm { get; set; }
        public string TokuiNm { get; set; }
        public byte SeikyuTesKbnFut { get; set; }
        public decimal SeikyuTesuRituFut { get; set; }
        public int CountFutai { get; set; }
        public int CountMFutu { get; set; }
        public string SyuKoYmd { get; set; }
        public string SyuKoTime { get; set; }
        public string KikYmd { get; set; }
        public string KikTime { get; set; }
        public string UnkYmd { get; set; }
        public string HaiSYmd { get; set; }
        public string HaiSTime { get; set; }
        public string TouYmd { get; set; }
        public string TouChTime { get; set; }
        public string UnkSyuKoYmd { get; set; }
        public string UnkSyuKoTime { get; set; }
        public string UnkKikYmd { get; set; }
        public string UnkKikTime { get; set; }
    }

    public class ETCSearchParam
    {
        public byte OutputSetting { get; set; }
        public ETCCompany SelectedCompany { get; set; }
        public ETCEigyo SelectedEigyo { get; set; }
        public ETCSyaRyo SelectedSyaRyoFrom { get; set; }
        public ETCSyaRyo SelectedSyaRyoTo { get; set; }
        public ETCFutai SelectedFutai { get; set; }
        public ETCSeisan SelectedSeisan { get; set; }
        public DateTime? ETCDateFrom { get; set; }
        public DateTime? ETCDateTo { get; set; }
        public DateTime? ReturnDateFrom { get; set; }
        public DateTime? ReturnDateTo { get; set; }
        public ETCDropDown SelectedSortOrder { get; set; }
        public ETCDropDown SelectedTesuKbn { get; set; }
        public ETCDropDown SelectedTensoKbn { get; set; }
        public ETCDropDown SelectedPageSize { get; set; }
        public int AcquisitionRange { get; set; }
        public List<string> ListFileName { get; set; } = new List<string>();
        public byte ScreenType { get; set; }
        public int SyaryoCd { get; set; }
        public int TenantCdSeq { get; set; } = new ClaimModel().TenantID;
    }

    public class ETCDropDown
    {
        public string Text { get; set; }
        public byte Value { get; set; }
    }

    public class ETCCompany
    {
        public int TenantCdSeq { get; set; }
        public int CompanyCdSeq { get; set; }
        public int CompanyCd { get; set; }
        public string RyakuMn { get; set; }
        public string Text => $"{CompanyCd:00000} : {RyakuMn}";
    }

    public class ETCEigyo
    {
        public int CompanyCdSeq { get; set; }
        public int EigyoCdSeq { get; set; }
        public int EigyoCd { get; set; }
        public string RyakuMn { get; set; }
        public string Text => $"{EigyoCd:00000} : {RyakuMn}";
    }

    public class ETCSyaRyo
    {
        public int CompanyCdSeq { get; set; }
        public int EigyoCdSeq { get; set; }
        public int SyaRyoCdSeq { get; set; }
        public int SyaRyoCd { get; set; }
        public string SyaRyoNm { get; set; }
        public string Text => $"{SyaRyoCd:00000} : {SyaRyoNm}";
    }

    public class ETCFutai
    {
        public int FutaiCdSeq { get; set; }
        public int FutaiCd { get; set; }
        public string FutaiNm { get; set; }
        public byte ZeiHyoKbn { get; set; }
        public string Text => $"{FutaiCd:0000} : {FutaiNm}";
    }

    public class ETCSeisan
    {
        public int SeisanCdSeq { get; set; }
        public int SeisanCd { get; set; }
        public string SeisanNm { get; set; }
        public byte SeisanKbn { get; set; }
        public string Text => $"{SeisanCd:0000} : {SeisanNm}";
    }

    public class ETCRyokin
    {
        public byte RyokinTikuCd { get; set; }
        public string RyokinCd { get; set; }
        public string RyokinNm { get; set; }
        public string Text => $"{RyokinCd} : {RyokinNm}";
    }

    public class ETCCodeKbn
    {
        public int CodeKbnSeq { get; set; }
        public string CodeKbn { get; set; }
        public string CodeKbnNm { get; set; }
        public string RyakuNm { get; set; }
        public string Text => $"{CodeKbn} : {RyakuNm}";
    }

    public class ETCKyoSet
    {
        public string SetteiCd { get; set; }
        public decimal Zeiritsu1 { get; set; }
        public string Zei1StaYmd { get; set; }
        public string Zei1EndYmd { get; set; }
        public decimal Zeiritsu2 { get; set; }
        public string Zei2StaYmd { get; set; }
        public string Zei2EndYmd { get; set; }
        public decimal Zeiritsu3 { get; set; }
        public string Zei3StaYmd { get; set; }
        public string Zei3EndYmd { get; set; }
    }

    public class ETCHaisha
    {
        public string UkeNo { get; set; }
        public short UnkRen { get; set; }
        public short TeiDanNo { get; set; }
        public short BunkRen { get; set; }
        public int You_YouTblSeq { get; set; }
        public int You_JitaFlg { get; set; }
    }

    public class ETCJitaYouTblSeq
    {
        public int YouTblSeq { get; set; }
        public int YouCdSeq { get; set; }
        public int YouSitCdSeq { get; set; }
        public string HasYmd { get; set; }
    }

    public class ETCFutTum
    {
        public short TeiDanNo { get; set; }
        public short BunkRen { get; set; }
        public short Suryo { get; set; }
        public int Num { get; set; }
        public int SyaRyoSyo { get; set; }
        public int SyaRyoTes { get; set; }
    }

    public class FormVisibleChangedEvent
    {
        public bool IsVisible { get; set; }
        public bool IsReload { get; set; }
    }

    public class MishumData
    {
        public short MisyuRen { get; set; }
    }
}
