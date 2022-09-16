using HassyaAllrightCloud.Application.StaffSchedule.Commands;
using HassyaAllrightCloud.Application.StaffSchedule.Queries;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.Pages;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IStaffScheduleService
    {
        Task<IEnumerable<StaffScheduleData>> GetStaffSchedule(int CompanyId, int UserId);
        Task<IEnumerable<CompanyScheduleInfo>> GetGroupScheduleInfo(int Tenant, int CompanyId, int UserId);
        Task<bool> UpdateStaffSchedule(StaffScheduleData model);
        Task<bool> SaveStaffSchedule(StaffScheduleData model);
        Task<bool> DeleteStaffSchedule(AppointmentList model);
        Task<Dictionary<string, List<AppointmentList>>> RangeByStaff(List<AppointmentList> lstStaffSchedule, List<StaffsData> lstStaff);
        Task<Dictionary<string, List<AppointmentList>>> RangeByTime(List<AppointmentList> lstStaffSchedule, List<StaffsData> lstStaff);
        Task<string> GetDisPlayRecurrenceRule(string currentRule, IStringLocalizer<StaffScheduleMobile> Lang, string formatDate);
        Task<List<AppointmentList>> GetAppointmentLists(int employeeId, int tenantCdSeq, int companyId, string fromDate, string toDate, List<PlanType> planTypes, List<AppointmentLabel> appointmentLabels);
        Task<List<AppointmentList>> GetAppointmentListsGroup(int employeeId, int groupId, int tenantCdSeq, string fromDate, string toDate, List<PlanType> planTypes, List<AppointmentLabel> appointmentLabels);
        Task<List<AppointmentList>> GetAppointmentListsCustomGroup(int employeeId, int groupId, int tenantCdSeq, string fromDate, string toDate, List<PlanType> planTypes, List<AppointmentLabel> appointmentLabels);
        Task<List<WorkHourModel>> GetWorkHoursGroup(string pre27DayToDate, string toDate, int groupId);
        Task<List<WorkHourModel>> GetWorkHoursCustomGroup(string pre27DayToDate, string toDate, int groupId);
        Task<List<CalendarSetModel>> GetCalendarSets(int companyCdSeq);
        Task<bool> SaveCalendarSet(CalendarSetModel calendarSet, bool isDelete);
        Task<bool> SaveHaiin(AppointmentList model);
    }

    public class StaffScheduleService : IStaffScheduleService
    {
        private readonly IMediator _mediator;
        public StaffScheduleService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IEnumerable<StaffScheduleData>> GetStaffSchedule(int CompanyId, int UserId)
        {
            return await _mediator.Send(new GetStaffSchedule() { CompanyId = CompanyId, UserId = UserId });
        }
        public async Task<IEnumerable<CompanyScheduleInfo>> GetGroupScheduleInfo(int Tenant, int CompanyId, int UserId)
        {
            return await _mediator.Send(new GetGroupScheduleInfo() { Tenant = Tenant, CompanyId = CompanyId, UserId = UserId });

        }
        public async Task<bool> UpdateStaffSchedule(StaffScheduleData model)
        {
            return await _mediator.Send(new UpdateStaffSchedule() { model = model });
        }

        public async Task<bool> DeleteStaffSchedule(AppointmentList model)
        {
            return await _mediator.Send(new DeleteStaffSchedule() { model = model});
        }

        public async Task<bool> SaveStaffSchedule(StaffScheduleData model)
        {
            return await _mediator.Send(new SaveStaffSchedule() { model = model });
        }

        public Task<Dictionary<string, List<AppointmentList>>> RangeByStaff(List<AppointmentList> lstStaffSchedule, List<StaffsData> lstStaff)
        {
            Dictionary<string, List<AppointmentList>> result = new Dictionary<string, List<AppointmentList>>();
            List<int> memberGroupSchedule = lstStaffSchedule.Select(x => x.KankSya).ToList().SelectMany(o => o).Distinct().ToList();
            foreach (int item in memberGroupSchedule)
            {
                string key = lstStaff.Where(p => p.SyainCdSeq == item).Select(p => p.SyainNm).FirstOrDefault();
                if (!result.ContainsKey(key))
                {
                    result.Add(key, lstStaffSchedule.Where(p => p.KankSya.Contains(item)).OrderByDescending(p => p.AllDayKbn).OrderBy(p => p.StartDateDisplay.ToString("HHmm")).ToList());
                }
            }
            result = result.OrderBy(p => p.Value[0].SyainCdSeq).ToDictionary(p => p.Key, p => p.Value);
            return Task.FromResult(result);
        }

        public Task<Dictionary<string, List<AppointmentList>>> RangeByTime(List<AppointmentList> lstStaffSchedule, List<StaffsData> lstStaff)
        {
            Dictionary<string, List<AppointmentList>> result = new Dictionary<string, List<AppointmentList>>();
            int i = 0;
            lstStaffSchedule = lstStaffSchedule.OrderByDescending(y => y.AllDayKbn).ThenBy(x => x.StartDateDisplay.ToString("HHmm")).ToList();
            foreach (var item in lstStaffSchedule)
            {
                if (item.KankSya != null && item.KankSya.Count > 0)
                {
                    foreach (var itemPar in item.KankSya)
                    {
                        AppointmentList itemTemp = new AppointmentList(item);
                        string currentSyainnm = lstStaff.Where(p => p.SyainCdSeq == itemPar).Select(p => p.SyainNm).FirstOrDefault(); ;
                        if (!result.ContainsKey(currentSyainnm + i.ToString()))
                        {
                            List<AppointmentList> lst = new List<AppointmentList>();
                            itemTemp.SyainnmDisplay = currentSyainnm;
                            lst.Add(itemTemp);
                            result.Add(currentSyainnm + i.ToString(), lst);
                            i++;
                        }
                    }
                }
                else
                {
                    if (!result.ContainsKey(item.Syainnm + i.ToString()))
                    {
                        List<AppointmentList> lst = new List<AppointmentList>();
                        item.SyainnmDisplay = item.Syainnm;
                        lst.Add(item);
                        result.Add(item.Syainnm + i.ToString(), lst);
                        i++;
                    }
                }
            }
            result  = result.OrderByDescending(p => p.Value[0].AllDayKbn).ThenBy(p => p.Value[0].StartDateDisplay.ToString("HHmm")).ThenBy(p => p.Value[0].SyainCdSeq).ToDictionary(p => p.Key, p => p.Value);
            return Task.FromResult(result);
        }

        public async Task<string> GetDisPlayRecurrenceRule(string currentRule, IStringLocalizer<StaffScheduleMobile> Lang, string formatDate)
        {
            string result = "";
            string[] arrType = currentRule.Split(";");
            string[] arrDetail0;
            string[] arrDetail1;
            string[] arrDetail2;
            string[] arrDetail3;
            string[] arrDetail4;
            string[] arrDetailDate;
            string dValue = "";
            if (arrType.Length > 0)
            {
                arrDetail0 = arrType[0].Split("=");
                if (arrDetail0.Length > 0)
                {
                    result = Lang["RecurrenceAfter"] + ": ";
                    switch (arrDetail0[1])
                    {
                        case "HOURLY":
                            arrDetail1 = arrType[1].Split("=");
                            if (arrDetail1[1] == "1")
                            {
                                result += Lang["RecurrenceRepeatHourly"] + " ";
                            }
                            else
                            {
                                result += arrDetail1[1] + Lang["TimeTimes"] + " ";
                            }
                            if (arrType.Length > 2)
                            {
                                arrDetail2 = arrType[1].Split("=");
                                if (arrDetail2[0].Equals("UNTIL"))
                                {
                                    result += (DateDisplayValue(arrDetail2[1]) != null ? (DateDisplayValue(arrDetail2[1].Substring(0, 8)).Value.ToString(formatDate) + Lang["To"]) : "");
                                }
                                else
                                {
                                    result += arrDetail2[1] + Lang["Times"];
                                }
                            }
                            break;
                        case "DAILY":
                            if (arrType.Length == 1)
                            {
                                result += Lang["RecurrenceRepeatDaily"];
                            }
                            if (arrType.Length > 1)
                            {
                                arrDetail1 = arrType[1].Split("=");
                                switch (arrDetail1[0])
                                {
                                    case "UNTIL":
                                        result += (DateDisplayValue(arrDetail1[1].Substring(0, 8)) != null ? (DateDisplayValue(arrDetail1[1].Substring(0, 8)).Value.ToString(formatDate) + " " + Lang["To"]) : "");
                                        break;
                                    case "COUNT":
                                        result += arrDetail1[1] + " " + Lang["Times"];
                                        break;
                                    default:
                                        if (arrDetail1[1] == "1")
                                        {
                                            result += Lang["RecurrenceRepeatDaily"] + " ";
                                        }
                                        else
                                        {
                                            result += arrDetail1[1] + Lang["DayTimes"] + " ";
                                        }
                                        break;
                                }
                            }
                            if (arrType.Length > 2)
                            {
                                arrDetail2 = arrType[1].Split("=");
                                if (arrDetail2[0].Equals("UNTIL"))
                                {
                                    result += (DateDisplayValue(arrDetail2[1].Substring(0, 8)) != null ? (DateDisplayValue(arrDetail2[1].Substring(0, 8)).Value.ToString(formatDate) + " " + Lang["To"]) : "");
                                }
                                else
                                {
                                    result += arrDetail2[1] + " " + Lang["Times"];
                                }
                            }
                            break;
                        case "WEEKLY":
                            if (arrType.Length == 1)
                            {
                                result += Lang["RecurrenceRepeatWeekly"];
                            }
                            if (arrType.Length > 1)
                            {
                                arrDetail1 = arrType[1].Split("=");
                                if (arrDetail1[1].Length > 0)
                                {
                                    arrDetailDate = arrDetail1[1].Split(",");
                                    for (int j = 0; j < arrDetailDate.Length; j++)
                                    {
                                        switch (arrDetailDate[j])
                                        {
                                            case "SU":
                                                dValue += Lang["Sunday"] + ", ";
                                                break;
                                            case "MO":
                                                dValue += Lang["Monday"] + ", ";
                                                break;
                                            case "TU":
                                                dValue += Lang["Tuesday"] + ", ";
                                                break;
                                            case "WE":
                                                dValue += Lang["wednesday"] + ", ";
                                                break;
                                            case "TH":
                                                dValue += Lang["Thursday"] + ", ";
                                                break;
                                            case "FR":
                                                dValue += Lang["Friday"] + ", ";
                                                break;
                                            default:
                                                dValue += Lang["Saturday"] + ", ";
                                                break;
                                        }
                                    }
                                    dValue = dValue.Substring(0, dValue.LastIndexOf(",") - 1);
                                }
                            }

                            if (arrType.Length > 2)
                            {
                                arrDetail2 = arrType[2].Split("=");
                                switch (arrDetail2[0])
                                {
                                    case "UNTIL":
                                        result += Lang["RecurrenceRepeatWeekly"] + " ";
                                        result += dValue + ", ";
                                        result += (DateDisplayValue(arrDetail2[1].Substring(0, 8)) != null ? (DateDisplayValue(arrDetail2[1].Substring(0, 8)).Value.ToString(formatDate) + " " + Lang["To"]) : "");
                                        break;
                                    case "COUNT":
                                        result += Lang["RecurrenceRepeatWeekly"] + " ";
                                        result += dValue + ", ";
                                        result += arrDetail2[1] + " " + Lang["Times"];
                                        break;
                                    default:
                                        if (arrDetail2[1] == "1")
                                        {
                                            result += Lang["RecurrenceRepeatWeekly"] + ", ";
                                        }
                                        else
                                        {
                                            result += arrDetail2[1] + Lang["WeekTimes"] + ", ";
                                        }
                                        result += dValue + " ";
                                        break;
                                }
                            }
                            else
                            {
                                result += Lang["RecurrenceRepeatWeekly"] + " ";
                                result += dValue + " ";
                            }
                            if (arrType.Length > 3)
                            {
                                arrDetail3 = arrType[3].Split("=");
                                if (arrDetail3[0].Equals("UNTIL"))
                                {
                                    result += (DateDisplayValue(arrDetail3[1].Substring(0, 8)) != null ? (DateDisplayValue(arrDetail3[1].Substring(0, 8)).Value.ToString(formatDate) + " " + Lang["To"]) : "");
                                }
                                else
                                {
                                    result += arrDetail3[1] + Lang["Times"];
                                }
                            }
                            break;
                        case "MONTHLY":
                            if (arrType.Length == 1)
                            {
                                result += Lang["RecurrenceRepeatMonthly"];
                            }
                            if (arrType.Length > 1)
                            {
                                arrDetail1 = arrType[1].Split("=");
                                dValue = arrDetail1[1] + Lang["Day"];
                            }
                            if (arrType.Length > 2)
                            {
                                arrDetail2 = arrType[2].Split("=");
                                switch (arrDetail2[0])
                                {
                                    case "UNTIL":
                                        result += Lang["RecurrenceRepeatMonthly"] + " ";
                                        result += dValue + ", ";
                                        result += (DateDisplayValue(arrDetail2[1].Substring(0, 8)) != null ? (DateDisplayValue(arrDetail2[1].Substring(0, 8)).Value.ToString(formatDate) + " " + Lang["To"]) : "");
                                        break;
                                    case "COUNT":
                                        result += Lang["RecurrenceRepeatMonthly"] + " ";
                                        result += dValue + ", ";
                                        result += arrDetail2[1] + " " + Lang["Times"];
                                        break;
                                    default:
                                        if (arrDetail2[1] == "1")
                                        {
                                            result += Lang["RecurrenceRepeatMonthly"] + " ";
                                        }
                                        else
                                        {
                                            result += arrDetail2[1] + Lang["MonthTimes"];
                                        }
                                        result += dValue + " ";
                                        break;
                                }
                            }
                            else
                            {
                                result += Lang["RecurrenceRepeatMonthly"] + " ";
                                result += dValue + " ";
                            }
                            if (arrType.Length > 3)
                            {
                                arrDetail3 = arrType[3].Split("=");
                                if (arrDetail3[0].Equals("UNTIL"))
                                {
                                    result += (DateDisplayValue(arrDetail3[1].Substring(0, 8)) != null ? (DateDisplayValue(arrDetail3[1].Substring(0, 8)).Value.ToString(formatDate) + " " + Lang["To"]) : "");
                                }
                                else
                                {
                                    result += arrDetail3[1] + " " + Lang["Times"];
                                }
                            }
                            break;
                        default:
                            if (arrType.Length == 1)
                            {
                                result += Lang["RecurrenceRepeatYearly"];
                            }
                            if (arrType.Length > 1)
                            {
                                arrDetail1 = arrType[1].Split("=");
                                dValue = arrDetail1[1] + Lang["Day"];
                            }
                            if (arrType.Length > 2)
                            {
                                arrDetail1 = arrType[1].Split("=");
                                arrDetail2 = arrType[2].Split("=");
                                dValue = arrDetail2[1] + Lang["Month"] + arrDetail1[1] + Lang["Day"];
                            }
                            if (arrType.Length > 3)
                            {
                                arrDetail3 = arrType[3].Split("=");
                                switch (arrDetail3[0])
                                {
                                    case "UNTIL":
                                        result += Lang["RecurrenceRepeatYearly"] + " ";
                                        result += dValue + ", ";
                                        result += (DateDisplayValue(arrDetail3[1].Substring(0, 8)) != null ? (DateDisplayValue(arrDetail3[1].Substring(0, 8)).Value.ToString(formatDate) + " " + Lang["To"]) : "");
                                        break;
                                    case "COUNT":
                                        result += Lang["RecurrenceRepeatYearly"] + " ";
                                        result += dValue + ", ";
                                        result += arrDetail3[1] + " " + Lang["Times"];
                                        break;
                                    default:
                                        if (arrDetail3[1] == "1")
                                        {
                                            result += Lang["RecurrenceRepeatYearly"] + " ";
                                        }
                                        else
                                        {
                                            result += arrDetail3[1] + Lang["YearTimes"] + " ";
                                        }
                                        result += dValue + " ";
                                        break;
                                }
                            }
                            else
                            {
                                result += Lang["RecurrenceRepeatYearly"] + " ";
                                result += dValue + " ";
                            }
                            if (arrType.Length > 4)
                            {
                                arrDetail4 = arrType[4].Split("=");
                                if (arrDetail4[0].Equals("UNTIL"))
                                {
                                    result += (DateDisplayValue(arrDetail4[1].Substring(0, 8)) != null ? (DateDisplayValue(arrDetail4[1].Substring(0, 8)).Value.ToString(formatDate) + " " + Lang["To"]) : "");
                                }
                                else
                                {
                                    result += arrDetail4[1] + " " + Lang["Times"];
                                }
                            }
                            break;
                    }

                }
            }
            return await Task.FromResult(result);
        }

        public DateTime? DateDisplayValue(string Ymd)
        {
            DateTime DateValue;
            string DateFormat = "yyyyMMdd";
            if (!DateTime.TryParseExact(Ymd, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateValue))
            {
                return null;
            }
            else
            {
                return DateTime.ParseExact(Ymd, DateFormat, CultureInfo.InvariantCulture);
            }
        }

        public async Task<List<AppointmentList>> GetAppointmentLists(int employeeId, int tenantCdSeq, int companyId, string fromDate, string toDate, List<PlanType> planTypes, List<AppointmentLabel> appointmentLabels)
        {
            return await _mediator.Send(new GetAppointmentList() { EmployeeId = employeeId, CompanyId = companyId, FromDate = fromDate, ToDate = toDate, TenantCdSeq = tenantCdSeq, PlanTypes = planTypes, AppointmentLabels = appointmentLabels});
        }

        public async Task<List<AppointmentList>> GetAppointmentListsGroup(int employeeId, int groupId, int tenantCdSeq, string fromDate, string toDate, List<PlanType> planTypes, List<AppointmentLabel> appointmentLabels)
        {
            return await _mediator.Send(new GetAppointmentListsGroup() {EmployeeId = employeeId, GroupId = groupId, FromDate = fromDate, ToDate = toDate, TenantCdSeq = tenantCdSeq, PlanTypes = planTypes, AppointmentLabels = appointmentLabels });
        }

        public async Task<List<WorkHourModel>> GetWorkHoursGroup(string pre27DayToDate, string toDate, int groupId)
        {
            return await _mediator.Send(new GetWorkHourGroup() { Fre27ToDate = pre27DayToDate, ToDate = toDate, GroupId = groupId });
        }

        public async Task<List<AppointmentList>> GetAppointmentListsCustomGroup(int employeeId, int groupId, int tenantCdSeq, string fromDate, string toDate, List<PlanType> planTypes, List<AppointmentLabel> appointmentLabels)
        {
            return await _mediator.Send(new GetAppointmentListsCustomGroup() { EmployeeId = employeeId, GroupId = groupId, FromDate = fromDate, ToDate = toDate, TenantCdSeq = tenantCdSeq, PlanTypes = planTypes, AppointmentLabels = appointmentLabels });
        }

        public async Task<List<WorkHourModel>> GetWorkHoursCustomGroup(string pre27DayToDate, string toDate, int groupId)
        {
            return await _mediator.Send(new GetWorkHourCustomGroup() { Fre27ToDate = pre27DayToDate, ToDate = toDate, GroupId = groupId });
        }

        public async Task<List<CalendarSetModel>> GetCalendarSets(int companyCdSeq)
        {
            return await _mediator.Send(new GetCalendarSets() { CompanyCdSeq = companyCdSeq});
        }

        public async Task<bool> SaveCalendarSet(CalendarSetModel calendarSet, bool isDelete)
        {
            return await _mediator.Send(new SaveCalendarSet() { CalendarSetModel = calendarSet, IsDelete = isDelete });
        }
        public async Task<bool> SaveHaiin(AppointmentList model)
        {
            return await _mediator.Send(new SaveHaiinMailSeenCrewInfoCommand() { staffInfo = model });
        }

    }
}
