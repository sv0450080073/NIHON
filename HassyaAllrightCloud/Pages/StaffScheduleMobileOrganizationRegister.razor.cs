using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Pages
{
    public class StaffScheduleMobileOrganizationRegisterBase : ComponentBase
    {
        #region Inject
        [Inject]
        protected IStringLocalizer<StaffScheduleMobile> Lang { get; set; }
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }
        [Inject]
        IStaffListService SyainService { get; set; }
        [Inject]
        IScheduleCustomGroupService ScheduleCustomGroupService { get; set; }
        [Inject]
        NavigationManager NavigationManager { get; set; }
        #endregion

        #region parameter
        [Parameter]
        public string groupScheduleID { get; set; }
        #endregion

        #region properties and variable
        protected List<StaffsData> StaffList { get; set; }
        protected bool ShowErrorPopup { get; set; } = false;
        protected bool isLoading = true;
        protected string key = "groupschedule";
        protected CustomGroupScheduleForm customGroupScheduleForm = new CustomGroupScheduleForm();
        protected GroupScheduleInfo currentGroupSelected = new GroupScheduleInfo();

        #endregion

        #region Function
        protected override async Task OnInitializedAsync()
        {
            StaffList = await SyainService.Get(new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq);
            if (groupScheduleID != null)
            {
                currentGroupSelected = EncryptHelper.DecryptFromUrl<GroupScheduleInfo>(groupScheduleID);
                customGroupScheduleForm = new CustomGroupScheduleForm(currentGroupSelected, StaffList);
            }
            isLoading = false;
            StateHasChanged();
        }
        /// <summary>
        /// Click button back to staffScheduleMobile page
        /// </summary>
        protected void BackToStaffScheduleMobileOrganization()
        {
            NavigationManager.NavigateTo("/StaffScheduleMobileOrganization");
        }
        protected void ChangeGroupName(string NewName)
        {
            customGroupScheduleForm.GroupName = NewName.Trim();
            if (currentGroupSelected != null)
            {
                currentGroupSelected.GroupName = customGroupScheduleForm.GroupName;
            }
            InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// save group with member
        /// </summary>
        protected async void SaveGroupSchedule()
        {
            if (string.IsNullOrEmpty(customGroupScheduleForm.GroupName) || string.IsNullOrEmpty(customGroupScheduleForm.GroupName.Trim()) || customGroupScheduleForm.StaffList.Count() == 0)
            {
                return;
            }

            isLoading = true;
            await Task.Run(() =>
            {
                InvokeAsync(StateHasChanged).Wait();
                bool IsUpdateSuccessfully = ScheduleCustomGroupService.Update(customGroupScheduleForm).Result;
                if (!IsUpdateSuccessfully)
                {
                    ShowErrorPopup = true;
                }
                isLoading = false;
                BackToStaffScheduleMobileOrganization();
                InvokeAsync(StateHasChanged);
            });
        }
        /// <summary>
        /// change member selected
        /// </summary>
        /// <param name="NewMemberList"></param>
        protected void ChangeMemberList(IEnumerable<StaffsData> NewMemberList)
        {
            customGroupScheduleForm.StaffList = (NewMemberList == null ? new List<StaffsData>() : NewMemberList.ToList());
            InvokeAsync(StateHasChanged);
        }
        #endregion
    }
}
