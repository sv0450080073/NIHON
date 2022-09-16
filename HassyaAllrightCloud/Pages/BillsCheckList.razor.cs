using DevExpress.XtraPrinting;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.BillPrint;
using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Commons.Helpers;
using Microsoft.AspNetCore.Hosting;
using System.Data;
using static HassyaAllrightCloud.Commons.Constants.Constants;
using HassyaAllrightCloud.Pages.Components;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Domain.Entities;
using Microsoft.AspNetCore.Components.Forms;
using HassyaAllrightCloud.Infrastructure.Services;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using HassyaAllrightCloud.IService.CommonComponents;

namespace HassyaAllrightCloud.Pages
{
    public class BillsCheckListBase : ComponentBase
    {
        #region Inject
        [Inject]
        protected IStringLocalizer<BillsCheckList> Lang { get; set; }
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }
        [Inject]
        protected ITPM_CodeKbListService CodeKbListService { get; set; }
        [Inject]
        protected ITPM_YoyKbnDataListService YoyKbnDataListService { get; set; }
        [Inject]
        protected ICustomerCLassificationListService CustomerCLassificationService { get; set; }
        [Inject]
        protected IBillCheckListService BillCheckListService { get; set; }
        [Inject]
        protected ICustomerListService CustomerService { get; set; }
        [Inject]
        protected NavigationManager navigationManager { get; set; }
        [Inject]
        protected IWebHostEnvironment hostingEnvironment { get; set; }
        [Inject]
        protected IErrorHandlerService errorModalService { get; set; }
        [Inject]
        protected IFilterCondition FilterConditionService { get; set; }
        [Inject]
        protected IGetFilterDataService GetFilterDataService { get; set; }
        [Inject]
        protected IGenerateFilterValueDictionary GenerateFilterValueDictionaryService { get; set; }
        [Inject]
        protected AppSettingsService AppSettingsService { get; set; }
        [Inject]
        protected ICustomerComponentService _service { get; set; }
        [Inject]
        protected IReservationClassComponentService _yoyakuservice { get; set; }
        #endregion

        #region Propeties and variable        
        protected List<BillCheckListGridData> GridDatas { get; set; }
        protected List<BillCheckListGridData> GridDisplay { get; set; }
        protected List<BillCheckListTotalData> CurrentTotal { get; set; }    
        protected bool isLoading { get; set; } = true;
        protected bool ReportActive { get; set; } = false;
        protected bool NotActiveBillAdress { get; set; } = false;
        protected bool itemCheckAll { get; set; } = false;
        public bool isFirstRender { get; set; } = true;
        public bool isFirstCmbCus { get; set; } = true;
        public bool isInitComplete { get; set; } = false;
        protected byte RecordsPerPage { get; set; } = 25;
        protected List<ComboboxFixField> lstBillIssuedClassification { get; set; } = new List<ComboboxFixField>();
        public List<CustomerComponentGyosyaData> ListGyosya { get; set; } = new List<CustomerComponentGyosyaData>();
        public List<CustomerComponentTokiskData> TokiskData { get; set; } = new List<CustomerComponentTokiskData>();
        public List<CustomerComponentTokiStData> TokiStData { get; set; } = new List<CustomerComponentTokiStData>();
        public List<ReservationClassComponentData> ListReservationClass { get; set; } = new List<ReservationClassComponentData>(); 
        protected List<ComboboxFixField> lstBillTypeSortGridData { get; set; } = new List<ComboboxFixField>();
        protected BillsCheckListFormData billCheckListForm = new BillsCheckListFormData();
        protected List<BillOfficeData> billOfficeList = new List<BillOfficeData>();
        // protected List<BillAddress> billAddressList = new List<BillAddress>();
        protected List<BillAddress> billAddressListDistinct = new List<BillAddress>();
        // protected List<ReservationData> reservationClassificationList = new List<ReservationData>();
        protected List<InvoiceType> billClassificationList = new List<InvoiceType>();
        protected Dictionary<int, bool> lstItemGridCheck = new Dictionary<int, bool>();
        protected List<bool> lstItemGridCheckPerPage = new List<bool>();
        protected Dictionary<int, bool> lstItemCheckAll = new Dictionary<int, bool>();
        protected Dictionary<int, OutDataTable> lstItemParam = new Dictionary<int, OutDataTable>();
        public Dictionary<string, string> LangDic = new Dictionary<string, string>();
        Dictionary<string, string> keyValueFilterPairs = new Dictionary<string, string>();
        protected string dateFormat = "yyyy/MM/dd";
        protected int MaxPageCount = 5;
        protected int CurrentPage = 0;
        protected int NumberOfPage;
        protected Pagination paging = new Pagination();
        protected int FirstPageSelect = 0;
        protected int activeTabIndex = 0;
        protected bool IsValid = true;
        protected bool IsNoData = false;
        protected bool isChangePageNumber = false;
        protected bool ShowErrorPopup = false;
        protected string ErrorMessage = "";
        // For report
        protected bool isDisableCsv { get; set; } = true;
        protected int activeButtonReport = 0;
        protected bool btnReportActive = true;
        protected List<ComboboxFixField> lstPageSize { get; set; } = new List<ComboboxFixField>();
        protected List<ComboboxFixField> lstOutType { get; set; } = new List<ComboboxFixField>();
        protected List<ComboboxFixField> lstGroupType { get; set; } = new List<ComboboxFixField>();
        protected List<ComboboxFixField> lstDelimiterType { get; set; } = new List<ComboboxFixField>();        
        protected static int[] listPage = new int[] { 10, 25, 50, 100 };
        protected string lblPageSize = "";
        protected OutputReportType ActiveButtonReport
        {
            get => billCheckListForm.OutputType;
            set
            {
                billCheckListForm.OutputType = value;
                StateHasChanged();
            }
        }
        protected int ActiveTabIndex
        {
            get => activeTabIndex;
            set
            {
                activeTabIndex = value;
                AdjustHeightWhenTabChanged();
            }
        }

        #endregion

        #region Function
        protected override async void OnInitialized()
        {
            try
            {
                isInitComplete = false;
                billCheckListForm.ActiveV = (int)ViewMode.Medium;
                CurrentTotal = new List<BillCheckListTotalData>();
                // For show error tooltip
                var dataLang = Lang.GetAllStrings();
                LangDic = dataLang.ToDictionary(l => l.Name, l => l.Value);
                // Set data for tab report
                // Add data for report page type
                lstPageSize.AddRange(new List<ComboboxFixField>() {
                new ComboboxFixField(){ IdValue = 0, StringValue = Lang["PageType_" + BillTypePagePrintList.BillTypePagePrintData[0].IdValue.ToString()] },
                new ComboboxFixField(){ IdValue = 1, StringValue = Lang["PageType_" + BillTypePagePrintList.BillTypePagePrintData[1].IdValue.ToString()] },
                new ComboboxFixField(){ IdValue = 2, StringValue =  Lang["PageType_" + BillTypePagePrintList.BillTypePagePrintData[2].IdValue.ToString()] }
                });
                billCheckListForm.PageSize = lstPageSize.FirstOrDefault();

                // Add data for output type
                lstOutType.AddRange(new List<ComboboxFixField>() {
                new ComboboxFixField(){ IdValue = 0, StringValue = Lang["OutType_" + ShowHeaderOptions.ShowHeaderOptionData[0].IdValue.ToString()] },
                new ComboboxFixField(){ IdValue = 1, StringValue = Lang["OutType_" + ShowHeaderOptions.ShowHeaderOptionData[1].IdValue.ToString()] }
                });
                billCheckListForm.ActiveHeaderOption = lstOutType.FirstOrDefault();

                // Add data for group type print
                lstGroupType.AddRange(new List<ComboboxFixField>() {
                new ComboboxFixField(){ IdValue = 0, StringValue = Lang["GroupType_" + GroupTypes.GroupTypeData[0].IdValue.ToString()] },
                new ComboboxFixField(){ IdValue = 1, StringValue = Lang["GroupType_" + GroupTypes.GroupTypeData[1].IdValue.ToString()] }
                });
                billCheckListForm.GroupType = lstGroupType.FirstOrDefault();

                // Add data for delimiter type print
                lstDelimiterType.AddRange(new List<ComboboxFixField>() {
                new ComboboxFixField(){ IdValue = 0, StringValue = Lang["DelimiterType_" + DelimiterTypes.DelimiterTypeData[0].IdValue.ToString()] },
                new ComboboxFixField(){ IdValue = 1, StringValue = Lang["DelimiterType_" + DelimiterTypes.DelimiterTypeData[1].IdValue.ToString()] },
                new ComboboxFixField(){ IdValue = 2, StringValue = Lang["DelimiterType_" + DelimiterTypes.DelimiterTypeData[2].IdValue.ToString()] }
                });
                billCheckListForm.DelimiterType = lstDelimiterType[2];

                // Add data for combobox BillIssuedClassificationListData
                lstBillIssuedClassification.AddRange(new List<ComboboxFixField>() {
                new ComboboxFixField(){ IdValue = 0, StringValue = Lang["BillIssuedClassification_" + BillIssuedClassificationListData.BillIssuedClassificationList[0].IdValue.ToString()] },
                new ComboboxFixField(){ IdValue = 1, StringValue = Lang["BillIssuedClassification_" + BillIssuedClassificationListData.BillIssuedClassificationList[1].IdValue.ToString()] },
                new ComboboxFixField(){ IdValue = 2, StringValue = Lang["BillIssuedClassification_" + BillIssuedClassificationListData.BillIssuedClassificationList[2].IdValue.ToString()] }
                });
                billCheckListForm.BillIssuedClassification = lstBillIssuedClassification.FirstOrDefault();

                // Add data for
                lstBillTypeSortGridData.AddRange(new List<ComboboxFixField>() {
                new ComboboxFixField(){ IdValue = 0, StringValue = Lang["BillTypeSort_" + BillTypeSortGridList.BillTypeSortGridData[0].IdValue.ToString()] },
                new ComboboxFixField(){ IdValue = 1, StringValue = Lang["BillTypeSort_" + BillTypeSortGridList.BillTypeSortGridData[1].IdValue.ToString()]}
                });
                billCheckListForm.BillTypeOrder = lstBillTypeSortGridData.FirstOrDefault();

                billOfficeList = await BillCheckListService.GetBillOffice(new ClaimModel().TenantID);
                if (billOfficeList.Count > 0)
                {
                    billCheckListForm.BillOffice = billOfficeList.FirstOrDefault();
                }

                // 得意先, 仕入先
                var taskTokisk = _service.GetListTokisk();
                var taskTokiSt = _service.GetListTokiSt();
                var taskGyosya = _service.GetListGyosya();
                // 予約区分, 請求区分 
                var cmbBillClassification = CodeKbListService.GetdataSEIKYUKBN(new ClaimModel().TenantID);
                var reservationClass = _yoyakuservice.GetListReservationClass();

                await Task.WhenAll(taskGyosya, taskTokisk, taskTokiSt, cmbBillClassification, reservationClass);

                ListGyosya = taskGyosya.Result;
                TokiskData = taskTokisk.Result;
                TokiStData = taskTokiSt.Result;
                ListReservationClass = reservationClass.Result;
                billClassificationList = cmbBillClassification.Result.ToList();
                billClassificationList.Insert(0, null);

                List<TkdInpCon> filterValues = FilterConditionService.GetFilterCondition(FormFilterName.BillCheckList, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq).Result;
                
                if(filterValues.Count > 0)
                {
                    billCheckListForm = GetFilterDataService.GetBillCheckListFormData(filterValues, ListGyosya, TokiskData, TokiStData, ListReservationClass, billClassificationList
                                       , billOfficeList, lstBillIssuedClassification, lstBillTypeSortGridData, lstPageSize, lstOutType, lstGroupType, lstDelimiterType);
                }
                billAddressListDistinct = await BillCheckListService.GetBillCheckListForCmbBillAddress(billCheckListForm, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, new ClaimModel().TenantID);
                

                //   get data for bill address distinct
                List<BillCheckListGridData> FirstPageData;
                int rowCount;
                if (billAddressListDistinct.Count > 0)
                {
                    btnReportActive = true;
                    billCheckListForm.BillAdress = billAddressListDistinct.FirstOrDefault();
                    (rowCount, CurrentTotal, FirstPageData) = await BillCheckListService.GetBillCheckListGridData(billCheckListForm, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, new ClaimModel().TenantID, 0, RecordsPerPage);
                    GridDatas = new List<BillCheckListGridData>(new BillCheckListGridData[rowCount]);
                    NumberOfPage = (GridDatas.Count() + RecordsPerPage - 1) / RecordsPerPage;
                    IsNoData = GridDatas.Count() == 0;
                    if (!IsNoData)
                    {
                        GridDatas.RemoveRange(0, FirstPageData.Count());
                        GridDatas.InsertRange(0, FirstPageData);
                        SelectPage(FirstPageSelect);
                    }
                    else
                    {
                        FirstPageSelect = 0;
                    }
                }
                else
                {
                    IsNoData = true;
                    btnReportActive = false;
                    NotActiveBillAdress = true;
                    billCheckListForm.BillAdress = null;
                }

                isLoading = false;
                if(!isInitComplete)
                    isInitComplete = true;
                await InvokeAsync(StateHasChanged);
                base.OnInitialized();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
                isLoading = false;
                await InvokeAsync(StateHasChanged);
            }
            
        }
        protected override  void OnAfterRender(bool firstRender)
        {
            try
            {
                if (isFirstRender && ListGyosya.Count > 0)
                {
                    JSRuntime.InvokeVoidAsync("setEventforCodeNumberFieldV2", ".bill-check-list-invoice-number", true, 10);
                    JSRuntime.InvokeAsync<string>("addMaxLength", "length", 10);
                    JSRuntime.InvokeVoidAsync("loadPageScript", "billCheckListPage", "fadeToggleBillTitle");
                    JSRuntime.InvokeVoidAsync("loadPageScript", "billCheckListPage", "fadeToggleTable");
                    JSRuntime.InvokeVoidAsync("loadPageScript", "billCheckListPage", "fadeToggleWidthAdjustHeight");
                    JSRuntime.InvokeVoidAsync("setBlurEventOnPressEnter", ".bill-check-list-invoice-number");
                    JSRuntime.InvokeVoidAsync("loadPageScript", "billCheckListPage", "tabindexFix", ".form-bill-focus", true);
                    isFirstRender = false;
                }
                else
                {
                    JSRuntime.InvokeVoidAsync("loadPageScript", "billCheckListPage", "tabindexFix", ".form-bill-focus");
                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }

        }
        private async Task LoadBillAdressDict()
        {
            try
            {
                if (IsValid)
                {
                    //   get data for bill address distinct
                    billAddressListDistinct = await BillCheckListService.GetBillCheckListForCmbBillAddress(billCheckListForm, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, new ClaimModel().TenantID);
                    if (billAddressListDistinct.Count > 0)
                    {
                        IsNoData = false;
                        NotActiveBillAdress = false;
                        billCheckListForm.BillAdress = billAddressListDistinct[0];

                    }
                    else
                    {
                        IsNoData = true;
                        billCheckListForm.BillAdress = null;
                        NotActiveBillAdress = true;
                        btnReportActive = false;
                        GridDisplay = new List<BillCheckListGridData>();
                        if (isLoading)
                            isLoading = false;
                        GridDisplay = new List<BillCheckListGridData>();
                        await InvokeAsync(StateHasChanged);
                    }
                }
                else
                {
                    billAddressListDistinct = new List<BillAddress>();
                    if (isLoading)
                        isLoading = false;
                    GridDisplay = new List<BillCheckListGridData>();
                    await InvokeAsync(StateHasChanged);
                }
                
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
           
        }
        protected void ChangePageSize(byte value)
        {
            isChangePageNumber = true;
            RecordsPerPage = value;
            StateHasChanged();
        }
        /// <summary>
        /// change value combobox
        /// </summary>
        /// <param name="ValueName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected async Task ChangeValueForm(string ValueName, dynamic value, EditContext formContext)
        {
            try
            {
                bool validateText = true;
                await Task.Run(() =>
                {
                    if (value is string && string.IsNullOrEmpty(value))
                    {
                        value = null;
                    }
                    switch (ValueName)
                    {
                        case nameof(billCheckListForm.StartReceiptNumber):
                            if (!string.IsNullOrEmpty(value))
                            {
                                if (!long.TryParse(value, out long result))
                                    result = 0;
                                if (result != 0 && result.ToString().Length > 10)
                                {
                                    value = result.ToString().Substring(0, 10);
                                }
                                if (result == 0)
                                {
                                    validateText = false;
                                    value = billCheckListForm.StartReceiptNumber;
                                }
                            }

                            break;
                        case nameof(billCheckListForm.EndReceiptNumber):
                            if (!string.IsNullOrEmpty(value))
                            {
                                if (!long.TryParse(value, out long resultTo))
                                    resultTo = 0;
                                if (resultTo != 0 && resultTo.ToString().Length > 10)
                                {
                                    value = resultTo.ToString().Substring(0, 10);
                                }
                                if (resultTo == 0)
                                {
                                    validateText = false;
                                    value = billCheckListForm.EndReceiptNumber;
                                }
                            }

                            break;
                        default:
                            break;
                    }

                    var propertyInfo = billCheckListForm.GetType().GetProperty(ValueName);
                    propertyInfo.SetValue(billCheckListForm, value, null);

                    InvokeAsync(StateHasChanged).Wait();
                    if (validateText)
                    {
                        isLoading = true;
                        InvokeAsync(StateHasChanged).Wait();
                        if (formContext != null)
                        {
                            IsValid = formContext.GetValidationMessages().Distinct().Count() == 0;
                        }
                        if (ValueName != nameof(billCheckListForm.GyosyaTokuiSakiFrom) && ValueName != nameof(billCheckListForm.GyosyaTokuiSakiTo)
                        && ValueName != nameof(billCheckListForm.TokiskTokuiSakiFrom) && ValueName != nameof(billCheckListForm.TokiskTokuiSakiTo))
                        {
                            if (ValueName == nameof(billCheckListForm.TokiStTokuiSakiFrom) && ValueName == nameof(billCheckListForm.TokiStTokuiSakiFrom))
                            {
                                if (!isFirstCmbCus)
                                {
                                    //  btnReportActive = IsValid;
                                    if (ValueName != nameof(billCheckListForm.BillAdress))
                                    {
                                        LoadBillAdressDict().Wait();
                                    }
                                    ChangeState().Wait();
                                }
                                else
                                {
                                    isFirstCmbCus = false;
                                }
                            }
                            else
                            {
                                //  btnReportActive = IsValid;
                                if (ValueName != nameof(billCheckListForm.BillAdress))
                                {
                                    LoadBillAdressDict().Wait();
                                }
                                ChangeState().Wait();
                            }
                        }
                        
                        if (isLoading)
                            isLoading = false;
                        InvokeAsync(StateHasChanged);
                    }
                });
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }
        /// <summary>
        /// Check invalid for form condition
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        protected IEnumerable<string> Store(IEnumerable<string> errorMessage)
        {
            try
            {
                if (errorMessage.Count() > 0)
                {
                    IsValid = false;
                    btnReportActive = false;
                }
                else
                {
                    IsValid = true;
                    if (!IsNoData)
                        btnReportActive = true;
                }
                return errorMessage;
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
                return errorMessage;
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
                var propertyInfo = billCheckListForm.GetType().GetProperty(ValueName);
                propertyInfo.SetValue(billCheckListForm, value, null);
                keyValueFilterPairs = GenerateFilterValueDictionaryService.GenerateForBillCheckListFormData(billCheckListForm).Result;
                FilterConditionService.SaveFilterCondtion(keyValueFilterPairs, FormFilterName.BillCheckList, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq).Wait();
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }
        /// set value when check box item bill type change
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        protected async Task CheckedValueChanged(string fieldName, object newValue)
        {
            try
            {
                switch (fieldName)
                {
                    case nameof(billCheckListForm.itemFare):
                        billCheckListForm.itemFare = Convert.ToBoolean(newValue);
                        break;
                    case nameof(billCheckListForm.itemIncidental):
                        billCheckListForm.itemIncidental = Convert.ToBoolean(newValue);
                        break;
                    case nameof(billCheckListForm.itemTollFee):
                        billCheckListForm.itemTollFee = Convert.ToBoolean(newValue);
                        break;
                    case nameof(billCheckListForm.itemArrangementFee):
                        billCheckListForm.itemArrangementFee = Convert.ToBoolean(newValue);
                        break;
                    case nameof(billCheckListForm.itemGuideFee):
                        billCheckListForm.itemGuideFee = Convert.ToBoolean(newValue);
                        break;
                    case nameof(billCheckListForm.itemShippingCharge):
                        billCheckListForm.itemShippingCharge = Convert.ToBoolean(newValue);
                        break;
                    default:
                        billCheckListForm.itemCancellationCharge = Convert.ToBoolean(newValue);
                        break;
                }
                isLoading = true;
                await Task.Run(() =>
                {
                    InvokeAsync(StateHasChanged).Wait();
                    LoadBillAdressDict().Wait();
                    ChangeState().Wait();
                    isLoading = false;

                });
                InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }
        /// <summary>
        /// event when check box in grid change
        /// </summary>
        /// <param name="i"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        protected async Task CheckedValueGridChanged(int key, int i, BillCheckListGridData data, bool newValue)
        {
            try
            {
                BillCheckListTotalData currentRow = new BillCheckListTotalData();
                BillCheckListTotalData currentTotalBySeiFutSyu = new BillCheckListTotalData();
                BillCheckListTotalData currentTotalSeiFutSyuBy0 = new BillCheckListTotalData();
                List<BillCheckListTotalData> firstToTal = new List<BillCheckListTotalData>();
                List<BillCheckListTotalData> TempToTal = new List<BillCheckListTotalData>();
                TempToTal = CurrentTotal;
                firstToTal = TempToTal.Where(x => x.Type == 1).DefaultIfEmpty().ToList();
                currentTotalSeiFutSyuBy0 = TempToTal.Where(x => x.Type == 1 && x.SeiFutSyu == 0).FirstOrDefault();
                if (newValue && !lstItemGridCheck.ContainsKey(key))
                {
                    ReportActive = true;
                    lstItemGridCheck.Add(key, newValue);
                    // Set value for list param
                    lstItemParam.Add(key, new OutDataTable() { UkeNo = data.UkeNo, FutTumRen = data.FutTumRen, FutuUnkRen = data.FutuUnkRen, SeiFutSyu = data.SeiFutSyu });
                    if (firstToTal[0] != null)
                    {
                        currentTotalBySeiFutSyu = firstToTal.Where(x => x.SeiFutSyu == data.SeiFutSyu).SingleOrDefault();
                        if (currentTotalBySeiFutSyu == null)
                        {
                            currentRow = new BillCheckListTotalData()
                            {
                                Type = 1,
                                SeiFutSyu = data.SeiFutSyu,
                                BillAmountTotal = data.BillAmount,
                                DepositAmountTotal = data.DepositAmount,
                                UnpaidAmountTotal = data.UnpaidAmount,
                                SalesAmountTotal = data.SalesAmount,
                                TaxAmountTotal = data.TaxAmount,
                                CommissionAmount = data.CommissionAmount,
                            };
                            CurrentTotal.Add(currentRow);
                        }
                        else
                        {
                            currentRow = new BillCheckListTotalData();
                            currentRow = currentTotalBySeiFutSyu;
                            CurrentTotal.Remove(currentRow);
                            currentTotalBySeiFutSyu.BillAmountTotal += data.BillAmount;
                            currentTotalBySeiFutSyu.DepositAmountTotal += data.DepositAmount;
                            currentTotalBySeiFutSyu.UnpaidAmountTotal += data.UnpaidAmount;
                            currentTotalBySeiFutSyu.SalesAmountTotal += data.SalesAmount;
                            currentTotalBySeiFutSyu.TaxAmountTotal += data.TaxAmount;
                            currentTotalBySeiFutSyu.CommissionAmount += data.CommissionAmount;
                            CurrentTotal.Add(currentTotalBySeiFutSyu);
                        }
                        // add total SeiFutSyu = 0
                        currentTotalSeiFutSyuBy0 = firstToTal.Where(x => x.SeiFutSyu == 0).SingleOrDefault();
                        BillCheckListTotalData Temp = new BillCheckListTotalData();
                        Temp = currentTotalSeiFutSyuBy0;
                        CurrentTotal.Remove(Temp);
                        currentTotalSeiFutSyuBy0.BillAmountTotal += data.BillAmount;
                        currentTotalSeiFutSyuBy0.DepositAmountTotal += data.DepositAmount;
                        currentTotalSeiFutSyuBy0.UnpaidAmountTotal += data.UnpaidAmount;
                        currentTotalSeiFutSyuBy0.SalesAmountTotal += data.SalesAmount;
                        currentTotalSeiFutSyuBy0.TaxAmountTotal += data.TaxAmount;
                        currentTotalSeiFutSyuBy0.CommissionAmount += data.CommissionAmount;
                        CurrentTotal.Add(currentTotalSeiFutSyuBy0);
                    }
                    else
                    {
                        currentRow = new BillCheckListTotalData()
                        {
                            Type = 1,
                            SeiFutSyu = data.SeiFutSyu,
                            BillAmountTotal = data.BillAmount,
                            DepositAmountTotal = data.DepositAmount,
                            UnpaidAmountTotal = data.UnpaidAmount,
                            SalesAmountTotal = data.SalesAmount,
                            TaxAmountTotal = data.TaxAmount,
                            CommissionAmount = data.CommissionAmount,
                        };
                        currentTotalSeiFutSyuBy0 = new BillCheckListTotalData()
                        {
                            Type = 1,
                            SeiFutSyu = 0,
                            BillAmountTotal = data.BillAmount,
                            DepositAmountTotal = data.DepositAmount,
                            UnpaidAmountTotal = data.UnpaidAmount,
                            SalesAmountTotal = data.SalesAmount,
                            TaxAmountTotal = data.TaxAmount,
                            CommissionAmount = data.CommissionAmount,
                        };
                        CurrentTotal.Add(currentRow);
                        CurrentTotal.Add(currentTotalSeiFutSyuBy0);
                    }
                }
                if (!newValue && lstItemGridCheck.ContainsKey(key))
                {
                    lstItemGridCheck.Remove(key);
                    // Remove param
                    lstItemParam.Remove(key);
                    if (firstToTal != null)
                    {
                        currentTotalBySeiFutSyu = firstToTal.Where(x => x.SeiFutSyu == data.SeiFutSyu).SingleOrDefault();
                        if (currentTotalBySeiFutSyu != null)
                        {
                            currentRow = new BillCheckListTotalData();
                            currentRow = currentTotalBySeiFutSyu;
                            CurrentTotal.Remove(currentRow);
                            currentTotalBySeiFutSyu.BillAmountTotal -= data.BillAmount;
                            currentTotalBySeiFutSyu.DepositAmountTotal -= data.DepositAmount;
                            currentTotalBySeiFutSyu.UnpaidAmountTotal -= data.UnpaidAmount;
                            currentTotalBySeiFutSyu.SalesAmountTotal -= data.SalesAmount;
                            currentTotalBySeiFutSyu.TaxAmountTotal -= data.TaxAmount;
                            currentTotalBySeiFutSyu.CommissionAmount -= data.CommissionAmount;
                            CurrentTotal.Add(currentTotalBySeiFutSyu);

                            // reduce total SeiFutSyu = 0
                            currentTotalSeiFutSyuBy0 = firstToTal.Where(x => x.SeiFutSyu == 0).SingleOrDefault();
                            BillCheckListTotalData Temp = new BillCheckListTotalData();
                            Temp = currentTotalSeiFutSyuBy0;
                            CurrentTotal.Remove(Temp);
                            currentTotalSeiFutSyuBy0.BillAmountTotal -= data.BillAmount;
                            currentTotalSeiFutSyuBy0.DepositAmountTotal -= data.DepositAmount;
                            currentTotalSeiFutSyuBy0.UnpaidAmountTotal -= data.UnpaidAmount;
                            currentTotalSeiFutSyuBy0.SalesAmountTotal -= data.SalesAmount;
                            currentTotalSeiFutSyuBy0.TaxAmountTotal -= data.TaxAmount;
                            currentTotalSeiFutSyuBy0.CommissionAmount -= data.CommissionAmount;
                            CurrentTotal.Add(currentTotalSeiFutSyuBy0);
                        }
                    }
                }

                lstItemGridCheckPerPage[i] = newValue;
                if (!lstItemGridCheckPerPage.Contains(false))
                {
                    itemCheckAll = true;
                    if (!lstItemCheckAll.ContainsKey(CurrentPage))
                        lstItemCheckAll.Add(CurrentPage, true);
                }
                if (lstItemGridCheckPerPage.Contains(false))
                {
                    itemCheckAll = false;
                    if (lstItemCheckAll.ContainsKey(CurrentPage))
                        lstItemCheckAll.Remove(CurrentPage);
                }
                if (!lstItemGridCheckPerPage.Contains(true))
                {
                    ReportActive = false;
                }
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }
        /// <summary>
        /// event when check all button in grid change
        /// </summary>
        /// <param name="newValue"></param>
        /// <returns></returns>
        protected async Task CheckedItemAllChanged(bool newValue)
        {
            try
            {
                int i;
                int j = 0;
                int length = Math.Min((CurrentPage + 1) * RecordsPerPage, GridDatas.Count);
                if (newValue)
                {
                    ReportActive = true;
                    var myList = Enumerable.Repeat(true, GridDisplay.Count).ToList();
                    lstItemGridCheckPerPage = new List<bool>(myList);
                    if (!lstItemCheckAll.ContainsKey(CurrentPage))
                        lstItemCheckAll.Add(CurrentPage, true);
                    for (i = ((CurrentPage) * RecordsPerPage + 1); i <= length; i++)
                    {
                        if (!lstItemGridCheck.ContainsKey(i))
                        {
                            await CheckedValueGridChanged(i, j, GridDisplay[j], newValue);
                        }
                        j++;
                    }
                }
                else
                {
                    if (lstItemCheckAll.ContainsKey(CurrentPage))
                        lstItemCheckAll.Remove(CurrentPage);
                    lstItemGridCheckPerPage = new List<bool>(new bool[GridDisplay.Count]);
                    for (i = ((CurrentPage) * RecordsPerPage + 1); i <= length; i++)
                    {
                        if (lstItemGridCheck.ContainsKey(i))
                        {
                            await CheckedValueGridChanged(i, j, GridDisplay[j], newValue);
                        }
                        j++;
                    }
                }
                if (!lstItemGridCheckPerPage.Contains(true))
                {
                    ReportActive = false;
                }
                itemCheckAll = newValue;
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
           
        }
        /// <summary>
        /// change state in page number
        /// </summary>
        /// <param name="Page"></param>
        /// <returns></returns>
        protected async Task ChangeState(int Page = 0)
        {
            try
            {
                if (isChangePageNumber)
                    isChangePageNumber = false;
                if (IsValid && billAddressListDistinct.Count > 0)
                {
                    keyValueFilterPairs = GenerateFilterValueDictionaryService.GenerateForBillCheckListFormData(billCheckListForm).Result;
                    FilterConditionService.SaveFilterCondtion(keyValueFilterPairs, FormFilterName.BillCheckList, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq).Wait();
                    List<BillCheckListGridData> FirstPageData;
                    int rowCount = 0;
                    FirstPageSelect = Page;
                    (rowCount, CurrentTotal, FirstPageData) = await BillCheckListService.GetBillCheckListGridData(billCheckListForm, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, new ClaimModel().TenantID, FirstPageSelect * RecordsPerPage, RecordsPerPage);
                    GridDatas = new List<BillCheckListGridData>(new BillCheckListGridData[rowCount]);
                    paging.TotalCount = rowCount;
                    paging.currentPage = Page;
                    NumberOfPage = (GridDatas.Count() + RecordsPerPage - 1) / RecordsPerPage;
                    IsNoData = GridDatas.Count() == 0;
                    if (!IsNoData)
                    {
                        btnReportActive = true;
                        FirstPageSelect = Math.Min(Page, NumberOfPage - 1);
                        GridDatas.RemoveRange(FirstPageSelect * RecordsPerPage, FirstPageData.Count());
                        GridDatas.InsertRange(FirstPageSelect * RecordsPerPage, FirstPageData);
                        SelectPage(FirstPageSelect);
                        
                    }
                    else
                    {
                        btnReportActive = false;
                        GridDisplay = new List<BillCheckListGridData>();
                        CurrentTotal = new List<BillCheckListTotalData>();
                    }

                    lstItemGridCheck = new Dictionary<int, bool>();
                    lstItemCheckAll = new Dictionary<int, bool>();
                    lstItemParam = new Dictionary<int, OutDataTable>();
                    InvokeAsync(StateHasChanged).Wait();
                }
                else
                {
                    btnReportActive = false;
                    GridDisplay = new List<BillCheckListGridData>();
                    CurrentTotal = new List<BillCheckListTotalData>();
                    lstItemGridCheck = new Dictionary<int, bool>();
                    lstItemCheckAll = new Dictionary<int, bool>();
                    lstItemParam = new Dictionary<int, OutDataTable>();
                    if (Page == 0 && paging != null)
                    {
                        paging.TotalCount = 0;
                        paging.currentPage = Page;
                    }
                    InvokeAsync(StateHasChanged).Wait();
                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
           
        }
        /// <summary>
        /// Change size in grid
        /// </summary>
        /// <param name="e"></param>
        /// <param name="number"></param>
        protected void clickV(MouseEventArgs e, int number)
        {
            billCheckListForm.ActiveV = number;
            keyValueFilterPairs = GenerateFilterValueDictionaryService.GenerateForBillCheckListFormData(billCheckListForm).Result;
            FilterConditionService.SaveFilterCondtion(keyValueFilterPairs, FormFilterName.BillCheckList, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq).Wait();
        }
        /// <summary>
        /// When click all all item active, when click second item if 2 after active => all item active...
        /// </summary>
        /// <param name="e"></param>
        /// <param name="number"></param>
        protected async void clickChooseTypeTotal(MouseEventArgs e, int number)
        {
            try
            {
                // if click all all item active
                if (number == 4 && billCheckListForm.lstActiveTypeTotal.Count < 3)
                {
                    if (!billCheckListForm.lstActiveTypeTotal.Contains(1))
                        billCheckListForm.lstActiveTypeTotal.Add(1);
                    if (!billCheckListForm.lstActiveTypeTotal.Contains(2))
                        billCheckListForm.lstActiveTypeTotal.Add(2);
                    if (!billCheckListForm.lstActiveTypeTotal.Contains(3))
                        billCheckListForm.lstActiveTypeTotal.Add(3);
                }
                else if (billCheckListForm.lstActiveTypeTotal.Count == 3 && number == 4)
                {
                    if (billCheckListForm.lstActiveTypeTotal.Contains(1))
                        billCheckListForm.lstActiveTypeTotal.Remove(1);
                    if (billCheckListForm.lstActiveTypeTotal.Contains(2))
                        billCheckListForm.lstActiveTypeTotal.Remove(2);
                    if (billCheckListForm.lstActiveTypeTotal.Contains(3))
                        billCheckListForm.lstActiveTypeTotal.Remove(3);
                }
                // if click first item first item active
                if (number != 4 && !billCheckListForm.lstActiveTypeTotal.Contains(number))
                {
                    billCheckListForm.lstActiveTypeTotal.Add(number);
                }
                else
                {
                    billCheckListForm.lstActiveTypeTotal.Remove(number);
                }
                billCheckListForm.lstActiveTypeTotal.Sort();
                keyValueFilterPairs = GenerateFilterValueDictionaryService.GenerateForBillCheckListFormData(billCheckListForm).Result;
                FilterConditionService.SaveFilterCondtion(keyValueFilterPairs, FormFilterName.BillCheckList, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq).Wait();
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
           
        }
        /// <summary>
        /// adjust height when tab change
        /// </summary>
        protected async void AdjustHeightWhenTabChanged()
        {
            try
            {
                await Task.Run(() =>
                {
                    InvokeAsync(StateHasChanged).Wait();
                    JSRuntime.InvokeVoidAsync("loadPageScript", "billCheckListPage", "AdjustHeight");
                });
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }
        /// <summary>
        /// event when select bill address change
        /// </summary>
        /// <param name="idBillAddress"></param>
        protected void billAddressChanged(dynamic idBillAddress)
        {
            try
            {
                billCheckListForm.BillAdress = (BillAddress)idBillAddress;
                keyValueFilterPairs = GenerateFilterValueDictionaryService.GenerateForBillCheckListFormData(billCheckListForm).Result;
                FilterConditionService.SaveFilterCondtion(keyValueFilterPairs, FormFilterName.BillCheckList, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq).Wait();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }
        /// <summary>
        /// Get color for each row in grid check list data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected int GetColorPattern(BillCheckListGridData data)
        {
            if (data is null)
            {
                return 0;
            }
            try
            {
                
                BillCheckListColorPattern result;
                if (data.NyuKinKbn == 2)
                {
                    result = BillCheckListColorPattern.Deposited;
                }
                else if (data.NCouKbn == 2)
                {
                    result = BillCheckListColorPattern.Coupon;
                }
                else if (data.NCouKbn == 3)
                {
                    result = BillCheckListColorPattern.Some;
                }
                else if (data.NyuKinKbn == 3)
                {
                    result = BillCheckListColorPattern.PartialEntry;
                }
                else if (data.NyuKinKbn == 4)
                {
                    result = BillCheckListColorPattern.OverPayment;
                }
                else
                {
                    result = BillCheckListColorPattern.NotPayment;
                }
                return (int)result;
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
                return 0;
            }
            
        }
        /// <summary>
        /// select data when select page chage
        /// </summary>
        /// <param name="index"></param>
        protected async void SelectPage(int index)
        {
            try
            {
                isLoading = true;
                if (isChangePageNumber)
                {
                    await Task.Run(() =>
                    {
                        InvokeAsync(StateHasChanged).Wait();
                        ChangeState().Wait();

                    });
                }
                List<BillCheckListGridData> TempGridData = GridDatas.GetRange(index * RecordsPerPage, Math.Min(RecordsPerPage, GridDatas.Count() - index * RecordsPerPage));

                if (TempGridData.Count() > 0 && TempGridData[0] == null)
                {
                    await Task.Run(() =>
                    {
                        InvokeAsync(StateHasChanged).Wait();
                        GetPageData(index).Wait();
                        if (!lstItemCheckAll.ContainsKey(CurrentPage))
                        {
                            itemCheckAll = false;

                            lstItemGridCheckPerPage = new List<bool>(new bool[GridDisplay.Count]);
                            foreach (var item in lstItemGridCheck)
                            {
                                if (((item.Key - 1 + RecordsPerPage) / RecordsPerPage - 1) == CurrentPage)
                                    lstItemGridCheckPerPage[item.Key - (CurrentPage * RecordsPerPage) - 1] = true;
                            }
                        }
                        else
                        {
                            itemCheckAll = true;
                            var myList = Enumerable.Repeat(true, GridDisplay.Count).ToList();
                            lstItemGridCheckPerPage = new List<bool>(myList);
                        }
                        InvokeAsync(StateHasChanged).Wait();
                        JSRuntime.InvokeVoidAsync("scrollToTop");
                    });
                }
                else
                {
                    GridDisplay = TempGridData;
                    CurrentPage = index;
                    if (!lstItemCheckAll.ContainsKey(CurrentPage))
                    {
                        itemCheckAll = false;
                        lstItemGridCheckPerPage = new List<bool>(new bool[GridDisplay.Count]);
                        foreach (var item in lstItemGridCheck)
                        {
                            if (((item.Key - 1 + RecordsPerPage) / RecordsPerPage - 1) == CurrentPage)
                                lstItemGridCheckPerPage[item.Key - (CurrentPage * RecordsPerPage) - 1] = true;
                        }
                    }
                    else
                    {
                        itemCheckAll = true;
                        var myList = Enumerable.Repeat(true, GridDisplay.Count).ToList();
                        lstItemGridCheckPerPage = new List<bool>(myList);
                    }

                }
                // Calculator total in per page
                List<BillCheckListTotalData> lstPerPage = new List<BillCheckListTotalData>();
                List<BillCheckListGridData> lstTemp = new List<BillCheckListGridData>();
                lstTemp = GridDisplay;
                lstPerPage = lstTemp.GroupBy(x => x.SeiFutSyu).Select(g => new BillCheckListTotalData
                {
                    Type = 2,
                    SeiFutSyu = g.Key,
                    BillAmountTotal = g.Sum(s => s.BillAmount),
                    DepositAmountTotal = g.Sum(s => s.DepositAmount),
                    UnpaidAmountTotal = g.Sum(s => s.UnpaidAmount),
                    SalesAmountTotal = g.Sum(s => s.SalesAmount),
                    TaxAmountTotal = g.Sum(s => s.TaxAmount),
                    CommissionAmount = g.Sum(s => s.CommissionAmount)
                }).DefaultIfEmpty().ToList();
                if (lstPerPage[0] != null)
                {
                    BillCheckListTotalData total = new BillCheckListTotalData()
                    {
                        Type = 2,
                        SeiFutSyu = 0,
                        BillAmountTotal = lstPerPage.Sum(x => x.BillAmountTotal),
                        DepositAmountTotal = lstPerPage.Sum(x => x.DepositAmountTotal),
                        UnpaidAmountTotal = lstPerPage.Sum(x => x.UnpaidAmountTotal),
                        SalesAmountTotal = lstPerPage.Sum(x => x.SalesAmountTotal),
                        TaxAmountTotal = lstPerPage.Sum(x => x.TaxAmountTotal),
                        CommissionAmount = lstPerPage.Sum(x => x.CommissionAmount)

                    };
                    CurrentTotal = CurrentTotal.Where(x => x.Type == 1 || x.Type == 3).DefaultIfEmpty().ToList();
                    CurrentTotal.Add(total);
                    foreach (var item in lstPerPage)
                    {
                        CurrentTotal.Add(item);
                    }
                }
                if (isLoading)
                    isLoading = false;
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }
        /// <summary>
        /// Get data for  page
        /// </summary>
        /// <param name="PageNo"></param>
        /// <returns></returns>
        protected async Task GetPageData(int PageNo)
        {
            try
            {
                int rowCount = 0;
                int OffSet = PageNo * RecordsPerPage;
                List<BillCheckListGridData> PageData;
                (rowCount, CurrentTotal, PageData) = await BillCheckListService.GetBillCheckListGridData(billCheckListForm, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, new ClaimModel().TenantID, OffSet, RecordsPerPage);
                GridDatas.RemoveRange(OffSet, PageData.Count());
                GridDatas.InsertRange(OffSet, PageData);
                GridDisplay = PageData;
                CurrentPage = PageNo;
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }
        /// <summary>
        /// Get number for page number
        /// </summary>
        /// <returns></returns>
        protected int[] GetPagination()
        {
            try
            {
                if (NumberOfPage <= MaxPageCount)
                {
                    return Enumerable.Range(0, NumberOfPage).ToArray();
                }
                else
                {
                    int BeginIndex = ((int)Math.Floor(CurrentPage * 1.0 / MaxPageCount)) * MaxPageCount;
                    int Count = Math.Min(MaxPageCount, NumberOfPage - BeginIndex);
                    return Enumerable.Range(BeginIndex, Count).ToArray();
                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
                return Enumerable.Range(0, NumberOfPage).ToArray();
            }
            
        }
        /// <summary>
        /// Display date from data for grid
        /// </summary>
        /// <param name="Ymd"></param>
        /// <returns></returns>
        public DateTime? DateDisplayValue(string Ymd)
        {
            try
            {
                DateTime DateValue;
                string DateFormat = "yyyyMMdd";
                if (!DateTime.TryParseExact(Ymd, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateValue))
                {
                    return null;
                }
                else
                {
                    return DateTime.ParseExact(Ymd, DateFormat, CultureInfo.InvariantCulture);
                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
                return null;
            }
            
        }
        
        /// <summary>
        /// event when click button pre
        /// </summary>
        ///
        protected async void OnClickButtonPre(MouseEventArgs e)
        {
            try
            {
                CurrentPage = 0;
                int idexPre = billAddressListDistinct.IndexOf(billCheckListForm.BillAdress) - 1;
                if (idexPre == 0)
                {
                    billCheckListForm.BillAdress = billAddressListDistinct[idexPre];
                }
                else
                {
                    if (idexPre > 0)
                    {
                        billCheckListForm.BillAdress = billAddressListDistinct[idexPre];
                    }
                }
                isLoading = true;
                await Task.Run(() =>
                {
                    InvokeAsync(StateHasChanged).Wait();
                    isLoading = false;
                    ChangeState().Wait();
                });
                InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }
        /// <summary>
        /// event when click button next
        /// </summary>
        ///
        protected async void OnClickButtonNext(MouseEventArgs e)
        {
            try
            {
                CurrentPage = 0;
                int idexNext = billAddressListDistinct.IndexOf(billCheckListForm.BillAdress) + 1;
                if (idexNext == (billAddressListDistinct.Count - 1))
                {
                    billCheckListForm.BillAdress = billAddressListDistinct[idexNext];
                }
                else
                {
                    if (idexNext < (billAddressListDistinct.Count - 1))
                    {
                        billCheckListForm.BillAdress = billAddressListDistinct[idexNext];
                    }
                }
                isLoading = true;
                await Task.Run(() =>
                {
                    InvokeAsync(StateHasChanged).Wait();
                    isLoading = false;
                    ChangeState().Wait();
                });
                InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }
        /// <summary>
        /// Clear conditon when click button clear
        /// </summary>
        ///
        protected async void ClearConditionFunction()
        {
            try
            {
                CurrentPage = 0;
                billCheckListForm = new BillsCheckListFormData()
                {
                    BillOffice = billOfficeList.FirstOrDefault(),
                    // report
                    PageSize = lstPageSize.FirstOrDefault(),
                    ActiveHeaderOption = lstOutType.FirstOrDefault(),
                    GroupType = lstGroupType.FirstOrDefault(),
                    DelimiterType = lstDelimiterType[2],
                    BillIssuedClassification = lstBillIssuedClassification.FirstOrDefault(),
                    BillTypeOrder = lstBillTypeSortGridData.FirstOrDefault()
                };
                billCheckListForm.ActiveV = (int)ViewMode.Medium;
                billCheckListForm.lstActiveTypeTotal = new List<int> { 1, 2, 3 };
                isLoading = true;
                await Task.Run(() =>
                {
                    InvokeAsync(StateHasChanged).Wait();
                    LoadBillAdressDict().Wait();
                    ChangeState().Wait();
                    isLoading = false;
                });
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }
        /// <summary>
        /// event when click button bill print
        /// </summary>
        ///
        protected void OnClickButtonBillPrint(MouseEventArgs e)
        {
            string baseUrl = AppSettingsService.GetBaseUrl();
            List<OutDataTable> lstPrint = lstItemParam.Values.ToList();
            foreach(var item in lstPrint) {
                item.supOutSiji = 3;
            }
            OutDataTableModel outDataTableModel = new OutDataTableModel() { outDataTables = lstPrint };
            var billParams = EncryptHelper.EncryptToUrl(outDataTableModel);
            var url = baseUrl + "/billprint" + string.Format("/?lstInfo={0}", billParams);
            JSRuntime.InvokeVoidAsync("open", url, "_blank");            
        }
        #endregion

        #region Report
        protected void OnSetOutputSetting(byte value)
        {
            activeButtonReport = value;
            if (activeButtonReport == 3)
            {
                isDisableCsv = false;
            }
            else
            {
                isDisableCsv = true;
            }
            StateHasChanged();
        }
        protected void ExportBtnClicked()
        {
            try
            {
                if (IsValid)
                {
                    if (billCheckListForm.OutputType == OutputReportType.Preview)
                    {
                        var searchString = EncryptHelper.EncryptToUrl(billCheckListForm);
                        JSRuntime.InvokeVoidAsync("open", "billchecklistreportpreview?searchString=" + searchString, "_blank");
                    }
                    else
                    {
                        isLoading = true;
                        Task.Run(() =>
                        {
                            InvokeAsync(StateHasChanged).Wait();
                            if (billCheckListForm.OutputType == OutputReportType.CSV)
                            {
                                PrintCsv();
                            }
                            else
                            {
                                Print();
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
        private void SetTableHeader(DataTable table)
        {
            try
            {
                List<string> listHeader = new List<string>() { "請求営業所コード", "請求営業所名", "請求営業所略名",
                "請求先業者コード", "請求先コード", "請求先支店コード", "請求先業者コード名", "請求先名",
                "請求先支店名", "請求先略名", "請求先支店略名", "請求年月日", "受付番号", "受付営業所コード",
                "受付営業所名", "受付営業所略名", "団体名", "行き先名", "配車年月日", "到着年月日", "請求付帯種別",
                "請求付帯種別名", "付帯積込品名", "精算コード", "精算名", "数量", "単価", "請求額", "入金年月日",
                "入金合計", "未収額", "売上額", "消費税額", "手数料率", "手数料額", "発生年月日", "発行年月日",
                "得意先コード使用開始年月日", "得意先コード使用終了年月日", "得意先支店コード使用開始年月日",
                "得意先支店コード使用終了年月日", "台数", "車種単価" };
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
        private async void PrintCsv()
        {
            try
            {
                var listData = await BillCheckListService.GetBillCheckListReportCsv(billCheckListForm, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, new ClaimModel().TenantID);
                var dt = listData.ToDataTable<BillCheckListModelCsvData>();
                while (dt.Columns.Count > 43)
                {
                    dt.Columns.RemoveAt(43);
                }
                SetTableHeader(dt);
                string path = string.Format("{0}/csv/{1}.csv", hostingEnvironment.WebRootPath, Guid.NewGuid());

                bool isWithHeader = billCheckListForm.ActiveHeaderOption.IdValue == 0 ? true : false;
                bool isEnclose = billCheckListForm.GroupType.IdValue == 0 ? true : false;
                string space = billCheckListForm.DelimiterType.IdValue == 0 ? "\t" : billCheckListForm.DelimiterType.IdValue == 1 ? ";" : ",";

                var result = CsvHelper.ExportDatatableToCsv(dt, path, true, isWithHeader, isEnclose, space);
                await new System.Threading.Tasks.TaskFactory().StartNew(() =>
                {
                    string myExportString = Convert.ToBase64String(result);
                    if (isLoading)
                    {
                        isLoading = false;
                        InvokeAsync(StateHasChanged).Wait();
                    }
                    JSRuntime.InvokeVoidAsync("downloadFileClientSide", myExportString, "csv", "BillCheckListReport");
                });
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }
        private async void Print()
        {
            try
            {
                var data = await BillCheckListService.GetBillCheckListReportData(billCheckListForm, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, new ClaimModel().TenantID);
                if (data.Count > 0)
                {
                    dynamic report = new Reports.BillCheckListReportA4();
                    if (billCheckListForm.PageSize.IdValue == BillTypePagePrintList.BillTypePagePrintData[1].IdValue)
                    {
                        report = new Reports.BillCheckListReportA3();
                    }
                    else
                    {
                        if (billCheckListForm.PageSize.IdValue == BillTypePagePrintList.BillTypePagePrintData[2].IdValue)
                            report = new Reports.BillCheckListReportB4();
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
                            JSRuntime.InvokeVoidAsync("downloadFileClientSide", myExportString, "pdf", "BillCheckListReport");
                            
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }

        }
        #endregion
    }
}
