using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using System;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class MonthlyTransportationFormSearch
    {
        public DateTime ProcessingDate { get; set; } = DateTime.Now;
        public CompanyItem Company { get; set; }
        public EigyoItem EigyoTo { get; set; }
        public EigyoItem EigyoFrom { get; set; }
        public ShippingItem ShippingFrom { get; set; }
        public ShippingItem ShippingTo { get; set; }
        public int OutputInstructionMode { get; set; }
    }

    public class AnnualTransportationRecordFormSearch
    {
        public CompanyItem Company { get; set; }
        public EigyoItem EigyoTo { get; set; }
        public EigyoItem EigyoFrom { get; set; }
        public ShippingItem ShippingFrom { get; set; }
        public ShippingItem ShippingTo { get; set; }
        public int OutputInstructionMode { get; set; }
        public DateTime ProcessingDateFrom { get; set; } = DateTime.Now;
        public DateTime ProcessingDateTo { get; set; } = DateTime.Now;
    }

    public class CommonListitems
    {
        public List<CompanyItem> Companys { get; set; }
        public List<EigyoItem> Eigyos { get; set; }
        public List<ShippingItem> Shippings { get; set; }
    }

    public class CompanyItem
    {
        public int TenantCdSeq { get; set; }
        public int CompanyCd { get; set; }
        public int CompanyCdSeq { get; set; }
        public string RyakuNm { get; set; }
        public string CompanyNm { get; set; }
        public string Text => CompanyCd > 0 ? $"{string.Format("{0:D5}", CompanyCd)}：{RyakuNm}" : "";
    }

    public class EigyoItem
    {
        public int EigyoCdSeq { get; set; }
        public int EigyoCd { get; set; }
        public string RyakuNm { get; set; }
        public string Text => EigyoCd > 0 ? $"{string.Format("{0:D5}", EigyoCd)}：{RyakuNm}" : "";
    }

    public class ShippingItem
    {
        public int CodeKbnSeq { get; set; }
        public int CodeKbn { get; set; }
        public string CodeKbnNm { get; set; }
        public string Text => CodeKbn > 0 ? $"{string.Format("{0:D2}", CodeKbn)}：{CodeKbnNm}" : "";
    }

    public class TKDJitHouResultInput
    {
        public byte UnsouKbn { get; set; }
        public string SyoriYm { get; set; }
        public int EigyoCdSeq { get; set; }
        public byte KataKbn { get; set; }
        public byte NenRyoKbn { get; set; }
        public int? NobeJyoCnt { get; set; }
        public int NobeRinCnt { get; set; }
        public int NobeSumCnt { get; set; }
        public int NobeJitCnt { get; set; }
        public decimal JitudoRitu { get; set; }
        public decimal RinjiRitu { get; set; }
        public decimal JitJisaKm { get; set; }
        public decimal JitKisoKm { get; set; }
        public int YusoJin { get; set; }
        public int UnkoCnt { get; set; }
        public int UnkoKikak1Cnt { get; set; }
        public int UnkoKikak2Cnt { get; set; }
        public int UnkoOthCnt { get; set; }
        public decimal UnsoSyu { get; set; }
        public decimal DayTotalKm { get; set; }
        public decimal DayYusoJin { get; set; }
        public decimal DayUnsoSyu { get; set; }
        public decimal DayJisaKm { get; set; }
        public decimal UnsoZaSyu { get; set; }
        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
        public int UpdSyainCd { get; set; }
        public string UpdPrgId { get; set; }
        public string EigCdSeq_EigyoCd { get; set; }
        public string EigCdSeq_EigyoNm { get; set; }
        public string EigCdSeq_RyakuNm { get; set; }
        public string KataKbn_CodeKbnNm { get; set; }
        public string KataKbn_RyakuNm { get; set; }
        public string NenryoKbn_CodeKbnNm { get; set; }
        public string NenryoKbn_RyakuNm { get; set; }
        public string UpdSyainCd_SyainCd { get; set; }
        public string UpdSyainCd_SyainNm { get; set; }
    }

    public class JitHouResult
    {
        public List<JitHouItem> JitHous { get; set; }
        public int RowCounts { get; set; }
    }

    public class JitHouItem
    {
        public byte UnsouKbn { get; set; }
        public string UnsouKbnNm { get; set; }
        public string UnsouKbnRyaku { get; set; }
        public int EigyoCd { get; set; }
        public string EigyoNm { get; set; }
        public string EigyoRyaku { get; set; }
        public byte KataKbn { get; set; }
        public string KataKbnNm { get; set; }
        public string KataKbnRyaku { get; set; }
        public int NobeJyoCnt { get; set; }
        public int NobeRinCnt { get; set; }
        public int NobeSumCnt { get; set; }
        public int NobeJitCnt { get; set; }
        public decimal JitJisaKm { get; set; }
        public decimal JitKisoKm { get; set; }
        public decimal JitSumKm { get; set; }
        public int YusoJin { get; set; }
        public int UnkoCnt { get; set; }
        public int UnkoKikak1Cnt { get; set; }
        public int UnkoKikak2Cnt { get; set; }
        public int UnkoOthCnt { get; set; }
        public int UnkoOthAllCnt { get; set; }
        public decimal UnsoSyu { get; set; } = 0;
        public string SyoriYm { get; set; }
    }

    public class JitHouReportItem
    {
        public string ProcessingDate { get; set; }
        public string ProcessingDateEnd { get; set; }
        public string Shipping { get; set; }
        public string CompanyName { get; set; }
        public string EigyoName { get; set; }
        public string Large_NobeJyoCnt { get; set; }
        public string Large_NobeRinCnt { get; set; }
        public string Large_NobeSumCnt { get; set; }
        public string Large_NobeJitCnt { get; set; }
        public string Large_JitudoRitu { get; set; }
        public string Large_RinjiZouRitu { get; set; }
        public string Large_SoukoKm_Jisya { get; set; }
        public string Large_SoukoKm_Kaiso { get; set; }
        public string Large_SoukoKm_Sum { get; set; }
        public string Large_YusouJin { get; set; }
        public string Large_UnkoCnt { get; set; }
        public string Large_UnkoOthAllCnt { get; set; }
        public string Large_UnsoSyu { get; set; }
        public string Large_DaySoukoKm { get; set; }
        public string Large_DayUnsoCnt { get; set; }
        public string Large_DayUnsoSyu { get; set; }
        public string Large_UnkoJisyaKm { get; set; }
        public string Medium_NobeJyoCnt { get; set; }
        public string Medium_NobeRinCnt { get; set; }
        public string Medium_NobeSumCnt { get; set; }
        public string Medium_NobeJitCnt { get; set; }
        public string Medium_JitudoRitu { get; set; }
        public string Medium_RinjiZouRitu { get; set; }
        public string Medium_SoukoKm_Jisya { get; set; }
        public string Medium_SoukoKm_Kaiso { get; set; }
        public string Medium_SoukoKm_Sum { get; set; }
        public string Medium_YusouJin { get; set; }
        public string Medium_UnkoCnt { get; set; }
        public string Medium_UnkoOthAllCnt { get; set; }
        public string Medium_UnsoSyu { get; set; }
        public string Medium_DaySoukoKm { get; set; }
        public string Medium_DayUnsoCnt { get; set; }
        public string Medium_DayUnsoSyu { get; set; }
        public string Medium_UnkoJisyaKm { get; set; }
        public string Small_NobeJyoCnt { get; set; }
        public string Small_NobeRinCnt { get; set; }
        public string Small_NobeSumCnt { get; set; }
        public string Small_NobeJitCnt { get; set; }
        public string Small_JitudoRitu { get; set; }
        public string Small_RinjiZouRitu { get; set; }
        public string Small_SoukoKm_Jisya { get; set; }
        public string Small_SoukoKm_Kaiso { get; set; }
        public string Small_SoukoKm_Sum { get; set; }
        public string Small_YusouJin { get; set; }
        public string Small_UnkoCnt { get; set; }
        public string Small_UnkoOthAllCnt { get; set; }
        public string Small_UnsoSyu { get; set; }
        public string Small_DaySoukoKm { get; set; }
        public string Small_DayUnsoCnt { get; set; }
        public string Small_DayUnsoSyu { get; set; }
        public string Small_UnkoJisyaKm { get; set; }
        public string Total_NobeJyoCnt => RecordNull ? "" : (Convert.ToDecimal(Large_NobeJyoCnt) + Convert.ToDecimal(Medium_NobeJyoCnt) + Convert.ToDecimal(Small_NobeJyoCnt)).ToString(FormatString.FormatNumber);
        public string Total_NobeRinCnt => RecordNull ? "" : (Convert.ToDecimal(Large_NobeRinCnt) + Convert.ToDecimal(Medium_NobeRinCnt) + Convert.ToDecimal(Small_NobeRinCnt)).ToString(FormatString.FormatNumber);
        public string Total_NobeSumCnt => RecordNull ? "" : (Convert.ToDecimal(Large_NobeSumCnt) + Convert.ToDecimal(Medium_NobeSumCnt) + Convert.ToDecimal(Small_NobeSumCnt)).ToString(FormatString.FormatNumber);
        public string Total_NobeJitCnt => RecordNull ? "" : (Convert.ToDecimal(Large_NobeJitCnt) + Convert.ToDecimal(Medium_NobeJitCnt) + Convert.ToDecimal(Small_NobeJitCnt)).ToString(FormatString.FormatNumber);
        public string Total_JitudoRitu => RecordNull ? "" : (Convert.ToDecimal(Total_NobeSumCnt) > 0 ? Convert.ToDecimal(Total_NobeJitCnt) / Convert.ToDecimal(Total_NobeSumCnt) * 100 : 0).ToString(FormatString.FormatDecimalOnePlace);
        public string Total_RinjiZouRitu => RecordNull ? "" : (Convert.ToDecimal(Total_NobeJitCnt) > 0 ? Convert.ToDecimal(Total_NobeRinCnt) / Convert.ToDecimal(Total_NobeJitCnt) * 100 : 0).ToString(FormatString.FormatDecimalOnePlace);
        public string Total_SoukoKm_Jisya => RecordNull ? "" : (Convert.ToDecimal(Large_SoukoKm_Jisya) + Convert.ToDecimal(Medium_SoukoKm_Jisya) + Convert.ToDecimal(Small_SoukoKm_Jisya)).ToString(FormatString.FormatDecimalTwoPlace);
        public string Total_SoukoKm_Kaiso => RecordNull ? "" : (Convert.ToDecimal(Large_SoukoKm_Kaiso) + Convert.ToDecimal(Medium_SoukoKm_Kaiso) + Convert.ToDecimal(Small_SoukoKm_Kaiso)).ToString(FormatString.FormatDecimalTwoPlace);
        public string Total_SoukoKm_Sum => RecordNull ? "" : (Convert.ToDecimal(Large_SoukoKm_Sum) + Convert.ToDecimal(Medium_SoukoKm_Sum) + Convert.ToDecimal(Small_SoukoKm_Sum)).ToString(FormatString.FormatDecimalTwoPlace);
        public string Total_YusouJin => RecordNull ? "" : (Convert.ToDecimal(Large_YusouJin) + Convert.ToDecimal(Medium_YusouJin) + Convert.ToDecimal(Small_YusouJin)).ToString(FormatString.FormatNumber);
        public string Total_UnkoCnt => RecordNull ? "" : (Convert.ToDecimal(Large_UnkoCnt) + Convert.ToDecimal(Medium_UnkoCnt) + Convert.ToDecimal(Small_UnkoCnt)).ToString(FormatString.FormatNumber);
        public string Total_UnkoOthAllCnt => RecordNull ? "" : (Convert.ToDecimal(Large_UnkoOthAllCnt) + Convert.ToDecimal(Medium_UnkoOthAllCnt) + Convert.ToDecimal(Small_UnkoOthAllCnt)).ToString(FormatString.FormatNumber);
        public string Total_UnsoSyu => RecordNull ? "" : (Convert.ToDecimal(Large_UnsoSyu) + Convert.ToDecimal(Medium_UnsoSyu) + Convert.ToDecimal(Small_UnsoSyu)).ToString(FormatString.FormatNumber);
        public string Total_DaySoukoKm => RecordNull ? "" : (Convert.ToDecimal(Total_NobeJitCnt) > 0 ? Convert.ToDecimal(Total_SoukoKm_Sum) / Convert.ToDecimal(Total_NobeJitCnt) : 0).ToString(FormatString.FormatDecimalOnePlace);
        public string Total_DayUnsoCnt => RecordNull ? "" : (Convert.ToDecimal(Total_NobeJitCnt) > 0 ? Convert.ToDecimal(Total_YusouJin) / Convert.ToDecimal(Total_NobeJitCnt) : 0).ToString(FormatString.FormatDecimalOnePlace);
        public string Total_DayUnsoSyu => RecordNull ? "" : (Convert.ToDecimal(Total_NobeJitCnt) > 0 ? Convert.ToDecimal(Total_UnsoSyu) / Convert.ToDecimal(Total_NobeJitCnt) : 0).ToString(FormatString.FormatNumber);
        public string Total_UnkoJisyaKm => RecordNull ? "" : (Convert.ToDecimal(Total_UnkoCnt) > 0 ? Convert.ToDecimal(Total_SoukoKm_Jisya) / Convert.ToDecimal(Total_UnkoCnt) : 0).ToString(FormatString.FormatDecimalOnePlace);
        public bool RecordNull { get; set; }
    }

    public class JitHouReports
    {
        public List<JitHouReportPerPage> jitHouReportPerPage { get; set; }
    }

    public class JitHouReportPerPage
    {
        public JitHouReportItem JitHouReportItem1 { get; set; }
        public JitHouReportItem JitHouReportItem2 { get; set; }
        public JitHouReportItem JitHouReportItem3 { get; set; }
        public JitHouReportItem JitHouReportItem4 { get; set; }
    }

    public class SearchParam
    {
        public string StrDate { get; set; }
        public string EndDate { get; set; }
        public int CompnyCd { get; set; }
        public int CompanyCdSeq { get; set; }
        public string CompanyName { get; set; }
        public int StrEigyoCd { get; set; }
        public int EndEigyoCd { get; set; }
        public int StrEigyoCdSeq { get; set; }
        public int EndEigyoCdSeq { get; set; }
        public int StrUnsouKbn { get; set; }
        public int EndUnsouKbn { get; set; }
        public int TenantCdSeq { get; set; }
    }

    public class MonthlyTransportationModel
    {
        //「輸送実績報告書1」タブ
        public int? LargeOil_NobeJyoCnt { get; set; }
        public int? LargeOil_NobeRinCnt { get; set; }
        public int? LargeOil_NobeSumCnt { get; set; }
        public int? LargeOil_NobeJitCnt { get; set; }
        public decimal? LargeOil_JitudoRitu { get; set; }
        public decimal? LargeOil_RinjiRitu { get; set; }
        public decimal? LargeOil_JitJisaKm { get; set; }
        public decimal? LargeOil_JitKisoKm { get; set; }
        public decimal? LargeOil_KmKei { get; set; }
        public int? LargeOil_YusoJin { get; set; }
        public int? LargeGasoline_NobeJyoCnt { get; set; }
        public int? LargeGasoline_NobeRinCnt { get; set; }
        public int? LargeGasoline_NobeSumCnt { get; set; }
        public int? LargeGasoline_NobeJitCnt { get; set; }
        public decimal? LargeGasoline_JitudoRitu { get; set; }
        public decimal? LargeGasoline_RinjiRitu { get; set; }
        public decimal? LargeGasoline_JitJisaKm { get; set; }
        public decimal? LargeGasoline_JitKisoKm { get; set; }
        public decimal? LargeGasoline_KmKei { get; set; }
        public int? LargeGasoline_YusoJin { get; set; }
        public int? LargeLPG_NobeJyoCnt { get; set; }
        public int? LargeLPG_NobeRinCnt { get; set; }
        public int? LargeLPG_NobeSumCnt { get; set; }
        public int? LargeLPG_NobeJitCnt { get; set; }
        public decimal? LargeLPG_JitudoRitu { get; set; }
        public decimal? LargeLPG_RinjiRitu { get; set; }
        public decimal? LargeLPG_JitJisaKm { get; set; }
        public decimal? LargeLPG_JitKisoKm { get; set; }
        public decimal? LargeLPG_KmKei { get; set; }
        public int? LargeLPG_YusoJin { get; set; }
        public int? LargeGasTurbine_NobeJyoCnt { get; set; }
        public int? LargeGasTurbine_NobeRinCnt { get; set; }
        public int? LargeGasTurbine_NobeSumCnt { get; set; }
        public int? LargeGasTurbine_NobeJitCnt { get; set; }
        public decimal? LargeGasTurbine_JitudoRitu { get; set; }
        public decimal? LargeGasTurbine_RinjiRitu { get; set; }
        public decimal? LargeGasTurbine_JitJisaKm { get; set; }
        public decimal? LargeGasTurbine_JitKisoKm { get; set; }
        public decimal? LargeGasTurbine_KmKei { get; set; }
        public int? LargeGasTurbine_YusoJin { get; set; }
        public int? LargeOther_NobeJyoCnt { get; set; }
        public int? LargeOther_NobeRinCnt { get; set; }
        public int? LargeOther_NobeSumCnt { get; set; }
        public int? LargeOther_NobeJitCnt { get; set; }
        public decimal? LargeOther_JitudoRitu { get; set; }
        public decimal? LargeOther_RinjiRitu { get; set; }
        public decimal? LargeOther_JitJisaKm { get; set; }
        public decimal? LargeOther_JitKisoKm { get; set; }
        public decimal? LargeOther_KmKei { get; set; }
        public int? LargeOther_YusoJin { get; set; }
        public int? MediumOil_NobeJyoCnt { get; set; }
        public int? MediumOil_NobeRinCnt { get; set; }
        public int? MediumOil_NobeSumCnt { get; set; }
        public int? MediumOil_NobeJitCnt { get; set; }
        public decimal? MediumOil_JitudoRitu { get; set; }
        public decimal? MediumOil_RinjiRitu { get; set; }
        public decimal? MediumOil_JitJisaKm { get; set; }
        public decimal? MediumOil_JitKisoKm { get; set; }
        public decimal? MediumOil_KmKei { get; set; }
        public int? MediumOil_YusoJin { get; set; }
        public int? MediumGasoline_NobeJyoCnt { get; set; }
        public int? MediumGasoline_NobeRinCnt { get; set; }
        public int? MediumGasoline_NobeSumCnt { get; set; }
        public int? MediumGasoline_NobeJitCnt { get; set; }
        public decimal? MediumGasoline_JitudoRitu { get; set; }
        public decimal? MediumGasoline_RinjiRitu { get; set; }
        public decimal? MediumGasoline_JitJisaKm { get; set; }
        public decimal? MediumGasoline_JitKisoKm { get; set; }
        public decimal? MediumGasoline_KmKei { get; set; }
        public int? MediumGasoline_YusoJin { get; set; }
        public int? MediumLPG_NobeJyoCnt { get; set; }
        public int? MediumLPG_NobeRinCnt { get; set; }
        public int? MediumLPG_NobeSumCnt { get; set; }
        public int? MediumLPG_NobeJitCnt { get; set; }
        public decimal? MediumLPG_JitudoRitu { get; set; }
        public decimal? MediumLPG_RinjiRitu { get; set; }
        public decimal? MediumLPG_JitJisaKm { get; set; }
        public decimal? MediumLPG_JitKisoKm { get; set; }
        public decimal? MediumLPG_KmKei { get; set; }
        public int? MediumLPG_YusoJin { get; set; }
        public int? MediumGasTurbine_NobeJyoCnt { get; set; }
        public int? MediumGasTurbine_NobeRinCnt { get; set; }
        public int? MediumGasTurbine_NobeSumCnt { get; set; }
        public int? MediumGasTurbine_NobeJitCnt { get; set; }
        public decimal? MediumGasTurbine_JitudoRitu { get; set; }
        public decimal? MediumGasTurbine_RinjiRitu { get; set; }
        public decimal? MediumGasTurbine_JitJisaKm { get; set; }
        public decimal? MediumGasTurbine_JitKisoKm { get; set; }
        public decimal? MediumGasTurbine_KmKei { get; set; }
        public int? MediumGasTurbine_YusoJin { get; set; }
        public int? MediumOther_NobeJyoCnt { get; set; }
        public int? MediumOther_NobeRinCnt { get; set; }
        public int? MediumOther_NobeSumCnt { get; set; }
        public int? MediumOther_NobeJitCnt { get; set; }
        public decimal? MediumOther_JitudoRitu { get; set; }
        public decimal? MediumOther_RinjiRitu { get; set; }
        public decimal? MediumOther_JitJisaKm { get; set; }
        public decimal? MediumOther_JitKisoKm { get; set; }
        public decimal? MediumOther_KmKei { get; set; }
        public int? MediumOther_YusoJin { get; set; }
        public int? SmallOil_NobeJyoCnt { get; set; }
        public int? SmallOil_NobeRinCnt { get; set; }
        public int? SmallOil_NobeSumCnt { get; set; }
        public int? SmallOil_NobeJitCnt { get; set; }
        public decimal? SmallOil_JitudoRitu { get; set; }
        public decimal? SmallOil_RinjiRitu { get; set; }
        public decimal? SmallOil_JitJisaKm { get; set; }
        public decimal? SmallOil_JitKisoKm { get; set; }
        public decimal? SmallOil_KmKei { get; set; }
        public int? SmallOil_YusoJin { get; set; }
        public int? SmallGasoline_NobeJyoCnt { get; set; }
        public int? SmallGasoline_NobeRinCnt { get; set; }
        public int? SmallGasoline_NobeSumCnt { get; set; }
        public int? SmallGasoline_NobeJitCnt { get; set; }
        public decimal? SmallGasoline_JitudoRitu { get; set; }
        public decimal? SmallGasoline_RinjiRitu { get; set; }
        public decimal? SmallGasoline_JitJisaKm { get; set; }
        public decimal? SmallGasoline_JitKisoKm { get; set; }
        public decimal? SmallGasoline_KmKei { get; set; }
        public int? SmallGasoline_YusoJin { get; set; }
        public int? SmallLPG_NobeJyoCnt { get; set; }
        public int? SmallLPG_NobeRinCnt { get; set; }
        public int? SmallLPG_NobeSumCnt { get; set; }
        public int? SmallLPG_NobeJitCnt { get; set; }
        public decimal? SmallLPG_JitudoRitu { get; set; }
        public decimal? SmallLPG_RinjiRitu { get; set; }
        public decimal? SmallLPG_JitJisaKm { get; set; }
        public decimal? SmallLPG_JitKisoKm { get; set; }
        public decimal? SmallLPG_KmKei { get; set; }
        public int? SmallLPG_YusoJin { get; set; }
        public int? SmallGasTurbine_NobeJyoCnt { get; set; }
        public int? SmallGasTurbine_NobeRinCnt { get; set; }
        public int? SmallGasTurbine_NobeSumCnt { get; set; }
        public int? SmallGasTurbine_NobeJitCnt { get; set; }
        public decimal? SmallGasTurbine_JitudoRitu { get; set; }
        public decimal? SmallGasTurbine_RinjiRitu { get; set; }
        public decimal? SmallGasTurbine_JitJisaKm { get; set; }
        public decimal? SmallGasTurbine_JitKisoKm { get; set; }
        public decimal? SmallGasTurbine_KmKei { get; set; }
        public int? SmallGasTurbine_YusoJin { get; set; }
        public int? SmallOther_NobeJyoCnt { get; set; }
        public int? SmallOther_NobeRinCnt { get; set; }
        public int? SmallOther_NobeSumCnt { get; set; }
        public int? SmallOther_NobeJitCnt { get; set; }
        public decimal? SmallOther_JitudoRitu { get; set; }
        public decimal? SmallOther_RinjiRitu { get; set; }
        public decimal? SmallOther_JitJisaKm { get; set; }
        public decimal? SmallOther_JitKisoKm { get; set; }
        public decimal? SmallOther_KmKei { get; set; }
        public int? SmallOther_YusoJin { get; set; }
        public int? TotalOil_NobeJyoCnt => CommonUtil.CheckNullAndSum(LargeOil_NobeJyoCnt, MediumOil_NobeJyoCnt, SmallOil_NobeJyoCnt);
        public int? TotalOil_NobeRinCnt => CommonUtil.CheckNullAndSum(LargeOil_NobeRinCnt, MediumOil_NobeRinCnt, SmallOil_NobeRinCnt);
        public int? TotalOil_NobeSumCnt => CommonUtil.CheckNullAndSum(LargeOil_NobeSumCnt, MediumOil_NobeSumCnt, SmallOil_NobeSumCnt);
        public int? TotalOil_NobeJitCnt => CommonUtil.CheckNullAndSum(LargeOil_NobeJitCnt, MediumOil_NobeJitCnt, SmallOil_NobeJitCnt);
        public decimal? TotalOil_JitudoRitu => CommonUtil.CheckNullAndPercentage(CommonUtil.CheckNullAndSum(LargeOil_NobeJitCnt, MediumOil_NobeJitCnt, SmallOil_NobeJitCnt),
                                                                                CommonUtil.CheckNullAndSum(LargeOil_NobeSumCnt, MediumOil_NobeSumCnt, SmallOil_NobeSumCnt));
        public decimal? TotalOil_RinjiRitu => CommonUtil.CheckNullAndPercentage(CommonUtil.CheckNullAndSum(LargeOil_NobeRinCnt, MediumOil_NobeRinCnt, SmallOil_NobeRinCnt),
                                                                              CommonUtil.CheckNullAndSum(LargeOil_NobeJitCnt, MediumOil_NobeJitCnt, SmallOil_NobeJitCnt));
        public decimal? TotalOil_JitJisaKm => CommonUtil.CheckNullAndSum(LargeOil_JitJisaKm, MediumOil_JitJisaKm, SmallOil_JitJisaKm);
        public decimal? TotalOil_JitKisoKm => CommonUtil.CheckNullAndSum(LargeOil_JitKisoKm, MediumOil_JitKisoKm, SmallOil_JitKisoKm);
        public decimal? TotalOil_KmKei => CommonUtil.CheckNullAndSum(LargeOil_KmKei, MediumOil_KmKei, SmallOil_KmKei);
        public int? TotalOil_YusoJin => CommonUtil.CheckNullAndSum(LargeOil_YusoJin, MediumOil_YusoJin, SmallOil_YusoJin);
        public int? TotalGasoline_NobeJyoCnt => CommonUtil.CheckNullAndSum(LargeGasoline_NobeJyoCnt, MediumGasoline_NobeJyoCnt, SmallGasoline_NobeJyoCnt);
        public int? TotalGasoline_NobeRinCnt => CommonUtil.CheckNullAndSum(LargeGasoline_NobeRinCnt, MediumGasoline_NobeRinCnt, SmallGasoline_NobeRinCnt);
        public int? TotalGasoline_NobeSumCnt => CommonUtil.CheckNullAndSum(LargeGasoline_NobeSumCnt, MediumGasoline_NobeSumCnt, SmallGasoline_NobeSumCnt);
        public int? TotalGasoline_NobeJitCnt => CommonUtil.CheckNullAndSum(LargeGasoline_NobeJitCnt, MediumGasoline_NobeJitCnt, SmallGasoline_NobeJitCnt);
        public decimal? TotalGasoline_JitudoRitu => CommonUtil.CheckNullAndPercentage(CommonUtil.CheckNullAndSum(LargeGasoline_NobeJitCnt, MediumGasoline_NobeJitCnt, SmallGasoline_NobeJitCnt),
                                                                                CommonUtil.CheckNullAndSum(LargeGasoline_NobeSumCnt, MediumGasoline_NobeSumCnt, SmallGasoline_NobeSumCnt));
        public decimal? TotalGasoline_RinjiRitu => CommonUtil.CheckNullAndPercentage(CommonUtil.CheckNullAndSum(LargeGasoline_NobeRinCnt, MediumGasoline_NobeRinCnt, SmallGasoline_NobeRinCnt),
                                                                              CommonUtil.CheckNullAndSum(LargeGasoline_NobeJitCnt, MediumGasoline_NobeJitCnt, SmallGasoline_NobeJitCnt));
        public decimal? TotalGasoline_JitJisaKm => CommonUtil.CheckNullAndSum(LargeGasoline_JitJisaKm, MediumGasoline_JitJisaKm, SmallGasoline_JitJisaKm);
        public decimal? TotalGasoline_JitKisoKm => CommonUtil.CheckNullAndSum(LargeGasoline_JitKisoKm, MediumGasoline_JitKisoKm, SmallGasoline_JitKisoKm);
        public decimal? TotalGasoline_KmKei => CommonUtil.CheckNullAndSum(LargeGasoline_KmKei, MediumGasoline_KmKei, SmallGasoline_KmKei);
        public int? TotalGasoline_YusoJin => CommonUtil.CheckNullAndSum(LargeGasoline_YusoJin, MediumGasoline_YusoJin, SmallGasoline_YusoJin);
        public int? TotalLPG_NobeJyoCnt => CommonUtil.CheckNullAndSum(LargeLPG_NobeJyoCnt, MediumLPG_NobeJyoCnt, SmallLPG_NobeJyoCnt);
        public int? TotalLPG_NobeRinCnt => CommonUtil.CheckNullAndSum(LargeLPG_NobeRinCnt, MediumLPG_NobeRinCnt, SmallLPG_NobeRinCnt);
        public int? TotalLPG_NobeSumCnt => CommonUtil.CheckNullAndSum(LargeLPG_NobeSumCnt, MediumLPG_NobeSumCnt, SmallLPG_NobeSumCnt);
        public int? TotalLPG_NobeJitCnt => CommonUtil.CheckNullAndSum(LargeLPG_NobeJitCnt, MediumLPG_NobeJitCnt, SmallLPG_NobeJitCnt);
        public decimal? TotalLPG_JitudoRitu => CommonUtil.CheckNullAndPercentage(CommonUtil.CheckNullAndSum(LargeLPG_NobeJitCnt, MediumLPG_NobeJitCnt, SmallLPG_NobeJitCnt),
                                                                                 CommonUtil.CheckNullAndSum(LargeLPG_NobeSumCnt, MediumLPG_NobeSumCnt, SmallLPG_NobeSumCnt));
        public decimal? TotalLPG_RinjiRitu => CommonUtil.CheckNullAndPercentage(CommonUtil.CheckNullAndSum(LargeLPG_NobeRinCnt, MediumLPG_NobeRinCnt, SmallLPG_NobeRinCnt),
                                                                              CommonUtil.CheckNullAndSum(LargeLPG_NobeJitCnt, MediumLPG_NobeJitCnt, SmallLPG_NobeJitCnt));
        public decimal? TotalLPG_JitJisaKm => CommonUtil.CheckNullAndSum(LargeLPG_JitJisaKm, MediumLPG_JitJisaKm, SmallLPG_JitJisaKm);
        public decimal? TotalLPG_JitKisoKm => CommonUtil.CheckNullAndSum(LargeLPG_JitKisoKm, MediumLPG_JitKisoKm, SmallLPG_JitKisoKm);
        public decimal? TotalLPG_KmKei => CommonUtil.CheckNullAndSum(LargeLPG_KmKei, MediumLPG_KmKei, SmallLPG_KmKei);
        public int? TotalLPG_YusoJin => CommonUtil.CheckNullAndSum(LargeLPG_YusoJin, MediumLPG_YusoJin, SmallLPG_YusoJin);
        public int? TotalGasTurbine_NobeJyoCnt => CommonUtil.CheckNullAndSum(LargeGasTurbine_NobeJyoCnt, MediumGasTurbine_NobeJyoCnt, SmallGasTurbine_NobeJyoCnt);
        public int? TotalGasTurbine_NobeRinCnt => CommonUtil.CheckNullAndSum(LargeGasTurbine_NobeRinCnt, MediumGasTurbine_NobeRinCnt, SmallGasTurbine_NobeRinCnt);
        public int? TotalGasTurbine_NobeSumCnt => CommonUtil.CheckNullAndSum(LargeGasTurbine_NobeSumCnt, MediumGasTurbine_NobeSumCnt, SmallGasTurbine_NobeSumCnt);
        public int? TotalGasTurbine_NobeJitCnt => CommonUtil.CheckNullAndSum(LargeGasTurbine_NobeJitCnt, MediumGasTurbine_NobeJitCnt, SmallGasTurbine_NobeJitCnt);
        public decimal? TotalGasTurbine_JitudoRitu => CommonUtil.CheckNullAndPercentage(CommonUtil.CheckNullAndSum(LargeGasTurbine_NobeJitCnt, MediumGasTurbine_NobeJitCnt, SmallGasTurbine_NobeJitCnt),
                                                                                        CommonUtil.CheckNullAndSum(LargeGasTurbine_NobeSumCnt, MediumGasTurbine_NobeSumCnt, SmallGasTurbine_NobeSumCnt));
        public decimal? TotalGasTurbine_RinjiRitu => CommonUtil.CheckNullAndPercentage(CommonUtil.CheckNullAndSum(LargeGasTurbine_NobeRinCnt, MediumGasTurbine_NobeRinCnt, SmallGasTurbine_NobeRinCnt),
                                                                                   CommonUtil.CheckNullAndSum(LargeGasTurbine_NobeJitCnt, MediumGasTurbine_NobeJitCnt, SmallGasTurbine_NobeJitCnt));
        public decimal? TotalGasTurbine_JitJisaKm => CommonUtil.CheckNullAndSum(LargeGasTurbine_JitJisaKm, MediumGasTurbine_JitJisaKm, SmallGasTurbine_JitJisaKm);
        public decimal? TotalGasTurbine_JitKisoKm => CommonUtil.CheckNullAndSum(LargeGasTurbine_JitKisoKm, MediumGasTurbine_JitKisoKm, SmallGasTurbine_JitKisoKm);
        public decimal? TotalGasTurbine_KmKei => CommonUtil.CheckNullAndSum(LargeGasTurbine_KmKei, MediumGasTurbine_KmKei, SmallGasTurbine_KmKei);
        public int? TotalGasTurbine_YusoJin => CommonUtil.CheckNullAndSum(LargeGasTurbine_YusoJin, MediumGasTurbine_YusoJin, SmallGasTurbine_YusoJin);
        public int? TotalOther_NobeJyoCnt => CommonUtil.CheckNullAndSum(LargeOther_NobeJyoCnt, MediumOther_NobeJyoCnt, SmallOther_NobeJyoCnt);
        public int? TotalOther_NobeRinCnt => CommonUtil.CheckNullAndSum(LargeOther_NobeRinCnt, MediumOther_NobeRinCnt, SmallOther_NobeRinCnt);
        public int? TotalOther_NobeSumCnt => CommonUtil.CheckNullAndSum(LargeOther_NobeSumCnt, MediumOther_NobeSumCnt, SmallOther_NobeSumCnt);
        public int? TotalOther_NobeJitCnt => CommonUtil.CheckNullAndSum(LargeOther_NobeJitCnt, MediumOther_NobeJitCnt, SmallOther_NobeJitCnt);
        public decimal? TotalOther_JitudoRitu => CommonUtil.CheckNullAndPercentage(CommonUtil.CheckNullAndSum(LargeOther_NobeJitCnt, MediumOther_NobeJitCnt, SmallOther_NobeJitCnt),
                                                                                   CommonUtil.CheckNullAndSum(LargeOther_NobeSumCnt, MediumOther_NobeSumCnt, SmallOther_NobeSumCnt));
        public decimal? TotalOther_RinjiRitu => CommonUtil.CheckNullAndPercentage(CommonUtil.CheckNullAndSum(LargeOther_NobeRinCnt, MediumOther_NobeRinCnt, SmallOther_NobeRinCnt),
                                                                                   CommonUtil.CheckNullAndSum(LargeOther_NobeJitCnt, MediumOther_NobeJitCnt, SmallOther_NobeJitCnt));
        public decimal? TotalOther_JitJisaKm => CommonUtil.CheckNullAndSum(LargeOther_JitJisaKm, MediumOther_JitJisaKm, SmallOther_JitJisaKm);
        public decimal? TotalOther_JitKisoKm => CommonUtil.CheckNullAndSum(LargeOther_JitKisoKm, MediumOther_JitKisoKm, SmallOther_JitKisoKm);
        public decimal? TotalOther_KmKei => CommonUtil.CheckNullAndSum(LargeOther_KmKei, MediumOther_KmKei, SmallOther_KmKei);
        public int? TotalOther_YusoJin => CommonUtil.CheckNullAndSum(LargeOther_YusoJin, MediumOther_YusoJin, SmallOther_YusoJin);

        //「輸送実績報告書2」タブ
        public int? LargeOil_UnkoCnt { get; set; }
        public int? LargeOil_UnkoKikak1Cnt { get; set; }
        public int? LargeOil_UnkoKikak2Cnt { get; set; }
        public int? LargeOil_UnkoOthCnt { get; set; }
        public int? LargeOil_UnsoSyu { get; set; }
        public decimal? LargeOil_DayTotalKm { get; set; }
        public decimal? LargeOil_DayYusoJin { get; set; }
        public decimal? LargeOil_DayUnsoSyu { get; set; }
        public decimal? LargeOil_DayJisaKm { get; set; }
        public int? LargeOil_UnsoZaSyu { get; set; }
        public int? LargeGasoline_UnkoCnt { get; set; }
        public int? LargeGasoline_UnkoKikak1Cnt { get; set; }
        public int? LargeGasoline_UnkoKikak2Cnt { get; set; }
        public int? LargeGasoline_UnkoOthCnt { get; set; }
        public int? LargeGasoline_UnsoSyu { get; set; }
        public decimal? LargeGasoline_DayTotalKm { get; set; }
        public decimal? LargeGasoline_DayYusoJin { get; set; }
        public decimal? LargeGasoline_DayUnsoSyu { get; set; }
        public decimal? LargeGasoline_DayJisaKm { get; set; }
        public int? LargeGasoline_UnsoZaSyu { get; set; }
        public int? LargeLPG_UnkoCnt { get; set; }
        public int? LargeLPG_UnkoKikak1Cnt { get; set; }
        public int? LargeLPG_UnkoKikak2Cnt { get; set; }
        public int? LargeLPG_UnkoOthCnt { get; set; }
        public int? LargeLPG_UnsoSyu { get; set; }
        public decimal? LargeLPG_DayTotalKm { get; set; }
        public decimal? LargeLPG_DayYusoJin { get; set; }
        public decimal? LargeLPG_DayUnsoSyu { get; set; }
        public decimal? LargeLPG_DayJisaKm { get; set; }
        public int? LargeLPG_UnsoZaSyu { get; set; }
        public int? LargeGasTurbine_UnkoCnt { get; set; }
        public int? LargeGasTurbine_UnkoKikak1Cnt { get; set; }
        public int? LargeGasTurbine_UnkoKikak2Cnt { get; set; }
        public int? LargeGasTurbine_UnkoOthCnt { get; set; }
        public int? LargeGasTurbine_UnsoSyu { get; set; }
        public decimal? LargeGasTurbine_DayTotalKm { get; set; }
        public decimal? LargeGasTurbine_DayYusoJin { get; set; }
        public decimal? LargeGasTurbine_DayUnsoSyu { get; set; }
        public decimal? LargeGasTurbine_DayJisaKm { get; set; }
        public int? LargeGasTurbine_UnsoZaSyu { get; set; }
        public int? LargeOther_UnkoCnt { get; set; }
        public int? LargeOther_UnkoKikak1Cnt { get; set; }
        public int? LargeOther_UnkoKikak2Cnt { get; set; }
        public int? LargeOther_UnkoOthCnt { get; set; }
        public int? LargeOther_UnsoSyu { get; set; }
        public decimal? LargeOther_DayTotalKm { get; set; }
        public decimal? LargeOther_DayYusoJin { get; set; }
        public decimal? LargeOther_DayUnsoSyu { get; set; }
        public decimal? LargeOther_DayJisaKm { get; set; }
        public int? LargeOther_UnsoZaSyu { get; set; }
        public int? MediumOil_UnkoCnt { get; set; }
        public int? MediumOil_UnkoKikak1Cnt { get; set; }
        public int? MediumOil_UnkoKikak2Cnt { get; set; }
        public int? MediumOil_UnkoOthCnt { get; set; }
        public int? MediumOil_UnsoSyu { get; set; }
        public decimal? MediumOil_DayTotalKm { get; set; }
        public decimal? MediumOil_DayYusoJin { get; set; }
        public decimal? MediumOil_DayUnsoSyu { get; set; }
        public decimal? MediumOil_DayJisaKm { get; set; }
        public int? MediumOil_UnsoZaSyu { get; set; }
        public int? MediumGasoline_UnkoCnt { get; set; }
        public int? MediumGasoline_UnkoKikak1Cnt { get; set; }
        public int? MediumGasoline_UnkoKikak2Cnt { get; set; }
        public int? MediumGasoline_UnkoOthCnt { get; set; }
        public int? MediumGasoline_UnsoSyu { get; set; }
        public decimal? MediumGasoline_DayTotalKm { get; set; }
        public decimal? MediumGasoline_DayYusoJin { get; set; }
        public decimal? MediumGasoline_DayUnsoSyu { get; set; }
        public decimal? MediumGasoline_DayJisaKm { get; set; }
        public int? MediumGasoline_UnsoZaSyu { get; set; }
        public int? MediumLPG_UnkoCnt { get; set; }
        public int? MediumLPG_UnkoKikak1Cnt { get; set; }
        public int? MediumLPG_UnkoKikak2Cnt { get; set; }
        public int? MediumLPG_UnkoOthCnt { get; set; }
        public int? MediumLPG_UnsoSyu { get; set; }
        public decimal? MediumLPG_DayTotalKm { get; set; }
        public decimal? MediumLPG_DayYusoJin { get; set; }
        public decimal? MediumLPG_DayUnsoSyu { get; set; }
        public decimal? MediumLPG_DayJisaKm { get; set; }
        public int? MediumLPG_UnsoZaSyu { get; set; }
        public int? MediumGasTurbine_UnkoCnt { get; set; }
        public int? MediumGasTurbine_UnkoKikak1Cnt { get; set; }
        public int? MediumGasTurbine_UnkoKikak2Cnt { get; set; }
        public int? MediumGasTurbine_UnkoOthCnt { get; set; }
        public int? MediumGasTurbine_UnsoSyu { get; set; }
        public decimal? MediumGasTurbine_DayTotalKm { get; set; }
        public decimal? MediumGasTurbine_DayYusoJin { get; set; }
        public decimal? MediumGasTurbine_DayUnsoSyu { get; set; }
        public decimal? MediumGasTurbine_DayJisaKm { get; set; }
        public int? MediumGasTurbine_UnsoZaSyu { get; set; }
        public int? MediumOther_UnkoCnt { get; set; }
        public int? MediumOther_UnkoKikak1Cnt { get; set; }
        public int? MediumOther_UnkoKikak2Cnt { get; set; }
        public int? MediumOther_UnkoOthCnt { get; set; }
        public int? MediumOther_UnsoSyu { get; set; }
        public decimal? MediumOther_DayTotalKm { get; set; }
        public decimal? MediumOther_DayYusoJin { get; set; }
        public decimal? MediumOther_DayUnsoSyu { get; set; }
        public decimal? MediumOther_DayJisaKm { get; set; }
        public int? MediumOther_UnsoZaSyu { get; set; }
        public int? SmallOil_UnkoCnt { get; set; }
        public int? SmallOil_UnkoKikak1Cnt { get; set; }
        public int? SmallOil_UnkoKikak2Cnt { get; set; }
        public int? SmallOil_UnkoOthCnt { get; set; }
        public int? SmallOil_UnsoSyu { get; set; }
        public decimal? SmallOil_DayTotalKm { get; set; }
        public decimal? SmallOil_DayYusoJin { get; set; }
        public decimal? SmallOil_DayUnsoSyu { get; set; }
        public decimal? SmallOil_DayJisaKm { get; set; }
        public int? SmallOil_UnsoZaSyu { get; set; }
        public int? SmallGasoline_UnkoCnt { get; set; }
        public int? SmallGasoline_UnkoKikak1Cnt { get; set; }
        public int? SmallGasoline_UnkoKikak2Cnt { get; set; }
        public int? SmallGasoline_UnkoOthCnt { get; set; }
        public int? SmallGasoline_UnsoSyu { get; set; }
        public decimal? SmallGasoline_DayTotalKm { get; set; }
        public decimal? SmallGasoline_DayYusoJin { get; set; }
        public decimal? SmallGasoline_DayUnsoSyu { get; set; }
        public decimal? SmallGasoline_DayJisaKm { get; set; }
        public int? SmallGasoline_UnsoZaSyu { get; set; }
        public int? SmallLPG_UnkoCnt { get; set; }
        public int? SmallLPG_UnkoKikak1Cnt { get; set; }
        public int? SmallLPG_UnkoKikak2Cnt { get; set; }
        public int? SmallLPG_UnkoOthCnt { get; set; }
        public int? SmallLPG_UnsoSyu { get; set; }
        public decimal? SmallLPG_DayTotalKm { get; set; }
        public decimal? SmallLPG_DayYusoJin { get; set; }
        public decimal? SmallLPG_DayUnsoSyu { get; set; }
        public decimal? SmallLPG_DayJisaKm { get; set; }
        public int? SmallLPG_UnsoZaSyu { get; set; }
        public int? SmallGasTurbine_UnkoCnt { get; set; }
        public int? SmallGasTurbine_UnkoKikak1Cnt { get; set; }
        public int? SmallGasTurbine_UnkoKikak2Cnt { get; set; }
        public int? SmallGasTurbine_UnkoOthCnt { get; set; }
        public int? SmallGasTurbine_UnsoSyu { get; set; }
        public decimal? SmallGasTurbine_DayTotalKm { get; set; }
        public decimal? SmallGasTurbine_DayYusoJin { get; set; }
        public decimal? SmallGasTurbine_DayUnsoSyu { get; set; }
        public decimal? SmallGasTurbine_DayJisaKm { get; set; }
        public int? SmallGasTurbine_UnsoZaSyu { get; set; }
        public int? SmallOther_UnkoCnt { get; set; }
        public int? SmallOther_UnkoKikak1Cnt { get; set; }
        public int? SmallOther_UnkoKikak2Cnt { get; set; }
        public int? SmallOther_UnkoOthCnt { get; set; }
        public int? SmallOther_UnsoSyu { get; set; }
        public decimal? SmallOther_DayTotalKm { get; set; }
        public decimal? SmallOther_DayYusoJin { get; set; }
        public decimal? SmallOther_DayUnsoSyu { get; set; }
        public decimal? SmallOther_DayJisaKm { get; set; }
        public int? SmallOther_UnsoZaSyu { get; set; }
        public int? TotalOil_UnkoCnt => CommonUtil.CheckNullAndSum(LargeOil_UnkoCnt, MediumOil_UnkoCnt, SmallOil_UnkoCnt);
        public int? TotalOil_UnkoKikak1Cnt => CommonUtil.CheckNullAndSum(LargeOil_UnkoKikak1Cnt, MediumOil_UnkoKikak1Cnt, SmallOil_UnkoKikak1Cnt);
        public int? TotalOil_UnkoKikak2Cnt => CommonUtil.CheckNullAndSum(LargeOil_UnkoKikak2Cnt, MediumOil_UnkoKikak2Cnt, SmallOil_UnkoKikak2Cnt);
        public int? TotalOil_UnkoOthCnt => CommonUtil.CheckNullAndSum(LargeOil_UnkoOthCnt, MediumOil_UnkoOthCnt, SmallOil_UnkoOthCnt);
        public int? TotalOil_UnsoSyu => CommonUtil.CheckNullAndSum(LargeOil_UnsoSyu, MediumOil_UnsoSyu, SmallOil_UnsoSyu);
        public decimal? TotalOil_DayTotalKm => CommonUtil.CheckNullAndCaculate(TotalOil_KmKei, TotalOil_NobeJitCnt);
        public decimal? TotalOil_DayYusoJin => CommonUtil.CheckNullAndCaculate(TotalOil_YusoJin, TotalOil_NobeJitCnt);
        public decimal? TotalOil_DayUnsoSyu => CommonUtil.CheckNullAndCaculate(TotalOil_UnsoSyu, TotalOil_NobeJitCnt);
        public decimal? TotalOil_DayJisaKm => CommonUtil.CheckNullAndCaculate(TotalOil_JitJisaKm, TotalOil_UnkoCnt);
        public int? TotalOil_UnsoZaSyu => CommonUtil.CheckNullAndSum(LargeOil_UnsoZaSyu, MediumOil_UnsoZaSyu, SmallOil_UnsoZaSyu);
        public int? TotalGasoline_UnkoCnt => CommonUtil.CheckNullAndSum(LargeGasoline_UnkoCnt, MediumGasoline_UnkoCnt, SmallGasoline_UnkoCnt);
        public int? TotalGasoline_UnkoKikak1Cnt => CommonUtil.CheckNullAndSum(LargeGasoline_UnkoKikak1Cnt, MediumGasoline_UnkoKikak1Cnt, SmallGasoline_UnkoKikak1Cnt);
        public int? TotalGasoline_UnkoKikak2Cnt => CommonUtil.CheckNullAndSum(LargeGasoline_UnkoKikak2Cnt, MediumGasoline_UnkoKikak2Cnt, SmallGasoline_UnkoKikak2Cnt);
        public int? TotalGasoline_UnkoOthCnt => CommonUtil.CheckNullAndSum(LargeGasoline_UnkoOthCnt, MediumGasoline_UnkoOthCnt, SmallGasoline_UnkoOthCnt);
        public int? TotalGasoline_UnsoSyu => CommonUtil.CheckNullAndSum(LargeGasoline_UnsoSyu, MediumGasoline_UnsoSyu, SmallGasoline_UnsoSyu);
        public decimal? TotalGasoline_DayTotalKm => CommonUtil.CheckNullAndCaculate(TotalGasoline_KmKei, TotalGasoline_NobeJitCnt);
        public decimal? TotalGasoline_DayYusoJin => CommonUtil.CheckNullAndCaculate(TotalGasoline_YusoJin, TotalGasoline_NobeJitCnt);
        public decimal? TotalGasoline_DayUnsoSyu => CommonUtil.CheckNullAndCaculate(TotalGasoline_UnsoSyu, TotalGasoline_NobeJitCnt);
        public decimal? TotalGasoline_DayJisaKm => CommonUtil.CheckNullAndCaculate(TotalGasoline_JitJisaKm, TotalGasoline_UnkoCnt);
        public int? TotalGasoline_UnsoZaSyu => CommonUtil.CheckNullAndSum(LargeGasoline_UnsoZaSyu, MediumGasoline_UnsoZaSyu, SmallGasoline_UnsoZaSyu);
        public int? TotalLPG_UnkoCnt => CommonUtil.CheckNullAndSum(LargeLPG_UnkoCnt, MediumLPG_UnkoCnt, SmallLPG_UnkoCnt);
        public int? TotalLPG_UnkoKikak1Cnt => CommonUtil.CheckNullAndSum(LargeLPG_UnkoKikak1Cnt, MediumLPG_UnkoKikak1Cnt, SmallLPG_UnkoKikak1Cnt);
        public int? TotalLPG_UnkoKikak2Cnt => CommonUtil.CheckNullAndSum(LargeLPG_UnkoKikak2Cnt, MediumLPG_UnkoKikak2Cnt, SmallLPG_UnkoKikak2Cnt);
        public int? TotalLPG_UnkoOthCnt => CommonUtil.CheckNullAndSum(LargeLPG_UnkoOthCnt, MediumLPG_UnkoOthCnt, SmallLPG_UnkoOthCnt);
        public int? TotalLPG_UnsoSyu => CommonUtil.CheckNullAndSum(LargeLPG_UnsoSyu, MediumLPG_UnsoSyu, SmallLPG_UnsoSyu);
        public decimal? TotalLPG_DayTotalKm => CommonUtil.CheckNullAndCaculate(TotalLPG_KmKei, TotalLPG_NobeJitCnt);
        public decimal? TotalLPG_DayYusoJin => CommonUtil.CheckNullAndCaculate(TotalLPG_YusoJin, TotalLPG_NobeJitCnt);
        public decimal? TotalLPG_DayUnsoSyu => CommonUtil.CheckNullAndCaculate(TotalLPG_UnsoSyu, TotalLPG_NobeJitCnt);
        public decimal? TotalLPG_DayJisaKm => CommonUtil.CheckNullAndCaculate(TotalLPG_JitJisaKm, TotalLPG_UnkoCnt);
        public int? TotalLPG_UnsoZaSyu => CommonUtil.CheckNullAndSum(LargeLPG_UnsoZaSyu, MediumLPG_UnsoZaSyu, SmallLPG_UnsoZaSyu);
        public int? TotalGasTurbine_UnkoCnt => CommonUtil.CheckNullAndSum(LargeGasTurbine_UnkoCnt, MediumGasTurbine_UnkoCnt, SmallGasTurbine_UnkoCnt);
        public int? TotalGasTurbine_UnkoKikak1Cnt => CommonUtil.CheckNullAndSum(LargeGasTurbine_UnkoKikak1Cnt, MediumGasTurbine_UnkoKikak1Cnt, SmallGasTurbine_UnkoKikak1Cnt);
        public int? TotalGasTurbine_UnkoKikak2Cnt => CommonUtil.CheckNullAndSum(LargeGasTurbine_UnkoKikak2Cnt, MediumGasTurbine_UnkoKikak2Cnt, SmallGasTurbine_UnkoKikak2Cnt);
        public int? TotalGasTurbine_UnkoOthCnt => CommonUtil.CheckNullAndSum(LargeGasTurbine_UnkoOthCnt, MediumGasTurbine_UnkoOthCnt, SmallGasTurbine_UnkoOthCnt);
        public int? TotalGasTurbine_UnsoSyu => CommonUtil.CheckNullAndSum(LargeGasTurbine_UnsoSyu, MediumGasTurbine_UnsoSyu, SmallGasTurbine_UnsoSyu);
        public decimal? TotalGasTurbine_DayTotalKm => CommonUtil.CheckNullAndCaculate(TotalGasTurbine_KmKei, TotalGasTurbine_NobeJitCnt);
        public decimal? TotalGasTurbine_DayYusoJin => CommonUtil.CheckNullAndCaculate(TotalGasTurbine_YusoJin, TotalGasTurbine_NobeJitCnt);
        public decimal? TotalGasTurbine_DayUnsoSyu => CommonUtil.CheckNullAndCaculate(TotalGasTurbine_UnsoSyu, TotalGasTurbine_NobeJitCnt);
        public decimal? TotalGasTurbine_DayJisaKm => CommonUtil.CheckNullAndCaculate(TotalGasTurbine_JitJisaKm, TotalGasTurbine_UnkoCnt);
        public int? TotalGasTurbine_UnsoZaSyu => CommonUtil.CheckNullAndSum(LargeGasTurbine_UnsoZaSyu, MediumGasTurbine_UnsoZaSyu, SmallGasTurbine_UnsoZaSyu);
        public int? TotalOther_UnkoCnt => CommonUtil.CheckNullAndSum(LargeOther_UnkoCnt, MediumOther_UnkoCnt, SmallOther_UnkoCnt);
        public int? TotalOther_UnkoKikak1Cnt => CommonUtil.CheckNullAndSum(LargeOther_UnkoKikak1Cnt, MediumOther_UnkoKikak1Cnt, SmallOther_UnkoKikak1Cnt);
        public int? TotalOther_UnkoKikak2Cnt => CommonUtil.CheckNullAndSum(LargeOther_UnkoKikak2Cnt, MediumOther_UnkoKikak2Cnt, SmallOther_UnkoKikak2Cnt);
        public int? TotalOther_UnkoOthCnt => CommonUtil.CheckNullAndSum(LargeOther_UnkoOthCnt, MediumOther_UnkoOthCnt, SmallOther_UnkoOthCnt);
        public int? TotalOther_UnsoSyu => CommonUtil.CheckNullAndSum(LargeOther_UnsoSyu, MediumOther_UnsoSyu, SmallOther_UnsoSyu);
        public decimal? TotalOther_DayTotalKm => CommonUtil.CheckNullAndCaculate(TotalOther_KmKei, TotalOther_NobeJitCnt);
        public decimal? TotalOther_DayYusoJin => CommonUtil.CheckNullAndCaculate(TotalOther_YusoJin, TotalOther_NobeJitCnt);
        public decimal? TotalOther_DayUnsoSyu => CommonUtil.CheckNullAndCaculate(TotalOther_UnsoSyu, TotalOther_NobeJitCnt);
        public decimal? TotalOther_DayJisaKm => CommonUtil.CheckNullAndCaculate(TotalOther_JitJisaKm, TotalOther_UnkoCnt);
        public int? TotalOther_UnsoZaSyu => CommonUtil.CheckNullAndSum(LargeOther_UnsoZaSyu, MediumOther_UnsoZaSyu, SmallOther_UnsoZaSyu);
    }
}
