#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\Sticker.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7903f0b2457a03a7a014df92d8c8ec4812b733c1"
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
    [Microsoft.AspNetCore.Components.RouteAttribute("/sticker")]
    public partial class Sticker : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 342 "E:\Project\HassyaAllrightCloud\Pages\Sticker.razor"
       
    public StickerData data = new StickerData();
    EditContext formContext;
    string baseUrl;

    List<ReservationData> reservationlst = new List<ReservationData>();
    List<LoadCustomerList> customerlst = new List<LoadCustomerList>();
    List<LoadSaleBranch> salebranchlst = new List<LoadSaleBranch>();
    List<string> processingdivisionlst = new List<string>();
    List<string> categorynamelst = new List<string>();
    List<string> displayorderlst = new List<string>();
    List<string> stickerusedlst = new List<string>();
    List<string> fontnamelst = new List<string>();
    List<string> fontstylelst = new List<string>();
    List<string> fontsizelst = new List<string>();
    List<string> fontscriptlst = new List<string>();
    List<LoadSticker> StickerList = new List<LoadSticker>();

    bool SearchResult { get; set; } = false;
    bool IsShowPreviewReport { get; set; } = false;
    bool IsShowOptionPrintReport { get; set; } = false;
    bool IsEdit { get; set; } = false;
    bool IsSignUp { get; set; } = false;
    bool PopupDelete { get; set; } = false;
    bool PopupEdit { get; set; } = false;    

    int FlagProcessing { get; set; }

    string DisplayBookingTypeCmb = string.Empty;
    bool IsSelectedAll = true;
    IEnumerable<ReservationData> BookingTypesSelected { get; set; } = new List<ReservationData>();

    #region Component Lifecycle
    /// <summary>
    /// Invoked once, after OnInit is finished.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        formContext = new EditContext(data);
        baseUrl = AppSettingsService.GetBaseUrl();
        reservationlst = await TPM_YoyKbnDataService.GetYoyKbnbySiyoKbn();
        reservationlst.Insert(0, new ReservationData());
        customerlst = await Http.GetJsonAsync<List<LoadCustomerList>>(baseUrl + "/api/Customer/get");
        salebranchlst = await Http.GetJsonAsync<List<LoadSaleBranch>>(baseUrl + "/api/ReceiveBookingSaleBranch/" + new ClaimModel().TenantID);
        processingdivisionlst = new List<string>(){
            "予約データ",
            "手入力データ",
        };
        categorynamelst = new List<string>(){
            "すべて",
            "ツア",
            "学校",
        };
        displayorderlst = new List<string>(){
            "すべて",
            "出庫時間順",
            "得意先順",
        };
        stickerusedlst = new List<string>() {
            "東急観光​",
        };
        fontnamelst = new List<string>() {
            "ＭＳ 明朝",
        };
        fontstylelst = new List<string>() {
            "太字",
        };
        fontsizelst = new List<string>() {
            "12",
        };
        fontscriptlst = new List<string>() {
            "japanese",
        };
        StickerList.Add(new LoadSticker {
            Organization = "1台中1号車",
            CarNumber = "01",
            DeliveryTime = "10:00",
            Customer = "網走バス",
            ReservationGroupName = "予約書団体名",
            BasicSizeUsed = "",
            StickerNumber = 5,
            Sticker = "ステッカー",
            StickerSize = "東急観光​",
            SideSticker = "サイドステッカー",
            SideUseSize = "東急観光​",
            Detail = "詳細",
        });

        data.ProcessingDivision = processingdivisionlst.First();
        data.DateOfDispatch = DateTime.Today;
        data.ReceptionOffice = salebranchlst.First();
        data.CustomerFrom = customerlst.First();
        data.CustomerTo = customerlst.First();
        data.DisplayOrder = displayorderlst.First();
        data.CategoryName = categorynamelst.First();
        data.StickerUsed = stickerusedlst.First();
        data.FontName = fontnamelst.First();
        data.FontStyle = fontstylelst.First();
        data.FontSize = fontsizelst.First();
        data.FontScript = fontscriptlst.First();

        BookingTypesSelected = reservationlst;
    }
    #endregion

    #region Value changed methods
    /// <summary>
    ///
    /// </summary>
    /// <param name="selectedItem"></param>
    void OnSelectProcessingVivisionItemChanged(string selectedItem)
    {
        data.ProcessingDivision = selectedItem;
        StateHasChanged();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="newDate"></param>
    void OnDateOfDispatchChanged(DateTime newDate)
    {
        data.DateOfDispatch = newDate;
        StateHasChanged();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="reservations"></param>
    private void OnBookingTypesChanged(IEnumerable<ReservationData> reservations)
    {
        int newCount = reservations.Count();
        int maxCount = reservationlst.Count();

        BookingTypesSelected = reservations;

        if (newCount == 1 && (!BookingTypesSelected.FirstOrDefault()?.BookingTypeName?.Equals(Constants.SelectedAll) ?? false))
        {
            DisplayBookingTypeCmb = BookingTypesSelected.FirstOrDefault()?.BookingTypeName ?? string.Empty;
        }
        else if (newCount == maxCount)
        {
            DisplayBookingTypeCmb = Constants.SelectedAll;
        }
        else
        {
            DisplayBookingTypeCmb = $"{Lang["Choices"]}：{newCount}";
        }

        bool isContainSelectAll = BookingTypesSelected.Where(_ => _.BookingTypeName.Equals(Constants.SelectedAll)).Any();

        if (IsSelectedAll == true)
        {
            if (!isContainSelectAll)
            {
                BookingTypesSelected = BookingTypesSelected.Take(0);
                IsSelectedAll = false;
            }
            else if (isContainSelectAll && newCount < maxCount)
            {
                BookingTypesSelected = BookingTypesSelected.Where(_ => !_.BookingTypeName.Equals(Constants.SelectedAll));
                IsSelectedAll = false;
            }
        }
        else
        {
            if (isContainSelectAll || (!isContainSelectAll && newCount == maxCount - 1))
            {
                BookingTypesSelected = reservationlst;
                IsSelectedAll = true;
            }
        }

        data.BookingTypes = BookingTypesSelected.ToList();

        StateHasChanged();
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

    /// <summary>
    ///
    /// </summary>
    /// <param name="selectedItem"></param>
    void OnCategoryNameChanged(string selectedItem)
    {
        data.CategoryName = selectedItem;
        StateHasChanged();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="selectedItem"></param>
    void OnDisplayOrderChanged(string selectedItem)
    {
        data.DisplayOrder = selectedItem;
        StateHasChanged();
    }
    #endregion

    #region Button Action
    /// <summary>
    ///
    /// </summary>
    void Search()
    {
        SearchResult = true;
        IsEdit = false;
        IsSignUp = false;
        if (data.ProcessingDivision == "予約データ")
        {
            FlagProcessing = 1;
        }
        else
        {
            FlagProcessing = 2;
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    private void HandleValidSubmit()
    {
        // To do
    }    

    /// <summary>
    ///
    /// </summary>
    void Print()
    {
        // To do
    }

    /// <summary>
    ///
    /// </summary>
    void Export()
    {
        // To do
    }

    /// <summary>
    ///
    /// </summary>
    void Delete()
    {
        // To do
    }

    /// <summary>
    /// 
    /// </summary>
    protected void HandleAddSticker()
    {
        //to do
        IsSignUp = false;
        StateHasChanged();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    protected void HandleSaveEditSticker(int index)
    {
        //to do
        IsEdit = false;
        StateHasChanged();
    }

    /// <summary>
    /// 
    /// </summary>
    protected void HandleCancelCreateSticker()
    {
        IsSignUp = false;
        StateHasChanged();
    }

    /// <summary>
    /// 
    /// </summary>    
    protected void HandleCancelEditSticker()
    {
        IsEdit = false;
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
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IStringLocalizer<Sticker> Lang { get; set; }
    }
}
#pragma warning restore 1591
