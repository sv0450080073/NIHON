#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\Components\YatoiSha.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5e97d4570e69db086d2a2427d69101918f27f659"
// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace HassyaAllrightCloud.Pages.Components
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
#line 5 "E:\Project\HassyaAllrightCloud\Pages\Components\YatoiSha.razor"
using HassyaAllrightCloud.Pages.Components.Popup;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "E:\Project\HassyaAllrightCloud\Pages\Components\YatoiSha.razor"
using HassyaAllrightCloud.Commons.Extensions;

#line default
#line hidden
#nullable disable
    public partial class YatoiSha : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 205 "E:\Project\HassyaAllrightCloud\Pages\Components\YatoiSha.razor"
       
    [Parameter] public IncidentalViewMode CurrentViewMode { get; set; }
    [Parameter] public string UkeNo { get; set; }
    [Parameter] public short UnkRen { get; set; }
    [Parameter] public int YouTblSeq { get; set; }

    protected List<ScheduleSelectorModel> FutaiScheduleList { get; set; }
    protected List<LoadYFutai> LoadYFutaiList { get; set; }
    protected List<LoadYTsumi> LoadYTsumiList { get; set; }
    protected List<LoadYRyokin> LoadYRyokinList { get; set; }
    protected List<LoadYSeisan> LoadYSeisanList { get; set; }
    protected List<YouShaSaveType> SaveTypeList { get; set; }
    protected List<TaxTypeList> TaxTypeDataList { get; set; }
    protected SettingTaxRate SettingTaxRate { get; set; }

    protected LoanBookingIncidentalData LoanBookingIncidental { get; set; }
    protected List<LoadYFutTu> YFutTuListNotDeleted { get => LoanBookingIncidental.LoadYFutTuList.Where(f => f.EditState != FormEditState.Deleted).ToList(); }
    protected LoadYFutTu SelectedLoadYFutTuItem { get; set; }
    protected EditContext FormContext { get; set; }

    protected string TempStr { get; set; }

    protected bool IsLoading { get; set; } = true;
    protected bool ShowFormCreateFuttum { get; set; }
    protected bool IsDisableEdit { get; set; }

    protected CultureInfo CurrentCulture { get; set; }
    protected MyPopupModel MyPopup { get; set; }

    private int youFutRenToDelete;
    private string baseUrl;

    #region Localization string
    string Ok;
    string Yes;
    string No;
    string PopupTitleInfo;
    string PopupTitleError;
    string UpdateFuttumSucces;
    string PopupConfirmDeleteTitle;
    string PopupConfirmDeleteContent;
    string UpdateFail;
    string PopupConfirmSaveMishumFuttumContent;
    private void LocalizationInit()
    {
        Ok = Lang["Ok"];
        Yes = Lang["Yes"];
        No = Lang["No"];
        PopupTitleInfo = Lang["PopupTitleInfo"];
        PopupTitleError = Lang["PopupTitleError"];

        PopupConfirmDeleteTitle = Lang["VerificationDelete"];
        PopupConfirmDeleteContent = Lang["BI_T004"];
        PopupConfirmSaveMishumFuttumContent = Lang["BI_T003"];

        UpdateFuttumSucces = Lang["UpdateSuccess"];
        UpdateFail = Lang["UpdateFail"];
    }
    #endregion

    #region Component Lifecycle

    protected override async Task OnInitializedAsync()
    {
        LocalizationInit();
        MyPopup = new MyPopupModel();
        baseUrl = AppSettingsService.GetBaseUrl();
        CurrentCulture = new CultureInfo("ja-JP");
        SelectedLoadYFutTuItem = new LoadYFutTu();
        SelectedLoadYFutTuItem.FirstLoad = false;

        await LoadAllDataComboboxAsync();
        var loadnIncidentalDb = await LoadLoanBookingIncidentalDataAsync();
        if (loadnIncidentalDb != null)
        {
            LoanBookingIncidental = loadnIncidentalDb;
            FutaiScheduleList = GetFutaiScheduleList(LoanBookingIncidental);
            LoadSelectedDataCombobox();

            // enable auto calculate
            LoanBookingIncidental.LoadYFutTuList.ForEach(f => f.FirstLoad = false);

            FormContext = new EditContext(LoanBookingIncidental);
        }
        else
        {
            NavManager.NavigateTo(string.Format("/partnerbookinginput?UkeCd={0}&UnkRen={1}", UkeNo.Substring(5), UnkRen), true);
            return;
        }

        await base.OnInitializedAsync().ContinueWith((t) => { IsLoading = false; });
        await InvokeAsync(StateHasChanged);
    }

    #endregion

    #region Common methods
    private async Task LoadAllDataComboboxAsync()
    {
        if (CurrentViewMode == IncidentalViewMode.Futai)
        {
            LoadYFutaiList = await HttpClient.GetJsonAsync<List<LoadYFutai>>(baseUrl + "/api/LoanBookingIncidental/LoadYFutai?tenantId=" + new ClaimModel().TenantID);
        }
        else if (CurrentViewMode == IncidentalViewMode.Tsumi)
        {
            LoadYTsumiList = await HttpClient.GetJsonAsync<List<LoadYTsumi>>(baseUrl + "/api/LoanBookingIncidental/LoadYTsumi");
        }
        LoadYRyokinList = await HttpClient.GetJsonAsync<List<LoadYRyokin>>(baseUrl + "/api/LoanBookingIncidental/LoadYRyokin");
        LoadYSeisanList = await HttpClient.GetJsonAsync<List<LoadYSeisan>>(baseUrl + "/api/LoanBookingIncidental/LoadYSeisan?tenantId=" + new ClaimModel().TenantID);
        SaveTypeList = LoadYSeisanList.Select(c => new YouShaSaveType() { Id = c.SeisanKbn, Name = c.CodeKbRyakuNm }).GroupBy(c => c.Id).Select(c => c.First()).ToList();
        TaxTypeDataList = await HttpClient.GetJsonAsync<List<TaxTypeList>>(baseUrl + "/api/LoanBookingIncidental/TaxTypeList?tenantId=" + new ClaimModel().TenantID);
        SettingTaxRate = await HttpClient.GetJsonAsync<SettingTaxRate>(baseUrl + "/api/BookingIncidental/SettingTaxRate");
    }

    private List<ScheduleSelectorModel> GetFutaiScheduleList(LoanBookingIncidentalData incidental)
    {
        return ScheduleHelper.GetScheduleSelectorList(incidental.HaiSYmd, incidental.TouYmd, incidental.IsPreviousDay, incidental.IsAfterDay);
    }

    private void LoadSelectedDataCombobox()
    {
        if (LoadYSeisanList != null)
        {
            for (int youFutRen = 0; youFutRen < LoanBookingIncidental.LoadYFutTuList.Count; youFutRen++)
            {
                var yfuttu = LoanBookingIncidental.LoadYFutTuList[youFutRen];
                yfuttu.SelectedLoadYSeisan =
                    LoadYSeisanList.SingleOrDefault(s => s.SeisanCdSeq == yfuttu.SelectedLoadYSeisan.SeisanCdSeq);
            }
        }
        if (CurrentViewMode == IncidentalViewMode.Futai && LoadYFutaiList != null)
        {
            for (int youFutRen = 0; youFutRen < LoanBookingIncidental.LoadYFutTuList.Count; youFutRen++)
            {
                var yfuttu = LoanBookingIncidental.LoadYFutTuList[youFutRen];
                yfuttu.SelectedLoadYFutai =
                    LoadYFutaiList.SingleOrDefault(s => s.FutaiCdSeq == yfuttu.SelectedLoadYFutai.FutaiCdSeq);
            }
        }
        else if (CurrentViewMode == IncidentalViewMode.Tsumi && LoadYTsumiList != null)
        {
            for (int youFutRen = 0; youFutRen < LoanBookingIncidental.LoadYFutTuList.Count; youFutRen++)
            {
                var yfuttu = LoanBookingIncidental.LoadYFutTuList[youFutRen];
                yfuttu.SelectedLoadYTsumi =
                    LoadYTsumiList.SingleOrDefault(s => s.CodeKbnSeq == yfuttu.SelectedLoadYTsumi.CodeKbnSeq);
            }
        }
        if (LoadYRyokinList != null)
        {
            for (int youFutRen = 0; youFutRen < LoanBookingIncidental.LoadYFutTuList.Count; youFutRen++)
            {
                var yfuttu = LoanBookingIncidental.LoadYFutTuList[youFutRen];
                yfuttu.SelectedLoadYRyoKin =
                    LoadYRyokinList.SingleOrDefault(s => s.RyoKinTikuCd == yfuttu.SelectedLoadYRyoKin.RyoKinTikuCd
                                                    && s.RyoKinCd == yfuttu.SelectedLoadYRyoKin.RyoKinCd);
                yfuttu.SelectedLoadYShuRyoKin =
                    LoadYRyokinList.SingleOrDefault(s => s.RyoKinTikuCd == yfuttu.SelectedLoadYShuRyoKin.RyoKinTikuCd && s.RyoKinCd == yfuttu.SelectedLoadYShuRyoKin.RyoKinCd);
            }
        }
        if (SaveTypeList != null)
        {
            for (int youFutRen = 0; youFutRen < LoanBookingIncidental.LoadYFutTuList.Count; youFutRen++)
            {
                var yfuttu = LoanBookingIncidental.LoadYFutTuList[youFutRen];
                yfuttu.SaveType = SaveTypeList.SingleOrDefault(i => i.Id == yfuttu.SaveType.Id);
            }
        }
        if (TaxTypeDataList != null)
        {
            for (int youFutRen = 0; youFutRen < LoanBookingIncidental.LoadYFutTuList.Count; youFutRen++)
            {
                var yfuttu = LoanBookingIncidental.LoadYFutTuList[youFutRen];
                yfuttu.TaxType =
                    TaxTypeDataList.SingleOrDefault(s => s.IdValue == yfuttu.TaxType.IdValue);
            }
        }
        if (FutaiScheduleList != null)
        {
            for (int youFutRen = 0; youFutRen < LoanBookingIncidental.LoadYFutTuList.Count; youFutRen++)
            {
                var yfuttu = LoanBookingIncidental.LoadYFutTuList[youFutRen];
                yfuttu.ScheduleDate =
                    FutaiScheduleList.SingleOrDefault(s => s.Date == yfuttu.ScheduleDate.Date) ?? yfuttu.ScheduleDate;
            }
        }
    }

    protected bool IsEnableSubmitButton()
    {
        if (IsDisableEdit)
        {
            return false;
        }
        if (ShowFormCreateFuttum || LoanBookingIncidental.LoadYFutTuList.Any(f => f.Editing))
        {
            return false;
        }
        var result = FormContext.IsModified() && FormContext.Validate();
        return result;
    }

    private void HandleClosePopup()
    {
        MyPopup.Hide();
        StateHasChanged();
    }

    private async Task<LoanBookingIncidentalData> LoadLoanBookingIncidentalDataAsync()
    {
        try
        {
            string getLoanIncidentalUrl = string.Format("{0}/api/LoanBookingIncidental?tenantId={1}&ukeNo={2}&unkRen={3}&youTblSeq={4}&viewMode={5}"
                , baseUrl, new ClaimModel().TenantID, UkeNo, UnkRen, YouTblSeq, CurrentViewMode);
            var response = await HttpClient.GetAsync(getLoanIncidentalUrl);
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<LoanBookingIncidentalData>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return null;
            }
        }
        catch (Exception)
        {
            return null;
        }
    }

    private async Task<List<BookingDisableEditState>> LoadBookingEditableStateAsync(string ukeNo)
    {
        try
        {
            string uri = string.Format("{0}/api/BookingInput/DisabledBookingStateList?ukeNo={1}", baseUrl, ukeNo);
            var response = await HttpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<List<BookingDisableEditState>>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return null;
            }
        }
        catch (Exception)
        {
            return null;
        }
    }

    #endregion

    #region Popup methods

    private void ShowPopupSaveSuccess()
    {
        MyPopup.Build()
            .WithTitle(PopupTitleInfo)
            .WithBody(UpdateFuttumSucces)
            .WithIcon(MyPopupIconType.Info)
            .AddButton(new MyPopupFooterButton(Ok, HandleClosePopup))
            .Show();
    }

    private void ShowPopupSaveError()
    {
        MyPopup.Build()
            .WithTitle(PopupTitleInfo)
            .WithBody(UpdateFail)
            .WithIcon(MyPopupIconType.Error)
            .AddButton(new MyPopupFooterButton(Ok, HandleClosePopup))
            .Show();
    }

    private void ShowPopupConfirmDelete()
    {
        MyPopup.Build()
            .WithTitle(PopupConfirmDeleteTitle)
            .WithBody(PopupConfirmDeleteContent)
            .WithIcon(MyPopupIconType.Warning)
            .AddButton(new MyPopupFooterButton(Yes, ButtonRenderStyle.Danger, HandleConfirmDelete))
            .AddButton(new MyPopupFooterButton(No, ButtonRenderStyle.Secondary, HandleClosePopup))
            .Show();
    }

    #endregion

    #region Handle event

    protected void HandleCreateFuttum()
    {
        if (IsDisableEdit == false && ShowFormCreateFuttum == false)
        {
            LoanBookingIncidental.LoadYFutTuList.ForEach(f => f.Editing = false);

            SelectedLoadYFutTuItem = new LoadYFutTu();
            SelectedLoadYFutTuItem.FirstLoad = false;
            SelectedLoadYFutTuItem.YouFutTumRen = ++LoanBookingIncidental.YouFutTumRenMax;
            SelectedLoadYFutTuItem.FuttumKbnMode = CurrentViewMode;

            SelectedLoadYFutTuItem.TaxType = TaxTypeDataList.SingleOrDefault(t => t.IdValue == Constants.ForeignTax.IdValue);

            SelectedLoadYFutTuItem.ScheduleDate = FutaiScheduleList.FirstOrDefault();
            SelectedLoadYFutTuItem.SettingQuantityList = LoanBookingIncidental.SettingQuantityList
                .Where(s => SelectedLoadYFutTuItem.ScheduleDate.Date.IsInRange(s.GarageLeaveDate, s.GarageReturnDate))
                .Select(setting => CommonHelper.SimpleCloneModel<SettingQuantity>(setting))
                .ToList();

            SelectedLoadYFutTuItem.DefaultTaxRate = SettingTaxRate.GetTaxRate(LoanBookingIncidental.HaiSYmd);
            SelectedLoadYFutTuItem.Zeiritsu = SelectedLoadYFutTuItem.DefaultTaxRate.ToString();
            SelectedLoadYFutTuItem.TesuRitu = LoanBookingIncidental.DefaultFutaiChargeRate.ToString();
            SelectedLoadYFutTuItem.RoundType = LoanBookingIncidental.RoundType;
            SelectedLoadYFutTuItem.SaveType = SaveTypeList.FirstOrDefault(
                i => i.Id == (CurrentViewMode == IncidentalViewMode.Futai ? 1 : 2)
            );

            ShowFormCreateFuttum = true;
        }
    }

    protected void HandleAddFuttum()
    {
        LoanBookingIncidental.LoadYFutTuList.Add(SelectedLoadYFutTuItem);
        SelectedLoadYFutTuItem.EditState = FormEditState.Added;
        FormContext.NotifyFieldChanged(() => LoanBookingIncidental.LoadYFutTuList);
        ShowFormCreateFuttum = false;
        StateHasChanged();
    }

    protected void HandleCancelCreateFuttum(int youFutRen)
    {
        ShowFormCreateFuttum = false;
        StateHasChanged();
    }

    protected void HandleEditFuttum(int youFutRen)
    {
        if (IsDisableEdit == false)
        {
            LoanBookingIncidental.LoadYFutTuList.ForEach(f => f.Editing = false);
            HandleCancelCreateFuttum(youFutRen);

            var selectedFuttum = LoanBookingIncidental.LoadYFutTuList.SingleOrDefault(f => f.YouFutTumRen == youFutRen);
            selectedFuttum.DefaultTaxRate = SettingTaxRate.GetTaxRate(LoanBookingIncidental.HaiSYmd);
            selectedFuttum.Editing = true;
            StateHasChanged();
        }
    }

    protected void HandleSaveEditFuttum(int youFutRen)
    {
        var selectedFuttum = LoanBookingIncidental.LoadYFutTuList.SingleOrDefault(f => f.YouFutTumRen == youFutRen);
        selectedFuttum.Editing = false;
        selectedFuttum.EditState = selectedFuttum.EditState == FormEditState.Added ? FormEditState.Added : FormEditState.Edited;
        FormContext.NotifyFieldChanged(() => LoanBookingIncidental.LoadYFutTuList);
        StateHasChanged();
    }

    protected void HandleCancelEditFuttum(int youFutRen)
    {
        var selectedFuttum = LoanBookingIncidental.LoadYFutTuList.SingleOrDefault(f => f.YouFutTumRen == youFutRen);
        selectedFuttum.Editing = false;
        StateHasChanged();
    }

    protected void HandleDeleteFuttum(int youFutRen)
    {
        if (IsDisableEdit == false)
        {
            var selectedFuttum = LoanBookingIncidental.LoadYFutTuList.SingleOrDefault(f => f.YouFutTumRen == youFutRenToDelete);
            youFutRenToDelete = youFutRen;
            ShowPopupConfirmDelete();
            StateHasChanged();
        }
    }

    protected void HandleConfirmDelete()
    {
        var selectedFuttum = LoanBookingIncidental.LoadYFutTuList.SingleOrDefault(f => f.YouFutTumRen == youFutRenToDelete);

        // check if create new then delete
        if (selectedFuttum.EditState == FormEditState.Added)
        {
            LoanBookingIncidental.YouFutTumRenMax -= 1;
            LoanBookingIncidental.LoadYFutTuList.Remove(selectedFuttum);
        }
        else
        {
            selectedFuttum.EditState = FormEditState.Deleted;
        }

        FormContext.NotifyFieldChanged(() => LoanBookingIncidental.LoadYFutTuList);
        MyPopup.Hide();
        StateHasChanged();
    }

    protected void HandleConfirmSaveMishumFuttum()
    {
        LoanBookingIncidental.IsSaveMishumFuttum = true;
    }

    protected async Task HandleSaveData()
    {
        HttpResponseMessage response = new HttpResponseMessage();
        var requstUri = string.Format("{0}/api/LoanBookingIncidental", baseUrl);
        response = await HttpClient.PutJsonAsync(requstUri, LoanBookingIncidental);

        if (response.IsSuccessStatusCode)
        {
            ShowPopupSaveSuccess();
        }
        else
        {
            ShowPopupSaveError();
        }
        var loadnIncidentalDb = await LoadLoanBookingIncidentalDataAsync();
        if (loadnIncidentalDb != null)
        {
            LoanBookingIncidental = loadnIncidentalDb;
            FutaiScheduleList = GetFutaiScheduleList(LoanBookingIncidental);
            LoadSelectedDataCombobox();

            // enable auto calculate
            LoanBookingIncidental.LoadYFutTuList.ForEach(f => f.FirstLoad = false);

            FormContext = new EditContext(LoanBookingIncidental);
        }
        else
        {
            NavManager.NavigateTo(string.Format("/partnerbookinginput?UkeCd={0}&UnkRen={1}", UkeNo.Substring(5), UnkRen), true);
        }
        await InvokeAsync(StateHasChanged);
    }

    protected void HandleValidSubmit()
    {
        if (FormContext.Validate())
        {
            MyPopup.Build()
                .WithTitle(PopupTitleInfo)
                .WithBody(PopupConfirmSaveMishumFuttumContent)
                .WithIcon(MyPopupIconType.Info)
                .AddButton(new MyPopupFooterButton(Yes, ButtonRenderStyle.Danger, async () =>
                {
                    LoanBookingIncidental.IsSaveMishumFuttum = true;
                    await HandleSaveData();
                }))
                .AddButton(new MyPopupFooterButton(No, ButtonRenderStyle.Secondary, async () => await HandleSaveData()))
                .Show();
        }
    }

    #endregion

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private CustomNavigation NavManager { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private CustomHttpClient HttpClient { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private AppSettingsService AppSettingsService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IStringLocalizer<YatoiSha> Lang { get; set; }
    }
}
#pragma warning restore 1591
