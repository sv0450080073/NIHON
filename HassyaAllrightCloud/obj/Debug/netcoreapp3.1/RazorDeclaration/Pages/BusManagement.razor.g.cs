#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\BusManagement.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1c16826b4c2a8e20b02428eb33c62a200edeb10c"
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
#line 36 "E:\Project\HassyaAllrightCloud\_Imports.razor"
[Authorize]

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/bussmanagement")]
    public partial class BusManagement : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 455 "E:\Project\HassyaAllrightCloud\Pages\BusManagement.razor"
       
    int activeTabIndex = 0;
    int ActiveTabIndex { get => activeTabIndex; set { activeTabIndex = value; StateHasChanged(); } }
    int activeTabIndex1 = 0;
    int ActiveTabIndex1 { get => activeTabIndex1; set { activeTabIndex1 = value; StateHasChanged(); } }
    bool Value { get; set; }
    bool PopupSignUp = false;
    bool PopupOutputCSV = false;
    bool isReadonlyCSV = true;
    bool isReadonlyLayout = false;
    bool ResultSearch = false;
    bool LoadList = false;
    List<BusManagementData> DataSource = new List<BusManagementData>();
    public static List<string> ListDataCarType { get; set; }
    public static List<string> ListDataVehicle { get; set; }
    public static List<string> ListDataCompany { get; set; }
    public static List<string> ListDataBranch { get; set; }
    public static List<string> ListDataCompanyBranch { get; set; }
    public static List<string> ListDataLayout { get; set; }
    public static List<string> ListDataGarage { get; set; }
    public static List<string> ListDataBusiness { get; set; }
    public static List<string> ListDataPersonInCharge { get; set; }
    public static List<string> ListDataFuel { get; set; }

    public class BusManagementData
    {
        public string Field1 { get; set; }
        public string SyaSyuCd { get; set; }
        public string SyaSyuNm { get; set; }
        public string EigyoCd { get; set; }
        public string Field5_RyakuNm { get; set; }
        public string CompanyCd { get; set; }
        public string Field7_RyakuNm { get; set; }
        public string TenkoNo { get; set; }
        public string SyaRyoCd { get; set; }
        public string SyaRyoNm { get; set; }
        public string StaYmd { get; set; }
        public string EndYmd { get; set; }
        public string KariSyaRyoNm { get; set; }
        public string Kana { get; set; }
        public string TeiCnt { get; set; }
        public string HojoCnt { get; set; }
        public string Nenryo1_CodeKbnNm { get; set; }
        public string Nenryo2_CodeKbnNm { get; set; }
        public string Nenryo3_CodeKbnNm { get; set; }
        public string SyainCd { get; set; }
        public string SyainNm { get; set; }
    }

    private bool showListCarType, showListCompany = false;
    private string showListCarTypeClass => showListCarType ? "show" : null;
    private string showListCompanyClass => showListCompany ? "show" : null;

    /// <summary>
    /// Load javascript of page
    /// </summary>
    protected override void OnParametersSet()
    {
        JSRuntime.InvokeVoidAsync("loadPageScript", "busManagementPage");
        base.OnParametersSet();
    }

    private void toggleListCarType()
    {
        showListCarType = !showListCarType;
    }
    private void toggleListCompany()
    {
        showListCompany = !showListCompany;
    }

    private void DeleteRow()
    {
        // To do
    }

    /// <summary>
    /// select all items => display "全て" at position top
    /// select 1 item => display this item
    /// select more than 2 items => display 選択項目：number items
    /// no select item => display 選択項目：0 and message error
    /// 
    /// case 1: flag = true but no have contain item "全て" => remove all items, change flag = false
    /// case 2: flag = false but have contain item "全て" or all items is selected but no have contain item "全て"  => selected all items, change flag = true
    /// case 3: flag = true, have contain item "全て" but total selected items != total items in list => remove item "全て", change flag = false
    /// </summary>
    /// <param name="selectedCarTypeItems"></param>
    IEnumerable<string> SelectedCarTypeItems { get; set; } = new List<string>();
    private bool checkCarTypeAll { get; set; } = true;
    private void SelectedCarTypeItemsChanged(IEnumerable<string> selectedCarTypeItems)
    {
        SelectedCarTypeItems = selectedCarTypeItems;
        if (checkCarTypeAll == true && !SelectedCarTypeItems.Contains("全て"))
        {
            SelectedCarTypeItems = SelectedCarTypeItems.Take(0);
            checkCarTypeAll = false;
        }
        if (checkCarTypeAll == false && (SelectedCarTypeItems.Contains("全て") || (!SelectedCarTypeItems.Contains("全て") && SelectedCarTypeItems.Count() == ListDataCarType.Count() - 1)) )
        {
            SelectedCarTypeItems = ListDataCarType;
            checkCarTypeAll = true;
        }
        if (checkCarTypeAll == true && SelectedCarTypeItems.Contains("全て") && SelectedCarTypeItems.Count() < ListDataCarType.Count())
        {
            SelectedCarTypeItems = SelectedCarTypeItems.Where(t => t != "全て");
            checkCarTypeAll = false;
        }
        StateHasChanged();
    }

    /// <summary>
    /// select all items => display "全て" at position top
    /// select 1 item => display this item
    /// select more than 2 items => display 選択項目：number items
    /// no select item => display 選択項目：0 and message error
    /// 
    /// case 1: flag = true but no have contain item "全て" => remove all items, change flag = false
    /// case 2: flag = false but have contain item "全て" or all items is selected but no have contain item "全て"  => selected all items, change flag = true
    /// case 3: flag = true, have contain item "全て" but total selected items != total items in list => remove item "全て", change flag = false
    /// </summary>
    /// <param name="selectedCompanyItems"></param>
    IEnumerable<string> SelectedCompanyItems { get; set; } = new List<string>();
    private bool checkCompanyAll { get; set; } = true;
    private void SelectedCompanyChanged(IEnumerable<string> selectedCompanyItems)
    {
        SelectedCompanyItems = selectedCompanyItems;
        if (checkCarTypeAll == true && !SelectedCompanyItems.Contains("全て"))
        {
            SelectedCompanyItems = SelectedCompanyItems.Take(0);
            checkCompanyAll = false;
        }
        if (checkCarTypeAll == false && (SelectedCompanyItems.Contains("全て") || (!SelectedCompanyItems.Contains("全て") && SelectedCompanyItems.Count() == ListDataCompany.Count() - 1)) )
        {
            SelectedCompanyItems = ListDataCompany;
            checkCompanyAll = true;
        }
        if (checkCarTypeAll == true && SelectedCompanyItems.Contains("全て") && SelectedCompanyItems.Count() < ListDataCompany.Count())
        {
            SelectedCompanyItems = SelectedCompanyItems.Where(t => t != "全て");
            checkCompanyAll = false;
        }
        StateHasChanged();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await JSRuntime.InvokeVoidAsync("handleRadioPopupSignUp");
        await JSRuntime.InvokeVoidAsync("CustomInputFile");
        await JSRuntime.InvokeVoidAsync("closeDropdown");
        await JSRuntime.InvokeVoidAsync("scroll");
    }

    protected override void OnInitialized()
    {
        for (var i = 0; i < 200; i++)
        {
            DataSource.Add(new BusManagementData
            {
                Field1 = "貸切",
                SyaSyuCd = "002",
                SyaSyuNm = "ｽｰﾊﾟｰﾊｲﾃﾞｯｶｰ",
                EigyoCd = "00001",
                Field5_RyakuNm = "工房バス株式会社  関西支店",
                CompanyCd = "00001",
                Field7_RyakuNm = "工房バス株式",
                TenkoNo = "0000000001",
                SyaRyoCd = "00791",
                SyaRyoNm = "沖縄特別両行ー００１",
                StaYmd = "2000/06/01",
                EndYmd = "2030/06/01",
                KariSyaRyoNm = "沖縄特別両行ー００１",
                Kana = "ｽｰﾊﾟｰﾊｲﾃﾞｯｶｰ",
                TeiCnt = "50",
                HojoCnt = "03",
                Nenryo1_CodeKbnNm = "オイル",
                Nenryo2_CodeKbnNm = "軽油",
                Nenryo3_CodeKbnNm = "灯油",
                SyainCd = "0000050001",
                SyainNm = "工房ー高田　直成",
            });
        }
        ListDataCarType = new List<string>() {
            "全て",
            "001：ｽｰﾊﾟｰﾊｲﾃﾞｯｶｰ",
            "002：ｽｰﾊﾟｰﾊｲﾃﾞｯｶｰ",
        };
        SelectedCarTypeItems = ListDataCarType;

        ListDataVehicle = new List<string>() {
            "全て",
            "00001：エ001",
            "99999：エ99999",
        };
        ListDataCompany = new List<string>() {
            "全て",
            "00001：工房",
        };
        SelectedCompanyItems = ListDataCompany;

        ListDataBranch = new List<string>() {
            "全て",
            "00001：工房ASIA",
            "00009：工房　本社",
        };
        ListDataLayout = new List<string>() {
            "001：車両情報",
        };
        ListDataCompanyBranch = new List<string>() {
            "00001:工房1　00001：三共バス  予約",
            "00002:工房2　00001：三共バス  予約",
            "00003:工房　00001：三共バス  予約",
        };
        ListDataGarage = new List<string>() {
            "00001:車庫１",
            "00002:車庫２",
            "00003:車庫３",
        };
        ListDataBusiness = new List<string>() {
            "0：未指定",
            "1：貸切",
            "2：乗合",
        };
        ListDataPersonInCharge = new List<string>() {
            "0000050001：高田　直成",
        };
        ListDataFuel = new List<string>() {
            "01：軽油",
            "02：灯油",
            "03：オイル"
        };
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IJSRuntime JSRuntime { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IStringLocalizer<BusManagement> Lang { get; set; }
    }
}
#pragma warning restore 1591
