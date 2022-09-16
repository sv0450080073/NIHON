using HassyaAllrightCloud.Commons.Constants;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class TransportDailyReportData
    {
        public string EigyoCd { get; set; }
        public string EigyoNm { get; set; }
        public string EigyoRyakuNm { get; set; }
        public string SyaRyoCd { get; set; }
        public string SyaRyoNm { get; set; }
        public byte TeiCnt { get; set; }
        public string GyosyaCd { get; set; }
        public string TokuiCd { get; set; }
        public string SitenCd { get; set; }
        public string TokuiNm { get; set; }
        public string SitenNm { get; set; }
        public string Tokui_RyakuNm { get; set; }
        public string Siten_RyakuNm { get; set; }
        public string DanTaNm { get; set; }
        public string DanTaNm2 { get; set; }
        public string IkNm { get; set; }
        public string Hihaku { get; set; }
        public string SyuKoYmd { get; set; }
        public string SyuKoTime { get; set; }
        public string HaiSYmd { get; set; }
        public string HaiSTime { get; set; }
        public string TouYmd { get; set; }
        public string TouChTime { get; set; }
        public string KikYmd { get; set; }
        public string KikTime { get; set; }
        public int SyaRyoUnc { get; set; }
        public string Zeiritsu { get; set; }
        public int SyaRyoSyo { get; set; }
        public string TesuRitu { get; set; }
        public int SyaRyoTes { get; set; }
        public int TotalSyaRyo => SyaRyoSyo - SyaRyoTes;
        public string JyoSyaJin { get; set; }
        public string PlusJin { get; set; }
        public string Total_JisaIPKm { get; set; }
        public string Total_JisaKSKm { get; set; }
        public string Total_KisoIPKm { get; set; }
        public string Total_KisoKSKm { get; set; }
        public string Total_OthKm { get; set; }
        public string Total_TotalKm { get; set; }
        public string Nenryo1Cd { get; set; }
        public string Nenryo1Nm { get; set; }
        public string Nenryo1RyakuNm { get; set; }
        public string Nenryo1 { get; set; }
        public string Nenryo2Cd { get; set; }
        public string Nenryo2Nm { get; set; }
        public string Nenryo2RyakuNm { get; set; }
        public string Nenryo2 { get; set; }
        public string Nenryo3Cd { get; set; }
        public string Nenryo3Nm { get; set; }
        public string Nenryo3RyakuNm { get; set; }
        public string Nenryo3 { get; set; }
        public byte SyokumuKbn1 { get; set; }
        public string SyokumuNm1 { get; set; }
        public string SyainCd1 { get; set; }
        public string SyainNm1 { get; set; }
        public byte SyokumuKbn2 { get; set; }
        public string SyokumuNm2 { get; set; }
        public string SyainCd2 { get; set; }
        public string SyainNm2 { get; set; }
        public byte SyokumuKbn3 { get; set; }
        public string SyokumuNm3 { get; set; }
        public string SyainCd3 { get; set; }
        public string SyainNm3 { get; set; }
        public byte SyokumuKbn4 { get; set; }
        public string SyokumuNm4 { get; set; }
        public string SyainCd4 { get; set; }
        public string SyainNm4 { get; set; }
        public byte SyokumuKbn5 { get; set; }
        public string SyokumuNm5 { get; set; }
        public string SyainCd5 { get; set; }
        public string SyainNm5 { get; set; }
        public string UkeNo { get; set; }
        public short UnkRen { get; set; }
        public short TeiDanNo { get; set; }
        public short BunkRen { get; set; }
        public string KariSyaRyoNm { get; set; }
        public string YoyaKbnNm { get; set; }
        public byte TesKbn { get; set; }
        public string GyosyaNm { get; set; }
        public string StMeter { get; set; }
        public string EndMeter { get; set; }
        public string MonthTotal_JisaIPKm { get; set; }
        public string MonthTotal_JisaKSKm { get; set; }
        public string MonthTotal_KisoIPKm { get; set; }
        public string MonthTotal_KisoKSKm { get; set; }
        public string MonthTotal_OthKm { get; set; }
        public string MonthTotal_TotalKm { get; set; }
        public string SyainKariNm1 { get; set; }
        public string SyainKariNm2 { get; set; }
        public string SyainKariNm3 { get; set; }
        public string SyainKariNm4 { get; set; }
        public string SyainKariNm5 { get; set; }
        public string No { get; set; }

        public string DisplayDantaNm
        {
            get
            {
                return string.Format("{0} {1}", DanTaNm, DanTaNm2);
            }
        }

        public string DisplaySyuko
        {
            get
            {
                var displaySyuKo = string.Empty;
                if (!string.IsNullOrWhiteSpace(SyuKoYmd))
                {
                    var dayOfWeekSyuKo = DateTime.ParseExact(SyuKoYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture);
                    displaySyuKo = dayOfWeekSyuKo.ToString(CommonConstants.Format2YMDWithDOW);
                }
                return displaySyuKo;
            }
        }

        public string DisplayKik
        {
            get
            {
                var displayKik = string.Empty;
                if (!string.IsNullOrWhiteSpace(KikYmd))
                {
                    var dayOfWeekSyuKo = DateTime.ParseExact(KikYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture);
                    displayKik = dayOfWeekSyuKo.ToString(CommonConstants.Format2YMDWithDOW);
                }
                return displayKik;
            }
        }

        public string SumSyaRyo
        {
            get
            {
                return (SyaRyoUnc - SyaRyoTes).ToString("N0");
            }
        }

        public string SyainNm
        {
            get
            {
                return string.Format("{0} {1} {2} {3} {4}", SyainNm1, SyainNm2, SyainNm3, SyainNm4, SyainNm5);
            }
        }
    }

    public class TotalTransportDailyReportData : ICloneable
    {
        public int EigyoCd { get; set; }
        public string RyakuNm { get; set; }
        public string EigyoNm { get; set; }
        public string TotalActualSyaryo { get; set; }
        public string TotalWorkStock { get; set; }
        public string TotalWorkNight { get; set; }
        public string TempIncrease { get; set; }
        public string TotalDantaiHeadOffice { get; set; }
        public string TotalDantaiMediator { get; set; }
        public string TotalUnkoHeadOffice { get; set; }
        public string TotalUnkoMediator { get; set; }
        public string SumSyaRyoUnc { get; set; }
        public string SumSyaRyoTes { get; set; }
        public string SumJyoSyaJin { get; set; }
        public string SumPlusJin { get; set; }
        public string SumJisaIPKm { get; set; }
        public string SumJisaKSKm { get; set; }
        public string SumKisoIPKm { get; set; }
        public string SumKisoKOKm { get; set; }
        public string SumOthKm { get; set; }
        public string SumTotalKm { get; set; }
        public string SumNenryo1 { get; set; }
        public string SumNenryo2 { get; set; }
        public string SumNenryo3 { get; set; }
        public string CurMonthSyaRyoUnc { get; set; }
        public string CurMonthSyaRyoTes { get; set; }
        public string CurMonthJyoSyaJin { get; set; }
        public string CurMonthPlusJin { get; set; }
        public string CurMonthJisaIPKm { get; set; }
        public string CurMonthJisaKSKm { get; set; }
        public string CurMonthKisoIPKm { get; set; }
        public string CurMonthKisoKOKm { get; set; }
        public string CurMonthOthKm { get; set; }
        public string CurMonthTotalKm { get; set; }
        public string CurMonthNenryo1 { get; set; }
        public string CurMonthNenryo2 { get; set; }
        public string CurMonthNenryo3 { get; set; }
        public string Text1 { get; set; }
        public string Text2 { get; set; }

        public string SumSyaRyo
        {
            get
            {
                return (int.Parse(SumSyaRyoUnc) - int.Parse(SumSyaRyoTes)).ToString("N0");
            }
        }

        public string CurMonthSumSyaRyo
        {
            get
            {
                return (int.Parse(CurMonthSyaRyoUnc) - int.Parse(CurMonthSyaRyoTes)).ToString("N0");
            }
        }

        public string Total
        {
            get
            {
                return (decimal.Parse(TotalActualSyaryo) - decimal.Parse(TotalWorkStock) - decimal.Parse(TotalWorkNight) + decimal.Parse(TempIncrease)).ToString();
            }
        }

        public string TotalDantai
        {
            get
            {
                return (decimal.Parse(TotalDantaiHeadOffice) + decimal.Parse(TotalDantaiMediator)).ToString();
            }
        }

        public string TotalUnko
        {
            get
            {
                return (decimal.Parse(TotalUnkoHeadOffice) + decimal.Parse(TotalUnkoMediator)).ToString();
            }
        }

        // copy value of all properties into new object
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class TransportDailyReportSearchParams : ICloneable
    {
        public TransportDropDown pageSize { get; set; }
        public TransportDropDown csvHeader { get; set; }
        public TransportDropDown csvEnclose { get; set; }
        public TransportDropDown csvSpace { get; set; }
        public TransportDropDown aggregation { get; set; }
        public CompanySearchData selectedCompany { get; set; }
        public EigyoSearchData selectedEigyoFrom { get; set; }
        public EigyoSearchData selectedEigyoTo { get; set; }
        public EigyoSearchData selectedEigyo { get; set; }
        public DateTime selectedDate { get; set; } = DateTime.Now;
        public byte OutputSetting { get; set; }
        public byte OutputCategory { get; set; }
        public byte TotalType { get; set; }
        public byte fontSize { get; set; }

        public int CompanyCdSeq { get; set; } = new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID;
        public int TenantCdSeq { get; set; } = new ClaimModel().TenantID;

        // copy value of all properties into new object
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class CompanySearchData
    {
        public int TenantCdSeq { get; set; }
        public int CompanyCdSeq { get; set; }
        public int CompanyCd { get; set; }
        public string RyakuNm { get; set; }
        public string Text => $"{CompanyCd:00000} : {RyakuNm}";
    }

    public class EigyoSearchData
    {
        public int EigyoCdSeq { get; set; }
        public int EigyoCd { get; set; }
        public string RyakuNm { get; set; }
        public string Text => $"{EigyoCd:00000} : {RyakuNm}";
    }

    public class TransportDropDown
    {
        public string Text { get; set; }
        public byte Value { get; set; }
    }

    public class TotalData
    {
        public string SumJyoSyaJin { get; set; }
        public string SumSyaRyoUnc {get;set;}
        public string SumSyaRyoTes {get;set;}
        public string SumJisaIPKm {get;set;}
        public string SumJisaKSKm {get;set;}
        public string SumKisoIPKm {get;set;}
        public string SumKisoKOKm {get;set;}
        public string SumOthKm {get;set;}
        public string SumTotalKm {get;set;} 
        public string SumNenryo1 {get;set;} 
        public string SumNenryo2 {get;set;} 
        public string SumNenryo3 {get;set;}
        public string CurMonthSyaRyoUnc { get; set; }
        public string CurMonthSyaRyoTes { get; set; }
        public string CurMonthJyoSyaJin { get; set; }
        public string CurMonthPlusJin { get; set; }
        public string CurMonthJisaIPKm { get; set; }
        public string CurMonthJisaKSKm { get; set; }
        public string CurMonthKisoIPKm { get; set; }
        public string CurMonthKisoKOKm { get; set; }
        public string CurMonthOthKm { get; set; }
        public string CurMonthTotalKm { get; set; }
        public string CurMonthNenryo1 { get; set; }
        public string CurMonthNenryo2 { get; set; }
        public string CurMonthNenryo3 { get; set; }
    }

    public class TransportDailyReportPDF
    {
        public List<TransportDailyReportData> Data { get; set; }
        public TotalData TotalData { get; set; }
        public int Page { get; set; }
        public int TotalPage { get; set; }
        public string CurrentDate { get; set; }
        public string EigyoNm { get; set; }
        public string UnkoDate { get; set; }
        public string SyainCd { get; set; }
        public string SyainNm { get; set; }
        public string DisplayLabel { get; set; }
        public bool isDisplayTotal { get; set; } = false;
    }
}
