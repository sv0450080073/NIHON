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
    public class StaffScheduleMobileDetailsBase : ComponentBase
    {
        #region Inject
        [Inject]
        protected IStringLocalizer<StaffScheduleMobile> Lang { get; set; }
        [Inject]
        IStaffScheduleService StaffScheduleService { get; set; }
        [Inject]
        IScheduleGroupDataService StaffGroupDataService { get; set; }
        [Inject]
        NavigationManager NavigationManager { get; set; }

        #endregion

        #region parameter
        [Parameter]
        public AppointmentList scheduleStaffData { get; set; }
        [Parameter]
        public DateTime currentDate { get; set; }
        #endregion

        #region Properties And Variable
        protected string displayRecurrenceRule { get; set; }
        protected string formatDate = "yyyy年MM月dd日(ddd) HH:mm";
        protected string formatRecur = "yyyy年MM月dd日";
        protected bool isShowFooter = false;
        #endregion

        #region Function
        protected override async Task OnInitializedAsync()
        {
            CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            if (cultureInfo.Name != "ja-JP")
            {
                formatRecur = "yyyy/MM/dd";
                formatDate = "yyyy/MM/dd(ddd) HH:mm";
            }

            if (scheduleStaffData != null)
            {
                if (!string.IsNullOrEmpty(scheduleStaffData.RecurrenceRule))
                {
                    displayRecurrenceRule = await StaffScheduleService.GetDisPlayRecurrenceRule(scheduleStaffData.RecurrenceRule, Lang, formatRecur);
                }
                if (scheduleStaffData.DataType == 1 && scheduleStaffData.YoteiInfo.YoteiShoKbn == "3")
                {
                    isShowFooter = true;
                }
            }
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
        #endregion
    }
}
