#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\Components\Staff\StaffList.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6fad01971714de82e1a4fe84dcc5d69b8eabe78a"
// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace HassyaAllrightCloud.Pages.Components.Staff
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
#line 14 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Domain.Dto;

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
using HassyaAllrightCloud.Commons.Extensions;

#line default
#line hidden
#nullable disable
#nullable restore
#line 18 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using BlazorContextMenu;

#line default
#line hidden
#nullable disable
#nullable restore
#line 19 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Application.Validation;

#line default
#line hidden
#nullable disable
#nullable restore
#line 20 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Validation;

#line default
#line hidden
#nullable disable
#nullable restore
#line 21 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using DevExpress.Blazor;

#line default
#line hidden
#nullable disable
#nullable restore
#line 22 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using System.Linq;

#line default
#line hidden
#nullable disable
#nullable restore
#line 23 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Newtonsoft.Json;

#line default
#line hidden
#nullable disable
#nullable restore
#line 24 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Newtonsoft.Json.Linq;

#line default
#line hidden
#nullable disable
#nullable restore
#line 25 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Microsoft.Extensions.Localization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 26 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.IService;

#line default
#line hidden
#nullable disable
#nullable restore
#line 27 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using System.IO;

#line default
#line hidden
#nullable disable
#nullable restore
#line 28 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using LexLibrary.Line.NotifyBot.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 29 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using LexLibrary.Line.NotifyBot;

#line default
#line hidden
#nullable disable
#nullable restore
#line 30 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using DevExpress.Blazor.Reporting;

#line default
#line hidden
#nullable disable
#nullable restore
#line 31 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Pages.Components;

#line default
#line hidden
#nullable disable
#nullable restore
#line 32 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using SharedLibraries.UI.Components;

#line default
#line hidden
#nullable disable
#nullable restore
#line 33 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Blazored.Modal;

#line default
#line hidden
#nullable disable
#nullable restore
#line 34 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Blazored.Modal.Services;

#line default
#line hidden
#nullable disable
#nullable restore
#line 35 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using SharedLibraries.UI.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 37 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Radzen;

#line default
#line hidden
#nullable disable
#nullable restore
#line 38 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Radzen.Blazor;

#line default
#line hidden
#nullable disable
#nullable restore
#line 39 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Pages.Components.CommonComponents;

#line default
#line hidden
#nullable disable
    public partial class StaffList : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 66 "E:\Project\HassyaAllrightCloud\Pages\Components\Staff\StaffList.razor"
       
    [CascadingParameter] StaffContainer Container { get; set; }
    [Parameter] public List<StaffData> List { get; set; }
    [Parameter] public Flag[] AllowedStatuses { get; set; }
    [Parameter] public double Height { get; set; }
    [Inject] protected IErrorHandlerService errorModalService { get; set; }
    int clientX { get; set; }
    int clientY { get; set; }

    protected override void OnParametersSet()
    {
        try
        {
            switch (Container.Params.CrewSort)
            {
                case (int)StaffCrewSortOrder.EmployeeCodeOrder:
                    List = List.OrderBy(x => x.SyainCd).ToList();
                    break;
                case (int)StaffCrewSortOrder.PreviousDayEndTime:
                    List = List.OrderBy(x => x.PreDayEndTime).ToList();
                    break;
                case (int)StaffCrewSortOrder.AscendingTimeForOneWeek:
                    List = List.OrderBy(x => x.WorkTime).ToList();
                    break;
                case (int)StaffCrewSortOrder.FourWeeksLessTime:
                    List = List.OrderBy(x => x.WorkTime4Week).ToList();
                    break;
            }
            StateHasChanged();
        }
        catch (Exception ex)
        {
            errorModalService.HandleError(ex);
        }
    }

    private async Task HandleDrop(StaffData staff)
    {
        if (AllowedStatuses != null && !AllowedStatuses.Contains(Container.Payload.AllowStatus))
        {
            return;
        }
        if (staff.HolidayID != 0) Container.isAssignHoliday = true;
        await Container.UpdateJobAsync(staff);
    }

    private string GetColor(StaffData item)
    {
        return !string.IsNullOrWhiteSpace(item.ColKinKyu) ? item.ColKinKyu : "#000000";
    }

    private string GetTime(float time)
    {
        return string.Format("{0}:{1}", (int)(time / 60), (int)(time % 60));
    }

    private void ShowToolTip(StaffData staff, MouseEventArgs agrs)
    {
        clientX = Convert.ToInt32(agrs.ClientX) + 7;
        clientY = Convert.ToInt32(agrs.ClientY) + 7;
        if (staff.HolidayID != 0)
        {
            clientX = clientX - 480;
        }
        clientY = clientY - 320;
        if (!Container.isAssignJob && !Container.isAssignHoliday && !Container.isAHoliday && (staff.SyokumuKbn == 1 || staff.SyokumuKbn == 2 || staff.SyokumuKbn == 5)) staff.isShowToolTip = true;
    }

    private void HideToolTip(StaffData staff)
    {
        staff.isShowToolTip = false;
    }

    private void UpdateToolTip(StaffData staff, MouseEventArgs args)
    {
        clientX = Convert.ToInt32(args.ClientX) + 7;
        clientY = Convert.ToInt32(args.ClientY) + 7;
        if (staff.HolidayID != 0)
        {
            clientX = clientX - 480;
        }
        clientY = clientY - 320;
    }

    private string OnGetBackgroundColor(StaffData staff)
    {
        return Container.listSyain.Contains(staff.SyainCdSeq) ? "pink" : "#ffffff";
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IStringLocalizer<Pages.Staff> Lang { get; set; }
    }
}
#pragma warning restore 1591
