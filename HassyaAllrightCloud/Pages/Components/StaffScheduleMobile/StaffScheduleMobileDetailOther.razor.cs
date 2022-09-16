using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using SharedLibraries.UI.Services;
using SharedLibraries.Utility.Constant;
using SharedLibraries.Utility.Exceptions;
using SharedLibraries.Utility.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
namespace HassyaAllrightCloud.Pages
{
    public class StaffScheduleMobileDetailOtherBase : ComponentBase
    {
        #region Inject
        [Inject]
        protected IStringLocalizer<StaffScheduleMobile> Lang { get; set; }
        [Inject]
        IStaffScheduleService StaffScheduleService { get; set; }
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }
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
        protected bool isShowConfirm { get; set; } = false;
        #endregion

        #region Function
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await Task.Run(() =>
                {
                    InvokeAsync(StateHasChanged).Wait();
                    JSRuntime.InvokeVoidAsync("loadPageScript", "StaffScheduleMobileComponentPage");
                });
            }
        }
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
                if (!string.IsNullOrEmpty(scheduleStaffData.RecurrenceRule))
                {
                    displayRecurrenceRule = await StaffScheduleService.GetDisPlayRecurrenceRule(scheduleStaffData.RecurrenceRule, Lang, formatRecur);
                }
                isShowConfirm = scheduleStaffData.DataType == 3 && scheduleStaffData.SyainCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq && scheduleStaffData.HaiinInfo.ReadKbn != 1 ? true : false;
            }
        }
        protected void RejectToStaffScheduleMB()
        {

        }
        protected async void btnSeen_click()
        {
            isShowConfirm = await StaffScheduleService.SaveHaiin(scheduleStaffData);
            StateHasChanged();
        }
        protected async Task DownLoadFile(AttachFile item)
        {

        }
        #endregion
    }
}
