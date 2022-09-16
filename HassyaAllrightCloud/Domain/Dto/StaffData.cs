using HassyaAllrightCloud.Commons.Constants;
using System;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class StaffData
    {
        public int SyainCdSeq { get; set; }
        public int SyokumuCdSeq { get; set; }
        public string SyokumuNm { get; set; }
        public byte SyokumuKbn { get; set; }
        public string CodeKbnNm { get; set; }
        public string TenkoNo { get; set; }
        public int EigyoCdSeq { get; set; }
        public int EigyoCd { get; set; }
        public string RyakuNm { get; set; }
        public string SyainCd { get; set; }        
        public string SyainNm { get; set; }
        public string StaYmd { get; set; }
        public string EndYmd { get; set; }
        public int CompanyCdSeq { get; set; }
        public int CompanyCd { get; set; }
        public string CompanyNm { get; set; }

        public string StaffID { get; set; }
        public string StaffName { get; set; }
        public int StaffVehicle { get; set; }
        public double StaffHeight { get; set; }

        public double Height { get; set; }
        public int BranchID { get; set; }
        public int CompanyID { get; set; }
        public int HolidayID { get; set; }
        public string HolidayNm { get; set; }
        public int WorkID { get; set; }
        public string WorkNm { get; set; }
        public List<int> JobID { get; set; } = new List<int>();
        public Flag Status { get; set; }
        public string DayOff { get; set; }
        public double TimeStartString { get; set; }
        public Boolean IsGray { get; set; } = false;
        public byte BigTypeDrivingFlg { get; set; }
        public byte MediumTypeDrivingFlg { get; set; }
        public byte SmallTypeDrivingFlg { get; set; }
        public string Text => $"{EigyoCd.ToString("D5")}：{RyakuNm}　{SyainCd}：{SyainNm}　{WorkNm}";

        public string PreDayEndTime { get; set; }
        public float WorkTime { get; set; }
        public float WorkTime4Week { get; set; }
        public string EigyoName { get; set; }
        public Flag AllowStatus { get; set; }
        public string ColKinKyu { get; set; }
        public bool isAssignHoliday { get; set; } = false;
        public float Onday { get; set; }
        public float DayBefore { get; set; }

        public List<PopupDisplay> ToolTip { get; set; }
        public bool isShowToolTip { get; set; }

        public string KobanUpdYmd { get; set; } = string.Empty;
        public string KobanUpdTime { get; set; } = string.Empty;
    }
}

