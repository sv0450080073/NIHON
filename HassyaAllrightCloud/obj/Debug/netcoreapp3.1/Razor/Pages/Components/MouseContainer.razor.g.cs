#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\Components\MouseContainer.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "dd6b173d0adbed6423332b655e29daa39fc8c6f8"
// <auto-generated/>
#pragma warning disable 1591
namespace HassyaAllrightCloud.Pages.Components
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
    public partial class MouseContainer : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "div");
            __builder.AddAttribute(1, "class", "mouse-event" + " group-" + (
#nullable restore
#line 4 "E:\Project\HassyaAllrightCloud\Pages\Components\MouseContainer.razor"
                               Param.ActiveG

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(2, "\r\n");
            __Blazor.HassyaAllrightCloud.Pages.Components.MouseContainer.TypeInference.CreateCascadingValue_0(__builder, 3, 4, 
#nullable restore
#line 5 "E:\Project\HassyaAllrightCloud\Pages\Components\MouseContainer.razor"
                       this

#line default
#line hidden
#nullable disable
            , 5, (__builder2) => {
                __builder2.AddMarkupContent(6, "\r\n    ");
                __builder2.AddContent(7, 
#nullable restore
#line 6 "E:\Project\HassyaAllrightCloud\Pages\Components\MouseContainer.razor"
     ChildContent

#line default
#line hidden
#nullable disable
                );
                __builder2.AddMarkupContent(8, "\r\n");
            }
            );
            __builder.AddMarkupContent(9, "\r\n");
            __builder.CloseElement();
        }
        #pragma warning restore 1998
#nullable restore
#line 10 "E:\Project\HassyaAllrightCloud\Pages\Components\MouseContainer.razor"
       
    [Parameter] public DateTime Date { get; set; }
    [Parameter] public int GridData { get; set; } // 1: 1day 2: 3days, 3: 1week, 4: 1month
    [Parameter] public int GridView { get; set; } // 0:15minute, 1:1hour, 2:3hours, 3:6hours, 4:day
    [Parameter] public int Mode { get; set; } // 1:view, 2:edit, 3:create
    [Parameter] public double Width { get; set; }
    [Parameter] public int View { get; set; } // 1:large, 2:medium, 3:small
    [Parameter] public List<BusDataType> BusLists { get; set; }
    [Parameter] public List<BusDataType> BusGreenLists { get; set; }
    [Parameter] public List<BusDataType> BusGrayLists { get; set; }
    [Parameter] public List<ItemBus> Jobs { get; set; }
    [Parameter] public EventCallback<ItemBus> OnStatusUpdated { get; set; }
    [Parameter] public EventCallback<ItemBus> OnShowMenu { get; set; }
    [Parameter] public EventCallback<BusLineData> UpdateLineHeight { get; set; }
    [Parameter] public EventCallback<string> OnUpdateLineCut { get; set; }
    [Parameter] public EventCallback<ItemBus> OnSatusRemoved { get; set; }
    [Parameter] public RenderFragment ChildContent { get; set; }
    [Parameter] public ConfigBusSchedule Param { get; set; }
    [Parameter] public DateTime defaultdate { get; set; }
    [Parameter] public int Type { get; set; }
    public ItemBus Payload { get; set; }
    public bool OnDragAble { get; set; } = false;
    public bool OnMouseDown { get; set; } = false;
    public double StartX { get; set; } = -1;
    public double StartY { get; set; } = -1;
    public int IsLeft { get; set; } = 0;
    public ItemBus PayloadCreate { get; set; }
    public bool IsCreating { get; set; } = false;


    /**
     * Don't use today
     * 
     */
    public void MouseMovePanel(MouseEventArgs e)
    {
        int d = Param.Number_of_days;       
        if (!OnMouseDown)
        {
            return;
        }
        if (Mode == 3 && Payload != null && OnMouseDown)
        {
            double compairwith = Payload.WidthDefault / Payload.Width;
            if (StartX == -1)
            {
                StartX = e.ClientX;
            }
            double iWidth = Payload.Width;
            iWidth += (e.ClientX - StartX);
            //Payload.Name = "Editing able";
            Payload.CCSStyle = "width:" + iWidth.ToString() + "px;height:" + Payload.Height.ToString() + "px;top:" + Payload.Top.ToString() + "px;left:" + Payload.Left.ToString() + "px;background:" + Payload.ColorLine + ";";
            StartX = e.ClientX;
            Payload.Width = iWidth;
            Payload.WidthDefault = Payload.Width * compairwith;
            return;
        }

        if (Payload != null && OnMouseDown)

        {
            if (IsLeft == 2)
            {
                if (StartX == -1)
                {
                    StartX = e.ClientX;
                }
                double iWidth = Payload.Width;
                iWidth += (e.ClientX - StartX);
                //Payload.Name = "Editing able";
                Payload.CCSStyle = "width:" + iWidth.ToString() + "px;height:" + Payload.Height.ToString() + "px;top:" + Payload.Top.ToString() + "px;left:" + Payload.Left.ToString() + "px;background:" + Payload.ColorLine + ";";
                StartX = e.ClientX;
                double compairwith = Payload.WidthDefault / Payload.Width;
                if (iWidth > Payload.MaxWidth && Payload.MaxWidth != -1)
                {
                    Payload.Width = Payload.MaxWidth;
                    Payload.WidthDefault = Payload.Width * compairwith;
                    DateTime datestart = busScheduleHelper.convertwidthtodate(Width, Payload.Left, d, defaultdate);
                    Payload.StartDate = datestart.ToString("yyyyMMdd");
                    Payload.TimeStart = int.Parse(datestart.ToString("HHmm"));
                    DateTime dateend = busScheduleHelper.convertwidthtodate(Width, Payload.Left + Payload.Width, d, defaultdate);
                    Payload.EndDate = dateend.ToString("yyyyMMdd");
                    Payload.TimeEnd = int.Parse(dateend.ToString("HHmm"));
                }
                else
                {
                    Payload.Width = iWidth;
                    Payload.WidthDefault = Payload.Width * compairwith;
                    DateTime datestart = busScheduleHelper.convertwidthtodate(Width, Payload.Left, d, defaultdate);
                    Payload.StartDate = datestart.ToString("yyyyMMdd");
                    Payload.TimeStart = int.Parse(datestart.ToString("HHmm"));
                    DateTime dateend = busScheduleHelper.convertwidthtodate(Width, Payload.Left + Payload.Width, d, defaultdate);
                    Payload.EndDate = dateend.ToString("yyyyMMdd");
                    Payload.TimeEnd = int.Parse(dateend.ToString("HHmm"));
                }
                System.Diagnostics.Debug.Print("Bus ID:" + Payload.Id.ToString());
                System.Diagnostics.Debug.Print("Min Left:" + Payload.MinLeft.ToString());
                System.Diagnostics.Debug.Print("Left:" + Payload.Left.ToString());
                System.Diagnostics.Debug.Print("Width" + Payload.Width.ToString());
                System.Diagnostics.Debug.Print("times:" + Payload.TimeStart.ToString());
                System.Diagnostics.Debug.Print("dates" + Payload.StartDate.ToString());
                System.Diagnostics.Debug.Print("timee:" + Payload.TimeEnd.ToString());
                System.Diagnostics.Debug.Print("datee:" + Payload.EndDate.ToString());
                System.Diagnostics.Debug.Print("Minleft:" + Payload.MinLeft.ToString());
                System.Diagnostics.Debug.Print("Maxwidth:" + Payload.MaxWidth.ToString());
            }
            else if (IsLeft == 1)
            {
                double compairwith = Payload.WidthDefault / Payload.Width;
                if (StartX == -1)
                {
                    StartX = e.ClientX;
                }
                double iWidth = Payload.Width;
                if (e.ClientX < StartX)
                {
                    iWidth += (StartX - e.ClientX);
                    Payload.Left -= (StartX - e.ClientX);
                    if (Payload.Left < Payload.MinLeft && Payload.MinLeft != -1)
                    {
                        Payload.Left = Payload.MinLeft;
                        iWidth = Payload.Width;
                    }
                    Payload.Width = iWidth;
                    Payload.WidthDefault = Payload.Width * compairwith;
                    DateTime datestart = busScheduleHelper.convertwidthtodate(Width, Payload.Left, d, defaultdate);
                    Payload.StartDate = datestart.ToString("yyyyMMdd");
                    Payload.TimeStart = int.Parse(datestart.ToString("HHmm"));
                    DateTime dateend = busScheduleHelper.convertwidthtodate(Width, Payload.Left + Payload.Width, d, defaultdate);
                    Payload.EndDate = dateend.ToString("yyyyMMdd");
                    Payload.TimeEnd = int.Parse(dateend.ToString("HHmm"));
                }
                else if (e.ClientX > StartX)
                {
                    iWidth += (StartX - e.ClientX);

                    if (iWidth < 20)
                    {
                        Payload.Width = 20;
                        iWidth = 20;
                    }
                    else
                    {
                        Payload.Left += (e.ClientX - StartX);
                        Payload.Width = iWidth;
                        Payload.WidthDefault = Payload.Width * compairwith;
                        DateTime datestart = busScheduleHelper.convertwidthtodate(Width, Payload.Left, d, defaultdate);
                        Payload.StartDate = datestart.ToString("yyyyMMdd");
                        Payload.TimeStart = int.Parse(datestart.ToString("HHmm"));
                        DateTime dateend = busScheduleHelper.convertwidthtodate(Width, Payload.Left + Payload.Width, d, defaultdate);
                        Payload.EndDate = dateend.ToString("yyyyMMdd");
                        Payload.TimeEnd = int.Parse(dateend.ToString("HHmm"));
                    }
                }
                System.Diagnostics.Debug.Print("Bus ID:" + Payload.Id.ToString());
                System.Diagnostics.Debug.Print("Min Left:" + Payload.MinLeft.ToString());
                System.Diagnostics.Debug.Print("Left:" + Payload.Left.ToString());
                System.Diagnostics.Debug.Print("Width" + Payload.Width.ToString());
                System.Diagnostics.Debug.Print("times:" + Payload.TimeStart.ToString());
                System.Diagnostics.Debug.Print("dates" + Payload.StartDate.ToString());
                System.Diagnostics.Debug.Print("timee:" + Payload.TimeEnd.ToString());
                System.Diagnostics.Debug.Print("datee:" + Payload.EndDate.ToString());
                System.Diagnostics.Debug.Print("Minleft:" + Payload.MinLeft.ToString());
                System.Diagnostics.Debug.Print("Maxwidth:" + Payload.MaxWidth.ToString());
                //Payload.Name = "Editing able";
                Payload.CCSStyle = "width:" + iWidth.ToString() + "px;height:" + Payload.Height.ToString() + "px;top:" + Payload.Top.ToString() + "px;left:" + Payload.Left.ToString() + "px;";
                StartX = e.ClientX;
            }
        }
    }

    public void MouseUpProcess(MouseEventArgs e)
    {
        if (Payload != null && OnMouseDown)
        {
            OnMouseDown = false;
            StartX = -1;
            StartY = -1;
            //Payload.Name = "Edited able";
            if (Mode == 2)
            {
                Payload.Name = Payload.Name + " - Edited";
            }
        }
    }
    /*
     * End Don't use today
     * 
     */

    public async Task UpdateJobAsync(string newLine,int BusVehicle)
    {
        var task = Jobs.SingleOrDefault(x => x.BookingId == Payload.BookingId && x.TeiDanNo==Payload.TeiDanNo && x.BunkRen==Payload.BunkRen && x.haUnkRen==Payload.haUnkRen);

        if (task != null)
        {
            task.BusLine = newLine;
            task.BusVehicle = BusVehicle;
            ItemBus tmp = new ItemBus();
            tmp = Payload;
            Payload = new ItemBus();
            await OnStatusUpdated.InvokeAsync(tmp);
        }
    }

    public async Task UpdateJobAsync()
    {
        var task = Jobs.SingleOrDefault(x => x.Id == Payload.Id);

        if (task != null)
        {
            await OnStatusUpdated.InvokeAsync(Payload);
        }
    }

    public async Task CreatedJobAsync(ItemBus itemBus)
    {
        Jobs.Add(itemBus);
        Payload = itemBus;
        OnMouseDown = true;
        IsCreating = true;
        await OnStatusUpdated.InvokeAsync(itemBus);
    }

    public async Task UpdateMenuAsync(ItemBus item)
    {
        await OnShowMenu.InvokeAsync(item);
    }

    public async Task UpdateLineCutAsync(string text)
    {
        await OnUpdateLineCut.InvokeAsync(text);
    }

    public async Task RemoveJobAsync(ItemBus itemBus)
    {
        Jobs.Remove(itemBus);
        Payload = new ItemBus();
        await OnSatusRemoved.InvokeAsync(itemBus);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="lstbus"></param>
    /// <returns></returns>
    public async Task UpdateHeightAsync(BusLineData bus)
    {
        await UpdateLineHeight.InvokeAsync(bus);
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private BusScheduleHelper busScheduleHelper { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IJSRuntime jsRuntime { get; set; }
    }
}
namespace __Blazor.HassyaAllrightCloud.Pages.Components.MouseContainer
{
    #line hidden
    internal static class TypeInference
    {
        public static void CreateCascadingValue_0<TValue>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, TValue __arg0, int __seq1, global::Microsoft.AspNetCore.Components.RenderFragment __arg1)
        {
        __builder.OpenComponent<global::Microsoft.AspNetCore.Components.CascadingValue<TValue>>(seq);
        __builder.AddAttribute(__seq0, "Value", __arg0);
        __builder.AddAttribute(__seq1, "ChildContent", __arg1);
        __builder.CloseComponent();
        }
    }
}
#pragma warning restore 1591
