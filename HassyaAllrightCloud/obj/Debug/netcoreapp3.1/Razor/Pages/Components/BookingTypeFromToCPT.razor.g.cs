#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\Components\BookingTypeFromToCPT.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "45d891d75da6bd597f316d67dcb65b671ea813d7"
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
    public partial class BookingTypeFromToCPT : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "div");
            __builder.AddAttribute(1, "class", 
#nullable restore
#line 3 "E:\Project\HassyaAllrightCloud\Pages\Components\BookingTypeFromToCPT.razor"
              "has-tooltip-error"

#line default
#line hidden
#nullable disable
            );
            __builder.AddMarkupContent(2, "\r\n    ");
            __builder.OpenComponent<HassyaAllrightCloud.Pages.Components.Tooltip>(3);
            __builder.AddAttribute(4, "ValueExpressions", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Linq.Expressions.Expression<System.Func<System.Object>>>(
#nullable restore
#line 4 "E:\Project\HassyaAllrightCloud\Pages\Components\BookingTypeFromToCPT.razor"
                                 () => BookingFrom

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(5, "Lang", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Collections.Generic.Dictionary<System.String, System.String>>(
#nullable restore
#line 4 "E:\Project\HassyaAllrightCloud\Pages\Components\BookingTypeFromToCPT.razor"
                                                            LangDic

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(6, "Text", "");
            __builder.AddAttribute(7, "Position", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<HassyaAllrightCloud.Commons.Constants.PositionTooltip>(
#nullable restore
#line 4 "E:\Project\HassyaAllrightCloud\Pages\Components\BookingTypeFromToCPT.razor"
                                                                                       PositionTooltip.top

#line default
#line hidden
#nullable disable
            ));
            __builder.CloseComponent();
            __builder.AddMarkupContent(8, "\r\n    ");
            __Blazor.HassyaAllrightCloud.Pages.Components.BookingTypeFromToCPT.TypeInference.CreateDxComboBox_0(__builder, 9, 10, 
#nullable restore
#line 5 "E:\Project\HassyaAllrightCloud\Pages\Components\BookingTypeFromToCPT.razor"
                       BookingTypes

#line default
#line hidden
#nullable disable
            , 11, 
#nullable restore
#line 6 "E:\Project\HassyaAllrightCloud\Pages\Components\BookingTypeFromToCPT.razor"
                           Lang["BookingTypeNullText"]

#line default
#line hidden
#nullable disable
            , 12, "YoyaCodeName", 13, 
#nullable restore
#line 8 "E:\Project\HassyaAllrightCloud\Pages\Components\BookingTypeFromToCPT.razor"
                                false

#line default
#line hidden
#nullable disable
            , 14, 
#nullable restore
#line 9 "E:\Project\HassyaAllrightCloud\Pages\Components\BookingTypeFromToCPT.razor"
                               BookingFrom

#line default
#line hidden
#nullable disable
            , 15, 
#nullable restore
#line 10 "E:\Project\HassyaAllrightCloud\Pages\Components\BookingTypeFromToCPT.razor"
                                       (e) => OnBookingFromSelectedChanged(e)

#line default
#line hidden
#nullable disable
            , 16, 
#nullable restore
#line 11 "E:\Project\HassyaAllrightCloud\Pages\Components\BookingTypeFromToCPT.razor"
                                          () => BookingFrom

#line default
#line hidden
#nullable disable
            , 17, "width--290 custom-combo-box");
            __builder.AddMarkupContent(18, "\r\n");
            __builder.CloseElement();
            __builder.AddMarkupContent(19, "\r\n");
            __builder.AddMarkupContent(20, "<span class=\"mx-2\">～</span>\r\n");
            __builder.OpenElement(21, "div");
            __builder.AddAttribute(22, "class", 
#nullable restore
#line 16 "E:\Project\HassyaAllrightCloud\Pages\Components\BookingTypeFromToCPT.razor"
              "has-tooltip-error"

#line default
#line hidden
#nullable disable
            );
            __builder.AddMarkupContent(23, "\r\n    ");
            __builder.OpenComponent<HassyaAllrightCloud.Pages.Components.Tooltip>(24);
            __builder.AddAttribute(25, "ValueExpressions", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Linq.Expressions.Expression<System.Func<System.Object>>>(
#nullable restore
#line 17 "E:\Project\HassyaAllrightCloud\Pages\Components\BookingTypeFromToCPT.razor"
                                 () => BookingTo

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(26, "Lang", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Collections.Generic.Dictionary<System.String, System.String>>(
#nullable restore
#line 17 "E:\Project\HassyaAllrightCloud\Pages\Components\BookingTypeFromToCPT.razor"
                                                          LangDic

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(27, "Text", "");
            __builder.AddAttribute(28, "Position", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<HassyaAllrightCloud.Commons.Constants.PositionTooltip>(
#nullable restore
#line 17 "E:\Project\HassyaAllrightCloud\Pages\Components\BookingTypeFromToCPT.razor"
                                                                                     PositionTooltip.top

#line default
#line hidden
#nullable disable
            ));
            __builder.CloseComponent();
            __builder.AddMarkupContent(29, "\r\n    ");
            __Blazor.HassyaAllrightCloud.Pages.Components.BookingTypeFromToCPT.TypeInference.CreateDxComboBox_1(__builder, 30, 31, 
#nullable restore
#line 18 "E:\Project\HassyaAllrightCloud\Pages\Components\BookingTypeFromToCPT.razor"
                       BookingTypes

#line default
#line hidden
#nullable disable
            , 32, 
#nullable restore
#line 19 "E:\Project\HassyaAllrightCloud\Pages\Components\BookingTypeFromToCPT.razor"
                           Lang["BookingTypeNullText"]

#line default
#line hidden
#nullable disable
            , 33, "YoyaCodeName", 34, 
#nullable restore
#line 21 "E:\Project\HassyaAllrightCloud\Pages\Components\BookingTypeFromToCPT.razor"
                                false

#line default
#line hidden
#nullable disable
            , 35, 
#nullable restore
#line 22 "E:\Project\HassyaAllrightCloud\Pages\Components\BookingTypeFromToCPT.razor"
                               BookingTo

#line default
#line hidden
#nullable disable
            , 36, 
#nullable restore
#line 23 "E:\Project\HassyaAllrightCloud\Pages\Components\BookingTypeFromToCPT.razor"
                                       (e) => OnBookingToSelectedChanged(e)

#line default
#line hidden
#nullable disable
            , 37, 
#nullable restore
#line 24 "E:\Project\HassyaAllrightCloud\Pages\Components\BookingTypeFromToCPT.razor"
                                          () => BookingTo

#line default
#line hidden
#nullable disable
            , 38, "width--290 custom-combo-box");
            __builder.AddMarkupContent(39, "\r\n");
            __builder.CloseElement();
        }
        #pragma warning restore 1998
#nullable restore
#line 29 "E:\Project\HassyaAllrightCloud\Pages\Components\BookingTypeFromToCPT.razor"
       
    [Parameter] public List<ReservationData> BookingTypes { get; set; }
    [Parameter] public ReservationData BookingFrom { get; set; }
    [Parameter] public ReservationData BookingTo { get; set; }
    [Parameter] public Dictionary<string, string> LangDic { get; set; }
    [Parameter] public EventCallback<ReservationData> OnBookingFromSelected { get; set; }
    [Parameter] public EventCallback<ReservationData> OnBookingToSelected { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    async Task OnBookingFromSelectedChanged(ReservationData item)
    {
        BookingFrom = item;
        await OnBookingFromSelected.InvokeAsync(item);
        StateHasChanged();
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    async Task OnBookingToSelectedChanged(ReservationData item)
    {
        BookingTo = item;
        await OnBookingToSelected.InvokeAsync(item);
        StateHasChanged();
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IStringLocalizer<BookingTypeFromToCPT> Lang { get; set; }
    }
}
namespace __Blazor.HassyaAllrightCloud.Pages.Components.BookingTypeFromToCPT
{
    #line hidden
    internal static class TypeInference
    {
        public static void CreateDxComboBox_0<T>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, global::System.Collections.Generic.IEnumerable<T> __arg0, int __seq1, global::System.String __arg1, int __seq2, global::System.String __arg2, int __seq3, global::System.Boolean __arg3, int __seq4, T __arg4, int __seq5, global::System.Action<T> __arg5, int __seq6, global::System.Linq.Expressions.Expression<global::System.Func<T>> __arg6, int __seq7, global::System.String __arg7)
        {
        __builder.OpenComponent<global::DevExpress.Blazor.DxComboBox<T>>(seq);
        __builder.AddAttribute(__seq0, "Data", __arg0);
        __builder.AddAttribute(__seq1, "NullText", __arg1);
        __builder.AddAttribute(__seq2, "TextFieldName", __arg2);
        __builder.AddAttribute(__seq3, "AllowUserInput", __arg3);
        __builder.AddAttribute(__seq4, "SelectedItem", __arg4);
        __builder.AddAttribute(__seq5, "SelectedItemChanged", __arg5);
        __builder.AddAttribute(__seq6, "SelectedItemExpression", __arg6);
        __builder.AddAttribute(__seq7, "CssClass", __arg7);
        __builder.CloseComponent();
        }
        public static void CreateDxComboBox_1<T>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, global::System.Collections.Generic.IEnumerable<T> __arg0, int __seq1, global::System.String __arg1, int __seq2, global::System.String __arg2, int __seq3, global::System.Boolean __arg3, int __seq4, T __arg4, int __seq5, global::System.Action<T> __arg5, int __seq6, global::System.Linq.Expressions.Expression<global::System.Func<T>> __arg6, int __seq7, global::System.String __arg7)
        {
        __builder.OpenComponent<global::DevExpress.Blazor.DxComboBox<T>>(seq);
        __builder.AddAttribute(__seq0, "Data", __arg0);
        __builder.AddAttribute(__seq1, "NullText", __arg1);
        __builder.AddAttribute(__seq2, "TextFieldName", __arg2);
        __builder.AddAttribute(__seq3, "AllowUserInput", __arg3);
        __builder.AddAttribute(__seq4, "SelectedItem", __arg4);
        __builder.AddAttribute(__seq5, "SelectedItemChanged", __arg5);
        __builder.AddAttribute(__seq6, "SelectedItemExpression", __arg6);
        __builder.AddAttribute(__seq7, "CssClass", __arg7);
        __builder.CloseComponent();
        }
    }
}
#pragma warning restore 1591
