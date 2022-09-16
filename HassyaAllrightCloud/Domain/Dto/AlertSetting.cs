using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class AlertSetting
    {
        public string Date { get; set; }
        public string AlertName { get; set; }
        public string Number { get; set; }
        public string Link { get; set; }
    }

    public class AlertSettingTimeLine
    {
        public int TenantCdSeq { get; set; }
        public int AlertCdSeq { get; set; }
        public short AlertKbn { get; set; }
        public int AlertCd { get; set; }
        public string AlertNm { get; set; }
        public int DefaultVal { get; set; }
        public byte DefaultTimeline { get; set; }
        public byte DefaultZengo { get; set; }
        public byte DefaultDisplayKbn { get; set; }
        public int CurTenantCdSeq { get; set; }
        public int CurVal { get; set; }
        public byte CurTimeline { get; set; }
        public byte CurZengo { get; set; }
        public byte CurDisplayKbn { get; set; }
        public int SyainCdSeq { get; set; }
        public byte UserDisplayKbn { get; set; }
    }

    public class AlertSettingForCode36
    {
        public int SyaRyoCdSeq { get; set; } //車両Seq
        public string StaYmd { get; set; } //開始年月日
        public string EndYmd { get; set; } //終了年月日
        public int EigyoCdSeq { get; set; } //営業所Seq
        public int CompanyCdSeq { get; set; } //会社Seq
        public byte KataKbn { get; set; } //型区分
    }

    public class AlertSettingForCode37
    {
        public string UkeNo { get; set; } //受付番号
        public short UnkRen { get; set; } //運行日連番
        public short SyaSyuRen { get; set; } //車種連番
        public short TeiDanNo { get; set; } //悌団番号
        public short BunkRen { get; set; } //分割連番
        public int HaiSSryCdSeq { get; set; } //配車車輌コードＳＥＱ
        public string SyuKoYmd { get; set; } //出庫年月日
        public string KikYmd { get; set; } //帰庫年月日
        public byte KataKbn { get; set; } //型区分
    }

    public class AlertSettingForCode38
    {
        public int ShuriTblSeq { get; set; } //修理Seq
        public int SyaRyoCdSeq { get; set; } //車両Seq
        public string ShuriSYmd { get; set; } //修理開始年月日
        public string ShuriSTime { get; set; } //修理開始時間
        public string ShuriEYmd { get; set; } //修理終了年月日
        public string ShuriETime { get; set; } //修理終了時間
        public byte KataKbn { get; set; } //型区分
    }

    public class AlertSettingForCode39
    {
        public int SyainCdSeq { get; set; } //社員Seq
        public int SyokumuCdSeq { get; set; } //職務Seq
        public string SyokumuNm { get; set; } //職務名
        public byte SyokumuKbn { get; set; } //職務区分
        public string CodeKbnNm { get; set; } //職務区分名
        public string TenkoNo { get; set; } //点呼番号
        public int EigyoCdSeq { get; set; } //営業所Seq
        public int EigyoCd { get; set; } //営業所コード
        public string RyakuNm { get; set; } //営業所名
        public string SyainCd { get; set; } //社員コード
        public string SyainNm { get; set; } //社員名称
        public string StaYmd { get; set; } //開始年月日
        public string EndYmd { get; set; } //終了年月日
        public int CompanyCdSeq { get; set; } //会社コードSeq
        public int CompanyCd { get; set; } //会社コード
        public string CompanyNm { get; set; } //会社名
    }

    public class AlertSettingForCode310
    {
        public string UnkYmd { get; set; } //交番_運行年月日
        public int SyainCdSeq { get; set; } //交番_社員Seq
        public short KouBnRen { get; set; } //交番_交番連番
        public int KinKyuTblCdSeq { get; set; } //勤務休日_勤務休日TblSeq
        public byte KinKyuKbn { get; set; } //勤務休日種別_種別区分
        public byte KyusyutsuKbn { get; set; } //交番_運行年月日
        public byte BigTypeDrivingFlg { get; set; } //大型乗務フラグ
        public byte MediumTypeDrivingFlg { get; set; } //中型乗務フラグ
        public byte SmallTypeDrivingFlg { get; set; } //小型乗務フラグ
    }

    public class VehicleAcquisitionProcess
    {
        public string UkeNo { get; set; }
        public int UkeEigCdSeq { get; set; }
        public int YoyaKbnSeq { get; set; }
        public decimal Zeiritsu { get; set; }
        public int InTanCdSeq { get; set; }
        public string KaktYmd { get; set; }
        public string SeiTaiYmd { get; set; }
        public string DanTaNm { get; set; }
        public short UnkRen { get; set; }
        public short SyaSyuRen { get; set; }
        public byte KataKbn { get; set; }
        public string HaishaUkeNo { get; set; }
        public short HaishaUnkRen { get; set; }
        public short TeiDanNo { get; set; }
        public short BunkRen { get; set; }
        public string GoSya { get; set; }
        public short HenKai { get; set; }
        public short BunKsyuJyn { get; set; }
        public int HaiSsryCdSeq { get; set; }
        public int KssyaRseq { get; set; }
        public string SyuKoYmd { get; set; }
        public string SyuKoTime { get; set; }
        public int SyuEigCdSeq { get; set; }
        public int KikEigSeq { get; set; }
        public string HaiSkigou { get; set; }
        public string HaiSsetTime { get; set; }
        public string IkNm { get; set; }
        public string HaiSymd { get; set; }
        public string HaiStime { get; set; }
        public string HaiSnm { get; set; }
        public string HaiSjyus1 { get; set; }
        public string HaiSjyus2 { get; set; }
        public string TouYmd { get; set; }
        public string TouChTime { get; set; }
        public string TouNm { get; set; }
        public string TouKigou { get; set; }
        public string TouSetTime { get; set; }
        public string TouJyusyo1 { get; set; }
        public string TouJyusyo2 { get; set; }
        public string SyuPaTime { get; set; }
        public short JyoSyaJin { get; set; }
        public short PlusJin { get; set; }
        public short DrvJin { get; set; }
        public short GuiSu { get; set; }
        public short OthJin1 { get; set; }
        public short OthJin2 { get; set; }
        public string PlatNo { get; set; }
        public string HaiCom { get; set; }
        public string KikYmd { get; set; }
        public string KikTime { get; set; }
        public byte Kskbn { get; set; }
        public byte YouKataKbn { get; set; }
        public int YouTblSeq { get; set; }
        public int SyaRyoUnc { get; set; }
        public byte HaiSkbn { get; set; }
        public byte HaiIkbn { get; set; }
        public byte NippoKbn { get; set; }
        public string SyaSyuNm { get; set; }
        public string SyaSyuKigo { get; set; }
        public string SyaRyoNm { get; set; }
        public int SyaSyuCdSeq { get; set; }
        public byte TeiCnt { get; set; }
        public int TokuiSeq { get; set; }
        public string RyakuNm { get; set; }
        public string TTokuiSeq { get; set; }
        public string RRyakuNm { get; set; }
        public string YYouTblSeq { get; set; }
        public string YouCdSeq { get; set; }
        public string YouSitCdSeq { get; set; }
        public string NinkaKbn { get; set; }
    }

    public class CodeDataModel
    {
        public int CodeKbnSeq { get; set; }
        public string CodeKbnNm { get; set; }
        public string CodeKbn { get; set; }
        public string Text => $"{CodeKbnNm}";
        public string CodeText => $"{CodeKbn} : {CodeKbnNm}";
    }

    public class ShowAlertSettingGrid
    {
        public int TenantCdSeq { get; set; }
        public int AlertCdSeq { get; set; }
        public short AlertKbn { get; set; }
        public int AlertCd { get; set; }
        public string AlertNm { get; set; }
        public int CurTenantCdSeq { get; set; }
        public byte CurDisplayKbn { get; set; }
        public int SyainCdSeq { get; set; }
        public byte UserDisplayKbn { get; set; }
        public bool Checked { get; set; }
        public string AlertColor { get; set; }
    }

    public class ShowAlertSettingGridDisplay
    {
        public string AlertTypeName { get; set; }
        public string AlertTypeColor { get; set; }
        public List<ShowAlertSettingGrid> ShowAlertSettingGrids { get; set; }
    }
}
