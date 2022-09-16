using System;
using HassyaAllrightCloud.Commons.Constants;
using System.Collections.Generic;
using System.Globalization;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class BusTypeListData
    {
        public DateTime StartDate { get; set; } = DateTime.Today;
        public ReservationClassComponentData BookingTypeFrom { get; set; }
        public ReservationClassComponentData BookingTypeTo { get; set; }
        public CompanyData Company { get; set; }
        public LoadSaleBranch BranchStart { get; set; }
        public LoadSaleBranch BranchEnd { get; set; }
        public LoadStaffList SalesStaffStart { get; set; }
        public LoadStaffList SalesStaffEnd { get; set; }
        public LoadStaffList PersonInputStart { get; set; }
        public LoadStaffList PersonInputEnd { get; set; }
        public DestinationData DestinationStart { get; set; }
        public DestinationData DestinationEnd { get; set; }
        public TPM_CodeKbCodeSyuData BusType { get; set; }
        public BusTypesData VehicleFrom { get; set; }
        public BusTypesData VehicleTo { get; set; }
        public OutputReportType OutputType { get; set; } = OutputReportType.Preview;
        public DepositOutputClass DepositOutputTemplate { get; set; }
        public PaperSize PaperSize { get; set; } = new PaperSize();
        public ViewMode GridSize { get; set; } = new ViewMode();
        public GroupMode GroupMode { get; set; } = new GroupMode();
        public List<ReservationClassComponentData> ReservationList { get; set; }
        public int TenantCdSeq { get; set; }
        public int TenantCdSeqByCodeSyu { get; set; }
        public string SyainNm { get; set; }
        public string SyainCd { get; set; }
        public int numberDay { get; set; }

    }
    public class BusTypeListDataUri
    {
        public DateTime StartDate { get; set; } = DateTime.Today;
        public ReservationClassComponentData BookingTypeFrom { get; set; }
        public ReservationClassComponentData BookingTypeTo { get; set; }
        public CompanyData Company { get; set; }
        public LoadSaleBranch BranchStart { get; set; }
        public LoadSaleBranch BranchEnd { get; set; }
        public LoadStaffList SalesStaffStart { get; set; }
        public LoadStaffList SalesStaffEnd { get; set; }
        public LoadStaffList PersonInputStart { get; set; }
        public LoadStaffList PersonInputEnd { get; set; }
        public DestinationData DestinationStart { get; set; }
        public DestinationData DestinationEnd { get; set; }
        public TPM_CodeKbCodeSyuData BusType { get; set; }
        public BusTypesData VehicleFrom { get; set; }
        public BusTypesData VehicleTo { get; set; }
        public OutputReportType OutputType { get; set; } = OutputReportType.Preview;
        public DepositOutputClass DepositOutputTemplate { get; set; }
        public PaperSize PaperSize { get; set; } = new PaperSize();
        public ViewMode GridSize { get; set; } = new ViewMode();
        public GroupMode GroupMode { get; set; } = new GroupMode();
        public int TenantCdSeq { get; set; }
        public int TenantCdSeqByCodeSyu { get; set; }
        public string SyainNm { get; set; }
        public string SyainCd { get; set; }
        public string ReservationListID { get; set; }
        public int numberDayUri { get; set; }
    }
    public class VehicleTypeReport
    {
        public int SyaSyu1_SyaSyuCdSeq { get; set; }
        public string SyaSyu1_SyaSyuNm { get; set; }
        public int CntSyaSyu_CNT { get; set; }
    }
    public class TbTMP
    {
        public int SyaSyu_SyaSyuCdSeq { get; set; }
        public int Count_SyaSyuCdSeq { get; set; }
    }
    public class BusTypeItemDataReport
    {
        public int SyaSyuCdSeq { get; set; }
        public string SyaSyuNm { get; set; }
        public int KataKbn { get; set; }
        public int EigyoCdSeq { get; set; }
        public int EigyoCd { get; set; }
        public string EigyoNm { get; set; }
        public string Ei_RyakuNm { get; set; }
        public int CompanyCdSeq { get; set; }
        public string CompanyNm { get; set; }
        public string COM_RyakuNm { get; set; }
        public int CountNumberOfVehicle { get; set; }
        public string SyaSyuNmFormat
        {
            get
            {
                return SyaSyuNm + " (" + CountNumberOfVehicle + ") ";
            }
        }
    }
    public class BusTypeDetailDataReport
    {
        public int STT { get; set; }
        public int SyaSyuCdSeq { get; set; } = 0;
        public string SyaSyuNm { get; set; }
        public int Number { get; set; }
        public int KataKbn { get; set; }
        public string SyuKoYmd { get; set; }
        public string KikYmd { get; set; }
        public int CompanyCdSeq { get; set; }
        public int EigyoCdSeq { get; set; }
        public int EigyoCd { get; set; }
        public int DrvJin { get; set; }
        public int GuiSu { get; set; }
        public int KSKbn { get; set; }
        public int HaiSKbn { get; set; }
        public int HaiSSryCdSeq { get; set; }
        public int YouTblSeq { get; set; }
        public int UN_UnkoJKbn { get; set; }
        public string UN_SyukoYmd { get; set; }
        public string UN_KikYmd { get; set; }
        public string UN_HaiSYmd { get; set; }
        public string UN_TouYmd { get; set; }
        public string NumberText
        {
            get
            {
                //return Number != 0 && Number > 0 ? Number.ToString() : "";
                return Number != 0 ? Number.ToString() : "";
            }

        }
        public int IsDriver { get; set; }
    }
    public class NumberVehicleOfBusUnAsign
    {
        public int KataKbn { get; set; }
        public string SyuKoYmd { get; set; }
        public string KikYmd { get; set; }
        public int NumberOfVehicle { get; set; }
        public int CompanyCdSeq { get; set; }
        public int UkeEigCdSeq { get; set; }
        public int YYSYU_SyaSyuCdSeq { get; set; }
        public string UkeNo { get; set; }
        public int UnkRen { get; set; }
        public int SyaSyuRen { get; set; }
        public string NumberOfVehicleText
        {
            get
            {
                return NumberOfVehicle != 0 && NumberOfVehicle > 0 ? NumberOfVehicle.ToString() : "";
            }

        }

    }
    public class BusTypeReportHeatder
    {
        public string CalText01 { get; set; }
        public string CalText02 { get; set; }
        public string CalText03 { get; set; }
        public string CalText04 { get; set; }
        public string CalText05 { get; set; }
        public string CalText06 { get; set; }
        public string CalText07 { get; set; }
        public string CalText08 { get; set; }
        public string CalText09 { get; set; }
        public string CalText10 { get; set; }
        public string CalText11 { get; set; }
        public string CalText12 { get; set; }
        public string CalText13 { get; set; }
        public string CalText14 { get; set; }
        public string CalText15 { get; set; }
        public string CalText16 { get; set; }
        public string CalText17 { get; set; }
        public string CalText18 { get; set; }
        public string CalText19 { get; set; }
        public string CalText20 { get; set; }
        public string CalText21 { get; set; }
        public string CalText22 { get; set; }
        public string CalText23 { get; set; }
        public string CalText24 { get; set; }
        public string CalText25 { get; set; }
        public string CalText26 { get; set; }
        public string CalText27 { get; set; }
        public string CalText28 { get; set; }
        public string CalText29 { get; set; }
        public string CalText30 { get; set; }
        public string CalText31 { get; set; }
    }
    public class BusTypeReportBody
    {
        public BusTypeReportHeatder HeatderTable { get; set; } = new BusTypeReportHeatder();
        public string BusTypeNm { get; set; } = "";
        public int NumberOfBusType { get; set; }
        public string NumberOfBusTypeText
        {
            get
            {
                return NumberOfBusType > 0 ? NumberOfBusType.ToString() : "";
            }

        }
        public int CalValue01 { get; set; }
        public int CalValue02 { get; set; }
        public int CalValue03 { get; set; }
        public int CalValue04 { get; set; }
        public int CalValue05 { get; set; }
        public int CalValue06 { get; set; }
        public int CalValue07 { get; set; }
        public int CalValue08 { get; set; }
        public int CalValue09 { get; set; }
        public int CalValue10 { get; set; }
        public int CalValue11 { get; set; }
        public int CalValue12 { get; set; }
        public int CalValue13 { get; set; }
        public int CalValue14 { get; set; }
        public int CalValue15 { get; set; }
        public int CalValue16 { get; set; }
        public int CalValue17 { get; set; }
        public int CalValue18 { get; set; }
        public int CalValue19 { get; set; }
        public int CalValue20 { get; set; }
        public int CalValue21 { get; set; }
        public int CalValue22 { get; set; }
        public int CalValue23 { get; set; }
        public int CalValue24 { get; set; }
        public int CalValue25 { get; set; }
        public int CalValue26 { get; set; }
        public int CalValue27 { get; set; }
        public int CalValue28 { get; set; }
        public int CalValue29 { get; set; }
        public int CalValue30 { get; set; }
        public int CalValue31 { get; set; }
        public string CalValueText01
        {
            get { return CalValue01 != 0 ? CalValue01.ToString() : ""; }
        }
        public string CalValueText02
        {
            get { return CalValue02 != 0 ? CalValue02.ToString() : ""; }
        }
        public string CalValueText03
        {
            get { return CalValue03 != 0 ? CalValue03.ToString() : ""; }
        }
        public string CalValueText04
        {
            get { return CalValue04 != 0 ? CalValue04.ToString() : ""; }
        }
        public string CalValueText05
        {
            get { return CalValue05 != 0 ? CalValue05.ToString() : ""; }
        }
        public string CalValueText06
        {
            get { return CalValue06 != 0 ? CalValue06.ToString() : ""; }
        }
        public string CalValueText07
        {
            get { return CalValue07 != 0 ? CalValue07.ToString() : ""; }
        }
        public string CalValueText08
        {
            get { return CalValue08 != 0 ? CalValue08.ToString() : ""; }
        }
        public string CalValueText09
        {
            get { return CalValue09 != 0 ? CalValue09.ToString() : ""; }
        }
        public string CalValueText10
        {
            get { return CalValue10 != 0 ? CalValue10.ToString() : ""; }
        }
        public string CalValueText11
        {
            get { return CalValue11 != 0 ? CalValue11.ToString() : ""; }
        }
        public string CalValueText12
        {
            get { return CalValue12 != 0 ? CalValue12.ToString() : ""; }
        }
        public string CalValueText13
        {
            get { return CalValue13 != 0 ? CalValue13.ToString() : ""; }
        }
        public string CalValueText14
        {
            get { return CalValue14 != 0 ? CalValue14.ToString() : ""; }
        }
        public string CalValueText15
        {
            get { return CalValue15 != 0 ? CalValue15.ToString() : ""; }
        }
        public string CalValueText16
        {
            get { return CalValue16 != 0 ? CalValue16.ToString() : ""; }
        }
        public string CalValueText17
        {
            get { return CalValue17 != 0 ? CalValue17.ToString() : ""; }
        }
        public string CalValueText18
        {
            get { return CalValue18 != 0 ? CalValue18.ToString() : ""; }
        }
        public string CalValueText19
        {
            get { return CalValue19 != 0 ? CalValue19.ToString() : ""; }
        }
        public string CalValueText20
        {
            get { return CalValue20 != 0 ? CalValue20.ToString() : ""; }
        }
        public string CalValueText21
        {
            get { return CalValue21 != 0 ? CalValue21.ToString() : ""; }
        }
        public string CalValueText22
        {
            get { return CalValue22 != 0 ? CalValue22.ToString() : ""; }
        }
        public string CalValueText23
        {
            get { return CalValue23 != 0 ? CalValue23.ToString() : ""; }
        }
        public string CalValueText24
        {
            get { return CalValue24 != 0 ? CalValue24.ToString() : ""; }
        }
        public string CalValueText25
        {
            get { return CalValue25 != 0 ? CalValue25.ToString() : ""; }
        }
        public string CalValueText26
        {
            get { return CalValue26 != 0 ? CalValue26.ToString() : ""; }
        }
        public string CalValueText27
        {
            get { return CalValue27 != 0 ? CalValue27.ToString() : ""; }
        }
        public string CalValueText28
        {
            get { return CalValue28 != 0 ? CalValue28.ToString() : ""; }
        }
        public string CalValueText29
        {
            get { return CalValue29 != 0 ? CalValue29.ToString() : ""; }
        }
        public string CalValueText30
        {
            get { return CalValue30 != 0 ? CalValue30.ToString() : ""; }
        }
        public string CalValueText31
        {
            get { return CalValue31 != 0 ? CalValue31.ToString() : ""; }
        }
    }
    public class BusTypeListReportPDF
    {
        public List<BusTypeReportBody> ReportBodyList { get; set; } = new List<BusTypeReportBody>();
        public OrtherInfo OrtherInfo { get; set; } = new OrtherInfo();
        public int PageNumber { get; set; }
        public int TotalPage { get; set; }
    }
    public class BusTypeListReportFooter
    {
        public BusTypeReportBody SumBusTypeUnAsignList { get; set; } = new BusTypeReportBody();
        public BusTypeReportBody SumBusTypeHiringList { get; set; } = new BusTypeReportBody();
        public BusTypeReportBody SumBusTypeNormalList { get; set; } = new BusTypeReportBody();
        public BusTypeReportBody SumDriverList { get; set; } = new BusTypeReportBody();
        public BusTypeReportBody SumGuiSuList { get; set; } = new BusTypeReportBody();

    }
    public class OrtherInfo
    {
        public string CurrentDate { get; set; }
        public string DateSearch { get; set; }
        public string SyainNm { get; set; }
        public string SyainCd { get; set; }
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
        public string CurrentDateFormat
        {
            get
            {
                return FormatStringDate(CurrentDate);
            }
        }
        public string CurrentDateSearch
        {
            get
            {
                return FormatStringDate(DateSearch, false);
            }
        }
    }
    public class BusTypeReportGroupBody
    {
        public string SyaSyuName { get; set; }
        public int SyuSyuCdSeq { get; set; }
        public BusTypeReportHeatder HeatderTable { get; set; } = new BusTypeReportHeatder();
        public BusTypeReportBody BusKSKbn02 { get; set; } = new BusTypeReportBody();
        public BusTypeReportBody BusHaiSKbn02 { get; set; } = new BusTypeReportBody();
        public BusTypeReportBody BusRepair { get; set; } = new BusTypeReportBody();
        public BusTypeReportBody BusFee { get; set; } = new BusTypeReportBody();
        public BusTypeReportBody BusHiring { get; set; } = new BusTypeReportBody();
        public BusTypeReportBody BusUnAsign { get; set; } = new BusTypeReportBody();
        public BusTypeReportBody BusTotal { get; set; } = new BusTypeReportBody();
        public BusTypeReportBody HiGaeRi { get; set; } = new BusTypeReportBody();
        public BusTypeReportBody TomariDe { get; set; } = new BusTypeReportBody();
        public BusTypeReportBody HaKuChu { get; set; } = new BusTypeReportBody();
        public BusTypeReportBody HaKuKi { get; set; } = new BusTypeReportBody();
        public BusTypeReportBody YaKoDe { get; set; } = new BusTypeReportBody();
        public BusTypeReportBody YaKoKi { get; set; } = new BusTypeReportBody();
        public BusTypeReportBody YaKoChu { get; set; } = new BusTypeReportBody();
    }

    public class BusTypeListReportGroupPDF
    {
        public List<BusTypeReportGroupBody> ReportGroupBodyList { get; set; } = new List<BusTypeReportGroupBody>();
        public OrtherInfo OrtherInfo { get; set; } = new OrtherInfo();
        public int PageNumber { get; set; }
        public int TotalPage { get; set; }
    }


    public class BusRepairDataSource
    {
        public int ShuriCdSeq { get; set; }
        public string ShuriSYmd { get; set; }
        public string ShuriEYmd { get; set; }
        public int SyaRyoCdSeq { get; set; }
        public int SyaSyuCdSeq { get; set; }
    }
    public class HenSyaDataSource
    {
        public int SyaSyuCdSeq { get; set; }
        public int SyaRyoCdSeq { get; set; }
        public string StaYmd { get; set; }
        public string EndYmd { get; set; }
        public int EigyoCdSeq { get; set; }
        public int CompanyCdSeq { get; set; }
    }

    public class CalendarReport
    {
        public int TenantCdSeq { get; set; }
        public string CodeKbnNm { get; set; }
        public string CalenCom { get; set; }
        public string CalenYmd { get; set; }
        public int CountryCdSeq { get; set; }
        public int CompanyCdSeq { get; set; }
    }

    public class BusRunMode
    {
        public List<BusTypeDetailDataReport> BusHigaeri { get; set; } = new List<BusTypeDetailDataReport>();
        public List<BusTypeDetailDataReport> BusTomaride { get; set; } = new List<BusTypeDetailDataReport>();
        public List<BusTypeDetailDataReport> BusHakuchu { get; set; } = new List<BusTypeDetailDataReport>();
        public List<BusTypeDetailDataReport> BusHakuki { get; set; } = new List<BusTypeDetailDataReport>();
        public List<BusTypeDetailDataReport> BusYakode { get; set; } = new List<BusTypeDetailDataReport>();
        public List<BusTypeDetailDataReport> BusYakoki { get; set; } = new List<BusTypeDetailDataReport>();
        public List<BusTypeDetailDataReport> BusYakochu { get; set; } = new List<BusTypeDetailDataReport>();
        // List<BusTypeDetailDataReport> BusUnKnow { get; set; } = new List<BusTypeDetailDataReport>();
    }


}

