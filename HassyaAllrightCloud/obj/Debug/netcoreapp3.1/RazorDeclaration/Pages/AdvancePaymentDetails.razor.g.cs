#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\AdvancePaymentDetails.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "fa2ae6f197efb9ff52d9a5eeb6274b7a9589744a"
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
#line 1 "E:\Project\HassyaAllrightCloud\Pages\AdvancePaymentDetails.razor"
using DevExpress.XtraReports.UI;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\Project\HassyaAllrightCloud\Pages\AdvancePaymentDetails.razor"
using DevExpress.XtraPrinting;

#line default
#line hidden
#nullable disable
#nullable restore
#line 12 "E:\Project\HassyaAllrightCloud\Pages\AdvancePaymentDetails.razor"
using HassyaAllrightCloud.Pages.Components.CommonComponents;

#line default
#line hidden
#nullable disable
    public partial class AdvancePaymentDetails : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 223 "E:\Project\HassyaAllrightCloud\Pages\AdvancePaymentDetails.razor"
       
    [Parameter] public string DateFrom { get; set; }
    [Parameter] public string DateTo { get; set; }
    [Parameter] public string Option { get; set; }

    List<int> listCompany = new List<int>();

    public EditContext searchForm { get; set; }
    public AdvancePaymentDetailsSearchParam searchParams { get; set; } = new AdvancePaymentDetailsSearchParam();
    public List<PaymentSearchDropdown> listAddressSpectify { get; set; } = new List<PaymentSearchDropdown>();
    public List<PaperSizeDropdown> listPaperSize { get; set; } = new List<PaperSizeDropdown>();
    public List<SeikyuSakiSearch> listAddress { get; set; } = new List<SeikyuSakiSearch>();
    public Dictionary<string, string> LangDic = new Dictionary<string, string>();
    Dictionary<string, string> keyValueFilterPairs = new Dictionary<string, string>();
    public bool isDisableButton { get; set; } = false;
    public bool isDataNotFound { get; set; } = false;
    public bool isDisableCombobox { get; set; } = true;
    public CustomerComponent CustomerFromRef { get; set; }
    public CustomerComponent CustomerToRef { get; set; }
    public DefaultCustomerData DefaultValueFrom { get; set; } = new DefaultCustomerData();
    public DefaultCustomerData DefaultValueTo { get; set; } = new DefaultCustomerData();
    public bool isGyosyaValid { get; set; } = true;
    public bool isTokiskValid { get; set; } = true;
    public bool isTokistValid { get; set; } = true;
    protected bool IsFirstRender { get; set; } = true;
    protected bool isCustomerLoaded = false;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            await OnInitDataAsync();
        }
        catch (Exception ex)
        {
            errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    protected override void OnAfterRender(bool firstRender)
    {
        try
        {
            JSRuntime.InvokeVoidAsync("setEventforCodeNumberFieldV2", ".code-number", true, 10, true);
        }
        catch (Exception ex)
        {
            errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    private async Task OnInitDataAsync()
    {
        try
        {
            IsFirstRender = true;
            var dataLang = lang.GetAllStrings();
            LangDic = dataLang.ToDictionary(l => l.Name, l => l.Value);
            searchParams = new AdvancePaymentDetailsSearchParam();

            searchParams.OutputSetting = (byte)PrintMode.Preview;
            searchParams.PaperSize = new PaperSizeDropdown() { Value = (byte)PaperSize.A4, Text = lang["A4"] };

            listAddressSpectify = new List<PaymentSearchDropdown>();
            listAddressSpectify.Add(new PaymentSearchDropdown() { Value = 1, Text = lang["billAddress"] });
            listAddressSpectify.Add(new PaymentSearchDropdown() { Value = 2, Text = lang["customerAddress"] });
            listPaperSize.Add(new PaperSizeDropdown() { Value = (byte)PaperSize.A4, Text = lang["A4"] });
            listPaperSize.Add(new PaperSizeDropdown() { Value = (byte)PaperSize.A3, Text = lang["A3"] });
            listPaperSize.Add(new PaperSizeDropdown() { Value = (byte)PaperSize.B4, Text = lang["B4"] });
            //listAddress = await advancePaymentDetailsService.GetListAddressForSearch(searchParams.TenantCdSeq);
            //searchParams = GetFilterDataService.GetAdvancePaymentFormData(filterValues, listPaperSize, listAddressSpectify, listAddress);
            var filterValues = FilterConditionService.GetFilterCondition(FormFilterName.AdvancePaymentDetails, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq).Result;
            if (filterValues != null && filterValues.Any())
            {
                searchParams = GetFilterDataService.GetAdvancePaymentFormData(filterValues, listPaperSize, listAddressSpectify, listAddress);

                var customerModelFrom = (filterValues.FirstOrDefault(x => x.ItemNm == nameof(searchParams.CustomerModelFrom))?.JoInput)?.Split(',');
                var customerModelTo = (filterValues.FirstOrDefault(x => x.ItemNm == nameof(searchParams.CustomerModelTo))?.JoInput)?.Split(',');
                DefaultValueFrom.GyosyaCdSeq = int.Parse(customerModelFrom != null ? customerModelFrom[0] : "-1");
                DefaultValueFrom.TokiskCdSeq = int.Parse(customerModelFrom != null ? customerModelFrom[1] : "-1");
                DefaultValueFrom.TokiStCdSeq = int.Parse(customerModelFrom != null ? customerModelFrom[2] : "-1");
                DefaultValueTo.GyosyaCdSeq = int.Parse(customerModelTo != null ? customerModelTo[0] : "-1");
                DefaultValueTo.TokiskCdSeq = int.Parse(customerModelTo != null ? customerModelTo[1] : "-1");
                DefaultValueTo.TokiStCdSeq = int.Parse(customerModelTo != null ? customerModelTo[2] : "-1");
            }
            if (searchParams.AddressSpectify != null)
            {
                isDisableCombobox = false;
            }

            /*CHECK PARAM*/
            if (!string.IsNullOrWhiteSpace(DateFrom)
            && !string.IsNullOrWhiteSpace(DateTo)
            && !string.IsNullOrWhiteSpace(Option)
            && (Option == PrintMode.Preview.ToString() || Option == PrintMode.SaveAsPDF.ToString()))
            {
                NavManager.NavigateTo("/advancepaymentdetails", false);

                if (DateTime.TryParseExact(DateFrom, "yyyyMMdd", null, DateTimeStyles.None, out DateTime outDateTime))
                {
                    searchParams.ScheduleYmdStart = outDateTime;
                }
                if (DateTime.TryParseExact(DateTo, "yyyyMMdd", null, DateTimeStyles.None, out outDateTime))
                {
                    searchParams.ScheduleYmdEnd = outDateTime;
                }

                if (Option == PrintMode.Preview.ToString())
                {
                    searchParams.OutputSetting = (int)PrintMode.Preview;
                }
                if (Option == PrintMode.SaveAsPDF.ToString())
                {
                    searchParams.OutputSetting = (int)PrintMode.SaveAsPDF;
                }

                await OnNavigate();
            }

            searchForm = new EditContext(searchParams);
            searchParams.CustomerModelFrom = new CustomerModel();
            searchParams.CustomerModelTo = new CustomerModel();
        }
        catch (Exception ex)
        {
            errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    private async Task OninitCustomer(bool isSelecteAddressSpectify)
    {
        searchParams.CustomerModelFrom = new CustomerModel();
        searchParams.CustomerModelTo = new CustomerModel();

        if (CustomerFromRef != null && CustomerToRef != null)
        {
            if (isSelecteAddressSpectify)
            {
                DefaultValueFrom.GyosyaCdSeq = DefaultValueTo.GyosyaCdSeq = -1;
                await CustomerFromRef.SetNullComponent(true);
                await CustomerToRef.SetNullComponent(true);
            }
            else
            {
                await CustomerFromRef.SetNullComponent(false);
                await CustomerToRef.SetNullComponent(false);
            }
        }
    }

    protected async Task OnHandleChanged(dynamic value, string propName)
    {
        try
        {
            var classType = searchParams.GetType();
            var prop = classType.GetProperty(propName);

            switch (propName)
            {
                case "ReceptionNumber":

                    if (!String.IsNullOrEmpty(value) && (int.TryParse(value, out int v) && v > 0))
                    {
                        prop.SetValue(searchParams, (value as string).PadLeft(10, '0'), null);
                    }
                    else
                    {
                        prop.SetValue(searchParams, String.Empty, null);
                    }
                    break;
                case "ScheduleYmdStart":
                case "ScheduleYmdEnd":
                    prop.SetValue(searchParams, (value as DateTime?), null);
                    break;
                case "AddressSpectify":
                    prop.SetValue(searchParams, (value as PaymentSearchDropdown), null);
                    if (value != null)
                    {
                        isDisableCombobox = false;
                        await OninitCustomer(true);
                    }
                    else
                    {
                        isDisableCombobox = true;
                        await OninitCustomer(false);
                    }
                    break;
                case "StartAddress":
                case "EndAddress":
                    prop.SetValue(searchParams, (value as SeikyuSakiSearch), null);
                    break;
                case "PaperSize":
                    prop.SetValue(searchParams, (value as PaperSizeDropdown), null);
                    break;
            }
            StateHasChanged();
            isDisableButton = true ? searchForm.GetValidationMessages().Any() : false;
        }
        catch (Exception ex)
        {
            errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    protected async Task OnResetSearchData()
    {
        try
        {
            searchParams = new AdvancePaymentDetailsSearchParam();
            searchParams.OutputSetting = 1;
            searchParams.PaperSize = new PaperSizeDropdown() { Value = 1, Text = lang["A4"] }; ;
            searchParams.ReceptionNumber = null;
            searchParams.ScheduleYmdStart = null;
            searchParams.ScheduleYmdEnd = null;
            searchParams.AddressSpectify = null;
            isDisableCombobox = true;
            await OninitCustomer(false);
            await OnSaveFilterCondition();
        }
        catch (Exception ex)
        {
            errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    protected void OnOutputSetting(byte value)
    {
        try
        {
            searchParams.OutputSetting = value;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    protected void OnPaperSizeSetting(PaperSizeDropdown value)
    {
        try
        {
            searchParams.PaperSize = value;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }

    }

    protected async Task OnNavigate()
    {
        try
        {
            AdvancePaymentDetailsSearchParam searchParamsClone = (AdvancePaymentDetailsSearchParam)searchParams.Clone();
            if (!string.IsNullOrEmpty(searchParamsClone.ReceptionNumber))
            {
                searchParamsClone.ReceptionNumber = advancePaymentDetailsService.FormatCodeNumber(new ClaimModel().TenantID.ToString()) + searchParamsClone.ReceptionNumber;
            }

            keyValueFilterPairs = GenerateFilterValueDictionaryService.GenerateForAdvancedPayment(searchParams).Result;
            FilterConditionService.SaveFilterCondtion(keyValueFilterPairs, FormFilterName.AdvancePaymentDetails, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq).Wait();

            int ReturnRecordNumber = await advancePaymentDetailsService.GetAdvancePaymentDetailRowResult(searchParamsClone);
            if (ReturnRecordNumber == 0)
            {
                isDataNotFound = true;
            }
            else
            {
                await advancePaymentDetailsService.OnExportPdf(searchParamsClone.OutputSetting, searchParamsClone, JSRuntime, reportLayoutSettingService, 0);
                await OnSaveFilterCondition();
            }
        }
        catch (Exception ex)
        {
            errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    protected async Task OnModelChanged(string propertyName, dynamic val, bool isFrom)
    {
        var propertyFromInfo = searchParams.CustomerModelFrom.GetType().GetProperty(propertyName);
        var propertyToInfo = searchParams.CustomerModelTo.GetType().GetProperty(propertyName);

        if (propertyFromInfo != null && isFrom)
            propertyFromInfo.SetValue(searchParams.CustomerModelFrom, val);

        if (propertyToInfo != null && !isFrom)
            propertyToInfo.SetValue(searchParams.CustomerModelTo, val);

        if (IsFirstRender)
        {
            if (propertyName == nameof(searchParams.CustomerModelFrom.SelectedTokiSt) && !isFrom)
            {
                IsFirstRender = false;
                if (searchParams.AddressSpectify == null)
                    await OninitCustomer(false);
            }
            return;
        }
        else if (!IsFirstRender)
        {
            if (searchParams.CustomerModelFrom?.SelectedGyosya?.GyosyaCd > searchParams.CustomerModelTo?.SelectedGyosya?.GyosyaCd)
                isGyosyaValid = false;
            else
                isGyosyaValid = true;

            if ((searchParams.CustomerModelFrom?.SelectedGyosya?.GyosyaCd == searchParams.CustomerModelTo?.SelectedGyosya?.GyosyaCd) && (searchParams.CustomerModelFrom?.SelectedTokisk?.TokuiCd > searchParams.CustomerModelTo?.SelectedTokisk?.TokuiCd))
                isTokiskValid = false;
            else
                isTokiskValid = true;

            if ((searchParams.CustomerModelFrom?.SelectedGyosya?.GyosyaCd == searchParams.CustomerModelTo?.SelectedGyosya?.GyosyaCd) && (searchParams.CustomerModelFrom?.SelectedTokisk?.TokuiCd == searchParams.CustomerModelTo?.SelectedTokisk?.TokuiCd)
            && (searchParams.CustomerModelFrom?.SelectedTokiSt?.SitenCd > searchParams.CustomerModelTo?.SelectedTokiSt?.SitenCd))
                isTokistValid = false;
            else
                isTokistValid = true;
        }

        await InvokeAsync(StateHasChanged);
    }

    protected async Task OnSaveFilterCondition()
    {
        keyValueFilterPairs = await GenerateFilterValueDictionaryService.GenerateForAdvancedPayment(searchParams);
        await FilterConditionService.SaveFilterCondtion(keyValueFilterPairs, FormFilterName.AdvancePaymentDetails, 0, new ClaimModel().SyainCdSeq);
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IGenerateFilterValueDictionary GenerateFilterValueDictionaryService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IGetFilterDataService GetFilterDataService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IFilterCondition FilterConditionService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IErrorHandlerService errorModalService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IStringLocalizer<AdvancePaymentDetails> lang { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private CustomNavigation NavManager { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IJSRuntime JSRuntime { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IReportLayoutSettingService reportLayoutSettingService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IAdvancePaymentDetailsService advancePaymentDetailsService { get; set; }
    }
}
#pragma warning restore 1591
