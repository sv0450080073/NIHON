#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\Components\CommonComponents\ReservationClassComponent.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "86d23b8ff4a00615a2c511ed3d48042e9f449981"
// <auto-generated/>
#pragma warning disable 1591
namespace HassyaAllrightCloud.Pages.Components.CommonComponents
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
    public partial class ReservationClassComponent : ReservationClassComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "div");
            __builder.AddAttribute(1, "class", "row");
            __builder.AddMarkupContent(2, "\r\n    ");
            __builder.OpenElement(3, "div");
            __builder.AddAttribute(4, "class", "col-12 has-tooltip-error");
            __builder.AddMarkupContent(5, "\r\n        ");
            __builder.OpenComponent<HassyaAllrightCloud.Pages.Components.Tooltip>(6);
            __builder.AddAttribute(7, "ValueExpressions", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Linq.Expressions.Expression<System.Func<System.Object>>>(
#nullable restore
#line 5 "E:\Project\HassyaAllrightCloud\Pages\Components\CommonComponents\ReservationClassComponent.razor"
                                   ReservationClassExpression

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(8, "Lang", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Collections.Generic.Dictionary<System.String, System.String>>(
#nullable restore
#line 6 "E:\Project\HassyaAllrightCloud\Pages\Components\CommonComponents\ReservationClassComponent.razor"
                           LangDic

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(9, "Text", "");
            __builder.AddAttribute(10, "Position", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<HassyaAllrightCloud.Commons.Constants.PositionTooltip>(
#nullable restore
#line 6 "E:\Project\HassyaAllrightCloud\Pages\Components\CommonComponents\ReservationClassComponent.razor"
                                                      PositionTooltip.top

#line default
#line hidden
#nullable disable
            ));
            __builder.CloseComponent();
            __builder.AddMarkupContent(11, "\r\n        ");
            __Blazor.HassyaAllrightCloud.Pages.Components.CommonComponents.ReservationClassComponent.TypeInference.CreateDxComboBox_0(__builder, 12, 13, 
#nullable restore
#line 7 "E:\Project\HassyaAllrightCloud\Pages\Components\CommonComponents\ReservationClassComponent.razor"
                          ListReservationClass

#line default
#line hidden
#nullable disable
            , 14, 
#nullable restore
#line 8 "E:\Project\HassyaAllrightCloud\Pages\Components\CommonComponents\ReservationClassComponent.razor"
                                  SelectedReservationClass

#line default
#line hidden
#nullable disable
            , 15, 
#nullable restore
#line 9 "E:\Project\HassyaAllrightCloud\Pages\Components\CommonComponents\ReservationClassComponent.razor"
                                         OnReservationClassChanged

#line default
#line hidden
#nullable disable
            , 16, 
#nullable restore
#line 10 "E:\Project\HassyaAllrightCloud\Pages\Components\CommonComponents\ReservationClassComponent.razor"
                                            ReservationClassExpression

#line default
#line hidden
#nullable disable
            , 17, 
#nullable restore
#line 11 "E:\Project\HassyaAllrightCloud\Pages\Components\CommonComponents\ReservationClassComponent.razor"
                                            ClearButtonDisplayMode

#line default
#line hidden
#nullable disable
            , 18, 
#nullable restore
#line 12 "E:\Project\HassyaAllrightCloud\Pages\Components\CommonComponents\ReservationClassComponent.razor"
                                    false

#line default
#line hidden
#nullable disable
            , 19, 
#nullable restore
#line 13 "E:\Project\HassyaAllrightCloud\Pages\Components\CommonComponents\ReservationClassComponent.razor"
                                    DataGridFilteringMode.Contains

#line default
#line hidden
#nullable disable
            , 20, 
#nullable restore
#line 14 "E:\Project\HassyaAllrightCloud\Pages\Components\CommonComponents\ReservationClassComponent.razor"
                               Lang["reservation_nulltext"]

#line default
#line hidden
#nullable disable
            , 21, "Text", 22, 
#nullable restore
#line 16 "E:\Project\HassyaAllrightCloud\Pages\Components\CommonComponents\ReservationClassComponent.razor"
                              ReadOnly

#line default
#line hidden
#nullable disable
            , 23, 
#nullable restore
#line 17 "E:\Project\HassyaAllrightCloud\Pages\Components\CommonComponents\ReservationClassComponent.razor"
                                       DropDownDirection

#line default
#line hidden
#nullable disable
            , 24, 
#nullable restore
#line 18 "E:\Project\HassyaAllrightCloud\Pages\Components\CommonComponents\ReservationClassComponent.razor"
                                    RenderMode

#line default
#line hidden
#nullable disable
            , 25, 
#nullable restore
#line 19 "E:\Project\HassyaAllrightCloud\Pages\Components\CommonComponents\ReservationClassComponent.razor"
                               CssClass

#line default
#line hidden
#nullable disable
            , 26, Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.ChangeEventArgs>(this, 
#nullable restore
#line 20 "E:\Project\HassyaAllrightCloud\Pages\Components\CommonComponents\ReservationClassComponent.razor"
                              e => searchString = e.Value.ToString()

#line default
#line hidden
#nullable disable
            ), 27, Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.KeyboardEventArgs>(this, 
#nullable restore
#line 21 "E:\Project\HassyaAllrightCloud\Pages\Components\CommonComponents\ReservationClassComponent.razor"
                               Enter

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(28, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(29, "\r\n");
            __builder.CloseElement();
        }
        #pragma warning restore 1998
    }
}
namespace __Blazor.HassyaAllrightCloud.Pages.Components.CommonComponents.ReservationClassComponent
{
    #line hidden
    internal static class TypeInference
    {
        public static void CreateDxComboBox_0<T>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, global::System.Collections.Generic.IEnumerable<T> __arg0, int __seq1, T __arg1, int __seq2, global::System.Action<T> __arg2, int __seq3, global::System.Linq.Expressions.Expression<global::System.Func<T>> __arg3, int __seq4, global::DevExpress.Blazor.DataEditorClearButtonDisplayMode __arg4, int __seq5, global::System.Boolean __arg5, int __seq6, global::DevExpress.Blazor.DataGridFilteringMode __arg6, int __seq7, global::System.String __arg7, int __seq8, global::System.String __arg8, int __seq9, global::System.Boolean __arg9, int __seq10, global::DevExpress.Blazor.DropDownDirection __arg10, int __seq11, global::DevExpress.Blazor.ListRenderMode __arg11, int __seq12, global::System.String __arg12, int __seq13, global::System.Object __arg13, int __seq14, global::System.Object __arg14)
        {
        __builder.OpenComponent<global::DevExpress.Blazor.DxComboBox<T>>(seq);
        __builder.AddAttribute(__seq0, "Data", __arg0);
        __builder.AddAttribute(__seq1, "SelectedItem", __arg1);
        __builder.AddAttribute(__seq2, "SelectedItemChanged", __arg2);
        __builder.AddAttribute(__seq3, "SelectedItemExpression", __arg3);
        __builder.AddAttribute(__seq4, "ClearButtonDisplayMode", __arg4);
        __builder.AddAttribute(__seq5, "AllowUserInput", __arg5);
        __builder.AddAttribute(__seq6, "FilteringMode", __arg6);
        __builder.AddAttribute(__seq7, "NullText", __arg7);
        __builder.AddAttribute(__seq8, "TextFieldName", __arg8);
        __builder.AddAttribute(__seq9, "ReadOnly", __arg9);
        __builder.AddAttribute(__seq10, "DropDownDirection", __arg10);
        __builder.AddAttribute(__seq11, "ListRenderMode", __arg11);
        __builder.AddAttribute(__seq12, "CssClass", __arg12);
        __builder.AddAttribute(__seq13, "oninput", __arg13);
        __builder.AddAttribute(__seq14, "onkeyup", __arg14);
        __builder.CloseComponent();
        }
    }
}
#pragma warning restore 1591
