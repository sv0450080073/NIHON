#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\Components\Home\ShowAlertSettingPopUp.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "fd8ec008b8bbc2fb3615643795ad21668b602968"
// <auto-generated/>
#pragma warning disable 1591
namespace HassyaAllrightCloud.Pages.Components.Home
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
    public partial class ShowAlertSettingPopUp : ShowAlertSettingPopUpBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenComponent<DevExpress.Blazor.DxPopup>(0);
            __builder.AddAttribute(1, "Id", "show-alert-setting");
            __builder.AddAttribute(2, "CssClass", "modal-lg");
            __builder.AddAttribute(3, "Visible", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Boolean>(
#nullable restore
#line 3 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\ShowAlertSettingPopUp.razor"
                         isOpeningShowAlertSettingPopUp

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(4, "VisibleChanged", new System.Action<System.Boolean>(__value => isOpeningShowAlertSettingPopUp = __value));
            __builder.AddAttribute(5, "HeaderTemplate", (Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
                __builder2.AddMarkupContent(6, "\r\n        ");
                __builder2.OpenElement(7, "div");
                __builder2.AddAttribute(8, "class", "bg-primary text-white p-2");
                __builder2.AddMarkupContent(9, "\r\n            <i class=\"fa fa-cog\" aria-hidden=\"true\"></i> ");
                __builder2.AddContent(10, 
#nullable restore
#line 6 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\ShowAlertSettingPopUp.razor"
                                                          Lang["AlertSetting"]

#line default
#line hidden
#nullable disable
                );
                __builder2.AddMarkupContent(11, "\r\n        ");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(12, "\r\n        ");
                __builder2.OpenElement(13, "ul");
                __builder2.AddAttribute(14, "class", "validation-errors");
                __builder2.AddMarkupContent(15, "\r\n");
#nullable restore
#line 9 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\ShowAlertSettingPopUp.razor"
             if (!string.IsNullOrEmpty(errorMessage))
            {

#line default
#line hidden
#nullable disable
                __builder2.AddContent(16, "                ");
                __builder2.OpenElement(17, "li");
                __builder2.AddAttribute(18, "class", "validation-message");
                __builder2.AddContent(19, 
#nullable restore
#line 11 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\ShowAlertSettingPopUp.razor"
                                                Lang[errorMessage]

#line default
#line hidden
#nullable disable
                );
                __builder2.CloseElement();
                __builder2.AddMarkupContent(20, "\r\n");
#nullable restore
#line 12 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\ShowAlertSettingPopUp.razor"
            }

#line default
#line hidden
#nullable disable
                __builder2.AddContent(21, "        ");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(22, "\r\n    ");
            }
            ));
            __builder.AddAttribute(23, "Content", (Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
                __builder2.AddMarkupContent(24, "\r\n        ");
                __builder2.OpenElement(25, "table");
                __builder2.AddAttribute(26, "class", "mb-0 w-100");
                __builder2.AddAttribute(27, "id", "personal-setting-table");
                __builder2.AddMarkupContent(28, "\r\n            ");
                __builder2.OpenElement(29, "tbody");
                __builder2.AddMarkupContent(30, "\r\n                ");
                __builder2.OpenElement(31, "tr");
                __builder2.AddAttribute(32, "style", "background-color: #909090;");
                __builder2.AddMarkupContent(33, "\r\n                    ");
                __builder2.OpenElement(34, "th");
                __builder2.AddAttribute(35, "class", "text-center");
                __builder2.AddContent(36, 
#nullable restore
#line 19 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\ShowAlertSettingPopUp.razor"
                                             Lang["Muke"]

#line default
#line hidden
#nullable disable
                );
                __builder2.CloseElement();
                __builder2.AddMarkupContent(37, "\r\n                    ");
                __builder2.OpenElement(38, "th");
                __builder2.AddContent(39, 
#nullable restore
#line 20 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\ShowAlertSettingPopUp.razor"
                         Lang["AlertName"]

#line default
#line hidden
#nullable disable
                );
                __builder2.CloseElement();
                __builder2.AddMarkupContent(40, "\r\n                    ");
                __builder2.OpenElement(41, "th");
                __builder2.AddAttribute(42, "class", "text-center");
                __builder2.AddContent(43, 
#nullable restore
#line 21 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\ShowAlertSettingPopUp.razor"
                                             Lang["AlertDisplay"]

#line default
#line hidden
#nullable disable
                );
                __builder2.CloseElement();
                __builder2.AddMarkupContent(44, "\r\n                ");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(45, "\r\n");
#nullable restore
#line 23 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\ShowAlertSettingPopUp.razor"
                 for (var i = 0; i < showAlertSettingGridDisplays.Count(); i++)
                {
                    

#line default
#line hidden
#nullable disable
#nullable restore
#line 25 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\ShowAlertSettingPopUp.razor"
                     for (var j = 0; j < showAlertSettingGridDisplays[i].ShowAlertSettingGrids.Count(); j++)
                    {
                        var t1 = i; var t2 = j;

#line default
#line hidden
#nullable disable
                __builder2.AddContent(46, "                        ");
                __builder2.OpenElement(47, "tr");
                __builder2.AddMarkupContent(48, "\r\n");
#nullable restore
#line 29 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\ShowAlertSettingPopUp.razor"
                             if (t2 == 0)
                            {

#line default
#line hidden
#nullable disable
                __builder2.AddContent(49, "                                ");
                __builder2.OpenElement(50, "td");
                __builder2.AddAttribute(51, "rowspan", 
#nullable restore
#line 31 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\ShowAlertSettingPopUp.razor"
                                              showAlertSettingGridDisplays[t1].ShowAlertSettingGrids.Count()

#line default
#line hidden
#nullable disable
                );
                __builder2.AddAttribute(52, "class", "text-center position-relative width--190");
                __builder2.AddAttribute(53, "style", "background-color:" + " " + (
#nullable restore
#line 32 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\ShowAlertSettingPopUp.razor"
                                                               showAlertSettingGridDisplays[t1].AlertTypeColor

#line default
#line hidden
#nullable disable
                ) + ";");
                __builder2.AddMarkupContent(54, "\r\n                                    ");
                __builder2.OpenElement(55, "p");
                __builder2.AddAttribute(56, "class", "item-center w-100");
                __builder2.AddContent(57, 
#nullable restore
#line 33 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\ShowAlertSettingPopUp.razor"
                                                                  showAlertSettingGridDisplays[t1].AlertTypeName

#line default
#line hidden
#nullable disable
                );
                __builder2.CloseElement();
                __builder2.AddMarkupContent(58, "\r\n                                ");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(59, "\r\n");
#nullable restore
#line 35 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\ShowAlertSettingPopUp.razor"
                            }

#line default
#line hidden
#nullable disable
                __builder2.AddContent(60, "                            ");
                __builder2.OpenElement(61, "td");
                __builder2.AddAttribute(62, "style", "background-color:" + (
#nullable restore
#line 36 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\ShowAlertSettingPopUp.razor"
                                                          showAlertSettingGridDisplays[t1].ShowAlertSettingGrids[t2].AlertColor

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddContent(63, 
#nullable restore
#line 36 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\ShowAlertSettingPopUp.razor"
                                                                                                                                   showAlertSettingGridDisplays[t1].ShowAlertSettingGrids[t2].AlertNm

#line default
#line hidden
#nullable disable
                );
                __builder2.CloseElement();
                __builder2.AddMarkupContent(64, "\r\n                            ");
                __builder2.OpenElement(65, "td");
                __builder2.AddAttribute(66, "class", "text-center");
                __builder2.AddMarkupContent(67, "\r\n                                ");
                __Blazor.HassyaAllrightCloud.Pages.Components.Home.ShowAlertSettingPopUp.TypeInference.CreateDxCheckBox_0(__builder2, 68, 69, "d-inline-block", 70, 
#nullable restore
#line 39 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\ShowAlertSettingPopUp.razor"
                                                  t1 + "" + t2

#line default
#line hidden
#nullable disable
                , 71, Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 39 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\ShowAlertSettingPopUp.razor"
                                                                           e => UpdateValue(t1, t2)

#line default
#line hidden
#nullable disable
                ), 72, 
#nullable restore
#line 38 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\ShowAlertSettingPopUp.razor"
                                                                                     showAlertSettingGridDisplays[t1].ShowAlertSettingGrids[t2].Checked

#line default
#line hidden
#nullable disable
                , 73, __value => showAlertSettingGridDisplays[t1].ShowAlertSettingGrids[t2].Checked = __value, 74, () => showAlertSettingGridDisplays[t1].ShowAlertSettingGrids[t2].Checked);
                __builder2.AddMarkupContent(75, "\r\n                            ");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(76, "\r\n                        ");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(77, "\r\n");
#nullable restore
#line 42 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\ShowAlertSettingPopUp.razor"
                    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 42 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\ShowAlertSettingPopUp.razor"
                     
                }

#line default
#line hidden
#nullable disable
                __builder2.AddContent(78, "            ");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(79, "\r\n        ");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(80, "\r\n    ");
            }
            ));
            __builder.AddAttribute(81, "FooterTemplate", (Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
                __builder2.AddMarkupContent(82, "\r\n        ");
                __builder2.OpenComponent<DevExpress.Blazor.DxButton>(83);
                __builder2.AddAttribute(84, "SizeMode", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<DevExpress.Blazor.SizeMode?>(
#nullable restore
#line 48 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\ShowAlertSettingPopUp.razor"
                            SizeMode.Medium

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(85, "CssClass", "custom-btn mx-2 float-right");
                __builder2.AddAttribute(86, "RenderStyle", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<DevExpress.Blazor.ButtonRenderStyle>(
#nullable restore
#line 48 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\ShowAlertSettingPopUp.razor"
                                                                                                  ButtonRenderStyle.Dark

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(87, "RenderStyleMode", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<DevExpress.Blazor.ButtonRenderStyleMode>(
#nullable restore
#line 48 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\ShowAlertSettingPopUp.razor"
                                                                                                                                           ButtonRenderStyleMode.Outline

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(88, "Text", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.String>(
#nullable restore
#line 49 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\ShowAlertSettingPopUp.razor"
                         Lang["Cancel"]

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(89, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 49 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\ShowAlertSettingPopUp.razor"
                                                   e => CloseModal()

#line default
#line hidden
#nullable disable
                ));
                __builder2.CloseComponent();
                __builder2.AddMarkupContent(90, "\r\n        ");
                __builder2.OpenComponent<DevExpress.Blazor.DxButton>(91);
                __builder2.AddAttribute(92, "SizeMode", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<DevExpress.Blazor.SizeMode?>(
#nullable restore
#line 50 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\ShowAlertSettingPopUp.razor"
                            SizeMode.Medium

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(93, "CssClass", "custom-btn margin-0 float-right");
                __builder2.AddAttribute(94, "RenderStyle", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<DevExpress.Blazor.ButtonRenderStyle>(
#nullable restore
#line 50 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\ShowAlertSettingPopUp.razor"
                                                                                                      ButtonRenderStyle.Primary

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(95, "RenderStyleMode", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<DevExpress.Blazor.ButtonRenderStyleMode>(
#nullable restore
#line 50 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\ShowAlertSettingPopUp.razor"
                                                                                                                                                  ButtonRenderStyleMode.Contained

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(96, "Text", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.String>(
#nullable restore
#line 51 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\ShowAlertSettingPopUp.razor"
                         Lang["Save"]

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(97, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 51 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\ShowAlertSettingPopUp.razor"
                                                 e => Save()

#line default
#line hidden
#nullable disable
                ));
                __builder2.CloseComponent();
                __builder2.AddMarkupContent(98, "\r\n    ");
            }
            ));
            __builder.CloseComponent();
        }
        #pragma warning restore 1998
    }
}
namespace __Blazor.HassyaAllrightCloud.Pages.Components.Home.ShowAlertSettingPopUp
{
    #line hidden
    internal static class TypeInference
    {
        public static void CreateDxCheckBox_0<T>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, global::System.String __arg0, int __seq1, global::System.String __arg1, int __seq2, global::System.Object __arg2, int __seq3, T __arg3, int __seq4, global::System.Action<T> __arg4, int __seq5, global::System.Linq.Expressions.Expression<global::System.Func<T>> __arg5)
        {
        __builder.OpenComponent<global::DevExpress.Blazor.DxCheckBox<T>>(seq);
        __builder.AddAttribute(__seq0, "CssClass", __arg0);
        __builder.AddAttribute(__seq1, "Id", __arg1);
        __builder.AddAttribute(__seq2, "onclick", __arg2);
        __builder.AddAttribute(__seq3, "Checked", __arg3);
        __builder.AddAttribute(__seq4, "CheckedChanged", __arg4);
        __builder.AddAttribute(__seq5, "CheckedExpression", __arg5);
        __builder.CloseComponent();
        }
    }
}
#pragma warning restore 1591
