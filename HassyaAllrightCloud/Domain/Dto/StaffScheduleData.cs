using System;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class StaffScheduleData
    {
        public int ScheduleType;
        public int DisplayType;
        public int VacationType;
        public List<int> ScheduleLabel;
        public List<int> Staffs;
        public string Text;
        public bool AllDay;
        public DateTime StartDate;
        public DateTime EndDate;
        public string Description;
        public string RecurrenceRule;
        public string? RecurrenceException;
        public bool IsEditable;
        public int isPublic;
        public int scheduleId;
        public string color;
        public string status;
        public string EmployeeName;
        public string Destination;
        public string ScheduleYoteiType;
        public byte YoteiShoKbn; // 1, peding; 2, accept; 3 defuse;
        public string RemarkApprover { get; set; } // 承認者の備考
        public string Authorizer { get; set; } // 承認者
        public string TimeAppover { get; set; } // 承認時間
        public DateTime? DateAppover { get; set; } // 承認時間
        public string Remark { get; set; } //備考
        public bool IsSendNoti;
        public int StaffIdToSend;
        public int CalendarSeq;
    }

    public class LoadStaffSchedule
    {
        public int ScheduleSeq { get; set; }
        public int ScheduleType { get; set; }
        public int VacationType { get; set; }
        public string ScheduleLabel { get; set; }
        public int Staff { get; set; }
        public int CreateStaff { get; set; }
        public string Text { get; set; }
        public bool AllDay { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public string RecurrenceRule { get; set; }
        public string? RecurrenceException { get; set; }
        public int isPublic { get; set; }
        public int scheduleId { get; set; }
        public string color { get; set; }
        public string status { get; set; }
        public string EmployeeName { get; set; }
        public string Destination { get; set; }
        public string ScheduleYoteiType { get; set; }
        public byte YoteiShoKbn { get; set; } // 1, peding; 2, accept; 3 defuse;
        public string RemarkApprover { get; set; } // 承認者の備考
        public string Authorizer { get; set; } // 承認者
        public string TimeAppover { get; set; } // 承認時間
        public string DateAppover { get; set; } // 承認時間
        public string Remark { get; set; } //備考
        public Dictionary<string, DateTime> RepetitionDate { get; set; }

    }
    public class TypesInfo
    {
        public int Id { get; set; }
        public string Text { get; set; }
        //public string Color { get; set; }
    }

    public class CompanyScheduleInfo
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public List<GroupScheduleInfo> GroupInfo { get; set; }
    }

    public class CustomGroupScheduleForm
    {
        public int? GroupSeq { get; set; }
        public string GroupName { get; set; }
        public List<StaffsData> StaffList { get; set; }
        public CustomGroupScheduleForm()
        {
            GroupSeq = null;
            GroupName = null;
            StaffList = new List<StaffsData>();
        }
        public CustomGroupScheduleForm(GroupScheduleInfo GroupSchedule, List<StaffsData> Staffs)
        {
            GroupSeq = GroupSchedule.GroupId;
            GroupName = GroupSchedule.GroupName;
            StaffList = Staffs.FindAll(s => GroupSchedule.MembersId.Contains(s.SyainCdSeq));
        }
    }
}
