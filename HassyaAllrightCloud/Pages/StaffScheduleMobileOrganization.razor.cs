using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using Microsoft.JSInterop;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.IService;

namespace HassyaAllrightCloud.Pages
{
    public class StaffScheduleMobileOrganizationBase : ComponentBase
    {
        #region Inject
        [Inject]
        protected IStringLocalizer<StaffScheduleMobile> Lang { get; set; }
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }
        [Inject]
        IStaffScheduleService StaffScheduleService { get; set; }
        [Inject]
        NavigationManager NavigationManager { get; set; }
        [Inject]
        IScheduleCustomGroupService ScheduleCustomGroupService { get; set; }

        #endregion

        #region parameter
        [Parameter]
        public string searchParams { get; set; }

        #endregion

        #region Propeties and variable
        public List<CompanyScheduleInfo> CompaniesScheduleInfo { get; set; }
        protected bool isLoading = true;
        private bool isFirstRender = false;
        protected string key = "groupschedule";
        protected bool ShowErrorPopup { get; set; } = false;
        #endregion

        #region Function
        protected override async Task OnInitializedAsync()
        {
            IEnumerable<CompanyScheduleInfo> ScheduleInfo = await StaffScheduleService.GetGroupScheduleInfo(new ClaimModel().TenantID, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, 0);
            CompaniesScheduleInfo = ScheduleInfo.ToList();
            isFirstRender = true;
            isLoading = false;
            StateHasChanged();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

            if (isFirstRender)
            {
                await JSRuntime.InvokeVoidAsync("loadPageScript", "StaffScheduleMobileComponentPage", "fadeToggleGroupStaffScheduleMB");
                isFirstRender = false;
            }
        }
        protected void EditGroup(GroupScheduleInfo GroupSchedule)
        {
            var groupSchedule = EncryptHelper.EncryptToUrl(GroupSchedule);
            NavigationManager.NavigateTo("/StaffScheduleMobileOrganizationRegister?groupScheduleID=" + groupSchedule);
        }
        public async Task DeleteGroup(GroupScheduleInfo GroupSchedule)
        {
            isLoading = true;
            await Task.Run(() =>
            {
                InvokeAsync(StateHasChanged).Wait();
                bool IsDeleteSuccessfully = ScheduleCustomGroupService.Delete(GroupSchedule.GroupId).Result;
                if (!IsDeleteSuccessfully)
                {
                    ShowErrorPopup = true;
                }
                isLoading = false;
                InvokeAsync(StateHasChanged);
            });
            IEnumerable<CompanyScheduleInfo> ScheduleInfo = await StaffScheduleService.GetGroupScheduleInfo(new ClaimModel().TenantID, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, 0);
            CompaniesScheduleInfo = ScheduleInfo.ToList();
            await InvokeAsync(StateHasChanged);
        }
        /// <summary>
        /// Click button back to staffScheduleMobile page
        /// </summary>
        protected void BackToStaffScheduleMobile()
        {
            isFirstRender = false;
            NavigationManager.NavigateTo("/StaffScheduleMobile");
        }
        /// <summary>
        /// Click button back to staffScheduleMobile page
        /// </summary>
        protected void ShowScheduleGroup(GroupScheduleInfo groupInfo)
        {
            var groupSchedule = EncryptHelper.EncryptToUrl(groupInfo);
            NavigationManager.NavigateTo("/StaffScheduleMobile?groupSchedule=" + groupSchedule);
        }
        protected void AddScheduleGroup()
        {
            NavigationManager.NavigateTo("/StaffScheduleMobileOrganizationRegister");
        }
        #endregion
    }
}
