using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Pages
{
    public class CalendarMobileTypeSettingBase : ComponentBase
    {
        #region Inject
        [Inject]
        protected IStringLocalizer<StaffScheduleMobile> Lang { get; set; }
        [Inject]
        IStaffScheduleService StaffScheduleService { get; set; }
        #endregion
        #region parameter
        [Parameter]
        public DateTime currentDate { get; set; }
        [Parameter]
        public EventCallback<DateTime> ChangeDate { get; set; }
        [Parameter]
        public List<CalendarSetModel> CalendarSets { get; set; }
        [Parameter]
        public Dictionary<int, bool> SelectedCalendarDict { get; set; }
        [Parameter]
        public Dictionary<int, bool> SelectedBirthdayCommentDict { get; set; }
        [Parameter] public EventCallback<Dictionary<int, bool>> SelectedCalendarDictChanged { get; set; }
        [Parameter] public EventCallback<Dictionary<int, bool>> SelectedBirthdayCommentDictChanged { get; set; }
        #endregion

        #region Properties And Variable

        #endregion

        #region Function
        public async void CheckCalendar(bool value, CalendarSetModel calendarSet)
        {
            SelectedCalendarDict[calendarSet.CalendarSeq] = value;
            await SelectedCalendarDictChanged.InvokeAsync(SelectedCalendarDict);
            await InvokeAsync(StateHasChanged);
        }
        public async void CheckDateCommnet(bool value, int type)
        {
            SelectedBirthdayCommentDict[type] = value;
            await SelectedBirthdayCommentDictChanged.InvokeAsync(SelectedBirthdayCommentDict);
            await InvokeAsync(StateHasChanged);
        }
        #endregion
    }
}
