#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\Components\Staff\StaffGrid.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0692eda302af6f2655b8b14c1a6eac57b6286193"
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
    public partial class StaffGrid : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 113 "E:\Project\HassyaAllrightCloud\Pages\Components\Staff\StaffGrid.razor"
       
    [CascadingParameter] StaffContainer Container { get; set; }
    [Parameter] public List<StaffData> List { get; set; }
    CultureInfo ci = new CultureInfo("ja-JP");
    public List<StaffData> ListTmp { get; set; }
    [Parameter] public int? numberOfVehile { get; set; }
    [Parameter] public List<DrvJinItem> listDriver { get; set; }
    [Parameter] public List<GuiJinItem> listGuider { get; set; }
    [Parameter] public List<VehicleAllocationItem> listVehicleAllocation { get; set; }
    [Parameter] public List<CrewDataAcquisitionItem> listCrewDataAcquisition { get; set; }
    [Parameter] public Flag[] AllowedStatuses { get; set; }
    List<JobItem> ListJobs = new List<JobItem>();
    List<double> listHeight = new List<double>();
    double height = 0;
    double heightline = 2;
    [Inject] protected IErrorHandlerService errorModalService { get; set; }
    public List<StaffData> ListStaff { get; set; }

    protected override void OnParametersSet()
    {
        try
        {
            ListStaff = List.Where(_ => _.Status == Flag.Todo).ToList();
            if (numberOfVehile == null)
            {
                numberOfVehile = 0;
            }
            ListJobs = new List<JobItem>();
            ListTmp = new List<StaffData>();
            listHeight = new List<double>();
            foreach (var staff in ListStaff)
            {
                foreach (var job in Container.Jobs)
                {
                    if (staff.SyainCdSeq == job.SyainCdSeq)
                    {
                        staff.TimeStartString = job.TimeStartString;
                        DateTime start, end;
                        DateTime.TryParseExact(job.SyuKoYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture, DateTimeStyles.None, out start);
                        DateTime.TryParseExact(job.KikYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture, DateTimeStyles.None, out end);
                        if (DateTime.Compare(start, Container.Date) <= 0 && DateTime.Compare(end, Container.Date) >= 0)
                        {
                            if (!ListTmp.Contains(staff))
                                ListTmp.Add(staff);
                        }
                    }
                }
            }

            if (Container.Params.Sort == (int)StaffSortOrder.Earlier)
            {
                ListTmp = ListTmp.OrderBy(x => x.TimeStartString).ToList();
            }
            else if (Container.Params.Sort == (int)StaffSortOrder.Rolling)
            {
                ListTmp = ListTmp.OrderBy(x => x.TenkoNo).ToList();
            }
            else if (Container.Params.Sort == (int)StaffSortOrder.Job)
            {
                ListTmp = ListTmp.OrderBy(_ => _.SyokumuKbn).ToList();
            }

            ReformatRenderItem();
        }
        catch (Exception ex)
        {
            errorModalService.HandleError(ex);
        }
    }

    private void ReformatRenderItem()
    {
        foreach (var staff in ListTmp)
        {
            if (Container.Params.View == (int)ViewMode.Large)
            {
                heightline = 2; // 32px
            }
            if (Container.Params.View == (int)ViewMode.Medium)
            {
                heightline = 1.6; // 17px
            }
            if (Container.Params.View == (int)ViewMode.Small)
            {
                heightline = 1.3; // 14px
            }

            height = 0;
            List<JobItem> tmp = new List<JobItem>();
            tmp.AddRange(Container.Jobs.Where(x => staff.SyainCdSeq == x.SyainCdSeq));

            if (tmp.Count > 0)
            {

                int countTemp = 0;
                foreach (var job in tmp)
                {
                    job.Width = calWidth(job.SyuKoYmd, job.SyuKoTime, job.KikYmd, job.KikTime);
                    job.Left = calLeft(job.SyuKoYmd, job.SyuKoTime);
                    //job.Width = BusScheduleHelper.calwidth(Container.Width, job.SyuKoYmd, job.SyuKoTime, job.KikYmd, job.KikTime, 1);
                    //job.Left = BusScheduleHelper.calleft(Container.Width, job.SyuKoYmd, job.SyuKoTime, 1, Container.Date);
                    job.Height = heightline;
                    if (countTemp == 0)
                    {
                        job.Top = 0.3125; //5px
                    }
                    else
                    {
                        job.Top = heightline * countTemp + 0.3125 + (countTemp) * 0.3125;
                    }
                    job.CCSStyle = StyleItemText(job);
                    countTemp++;
                }
                ListJobs.AddRange(tmp);

                if (tmp.Count() == 1)
                {
                    height = heightline + 0.625;
                }
                else
                {
                    height = heightline * (tmp.Count()) + (tmp.Count() - 1) * 0.3125 + 0.625;
                }
                listHeight.Add(height);

                for (var i = 0; i < Container.Staffs.Count(); i++)
                {
                    if (staff.SyainCdSeq == Container.Staffs[i].SyainCdSeq)
                    {
                        Container.Staffs[i].Height = height;
                    }
                }
            }
        }

        if(Container.Params.Sort == (int)StaffSortOrder.Work)
        {
            listHeight.Clear();
            var temp = ListJobs.OrderBy(_ => _.UkeNo).ThenBy(_ => _.UnkRen).ThenBy(_ => _.TeiDanNo).ThenBy(_ => _.BunkRen).ThenBy(_ => _.HaiInRen).Select(_ => _.SyainCdSeq).Distinct().ToList();
            for(int i = 0; i < temp.Count; i++)
            {
                var staff = ListTmp.FirstOrDefault(_ => _.SyainCdSeq == temp[i]);
                var index = ListTmp.IndexOf(staff);
                var swap = ListTmp[i];
                ListTmp[i] = staff;
                ListTmp[index] = swap;
                listHeight.Add(staff.Height);
            }
        }
    }

    private double calWidth(string SyuKoYmd, string SyuKoTime, string KikYmd, string KikTime)
    {
        const double hourWidth = 4.166666666666667;
        if (SyuKoYmd == KikYmd)
        {
            var time = int.Parse(KikTime.Substring(0, 2)) - int.Parse(SyuKoTime.Substring(0, 2)) + Math.Abs(int.Parse(KikTime.Substring(2)) - int.Parse(SyuKoTime.Substring(2))) * 1.0 / 60;
            return hourWidth * time;
        }
        else
        {
            var time = 23 - int.Parse(SyuKoTime.Substring(0, 2)) + (59 - int.Parse(SyuKoTime.Substring(2))) * 1.0 / 60;
            return hourWidth * time;
        }
    }

    private double calLeft(string SyuKoYmd, string SyuKoTime)
    {
        if(SyuKoYmd == Container.Date.ToString(CommonConstants.FormatYMD))
        {
            const double hourWidth = 4.166666666666667;
            var time = int.Parse(SyuKoTime.Substring(0, 2)) + int.Parse(SyuKoTime.Substring(2)) * 1.0 / 60;
            return hourWidth * time;
        }
        return 0;
    }

    private int arrangePositionTmp(JobItem tmp, List<List<JobItem>> itemList)
    {
        if (itemList.Count <= 0)
        {
            return 0;
        }

        for (var i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].Count == 0)
            {
                return i;
            }

            if (itemList[i][itemList[i].Count - 1].TimeEndString <= tmp.TimeStartString)
            {
                return i;
            }
        }

        return itemList.Count;
    }

    private string StyleItemText(JobItem item)
    {
        return "width:" + item.Width.ToString() + "%;top:" + item.Top.ToString() + "rem;left:" + item.Left.ToString() + "%;height:" + heightline + "rem";
    }

    protected async Task HandleDropWork(StaffData staff)
    {
        if (AllowedStatuses != null && !AllowedStatuses.Contains(Container.Payload.AllowStatus))
        {
            return;
        }
        Container.isAddMoreWork = true;
        await Container.UpdateJobAsync(staff);
    }

    private string GetColor(StaffData item)
    {
        return !string.IsNullOrWhiteSpace(item.ColKinKyu) ? item.ColKinKyu : "#000000";
    }

    private string GetName(StaffData item, byte type)
    {
        if (type == 0)
            return !string.IsNullOrWhiteSpace(item.WorkNm) ? "???	" + item.WorkNm + "	)" : string.Empty;
        return !string.IsNullOrWhiteSpace(item.HolidayNm) ? "???	" + item.HolidayNm + "	)" : string.Empty;
    }

    protected bool IsPreventDefault()
    {
        if (Container.isJob)
        {
            return true;
        }
        return false;
    }

    private string GetTime(float time)
    {
        return string.Format("{0}:{1}", (int)(time / 60), (int)(time % 60));
    }

    private void ShowToolTip(StaffData staff)
    {
        if (!Container.isAssignJob && !Container.isAssignHoliday && !Container.isAHoliday && (staff.SyokumuKbn == 1 || staff.SyokumuKbn == 2 || staff.SyokumuKbn == 5)) staff.isShowToolTip = true;
    }

    private void HideToolTip(StaffData staff)
    {
        staff.isShowToolTip = false;
    }

    private string OnGetBackgroundColor(StaffData staff)
    {
        return Container.listSyain.Contains(staff.SyainCdSeq) ? "pink" : "#ffffff";
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IStringLocalizer<Pages.Staff> Lang { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private BusScheduleHelper BusScheduleHelper { get; set; }
    }
}
#pragma warning restore 1591
