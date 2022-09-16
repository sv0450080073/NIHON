using HassyaAllrightCloud.Commons.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static HassyaAllrightCloud.Commons.Helpers.BookingInputHelper;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class HaiShaQueryData
    {
        public string HAISHAUkeNo { get; set; }
        public int YYKSHOUkeCD { get; set; }
        public short HAISHAUnkRen { get; set; }
        public short HAISHASyaSyuRen { get; set; }
        public short HAISHATeiDanNo { get; set; }
        public short HAISHABunkRen { get; set; }
        public string HAISHAGoSya { get; set; }
        public int HAISHASyuEigCdSeq { get; set; }
        public int HAISHAHaiSSryCdSeq { get; set; }
        public string HAISHADanTaNm2 { get; set; }
        public int HAISHAIkMapCdSeq { get; set; }
        public string HAISHAIkNm { get; set; }
        public string HAISHASyuKoYmd { get; set; }
        public string HAISHASyuKoTime { get; set; }
        public string HAISHASyuPaTime { get; set; }
        public string HAISHAHaiSYmd { get; set; }
        public string HAISHAHaiSTime { get; set; }
        public int HAISHAHaiSCdSeq { get; set; }
        public string HAISHAHaiSNm { get; set; }
        public string HAISHAHaiSJyus1 { get; set; }
        public string HAISHAHaiSJyus2 { get; set; }
        public string HAISHAHaiSKigou { get; set; }
        public int HAISHAHaiSKouKCdSeq { get; set; }
        public string HAISHAHaiSKouKNm { get; set; }
        public int HAISHAHaiSBinCdSeq { get; set; }
        public string HAISHAHaisBinNm { get; set; }
        public string HAISHAHaiSSetTime { get; set; }
        public string HAISHAKikYmd { get; set; }
        public string HAISHAKikTime { get; set; }
        public string HAISHATouYmd { get; set; }
        public string HAISHATouChTime { get; set; }
        public int HAISHATouCdSeq { get; set; }
        public string HAISHATouNm { get; set; }
        public string HAISHATouJyusyo1 { get; set; }
        public string HAISHATouJyusyo2 { get; set; }
        public string HAISHATouKigou { get; set; }
        public int HAISHATouKouKCdSeq { get; set; }
        public string HAISHATouSKouKNm { get; set; }
        public int HAISHATouBinCdSeq { get; set; }
        public string HAISHATouBinNm { get; set; }
        public string HAISHATouSetTime { get; set; }
        public short HAISHAJyoSyaJin { get; set; }
        public short HAISHAPlusJin { get; set; }
        public short HAISHADrvJin { get; set; }
        public short HAISHAGuiSu { get; set; }
        public byte HAISHAOthJinKbn1 { get; set; }
        public short HAISHAOthJin1 { get; set; }
        public byte HAISHAOthJinKbn2 { get; set; }
        public short HAISHAOthJin2 { get; set; }
        public byte HAISHAKSKbn { get; set; }
        public byte HAISHAHaiSKbn { get; set; }
        public byte HAISHAHaiIKbn { get; set; }
        public byte HAISHANippoKbn { get; set; }
        public int HAISHAYouTblSeq { get; set; }
        public byte HAISHAYouKataKbn { get; set; }
        public int HAISHASyaRyoUnc { get; set; }
        public int HAISHASyaRyoSyo { get; set; }
        public int HAISHASyaRyoUncSyaRyoSyo { get; set; }
        public int HAISHASyaRyoTes { get; set; }
        public int HAISHAYoushaUnc { get; set; }
        public int HAISHAYoushaSyo { get; set; }
        public int HAISHAYoushaTes { get; set; }
        public string HAISHAPlatNo { get; set; }
        public byte HAISHAUkeJyKbnCd { get; set; }
        public short HAISHABunKSyuJyn { get; set; }
        public string UNKOBIHaiSYmd { get; set; }
        public string UNKOBIHaiSTime { get; set; }
        public string UNKOBITouYmd { get; set; }
        public string UNKOBITouChTime { get; set; }
        public string UNKOBIDanTaNm { get; set; }
        public byte UNKOBIKSKbn { get; set; }
        public byte UNKOBIHaiSKbn { get; set; }
        public byte UNKOBIHaiIKbn { get; set; }
        public byte UNKOBINippoKbn { get; set; }
        public byte UNKOBIYouKbn { get; set; }
        public byte UNKOBIUkeJyKbnCd { get; set; }
        public int YYKSHOTokuiSeq { get; set; }
        public int YYKSHOSitenCdSeq { get; set; }
        public string TOKISKRyakuNm { get; set; }
        public string TOKISTRyakuNm { get; set; }
        public int SYARYOSyaRyoCd { get; set; }
        public string SYARYOSyaRyoNm { get; set; }
        public string SYASYUSyaSyuNm { get; set; }
        public string KAISHARyakuNm { get; set; }
        public string EIGYOSRyakuNm { get; set; }
        public int YOUSHAYouCdSeq { get; set; }
        public int YOUSHAYouSitCdSeq { get; set; }
        public string YOUSHASAKIRyakuNm { get; set; }
        public string YOUSHASAKISITENRyakuNm { get; set; }
        public string YOUSHAKATARyakuNm { get; set; }
        public string CodeKbOTHER1 { get; set; }
        public string CodeKbOTHER2 { get; set; }
        public byte SYASYUKataKbn { get; set; }
        public string KATAKBRyakuNm { get; set; }
        public int YYKSHOSeiEigCdSeq { get; set; }
        public string YYKSHOSeiTaiYmd { get; set; }

        public short GYOUSYAGyosyaCd { get; set; }
        public string GYOUSYAGyosyaNm { get; set; }
        public short YOUSHASAKITokuiCd { get; set; }
        public short YOUSHASAKISITENSitenCd { get; set; }
    }

    public class HaiShaDataTable
    {
        public int Index { get; set; }
        public int Row { get; set; }
        public string HAISHA_UkeNo { get; set; }
        public int YYKSHO_UkeCD { get; set; }
        public short HAISHA_UnkRen { get; set; }
        public short HAISHA_SyaSyuRen { get; set; }
        public short HAISHA_TeiDanNo { get; set; }
        public short HAISHA_BunkRen { get; set; }
        public string HAISHA_GoSya { get; set; }
        public int HAISHA_SyuEigCdSeq { get; set; }
        public int HAISHA_HaiSSryCdSeq { get; set; }
        public string HAISHA_DanTaNm2 { get; set; }
        public int HAISHA_IkMapCdSeq { get; set; }
        public string HAISHA_IkNm { get; set; }
        public string HAISHA_SyuKoYmd { get; set; }
        public string HAISHA_SyuKoTime { get; set; }
        public string HAISHA_SyuPaTime { get; set; }
        public string HAISHA_HaiSYmd { get; set; }
        public string HAISHA_HaiSTime { get; set; }
        public int HAISHA_HaiSCdSeq { get; set; }
        public string HAISHA_HaiSNm { get; set; }
        public string HAISHA_HaiSJyus1 { get; set; }
        public string HAISHA_HaiSJyus2 { get; set; }
        public string HAISHA_HaiSKigou { get; set; }
        public int HAISHA_HaiSKouKCdSeq { get; set; }
        public string HAISHA_HaiSKouKNm { get; set; }
        public int HAISHA_HaiSBinCdSeq { get; set; }
        public string HAISHA_HaisBinNm { get; set; }
        public string HAISHA_HaiSSetTime { get; set; }
        public string HAISHA_KikYmd { get; set; }
        public string HAISHA_KikTime { get; set; }
        public string HAISHA_TouYmd { get; set; }
        public string HAISHA_TouChTime { get; set; }
        public int HAISHA_TouCdSeq { get; set; }
        public string HAISHA_TouNm { get; set; }
        public string HAISHA_TouJyusyo1 { get; set; }
        public string HAISHA_TouJyusyo2 { get; set; }
        public string HAISHA_TouKigou { get; set; }
        public int HAISHA_TouKouKCdSeq { get; set; }
        public string HAISHA_TouSKouKNm { get; set; }
        public int HAISHA_TouBinCdSeq { get; set; }
        public string HAISHA_TouBinNm { get; set; }
        public string HAISHA_TouSetTime { get; set; }
        public short HAISHA_JyoSyaJin { get; set; }
        public short HAISHA_PlusJin { get; set; }
        public short HAISHA_DrvJin { get; set; }
        public short HAISHA_GuiSu { get; set; }
        public byte HAISHA_OthJinKbn1 { get; set; }
        public short HAISHA_OthJin1 { get; set; }
        public byte HAISHA_OthJinKbn2 { get; set; }
        public short HAISHA_OthJin2 { get; set; }
        public byte HAISHA_KSKbn { get; set; }
        public byte HAISHA_HaiSKbn { get; set; }
        public byte HAISHA_HaiIKbn { get; set; }
        public byte HAISHA_NippoKbn { get; set; }
        public int HAISHA_YouTblSeq { get; set; }
        public byte HAISHA_YouKataKbn { get; set; }
        public int HAISHA_SyaRyoUnc { get; set; }
        public int HAISHA_SyaRyoSyo { get; set; }
        public int HAISHA_SyaRyoUncSyaRyoSyo { get; set; }
        public int HAISHA_SyaRyoTes { get; set; }
        public int HAISHA_YoushaUnc { get; set; }
        public int HAISHA_YoushaSyo { get; set; }
        public int HAISHA_YoushaTes { get; set; }
        public string HAISHA_PlatNo { get; set; }
        public byte HAISHA_UkeJyKbnCd { get; set; }
        public short HAISHA_BunKSyuJyn { get; set; }
        public string UNKOBI_HaiSYmd { get; set; }
        public string UNKOBI_HaiSTime { get; set; }
        public string UNKOBI_TouYmd { get; set; }
        public string UNKOBI_TouChTime { get; set; }
        public string UNKOBI_DanTaNm { get; set; }
        public byte UNKOBI_KSKbn { get; set; }
        public byte UNKOBI_HaiSKbn { get; set; }
        public byte UNKOBI_HaiIKbn { get; set; }
        public byte UNKOBI_NippoKbn { get; set; }
        public byte UNKOBI_YouKbn { get; set; }
        public byte UNKOBI_UkeJyKbnCd { get; set; }
        public int YYKSHO_TokuiSeq { get; set; }
        public int YYKSHO_SitenCdSeq { get; set; }
        public string TOKISK_RyakuNm { get; set; }
        public string TOKIST_RyakuNm { get; set; }
        public int SYARYO_SyaRyoCd { get; set; }
        public string SYARYO_SyaRyoNm { get; set; }
        public string SYASYU_SyaSyuNm { get; set; }
        public string KAISHA_RyakuNm { get; set; }
        public string EIGYOS_RyakuNm { get; set; }
        public int YOUSHA_YouCdSeq { get; set; }
        public int YOUSHA_YouSitCdSeq { get; set; }
        public string YOUSHASAKI_RyakuNm { get; set; }
        public string YOUSHASAKISITEN_RyakuNm { get; set; }
        public string YOUSHAKATA_RyakuNm { get; set; }
        public string CodeKb_OTHER1 { get; set; }
        public string CodeKb_OTHER2 { get; set; }
        public byte SYASYU_KataKbn { get; set; }
        public string KATAKB_RyakuNm { get; set; }
        public int YYKSHO_SeiEigCdSeq { get; set; }
        public string YYKSHO_SeiTaiYmd { get; set; }

        public short GYOUSYA_GyosyaCd { get; set; }
        public string GYOUSYA_GyosyaNm { get; set; }
        public short YOUSHASAKI_TokuiCd { get; set; }
        public short YOUSHASAKISITEN_SitenCd { get; set; }

        public bool Checked { get; set; }
        public string StyleCSS { get; set; }
        public string ColorClass { get; set; }

        public string TextGoSya
        {
            get
            {
                return HAISHA_BunKSyuJyn > 0 ? String.Format("{0} - {1}	", HAISHA_GoSya, HAISHA_BunkRen) : HAISHA_GoSya;
            }
        }

        public string TextHaiSYmd_HaiSTime
        {
            get
            {
                return String.Format("{0}  {1}	", ParseStringToStringDateTime(HAISHA_HaiSYmd), ParseStringToStringTime(HAISHA_HaiSTime));
            }
        }
        public string TextHaiTouYmd_HaiTouTime
        {
            get
            {
                return String.Format("{0}  {1}	", ParseStringToStringDateTime(HAISHA_TouYmd), ParseStringToStringTime(HAISHA_TouChTime));

            }
        }
        public string TextHaiSyuKoYmd_HaiSyuKoTime
        {
            get
            {
                var result = HAISHA_YouTblSeq != 0 ? "" : String.Format("{0}  {1}	", ParseStringToStringDateTime(HAISHA_SyuKoYmd), ParseStringToStringTime(HAISHA_SyuKoTime));
                return result;
            }
        }
        public string TextHaiKikYmd_HaiKikTime
        {
            get
            {
                var result = HAISHA_YouTblSeq != 0 ? "" : String.Format("{0}  {1}	", ParseStringToStringDateTime(HAISHA_KikYmd), ParseStringToStringTime(HAISHA_KikTime));
                return result;

            }
        }

        public string TextHaiSSetTime
        {
            get
            {
                return String.Format("{0}", ParseStringToStringTime(HAISHA_HaiSSetTime));
            }
        }
        public string TextTouSetTime
        {
            get
            {
                return String.Format("{0}", ParseStringToStringTime(HAISHA_TouSetTime));
            }
        }
        public bool CheckHasYouSha { get; set; } = false;
        public string TextHaiSha_GoSya
        {
            get
            {
                return String.Format("{0:D4}", HAISHA_GoSya);
            }
        }
        public string ParseStringToStringDateTime(string value)
        {
            string result = "";
            if (!String.IsNullOrEmpty(value) && !String.IsNullOrWhiteSpace(value))
            {
                result = DateTime.ParseExact(value, "yyyyMMdd", new CultureInfo("ja-JP")).ToString("yyyy/MM/dd");
            }
            return result;
        }
        public string ParseStringToStringTime(string value)
        {
            string result = "";
            if (!String.IsNullOrEmpty(value) && !String.IsNullOrWhiteSpace(value))
            {
                result = DateTime.ParseExact(value, "HHmm", new CultureInfo("ja-JP")).ToString("HH:mm");
            }
            return result;
        }
        public string Empty { get; set; }
        public string HAISHAYoushaUnc_YoushaSyoText
        {
            get
            {
                return CommonUtil.CurencyFormat(HAISHA_YoushaUnc + HAISHA_YoushaSyo);
            }
        }
        public string HAISHA_SyaRyoUncSyaRyoSyoText
        {
            get
            {
                return CommonUtil.CurencyFormat(HAISHA_SyaRyoUncSyaRyoSyo);
            }
        }
        public string HAISHA_YoushaUncText
        {
            get
            {
                return CommonUtil.CurencyFormat(HAISHA_YoushaUnc);
            }
        }
        public string HAISHA_YoushaSyoText
        {
            get
            {
                return CommonUtil.CurencyFormat(HAISHA_YoushaSyo);
            }
        }
        public string HAISHA_YoushaTesText
        {
            get
            {
                return CommonUtil.CurencyFormat(HAISHA_YoushaTes);
            }
        }
        public string HAISHA_SyaRyoUncText
        {
            get
            {
                return CommonUtil.CurencyFormat(HAISHA_SyaRyoUnc);
            }
        }
        public string HAISHA_SyaRyoSyoText
        {
            get
            {
                return CommonUtil.CurencyFormat(HAISHA_SyaRyoSyo);
            }
        }
        public string HAISHA_SyaRyoTesText
        {
            get
            {
                return CommonUtil.CurencyFormat(HAISHA_SyaRyoTes);
            }
        }
        public string TextHaiSSetTime_HAISHA_HaiSKouKNm
        {
            get
            {
                // var result =  HAISHA_YouTblSeq == 0 ? "": TextHaiSSetTime + " " + HAISHA_HaisBinNm;
                return TextHaiSSetTime + " " + HAISHA_HaisBinNm;

            }
        }
        public string TextTouSetTime_HAISHA_TouSKouKNm
        {
            get
            {
                //var result = HAISHA_YouTblSeq == 0 ? "" : TextTouSetTime + " " + HAISHA_TouBinNm;
                return TextTouSetTime + " " + HAISHA_TouBinNm; 
            }
        }
        public string HAISHA_HaiSJyus1_HAISHA_HaiSJyus2
        {
            get
            {
                return HAISHA_HaiSJyus1 + " " + HAISHA_HaiSJyus2;
            }
        }
        public string HAISHA_TouJyusyo1_HAISHA_TouJyusyo2
        {
            get
            {
                return HAISHA_TouJyusyo1 + " " + HAISHA_TouJyusyo2;
            }
        }
        public string HAISHA_JyoSyaJinText
        {
            get
            {
                return HAISHA_JyoSyaJin + " 人";
            }
        }
        public string HAISHA_PlusJinText
        {
            get
            {
                return HAISHA_PlusJin + " 人";
            }
        }
        public string CodeKb_OTHER1_HAISHA_OthJin1
        {
            get
            {
                return CodeKb_OTHER1 + " " + HAISHA_OthJin1 + " 人";
            }
        }
        public string CodeKb_OTHER2_HAISHA_OthJin2
        {
            get
            {
                return CodeKb_OTHER2 + " " + HAISHA_OthJin2 + " 人";
            }
        }
        public string YOUSHAKATA_RyakuNmText
        {
            get
            {
                var result = HAISHA_YouTblSeq != 0 ? YOUSHAKATA_RyakuNm : "";
                return result;
            }
        }
        public string TOKISK_RyakuNmText
        {
            get
            {
                var result = HAISHA_YouTblSeq != 0 ? YOUSHASAKI_RyakuNm : "";
                return result;
            }
        }
        public string EIGYOS_RyakuNmText
        {
            get
            {
                var result = HAISHA_YouTblSeq != 0 ? "" : EIGYOS_RyakuNm;
                return result;
            }
        }

        public string SYASYU_SyaSyuNmText
        {
            get
            {
                var result = HAISHA_YouTblSeq != 0 ? "" : SYASYU_SyaSyuNm;
                return result;
            }
        }
        public string HAISHAYoushaUnc_YoushaSyoTextGrid
        {
            get
            {
                var result = HAISHA_YouTblSeq != 0 ? HAISHAYoushaUnc_YoushaSyoText : HAISHA_SyaRyoUncSyaRyoSyoText;
                return result;
            }
        }

        public string HAISHA_YoushaUncTextGrid
        {
            get
            {
                var result = HAISHA_YouTblSeq != 0 ? HAISHA_YoushaUncText : HAISHA_SyaRyoUncText;
                return result;
            }
        }
        public string HAISHA_YoushaSyoTextGrid
        {
            get
            {
                var result = HAISHA_YouTblSeq != 0 ? HAISHA_YoushaSyoText : HAISHA_SyaRyoSyoText;
                return result;
            }
        }
        public string HAISHA_YoushaTesTextGrid
        {
            get
            {
                var result = HAISHA_YouTblSeq != 0 ? HAISHA_YoushaTesText : HAISHA_SyaRyoTesText;
                return result;
            }
        }
        public string TOKIST_RyakuNmGrid
        {
            get
            {
                var result = HAISHA_YouTblSeq != 0 ? YOUSHASAKISITEN_RyakuNm : "";
                return result;
            }
        }
        public string SYARYO_SyaRyoNmGrid
        {
            get
            {
                var result = HAISHA_YouTblSeq != 0 ? "" : SYARYO_SyaRyoNm;
                return result;
            }
        }

    }
    public class YouShaDataTable
    {
        public int Index { get; set; }
        public int Row { get; set; }
        public string YOUSHA_UkeNo { get; set; }
        public short YOUSHA_UnkRen { get; set; }
        public int YOUSHA_YouTblSeq { get; set; }
        public int GYOSYA_GyosyaCdSeq { get; set; }
        public short GYOSYA_GyosyaCd { get; set; }
        public int YOUSHA_YouCdSeq { get; set; }
        public short YOUSHASAKI_TokuiCd { get; set; }
        public string YOUSHASAKI_RyakuNm { get; set; }
        public int YOUSHA_YouSitCdSeq { get; set; }
        public int YOUSHASAKISITEN_SitenCd { get; set; }
        public string YOUSHASAKISITEN_RyakuNm { get; set; }
        public int Sum_SyaSyuDai { get; set; }
        public int YOUSHA_SyaRyo_SyaRyoSyo { get; set; }
        public int YOUSHA_SyaRyoUnc { get; set; }
        public int YOUSHA_SyaRyoSyo { get; set; }
        public decimal YOUSHA_TesuRitu { get; set; }
        public int YOUSHA_SyaRyoTes { get; set; }
        public string YOUSHA_SihYotYmd { get; set; }
        public string Text
        {
            get
            {
                return String.Format("{0}-{1}-{2}", GYOSYA_GyosyaCd.ToString("D3"), YOUSHASAKI_TokuiCd.ToString("D4"), YOUSHASAKISITEN_SitenCd.ToString("D4"));
            }
        }
        public string TextSihYotYmd
        {
            get
            {
                return String.Format("{0}", DateTime.ParseExact(YOUSHA_SihYotYmd, "yyyyMMdd", new CultureInfo("ja-JP")).ToString("yyyy/MM/dd"));
            }
        }
        public string YOUSHA_SyaRyo_SyaRyoSyoText
        {
            get
            {
                return CommonUtil.CurencyFormat(YOUSHA_SyaRyo_SyaRyoSyo);               
            }
        }
        public string YOUSHA_SyaRyoUncText
        {
            get
            {
                return CommonUtil.CurencyFormat(YOUSHA_SyaRyoUnc);
            }
        }
        public string YOUSHA_SyaRyoSyoText
        {
            get
            {
                return CommonUtil.CurencyFormat(YOUSHA_SyaRyoSyo);
            }
        }
        public string YOUSHA_TesuRituText
        {
            get
            {
                return CommonUtil.CurencyFormat(YOUSHA_TesuRitu)+" %";
            }
        }
        public string YOUSHASyaRyoTes_TesuRituText
        {
            get
            {
                return CommonUtil.CurencyFormat(YOUSHA_SyaRyoTes+ YOUSHA_TesuRitu);
            }
        }
     

    }
    public class YyKSyuDataPopup
    {
        public int RowID { get; set; } = 0;
        public string YYKSYU_UkeNo { get; set; }
        public short YYKSYU_UnkRen { get; set; }
        public short YYKSYU_SyaSyuRen { get; set; }
        public int YYKSYU_SyaSyuCdSeq { get; set; }
        public byte YYKSYU_KataKbn { get; set; }
        public short YYKSYU_SyaSyuDai { get; set; }
        public int YYKSYU_SyaSyuTan { get; set; }
        public int YYKSYU_SyaRyoUnc { get; set; }
        public int YYKSYU_UnitBusPrice { get; set; }
        public string YYKSYU_SYASYU_SyaSyuNm { get; set; }
        public string YYKSYU_KATAKBN_CodeKbnNm { get; set; }
        public string YOUSYU_UkeNo { get; set; }
        public short YOUSYU_UnkRen { get; set; }
        public int YOUSYU_YouTblSeq { get; set; }
        public short YOUSYU_SyaSyuRen { get; set; }
        public byte YOUSYU_YouKataKbn { get; set; }
        public short YOUSYU_SyaSyuDai { get; set; }
        public int YOUSYU_SyaSyuTan { get; set; }
        public int YOUSYU_SyaRyoUnc { get; set; }

        public bool CheckChoose { get; set; } = false;
        public bool CheckDisable { get; set; } = true;
        public BusTypeDataPartner BusTypeDataPartner { get; set; } = new BusTypeDataPartner();
    }
    public class BusTypeDataPartner
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }
    public class TokistData
    {
        public int TOKISK_TokuiSeq { get; set; }
        public short TOKISK_TokuiCd { get; set; }
        public string TOKISK_RyakuNm { get; set; }
        public int TOKIST_SitenCdSeq { get; set; }
        public short TOKIST_SitenCd { get; set; }
        public string TOKIST_RyakuNm { get; set; }
        public decimal TOKIST_TesuRitu { get; set; }
        public string Text
        {
            get
            {
                return String.Format("{0} : {1} {2} : {3}", TOKISK_TokuiCd.ToString("D4"), TOKISK_RyakuNm, TOKIST_SitenCd.ToString("D4"), TOKIST_RyakuNm);
            }
        }
    }
    public class YouShaDataPopup
    {

        public string YOUSHA_UkeNo { get; set; }
        public short YOUSHA_UnkRen { get; set; }
        public int YOUSHA_YouTblSeq { get; set; }
        public int YOUSHA_YouCdSeq { get; set; }
        public int YOUSHA_YouSitCdSeq { get; set; }
        public string TOKISK_RyakuNm { get; set; }
        public string TOKIST_RyakuNm { get; set; }
        public int YOUSHA_SyaRyoUnc { get; set; }
        public byte YOUSHA_ZeiKbn { get; set; }
        public decimal YOUSHA_Zeiritsu { get; set; }
        public int YOUSHA_SyaRyoSyo { get; set; }
        public decimal YOUSHA_TesuRitu { get; set; }
        public int YOUSHA_SyaRyoTes { get; set; }
        public string YOUSHA_SihYotYmd { get; set; }
        public string YOUSHA_HasYmd { get; set; }
        public int Sum_MoneyAllShow { get; set; } = 0;
        public short GYOUSYA_GyosyaCd { get; set; }
        public string GYOUSYA_GyosyaNm { get; set; }
        public short TOKISK_TokuiCd { get; set; }
        public short TOKIST_SitenCd { get; set; }
        public int TOKIST_SitenCdSeq { get; set; }
        public int TOKISK_TokuiCdSeq { get; set; }
        public int GYOUSYA_GyosyaCdSeq { get; set; }
    }
    public class CodeKbnDataPopup
    {
        public string CodeKbn { get; set; }
        public string CodeKbnNm { get; set; }
    }
    public class YouShaDataInsert
    {
        public List<YyKSyuDataPopup> YyKSyuDataPopups { get; set; } = new List<YyKSyuDataPopup>();
        public BusTypeDataPartner BusTypeDataPartner { get; set; } = new BusTypeDataPartner();
        public TokistData TokistData { get; set; } = new TokistData();
        public YouShaDataPopup YouShaDataPopup { get; set; } = new YouShaDataPopup();
        public CodeKbnDataPopup CodeKbnDataPopup { get; set; } = new CodeKbnDataPopup();
        public List<HaiShaDataTable> HaiShaDataTableList { get; set; } = new List<HaiShaDataTable>();
        public int Sum_YYKSYU_SyaSyuDai { get; set; } = 0;
        public int Sum_YYKSYU_UnitBusPrice { get; set; } = 0;
        public int Sum_YOUSYU_SyaSyuDai { get; set; } = 0;
        public int Sum_YOUSYU_SyaSyuTan { get; set; } = 0;
        public DateTime InvoiceDate { set; get; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
        public List<TKM_KasSetData> TKM_KasSetDataList{ get; set; } = new List<TKM_KasSetData>();
    }
    public class VATDataPopup
    {
        public decimal VAT_HaiSha { get; set; } = 0;
        public decimal VAT_Customer { get; set; } = 0;
    }
    public class VPM_KyoSetData
    {
        public decimal Zeiritsu1 { get; set; } = 0;
        public string Zei1StaYmd { get; set; } = "";
        public string Zei1EndYmd { get; set; } = "";
        public decimal Zeiritsu2 { get; set; } = 0;
        public string Zei2StaYmd { get; set; } = "";
        public string Zei2EndYmd { get; set; } = "";
        public decimal Zeiritsu3 { get; set; } = 0;
        public string Zei3StaYmd { get; set; } = "";
        public string Zei3EndYmd { get; set; } = "";
    }
    public class TKM_KasSetData
    {
        public int CompanyCdSeq { get; set; }
        public int UriKbn { get; set; }
        public int SyohiHasu { get; set; }
        public int TesuHasu { get; set; }
    }
    public class CodeKbnBunruiDataPopup
    {
        public int HAICHI_BunruiCdSeq { get; set; }
        public int HAICHI_HaiSCdSeq { get; set; }
        public string HAICHI_HaiSCd { get; set; }
        public string HAICHI_RyakuNm { get; set; }
        public string HAICHI_Jyus1 { get; set; }
        public string HAICHI_Jyus2 { get; set; }
        public string HAICHI_HaiSKigou { get; set; }
        public string BUNRUI_CodeKbnNm { get; set; }
        public string Text
        {
            get
            {
                return String.Format("{0}  {1}:{2}", BUNRUI_CodeKbnNm, HAICHI_HaiSCd, HAICHI_RyakuNm);
            }
        }
    }
    public class VehicleDispatchPopup
    {
        public int KOUTU_BunruiCdSeq { get; set; }
        public int KOUTU_KoukCdSeq { get; set; }
        public int KOUTU_KoukCd { get; set; }
        public string KOUTU_KoukNm { get; set; }
        public int BIN_BinCdSeq { get; set; }
        public int BIN_BinCd { get; set; }
        public string BUNRUI_CodeKbnNm { get; set; }
        public string BIN_BinNm { get; set; }
        public string BIN_SyuPaTime { get; set; }
        public string BIN_TouChTime { get; set; }
        public string Text
        {
            get
            {
                return String.Format("{0} {1}:{2}", BUNRUI_CodeKbnNm, KOUTU_KoukCd.ToString("D5"), KOUTU_KoukNm);
            }
        }
    }
    public class HaiShaDataUpdate
    {
        public string HAISHA_UkeNo { get; set; }
        public short HAISHA_UnkRen { get; set; }
        public short HAISHA_SyaSyuRen { get; set; }
        public short HAISHA_TeiDanNo { get; set; }
        public short HAISHA_BunkRen { get; set; }
        public string HaiSYmd { get; set; }
        public string HaiSTime { get; set; }
        public string SyuPaTime { get; set; }
        public string TouYmd { get; set; }
        public string TouChTime { get; set; }
        public string GoSya { get; set; }
        public string DataNm2 { get; set; }
        public CodeKbnBunruiDataPopup codeKbnBunruiDataPopupStart { get; set; } = new CodeKbnBunruiDataPopup();
        public string HAISHA_HaiSNm { get; set; }
        public string HAISHA_HaiSJyus1 { get; set; }
        public string HAISHA_HaiSJyus2 { get; set; }
        public string HAISHA_HaiSKigou { get; set; }
        public VehicleDispatchPopup vehicleDispatchPopupStart { get; set; } = new VehicleDispatchPopup();
        public string BIN_TouChTime { get; set; }
        public string BIN_BinNmStart { get; set; }
        public CodeKbnBunruiDataPopup codeKbnBunruiDataPopupEnd { get; set; } = new CodeKbnBunruiDataPopup();
        public string HAISHA_TouchName { get; set; }
        public string HAISHA_TouJyusyo1 { get; set; }
        public string HAISHA_TouJyusyo2 { get; set; }
        public string HAISHA_TouKigou { get; set; }
        public VehicleDispatchPopup vehicleDispatchPopupEnd { get; set; } = new VehicleDispatchPopup();
        public string BIN_SyuPaTime { get; set; }
        public string BIN_BinNmEnd { get; set; }
        public int HAISHA_DrvJin { get; set; }
        public int HAISHA_GuiSu { get; set; }
        public int HAISHA_JyoSyaJin { get; set; }
        public int HAISHA_PlusJin { get; set; }
        public TPM_CodeKbDataOTHJINKBN tPM_CodeKbDataOTHJINKBN01 { get; set; } = new TPM_CodeKbDataOTHJINKBN();
        public int OthJin1 { get; set; } = 0;
        public TPM_CodeKbDataOTHJINKBN tPM_CodeKbDataOTHJINKBN02 { get; set; } = new TPM_CodeKbDataOTHJINKBN();
        public int OthJin2 { get; set; } = 0;
        public TPM_CodeKbDataKenCD tPM_CodeKbDataKenCD { get; set; } = new TPM_CodeKbDataKenCD();
        public string HAISHA_IkNm { get; set; }
        public int YOUSHA_YouCdSeq { get; set; }
        public int YOUSHA_YouSitCdSeq { get; set; }
        public string YOUSHASAKI_RyakuNm { get; set; }
        public string YOUSHASAKISITEN_RyakuNm { get; set; }
        public short GYOUSYA_GyosyaCd { get; set; }
        public string GYOUSYA_GyosyaNm { get; set; }
        public short YOUSHASAKI_TokuiCd { get; set; }
        public short YOUSHASAKISITEN_SitenCd { get; set; }
        public string TextGyosya => $"{GYOUSYA_GyosyaCd:000} : {GYOUSYA_GyosyaNm}";
        public string TextYoushasaki => $"{YOUSHASAKI_TokuiCd:0000} : {YOUSHASAKI_RyakuNm}";
        public string TextYoushasakisiten => $"{YOUSHASAKISITEN_SitenCd:0000} : {YOUSHASAKISITEN_RyakuNm}";
        public string TextYouSha
        {
            get
            {
                return String.Format("{0}:{1} {2}:{3}", YOUSHA_YouCdSeq.ToString("D4"), YOUSHASAKI_RyakuNm, YOUSHA_YouSitCdSeq.ToString("D4"), YOUSHASAKISITEN_RyakuNm);
            }
        }
        public string TOKISK_RyakuNm { get; set; }
        public string TOKIST_RyakuNm { get; set; }
        public string TextTokisk
        {
            get
            {
                return String.Format("{0}{1} ", TOKISK_RyakuNm, TOKIST_RyakuNm);
            }
        }
        public TPM_CodeKbData tpm_CodeUKEJYKBNCDKbData { get; set; } = new TPM_CodeKbData();
        public string UNKOBI_HaiSYmd { get; set; } = "";
        public string UNKOBI_HaiSTime { get; set; } = "";
        public string UNKOBI_TouYmd { get; set; } = "";
        public string UNKOBI_TouChTime { get; set; } = "";
        public List<YouSyuTable> YouSyuTableList { get; set; } = new List<YouSyuTable>();

        public DateTime CheckHaiSHaiSha
        {
            get
            {
                return ParseStringToDateTime(HaiSYmd, HaiSTime);
            }
        }
        public DateTime CheckTouHaiSha
        {
            get
            {
                return ParseStringToDateTime(TouYmd, TouChTime);
            }
        }
        public DateTime CheckTouUnkobi;
        public DateTime CheckHaiSUnkobi;
        private DateTime ParseStringToDateTime(string dateTime, string Time)
        {
            DateTime result;
            DateTime.TryParseExact(dateTime + Time, "yyyyMMddHHmm", new CultureInfo("ja-JP"), DateTimeStyles.None, out result);
            return result;
        }
    }
    public class YouSyuTable
    {
        public string YYKSYU_UkeNo { get; set; }
        public short YYKSYU_UnkRen { get; set; }
        public short YYKSYU_SyaSyuRen { get; set; }
        public int YYKSYU_SyaSyuCdSeq { get; set; }
        public byte YYKSYU_KataKbn { get; set; }
        public short YYKSYU_SyaSyuDai { get; set; }
        public string YYKSYU_SYASYU_SyaSyuNm { get; set; }
        public string YYKSYU_KATAKBN_CodeKbnNm { get; set; }
    }

    public class ParterBookingInputHaita
    {
        public string UnkobiUpdYmdTime { get; set; }
        public string YoushaUpdYmdTime { get; set; }
        public string YouSyuUpdYmdTime { get; set; }
        public string HaishaUpdYmdTime { get;  set; }
        public string MihrimUpdYmdTime { get;  set; }
        public string YykshoUpdYmdTime { get;  set; }
        public string HaiinUpdYmdTime { get; set; }
    }
}

