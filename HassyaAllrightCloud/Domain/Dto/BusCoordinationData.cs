using HassyaAllrightCloud.Commons.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class BusCoordinationSearchParam
    {
        public int DateType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        //public List<ReservationData> ReservationList { get; set; }
        public List<int> ReservationList { get; set; }
        // 得意先

        // New
        public CustomerComponentGyosyaData GyosyaTokuiSakiFrom { get; set; }
        public CustomerComponentGyosyaData GyosyaTokuiSakiTo { get; set; }
        public CustomerComponentTokiskData TokiskTokuiSakiFrom { get; set; }
        public CustomerComponentTokiskData TokiskTokuiSakiTo { get; set; }
        public CustomerComponentTokiStData TokiStTokuiSakiFrom { get; set; }
        public CustomerComponentTokiStData TokiStTokuiSakiTo { get; set; }

        // 仕入先

        // New
        public CustomerComponentGyosyaData GyosyaShiireSakiFrom { get; set; }
        public CustomerComponentGyosyaData GyosyaShiireSakiTo { get; set; }
        public CustomerComponentTokiskData TokiskShiireSakiFrom { get; set; }
        public CustomerComponentTokiskData TokiskShiireSakiTo { get; set; }
        public CustomerComponentTokiStData TokiStShiireSakiFrom { get; set; }
        public CustomerComponentTokiStData TokiStShiireSakiTo { get; set; }

        public LoadCustomerList CustomerStart { get; set; } = new LoadCustomerList();
        public LoadCustomerList CustomerEnd { get; set; } = new LoadCustomerList();
        public LoadCustomerList CustomerStart01 { get; set; } = new LoadCustomerList();
        public LoadCustomerList CustomerEnd01 { get; set; } = new LoadCustomerList();
        public LoadServiceOffice SaleBranch { get; set; } = new LoadServiceOffice();
        public string BookingFrom { get; set; } = "";
        public string BookingTo { get; set; } = "";
        public LoadStaffList Staff { get; set; } = new LoadStaffList();
        public LoadStaffList PersonInput { get; set; } = new LoadStaffList();
        public short UnkRen { get; set; } = 0;
        public LoadStaffList Staff01 { get; set; } = new LoadStaffList();
        public LoadStaffList PersonInput01 { get; set; } = new LoadStaffList();
        public OutputInstruction OutputSetting { get; set; }
        // 予約区分
        // New
        public ReservationClassComponentData YoyakuFrom { get; set; }
        public ReservationClassComponentData YoyakuTo { get; set; }
        public string UkenoList { get; set; }
        public int FormOutput { get; set; }
    }

    public class CurrentBusCoordination
    {
        public string UNKOBI_Ukeno { get; set; }
        public string UNKOBI_UnkRen { get; set; }
        public string UNKOBI_HaiSYmd { get; set; }
        public string UNKOBI_TouYmd { get; set; }
        public string SIRSK_TokuiNm { get; set; } = "";
        public string SIRST_SitenNm { get; set; } = "";
        public string UNKOBI_DanTaNm { get; set; } = "";
        public string UNKOBI_DanTaNm2 { get; set; } = "";
        public string TOKISK_TokuiNm { get; set; } = "";
        public string TOKIST_SitenNm { get; set; } = "";
        public string UNKOBI_KanjJyus1 { get; set; }
        public string UNKOBI_KanjTel { get; set; }
        public string YYKSHO_TokuiTel { get; set; }
        public string UNKOBI_KanjJyus2 { get; set; }
        public string UNKOBI_KanjNm { get; set; } = "";
        public string YYKSHO_TokuiTanNm { get; set; } = "";
        public string UNKOBI_HaiSNm { get; set; } = "";
        public string UNKOBI_HaiSJyus1 { get; set; }
        public string UNKOBI_HaiSJyus2 { get; set; }
        public string UNKOBI_HaiSTime { get; set; }
        public string HaiSKoukRyaku { get; set; }
        public string HaiSBinNm { get; set; }
        public string UNKOBI_HaiSSetTime { get; set; }
        public string UNKOBI_IkNm { get; set; }
        public string UNKOBI_SyuPaTime { get; set; }
        public string SJJOKBN1_CodeKbnNm { get; set; } = "";
        public string SJJOKBN2_CodeKbnNm { get; set; } ="";
        public string SJJOKBN3_CodeKbnNm { get; set; } = "";
        public string SJJOKBN4_CodeKbnNm { get; set; } = "";
        public string SJJOKBN5_CodeKbnNm { get; set; } = "";
        public string OTHERJIN1_CodeKbnNm { get; set; } = "";
        public string UNKOBI_OthJin1 { get; set; }
        public string OTHERJIN2_CodeKbnNm { get; set; } = "";
        public string UNKOBI_OthJin2 { get; set; }
        public string UNKOBI_JyoSyaJin { get; set; }
        public string UNKOBI_PlusJin { get; set; }
        public string UNKOBI_HaiSKouKNm { get; set; } = "";
        public string UNKOBI_HaisBinNm { get; set; } = "";
        public string UNKOBI_TouSKouKNm { get; set; } = "";
        public string UNKOBI_TouSBinNm { get; set; } = "";
        public string UNKOBI_TouSetTime { get; set; }
        public string UNKOBI_KikYmd { get; set; }
        public string  UNKOBI_SyuKoYmd { get; set; }
        public string UNKOBI_BikoNm { get; set; } = "";
        public string UNKOBI_TouNm { get; set; } = "";
        public string UNKOBI_TouJyusyo1 { get; set; } = "";
        public string UNKOBI_TouJyusyo2 { get; set; } = "";
        public string UNKOBI_TouChTime { get; set; } = "";
        public string YYKSHO_UntKin { get; set; } = "";
        public string ZEIKBN_CodeKbnNm { get; set; } = "";
        public string YYKSHO_ZeiRui { get; set; } = "";
        public string Bus_ZeiKomiKinGak { get; set; } = "";
        public string Bus_TesuRyoRitu { get; set; } = "";
        public string YYKSHO_TesuRyoG { get; set; } = "";
        public string YouSyaGak { get; set; } = "";
        public string YYKSHO_UkeCd { get; set; } = ""; 
        /*Procedure not respon */
        public string SijJoKNm01 { get; set; }
        public string SijJoKNm02 { get; set; }
        public string SijJoKNm03 { get; set; }
        public string SijJoKNm04 { get; set; }
        public string SijJoKNm05 { get; set; }
        public string UKEJOKEN_CodeKbnNm { get; set; }

        public string YYKSHO_UntKinShow
        {
            get
            {
                return FormatCurrency(YYKSHO_UntKin);
            }
        }
        public string Bus_ZeiKomiKinGakShow
        {
            get
            {
                return FormatCurrency(Bus_ZeiKomiKinGak);
            }
        }
        public string YYKSHO_ZeiRuiShow
        {
            get
            {
                return FormatCurrency(YYKSHO_ZeiRui);
            }
        }
        public string YYKSHO_TesuRyoGShow
        {
            get
            {
                return FormatCurrency(YYKSHO_TesuRyoG);
            }
        }
        public string YouSyaGakShow
        {
            get
            {
                return FormatCurrency(YouSyaGak);
            }
        }
        private string FormatCurrency(string value)
        {
            return CommonUtil.CurencyFormat(int.Parse(value));
        }

    }

    public class BusCoordinationReportPDF
    {
        public CurrentBusCoordination BusCoordination { get; set; } = new CurrentBusCoordination();
        public int PageNumber { get; set; }
        public int TotalPage { get; set; }
        public string Ukeno { get; set; }     
        public BusTypeShowReport BusTypeShowReport { get; set; } = new BusTypeShowReport();
        public YouShaDataShowReport YouShaShowReport { get; set; } = new YouShaDataShowReport();
        public FutTumDataShowReport FutTaiShowReport { get; set; } = new FutTumDataShowReport();
        public FutTumDataShowReport FutTumShowReport { get; set; } = new FutTumDataShowReport();
        public List<JourneysDataReport> JourneysShowReport { get; set; } = new List<JourneysDataReport>();
        public YykSyuDataShowReport YykSyuShowReport { get; set; } = new YykSyuDataShowReport();
    }
    public class BusTypeDataReport
    {
        public string YYKSYU_Ukeno { get; set; }
        public string YYKSYU_UnkRen { get; set; }
        public string YYKSYU_SyaSyuRen { get; set; }
        public string YYKSYU_SyaSyuCdSeq { get; set; }
        public string YYKSYU_SyaSyuDai { get; set; }
        public string SYASYU_SyaSyuNm { get; set; }
        public string YYKSYU_SyaSyuTan { get; set; }
        public string YYKSYU_SyaRyoUnc { get; set; }
    }
    public class YouShaDataReport
    {
        public string YOUSHA_UkeNo { get; set; }
        public string YOUSHA_UnkRen { get; set; }
        public string YOUSHA_YouCdSeq { get; set; }
        public string YOUSHA_YouSitCdSeq { get; set; }
        public string YOUSHA_Count { get; set; }
        public string YOUSHA_Nm { get; set; }

    }
    public class YouShaDataShowReport
    {
        public string YOUSHA_Nm01 { get; set; }
        public string YOUSHA_Nm02 { get; set; }
        public string YOUSHA_Nm03 { get; set; }
        public string YOUSHA_Nm04 { get; set; }
        public string YOUSHA_Nm05 { get; set; }
        public string YOUSHA_Nm06 { get; set; }
        public string YOUSHA_Count01 { get; set; }
        public string YOUSHA_Count02 { get; set; }
        public string YOUSHA_Count03 { get; set; }
        public string YOUSHA_Count04 { get; set; }
        public string YOUSHA_Count05 { get; set; }
        public string YOUSHA_Count06 { get; set; }

    }
    public class BusTypeShowReport
    {
        public string SYASYU_SyaSyuNm01 { get; set; }
        public string SYASYU_SyaSyuNm02 { get; set; }
        public string SYASYU_SyaSyuNm03 { get; set; }
        public string SYASYU_SyaSyuNm04 { get; set; }
        public string SYASYU_SyaSyuNm05 { get; set; }
        public string SYASYU_SyaSyuNm06 { get; set; }
        public string YYKSYU_SyaSyuDai01 { get; set; }
        public string YYKSYU_SyaSyuDai02 { get; set; }
        public string YYKSYU_SyaSyuDai03 { get; set; }
        public string YYKSYU_SyaSyuDai04 { get; set; }
        public string YYKSYU_SyaSyuDai05 { get; set; }
        public string YYKSYU_SyaSyuDai06 { get; set; }

    }
    public class FutTumDataReport
    {
        public string FUTTUM_UkeNo { get; set; }
        public string FUTTUM_UnkRen { get; set; }
        public string FUTTUM_FutTumKbn { get; set; }
        public string FUTTUM_FutTumRen { get; set; }
        public string FUTTUM_Nittei { get; set; }
        public string FUTTUM_FutTumCdSeq { get; set; }
        public string FUTTUM_FutTumNm { get; set; }
        public string FUTTUM_SeisanCdSeq { get; set; }
        public string FUTTUM_SeisanNm { get; set; }
        public string FUTTUM_Suryo { get; set; }
        public string FUTTUM_TanKa { get; set; }
        public string FUTTUM_UriGakKin { get; set; }
        public string FUTTUM_HasYmd { get; set; }

    }
    public class FutTumDataShowReport
    {
        public string FutaiDate01 { get; set; }
        public string FutaiNm01 { get; set; }
        public string FutaiSeisanNm01 { get; set; }
        public string FutSuLabel01 { get; set; }
        public string FutaiSu01 { get; set; }
        public string FutaiSetu01 { get; set; }
        public string FutTanka01 { get; set; }
        public string FutaiKin01 { get; set; }
        //
        public string FutaiDate02 { get; set; }
        public string FutaiNm02 { get; set; }
        public string FutaiSeisanNm02 { get; set; }
        public string FutSuLabel02 { get; set; }
        public string FutaiSu02 { get; set; }
        public string FutaiSetu02 { get; set; }
        public string FutTanka02 { get; set; }
        public string FutaiKin02 { get; set; }
        //
        public string FutaiDate03 { get; set; }
        public string FutaiNm03 { get; set; }
        public string FutaiSeisanNm03 { get; set; }
        public string FutSuLabel03 { get; set; }
        public string FutaiSu03 { get; set; }
        public string FutaiSetu03 { get; set; }
        public string FutTanka03 { get; set; }
        public string FutaiKin03 { get; set; }
        //
        public string FutaiDate04 { get; set; }
        public string FutaiNm04 { get; set; }
        public string FutaiSeisanNm04 { get; set; }
        public string FutSuLabel04 { get; set; }
        public string FutaiSu04 { get; set; }
        public string FutaiSetu04 { get; set; }
        public string FutTanka04 { get; set; }
        public string FutaiKin04 { get; set; }
        //
        public string FutaiDate05 { get; set; }
        public string FutaiNm05 { get; set; }
        public string FutaiSeisanNm05 { get; set; }
        public string FutSuLabel05 { get; set; }
        public string FutaiSu05 { get; set; }
        public string FutaiSetu05 { get; set; }
        public string FutTanka05 { get; set; }
        public string FutaiKin05 { get; set; }
    }
    public class KyoSetData
    {
        public string SijJoKNm01 { get; set; }
        public string SijJoKNm02 { get; set; }
        public string SijJoKNm03 { get; set; }
        public string SijJoKNm04 { get; set; }
        public string SijJoKNm05 { get; set; }
    }
    public class JourneysDataReport
    {
        public string DateKotei { get; set; } = "";
        public string Koutei { get; set; } = "";
        public string TehNm { get; set; } = "";
        public string TehTel { get; set; } = "";
        public string DateShow { get; set; } = "";
    }
    public class YykSyuDataReport
    {
        public string YYKSYU_UkeNo { get; set; }
        public string YYKSYU_UnkRen { get; set; }
        public string YYKSYU_SyaSyuRen { get; set; }
        public string YYKSYU_SyaSyuCdSeq { get; set; }
        public string YYKSYU_SyaSyuDai { get; set; }
        public string SYASYU_SyaSyuNm { get; set; }
        public string YYKSYU_SyaSyuTan { get; set; }
        public string YYKSYU_SyaRyoUnc { get; set; }
    }
    public class YykSyuDataShowReport
    {
        public string SpecialCharacters { get; set; } = "X";
        public string YYKSYU_SyaSyuTan01 { get; set; }    
        public string YYKSYU_SyaSyuDai01 { get; set; }
        public string YYKSYU_SyaRyoUnc01 { get; set; }
        public string YYKSYU_SyaSyuTan02 { get; set; }
        public string YYKSYU_SyaSyuDai02 { get; set; }
        public string YYKSYU_SyaRyoUnc02 { get; set; }
        public string YYKSYU_SyaSyuTan03 { get; set; }
        public string YYKSYU_SyaSyuDai03 { get; set; }
        public string YYKSYU_SyaRyoUnc03 { get; set; }
        public string YYKSYU_SyaSyuTan04 { get; set; }
        public string YYKSYU_SyaSyuDai04 { get; set; }
        public string YYKSYU_SyaRyoUnc04 { get; set; }
        public string YYKSYU_SyaSyuTan05 { get; set; }
        public string YYKSYU_SyaSyuDai05 { get; set; }
        public string YYKSYU_SyaRyoUnc05 { get; set; }
        public string YYKSYU_SyaSyuTan06 { get; set; }
        public string YYKSYU_SyaSyuDai06 { get; set; }
        public string YYKSYU_SyaRyoUnc06 { get; set; }

    }


}
