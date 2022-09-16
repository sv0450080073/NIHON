using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class BusBookingData
    {
        public string Yyksho_UkeNo { get; set; }
        public int Yyksho_UkeCd { get; set; }
        public int Yyksho_UkeEigCdSeq { get; set; }
        public int Yyksho_YoyaKbnSeq { get; set; }
        public decimal Yyksho_Zeiritsu { get; set; }
        public int Yyksho_InTanCdSeq { get; set; }
        public string Yyksho_KaktYmd { get; set; }
        public string Yyksho_SeiTaiYmd { get; set; }
        public short Unkobi_UnkRen { get; set; }
        public string Unkobi_HaiSYmd { get; set; }
        public string Unkobi_HaiSTime { get; set; }
        public string Unkobi_TouYmd { get; set; }
        public string Unkobi_TouChTime { get; set; }
        public string Unkobi_SyuPaTime { get; set; }
        public string Unkobi_DanTaNm { get; set; }
        public string Unkobi_IkNm { get; set; }
        public string Unkobi_HaiSNm { get; set; }
        public byte Unkobi_KSKbn { get; set; }
        public byte Unkobi_YouKbn { get; set; }
        public string Unkobi_SyuKoTime { get; set; }
        public string Unkobi_KikTime { get; set; }
        public short YykSyu_UnkRen { get; set; }
        public short YykSyu_SyaSyuRen { get; set; }
        public int YykSyu_KataKbn { get; set; }
        public string Haisha_GoSya { get; set; }
        public string Haisha_UkeNo { get; set; }
        public short Haisha_UnkRen { get; set; }
        public short Haisha_TeiDanNo { get; set; }
        public short Haisha_BunkRen { get; set; }
        public short Haisha_HenKai { get; set; }
        public short Haisha_BunKSyuJyn { get; set; }
        public int Haisha_HaiSSryCdSeq { get; set; }
        public int Haisha_KSSyaRSeq { get; set; }
        public string Haisha_SyuKoYmd { get; set; }
        public string Haisha_SyuKoTime { get; set; }
        public string Haisha_SyuPaTime { get; set; }
        public int Haisha_SyuEigCdSeq { get; set; }
        public int Haisha_KikEigSeq { get; set; }
        public string Haisha_HaiSKigou { get; set; }
        public string Haisha_HaiSSetTime { get; set; }
        public string Haisha_IkNm { get; set; }
        public string Haisha_HaiSYmd { get; set; }
        public string Haisha_HaiSTime { get; set; }
        public string Haisha_HaiSNm { get; set; }
        public string Haisha_HaiSJyus1 { get; set; }
        public string Haisha_HaiSJyus2 { get; set; }
        public string Haisha_TouYmd { get; set; }
        public string Haisha_TouChTime { get; set; }
        public string Haisha_TouNm { get; set; }
        public string Haisha_TouKigou { get; set; }
        public string Haisha_TouSetTime { get; set; }
        public string Haisha_TouJyusyo1 { get; set; }
        public string Haisha_TouJyusyo2 { get; set; }
        public short Haisha_JyoSyaJin { get; set; }
        public short Haisha_PlusJin { get; set; }
        public short Haisha_DrvJin { get; set; }
        public short Haisha_GuiSu { get; set; }
        public short Haisha_OthJin1 { get; set; }
        public short Haisha_OthJin2 { get; set; }
        public string Haisha_PlatNo { get; set; }
        public string Haisha_HaiCom { get; set; }
        public string Haisha_KikYmd { get; set; }
        public string Haisha_KikTime { get; set; }
        public byte Haisha_KSKbn { get; set; }
        public byte Haisha_YouKataKbn { get; set; }
        public int Haisha_YouTblSeq { get; set; }
        public int Haisha_SyaRyoUnc { get; set; }
        public byte Haisha_HaiSKbn { get; set; }
        public byte Haisha_HaiIKbn { get; set; }
        public int Haisha_NippoKbn { get; set; }
        public string SyaSyu_SyaSyuNm { get; set; }
        public string SyaSyu_SyaSyuNm_Haisha { get; set; }
        public int SyaSyu_SyaSyuCdSeq { get; set; }
        public string SyaSyu_SyaSyuKigo { get; set; }
        public string SyaRyo_SyaRyoNm { get; set; }
        public byte SyaRyo_TeiCnt { get; set; }
        public int TokiSk_TokuiSeq { get; set; }
        public string TokiSk_RyakuNm { get; set; }
        public int TokiSt_TokuiSeq { get; set; }
        public string TokiSt_RyakuNm { get; set; }
        public int Yousha_YouTblSeq { get; set; }
        public int Yousha_YouCdSeq { get; set; }
        public int Yousha_YouSitCdSeq { get; set; }
        public string TextDisplay { get; set; }
        public string CodeKb_RyakuNm { get; set; }
        public int Syasyu_KataKbn { get; set; }
        public int SyaRyo_NinKaKbn { get; set; }
        public string Haisha_BikoNm { get; set; }
        public string Yyksho_BikoNm { get; set; }
        public string Unkobi_BikoNm { get; set; }
        //haita check
        public string Yyksho_UpdYmd { get; set; }
        public string Yyksho_UpdTime { get; set; }
        public string Unkobi_UpdYmd { get; set; }
        public string Unkobi_UpdTime { get; set; }
        public string Eigyos_RyakuNm { get; set; }
        public string Syain_SyainNm { get; set; }
        public string Eigyoshos_RyakuNm { get; set; }
        public string Eigyoshos1_RyakuNm { get; set; }
        public string Haisha_HaiSKouKNm { get; set; }
        public string Haisha_HaisBinNm { get; set; }
        public string Haisha_TouSKouKNm { get; set; }
        public string Haisha_TouSBinNm { get; set; }
        public string UKEJOKEN_CodeKbnNm { get; set; }
        public string SIJJOKBN1_CodeKbnNm { get; set; }
        public string SIJJOKBN2_CodeKbnNm { get; set; }
        public string SIJJOKBN3_CodeKbnNm { get; set; }
        public string SIJJOKBN4_CodeKbnNm { get; set; }
        public string SIJJOKBN5_CodeKbnNm { get; set; }
        public string YySyasyu_SyaSyuNm { get; set; }
        public string Eigyotan_SyainNm { get;set; }
    }

    public class BookingRemarkHaitaCheck
    {
        public string UkeNo { get; set; }
        public short UnkRen { get; set; }
        public string YykshoUpdYmdTIme { get; set; }
        public string UnkobiUpdYmdTIme { get; set; }

    }
}
