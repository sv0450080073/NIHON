#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\VenderRequestForm.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e0859b337e58cb4dba7be76932681b2f5f9bc81d"
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
    [Microsoft.AspNetCore.Components.RouteAttribute("/venderrequestform")]
    public partial class VenderRequestForm : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 115 "E:\Project\HassyaAllrightCloud\Pages\VenderRequestForm.razor"
       
    public VenderRequestFormData data = new VenderRequestFormData();
    EditContext formContext;
    List<ReservationData> reservationlst = new List<ReservationData>();
    List<LoadCustomerList> customerlst = new List<LoadCustomerList>();
    List<LoadSaleBranch> salebranchlst = new List<LoadSaleBranch>();
    DateTime startdate = DateTime.Today;
    DateTime enddate = DateTime.Today;
    int receiptnumberfrom { get; set; } = 1;
    int receiptnumberto { get; set; } = 99999999;
    string baseUrl;

    #region string LocalizationInit
    string PageTitle;
    string ReceiptNumber;
    string ReservationCategory;
    string DateOfDispatch;
    string ReceptionOffice;
    string CarDestination;
    string Preview;
    string Export;

    /// <summary>
    /// Initialize localization strings
    /// </summary>
    private void LocalizationInit()
    {
        PageTitle = Lang["PageTitle"];
        ReceiptNumber = Lang["ReceiptNumber"];
        ReservationCategory = Lang["ReservationCategory"];
        DateOfDispatch = Lang["DateOfDispatch"];
        ReceptionOffice = Lang["ReceptionOffice"];
        CarDestination = Lang["CarDestination"];
        Preview = Lang["Preview"];
        Export = Lang["Export"];
    }
    #endregion

    #region Component Lifecycle
    /// <summary>
    /// Invoked once, after OnInit is finished.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        LocalizationInit();
        formContext = new EditContext(data);
        baseUrl = AppSettingsService.GetBaseUrl();
        reservationlst = await TPM_YoyKbnDataService.GetYoyKbnbySiyoKbn();
        customerlst = await Http.GetJsonAsync<List<LoadCustomerList>>(baseUrl + "/api/Customer/get");
        salebranchlst = await Http.GetJsonAsync<List<LoadSaleBranch>>(baseUrl + "/api/ReceiveBookingSaleBranch/" + Common.TenantID);

        data.ReservationStart = reservationlst.First();
        data.ReservationEnd = reservationlst.First();
        data.ReceptionOffice = salebranchlst.First();
        data.CustomerFrom = customerlst.First();
        data.CustomerTo = customerlst.First();
    }
    #endregion

    #region Value changed methods
    /// <summary>
    ///
    /// </summary>
    /// <param name="newValue"></param>
    void OnReceiptNumberFromChange(string newValue)
    {
        receiptnumberfrom = int.Parse(newValue);
        data.ReceiptNumberFrom = newValue;
        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="newValue"></param>
    void OnReceiptNumberToChange(string newValue)
    {
        receiptnumberto = int.Parse(newValue);
        data.ReceiptNumberTo = newValue;
        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="reservation"></param>
    void OnReservationStartChanged(ReservationData reservation)
    {
        data.ReservationStart = reservation;
        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="reservation"></param>
    void OnReservationEndChanged(ReservationData reservation)
    {
        data.ReservationEnd = reservation;
        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="newDate"></param>
    void OnDispatchDateFromChanged(DateTime newDate)
    {
        startdate = newDate;
        data.StartDate = newDate;
        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="newDate"></param>
    void OnDispatchDateToChanged(DateTime newDate)
    {
        enddate = newDate;
        data.EndDate = newDate;
        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="e"></param>
    void OnReceptionOfficeChanged(LoadSaleBranch e)
    {
        data.ReceptionOffice = e ?? new LoadSaleBranch();
        formContext.Validate();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="e"></param>
    void OnCustomerFromChanged(LoadCustomerList e)
    {
        data.CustomerFrom = e ?? new LoadCustomerList();
        formContext.Validate();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="e"></param>
    void OnCustomerToChanged(LoadCustomerList e)
    {
        data.CustomerTo = e ?? new LoadCustomerList();
        formContext.Validate();
    }
    #endregion

    #region Button Action
    private void HandleValidSubmit()
    {
        // To do
    }
    #endregion

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private CustomHttpClient Http { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private AppSettingsService AppSettingsService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ITPM_EigyosDataListService TPM_EigyosDataService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ITPM_CompnyDataListService TPM_CompnyDataService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ITPM_YoyKbnDataListService TPM_YoyKbnDataService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IStringLocalizer<VenderRequestForm> Lang { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IJSRuntime JSRuntime { get; set; }
    }
}
#pragma warning restore 1591
