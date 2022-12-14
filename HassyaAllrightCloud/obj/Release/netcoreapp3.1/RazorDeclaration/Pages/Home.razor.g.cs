#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\Home.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0b6247574ab1fd171839a70769bd2e60526967c6"
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
#nullable restore
#line 8 "E:\Project\HassyaAllrightCloud\Pages\Home.razor"
using Blazor.DragDrop.Core;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "E:\Project\HassyaAllrightCloud\Pages\Home.razor"
using HassyaAllrightCloud.Pages.Components.Home;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/home")]
    public partial class Home : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 282 "E:\Project\HassyaAllrightCloud\Pages\Home.razor"
       

    #region Notify Variables
    IEnumerable<NoticeDisplayKbnDto> ListPeopleCanSee;
    NoticeDisplayKbnDto people;

    List<Tkd_NoticeListDto> notification;
    bool NotifySettingPopup { get; set; } = false;
    bool DeleteNotifyPopup { get; set; } = false;
    public int Id { get; set; }
    public NotificationPopup CreateForm { get; set; }
    string PopupNoficationTitle;
    #endregion Notify Variables

    #region Menu Variables
    public MenuPopup CreateMenuForm { get; set; }
    public SitePopup CreateSiteForm { get; set; }
    string PopupMenuTitle;
    string PopupSiteTitle;
    bool IsShowEditDeleteIconOfMenu = false;
    bool IsShowEditDeleteIconOfSite = false;
    public int MenuId { get; set; }
    public int SiteId { get; set; }
    public bool isOpeningCreateMenuPopup { get; set; }
    public bool isOpeningCreateSitePopup { get; set; }
    bool DeleteMenuPopup { get; set; } = false;
    bool DeleteSitePopup { get; set; } = false;
    public List<TKD_FavoriteMenuData> FavoriteMenuDataList = new List<TKD_FavoriteMenuData>();
    public List<TKD_FavoriteSiteData> FavoriteSiteDataList = new List<TKD_FavoriteSiteData>();
    #endregion Menu Variables

    #region Note Variables
    public NotePopup CreateNoteForm { get; set; }
    bool isOpeningCreateNotePopup = false;
    TKD_PersonalNoteData PersonalNoteData = new TKD_PersonalNoteData();
    #endregion Note Variables

    List<string[]> alarm = new List<string[]>();

    protected override void OnParametersSet()
    {
        JSRuntime.InvokeVoidAsync("loadPageScript", "homePage");
        base.OnParametersSet();
    }

    protected override async Task OnInitializedAsync()
    {
        notification = NoticeService.GetNoticeList().Result.ToList();
        alarm.Add(new string[] { "2???20???", "????????????" });
        alarm.Add(new string[] { "2???21???", "???????????????" });
        alarm.Add(new string[] { "2???21???", "????????????6???" });
        alarm.Add(new string[] { "1??????", "????????????48???" });
        people = ListPeopleCanSee?.FirstOrDefault();

        FavoriteSiteDataList = await TKD_FavoriteSiteService.GetFavoriteSiteList();
        FavoriteMenuDataList = await TKD_FavoriteMenuService.GetFavoriteMenuList();
        PersonalNoteData = await TKD_PersonalNoteDataService.Get();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            JSRuntime.InvokeVoidAsync("loadSortableJs", "favoriteMenuList", DotNetObjectReference.Create(this));
            JSRuntime.InvokeVoidAsync("loadSortableJs", "favoriteSiteList", DotNetObjectReference.Create(this));
        }
    }

    #region Notify
    protected void AddNotify()
    {
        Id = 0;
        NotifySettingPopup = true;
        ListPeopleCanSee = NoticeService.GetNoticeDisplayKbnList().Result;
        StateHasChanged();
    }

    protected void OpenEditNoticePopUp(int id)
    {
        Id = id;
        NotifySettingPopup = true;
        ListPeopleCanSee = NoticeService.GetNoticeDisplayKbnList().Result;
        StateHasChanged();
    }

    protected void OpenNoticeDeletePopup(int id)
    {
        Id = id;
        DeleteNotifyPopup = true;
    }

    protected async void DeleteNotice(int id)
    {
        Id = id;
        await NoticeService.DeleteNotice(id);
        DeleteNotifyPopup = false;
        ReloadNoticeData();
        StateHasChanged();
    }

    protected void ReloadNoticeData()
    {
        notification = NoticeService.GetNoticeList().Result.ToList();
    }

    protected async Task SaveNotice()
    {
        var result = await CreateForm.Save();
        if (result)
        {
            NotifySettingPopup = false;
            ReloadNoticeData();
        }
    }

    #endregion Notify

    #region Menu

    public void AddMenu()
    {
        MenuId = 0;
        isOpeningCreateMenuPopup = true;
        StateHasChanged();
    }

    public void OpenEditMenuPopup(int menuId)
    {
        MenuId = menuId;
        isOpeningCreateMenuPopup = true;
        StateHasChanged();
    }

    protected void OpenMenuDeletePopup(int id)
    {
        MenuId = id;
        DeleteMenuPopup = true;
    }

    public async void DeleteMenu(int menuId)
    {
        MenuId = menuId;
        await TKD_FavoriteMenuService.DeleteFavoriteMenu(menuId);
        DeleteMenuPopup = false;
        ReloadMenuData();
        StateHasChanged();
    }

    protected async Task SaveMenu()
    {
        var result = await CreateMenuForm.Save();
        if (result == null)
        {
            isOpeningCreateMenuPopup = false;
            ReloadMenuData();
        }
    }

    protected void ReloadMenuData()
    {
        FavoriteMenuDataList = TKD_FavoriteMenuService.GetFavoriteMenuList().Result;
    }

    private void ToggleShowEditDeleteIconOfMenu()
    {
        IsShowEditDeleteIconOfMenu = !IsShowEditDeleteIconOfMenu;
        StateHasChanged();
    }

    [JSInvokable]
    public async Task OnFavouriteMenuOrderChange(ListOrderDto[] orderedList, string elementId, int oldIndex, int newIndex)
    {
        if (elementId == "favoriteMenuList")
        {
            await TKD_FavoriteMenuService.SaveFavoriteMenuOrder(orderedList);
        }
        else
        {
            await TKD_FavoriteSiteService.SaveFavoriteSiteOrder(orderedList);
        }
    }

    #endregion Menu

    #region Site
    public void AddSite()
    {
        SiteId = 0;
        isOpeningCreateSitePopup = true;
        StateHasChanged();
    }

    public void OpenEditSitePopup(int siteId)
    {
        SiteId = siteId;
        isOpeningCreateSitePopup = true;
        StateHasChanged();
    }

    protected void OpenSiteDeletePopup(int id)
    {
        SiteId = id;
        DeleteSitePopup = true;
    }

    public async void DeleteSite(int siteId)
    {
        SiteId = siteId;
        await TKD_FavoriteSiteService.DeleteFavoriteSite(siteId);
        DeleteSitePopup = false;
        ReloadSiteData();
        StateHasChanged();
    }

    protected async Task SaveSite()
    {
        var result = await CreateSiteForm.Save();
        if (result == null)
        {
            isOpeningCreateSitePopup = false;
            ReloadSiteData();
        }
    }

    protected void ReloadSiteData()
    {
        FavoriteSiteDataList = TKD_FavoriteSiteService.GetFavoriteSiteList().Result;
    }

    private void ToggleShowEditDeleteIconOfSite()
    {
        IsShowEditDeleteIconOfSite = !IsShowEditDeleteIconOfSite;
        StateHasChanged();
    }

    #endregion Site

    #region Note
    public void OpenEditNotePopup()
    {
        isOpeningCreateNotePopup = true;
        StateHasChanged();
    }

    protected async Task SaveNote()
    {
        var result = await CreateNoteForm.Save();
        if (result == null)
        {
            isOpeningCreateNotePopup = false;
            ReloadNoteData();
        }
    }

    protected void ReloadNoteData()
    {
        PersonalNoteData = TKD_PersonalNoteDataService.Get().Result;
    }
    #endregion Note

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IStringLocalizer<Home> Lang { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IJSRuntime JSRuntime { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private INoticeService NoticeService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ITKD_PersonalNoteDataService TKD_PersonalNoteDataService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ITKD_FavoriteSiteService TKD_FavoriteSiteService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ITKD_FavoriteMenuService TKD_FavoriteMenuService { get; set; }
    }
}
#pragma warning restore 1591
