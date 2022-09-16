using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class StaffsLines
    {
        public string Haisha_UkeNo { get; set; } 
        public short Haisha_UnkRen { get; set; } 
        public short Haisha_SyaSyuRen { get; set; } 
        public short Haisha_TeiDanNo { get; set; } 
        public short Haisha_BunkRen { get; set; } 
        public string Haisha_GoSya { get; set; } 
        public int Haisha_KSKbn { get; set; } 
        public int Haisha_HaiSKbn { get; set; } 
        public int Haisha_HaiIKbn { get; set; } 
        public int Haisha_KhinKbn { get; set; } 
        public int Haisha_NippoKbn { get; set; } 
        public string Haisha_DanTaNm2 { get; set; } 
        public string Haisha_IkNm { get; set; } 
        public int Haisha_HaiSSryCdSeq { get; set; } 
        public int Haisha_SyuEigCdSeq { get; set; } 
        public string Haisha_HaiSYmd { get; set; } 
        public string Haisha_HaiSTime { get; set; } 
        public string Haisha_SyuKoYmd { get; set; } 
        public string Haisha_SyuKoTime { get; set; } 
        public string Haisha_SyuPaTime { get; set; } 
        public string Haisha_HaiSNm { get; set; } 
        public string Haisha_TouYmd { get; set; } 
        public string Haisha_TouChTime { get; set; } 
        public string Haisha_KikYmd { get; set; } 
        public string Haisha_KikTime { get; set; } 
        public string Haisha_TouNm { get; set; } 
        public int Haisha_JyoSyaJin { get; set; } 
        public int Haisha_PlusJin { get; set; } 
        public int Haisha_DrvJin { get; set; } 
        public int Haisha_GuiSu { get; set; } 
        public int Haisha_BunKSyuJyn { get; set; }
        public object Haisha_HaiSKouKNm { get; set; } 
        public object Haisha_HaiSBinNm { get; set; } 
        public string Haisha_HaiSSetTime { get; set; } 
        public string Haisha_HaiSKigou { get; set; } 
        public object Haisha_TouSKouKNm { get; set; } 
        public object Haisha_TouSBinNm { get; set; } 
        public string Haisha_TouSetTime { get; set; } 
        public string Haisha_TouKigou { get; set; } 
        public int Haisha_OthJin1 { get; set; } 
        public int Haisha_OthJin2 { get; set; } 
        public string Haisha_PlatNo { get; set; } 
        public string Haisha_HaiCom { get; set; } 
        public int Haisha_SyaRyoUnc { get; set; } 
        public string Haisha_HaiSJyus1 { get; set; } 
        public string Haisha_HaiSjyus2 { get; set; } 
        public string Haisha_TouJyusyo1 { get; set; } 
        public string Haisha_TouJyusyo2 { get; set; } 
        public int Haisha_KikEigSeq { get; set; }
        public int Haisha_YouTblSeq { get; set; }
        public int Yykasho_UkeCd { get; set; } 
        public int Yykasho_NippoKbn { get; set; } 
        public string Yykasho_KaktYmd { get; set; } 
        public int Yykasho_TokuiSeq { get; set; } 
        public string Tokisk_RyakuNm { get; set; } 
        public int Yykasho_SitenCdSeq { get; set; } 
        public int Yykasho_UkeEigCdSeq { get; set; }         
        public string Tokist_RyakuNm { get; set; } 
        public int Yykasho_InTanCdSeq { get; set; } 
        public string Syain_SyainNm { get; set; } 
        public int Yykasho_EigTanCdSeq { get; set; } 
        public string Eigyotan_SyainNm { get; set; } 
        public string Unkobi_DanTaNm { get; set; } 
        public string Unkobi_HaiSYmd { get; set; } 
        public string Unkobi_HaiSTime { get; set; } 
        public string Unkobi_TouYmd { get; set; } 
        public string Unkobi_TouChTime { get; set; } 
        public int Unkobi_ZenHaFlg { get; set; } 
        public int Unkobi_KhakFlg { get; set; } 
        public string Unkobi_SyukoYmd { get; set; } 
        public string Unkobi_KikYmd { get; set; } 
        public int Unkobi_UnkoJKbn { get; set; } 
        public string Syaryo_SyaRyoNm { get; set; } 
        public int Syaryo_TeiCnt { get; set; } 
        public string Syasyu_SyaSyuNm { get; set; } 
        public string Syasyu_SyaSyuKigo { get; set; } 
        public string YySyasyu_SyaSyuNm { get; set; } 
        public string Haiin_UkeNo { get; set; } 
        public int Haiin_UnkRen { get; set; } 
        public int Haiin_TeiDanNo { get; set; } 
        public int Haiin_BunkRen { get; set; } 
        public int Haiin_HaiInRen { get; set; } 
        public int Haiin_SyainCdSeq { get; set; } 
        public string Haiin_SyukinTime { get; set; } 
        public string Haiin_Syukinbasy { get; set; } 
        public string Haiin_TaiknTime { get; set; } 
        public string Haiin_TaiknBasy { get; set; } 
        public int Kyoshe_SyokumuCdSeq { get; set; } 
        public int Syokum_SyokumuKbn { get; set; } 
        public string Syokum_SyokumuNm { get; set; } 
        public string Eigyoshos_RyakuNm { get; set; } 
        public string Eigyoshos1_RyakuNm { get; set; } 
        public int Yyksyu_SyaSyuDai { get; set; } 
        public byte Yyksyu_KataKbn { get; set; } 
        public string UKEJOKEN_CodeKbnNm { get; set; } 
        public object SIJJOKBN1_CodeKbnNm { get; set; } 
        public object SIJJOKBN2_CodeKbnNm { get; set; } 
        public object SIJJOKBN3_CodeKbnNm { get; set; } 
        public object SIJJOKBN4_CodeKbnNm { get; set; } 
        public object SIJJOKBN5_CodeKbnNm { get; set; }
        public string Haisha_BikoNm { get; set; }
        public string Yyksho_BikoNm { get; set; }
        public string Unkobi_BikoNm { get; set; }
    }
}
