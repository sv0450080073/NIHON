#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\LeaveApplicationManagement.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "f0720f5c98ad20b3e313f611fc4f52c9eb52702f"
// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace HassyaAllrightCloud.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using System.Globalization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Infrastructure.Services;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 12 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 13 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Domain.Entities;

#line default
#line hidden
#nullable disable
#nullable restore
#line 15 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Commons.Helpers;

#line default
#line hidden
#nullable disable
#nullable restore
#line 16 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Commons.Constants;

#line default
#line hidden
#nullable disable
#nullable restore
#line 17 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using BlazorContextMenu;

#line default
#line hidden
#nullable disable
#nullable restore
#line 18 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Application.Validation;

#line default
#line hidden
#nullable disable
#nullable restore
#line 19 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using DevExpress.Blazor;

#line default
#line hidden
#nullable disable
#nullable restore
#line 20 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using System.Linq;

#line default
#line hidden
#nullable disable
#nullable restore
#line 21 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Newtonsoft.Json;

#line default
#line hidden
#nullable disable
#nullable restore
#line 22 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Newtonsoft.Json.Linq;

#line default
#line hidden
#nullable disable
#nullable restore
#line 23 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Microsoft.Extensions.Localization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 24 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.IService;

#line default
#line hidden
#nullable disable
#nullable restore
#line 25 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using System.IO;

#line default
#line hidden
#nullable disable
#nullable restore
#line 26 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using LexLibrary.Line.NotifyBot.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 27 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using LexLibrary.Line.NotifyBot;

#line default
#line hidden
#nullable disable
#nullable restore
#line 28 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using DevExpress.Blazor.Reporting;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "E:\Project\HassyaAllrightCloud\Pages\LeaveApplicationManagement.razor"
using HassyaAllrightCloud.Domain.Dto;

#line default
#line hidden
#nullable disable
    public partial class LeaveApplicationManagement : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 434 "E:\Project\HassyaAllrightCloud\Pages\LeaveApplicationManagement.razor"
       
    string dateFormat = "yy/MM/dd";
    ScheduleManageForm scheduleMagafeForm = new ScheduleManageForm();
    ScheduleDetail scheduleDetail = new ScheduleDetail();
    List<Status> ApprovalStatus = new List<Status>();
    List<Status> ApprovalStatusForApprover = new List<Status>();
    IEnumerable<Branch> branches = new List<Branch>();
    IEnumerable<Staffs> staffs = new List<Staffs>();
    IEnumerable<CustomGroup> customGroups = new List<CustomGroup>();
    List<Branch> branchesDropdown = new List<Branch>();
    List<Staffs> staffsDropdown = new List<Staffs>();
    List<CustomGroup> customGroupsDropdown = new List<CustomGroup>();
    CustomGroupScheduleForm customGroupScheduleForm = new CustomGroupScheduleForm();
    IEnumerable<ScheduleManageGridData> DataSource = new List<ScheduleManageGridData>();
    bool isShowDetailSchedulePopup = false;
    bool PopupGroupScheduleAdd = false;
    List<StaffsData> StaffList;
    int? CurrentClick { get; set; } = null;
    int? CurrentScroll { get; set; }
    bool isLoading = false;
    bool IsValid = true;
    int scheduleId = 0;
    bool required;

    protected override async void OnInitialized()
    {
        ApprovalStatus.Add(null);
        ApprovalStatus.Add(new Status()
        {
            status = StaffScheduleConstants.Pending
        });
        ApprovalStatus.Add(new Status()
        {
            status = StaffScheduleConstants.Accept
        });
        ApprovalStatus.Add(new Status()
        {
            status = StaffScheduleConstants.Refuse
        });
        ApprovalStatusForApprover.Add(new Status()
        {
            status = StaffScheduleConstants.Pending
        });
        ApprovalStatusForApprover.Add(new Status()
        {
            status = StaffScheduleConstants.Accept
        });
        ApprovalStatusForApprover.Add(new Status()
        {
            status = StaffScheduleConstants.Refuse
        });

        DataSource = IScheduleManageService.GetScheduleDataGrid(scheduleMagafeForm).Result;
        staffs = IScheduleManageService.GetScheduleStaff().Result;
        branches = IScheduleManageService.GetStaffOffice().Result;
        customGroups = IScheduleManageService.GetScheduleCustomGroup().Result;
        staffsDropdown = staffs.ToList();
        staffsDropdown.Insert(0, null);
        branchesDropdown = branches.ToList();
        branchesDropdown.Insert(0, null);
        customGroupsDropdown = customGroups.ToList();
        customGroupsDropdown.Insert(0, null);
        StaffList = await SyainService.Get(Common.CompanyID, Common.UpdSyainCd);
    }

    async Task ChangeValueScheduleDetail(string ValueName, dynamic value)
    {
        required = false;
        if(ValueName == StaffScheduleConstants.NoteName)
        {
            var propertyInfo = scheduleDetail.GetType().GetProperty(ValueName);
            propertyInfo.SetValue(scheduleDetail, value.Value, null);
        }
        else
        {
            if (value is string && string.IsNullOrEmpty(value))
            {
                value = null;
            }
            var propertyInfo = scheduleDetail.GetType().GetProperty(ValueName);
            propertyInfo.SetValue(scheduleDetail, value, null);
        }

        if(scheduleDetail.ApprovalStatus.status == StaffScheduleConstants.Refuse && scheduleDetail.ShoRejBiko == string.Empty)
        {
            required = true;
        }
        StateHasChanged();
    }


    async Task ChangeValueForm(string ValueName, dynamic value)
    {
        if (value is string && string.IsNullOrEmpty(value))
        {
            value = null;
        }
        var propertyInfo = scheduleMagafeForm.GetType().GetProperty(ValueName);
        propertyInfo.SetValue(scheduleMagafeForm, value, null);
        isLoading = true;
        await Task.Run(() =>
        {
            InvokeAsync(StateHasChanged).Wait();
            isLoading = false;
            ReloadData().Wait();
        });
    }

    void SaveScheduleDetail()
    {
        if(scheduleDetail.ApprovalStatus.status == StaffScheduleConstants.Refuse && scheduleDetail.ShoRejBiko == string.Empty)
        {
            required = true;
        }
        else
        {
            required = false;
            var result = IScheduleManageService.UpdateScheduleDetail(scheduleDetail).Result;
            isShowDetailSchedulePopup = false;
        }
        ReloadData().Wait();
        StateHasChanged();
    }

    async Task ReloadData()
    {
        if (IsValid)
        {
            DataSource = IScheduleManageService.GetScheduleDataGrid(scheduleMagafeForm).Result;
            await InvokeAsync(StateHasChanged);
        }
        await InvokeAsync(StateHasChanged);
    }

    void OnRowClick(DataGridRowClickEventArgs<ScheduleManageGridData> e)
    {
        scheduleId = e.DataItem.ScheduleId;
        var LastXClicked = Convert.ToInt32(e.MouseEventArgs.ClientX);
        var LastYClicked = Convert.ToInt32(e.MouseEventArgs.ClientY);
        if (!e.MouseEventArgs.ShiftKey && !e.MouseEventArgs.CtrlKey)
        {
            blazorContextMenuService.ShowMenu("gridRowsClickMenu", LastXClicked, LastYClicked);
        }
        else
        {
            blazorContextMenuService.HideMenu("gridRowClickMenu");
        }
    }
    void ShowScheduleDetail()
    {
        scheduleDetail = IScheduleManageService.GetScheduleDeTail(scheduleId).Result;
        var approver = IScheduleManageService.GetApprover(Common.UpdSyainCd).Result;
        scheduleDetail.ShoUpdYmd = DateTime.Now.ToString().Substring(0, 10);
        scheduleDetail.ShoUpdTime = DateTime.Now.TimeOfDay.ToString().Substring(0, 8);
        scheduleDetail.ShoSyainCd = approver.SyainCd;
        scheduleDetail.ShoSyainNm = approver.Name;
        scheduleDetail.SyainCdSeq = approver.Seg;
        if (scheduleDetail.YoteiShoKbn == 1)
        {
            scheduleDetail.ApprovalStatus = new Status()
            {
                status = StaffScheduleConstants.Pending
            };
        }
        else if (scheduleDetail.YoteiShoKbn == 2)
        {
            scheduleDetail.ApprovalStatus = new Status()
            {
                status = StaffScheduleConstants.Accept
            };
        }
        else
        {
            scheduleDetail.ApprovalStatus = new Status()
            {
                status = StaffScheduleConstants.Refuse
            };
        }
        isShowDetailSchedulePopup = true;
    }
    void AcceptSchedule()
    {
        scheduleDetail = IScheduleManageService.GetScheduleDeTail(scheduleId).Result;
        var approver = IScheduleManageService.GetApprover(Common.UpdSyainCd).Result;
        scheduleDetail.ShoUpdYmd = DateTime.Now.ToString().Substring(0, 10);
        scheduleDetail.ShoUpdTime = DateTime.Now.TimeOfDay.ToString().Substring(0, 8);
        scheduleDetail.ShoSyainCd = approver.SyainCd;
        scheduleDetail.ShoSyainNm = approver.Name;
        scheduleDetail.SyainCdSeq = approver.Seg;
        scheduleDetail.ShoRejBiko = scheduleDetail.ShoRejBiko == null ? string.Empty : scheduleDetail.ShoRejBiko;
        scheduleDetail.ApprovalStatus = new Status()
        {
            status = StaffScheduleConstants.Accept
        };
        var result = IScheduleManageService.UpdateScheduleDetail(scheduleDetail).Result;
        ReloadData().Wait();
    }
    void RefuseSchedule()
    {
        scheduleDetail = IScheduleManageService.GetScheduleDeTail(scheduleId).Result;
        var approver = IScheduleManageService.GetApprover(Common.UpdSyainCd).Result;
        scheduleDetail.ShoUpdYmd = DateTime.Now.ToString().Substring(0, 10);
        scheduleDetail.ShoUpdTime = DateTime.Now.TimeOfDay.ToString().Substring(0, 8);
        scheduleDetail.ShoSyainCd = approver.SyainCd;
        scheduleDetail.ShoSyainNm = approver.Name;
        scheduleDetail.SyainCdSeq = approver.Seg;
        scheduleDetail.ShoRejBiko = scheduleDetail.ShoRejBiko == null ? string.Empty : scheduleDetail.ShoRejBiko;
        scheduleDetail.ApprovalStatus = new Status()
        {
            status = StaffScheduleConstants.Refuse
        };
        var result = IScheduleManageService.UpdateScheduleDetail(scheduleDetail).Result;
        ReloadData().Wait();
    }
    IEnumerable<string> Store(IEnumerable<string> errorMessage)
    {
        if (errorMessage.Count() > 0)
        {
            IsValid = false;
        }
        else
        {
            IsValid = true;
        }
        return errorMessage;
    }
    void ChangeGroupName(string NewName)
    {
        customGroupScheduleForm.GroupName = NewName.Trim();
        InvokeAsync(StateHasChanged);
    }

    void ChangeMemberList(IEnumerable<StaffsData> NewMemberList)
    {
        customGroupScheduleForm.StaffList = (NewMemberList == null ? new List<StaffsData>() : NewMemberList.ToList());
        InvokeAsync(StateHasChanged);
    }
    void AddCustomGroup()
    {
        PopupGroupScheduleAdd = true;
        customGroupScheduleForm = new CustomGroupScheduleForm();
        InvokeAsync(StateHasChanged);
    }
    private async void SaveGroupSchedule()
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
            isLoading = false;
            PopupGroupScheduleAdd = false;
            InvokeAsync(StateHasChanged);
        });
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IStaffListService SyainService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IScheduleCustomGroupService ScheduleCustomGroupService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IScheduleManageService IScheduleManageService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IBlazorContextMenuService blazorContextMenuService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IJSRuntime JSRuntime { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IStringLocalizer<LeaveApplicationManagement> Lang { get; set; }
    }
}
#pragma warning restore 1591
