#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "caba9daed0dd1d30aa07981988ffbf6486af9df5"
// <auto-generated/>
#pragma warning disable 1591
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
#line 2 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
using HassyaAllrightCloud.Commons;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.LayoutAttribute(typeof(SPLayout))]
    public partial class VehicleAvailabilityConfirmationMobile : VehicleAvailabilityConfirmationMobileBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "div");
            __builder.AddAttribute(1, "class", "form-group d-flex flex-nowrap align-items-center vh-header-content");
            __builder.AddMarkupContent(2, "\r\n    ");
            __builder.OpenElement(3, "div");
            __builder.AddAttribute(4, "class", "col-md-5 pl-1");
            __builder.AddMarkupContent(5, "\r\n        ");
            __builder.OpenElement(6, "button");
            __builder.AddAttribute(7, "type", "button");
            __builder.AddAttribute(8, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 7 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                                        async (e) => await OnSetCurrentDate()

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(9, "class", "btn btn-sm bg-light float-left");
            __builder.AddContent(10, 
#nullable restore
#line 7 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                                                                                                                       _lang["today"]

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(11, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(12, "\r\n    ");
            __builder.OpenElement(13, "div");
            __builder.AddAttribute(14, "class", "col-md-2");
            __builder.AddMarkupContent(15, "\r\n        ");
            __Blazor.HassyaAllrightCloud.Pages.VehicleAvailabilityConfirmationMobile.TypeInference.CreateDxComboBox_0(__builder, 16, 17, 
#nullable restore
#line 10 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                                  busTypeSelected

#line default
#line hidden
#nullable disable
            , 18, 
#nullable restore
#line 11 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                                              () => busTypeSelected

#line default
#line hidden
#nullable disable
            , 19, 
#nullable restore
#line 12 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                           busTypes

#line default
#line hidden
#nullable disable
            , 20, 
#nullable restore
#line 13 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                               _lang["BusTypeNullText"]

#line default
#line hidden
#nullable disable
            , 21, 
#nullable restore
#line 14 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                                    false

#line default
#line hidden
#nullable disable
            , 22, 
#nullable restore
#line 15 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                                    nameof(BusType.DisplayName)

#line default
#line hidden
#nullable disable
            , 23, 
#nullable restore
#line 16 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                                   DataGridFilteringMode.Contains

#line default
#line hidden
#nullable disable
            , 24, 
#nullable restore
#line 17 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                                         async item => await BusTypeChanged(item)

#line default
#line hidden
#nullable disable
            );
            __builder.AddMarkupContent(25, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(26, "\r\n    ");
            __builder.OpenElement(27, "div");
            __builder.AddAttribute(28, "class", "col-md-5 pr-0");
            __builder.AddMarkupContent(29, "\r\n        ");
            __builder.OpenElement(30, "button");
            __builder.AddAttribute(31, "type", "button");
            __builder.AddAttribute(32, "class", "btn btn-sm vh-btn-refresh fa fa-refresh float-right");
            __builder.AddAttribute(33, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 20 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                                                                                                    async () => await OnRefresh()

#line default
#line hidden
#nullable disable
            ));
            __builder.CloseElement();
            __builder.AddMarkupContent(34, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(35, "\r\n");
            __builder.CloseElement();
            __builder.AddMarkupContent(36, "\r\n");
            __builder.OpenElement(37, "div");
            __builder.AddAttribute(38, "class", "row m-0 setting-date");
            __builder.AddMarkupContent(39, "\r\n    ");
            __builder.OpenElement(40, "table");
            __builder.AddAttribute(41, "class", "w-100");
            __builder.AddAttribute(42, "style", "height: 40px");
            __builder.AddMarkupContent(43, "\r\n        ");
            __builder.OpenElement(44, "tr");
            __builder.AddMarkupContent(45, "\r\n            ");
            __builder.OpenElement(46, "th");
            __builder.AddAttribute(47, "class", "w-50 pl-2");
            __builder.AddContent(48, 
#nullable restore
#line 26 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                                   selectedMonth.ToString(DateTimeFormat.yyyyM_JP)

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(49, "\r\n            ");
            __builder.OpenElement(50, "th");
            __builder.AddAttribute(51, "class", "pr-2 w-50 text-right");
            __builder.AddMarkupContent(52, "\r\n                ");
            __builder.OpenElement(53, "button");
            __builder.AddAttribute(54, "class", "calendar-button");
            __builder.AddAttribute(55, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 28 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                                                          async () => await NextPrevMonth(0)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(56, "<i class=\"fa fa-chevron-circle-left\"></i>");
            __builder.CloseElement();
            __builder.AddMarkupContent(57, "\r\n                ");
            __builder.OpenElement(58, "button");
            __builder.AddAttribute(59, "class", "calendar-button");
            __builder.AddAttribute(60, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 29 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                                                          async () => await NextPrevMonth(1)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(61, "<i class=\"fa fa-chevron-circle-right\"></i>");
            __builder.CloseElement();
            __builder.AddMarkupContent(62, "\r\n            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(63, "\r\n        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(64, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(65, "\r\n");
            __builder.CloseElement();
            __builder.AddMarkupContent(66, "\r\n");
            __builder.OpenElement(67, "div");
            __builder.AddAttribute(68, "class", "row m-0");
            __builder.AddMarkupContent(69, "\r\n    ");
            __builder.OpenElement(70, "ul");
            __builder.AddAttribute(71, "class", "p-0 list-unstyled mb-0 text-center w-100");
            __builder.AddMarkupContent(72, "\r\n        ");
            __builder.OpenElement(73, "li");
            __builder.AddAttribute(74, "class", "calendar-header");
            __builder.AddContent(75, 
#nullable restore
#line 36 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                                     _lang["sun"]

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(76, "\r\n        ");
            __builder.OpenElement(77, "li");
            __builder.AddAttribute(78, "class", "calendar-header");
            __builder.AddContent(79, 
#nullable restore
#line 37 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                                     _lang["mon"]

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(80, "\r\n        ");
            __builder.OpenElement(81, "li");
            __builder.AddAttribute(82, "class", "calendar-header");
            __builder.AddContent(83, 
#nullable restore
#line 38 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                                     _lang["tue"]

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(84, "\r\n        ");
            __builder.OpenElement(85, "li");
            __builder.AddAttribute(86, "class", "calendar-header");
            __builder.AddContent(87, 
#nullable restore
#line 39 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                                     _lang["wen"]

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(88, "\r\n        ");
            __builder.OpenElement(89, "li");
            __builder.AddAttribute(90, "class", "calendar-header");
            __builder.AddContent(91, 
#nullable restore
#line 40 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                                     _lang["thu"]

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(92, "\r\n        ");
            __builder.OpenElement(93, "li");
            __builder.AddAttribute(94, "class", "calendar-header");
            __builder.AddContent(95, 
#nullable restore
#line 41 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                                     _lang["fri"]

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(96, "\r\n        ");
            __builder.OpenElement(97, "li");
            __builder.AddAttribute(98, "class", "calendar-header");
            __builder.AddContent(99, 
#nullable restore
#line 42 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                                     _lang["sat"]

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(100, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(101, "\r\n");
            __builder.CloseElement();
            __builder.AddMarkupContent(102, "\r\n");
            __builder.OpenElement(103, "div");
            __builder.AddAttribute(104, "class", "row m-0");
            __builder.AddAttribute(105, "id", "vh-schedule-mobile");
            __builder.AddMarkupContent(106, "\r\n    ");
            __builder.OpenElement(107, "ul");
            __builder.AddAttribute(108, "class", "p-0 list-unstyled mb-0 text-center w-100");
            __builder.AddMarkupContent(109, "\r\n");
#nullable restore
#line 47 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
         foreach (var item in listDate.Select((value, i) => new { i, value }))
        {

#line default
#line hidden
#nullable disable
            __builder.AddContent(110, "            ");
            __builder.OpenElement(111, "li");
            __builder.AddAttribute(112, "class", (
#nullable restore
#line 49 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                        string.Format("calendar-item{0}{1}{2}{3}",
                    lastItem.Contains(item.i) ? " calendar-item-border-right" : string.Empty,
                    item.i > 34 ? " calendar-item-border-btm" : string.Empty,
                    selectedDate == item.value ? " selected-date" : string.Empty,
                    item.value.Month < selectedMonth.Month || item.value.Month > selectedMonth.Month ? " opacity05 " : string.Empty)

#line default
#line hidden
#nullable disable
            ) + " ");
            __builder.AddAttribute(113, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 54 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                          async () => await OnSelectDate(item.value)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(114, "\r\n                ");
            __builder.OpenElement(115, "div");
            __builder.AddAttribute(116, "class", "calendar-child-item");
            __builder.AddContent(117, 
#nullable restore
#line 55 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                                                   item.value.Day < 10 ? string.Format("0{0}", item.value.Day) : item.value.Day.ToString()

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(118, "\r\n            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(119, "\r\n");
#nullable restore
#line 57 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
        }

#line default
#line hidden
#nullable disable
            __builder.AddContent(120, "    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(121, "\r\n");
            __builder.CloseElement();
            __builder.AddMarkupContent(122, "\r\n");
            __builder.OpenElement(123, "div");
            __builder.AddAttribute(124, "class", "vh-header-detail-content");
            __builder.AddMarkupContent(125, "\r\n    ");
            __builder.OpenElement(126, "div");
            __builder.AddAttribute(127, "class", "float-left");
            __builder.AddMarkupContent(128, "\r\n        ");
            __builder.OpenElement(129, "label");
            __builder.AddAttribute(130, "class", "custome-size-device");
            __builder.AddAttribute(131, "style", "margin-left: 7px;margin-right: 7px;");
            __builder.AddContent(132, 
#nullable restore
#line 62 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                                                                                         IsYesterday ? _lang["yesterday"] : IsToday ? _lang["today"] : IsTomorrow ? _lang["tomorrow"] : string.Empty

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(133, "\r\n        ");
            __builder.OpenElement(134, "label");
            __builder.AddAttribute(135, "class", "custome-size-device");
            __builder.AddMarkupContent(136, "空 運 転 手 ");
            __builder.AddContent(137, 
#nullable restore
#line 63 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                                                    EmptyLargeDriverCount

#line default
#line hidden
#nullable disable
            );
            __builder.AddMarkupContent(138, " 人 (");
            __builder.AddContent(139, 
#nullable restore
#line 63 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                                                                              AbsenceLargeDriverCount

#line default
#line hidden
#nullable disable
            );
            __builder.AddContent(140, "/");
            __builder.AddContent(141, 
#nullable restore
#line 63 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                                                                                                       LargeDriverCount

#line default
#line hidden
#nullable disable
            );
            __builder.AddContent(142, ")");
            __builder.CloseElement();
            __builder.AddMarkupContent(143, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(144, "\r\n    ");
            __builder.OpenElement(145, "div");
            __builder.AddAttribute(146, "class", "float-right");
            __builder.AddMarkupContent(147, "\r\n        ");
            __builder.OpenElement(148, "label");
            __builder.AddAttribute(149, "class", "custome-size-device");
            __builder.AddContent(150, 
#nullable restore
#line 66 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                                            selectedDate.ToString(DateTimeFormat.yyyyMMdd_ddd_JP)

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(151, "\r\n        ");
            __builder.OpenElement(152, "button");
            __builder.AddAttribute(153, "type", "button");
            __builder.AddAttribute(154, "class", "btn btn-sm fa fa-chevron-circle-left vh-btn-custom-sfmb btn-custom-sfmb-left");
            __builder.AddAttribute(155, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 67 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                                                                                                                             () => NextPrevDate(0)

#line default
#line hidden
#nullable disable
            ));
            __builder.CloseElement();
            __builder.AddMarkupContent(156, "\r\n        ");
            __builder.OpenElement(157, "button");
            __builder.AddAttribute(158, "type", "button");
            __builder.AddAttribute(159, "class", "btn btn-sm fa fa-chevron-circle-right vh-btn-custom-sfmb");
            __builder.AddAttribute(160, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 68 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                                                                                                         () => NextPrevDate(1)

#line default
#line hidden
#nullable disable
            ));
            __builder.CloseElement();
            __builder.AddMarkupContent(161, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(162, "\r\n");
            __builder.CloseElement();
            __builder.AddMarkupContent(163, "\r\n\r\n");
            __builder.OpenElement(164, "div");
            __builder.AddAttribute(165, "id", "tableVhScheduleMB");
            __builder.AddAttribute(166, "class", "lst-sfmb-table tableVhScheduleMB");
            __builder.AddMarkupContent(167, "\r\n");
#nullable restore
#line 73 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
     if (groupBusInfo != null && groupBusInfo?.Count > 0)
    {
        

#line default
#line hidden
#nullable disable
#nullable restore
#line 75 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
         foreach (var item in groupBusInfo)
        {

#line default
#line hidden
#nullable disable
            __builder.AddContent(168, "            ");
            __builder.OpenElement(169, "div");
            __builder.AddAttribute(170, "class", "vh-schedule-mobile-title-section");
            __builder.AddMarkupContent(171, "\r\n                <i class=\"fa fa-angle-down vh-lbl-title-header-sfmb\" aria-hidden=\"true\"></i>\r\n                ");
            __builder.OpenElement(172, "label");
            __builder.AddAttribute(173, "style", "width: 70px");
            __builder.AddContent(174, 
#nullable restore
#line 79 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                                            item.BusTypeName

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(175, "\r\n                ");
            __builder.AddMarkupContent(176, "<label>空台数：</label>\r\n                ");
            __builder.OpenElement(177, "label");
            __builder.AddAttribute(178, "style", "width: 40px;");
            __builder.AddContent(179, 
#nullable restore
#line 81 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                                             item.UnUseBusCount

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(180, "\r\n                ");
            __builder.OpenElement(181, "label");
            __builder.AddContent(182, "(");
            __builder.AddContent(183, 
#nullable restore
#line 82 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                         item.InUseBusCount

#line default
#line hidden
#nullable disable
            );
            __builder.AddContent(184, " /");
            __builder.AddContent(185, 
#nullable restore
#line 82 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                                              item.BusCount

#line default
#line hidden
#nullable disable
            );
            __builder.AddContent(186, ")");
            __builder.CloseElement();
            __builder.AddMarkupContent(187, "\r\n                ");
            __builder.AddMarkupContent(188, "<button class=\"btn btn-sm vh-btn-plus-right-sfmb\"> <i class=\"fa fa-plus\"></i> </button>\r\n            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(189, "\r\n");
            __builder.AddContent(190, "            ");
            __builder.OpenElement(191, "div");
            __builder.AddAttribute(192, "class", "vh-group-schedule-sfmbg");
            __builder.AddMarkupContent(193, "\r\n");
#nullable restore
#line 87 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                 if (item.BusDetails != null && item.BusDetails?.Count > 0)
                {
                    

#line default
#line hidden
#nullable disable
#nullable restore
#line 89 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                     foreach (var itemx in item.BusDetails)
                    {

#line default
#line hidden
#nullable disable
            __builder.AddContent(194, "                        ");
            __builder.OpenElement(195, "div");
            __builder.AddAttribute(196, "id", "otherGroupScheduleList");
            __builder.AddAttribute(197, "class", "group-schedule-sfmb");
            __builder.AddAttribute(198, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 91 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                                                                                               () => OnNavigate(itemx)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(199, "\r\n                            ");
            __builder.OpenElement(200, "a");
            __builder.AddMarkupContent(201, "\r\n                                ");
            __builder.OpenElement(202, "label");
            __builder.AddAttribute(203, "style", "width: 90px;");
            __builder.AddAttribute(204, "class", "float-left vh-lbl-left-sfmb");
            __builder.AddContent(205, 
#nullable restore
#line 93 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                                                                                                 itemx.BusTypeNameDetail

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(206, "\r\n                                ");
            __builder.OpenElement(207, "label");
            __builder.AddAttribute(208, "class", "vh-lbl-left-sfmb");
            __builder.AddContent(209, 
#nullable restore
#line 94 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                                                                 itemx.Status

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(210, "\r\n                            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(211, "\r\n                        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(212, "\r\n");
#nullable restore
#line 97 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 97 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
                     
                }

#line default
#line hidden
#nullable disable
            __builder.AddContent(213, "            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(214, "\r\n");
#nullable restore
#line 100 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
        }

#line default
#line hidden
#nullable disable
#nullable restore
#line 100 "E:\Project\HassyaAllrightCloud\Pages\VehicleAvailabilityConfirmationMobile.razor"
         
    }

#line default
#line hidden
#nullable disable
            __builder.CloseElement();
        }
        #pragma warning restore 1998
    }
}
namespace __Blazor.HassyaAllrightCloud.Pages.VehicleAvailabilityConfirmationMobile
{
    #line hidden
    internal static class TypeInference
    {
        public static void CreateDxComboBox_0<T>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, T __arg0, int __seq1, global::System.Linq.Expressions.Expression<global::System.Func<T>> __arg1, int __seq2, global::System.Collections.Generic.IEnumerable<T> __arg2, int __seq3, global::System.String __arg3, int __seq4, global::System.Boolean __arg4, int __seq5, global::System.String __arg5, int __seq6, global::DevExpress.Blazor.DataGridFilteringMode __arg6, int __seq7, global::System.Action<T> __arg7)
        {
        __builder.OpenComponent<global::DevExpress.Blazor.DxComboBox<T>>(seq);
        __builder.AddAttribute(__seq0, "SelectedItem", __arg0);
        __builder.AddAttribute(__seq1, "SelectedItemExpression", __arg1);
        __builder.AddAttribute(__seq2, "Data", __arg2);
        __builder.AddAttribute(__seq3, "NullText", __arg3);
        __builder.AddAttribute(__seq4, "AllowUserInput", __arg4);
        __builder.AddAttribute(__seq5, "TextFieldName", __arg5);
        __builder.AddAttribute(__seq6, "FilteringMode", __arg6);
        __builder.AddAttribute(__seq7, "SelectedItemChanged", __arg7);
        __builder.CloseComponent();
        }
    }
}
#pragma warning restore 1591