#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\QuotationWithJourney.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "34d0c4e01796adcab6b9442fe4cb96be1fd99c47"
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
#line 3 "E:\Project\HassyaAllrightCloud\Pages\QuotationWithJourney.razor"
using HassyaAllrightCloud.Pages.Components.Popup;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "E:\Project\HassyaAllrightCloud\Pages\QuotationWithJourney.razor"
using HassyaAllrightCloud.Pages.Components.CommonComponents;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/quotationwithjourney")]
    public partial class QuotationWithJourney : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 286 "E:\Project\HassyaAllrightCloud\Pages\QuotationWithJourney.razor"
       
    protected bool IsFirstLoading { get; set; } = true;
    protected EditContext FormContext { get; set; }
    protected SimpleQuotationData FilterData;
    protected List<LoadSaleBranch> Branches { get; set; } = new List<LoadSaleBranch>();
    protected Dictionary<string, string> LangDic { get; set; }
    protected MyPopupModel MyPopup { get; set; }
    protected bool IsLoading { get; set; }

    private CustomerComponent CustomerRef { get; set; }
    private DefaultCustomerData DefaultValueCustomerFrom { get; set; }
    private DefaultCustomerData DefaultValueCustomerTo { get; set; }

    protected bool BranchEmpty => (Branches?.Count ?? 0) == 0;
    protected bool IsExporting;

    private string baseUrl;
    private bool isFilterApply;
    private string filterConditionFormName = "MK1300";
    private int userLoginId = (new ClaimModel()).SyainCdSeq;
    private int tenantId = (new ClaimModel()).TenantID;
    private string popupTitleInfo;
    private string close;

    protected string BranchEmptyMessage;
    protected string FilterDataEmptyMessage;

    #region Localization string

    /// <summary>
    /// Init value for Localization text
    /// </summary>
    private void LocalizationInit()
    {
        try
        {
            var dataLang = Lang.GetAllStrings();
            LangDic = dataLang.ToDictionary(l => l.Name, l => l.Value);

            BranchEmptyMessage = Lang[Constants.ErrorMessage.SQ_BranchGetErrorOrEmpty];
            FilterDataEmptyMessage = Lang[Constants.ErrorMessage.SQ_SubmitGetErrorOrEmpty];

            popupTitleInfo = Lang["popupTitleInfo"];
            close = Lang["close"];
        }
        catch (Exception)
        {
            throw;
        }
    }

    #endregion

    #region Component Lifecycle

    protected override async Task OnInitializedAsync()
    {
        try
        {
            FilterData = new SimpleQuotationData(tenantId, userLoginId);
            MyPopup = new MyPopupModel();
            LocalizationInit();
            baseUrl = AppSettingsService.GetBaseUrl();
            FormContext = new EditContext(FilterData);

            await LoadBranchAsync();

            await ApplyFilter();

            IsFirstLoading = false;
            await base.OnInitializedAsync();
            await InvokeAsync(StateHasChanged);
        }
        catch (Exception ex)
        {
            // show
            ErrorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    protected override void OnAfterRender(bool firstRender)
    {
        try
        {
            JSRuntime.InvokeAsync<object>("browserResize.collapseButtonFixedBottom");
            JSRuntime.InvokeAsync<string>("loadPageScript", "simpleQuotationPage", "tabKey");
            JSRuntime.InvokeAsync<string>("loadPageScript", "simpleQuotationPage", "enterKey");
            JSRuntime.InvokeVoidAsync("setEventforCodeNumberField");
        }
        catch (Exception ex)
        {
            // show
            ErrorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    #endregion

    #region Load Data
    /// <summary>
    /// LoadBookingType for booking type combobox
    /// </summary>
    protected async Task LoadBranchAsync()
    {
        try
        {
            Branches = await BranchDataService.GetBranchDataByTenantId(new ClaimModel().TenantID);
            if (Branches?.Count > 0) InsertSelectAll(Branches);
            FilterData.BranchStart = Branches.FirstOrDefault();
            FilterData.BranchEnd = Branches.FirstOrDefault();
            await InvokeAsync(StateHasChanged);
        }
        catch (Exception)
        {
            // TODO: Error load branch
            Branches = new List<LoadSaleBranch>();
            throw;
        }
    }

    #endregion

    #region Value changed method

    /// <summary>
    /// Clear button action handler
    /// </summary>
    protected async Task HandleResetSearchOption()
    {
        try
        {
            FilterData.SimpleCloneProperties(new SimpleQuotationData(FilterData.TenantId, FilterData.UserLoginId));
            await FilterServices.DeleteCustomFilerCondition(userLoginId, 1, filterConditionFormName);
            FormContext.MarkAsUnmodified();

            await InvokeAsync(StateHasChanged);
        }
        catch (Exception ex)
        {
            // show
            ErrorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    /// <summary>
    /// Changed export type preview and pdf
    /// </summary>
    /// <param name="exportType">New export type</param>
    protected void HandleExportTypeClicked(OutputReportType exportType)
    {
        try
        {
            FilterData.ExportType = exportType;
            if (!isFilterApply)
            {
                StateHasChanged();
            }
        }
        catch (Exception ex)
        {
            // show
            ErrorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    /// <summary>
    /// Changed OutputOrientation
    /// </summary>
    /// <param name="newOutPut">New OutputOrientation</param>
    protected void HandleOutputOrientationChanged(OutputOrientation newOutPut)
    {
        try
        {
            FilterData.OutputOrientation = newOutPut;
            if (!isFilterApply)
            {
                StateHasChanged();
            }
        }
        catch (Exception ex)
        {
            // show
            ErrorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    /// <summary>
    /// Change pickup from to date value
    /// </summary>
    /// <param name="newBranch">New selected PickupDate</param>
    /// <param name="isFrom">true is PickupDate from, false is PickupDate to</param>
    protected async void HandlePickupDateChanged(DateTime? newDate, bool isFrom = true)
    {
        try
        {
            if (isFrom)
            {
                FilterData.StartPickupDate = newDate;
            }
            else
            {
                FilterData.EndPickupDate = newDate;
            }

            if (!isFilterApply)
            {
                //IsLoading = true;
                // load data customer
                //await LoadCustomerAsync().ContinueWith(t => IsLoading = false);
                await InvokeAsync(StateHasChanged);
            }
        }
        catch (Exception ex)
        {
            // show
            ErrorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    /// <summary>
    /// Change arrival from to date value
    /// </summary>
    /// <param name="newDate">New selected ArrivalDate</param>
    /// <param name="isFrom">true is ArrivalDate from, false is ArrivalDate to</param>
    protected async void HandleArrivalDateChanged(DateTime? newDate, bool isFrom = true)
    {
        try
        {
            if (isFrom)
            {
                FilterData.StartArrivalDate = newDate;
            }
            else
            {
                FilterData.EndArrivalDate = newDate;
            }

            if (!isFilterApply)
            {
                //IsLoading = true;
                // load data customer
                //await LoadCustomerAsync().ContinueWith(t => IsLoading = false);
                await InvokeAsync(StateHasChanged);
            }
        }
        catch (Exception ex)
        {
            // show
            ErrorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    private void ChangeValueForm(string ValueName, dynamic value, EditContext formContext)
    {
        try
        {
            bool isChangeValue = true;

            if (isChangeValue)
            {
                var propertyInfo = FilterData.GetType().GetProperty(ValueName);
                propertyInfo.SetValue(FilterData, value, null);
                formContext.Validate();
            }
            InvokeAsync(StateHasChanged);
        }
        catch (Exception ex)
        {
            ErrorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    /// <summary>
    /// Change ukecd value
    /// </summary>
    /// <param name="newUkeCd">New ukecd</param>
    /// <param name="isFrom">true is ukecd from, false is ukecd to</param>
    protected void HandleUkeCdChanged(string newUkeCd, bool isFrom = true)
    {
        try
        {
            if (isFrom)
            {
                FilterData.UkeCdFrom = newUkeCd;
            }
            else
            {
                FilterData.UkeCdTo = newUkeCd;
            }

            if (!isFilterApply)
            {
                StateHasChanged();
            }
        }
        catch (Exception ex)
        {
            // show
            ErrorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    /// <summary>
    /// Change branch selected item
    /// </summary>
    /// <param name="newBranch">New selected branch</param>
    /// <param name="isFrom">true is branch from, false is branch to</param>
    protected void HandleSaleBranchChanged(LoadSaleBranch newBranch, bool isFrom = true)
    {
        try
        {
            if (isFrom)
            {
                FilterData.BranchStart = newBranch;
            }
            else
            {
                FilterData.BranchEnd = newBranch;
            }

            if (!isFilterApply)
            {
                StateHasChanged();
            }
        }
        catch (Exception ex)
        {
            // show
            ErrorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    /// <summary>
    /// Check or uncheck is display min max fare in report
    /// </summary>
    /// <param name="value">Check or uncheck</param>
    protected void HandleCheckeFareChanged(bool value)
    {
        try
        {
            FilterData.Fare = value;
            if (!isFilterApply)
            {
                StateHasChanged();
            }
        }
        catch (Exception ex)
        {
            // show
            ErrorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    /// <summary>
    /// Handle user submit
    /// </summary>
    protected async Task HandleButtonReportClicked()
    {
        try
        {
            if (IsExportEnable())
            {
                IsExporting = true;
                await SaveCurrentFilter();

                var bookingKeys = await SimpleQuotationService.GetBookingKeyListAsync(FilterData);
                if (bookingKeys != null && bookingKeys.Any())
                {
                    var param = new SimpleQuotationReportFilter(
                        bookingKeys,
                        tenantId,
                        FilterData.Fare,
                        FilterData.OutputOrientation == OutputOrientation.Horizontal
                            ? QuotationReportType.JourneyHorizontal
                            : QuotationReportType.JourneyVertical
                    );

                    switch (FilterData.ExportType)
                    {
                        case OutputReportType.Preview:
                            HandlePreviewReport(param);
                            IsExporting = false;
                            break;
                        case OutputReportType.ExportPdf:
                            await HandleExportPDFReport(param).ContinueWith(t => IsExporting = false);
                            break;
                        default:
                            IsExporting = false;
                            break;
                    }
                }
                else
                {
                    IsExporting = false;
                    MyPopup.Build()
                        .WithTitle(popupTitleInfo)
                        .WithBody(FilterDataEmptyMessage)
                        .WithIcon(MyPopupIconType.Info)
                        .AddButton(new MyPopupFooterButton(close, HandleClosePopup))
                        .Show();
                }
            }
            await InvokeAsync(StateHasChanged);
        }
        catch (Exception ex)
        {
            // show
            ErrorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    /// <summary>
    /// Handle preview report data
    /// </summary>
    /// <param name="param">Bookingkeys of filter form, tenantId, is display min max fare</param>
    private void HandlePreviewReport(SimpleQuotationReportFilter param)
    {
        try
        {
            var searchString = EncryptHelper.EncryptToUrl(param);
            JSRuntime.InvokeVoidAsync("open", "SimpleQuotationReportPreview?searchString=" + searchString, "_blank");
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Handle export pdf report data
    /// </summary>
    /// <param name="param">Bookingkeys of filter form, tenantId, is display min max fare</param>
    private async Task HandleExportPDFReport(SimpleQuotationReportFilter param)
    {
        try
        {
            DevExpress.XtraReports.UI.XtraReport report = await SimpleQuotationService.GetReport(param);

            await new System.Threading.Tasks.TaskFactory().StartNew(() =>
            {
                report.CreateDocument();
                using (MemoryStream ms = new MemoryStream())
                {
                    report.ExportToPdf(ms);
                    byte[] exportedFileBytes = ms.ToArray();
                    string myExportString = Convert.ToBase64String(exportedFileBytes);
                    JSRuntime.InvokeVoidAsync("downloadFileClientSide", myExportString, "pdf", "QuotationWithJourneyReport");
                }
            });
        }
        catch (Exception)
        {
            throw;
        }
    }

    #endregion

    #region Action

    /// <summary>
    /// Handle close popup
    /// </summary>
    private void HandleClosePopup()
    {
        try
        {
            MyPopup.Hide();
            StateHasChanged();
        }
        catch (Exception ex)
        {
            // show
            ErrorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    /// <summary>
    /// Check if form data is valid to submit
    /// </summary>
    protected bool IsExportEnable()
    {
        try
        {
            return !IsExporting
                    //&& !BookingTypeEmpty
                    //&& !CustomerEmpty
                    && !BranchEmpty
                    && FormContext.Validate();
        }
        catch (Exception ex)
        {
            // show
            ErrorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            return false;
        }
    }


    #endregion

    #region Helper

    /// <summary>
    /// Save form filter
    /// </summary>
    private async Task SaveCurrentFilter()
    {
        try
        {
            await FilterServices.SaveCustomFilterAndConditions(
                SimpleQuotationService.GetFieldValues(FilterData),
                filterConditionFormName,
                userLoginId
            );
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Apply filter if page have saved filter in db
    /// </summary>
    private async Task ApplyFilter()
    {
        try
        {
            var filterValues = (await FilterServices.GetFilterCondition(filterConditionFormName, userLoginId))
                                        .ToDictionary(inp => inp.ItemNm, inp => inp.JoInput).ConvertMultipleToSingleValues()
                                        .ConvertMultipleToSingleValues();
            if (filterValues.Count > 0)
            {
                isFilterApply = true;
                SimpleQuotationService.ApplyFilter(ref FilterData, filterValues);

                HandlePickupDateChanged(FilterData.StartPickupDate);
                HandlePickupDateChanged(FilterData.EndPickupDate, false);
                // load new list customer after change PickupDate
                //await LoadCustomerAsync();
                HandleArrivalDateChanged(FilterData.StartArrivalDate);
                HandleArrivalDateChanged(FilterData.EndArrivalDate, false);
                //HandleBookingTypeChanged(FilterData.BookingTypeStart == null ? null : BookingTypes.FirstOrDefault(_ => _ != null && _.YoyaKbnSeq == FilterData.BookingTypeStart.YoyaKbnSeq));
                //HandleBookingTypeChanged(FilterData.BookingTypeEnd == null ? null : BookingTypes.FirstOrDefault(_ => _ != null && _.YoyaKbnSeq == FilterData.BookingTypeEnd.YoyaKbnSeq), false);
                //HandleCustomerSelectedChanged(FilterData.CustomerStart == null ? null : Customers.FirstOrDefault(_ => _ != null && _.TokuiSeq == FilterData.CustomerStart.TokuiSeq && _.SitenCdSeq == FilterData.CustomerStart.SitenCdSeq));
                //HandleCustomerSelectedChanged(FilterData.CustomerEnd == null ? null : Customers.FirstOrDefault(_ => _ != null && _.TokuiSeq == FilterData.CustomerEnd.TokuiSeq && _.SitenCdSeq == FilterData.CustomerEnd.SitenCdSeq), false);
                HandleSaleBranchChanged(FilterData.BranchStart == null ? null : Branches.FirstOrDefault(_ => _ != null && _.EigyoCdSeq == FilterData.BranchStart.EigyoCdSeq));
                HandleSaleBranchChanged(FilterData.BranchEnd == null ? null : Branches.FirstOrDefault(_ => _ != null && _.EigyoCdSeq == FilterData.BranchEnd.EigyoCdSeq), false);
                HandleUkeCdChanged(FilterData.UkeCdFrom);
                HandleUkeCdChanged(FilterData.UkeCdTo, false);
                HandleExportTypeClicked(FilterData.ExportType);
                HandleOutputOrientationChanged(FilterData.OutputOrientation);
                HandleCheckeFareChanged(FilterData.Fare);

                isFilterApply = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Add null item to first position for combobox data list
    /// </summary>
    /// <param name="source">Data list of combobox</param>
    private void InsertSelectAll<T>(List<T> source) where T : class
    {
        try
        {
            if (source.Any(item => item is null))
                return;
            source.Insert(0, null);
        }
        catch (Exception)
        {
            throw;
        }
    }

    #endregion

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ISimpleQuotationService SimpleQuotationService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IErrorHandlerService ErrorModalService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private AppSettingsService AppSettingsService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IFilterCondition FilterServices { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ICustomerListService CustomerService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ITPM_EigyosDataListService BranchDataService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ITPM_YoyKbnDataListService TPM_YoyKbnDataService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IJSRuntime JSRuntime { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IStringLocalizer<QuotationWithJourney> Lang { get; set; }
    }
}
#pragma warning restore 1591
