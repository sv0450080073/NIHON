#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\Components\CommonComponents\DateInputComponent.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "c11481e7bd7cb77434783b1c4e699f2a717b5184"
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
    public partial class DateInputComponent : DateInputComponentBase
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
#line 5 "E:\Project\HassyaAllrightCloud\Pages\Components\CommonComponents\DateInputComponent.razor"
                                   DateInputExpression

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(8, "Lang", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Collections.Generic.Dictionary<System.String, System.String>>(
#nullable restore
#line 6 "E:\Project\HassyaAllrightCloud\Pages\Components\CommonComponents\DateInputComponent.razor"
                        LangDic

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(9, "Text", "");
            __builder.AddAttribute(10, "Position", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<HassyaAllrightCloud.Commons.Constants.PositionTooltip>(
#nullable restore
#line 6 "E:\Project\HassyaAllrightCloud\Pages\Components\CommonComponents\DateInputComponent.razor"
                                                   PositionTooltip.top

#line default
#line hidden
#nullable disable
            ));
            __builder.CloseComponent();
            __builder.AddMarkupContent(11, "\r\n        ");
            __Blazor.HassyaAllrightCloud.Pages.Components.CommonComponents.DateInputComponent.TypeInference.CreateDxDateEdit_0(__builder, 12, 13, 
#nullable restore
#line 7 "E:\Project\HassyaAllrightCloud\Pages\Components\CommonComponents\DateInputComponent.razor"
                           SelectedDate

#line default
#line hidden
#nullable disable
            , 14, 
#nullable restore
#line 8 "E:\Project\HassyaAllrightCloud\Pages\Components\CommonComponents\DateInputComponent.razor"
                                    () => SelectedDate

#line default
#line hidden
#nullable disable
            , 15, 
#nullable restore
#line 9 "E:\Project\HassyaAllrightCloud\Pages\Components\CommonComponents\DateInputComponent.razor"
                             Format

#line default
#line hidden
#nullable disable
            , 16, 
#nullable restore
#line 10 "E:\Project\HassyaAllrightCloud\Pages\Components\CommonComponents\DateInputComponent.razor"
                                   async(newValue) => await OnDateChanged(newValue)

#line default
#line hidden
#nullable disable
            , 17, 
#nullable restore
#line 11 "E:\Project\HassyaAllrightCloud\Pages\Components\CommonComponents\DateInputComponent.razor"
                                            ClearButtonDisplayMode

#line default
#line hidden
#nullable disable
            , 18, 
#nullable restore
#line 12 "E:\Project\HassyaAllrightCloud\Pages\Components\CommonComponents\DateInputComponent.razor"
                               CssClass

#line default
#line hidden
#nullable disable
            , 19, 
#nullable restore
#line 13 "E:\Project\HassyaAllrightCloud\Pages\Components\CommonComponents\DateInputComponent.razor"
                              ReadOnly

#line default
#line hidden
#nullable disable
            , 20, 
#nullable restore
#line 14 "E:\Project\HassyaAllrightCloud\Pages\Components\CommonComponents\DateInputComponent.razor"
                                    DisplayFormat

#line default
#line hidden
#nullable disable
            , 21, 
#nullable restore
#line 15 "E:\Project\HassyaAllrightCloud\Pages\Components\CommonComponents\DateInputComponent.razor"
                                       DropDownDirection

#line default
#line hidden
#nullable disable
            , 22, Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.ChangeEventArgs>(this, 
#nullable restore
#line 16 "E:\Project\HassyaAllrightCloud\Pages\Components\CommonComponents\DateInputComponent.razor"
                              e => dateInputString = e.Value.ToString()

#line default
#line hidden
#nullable disable
            ), 23, Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.KeyboardEventArgs>(this, 
#nullable restore
#line 17 "E:\Project\HassyaAllrightCloud\Pages\Components\CommonComponents\DateInputComponent.razor"
                                async e => await Enter(e)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(24, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(25, "\r\n");
            __builder.CloseElement();
        }
        #pragma warning restore 1998
    }
}
namespace __Blazor.HassyaAllrightCloud.Pages.Components.CommonComponents.DateInputComponent
{
    #line hidden
    internal static class TypeInference
    {
        public static void CreateDxDateEdit_0<T>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, T __arg0, int __seq1, global::System.Linq.Expressions.Expression<global::System.Func<T>> __arg1, int __seq2, global::System.String __arg2, int __seq3, global::System.Action<T> __arg3, int __seq4, global::DevExpress.Blazor.DataEditorClearButtonDisplayMode __arg4, int __seq5, global::System.String __arg5, int __seq6, global::System.Boolean __arg6, int __seq7, global::System.String __arg7, int __seq8, global::DevExpress.Blazor.DropDownDirection __arg8, int __seq9, global::System.Object __arg9, int __seq10, global::System.Object __arg10)
        {
        __builder.OpenComponent<global::DevExpress.Blazor.DxDateEdit<T>>(seq);
        __builder.AddAttribute(__seq0, "Date", __arg0);
        __builder.AddAttribute(__seq1, "DateExpression", __arg1);
        __builder.AddAttribute(__seq2, "Format", __arg2);
        __builder.AddAttribute(__seq3, "DateChanged", __arg3);
        __builder.AddAttribute(__seq4, "ClearButtonDisplayMode", __arg4);
        __builder.AddAttribute(__seq5, "CssClass", __arg5);
        __builder.AddAttribute(__seq6, "ReadOnly", __arg6);
        __builder.AddAttribute(__seq7, "DisplayFormat", __arg7);
        __builder.AddAttribute(__seq8, "DropDownDirection", __arg8);
        __builder.AddAttribute(__seq9, "oninput", __arg9);
        __builder.AddAttribute(__seq10, "onkeydown", __arg10);
        __builder.CloseComponent();
        }
    }
}
#pragma warning restore 1591
