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
using HassyaAllrightCloud.Domain.Dto.BillingList;
using Microsoft.AspNetCore.Components.Forms;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Pages.Components;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Services;
using HassyaAllrightCloud.IService.CommonComponents;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;

namespace HassyaAllrightCloud.Pages
{
    public class BillingListBase : ComponentBase
    {
        [CascadingParameter(Name = "ClaimModel")]
        protected ClaimModel ClaimModel { get; set; }
        #region Inject
        [Inject]
        protected IStringLocalizer<BillingList> Lang { get; set; }
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }
        [Inject]
        protected ITPM_CodeKbListService CodeKbListService { get; set; }
        [Inject]
        protected ITPM_YoyKbnDataListService YoyKbnDataListService { get; set; }
        [Inject]
        protected IBillingListService billingListService { get; set; }
        [Inject]
        protected IBillCheckListService BillCheckListService { get; set; }
        [Inject]
        protected ICustomerListService CustomerService { get; set; }
        [Inject]
        protected NavigationManager navigationManager { get; set; }
        [Inject]
        protected IWebHostEnvironment hostingEnvironment { get; set; }
        [Inject] public ILoadingService _loading { get; set; }
        [Inject]
        protected IErrorHandlerService errorModalService { get; set; }
        [Inject]
        protected IFilterCondition FilterConditionService { get; set; }
        [Inject] protected IReservationClassComponentService ReservationClassComponentService { get; set; }
        [Inject] protected ICustomerComponentService CustomerComponentService { get; set; }
        [Inject]
        IGenerateFilterValueDictionary GenerateFilterValueDictionaryService { get; set; }
        [Inject] public AppSettingsService AppSettingsService { get; set; }
        #endregion

        #region Propeties and variable
        public Dictionary<string, string> LangDic = new Dictionary<string, string>();
        protected List<BillingListDetailGrid> billingListDetailGrids { get; set; } = new List<BillingListDetailGrid>();
        protected List<BillingListDetailGrid> billingListDetailGridsCheck { get; set; } = new List<BillingListDetailGrid>();
        protected List<BillingListGrid> billingListGrids { get; set; } = new List<BillingListGrid>();
        protected List<BillingListTotal> CurrentTotal { get; set; } = new List<BillingListTotal>();
        protected int ActiveTabIndex
        {
            get => activeTabIndex;
            set
            {
                activeTabIndex = value;
                AdjustHeightWhenTabChanged();
            }
        }
        protected bool ReportActive { get; set; } = false;
        protected bool NotActiveBillAdress { get; set; } = false;
        protected bool itemCheckAll { get; set; } = false;
        public bool isFirstRender { get; set; } = true;

        protected BillingListFilter billingListFilter { get; set; } = new BillingListFilter();
        protected List<BillOfficeData> billOfficeList = new List<BillOfficeData>();
        protected List<InvoiceType> billClassificationList = new List<InvoiceType>();
        public List<ReservationClassComponentData> ListReservationClass { get; set; } = new List<ReservationClassComponentData>();
        protected List<CustomerComponentGyosyaData> ListGyosya { get; set; } = new List<CustomerComponentGyosyaData>();
        protected List<CustomerComponentTokiskData> ListTokisk { get; set; } = new List<CustomerComponentTokiskData>();
        protected List<CustomerComponentTokiStData> ListTokiSt { get; set; } = new List<CustomerComponentTokiStData>();
        protected List<bool> lstItemGridCheckPerPage = new List<bool>();
        
        protected List<string> codeList = new List<string>();
        protected List<TransferAmountOutputClassification> transferAmountOutputClassifications = new List<TransferAmountOutputClassification>();
        protected Pagination paging = new Pagination();
        Dictionary<string, string> keyValueFilterPairs = new Dictionary<string, string>();
        public byte itemPerPage { get; set; } = 25;

        public int NumberOfPage { get; set; }
        protected int CurrentPage = 1;
        protected string dateFormat = "yy/MM/dd";
        protected int MaxPageCount = 5;
        protected int activeTabIndex = 0;
        protected bool IsValid = true;
        protected bool IsNoData = false;
        public int syainCdSeq { get; set; }
        // For report
        protected bool isDisableCsv { get; set; } = true;
        protected int activeButtonReport = 0;
        protected bool btnReportActive = true;
        protected OutputReportType ActiveButtonReport
        {
            get => billingListFilter.OutputType;
            set
            {
                billingListFilter.OutputType = value;
                StateHasChanged();
            }
        }
        public EditContext formContext;
        public string errorMessage { get; set; }
        public List<ComboboxFixField> BillTypePagePrintData = new List<ComboboxFixField>();
        public List<OutDataTable> outDataTables { get; set; } = new List<OutDataTable>();
        public BillingListSum billingListSum = new BillingListSum();
        public Components.CommonComponents.CustomerComponent startCustomerComponent { get; set; }
        = new Components.CommonComponents.CustomerComponent();
        public Components.CommonComponents.CustomerComponent endCustomerComponent { get; set; }
        = new Components.CommonComponents.CustomerComponent();
        #endregion

        #region Function
        protected override async Task OnInitializedAsync()
        {
            try
            {
                await Init();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public async Task Init()
        {
            if (ClaimModel != null)
            {
                syainCdSeq = ClaimModel.SyainCdSeq;
            }
            LangDic = Lang.GetAllStrings().ToDictionary(l => l.Name, l => l.Value);
            billingListFilter.ActiveV = (int)ViewMode.Medium;
            CurrentTotal.Add(new BillingListTotal()
            {
                Text = "選択計",
                BillAmount = 0,
                CommissionAmount = 0,
                DepositAmount = 0,
                SalesAmount = 0,
                SeiFutSyu = 0,
                TaxAmount = 0,
                UnpaidAmount = 0,
                Type = 1
            });
            formContext = new EditContext(billingListFilter);

            billClassificationList = (await CodeKbListService.GetdataSEIKYUKBN(new ClaimModel().TenantID)).ToList();

            // Add data for combobox BillIssuedClassificationListData
            BillIssuedClassificationListData.BillIssuedClassificationList[0].StringValue = Lang["BillIssuedClassification_" + BillIssuedClassificationListData.BillIssuedClassificationList[0].IdValue.ToString()];
            BillIssuedClassificationListData.BillIssuedClassificationList[1].StringValue = Lang["BillIssuedClassification_" + BillIssuedClassificationListData.BillIssuedClassificationList[1].IdValue.ToString()];
            BillIssuedClassificationListData.BillIssuedClassificationList[2].StringValue = Lang["BillIssuedClassification_" + BillIssuedClassificationListData.BillIssuedClassificationList[2].IdValue.ToString()];
            // Add data for
            BillTypeSortGridList.BillTypeSortGridData[0].StringValue = Lang["BillTypeSort_" + BillTypeSortGridList.BillTypeSortGridData[0].IdValue.ToString()];
            BillTypeSortGridList.BillTypeSortGridData[1].StringValue = Lang["BillTypeSort_" + BillTypeSortGridList.BillTypeSortGridData[1].IdValue.ToString()];
            // set value for combobox BillTypeOrder

            billOfficeList = await BillCheckListService.GetBillOffice(new ClaimModel().TenantID);
            ListReservationClass = await ReservationClassComponentService.GetListReservationClass();
            ListGyosya = await CustomerComponentService.GetListGyosya();
            ListTokisk = await CustomerComponentService.GetListTokisk();
            ListTokiSt = await CustomerComponentService.GetListTokiSt();

            //transferAmountOutputClassifications
            transferAmountOutputClassifications.Add(new TransferAmountOutputClassification { Code = 1, Name = Lang["NotTransferAmount"] });
            transferAmountOutputClassifications.Add(new TransferAmountOutputClassification { Code = 2, Name = Lang["TransferAmount"] });
            // Set data for tab report

            // Add data for report page type
            BillTypePagePrintData = new List<ComboboxFixField>
            {
                new ComboboxFixField { IdValue = 0, StringValue = "A4" },
                new ComboboxFixField { IdValue = 1, StringValue = "A3" },
                new ComboboxFixField { IdValue = 2, StringValue = "B4" }
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
            InitFilter();
            List<TkdInpCon> filterValues = await FilterConditionService.GetFilterCondition(FormFilterName.BillingList, 0, syainCdSeq);
            if (filterValues.Any())
            {
                foreach (var item in filterValues)
                {
                    var propertyInfo = billingListFilter.GetType().GetProperty(item.ItemNm);
                    switch (item.ItemNm)
                    {
                        case nameof(billingListFilter.CloseDate):
                        case nameof(billingListFilter.StartReceiptNumber):
                        case nameof(billingListFilter.EndReceiptNumber):
                            if (item.ItemNm == nameof(billingListFilter.CloseDate))
                                propertyInfo.SetValue(billingListFilter, string.IsNullOrWhiteSpace(item.JoInput) ? (byte?)null : byte.Parse(item.JoInput), null);
                            else
                                propertyInfo.SetValue(billingListFilter, string.IsNullOrWhiteSpace(item.JoInput) ? (long?)null : long.Parse(item.JoInput), null);
                            break;
                        case nameof(billingListFilter.ActiveV):
                            propertyInfo.SetValue(billingListFilter, int.Parse(item.JoInput), null);
                            break;
                        case nameof(billingListFilter.startCustomerComponentGyosyaData):
                        case nameof(billingListFilter.endCustomerComponentGyosyaData):
                            if (!string.IsNullOrWhiteSpace(item.JoInput))
                            {
                                var gyosya = ListGyosya.Where(x => x.GyosyaCdSeq == int.Parse(item.JoInput)).FirstOrDefault();
                                propertyInfo.SetValue(billingListFilter, gyosya, null);
                                if (item.ItemNm == nameof(billingListFilter.startCustomerComponentGyosyaData))
                                    startCustomerComponent.OnChangeDefaultGyosya(gyosya.GyosyaCdSeq);
                                else
                                    endCustomerComponent.OnChangeDefaultGyosya(gyosya.GyosyaCdSeq);
                            }
                            break;
                        case nameof(billingListFilter.startCustomerComponentTokiskData):
                        case nameof(billingListFilter.endCustomerComponentTokiskData):
                            if (!string.IsNullOrWhiteSpace(item.JoInput))
                            {
                                var tokisk = ListTokisk.Where(x => x.TokuiSeq == int.Parse(item.JoInput)).FirstOrDefault();
                                propertyInfo.SetValue(billingListFilter, tokisk, null);
                                if (item.ItemNm == nameof(billingListFilter.startCustomerComponentGyosyaData))
                                    startCustomerComponent.OnChangeDefaultTokisk(tokisk.TokuiSeq);
                                else
                                    endCustomerComponent.OnChangeDefaultTokisk(tokisk.TokuiSeq);
                            }
                            break;
                        case nameof(billingListFilter.startCustomerComponentTokiStData):
                        case nameof(billingListFilter.endCustomerComponentTokiStData):
                            if (!string.IsNullOrWhiteSpace(item.JoInput))
                            {
                                var tokist = ListTokiSt.Where(x => x.TokuiSeq.ToString() + x.SitenCdSeq.ToString() == item.JoInput).FirstOrDefault();
                                propertyInfo.SetValue(billingListFilter, tokist, null);
                                if (item.ItemNm == nameof(billingListFilter.startCustomerComponentGyosyaData))
                                    startCustomerComponent.OnChangeDefaultTokiSt(tokist.SitenCdSeq);
                                else
                                    endCustomerComponent.OnChangeDefaultTokiSt(tokist.SitenCdSeq);
                            }
                            break;
                        case nameof(billingListFilter.BillDate):
                            var datetime = string.IsNullOrWhiteSpace(item.JoInput) ? (DateTime?)null : DateTime.ParseExact(item.JoInput, "yyyyMMdd", CultureInfo.InvariantCulture);
                            propertyInfo.SetValue(billingListFilter, datetime, null);
                            break;
                        case nameof(billingListFilter.BillTypes):
                            var billTypes = string.IsNullOrWhiteSpace(item.JoInput) ? new List<int>() : item.JoInput.Split(",").Select(int.Parse).ToList();
                            propertyInfo.SetValue(billingListFilter, billTypes, null);
                            break;
                        case nameof(billingListFilter.BillOffice):
                            if (!string.IsNullOrWhiteSpace(item.JoInput))
                            {
                                var billOffice = billOfficeList.Where(x => x.EigyoCd == item.JoInput).FirstOrDefault();
                                propertyInfo.SetValue(billingListFilter, billOffice, null);
                            }
                            break;
                        case nameof(billingListFilter.StartReservationClassification):
                        case nameof(billingListFilter.EndReservationClassification):
                            if (!string.IsNullOrWhiteSpace(item.JoInput))
                            {
                                var reservationClassification = ListReservationClass.Where(x => x.YoyaKbnSeq == int.Parse(item.JoInput)).FirstOrDefault();
                                propertyInfo.SetValue(billingListFilter, reservationClassification, null);
                            }
                            break;
                        case nameof(billingListFilter.StartBillClassification):
                        case nameof(billingListFilter.EndBillClassification):
                            if (!string.IsNullOrWhiteSpace(item.JoInput))
                            {
                                var billClassification = billClassificationList.Where(x => x.CodeKbn == item.JoInput).FirstOrDefault();
                                propertyInfo.SetValue(billingListFilter, billClassification, null);
                            }
                            break;
                        case nameof(billingListFilter.BillIssuedClassification):
                            if (!string.IsNullOrWhiteSpace(item.JoInput))
                            {
                                var billIssuedClassification = BillIssuedClassificationListData.BillIssuedClassificationList.Where(x => x.IdValue == int.Parse(item.JoInput)).FirstOrDefault();
                                propertyInfo.SetValue(billingListFilter, billIssuedClassification, null);
                            }
                            break;
                        case nameof(billingListFilter.BillTypeOrder):
                        case nameof(billingListFilter.PageSize):
                        case nameof(billingListFilter.ActiveHeaderOption):
                        case nameof(billingListFilter.GroupType):
                        case nameof(billingListFilter.DelimiterType):
                            if (!string.IsNullOrWhiteSpace(item.JoInput))
                            {
                                List<ComboboxFixField> data = item.ItemNm == nameof(billingListFilter.BillTypeOrder) ? BillTypeSortGridList.BillTypeSortGridData
                                    : item.ItemNm == nameof(billingListFilter.PageSize) ? BillTypePagePrintData : item.ItemNm == nameof(billingListFilter.ActiveHeaderOption) ? ShowHeaderOptions.ShowHeaderOptionData
                                    : item.ItemNm == nameof(billingListFilter.GroupType) ? GroupTypes.GroupTypeData : item.ItemNm == nameof(billingListFilter.DelimiterType) ? DelimiterTypes.DelimiterTypeData
                                    : new List<ComboboxFixField>();
                                propertyInfo.SetValue(billingListFilter, data.Where(x => x.StringValue == item.JoInput).FirstOrDefault(), null);
                            }
                            break;

                        case nameof(billingListFilter.TransferAmountOutputClassification):
                            if (!string.IsNullOrWhiteSpace(item.JoInput))
                            {
                                var billIssuedClassification = transferAmountOutputClassifications.Where(x => x.Code == int.Parse(item.JoInput)).FirstOrDefault();
                                propertyInfo.SetValue(billingListFilter, billIssuedClassification, null);
                            }
                            break;
                        case nameof(billingListFilter.itemFare):
                        case nameof(billingListFilter.itemIncidental):
                        case nameof(billingListFilter.itemTollFee):
                        case nameof(billingListFilter.itemArrangementFee):
                        case nameof(billingListFilter.itemGuideFee):
                        case nameof(billingListFilter.itemLoaded):
                        case nameof(billingListFilter.itemCancellationCharge):
                        case nameof(billingListFilter.isListMode):
                            var status = item.JoInput == "0" ? false : true; ;
                            propertyInfo.SetValue(billingListFilter, status, null);
                            break;
                        case nameof(billingListFilter.OutputType):
                            OutputReportType outputType = (OutputReportType)int.Parse(item.JoInput);
                            propertyInfo.SetValue(billingListFilter, outputType, null);
                            break;
                    }
                }
            }
            billClassificationList.Insert(0, null);
            if (billingListFilter.isListMode)
                await SelectPageForListGrid(0);
            else
                await SelectPage(0, null);
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                await JSRuntime.InvokeVoidAsync("loadPageScript", "billinglist", "fadeToggleTable");
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public async Task Reset()
        {
            try
            {
                await FilterConditionService.DeleteCustomFilerCondition(syainCdSeq, 0, FormFilterName.BillingList);
                await Init();
                await JSRuntime.InvokeVoidAsync("loadPageScript", "billinglist", "clickSearchDropDown");
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }
        public void InitFilter()
        {
            try
            {
                billingListFilter.CloseDate = null;
                billingListFilter.ActiveV = (int)ViewMode.Medium;
                billingListFilter.OutputType = OutputReportType.Preview;
                billingListFilter.BillIssuedClassification = BillIssuedClassificationListData.BillIssuedClassificationList.FirstOrDefault();
                billingListFilter.BillTypeOrder = BillTypeSortGridList.BillTypeSortGridData.FirstOrDefault();
                billingListFilter.BillOffice = billOfficeList.FirstOrDefault();
                billingListFilter.TransferAmountOutputClassification = transferAmountOutputClassifications.FirstOrDefault();
                billingListFilter.PageSize = BillTypePagePrintList.BillTypePagePrintData[0];
                billingListFilter.ActiveHeaderOption = ShowHeaderOptions.ShowHeaderOptionData[0];
                billingListFilter.GroupType = GroupTypes.GroupTypeData[0];
                billingListFilter.DelimiterType = DelimiterTypes.DelimiterTypeData[2];
                billingListFilter.PageSize = BillTypePagePrintData.FirstOrDefault();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public async Task SelectPage(int index, string code)
        {
            try
            {
                if (index < 0)
                    return;
                await _loading.ShowAsync();
                errorMessage = string.Empty;
                //param code != null : choose from code, else choose from form search
                billingListFilter.Code = code;
                if (string.IsNullOrWhiteSpace(code))
                {
                    codeList = await billingListService.GetBillingListDetailCodeAsync(billingListFilter);
                    if(codeList == null || !codeList.Any())
                    {
                        billingListDetailGrids.Clear();
                        IsNoData = !billingListDetailGrids.Any();
                        await _loading.HideAsync();
                        await InvokeAsync(StateHasChanged);
                        return;
                    }
                    billingListFilter.Code = codeList.FirstOrDefault();
                }
                CurrentPage = index;
                billingListFilter.Offset = index * itemPerPage;
                billingListFilter.Limit = itemPerPage;
                var result = await billingListService.GetBillingListDetailAsync(billingListFilter);
                billingListDetailGrids = result.billingListDetailGrids;
                NumberOfPage = result.CountNumber;
                IsNoData = !billingListDetailGrids.Any();
                if (IsNoData)
                {
                    await _loading.HideAsync();
                    await InvokeAsync(StateHasChanged);
                    return;
                }

                await SaveFilterCondition();
                CurrentTotal = CurrentTotal.Where(x => x.Type == 1).ToList();
                CurrentTotal.AddRange(result.billingListTotals);
                for (byte i = 1; i <= 7; i++)
                {
                    var item = InitTotalData(i);
                    if (item != null)
                        CurrentTotal.Add(item);
                }

                var pageItem = CurrentTotal.Where(x => x.Type == 2);
                if (pageItem.Any())
                {
                    BillingListTotal billingListTotal = new BillingListTotal()
                    {
                        Text = "頁計",
                        BillAmount = pageItem.Sum(x => x.BillAmount),
                        CommissionAmount = pageItem.Sum(x => x.CommissionAmount),
                        DepositAmount = pageItem.Sum(x => x.DepositAmount),
                        SalesAmount = pageItem.Sum(x => x.SalesAmount),
                        SeiFutSyu = 0,
                        TaxAmount = pageItem.Sum(x => x.TaxAmount),
                        UnpaidAmount = pageItem.Sum(x => x.UnpaidAmount),
                        Type = 2
                    };
                    CurrentTotal.Add(billingListTotal);
                }

                var totalItem = CurrentTotal.Where(x => x.Type == 3);
                if (totalItem.Any())
                {
                    BillingListTotal billingListTotal = new BillingListTotal()
                    {
                        Text = "累計",
                        BillAmount = totalItem.Sum(x => x.BillAmount),
                        CommissionAmount = totalItem.Sum(x => x.CommissionAmount),
                        DepositAmount = totalItem.Sum(x => x.DepositAmount),
                        SalesAmount = totalItem.Sum(x => x.SalesAmount),
                        SeiFutSyu = 0,
                        TaxAmount = totalItem.Sum(x => x.TaxAmount),
                        UnpaidAmount = totalItem.Sum(x => x.UnpaidAmount),
                        Type = 3
                    };
                    CurrentTotal.Add(billingListTotal);
                }

                foreach (var item in billingListDetailGrids)
                {
                    var data = billingListDetailGridsCheck.Where(x => x.UkeNo == item.UkeNo && x.MisyuRen == item.MisyuRen).FirstOrDefault();
                    if (data != null)
                        item.Checked = data.Checked;
                }
                itemCheckAll = billingListDetailGrids.All(x => billingListDetailGridsCheck.Any(y => (y.UkeNo == x.UkeNo) && (y.MisyuRen == x.MisyuRen) && x.Checked));
                await _loading.HideAsync();
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
                await _loading.HideAsync();
                await InvokeAsync(StateHasChanged);
            }
        }

        public BillingListTotal InitTotalData(byte index)
        {
            try
            {
                var items = billingListDetailGrids.Where(x => x.SeiFutSyu == index);
                if (!items.Any())
                {
                    return null;
                }
                BillingListTotal billingListTotal = new BillingListTotal()
                {
                    Text = "頁計",
                    BillAmount = items.Sum(x => x.SeiKin),
                    CommissionAmount = items.Sum(x => x.SyaRyoTes),
                    DepositAmount = items.Sum(x => (long)x.NyuKinRui),
                    SalesAmount = items.Sum(x => x.UriGakKin),
                    SeiFutSyu = index,
                    TaxAmount = items.Sum(x => x.SyaRyoSyo),
                    UnpaidAmount = items.Sum(x => x.SeiKin - (long)x.NyuKinRui),
                    Type = 2
                };
                return billingListTotal;
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
                return null;
            }
        }
        public async Task SelectPageForListGrid(int index)
        {
            try
            {
                await _loading.ShowAsync();
                errorMessage = string.Empty;
                if (index >= 0)
                {
                    CurrentPage = index;
                    billingListFilter.Offset = index * itemPerPage;
                    billingListFilter.Limit = itemPerPage;
                    var result = await billingListService.GetBillingListAsync(billingListFilter);
                    billingListGrids = result.billingListGrids;
                    billingListSum = result.billingListSum;
                    NumberOfPage = result.CountNumber;
                    IsNoData = !billingListGrids.Any();
                    if(!IsNoData)
                        await SaveFilterCondition();
                }
                await _loading.HideAsync();
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        /// <summary>
        /// change value combobox
        /// </summary>
        /// <param name="ValueName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected async Task ChangeValueForm(string ValueName, dynamic value)
        {
            try
            {
                if (value is string && string.IsNullOrEmpty(value))
                    value = null;
                var propertyInfo = billingListFilter.GetType().GetProperty(ValueName);
                propertyInfo.SetValue(billingListFilter, value, null);
                StateHasChanged();
                if (!formContext.Validate())
                {
                    StateHasChanged();
                    return;
                }
                InitCurrentTotal();
                billingListDetailGridsCheck = new List<BillingListDetailGrid>();
                if (ValueName == nameof(billingListFilter.startCustomerComponentGyosyaData)
                    || ValueName == nameof(billingListFilter.startCustomerComponentTokiskData)
                    || ValueName == nameof(billingListFilter.endCustomerComponentGyosyaData)
                    || ValueName == nameof(billingListFilter.endCustomerComponentTokiskData))
                    return;
                if (billingListFilter.isListMode)
                    await SelectPageForListGrid(0);
                else
                    await SelectPage(0, ValueName == nameof(billingListFilter.Code) ? value : null);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public async Task SaveFilterCondition()
        {
            try
            {
                keyValueFilterPairs = await GenerateFilterValueDictionaryService.GenerateBillingListFilter(billingListFilter);
                await FilterConditionService.SaveFilterCondtion(keyValueFilterPairs, FormFilterName.BillingList, 0, syainCdSeq);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
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
                    case nameof(billingListFilter.itemFare):
                        billingListFilter.itemFare = Convert.ToBoolean(newValue);
                        if (billingListFilter.itemFare)
                            billingListFilter.BillTypes.Add(1);
                        else
                            billingListFilter.BillTypes.Remove(1);
                        break;
                    case nameof(billingListFilter.itemIncidental):
                        billingListFilter.itemIncidental = Convert.ToBoolean(newValue);
                        if (billingListFilter.itemIncidental)
                            billingListFilter.BillTypes.Add(2);
                        else
                            billingListFilter.BillTypes.Remove(2);
                        break;
                    case nameof(billingListFilter.itemTollFee):
                        billingListFilter.itemTollFee = Convert.ToBoolean(newValue);
                        if (billingListFilter.itemTollFee)
                            billingListFilter.BillTypes.Add(3);
                        else
                            billingListFilter.BillTypes.Remove(3);
                        break;
                    case nameof(billingListFilter.itemArrangementFee):
                        billingListFilter.itemArrangementFee = Convert.ToBoolean(newValue);
                        if (billingListFilter.itemArrangementFee)
                            billingListFilter.BillTypes.Add(4);
                        else
                            billingListFilter.BillTypes.Remove(4);
                        break;
                    case nameof(billingListFilter.itemGuideFee):
                        billingListFilter.itemGuideFee = Convert.ToBoolean(newValue);
                        if (billingListFilter.itemGuideFee)
                            billingListFilter.BillTypes.Add(5);
                        else
                            billingListFilter.BillTypes.Remove(5);
                        break;
                    case nameof(billingListFilter.itemLoaded):
                        billingListFilter.itemLoaded = Convert.ToBoolean(newValue);
                        if (billingListFilter.itemLoaded)
                            billingListFilter.BillTypes.Add(6);
                        else
                            billingListFilter.BillTypes.Remove(6);
                        break;
                    default:
                        billingListFilter.itemCancellationCharge = Convert.ToBoolean(newValue);
                        if (billingListFilter.itemCancellationCharge)
                            billingListFilter.BillTypes.Add(7);
                        else
                            billingListFilter.BillTypes.Remove(7);
                        break;
                }
                if (formContext.Validate())
                {
                    InitCurrentTotal();
                    billingListDetailGridsCheck = new List<BillingListDetailGrid>();
                    if (billingListFilter.isListMode)
                        await SelectPageForListGrid(0);
                    else
                        await SelectPage(0, null);
                }
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public void InitCurrentTotal()
        {
            try
            {
                CurrentTotal.Clear();
                CurrentTotal.Add(new BillingListTotal()
                {
                    Text = "選択計",
                    BillAmount = 0,
                    CommissionAmount = 0,
                    DepositAmount = 0,
                    SalesAmount = 0,
                    SeiFutSyu = 0,
                    TaxAmount = 0,
                    UnpaidAmount = 0,
                    Type = 1
                });
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
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
                if (number == 4 && billingListFilter.lstActiveTypeTotal.Count < 3)
                {
                    if (!billingListFilter.lstActiveTypeTotal.Contains(1))
                        billingListFilter.lstActiveTypeTotal.Add(1);
                    if (!billingListFilter.lstActiveTypeTotal.Contains(2))
                        billingListFilter.lstActiveTypeTotal.Add(2);
                    if (!billingListFilter.lstActiveTypeTotal.Contains(3))
                        billingListFilter.lstActiveTypeTotal.Add(3);
                }
                else if (billingListFilter.lstActiveTypeTotal.Count == 3 && number == 4)
                {
                    if (billingListFilter.lstActiveTypeTotal.Contains(1))
                        billingListFilter.lstActiveTypeTotal.Remove(1);
                    if (billingListFilter.lstActiveTypeTotal.Contains(2))
                        billingListFilter.lstActiveTypeTotal.Remove(2);
                    if (billingListFilter.lstActiveTypeTotal.Contains(3))
                        billingListFilter.lstActiveTypeTotal.Remove(3);
                }
                // if click first item first item active
                if (number != 4 && !billingListFilter.lstActiveTypeTotal.Contains(number))
                {
                    billingListFilter.lstActiveTypeTotal.Add(number);
                }
                else
                {
                    billingListFilter.lstActiveTypeTotal.Remove(number);
                }
                billingListFilter.lstActiveTypeTotal.Sort();
                await SaveFilterCondition();
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
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
                errorModalService.HandleError(ex);
            }
        }

        protected void CheckedValueGridChanged(BillingListDetailGrid data, bool newValue, bool isCheckAll)
        {
            try
            {
                var item = billingListDetailGridsCheck.Where(x => x.UkeNo == data.UkeNo && x.MisyuRen == data.MisyuRen).FirstOrDefault();
                if (item == null)
                {
                    billingListDetailGridsCheck.Add(data);
                }
                else
                {
                    if (item.Checked && data.Checked && isCheckAll && newValue)
                    {
                        item.Checked = (item != null) && newValue;
                        return;
                    }
                    item.Checked = (item != null) && newValue;
                }
                data.Checked = newValue;
                itemCheckAll = billingListDetailGrids.All(x => billingListDetailGridsCheck.Any(y => (y.UkeNo == x.UkeNo) && (y.MisyuRen == x.MisyuRen) && x.Checked));
                var billingListTotal = CurrentTotal.Where(x => x.SeiFutSyu == data.SeiFutSyu && x.Type == 1).FirstOrDefault();
                var totalSelectedItem = CurrentTotal.Where(x => x.SeiFutSyu == 0 && x.Type == 1).FirstOrDefault();

                if (data.Checked)
                {
                    if (billingListTotal == null)
                    {
                        var newItem = new BillingListTotal()
                        {
                            Text = "選択計",
                            BillAmount = data.SeiKin,
                            CommissionAmount = data.SyaRyoTes,
                            DepositAmount = (long)data.NyuKinRui,
                            SalesAmount = data.UriGakKin,
                            SeiFutSyu = data.SeiFutSyu,
                            TaxAmount = data.SyaRyoSyo,
                            UnpaidAmount = data.SeiKin - (long)data.NyuKinRui,
                            Type = 1
                        };
                        CurrentTotal.Add(newItem);
                    }
                    else
                    {
                        billingListTotal.BillAmount += data.SeiKin;
                        billingListTotal.CommissionAmount += data.SyaRyoTes;
                        billingListTotal.DepositAmount += (long)data.NyuKinRui;
                        billingListTotal.SalesAmount += data.UriGakKin;
                        billingListTotal.TaxAmount += data.SyaRyoSyo;
                        billingListTotal.UnpaidAmount += data.SeiKin - (long)data.NyuKinRui;
                    }

                    if (totalSelectedItem != null)
                    {
                        totalSelectedItem.BillAmount += data.SeiKin;
                        totalSelectedItem.CommissionAmount += data.SyaRyoTes;
                        totalSelectedItem.DepositAmount += (long)data.NyuKinRui;
                        totalSelectedItem.SalesAmount += data.UriGakKin;
                        totalSelectedItem.TaxAmount += data.SyaRyoSyo;
                        totalSelectedItem.UnpaidAmount += data.SeiKin - (long)data.NyuKinRui;
                    }
                }
                else
                {
                    if (totalSelectedItem != null)
                    {
                        totalSelectedItem.BillAmount -= data.SeiKin;
                        totalSelectedItem.CommissionAmount -= data.SyaRyoTes;
                        totalSelectedItem.DepositAmount -= (long)data.NyuKinRui;
                        totalSelectedItem.SalesAmount -= data.UriGakKin;
                        totalSelectedItem.TaxAmount -= data.SyaRyoSyo;
                        totalSelectedItem.UnpaidAmount -= data.SeiKin - (long)data.NyuKinRui;
                        CurrentTotal.Remove(billingListTotal);
                    }
                }
                if (!isCheckAll)
                {
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected void CheckedItemAllChanged(bool newValue)
        {
            foreach (var item in billingListDetailGrids)
            {
                CheckedValueGridChanged(item, newValue, true);
            }
            StateHasChanged();
        }

        /// <summary>
        /// Get color for each row in grid check list data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected int GetColorPattern(BillingListDetailGrid data)
        {
            try
            {
                if (data is null)
                {
                    return 0;
                }
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
                errorModalService.HandleError(ex);
                return 0;
            }
        }

        public async void OnClickButtonPre()
        {
            try
            {
                int idexPre = codeList.IndexOf(billingListFilter.Code) - 1;
                if (idexPre == 0)
                    billingListFilter.Code = codeList[idexPre];
                else
                {
                    if (idexPre > 0)
                        billingListFilter.Code = codeList[idexPre];
                }
                if (formContext.Validate())
                {
                    billingListDetailGridsCheck = new List<BillingListDetailGrid>();
                    //LastRowCheckDatas = new DepositCouponGrid();
                    InitCurrentTotal();
                    await SelectPage(0, billingListFilter.Code);
                }
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }
        /// <summary>
        /// event when click button next
        /// </summary>
        ///
        public async void OnClickButtonNext()
        {
            try
            {
                int idexNext = codeList.IndexOf(billingListFilter.Code) + 1;
                if (idexNext == (codeList.Count - 1))
                    billingListFilter.Code = codeList[idexNext];
                else
                {
                    if (idexNext < (codeList.Count - 1))
                        billingListFilter.Code = codeList[idexNext];
                }
                if (formContext.Validate())
                {
                    billingListDetailGridsCheck = new List<BillingListDetailGrid>();
                    //LastRowCheckDatas = new DepositCouponGrid();
                    InitCurrentTotal();
                    await SelectPage(0, billingListFilter.Code);
                }
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public async Task ChangeGridMode()
        {
            try
            {
                //bool isListMode = billingListFilter.isListMode;
                //billingListFilter = new BillingListFilter();
                //formContext = new EditContext(billingListFilter);
                //InitFilter();
                billingListFilter.isListMode = !billingListFilter.isListMode;
                paging = new Pagination();
                itemPerPage = 25;
                NumberOfPage = 0;
                if (billingListFilter.isListMode)
                {
                    billingListFilter.TransferAmountOutputClassification = transferAmountOutputClassifications.FirstOrDefault();
                    await SelectPageForListGrid(0);
                }
                else
                    await SelectPage(0, null);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public void OpenBillPrintPopUp()
        {
            try
            {
                outDataTables.Clear();
                foreach (var item in billingListDetailGridsCheck)
                {
                    var outDatatable = new OutDataTable()
                    {
                        FutTumRen = item.FutTumRen,
                        FutuUnkRen = item.FutuUnkRen,
                        SeiFutSyu = item.SeiFutSyu,
                        UkeNo = item.UkeNo,
                        supOutSiji = 3
                    };
                    outDataTables.Add(outDatatable);
                }
                string baseUrl = AppSettingsService.GetBaseUrl();
                OutDataTableModel outDataTableModel = new OutDataTableModel() { outDataTables = outDataTables };
                var billParams = EncryptHelper.EncryptToUrl(outDataTableModel);
                var url = baseUrl + "/billprint" + string.Format("/?lstInfo={0}", billParams);
                JSRuntime.InvokeVoidAsync("open", url, "_blank");   
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected async Task OnChangePage(int page)
        {
            try
            {
                if (billingListFilter.isListMode)
                    await SelectPageForListGrid(page);
                else
                    await SelectPage(page, billingListFilter.Code);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected void OnChangeItemPerPage(byte _itemPerPage)
        {
            try
            {
                itemPerPage = _itemPerPage;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected void OnClickLayoutSetting(byte type)
        {
            try
            {
                if (type == 0)
                {
                    
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
            StateHasChanged();
        }
        #endregion

        #region Report
        protected void OnSetOutputSetting(byte value)
        {
            try
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
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }
        protected async Task ExportBtnClicked()
        {
            try
            {
                if (!IsValid)
                {
                    await InvokeAsync(StateHasChanged);
                    return;
                }

                await _loading.ShowAsync();
                if (billingListFilter.OutputType == OutputReportType.Preview)
                {
                    var searchString = EncryptHelper.EncryptToUrl(billingListFilter);
                    await JSRuntime.InvokeVoidAsync("openNewUrlInNewTab", "billinglistreportpreview?searchString=" + searchString);

                    BillTypePagePrintData = new List<ComboboxFixField>
                {
                    new ComboboxFixField { IdValue = 0, StringValue = "A4" },
                    new ComboboxFixField { IdValue = 1, StringValue = "A3" },
                    new ComboboxFixField { IdValue = 2, StringValue = "B4" }
                };
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    if (billingListFilter.OutputType == OutputReportType.CSV)
                    {
                        await PrintCsv();
                    }
                    else
                    {
                        await Print();
                    }
                }
                await _loading.HideAsync();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                await _loading.HideAsync();
                errorModalService.HandleError(ex);
            }
        }
        private void SetTableHeader(DataTable table)
        {
            try
            {
                List<string> listHeader = new List<string>();
                if (!billingListFilter.isListMode)
                {
                    listHeader = new List<string>() { "請求営業所コード", "請求営業所名", "請求営業所略名",
                "請求先業者コード", "請求先コード", "請求先支店コード", "請求先業者コード名", "請求先名",
                "請求先支店名", "請求先略名", "請求先支店略名", "請求年月日", "受付番号", "受付営業所コード",
                "受付営業所名", "受付営業所略名", "団体名", "行き先名", "配車年月日", "到着年月日", "請求付帯種別",
                "請求付帯種別名", "付帯積込品名", "精算コード", "精算名", "数量", "単価", "請求額", "入金年月日",
                "入金合計", "未収額", "売上額", "消費税額", "手数料率", "手数料額", "発生年月日", "発行年月日",
                "得意先コード使用開始年月日", "得意先コード使用終了年月日", "得意先支店コード使用開始年月日",
                "得意先支店コード使用終了年月日", "台数", "車種単価" };
                }
                else
                {
                    listHeader = new List<string>() { "請求営業所コード", "請求営業所名", "請求営業所略名",
                "請求先業者コード", "請求先コード", "請求先支店コード", "請求先コードSEQ", "請求先支店コードSEQ",
                "請求先業者コード名", "請求先名", "請求先支店名", "請求先略名", "請求先支店略名", "運賃前月繰越額",
                "運賃売上額", "運賃消費税額", "運賃手数料区分", "運賃手数料区分名", "運賃手数料額", "運賃請求額",
                "運賃入金合計", "運賃未収残", "運賃前受金", "ガイド料前月繰越額", "ガイド料売上額", "ガイド料消費税額",
                "ガイド料手数料区分", "ガイド料手数料区分名", "ガイド料手数料額", "ガイド料請求額", "ガイド料入金合計",
                "ガイド料未収残", "ガイド料前受金", "その他付帯前月繰越額", "その他付帯売上額", "その他付帯消費税額",
                "その他手数料区分", "その他手数料区分名", "その他付帯手数料額", "その他付帯請求額", "その他付帯入金合計",
                "その他付帯未収残", "その他付帯前受金", "キャンセル料前月繰越額", "キャンセル料金額", "キャンセル料消費税額",
                "キャンセル料請求額", "キャンセル料入金合計", "キャンセル料未収残", "キャンセル料前受金", "合計前月繰越額",
                "合計売上額", "合計消費税額", "合計手数料額", "合計請求額", "合計入金合計", "合計未収残", "合計前受金",
                "運賃・ガイド・キャンセルの合計前月繰越額", "運賃・ガイド・キャンセルの合計売上額", "運賃・ガイド・キャンセルの合計消費税額",
                "運賃・ガイド・キャンセルの合計手数料額", "運賃・ガイド・キャンセルの合計請求額", "運賃・ガイド・キャンセルの合計入金合計",
                "運賃・ガイド・キャンセルの合計未収残", "運賃・ガイド・キャンセルの合計前受金" };
                }
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    table.Columns[i].ColumnName = listHeader[i];
                }
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }
        private async Task PrintCsv()
        {
            try
            {
                dynamic dt = null;
                if (billingListFilter.isListMode)
                {
                    var res = await billingListService.GetBillingListCsvAsync(billingListFilter);
                    dt = res.ToDataTable<BillingListGridCsvData>();
                    //while (dt.Columns.Count > 66)
                    //{
                    //    dt.Columns.RemoveAt(66);
                    //}
                }
                else
                {
                    var res = await billingListService.GetBillingListDetailCsvAsync(billingListFilter);
                    dt = res.ToDataTable<BillingListDetailGridCsvData>();
                }

                SetTableHeader(dt);
                string path = string.Format("{0}/csv/{1}.csv", hostingEnvironment.WebRootPath, Guid.NewGuid());

                bool isWithHeader = billingListFilter.ActiveHeaderOption.IdValue == 0 ? true : false;
                bool isEnclose = billingListFilter.GroupType.IdValue == 0 ? true : false;
                string space = billingListFilter.DelimiterType.IdValue == 0 ? "\t" : billingListFilter.DelimiterType.IdValue == 1 ? ";" : ",";

                var result = CsvHelper.ExportDatatableToCsv(dt, path, true, isWithHeader, isEnclose, space);
                await new System.Threading.Tasks.TaskFactory().StartNew(() =>
                {
                    string myExportString = Convert.ToBase64String(result);
                    JSRuntime.InvokeVoidAsync("downloadFileClientSide", myExportString, "csv", "BillingListReport");
                });
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }
        private async Task Print()
        {
            try
            {
                if (billingListFilter.isListMode)
                {
                    var data = await billingListService.GetBillingListReportAsync(billingListFilter);
                    if (data.Any())
                    {
                        dynamic report = new Reports.ReportTemplate.BillingListReport.BillingListReportA4();
                        if (billingListFilter.PageSize.IdValue == BillTypePagePrintList.BillTypePagePrintData[1].IdValue)
                        {
                            report = new Reports.ReportTemplate.BillingListReport.BillingListReportA3();
                        }
                        else
                        {
                            if (billingListFilter.PageSize.IdValue == BillTypePagePrintList.BillTypePagePrintData[2].IdValue)
                                report = new Reports.ReportTemplate.BillingListReport.BillingListReportB4();
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
                                JSRuntime.InvokeVoidAsync("downloadFileClientSide", myExportString, "pdf", "BillingListReport");
                            }
                        });
                    }
                }
                else
                {
                    var data = await billingListService.GetBillingListDetailReportAsync(billingListFilter);
                    if (data.Any())
                    {
                        dynamic report = new Reports.ReportTemplate.BillingListReport.BillingListDetailReportA4();
                        if (billingListFilter.PageSize.IdValue == BillTypePagePrintList.BillTypePagePrintData[1].IdValue)
                        {
                            report = new Reports.ReportTemplate.BillingListReport.BillingListDetailReportA3();
                        }
                        else
                        {
                            if (billingListFilter.PageSize.IdValue == BillTypePagePrintList.BillTypePagePrintData[2].IdValue)
                                report = new Reports.ReportTemplate.BillingListReport.BillingListDetailReportB4();
                        }
                        report.DataSource = data;
                        await new System.Threading.Tasks.TaskFactory().StartNew(() =>
                        {
                            report.CreateDocument();
                            using (MemoryStream ms = new MemoryStream())
                            {
                                if (billingListFilter.OutputType == OutputReportType.Print)
                                {
                                    PrintToolBase tool = new PrintToolBase(report.PrintingSystem);
                                    tool.Print();
                                    return;
                                }
                                report.ExportToPdf(ms);

                                byte[] exportedFileBytes = ms.ToArray();
                                string myExportString = Convert.ToBase64String(exportedFileBytes);
                                JSRuntime.InvokeVoidAsync("downloadFileClientSide", myExportString, "pdf", "BillingListDetailReport");
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }
        #endregion
    }
}
