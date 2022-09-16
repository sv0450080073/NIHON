using System;
using System.Collections.Generic;
using System.Globalization;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Entities;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class RepairListData
    {
        public DateTime StartDate { get; set; } = DateTime.Today;
        public DateTime EndDate { get; set; } = DateTime.Today;
        public DepartureOfficeData BranchFrom { get; set; } = new DepartureOfficeData();
        public DepartureOfficeData BranchTo { get; set; } = new DepartureOfficeData();
        public TPM_SyaRyoData VehicleFrom { get; set; } = new TPM_SyaRyoData();
        public TPM_SyaRyoData VehicleTo { get; set; } = new TPM_SyaRyoData();
        public RepairDivision RepairFrom { get; set; } = new RepairDivision();
        public RepairDivision RepairTo { get; set; } = new RepairDivision();
        public RepairOutputOrderData OutputOrder { get; set; } = new RepairOutputOrderData();
        public PaperSize PaperSize { get; set; } = new PaperSize();
        public OutputInstruction OutputSetting { get; set; }
        public List<CompanyChartData> CompanyChartData { get; set; }
        public int TenantCdSeq { get; set; }
        public VpmSyain SyainNmItem { get; set; } = new VpmSyain();
    }
    public class RepairListDataUri
    {
        public DateTime StartDate { get; set; } = DateTime.Today;
        public DateTime EndDate { get; set; } = DateTime.Today;
        public DepartureOfficeData BranchFrom { get; set; } = new DepartureOfficeData();
        public DepartureOfficeData BranchTo { get; set; } = new DepartureOfficeData();
        public TPM_SyaRyoData VehicleFrom { get; set; } = new TPM_SyaRyoData();
        public TPM_SyaRyoData VehicleTo { get; set; } = new TPM_SyaRyoData();
        public RepairDivision RepairFrom { get; set; } = new RepairDivision();
        public RepairDivision RepairTo { get; set; } = new RepairDivision();
        public RepairOutputOrderData OutputOrder { get; set; } = new RepairOutputOrderData();
        public PaperSize PaperSize { get; set; } = new PaperSize();
        public OutputInstruction OutputSetting { get; set; }
        public string CompanyChartDataID { get; set; }
        public int TenantCdSeq { get; set; }
        public VpmSyain SyainNmItem { get; set; } = new VpmSyain();
        public string Uri { get; set; } = "";
    }
    public class RepairOutputOrderData
    {
        public int IdValue { get; set; }
        public string StringValue { get; set; }
    }

    public class RepairOutputOrderListData
    {
        public static List<RepairOutputOrderData> OutputOrderlst = new List<RepairOutputOrderData>
        {
            new RepairOutputOrderData { IdValue = 1, StringValue = "車輌順", },
            new RepairOutputOrderData { IdValue = 2, StringValue = "修理開始年月日"},
            new RepairOutputOrderData { IdValue = 3, StringValue = "修理終了年月日"},
        };
    }
    public class RepairListReportPDF
    {
        public HeaderReport HeaderReport { get; set; } = new HeaderReport();
        public List<CurrentRepairList> CurrentRepairList { get; set; } = new List<CurrentRepairList>();
        public int PageNumber { get; set; }
        public int TotalPage { get; set; }
    }

    public class HeaderReport
    {
        public string EigyoCdFrom { get; set; }
        public string EigyoCdTo { get; set; }
        public string SyaRyoCdFrom { get; set; }
        public string SyaRyoCdCdTo { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string OrderNm { get; set; }
        public string CurrentDateTime { get; set; }
        public string SyainCd { get; set; }
        public string SyainNm { get; set; }
        private string FormatStringDate(string dateTime, bool isTime = true)
        {
            if (!string.IsNullOrEmpty(dateTime) && !string.IsNullOrWhiteSpace(dateTime))
            {
                DateTime result;
                if (isTime)
                {

                    DateTime.TryParseExact(dateTime, "yyyyMMddHHmm", new CultureInfo("ja-JP"), DateTimeStyles.None, out result);
                    return result.ToString("yyyy/MM/dd   HH:mm");
                }
                else
                {

                    DateTime.TryParseExact(dateTime, "yyyyMMdd", new CultureInfo("ja-JP"), DateTimeStyles.None, out result);
                    return result.ToString("yyyy/MM/dd");
                }
            }
            return string.Empty;
        }
        public string DateFromFormat
        {
            get
            {
                return FormatStringDate(DateFrom, false);
            }
        }
        public string DateToFormat
        {
            get
            {
                return FormatStringDate(DateTo, false);
            }
        }
        public string DateCurrentFormat
        {
            get
            {
                return FormatStringDate(CurrentDateTime);
            }
        }

    }
    public class CurrentRepairList
    {
        public string RowNum { get; set; }
        public string ShuriSYmd { get; set; }
        public string ShuriSTime { get; set; }
        public string ShuriEYmd { get; set; }
        public string ShuriETime { get; set; }
        public string BikoNm { get; set; }
        public int SyaRyoCdSeq { get; set; }
        public int SyaRyoCd { get; set; }
        public string SyaRyoNm { get; set; }
        public string KariSyaRyoNm { get; set; }
        public int EigyoCdSeq { get; set; }
        public int EigyoCd { get; set; }
        public string EigyoNm { get; set; }
        public string RyakuNm { get; set; }
        public int RepairCdSeq { get; set; }
        public int RepairCd { get; set; }
        public string RepairNm { get; set; }
        public string RepairRyakuNm { get; set; }

        public string ShuriSYmdTimeFormat
        {
            get
            {
                return FormatString(ShuriSYmd, ShuriSTime);
            }
        }
        public string ShuriEYmdTimeFormat
        {
            get
            {
                return FormatString(ShuriEYmd, ShuriETime);
            }
        }
        private string FormatString(string dateTime, string Time)
        {
            if (!string.IsNullOrEmpty(dateTime) && !string.IsNullOrWhiteSpace(dateTime))
            {
                DateTime result;
                DateTime.TryParseExact(dateTime + Time, "yyyyMMddHHmm", new CultureInfo("ja-JP"), DateTimeStyles.None, out result);
                return result.ToString("yyyy/MM/dd (ddd) HH:mm");
            }
            return string.Empty;
        }
        public string EigyoCdText
        {
            get
            {
                return EigyoCd > 0 ? EigyoCd.ToString("D5") : "";
            }
        }
        public string SyaRyoCdText
        {
            get
            {
                return SyaRyoCd > 0 ? SyaRyoCd.ToString("D5") : "";
            }
        }
        public string RepairCdText
        {
            get
            {
                return RepairCd > 0 ? RepairCd.ToString("D2") : "";
            }
        }
        public string EigyoNmText
        {
            get
            {
                return CheckLimitLengthString(RyakuNm, 15);
            }
        }
        public string SyaRyoNmText
        {
            get
            {
                return CheckLimitLengthString(SyaRyoNm, 10);
            }
        }
        public string RepairNmText
        {
            get
            {
                return CheckLimitLengthString(RepairRyakuNm, 8);
            }
        }
        public string  BikoText
        {
            get
            {
                return CheckLimitLengthString(BikoNm, 28);
            }
        }
        private string CheckLimitLengthString(string str, int limit)
        {
           /* if (str.Length > limit)
            {
                return str.Substring(0, limit);
            }*/
            return str;
        }

    }
}
