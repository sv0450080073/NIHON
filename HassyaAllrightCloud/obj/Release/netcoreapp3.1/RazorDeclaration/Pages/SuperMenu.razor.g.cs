#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\SuperMenu.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "36f8cf609c86df345a2dd1221644181d11bab7e1"
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
    [Microsoft.AspNetCore.Components.RouteAttribute("/supermenu")]
    public partial class SuperMenu : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 550 "E:\Project\HassyaAllrightCloud\Pages\SuperMenu.razor"
       
    [Parameter] public string type { get; set; }
    [Parameter] public string HaishaBiFrom { get; set; }
    [Parameter] public string HaishaBiTo { get; set; }
    [Parameter] public string TochakuBiFrom { get; set; }
    [Parameter] public string TochakuBiTo { get; set; }
    [Parameter] public string YoyakuBiFrom { get; set; }
    [Parameter] public string YoyakuBiTo { get; set; }
    [Parameter] public string UketsukeBangoFrom { get; set; }
    [Parameter] public string UketsukeBangoTo { get; set; }
    [Parameter] public string YoyakuKbnFrom { get; set; }
    [Parameter] public string YoyakuKbnTo { get; set; }
    [Parameter] public string EigyoTantoShaFrom { get; set; }
    [Parameter] public string EigyoTantoShaTo { get; set; }
    [Parameter] public string UketsukeEigyoJoFrom { get; set; }
    [Parameter] public string UketsukeEigyoJoTo { get; set; }
    [Parameter] public string NyuryokuTantoShaFrom { get; set; }
    [Parameter] public string NyuryokuTantoShaTo { get; set; }
    [Parameter] public string TokuiSakiFrom { get; set; }
    [Parameter] public string TokuiSakiTo { get; set; }
    [Parameter] public string ShiireSakiFrom { get; set; }
    [Parameter] public string ShiireSakiTo { get; set; }
    [Parameter] public string DantaiKbnFrom { get; set; }
    [Parameter] public string DantaiKbnTo { get; set; }
    [Parameter] public string KyakuDaneKbnFrom { get; set; }
    [Parameter] public string KyakuDaneKbnTo { get; set; }
    [Parameter] public string YukiSakiFrom { get; set; }
    [Parameter] public string YukiSakiTo { get; set; }
    [Parameter] public string HaishaChiFrom { get; set; }
    [Parameter] public string HaishaChiTo { get; set; }
    [Parameter] public string HasseiChiFrom { get; set; }
    [Parameter] public string HasseiChiTo { get; set; }
    [Parameter] public string AreaFrom { get; set; }
    [Parameter] public string AreaTo { get; set; }
    [Parameter] public string ShashuFrom { get; set; }
    [Parameter] public string ShashuTo { get; set; }
    [Parameter] public string ShashuTankaFrom { get; set; }
    [Parameter] public string ShashuTankaTo { get; set; }
    [Parameter] public string UketsukeJokenFrom { get; set; }
    [Parameter] public string UketsukeJokenTo { get; set; }

    public int ActiveV { get; set; }
    string dateFormat = "yy/MM/dd";
    List<SuperMenuReservationData> SuperMenuReservationGridData = new List<SuperMenuReservationData>();
    List<SuperMenuVehicleData> SuperMenuVehicleGridData = new List<SuperMenuVehicleData>();
    public HyperFormData hyperData = new HyperFormData();
    public HyperFormData hyperDataInit = new HyperFormData();
    int activeTabIndex = 0;
    int ActiveTabIndex
    {
        get => activeTabIndex;
        set
        {
            activeTabIndex = value;
            AdjustHeightWhenTabChanged();
        }
    }
    List<BookingTypeData> BookingTypeList;
    List<SaleBranchData> SaleBranchList;
    List<StaffsData> StaffList;
    List<LoadCustomerList> CustomerList;
    List<CodeTypeData> CodeKbList;
    List<CustomerClassification> CustomerClassificationList;
    List<LoadLocation> DestinationList;
    List<LoadDispatchArea> DispatchList;
    List<LoadLocation> OriginList;
    List<LoadLocation> AreaList;
    List<BusTypesData> BusTypeList;
    List<VpmCodeKb> ConditionList;
    bool IsValid = true;
    bool isLoading { get; set; } = true;
    bool IsInitForDate = false;
    bool IsNoData = false;
    int RecordsPerPage = 30;
    int FirstPageSelect = 0;

    SuperMenuReservationTotalGridData SuperMenuReservationTotalGridData = new SuperMenuReservationTotalGridData();
    SuperMenuVehicleTotalGridData SuperMenuVehicleTotalGridData = new SuperMenuVehicleTotalGridData();

    /// <summary>
    /// Load javascript of page
    /// </summary>
    protected override void OnParametersSet()
    {
        JSRuntime.InvokeVoidAsync("loadPageScript", "superMenuPage");
        base.OnParametersSet();
    }

    protected override async Task OnInitializedAsync()
    {
        ActiveV = (int)ViewMode.Medium;
        // ????????????
        List<BookingTypeData> TempBookingTypeList = await YoyKbnService.GetBySiyoKbn();
        BookingTypeList = TempBookingTypeList;
        BookingTypeList.Insert(0, null);

        // ???????????????
        List<SaleBranchData> TempSaleBranchList = await EigyosService.Get(Common.CompanyID);
        SaleBranchList = TempSaleBranchList;
        SaleBranchList.Insert(0, null);

        // ????????????, ????????????
        List<StaffsData> TempStaffList = await SyainService.Get(Common.CompanyID);
        StaffList = TempStaffList;
        StaffList.Insert(0, null);

        // ?????????, ?????????
        List<LoadCustomerList> TempCustomerList = await CustomerService.Get(Common.TenantID);
        CustomerList = TempCustomerList;
        CustomerList.Insert(0, null);

        // ????????????
        List<CodeTypeData> TempCodeKbList = await CodeKbService.GetDantai(Common.TenantID);
        CodeKbList = TempCodeKbList;
        CodeKbList.Insert(0, null);

        // ????????????
        List<CustomerClassification> TempCustomerClassificationList = await CustomerCLassificationService.Get(Common.TenantID);
        CustomerClassificationList = TempCustomerClassificationList;
        CustomerClassificationList.Insert(0, null);

        // ??????
        List<LoadLocation> TempDestinationList = await LocationService.GetDestination(Common.TenantID);
        DestinationList = TempDestinationList;
        DestinationList.Insert(0, null);

        // ?????????
        List<LoadDispatchArea> TempDispatchList = await DispatchService.Get(Common.TenantID);
        DispatchList = TempDispatchList;
        DispatchList.Insert(0, null);

        // ?????????
        List<LoadLocation> TempOriginList = await LocationService.GetOrigin(Common.TenantID);
        OriginList = TempOriginList;
        OriginList.Insert(0, null);

        // ?????????
        List<LoadLocation> TempAreaList = await LocationService.GetArea(Common.TenantID);
        AreaList = TempAreaList;
        AreaList.Insert(0, null);

        // ??????
        List<BusTypesData> TempBusTypeList = await BusTypeService.GetAll(Common.TenantID);
        BusTypeList = TempBusTypeList;
        BusTypeList.Insert(0, null);

        // ????????????
        List<VpmCodeKb> TempConditionList = await CodeKbService.GetJoken(Common.TenantID);
        ConditionList = TempConditionList;
        ConditionList.Insert(0, null);

        hyperData = HyperFormData.ToObject(HaishaBiFrom, HaishaBiTo, TochakuBiFrom, TochakuBiTo, YoyakuBiFrom, YoyakuBiTo, UketsukeBangoFrom, UketsukeBangoTo,
            YoyakuKbnFrom, YoyakuKbnTo, TempBookingTypeList,
            EigyoTantoShaFrom, EigyoTantoShaTo, TempStaffList,
            UketsukeEigyoJoFrom, UketsukeEigyoJoTo, TempSaleBranchList,
            NyuryokuTantoShaFrom, NyuryokuTantoShaTo,
            TokuiSakiFrom, TokuiSakiTo, TempCustomerList,
            ShiireSakiFrom, ShiireSakiTo,
            DantaiKbnFrom, DantaiKbnTo, TempCodeKbList,
            KyakuDaneKbnFrom, KyakuDaneKbnTo, TempCustomerClassificationList,
            YukiSakiFrom, YukiSakiTo, TempDestinationList,
            HaishaChiFrom, HaishaChiTo, TempDispatchList,
            HasseiChiFrom, HasseiChiTo, TempOriginList,
            AreaFrom, AreaTo, TempAreaList,
            ShashuFrom, ShashuTo, TempBusTypeList,
            ShashuTankaFrom, ShashuTankaTo,
            UketsukeJokenFrom, UketsukeJokenTo, TempConditionList);
        hyperDataInit = new HyperFormData(hyperData);
        IsInitForDate = CheckInitForDate();
        if (!(int.TryParse(type, out _)) || !(Enum.IsDefined(typeof(SuperMenyTypeDisplay), int.Parse(type))) || !IsInitForDate)
        {
            NavigationManager.NavigateTo("/hypermenu", true);
            return;
        }
    }

    private bool CheckInitForDate()
    {
        return !(hyperData.HaishaBiFrom == null &&
            hyperData.HaishaBiTo == null &&
            hyperData.TochakuBiFrom == null &&
            hyperData.TochakuBiTo == null &&
            hyperData.YoyakuBiFrom == null &&
            hyperData.YoyakuBiTo == null);
    }

    protected override async void OnAfterRender(bool firstRender)
    {
        await JSRuntime.InvokeVoidAsync("loadPageScript", "hyperMenuPage", "hyperMenuPageTabKey");
        if (firstRender)
        {
            await JSRuntime.InvokeVoidAsync("setEventforCurrencyField", false);
            await JSRuntime.InvokeVoidAsync("setEventforCodeNumberField");
            await JSRuntime.InvokeAsync<string>("addMaxLength", "length", 7);
            await JSRuntime.InvokeAsync<string>("addMaxLength", "length", 10);
            await JSRuntime.InvokeVoidAsync("fadeToggleWidthAdjustHeight");

            if (int.Parse(type) == (int)SuperMenyTypeDisplay.Reservation)
            {
                SuperMenuReservationGridData = await HyperDataService.GetSuperMenuReservationData(hyperData, Common.CompanyID, Common.TenantID);
                IsNoData = SuperMenuReservationGridData.Count() == 0;
                if (!IsNoData)
                {
                    List<SuperMenuReservationData> FirstPageData = await HyperDataService.GetSuperMenuReservationData(hyperData, Common.CompanyID, Common.TenantID, 0, RecordsPerPage);
                    SuperMenuReservationGridData.RemoveRange(0, FirstPageData.Count());
                    SuperMenuReservationGridData.InsertRange(0, FirstPageData);
                    SuperMenuReservationTotalGridData = await HyperDataService.GetSuperMenuReservationTotalData(hyperData, Common.CompanyID, Common.TenantID);
                }
            }
            else // Supper menu vehicle
            {
                SuperMenuVehicleGridData = await HyperDataService.GetSuperMenuVehicleData(hyperData, Common.CompanyID, Common.TenantID);
                IsNoData = SuperMenuVehicleGridData.Count() == 0;
                if (!IsNoData)
                {
                    List<SuperMenuVehicleData> FirstPageData = await HyperDataService.GetSuperMenuVehicleData(hyperData, Common.CompanyID, Common.TenantID, 0, RecordsPerPage);
                    SuperMenuVehicleGridData.RemoveRange(0, FirstPageData.Count());
                    SuperMenuVehicleGridData.InsertRange(0, FirstPageData);
                    SuperMenuVehicleTotalGridData = await HyperDataService.GetSuperMenuVehicleTotalData(hyperData, Common.CompanyID, Common.TenantID);
                }
            }
            isLoading = false;
            StateHasChanged();
        }
    }

    void clickV(MouseEventArgs e, int number)
    {
        ActiveV = number;
    }

    private IEnumerable<string> Store(IEnumerable<string> errorMessage)
    {
        if (errorMessage.Count() > 0)
        {
            IsValid = false;
        }
        else
        {
            IsValid = true;
        }
        return errorMessage;
    }

    private async Task ChangeState(int Page = 0)
    {
        IsInitForDate = CheckInitForDate();
        if (IsValid && IsInitForDate)
        {
            if (int.Parse(type) == (int)SuperMenyTypeDisplay.Reservation)
            {
                SuperMenuReservationGridData = await HyperDataService.GetSuperMenuReservationData(hyperData, Common.CompanyID, Common.TenantID);
                int NumberOfPage = (SuperMenuReservationGridData.Count() + RecordsPerPage - 1) / RecordsPerPage;
                IsNoData = SuperMenuReservationGridData.Count() == 0;
                if (!IsNoData)
                {
                    FirstPageSelect = Math.Min(Page, NumberOfPage - 1);
                    List<SuperMenuReservationData> FirstPageData = await HyperDataService.GetSuperMenuReservationData(hyperData, Common.CompanyID, Common.TenantID, FirstPageSelect * RecordsPerPage, RecordsPerPage);
                    SuperMenuReservationGridData.RemoveRange(FirstPageSelect * RecordsPerPage, FirstPageData.Count());
                    SuperMenuReservationGridData.InsertRange(FirstPageSelect * RecordsPerPage, FirstPageData);
                    SuperMenuReservationTotalGridData = await HyperDataService.GetSuperMenuReservationTotalData(hyperData, Common.CompanyID, Common.TenantID);
                }
                else
                {
                    FirstPageSelect = 0;
                    SuperMenuReservationTotalGridData = new SuperMenuReservationTotalGridData();
                }
            }
            else
            {
                SuperMenuVehicleGridData = await HyperDataService.GetSuperMenuVehicleData(hyperData, Common.CompanyID, Common.TenantID);
                int NumberOfPage = (SuperMenuVehicleGridData.Count() + RecordsPerPage - 1) / RecordsPerPage;
                IsNoData = SuperMenuVehicleGridData.Count() == 0;
                if (!IsNoData)
                {
                    FirstPageSelect = Math.Min(Page, NumberOfPage - 1);
                    List<SuperMenuVehicleData> FirstPageData = await HyperDataService.GetSuperMenuVehicleData(hyperData, Common.CompanyID, Common.TenantID, FirstPageSelect * RecordsPerPage, RecordsPerPage);
                    SuperMenuVehicleGridData.RemoveRange(FirstPageSelect * RecordsPerPage, FirstPageData.Count());
                    SuperMenuVehicleGridData.InsertRange(FirstPageSelect * RecordsPerPage, FirstPageData);
                    SuperMenuVehicleTotalGridData = await HyperDataService.GetSuperMenuVehicleTotalData(hyperData, Common.CompanyID, Common.TenantID);
                }
                else
                {
                    FirstPageSelect = 0;
                    SuperMenuVehicleTotalGridData = new SuperMenuVehicleTotalGridData();
                }
            }
        }
        await InvokeAsync(StateHasChanged);
    }

    private async Task ChangeValueForm(string ValueName, dynamic value)
    {
        if (value is string && string.IsNullOrEmpty(value))
        {
            value = null;
        }
        var propertyInfo = hyperData.GetType().GetProperty(ValueName);
        propertyInfo.SetValue(hyperData, value, null);
        isLoading = true;
        await Task.Run(() =>
        {
            InvokeAsync(StateHasChanged).Wait();
            isLoading = false;
            ChangeState().Wait();
        });
    }

    private async void AdjustHeightWhenTabChanged()
    {
        await Task.Run(() =>
        {
            InvokeAsync(StateHasChanged).Wait();
            JSRuntime.InvokeVoidAsync("AdjustHeight");
        });
    }

    private void ResetHyperForm()
    {
        ActiveV = (int)ViewMode.Medium;
        hyperData = new HyperFormData(hyperDataInit);
        CheckInitForDate();
        InvokeAsync(StateHasChanged);
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private NavigationManager NavigationManager { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IStringLocalizer<HyperMenu> HyperLang { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IStringLocalizer<SuperMenu> Lang { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IJSRuntime JSRuntime { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IBusTypeListService BusTypeService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IDispatchListService DispatchService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ILocationListService LocationService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ICustomerCLassificationListService CustomerCLassificationService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ITPM_CodeKbListService CodeKbService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ICustomerListService CustomerService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IStaffListService SyainService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ISaleBranchListService EigyosService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IBookingTypeListService YoyKbnService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private AppSettingsService AppSettingsService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private CustomHttpClient Http { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IHyperDataService HyperDataService { get; set; }
    }
}
#pragma warning restore 1591
