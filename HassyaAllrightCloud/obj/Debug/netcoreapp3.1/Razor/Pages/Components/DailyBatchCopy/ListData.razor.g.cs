#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "dd5f7cd0d3ca316902880da3e78f64ca0b7cfe39"
// <auto-generated/>
#pragma warning disable 1591
namespace HassyaAllrightCloud.Pages.Components.DailyBatchCopy
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
    public partial class ListData : ListDataBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "div");
            __builder.AddAttribute(1, "id", "table-container");
            __builder.AddAttribute(2, "class", "mb-2 w-100 overflow-auto");
            __builder.AddAttribute(3, "style", "display: flex; flex: 1; flex-direction: column;");
            __builder.AddMarkupContent(4, "\r\n    ");
            __builder.OpenElement(5, "table");
            __builder.AddAttribute(6, "class", "table normal-table table-sm vehicle-table fixed-header mb-0 table-layout");
            __builder.AddAttribute(7, "style", "min-width: 1970px; table-layout: fixed; border-collapse: separate; border-spacing: 0");
            __builder.AddMarkupContent(8, "\r\n        ");
            __builder.OpenElement(9, "thead");
            __builder.AddMarkupContent(10, "\r\n            ");
            __builder.OpenElement(11, "tr");
            __builder.AddMarkupContent(12, "\r\n                <th style=\"width: 50px\" class=\"sticky header-sticky\"></th>\r\n                ");
            __builder.OpenElement(13, "th");
            __builder.AddAttribute(14, "style", "width: 50px; left: 50px !important; border-right: 1px solid #dee2e6 !important");
            __builder.AddAttribute(15, "class", "sticky header-sticky");
            __builder.AddContent(16, 
#nullable restore
#line 9 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                                                                                                                         _lang["No"]

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(17, "\r\n                ");
            __builder.OpenElement(18, "th");
            __builder.AddAttribute(19, "style", "width: 140px; z-index: 1");
            __builder.AddContent(20, 
#nullable restore
#line 10 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                                      _lang["Receipt_number"]

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(21, "\r\n                ");
            __builder.OpenElement(22, "th");
            __builder.AddAttribute(23, "style", "width: 120px; z-index: 1");
            __builder.AddContent(24, 
#nullable restore
#line 11 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                                      _lang["Vehicle_dispatch_date"]

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(25, "\r\n                ");
            __builder.OpenElement(26, "th");
            __builder.AddAttribute(27, "style", "width: 120px; z-index: 1");
            __builder.AddContent(28, 
#nullable restore
#line 12 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                                      _lang["Arrival_date"]

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(29, "\r\n                ");
            __builder.OpenElement(30, "th");
            __builder.AddAttribute(31, "style", "width: 130px; z-index: 1");
            __builder.AddContent(32, 
#nullable restore
#line 13 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                                      _lang["Customer"]

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(33, "\r\n                ");
            __builder.OpenElement(34, "th");
            __builder.AddAttribute(35, "style", "width: 130px; z-index: 1");
            __builder.AddContent(36, 
#nullable restore
#line 14 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                                      _lang["Branch_name"]

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(37, "\r\n                ");
            __builder.OpenElement(38, "th");
            __builder.AddAttribute(39, "style", "width: 200px; z-index: 1");
            __builder.AddContent(40, 
#nullable restore
#line 15 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                                      _lang["Group_name"]

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(41, "\r\n                ");
            __builder.OpenElement(42, "th");
            __builder.AddAttribute(43, "style", "width: 130px; z-index: 1");
            __builder.AddContent(44, 
#nullable restore
#line 16 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                                      _lang["Destination_name"]

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(45, "\r\n                ");
            __builder.OpenElement(46, "th");
            __builder.AddAttribute(47, "style", "width: 150px; z-index: 1");
            __builder.AddContent(48, 
#nullable restore
#line 17 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                                      _lang["Drivers_guides"]

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(49, "\r\n                ");
            __builder.OpenElement(50, "th");
            __builder.AddAttribute(51, "style", "width: 120px; z-index: 1");
            __builder.AddContent(52, 
#nullable restore
#line 18 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                                      _lang["Fare"]

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(53, "\r\n                ");
            __builder.OpenElement(54, "th");
            __builder.AddAttribute(55, "style", "width: 120px; z-index: 1");
            __builder.AddContent(56, 
#nullable restore
#line 19 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                                      _lang["Fee"]

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(57, "\r\n                ");
            __builder.OpenElement(58, "th");
            __builder.AddAttribute(59, "style", "width: 120px; z-index: 1");
            __builder.AddContent(60, 
#nullable restore
#line 20 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                                      _lang["Unit_price"]

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(61, "\r\n                ");
            __builder.OpenElement(62, "th");
            __builder.AddAttribute(63, "style", "width: 70px; z-index: 1");
            __builder.AddContent(64, 
#nullable restore
#line 21 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                                     _lang["Number_of_units"]

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(65, "\r\n                ");
            __builder.OpenElement(66, "th");
            __builder.AddAttribute(67, "style", "width: 120px; z-index: 1");
            __builder.AddContent(68, 
#nullable restore
#line 22 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                                      _lang["Bus_fare"]

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(69, "\r\n                ");
            __builder.OpenElement(70, "th");
            __builder.AddAttribute(71, "style", "width: 120px; z-index: 1");
            __builder.AddContent(72, 
#nullable restore
#line 23 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                                      _lang["Guide_unit_price"]

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(73, "\r\n                ");
            __builder.OpenElement(74, "th");
            __builder.AddAttribute(75, "style", "width: 120px; z-index: 1");
            __builder.AddContent(76, 
#nullable restore
#line 24 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                                      _lang["Guide_fee"]

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(77, "\r\n            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(78, "\r\n        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(79, "\r\n        ");
            __builder.OpenElement(80, "tbody");
            __builder.AddMarkupContent(81, "\r\n");
#nullable restore
#line 28 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
             if (listDataDisplay.Count > 0)
            {
                var count = 1;
                

#line default
#line hidden
#nullable disable
#nullable restore
#line 31 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                 foreach (var item in listDataDisplay)
                {

#line default
#line hidden
#nullable disable
            __builder.AddContent(82, "                    ");
            __builder.OpenElement(83, "tr");
            __builder.AddMarkupContent(84, "\r\n                        ");
            __builder.OpenElement(85, "td");
            __builder.AddAttribute(86, "class", "text-center sticky col-sticky");
            __builder.OpenElement(87, "i");
            __builder.AddAttribute(88, "class", "fa fa-times remove-item");
            __builder.AddAttribute(89, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 34 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                                                                                                 async () => await OnRemoveItem(item)

#line default
#line hidden
#nullable disable
            ));
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(90, "\r\n                        ");
            __builder.OpenElement(91, "td");
            __builder.AddAttribute(92, "class", "text-center sticky col-sticky");
            __builder.AddAttribute(93, "style", "left: 50px !important");
            __builder.AddContent(94, 
#nullable restore
#line 35 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                                                                                  count++

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(95, "\r\n                        ");
            __builder.OpenElement(96, "td");
            __builder.AddAttribute(97, "class", "text-left");
            __builder.AddContent(98, 
#nullable restore
#line 36 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                               item.UkeNo.Substring(5)

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(99, "\r\n                        ");
            __builder.OpenElement(100, "td");
            __builder.AddAttribute(101, "class", "text-left");
            __builder.AddMarkupContent(102, "\r\n                            ");
            __builder.AddContent(103, 
#nullable restore
#line 38 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                              string.IsNullOrEmpty(item.HaiSYmd) ? string.Empty : DateTime.ParseExact(item.HaiSYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture).ToString(CommonConstants.FormatYMDWithSlash)

#line default
#line hidden
#nullable disable
            );
            __builder.AddMarkupContent(104, "\r\n                        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(105, "\r\n                        ");
            __builder.OpenElement(106, "td");
            __builder.AddAttribute(107, "class", "text-left");
            __builder.AddMarkupContent(108, "\r\n                            ");
            __builder.AddContent(109, 
#nullable restore
#line 41 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                              string.IsNullOrEmpty(item.TouYmd) ? string.Empty : DateTime.ParseExact(item.TouYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture).ToString(CommonConstants.FormatYMDWithSlash)

#line default
#line hidden
#nullable disable
            );
            __builder.AddMarkupContent(110, "\r\n                        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(111, "\r\n                        ");
            __builder.OpenElement(112, "td");
            __builder.AddAttribute(113, "class", "text-left");
            __builder.AddContent(114, 
#nullable restore
#line 43 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                               item.TokisakiNm

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(115, "\r\n                        ");
            __builder.OpenElement(116, "td");
            __builder.AddAttribute(117, "class", "text-left");
            __builder.AddContent(118, 
#nullable restore
#line 44 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                               item.TokisitenNm

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(119, "\r\n                        ");
            __builder.OpenElement(120, "td");
            __builder.AddAttribute(121, "class", "text-left");
            __builder.AddContent(122, 
#nullable restore
#line 45 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                               item.DanTaNm

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(123, "\r\n                        ");
            __builder.OpenElement(124, "td");
            __builder.AddAttribute(125, "class", "text-left");
            __builder.AddContent(126, 
#nullable restore
#line 46 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                               item.IkNm

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(127, "\r\n                        ");
            __builder.OpenElement(128, "td");
            __builder.AddAttribute(129, "class", "text-right");
            __builder.AddContent(130, 
#nullable restore
#line 47 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                                item.DriverNum

#line default
#line hidden
#nullable disable
            );
            __builder.AddContent(131, " / ");
            __builder.AddContent(132, 
#nullable restore
#line 47 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                                                  item.GuiderNum

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(133, "\r\n                        ");
            __builder.OpenElement(134, "td");
            __builder.AddAttribute(135, "class", "text-right");
            __builder.AddContent(136, 
#nullable restore
#line 48 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                                item.Unchin.ToString("N0")

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(137, "\r\n                        ");
            __builder.OpenElement(138, "td");
            __builder.AddAttribute(139, "class", "text-right");
            __builder.AddContent(140, 
#nullable restore
#line 49 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                                item.Ryokin.ToString("N0")

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(141, "\r\n                        ");
            __builder.OpenElement(142, "td");
            __builder.AddAttribute(143, "class", "text-right");
            __builder.AddContent(144, 
#nullable restore
#line 50 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                                item.SyaSyuTan.ToString("N0")

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(145, "\r\n                        ");
            __builder.OpenElement(146, "td");
            __builder.AddAttribute(147, "class", "text-right");
            __builder.AddContent(148, 
#nullable restore
#line 51 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                                item.SyaSyuDai

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(149, "\r\n                        ");
            __builder.OpenElement(150, "td");
            __builder.AddAttribute(151, "class", "text-right");
            __builder.AddContent(152, 
#nullable restore
#line 52 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                                item.SyaRyoUnc.ToString("N0")

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(153, "\r\n                        ");
            __builder.OpenElement(154, "td");
            __builder.AddAttribute(155, "class", "text-right");
            __builder.AddContent(156, 
#nullable restore
#line 53 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                                item.UnitGuiderPrice.ToString("N0")

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(157, "\r\n                        ");
            __builder.OpenElement(158, "td");
            __builder.AddAttribute(159, "class", "text-right");
            __builder.AddContent(160, 
#nullable restore
#line 54 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                                item.UnitGuiderFee.ToString("N0")

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(161, "\r\n                    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(162, "\r\n");
#nullable restore
#line 56 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                }

#line default
#line hidden
#nullable disable
#nullable restore
#line 56 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                 
            }
            else
            {

#line default
#line hidden
#nullable disable
            __builder.AddContent(163, "                ");
            __builder.OpenElement(164, "tr");
            __builder.AddMarkupContent(165, "\r\n                    ");
            __builder.OpenElement(166, "td");
            __builder.AddAttribute(167, "colspan", "17");
            __builder.AddContent(168, 
#nullable restore
#line 61 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                      _lang["tablehasnorows"]

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(169, "\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(170, "\r\n");
#nullable restore
#line 63 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
            }

#line default
#line hidden
#nullable disable
            __builder.AddContent(171, "        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(172, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(173, "\r\n");
            __builder.CloseElement();
            __builder.AddMarkupContent(174, "\r\n");
            __builder.OpenElement(175, "div");
            __builder.AddAttribute(176, "class", "mb-2");
            __builder.AddMarkupContent(177, "\r\n    ");
            __builder.OpenComponent<HassyaAllrightCloud.Pages.Components.Pagination>(178);
            __builder.AddAttribute(179, "OnChangePage", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<System.Int32>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<System.Int32>(this, 
#nullable restore
#line 68 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                                                   OnChangePage

#line default
#line hidden
#nullable disable
            )));
            __builder.AddAttribute(180, "ItemPerPage", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Byte>(
#nullable restore
#line 68 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                                                                              itemPerPage

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(181, "OnChangeItemPerPage", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<System.Byte>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<System.Byte>(this, 
#nullable restore
#line 68 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                                                                                                                OnChangeItemPerPage

#line default
#line hidden
#nullable disable
            )));
            __builder.AddAttribute(182, "TotalCount", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Int32>(
#nullable restore
#line 69 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                                                 listData.Count

#line default
#line hidden
#nullable disable
            ));
            __builder.AddComponentReferenceCapture(183, (__value) => {
#nullable restore
#line 69 "E:\Project\HassyaAllrightCloud\Pages\Components\DailyBatchCopy\ListData.razor"
                                                                                       paging = (HassyaAllrightCloud.Pages.Components.Pagination)__value;

#line default
#line hidden
#nullable disable
            }
            );
            __builder.CloseComponent();
            __builder.AddMarkupContent(184, "\r\n");
            __builder.CloseElement();
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591