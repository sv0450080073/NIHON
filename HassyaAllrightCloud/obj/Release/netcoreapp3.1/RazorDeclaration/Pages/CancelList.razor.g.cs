#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\CancelList.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "16e68ec1bffa54e127095b5075020e3e1aa2fca8"
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
    [Microsoft.AspNetCore.Components.RouteAttribute("/cancellist")]
    public partial class CancelList : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 534 "E:\Project\HassyaAllrightCloud\Pages\CancelList.razor"
       
    CancelListData.DataSearch data = new CancelListData.DataSearch();
    List<ReservationData> reservationlst = new List<ReservationData>();
    List<CompanyChartData> companylst = new List<CompanyChartData>();
    List<BranchChartData> branchlst = new List<BranchChartData>();
    List<LoadCustomerList> customerlst = new List<LoadCustomerList>();
    List<SupplierData> supplierlst = new List<SupplierData>();
    List<LoadStaff> stafflst = new List<LoadStaff>();
    List<string> reservationtypelst = new List<string>();
    List<string> cancellationchargelst = new List<string>();
    List<string> size_of_paper = new List<string>();
    List<string> heading_output = new List<string>();
    List<string> grouping_output = new List<string>();
    List<string> separator_output = new List<string>();
    List<string> pagebreak_specification = new List<string>();
    DateTime startdate = DateTime.Today;
    DateTime enddate = DateTime.Today;
    int activeTabIndex = 0;
    int ActiveTabIndex { get => activeTabIndex; set { activeTabIndex = value; StateHasChanged(); } }
    int datetype;
    int receiptnumberfrom { get; set; } = 1;
    int receiptnumberto { get; set; } = 99999999;
    int printmode;
    string baseUrl;
    int sortby;
    bool ResultSearch = false;
    bool isShow = false;
    bool isDisabled = false;
    bool isCSV = false;

    #region Component Lifecycle
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    protected override async Task OnInitializedAsync()
    {
        baseUrl = AppSettingsService.GetBaseUrl();
        reservationlst = await TPM_YoyKbnDataService.GetYoyKbnbySiyoKbn();
        companylst = await TPM_CompnyDataService.GetCompany(Common.TenantID);
        companylst.Insert(0, new CompanyChartData());
        if (data.Company != null)
        {
            branchlst = TPM_EigyosDataService.GetBranchbyCompany1(data.Company.CompanyCdSeq, Common.TenantID);
        }
        branchlst.Insert(0, new BranchChartData());
        customerlst = await Http.GetJsonAsync<List<LoadCustomerList>>(baseUrl + "/api/Customer/get");
        supplierlst = await Http.GetJsonAsync<List<SupplierData>>(baseUrl + "/api/BookingInput/Supervisor/Supplier");
        stafflst = await Http.GetJsonAsync<List<LoadStaff>>(baseUrl + "/api/Staff/" + Common.TenantID);
        reservationtypelst = new List<string>()
        {
            "?????????????????????",
            "?????????????????????",
            "??????",
        };
        cancellationchargelst = new List<string>()
        {
            "??????",
            "??????",
        };
        size_of_paper = new List<string>() {
            "A3",
            "A4",
            "B4",
        };
        heading_output = new List<string>()
        {
            "????????????",
            "???????????????"
        };
        grouping_output = new List<string>()
        {
            "??????????????????",
            "????????????????????????",
        };
        separator_output = new List<string>()
        {
            "?????????",
            "???????????????",
            "????????????",
            "????????????",
            "???????????????",
        };
        pagebreak_specification = new List<string>()
        {
            "????????????"
        };

        datetype = (int)DateType.Cancellation;
        printmode = (int)PrintMode.Preview;
        sortby = (int)SortCancel.Customer;
        data.BookingCategory = reservationlst.First();
        data.ReservationType = reservationtypelst.First();
        data.CancellationCharge = cancellationchargelst.First();
        data.Company = companylst.First();
        data.BranchStart = branchlst.First();
        data.BranchEnd = branchlst.First();
        if (stafflst.Count != 0)
        {
            data.ListStaffStart = stafflst.ToList().Where(t => t.SyainCd == Common.UserId).First();
            data.ListStaffEnd = stafflst.ToList().Where(t => t.SyainCd == Common.UserId).First();
        }
        data.SizeOfPaper = size_of_paper.First();
        data.HeadingOutput = heading_output.First();
        data.GroupingOutput = grouping_output.First();
        data.SeparatorOutput = separator_output.First();
        data.PagebreakSpecification = pagebreak_specification.First();
    }
    #endregion

    #region value changed method
    /// <summary>
    ///
    /// </summary>
    /// <param name="newDate"></param>
    void OnStartDateChanged(DateTime newDate)
    {
        startdate = newDate;
        data.StartDate = newDate;
        StateHasChanged();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="newDate"></param>
    void OnEndDateChanged(DateTime newDate)
    {
        enddate = newDate;
        data.EndDate = newDate;
        StateHasChanged();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="e"></param>
    /// <param name="number"></param>
    void SelectDateType(MouseEventArgs e, int number)
    {
        datetype = number;
        data.DateType = number;
        StateHasChanged();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="e"></param>
    /// <param name="number"></param>
    void SelectSortBy(MouseEventArgs e, int number)
    {
        sortby = number;
        StateHasChanged();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="e"></param>
    /// <param name="number"></param>
    void SelectPrintMode(MouseEventArgs e, int number)
    {
        if (number == (int)PrintMode.SaveAsExcel)
        {
            isCSV = true;
        }
        else
        {
            isCSV = false;
        }
        printmode = number;
        data.PrintMode = number;
        StateHasChanged();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="reservation"></param>
    void OnBookingCategoryChanged(ReservationData reservation)
    {
        data.BookingCategory = reservation;
        StateHasChanged();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="selectedCompanyItems"></param>
    void OnSelectedCompanyItemsChanged(CompanyChartData selectedCompanyItems)
    {
        data.Company = selectedCompanyItems;
        branchlst = new List<BranchChartData>();
        if (data.Company != null)
        {
            branchlst = TPM_EigyosDataService.GetBranchbyCompany1(selectedCompanyItems.CompanyCdSeq, Common.TenantID);
        }
        branchlst.Insert(0, new BranchChartData());
        data.BranchStart = branchlst.First();
        data.BranchEnd = branchlst.First();
        StateHasChanged();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="selectedItems"></param>
    void OnSelectedBranchStartItemsChanged(BranchChartData selectedItems)
    {
        data.BranchStart = selectedItems;
        StateHasChanged();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="selectedItems"></param>
    void OnSelectedBranchEndItemsChanged(BranchChartData selectedItems)
    {
        data.BranchEnd = selectedItems;
        StateHasChanged();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="selectedItems"></param>
    void OnSelectedReservationTypeItemsChanged(string selectedItems)
    {
        data.ReservationType = selectedItems;
        StateHasChanged();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="selectedItems"></param>
    void OnSelectedCancellationChargeItemsChanged(string selectedItems)
    {
        data.CancellationCharge = selectedItems;
        StateHasChanged();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="newValue"></param>
    void OnReceiptNumberFromChange(string newValue)
    {
        receiptnumberfrom = int.Parse(newValue);
        data.ReceiptNumberFrom = newValue;
        StateHasChanged();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="newValue"></param>
    void OnReceiptNumberToChange(string newValue)
    {
        receiptnumberto = int.Parse(newValue);
        data.ReceiptNumberTo = newValue;
        StateHasChanged();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="selectedItems"></param>
    void OnCustomerStartChanged(LoadCustomerList selectedItems)
    {
        data.CustomerStart = selectedItems;
        StateHasChanged();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="selectedItems"></param>
    void OnCustomerEndChanged(LoadCustomerList selectedItems)
    {
        data.CustomerEnd = selectedItems;
        StateHasChanged();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="selectedItems"></param>
    void OnSupplierStartChanged(SupplierData selectedItems)
    {
        data.ListSupplierStart = selectedItems;
        StateHasChanged();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="selectedItems"></param>
    void OnSupplierEndChanged(SupplierData selectedItems)
    {
        data.ListSupplierEnd = selectedItems;
        StateHasChanged();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="e"></param>
    void OnStaffStartChanged(LoadStaff selectedItems)
    {
        data.ListStaffStart = selectedItems;
        StateHasChanged();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="e"></param>
    void OnStaffEndChanged(LoadStaff selectedItems)
    {
        data.ListStaffEnd = selectedItems;
        StateHasChanged();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="size"></param>
    void OnSelectSizeOfPaper(string size)
    {
        data.SizeOfPaper = size;
        StateHasChanged();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="selectedItems"></param>
    void OnSelectedHeadingOutput(string selectedItems)
    {
        data.HeadingOutput = selectedItems;
        StateHasChanged();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="selectedItems"></param>
    void OnSelectedGroupingOutput(string selectedItems)
    {
        data.GroupingOutput = selectedItems;
        StateHasChanged();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="selectedItems"></param>
    void OnSelectedSeparatorOutput(string selectedItems)
    {
        data.SeparatorOutput = selectedItems;
        if (data.SeparatorOutput == "????????????")
        {
            isDisabled = true;
            data.HeadingOutput = heading_output.Single(x => x == "???????????????");
            data.GroupingOutput = grouping_output.Single(x => x == "????????????????????????");
        }
        else
        {
            isDisabled = false;
        }
        if (data.SeparatorOutput == "???????????????")
        {
            isShow = true;
        }
        else
        {
            isShow = false;
        }
        StateHasChanged();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="newValue"></param>
    void OnNoteChanged(string newValue)
    {
        data.Note = newValue;
        StateHasChanged();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="newValue"></param>
    void OnSelectPagebreakSpecification(string newValue)
    {
        data.PagebreakSpecification = newValue;
        StateHasChanged();
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
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IJSRuntime JSRuntime { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IStringLocalizer<CancelList> Lang { get; set; }
    }
}
#pragma warning restore 1591
