
using System;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class BusInfomation
    {
        public int InUseLargeBusCount { get; set; }
        public int LargeBusCount { get; set; }
        public int InUseMediumBusCount { get; set; }
        public int MediumBusCount { get; set; }
        public int InUseSmallBusCount { get; set; }
        public int SmallBusCount { get; set; }
        public DateTime DateSelected { get; set; }
    }

    public class DriverInfomation
    {
        public int AbsenceLargeDriverCount { get; set; }
        public int LargeDriverCount { get; set; }
        public int AbsenceMediumDriverCount { get; set; }
        public int MediumDriverCount { get; set; }
        public int AbsenceSmallDriverCount { get; set; }
        public int SmallDriverCount { get; set; }
        public DateTime DateSelected { get; set; }
    }

    public class StaffInfo
    {
        public string UkeNo { get; set; }
        public short UnkRen { get; set; }
        public short TeiDanNo { get; set; }
        public short BunkRen { get; set; }
        public string SyuKoYmd { get; set; }
        public string KikYmd { get; set; }
        public short DrvJin { get; set; }
        public byte KataKbn { get; set; }
    }

    public class WorkHolidayData
    {
        public int SyainCdSeq { get; set; }
        public byte BigTypeDrivingFlg { get; set; }
        public byte MediumTypeDrivingFlg { get; set; }
        public byte SmallTypeDrivingFlg { get; set; }
        public string KinKyuSYmd { get; set; }
        public string KinKyuEYmd { get; set; }
    }

    public class EmployeeData
    {
        public int SyainCdSeq { get; set; }
        public byte BigTypeDrivingFlg { get; set; }
        public byte MediumTypeDrivingFlg { get; set; }
        public byte SmallTypeDrivingFlg { get; set; }
        public string StaYmd { get; set; }
        public string EndYmd { get; set; }
    }

    public class ShuriData
    {
        public int SyaRyoCdSeq { get; set; }
        public string ShuriSYmd { get; set; }
        public string ShuriEYmd { get; set; }
    }

    public class BusData
    {
        public int SyaRyoCdSeq { get; set; }
        public byte KataKbn { get; set; }
        public string SyaRyoNm { get; set; }
        public string StaYmd { get; set; }
        public string EndYmd { get; set; }
        public byte NinkaKbn { get; set; }
        public int SyaSyuCdSeq { get; set; }
        public string SyaSyuNm { get; set; }
    }
    public class BusAllocation
    {
        public string UkeNo { get; set; }
        public int HaiSSryCdSeq { get; set; }
        public string SyuKoYmd { get; set; }
        public string KikYmd { get; set; }
        public byte KSKbn { get; set; }
        public int SyaSyuCdSeq { get; set; }
        public string SyaSyuNm { get; set; }
        public byte KataKbn { get; set; }
        public byte NinkaKbn { get; set; }
    }
}
