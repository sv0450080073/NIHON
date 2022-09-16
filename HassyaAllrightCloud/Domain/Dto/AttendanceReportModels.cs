using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using System;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class AttendanceReportSearchModel
    {
        public CompanyData Company { get; set; }
        public EigyoListItem EigyoFrom { get; set; }
        public EigyoListItem EigyoTo{ get; set; }
        public OutputType OutputType { get; set; }
        public PageSizeItem PageSize { get; set; }
        public ReservationClassComponentData RegistrationTypeFrom { get; set; }
        public ReservationClassComponentData RegistrationTypeTo { get; set; }
        public DateTime ProcessingDate { get; set; }
    }

    public class PageSizeItem
    {
        public PageSize PageSize { get; set; }
        public string Name { get; set; }
    }

    public class AttendanceReportModel
    {
        public PageSize PageSize { get; set; }
        public string ProcessingDate { get; set; }
        public int CompanyCdSeq { get; set; }
        public int EigyoFrom { get; set; }
        public int EigyoTo { get; set; }
        public int RegistrationTypeSortFrom { get; set; }
        public int RegistrationTypeSortTo { get; set; }
        public int CurrentTenantId { get; set; }
        public int TenantId { get; set; } = new ClaimModel().TenantID;
        public Guid ReportId { get; set; }
    }

    public class AttendanceReportSPModel
    {
        public byte UnsouKbn { get; set; }
        public string UnsouKbnNm { get; set; }
        public string UnsouKbnRyaku { get; set; }
        public int TotalNobeJyoCnt { get; set; }
        public int TotalNobeRinCnt { get; set; }
        public int TotalNobeSumCnt { get; set; }
        public int TotalNobeJitCnt { get; set; }
        public decimal TotalJitJisaKm { get; set; }
        public decimal TotalJitKisoKm { get; set; }
        public decimal TotalJitSumKm { get; set; }
        public int TotalYusoJin { get; set; }
        public int TotalUnkoCnt { get; set; }
        public int TotalUnkoKikak1Cnt { get; set; }
        public int TotalUnkoKikak2Cnt { get; set; }
        public int TotalUnkoOthCnt { get; set; }
        public int TotalUnkoOthAllCnt { get; set; }
        public decimal TotalUnsoSyu { get; set; }
    }

    public class AttendanceReportPage
    {
        public AttendanceReportCommonData CommonData { get; set; }
        public AttendanceReportDataItem Data { get; set; }
    }

    public class AttendanceReportCommonData
    {
        public string CurrentDateTime { get; set; }
        public string CurrentPage { get; set; }
        public string ProcessingDate { get; set; }
        public string EigyoName { get; set; }
        public string CurrentUser { get; set; }

        public string HolidayTypeNm1 { get; set; }
        public string HolidayTypeNm2 { get; set; }
        public string HolidayTypeNm3 { get; set; }
        public string HolidayTypeNm4 { get; set; }
        public string HolidayTypeNm5 { get; set; }
        public string HolidayTypeNm6 { get; set; }
        public string HolidayTypeNm7 { get; set; }
        public string HolidayTypeNm8 { get; set; }
        public string HolidayTypeNm9 { get; set; }
        public string HolidayTypeNm10 { get; set; }

        #region 33-49
        public string DailyVehiclesCount { get; set; }
        public string TodayVehiclesCount { get; set; }
        public string OvernightVehiclesCount { get; set; }
        public string WorkingVehiclesTotal { get; set; }
        public string SuspendedVehiclesCount { get; set; }
        public string WorkingDriversCount { get; set; }
        public string WorkingGuidesCount { get; set; }
        public string WorkingStaffsTotal { get; set; }
        public string AbsenceDriversCount { get; set; }
        public string AbsenceGuidesCount { get; set; }
        public string AbsenceStaffsCount { get; set; }
        public string WaitingVehiclesCount { get; set; }
        public string WaitingDriversCount { get; set; }
        public string WaitingGuidesCount { get; set; }
        public string WaitingStaffsCount { get; set; }
        public string FirstStartTime { get; set; }
        public string LastReturnTime { get; set; }
        #endregion
    }
    public class KasSetDto
    {
        public string JisKinKyuNm01 { get; set; }
        public string JisKinKyuNm02 { get; set; }
        public string JisKinKyuNm03 { get; set; }
        public string JisKinKyuNm04 { get; set; }
        public string JisKinKyuNm05 { get; set; }
        public string JisKinKyuNm06 { get; set; }
        public string JisKinKyuNm07 { get; set; }
        public string JisKinKyuNm08 { get; set; }
        public string JisKinKyuNm09 { get; set; }
        public string JisKinKyuNm10 { get; set; }
    }

    public class VehicleInfo
    {
        public string SyaRyoSyaRyoNm { get; set; }
        public string SyuRiCdRyakuNm { get; set; }
        public int Index { get; internal set; }
    }
    public class VehicleReportItem
    {
        public string SyaRyoSyaRyoNm { get; set; }
        public string SyaSyuSyaSyuNm { get; set; }
        public int Index { get; set; }
    }

    public class SyainSyainDataItem
    {
        public int Index { get; set; }
        public string SyainSyainNm { get; set; }
        public string KinKyuCdRyakuNm { get; set; }
    }
    public class HolidayInfo
    {
        public int Index { get; set; }
        public string Holiday1 { get; set; }
        public string Holiday2 { get; set; }
        public string Holiday3 { get; set; }
        public string Holiday4 { get; set; }
        public string Holiday5 { get; set; }
        public string Holiday6 { get; set; }
        public string Holiday7 { get; set; }
        public string Holiday8 { get; set; }
        public string Holiday9 { get; set; }
        public string Holiday10 { get; set; }
    }
    public class AttendanceReportDataItem
    {
        public List<VehicleReportItem> VehicleItems { get; set; }
        public List<VehicleInfo> VehicleInfoList { get; set; }
        public List<SyainSyainDataItem> Drivers { get; internal set; }
        public List<HolidayInfo> HolidayInfos { get; internal set; }
        public List<SyainSyainDataItem> Guiders { get; internal set; }
    }
}
