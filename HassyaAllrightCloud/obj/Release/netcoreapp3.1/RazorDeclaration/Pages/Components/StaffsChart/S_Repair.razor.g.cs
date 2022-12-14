#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\Components\StaffsChart\S_Repair.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d0ee86a6bc827a00ce6c1aff527dcf8f70ab7215"
// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace HassyaAllrightCloud.Pages.Components.StaffsChart
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
using BlazorContextMenu;

#line default
#line hidden
#nullable disable
#nullable restore
#line 18 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Application.Validation;

#line default
#line hidden
#nullable disable
#nullable restore
#line 19 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using DevExpress.Blazor;

#line default
#line hidden
#nullable disable
#nullable restore
#line 20 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using System.Linq;

#line default
#line hidden
#nullable disable
#nullable restore
#line 21 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Newtonsoft.Json;

#line default
#line hidden
#nullable disable
#nullable restore
#line 22 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Newtonsoft.Json.Linq;

#line default
#line hidden
#nullable disable
#nullable restore
#line 23 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Microsoft.Extensions.Localization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 24 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.IService;

#line default
#line hidden
#nullable disable
#nullable restore
#line 25 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using System.IO;

#line default
#line hidden
#nullable disable
#nullable restore
#line 26 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using LexLibrary.Line.NotifyBot.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 27 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using LexLibrary.Line.NotifyBot;

#line default
#line hidden
#nullable disable
#nullable restore
#line 28 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using DevExpress.Blazor.Reporting;

#line default
#line hidden
#nullable disable
    public partial class S_Repair : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 10 "E:\Project\HassyaAllrightCloud\Pages\Components\StaffsChart\S_Repair.razor"
       
    [Parameter] public EventCallback<VpmKinKyu> OnCallBackRepair { get; set; }
    IEnumerable<VpmKinKyu> KinKyulst { get; set; }
    IEnumerable<VpmKinKyu> KinKyuItem { get; set; }

    protected override async Task OnInitializedAsync()
    {
        KinKyulst = new List<VpmKinKyu>() {
            new VpmKinKyu { KinKyuCdSeq = 1, KinKyuCd = 1, KinKyuNm = "??????", KinKyuKbn = 2, ColKinKyu = "Red" },
            new VpmKinKyu { KinKyuCdSeq = 2, KinKyuCd = 2, KinKyuNm = "??????", KinKyuKbn = 2, ColKinKyu = "Red" },
            new VpmKinKyu { KinKyuCdSeq = 3, KinKyuCd = 3, KinKyuNm = "??????", KinKyuKbn = 2, ColKinKyu = "Red" },
            new VpmKinKyu { KinKyuCdSeq = 4, KinKyuCd = 4, KinKyuNm = "??????", KinKyuKbn = 2, ColKinKyu = "Red" },
            new VpmKinKyu { KinKyuCdSeq = 5, KinKyuCd = 5, KinKyuNm = "??????", KinKyuKbn = 2, ColKinKyu = "Red" },
            new VpmKinKyu { KinKyuCdSeq = 6, KinKyuCd = 6, KinKyuNm = "?????????", KinKyuKbn = 2, ColKinKyu = "Red" },
            new VpmKinKyu { KinKyuCdSeq = 7, KinKyuCd = 7, KinKyuNm = "??????", KinKyuKbn = 2, ColKinKyu = "Red" },
            new VpmKinKyu { KinKyuCdSeq = 8, KinKyuCd = 8, KinKyuNm = "??????", KinKyuKbn = 2, ColKinKyu = "Red" },
            new VpmKinKyu { KinKyuCdSeq = 9, KinKyuCd = 9, KinKyuNm = "??????", KinKyuKbn = 2, ColKinKyu = "Red" },
            new VpmKinKyu { KinKyuCdSeq = 10, KinKyuCd = 10, KinKyuNm = "??????", KinKyuKbn = 2, ColKinKyu = "Red" },
            new VpmKinKyu { KinKyuCdSeq = 11, KinKyuCd = 11, KinKyuNm = "", KinKyuKbn = 1, ColKinKyu = "" },
            new VpmKinKyu { KinKyuCdSeq = 12, KinKyuCd = 12, KinKyuNm = "??????", KinKyuKbn = 1, ColKinKyu = "" },
            new VpmKinKyu { KinKyuCdSeq = 13, KinKyuCd = 13, KinKyuNm = "????????????", KinKyuKbn = 1, ColKinKyu = "White" },
            new VpmKinKyu { KinKyuCdSeq = 14, KinKyuCd = 14, KinKyuNm = "????????????", KinKyuKbn = 1, ColKinKyu = "White" },
            new VpmKinKyu { KinKyuCdSeq = 15, KinKyuCd = 15, KinKyuNm = "??????", KinKyuKbn = 1, ColKinKyu = "" },
            new VpmKinKyu { KinKyuCdSeq = 16, KinKyuCd = 16, KinKyuNm = "????????????", KinKyuKbn = 1, ColKinKyu = "" },
            new VpmKinKyu { KinKyuCdSeq = 17, KinKyuCd = 17, KinKyuNm = "??????", KinKyuKbn = 1, ColKinKyu = "" },
            new VpmKinKyu { KinKyuCdSeq = 18, KinKyuCd = 18, KinKyuNm = "???????????????", KinKyuKbn = 1, ColKinKyu = "" },
            new VpmKinKyu { KinKyuCdSeq = 19, KinKyuCd = 19, KinKyuNm = "????????????", KinKyuKbn = 1, ColKinKyu = "" },
            new VpmKinKyu { KinKyuCdSeq = 20, KinKyuCd = 20, KinKyuNm = "????????????", KinKyuKbn = 1, ColKinKyu = "" },
            new VpmKinKyu { KinKyuCdSeq = 21, KinKyuCd = 21, KinKyuNm = "??????", KinKyuKbn = 1, ColKinKyu = "" },
            new VpmKinKyu { KinKyuCdSeq = 22, KinKyuCd = 50, KinKyuNm = "??????", KinKyuKbn = 1, ColKinKyu = "" },
            new VpmKinKyu { KinKyuCdSeq = 23, KinKyuCd = 51, KinKyuNm = "??????", KinKyuKbn = 1, ColKinKyu = "" },
            new VpmKinKyu { KinKyuCdSeq = 24, KinKyuCd = 52, KinKyuNm = "??????", KinKyuKbn = 1, ColKinKyu = "" },
            new VpmKinKyu { KinKyuCdSeq = 25, KinKyuCd = 53, KinKyuNm = "??????", KinKyuKbn = 1, ColKinKyu = "" },
            new VpmKinKyu { KinKyuCdSeq = 26, KinKyuCd = 54, KinKyuNm = "??????", KinKyuKbn = 1, ColKinKyu = "" },
            new VpmKinKyu { KinKyuCdSeq = 27, KinKyuCd = 55, KinKyuNm = "??????", KinKyuKbn = 1, ColKinKyu = "" },
            new VpmKinKyu { KinKyuCdSeq = 28, KinKyuCd = 97, KinKyuNm = "???????????????", KinKyuKbn = 1, ColKinKyu = "" },
            new VpmKinKyu { KinKyuCdSeq = 29, KinKyuCd = 98, KinKyuNm = "???????????????", KinKyuKbn = 2, ColKinKyu = "Red" },
        };
        if (KinKyulst.Count() > 0)
        {
            KinKyuItem = Enumerable.Empty<VpmKinKyu>();
            KinKyuItem = KinKyulst.Take(1);
            await OnCallBackRepair.InvokeAsync(KinKyulst.ToList().First());
        }
    }

    public async Task OnSelectedKinKyuItemChanged(IEnumerable<VpmKinKyu> selectedItem)
    {
        KinKyuItem = selectedItem;
        await OnCallBackRepair.InvokeAsync(selectedItem.ToList().First());
    }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
