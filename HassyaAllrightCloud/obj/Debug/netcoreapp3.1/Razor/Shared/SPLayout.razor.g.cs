#pragma checksum "E:\Project\HassyaAllrightCloud\Shared\SPLayout.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "34251ec66b74a3ec6140e5184668c3792182a379"
// <auto-generated/>
#pragma warning disable 1591
namespace HassyaAllrightCloud.Shared
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
    public partial class SPLayout : LayoutComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
#nullable restore
#line 4 "E:\Project\HassyaAllrightCloud\Shared\SPLayout.razor"
 if (!isNotAllowNavigate)
{

#line default
#line hidden
#nullable disable
            __builder.AddContent(0, "    ");
            __builder.OpenComponent<HassyaAllrightCloud.Pages.Components.Loading>(1);
            __builder.CloseComponent();
            __builder.AddMarkupContent(2, "\r\n    ");
            __builder.OpenElement(3, "div");
            __builder.AddAttribute(4, "class", "sp-layout");
            __builder.AddMarkupContent(5, "\r\n        ");
            __builder.OpenElement(6, "div");
            __builder.AddAttribute(7, "class", "header d-flex justify-content-between mt-3");
            __builder.AddMarkupContent(8, "\r\n            ");
            __builder.OpenElement(9, "div");
            __builder.AddAttribute(10, "class", "left d-flex ml-3");
            __builder.AddMarkupContent(11, "\r\n                ");
            __builder.OpenElement(12, "div");
            __builder.AddAttribute(13, "class", "d-flex");
            __builder.AddMarkupContent(14, "\r\n                    ");
            __builder.OpenElement(15, "div");
            __builder.AddAttribute(16, "class", "logo-img mr-3");
            __builder.AddMarkupContent(17, "\r\n                        ");
            __builder.OpenElement(18, "img");
            __builder.AddAttribute(19, "class", "mr-3");
            __builder.AddAttribute(20, "src", "images/logo_vector.svg");
            __builder.AddAttribute(21, "alt", 
#nullable restore
#line 12 "E:\Project\HassyaAllrightCloud\Shared\SPLayout.razor"
                                                                             lang["logo-nulltext"]

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(22, "\r\n                    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(23, "\r\n                    ");
            __builder.OpenElement(24, "div");
            __builder.AddAttribute(25, "class", "center mt-auto mb-auto");
            __builder.AddMarkupContent(26, "\r\n                        ");
            __builder.OpenElement(27, "b");
            __builder.OpenElement(28, "p");
            __builder.AddAttribute(29, "class", "app-name");
            __builder.AddContent(30, 
#nullable restore
#line 15 "E:\Project\HassyaAllrightCloud\Shared\SPLayout.razor"
                                                lang["app-name"]

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(31, "\r\n                        ");
            __builder.OpenElement(32, "b");
            __builder.OpenElement(33, "p");
            __builder.AddAttribute(34, "class", "screen-name");
            __builder.AddContent(35, 
#nullable restore
#line 16 "E:\Project\HassyaAllrightCloud\Shared\SPLayout.razor"
                                                   ScreenName

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(36, "\r\n                    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(37, "\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(38, "\r\n            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(39, "\r\n\r\n            ");
            __builder.OpenElement(40, "div");
            __builder.AddAttribute(41, "class", "right d-flex mr-3");
            __builder.AddMarkupContent(42, "\r\n                ");
            __builder.OpenElement(43, "div");
            __builder.AddAttribute(44, "class", "text-center d-flex");
            __builder.AddMarkupContent(45, "\r\n                    ");
            __builder.OpenElement(46, "div");
            __builder.AddAttribute(47, "class", "m-auto");
            __builder.AddMarkupContent(48, "\r\n                        ");
            __builder.OpenElement(49, "p");
            __builder.AddAttribute(50, "class", "text-primary user-name");
            __builder.AddContent(51, 
#nullable restore
#line 24 "E:\Project\HassyaAllrightCloud\Shared\SPLayout.razor"
                                                            new HassyaAllrightCloud.Domain.Dto.ClaimModel().Name

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(52, "\r\n                        ");
            __builder.OpenElement(53, "button");
            __builder.AddAttribute(54, "class", "btn btn-sm btn-outline-dark custom-btn-radius");
            __builder.AddContent(55, 
#nullable restore
#line 25 "E:\Project\HassyaAllrightCloud\Shared\SPLayout.razor"
                                                                                       lang["logout-btn"]

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(56, "\r\n                    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(57, "\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(58, "\r\n            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(59, "\r\n        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(60, "\r\n        ");
            __builder.OpenElement(61, "div");
            __builder.AddAttribute(62, "class", "mt-3");
            __builder.AddMarkupContent(63, "\r\n            ");
            __builder.AddContent(64, 
#nullable restore
#line 31 "E:\Project\HassyaAllrightCloud\Shared\SPLayout.razor"
             Body

#line default
#line hidden
#nullable disable
            );
            __builder.AddMarkupContent(65, "\r\n        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(66, "\r\n        ");
            __builder.OpenElement(67, "div");
            __builder.AddAttribute(68, "class", "footer mt-3 position-fixed");
            __builder.AddMarkupContent(69, "\r\n            ");
            __builder.OpenElement(70, "div");
            __builder.AddAttribute(71, "class", "d-flex justify-content-around w-100");
            __builder.AddMarkupContent(72, "\r\n                ");
            __builder.OpenElement(73, "div");
            __builder.AddAttribute(74, "class", "custom-py" + " " + (
#nullable restore
#line 35 "E:\Project\HassyaAllrightCloud\Shared\SPLayout.razor"
                                        CurrentUrl == string.Empty || CurrentUrl == "homemobile" ? "active" : string.Empty

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(75, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 35 "E:\Project\HassyaAllrightCloud\Shared\SPLayout.razor"
                                                                                                                                         e => Navigate("homemobile")

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(76, "\r\n                    <i class=\"fa fa-home\"></i>\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(77, "\r\n                ");
            __builder.OpenElement(78, "div");
            __builder.AddAttribute(79, "class", "custom-py" + " " + (
#nullable restore
#line 38 "E:\Project\HassyaAllrightCloud\Shared\SPLayout.razor"
                                        CurrentUrl == "staffschedulemobile" ? "active" : string.Empty

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(80, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 38 "E:\Project\HassyaAllrightCloud\Shared\SPLayout.razor"
                                                                                                                    e => Navigate("staffschedulemobile")

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(81, "\r\n                    <i class=\"fa fa-calendar\"></i>\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(82, "\r\n                ");
            __builder.OpenElement(83, "div");
            __builder.AddAttribute(84, "class", "custom-py" + " " + (
#nullable restore
#line 41 "E:\Project\HassyaAllrightCloud\Shared\SPLayout.razor"
                                        CurrentUrl == "driver" ? "active" : string.Empty

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(85, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 41 "E:\Project\HassyaAllrightCloud\Shared\SPLayout.razor"
                                                                                                       e => Navigate("driver")

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(86, "\r\n                    <i class=\"fa fa-drivers-license\"></i>\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(87, "\r\n                ");
            __builder.OpenElement(88, "div");
            __builder.AddAttribute(89, "class", "custom-py" + " " + (
#nullable restore
#line 44 "E:\Project\HassyaAllrightCloud\Shared\SPLayout.razor"
                                        CurrentUrl == "availabilitycheck" ? "active" : string.Empty

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(90, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 44 "E:\Project\HassyaAllrightCloud\Shared\SPLayout.razor"
                                                                                                                  e => Navigate("availabilitycheck")

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(91, "\r\n                    <i class=\"fa fa-bus\"></i>\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(92, "\r\n                ");
            __builder.OpenElement(93, "div");
            __builder.AddAttribute(94, "class", "custom-py" + " " + (
#nullable restore
#line 47 "E:\Project\HassyaAllrightCloud\Shared\SPLayout.razor"
                                        CurrentUrl == "chart" ? "active" : string.Empty

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(95, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 47 "E:\Project\HassyaAllrightCloud\Shared\SPLayout.razor"
                                                                                                      e => Navigate("chart")

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(96, "\r\n                    <i class=\"fa fa-bar-chart\"></i>\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(97, "\r\n            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(98, "\r\n        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(99, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(100, "\r\n");
#nullable restore
#line 53 "E:\Project\HassyaAllrightCloud\Shared\SPLayout.razor"
}

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
#nullable restore
#line 56 "E:\Project\HassyaAllrightCloud\Shared\SPLayout.razor"
       
    [Inject]
    protected IJSRuntime JSRuntime { get; set; }
    private string CurrentUrl { get; set; }
    private string ScreenName { get; set; }
    private bool isNotAllowNavigate { get; set; } = true;

    protected override void OnInitialized()
    {
        JSRuntime.InvokeVoidAsync("loadPageScript", "spLayoutPage", "checkFromMB", DotNetObjectReference.Create(this));
    }
    protected override void OnAfterRender(bool firstRender)
    {
        var checkUrl = NavigationManager.Uri.Replace(NavigationManager.BaseUri, string.Empty).Split("?")[0].ToLower();
        if (CurrentUrl != checkUrl)
        {
            CurrentUrl = checkUrl;
            ScreenName = lang[$"screen-name.{CurrentUrl}"];
            StateHasChanged();
        }
    }
    private void Navigate(string url)
    {
        if (!isNotAllowNavigate)
        {
            NavigationManager.NavigateTo(url);
            CurrentUrl = url;
            ScreenName = lang[$"screen-name.{CurrentUrl}"];
        }
        else
        {
            NavigationManager.NavigateTo("/notfound");
        }
    }
    [JSInvokable]
    public async void checkBrower(bool isNotAllow)
    {
        isNotAllowNavigate = isNotAllow;
        if (isNotAllowNavigate)
        {
            NavigationManager.NavigateTo("/notfound");
        }
        StateHasChanged();
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IStringLocalizer<SPLayout> lang { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private CustomNavigation NavigationManager { get; set; }
    }
}
#pragma warning restore 1591
