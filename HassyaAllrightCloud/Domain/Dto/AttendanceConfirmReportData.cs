using DevExpress.CodeParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class AttendanceConfirmReportData
    {
        public DateTime OperationDate { get; set; }
        public List<CompanyChartData> CompanyChartData { get; set; }
        public DepartureOfficeData VehicleDispatchOffice1 { get; set; } = new DepartureOfficeData();
        public DepartureOfficeData VehicleDispatchOffice2 { get; set; } = new DepartureOfficeData();
        public List<ReservationData> ReservationList { get; set; } = new List<ReservationData>();
        public string Undelivered { get; set; }
        public OutputOrderData OutputOrder { get; set; } = new OutputOrderData();
        public string SizeOfPaper { get; set; }
        public string TxtInstructions { get; set; }
        public string TxtKeyObjectives { get; set; }
        public List<string> KeyObjectivesList { get; set; } = new List<string>();
        public List<string> InstructionsList { get; set; } = new List<string>();
        public int TenantCdSeq { get; set; } = 0;
        public string SyainNm { get; set; } = "";
        public string DateTimeFooter { get; set; } = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
        public OutputInstruction OutputSetting { get; set; }

        public ReservationClassComponentData BookingTypeFrom { get; set; }
        public ReservationClassComponentData BookingTypeTo { get; set; }
    }

    public class AttendanceConfirmReportDataUri
    {
        public DateTime OperationDate { get; set; }
        public string CompanyChartDataID { get; set; }
        public DepartureOfficeData VehicleDispatchOffice1 { get; set; } = new DepartureOfficeData();
        public DepartureOfficeData VehicleDispatchOffice2 { get; set; } = new DepartureOfficeData();
        public string ReservationListID { get; set; } 
        public string Undelivered { get; set; }
        public OutputOrderData OutputOrder { get; set; } = new OutputOrderData();
        public string SizeOfPaper { get; set; }
        public string TxtInstructions { get; set; }
        public string TxtKeyObjectives { get; set; }
        public List<string> KeyObjectivesList { get; set; } = new List<string>();
        public List<string> InstructionsList { get; set; } = new List<string>();
        public int TenantCdSeq { get; set; } = 0;
        public string SyainNm { get; set; } = "";
        public string DateTimeFooter { get; set; } = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
        public ReservationClassComponentData BookingTypeFrom { get; set; }
        public ReservationClassComponentData BookingTypeTo { get; set; }
        public string Uri { get; set; } = "";
    }
    public class AttendanceConfirmReportPDF
    {
        public int PageNumber { get; set; }
        public int TotalPage { get; set; }
        public string EigyoCdEigyoNm { get; set; }
        public string DateTimeHeader { get; set; }
        public string DateTimeFooter { get; set; }
        public string SyainNm { get; set; }
        public string KeyObjectivesList { get; set; } ="" ;
        public string InstructionsList { get; set; } = "";

        public List<AttendanceConfirmReport> AttendanceConfirmReport { get; set; } = new List<AttendanceConfirmReport>();
    }
    public class AttendanceConfirmReport
    {
        public CurrentAttendanceConfirm BusCurrent { get; set; } = new CurrentAttendanceConfirm();
        public InfoHaiinEmployee Haiin { get; set; } = new InfoHaiinEmployee();
    }
    public class CurrentAttendanceConfirm
    {
        public string UkeNo { get; set; }
        public string UnkRen { get; set; }
        public string BunkRen { get; set; }
        public string CompanyCdSeq { get; set; }
        public string CompanyCd { get; set; }
        public string EigyoCdSeq { get; set; }
        public string EigyoCd { get; set; }
        public string EigyoNm { get; set; }
        public string RyakuNm { get; set; }
        public string SyuKoYmd { get; set; }
        public string SyuKoTime { get; set; }
        public string HaiSYmd { get; set; }
        public string HaiSTime { get; set; }
        public string TouYmd { get; set; }
        public string TouChTime { get; set; }
        public string KikYmd { get; set; }
        public string HAISHA_KikTime { get; set; }
        public string GoSya { get; set; }
        public string TeiDanNo { get; set; } = "";
        public string IkNm { get; set; }
        public string HaiSNm { get; set; }
        public string HaiSKouKCdSeq { get; set; }
        public string HaiSKouKNm { get; set; }
        public string HaiSBinCdSeq { get; set; }
        public string HaiSBinNm { get; set; }
        public string HaiSSetTime { get; set; }
        public string TouNm { get; set; }
        public string TouKouKCdSeq { get; set; }
        public string TouSKouKNm { get; set; }
        public string TouBinCdSeq { get; set; }
        public string TouBinNm { get; set; }
        public string TouSetTime { get; set; }
        public string UNKOBI_SyuKoTime { get; set; }
        public string UNKOBI_HaiSYmd { get; set; }
        public string UNKOBI_HaiSTime { get; set; }
        public string UNKOBI_TouYmd { get; set; }
        public string UNKOBI_TouChTime { get; set; }
        public string UNKOBI_KikTime { get; set; }
        public string UNKOBI_DanTaNm { get; set; }
        public string SyaSyuCdSeq { get; set; }
        public string SyaSyuCd { get; set; }
        public string SyaSyuNm { get; set; }
        public string SyaRyoCdSeq { get; set; }
        public string SyaRyoCd { get; set; }
        public string SyaRyoNm { get; set; }
        public string TenkoNo { get; set; }
        public string DayBusRunning { get; set; } ="";
        public string TotalDayBusRun { get; set; } ="";
        public string VistLocation { get; set; } = "";
        public string VistLocationCompact { get; set; }
        public string SyuKoTimeMain { get; set; }= "";
        public string HaiSTimeMain { get; set; } ="";
        public string KikTimeMain { get; set; } = "";
        public string TotalBus { get; set; }= "";
        public int DayBusRunningReal { get; set; } 
        public int TotalDayBusRunReal { get; set; } 
        public string RowNum { get; set; } = "";
    }
    public class InfoHaiinEmployee
    {
        public string DR_SyainCd { get; set; }
        public string DR_SyainNm { get; set; }
        public string GUI_SyainCd { get; set; }
        public string GUI_SyainNm { get; set; }
        public string TeiDanNo { get; set; }
    }
    public class TehaiReport
    {
        public string Ukeno { get; set; }
        public int UnkRen { get; set; }
        public int TeiDanNo { get; set; }
        public int BunkRen { get; set; }
        public int Nittei { get; set; }
        public string TehNm { get; set; }
    }

}