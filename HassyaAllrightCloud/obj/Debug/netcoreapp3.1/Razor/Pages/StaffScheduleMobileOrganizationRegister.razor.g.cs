#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganizationRegister.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2d6ebc70caf7699a94d2f9325da38fe5b434eeb8"
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
    [Microsoft.AspNetCore.Components.LayoutAttribute(typeof(SPLayout))]
    public partial class StaffScheduleMobileOrganizationRegister : StaffScheduleMobileOrganizationRegisterBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
#nullable restore
#line 4 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganizationRegister.razor"
 if (isLoading)
{

#line default
#line hidden
#nullable disable
            __builder.AddContent(0, "    ");
            __builder.AddMarkupContent(1, "<div class=\"loader\">\r\n        <div class=\"loader-icon\"></div>\r\n    </div>\r\n");
#nullable restore
#line 9 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganizationRegister.razor"
}

#line default
#line hidden
#nullable disable
            __builder.OpenComponent<DevExpress.Blazor.DxPopup>(2);
            __builder.AddAttribute(3, "Visible", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Boolean>(
#nullable restore
#line 10 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganizationRegister.razor"
                         ShowErrorPopup

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(4, "VisibleChanged", new System.Action<System.Boolean>(__value => ShowErrorPopup = __value));
            __builder.AddAttribute(5, "HeaderTemplate", (Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
                __builder2.AddMarkupContent(6, "\r\n        ");
                __builder2.OpenElement(7, "div");
                __builder2.AddAttribute(8, "class", "bg-dark text-white p-2");
                __builder2.AddMarkupContent(9, "\r\n            <i class=\"fa fa-exclamation-circle\" aria-hidden=\"true\"></i> ");
                __builder2.AddContent(10, 
#nullable restore
#line 13 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganizationRegister.razor"
                                                                         Lang["Error"]

#line default
#line hidden
#nullable disable
                );
                __builder2.AddMarkupContent(11, "\r\n        ");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(12, "\r\n    ");
            }
            ));
            __builder.AddAttribute(13, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
                __builder2.AddMarkupContent(14, "\r\n        ");
                __builder2.OpenElement(15, "p");
                __builder2.AddContent(16, 
#nullable restore
#line 17 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganizationRegister.razor"
            Lang["UpdateDatabaseErrorMessage"]

#line default
#line hidden
#nullable disable
                );
                __builder2.CloseElement();
                __builder2.AddMarkupContent(17, "\r\n        ");
                __builder2.OpenElement(18, "div");
                __builder2.AddAttribute(19, "class", "text-center");
                __builder2.AddMarkupContent(20, "\r\n            ");
                __builder2.OpenElement(21, "a");
                __builder2.AddAttribute(22, "href", "javascript:void(0)");
                __builder2.AddAttribute(23, "class", "btn btn-outline-secondary width--90");
                __builder2.AddAttribute(24, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 19 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganizationRegister.razor"
                                                                                                 () => ShowErrorPopup = false

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddContent(25, "OK");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(26, "\r\n        ");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(27, "\r\n    ");
            }
            ));
            __builder.CloseComponent();
            __builder.AddMarkupContent(28, "\r\n\r\n");
            __builder.OpenElement(29, "div");
            __builder.AddAttribute(30, "id", "content-schedule-organization-sfmb");
            __builder.AddAttribute(31, "style", "overflow-y: auto");
            __builder.AddMarkupContent(32, "\r\n    ");
            __builder.OpenElement(33, "div");
            __builder.AddAttribute(34, "class", "body-schedule-mobile");
            __builder.AddMarkupContent(35, "\r\n        ");
            __builder.OpenElement(36, "div");
            __builder.AddAttribute(37, "class", "lst-sfmb-header");
            __builder.AddMarkupContent(38, "\r\n            ");
            __builder.OpenElement(39, "a");
            __builder.AddAttribute(40, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 27 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganizationRegister.razor"
                         BackToStaffScheduleMobileOrganization

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(41, "\r\n                ");
            __builder.AddMarkupContent(42, "<div class=\"float-left\">\r\n                    <i class=\"fa fa-angle-left lbl-text-white\" aria-hidden=\"true\"></i>\r\n                </div>\r\n                ");
            __builder.OpenElement(43, "div");
            __builder.AddAttribute(44, "class", "text-center");
            __builder.AddMarkupContent(45, "\r\n                    ");
            __builder.OpenElement(46, "label");
            __builder.AddAttribute(47, "class", "lbl-text-white-title");
            __builder.AddContent(48, 
#nullable restore
#line 32 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganizationRegister.razor"
                                                         Lang["TitleScheduleRegister"]

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(49, "\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(50, "\r\n            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(51, "\r\n        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(52, "\r\n        ");
            __builder.OpenElement(53, "div");
            __builder.AddAttribute(54, "class", "region-register-group-sfmb");
            __builder.AddMarkupContent(55, "\r\n            ");
            __builder.OpenComponent<Microsoft.AspNetCore.Components.Forms.EditForm>(56);
            __builder.AddAttribute(57, "Model", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Object>(
#nullable restore
#line 37 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganizationRegister.razor"
                                                    customGroupScheduleForm

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(58, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment<Microsoft.AspNetCore.Components.Forms.EditContext>)((formContext) => (__builder2) => {
                __builder2.AddMarkupContent(59, "\r\n                ");
                __builder2.OpenComponent<Microsoft.AspNetCore.Components.Forms.DataAnnotationsValidator>(60);
                __builder2.CloseComponent();
                __builder2.AddMarkupContent(61, "\r\n                ");
                __builder2.OpenComponent<HassyaAllrightCloud.Application.Validation.FluentValidator<CustomGroupScheduleFormValidator>>(62);
                __builder2.CloseComponent();
                __builder2.AddMarkupContent(63, "\r\n                ");
                __builder2.OpenElement(64, "ul");
                __builder2.AddAttribute(65, "class", "validation-errors");
                __builder2.AddMarkupContent(66, "\r\n");
#nullable restore
#line 41 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganizationRegister.razor"
                     foreach (var message in formContext.GetValidationMessages().Distinct())
                    {

#line default
#line hidden
#nullable disable
                __builder2.AddContent(67, "                        ");
                __builder2.OpenElement(68, "li");
                __builder2.AddAttribute(69, "class", "validation-message");
                __builder2.AddContent(70, 
#nullable restore
#line 43 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganizationRegister.razor"
                                                        Lang[message]

#line default
#line hidden
#nullable disable
                );
                __builder2.CloseElement();
                __builder2.AddMarkupContent(71, "\r\n");
#nullable restore
#line 44 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganizationRegister.razor"
                    }

#line default
#line hidden
#nullable disable
                __builder2.AddContent(72, "                ");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(73, "\r\n                ");
                __builder2.OpenElement(74, "div");
                __builder2.AddAttribute(75, "class", "group-name-register-sfmb");
                __builder2.AddMarkupContent(76, "\r\n                    ");
                __builder2.OpenElement(77, "label");
                __builder2.AddAttribute(78, "class", "lbl-register-group");
                __builder2.AddContent(79, 
#nullable restore
#line 47 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganizationRegister.razor"
                                                       Lang["GroupName"]

#line default
#line hidden
#nullable disable
                );
                __builder2.CloseElement();
                __builder2.AddMarkupContent(80, "\r\n                    ");
                __builder2.OpenComponent<DevExpress.Blazor.DxTextBox>(81);
                __builder2.AddAttribute(82, "CssClass", "txt-register-group length10");
                __builder2.AddAttribute(83, "Text", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.String>(
#nullable restore
#line 49 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganizationRegister.razor"
                                      customGroupScheduleForm.GroupName

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(84, "TextExpression", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Linq.Expressions.Expression<System.Func<System.String>>>(
#nullable restore
#line 50 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganizationRegister.razor"
                                                 () => customGroupScheduleForm.GroupName

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(85, "TextChanged", new System.Action<System.String>(
#nullable restore
#line 51 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganizationRegister.razor"
                                              (newValue) => ChangeGroupName(newValue)

#line default
#line hidden
#nullable disable
                ));
                __builder2.CloseComponent();
                __builder2.AddMarkupContent(86, "\r\n                ");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(87, "\r\n                ");
                __builder2.OpenElement(88, "div");
                __builder2.AddAttribute(89, "class", true);
                __builder2.AddMarkupContent(90, "\r\n                    ");
                __builder2.OpenElement(91, "label");
                __builder2.AddAttribute(92, "class", "lbl-register-group");
                __builder2.AddContent(93, 
#nullable restore
#line 55 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganizationRegister.razor"
                                                       Lang["SelectMember"]

#line default
#line hidden
#nullable disable
                );
                __builder2.CloseElement();
                __builder2.AddMarkupContent(94, "\r\n                    ");
                __Blazor.HassyaAllrightCloud.Pages.StaffScheduleMobileOrganizationRegister.TypeInference.CreateDxTagBox_0(__builder2, 95, 96, 
#nullable restore
#line 56 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganizationRegister.razor"
                                     StaffList

#line default
#line hidden
#nullable disable
                , 97, "SyainNm", 98, "txt-register-group", 99, 
#nullable restore
#line 59 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganizationRegister.razor"
                                              DataGridFilteringMode.StartsWith

#line default
#line hidden
#nullable disable
                , 100, 
#nullable restore
#line 60 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganizationRegister.razor"
                                                       DataEditorClearButtonDisplayMode.Auto

#line default
#line hidden
#nullable disable
                , 101, 
#nullable restore
#line 61 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganizationRegister.razor"
                                              customGroupScheduleForm.StaffList

#line default
#line hidden
#nullable disable
                , 102, 
#nullable restore
#line 62 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganizationRegister.razor"
                                                         () => customGroupScheduleForm.StaffList

#line default
#line hidden
#nullable disable
                , 103, 
#nullable restore
#line 63 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganizationRegister.razor"
                                                      (newValue) => ChangeMemberList(newValue)

#line default
#line hidden
#nullable disable
                );
                __builder2.AddMarkupContent(104, "\r\n                ");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(105, "\r\n                ");
                __builder2.OpenElement(106, "div");
                __builder2.AddAttribute(107, "class", "text-center");
                __builder2.AddMarkupContent(108, "\r\n                    ");
                __builder2.OpenElement(109, "button");
                __builder2.AddAttribute(110, "class", "btn btn-sm btn-cancel-group-sfmb");
                __builder2.AddAttribute(111, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 66 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganizationRegister.razor"
                                                                               BackToStaffScheduleMobileOrganization

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddContent(112, 
#nullable restore
#line 66 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganizationRegister.razor"
                                                                                                                       Lang["Cancel"]

#line default
#line hidden
#nullable disable
                );
                __builder2.CloseElement();
                __builder2.AddMarkupContent(113, "\r\n                    ");
                __builder2.OpenElement(114, "button");
                __builder2.AddAttribute(115, "class", "btn btn-sm btn-register-group-sfmb");
                __builder2.AddAttribute(116, "disabled", 
#nullable restore
#line 67 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganizationRegister.razor"
                                                                                   string.IsNullOrEmpty(customGroupScheduleForm.GroupName) || string.IsNullOrEmpty(customGroupScheduleForm.GroupName.Trim()) || customGroupScheduleForm.StaffList.Count() == 0 ? "disabled" : null

#line default
#line hidden
#nullable disable
                );
                __builder2.AddAttribute(117, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 67 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganizationRegister.razor"
                                                                                                                                                                                                                                                                                               SaveGroupSchedule

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddContent(118, 
#nullable restore
#line 67 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganizationRegister.razor"
                                                                                                                                                                                                                                                                                                                   Lang["Register"]

#line default
#line hidden
#nullable disable
                );
                __builder2.CloseElement();
                __builder2.AddMarkupContent(119, "\r\n                ");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(120, "\r\n            ");
            }
            ));
            __builder.CloseComponent();
            __builder.AddMarkupContent(121, "\r\n        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(122, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(123, "\r\n");
            __builder.CloseElement();
        }
        #pragma warning restore 1998
    }
}
namespace __Blazor.HassyaAllrightCloud.Pages.StaffScheduleMobileOrganizationRegister
{
    #line hidden
    internal static class TypeInference
    {
        public static void CreateDxTagBox_0<T>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, global::System.Collections.Generic.IEnumerable<T> __arg0, int __seq1, global::System.String __arg1, int __seq2, global::System.String __arg2, int __seq3, global::DevExpress.Blazor.DataGridFilteringMode __arg3, int __seq4, global::DevExpress.Blazor.DataEditorClearButtonDisplayMode __arg4, int __seq5, global::System.Collections.Generic.IEnumerable<T> __arg5, int __seq6, global::System.Linq.Expressions.Expression<global::System.Func<global::System.Collections.Generic.IEnumerable<T>>> __arg6, int __seq7, global::System.Action<global::System.Collections.Generic.IEnumerable<T>> __arg7)
        {
        __builder.OpenComponent<global::DevExpress.Blazor.DxTagBox<T>>(seq);
        __builder.AddAttribute(__seq0, "Data", __arg0);
        __builder.AddAttribute(__seq1, "TextFieldName", __arg1);
        __builder.AddAttribute(__seq2, "CssClass", __arg2);
        __builder.AddAttribute(__seq3, "FilteringMode", __arg3);
        __builder.AddAttribute(__seq4, "ClearButtonDisplayMode", __arg4);
        __builder.AddAttribute(__seq5, "SelectedItems", __arg5);
        __builder.AddAttribute(__seq6, "SelectedItemsExpression", __arg6);
        __builder.AddAttribute(__seq7, "SelectedItemsChanged", __arg7);
        __builder.CloseComponent();
        }
    }
}
#pragma warning restore 1591
