#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\Components\YearPicker.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "11d505021d8d0598b26ab23bb6f2809d068fafcc"
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
#nullable restore
#line 42 "E:\Project\HassyaAllrightCloud\Pages\Components\YearPicker.razor"
using System.Linq.Expressions;

#line default
#line hidden
#nullable disable
    public partial class YearPicker : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "div");
            __builder.AddAttribute(1, "class", "year-picker-component");
            __builder.AddAttribute(2, "id", 
#nullable restore
#line 2 "E:\Project\HassyaAllrightCloud\Pages\Components\YearPicker.razor"
                                        Id

#line default
#line hidden
#nullable disable
            );
            __builder.AddMarkupContent(3, "\r\n    ");
            __builder.OpenElement(4, "div");
            __builder.AddAttribute(5, "class", "input-group");
            __builder.AddMarkupContent(6, "\r\n        ");
            __builder.OpenElement(7, "input");
            __builder.AddAttribute(8, "value", 
#nullable restore
#line 4 "E:\Project\HassyaAllrightCloud\Pages\Components\YearPicker.razor"
                       InputDate?.Year.AddPaddingLeft(4, '0')

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(9, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.ChangeEventArgs>(this, 
#nullable restore
#line 4 "E:\Project\HassyaAllrightCloud\Pages\Components\YearPicker.razor"
                                                                          async args => await InputChanged(args?.Value.ToString())

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMultipleAttributes(10, Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<global::System.Collections.Generic.IEnumerable<global::System.Collections.Generic.KeyValuePair<string, object>>>(
#nullable restore
#line 4 "E:\Project\HassyaAllrightCloud\Pages\Components\YearPicker.razor"
                                                                                                                                                 UnmatchedParameters

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(11, "class", "form-control" + " form-control-sm" + " year-picker-input" + " " + (
#nullable restore
#line 5 "E:\Project\HassyaAllrightCloud\Pages\Components\YearPicker.razor"
                                                                      GetClass()

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(12, "type", "text");
            __builder.CloseElement();
            __builder.AddMarkupContent(13, "\r\n        ");
            __builder.OpenElement(14, "div");
            __builder.AddAttribute(15, "class", "input-group-append");
            __builder.AddMarkupContent(16, "\r\n            ");
            __builder.OpenElement(17, "button");
            __builder.AddAttribute(18, "class", "btn btn-sm btn-pick-year");
            __builder.AddAttribute(19, "type", "button");
            __builder.AddAttribute(20, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 7 "E:\Project\HassyaAllrightCloud\Pages\Components\YearPicker.razor"
                                                                             InputOnClick

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(21, "\r\n                <i class=\"fa fa-calendar\"></i>\r\n            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(22, "\r\n        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(23, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(24, "\r\n\r\n");
#nullable restore
#line 13 "E:\Project\HassyaAllrightCloud\Pages\Components\YearPicker.razor"
     if (showPopup)
    {

#line default
#line hidden
#nullable disable
            __builder.AddContent(25, "        ");
            __builder.OpenElement(26, "div");
            __builder.AddAttribute(27, "class", "year-picker");
            __builder.AddAttribute(28, "onmousewheel", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.WheelEventArgs>(this, 
#nullable restore
#line 15 "E:\Project\HassyaAllrightCloud\Pages\Components\YearPicker.razor"
                                                OnWheelScrolling

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(29, "\r\n            ");
            __builder.OpenElement(30, "div");
            __builder.AddAttribute(31, "class", "year-picker-header d-flex");
            __builder.AddMarkupContent(32, "\r\n                ");
            __builder.OpenElement(33, "button");
            __builder.AddAttribute(34, "class", "btn btn-sm");
            __builder.AddAttribute(35, "type", "button");
            __builder.AddAttribute(36, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 17 "E:\Project\HassyaAllrightCloud\Pages\Components\YearPicker.razor"
                                                                   (e) => showPopup = false

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(37, "\r\n                    <i class=\"fa fa-close\"></i>\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(38, "\r\n                ");
            __builder.AddMarkupContent(39, "<p class=\"p-0\">Year</p>\r\n                ");
            __builder.OpenElement(40, "button");
            __builder.AddAttribute(41, "class", "btn btn-sm");
            __builder.AddAttribute(42, "type", "button");
            __builder.AddAttribute(43, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 21 "E:\Project\HassyaAllrightCloud\Pages\Components\YearPicker.razor"
                                                                   async (e) => await Picked()

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(44, "\r\n                    <i class=\"fa fa-check\"></i>\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(45, "\r\n            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(46, "\r\n            ");
            __builder.OpenElement(47, "div");
            __builder.AddAttribute(48, "class", "year-picker-years");
            __builder.AddMarkupContent(49, "\r\n");
#nullable restore
#line 26 "E:\Project\HassyaAllrightCloud\Pages\Components\YearPicker.razor"
                 foreach (var i in years)
                {
                    if (i < min || i > max)
                    {

#line default
#line hidden
#nullable disable
            __builder.AddContent(50, "                        ");
            __builder.AddMarkupContent(51, "<p>&nbsp;</p>\r\n");
#nullable restore
#line 31 "E:\Project\HassyaAllrightCloud\Pages\Components\YearPicker.razor"
                    }
                    else
                    {

#line default
#line hidden
#nullable disable
            __builder.AddContent(52, "                        ");
            __builder.OpenElement(53, "p");
            __builder.AddAttribute(54, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 34 "E:\Project\HassyaAllrightCloud\Pages\Components\YearPicker.razor"
                                     (e) => UpdateSelectedYear(i)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(55, "class", 
#nullable restore
#line 34 "E:\Project\HassyaAllrightCloud\Pages\Components\YearPicker.razor"
                                                                            selectedYear == i ? "selected" : string.Empty

#line default
#line hidden
#nullable disable
            );
            __builder.AddContent(56, 
#nullable restore
#line 34 "E:\Project\HassyaAllrightCloud\Pages\Components\YearPicker.razor"
                                                                                                                             i

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(57, "\r\n");
#nullable restore
#line 35 "E:\Project\HassyaAllrightCloud\Pages\Components\YearPicker.razor"
                    }
                }

#line default
#line hidden
#nullable disable
            __builder.AddContent(58, "            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(59, "\r\n        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(60, "\r\n");
#nullable restore
#line 39 "E:\Project\HassyaAllrightCloud\Pages\Components\YearPicker.razor"
    }

#line default
#line hidden
#nullable disable
            __builder.CloseElement();
        }
        #pragma warning restore 1998
#nullable restore
#line 44 "E:\Project\HassyaAllrightCloud\Pages\Components\YearPicker.razor"
       
    [CascadingParameter] private EditContext EditContext { get; set; }
    [Parameter] public Expression<Func<object>> ValueExpressions { get; set; }
    bool showPopup = false;
    bool isMouseOut = false;
    int? selectedYear;
    int min = 1;
    int max = 9999;
    List<int> years;
    string Id { get; set; } = Guid.NewGuid().ToString();
    [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> UnmatchedParameters { get; set; }
    [Parameter] public DateTime? InputDate { get; set; }

    [Parameter]
    public EventCallback<DateTime?> YearChanged { get; set; }

    [Inject]
    IJSRuntime jsRuntime { get; set; }

    [JSInvokable]
    public void InvokeClickOutside()
    {
        showPopup = false;
        StateHasChanged();
    }

    protected override void OnInitialized()
    {
        if (InputDate == null) InputDate = DateTime.Now;
        InitYearPicker(InputDate.Value.Year);
    }


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await jsRuntime.InvokeVoidAsync("initClickOutSide", Id, DotNetObjectReference.Create(this));
        }
    }

    string GetClass()
    {
        if (EditContext != null)
            return (EditContext.GetValidationMessages(ValueExpressions).Any() ? " border-danger" : " border-primary");
        else return string.Empty;
    }

    async Task InputChanged(string newVal)
    {
        if (string.IsNullOrEmpty(newVal))
        {
            InputDate = null;
            selectedYear = null;
            await YearChanged.InvokeAsync(null);
        }
        else
        {
            if (int.TryParse(newVal, out int result))
            {
                if (result >= min && result <= max)
                    selectedYear = result;
                else
                    selectedYear = result > max ? max : min;

                InputDate = new DateTime(selectedYear.Value, 1, 1);
                await YearChanged.InvokeAsync(InputDate.Value);
            }
            else
            {
                // Return old value
                var temp = 1;
                InputDate = new DateTime(temp, 1, 1);
                StateHasChanged();
                await Task.Delay(1);    // flush UI changes
                InputDate = new DateTime(selectedYear ?? DateTime.Now.Year, 1, 1);
                await YearChanged.InvokeAsync(InputDate.Value);
                StateHasChanged();
                await Task.Delay(1);    // flush UI changes
            }
        }
    }

    void UpdateSelectedYear(int year)
    {
        if (year >= min && year <= max)
        {
            InitYearPicker(year);
        }
    }

    void InputOnClick(MouseEventArgs e)
    {
        showPopup = !showPopup;
        if (showPopup)
        {
            InitYearPicker(InputDate?.Year);
        }
    }

    private void InitYearPicker(int? year)
    {
        if (year >= min && year <= max)
            selectedYear = year;
        else
            selectedYear = DateTime.Now.Year;

        years = new List<int>() { selectedYear.Value - 2, selectedYear.Value - 1, selectedYear.Value, selectedYear.Value + 1, selectedYear.Value + 2 };
    }

    async Task Picked()
    {
        selectedYear = years[years.Count / 2];
        showPopup = false;
        InputDate = new DateTime(selectedYear.Value, 1, 1);
        await YearChanged.InvokeAsync(InputDate.Value);
    }

    protected void OnWheelScrolling(WheelEventArgs e)
    {
        if (e.DeltaY > 0) // Down
        {
            if (years.Last() < max + 2)
            {
                years.Add(years.Last() + 1);
                years.Remove(years.First());
            }
        }
        else // Up
        {
            if (years.First() > min - 2)
            {
                years.Insert(0, years.First() - 1);
                years.Remove(years.Last());
            }
        }

        selectedYear = years[years.Count / 2];
    }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
