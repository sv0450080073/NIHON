#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\Components\Staff\JobList.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "425e4a8993aac344d27b4280f9450d0e6e350714"
// <auto-generated/>
#pragma warning disable 1591
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
#nullable restore
#line 4 "E:\Project\HassyaAllrightCloud\Pages\Components\Staff\JobList.razor"
using System.Data;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "E:\Project\HassyaAllrightCloud\Pages\Components\Staff\JobList.razor"
using System.Diagnostics;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "E:\Project\HassyaAllrightCloud\Pages\Components\Staff\JobList.razor"
using System.Threading;

#line default
#line hidden
#nullable disable
    public partial class JobList : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "div");
            __builder.AddAttribute(1, "class", "custom-scroll border border-dark border-top-0");
            __builder.AddAttribute(2, "style", "overflow-x: auto;");
            __builder.AddMarkupContent(3, "\r\n    ");
            __builder.OpenElement(4, "ul");
            __builder.AddAttribute(5, "class", "list-unstyled list mb-0 border-0");
            __builder.AddMarkupContent(6, "\r\n");
#nullable restore
#line 11 "E:\Project\HassyaAllrightCloud\Pages\Components\Staff\JobList.razor"
         if (List.Count() == 0)
        {

#line default
#line hidden
#nullable disable
            __builder.AddContent(7, "            ");
            __builder.OpenElement(8, "li");
            __builder.AddAttribute(9, "class", "text-center");
            __builder.OpenElement(10, "small");
            __builder.AddContent(11, 
#nullable restore
#line 13 "E:\Project\HassyaAllrightCloud\Pages\Components\Staff\JobList.razor"
                                            Lang["BI_T001"]

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(12, "\r\n");
#nullable restore
#line 14 "E:\Project\HassyaAllrightCloud\Pages\Components\Staff\JobList.razor"
        }

#line default
#line hidden
#nullable disable
#nullable restore
#line 15 "E:\Project\HassyaAllrightCloud\Pages\Components\Staff\JobList.razor"
         foreach (var item in List)
        {

#line default
#line hidden
#nullable disable
            __builder.AddContent(13, "            ");
            __builder.OpenElement(14, "li");
            __builder.AddAttribute(15, "class", "draggable");
            __builder.AddAttribute(16, "draggable", "true");
            __builder.AddAttribute(17, "ondragstart", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.DragEventArgs>(this, 
#nullable restore
#line 17 "E:\Project\HassyaAllrightCloud\Pages\Components\Staff\JobList.razor"
                                                                   () => HandleDragStart(item)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(18, "oncontextmenu", "return false;");
            __builder.AddAttribute(19, "onmouseup", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 17 "E:\Project\HassyaAllrightCloud\Pages\Components\Staff\JobList.razor"
                                                                                                                                           e => OnRowClick(e, item)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(20, "ondragend", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.DragEventArgs>(this, 
#nullable restore
#line 17 "E:\Project\HassyaAllrightCloud\Pages\Components\Staff\JobList.razor"
                                                                                                                                                                                 HandleDragEnd

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(21, "\r\n                ");
            __builder.OpenElement(22, "div");
            __builder.AddAttribute(23, "class", "d-block");
            __builder.AddMarkupContent(24, "\r\n                    ");
            __builder.OpenElement(25, "p");
            __builder.AddAttribute(26, "class", "mb-0");
            __builder.AddContent(27, 
#nullable restore
#line 19 "E:\Project\HassyaAllrightCloud\Pages\Components\Staff\JobList.razor"
                                      item.DanTaNm + ShowTime(GetTime(item.Kotei_SyukoTime), GetTime(item.Kotei_KikTime)) + " ／ " + item.SyokumuNm

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(28, "\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(29, "\r\n            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(30, "\r\n");
#nullable restore
#line 22 "E:\Project\HassyaAllrightCloud\Pages\Components\Staff\JobList.razor"
        }

#line default
#line hidden
#nullable disable
            __builder.AddContent(31, "    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(32, "\r\n");
            __builder.CloseElement();
            __builder.AddMarkupContent(33, "\r\n\r\n");
            __builder.OpenComponent<BlazorContextMenu.ContextMenu>(34);
            __builder.AddAttribute(35, "Id", "gridRowsClickMenu");
            __builder.AddAttribute(36, "CssClass", "contextmenu");
            __builder.AddAttribute(37, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment<BlazorContextMenu.MenuRenderingContext>)((context) => (__builder2) => {
                __builder2.AddMarkupContent(38, "\r\n    ");
                __builder2.OpenComponent<BlazorContextMenu.Item>(39);
                __builder2.AddAttribute(40, "OnClick", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<BlazorContextMenu.ItemClickEventArgs>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<BlazorContextMenu.ItemClickEventArgs>(this, 
#nullable restore
#line 27 "E:\Project\HassyaAllrightCloud\Pages\Components\Staff\JobList.razor"
                   OnCheck

#line default
#line hidden
#nullable disable
                )));
                __builder2.AddAttribute(41, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder3) => {
                    __builder3.AddContent(42, 
#nullable restore
#line 27 "E:\Project\HassyaAllrightCloud\Pages\Components\Staff\JobList.razor"
                             Lang["Check"]

#line default
#line hidden
#nullable disable
                    );
                }
                ));
                __builder2.CloseComponent();
                __builder2.AddMarkupContent(43, "\r\n");
            }
            ));
            __builder.CloseComponent();
        }
        #pragma warning restore 1998
#nullable restore
#line 31 "E:\Project\HassyaAllrightCloud\Pages\Components\Staff\JobList.razor"
       
    [CascadingParameter] StaffContainer Container { get; set; }
    //[Parameter] public List<JobData> List { get; set; }
    [Parameter] public List<JobItem> List { get; set; }

    protected override void OnParametersSet()
    {
        if (Container.Params.WorkSort == (int)StaffWorkSortOrder.Earlier)
        {
            List = List.OrderBy(x => x.SyuKoTime).ToList();
        }
        else if (Container.Params.WorkSort == (int)StaffWorkSortOrder.Time)
        {
            List = List.OrderByDescending(x => x.SyuKoTime).ToList();
        }
    }

    private void HandleDragStart(JobItem selectItem)
    {
        Container.Job = selectItem;
        Container.Payload.AllowStatus = Flag.Job;
        Container.isJob = true;
        Container.isWork = false;
        Container.isHoliday = false;
        Container.isSwapJob = false;
        Container.isAssignJob = true;
        Container.OnChange();
    }

    private void HandleDragEnd()
    {
        Container.isAssignJob = false;
    }

    private string GetTime(string time)
    {
        return string.IsNullOrEmpty(time.Trim()) ? string.Empty : time.Insert(2, ":");
    }

    private string ShowTime(string start, string end)
    {
        if (string.IsNullOrEmpty(start) && string.IsNullOrEmpty(end)) return string.Empty;
        return "(" + start + " ～ " + end + ")";
    }

    private async Task OnRowClick(MouseEventArgs args, JobItem item)
    {
        if (args.Button == 2)
        {
            await blazorContextMenuService.ShowMenu("gridRowsClickMenu", Convert.ToInt32(args.ClientX) + 5, Convert.ToInt32(args.ClientY) + 5, item);
            StateHasChanged();
        }
    }

    private async Task OnCheck(ItemClickEventArgs args)
    {
        await Container.ShowLoading();
        Container.ErrorMessage.Clear();
        Container.listSyain.Clear();
        var item = args.Data as JobItem;
        DataTable KobanTable = new DataTable();
        KobanTable.Columns.Add("UnkYmd", typeof(string));
        KobanTable.Columns.Add("UkeNo", typeof(string));
        KobanTable.Columns.Add("UnkRen", typeof(short));
        KobanTable.Columns.Add("TeiDanNo", typeof(short));
        KobanTable.Columns.Add("BunkRen", typeof(short));
        KobanTable.Columns.Add("SyukinYmd", typeof(string));
        KobanTable.Columns.Add("SyukinTime", typeof(string));
        KobanTable.Columns.Add("TaikinYmd", typeof(string));
        KobanTable.Columns.Add("TaiknTime", typeof(string));
        KobanTable.Clear();
        await _staffService.GetTableKoban(item, KobanTable);
        var check = await _staffService.ValidateBeforeAssignJob(Container.Date.ToString(CommonConstants.FormatYMD), 0, KobanTable, string.Empty, 0, 0, 0, false);
        if (check.Item1 > 0)
        {
            Container.ErrorMessage.Add(Lang["BI_T006"]);
        }
        else if (check.Item2.Any())
        {
            foreach(var checkData in check.Item2)
            {
                Container.listSyain.Add(checkData.Item1);
            }
        }
        await Container.StateChanged();
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IStaffListService _staffService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IBlazorContextMenuService blazorContextMenuService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IStringLocalizer<Pages.Staff> Lang { get; set; }
    }
}
#pragma warning restore 1591