using HassyaAllrightCloud.Commons.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.BillPrint;
using Microsoft.AspNetCore.Hosting;
using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Components.Web;
using HassyaAllrightCloud.Commons.Helpers;
using System.IO;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using System.Data;
using HassyaAllrightCloud.Validation;
using HassyaAllrightCloud.Pages.Components;
using SharedLibraries.UI.Models;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Domain.Entities;
using Microsoft.AspNetCore.Components.Forms;
using BlazorContextMenu;
using HassyaAllrightCloud.Infrastructure.Services;
using HassyaAllrightCloud.IService.CommonComponents;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;

namespace HassyaAllrightCloud.Pages
{
    public class ReceivableListBase : ComponentBase
    {
        [Inject]
        protected IDepositCouponService depositCouponService { get; set; }
        [Inject]
        protected IStringLocalizer<ReceivableList> Lang { get; set; }
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }
        [Inject]
        public IWebHostEnvironment hostingEnvironment { get; set; }
        [Inject]
        public IReceivableListService ReceivableListService { get; set; }
        [Inject]
        public IDepositListService DepositListService { get; set; }
        [Inject]
        protected ILoadingService _loadingService { get; set; }
        [Inject]
        protected IErrorHandlerService errorModalService { get; set; }
        [Inject]
        protected IFilterCondition FilterConditionService { get; set; }
        [Inject]
        protected IGenerateFilterValueDictionary GenerateFilterValueDictionaryService { get; set; }
        [Inject]
        protected IGetFilterDataService GetFilterDataService { get; set; }
        [Inject]
        protected IBlazorContextMenuService blazorContextMenuService { get; set; }
        [Inject]
        protected AppSettingsService AppSettingsService { get; set; }
        [Inject] 
        protected IDepositListService depositListService { get; set; }

        public Dictionary<string, string> LangDic = new Dictionary<string, string>();
        public HeaderTemplate Header { get; set; }
        public HeaderTemplate HeaderBP { get; set; }
        public BodyTemplate Body { get; set; }
        public BodyTemplate BodyBP { get; set; }
        protected List<ReservationListDetaiGridDataModel> DataItems { get; set; } = new List<ReservationListDetaiGridDataModel>();
        protected List<BussinesPlanReceivableGridDataModel> DataItemsBP { get; set; } = new List<BussinesPlanReceivableGridDataModel>();
        protected bool dataNotFound { get; set; }
        protected bool dataNotFoundBP { get; set; }
        protected Pagination pagination { get; set; }
        protected Pagination paginationBP { get; set; }
        protected int gridSizeClass { get; set; } = (int)ViewMode.Medium;
        protected int gridSizeClassBP { get; set; } = (int)ViewMode.Medium;
        public int ActiveTabIndex
        {
            get => activeTabIndex;
            set
            {
                activeTabIndex = value;
                AdjustHeightWhenTabChanged();
            }
        }
        public int TotalCount { get; set; }
        public int TotalCountBP { get; set; }
        public int ActiveV { get; set; }
        public bool isOpenCharterInquiryPopUp { get; set; }
        public OutDataTable outDataTable { get; set; }
        public int GridModeActiveV { get; set; } = 1;
        protected bool isLoading { get; set; } = true;
        int activeTabIndex = 0;
        public string dateFormat = "yyyy/MM/dd";
        public List<string> Errors = new List<string>();
        protected bool btnReportActive = true;
        public bool EnablePreSaleBranch = false;
        public bool EnableNextSaleBranch = true;
        public bool EnablePreBillingPayment = false;
        public bool EnableNextBillingPayment = true;
        public ReceivablePaymentSummary Summary = new ReceivablePaymentSummary();
        public BusinessPlanReceivablePaymentSummary BusinessPlanSummary = new BusinessPlanReceivablePaymentSummary();
        public byte ItemPerPage = 25;
        public byte ItemPerPageBP = 25;
        public int TotalRow;
        public int TotalRowBP;
        public List<int> billingTypes { get; set; } = new List<int>();
        public ReceivableFilterModel ReceivableFilterModel = new ReceivableFilterModel();
        public List<CompanyData> CompanyDatas = new List<CompanyData>();
        public List<LoadSaleBranchList> SaleBranchs = new List<LoadSaleBranchList>();
        public List<SelectedSaleBranchModel> SelectedSaleBranchPayments = new List<SelectedSaleBranchModel>();
        public List<SelectedSaleBranchModel> InitSelectedSaleBranchPayments = new List<SelectedSaleBranchModel>();
        public List<SaleOfficeModel> SaleBranchTypes = new List<SaleOfficeModel>();
        public DateTime StartPaymentDate { get; set; }
        public DateTime EndPaymentDate { get; set; }
        public List<BillingAddressModel> BillingAddressList = new List<BillingAddressModel>();
        public List<SelectedPaymentAddressModel> SelectedBillingAddressPaymentList = new List<SelectedPaymentAddressModel>();
        public List<SelectedPaymentAddressModel> InitSelectedBillingAddressPaymentList = new List<SelectedPaymentAddressModel>();
        public List<ReservationCategoryModel> ReservationTypes = new List<ReservationCategoryModel>();
        public List<CheckBoxFilter> checkBoxBillingTypes = new List<CheckBoxFilter>();
        public List<string> BillingTypeNames = new List<string>();
        public List<TransferBankModel> TransferBanks = new List<TransferBankModel>();
        public List<DepositOutputClass> DepositOutputs = new List<DepositOutputClass>();
        public CharterSettingModel CharterSetting = new CharterSettingModel();
        public List<ReservationListDetaiGridDataModel> ReceivableGridData = new List<ReservationListDetaiGridDataModel>();
        public List<ReservationListDetaiGridDataModel> InitReceivableGridData = new List<ReservationListDetaiGridDataModel>();
        public List<BussinesPlanReceivableGridDataModel> BussinesPlanReceivableGridData = new List<BussinesPlanReceivableGridDataModel>();
        public List<BussinesPlanReceivableGridDataModel> InitBussinesPlanReceivableGridData = new List<BussinesPlanReceivableGridDataModel>();
        public string FormName = FormFilterName.ReceivableList;
        Dictionary<string, string> keyValueFilterPairs = new Dictionary<string, string>();
        public List<ComboboxFixField> BillTypePagePrintData = new List<ComboboxFixField>();
        public List<ComboboxFixField> DelimiterTypeData = new List<ComboboxFixField>();
        public ReservationListDetaiGridDataModel CheckedItem = new ReservationListDetaiGridDataModel();
        public ReservationListDetaiGridDataModel SelectedItem { get; set; }
        public string errorMessage { get; set; } = string.Empty;
        public List<ReservationClassComponentData> ListReservationClass { get; set; } = new List<ReservationClassComponentData>();
        protected List<CustomerComponentGyosyaData> ListGyosya { get; set; } = new List<CustomerComponentGyosyaData>();
        protected List<CustomerComponentTokiskData> ListTokisk { get; set; } = new List<CustomerComponentTokiskData>();
        protected List<CustomerComponentTokiStData> ListTokiSt { get; set; } = new List<CustomerComponentTokiStData>();

        public int LastXClicked { get; set; }
        public int LastYClicked { get; set; }
        public EditContext searchForm { get; set; }
        public Components.CommonComponents.CustomerComponent startCustomerComponent { get; set; }
        = new Components.CommonComponents.CustomerComponent();
        public Components.CommonComponents.CustomerComponent endCustomerComponent { get; set; }
        = new Components.CommonComponents.CustomerComponent();
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var dataLang = Lang.GetAllStrings();
                LangDic = dataLang.ToDictionary(l => l.Name, l => l.Value);
                InitHeader();
                InitBody();
                InitHeaderBP();
                InitBodyBP();
                BillTypePagePrintData.Add(new ComboboxFixField()
                {
                    IdValue = 0,
                    StringValue = Lang["PageType_0"]
                });
                BillTypePagePrintData.Add(new ComboboxFixField()
                {
                    IdValue = 1,
                    StringValue = Lang["PageType_1"]
                });
                BillTypePagePrintData.Add(new ComboboxFixField()
                {
                    IdValue = 2,
                    StringValue = Lang["PageType_2"]
                });
                ReceivableFilterModel.ReportPageSize = BillTypePagePrintData[0];

                // Add data for output type
                ShowHeaderOptions.ShowHeaderOptionData[0].StringValue = Lang["OutType_" + ShowHeaderOptions.ShowHeaderOptionData[0].IdValue.ToString()];
                ShowHeaderOptions.ShowHeaderOptionData[1].StringValue = Lang["OutType_" + ShowHeaderOptions.ShowHeaderOptionData[1].IdValue.ToString()];
                ReceivableFilterModel.ActiveHeaderOption = ShowHeaderOptions.ShowHeaderOptionData[0];

                // Add data for group type print
                GroupTypes.GroupTypeData[0].StringValue = Lang["GroupType_" + GroupTypes.GroupTypeData[0].IdValue.ToString()];
                GroupTypes.GroupTypeData[1].StringValue = Lang["GroupType_" + GroupTypes.GroupTypeData[1].IdValue.ToString()];
                ReceivableFilterModel.GroupType = GroupTypes.GroupTypeData[0];

                // Add data for delimiter type print
                DelimiterTypes.DelimiterTypeData[0].StringValue = Lang["DelimiterType_" + DelimiterTypes.DelimiterTypeData[0].IdValue.ToString()];
                DelimiterTypes.DelimiterTypeData[1].StringValue = Lang["DelimiterType_" + DelimiterTypes.DelimiterTypeData[1].IdValue.ToString()];
                DelimiterTypes.DelimiterTypeData[2].StringValue = Lang["DelimiterType_" + DelimiterTypes.DelimiterTypeData[2].IdValue.ToString()];
                DelimiterTypeData.Add(DelimiterTypes.DelimiterTypeData[0]);
                DelimiterTypeData.Add(DelimiterTypes.DelimiterTypeData[1]);
                DelimiterTypeData.Add(DelimiterTypes.DelimiterTypeData[2]);
                ReceivableFilterModel.DelimiterType = DelimiterTypes.DelimiterTypeData[2];

                ReceivableUnpaidList.ReceivableUnpaids[0].StringValue = "";
                ReceivableUnpaidList.ReceivableUnpaids[1].StringValue = Lang["Unpaid1"];
                ReceivableUnpaidList.ReceivableUnpaids[2].StringValue = Lang["Unpaid2"];
                ReceivableUnpaidList.ReceivableUnpaids[3].StringValue = Lang["Unpaid3"];
                ReceivableFilterModel.UnpaidSelection = ReceivableUnpaidList.ReceivableUnpaids[0];
                ReceivableFilterModel.PageSize = 25;
                ReceivableFilterModel.PageSizeBP = 25;
                ReceivableFilterModel.ReceivableReport = 1;
                GenerateDepositOutputTemplate();
                ActiveV = (int)ViewMode.Medium;
                GridModeActiveV = (int)ReceiableGridMode.Detail;
                GenerateAllCheckBoxName();
                GenerateCheckBoxBillingType(7, BillingTypeNames);
                CompanyDatas = DepositListService.GetCompanyData(new ClaimModel().TenantID).Result;
                ReceivableFilterModel.CompanyData = CompanyDatas.Where(x => x.CompanyCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID).FirstOrDefault();
                CompanyDatas.Insert(0, null);
                SaleBranchs = DepositListService.GetSaleBranchData(new ClaimModel().TenantID).Result;
                SaleBranchs.Insert(0, null);
                SaleBranchTypes = DepositListService.GetSaleOfficeModel();
                var removedSaleBranch = SaleBranchTypes.Where(x => x.SaleOfficeKbn == 3).FirstOrDefault();
                SaleBranchTypes.Remove(removedSaleBranch);
                ReceivableFilterModel.SaleOfficeType = SaleBranchTypes.FirstOrDefault();
                BillingAddressList = DepositListService.GetBillingAddresses(new ClaimModel().TenantID).Result;
                BillingAddressList.Insert(0, null);
                ReservationTypes = DepositListService.GetReservationCategories(new ClaimModel().TenantID).Result;
                ReservationTypes.Insert(0, null);
                TransferBanks = DepositListService.GetTransferBanks().Result;
                TransferBanks.Insert(0, null);
                ReceivableFilterModel.PageSize = 25;
                ReceivableFilterModel.PageSizeBP = 25;
                ReceivableFilterModel.ReportPageSize = BillTypePagePrintData[0];
                InitSelectedSaleBranchPayments = ReceivableListService.GetSelectedSaleBranches(ReceivableFilterModel, new ClaimModel().TenantID, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID).Result;
                InitSelectedBillingAddressPaymentList = ReceivableListService.GetSelectedPaymentAddresses(ReceivableFilterModel, new ClaimModel().TenantID, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID).Result;
                searchForm = new EditContext(ReceivableFilterModel);
                ListReservationClass = await depositListService.GetListReservationClass();
                ListGyosya = await depositListService.GetListGyosya();
                ListTokisk = await depositListService.GetListTokisk();
                ListTokiSt = await depositListService.GetListTokiSt();
                List<TkdInpCon> filterValues = FilterConditionService.GetFilterCondition(FormName, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq).Result;
                if (filterValues.Count > 0)
                {
                    ReceivableFilterModel = GenerateFilterModel(filterValues);
                }
                ReceivableFilterModel.PageSize = 25;
                ReceivableFilterModel.PageSizeBP = 25;
                searchForm = new EditContext(ReceivableFilterModel);
                
                GenerateSelectedSaleBranch();
                GenerateSelectedBillingAddressPayment();
                GenerateData();
                RenderPreNextButton();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        private ReceivableFilterModel GenerateFilterModel(List<TkdInpCon> filterValues)
        {
            ReceivableFilterModel result = new ReceivableFilterModel();
            foreach(var item in filterValues)
            {
                switch (item.ItemNm)
                {
                    case nameof(ReceivableFilterModel.CompanyData):
                        result.CompanyData = string.IsNullOrWhiteSpace(item.JoInput) ? CompanyDatas.Where(x => x != null && x.CompanyCdSeq == new ClaimModel().CompanyID).FirstOrDefault() : CompanyDatas.Where(x => x!= null && x.CompanyCdSeq == Int32.Parse(item.JoInput)).FirstOrDefault();
                        break;
                    case nameof(ReceivableFilterModel.StartSaleBranchList):
                        result.StartSaleBranchList = string.IsNullOrWhiteSpace(item.JoInput) ? null : SaleBranchs.Where(x => x != null && x.EigyoCdSeq == Int32.Parse(item.JoInput)).FirstOrDefault();
                        break;
                    case nameof(ReceivableFilterModel.EndSaleBranchList):
                        result.EndSaleBranchList = string.IsNullOrWhiteSpace(item.JoInput) ? null : SaleBranchs.Where(x => x != null && x.EigyoCdSeq == Int32.Parse(item.JoInput)).FirstOrDefault();
                        break;
                    case nameof(ReceivableFilterModel.SaleOfficeType):
                        result.SaleOfficeType = string.IsNullOrWhiteSpace(item.JoInput) ? null : SaleBranchTypes.Where(x => x != null && x.SaleOfficeKbn == Int32.Parse(item.JoInput)).FirstOrDefault();
                        break;
                    case nameof(ReceivableFilterModel.StartPaymentDate):
                        if (string.IsNullOrWhiteSpace(item.JoInput))
                        {
                            result.StartPaymentDate = null;
                        }
                        else
                        {
                            result.StartPaymentDate = DateTime.ParseExact(item.JoInput, "yyyyMMdd", null);
                        }
                        break;
                    case nameof(ReceivableFilterModel.EndPaymentDate):
                        if (string.IsNullOrWhiteSpace(item.JoInput))
                        {
                            result.EndPaymentDate = null;
                        }
                        else
                        {
                            result.EndPaymentDate = DateTime.ParseExact(item.JoInput, "yyyyMMdd", null);
                        }
                        break;
                    case nameof(ReceivableFilterModel.startCustomerComponentGyosyaData):
                        if (!string.IsNullOrWhiteSpace(item.JoInput))
                        {
                            var gyosya = ListGyosya.Where(x => x.GyosyaCdSeq == int.Parse(item.JoInput)).FirstOrDefault();
                            result.startCustomerComponentGyosyaData = gyosya;
                            startCustomerComponent.DefaultGyosya = gyosya.GyosyaCdSeq;
                        }
                        break;
                    case nameof(ReceivableFilterModel.endCustomerComponentGyosyaData):
                        if (!string.IsNullOrWhiteSpace(item.JoInput))
                        {
                            var gyosya = ListGyosya.Where(x => x.GyosyaCdSeq == int.Parse(item.JoInput)).FirstOrDefault();
                            result.endCustomerComponentGyosyaData = gyosya;
                            endCustomerComponent.DefaultGyosya = gyosya.GyosyaCdSeq;
                        }
                        break;
                    case nameof(ReceivableFilterModel.startCustomerComponentTokiskData):
                        if (!string.IsNullOrWhiteSpace(item.JoInput))
                        {
                            var tokisk = ListTokisk.Where(x => x.TokuiSeq == int.Parse(item.JoInput)).FirstOrDefault();
                            result.startCustomerComponentTokiskData = tokisk;
                            startCustomerComponent.DefaultTokisk = tokisk.TokuiSeq;
                        }
                        break;
                    case nameof(ReceivableFilterModel.endCustomerComponentTokiskData):
                        if (!string.IsNullOrWhiteSpace(item.JoInput))
                        {
                            var tokisk = ListTokisk.Where(x => x.TokuiSeq == int.Parse(item.JoInput)).FirstOrDefault();
                            result.endCustomerComponentTokiskData = tokisk;
                            endCustomerComponent.DefaultTokisk = tokisk.TokuiSeq;
                        }
                        break;
                    case nameof(ReceivableFilterModel.startCustomerComponentTokiStData):
                        if (!string.IsNullOrWhiteSpace(item.JoInput))
                        {
                            var tokist = ListTokiSt.Where(x => x.TokuiSeq.ToString() + x.SitenCdSeq.ToString() == item.JoInput).FirstOrDefault();
                            result.startCustomerComponentTokiStData = tokist;
                            startCustomerComponent.DefaultTokiSt = tokist.SitenCdSeq;
                        }
                        break;
                    case nameof(ReceivableFilterModel.endCustomerComponentTokiStData):
                        if (!string.IsNullOrWhiteSpace(item.JoInput))
                        {
                            var tokist = ListTokiSt.Where(x => x.TokuiSeq.ToString() + x.SitenCdSeq.ToString() == item.JoInput).FirstOrDefault();
                            result.endCustomerComponentTokiStData = tokist;
                            endCustomerComponent.DefaultTokiSt = tokist.SitenCdSeq;
                        }
                        break;
                    case nameof(ReceivableFilterModel.StartReceiptNumber):
                        result.StartReceiptNumber = string.IsNullOrWhiteSpace(item.JoInput) ? null : item.JoInput;
                        break;
                    case nameof(ReceivableFilterModel.EndReceiptNumber):
                        result.EndReceiptNumber = string.IsNullOrWhiteSpace(item.JoInput) ? null : item.JoInput;
                        break;
                    case nameof(ReceivableFilterModel.StartReservationClassification):
                        if (!string.IsNullOrWhiteSpace(item.JoInput))
                        {
                            var reservationClassification = ListReservationClass.Where(x => x.YoyaKbnSeq == int.Parse(item.JoInput)).FirstOrDefault();
                            result.StartReservationClassification = reservationClassification;
                        }
                        break;
                    case nameof(ReceivableFilterModel.EndReservationClassification):
                        if (!string.IsNullOrWhiteSpace(item.JoInput))
                        {
                            var reservationClassification = ListReservationClass.Where(x => x.YoyaKbnSeq == int.Parse(item.JoInput)).FirstOrDefault();
                            result.EndReservationClassification = reservationClassification;
                        }
                        break;
                    case nameof(ReceivableFilterModel.BillingType):
                        result.BillingType = string.IsNullOrWhiteSpace(item.JoInput) ? null : item.JoInput;
                        if (!string.IsNullOrWhiteSpace(item.JoInput))
                        {
                            var types = item.JoInput.Split(',');
                            foreach (var word in types)
                            {
                                foreach (var type in checkBoxBillingTypes)
                                {
                                    if (type.Id == word)
                                    {
                                        type.IsChecked = true;
                                    }
                                    if(type.IsChecked == true && !billingTypes.Contains(Int32.Parse(type.Id)))
                                    {
                                        billingTypes.Add(Int32.Parse(type.Id));
                                    }
                                }
                            }
                        }
                        break;
                    case nameof(ReceivableFilterModel.UnpaidSelection):
                        result.UnpaidSelection = string.IsNullOrWhiteSpace(item.JoInput) ? ReceivableUnpaidList.ReceivableUnpaids[0] : ReceivableUnpaidList.ReceivableUnpaids[Int32.Parse(item.JoInput)];
                        break;
                    case nameof(ReceivableFilterModel.PaymentDate):
                        if (string.IsNullOrWhiteSpace(item.JoInput))
                        {
                            result.PaymentDate = null;
                        }
                        else
                        {
                            result.PaymentDate = DateTime.ParseExact(item.JoInput, "yyyyMMdd", null);
                        }
                        break;
                    case nameof(ReceivableFilterModel.ReportPageSize):
                        ComboboxFixField pageSizeReport = string.IsNullOrEmpty(item.JoInput) ? BillTypePagePrintData[0] : BillTypePagePrintData.Where(x => x.StringValue == item.JoInput).FirstOrDefault();
                        result.ReportPageSize = pageSizeReport;
                        break;
                    case nameof(ReceivableFilterModel.ActiveHeaderOption):
                        ComboboxFixField activeHeaderOption = string.IsNullOrEmpty(item.JoInput) ? ShowHeaderOptions.ShowHeaderOptionData[0] : ShowHeaderOptions.ShowHeaderOptionData.Where(x => x.StringValue == item.JoInput).FirstOrDefault();
                        result.ActiveHeaderOption = activeHeaderOption;
                        break;
                    case nameof(ReceivableFilterModel.GroupType):
                        ComboboxFixField groupType = string.IsNullOrEmpty(item.JoInput) ? GroupTypes.GroupTypeData[0] : GroupTypes.GroupTypeData.Where(x => x.StringValue == item.JoInput).FirstOrDefault();
                        result.GroupType = groupType;
                        break;
                    case nameof(ReceivableFilterModel.DelimiterType):
                        ComboboxFixField delimiterType = string.IsNullOrEmpty(item.JoInput) ? DelimiterTypes.DelimiterTypeData[2] : DelimiterTypes.DelimiterTypeData.Where(x => x.StringValue == item.JoInput).FirstOrDefault();
                        result.DelimiterType = delimiterType;
                        break;
                    case nameof(ReceivableFilterModel.OutputType):
                        OutputReportType outputType = string.IsNullOrEmpty(item.JoInput) ? (OutputReportType)1 : (OutputReportType)int.Parse(item.JoInput);
                        result.OutputType = outputType;
                        break;
                    case nameof(ReceivableFilterModel.ReceivableReport):
                        result.ReceivableReport = string.IsNullOrEmpty(item.JoInput) ? 1 : int.Parse(item.JoInput);
                        GridModeActiveV = result.ReceivableReport;
                        break;
                }
            }
            return result;
        }
        protected async Task ResetForm()
        {
            try
            {
                ActiveV = (int)ViewMode.Medium;
                GridModeActiveV = (int)ReceiableGridMode.Detail;
                InitBody();
                InitBodyBP();
                InitHeader();
                InitHeaderBP();
                FilterConditionService.DeleteFilterCondition(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, 0, FormName).Wait();
                await Task.Run(() =>
                {
                    isLoading = true;
                    InvokeAsync(StateHasChanged);
                });
                Errors = new List<string>();
                ItemPerPage = 25;
                ItemPerPageBP = 25;
                ReceivableFilterModel = new ReceivableFilterModel();
                ReceivableFilterModel.ReportPageSize = BillTypePagePrintData[0];
                ReceivableFilterModel.ActiveHeaderOption = ShowHeaderOptions.ShowHeaderOptionData[0];
                ReceivableFilterModel.GroupType = GroupTypes.GroupTypeData[0];
                ReceivableFilterModel.DelimiterType = DelimiterTypes.DelimiterTypeData[2];
                ReceivableFilterModel.UnpaidSelection = ReceivableUnpaidList.ReceivableUnpaids[0];
                ReceivableFilterModel.PageSize = 25;
                ReceivableFilterModel.PageSizeBP = 25;
                ReceivableFilterModel.ReceivableReport = 1;
                ReceivableFilterModel.SaleOfficeType = SaleBranchTypes.FirstOrDefault();
                SelectedSaleBranchPayments = InitSelectedSaleBranchPayments;
                ReceivableFilterModel.SelectedSaleBranchPayment = SelectedSaleBranchPayments.FirstOrDefault();
                SelectedBillingAddressPaymentList = InitSelectedBillingAddressPaymentList;
                ReceivableFilterModel.SelectedBillingAddressPayment = SelectedBillingAddressPaymentList.FirstOrDefault();
                ReceivableFilterModel.CompanyData = CompanyDatas.Where(x => x != null && x.CompanyCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID).FirstOrDefault();
                foreach(var item in checkBoxBillingTypes)
                {
                    item.IsChecked = false;
                }
                searchForm = new EditContext(ReceivableFilterModel);
                GenerateSelectedSaleBranch();
                GenerateSelectedBillingAddressPayment();
                ReceivableFilterModel.PageNum = 0;
                ReceivableFilterModel.PageSize = 25;
                ReceivableFilterModel.PageNumBP = 0;
                ReceivableFilterModel.PageSizeBP = 25;
                pagination.currentPage = 0;
                if (ReceivableFilterModel.SelectedSaleBranchPayment != null && ReceivableFilterModel.SelectedBillingAddressPayment != null)
                {
                    (ReceivableGridData, Summary, TotalRow) = ReceivableListService.GetReceivableGridData(ReceivableFilterModel, false, ReceivableFilterModel.PageNum, ReceivableFilterModel.PageSize, new ClaimModel().TenantID, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID).Result;
                    dataNotFound = ReceivableGridData == null || ReceivableGridData.Count == 0;
                    DataItems = ReceivableGridData;
                    (BussinesPlanReceivableGridData, BusinessPlanSummary, TotalRowBP) = ReceivableListService.GetPlanReceivableGridDatas(ReceivableFilterModel, false, ReceivableFilterModel.PageNumBP, ReceivableFilterModel.PageSizeBP, new ClaimModel().TenantID, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID).Result;
                    dataNotFoundBP = BussinesPlanReceivableGridData == null || BussinesPlanReceivableGridData.Count == 0;
                    DataItemsBP = BussinesPlanReceivableGridData;
                    btnReportActive = true;
                }
                else
                {
                    ReceivableGridData = new List<ReservationListDetaiGridDataModel>();
                    BussinesPlanReceivableGridData = new List<BussinesPlanReceivableGridDataModel>();
                    TotalRow = 0;
                    TotalRowBP = 0;
                    DataItems = ReceivableGridData;
                    DataItemsBP = BussinesPlanReceivableGridData;
                    Summary = new ReceivablePaymentSummary();
                    BusinessPlanSummary = new BusinessPlanReceivablePaymentSummary();
                    btnReportActive = false;
                }
                await Task.Run(() =>
                {
                    isLoading = false;
                    InvokeAsync(StateHasChanged);
                });
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }
        private void InitHeader()
        {
            try
            {
                Header = new HeaderTemplate()
                {
                    StickyCount = 2,
                    Rows = new List<RowHeaderTemplate>()
                {
                    new RowHeaderTemplate()
                    {
                        Columns = new List<ColumnHeaderTemplate>()
                        {
                            new ColumnHeaderTemplate() { ColName = Lang["NoGrid"], RowSpan = 2,Width = 50 },
                            new ColumnHeaderTemplate() { ColName = Lang["ReceiptNumberGrid"],Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["CustomerNameGrid"], RowSpan = 2,Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["BillingDateGrid"], RowSpan = 2,Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["OperationScheduleSerialNumberGrid"], RowSpan = 2,Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["GroupNameGrid"],Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["DeliveryDateGrid"],Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["BillingTypeGrid"], RowSpan = 2,Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["LodingGoodsNameGrid"],Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["QuantityGrid"],Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["SalesAmountGrid"],RowSpan = 2, Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["TaxRateGrid"], Width = 150 , RowSpan = 2},
                            new ColumnHeaderTemplate() { ColName = Lang["FeeRateGrid"], Width = 150 ,  RowSpan = 2},
                            new ColumnHeaderTemplate() { ColName = Lang["BillingAmountGrid"],RowSpan = 2, Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["DepositAmountGrid"],RowSpan = 2, Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["UnpaidAmountGrid"],RowSpan = 2, Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["CouponAmountGrid"],RowSpan = 2, Width = 150 },
                        }
                    },
                    new RowHeaderTemplate()
                    {
                        Columns = new List<ColumnHeaderTemplate>()
                        {
                            new ColumnHeaderTemplate() { ColName = Lang["ReceptionOfficeGrid"], Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["DestionationNameGrid"], Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["ArrivalDateGrid"], Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["PaymentNameGrid"], Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["UnitPriceGrid"], Width = 150 },
                        }
                    }
                }
                };
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }
        private void InitHeaderBP()
        {
            try
            {
                HeaderBP = new HeaderTemplate()
                {
                    StickyCount = 1,
                    Rows = new List<RowHeaderTemplate>()
                {
                    new RowHeaderTemplate()
                    {
                        Columns = new List<ColumnHeaderTemplate>()
                        {
                            new ColumnHeaderTemplate() { ColName = Lang["NoGrid"], RowSpan = 2,Width = 50 },
                            new ColumnHeaderTemplate() { ColName = Lang["PaymentAddressBP"],RowSpan = 2,Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["TicketPriceBP"], ColSpan = 5,Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["GuideFeeBP"], ColSpan = 5,Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["OtherBP"], ColSpan = 5,Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["CancelBP"], ColSpan = 4,Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["SubTotalBP"], RowSpan = 2,Width = 150 },
                        }
                    },
                    new RowHeaderTemplate()
                    {
                        Columns = new List<ColumnHeaderTemplate>()
                        {
                            new ColumnHeaderTemplate() { ColName = Lang["AmountBP"], Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["TaxAmountBP"], Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["FeeAmountBP"], Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["DepositAmountBP"], Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["UnpaidAmountBP"], Width = 150 },

                            new ColumnHeaderTemplate() { ColName = Lang["SaleAmount"], Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["TaxAmountBP"], Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["FeeAmountBP"], Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["DepositAmountBP"], Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["UnpaidAmountBP"], Width = 150 },

                            new ColumnHeaderTemplate() { ColName = Lang["SaleAmount"], Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["TaxAmountBP"], Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["FeeAmountBP"], Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["DepositAmountBP"], Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["UnpaidAmountBP"], Width = 150 },

                            new ColumnHeaderTemplate() { ColName = Lang["AmountBP"], Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["TaxAmountBP"], Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["DepositAmountBP"], Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["UnpaidAmountBP"], Width = 150 },
                        }
                    }
                }
                };
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }
        private void InitBody()
        {
            try
            {
                Body = new BodyTemplate()
                {
                    CustomCssDelegate = CustomRowCss,
                    Rows = new List<RowBodyTemplate>()
                {
                    new RowBodyTemplate()
                    {
                        Columns = new List<ColumnBodyTemplate>()
                        {
                            new ColumnBodyTemplate() { Control = new MultiLineControl<ReservationListDetaiGridDataModel> { MutilineText = GetNo }, RowSpan= 2, AlignCol = AlignColEnum.Center },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(ReservationListDetaiGridDataModel.ReceiptNumber)},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(ReservationListDetaiGridDataModel.SeiRyakuNm)},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(ReservationListDetaiGridDataModel.BillingDate), CustomTextFormatDelegate = KoboGridHelper.FormatYYYYMMDDDelegate, RowSpan = 2 , AlignCol = AlignColEnum.Center},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(ReservationListDetaiGridDataModel.OperationScheduleSerialNumber), RowSpan = 2, AlignCol = AlignColEnum.Center },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(ReservationListDetaiGridDataModel.GroupName) },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(ReservationListDetaiGridDataModel.DeliveryDate), CustomTextFormatDelegate = KoboGridHelper.FormatYYYYMMDDDelegate, AlignCol = AlignColEnum.Center },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(ReservationListDetaiGridDataModel.BillingType), RowSpan = 2, AlignCol = AlignColEnum.Left },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(ReservationListDetaiGridDataModel.LodingGoodsName) },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(ReservationListDetaiGridDataModel.Quantity),AlignCol = AlignColEnum.Right},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(ReservationListDetaiGridDataModel.SalesAmount),  RowSpan = 2,AlignCol = AlignColEnum.Right, CustomTextFormatDelegate = KoboGridHelper.AddCommasForLongNumber},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(ReservationListDetaiGridDataModel.TaxRate),AlignCol = AlignColEnum.Right},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(ReservationListDetaiGridDataModel.FeeRate),AlignCol = AlignColEnum.Right},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(ReservationListDetaiGridDataModel.BillingAmount),  RowSpan = 2,AlignCol = AlignColEnum.Right, CustomTextFormatDelegate = KoboGridHelper.AddCommasForLongNumber},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(ReservationListDetaiGridDataModel.DepositAmount),  RowSpan = 2,AlignCol = AlignColEnum.Right, CustomTextFormatDelegate = KoboGridHelper.AddCommasForLongNumber},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(ReservationListDetaiGridDataModel.UnpaidAmount),  RowSpan = 2,AlignCol = AlignColEnum.Right, CustomTextFormatDelegate = KoboGridHelper.AddCommasForLongNumber},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(ReservationListDetaiGridDataModel.CouponAmount),  RowSpan = 2,AlignCol = AlignColEnum.Right, CustomTextFormatDelegate = KoboGridHelper.AddCommasForLongNumber}
                        }
                    },
                     new RowBodyTemplate()
                    {
                        Columns = new List<ColumnBodyTemplate>()
                        {
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(ReservationListDetaiGridDataModel.ReceptionOffice)},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(ReservationListDetaiGridDataModel.SeiSitRyakuNm)},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(ReservationListDetaiGridDataModel.DestinationName)},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(ReservationListDetaiGridDataModel.ArrivalDate), CustomTextFormatDelegate = KoboGridHelper.FormatYYYYMMDDDelegate, AlignCol = AlignColEnum.Center},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(ReservationListDetaiGridDataModel.PaymentName)},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(ReservationListDetaiGridDataModel.UnitPrice),AlignCol = AlignColEnum.Right},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(ReservationListDetaiGridDataModel.TaxAmount),AlignCol = AlignColEnum.Right, CustomTextFormatDelegate = KoboGridHelper.AddCommasForLongNumber},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(ReservationListDetaiGridDataModel.FeeAmount),AlignCol = AlignColEnum.Right, CustomTextFormatDelegate = KoboGridHelper.AddCommasForLongNumber},
                        }
                    }
                }
                };
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }

        private IEnumerable<string> GetNo(ReservationListDetaiGridDataModel item)
        {
            if(item.YouKbn == 2) {
                yield return item.No;
                yield return Lang["rent"];
            }
            else
            {
                yield return item.No;
                yield return string.Empty;
            }
        }

        private void InitBodyBP()
        {
            try
            {
                BodyBP = new BodyTemplate()
                {
                    Rows = new List<RowBodyTemplate>()
                {
                    new RowBodyTemplate()
                    {
                        Columns = new List<ColumnBodyTemplate>()
                        {
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(BussinesPlanReceivableGridDataModel.No), RowSpan = 2, AlignCol = AlignColEnum.Center },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(BussinesPlanReceivableGridDataModel.SeikyuCd)},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(BussinesPlanReceivableGridDataModel.UnUriGakKin),  RowSpan = 2,AlignCol = AlignColEnum.Right, CustomTextFormatDelegate = KoboGridHelper.AddCommasForLongNumber},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(BussinesPlanReceivableGridDataModel.UnSyaRyoSyo),  RowSpan = 2,AlignCol = AlignColEnum.Right, CustomTextFormatDelegate = KoboGridHelper.AddCommasForLongNumber},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(BussinesPlanReceivableGridDataModel.UnSyaRyoTes),  RowSpan = 2,AlignCol = AlignColEnum.Right, CustomTextFormatDelegate = KoboGridHelper.AddCommasForLongNumber},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(BussinesPlanReceivableGridDataModel.UnNyukinG),  RowSpan = 2,AlignCol = AlignColEnum.Right, CustomTextFormatDelegate = KoboGridHelper.AddCommasForLongNumber},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(BussinesPlanReceivableGridDataModel.UnMisyuG),  RowSpan = 2,AlignCol = AlignColEnum.Right, CustomTextFormatDelegate = KoboGridHelper.AddCommasForLongNumber},

                            new ColumnBodyTemplate() { DisplayFieldName = nameof(BussinesPlanReceivableGridDataModel.GaUriGakKin),  RowSpan = 2,AlignCol = AlignColEnum.Right, CustomTextFormatDelegate = KoboGridHelper.AddCommasForLongNumber},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(BussinesPlanReceivableGridDataModel.GaSyaRyoSyo),  RowSpan = 2,AlignCol = AlignColEnum.Right, CustomTextFormatDelegate = KoboGridHelper.AddCommasForLongNumber},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(BussinesPlanReceivableGridDataModel.GaSyaRyoTes),  RowSpan = 2,AlignCol = AlignColEnum.Right, CustomTextFormatDelegate = KoboGridHelper.AddCommasForLongNumber},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(BussinesPlanReceivableGridDataModel.GaNyukinG),  RowSpan = 2,AlignCol = AlignColEnum.Right, CustomTextFormatDelegate = KoboGridHelper.AddCommasForLongNumber},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(BussinesPlanReceivableGridDataModel.GaMisyuG),  RowSpan = 2,AlignCol = AlignColEnum.Right, CustomTextFormatDelegate = KoboGridHelper.AddCommasForLongNumber},

                            new ColumnBodyTemplate() { DisplayFieldName = nameof(BussinesPlanReceivableGridDataModel.SoUriGakKin),  RowSpan = 2,AlignCol = AlignColEnum.Right, CustomTextFormatDelegate = KoboGridHelper.AddCommasForLongNumber},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(BussinesPlanReceivableGridDataModel.SoSyaRyoSyo),  RowSpan = 2,AlignCol = AlignColEnum.Right, CustomTextFormatDelegate = KoboGridHelper.AddCommasForLongNumber},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(BussinesPlanReceivableGridDataModel.SoSyaRyoTes),  RowSpan = 2,AlignCol = AlignColEnum.Right, CustomTextFormatDelegate = KoboGridHelper.AddCommasForLongNumber},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(BussinesPlanReceivableGridDataModel.SoNyukinG),  RowSpan = 2,AlignCol = AlignColEnum.Right, CustomTextFormatDelegate = KoboGridHelper.AddCommasForLongNumber},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(BussinesPlanReceivableGridDataModel.SoMisyuG),  RowSpan = 2,AlignCol = AlignColEnum.Right, CustomTextFormatDelegate = KoboGridHelper.AddCommasForLongNumber},

                            new ColumnBodyTemplate() { DisplayFieldName = nameof(BussinesPlanReceivableGridDataModel.CaUriGakKin),  RowSpan = 2,AlignCol = AlignColEnum.Right, CustomTextFormatDelegate = KoboGridHelper.AddCommasForLongNumber},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(BussinesPlanReceivableGridDataModel.CaSyaRyoSyo),  RowSpan = 2,AlignCol = AlignColEnum.Right, CustomTextFormatDelegate = KoboGridHelper.AddCommasForLongNumber},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(BussinesPlanReceivableGridDataModel.CaNyukinG),  RowSpan = 2,AlignCol = AlignColEnum.Right, CustomTextFormatDelegate = KoboGridHelper.AddCommasForLongNumber},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(BussinesPlanReceivableGridDataModel.CaMisyuG),  RowSpan = 2,AlignCol = AlignColEnum.Right, CustomTextFormatDelegate = KoboGridHelper.AddCommasForLongNumber},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(BussinesPlanReceivableGridDataModel.MisyuG),  RowSpan = 2,AlignCol = AlignColEnum.Right, CustomTextFormatDelegate = KoboGridHelper.AddCommasForLongNumber},
                        }
                    },
                     new RowBodyTemplate()
                    {
                        Columns = new List<ColumnBodyTemplate>()
                        {
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(BussinesPlanReceivableGridDataModel.SeikyuNm)},
                        }
                    }
                }
                };
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }

        private Func<object, string> CustomRowCss = (item) =>
        {
            var model = item as ReservationListDetaiGridDataModel;
            var cssClass = "";
            if (model.NyuKinKbn == 2)
            {
                cssClass = "deposited";
            }
            else
            {
                if (model.NCouKbn == 2)
                {
                    cssClass = "coupon";
                }
                else
                {
                    if (model.NCouKbn == 3)
                    {
                        cssClass = "some";
                    }
                    else
                    {
                        if (model.NyuKinKbn == 3)
                        {
                            cssClass = "partially-included";
                        }
                        else
                        {
                            if (model.NyuKinKbn == 4)
                            {
                                cssClass = "overpayment";
                            }
                        }
                    }
                }
            }
            return cssClass;
        };
        protected OutputReportType ActiveButtonReport
        {
            get => ReceivableFilterModel.OutputType;
            set
            {
                ReceivableFilterModel.OutputType = value;
                StateHasChanged();
            }
        }
        protected void OnChangeItemPerPage(byte _itemPerPage)
        {
            ItemPerPage = _itemPerPage;
            ReceivableFilterModel.PageSize = ItemPerPage;
            StateHasChanged();
        }
        protected void OnChangeItemPerPageBP(byte _itemPerPage)
        {
            ItemPerPageBP = _itemPerPage;
            ReceivableFilterModel.PageSizeBP = ItemPerPageBP;
            StateHasChanged();
        }
        public void clickV(MouseEventArgs e, int number)
        {
            try
            {
                gridSizeClass = number;
                ActiveV = number;
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }
        protected async Task PageChanged(int pageNum)
        {
            try
            {

            ReceivableFilterModel.PageNum = pageNum == -1 ? 0 : pageNum ;

                await Task.Run(() =>
                {
                    GenerateData();
                });
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }
        protected async Task PageChangedBP(int pageNum)
        {
            try
            {
            ReceivableFilterModel.PageNumBP = pageNum * ItemPerPage;

                await Task.Run(() =>
                {
                    GenerateData();
                });
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }

        }
        public async void ClickGridMode(MouseEventArgs e, int number)
        {
            try
            {
                GridModeActiveV = number;
                ReceivableFilterModel.ReceivableReport = GridModeActiveV;
                ReceivableFilterModel.PageNum = 0;
                ReceivableFilterModel.PageNumBP = 0;
                await Task.Run(() =>
                {
                    GenerateData();
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
                if (searchForm.GetValidationMessages().Count() == 0)
                {
                    if (ReceivableFilterModel.OutputType == OutputReportType.Preview)
                    {
                        var searchString = EncryptHelper.EncryptToUrl(ReceivableFilterModel);
                        JSRuntime.InvokeVoidAsync("open", "receivablelistreportpreview?searchString=" + searchString, "_blank");
                    }
                    else
                    {
                        if (ReceivableFilterModel.OutputType == OutputReportType.CSV)
                        {
                            PrintCsv();
                        }
                        else
                        {
                            Print();
                        }
                    }
                    keyValueFilterPairs = GenerateFilterValueDictionaryService.GenerateForReservableList(ReceivableFilterModel, checkBoxBillingTypes).Result;
                    FilterConditionService.SaveFilterCondtion(keyValueFilterPairs, FormFilterName.ReceivableList, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq).Wait();
                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }
        public async void SelectPreviousSaleOffice()
        {
            try
            {
                if (ReceivableFilterModel.SelectedSaleBranchPayment.Id > 1)
                {
                    EnableNextSaleBranch = true;
                    ReceivableFilterModel.SelectedSaleBranchPayment = SelectedSaleBranchPayments[ReceivableFilterModel.SelectedSaleBranchPayment.Id - 2];
                    if (ReceivableFilterModel.SelectedSaleBranchPayment.Id == 1)
                    {
                        EnablePreSaleBranch = false;
                    }

                    await Task.Run(() =>
                    {
                        GenerateSelectedBillingAddressPayment();
                        if (ReceivableFilterModel.SelectedBillingAddressPayment != null)
                        {
                            if (GridModeActiveV != (int)ReceiableGridMode.Detail)
                            {
                                ReceivableFilterModel.PageNumBP = paginationBP.currentPage = 0;
                            }
                            else
                            {
                                ReceivableFilterModel.PageNum = pagination.currentPage = 0;
                            }
                        }
                        GenerateData();
                    });

                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }
        public async void SelectNextSaleOffice()
        {
            try
            {
                if (ReceivableFilterModel.SelectedSaleBranchPayment.Id < SelectedSaleBranchPayments.Count && SelectedSaleBranchPayments.Count > 1)
                {
                    EnablePreSaleBranch = true;
                    ReceivableFilterModel.SelectedSaleBranchPayment = SelectedSaleBranchPayments[ReceivableFilterModel.SelectedSaleBranchPayment.Id];
                    if (ReceivableFilterModel.SelectedSaleBranchPayment.Id == SelectedSaleBranchPayments.Count)
                    {
                        EnableNextSaleBranch = false;
                    }

                    await Task.Run(() =>
                    {
                        GenerateSelectedBillingAddressPayment();
                        if (ReceivableFilterModel.SelectedBillingAddressPayment != null)
                        {
                            if (GridModeActiveV != (int)ReceiableGridMode.Detail)
                            {
                                ReceivableFilterModel.PageNumBP = paginationBP.currentPage = 0;
                            }
                            else
                            {
                                ReceivableFilterModel.PageNum = pagination.currentPage = 0;
                            }
                        }
                        GenerateData();
                    });

                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }
        public async void SelectPreviousBillingPaymentAddress()
        {
            try
            {
                if (ReceivableFilterModel.SelectedBillingAddressPayment.Id > 1)
                {
                    if (GridModeActiveV != (int)ReceiableGridMode.Detail)
                    {
                        ReceivableFilterModel.PageNumBP = paginationBP.currentPage = 0;
                    }
                    else
                    {
                        ReceivableFilterModel.PageNum = pagination.currentPage = 0;
                    }
                    EnableNextBillingPayment = true;
                    ReceivableFilterModel.SelectedBillingAddressPayment = SelectedBillingAddressPaymentList[ReceivableFilterModel.SelectedBillingAddressPayment.Id - 2];
                    if (ReceivableFilterModel.SelectedBillingAddressPayment.Id == 1)
                    {
                        EnablePreBillingPayment = false;
                    }

                    await Task.Run(() =>
                    {
                        GenerateData();
                    });

                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }
        public async void SelectNextBillingPaymentAddress()
        {
            try
            {
                if (ReceivableFilterModel.SelectedBillingAddressPayment.Id < SelectedBillingAddressPaymentList.Count && SelectedBillingAddressPaymentList.Count > 1)
                {
                    if (GridModeActiveV != (int)ReceiableGridMode.Detail)
                    {
                        ReceivableFilterModel.PageNumBP = paginationBP.currentPage = 0;
                    }
                    else
                    {
                        ReceivableFilterModel.PageNum = pagination.currentPage = 0;
                    }
                    EnablePreBillingPayment = true;
                    ReceivableFilterModel.SelectedBillingAddressPayment = SelectedBillingAddressPaymentList[ReceivableFilterModel.SelectedBillingAddressPayment.Id];
                    if (ReceivableFilterModel.SelectedBillingAddressPayment.Id == SelectedBillingAddressPaymentList.Count)
                    {
                        EnableNextBillingPayment = false;
                    }


                    await Task.Run(() =>
                    {
                        GenerateData();
                    });

                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }
        public void GenerateSelectedSaleBranch()
        {
            try
            {
                ReceivableFilterModel.SelectedSaleBranchPayment = null;
                SelectedSaleBranchPayments = ReceivableListService.GetSelectedSaleBranches(ReceivableFilterModel, new ClaimModel().TenantID, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID).Result;
                if (SelectedSaleBranchPayments.Count > 0)
                {
                    ReceivableFilterModel.SelectedSaleBranchPayment = SelectedSaleBranchPayments.FirstOrDefault();
                    if (SelectedSaleBranchPayments.Count == 1)
                    {
                        EnableNextSaleBranch = false;
                        EnablePreSaleBranch = false;
                    }
                    else
                    {
                        EnableNextSaleBranch = true;
                        EnablePreSaleBranch = false;
                    }
                }
                else
                {
                    Errors.Add("BI_T007");
                    SelectedSaleBranchPayments = new List<SelectedSaleBranchModel>();
                    ReceivableFilterModel.SelectedSaleBranchPayment = SelectedSaleBranchPayments.FirstOrDefault();
                    EnableNextSaleBranch = false;
                    EnablePreSaleBranch = false;
                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }
        public void GenerateSelectedBillingAddressPayment()
        {
            try
            {
                ReceivableFilterModel.SelectedBillingAddressPayment = null;
                if (ReceivableFilterModel.SelectedSaleBranchPayment != null)
                {
                    SelectedBillingAddressPaymentList = ReceivableListService.GetSelectedPaymentAddresses(ReceivableFilterModel, new ClaimModel().TenantID, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID).Result;
                    if (SelectedBillingAddressPaymentList.Count > 0)
                    {
                        ReceivableFilterModel.SelectedBillingAddressPayment = SelectedBillingAddressPaymentList.FirstOrDefault();
                        if (SelectedBillingAddressPaymentList.Count == 1)
                        {
                            EnableNextBillingPayment = false;
                            EnablePreBillingPayment = false;
                        }
                        else
                        {
                            EnableNextBillingPayment = true;
                            EnablePreBillingPayment = false;
                        }
                    }
                    else
                    {
                        Errors.Add("BI_T007");
                        SelectedBillingAddressPaymentList = new List<SelectedPaymentAddressModel>();
                        ReceivableFilterModel.SelectedBillingAddressPayment = SelectedBillingAddressPaymentList.FirstOrDefault();
                        EnableNextBillingPayment = false;
                        EnablePreBillingPayment = false;
                    }
                }
                else
                {
                    Errors.Add("BI_T007");
                    SelectedBillingAddressPaymentList = new List<SelectedPaymentAddressModel>();
                    ReceivableFilterModel.SelectedBillingAddressPayment = SelectedBillingAddressPaymentList.FirstOrDefault();
                    EnableNextBillingPayment = false;
                    EnablePreBillingPayment = false;
                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }
        
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                await JSRuntime.InvokeVoidAsync("inputNumber", ".number", true);
                JSRuntime.InvokeVoidAsync("setEventforCodeNumberField", 10);
                JSRuntime.InvokeVoidAsync("setBlurEventOnPressEnter", ".enter");
                JSRuntime.InvokeVoidAsync("EnterTab", ".enterField", true);
                JSRuntime.InvokeAsync<string>("addMaxLength", "length", 10);
                JSRuntime.InvokeVoidAsync("setEventforCodeNumberFieldV2", ".invoice-number", true, 10);
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }
        public async Task GenerateData()
        {
            try
            {
                var totalRows = 0;
                var totalRowsBP = 0;
                if (searchForm.GetValidationMessages().Count() == 0)
                {
                    await Task.Run(() =>
                    {
                        isLoading = true;
                        InvokeAsync(StateHasChanged);
                    });
                    if (ReceivableFilterModel.SelectedSaleBranchPayment != null && ReceivableFilterModel.SelectedBillingAddressPayment != null)
                    {
                        if (GridModeActiveV == (int)ReceiableGridMode.Detail)
                        {
                            (ReceivableGridData, Summary, TotalRow) = ReceivableListService.GetReceivableGridData(ReceivableFilterModel, false, ReceivableFilterModel.PageNum, ReceivableFilterModel.PageSize, new ClaimModel().TenantID, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID).Result;
                            dataNotFound = ReceivableGridData == null || ReceivableGridData.Count == 0;
                            DataItems = ReceivableGridData;
                            if (ReceivableGridData == null || ReceivableGridData.Count == 0)
                            {
                                Errors.Add("BI_T007");
                            }
                            else
                            {
                                Errors.RemoveAll(x => x.Equals("BI_T007"));
                                btnReportActive = true;
                                keyValueFilterPairs = GenerateFilterValueDictionaryService.GenerateForReservableList(ReceivableFilterModel, checkBoxBillingTypes).Result;
                                FilterConditionService.SaveFilterCondtion(keyValueFilterPairs, FormFilterName.ReceivableList, 0, new ClaimModel().SyainCdSeq).Wait();
                            }
                        }
                        else
                        {
                            (BussinesPlanReceivableGridData, BusinessPlanSummary, TotalRowBP) = ReceivableListService.GetPlanReceivableGridDatas(ReceivableFilterModel, false, ReceivableFilterModel.PageNumBP, ReceivableFilterModel.PageSizeBP, new ClaimModel().TenantID, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID).Result;
                            dataNotFoundBP = BussinesPlanReceivableGridData == null || BussinesPlanReceivableGridData.Count == 0;
                            DataItemsBP = BussinesPlanReceivableGridData;
                            if (BussinesPlanReceivableGridData == null || BussinesPlanReceivableGridData.Count == 0)
                            {
                                Errors.Add("BI_T007");
                            }
                            else
                            {
                                Errors.RemoveAll(x => x.Equals("BI_T007"));
                                btnReportActive = true;
                                keyValueFilterPairs = GenerateFilterValueDictionaryService.GenerateForReservableList(ReceivableFilterModel, checkBoxBillingTypes).Result;
                                FilterConditionService.SaveFilterCondtion(keyValueFilterPairs, FormFilterName.ReceivableList, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq).Wait();
                            }
                        }
                    }
                    else
                    {
                        Errors.Add("BI_T007");
                        ReceivableGridData = new List<ReservationListDetaiGridDataModel>();
                        BussinesPlanReceivableGridData = new List<BussinesPlanReceivableGridDataModel>();
                        TotalRow = 0;
                        TotalRowBP = 0;
                        DataItems = ReceivableGridData;
                        DataItemsBP = BussinesPlanReceivableGridData;
                        Summary = new ReceivablePaymentSummary();
                        BusinessPlanSummary = new BusinessPlanReceivablePaymentSummary();
                        btnReportActive = false;
                    }
                    await Task.Run(() =>
                    {
                        isLoading = false;
                        InvokeAsync(StateHasChanged);
                    });
                    
                }
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
        private void RenderPreNextButton()
        {
            try
            {
                if (SelectedBillingAddressPaymentList.Count == 1 || SelectedBillingAddressPaymentList.Count == 0)
                {
                    EnableNextBillingPayment = false;
                    EnablePreBillingPayment = false;
                }
                if (SelectedSaleBranchPayments.Count == 1 || SelectedSaleBranchPayments.Count == 0)
                {
                    EnableNextSaleBranch = false;
                    EnablePreSaleBranch = false;
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }
        public async Task ChangeValueForm(string ValueName, dynamic value)
        {
            try
            {
                bool validateText = true;
                Errors.RemoveAll(x => x.Equals("BI_T007"));
                if (value is string && string.IsNullOrEmpty(value))
                {
                    value = null;
                }
                if (ValueName == nameof(ReceivableFilterModel.StartReceiptNumber) || ValueName == nameof(ReceivableFilterModel.EndReceiptNumber))
                {
                    if(Int64.TryParse(value, out long num))
                    {
                        value = value == null ? null : value.ToString().PadLeft(10, '0');
                    }
                    else
                    {
                        value = ReceivableFilterModel.StartReceiptNumber;
                        validateText = false;
                    }
                }
                if (ValueName == nameof(ReceivableFilterModel.EndReceiptNumber))
                {
                    if (Int64.TryParse(value, out long num))
                    {
                        value = value == null ? null : value.ToString().PadLeft(10, '0');
                    }
                    else
                    {
                        value = ReceivableFilterModel.EndReceiptNumber;
                        validateText = false;
                    }
                }
                var propertyInfo = ReceivableFilterModel.GetType().GetProperty(ValueName);
                propertyInfo.SetValue(ReceivableFilterModel, value, null);
                await Task.Run(() =>
                {
                    InvokeAsync(StateHasChanged);
                });
                if (ValueName == nameof(ReceivableFilterModel.SelectedSaleBranchPayment))
                {
                    if (ReceivableFilterModel.SelectedSaleBranchPayment.Id == SelectedSaleBranchPayments.Count)
                    {
                        EnableNextSaleBranch = false;
                        EnablePreSaleBranch = true;
                    }
                    else if(ReceivableFilterModel.SelectedSaleBranchPayment.Id == 1)
                    {
                        EnableNextSaleBranch = true;
                        EnablePreSaleBranch = false;
                    }
                    else if (ReceivableFilterModel.SelectedSaleBranchPayment.Id > 1 && ReceivableFilterModel.SelectedSaleBranchPayment.Id < SelectedSaleBranchPayments.Count)
                    {
                        EnableNextSaleBranch = true;
                        EnablePreSaleBranch = true;
                    }
                    GenerateSelectedBillingAddressPayment();
                    if (SelectedBillingAddressPaymentList.Count > 0)
                    {
                        if (GridModeActiveV != (int)ReceiableGridMode.Detail)
                        {
                            ReceivableFilterModel.PageNumBP = paginationBP.currentPage = 0;
                        }
                        else
                        {
                            ReceivableFilterModel.PageNum = pagination.currentPage = 0;
                        }
                        GenerateData();
                        StateHasChanged();
                    }
                    else
                    {
                        Errors.Add("BI_T007");
                        EnableNextBillingPayment = false;
                        EnablePreBillingPayment = false;
                        StateHasChanged();
                    }
                }
                else if (ValueName == nameof(ReceivableFilterModel.SelectedBillingAddressPayment))
                {
                    if (ReceivableFilterModel.SelectedBillingAddressPayment.Id == SelectedBillingAddressPaymentList.Count)
                    {
                        EnableNextBillingPayment = false;
                        EnablePreBillingPayment = true;
                    }
                    else if (ReceivableFilterModel.SelectedBillingAddressPayment.Id == 1)
                    {
                        EnableNextBillingPayment = true;
                        EnablePreBillingPayment = false;
                    }
                    else if (ReceivableFilterModel.SelectedBillingAddressPayment.Id > 1 && ReceivableFilterModel.SelectedBillingAddressPayment.Id < SelectedBillingAddressPaymentList.Count)
                    {
                        EnableNextBillingPayment = true;
                        EnablePreBillingPayment = true;
                    }
                    if (GridModeActiveV != (int)ReceiableGridMode.Detail)
                    {
                        ReceivableFilterModel.PageNumBP = paginationBP.currentPage = 0;
                    }
                    else
                    {
                        ReceivableFilterModel.PageNum = pagination.currentPage = 0;
                    }
                    GenerateData();
                    StateHasChanged();
                }
                else
                {
                    if (searchForm.GetValidationMessages().Count() == 0)
                    {
                        if (validateText)
                        {
                            await Task.Run(() =>
                            {
                                isLoading = true;
                                InvokeAsync(StateHasChanged);
                            });
                            if(ValueName != nameof(ReceivableFilterModel.startCustomerComponentGyosyaData) && ValueName != nameof(ReceivableFilterModel.startCustomerComponentTokiskData) && ValueName != nameof(ReceivableFilterModel.endCustomerComponentTokiskData) && ValueName != nameof(ReceivableFilterModel.endCustomerComponentGyosyaData))
                            {
                                GenerateSelectedSaleBranch();
                                GenerateSelectedBillingAddressPayment();
                                GenerateData();
                            }
                            await Task.Run(() =>
                            {
                                isLoading = false;
                                InvokeAsync(StateHasChanged);
                            });
                        }
                        
                    }
                    else
                    {
                        if (validateText)
                        {
                            await Task.Run(() =>
                            {
                                EnableNextBillingPayment = false;
                                EnableNextSaleBranch = false;
                                EnablePreBillingPayment = false;
                                EnablePreSaleBranch = false;
                                ReceivableGridData = new List<ReservationListDetaiGridDataModel>();
                                BussinesPlanReceivableGridData = new List<BussinesPlanReceivableGridDataModel>();
                                InvokeAsync(StateHasChanged);
                            });

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }

        protected async Task RowClick(RowClickEventArgs<ReservationListDetaiGridDataModel> args)
        {
            try
            {
                SelectedItem = args.SelectedItem;
                LastXClicked = Convert.ToInt32(args.Event.ClientX);
                LastYClicked = Convert.ToInt32(args.Event.ClientY);
                if (!args.Event.ShiftKey && !args.Event.CtrlKey && SelectedItem.YouKbn == 2)
                {
                    CheckedItem = SelectedItem;
                    await blazorContextMenuService.ShowMenu("gridRowClickMenu2Item", LastXClicked, LastYClicked);
                }
                if (!args.Event.ShiftKey && !args.Event.CtrlKey && SelectedItem.YouKbn != 2)
                {
                    CheckedItem = SelectedItem;
                    await blazorContextMenuService.ShowMenu("gridRowClickMenu1Item", LastXClicked, LastYClicked);
                }

                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected async void ItemClick(ItemClickEventArgs e)
        {
            try
            {
                string baseUrl = AppSettingsService.GetBaseUrl();
                string url = "";
                string UkeNo = CheckedItem.ReceiptNumber;
                ObjectPram temp = new ObjectPram();
                temp.key = UkeNo;
                string UkeCdPram = EncryptHelper.EncryptToUrl(temp);
                await Task.Run(() =>
                {
                    switch (e.MenuItem.Id)
                    {
                        case "booking-input-edit":
                            url = baseUrl + "/bookinginput";
                            url = url + string.Format("/?UkeCd={0}", UkeNo);
                            JSRuntime.InvokeVoidAsync("open", url, "_blank");
                            break;
                        case "check-bus":
                            outDataTable = new OutDataTable()
                            {
                                FutTumRen = SelectedItem.FutTumRen,
                                FutuUnkRen = SelectedItem.FutuUnkRen,
                                SeiFutSyu = SelectedItem.SeiFutSyu,
                                UkeNo = SelectedItem.UkeNo
                            };
                            isOpenCharterInquiryPopUp = depositCouponService.CheckOpenChaterInquiryPopUpAsync(outDataTable).Result;
                            errorMessage = isOpenCharterInquiryPopUp ? string.Empty : @Lang["BI_T008"];

                            break;
                    }
                });

                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        public async Task FilterChanged(CheckBoxFilter checkBoxFilter)
        {
            try
            {
                checkBoxFilter.IsChecked = !checkBoxFilter.IsChecked;
                switch (checkBoxFilter.Number)
                {
                    case 1:
                        ReceivableFilterModel.FareBilTyp = checkBoxFilter.IsChecked ? (byte)1 : (byte)2;
                        break;
                    case 2:
                        ReceivableFilterModel.FutaiBilTyp = checkBoxFilter.IsChecked ? (byte)1 : (byte)2;
                        break;
                    case 3:
                        ReceivableFilterModel.TollFeeBilTyp = checkBoxFilter.IsChecked ? (byte)1 : (byte)2;
                        break;
                    case 4:
                        ReceivableFilterModel.ArrangementFeeBilTyp = checkBoxFilter.IsChecked ? (byte)1 : (byte)2;
                        break;
                    case 5:
                        ReceivableFilterModel.GuideFeeBilTyp = checkBoxFilter.IsChecked ? (byte)1 : (byte)2;
                        break;
                    case 6:
                        ReceivableFilterModel.LoadedItemBilTyp = checkBoxFilter.IsChecked ? (byte)1 : (byte)2;
                        break;
                    case 7:
                        ReceivableFilterModel.CancelFeeBilTyp = checkBoxFilter.IsChecked ? (byte)1 : (byte)2;
                        break;
                }
                if (checkBoxFilter.IsBillingType == true)
                {
                    if (billingTypes.Any(x => x == checkBoxFilter.Number))
                    {
                        billingTypes.Remove(checkBoxFilter.Number);
                    }
                    else
                    {
                        billingTypes.Add(checkBoxFilter.Number);
                    }
                    ReceivableFilterModel.BillingType = string.Join(",", billingTypes);
                }
                await Task.Run(() =>
                {
                    isLoading = true;
                    InvokeAsync(StateHasChanged);
                    GenerateSelectedSaleBranch();
                    GenerateSelectedBillingAddressPayment();
                    GenerateData();
                });
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }

        protected async Task UpdateFormModel(string ValueName, dynamic value)
        {
            try
            {
                if (value is string && string.IsNullOrEmpty(value))
                {
                    value = null;
                }
                var propertyInfo = ReceivableFilterModel.GetType().GetProperty(ValueName);
                propertyInfo.SetValue(ReceivableFilterModel, value, null);
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }
        public void GenerateCheckBoxBillingType(int numberOfBillingType, List<string> BillingTypeNames)
        {
            try
            {
                for (int i = 1; i <= numberOfBillingType; i++)
                {
                    checkBoxBillingTypes.Add(new CheckBoxFilter()
                    {
                        Id = i.ToString(),
                        IsChecked = false,
                        Name = BillingTypeNames[i - 1],
                        Number = i,
                        IsBillingType = true
                    });
                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }
        public void GenerateAllCheckBoxName()
        {
            try
            {
                BillingTypeNames.Add(Lang["TicketPriceCheck"]);
                BillingTypeNames.Add(Lang["RandomCheck"]);
                BillingTypeNames.Add(Lang["TollsCheck"]);
                BillingTypeNames.Add(Lang["LoadingPriceCheck"]);
                BillingTypeNames.Add(Lang["GuidePriceCheck"]);
                BillingTypeNames.Add(Lang["LoadingStockPriceCheck"]);
                BillingTypeNames.Add(Lang["CancelPriceCheck"]);
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }
        public void GenerateDepositOutputTemplate()
        {
            try
            {
                DepositOutputs.Add(new DepositOutputClass()
                {
                    Id = 1,
                    Name = Lang["DepositList"]
                });
                DepositOutputs.Add(new DepositOutputClass()
                {
                    Id = 2,
                    Name = Lang["DepositOfficeType"]
                });
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }
        public async void Print()
        {
            try
            {
                await _loadingService.ShowAsync();
                List<ReceivableListReportDataModel> data = new List<ReceivableListReportDataModel>();
                List<BusinessPlanReceivableListReportDataModel> businessPlanData = new List<BusinessPlanReceivableListReportDataModel>();
                if (GridModeActiveV == (int)ReceiableGridMode.Detail)
                {
                    data = await ReceivableListService.GetReceivableListReportDatas(ReceivableFilterModel, new ClaimModel().TenantID, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID);
                }
                else
                {
                    businessPlanData = await ReceivableListService.GetBusinessPlanReceivableListReportDatas(ReceivableFilterModel, new ClaimModel().TenantID, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID);
                }
                if (data.Count > 0 || businessPlanData.Count > 0)
                {
                    XtraReport report = new XtraReport();

                    if (ReceivableFilterModel.ReceivableReport == 1)
                    {
                        report = new Reports.ReceivableListReportA4();
                        if (ReceivableFilterModel.ReportPageSize.IdValue == BillTypePagePrintList.BillTypePagePrintData[1].IdValue)
                        {
                            report = new Reports.ReceivableListReportA3();
                        }
                        else
                        {
                            if (ReceivableFilterModel.ReportPageSize.IdValue == BillTypePagePrintList.BillTypePagePrintData[2].IdValue)
                            {
                                report = new Reports.ReceivableListReportB4();
                            }
                        }
                        report.DataSource = data;
                    }
                    else
                    {
                        report = new Reports.BusinessPlanReceivableListReportA4();
                        if (ReceivableFilterModel.ReportPageSize.IdValue == BillTypePagePrintList.BillTypePagePrintData[1].IdValue)
                        {
                            report = new Reports.BusinessPlanReceivableListReportA3();
                        }
                        else
                        {
                            if (ReceivableFilterModel.ReportPageSize.IdValue == BillTypePagePrintList.BillTypePagePrintData[2].IdValue)
                            {
                                report = new Reports.BusinessPlanReceivableListReportB4();
                            }
                        }
                        report.DataSource = businessPlanData;
                    }
                    await new System.Threading.Tasks.TaskFactory().StartNew(() =>
                    {
                        report.CreateDocument();
                        using (MemoryStream ms = new MemoryStream())
                        {
                            if (ReceivableFilterModel.OutputType == OutputReportType.Print)
                            {
                                PrintToolBase tool = new PrintToolBase(report.PrintingSystem);
                                tool.Print();
                                return;
                            }
                            report.ExportToPdf(ms);

                            byte[] exportedFileBytes = ms.ToArray();
                            string myExportString = Convert.ToBase64String(exportedFileBytes);
                            string name = ReceivableFilterModel.ReceivableReport == 1 ? "ReceivableList" : "BusinessPlanReceivableList";
                            JSRuntime.InvokeVoidAsync("downloadFileClientSide", myExportString, "pdf", name);
                        }
                    });
                }
                await _loadingService.HideAsync();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }
        public async void PrintCsv()
        {
            try
            {
                await _loadingService.ShowAsync();
                List<ReceivableCSVDataModel> listData = new List<ReceivableCSVDataModel>();
                List<BusinessPlanReceivableCSVDataModel> listBPData = new List<BusinessPlanReceivableCSVDataModel>();
                if (GridModeActiveV == (int)ReceiableGridMode.Detail)
                {
                    listData = await ReceivableListService.GetReceivableListCSVDatas(ReceivableFilterModel, new ClaimModel().TenantID, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID);
                }
                else
                {
                    listBPData = await ReceivableListService.GetBusinessPlanReceivableListCSVDatas(ReceivableFilterModel, new ClaimModel().TenantID, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID);
                }
                var dt = listData.ToDataTable<ReceivableCSVDataModel>();
                var dtBP = listBPData.ToDataTable<BusinessPlanReceivableCSVDataModel>();
                while (dt.Columns.Count > 52)
                {
                    dt.Columns.RemoveAt(52);
                }
                while (dtBP.Columns.Count > 31)
                {
                    dtBP.Columns.RemoveAt(31);
                }
                SetTableHeader(dt);
                SetBPTableHeader(dtBP);
                string path = string.Format("{0}/csv/{1}.csv", hostingEnvironment.WebRootPath, Guid.NewGuid());

                bool isWithHeader = ReceivableFilterModel.ActiveHeaderOption.IdValue == 0 ? true : false;
                bool isEnclose = ReceivableFilterModel.GroupType.IdValue == 0 ? true : false;
                string space = ReceivableFilterModel.DelimiterType.IdValue == 0 ? "\t" : ReceivableFilterModel.DelimiterType.IdValue == 1 ? ";" : ",";
                var result = CsvHelper.ExportDatatableToCsv(dt, path, true, isWithHeader, isEnclose, space);

                if (ReceivableFilterModel.ReceivableReport == 2)
                {
                    result = CsvHelper.ExportDatatableToCsv(dtBP, path, true, isWithHeader, isEnclose, space);
                }
                await new System.Threading.Tasks.TaskFactory().StartNew(() =>
                {
                    string myExportString = Convert.ToBase64String(result);
                    string name = ReceivableFilterModel.ReceivableReport == 1 ? "ReceivableList" : "BusinessPlanReceivableList";
                    JSRuntime.InvokeVoidAsync("downloadFileClientSide", myExportString, "csv", name);
                });
                await _loadingService.HideAsync();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }
        public void SetTableHeader(DataTable table)
        {
            try
            {
                List<string> listHeader = new List<string>() { "営業所コード", "営業所名", "営業所略名", "請求先業者コード", "請求先コード", "請求先支店コード", "請求先業者コード名", "請求先名", "請求先支店名", "請求先略名", "請求先支店略名", "受付番号", "受付営業所コード", "受付営業所名", "受付営業所略名",
            "得意先業者コード", "得意先コード", "得意先支店コード", "得意先業者コード名", "得意先名", "得意先支店名", "得意先略名", "得意先支店略名", "請求年月日", "運行日連番", "請求付帯種別", "請求付帯種別名", "団体名", "行き先名", "配車年月日", "到着年月日", "付帯積込品区分",
                "付帯積込品名", "精算コード", "精算名", "数量", "単価", "売上額", "税区分", "税区分名", "消費税率",
            "消費税額", "手数料率", "手数料額", "請求額", "入金累計", "未収額", "クーポン消込額", "得意先コード使用開始年月日", "得意先コード使用終了年月日", "得意先支店コード使用開始年月日", "得意先支店コード使用終了年月日"};
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
        public void SetBPTableHeader(DataTable table)
        {
            try
            {
                List<string> listHeader = new List<string>() { "営業所コード", "営業所名", "営業所略名", "請求先業者コード", "請求先コード", "請求先支店コード", "請求先業者コード名", "請求先名", "請求先支店名", "請求先略名", "請求先支店略名", "運賃・金額",
                "運賃・消費税額", "運賃・手数料額", "運賃・入金累計", "運賃・未収額", "ガイド料・売上額", "ガイド料・消費税額", "ガイド料・手数料額", "ガイド料・入金累計", "ガイド料・未収額",
                "その他付帯・売上額", "その他付帯・消費税額", "その他付帯・手数料額", "その他付帯・入金累計", "その他付帯・未収額", "キャンセル料・金額", "キャンセル料・消費税額", "キャンセル料・入金累計", "キャンセル料・未収額", "未収額合計" };
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
    }
}
