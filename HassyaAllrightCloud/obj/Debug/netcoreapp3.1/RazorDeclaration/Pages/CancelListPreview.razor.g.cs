#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\CancelListPreview.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b06a668434a565522afc2c7504b45d27d63dee53"
// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

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
#nullable restore
#line 1 "E:\Project\HassyaAllrightCloud\Pages\CancelListPreview.razor"
using HassyaAllrightCloud.IService.CommonComponents;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "E:\Project\HassyaAllrightCloud\Pages\CancelListPreview.razor"
using HassyaAllrightCloud.Domain.Dto.CommonComponents;

#line default
#line hidden
#nullable disable
    public partial class CancelListPreview : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 11 "E:\Project\HassyaAllrightCloud\Pages\CancelListPreview.razor"
       
    [Parameter] public string SearchString { get; set; }
    List<ReservationClassComponentData> reservationlst = new List<ReservationClassComponentData>();
    public CancelListDataUri CancelListDataUri { get; set; } = new CancelListDataUri();
    public CancelListReportSearchParamsUri CancelListReportSearchParamsUri { get; set; } = new CancelListReportSearchParamsUri();
    private string PreportUrl { get; set; } = "";
    protected override async Task OnInitializedAsync()
    {
        reservationlst = await _yoyakuservice.GetListReservationClass();
        if (reservationlst == null)
        {
            reservationlst = new List<ReservationClassComponentData>();
        }
        else
        {
            reservationlst.Insert(0, new ReservationClassComponentData());
        }
        CancelListReportSearchParamsUri = EncryptHelper.Decrypt<CancelListReportSearchParamsUri>(SearchString);
        PreportUrl = EncryptHelper.EncryptToUrl(SetValueUri(CancelListReportSearchParamsUri));
        // NavManager.NavigateTo("/CancelListPreview", false);
    }
    private CancelListReportSearchParams SetValueUri(CancelListReportSearchParamsUri data)
    {
        var result = new CancelListReportSearchParams();
        result.TenantId = data.TenantId;
        result.UserLoginId = data.UserLoginId;
        result.BookingKeys = data.BookingKeys;
        result.SearchCondition = SetValueCancelListData(data.SearchCondition);
        return result;
    }
    private List<ReservationClassComponentData> CutSpecialCharactersReservationList(string strValue)
    {
        var result = new List<ReservationClassComponentData>();
        if (!string.IsNullOrEmpty(strValue))
        {
            string[] strValueArr = strValue.Split('-');
            result = reservationlst.Where(x => strValueArr.Contains(x.YoyaKbnSeq.ToString())).ToList();
        }
        return result;
    }
    private CancelListData SetValueCancelListData(CancelListDataUri data)
    {
        var result = new CancelListData();
        result._ukeCdFrom = data._ukeCdFrom;
        result._ukeCdTo = data._ukeCdTo;
        result.DateType = data.DateType;
        result.DateTypeText = data.DateTypeText;
        result.Sort = data.Sort;
        result.SortText = data.SortText;
        result.YoyakuFrom = data.YoyakuFrom;
        result.YoyakuTo = data.YoyakuTo;
        result.StartDate = data.StartDate;
        result.EndDate = data.EndDate;
        result.Company = data.Company;
        result.CancelBookingType = data.CancelBookingType;
        result.CancelCharge = data.CancelCharge;
        result.BreakPage = data.BreakPage;
        result.BranchStart = data.BranchStart;
        result.BranchEnd = data.BranchEnd;
        result.GyosyaTokuiSakiFrom = data.GyosyaTokuiSakiFrom;
        result.GyosyaTokuiSakiTo = data.GyosyaTokuiSakiTo;
        result.GyosyaShiireSakiFrom = data.GyosyaShiireSakiFrom;
        result.GyosyaShiireSakiTo = data.GyosyaShiireSakiTo;
        result.TokiskTokuiSakiFrom = data.TokiskTokuiSakiFrom;
        result.TokiskTokuiSakiTo = data.TokiskTokuiSakiTo;
        result.TokiskShiireSakiFrom = data.TokiskShiireSakiFrom;
        result.TokiskShiireSakiTo = data.TokiskShiireSakiTo;
        result.TokiStShiireSakiFrom = data.TokiStShiireSakiFrom;
        result.TokiStShiireSakiTo = data.TokiStShiireSakiTo;
        result.TokiStTokuiSakiFrom = data.TokiStTokuiSakiFrom;
        result.TokiStTokuiSakiTo = data.TokiStTokuiSakiTo;
        result.StaffStart = data.StaffStart;
        result.StaffEnd = data.StaffEnd;
        result.CancelStaffStart = data.CancelStaffStart;
        result.CancelStaffEnd = data.CancelStaffEnd;
        result.ExportType = data.ExportType;
        result.PaperSize = data.PaperSize;
        result.CsvConfigOption = data.CsvConfigOption;
        return result;
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IReservationClassComponentService _yoyakuservice { get; set; }
    }
}
#pragma warning restore 1591