using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class Staffs
    {
        public string SyainCd { get; set; }
        public int Seg { get; set; }
        public string Name { get; set; }
        public int EigyoCdSeq { get; set; }
        public string Text => string.IsNullOrEmpty(SyainCd) ? "" : $"{SyainCd} : {Name}";

    }

    public class EigyoStaffItem
    {
        public int EigyoCdSeq { get; set; }
        public int EigyoCd { get; set; }
        public string EigyoNm { get; set; }
        public string RyakuNm { get; set; }
        public string CalcuSyuTime { get; set; }
        public string CalcuTaiTime { get; set; }
        public string Text => $"{EigyoCd:00000}：{RyakuNm}";
    }

    public class WorkLeaveItem
    {
        public int KinKyuCdSeq { get; set; }
        public short KinKyuCd { get; set; }
        public string KinKyuNm { get; set; }
        public string RyakuNm { get; set; }
        public byte KinKyuKbn { get; set; }
        public string ColKinKyu { get; set; }
        public string KyuSyukinNm { get; set; }
        public string KyuSyukinRyaku { get; set; }
        public string DefaultSyukinTime { get; set; }
        public string DefaultTaiknTime { get; set; }
    }

    public class WorkHolidayItem
    {
        public int KinKyuTblCdSeq { get; set; }
        public string SyukinTime { get; set; }
        public string TaiknTime { get; set; }
        public short KinKyuCd { get; set; }
        public string KinKyuNm { get; set; }
        public string RyakuNm { get; set; }
        public byte KinKyuKbn { get; set; }
        public string ColKinKyu { get; set; }
        public int SyainCdSeq { get; set; }
        public string SyainCd { get; set; }
        public string SyainNm { get; set; }
        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
    }

    public class VehicleAllocationItem
    {
        public string UkeNo { get; set; }
        public short UnkRen { get; set; }
        public short TeiDanNo { get; set; }
        public short BunkRen { get; set; }
        public string SyuKoYmd { get; set; }
        public string SyuKoTime { get; set; }
        public string HaiSYmd { get; set; }
        public string HaiSTime { get; set; }
        public string KikYmd { get; set; }
        public string KikTime { get; set; }
        public string TouYmd { get; set; }
        public string TouChTime { get; set; }
        public string HaiSNm { get; set; }
        public string TouNm { get; set; }
        public short DrvJin { get; set; }
        public short GuiSu { get; set; }
        public int HaiSSryCdSeq { get; set; }
        public short SyaSyuRen { get; set; }
        public string DanTaNm { get; set; }
        public int SyainCdSeq { get; set; }
        public byte HaiInRen { get; set; }
        public string SyukinTime { get; set; }
        public string TaiknTime { get; set; }
        public string Syukinbasy { get; set; }
        public string TaiknBasy { get; set; }
        public int SyokumuKbn { get; set; }
        public byte UnkoJKbn { get; set; }
        public byte KaknKais { get; set; }
        public string KaktYmd { get; set; }
        public int EigyoCdSeq { get; set; }
        public string KoteiSyukoTime { get; set; }
        public string KoteiKikTime { get; set; } = string.Empty;
        public byte HaiSKbn { get; set; }
        public byte KSKbn { get; set; }
        public string HaishaUpdYmdTime { get; set; }
        public string HaiinUpdYmdTime { get; set; }
    }

    public class CrewDataAcquisitionItem
    {
        public int SyainCdSeq { get; set; }
        public string SyainCd { get; set; }
        public string SyainNm { get; set; }
        public string TenkoNo { get; set; }
        public int SyokumuCdSeq { get; set; }
        public string SyokumuNm { get; set; }
        public byte SyokumuKbn { get; set; }
        public byte JigyoKbn { get; set; }
        public string CodeKbnNm { get; set; }
        public int EigyoCdSeq { get; set; }
        public int EigyoCd { get; set; }
        public string EigyoNm { get; set; }
        public int CompanyCdSeq { get; set; }
    }

    public class DrvJinItem
    {
        public string UkeNo { get; set; }
        public short UnkRen { get; set; }
        public short TeiDanNo { get; set; }
        public short BunkRen { get; set; }
        public int DrvJin { get; set; }
        public int SyainCdSeq { get; set; }
        public int EigyoCdSeq { get; set; }
    }

    public class GuiJinItem
    {
        public string UkeNo { get; set; }
        public short UnkRen { get; set; }
        public short TeiDanNo { get; set; }
        public short BunkRen { get; set; }
        public int GuiSu { get; set; }
        public int SyainCdSeq { get; set; }
        public int EigyoCdSeq { get; set; }
    }

    public class JobItem
    {
        public int JobID { get; set; }
        public string UkeNo { get; set; }
        public short UnkRen { get; set; }
        public short TeiDanNo { get; set; }
        public short BunkRen { get; set; }
        public byte HaiInRen { get; set; }
        public string DanTaNm { get; set; }
        public int SyainCdSeq { get; set; }
        public byte SyokumuKbn { get; set; }
        public string SyokumuNm { get; set; }
        public string SyuKoYmd { get; set; }
        public string SyuKoTime { get; set; }
        public string KikYmd { get; set; }
        public string KikTime { get; set; }
        public string HaiSYmd { get; set; }
        public string TouYmd { get; set; }
        public string ColorLine { get; set; }
        public short SyaSyuRen { get; set; }
        public byte UnkoJKbn { get; set; }
        public string Kotei_SyukoTime { get; set; }
        public string Kotei_KikTime { get; set; }

        public double Width { get; set; }
        public double Height { get; set; }
        public double Top { get; set; }
        public double Left { get; set; }
        public string CCSStyle { get; set; }
        public double TimeStartString => double.Parse(SyuKoYmd + SyuKoTime);
        public double TimeEndString => double.Parse(KikYmd + KikTime);
        public bool IsDoing { get; set; }
        public int EigyoCdSeq { get; set; }

        public string HaishaUpdYmdTime { get; set; }
        public string HaiinUpdYmdTime { get; set; }
    }

    public class NumberOfVehicle
    {
        public int EigyoCdSeq { get; set; }
        public int SyaRyoNum { get; set; }
    }
    
    public class HaishaStaffItem
    {
        public string UkeNo { get; set; }
        public short UnkRen { get; set; }
        public short TeiDanNo { get; set; }
        public short BunkRen { get; set; }
        public short SyaSyuRen { get; set; }
        public string SyuKoYmd { get; set; }
        public string SyuKoTime { get; set; }
        public string KikYmd { get; set; }
        public string KikTime { get; set; }
    }

    public class KoteikItem
    {
        public short Nittei { get; set; }
        public string SyukoTime { get; set; }
        public string KikTime { get; set; }
    }

    public class SyuTaikinCalculationTimeItem
    {
        public byte KouZokPtnKbn { get; set; }
        public int SyukinCalculationTimeMinutes { get; set; }
        public int TaikinCalculationTimeMinutes { get; set; }
    }

    public class PreDayEndTimeItem
    {
        public int SyainCdSeq { get; set; }
        public string ZenjituTaiknTime { get; set; }
    }

    public class WorkTimeItem
    {
        public int SyainCdSeq { get; set; }
        public string SyukinTime { get; set; }
        public string TaiknTime { get; set; }
        public string KouSTime { get; set; }
        public string UnkYmd { get; set; }
    }

    public class StaffEigyo
    {
        public int EigyoCdSeq { get; set; }
        public string EigyoName { get; set; }
    }

    public class RestraintTime
    {
        public int SyainCdSeq { get; set; }
        public int Num { get; set; }
        public string StaDate { get; set; }
        public string EndDate { get; set; }
        public string DrvJin { get; set; }
        public int KousokuMinute { get; set; }
    }

    public class RestPeriod
    {
        public int SyainCdSeq { get; set; }
        public string UnkYmd { get; set; }
        public string DrvJin { get; set; }
        public int KyusoCnt { get; set; }
        public int KyusokuMinute { get; set; }
    }

    public class Holiday
    {
        public int SyainCdSeq { get; set; }
        public string StaDate { get; set; }
        public string EndDate { get; set; }
        public int LeaveCnt { get; set; }
    }

    public class TimeSearchParam
    {
        public int CompanyCdSeq { get; set; } = new ClaimModel().CompanyID;
        public int SyainCdSeq { get; set; } = new ClaimModel().SyainCdSeq;
        public string UnkYmd { get; set; }
        public int Times { get; set; }
        public byte DriverNaikinOnly { get; set; }
        public DataTable KobanTableType { get; set; }
        public string DelUkeNo { get; set; } = string.Empty;
        public short DelUnkRen { get; set; }
        public short DelTeiDanNo { get; set; }
        public short DelBunkRen { get; set; }
    }

    public class KobanTableType
    {
        public string UnkYmd { get; set; }
        public string UkeNo { get; set; }
        public short UnkRen { get; set; }
        public short TeiDanNo { get; set; }
        public short BunkRen { get; set; }
        public string SyukinYmd { get; set; }
        public string SyukinTime { get; set; }
        public string TaikinYmd { get; set; }
        public string TaiknTime { get; set; }
    }

    public class PopupData
    {
        public int KijunSeq { get; set; }
        public string KijunNm { get; set; }
        public byte PeriodUnit { get; set; }
        public int PeriodValue { get; set; }
        public int KijunRef { get; set; }
        public byte RefPeriodUnit { get; set; }
        public int RefPeriodValue { get; set; }
    }

    public class PopupValue
    {
        public int KijunSeq { get; set; }
        public int JokenNo { get; set; }
        public byte RestrictedTarget { get; set; }
        public byte RestrictedExp { get; set; }
        public int RestrictedValue { get; set; }
    }

    public class PopupDisplay
    {
        public int SyainCdSeq { get; set; }
        public int KijunSeq { get; set; }
        public string KijunNm { get; set; }
        public string KijunValue { get; set; }
    }

    public class StaffHaitaCheck
    {
        public string HaishaUpdYmd { get; set; }
        public string HaishaUpdTime { get; set; }
        public string HaiinUpdYmdTime { get; set; }
    }
}
