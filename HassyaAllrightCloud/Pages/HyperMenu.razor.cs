using HassyaAllrightCloud.Infrastructure.Services;
using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using HassyaAllrightCloud.Pages.Components.CommonComponents;
using HassyaAllrightCloud.IService.CommonComponents;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using System.Data;
using System.Text;
using Microsoft.JSInterop;
using Microsoft.Extensions.Localization;
using BlazorContextMenu;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Pages.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Forms;
using System.IO;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Commons.Constants;

namespace HassyaAllrightCloud.Pages
{
    public class HyperMenuBase : ComponentBase
    {
        #region Inject
        [Inject] protected CustomHttpClient Http { get; set; }
        [Inject] protected ITPM_YoyKbnDataListService TPM_YoyKbnDataService { get; set; }
        [Inject] protected ISaleBranchListService TpmEigyosService { get; set; }
        [Inject] protected IStaffListService SyainService { get; set; }
        [Inject] protected ICustomerListService CustomerService { get; set; }
        [Inject] protected ITPM_CodeKbListService CodeKbService { get; set; }
        [Inject] protected ICustomerCLassificationListService CustomerCLassificationService { get; set; }
        [Inject] protected ILocationListService LocationService { get; set; }
        [Inject] protected IDispatchListService DispatchService { get; set; }
        [Inject] protected IBusTypeListService BusTypeService { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected CustomNavigation NavigationManager { get; set; }
        [Inject] protected AppSettingsService AppSettingsService { get; set; }
        [Inject] protected IFilterCondition FilterConditionService { get; set; }
        [Inject] protected IGenerateFilterValueDictionary GenerateFilterValueDictionaryService { get; set; }
        [Inject] protected IGetFilterDataService GetFilterDataService { get; set; }
        [Inject] protected IErrorHandlerService errorModalService { get; set; }
        [Inject] protected ICustomerComponentService _service { get; set; }
        [Inject] protected IReservationClassComponentService _yoyakuservice { get; set; }
        [Inject] protected IHyperDataService HyperDataService { get; set; }
        [Inject] protected IStringLocalizer<HyperMenu> Lang { get; set; }
        #endregion
        [Parameter] public string HaishaBiFrom { get; set; }
        [Parameter] public string HaishaBiTo { get; set; }
        [Parameter] public string TochakuBiFrom { get; set; }
        [Parameter] public string TochakuBiTo { get; set; }
        [Parameter] public string YoyakuBiFrom { get; set; }
        [Parameter] public string YoyakuBiTo { get; set; }
        [Parameter] public string UketsukeBangoFrom { get; set; }
        [Parameter] public string UketsukeBangoTo { get; set; }
        [Parameter] public string YoyakuFrom { get; set; }
        [Parameter] public string YoyakuTo { get; set; }
        [Parameter] public string EigyoTantoShaFrom { get; set; }
        [Parameter] public string EigyoTantoShaTo { get; set; }
        [Parameter] public string UketsukeEigyoJoFrom { get; set; }
        [Parameter] public string UketsukeEigyoJoTo { get; set; }
        [Parameter] public string NyuryokuTantoShaFrom { get; set; }
        [Parameter] public string NyuryokuTantoShaTo { get; set; }
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
        [Parameter] public string DantaiNm { get; set; }
        [Parameter] public string MaxMinSetting { get; set; }
        [Parameter] public string ReservationStatus { get; set; }
        [Parameter] public string GyosyaTokuiSakiFrom { get; set; }
        [Parameter] public string GyosyaTokuiSakiTo { get; set; }
        [Parameter] public string TokiskTokuiSakiFrom { get; set; }
        [Parameter] public string TokiskTokuiSakiTo { get; set; }
        [Parameter] public string TokiStTokuiSakiFrom { get; set; }
        [Parameter] public string TokiStTokuiSakiTo { get; set; }
        [Parameter] public string GyosyaShiireSakiFrom { get; set; }
        [Parameter] public string GyosyaShiireSakiTo { get; set; }
        [Parameter] public string TokiskShiireSakiFrom { get; set; }
        [Parameter] public string TokiskShiireSakiTo { get; set; }
        [Parameter] public string TokiStShiireSakiFrom { get; set; }
        [Parameter] public string TokiStShiireSakiTo { get; set; }

        protected string dateFormat { get; set; } = "yyyy/MM/dd";
        public HyperFormData hyperData = new HyperFormData();
        protected List<ReservationData> BookingTypeList;
        protected List<SaleBranchData> SaleBranchList;
        protected List<StaffsData> StaffList;
        protected List<LoadCustomerList> CustomerList;
        protected List<CodeTypeData> CodeKbList;
        protected List<CustomerClassification> CustomerClassificationList;
        protected List<LoadLocation> DestinationList;
        protected List<LoadDispatchArea> DispatchList;
        protected List<LoadLocation> OriginList;
        protected List<LoadLocation> AreaList;
        protected List<BusTypesData> BusTypeList;
        protected List<VpmCodeKb> ConditionList;
        protected List<ComboboxFixField> MaxMinSettingList = new List<ComboboxFixField>();
        protected List<ComboboxFixField> ReservationStatusList = new List<ComboboxFixField>();
        public List<CustomerComponentGyosyaData> ListGyosya { get; set; } = new List<CustomerComponentGyosyaData>();
        public List<CustomerComponentTokiskData> TokiskData { get; set; } = new List<CustomerComponentTokiskData>();
        public List<CustomerComponentTokiStData> TokiStData { get; set; } = new List<CustomerComponentTokiStData>();
        public List<ReservationClassComponentData> ListReservationClass { get; set; } = new List<ReservationClassComponentData>();
        protected bool IsValid = true;
        protected bool IsInitForDate { get; set; } = false;
        protected bool IsClickNavigate { get; set; } = false;
        protected bool IsNoData { get; set; } = false;
        protected bool ShowErrorPopup { get; set; } = false;
        protected bool isLoading { get; set; } = true;
        protected string ErrorMessage { get; set; } = "";
        protected bool isInitComplete { get; set; } = false;
        protected bool isFirstInit { get; set; } = false;
        protected Dictionary<string, string> LangDic { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Load javascript of page
        /// </summary>
        protected override void OnParametersSet()
        {
            try
            {
                JSRuntime.InvokeVoidAsync("loadPageScript", "hyperMenuPage");
                base.OnParametersSet();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {

                // 受付営業所
                List<SaleBranchData> TempSaleBranchList = await TpmEigyosService.Get(new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID);
                SaleBranchList = TempSaleBranchList;
                SaleBranchList.Insert(0, null);

                // 営業担当, 入力担当
                List<StaffsData> TempStaffList = await SyainService.Get(new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID);
                StaffList = TempStaffList;
                StaffList.Insert(0, null);

                ListReservationClass = await _yoyakuservice.GetListReservationClass();

                // 得意先, 仕入先
                var taskTokisk = _service.GetListTokisk();
                var taskTokiSt = _service.GetListTokiSt();
                var taskGyosya = _service.GetListGyosya();

                await Task.WhenAll(taskGyosya, taskTokisk, taskTokiSt);

                ListGyosya = taskGyosya.Result;
                TokiskData = taskTokisk.Result;
                TokiStData = taskTokiSt.Result;

                // 団体区分
                List<CodeTypeData> TempCodeKbList = await CodeKbService.GetDantai(new ClaimModel().TenantID);
                CodeKbList = TempCodeKbList;
                CodeKbList.Insert(0, null);

                // 客種区分
                List<CustomerClassification> TempCustomerClassificationList = await CustomerCLassificationService.Get(new ClaimModel().TenantID);
                CustomerClassificationList = TempCustomerClassificationList;
                CustomerClassificationList.Insert(0, null);

                // 行先
                List<LoadLocation> TempDestinationList = await LocationService.GetDestination(new ClaimModel().TenantID);
                DestinationList = TempDestinationList;
                DestinationList.Insert(0, null);

                // 配車地
                List<LoadDispatchArea> TempDispatchList = await DispatchService.Get(new ClaimModel().TenantID);
                DispatchList = TempDispatchList;
                DispatchList.Insert(0, null);

                // 発生地
                List<LoadLocation> TempOriginList = await LocationService.GetOrigin(new ClaimModel().TenantID);
                OriginList = TempOriginList;
                OriginList.Insert(0, null);

                // エリア
                List<LoadLocation> TempAreaList = await LocationService.GetArea(new ClaimModel().TenantID);
                AreaList = TempAreaList;
                AreaList.Insert(0, null);

                // 車種
                List<BusTypesData> TempBusTypeList = await BusTypeService.GetAll(new ClaimModel().TenantID);
                BusTypeList = TempBusTypeList;
                BusTypeList.Insert(0, null);

                // 受付条件
                List<VpmCodeKb> TempConditionList = await CodeKbService.GetJoken(new ClaimModel().TenantID);
                ConditionList = TempConditionList;
                ConditionList.Insert(0, null);

                // 上限下限設定
                MaxMinSettingList.AddRange(new List<ComboboxFixField> {
                null,
                new ComboboxFixField() { IdValue = 0, StringValue = Lang["AlreadySet"]},
                new ComboboxFixField() { IdValue = 1, StringValue = Lang["NotYetSet"]}
            });

                // 予約ステータス
                ReservationStatusList.AddRange(new List<ComboboxFixField> {
                null,
                new ComboboxFixField() { IdValue = 0, StringValue = Lang["Reservation"]},
                new ComboboxFixField() { IdValue = 1, StringValue = Lang["Estimates"]}
            });

                var dataLang = Lang.GetAllStrings();
                LangDic = dataLang.ToDictionary(l => l.Name, l => l.Value);

                if (HaishaBiFrom != null || HaishaBiTo != null || TochakuBiFrom != null || TochakuBiTo != null || YoyakuBiFrom != null || YoyakuBiTo != null)
                {
                    hyperData = HyperFormData.ToObject(HaishaBiFrom, HaishaBiTo, TochakuBiFrom, TochakuBiTo, YoyakuBiFrom, YoyakuBiTo, UketsukeBangoFrom, UketsukeBangoTo,
                        YoyakuFrom, YoyakuTo, ListReservationClass,
                        EigyoTantoShaFrom, EigyoTantoShaTo, TempStaffList,
                        UketsukeEigyoJoFrom, UketsukeEigyoJoTo, TempSaleBranchList,
                        NyuryokuTantoShaFrom, NyuryokuTantoShaTo,
                        GyosyaTokuiSakiFrom, GyosyaTokuiSakiTo, ListGyosya,
                        TokiskTokuiSakiFrom, TokiskTokuiSakiTo, TokiskData,
                        TokiStTokuiSakiFrom, TokiStTokuiSakiTo, TokiStData,
                        GyosyaShiireSakiFrom, GyosyaShiireSakiTo,
                        TokiskShiireSakiFrom, TokiskShiireSakiTo,
                        TokiStShiireSakiFrom, TokiStShiireSakiTo,
                        DantaiKbnFrom, DantaiKbnTo, TempCodeKbList,
                        KyakuDaneKbnFrom, KyakuDaneKbnTo, TempCustomerClassificationList,
                        YukiSakiFrom, YukiSakiTo, TempDestinationList,
                        HaishaChiFrom, HaishaChiTo, TempDispatchList,
                        HasseiChiFrom, HasseiChiTo, TempOriginList,
                        AreaFrom, AreaTo, TempAreaList,
                        ShashuFrom, ShashuTo, TempBusTypeList,
                        ShashuTankaFrom, ShashuTankaTo,
                        UketsukeJokenFrom, UketsukeJokenTo, TempConditionList,
                        DantaiNm,
                        MaxMinSetting, MaxMinSettingList,
                        ReservationStatus, ReservationStatusList);
                }
                else
                {
                    List<TkdInpCon> filterValues = await FilterConditionService.GetFilterCondition(FormFilterName.HyperMenu, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq);
                    hyperData = GetFilterDataService.GetHyperFormData(filterValues, StaffList, SaleBranchList, CodeKbList, CustomerClassificationList,
                        DestinationList, DispatchList, OriginList, AreaList, BusTypeList, ConditionList, new List<ComboboxFixField>(), new List<ComboboxFixField>(), new List<ComboboxFixField>(), new List<ComboboxFixField>(),
                        MaxMinSettingList, ReservationStatusList, ListReservationClass, ListGyosya, TokiskData, TokiStData);
                }
                isInitComplete = true;
                isFirstInit = true;
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
                JSRuntime.InvokeVoidAsync("setEventforCurrencyField", false);
                JSRuntime.InvokeVoidAsync("setEventforCodeNumberField");
                JSRuntime.InvokeAsync<string>("addMaxLength", "length", 9);
                JSRuntime.InvokeAsync<string>("addMaxLength", "length", 10);
                if (isFirstInit)
                {
                    isLoading = false;
                    isFirstInit = false;
                    CheckInitForDate();
                    JSRuntime.InvokeAsync<string>("focusEditor", "focus");
                    JSRuntime.InvokeVoidAsync("adjustHyperAreaWidth");
                    JSRuntime.InvokeVoidAsync("loadPageScript", "hyperMenuPage", "hyperMenuPageTabKey");
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected async void RedirectToSuperMenu(SuperMenyTypeDisplay type)
        {
            try
            {
                IsClickNavigate = true;
                CheckInitForDate();
                if (IsValid && IsInitForDate)
                {
                    isLoading = true;
                    await Task.Run(() =>
                    {
                        CheckRecordForSuperMenuGrid(type).Wait();
                        Dictionary<string, string> keyValueFilterPairs = GenerateFilterValueDictionaryService.GenerateForHyperFormData(hyperData).Result;
                        FilterConditionService.SaveFilterCondtion(keyValueFilterPairs, FormFilterName.HyperMenu, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq).Wait();
                        if (!IsNoData)
                        {
                            string formName = string.Empty;
                            Dictionary<string, string> dic = GenerateFilterValueDictionaryService.GenerateForHyperFormData(hyperData).Result;
                            if (type == SuperMenyTypeDisplay.Reservation)
                            {
                                formName = FormFilterName.SuperMenuReservation;
                            }
                            if (type == SuperMenyTypeDisplay.Vehicle)
                            {
                                formName = FormFilterName.SuperMenuVehicle;
                            }
                            FilterConditionService.SaveFilterCondtion(dic, formName, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq).Wait();
                        }
                        isLoading = false;
                    });
                    if (!IsNoData)
                    {
                        NavigationManager.NavigateTo(string.Format("/supermenu?&type={0}", (int)type), true);
                    }
                    else
                    {
                        await InvokeAsync(StateHasChanged);
                    }
                }
                else if (!IsInitForDate)
                {
                    ShowErrorPopup = true;
                    ErrorMessage = Lang["NotSpecifyDateMessage"];
                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected async Task RedirectToGraph(GraphTypeDisplay type)
        {
            try
            {
                IsClickNavigate = true;
                CheckInitForDate();
                if (IsValid && IsInitForDate)
                {
                    string Message = HyperDataService.CheckValidationForGraph(type, hyperData, hyperData.dateType);
                    if (!string.IsNullOrEmpty(Message))
                    {
                        ShowErrorPopup = true;
                        ErrorMessage = Lang[Message];
                        return;
                    }

                    isLoading = true;
                    await Task.Run(() =>
                    {
                        CheckRecordForGraphSaleStaffDay().Wait();
                        Dictionary<string, string> keyValueFilterPairs = GenerateFilterValueDictionaryService.GenerateForHyperFormData(hyperData).Result;
                        FilterConditionService.SaveFilterCondtion(keyValueFilterPairs, FormFilterName.HyperMenu, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq).Wait();
                        isLoading = false;
                    });
                    if (!IsNoData)
                    {
                        string QueryParam = hyperData.ToQueryString();
                        NavigationManager.NavigateTo(string.Format("/hypergraph?{0}&type={1}", QueryParam, (int)type), true);
                    }
                    else
                    {
                        await InvokeAsync(StateHasChanged);
                    }
                }
                else if (!IsInitForDate)
                {
                    ShowErrorPopup = true;
                    ErrorMessage = Lang["NotSpecifyDateMessage"];
                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected async Task CheckRecordForSuperMenuGrid(SuperMenyTypeDisplay type)
        {
            try
            {
                if (type == SuperMenyTypeDisplay.Reservation)
                {
                    List<SuperMenuReservationData> data = await HyperDataService.GetSuperMenuReservationData(hyperData, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, new ClaimModel().TenantID, null);
                    IsNoData = data.Count == 0;
                }
                else
                {
                    List<SuperMenuVehicleData> data = await HyperDataService.GetSuperMenuVehicleData(hyperData, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, new ClaimModel().TenantID, null);
                    IsNoData = data.Count == 0;
                }
                if (IsNoData)
                {
                    ShowErrorPopup = true;
                    ErrorMessage = Lang["NoDataMessage"];
                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected async Task CheckRecordForGraphSaleStaffDay()
        {
            try
            {
                List<HyperGraphData> data = await HyperDataService.GetHyperGraphData(hyperData, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, new ClaimModel().TenantID, true);
                if (data.Count() == 0)
                {
                    IsNoData = true;
                    ShowErrorPopup = true;
                    ErrorMessage = Lang["NoDataMessage"];
                }
                else
                {
                    IsNoData = false;
                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected void ChangeValueForm(string ValueName, dynamic value, EditContext formContext)
        {
            try
            {
                bool isChangeValue = true;
                if (value is string && string.IsNullOrEmpty(value))
                {
                    value = null;
                }
                else
                {
                    string[] NumberField = { nameof(hyperData.UketsukeBangoFrom), nameof(hyperData.UketsukeBangoTo), nameof(hyperData.ShashuTankaFrom), nameof(hyperData.ShashuTankaTo) };
                    int maxLength = 10;
                    if (ValueName == nameof(hyperData.ShashuTankaFrom) || ValueName == nameof(hyperData.ShashuTankaTo))
                    {
                        maxLength = 9;
                    }

                    if (NumberField.Contains(ValueName))
                    {
                        if (!long.TryParse(value, out long result))
                        {
                            result = 0;
                        }
                        if (result != 0 && result.ToString().Length > maxLength)
                        {
                            value = result.ToString().Substring(0, maxLength);
                        }
                        isChangeValue = result != 0;
                    }
                }

                if (isChangeValue)
                {
                    var propertyInfo = hyperData.GetType().GetProperty(ValueName);
                    propertyInfo.SetValue(hyperData, value, null);
                    if (ValueName == nameof(hyperData.HaishaBiFrom))
                    {
                        hyperData.HaishaBiTo = value;
                    }
                    if (ValueName == nameof(hyperData.TochakuBiFrom))
                    {
                        hyperData.TochakuBiTo = value;
                    }
                    if (ValueName == nameof(hyperData.YoyakuBiFrom))
                    {
                        hyperData.YoyakuBiTo = value;
                    }
                    IsValid = formContext.Validate();
                    CheckInitForDate();
                }
                InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected void OnDateInputChange(string ValueName, string value, EditContext formContext)
        {
            try
            {
                var date = CommonHelper.ConvertToDateTime(value);
                if (date == null)
                    return;
                ChangeValueForm(ValueName, date, formContext);
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected void CheckInitForDate()
        {
            try
            {
                if (hyperData.HaishaBiFrom == null &&
                    hyperData.HaishaBiTo == null &&
                    hyperData.TochakuBiFrom == null &&
                    hyperData.TochakuBiTo == null &&
                    hyperData.YoyakuBiFrom == null &&
                    hyperData.YoyakuBiTo == null)
                {
                    IsInitForDate = false;
                }
                else
                {
                    IsInitForDate = true;
                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected void ChangeDateType(DateType type)
        {
            try
            {
                DateTime? dateFrom = null;
                DateTime? dateTo = null;
                switch (hyperData.dateType)
                {
                    case (int)DateType.Dispatch:
                        {
                            dateFrom = hyperData.HaishaBiFrom;
                            dateTo = hyperData.HaishaBiTo;
                            break;
                        }
                    case (int)DateType.Arrival:
                        {
                            dateFrom = hyperData.TochakuBiFrom;
                            dateTo = hyperData.TochakuBiTo;
                            break;
                        }
                    case (int)DateType.Reservation:
                        {
                            dateFrom = hyperData.YoyakuBiFrom;
                            dateTo = hyperData.YoyakuBiTo;
                            break;
                        }
                    default:
                        break;
                }

                switch (type)
                {
                    case DateType.Dispatch:
                        {
                            hyperData.HaishaBiFrom = dateFrom;
                            hyperData.HaishaBiTo = dateTo;
                            hyperData.TochakuBiFrom = null;
                            hyperData.TochakuBiTo = null;
                            hyperData.YoyakuBiFrom = null;
                            hyperData.YoyakuBiTo = null;
                            break;
                        }
                    case DateType.Arrival:
                        {
                            hyperData.HaishaBiFrom = null;
                            hyperData.HaishaBiTo = null;
                            hyperData.TochakuBiFrom = dateFrom;
                            hyperData.TochakuBiTo = dateTo;
                            hyperData.YoyakuBiFrom = null;
                            hyperData.YoyakuBiTo = null;
                            break;
                        }
                    case DateType.Reservation:
                        {
                            hyperData.HaishaBiFrom = null;
                            hyperData.HaishaBiTo = null;
                            hyperData.TochakuBiFrom = null;
                            hyperData.TochakuBiTo = null;
                            hyperData.YoyakuBiFrom = dateFrom;
                            hyperData.YoyakuBiTo = dateTo;
                            break;
                        }
                    default: break;
                }
                hyperData.dateType = (int)type;
                InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected void ResetHyperForm(EditContext formContext)
        {
            try
            {
                hyperData.Reinit();
                IsValid = formContext.Validate();
                CheckInitForDate();
                InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }
    }
}
