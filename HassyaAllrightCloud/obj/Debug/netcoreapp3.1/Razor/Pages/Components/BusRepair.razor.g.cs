#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\Components\BusRepair.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "fb9ecbb22d57aff34eb9e0850bfa438eb9304105"
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
    public partial class BusRepair : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "div");
            __builder.AddAttribute(1, "class", "busrepair-bar");
            __builder.AddMarkupContent(2, "\r\n    ");
            __builder.AddMarkupContent(3, "<h5 style=\"width: 100%;text-align: center;margin-top: 9px;\">修理項目</h5>\r\n");
#nullable restore
#line 4 "E:\Project\HassyaAllrightCloud\Pages\Components\BusRepair.razor"
     if (CodeKbnlst.Count() != 0)
    {

#line default
#line hidden
#nullable disable
            __builder.AddContent(4, "    ");
            __Blazor.HassyaAllrightCloud.Pages.Components.BusRepair.TypeInference.CreateDxListBox_0(__builder, 5, 6, 
#nullable restore
#line 6 "E:\Project\HassyaAllrightCloud\Pages\Components\BusRepair.razor"
                      CodeKbnlst

#line default
#line hidden
#nullable disable
            , 7, 
#nullable restore
#line 7 "E:\Project\HassyaAllrightCloud\Pages\Components\BusRepair.razor"
                               ListRenderMode.Entire

#line default
#line hidden
#nullable disable
            , 8, "Text", 9, 
#nullable restore
#line 9 "E:\Project\HassyaAllrightCloud\Pages\Components\BusRepair.razor"
                              codeKbDataselect

#line default
#line hidden
#nullable disable
            , 10, 
#nullable restore
#line 10 "E:\Project\HassyaAllrightCloud\Pages\Components\BusRepair.razor"
                                       e => OnSelectedChanged(e)

#line default
#line hidden
#nullable disable
            , 11, "demo-listbox", 12, "max-height: 400px;");
            __builder.AddMarkupContent(13, "\r\n");
#nullable restore
#line 14 "E:\Project\HassyaAllrightCloud\Pages\Components\BusRepair.razor"
    }

#line default
#line hidden
#nullable disable
            __builder.CloseElement();
        }
        #pragma warning restore 1998
#nullable restore
#line 17 "E:\Project\HassyaAllrightCloud\Pages\Components\BusRepair.razor"
       
    IEnumerable<VPM_RepairData> CodeKbnlst { get; set; }
    IEnumerable<VPM_RepairData> codeKbDataselect { get; set; }
    [Parameter] public EventCallback<VPM_RepairData> OnCallBackIdCode { get; set; }

    protected override async Task OnInitializedAsync()
    {
        CodeKbnlst = new List<VPM_RepairData>();
        CodeKbnlst = await TPM_CodeKbnService.GetDataBusRepairType(new ClaimModel().TenantID);
        if (CodeKbnlst.Count() > 0)
        {

            codeKbDataselect = Enumerable.Empty<VPM_RepairData>();
            codeKbDataselect = CodeKbnlst.Take(1);
            await OnCallBackIdCode.InvokeAsync(CodeKbnlst.ToList().First());
        }
    }

    public async Task OnSelectedChanged(IEnumerable<VPM_RepairData> e)
    {
        codeKbDataselect = Enumerable.Empty<VPM_RepairData>();
        codeKbDataselect = e.Take(1);
        await OnCallBackIdCode.InvokeAsync(e.ToList().First());
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ITPM_CodeKbListService TPM_CodeKbnService { get; set; }
    }
}
namespace __Blazor.HassyaAllrightCloud.Pages.Components.BusRepair
{
    #line hidden
    internal static class TypeInference
    {
        public static void CreateDxListBox_0<T>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, global::System.Collections.Generic.IEnumerable<T> __arg0, int __seq1, global::DevExpress.Blazor.ListRenderMode __arg1, int __seq2, global::System.String __arg2, int __seq3, global::System.Collections.Generic.IEnumerable<T> __arg3, int __seq4, global::System.Action<global::System.Collections.Generic.IEnumerable<T>> __arg4, int __seq5, global::System.String __arg5, int __seq6, System.Object __arg6)
        {
        __builder.OpenComponent<global::DevExpress.Blazor.DxListBox<T>>(seq);
        __builder.AddAttribute(__seq0, "Data", __arg0);
        __builder.AddAttribute(__seq1, "ListRenderMode", __arg1);
        __builder.AddAttribute(__seq2, "TextFieldName", __arg2);
        __builder.AddAttribute(__seq3, "SelectedItems", __arg3);
        __builder.AddAttribute(__seq4, "SelectedItemsChanged", __arg4);
        __builder.AddAttribute(__seq5, "CssClass", __arg5);
        __builder.AddAttribute(__seq6, "style", __arg6);
        __builder.CloseComponent();
        }
    }
}
#pragma warning restore 1591