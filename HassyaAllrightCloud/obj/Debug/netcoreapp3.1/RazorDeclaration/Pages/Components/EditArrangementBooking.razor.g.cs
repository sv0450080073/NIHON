#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\Components\EditArrangementBooking.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "bb51884139954175d5225a20c83fb71b656301fd"
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
    public partial class EditArrangementBooking : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 167 "E:\Project\HassyaAllrightCloud\Pages\Components\EditArrangementBooking.razor"
       
    [Parameter] public BookingArrangementData ArrangementOrigin { get; set; }
    [Parameter] public bool IsEditMode { get; set; }
    [Parameter] public List<ArrangementType> ArrangementTypeList { get; set; }
    [Parameter] public List<ArrangementCode> ArrangementCodeList { get; set; }
    [Parameter] public List<ArrangementPlaceType> ArrangementPlaceTypeList { get; set; }
    [Parameter] public List<ArrangementLocation> ArrangementLocationList { get; set; }
    [Parameter] public List<ScheduleSelectorModel> ScheduleList { get; set; }
    protected IEnumerable<ArrangementLocation> FilteredArrangementLocationList { get; set; }
    [Parameter] public EventCallback<int> OnSubmit { get; set; }
    [Parameter] public EventCallback<int> OnCancel { get; set; }
    protected EditContext FormContext { get; set; }
    protected BookingArrangementData ArrangementModel { get; set; }
    Dictionary<string, string> LangDic = new Dictionary<string, string>();
    Dictionary<string, string> emptyItemMessage = new Dictionary<string, string>();
    string EmptyArrangementTypeListMessage = "";
    string EmptyArrangementCodeListMessage = "";
    string EmptyArrangementPlaceTypeListMessage = "";
    string EmptyFilteredArrangementLocationListMessage = "";
    protected bool IsEmptyFilteredArrangementLocationList = false;
    protected bool IsEmptyArrangementTypeList
    {
        get
        {
            return ArrangementTypeList is null || ArrangementTypeList.Count == 0;
        }
    }
    protected bool IsEmptyArrangementCodeList
    {
        get
        {
            return ArrangementCodeList is null || ArrangementCodeList.Count == 0;
        }
    }
    protected bool IsEmptyArrangementPlaceTypeList
    {
        get
        {
            return ArrangementPlaceTypeList is null || ArrangementPlaceTypeList.Count == 0;
        }
    }
    protected bool isServiceNull = false;
    private void LocalizationInit()
    {
        var dataLang = Lang.GetAllStrings();
        LangDic = dataLang.ToDictionary(l => l.Name, l => l.Value);
        EmptyArrangementTypeListMessage = Lang["BI_T004"];
        EmptyArrangementCodeListMessage = Lang["BI_T005"];
        EmptyArrangementPlaceTypeListMessage = Lang["BI_T006"];
        EmptyFilteredArrangementLocationListMessage = Lang["BI_T007"];
    }
    protected override void OnInitialized()
    {
        LocalizationInit();
        if (IsEditMode)
        {
            var json = JsonConvert.SerializeObject(ArrangementOrigin);
            ArrangementModel = JsonConvert.DeserializeObject<BookingArrangementData>(json);
            LoadSelectedDataCombobox(ArrangementModel);
        }
        else
        {
            ArrangementModel = ArrangementOrigin;
        }
        if (IsEmptyArrangementTypeList)
        {
            emptyItemMessage[nameof(EmptyArrangementTypeListMessage)] = EmptyArrangementTypeListMessage;
        }
        if (IsEmptyArrangementCodeList)
        {
            emptyItemMessage[nameof(EmptyArrangementCodeListMessage)] = EmptyArrangementCodeListMessage;
        }
        if (IsEmptyArrangementPlaceTypeList)
        {
            emptyItemMessage[nameof(EmptyArrangementPlaceTypeListMessage)] = EmptyArrangementPlaceTypeListMessage;
        }
        FilteredArrangementLocationList = ArrangementLocationList;
        if (FilteredArrangementLocationList is null || FilteredArrangementLocationList.Count() == 0)
        {
            IsEmptyFilteredArrangementLocationList = true;
            emptyItemMessage[nameof(EmptyFilteredArrangementLocationListMessage)] = EmptyFilteredArrangementLocationListMessage;
        }
        if(IsEmptyArrangementTypeList || IsEmptyArrangementCodeList  || IsEmptyArrangementPlaceTypeList || IsEmptyFilteredArrangementLocationList)
        {
            isServiceNull = true;
        }
        FormContext = new EditContext(ArrangementModel);
    }

    protected override void OnAfterRender(bool firstRender)
    {
        JSRuntime.InvokeVoidAsync("setEventforCurrencyField");
    }

    private void LoadSelectedDataCombobox(BookingArrangementData arrangementData)
    {
        if (ArrangementTypeList != null && arrangementData.SelectedArrangementType != null)
        {
            int code = arrangementData.SelectedArrangementType.TypeCode;
            arrangementData.SelectedArrangementType = ArrangementTypeList.SingleOrDefault(t => t.TypeCode == code);
        }
        if (ArrangementLocationList != null && arrangementData.SelectedArrangementLocation != null)
        {
            int code = arrangementData.SelectedArrangementLocation.BasyoMapCdSeq;
            arrangementData.SelectedArrangementLocation = ArrangementLocationList.SingleOrDefault(t => t.BasyoMapCdSeq == code);
        }
        if (ScheduleList != null && arrangementData.Schedule != null)
        {
            arrangementData.Schedule =
                ScheduleList.SingleOrDefault(s => s.Date == arrangementData.Schedule.Date) ?? arrangementData.Schedule;
        }
        if (ArrangementPlaceTypeList != null && arrangementData.SelectedArrangementPlaceType != null)
        {
            arrangementData.SelectedArrangementPlaceType =
            ArrangementPlaceTypeList.SingleOrDefault(s => s.CodeKbnSeq == arrangementData.SelectedArrangementPlaceType.CodeKbnSeq) ?? ArrangementPlaceTypeList.FirstOrDefault();
        }
        if (ArrangementCodeList != null && arrangementData.SelectedArrangementCode != null)
        {
            arrangementData.SelectedArrangementCode =
            ArrangementCodeList.SingleOrDefault(s => s.CodeKbnSeq == arrangementData.SelectedArrangementCode.CodeKbnSeq) ?? ArrangementCodeList.FirstOrDefault();
        }
    }

    protected bool IsEnableSaveButton()
    {
        return FormContext.IsModified() && FormContext.Validate() && !isServiceNull;
    }

    protected IEnumerable<ArrangementLocation> GetFilteredArrangementLocationList(BookingArrangementData arrangementData)
    {
        IEnumerable<ArrangementLocation> filteredList = ArrangementLocationList;
        if (arrangementData.SelectedArrangementCode != null && arrangementData.SelectedArrangementCode.CodeKbnSeq != 0)
        {
            filteredList = filteredList.Where(l => l.BasyoKenCdSeq == arrangementData.SelectedArrangementCode.CodeKbnSeq);
        }
        if (arrangementData.SelectedArrangementPlaceType != null && arrangementData.SelectedArrangementPlaceType.CodeKbnSeq != 0)
        {
            filteredList = filteredList.Where(l => l.BunruiCdSeq == arrangementData.SelectedArrangementPlaceType.CodeKbnSeq);
        }
        HandleSelectedArrangementLocationChanged(filteredList.FirstOrDefault());
        return filteredList;
    }

    #region Handle Change Value

    protected void HandleLocationNameChanged(string newValue)
    {
        ArrangementModel.LocationName = newValue;
        StateHasChanged();
    }

    protected void HandleAddress1Changed(string newValue)
    {
        ArrangementModel.Address1 = newValue;
        StateHasChanged();
    }

    protected void HandleAddress2Changed(string newValue)
    {
        ArrangementModel.Address2 = newValue;
        StateHasChanged();
    }

    protected void HandleTelChanged(string newValue)
    {
        ArrangementModel.Tel = newValue;
        StateHasChanged();
    }

    protected void HandleFaxChanged(string newValue)
    {
        ArrangementModel.Fax = newValue;
        StateHasChanged();
    }

    protected void HandleInchargeStaffChanged(string newValue)
    {
        ArrangementModel.InchargeStaff = newValue;
        StateHasChanged();
    }

    protected void HandleCommentChanged(string newValue)
    {
        ArrangementModel.Comment = newValue;
        StateHasChanged();
    }

    protected void HandleArriveTimeChanged(BookingInputHelper.MyTime newValue)
    {
        StateHasChanged();
    }

    protected void HandleDepartureTimeChanged(BookingInputHelper.MyTime newValue)
    {
        StateHasChanged();
    }

    protected void HandleSelectedScheduleChanged(ScheduleSelectorModel newValue)
    {
        ArrangementModel.Schedule = newValue;
        StateHasChanged();
    }

    protected void HandleSelectedArrangementCodeChanged(ArrangementCode newValue)
    {
        ArrangementModel.SelectedArrangementCode = newValue;
        FilteredArrangementLocationList = GetFilteredArrangementLocationList(ArrangementModel);
        StateHasChanged();
    }

    protected void HandleSelectedArrangementPlaceTypeChanged(ArrangementPlaceType newValue)
    {
        ArrangementModel.SelectedArrangementPlaceType = newValue;
        FilteredArrangementLocationList = GetFilteredArrangementLocationList(ArrangementModel);
        StateHasChanged();
    }

    protected void HandleSelectedArrangementLocationChanged(ArrangementLocation newValue)
    {
        ArrangementModel.SelectedArrangementLocation = newValue;
        ArrangementModel.LocationName = newValue?.BasyoNm ?? string.Empty;
        StateHasChanged();
    }

    protected void HandleSelectedArrangementTypeChanged(ArrangementType newValue)
    {
        ArrangementModel.SelectedArrangementType = newValue;
        StateHasChanged();
    }

    #endregion

    protected void HandleAddFuttum()
    {
        if (IsEditMode)
        {
            ArrangementOrigin.SimpleCloneProperties(ArrangementModel);
        }
        OnSubmit.InvokeAsync(ArrangementOrigin.No);
    }

    protected void HandleCancelFuttum()
    {
        OnCancel.InvokeAsync(ArrangementOrigin.No);
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IJSRuntime JSRuntime { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private CustomHttpClient HttpClient { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private AppSettingsService AppSettingsService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IStringLocalizer<Tehai> Lang { get; set; }
    }
}
#pragma warning restore 1591
