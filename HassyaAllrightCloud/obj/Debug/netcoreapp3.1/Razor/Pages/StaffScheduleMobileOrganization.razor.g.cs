#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganization.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7c48c857c9747800e8a89ec0305df1e396787078"
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
    public partial class StaffScheduleMobileOrganization : StaffScheduleMobileOrganizationBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
#nullable restore
#line 4 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganization.razor"
 if (isLoading)
{

#line default
#line hidden
#nullable disable
            __builder.AddContent(0, "    ");
            __builder.AddMarkupContent(1, "<div class=\"loader\">\r\n        <div class=\"loader-icon\"></div>\r\n    </div>\r\n");
#nullable restore
#line 9 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganization.razor"
}

#line default
#line hidden
#nullable disable
            __builder.AddMarkupContent(2, "\r\n");
            __builder.OpenComponent<DevExpress.Blazor.DxPopup>(3);
            __builder.AddAttribute(4, "Visible", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Boolean>(
#nullable restore
#line 11 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganization.razor"
                         ShowErrorPopup

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(5, "VisibleChanged", new System.Action<System.Boolean>(__value => ShowErrorPopup = __value));
            __builder.AddAttribute(6, "HeaderTemplate", (Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
                __builder2.AddMarkupContent(7, "\r\n        ");
                __builder2.OpenElement(8, "div");
                __builder2.AddAttribute(9, "class", "bg-dark text-white p-2");
                __builder2.AddMarkupContent(10, "\r\n            <i class=\"fa fa-exclamation-circle\" aria-hidden=\"true\"></i> ");
                __builder2.AddContent(11, 
#nullable restore
#line 14 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganization.razor"
                                                                         Lang["Error"]

#line default
#line hidden
#nullable disable
                );
                __builder2.AddMarkupContent(12, "\r\n        ");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(13, "\r\n    ");
            }
            ));
            __builder.AddAttribute(14, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
                __builder2.AddMarkupContent(15, "\r\n        ");
                __builder2.OpenElement(16, "p");
                __builder2.AddContent(17, 
#nullable restore
#line 18 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganization.razor"
            Lang["UpdateDatabaseErrorMessage"]

#line default
#line hidden
#nullable disable
                );
                __builder2.CloseElement();
                __builder2.AddMarkupContent(18, "\r\n        ");
                __builder2.OpenElement(19, "div");
                __builder2.AddAttribute(20, "class", "text-center");
                __builder2.AddMarkupContent(21, "\r\n            ");
                __builder2.OpenElement(22, "a");
                __builder2.AddAttribute(23, "href", "javascript:void(0)");
                __builder2.AddAttribute(24, "class", "btn btn-outline-secondary width--90");
                __builder2.AddAttribute(25, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 20 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganization.razor"
                                                                                                 () => ShowErrorPopup = false

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddContent(26, "OK");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(27, "\r\n        ");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(28, "\r\n    ");
            }
            ));
            __builder.CloseComponent();
            __builder.AddMarkupContent(29, "\r\n\r\n");
            __builder.OpenElement(30, "div");
            __builder.AddAttribute(31, "id", "content-schedule-organization-sfmb");
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
#line 28 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganization.razor"
                         BackToStaffScheduleMobile

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
#line 33 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganization.razor"
                                                         Lang["TitleScheduleSelection"]

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
            __builder.AddAttribute(54, "id", "tableGroupScheduleMB");
            __builder.AddAttribute(55, "class", "lst-sfmb-table");
            __builder.AddMarkupContent(56, "\r\n            ");
            __builder.OpenElement(57, "div");
            __builder.AddAttribute(58, "class", "group-schedule-mobile-title-section");
            __builder.AddMarkupContent(59, "\r\n                <i class=\"fa fa-angle-up lbl-title-header-sfmb\" aria-hidden=\"true\"></i>");
            __builder.AddContent(60, 
#nullable restore
#line 39 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganization.razor"
                                                                                         new HassyaAllrightCloud.Domain.Dto.ClaimModel().Name

#line default
#line hidden
#nullable disable
            );
            __builder.AddMarkupContent(61, "\r\n            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(62, "\r\n            ");
            __builder.OpenElement(63, "div");
            __builder.AddAttribute(64, "class", "group-schedule-sfmbg");
            __builder.AddMarkupContent(65, "\r\n                ");
            __builder.OpenElement(66, "a");
            __builder.AddAttribute(67, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 42 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganization.razor"
                             BackToStaffScheduleMobile

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(68, "\r\n                    ");
            __builder.OpenElement(69, "label");
            __builder.AddAttribute(70, "class", "float-left lbl-left-sfmb");
            __builder.AddContent(71, 
#nullable restore
#line 43 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganization.razor"
                                                              new HassyaAllrightCloud.Domain.Dto.ClaimModel().Name

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(72, "\r\n                    ");
            __builder.AddMarkupContent(73, "<div class=\"float-right\">\r\n                        <i class=\"fa fa-angle-right icon-size-sfmb group-i-sfmb\" aria-hidden=\"true\"></i>\r\n                    </div>\r\n                    <div class=\"text-center\" style=\"height: 40px\"></div>\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(74, "\r\n            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(75, "\r\n");
#nullable restore
#line 50 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganization.razor"
             if (CompaniesScheduleInfo != null && CompaniesScheduleInfo.Count > 0)
            {
                

#line default
#line hidden
#nullable disable
#nullable restore
#line 52 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganization.razor"
                 foreach (CompanyScheduleInfo CompanyScheduleInfo in CompaniesScheduleInfo)
                {

#line default
#line hidden
#nullable disable
            __builder.AddContent(76, "                    ");
            __builder.OpenElement(77, "div");
            __builder.AddAttribute(78, "class", "group-schedule-mobile-title-section");
            __builder.AddMarkupContent(79, "\r\n                        <i class=\"fa fa-angle-up lbl-title-header-sfmb\" aria-hidden=\"true\"></i> ");
            __builder.AddContent(80, 
#nullable restore
#line 55 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganization.razor"
                                                                                                  CompanyScheduleInfo.CompanyId != 0 ? CompanyScheduleInfo.CompanyName : Lang["Custom"]

#line default
#line hidden
#nullable disable
            );
            __builder.AddMarkupContent(81, "\r\n");
#nullable restore
#line 56 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganization.razor"
                         if (CompanyScheduleInfo.CompanyId == 0)
                        {

#line default
#line hidden
#nullable disable
            __builder.AddContent(82, "                            ");
            __builder.OpenElement(83, "button");
            __builder.AddAttribute(84, "class", "btn btn-sm btn-plus-right-sfmb");
            __builder.AddAttribute(85, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 58 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganization.razor"
                                                                                     ()=> AddScheduleGroup()

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(86, " <i class=\"fa fa-plus\"></i> ");
            __builder.CloseElement();
            __builder.AddMarkupContent(87, "\r\n");
#nullable restore
#line 59 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganization.razor"
                        }

#line default
#line hidden
#nullable disable
            __builder.AddContent(88, "                    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(89, "\r\n                    ");
            __builder.OpenElement(90, "div");
            __builder.AddAttribute(91, "class", "group-schedule-sfmbg");
            __builder.AddMarkupContent(92, "\r\n");
#nullable restore
#line 62 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganization.razor"
                         if (CompanyScheduleInfo.GroupInfo != null && CompanyScheduleInfo.GroupInfo.Count > 0)
                        {
                            

#line default
#line hidden
#nullable disable
#nullable restore
#line 64 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganization.razor"
                             foreach (GroupScheduleInfo GroupInfo in CompanyScheduleInfo.GroupInfo)
                            {

#line default
#line hidden
#nullable disable
            __builder.AddContent(93, "                                ");
            __builder.OpenElement(94, "div");
            __builder.AddAttribute(95, "id", "otherGroupScheduleList");
            __builder.AddAttribute(96, "class", "group-schedule-sfmb");
            __builder.AddMarkupContent(97, "\r\n");
#nullable restore
#line 67 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganization.razor"
                                     if (GroupInfo.CompanyId == 0)
                                    {

#line default
#line hidden
#nullable disable
            __builder.AddContent(98, "                                        ");
            __builder.OpenElement(99, "a");
            __builder.AddAttribute(100, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 69 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganization.razor"
                                                     ()=> ShowScheduleGroup(GroupInfo)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(101, "\r\n                                            ");
            __builder.OpenElement(102, "label");
            __builder.AddAttribute(103, "class", "float-left lbl-left-sfmb");
            __builder.AddContent(104, 
#nullable restore
#line 70 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganization.razor"
                                                                                     GroupInfo.GroupName

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(105, "\r\n                                        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(106, "\r\n                                        ");
            __builder.OpenElement(107, "div");
            __builder.AddAttribute(108, "class", "float-right");
            __builder.AddMarkupContent(109, "\r\n                                            ");
            __builder.OpenElement(110, "button");
            __builder.AddAttribute(111, "class", "btn btn-sm group-btn-sfmb");
            __builder.AddAttribute(112, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 73 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganization.razor"
                                                                                                ()=> EditGroup(GroupInfo)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(113, "\r\n                                                <i class=\"fa fa-pencil\"></i>\r\n                                            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(114, "\r\n                                            ");
            __builder.OpenElement(115, "button");
            __builder.AddAttribute(116, "class", "btn btn-sm group-btn-sfmb");
            __builder.AddAttribute(117, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 76 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganization.razor"
                                                                                                ()=> DeleteGroup(GroupInfo)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(118, "\r\n                                                <i class=\"fa fa-trash\"></i>\r\n                                            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(119, "\r\n                                            ");
            __builder.OpenElement(120, "button");
            __builder.AddAttribute(121, "class", "btn btn-sm group-btn-sfmb");
            __builder.AddAttribute(122, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 79 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganization.razor"
                                                                                                ()=> ShowScheduleGroup(GroupInfo)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(123, "\r\n                                                <i class=\"fa fa-angle-right icon-size-sfmb\" aria-hidden=\"true\"></i>\r\n                                            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(124, "\r\n                                        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(125, "\r\n                                        ");
            __builder.OpenElement(126, "a");
            __builder.AddAttribute(127, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 83 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganization.razor"
                                                     ()=> ShowScheduleGroup(GroupInfo)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(128, "<div class=\"text-center\" style=\"height: 40px\"></div>");
            __builder.CloseElement();
            __builder.AddMarkupContent(129, "\r\n");
#nullable restore
#line 84 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganization.razor"
                                    }
                                    else
                                    {

#line default
#line hidden
#nullable disable
            __builder.AddContent(130, "                                        ");
            __builder.OpenElement(131, "a");
            __builder.AddAttribute(132, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 87 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganization.razor"
                                                     ()=> ShowScheduleGroup(GroupInfo)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(133, "\r\n                                            ");
            __builder.OpenElement(134, "label");
            __builder.AddAttribute(135, "class", "float-left lbl-left-sfmb");
            __builder.AddContent(136, 
#nullable restore
#line 88 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganization.razor"
                                                                                     GroupInfo.GroupName

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(137, "\r\n                                            ");
            __builder.AddMarkupContent(138, @"<div class=""float-right"">
                                                <i class=""fa fa-angle-right icon-size-sfmb group-i-sfmb"" aria-hidden=""true""></i>
                                            </div>
                                            <div class=""text-center"" style=""height: 40px""></div>
                                        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(139, "\r\n");
#nullable restore
#line 94 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganization.razor"
                                    }

#line default
#line hidden
#nullable disable
            __builder.AddContent(140, "                                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(141, "\r\n");
#nullable restore
#line 96 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganization.razor"
                            }

#line default
#line hidden
#nullable disable
#nullable restore
#line 96 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganization.razor"
                             
                        }

#line default
#line hidden
#nullable disable
            __builder.AddContent(142, "                    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(143, "\r\n");
#nullable restore
#line 99 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganization.razor"
                }

#line default
#line hidden
#nullable disable
#nullable restore
#line 99 "E:\Project\HassyaAllrightCloud\Pages\StaffScheduleMobileOrganization.razor"
                 
            }

#line default
#line hidden
#nullable disable
            __builder.AddContent(144, "        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(145, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(146, "\r\n");
            __builder.CloseElement();
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
