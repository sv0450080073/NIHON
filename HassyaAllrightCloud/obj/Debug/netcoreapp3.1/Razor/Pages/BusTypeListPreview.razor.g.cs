#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\BusTypeListPreview.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "4ab7109f363b332f589119b8991af37cd8ce1024"
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
#nullable restore
#line 3 "E:\Project\HassyaAllrightCloud\Pages\BusTypeListPreview.razor"
using HassyaAllrightCloud.Domain.Dto.CommonComponents;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "E:\Project\HassyaAllrightCloud\Pages\BusTypeListPreview.razor"
using HassyaAllrightCloud.IService.CommonComponents;

#line default
#line hidden
#nullable disable
    public partial class BusTypeListPreview : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
#nullable restore
#line 7 "E:\Project\HassyaAllrightCloud\Pages\BusTypeListPreview.razor"
 if (!string.IsNullOrEmpty(reportUrl) && !string.IsNullOrWhiteSpace(reportUrl))
{

#line default
#line hidden
#nullable disable
            __builder.AddContent(0, "    ");
            __builder.OpenComponent<DevExpress.Blazor.Reporting.DxDocumentViewer>(1);
            __builder.AddAttribute(2, "ReportUrl", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.String>(
#nullable restore
#line 9 "E:\Project\HassyaAllrightCloud\Pages\BusTypeListPreview.razor"
                                  reportUrl

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(3, "Height", "800px");
            __builder.AddAttribute(4, "Width", "100%");
            __builder.AddAttribute(5, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
                __builder2.AddMarkupContent(6, "\r\n        ");
                __builder2.OpenComponent<DevExpress.Blazor.Reporting.DxDocumentViewerTabPanelSettings>(7);
                __builder2.AddAttribute(8, "Width", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Int32>(
#nullable restore
#line 10 "E:\Project\HassyaAllrightCloud\Pages\BusTypeListPreview.razor"
                                                 180

#line default
#line hidden
#nullable disable
                ));
                __builder2.CloseComponent();
                __builder2.AddMarkupContent(9, "\r\n    ");
            }
            ));
            __builder.CloseComponent();
            __builder.AddMarkupContent(10, "\r\n");
#nullable restore
#line 12 "E:\Project\HassyaAllrightCloud\Pages\BusTypeListPreview.razor"
}

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
#nullable restore
#line 13 "E:\Project\HassyaAllrightCloud\Pages\BusTypeListPreview.razor"
       
    [Parameter] public string SearchString { get; set; }
    private string reportUrl { get; set; }
    List<ReservationClassComponentData> BookingTypes = new List<ReservationClassComponentData>();
    BusTypeListDataUri BusTypeListDataUri { get; set; } = new BusTypeListDataUri();
    protected override async Task OnInitializedAsync()
    {
        try
        {
            if (!string.IsNullOrEmpty(SearchString) && !String.IsNullOrWhiteSpace(SearchString))
            {
                await LoadBookingType();
                BusTypeListDataUri = EncryptHelper.Decrypt<BusTypeListDataUri>(SearchString);
                SearchString = EncryptHelper.EncryptToUrl(SetValueUri(BusTypeListDataUri));
                reportUrl = $"{nameof(IBusTypeListReportService)}?{SearchString}";
                NavManager.NavigateTo("/bustypelistPreview", false);
            }
        }
        catch(Exception ex)
        {

        }
    }
    private async Task LoadBookingType()
    {
        BookingTypes = await _service.GetListReservationClass();
        await InvokeAsync(StateHasChanged);
    }
    private BusTypeListData SetValueUri(BusTypeListDataUri data)
    {
        var result = new BusTypeListData();
        try
        {
            if (data != null)
            {
                result.StartDate = data.StartDate;
                result.BookingTypeFrom = data.BookingTypeFrom;
                result.BookingTypeTo = data.BookingTypeTo;
                result.Company = data.Company;
                result.BranchStart = data.BranchStart;
                result.BranchEnd = data.BranchEnd;
                result.SalesStaffStart = data.SalesStaffStart;
                result.SalesStaffEnd = data.SalesStaffEnd;
                result.PersonInputStart = data.PersonInputStart;
                result.PersonInputEnd = data.PersonInputEnd;
                result.DestinationStart = data.DestinationStart;
                result.DestinationEnd = data.DestinationEnd;
                result.BusType = data.BusType;
                result.VehicleFrom = data.VehicleFrom;
                result.VehicleTo = data.VehicleTo;
                result.OutputType = data.OutputType;
                result.DepositOutputTemplate = data.DepositOutputTemplate;
                result.PaperSize = data.PaperSize;
                result.GridSize = data.GridSize;
                result.GroupMode = data.GroupMode;
                result.TenantCdSeq = data.TenantCdSeq;
                result.SyainNm = data.SyainNm;
                result.SyainCd = data.SyainCd;
                result.TenantCdSeqByCodeSyu = data.TenantCdSeqByCodeSyu;
                result.ReservationList = CutSpecialCharactersReservationList(data.ReservationListID);
                result.numberDay = data.numberDayUri;
            }
        }
        catch (Exception ex)
        {
            //ErrorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
        return result;
    }
    private List<ReservationClassComponentData> CutSpecialCharactersReservationList(string strValue)
    {
        try
        {
            string[] strValueArr = strValue.Split('-');
            var result = BookingTypes.Where(x => strValueArr.Contains(x.YoyaKbnSeq.ToString())).ToList();
            return result;
        }
        catch(Exception ex)
        {
            return null;
        }
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IReservationClassComponentService _service { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private CustomNavigation NavManager { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ITPM_YoyKbnDataListService TPM_YoyKbnDataService { get; set; }
    }
}
#pragma warning restore 1591
