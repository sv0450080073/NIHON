#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\TransportationContract.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "c0797bd7ca68775ba6036875a9f34bf3996b635a"
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
#line 1 "E:\Project\HassyaAllrightCloud\Pages\TransportationContract.razor"
using DevExpress.XtraPrinting;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\Project\HassyaAllrightCloud\Pages\TransportationContract.razor"
using Microsoft.AspNetCore.WebUtilities;

#line default
#line hidden
#nullable disable
#nullable restore
#line 19 "E:\Project\HassyaAllrightCloud\Pages\TransportationContract.razor"
using HassyaAllrightCloud.Domain.Dto.CommonComponents;

#line default
#line hidden
#nullable disable
#nullable restore
#line 20 "E:\Project\HassyaAllrightCloud\Pages\TransportationContract.razor"
using HassyaAllrightCloud.IService.CommonComponents;

#line default
#line hidden
#nullable disable
    public partial class TransportationContract : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 236 "E:\Project\HassyaAllrightCloud\Pages\TransportationContract.razor"
       

    [Parameter] public string Date { get; set; }
    [Parameter] public string Option { get; set; }

    BookingTypeData allBookingType;
    IEnumerable<BookingTypeData> SelectedBookingTypeItems = new List<BookingTypeData>();
    Dictionary<string, string> LangDic = new Dictionary<string, string>();
    Dictionary<string, string> keyValueFilterPairs = new Dictionary<string, string>();
    public bool checkAll { get; set; } = true;
    List<BookingTypeData> YoyakuKbnList = new List<BookingTypeData>();
    List<SaleBranchData> SaleBranchList = new List<SaleBranchData>();
    List<StaffsData> StaffList = new List<StaffsData>();
    List<LoadCustomerList> CustomerList = new List<LoadCustomerList>();
    private List<CustomerComponentGyosyaData> listGyosya { get; set; }
    private List<CustomerComponentTokiskData> listTokiSaki { get; set; }
    private List<CustomerComponentTokiStData> listTokiSiten { get; set; }
    public TransportationContractFormData transportationContractFormData = new TransportationContractFormData();
    int ReturnRecordNumber;
    bool DataNotExistPopup { get; set; } = false;
    bool isLoading { get; set; }

    private async Task ToggleLoading(bool value)
    {
        isLoading = value;
        await InvokeAsync(StateHasChanged);
        await Task.Delay(100);
    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            await ToggleLoading(true);
            var dataLang = Lang.GetAllStrings();
            LangDic = dataLang.ToDictionary(l => l.Name, l => l.Value);

            var taskGyosya = _customerService.GetListGyosya();
            var taskTokisk = _customerService.GetListTokisk();
            var taskTokist = _customerService.GetListTokiSt();

            await Task.WhenAll(taskGyosya, taskTokisk, taskTokist);

            listGyosya = taskGyosya.Result;
            listTokiSaki = taskTokisk.Result;
            listTokiSiten = taskTokist.Result;

            YoyakuKbnList = await YoyKbnService.GetBySiyoKbn();
            allBookingType = new BookingTypeData() { YoyaKbnSeq = -1, YoyaKbnNm = Lang["All"] };
            YoyakuKbnList.Insert(0, allBookingType);
            SaleBranchList = await TpmEigyosService.Get(new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID);
            StaffList = await SyainService.Get(new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID);
            CustomerList = await CustomerService.Get(new ClaimModel().TenantID);
            transportationContractFormData = BuildSearchModel(transportationContractFormData).Result;
            SelectedBookingTypeItems = (transportationContractFormData.YoyakuKbnList != null) ? transportationContractFormData.YoyakuKbnList : YoyakuKbnList;
            checkAll = SelectedBookingTypeItems.Any(item => item != null && item.YoyaKbnSeq == -1);

            /*CHECK PARAM*/
            if (!string.IsNullOrWhiteSpace(Date)
                && !string.IsNullOrWhiteSpace(Option)
                && (Option == PrintMode.Preview.ToString() || Option == PrintMode.SaveAsPDF.ToString()))
            {
                NavManager.NavigateTo("/transportationcontract", false);

                if (DateTime.TryParseExact(Date, "yyyyMMdd", null, DateTimeStyles.None, out DateTime outDateTime))
                {
                    transportationContractFormData.DateFrom
                        = transportationContractFormData.DateTo
                        = outDateTime;
                }

                if (Option == PrintMode.Preview.ToString())
                {
                    transportationContractFormData.PrintMode = (int)PrintMode.Preview;
                }
                if (Option == PrintMode.SaveAsPDF.ToString())
                {
                    transportationContractFormData.PrintMode = (int)PrintMode.SaveAsPDF;
                }

                SelectedBookingTypeItemsChanged(SelectedBookingTypeItems);
                HandleValidSubmit(null);
            }
            await ToggleLoading(false);
        }
        catch (Exception ex)
        {
            errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }

    }

    /// <summary>
    /// Load javascript of page
    /// </summary>
    protected override void OnParametersSet()
    {
        try
        {
            JSRuntime.InvokeVoidAsync("loadPageScript", "transportationContractPage");
            StateHasChanged();
            base.OnParametersSet();
        }
        catch (Exception ex)
        {
            errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        try
        {
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync("focusFirstItem");
            }
            await JSRuntime.InvokeVoidAsync("setEventforCodeNumberFieldV2", ".code-number", true, 10, true);
            await JSRuntime.InvokeVoidAsync("EnterTab", "#canTab");
        }
        catch (Exception ex)
        {
            errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    private async Task<TransportationContractFormData> BuildSearchModel(TransportationContractFormData model)
    {
        var filters = await FilterConditionService.GetFilterCondition(FormFilterName.TransportationContract, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq);
        if (filters.Count() == 0)
        {
            InitializedStateOfFormData();
            model = transportationContractFormData;
        }
        else
        {
            foreach (var item in filters)
            {
                var propertyInfo = model.GetType().GetProperty(item.ItemNm);
                if (propertyInfo != null && !string.IsNullOrEmpty(item.JoInput))
                {
                    switch (item.ItemNm)
                    {
                        case nameof(TransportationContractFormData.PrintMode):
                            propertyInfo.SetValue(model, int.TryParse(item.JoInput, out int print) ? (PrintMode)print : PrintMode.Preview);
                            break;
                        case nameof(TransportationContractFormData.OutputUnit):
                            propertyInfo.SetValue(model, int.TryParse(item.JoInput, out int output) ? (OutputUnit)output : OutputUnit.EachBooking);
                            break;

                        case nameof(TransportationContractFormData.DateTypeContract):
                            propertyInfo.SetValue(model, int.TryParse(item.JoInput, out int dateType) ? (DateTypeContract)dateType : DateTypeContract.Dispatch);
                            break;
                        case nameof(TransportationContractFormData.DateTo):
                        case nameof(TransportationContractFormData.DateFrom):
                            propertyInfo.SetValue(model, DateTime.TryParseExact(item.JoInput, "yyyyMMdd", new CultureInfo("ja-JP"), DateTimeStyles.None, out DateTime selectedDate) ? selectedDate : (DateTime?)null);
                            break;
                        case nameof(TransportationContractFormData.OutputSelection):
                            propertyInfo.SetValue(model, int.TryParse(item.JoInput, out int outputSel) ? (OutputSelection)outputSel : (int)OutputSelection.All);
                            break;
                        case nameof(TransportationContractFormData.YearlyContract):
                            propertyInfo.SetValue(model, int.TryParse(item.JoInput, out int yearlyCtr) ? (YearlyContract)yearlyCtr : YearlyContract.NotOutput);
                            break;
                        case nameof(TransportationContractFormData.UketsukeEigyoJo):
                            var selectedEigyoCdSeq = int.TryParse(item.JoInput, out int selectedE) ? selectedE : 0;
                            var selectedEigyo = SaleBranchList.FirstOrDefault(e => e.EigyoCdSeq == selectedEigyoCdSeq);
                            propertyInfo.SetValue(model, selectedEigyo);
                            break;
                        case nameof(TransportationContractFormData.EigyoTantoSha):
                        case nameof(TransportationContractFormData.InpSyainCd):
                            var selectedPicCdSeq = int.TryParse(item.JoInput, out int selectedP) ? selectedP : 0;
                            var selectedPic = StaffList.FirstOrDefault(e => e.SyainCdSeq == selectedPicCdSeq);
                            propertyInfo.SetValue(model, selectedPic);
                            break;
                        case nameof(TransportationContractFormData.UkeNumber):
                            var ukeNo = int.TryParse(item.JoInput, out int selectedU) ? selectedU.ToString() : "";
                            propertyInfo.SetValue(model, ukeNo);
                            break;
                        case nameof(TransportationContractFormData.Gyosya):
                            var selectedGyosyaCdSeq = int.TryParse(item.JoInput, out int selectedG) ? selectedG : 0;
                            var selectedGyosya = listGyosya.FirstOrDefault(e => e.GyosyaCdSeq == selectedGyosyaCdSeq);
                            propertyInfo.SetValue(model, selectedGyosya);
                            break;
                        case nameof(TransportationContractFormData.TokuiSaki):
                            var selectedTokuiSakiCdSeq = int.TryParse(item.JoInput, out int selectedT) ? selectedT : 0;
                            var selectedTokuiSaki = listTokiSaki.FirstOrDefault(e => e.TokuiSeq == selectedTokuiSakiCdSeq);
                            propertyInfo.SetValue(model, selectedTokuiSaki);
                            break;

                        case nameof(TransportationContractFormData.TokuiSiten):
                            var selectedTokuiSitenCdSeq = int.TryParse(item.JoInput, out int selectedS) ? selectedS : 0;
                            var selectedTokuiSiten = listTokiSiten.FirstOrDefault(e => e.SitenCdSeq == selectedTokuiSitenCdSeq);
                            propertyInfo.SetValue(model, selectedTokuiSiten);
                            break;

                        case nameof(TransportationContractFormData.YoyakuKbnList):
                            var datas1 = item.JoInput.Split(",");
                            var selectedYykKbn = new List<BookingTypeData>();
                            foreach (string data in datas1)
                            {
                                if (!String.IsNullOrEmpty(data))
                                {
                                    var selectYoyKbn = int.TryParse(data, out int selectedY) ? selectedY : 0;
                                    var bookingTypeData = YoyakuKbnList.FirstOrDefault(x => x.YoyaKbnSeq == selectedY && x.TenantCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().TenantID);
                                    if (bookingTypeData != null) selectedYykKbn.Add(bookingTypeData);
                                }
                            }
                            propertyInfo.SetValue(model, selectedYykKbn);
                            break;
                        case nameof(TransportationContractFormData.IsUpdateExportDate):
                            bool IsUpdate = bool.Parse(item.JoInput);
                            propertyInfo.SetValue(model, IsUpdate);
                            break;
                    }
                }
            }
        }
        return model;
    }


    void InitializedStateOfFormData()
    {
        try
        {
            transportationContractFormData = new TransportationContractFormData();
            transportationContractFormData.PrintMode = (int)PrintMode.Preview;
            transportationContractFormData.OutputUnit = (int)OutputUnit.EachBooking;
            transportationContractFormData.DateTypeContract = (int)DateTypeContract.Dispatch;
            transportationContractFormData.OutputSelection = (int)OutputSelection.All;
            transportationContractFormData.YearlyContract = (int)YearlyContract.NotOutput;
            transportationContractFormData.DateFrom = DateTime.Today;
            transportationContractFormData.DateTo = DateTime.Today;
            transportationContractFormData.YoyakuKbnList = YoyakuKbnList;
            transportationContractFormData.IsUpdateExportDate = true;
        }
        catch (Exception ex)
        {
            errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }

    }


    void ChangePrintMode(MouseEventArgs e, int number)
    {
        try
        {
            transportationContractFormData.PrintMode = number;
        }
        catch (Exception ex)
        {
            errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    void ChangeOutputUnit(MouseEventArgs e, int number)
    {
        try
        {
            transportationContractFormData.OutputUnit = number;
        }
        catch (Exception ex)
        {
            errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    void ChangeIsUpdateExportDate(MouseEventArgs e, bool IsUpdate)
    {
        try
        {
            transportationContractFormData.IsUpdateExportDate = IsUpdate;
        }
        catch (Exception ex)
        {
            errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    void ChangeDateTypeContract(MouseEventArgs e, int number)
    {
        try
        {
            transportationContractFormData.DateTypeContract = number;
        }
        catch (Exception ex)
        {
            errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    void ChangeOutputSelection(MouseEventArgs e, int number)
    {
        try
        {
            transportationContractFormData.OutputSelection = number;
        }
        catch (Exception ex)
        {
            errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    void ChangeYearlyContract(MouseEventArgs e, int number)
    {
        try
        {
            transportationContractFormData.YearlyContract = number;
        }
        catch (Exception ex)
        {
            errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    void OnSelectedSaleBranchChanged(SaleBranchData e)
    {
        try
        {
            transportationContractFormData.UketsukeEigyoJo = e;
            InvokeAsync(StateHasChanged);
        }
        catch (Exception ex)
        {
            errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    void OnSelectedStaffChanged(StaffsData e)
    {
        try
        {
            transportationContractFormData.EigyoTantoSha = e;
            InvokeAsync(StateHasChanged);
        }
        catch (Exception ex)
        {
            errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    void OnSelectedInpStaffChanged(StaffsData e)
    {
        try
        {
            transportationContractFormData.InpSyainCd = e;
            InvokeAsync(StateHasChanged);
        }
        catch (Exception ex)
        {
            errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    void OnChangeGyosya(CustomerComponentGyosyaData e)
    {
        try
        {
            transportationContractFormData.Gyosya = e;
            InvokeAsync(StateHasChanged);
        }
        catch (Exception ex)
        {
            errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    void OnChangeTokuiSaki(CustomerComponentTokiskData e)
    {
        try
        {
            transportationContractFormData.TokuiSaki = e;
            InvokeAsync(StateHasChanged);
        }
        catch (Exception ex)
        {
            errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    void OnChangeTokuiSiten(CustomerComponentTokiStData e)
    {
        try
        {
            transportationContractFormData.TokuiSiten = e;
            InvokeAsync(StateHasChanged);
        }
        catch (Exception ex)
        {
            errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    void OnUkeNumberChanged(string newValue)
    {
        try
        {
            if (int.TryParse(newValue, out int v) && v > 0)
            {
                transportationContractFormData.UkeNumber = (newValue as string).PadLeft(10, '0');
            }
            else
            {
                transportationContractFormData.UkeNumber = null;
            }
            InvokeAsync(StateHasChanged);
        }
        catch (Exception ex)
        {
            errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }

    }

    void SelectedBookingTypeItemsChanged(IEnumerable<BookingTypeData> selectedBookingTypeItems)
    {
        try
        {
            SelectedBookingTypeItems = selectedBookingTypeItems;
            if (checkAll == true && !SelectedBookingTypeItems.Contains(allBookingType))
            {
                SelectedBookingTypeItems = SelectedBookingTypeItems.Take(0);
                checkAll = false;
            }
            if (checkAll == false && SelectedBookingTypeItems.Contains(allBookingType))
            {
                SelectedBookingTypeItems = YoyakuKbnList;
                checkAll = true;
            }
            if (checkAll == true && SelectedBookingTypeItems.Contains(allBookingType) && SelectedBookingTypeItems.Count() < YoyakuKbnList.Count())
            {
                SelectedBookingTypeItems = SelectedBookingTypeItems.Where(t => t.YoyaKbnNm != allBookingType.YoyaKbnNm);
                checkAll = false;
            }
            if (checkAll == false && !SelectedBookingTypeItems.Contains(allBookingType) && SelectedBookingTypeItems.Count() == YoyakuKbnList.Count() - 1)
            {
                SelectedBookingTypeItems = YoyakuKbnList;
                checkAll = true;
            }
            transportationContractFormData.YoyakuKbnList = SelectedBookingTypeItems.ToList();
            InvokeAsync(StateHasChanged);
        }
        catch (Exception ex)
        {
            errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }

    }


    private async void OnStartDateChanged(DateTime? newValue)
    {
        try
        {
            transportationContractFormData.DateFrom = newValue;
            transportationContractFormData.DateTo = newValue;
            await InvokeAsync(StateHasChanged);
        }
        catch (Exception ex)
        {
            errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    private async void OnEndDateChanged(DateTime? newValue)
    {
        try
        {
            transportationContractFormData.DateTo = newValue;
            await InvokeAsync(StateHasChanged);
        }
        catch (Exception ex)
        {
            errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }


    async void HandleValidSubmit(MouseEventArgs args)
    {
        try
        {
            await ToggleLoading(true);
            keyValueFilterPairs = GenerateFilterValueDictionaryService.GenerateForTransportationContract(transportationContractFormData).Result;
            FilterConditionService.SaveFilterCondtion(keyValueFilterPairs, FormFilterName.TransportationContract, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq).Wait();

            TransportationContractFormData transportationContractFormDataClone = (TransportationContractFormData)transportationContractFormData.Clone();
            if (!string.IsNullOrEmpty(transportationContractFormDataClone.UkeNumber))
            {
                transportationContractFormDataClone.UkeNumber = advancePaymentDetailsService.FormatCodeNumber(new ClaimModel().TenantID.ToString()) + CommonUtil.FormatCodeNumber(transportationContractFormDataClone.UkeNumber);
            }

            ReturnRecordNumber = await HikiukeshoReportService.GetHikiukeshoRowResult(transportationContractFormDataClone);

            if (ReturnRecordNumber <= 0)
            {
                DataNotExistPopup = true;
            }
            else
            {
                HikiukeshoReportService.CallReportService(transportationContractFormDataClone, SelectedBookingTypeItems, JSRuntime, 0);
                if (transportationContractFormData.IsUpdateExportDate)
                {
                    await HikiukeshoReportService.UpdateHikiukeshoExportDate(transportationContractFormDataClone);
                }
            }
            await ToggleLoading(false);
        }
        catch (Exception ex)
        {
            errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }

    }

    async Task ClearAll(MouseEventArgs args)
    {
        try
        {
            InitializedStateOfFormData();
            SelectedBookingTypeItems = YoyakuKbnList;
            await FilterConditionService.DeleteCustomFilerCondition(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, 0, FormFilterName.TransportationContract);
        }
        catch (Exception ex)
        {
            errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }

    }

    CultureInfo ci = new CultureInfo("ja-JP");

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ICustomerComponentService _customerService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IAdvancePaymentDetailsService advancePaymentDetailsService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IGenerateFilterValueDictionary GenerateFilterValueDictionaryService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IGetFilterDataService GetFilterDataService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IFilterCondition FilterConditionService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IErrorHandlerService errorModalService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IStringLocalizer<TransportationContract> Lang { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private CustomNavigation NavManager { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IJSRuntime JSRuntime { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IHikiukeshoReportService HikiukeshoReportService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ISaleBranchListService TpmEigyosService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ICustomerListService CustomerService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IStaffListService SyainService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IBusTypeListService BusTypeService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IBookingTypeListService YoyKbnService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private AppSettingsService AppSettingsService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private CustomHttpClient Http { get; set; }
    }
}
#pragma warning restore 1591