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
    public class SuperMenuBase : ComponentBase
    {
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
        [Parameter] public string type { get; set; }

        [Inject]
        public IHyperDataService HyperDataService { get; set; }
        [Inject]
        public CustomHttpClient Http { get; set; }
        [Inject]
        public AppSettingsService AppSettingsService { get; set; }
        [Inject]
        public ITPM_YoyKbnDataListService TPM_YoyKbnDataService { get; set; }
        [Inject]
        public ISaleBranchListService EigyosService { get; set; }
        [Inject]
        public IStaffListService SyainService { get; set; }
        [Inject]
        public ICustomerListService CustomerService { get; set; }
        [Inject]
        public ITPM_CodeKbListService CodeKbService { get; set; }
        [Inject]
        public ICustomerCLassificationListService CustomerCLassificationService { get; set; }
        [Inject]
        public ILocationListService LocationService { get; set; }
        [Inject]
        public IDispatchListService DispatchService { get; set; }
        [Inject]
        public IBusTypeListService BusTypeService { get; set; }
        [Inject]
        public IJSRuntime JSRuntime { get; set; }
        [Inject] public IStringLocalizer<SuperMenu> Lang { get; set; }
        [Inject] public IStringLocalizer<HyperMenu> HyperLang { get; set; }
        [Inject]
        public CustomNavigation NavigationManager { get; set; }
        [Inject]
        public IFilterCondition FilterConditionService { get; set; }
        [Inject]
        public IGenerateFilterValueDictionary GenerateFilterValueDictionaryService { get; set; }
        [Inject]
        public IGetFilterDataService GetFilterDataService { get; set; }
        [Inject]
        public IBlazorContextMenuService blazorContextMenuService { get; set; }
        [Inject]
        public IErrorHandlerService errorModalService { get; set; }
        [Inject]
        public IWebHostEnvironment hostingEnvironment { get; set; }
        [Inject]
        public ICustomerComponentService _service { get; set; }
        [Inject]
        public IReservationClassComponentService _yoyakuservice { get; set; }
        public string dateFormat = "yyyy/MM/dd";
        public string formName = string.Empty;
        public List<SuperMenuReservationData> SuperMenuReservationGridData = new List<SuperMenuReservationData>();
        public List<SuperMenuVehicleData> SuperMenuVehicleGridData = new List<SuperMenuVehicleData>();
        public HyperFormData hyperData = new HyperFormData();
        public HyperFormData hyperDataInit = new HyperFormData();
        public int activeTabIndex = 0;
        public int ActiveTabIndex
        {
            get => activeTabIndex;
            set
            {
                activeTabIndex = value;
                AdjustHeightWhenTabChanged();
            }
        }
        public List<ReservationData> BookingTypeList;
        public List<SaleBranchData> SaleBranchList;
        public List<StaffsData> StaffList;
        public List<LoadCustomerList> CustomerList;
        public List<CodeTypeData> CodeKbList;
        public List<CustomerClassification> CustomerClassificationList;
        public List<LoadLocation> DestinationList;
        public List<LoadDispatchArea> DispatchList;
        public List<LoadLocation> OriginList;
        public List<LoadLocation> AreaList;
        public List<BusTypesData> BusTypeList;
        public List<VpmCodeKb> ConditionList;
        public List<CustomerComponentGyosyaData> ListGyosya { get; set; } = new List<CustomerComponentGyosyaData>();
        public List<CustomerComponentTokiskData> TokiskData { get; set; } = new List<CustomerComponentTokiskData>();
        public List<CustomerComponentTokiStData> TokiStData { get; set; } = new List<CustomerComponentTokiStData>();
        public List<ReservationClassComponentData> ListReservationClass { get; set; } = new List<ReservationClassComponentData>();
        public bool IsValid = true;
        public bool ShowErrorPopup = false;
        public string ErrorMessage = "";
        public bool isLoading { get; set; } = true;
        public bool IsInitForDate = false;
        public bool IsNoData = false;
        public byte RecordsPerPage = (byte)25;
        public int FirstPageSelect = 0;
        public int filterId = 0;
        public bool isInitComplete = false;
        public bool isFirstInit = false;
        public Dictionary<string, string> keyValueFilterPairs = new Dictionary<string, string>();
        public List<CustomFilerModel> CustomFilters;
        public string CustomFilter = string.Empty;
        public CustomFilerModel CustomFilterModelChange = new CustomFilerModel();
        public CustomFilerModel DeleteCustomFilterModel = new CustomFilerModel();
        public CustomFilerModel AddCustomFilterModel = new CustomFilerModel();
        public bool ShowRenameFilter = false;
        public bool ShowComfirmDeleteFilter = false;
        public bool ShowAddNewCustomFilter = false;
        public List<ReservationData> TempBookingTypeList = new List<ReservationData>();
        public List<SaleBranchData> TempSaleBranchList = new List<SaleBranchData>();
        public List<StaffsData> TempStaffList = new List<StaffsData>();
        public List<LoadCustomerList> TempCustomerList = new List<LoadCustomerList>();
        public List<CodeTypeData> TempCodeKbList = new List<CodeTypeData>();
        public List<CustomerClassification> TempCustomerClassificationList = new List<CustomerClassification>();
        public List<LoadLocation> TempDestinationList = new List<LoadLocation>();
        public List<LoadDispatchArea> TempDispatchList = new List<LoadDispatchArea>();
        public List<LoadLocation> TempOriginList = new List<LoadLocation>();
        public List<LoadLocation> TempAreaList = new List<LoadLocation>();
        public List<BusTypesData> TempBusTypeList = new List<BusTypesData>();
        public List<VpmCodeKb> TempConditionList = new List<VpmCodeKb>();
        public SuperMenuReservationTotalGridData SuperMenuReservationTotalGridData = new SuperMenuReservationTotalGridData();
        public SuperMenuVehicleTotalGridData SuperMenuVehicleTotalGridData = new SuperMenuVehicleTotalGridData();
        public SuperMenuReservation childReservation;
        public SuperMenuVehicle childVehicle;
        public Dictionary<string, string> LangDic = new Dictionary<string, string>();
        public List<ComboboxFixField> PagePrintData = new List<ComboboxFixField>();
        public List<ComboboxFixField> MaxMinSettingList = new List<ComboboxFixField>();
        public List<ComboboxFixField> ReservationStatusList = new List<ComboboxFixField>();
        public Pagination paging = new Pagination();
        protected bool isChangeValue = false;
        protected bool isReloadTotal = false;
        private bool isCusFromFirstLoaded = false;
        private bool isCusToFirstLoaded = false;
        private bool isSupFromFirstLoaded = false;
        private bool isSupToFirstLoaded = false;
        private bool isFirstLoaded;


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
            try
            {

                // 受付営業所
                TempSaleBranchList = await EigyosService.Get(new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID);
                SaleBranchList = TempSaleBranchList;
                SaleBranchList.Insert(0, null);

                // 営業担当, 入力担当
                TempStaffList = await SyainService.Get(new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID);
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
                TempCodeKbList = await CodeKbService.GetDantai(new ClaimModel().TenantID);
                CodeKbList = TempCodeKbList;
                CodeKbList.Insert(0, null);

                // 客種区分
                TempCustomerClassificationList = await CustomerCLassificationService.Get(new ClaimModel().TenantID);
                CustomerClassificationList = TempCustomerClassificationList;
                CustomerClassificationList.Insert(0, null);

                // 行先
                TempDestinationList = await LocationService.GetDestination(new ClaimModel().TenantID);
                DestinationList = TempDestinationList;
                DestinationList.Insert(0, null);

                // 配車地
                TempDispatchList = await DispatchService.Get(new ClaimModel().TenantID);
                DispatchList = TempDispatchList;
                DispatchList.Insert(0, null);

                // 発生地
                TempOriginList = await LocationService.GetOrigin(new ClaimModel().TenantID);
                OriginList = TempOriginList;
                OriginList.Insert(0, null);

                // エリア
                TempAreaList = await LocationService.GetArea(new ClaimModel().TenantID);
                AreaList = TempAreaList;
                AreaList.Insert(0, null);

                // 車種
                TempBusTypeList = await BusTypeService.GetAll(new ClaimModel().TenantID);
                BusTypeList = TempBusTypeList;
                BusTypeList.Insert(0, null);

                // 受付条件
                TempConditionList = await CodeKbService.GetJoken(new ClaimModel().TenantID);
                ConditionList = TempConditionList;
                ConditionList.Insert(0, null);
                // 上限下限設定
                MaxMinSettingList = new List<ComboboxFixField> {
                null,
                new ComboboxFixField() { IdValue = 0, StringValue = Lang["AlreadySet"] },
                new ComboboxFixField() { IdValue = 1, StringValue = Lang["NotYetSet"] }
            };
                // 予約ステータス
                ReservationStatusList = new List<ComboboxFixField> {
                null,
                new ComboboxFixField() { IdValue = 0, StringValue = Lang["Reservation"] },
                new ComboboxFixField() { IdValue = 1, StringValue = Lang["Estimates"] }
            };
                // 帳票設定
                PagePrintData = new List<ComboboxFixField>
{
                new ComboboxFixField { IdValue = 1, StringValue = "A4" },
                new ComboboxFixField { IdValue = 2, StringValue = "A3" },
                new ComboboxFixField { IdValue = 3, StringValue = "B4" }
            };
                // Add data for output type
                ShowHeaderOptions.ShowHeaderOptionData[0].StringValue = Lang["OutType_" + ShowHeaderOptions.ShowHeaderOptionData[0].IdValue.ToString()];
                ShowHeaderOptions.ShowHeaderOptionData[1].StringValue = Lang["OutType_" + ShowHeaderOptions.ShowHeaderOptionData[1].IdValue.ToString()];

                // Add data for group type print
                GroupTypes.GroupTypeData[0].StringValue = Lang["GroupType_" + GroupTypes.GroupTypeData[0].IdValue.ToString()];
                GroupTypes.GroupTypeData[1].StringValue = Lang["GroupType_" + GroupTypes.GroupTypeData[1].IdValue.ToString()];

                // Add data for delimiter type print
                DelimiterTypes.DelimiterTypeData[0].StringValue = Lang["DelimiterType_" + DelimiterTypes.DelimiterTypeData[0].IdValue.ToString()];
                DelimiterTypes.DelimiterTypeData[1].StringValue = Lang["DelimiterType_" + DelimiterTypes.DelimiterTypeData[1].IdValue.ToString()];
                DelimiterTypes.DelimiterTypeData[2].StringValue = Lang["DelimiterType_" + DelimiterTypes.DelimiterTypeData[2].IdValue.ToString()];


                if (int.Parse(type) == (int)SuperMenyTypeDisplay.Reservation)
                {
                    formName = FormFilterName.SuperMenuReservation;
                }
                if (int.Parse(type) == (int)SuperMenyTypeDisplay.Vehicle)
                {
                    formName = FormFilterName.SuperMenuVehicle;
                }
                List<TkdInpCon> filterValues = FilterConditionService.GetFilterCondition(formName, filterId, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq).Result;

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
                    hyperData = GetFilterDataService.GetHyperFormData(filterValues, TempStaffList, TempSaleBranchList, TempCodeKbList, TempCustomerClassificationList,
                        TempDestinationList, TempDispatchList, TempOriginList, TempAreaList, TempBusTypeList, TempConditionList, PagePrintData, ShowHeaderOptions.ShowHeaderOptionData, GroupTypes.GroupTypeData, DelimiterTypes.DelimiterTypeData,
                        MaxMinSettingList, ReservationStatusList, ListReservationClass, ListGyosya, TokiskData, TokiStData);
                }
                hyperDataInit = new HyperFormData(hyperData);

                IsInitForDate = CheckInitForDate();
                CustomFilters = await FilterConditionService.GetCustomFilters(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, formName);
                CustomFilter = Newtonsoft.Json.JsonConvert.SerializeObject(CustomFilters);
                var dataLang = HyperLang.GetAllStrings();
                LangDic = dataLang.ToDictionary(l => l.Name, l => l.Value);

                if (!(int.TryParse(type, out _)) || !(Enum.IsDefined(typeof(SuperMenyTypeDisplay), int.Parse(type))) || !IsInitForDate)
                {
                    NavigationManager.NavigateTo("/hypermenu", true);
                    return;
                }
                else
                {
                    isInitComplete = true;
                    isFirstInit = true;
                }
            }
            catch (Exception ex)
            {
                isInitComplete = true;
                isFirstInit = true;
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        [JSInvokable]
        public void RenameCustomFilter(CustomFilerModel data)
        {
            try
            {
                ShowRenameFilter = true;
                CustomFilterModelChange = data;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }
        [JSInvokable]
        public void DeleteCustomFilter(CustomFilerModel data)
        {
            try
            {
                ShowComfirmDeleteFilter = true;
                DeleteCustomFilterModel = data;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }
        [JSInvokable]
        public void CustomFilterSelected(CustomFilerModel data)
        {
            try
            {
                var currentFilterId = 0;
                if (data != null)
                {
                    currentFilterId = data.FilterId;
                }
                List<TkdInpCon> filterValues = FilterConditionService.GetFilterCondition(formName, currentFilterId, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq).Result;
                hyperData = GetFilterDataService.GetHyperFormData(filterValues, TempStaffList, TempSaleBranchList, TempCodeKbList, TempCustomerClassificationList,
                    TempDestinationList, TempDispatchList, TempOriginList, TempAreaList, TempBusTypeList, TempConditionList, PagePrintData, ShowHeaderOptions.ShowHeaderOptionData, GroupTypes.GroupTypeData, DelimiterTypes.DelimiterTypeData,
                    MaxMinSettingList, ReservationStatusList, ListReservationClass, ListGyosya, TokiskData, TokiStData);
                ChangeState();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        public void ShowSaveCustomFilerPopup()
        {
            try
            {
                AddCustomFilterModel = new CustomFilerModel();
                ShowAddNewCustomFilter = true;
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }
        public async Task SaveNewFilterName(string ValueName, dynamic value)
        {
            try
            {
                var propertyInfo = AddCustomFilterModel.GetType().GetProperty(ValueName);
                propertyInfo.SetValue(AddCustomFilterModel, value, null);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        public void SaveCustomFilter()
        {
            try
            {
                if (IsValid)
                {
                    int maxFilterId = FilterConditionService.GetMaxCustomFilerId(formName, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq).Result;
                    var keyValueFilterPairs = GenerateFilterValueDictionaryService.GenerateForHyperFormData(hyperData).Result;
                    FilterConditionService.SaveCustomFiler(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, maxFilterId + 1, formName, AddCustomFilterModel.FilterName).Wait();
                    FilterConditionService.SaveFilterCondtion(keyValueFilterPairs, formName, maxFilterId + 1, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq);
                }
                ShowAddNewCustomFilter = false;
                ReloadFilterSearchData();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }
        public void DeleteCustomFilter()
        {
            try
            {
                FilterConditionService.DeleteCustomFilter(DeleteCustomFilterModel.EmployeeId, DeleteCustomFilterModel.FilterId, DeleteCustomFilterModel.FormName, DeleteCustomFilterModel.FilterName).Wait();
                FilterConditionService.DeleteCustomFilerCondition(DeleteCustomFilterModel.EmployeeId, DeleteCustomFilterModel.FilterId, DeleteCustomFilterModel.FormName).Wait();
                ShowComfirmDeleteFilter = false;
                ReloadFilterSearchData();
                List<TkdInpCon> filterValues = FilterConditionService.GetFilterCondition(formName, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq).Result;
                hyperData = GetFilterDataService.GetHyperFormData(filterValues, TempStaffList, TempSaleBranchList, TempCodeKbList, TempCustomerClassificationList,
                    TempDestinationList, TempDispatchList, TempOriginList, TempAreaList, TempBusTypeList, TempConditionList, PagePrintData, ShowHeaderOptions.ShowHeaderOptionData, GroupTypes.GroupTypeData, DelimiterTypes.DelimiterTypeData,
                    MaxMinSettingList, ReservationStatusList, ListReservationClass, ListGyosya, TokiskData, TokiStData);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        public async Task ChangeFilterName(string ValueName, dynamic value)
        {
            try
            {
                var propertyInfo = CustomFilterModelChange.GetType().GetProperty(ValueName);
                propertyInfo.SetValue(CustomFilterModelChange, value, null);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }


        public async void SaveFilterName()
        {
            try
            {
                FilterConditionService.SaveCustomFiler(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, CustomFilterModelChange.FilterId, formName, CustomFilterModelChange.FilterName).Wait();
                ShowRenameFilter = false;
                ReloadFilterSearchData();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        public void ReloadFilterSearchData()
        {
            try
            {
                CustomFilters = FilterConditionService.GetCustomFilters(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, formName).Result;
                CustomFilter = Newtonsoft.Json.JsonConvert.SerializeObject(CustomFilters);
                JSRuntime.InvokeAsync<string>("loadLibraryScript", "js/dx.all.js", "RenderFilterSearch", CustomFilter, DotNetObjectReference.Create(this));
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        public bool CheckInitForDate()
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
            return IsInitForDate;
        }

        protected override void OnAfterRender(bool firstRender)
        {
            try
            {
                JSRuntime.InvokeVoidAsync("setEventforCurrencyField", false);
                JSRuntime.InvokeVoidAsync("setEventforCodeNumberField");
                //JSRuntime.InvokeVoidAsync("loadPageScript", "hyperMenuPage", "hyperMenuPageTabKey");

                if (isFirstInit)
                {
                    isFirstInit = false;
                    JSRuntime.InvokeAsync<string>("loadLibraryScript", "js/dx.all.js", "RenderFilterSearch", CustomFilter, DotNetObjectReference.Create(this));
                    JSRuntime.InvokeVoidAsync("fadeToggleWidthAdjustHeight");
                    JSRuntime.InvokeVoidAsync("EnterTab", ".enterField", false);

                    if (int.Parse(type) == (int)SuperMenyTypeDisplay.Reservation)
                    {
                        SuperMenuReservationGridData = HyperDataService.GetSuperMenuReservationData(hyperData, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, new ClaimModel().TenantID).Result;
                        IsNoData = SuperMenuReservationGridData.Count() == 0;
                        if (!IsNoData)
                        {
                            List<SuperMenuReservationData> FirstPageData = HyperDataService.GetSuperMenuReservationData(hyperData, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, new ClaimModel().TenantID, 0, RecordsPerPage).Result;
                            SuperMenuReservationGridData.RemoveRange(0, FirstPageData.Count());
                            SuperMenuReservationGridData.InsertRange(0, FirstPageData);
                            SuperMenuReservationTotalGridData = HyperDataService.GetSuperMenuReservationTotalData(hyperData, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, new ClaimModel().TenantID).Result;
                        }
                        else
                        {
                            ShowErrorPopup = true;
                            ErrorMessage = Lang["NoDataMessage"];
                        }
                    }
                    else // Supper menu vehicle
                    {
                        SuperMenuVehicleGridData = HyperDataService.GetSuperMenuVehicleData(hyperData, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, new ClaimModel().TenantID).Result;
                        IsNoData = SuperMenuVehicleGridData.Count() == 0;
                        if (!IsNoData)
                        {
                            List<SuperMenuVehicleData> FirstPageData = HyperDataService.GetSuperMenuVehicleData(hyperData, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, new ClaimModel().TenantID, 0, RecordsPerPage).Result;
                            SuperMenuVehicleGridData.RemoveRange(0, FirstPageData.Count());
                            SuperMenuVehicleGridData.InsertRange(0, FirstPageData);
                            SuperMenuVehicleTotalGridData = HyperDataService.GetSuperMenuVehicleTotalData(hyperData, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, new ClaimModel().TenantID).Result;
                        }
                        else
                        {
                            ShowErrorPopup = true;
                            ErrorMessage = Lang["NoDataMessage"];
                        }
                    }
                    StateHasChanged();
                }
                
            }
            catch (Exception ex)
            {
                isLoading = false;
                StateHasChanged();
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        public void clickV(MouseEventArgs e, int number)
        {
            try
            {
                hyperData.ActiveV = number;
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        public async Task ChangeState(int Page = 0)
        {
            try
            {
                IsInitForDate = CheckInitForDate();
                if (IsValid && IsInitForDate)
                {
                    if (int.Parse(type) == (int)SuperMenyTypeDisplay.Reservation)
                    {
                        childReservation.CheckedItems = new List<SuperMenuReservationData>();
                        SuperMenuReservationGridData = new List<SuperMenuReservationData>();
                        SuperMenuReservationGridData = await HyperDataService.GetSuperMenuReservationData(hyperData, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, new ClaimModel().TenantID);
                        int NumberOfPage = (SuperMenuReservationGridData.Count() + RecordsPerPage - 1) / RecordsPerPage;
                        paging.TotalCount = SuperMenuReservationGridData.Count();
                        paging.currentPage = Page;
                        IsNoData = SuperMenuReservationGridData.Count() == 0;
                        if (!IsNoData)
                        {
                            FirstPageSelect = Math.Min(Page, NumberOfPage - 1);
                            List<SuperMenuReservationData> FirstPageData = new List<SuperMenuReservationData>();
                            FirstPageData = HyperDataService.GetSuperMenuReservationData(hyperData, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, new ClaimModel().TenantID, FirstPageSelect * RecordsPerPage, RecordsPerPage).Result;
                            if (SuperMenuReservationGridData.Count >= FirstPageData.Count)
                            {
                                SuperMenuReservationGridData.RemoveRange(FirstPageSelect * RecordsPerPage, FirstPageData.Count());
                                SuperMenuReservationGridData.InsertRange(FirstPageSelect * RecordsPerPage, FirstPageData);
                            }
                            SuperMenuReservationTotalGridData = await HyperDataService.GetSuperMenuReservationTotalData(hyperData, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, new ClaimModel().TenantID);
                        }
                        else
                        {
                            FirstPageSelect = 0;
                            SuperMenuReservationTotalGridData = new SuperMenuReservationTotalGridData();
                            ShowErrorPopup = true;
                            ErrorMessage = Lang["NoDataMessage"];
                        }
                    }
                    else
                    {
                        childVehicle.CheckedItems = new List<SuperMenuVehicleData>();
                        SuperMenuVehicleGridData = new List<SuperMenuVehicleData>();
                        SuperMenuVehicleGridData = await HyperDataService.GetSuperMenuVehicleData(hyperData, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, new ClaimModel().TenantID);
                        int NumberOfPage = (SuperMenuVehicleGridData.Count() + RecordsPerPage - 1) / RecordsPerPage;
                        paging.TotalCount = SuperMenuVehicleGridData.Count();
                        paging.currentPage = Page;
                        IsNoData = SuperMenuVehicleGridData.Count() == 0;
                        if (!IsNoData)
                        {
                            FirstPageSelect = Math.Min(Page, NumberOfPage - 1);
                            var taskDetail = HyperDataService.GetSuperMenuVehicleData(hyperData, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, new ClaimModel().TenantID, FirstPageSelect * RecordsPerPage, RecordsPerPage);
                            var taskTotal = HyperDataService.GetSuperMenuVehicleTotalData(hyperData, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, new ClaimModel().TenantID);

                            await Task.WhenAll(taskDetail, taskTotal);

                            List<SuperMenuVehicleData> FirstPageData = taskDetail.Result;
                            if (SuperMenuVehicleGridData.Count >= FirstPageData.Count)
                            {
                                SuperMenuVehicleGridData.RemoveRange(FirstPageSelect * RecordsPerPage, FirstPageData.Count());
                                SuperMenuVehicleGridData.InsertRange(FirstPageSelect * RecordsPerPage, FirstPageData);
                            }
                            SuperMenuVehicleTotalGridData = taskTotal.Result;
                        }
                        else
                        {
                            FirstPageSelect = 0;
                            SuperMenuVehicleTotalGridData = new SuperMenuVehicleTotalGridData();
                            ShowErrorPopup = true;
                            ErrorMessage = Lang["NoDataMessage"];
                        }
                    }
                    if (!IsNoData)
                    {
                        keyValueFilterPairs = GenerateFilterValueDictionaryService.GenerateForHyperFormData(hyperData).Result;
                        FilterConditionService.SaveFilterCondtion(keyValueFilterPairs, formName, filterId, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq).Wait();
                    }
                    isChangeValue = true;
                }
                else if (!IsInitForDate)
                {
                    ShowErrorPopup = true;
                    ErrorMessage = Lang["NotSpecifyDateMessage"];
                }
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        public async Task ChangeValueForm(string ValueName, dynamic value, EditContext formContext)
        {
            try
            {
                if (value is string && string.IsNullOrEmpty(value))
                {
                    value = null;
                }
                switch (ValueName)
                {
                    case nameof(hyperData.UketsukeBangoFrom):
                        if (!string.IsNullOrEmpty(value))
                        {
                            if (!long.TryParse(value, out long result))
                                result = 0;
                            if (result != 0 && result.ToString().Length > 10)
                            {
                                value = result.ToString().Substring(0, 10);
                            }
                            if (result == 0)
                                value = hyperData.UketsukeBangoFrom;
                        }
                        break;
                    case nameof(hyperData.UketsukeBangoTo):
                        if (!string.IsNullOrEmpty(value))
                        {
                            if (!long.TryParse(value, out long resultTo))
                                resultTo = 0;
                            if (resultTo != 0 && resultTo.ToString().Length > 10)
                            {
                                value = resultTo.ToString().Substring(0, 10);
                            }
                            if (resultTo == 0)
                                value = hyperData.UketsukeBangoTo;
                        }
                        break;
                    case nameof(hyperData.ShashuTankaFrom):
                        if (!string.IsNullOrEmpty(value))
                        {
                            if (!int.TryParse(value, out int resultTankaFrom))
                                resultTankaFrom = 0;
                            if (resultTankaFrom != 0 && resultTankaFrom.ToString().Length > 9)
                            {
                                value = resultTankaFrom.ToString().Substring(0, 9);
                            }
                            if (resultTankaFrom == 0)
                                value = hyperData.ShashuTankaFrom;
                        }
                        break;
                    case nameof(hyperData.ShashuTankaTo):
                        if (!string.IsNullOrEmpty(value))
                        {
                            if (!int.TryParse(value, out int resultTankaTo))
                                resultTankaTo = 0;
                            if (resultTankaTo != 0 && resultTankaTo.ToString().Length > 9)
                            {
                                value = resultTankaTo.ToString().Substring(0, 9);
                            }
                            if (resultTankaTo == 0)
                                value = hyperData.ShashuTankaTo;
                        }
                        break;
                    default:
                        break;
                }

                var propertyInfo = hyperData.GetType().GetProperty(ValueName);
                propertyInfo.SetValue(hyperData, value, null);
                isLoading = true;
                await Task.Run(() =>
                {
                    InvokeAsync(StateHasChanged).Wait();
                    IsValid = formContext.GetValidationMessages().Distinct().Count() == 0;
                    if (ValueName != nameof(hyperData.GyosyaTokuiSakiFrom) && ValueName != nameof(hyperData.GyosyaTokuiSakiTo)
                    && ValueName != nameof(hyperData.TokiskTokuiSakiFrom) && ValueName != nameof(hyperData.TokiskTokuiSakiTo)
                    && ValueName != nameof(hyperData.GyosyaShiireSakiFrom) && ValueName != nameof(hyperData.GyosyaShiireSakiTo)
                    && ValueName != nameof(hyperData.TokiskShiireSakiFrom) && ValueName != nameof(hyperData.TokiskShiireSakiTo)
                    && isCusFromFirstLoaded && isCusToFirstLoaded && isSupFromFirstLoaded && isSupToFirstLoaded)
                    {
                        isReloadTotal = true;
                        ChangeState().Wait();
                    }
                });
                isLoading = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        public async void AdjustHeightWhenTabChanged()
        {
            try
            {
                await Task.Run(() =>
                {
                    InvokeAsync(StateHasChanged).Wait();
                    JSRuntime.InvokeVoidAsync("AdjustHeight");
                });
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        public async void ResetHyperForm()
        {
            try
            {
                hyperData = new HyperFormData(hyperDataInit);
                CheckInitForDate();
                isLoading = true;
                await Task.Run(() =>
                {
                    InvokeAsync(StateHasChanged).Wait();
                    isReloadTotal = true;
                    ChangeState(0).Wait();
                });
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected void ExportBtnClicked()
        {
            try
            {
                if (IsValid)
                {
                    keyValueFilterPairs = GenerateFilterValueDictionaryService.GenerateForHyperFormData(hyperData).Result;
                    FilterConditionService.SaveFilterCondtion(keyValueFilterPairs, formName, filterId, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq).Wait();
                    if (hyperData.OutputType == OutputReportType.Preview)
                    {
                        if (int.TryParse(type, out _) && int.Parse(type) == (int)SuperMenyTypeDisplay.Reservation)
                        {
                            hyperData.Type = 1;
                            var searchString = EncryptHelper.EncryptToUrl(hyperData);
                            JSRuntime.InvokeVoidAsync("open", "superreservationreportpreview?searchString=" + searchString, "_blank");
                        }
                        else
                        {
                            hyperData.Type = 2;
                            var searchString = EncryptHelper.EncryptToUrl(hyperData);
                            JSRuntime.InvokeVoidAsync("open", "supervehiclereportpreview?searchString=" + searchString, "_blank");
                        }

                    }
                    else
                    {
                        isLoading = true;
                        Task.Run(() =>
                        {
                            InvokeAsync(StateHasChanged).Wait();
                            if (hyperData.OutputType == OutputReportType.CSV)
                            {
                                if (int.TryParse(type, out _) && int.Parse(type) == (int)SuperMenyTypeDisplay.Reservation)
                                {
                                    ReservationPrintCsv();
                                }
                                else
                                {
                                    VehiclePrintCsv();
                                }
                            }
                            else
                            {
                                if (int.TryParse(type, out _) && int.Parse(type) == (int)SuperMenyTypeDisplay.Reservation)
                                {
                                    ReservationPrint();
                                }
                                else
                                {
                                    VehiclePrint();
                                }
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        public async void VehiclePrint()
        {
            try
            {
                var data = await HyperDataService.GetSuperMenuVehicleReportData(hyperData, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, new ClaimModel().TenantID, 0);
                if (data.Count > 0)
                {
                    dynamic report = new Reports.SuperVehicleReportA4();
                    if (hyperData.PageSize.IdValue == PagePrintData[1].IdValue)
                    {
                        report = new Reports.SuperVehicleReportA3();
                    }
                    else
                    {
                        if (hyperData.PageSize.IdValue == PagePrintData[2].IdValue)
                            report = new Reports.SuperVehicleReportB4();
                    }
                    report.DataSource = data;
                    await new System.Threading.Tasks.TaskFactory().StartNew(() =>
                    {
                        report.CreateDocument();
                        using (MemoryStream ms = new MemoryStream())
                        {
                            report.ExportToPdf(ms);

                            byte[] exportedFileBytes = ms.ToArray();
                            string myExportString = Convert.ToBase64String(exportedFileBytes);
                            if (isLoading)
                            {
                                isLoading = false;
                                InvokeAsync(StateHasChanged).Wait();
                            }
                            JSRuntime.InvokeVoidAsync("downloadFileClientSide", myExportString, "pdf", "SuperMenuVehicleReport");

                        }
                    });
                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        public async void VehiclePrintCsv()
        {
            try
            {
                var listData = await HyperDataService.GetSuperMenuVehicleCsvData(hyperData, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, new ClaimModel().TenantID, 0);
                var dt = listData.ToDataTable<SuperMenuVehicleCsv>();
                while (dt.Columns.Count > 123)
                {
                    dt.Columns.RemoveAt(123);
                }
                SetTableHeaderVehicle(dt);
                string path = string.Format("{0}/csv/{1}.csv", hostingEnvironment.WebRootPath, Guid.NewGuid());

                bool isWithHeader = hyperData.ActiveHeaderOption.IdValue == 0 ? true : false;
                bool isEnclose = hyperData.GroupType.IdValue == 0 ? true : false;
                string space = hyperData.DelimiterType.IdValue == 0 ? "\t" : hyperData.DelimiterType.IdValue == 1 ? ";" : ",";

                var result = CsvHelper.ExportDatatableToCsv(dt, path, true, isWithHeader, isEnclose, space);
                await new System.Threading.Tasks.TaskFactory().StartNew(() =>
                {
                    string myExportString = Convert.ToBase64String(result);
                    if (isLoading)
                    {
                        isLoading = false;
                        InvokeAsync(StateHasChanged).Wait();
                    }
                    JSRuntime.InvokeVoidAsync("downloadFileClientSide", myExportString, "csv", "SuperMenuVehicleReport");
                });
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        public async void ReservationPrint()
        {
            try
            {
                var data = await HyperDataService.GetSuperMenuReservationReportData(hyperData, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, new ClaimModel().TenantID);
                if (data.Count > 0)
                {
                    dynamic report = new Reports.SuperReservationReportA4();
                    if (hyperData.PageSize.IdValue == PagePrintData[1].IdValue)
                    {
                        report = new Reports.SuperReservationReportA3();
                    }
                    else
                    {
                        if (hyperData.PageSize.IdValue == PagePrintData[2].IdValue)
                            report = new Reports.SuperReservationReportB4();
                    }
                    report.DataSource = data;
                    await new System.Threading.Tasks.TaskFactory().StartNew(() =>
                    {
                        report.CreateDocument();
                        using (MemoryStream ms = new MemoryStream())
                        {
                            report.ExportToPdf(ms);

                            byte[] exportedFileBytes = ms.ToArray();
                            string myExportString = Convert.ToBase64String(exportedFileBytes);
                            if (isLoading)
                            {
                                isLoading = false;
                                InvokeAsync(StateHasChanged).Wait();
                            }
                            JSRuntime.InvokeVoidAsync("downloadFileClientSide", myExportString, "pdf", "SuperMenuReservationReport");

                        }
                    });
                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        public async void ReservationPrintCsv()
        {
            try
            {
                var listData = await HyperDataService.GetSuperMenuReservationCsvData(hyperData, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, new ClaimModel().TenantID);
                var dt = listData.ToDataTable<SuperMenuReservationCsv>();
                while (dt.Columns.Count > 136)
                {
                    dt.Columns.RemoveAt(136);
                }
                SetTableHeaderReservation(dt);
                string path = string.Format("{0}/csv/{1}.csv", hostingEnvironment.WebRootPath, Guid.NewGuid());

                bool isWithHeader = hyperData.ActiveHeaderOption.IdValue == 0 ? true : false;
                bool isEnclose = hyperData.GroupType.IdValue == 0 ? true : false;
                string space = hyperData.DelimiterType.IdValue == 0 ? "\t" : hyperData.DelimiterType.IdValue == 1 ? ";" : ",";

                var result = CsvHelper.ExportDatatableToCsv(dt, path, true, isWithHeader, isEnclose, space);
                await new System.Threading.Tasks.TaskFactory().StartNew(() =>
                {
                    string myExportString = Convert.ToBase64String(result);
                    if (isLoading)
                    {
                        isLoading = false;
                        InvokeAsync(StateHasChanged).Wait();
                    }
                    JSRuntime.InvokeVoidAsync("downloadFileClientSide", myExportString, "csv", "SuperMenuReservationReport");
                });
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        public void SetTableHeaderVehicle(DataTable table)
        {
            try
            {
                List<string> listHeader = new List<string>() { "受付番号", "運行日連番", "悌団番号", "分割連番", "予約区分", "予約区分名", "受付年月日", "得意先業者コード",
                "得意先コード", "得意先支店コード", "得意先業者名", "得意先名", "得意先支店名", "得意先略名", "得意先支店略名", "得意先担当者名", "団体名", "団体名２", "行き先名", "車輛営業所コード", "車輛営業所名",
                "車輛営業所略名", "車輛コード", "車号", "号車", "傭車業者コード", "傭車先コード", "傭車支店コード", "傭車業者名", "傭車先名", "傭車支店名", "傭車先略名", "傭車支店略名", "配車年月日", "配車時間",
                "配車地名", "配車地住所１", "配車地住所２", "到着年月日", "到着時間", "到着地名", "到着地住所１", "到着地住所２",
                "出庫年月日", "出庫時間", "出庫営業所コード", "出庫営業所名", "出庫営業所略名", "帰庫年月日",
                "帰庫時間", "帰庫営業所コード", "帰庫営業所名", "帰庫営業所略名", "出発時間", "運転手数", "ガイド数", "車種コード", "車種名", "型区分", "型区分略名", "運賃",
                "消費税額", "手数料額", "ガイド料売上額", "ガイド料消費税額", "ガイド料手数料額", "その他付帯売上額", "その他付帯消費税額", "その他付帯手数料額", "傭車運賃", "傭車消費税額", "傭車手数料額", "傭車ガイド料発生額",
                "傭車ガイド料消費税額", "傭車ガイド料手数料額", "傭車その他付帯発生額", "傭車その他付帯消費税額", "傭車その他付帯手数料額", "乗務員コード１", "乗務員名１", "乗務員コード２", "乗務員名２", "乗務員コード３", "乗務員名３", "乗務員コード４",
                "乗務員名４", "乗務員コード５", "乗務員名５", "実車一般キロ", "実車高速キロ", "空車一般キロ", "空車高速キロ", "その他キロ", "総走行キロ", "燃料１", "燃料名１", "燃料２",
                "燃料名２", "燃料３", "燃料名３", "乗車人員", "プラス人員", "受付条件区分", "運行指示条件１", "運行指示条件２", "運行指示条件３", "運行指示条件４", "運行指示条件５", "受付営業所コード",
                "受付営業所名", "受付営業所略名", "営業担当者コード", "営業担当者名", "入力担当者コード", "入力担当者名", "団体コード", "団体区分名", "乗客区分コード", "乗客区分名", "キャンセル復活日付", "キャンセル復活理由",
                "キャンセル復活担当者コード", "キャンセル復活担当者名"
            };
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    table.Columns[i].ColumnName = listHeader[i];
                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        public void SetTableHeaderReservation(DataTable table)
        {
            try
            {
                List<string> listHeader = new List<string>() { "受付番号", "運行日連番", "予約区分", "予約区分名", "受付年月日", "確認総回数", "確定年月日", "得意先業者コード",
                "得意先コード", "得意先支店コード", "得意先業者名", "得意先名", "得意先支店名", "得意先略名", "得意先支店略名", "得意先担当者名", "得意先電話番号", "得意先ＦＡＸ番号", "得意先メールアドレス", "仕入先業者コード", "仕入先コード",
                "仕入先支店コード", "仕入先業者名", "仕入先名", "仕入先支店名", "仕入先略名", "仕入先支店略名", "団体名", "幹事氏名", "幹事住所１", "幹事住所２", "幹事電話番号", "幹事ＦＡＸ番号", "幹事携帯番号", "幹事メールアドレス",
                "幹事向けＤＭ発行フラグ", "行き先名", "配車年月日", "配車時間", "配車地分類コード", "配車地分類名", "配車地分類略名", "配車地コード",
                "配車地名", "到着年月日", "到着時間", "到着地分類コード", "到着地分類名", "到着地分類略名",
                "到着地コード", "到着地名", "出発時間", "運転手数", "ガイド数", "車種コード", "車種名", "型区分", "型区分略名", "車種台数", "運賃", "消費税額",
                "手数料額", "ガイド料売上額", "ガイド料消費税額", "ガイド料手数料額", "その他付帯売上額", "その他付帯消費税額", "その他付帯手数料額", "傭車台数", "傭車運賃", "傭車消費税額", "傭車手数料額", "傭車ガイド料発生額",
                "傭車ガイド料消費税額", "傭車ガイド料手数料額", "傭車その他付帯発生額", "傭車その他付帯消費税額", "傭車その他付帯手数料額", "乗車人員", "プラス人員", "請求区分", "請求区分略名", "請求年月日", "受付営業所コード", "受付営業所名", "受付営業所略名",
                "営業担当者コード", "営業担当者名", "入力担当者コード", "入力担当者名", "受付条件", "運行状態", "指示条件１", "指示条件２", "指示条件３", "指示条件４", "指示条件５", "発生地県名",
                "発生地コード", "発生地名", "エリア県名", "エリアコード", "エリア名", "団体区分コード", "団体区分名", "乗客区分コード", "乗客区分名", "備考名", "総走行時間", "点検時間",
                "調整時間", "深夜早朝時間", "総走行キロ", "実車時間", "実車キロ", "割引区分", "交替運転者拘束時間", "交替運転者深夜時間", "交替運転者走行キロ", "交替運転者配置料金フラグ", "特殊車両料金フラグ", "上限運賃",
                "下限運賃", "上限料金", "下限料金", "上限金額", "下限金額", "指数", "予約毎実車キロ", "予約毎総走行キロ", "予約毎実車時間", "予約毎総走行時間", "予約毎深夜早朝時間", "予約毎交替運転者配置料金フラグ",
                "予約毎特殊車両料金フラグ", "年間契約フラグ"
            };
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    table.Columns[i].ColumnName = listHeader[i];
                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        public void ValueCheckHaita(bool value)
        {
            if (!value)
            {
                ShowErrorPopup = true;
                ErrorMessage = Lang["BI_T028"];
            }
        }

        public async Task HaitaCallBack()
        {

        }

        protected async Task FirstLoad(int type)
        {
            switch (type)
            {
                case 1:
                    isCusFromFirstLoaded = true;
                    break;
                case 2:
                    isCusToFirstLoaded = true;
                    break;
                case 3:
                    isSupFromFirstLoaded = true;
                    break;
                case 4:
                    isSupToFirstLoaded = true;
                    break;
                default:
                    break;
            }
            if (isCusFromFirstLoaded && isCusToFirstLoaded && isSupFromFirstLoaded && isSupToFirstLoaded && !isFirstLoaded)
            {
                isFirstLoaded = true;
                ChangeState().Wait();
            }
        }

        public async Task OnDateInputChange(string ValueName, string value, EditContext formContext)
        {
            try
            {
                var date = CommonHelper.ConvertToDateTime(value);
                if (date == null)
                    return;
                await ChangeValueForm(ValueName, date, formContext);
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }
    }
}
