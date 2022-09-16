using HassyaAllrightCloud.Commons.Helpers;
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
    public class StaffScheduleMobileConfirmBase : ComponentBase
    {
        #region Inject
        [Inject]
        protected IStringLocalizer<StaffScheduleMobile> Lang { get; set; }
        [Inject]
        IStaffScheduleService StaffScheduleService { get; set; }
        [Inject]
        IScheduleGroupDataService StaffGroupDataService { get; set; }

        #endregion

        #region parameter
        [Parameter]
        public AppointmentList scheduleStaffData { get; set; }
        [Parameter]
        public DateTime currentDate { get; set; }
        [Parameter]
        public EventCallback<DateTime> ChangeDate { get; set; }
        #endregion

        #region Properties And Variable
        protected AppointmentList staffScheduleDataDisplay { get; set; } = new AppointmentList();
        protected BookedScheduleFeedback AppointmentDetailFB = new BookedScheduleFeedback();
        protected Dictionary<string, int> feedBackGroup = new Dictionary<string, int>();
        protected string displayRecurrenceRule { get; set; }
        protected string formatDate = "yyyy年MM月dd日(ddd) HH:mm";
        protected string formatRecur = "yyyy年MM月dd日";

        #endregion

        #region Function
        protected override async Task OnInitializedAsync()
        {
            CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            if (cultureInfo.Name != "ja-JP")
            {
                formatDate = "yyyy/MM/dd(ddd) HH:mm";
                formatRecur = "yyyy/MM/dd";
            }

            if (scheduleStaffData != null)
            {
                staffScheduleDataDisplay = scheduleStaffData;
                if (!string.IsNullOrEmpty(staffScheduleDataDisplay.RecurrenceRule))
                {
                    displayRecurrenceRule = await StaffScheduleService.GetDisPlayRecurrenceRule(staffScheduleDataDisplay.RecurrenceRule, Lang, formatRecur);
                }
                AppointmentDetailFB =  StaffGroupDataService.GetBookedScheduleFeedback(staffScheduleDataDisplay).Result;
            }
        }
        protected void SendDecide(int Value)
        {
            var result = StaffGroupDataService.SubmitScheduleFeedback(staffScheduleDataDisplay, Value).Result;
            AppointmentDetailFB = StaffGroupDataService.GetBookedScheduleFeedback(staffScheduleDataDisplay).Result;
            StateHasChanged();
        }
        #endregion
    }
}
