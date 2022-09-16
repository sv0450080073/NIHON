using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TimeZoneConverter;
using HassyaAllrightCloud.Commons.Extensions;

namespace HassyaAllrightCloud.Pages
{
    public class StaffScheduleMobileBase : ComponentBase
    {
        #region Inject
        [Inject]
        protected IStringLocalizer<StaffScheduleMobile> Lang { get; set; }
        [Inject]
        protected INotificationTemplateService NotificationTemplateService { get; set; }
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }
        [Inject]
        IStaffScheduleService StaffScheduleService { get; set; }
        [Inject]
        ITPM_CodeKbListService CodeKbService { get; set; }
        [Inject]
        ILeaveDayTypeService LeaveDayTypeService { get; set; }
        [Inject]
        IStaffListService SyainService { get; set; }
        [Inject]
        IScheduleCustomGroupService ScheduleCustomGroupService { get; set; }
        [Inject]
        IDisplaySettingService DisplaySettingService { get; set; }
        [Inject]
        IScheduleGroupDataService StaffGroupDataService { get; set; }
        [Inject]
        NavigationManager NavigationManager { get; set; }
        [Inject] protected ILoadingService _loading { get; set; }
        [Inject]
        protected IErrorHandlerService errorModalService { get; set; }
        #endregion

        #region parameter
        [Parameter]
        public string groupSchedule { get; set; }
        [Parameter]
        public string dateDisPlayCurrent { get; set; }
        [Parameter]
        public string currentSort { get; set; }

        #endregion

        #region Propeties and variable
        protected bool isMonth { get; set; } = true;
        protected bool isDay { get; set; } = false;
        protected DateTime dateDisplay { get; set; } = DateTime.Now;
        protected DateTime dateHighLight { get; set; } = DateTime.Now;
        protected int typeDisplayGrid { get; set; } = 1;
        protected bool SortStaffType { get; set; } = true;
        protected bool SortTimeType { get; set; } = true;
        protected bool displayDay { get; set; } = false;
        protected SearchParam searchParams { get; set; }
        protected bool PopupDetailConfirm { get; set; } = false;
        protected bool PopupDetailOther { get; set; } = false;
        protected bool IsSendMailError { get; set; } = false;
        protected bool PopupDetail { get; set; } = false;
        protected bool PopupIsSettingTypeCalendar { get; set; } = false;
        protected List<CalendarSetModel> CalendarSets = new List<CalendarSetModel>();
        protected Dictionary<int, bool> SelectedCalendarDict = new Dictionary<int, bool>();
        protected Dictionary<int, bool> SelectedBirthdayCommentDict = new Dictionary<int, bool>();
        protected string formatDate = "yyyy年MM月dd日(ddd)";
        protected List<string> timeRangeNow = new List<string>();
        protected bool IsFirstSelect = true;
        protected GroupScheduleInfo currentGroupSelected = new GroupScheduleInfo();
        protected List<VpmCodeKb> ScheduleTypeList = new List<VpmCodeKb>();
        protected List<VpmCodeKb> ScheduleLabelList = new List<VpmCodeKb>();
        protected string key = "groupschedule";
        protected List<StaffsData> StaffList;
        protected List<StaffModel> StaffListOfGroup;
        protected List<LoadLeaveDayType> LeaveDayTypeList = new List<LoadLeaveDayType>();
        protected Dictionary<int, Dictionary<int, bool>> SelectedGroupScheduleDict = new Dictionary<int, Dictionary<int, bool>>();
        protected List<CompanyScheduleInfo> CompaniesScheduleInfo;
        protected CompanyScheduleInfo currentCompanyGroupDisplay = new CompanyScheduleInfo();
        protected LoadDisplaySetting DisplaySettingData = new LoadDisplaySetting();
        protected LoadDisplaySetting DisplaySettingDataForm;
        protected List<TimeZoneInfo> TimeZones = new List<TimeZoneInfo>();
        protected List<NumberWithStringDisplay> DisplayTypes = new List<NumberWithStringDisplay>();
        protected List<NumberWithStringDisplay> WeekStartDays = new List<NumberWithStringDisplay>();
        protected List<StringWithStringDisplay> DayStartHours = new List<StringWithStringDisplay>();

        protected bool firstDisplayRender = false;
        protected bool firstSortGroupRender = false;
        protected bool isDeleted = false;
        protected bool isAdd = false;
        protected bool displayDataFirst = true;
        protected bool isFirstDay = true;
        protected bool isReRender = false;
        private string currentYearMonth = DateTime.Now.ToString("yyyyMM");
        ScheduleDataModel itemTemp = new ScheduleDataModel() { scheduleId = "67", EmployeeId = "000000000001", EmployeeName = "工房太郎", scheduleType = "70" };
        protected List<ScheduleDataModel> listStaffTime { get; set; } = new List<ScheduleDataModel>();

        // Change model 2020/12/18
        Dictionary<string, AppointmentList> currentAppoitmentList = new Dictionary<string, AppointmentList>();
        protected List<AppointmentList> currentListAppoitmentStaff = new List<AppointmentList>();
        protected Dictionary<string, List<AppointmentList>> CurrentScheduleDataModelDict = new Dictionary<string, List<AppointmentList>>();
        List<AppointmentLabel> appointmentLabels = new List<AppointmentLabel>();
        List<PlanType> planTypes = new List<PlanType>();
        List<AppointmentList> AppointmentLists = new List<AppointmentList>();
        List<AppointmentList> AppointmentListsGroup = new List<AppointmentList>();
        Dictionary<string, string> LangDictionary = new Dictionary<string, string>();
        CultureInfo Culture = System.Threading.Thread.CurrentThread.CurrentCulture;
        // Types of Schedule yotei
        List<TypesInfo> ScheduleYoteiTypeData = new List<TypesInfo>();
        // Types of Vacation: KinKyu
        List<TypesInfo> VacationKinKyuTypeData = new List<TypesInfo>();
        // Types of Schedule label: 絶対 and 希望
        List<TypesInfo> ScheduleLabelData = new List<TypesInfo>();
        protected AppointmentList groupParam = new AppointmentList();
        protected AppointmentList staffParam = new AppointmentList();
        protected List<WorkHourModel> workHourGroups { get; set; } = new List<WorkHourModel>();
        #endregion

        #region Function
        protected override void OnParametersSet()
        {
            if (groupSchedule != null)
            {
                currentGroupSelected = EncryptHelper.DecryptFromUrl<GroupScheduleInfo>(groupSchedule);
            }
            if (currentSort != null)
            {
                typeDisplayGrid = currentSort == "True" ? 1 : 2;
            }
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await Task.Run(() =>
                {
                    InvokeAsync(StateHasChanged).Wait();
                    JSRuntime.InvokeVoidAsync("loadPageScript", "staffScheduleMobilePage");
                });
                await JSRuntime.InvokeVoidAsync("loadPageScript", "staffScheduleMobilePage", "fadeToggleGroupScheduleMB");

            }
            if (firstDisplayRender)
            {
                if (displayDay)
                {
                    await JSRuntime.InvokeVoidAsync("loadPageScript", "staffScheduleMobilePage", "displayCalenda", false);
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("loadPageScript", "staffScheduleMobilePage", "displayCalenda", true);
                }
                firstDisplayRender = false;
            }
            if (currentAppoitmentList != null && currentAppoitmentList.Count > 0 && displayDataFirst)
            {
                displayDataFirst = false;
                GetAndDisplayScheduleByDate(dateDisplay);
            }
            if (firstSortGroupRender)
            {
                firstSortGroupRender = false;
                await JSRuntime.InvokeVoidAsync("loadPageScript", "staffScheduleMobilePage", "fadeToggleGroupScheduleMB");
            }
        }
        protected override async Task OnInitializedAsync()
        {
            await _loading.ShowAsync();
            //await Task.Delay(100);
            CalendarSets = await StaffScheduleService.GetCalendarSets(new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID);
            GenerateSelectedCalendar(CalendarSets);
            GenerateBirthdayCommentDict();
            currentAppoitmentList = new Dictionary<string, AppointmentList>();
            listStaffTime.Add(itemTemp);
            // dateDisplay in current timezone
            if (dateDisPlayCurrent != null)
            {
                dateDisplay = DateDisplayValue(dateDisPlayCurrent).Value;
            }

            CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            if (cultureInfo.Name != "ja-JP")
            {
                formatDate = "yyyy/MM/dd(ddd)";
            }
            timeRangeNow = GetTimeRangeNow(DateTime.Now);
            LeaveDayTypeList = await LeaveDayTypeService.Get();
            StaffList = await SyainService.Get(new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq);
            Dictionary<int, bool> CurrentUserDict = new Dictionary<int, bool>();
            if (!CurrentUserDict.ContainsKey(0))
            {
                CurrentUserDict.Add(0, true);
            }
            if (!SelectedGroupScheduleDict.ContainsKey(new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID))
            {
                SelectedGroupScheduleDict.Add(new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, CurrentUserDict);
            }
            IEnumerable<CompanyScheduleInfo> ScheduleInfo = await StaffScheduleService.GetGroupScheduleInfo(new ClaimModel().TenantID, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, 0);
            CompaniesScheduleInfo = ScheduleInfo.ToList();
            if (groupSchedule != null)
            {
                currentGroupSelected = EncryptHelper.DecryptFromUrl<GroupScheduleInfo>(groupSchedule);
                currentCompanyGroupDisplay = CompaniesScheduleInfo.Where(x => x.CompanyId == currentGroupSelected.CompanyId).FirstOrDefault();
                currentCompanyGroupDisplay.GroupInfo = currentCompanyGroupDisplay.GroupInfo.Where(y => y.CompanyId == currentGroupSelected.CompanyId && y.GroupId == currentGroupSelected.GroupId).ToList();
            }
            LoadDisplaySetting SettingData = await DisplaySettingService.Get(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq);
            DisplaySettingData = new LoadDisplaySetting(SettingData);
            DisplaySettingDataForm = new LoadDisplaySetting(SettingData);
            appointmentLabels = await DisplaySettingService.GetAppointmentLabel(new ClaimModel().TenantID);
            planTypes = await DisplaySettingService.GetPlanType(new ClaimModel().TenantID);
            // Dictionary for multilangue
            List<string> Keys = Lang.GetAllStrings().Select(x => x.Name).ToList();
            foreach (string Key in Keys)
            {
                LangDictionary.Add(Key, Lang[Key]);
            }
            // Add schedule yotei type into common list
            foreach (PlanType item in planTypes)
            {
                TypesInfo ScheduleTypes = new TypesInfo
                {
                    Id = int.Parse(item.Id),
                    Text = item.Text,
                };
                ScheduleYoteiTypeData.Add(ScheduleTypes);
            }
            // Add schedule KinKyu type into common list
            foreach (LoadLeaveDayType LeaveDayType in LeaveDayTypeList)
            {
                TypesInfo VacationTypes = new TypesInfo
                {
                    Id = LeaveDayType.TypeKbnSeq,
                    Text = LeaveDayType.TypeName,
                };
                VacationKinKyuTypeData.Add(VacationTypes);
            }
            // add type hope lable
            foreach (AppointmentLabel item in appointmentLabels)
            {
                TypesInfo ScheduleLabels = new TypesInfo
                {
                    Id = int.Parse(item.Id),
                    Text = item.Text,
                };
                ScheduleLabelData.Add(ScheduleLabels);
            }
            workHourGroups = StaffScheduleService.GetWorkHoursGroup(timeRangeNow[2], timeRangeNow[1], currentGroupSelected.GroupId).Result;
            GetListOfDisplaySettingScreen(SettingData);
            // init schedule display in current date
            ChangeDate(dateDisplay);
            DisplayScheduleByDate(dateDisplay);
            await _loading.HideAsync();
        }
        public void GenerateSelectedCalendar(List<CalendarSetModel> calendarInfo)
        {
            foreach (var item in calendarInfo)
            {
                if (!SelectedCalendarDict.ContainsKey(item.CalendarSeq))
                {
                    SelectedCalendarDict.Add(item.CalendarSeq, true);
                }
            }
        }
        public void GenerateBirthdayCommentDict()
        {
            SelectedBirthdayCommentDict.Add(1, true);
            SelectedBirthdayCommentDict.Add(2, true);
            SelectedBirthdayCommentDict.Add(3, true);
            SelectedBirthdayCommentDict.Add(4, true);
        }
        public List<string> GetTimeRangeNow(DateTime date)
        {
            var result = new List<string>();


            var startDate = new DateTime(date.AddMonths(-1).Year, date.AddMonths(-1).Month, 1);
            var endDate = new DateTime(date.AddMonths(1).Year, date.AddMonths(1).Month, DateTime.DaysInMonth(date.AddMonths(1).Year, date.AddMonths(1).Month));
            result.Add(startDate.ToString().Substring(0, 10).Replace("/", string.Empty));
            result.Add(endDate.ToString().Substring(0, 10).Replace("/", string.Empty));
            result.Add(date.AddDays(-27).ToString().Substring(0, 10).Replace("/", string.Empty));

            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            result.Add(firstDayOfMonth.ToString().Substring(0, 10).Replace("/", string.Empty));
            result.Add(lastDayOfMonth.ToString().Substring(0, 10).Replace("/", string.Empty));
            result.Add(date.AddDays(+2).ToString().Substring(0, 10).Replace("/", string.Empty));

            return result;
        }
        protected void GetListOfDisplaySettingScreen(LoadDisplaySetting SettingData)
        {
            // タイムゾーン
            TimeZones = TimeZoneInfo.GetSystemTimeZones().ToList();

            // 初期表示形式
            SettingData.DefaultDisplayType.Text = Lang["DefaultDisplayType" + SettingData.DefaultDisplayType.Value];
            for (int i = 0; i <= 1; i++)
            {
                DisplayTypes.Add(new NumberWithStringDisplay()
                {
                    Value = i,
                    Text = Lang["DefaultDisplayType" + i]
                });
            }

            // 週の開始日
            SettingData.WeekStartDay.Text = Lang["WeekStartDay" + SettingData.WeekStartDay.Value];
            for (int i = 0; i <= 6; i++)
            {
                WeekStartDays.Add(new NumberWithStringDisplay()
                {
                    Value = i,
                    Text = Lang["WeekStartDay" + i]
                });
            }

            // 表示する時間帯
            string Format = "{0:D2}:00";
            SettingData.DayStartTime.Text = SettingData.DayStartTime.Value == DisplaySettingConstants.DefaultDayStartTime ?
            Lang[DisplaySettingConstants.DefaultDayStartTime] : string.Format(Format, int.Parse(SettingData.DayStartTime.Value));
            DayStartHours.Add(new StringWithStringDisplay()
            {
                Value = DisplaySettingConstants.DefaultDayStartTime,
                Text = Lang[DisplaySettingConstants.DefaultDayStartTime]
            });
            for (int i = 0; i <= 23; i++)
            {
                DayStartHours.Add(new StringWithStringDisplay()
                {
                    Value = i.ToString(),
                    Text = string.Format(Format, i)
                });
            }
        }

        /// <summary>
        /// Get data schedule of staff or group when init 
        /// </summary>
        protected void ChangeDate(DateTime currentDate)
        {
            if (isReRender)
            {
                timeRangeNow = GetTimeRangeNow(currentDate);
                workHourGroups = StaffScheduleService.GetWorkHoursGroup(timeRangeNow[2], timeRangeNow[1], currentGroupSelected.GroupId).Result;
            }
            string JsonScheduleTypeData = Newtonsoft.Json.JsonConvert.SerializeObject(ScheduleYoteiTypeData);
            string JsonVacationTypeData = Newtonsoft.Json.JsonConvert.SerializeObject(VacationKinKyuTypeData);
            string JsonScheduleLabelTypeData = Newtonsoft.Json.JsonConvert.SerializeObject(ScheduleLabelData);
            string JsonStaffData = Newtonsoft.Json.JsonConvert.SerializeObject(StaffList);
            string JsonDisplaySettingData = Newtonsoft.Json.JsonConvert.SerializeObject(DisplaySettingData);
            TZConvert.TryWindowsToIana(DisplaySettingData.TimeZone.Id, out string CurrentTimeZone);
            if (string.IsNullOrEmpty(CurrentTimeZone))
            {
                CurrentTimeZone = DisplaySettingData.TimeZone.Id;
            }
            if (currentGroupSelected.GroupId == 0)
            {
                AppointmentLists = new List<AppointmentList>();
                AppointmentLists = ScheduleDataModelInTimezone(StaffScheduleService.GetAppointmentLists(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, new ClaimModel().TenantID, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, timeRangeNow[0], timeRangeNow[1], planTypes, appointmentLabels).Result, false, DisplaySettingData.TimeZone.Id);
                AppointmentLists = LoadScheduleBaseOnCalendarCheck(AppointmentLists, SelectedCalendarDict);
                AppointmentLists = LoadScheduleBaseOnDateCommentCheck(AppointmentLists, SelectedBirthdayCommentDict);
                string JsonSchedule = Newtonsoft.Json.JsonConvert.SerializeObject(AppointmentLists);
                string calendarJson = Newtonsoft.Json.JsonConvert.SerializeObject(CalendarSets);
                if (!isReRender)
                {
                    JSRuntime.InvokeAsync<string>("loadLibraryScript", "js/dx.all.js", "scheduleStaffMobile", JsonSchedule, calendarJson, JsonScheduleTypeData,
                         JsonVacationTypeData, JsonScheduleLabelTypeData, JsonStaffData, JsonDisplaySettingData, CurrentTimeZone, LangDictionary, Culture.Name, DotNetObjectReference.Create(this), new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, currentDate, true, currentYearMonth);
                }
                else
                {
                    JSRuntime.InvokeAsync<string>("loadPageScript", "staffScheduleMobilePage", "ReRenderStaffScheduleMobile", JsonSchedule, calendarJson, JsonScheduleTypeData,
                         JsonVacationTypeData, JsonScheduleLabelTypeData, JsonStaffData, JsonDisplaySettingData, CurrentTimeZone, LangDictionary, Culture.Name, DotNetObjectReference.Create(this), new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, currentDate, true, currentYearMonth);
                }
            }
            else
            {
                if (currentGroupSelected.CompanyId != 0)
                {
                    AppointmentListsGroup = ScheduleDataModelInTimezone(StaffScheduleService.GetAppointmentListsGroup(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, currentGroupSelected.GroupId, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, timeRangeNow[0], timeRangeNow[1], planTypes, appointmentLabels).Result, true, DisplaySettingData.TimeZone.Id);

                    string calendarJson = Newtonsoft.Json.JsonConvert.SerializeObject(CalendarSets);
                    string JsonObjectDict = Newtonsoft.Json.JsonConvert.SerializeObject(AppointmentListsGroup);
                    if (!isReRender)
                    {
                        JSRuntime.InvokeAsync<string>("loadLibraryScript", "js/dx.all.js", "scheduleStaffMobile", JsonObjectDict, calendarJson, JsonScheduleTypeData,
                            JsonVacationTypeData, JsonScheduleLabelTypeData, JsonStaffData, JsonDisplaySettingData, CurrentTimeZone, LangDictionary, Culture.Name, DotNetObjectReference.Create(this), new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, currentDate, false, currentYearMonth);
                    }
                    else
                    {
                        JSRuntime.InvokeAsync<string>("loadPageScript", "staffScheduleMobilePage", "ReRenderStaffScheduleMobile", JsonObjectDict, calendarJson, JsonScheduleTypeData,
                            JsonVacationTypeData, JsonScheduleLabelTypeData, JsonStaffData, JsonDisplaySettingData, CurrentTimeZone, LangDictionary, Culture.Name, DotNetObjectReference.Create(this), new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, currentDate, false, currentYearMonth);
                    }
                }
                else
                {
                    AppointmentListsGroup = ScheduleDataModelInTimezone(StaffScheduleService.GetAppointmentListsCustomGroup(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, currentGroupSelected.GroupId, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, timeRangeNow[0], timeRangeNow[1], planTypes, appointmentLabels).Result, true, DisplaySettingData.TimeZone.Id);

                    string calendarJson = Newtonsoft.Json.JsonConvert.SerializeObject(CalendarSets);
                    string JsonObjectDict = Newtonsoft.Json.JsonConvert.SerializeObject(AppointmentListsGroup);
                    if (!isReRender)
                    {
                        JSRuntime.InvokeAsync<string>("loadLibraryScript", "js/dx.all.js", "scheduleStaffMobile", JsonObjectDict, calendarJson, JsonScheduleTypeData,
                            JsonVacationTypeData, JsonScheduleLabelTypeData, JsonStaffData, JsonDisplaySettingData, CurrentTimeZone, LangDictionary, Culture.Name, DotNetObjectReference.Create(this), new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, currentDate, false, currentYearMonth);
                    }
                    else
                    {
                        JSRuntime.InvokeAsync<string>("loadPageScript", "staffScheduleMobilePage", "ReRenderStaffScheduleMobile", JsonObjectDict, calendarJson, JsonScheduleTypeData,
                            JsonVacationTypeData, JsonScheduleLabelTypeData, JsonStaffData, JsonDisplaySettingData, CurrentTimeZone, LangDictionary, Culture.Name, DotNetObjectReference.Create(this), new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, currentDate, false, currentYearMonth);
                    }
                }
            }
            currentYearMonth = currentDate.ToString("yyyyMM");
        }

        private List<AppointmentList> LoadScheduleBaseOnCalendarCheck(List<AppointmentList> appointments, Dictionary<int, bool> calendarChecks)
        {
            List<AppointmentList> unloadAppointments = new List<AppointmentList>();
            foreach (var item in calendarChecks)
            {
                if (item.Value == false)
                {
                    unloadAppointments = appointments.Where(x => x.DataType == 1 && x.YoteiInfo != null && x.YoteiInfo.CalendarSeq == item.Key).ToList();
                    foreach (var removeItem in unloadAppointments)
                    {
                        appointments.Remove(removeItem);
                    }
                }
            }

            return appointments;
        }

        private List<AppointmentList> LoadScheduleBaseOnDateCommentCheck(List<AppointmentList> appointments, Dictionary<int, bool> dateCommentChecks)
        {
            List<AppointmentList> unloadAppointments = new List<AppointmentList>();
            foreach (var item in dateCommentChecks)
            {
                if (item.Value == false)
                {
                    if (item.Key == 1)
                    {
                        unloadAppointments = appointments.Where(x => x.DataType == 5).ToList();
                        foreach (var removeItem in unloadAppointments)
                        {
                            appointments.Remove(removeItem);
                        }
                    }
                    else if (item.Key == 2)
                    {
                        unloadAppointments = appointments.Where(x => x.DataType == 4 && x.BirthDayInfo != null && (x.BirthDayInfo.SyainSyokumuKbn == 1 || x.BirthDayInfo.SyainSyokumuKbn == 2)).ToList();
                        foreach (var removeItem in unloadAppointments)
                        {
                            appointments.Remove(removeItem);
                        }
                    }
                    else if (item.Key == 3)
                    {
                        unloadAppointments = appointments.Where(x => x.DataType == 4 && x.BirthDayInfo != null && (x.BirthDayInfo.SyainSyokumuKbn == 3 || x.BirthDayInfo.SyainSyokumuKbn == 4)).ToList();
                        foreach (var removeItem in unloadAppointments)
                        {
                            appointments.Remove(removeItem);
                        }
                    }
                    else if (item.Key == 4)
                    {
                        unloadAppointments = appointments.Where(x => x.DataType == 4 && x.BirthDayInfo != null && (x.BirthDayInfo.SyainSyokumuKbn == 5)).ToList();
                        foreach (var removeItem in unloadAppointments)
                        {
                            appointments.Remove(removeItem);
                        }
                    }

                }
            }

            return appointments;
        }
        /// <summary>
        /// Get timezone for calendar
        /// </summary>
        /// <param name="dataModels"></param>
        /// <param name="timezoneId"></param>
        /// <returns></returns>
        public AppointmentList ScheduleDataInTimezone(AppointmentList data, string timezoneId)
        {
            data.StartDate = TimeZoneInfo.ConvertTimeToUtc(data.StartDateInDatetimeType).ToString("yyyy-MM-ddTHH:mm:ssZ");
            data.EndDate = TimeZoneInfo.ConvertTimeToUtc(data.EndDateInDatetimeType).ToString("yyyy-MM-ddTHH:mm:ssZ");
            TimeZoneInfo infotime = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
            data.StartDateDisplay = TimeZoneInfo.ConvertTimeFromUtc(data.StartDateDisplay.ToUniversalTime(), infotime);
            data.EndDateDisplay = TimeZoneInfo.ConvertTimeFromUtc(data.EndDateDisplay.ToUniversalTime(), infotime);
            data.StartDateInDatetimeType = TimeZoneInfo.ConvertTimeFromUtc(data.StartDateInDatetimeType.ToUniversalTime(), infotime);
            data.EndDateInDatetimeType = TimeZoneInfo.ConvertTimeFromUtc(data.EndDateInDatetimeType.ToUniversalTime(), infotime);


            return data;
        }
        [JSInvokable]
        public void setAppointmentCurrentDisplay(string staffData, DateTime startDate, DateTime endDate, bool isStaff)
        {
            TimeZoneInfo infotime = TimeZoneInfo.FindSystemTimeZoneById(DisplaySettingData.TimeZone.Id);
            startDate = TimeZoneInfo.ConvertTimeFromUtc(startDate.ToUniversalTime(), infotime);
            endDate = TimeZoneInfo.ConvertTimeFromUtc(endDate.ToUniversalTime(), infotime);
            displayDataFirst = true;
            AppointmentList staff = Newtonsoft.Json.JsonConvert.DeserializeObject<AppointmentList>(staffData);
            if (!(staff.SyainCdSeq != new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq && staff.DataType == 1 && staff.DisplayType == 90))
            {
                staff = ScheduleDataInTimezone(staff, DisplaySettingData.TimeZone.Id);
                string key = startDate.ToString("yyyyMMddHHmm") + endDate.ToString("yyyyMMddHHmm") + staff.DataType + staff.DisplayType + staff.SyainCdSeq + staff.ScheduleIdMobileDP;
                if (!currentAppoitmentList.ContainsKey(key))
                {
                    currentAppoitmentList.Add(key, staff);
                }
                if (startDate.ToString("yyyyMMdd") != endDate.ToString("yyyyMMdd"))
                {
                    double dayInterval = (endDate - startDate).TotalDays;
                    DateTime startDateNext = startDate.AddDays(1);
                    while (dayInterval > 0)
                    {
                        string keyDuplicate = startDateNext.ToString("yyyyMMddHHmm") + endDate.ToString("yyyyMMddHHmm") + staff.DataType + staff.DisplayType + staff.SyainCdSeq + staff.ScheduleIdMobileDP;
                        if (!currentAppoitmentList.ContainsKey(keyDuplicate))
                        {
                            currentAppoitmentList.Add(keyDuplicate, staff);
                        }
                        startDateNext = startDateNext.AddDays(1);
                        dayInterval = (endDate - startDateNext).TotalDays;
                    }
                }
            }
            stateHaschangeAfterReady();
        }

        /// <summary>
        /// show schedule of staff or group base on date
        /// </summary>
        /// <param name="currentDate"></param>
        [JSInvokable]
        public void showScheduleEachStaff(String data)
        {
            DateTime currentDate = Newtonsoft.Json.JsonConvert.DeserializeObject<DateTime>(data);
            TimeZoneInfo infotime = TimeZoneInfo.FindSystemTimeZoneById(DisplaySettingData.TimeZone.Id);
            currentDate = TimeZoneInfo.ConvertTimeFromUtc(currentDate.ToUniversalTime(), infotime);
            GetAndDisplayScheduleByDate(currentDate);
        }
        [JSInvokable]
        public void stateHaschangeAfterReady()
        {
            if (currentGroupSelected.GroupId == 0)
            {
                currentListAppoitmentStaff = currentAppoitmentList.Where(e => e.Key.Substring(0, 8) == dateDisplay.ToString("yyyyMMdd"))
                                                                    .Select(y => y.Value).OrderByDescending(y => y.AllDayKbn).ThenBy(y => y.StartDateDisplay.ToString("HHmm")).ToList();
            }
            else
            {
                currentListAppoitmentStaff = currentAppoitmentList.Where(e => e.Key.Substring(0, 8) == dateDisplay.ToString("yyyyMMdd"))
                                                                    .Select(y => y.Value).ToList();
                CurrentScheduleDataModelDict = StaffScheduleService.RangeByStaff(currentListAppoitmentStaff, StaffList).Result;
            }
        }
        /// <summary>
        /// show next month when click day of other month
        /// </summary>
        /// <param name="args"></param>
        [JSInvokable]
        public async Task<List<string>> otherMonth(String newValue)
        {
            try
            {
                List<string> result = new List<string>();
                await _loading.ShowAsync();
                if (!string.IsNullOrEmpty(newValue))
                {
                    int year = 0;
                    if (newValue.Length > 3 && int.TryParse(newValue.Substring(1, 4), out year))
                    {
                        DateTime currentDate = Newtonsoft.Json.JsonConvert.DeserializeObject<DateTime>(newValue);
                        TimeZoneInfo infotime = TimeZoneInfo.FindSystemTimeZoneById(DisplaySettingData.TimeZone.Id);
                        currentDate = TimeZoneInfo.ConvertTimeFromUtc(currentDate.ToUniversalTime(), infotime);
                        if (currentDate.ToString("yyyyMM") != currentYearMonth)
                        {
                            dateDisplay = currentDate;
                            currentYearMonth = currentDate.ToString("yyyyMM");
                            timeRangeNow = GetTimeRangeNow(currentDate);
                            workHourGroups = StaffScheduleService.GetWorkHoursGroup(timeRangeNow[2], timeRangeNow[1], currentGroupSelected.GroupId).Result;
                            if (currentGroupSelected.GroupId == 0)
                            {
                                AppointmentLists = new List<AppointmentList>();
                                AppointmentLists = ScheduleDataModelInTimezone(StaffScheduleService.GetAppointmentLists(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, new HassyaAllrightCloud.Domain.Dto.ClaimModel().TenantID, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, timeRangeNow[0], timeRangeNow[1], planTypes, appointmentLabels).Result, false, DisplaySettingData.TimeZone.Id);
                                AppointmentLists = LoadScheduleBaseOnCalendarCheck(AppointmentLists, SelectedCalendarDict);
                                AppointmentLists = LoadScheduleBaseOnDateCommentCheck(AppointmentLists, SelectedBirthdayCommentDict);
                            }
                            else
                            {
                                if (currentGroupSelected.CompanyId != 0)
                                {
                                    AppointmentListsGroup = ScheduleDataModelInTimezone(StaffScheduleService.GetAppointmentListsGroup(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, currentGroupSelected.GroupId, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, timeRangeNow[0], timeRangeNow[1], planTypes, appointmentLabels).Result, true, DisplaySettingData.TimeZone.Id);
                                }
                                else
                                {
                                    AppointmentListsGroup = ScheduleDataModelInTimezone(StaffScheduleService.GetAppointmentListsCustomGroup(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, currentGroupSelected.GroupId, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, timeRangeNow[0], timeRangeNow[1], planTypes, appointmentLabels).Result, true, DisplaySettingData.TimeZone.Id);
                                }
                            }
                        }

                    }
                }
                if (currentGroupSelected.GroupId == 0)
                {
                    var scheduledata = Newtonsoft.Json.JsonConvert.SerializeObject(AppointmentLists);
                    result.Add(scheduledata);
                    await _loading.HideAsync();
                    return result;
                }
                else
                {
                    var scheduledata = Newtonsoft.Json.JsonConvert.SerializeObject(AppointmentListsGroup);
                    result.Add(scheduledata);
                    await _loading.HideAsync();
                    return result;
                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
                await _loading.HideAsync();
                return null;
            }
        }
        [JSInvokable]
        public void isAddAppoitment(string data)
        {
            isAdd = Newtonsoft.Json.JsonConvert.DeserializeObject<bool>(data);

        }
        /// <summary>
        /// Display schedule each staff base on date
        /// </summary>
        /// <param name="currentDate"></param>
        /// <returns></returns>
        protected void DisplayScheduleByDate(DateTime currentDate)
        {
            DateTime currentDateReset = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day);
            dateDisplay = currentDateReset;
            HightLightDate(dateDisplay);
        }
        protected void GetAndDisplayScheduleByDate(DateTime currentDate)
        {
            DateTime currentDateReset = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day);
            dateDisplay = currentDateReset;
            if (currentGroupSelected.GroupId == 0)
            {
                currentListAppoitmentStaff = currentAppoitmentList.Where(e => e.Key.Substring(0, 8) == dateDisplay.ToString("yyyyMMdd"))
                                                                    .Select(y => y.Value).OrderByDescending(y => y.AllDayKbn).ThenBy(y => y.StartDateDisplay.ToString("HHmm")).ToList();
            }
            else
            {
                currentListAppoitmentStaff = currentAppoitmentList.Where(e => e.Key.Substring(0, 8) == dateDisplay.ToString("yyyyMMdd"))
                                                                    .Select(y => y.Value).ToList();
                CurrentScheduleDataModelDict = StaffScheduleService.RangeByStaff(currentListAppoitmentStaff, StaffList).Result;
            }
            HightLightDate(dateDisplay);
        }
        private void HightLightDate(DateTime _curentDateHightLight)
        {
            if (_curentDateHightLight.Day < 15)
            {
                isFirstDay = int.Parse(_curentDateHightLight.ToString("yyyyMM")) > int.Parse(currentYearMonth) ? false : true;
            }
            else
            {
                isFirstDay = int.Parse(_curentDateHightLight.ToString("yyyyMM")) == int.Parse(currentYearMonth) ? false : true;
            }
            TimeZoneInfo tzinfo = TimeZoneInfo.Local;
            TimeZoneInfo.ConvertTimeFromUtc(_curentDateHightLight.ToUniversalTime(), tzinfo);
            StateHasChanged();
            JSRuntime.InvokeVoidAsync("loadPageScript", "staffScheduleMobilePage", "displayCurrentDate", _curentDateHightLight.Day.ToString("00"), isFirstDay);
        }
        /// <summary>
        /// Click button plus on header calendar show popup add appointment mobile
        /// </summary>
        /// <param name="args"></param>
        protected void onClickPlus(MouseEventArgs args)
        {
            JSRuntime.InvokeVoidAsync("loadPageScript", "staffScheduleMobilePage", "showAppointmentPopupMobile");
        }

        /// <summary>
        /// Click button previous day change date of calendar
        /// </summary>
        /// <param name="args"></param>
        protected async Task PreviousDay(MouseEventArgs args)
        {
            await _loading.ShowAsync();
            DateTime currentDate;
            currentDate = dateDisplay.AddDays(-1);
            if (currentDate.ToString("yyyyMM") != currentYearMonth)
            {
                isReRender = true;
                ChangeDate(currentDate);
                DisplayScheduleByDate(currentDate);
            }
            else
            {
                isReRender = false;
                GetAndDisplayScheduleByDate(currentDate);
            }
            await _loading.HideAsync();
        }

        /// <summary>
        /// Click button next day set next date for calendar
        /// </summary>
        /// <param name="args"></param>
        protected async Task NextDay(MouseEventArgs args)
        {
            await _loading.ShowAsync();
            DateTime currentDate;
            currentDate = dateDisplay.AddDays(1);
            if (currentDate.ToString("yyyyMM") != currentYearMonth)
            {
                isReRender = true;
                ChangeDate(currentDate);
                DisplayScheduleByDate(currentDate);
            }
            else
            {
                isReRender = false;
                GetAndDisplayScheduleByDate(currentDate);
            }
            await _loading.HideAsync();
        }

        /// <summary>
        /// Click sort follow by staff
        /// typeDisplayGrid = 1;
        /// </summary>
        protected async Task SortFollowByStaff(MouseEventArgs args)
        {
            typeDisplayGrid = 1;
            firstSortGroupRender = true;
            if (currentGroupSelected.GroupId != 0)
            {
                CurrentScheduleDataModelDict = await StaffScheduleService.RangeByStaff(currentListAppoitmentStaff, StaffList);
            }
            StateHasChanged();
        }

        /// <summary>
        /// Click sort follow by time
        /// typeDisplayGrid = 2;
        /// </summary>
        protected async Task SortFollowByTime(MouseEventArgs args)
        {
            typeDisplayGrid = 2;
            if (currentGroupSelected.GroupId != 0)
            {
                CurrentScheduleDataModelDict = await StaffScheduleService.RangeByTime(currentListAppoitmentStaff, StaffList);
            }
            StateHasChanged();
        }
        /// <summary>
        /// when click button ShowTimeOfStaff get time and show grid
        /// typeDisplayGrid = 3
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected async Task ShowTimeOfStaff(MouseEventArgs args)
        {
            typeDisplayGrid = 3;
            if (currentGroupSelected.CompanyId != 0)
            {
                workHourGroups = await StaffScheduleService.GetWorkHoursGroup(timeRangeNow[2], timeRangeNow[1], currentGroupSelected.GroupId);
                StaffListOfGroup = SyainService.GetByGroup(currentGroupSelected.GroupId, timeRangeNow[5], workHourGroups);
            }
            else
            {
                workHourGroups = await StaffScheduleService.GetWorkHoursCustomGroup(timeRangeNow[2], timeRangeNow[1], currentGroupSelected.GroupId);
                StaffListOfGroup = SyainService.GetByCustomGroup(currentGroupSelected.GroupId, timeRangeNow[5], workHourGroups);
            }
            StateHasChanged();
        }
        /// <summary>
        /// Show type calendar choosen
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected void SetiingTypeCalendar(MouseEventArgs args)
        {
            PopupIsSettingTypeCalendar = true;
            StateHasChanged();
        }
        /// <summary>
        /// hide calendar when click day
        /// </summary>
        /// <param name="args"></param>
        protected async Task hideScheduleCalenda(MouseEventArgs args)
        {
            firstDisplayRender = true;
            displayDay = true;
            await InvokeAsync(StateHasChanged);
        }
        /// <summary>
        /// show calendar when click day
        /// </summary>
        /// <param name="args"></param>
        protected async Task showScheduleCalenda(MouseEventArgs args)
        {
            firstDisplayRender = true;
            displayDay = false;
            await InvokeAsync(StateHasChanged);
        }
        /// <summary>
        /// display today
        /// </summary>
        /// <param name="args"></param>
        protected async Task backToDay(MouseEventArgs args)
        {
            await _loading.ShowAsync();
            DateTime currentDate;
            currentDate = DateTime.Now;
            ChangeDate(currentDate);
            DisplayScheduleByDate(currentDate);
            await _loading.HideAsync();
        }
        /// <summary>
        /// refresh schedule
        /// </summary>
        /// <param name="args"></param>
        protected async Task RefreshNewSchedule(MouseEventArgs args)
        {
            await refreshSchedule();
        }
        [JSInvokable]
        public async Task refreshSchedule()
        {
            await _loading.ShowAsync();
            currentAppoitmentList = new Dictionary<string, AppointmentList>();
            isReRender = true;
            ChangeDate(dateDisplay);
            DisplayScheduleByDate(dateDisplay);
            await _loading.HideAsync();
        }
        /// <summary>
        /// redirect to group list
        /// </summary>
        /// <param name="args"></param>
        protected void RedirectToGroupList(MouseEventArgs args)
        {
            NavigationManager.NavigateTo("/StaffScheduleMobileOrganization");
        }
        /// <summary>
        /// Save appoitment
        /// </summary>
        /// <param name="data"></param>
        [JSInvokable]
        public async Task SaveAppointment(string data)
        {
            try
            {
                StaffScheduleData schedule = Newtonsoft.Json.JsonConvert.DeserializeObject<StaffScheduleData>(data);
                schedule = ConvertUtcToLocalTime(schedule);
                if (schedule.DisplayType == 1 && schedule.IsSendNoti)
                {
                    NotificationResult NotificationResult = await NotificationTemplateService.SendApplication(new HassyaAllrightCloud.Domain.Dto.ClaimModel().TenantID, NotificationSendMethod.Both, CodeKbnForNotification.Application, schedule.StaffIdToSend, schedule.StartDate, CreateReplaceDictionary(schedule));
                    IsSendMailError = NotificationResult.SendResultKbn == NotificationResultClassification.Failed;
                }
                var save = StaffScheduleService.SaveStaffSchedule(schedule).Result;
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }

        }
        [JSInvokable]
        public async Task UpdateAppointment(string data)
        {
            try
            {
                StaffScheduleData schedule = Newtonsoft.Json.JsonConvert.DeserializeObject<StaffScheduleData>(data);
                schedule = ConvertUtcToLocalTime(schedule);
                if (schedule.DisplayType == 1 && schedule.IsSendNoti)
                {
                    NotificationResult NotificationResult = await NotificationTemplateService.SendApplication(new ClaimModel().TenantID, NotificationSendMethod.Both, CodeKbnForNotification.Application, schedule.StaffIdToSend, schedule.StartDate, CreateReplaceDictionary(schedule));
                    IsSendMailError = NotificationResult.SendResultKbn == NotificationResultClassification.Failed;
                }
                // isAdd new
                if (isAdd)
                {
                    var save = StaffScheduleService.SaveStaffSchedule(schedule).Result;
                }
                else
                {
                    var save = StaffScheduleService.UpdateStaffSchedule(schedule).Result;
                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }
        /// <summary>
        /// show appoitment when click table
        /// </summary>
        /// <param name="item"></param>
        public void showAppoitmentStaff(AppointmentList item, bool isShowStaff)
        {
            staffParam = new AppointmentList();
            staffParam = item;
            if (!isDeleted)
            {
                if (item.DataType == 2 || item.DataType == 3)
                {
                    //schedule detail for haiin and kinkyuj
                    PopupDetailOther = true;
                }
                else if (item.DataType == 1)
                {
                    if (item.YoteiInfo.CreatorCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq)
                    {
                        if (item.YoteiInfo.YoteiShoKbn != "3")
                        {
                            string JsonObjectItem = Newtonsoft.Json.JsonConvert.SerializeObject(item);
                            JSRuntime.InvokeAsync<string>("loadPageScript", "staffScheduleMobilePage", "showEditStaffSchedule", JsonObjectItem, DotNetObjectReference.Create(this));
                        }
                        else
                        {
                            PopupDetail = true;
                        }
                    }
                    else
                    {
                        PopupDetailConfirm = true;
                    }
                }
            }
            else
            {
                isDeleted = false;
            }
            StateHasChanged();
        }
        private StaffScheduleData ConvertUtcToLocalTime(StaffScheduleData appointmentList)
        {
            appointmentList.StartDate = appointmentList.StartDate.ToLocalTime();
            appointmentList.EndDate = appointmentList.EndDate.ToLocalTime();

            return appointmentList;
        }

        /// <summary>
        /// show appoitment when click table
        /// </summary>
        /// <param name="item"></param>
        public void showAppoitmentGroup(AppointmentList item, bool isShowStaff)
        {
            staffParam = new AppointmentList();
            staffParam = item;
            if (!isDeleted)
            {
                if (item.DataType == 2 || item.DataType == 3)
                {
                    PopupDetailOther = true;
                }
                else if (item.DataType == 1)
                {
                    if (item.YoteiInfo.CreatorCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq)
                    {
                        if (item.YoteiInfo.YoteiShoKbn != "3")
                        {
                            string JsonObjectItem = Newtonsoft.Json.JsonConvert.SerializeObject(item);
                            JSRuntime.InvokeAsync<string>("loadPageScript", "staffScheduleMobilePage", "showEditStaffSchedule", JsonObjectItem, DotNetObjectReference.Create(this));
                        }
                        else
                        {
                            PopupDetail = true;
                        }
                    }
                    else
                    {
                        bool check = item.YoteiInfo.ParticipantByTimeArray.Where(x => x.ParticipantArray.Where(y => y.SyainCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq).Count() > 0).Count() > 0 ? true : false;
                        // if current SyainCdSeq in participant schedule list => show confirm form
                        if (check)
                        {
                            PopupDetailConfirm = true;
                        }
                        else
                        {
                            // Nếu cho schedule loại cho người khác xem mới cho xem không thì không hiển thị.
                            PopupDetail = true;
                        }
                    }
                }
            }
            else
            {
                isDeleted = false;
            }
            StateHasChanged();
        }
        public void deleteAppoitmentStaff(AppointmentList item)
        {
            isDeleted = true;
            var save = StaffScheduleService.DeleteStaffSchedule(item);
            string JsonObjectItem = Newtonsoft.Json.JsonConvert.SerializeObject(item);
            JSRuntime.InvokeAsync<string>("loadPageScript", "staffScheduleMobilePage", "deleteStaffSchedule", JsonObjectItem);
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
        /// <summary>
        /// Get Company and group info
        /// </summary>
        /// <param name="selectedGroupScheduleInfo"></param>
        public void GenerateSelectedGroup(List<GroupScheduleInfo> selectedGroupScheduleInfo)
        {
            foreach (var item in selectedGroupScheduleInfo)
            {
                if (!SelectedGroupScheduleDict.ContainsKey(item.CompanyId))
                {
                    Dictionary<int, bool> CurrentUserDict = new Dictionary<int, bool>();
                    CurrentUserDict.Add(item.GroupId, true);
                    SelectedGroupScheduleDict.Add(item.CompanyId, CurrentUserDict);
                }
                SelectedGroupScheduleDict[item.CompanyId][item.GroupId] = true;
            }
        }

        public List<AppointmentList> ScheduleDataModelInTimezone(List<AppointmentList> dataModels, bool isGroupData, string timezoneId)
        {
            foreach (var item in dataModels)
            {
                item.StartDate = TimeZoneInfo.ConvertTimeToUtc(item.StartDateInDatetimeType).ToString("yyyy-MM-ddTHH:mm:ssZ");
                item.EndDate = TimeZoneInfo.ConvertTimeToUtc(item.EndDateInDatetimeType).ToString("yyyy-MM-ddTHH:mm:ssZ");
                if (isGroupData)
                {
                    TimeZoneInfo infotime = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
                    item.StartDateDisplay = TimeZoneInfo.ConvertTimeFromUtc(item.StartDateDisplay.ToUniversalTime(), infotime);
                    item.EndDateDisplay = TimeZoneInfo.ConvertTimeFromUtc(item.EndDateDisplay.ToUniversalTime(), infotime);
                }
                else
                {
                    TimeZoneInfo infotime = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
                    item.StartDateDisplay = TimeZoneInfo.ConvertTimeFromUtc(item.StartDateInDatetimeType.ToUniversalTime(), infotime);
                    item.EndDateDisplay = TimeZoneInfo.ConvertTimeFromUtc(item.EndDateInDatetimeType.ToUniversalTime(), infotime);
                }
            }

            return dataModels;
        }
        protected Dictionary<string, string> CreateReplaceDictionary(StaffScheduleData data)
        {
            Dictionary<string, string> Result = new Dictionary<string, string>();
            Result.Add("{Applicant}", new HassyaAllrightCloud.Domain.Dto.ClaimModel().Name);
            LoadLeaveDayType loadLeaveDayType = LeaveDayTypeList.Where(item => item.TypeKbnSeq == data.DisplayType).FirstOrDefault();
            if (loadLeaveDayType != null)
            {
                Result.Add("{LeaveType}", loadLeaveDayType.TypeName);
            }
            Result.Add("{Title}", data.Text);
            Result.Add("{StartDate}", data.StartDate.ToString(Lang["FormatStringDate"]));
            Result.Add("{StartTime}", data.StartDate.ToString(Lang["FormatStringHour"]));
            Result.Add("{EndDate}", data.EndDate.ToString(Lang["FormatStringDate"]));
            Result.Add("{EndTime}", data.EndDate.ToString(Lang["FormatStringHour"]));
            Result.Add("{Description}", data.Description);
            return Result;
        }
        protected async void backToScheduleByCalendarType()
        {
            PopupIsSettingTypeCalendar = false;
            await _loading.ShowAsync();
            ChangeDate(dateDisplay);
            DisplayScheduleByDate(dateDisplay);
            await _loading.HideAsync();
        }
        protected void showCalendarTypeSetting()
        {
            PopupIsSettingTypeCalendar = true;
            StateHasChanged();
        }

        #endregion
    }
}
