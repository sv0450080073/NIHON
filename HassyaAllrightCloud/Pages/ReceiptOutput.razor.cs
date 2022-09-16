using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System;
using System.Web;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Forms;
using HassyaAllrightCloud.Commons.Helpers;
using System.Linq;
using System.IO;
using HassyaAllrightCloud.Pages.Components;
using HassyaAllrightCloud.Commons.Extensions;
using FluentValidation;
using SharedLibraries.UI.Services;
using SharedLibraries.Utility.Models;
using SharedLibraries.Utility.Constant;
using System.Drawing;
using HassyaAllrightCloud.Pages.Components.CommonComponents;

namespace HassyaAllrightCloud.Pages
{
    public class ReceiptOutputBase : ComponentBase
    {
        #region Inject
        [Inject]
        protected IJSRuntime _jSRuntime { get; set; }
        [Inject]
        protected IStringLocalizer<ReceiptOutput> _lang { get; set; }
        [Inject]
        protected IReceiptOutputService _service { get; set; }
        [Inject]
        protected IErrorHandlerService _errorModalService { get; set; }
        [Inject]
        protected IGenerateFilterValueDictionary _generateFilterValueDictionaryService { get; set; }
        [Inject]
        protected IFilterCondition _filterConditionService { get; set; }
        [Inject]
        protected ISharedLibrariesApi _s3Service { get; set; }
        [Inject]
        protected ISharedLibrariesApi _sharedLibrariesApi { get; set; }
        [Inject] protected NavigationManager _navigation { get; set; }
        [Inject]
        private ILoadingService _loading { get; set; }
        [Inject]
        private IBillPrintService _billPrintService { get; set; }
        [Inject] private IReportLayoutSettingService _reportLayoutSettingService { get; set; }
        [CascadingParameter(Name = "ClaimModel")] protected ClaimModel ClaimModel { get; set; }
        #endregion Inject

        #region Property
        protected EditContext editFormContext { get; set; }
        protected int activeTabIndex = 0;
        protected int ActiveTabIndex { get => activeTabIndex; set { activeTabIndex = value; StateHasChanged(); } }
        protected ReceiptOutputFormSeachModel searchModel { get; set; }
        protected List<BillOfficeReceipt> billOfficeReceipt = new List<BillOfficeReceipt>();
        //protected List<BillAddressReceiptFromTo> billAddressFrom = new List<BillAddressReceiptFromTo>();
        //protected List<BillAddressReceiptFromTo> billAddressTo = new List<BillAddressReceiptFromTo>();
        protected List<BillAddressReceipt> billAddressReceipt = new List<BillAddressReceipt>();
        public Dictionary<string, string> _langDic = new Dictionary<string, string>();
        protected BillAddressReceipt SelectedBillAddressReceipt { get; set; } = new BillAddressReceipt();
        protected List<Invoice> DataSource = new List<Invoice>();
        protected List<Invoice> DataSourcePaging = new List<Invoice>();
        protected S3File S3File = new S3File();
        protected List<Invoice> SelectedInvoices { get; set; }
        public BillOfficeReceipt SelectedBillOffice { get; set; }
        public CustomerComponent CustomerRef { get; set; }
        public DefaultCustomerData DefaultValueFrom { get; set; }
        public DefaultCustomerData DefaultValueTo { get; set; }

        protected int ActiveV { get; set; }
        protected bool ItemCheckAll { get; set; } = false;
        public bool IsLoading { get; set; }
        protected string SelectedBillingAmount;
        protected string SelectedTaxAmount;
        protected string SelectedFeeAmount;
        protected bool IsShowPopupOutput { get; set; } = false;
        protected bool IsShowPopupDowload { get; set; } = false;
        public byte ItemPerPage { get; set; } = Common.DefaultPageSize;
        public int CurrentPage { get; set; }
        protected Pagination Paging = new Pagination();
        //check show message data not found
        protected bool IsDataNotFound { get; set; } = false;
        public string InvoiceYearMoth { get; set; }
        protected bool IsFirstRender { get; set; } = true;
        public string FileName { get; set; }
        public string Url { get; set; }
        public bool isGyosyaValid { get; set; } = true;
        public bool isTokiskValid { get; set; } = true;
        public bool isTokistValid { get; set; } = true;

        #endregion Property

        #region Function
        protected override async Task OnInitializedAsync()
        {
            try
            {
                await InitData();
                await OnSearch(true);
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                _errorModalService.HandleError(ex);
            }
        }

        protected async Task InitData()
        {
            Paging.currentPage = 0;
            CurrentPage = 0;
            SelectedBillingAmount = "0";
            SelectedTaxAmount = "0";
            SelectedFeeAmount = "0";
            SelectedInvoices = new List<Invoice>();

            InvoiceYearMoth = await _service.GetInvoiceYearMonth();
            var dataLang = _lang.GetAllStrings();
            var result = await _service.GetCommonListItems(searchModel);
            billOfficeReceipt = result?.BillOfficeReceipts;
            //billAddressFrom = result?.BillAddressReceiptFromTos;
            //billAddressTo = result?.BillAddressReceiptFromTos;
            searchModel = new ReceiptOutputFormSeachModel();
            searchModel.CustomerModelFrom = new CustomerModel();
            searchModel.CustomerModelTo = new CustomerModel();
            DefaultValueFrom = new DefaultCustomerData();
            DefaultValueTo = new DefaultCustomerData();

            //get and load lates conditon search
            var filterValues = _filterConditionService.GetFilterCondition(FormFilterName.ReceiptOutput, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq).Result;

            if (filterValues.Any())
            {
                searchModel.BillOffice = billOfficeReceipt.FirstOrDefault(x => x.EigyoCdSeq == (filterValues.FirstOrDefault(x => x.ItemNm == nameof(ReceiptOutputFormSeachModel.BillOffice)) == null ? 0
                                                                       : int.Parse(filterValues.FirstOrDefault(x => x.ItemNm == nameof(ReceiptOutputFormSeachModel.BillOffice))?.JoInput)));
                searchModel.StaInvoicingDate = filterValues.FirstOrDefault(x => x.ItemNm == nameof(ReceiptOutputFormSeachModel.StaInvoicingDate)) != null
                                                 ? DateTime.ParseExact(filterValues.FirstOrDefault(x => x.ItemNm == nameof(ReceiptOutputFormSeachModel.StaInvoicingDate))?.JoInput, Formats.yyyyMMdd, null) : (DateTime?)null;
                searchModel.EndInvoicingDate = filterValues.FirstOrDefault(x => x.ItemNm == nameof(ReceiptOutputFormSeachModel.EndInvoicingDate)) != null
                                                 ? DateTime.ParseExact(filterValues.FirstOrDefault(x => x.ItemNm == nameof(ReceiptOutputFormSeachModel.EndInvoicingDate))?.JoInput, Formats.yyyyMMdd, null) : (DateTime?)null;
                searchModel.InvoiceYearMonth = filterValues.FirstOrDefault(x => x.ItemNm == nameof(ReceiptOutputFormSeachModel.InvoiceYearMonth)) != null
                                                 ? DateTime.ParseExact(filterValues.FirstOrDefault(x => x.ItemNm == nameof(ReceiptOutputFormSeachModel.InvoiceYearMonth))?.JoInput, Formats.yyyyMMdd, null) : (DateTime?)null;
                //searchModel.BillAddressFrom = billAddressFrom.FirstOrDefault(x => x.TokuiCdSeq == (filterValues.FirstOrDefault(x => x.ItemNm == nameof(ReceiptOutputFormSeachModel.BillAddressFrom)) == null
                //                                 ? 0 : int.Parse(filterValues.FirstOrDefault(x => x.ItemNm == nameof(ReceiptOutputFormSeachModel.BillAddressFrom))?.JoInput.Split(',')[0]))
                //                                                               && x.SitenCdSeq == (filterValues.FirstOrDefault(x => x.ItemNm == nameof(ReceiptOutputFormSeachModel.BillAddressFrom)) == null ? 0 : int.Parse(filterValues.FirstOrDefault(x => x.ItemNm == nameof(ReceiptOutputFormSeachModel.BillAddressFrom))?.JoInput.Split(',')[1])));
                //searchModel.BillAddressTo = billAddressFrom.FirstOrDefault(x => x.TokuiCdSeq == (filterValues.FirstOrDefault(x => x.ItemNm == nameof(ReceiptOutputFormSeachModel.BillAddressTo)) == null ? 0 : int.Parse(filterValues.FirstOrDefault(x => x.ItemNm == nameof(ReceiptOutputFormSeachModel.BillAddressTo))?.JoInput.Split(',')[0]))
                //                                                             && x.SitenCdSeq == (filterValues.FirstOrDefault(x => x.ItemNm == nameof(ReceiptOutputFormSeachModel.BillAddressTo)) == null ? 0 : int.Parse(filterValues.FirstOrDefault(x => x.ItemNm == nameof(ReceiptOutputFormSeachModel.BillAddressTo))?.JoInput.Split(',')[1])));

                var customerModelFrom = (filterValues.FirstOrDefault(x => x.ItemNm == nameof(searchModel.CustomerModelFrom))?.JoInput)?.Split(',');
                var customerModelTo = (filterValues.FirstOrDefault(x => x.ItemNm == nameof(searchModel.CustomerModelTo))?.JoInput)?.Split(',');
                DefaultValueFrom.GyosyaCdSeq = int.Parse(customerModelFrom != null ? customerModelFrom[0] : "-1");
                DefaultValueFrom.TokiskCdSeq = int.Parse(customerModelFrom != null ? customerModelFrom[1] : "-1");
                DefaultValueFrom.TokiStCdSeq = int.Parse(customerModelFrom != null ? customerModelFrom[2] : "-1");
                DefaultValueTo.GyosyaCdSeq = int.Parse(customerModelTo != null ? customerModelTo[0] : "-1");
                DefaultValueTo.TokiskCdSeq = int.Parse(customerModelTo != null ? customerModelTo[1] : "-1");
                DefaultValueTo.TokiStCdSeq = int.Parse(customerModelTo != null ? customerModelTo[2] : "-1");

                searchModel.ActiveV = filterValues.FirstOrDefault(x => x.ItemNm == nameof(ReceiptOutputFormSeachModel.ActiveV))?.JoInput == null
                                                 ? (int)ViewMode.Medium : int.Parse(filterValues.FirstOrDefault(x => x.ItemNm == nameof(ReceiptOutputFormSeachModel.ActiveV))?.JoInput);
                searchModel.StaInvoiceOutNum = filterValues.FirstOrDefault(x => x.ItemNm == nameof(ReceiptOutputFormSeachModel.StaInvoiceOutNum))?.JoInput != ""
                                                 ? filterValues.FirstOrDefault(x => x.ItemNm == nameof(ReceiptOutputFormSeachModel.StaInvoiceOutNum))?.JoInput?.PadLeft(8, '0') : "";
                searchModel.StaInvoiceSerNum = filterValues.FirstOrDefault(x => x.ItemNm == nameof(ReceiptOutputFormSeachModel.StaInvoiceSerNum))?.JoInput != ""
                                                 ? filterValues.FirstOrDefault(x => x.ItemNm == nameof(ReceiptOutputFormSeachModel.StaInvoiceSerNum))?.JoInput?.PadLeft(4, '0') : "";
                searchModel.EndInvoiceOutNum = filterValues.FirstOrDefault(x => x.ItemNm == nameof(ReceiptOutputFormSeachModel.EndInvoiceOutNum))?.JoInput != ""
                                                ? filterValues.FirstOrDefault(x => x.ItemNm == nameof(ReceiptOutputFormSeachModel.EndInvoiceOutNum))?.JoInput?.PadLeft(8, '0') : "";
                searchModel.EndInvoiceSerNum = filterValues.FirstOrDefault(x => x.ItemNm == nameof(ReceiptOutputFormSeachModel.EndInvoiceSerNum))?.JoInput != ""
                                                ? filterValues.FirstOrDefault(x => x.ItemNm == nameof(ReceiptOutputFormSeachModel.EndInvoiceSerNum))?.JoInput?.PadLeft(4, '0') : "";
                ActiveV = filterValues.FirstOrDefault(x => x.ItemNm == nameof(ReceiptOutputFormSeachModel.ActiveV))?.JoInput == "0" ? (int)ViewMode.Medium : int.Parse(filterValues.FirstOrDefault(x => x.ItemNm == nameof(ReceiptOutputFormSeachModel.ActiveV))?.JoInput ?? ActiveV.ToString());
            }
            else
            {
                ActiveV = (int)ViewMode.Medium;
                if (!string.IsNullOrEmpty(InvoiceYearMoth))
                    searchModel.InvoiceYearMonth = DateTime.ParseExact(InvoiceYearMoth, Formats.yyyyMM, null).AddMonths(1);
                else
                    searchModel.InvoiceYearMonth = DateTime.Now;
                SelectedBillOffice = billOfficeReceipt.FirstOrDefault(x => x.EigyoCdSeq == new ClaimModel().EigyoCdSeq);
                searchModel.BillOffice = billOfficeReceipt.FirstOrDefault(x => x.EigyoCdSeq == new ClaimModel().EigyoCdSeq);
            }

            result = await _service.GetCommonListItems(searchModel);
            billAddressReceipt = result?.BillAddressReceipts;
            searchModel.BillAddressReceipt = billAddressReceipt.FirstOrDefault();
            //billAddressFrom.Insert(0, null);
            SelectedBillAddressReceipt = billAddressReceipt.FirstOrDefault();
            _langDic = dataLang.ToDictionary(l => l.Name, l => l.Value);
            editFormContext = new EditContext(searchModel);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (IsFirstRender)
            {
                await LoadJS();
                IsFirstRender = false;
            }
        }

        /// <summary>
        /// Change tab need to reload js
        /// </summary>
        /// <returns></returns>
        public async Task LoadJS()
        {
            await _jSRuntime.InvokeVoidAsync("EnterTab");
            await _jSRuntime.InvokeVoidAsync("setEventforCodeNumberFieldV2", ".code-number-ser", true, 8);
            await _jSRuntime.InvokeVoidAsync("setEventforCodeNumberFieldV2", ".code-number-num", true, 4);
            await InvokeAsync(StateHasChanged);
        }

        protected async void UpdateFormValue(string propertyName, dynamic value)
        {
            try
            {
                await ToggleLoading(true);
                if ((propertyName == nameof(searchModel.StaInvoiceOutNum) || propertyName == nameof(searchModel.EndInvoiceOutNum)) && IsNumberic(value))
                {
                    if (!string.IsNullOrWhiteSpace(value))
                        value = string.Format("{0:D8}", int.Parse(((string)value).TruncateWithMaxLength(8)));
                    else
                        value = "";
                    SetValue(propertyName, value);
                }
                else if ((propertyName == nameof(searchModel.StaInvoiceSerNum) || propertyName == nameof(searchModel.EndInvoiceSerNum)) && IsNumberic(value))
                {
                    if (!string.IsNullOrWhiteSpace(value))
                        value = string.Format("{0:D4}", int.Parse(((string)value).TruncateWithMaxLength(4)));
                    else
                        value = "";
                    SetValue(propertyName, value);
                }
                else if (propertyName == nameof(searchModel.BillOffice) || propertyName == nameof(searchModel.StaInvoicingDate) || propertyName == nameof(searchModel.EndInvoicingDate)
                        || propertyName == nameof(searchModel.InvoiceYearMonth) || propertyName == nameof(searchModel.ActiveV))
                    SetValue(propertyName, value);
                var result = await _service.GetCommonListItems(searchModel);
                billAddressReceipt = result.BillAddressReceipts;
                searchModel.BillAddressReceipt = billAddressReceipt.FirstOrDefault();
                SelectedBillAddressReceipt = billAddressReceipt.FirstOrDefault();

                if (propertyName != nameof(searchModel.ActiveV))
                    await OnSearch(false);
                await ToggleLoading(false);
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                _errorModalService.HandleError(ex);
            }
        }

        private async void SetValue(string propertyName, dynamic value)
        {
            var propertyInfo = searchModel.GetType().GetProperty(propertyName);
            propertyInfo.SetValue(searchModel, value, null);
            //if form validate then save lastes conditon search
            if (editFormContext.Validate())
            {
                var keyValueFilterPairs = _generateFilterValueDictionaryService.GenerateForReceiptOutput(searchModel).Result;
                await _filterConditionService.SaveFilterCondtion(keyValueFilterPairs, FormFilterName.ReceiptOutput, 0, new ClaimModel().SyainCdSeq);
            }

            await InvokeAsync(StateHasChanged);
        }

        protected void ClickVAsync(MouseEventArgs e, int number)
        {
            try
            {
                ActiveV = number;
                UpdateFormValue(nameof(searchModel.ActiveV), ActiveV);
            }
            catch (Exception ex)
            {
                _errorModalService.HandleError(ex);
            }
        }

        /// <summary>
        /// Press button 請求先
        /// </summary>
        /// <param name="IsNext">Check is click next or previouse button 請求先</param>
        protected async Task BillAddressReceiptsChangedAsync(bool IsNext)
        {
            try
            {
                await _loading.ShowAsync();
                //set item uncheck to default
                CheckedItemAllChanged(false);

                if (IsNext)
                    SelectedBillAddressReceipt = billAddressReceipt.SkipWhile(x => x != SelectedBillAddressReceipt).Skip(1).DefaultIfEmpty(billAddressReceipt[0]).FirstOrDefault();
                else if (!IsNext && SelectedBillAddressReceipt == billAddressReceipt.FirstOrDefault())
                    SelectedBillAddressReceipt = null;
                else if (!IsNext)
                    SelectedBillAddressReceipt = billAddressReceipt.TakeWhile(x => x != SelectedBillAddressReceipt).DefaultIfEmpty(billAddressReceipt[billAddressReceipt.Count - 1]).LastOrDefault();
                SetValue(nameof(searchModel.BillAddressReceipt), SelectedBillAddressReceipt);
                await OnSearch(false);
                await _loading.HideAsync();
            }
            catch (Exception ex)
            {
                _errorModalService.HandleError(ex);
            }
        }

        /// <summary>
        /// Print report
        /// </summary>
        /// <returns></returns>
        protected async Task OutputBtn()
        {
            try
            {
                var claimModel = new ClaimModel();
                var tenantId = claimModel.TenantID.ToString("D5");
                IsShowPopupOutput = false;
                await _loading.ShowAsync();
                if (SelectedInvoices.Any() && searchModel.IssueDate != null)
                {
                    var result = _service.GetReceiptOutputReport(searchModel).Result;
                    if (result.Any())
                    {
                        var reportImage = await GetReportImage();
                        var receiptOutputReport = await _reportLayoutSettingService.GetCurrentTemplate(ReportIdForSetting.Receiptoutput, BaseNamespace.Receiptoutput, claimModel.TenantID, claimModel.EigyoCdSeq, (byte)PaperSize.A4);
                        var noReceiptOutputReport = await _reportLayoutSettingService.GetCurrentTemplate(ReportIdForSetting.Receiptoutput, BaseNamespace.NoReceiptoutput, claimModel.TenantID, claimModel.EigyoCdSeq, (byte)PaperSize.A4);
                        var currentTenant = await _billPrintService.GetTenantInfoAsync();
                        var date = DateTime.Now.ToString(Formats.yyyyMMdd);
                        var ryoOutSeq = result.FirstOrDefault().RyoOutSeq;
                        var filePath = $"{ClaimModel.TenantID.ToString("D5")}-{currentTenant.TenantCompanyName}/{SelectedBillAddressReceipt.TokuiSeq.ToString("D8")}-{SelectedBillAddressReceipt.TokuiNm}/領収書/{date}";

                        receiptOutputReport.DataSource = result;
                        noReceiptOutputReport.DataSource = result;

                        if (reportImage != null)
                        {
                            if (receiptOutputReport.Report.ControlType.Contains("ReceiptOutputReport1A4"))
                                (receiptOutputReport as Reports.ReceiptOutputReport1A4).SetImageSource(reportImage);
                            else
                                (receiptOutputReport as Reports.ReceiptOutputReport2A4).SetImageSource(reportImage);

                            if (noReceiptOutputReport.Report.ControlType.Contains("NoReceiptOutputReport1A4"))
                                (noReceiptOutputReport as Reports.NoReceiptOutputReport1A4).SetImageSource(reportImage);
                            else
                                (noReceiptOutputReport as Reports.NoReceiptOutputReport2A4).SetImageSource(reportImage);

                        }

                        receiptOutputReport.CreateDocument();
                        noReceiptOutputReport.CreateDocument();
                        receiptOutputReport.Pages.AddRange(noReceiptOutputReport.Pages);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            receiptOutputReport.ExportToPdf(ms);
                            byte[] exportedFileBytes = ms.ToArray();
                            FileName = $"{tenantId}_{ryoOutSeq.ToString("D8")}_領収書.pdf";

                            try
                            {
                                S3File = await _sharedLibrariesApi.UploadFileAsync(new SharedLibraries.Utility.Models.FileSendData
                                {
                                    File = exportedFileBytes,
                                    FileName = FileName,
                                    FilePath = filePath,
                                    FileSize = exportedFileBytes.Length,
                                    Password = "",
                                    TenantId = currentTenant.TenantCdSeq,
                                    UpdSyainCd = claimModel.SyainCdSeq,
                                    UpdPrgID = ""
                                });
                            }
                            catch (Exception ex)
                            {
                                _errorModalService.HandleError(ex);
                            }
                        }
                    }

                    Url = $"{_navigation.BaseUri}FileDownload?fileId={HttpUtility.UrlEncode(S3File.EncryptedId)}";
                    IsShowPopupDowload = true;
                    //refresh grid
                    await OnSearch(false);
                    //reload checkbox has checked
                    CheckedItemAllChanged(false);
                    //init list item checked
                    SelectedInvoices = new List<Invoice>();
                }
                await _loading.HideAsync();
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                _errorModalService.HandleError(ex);
            }
        }

        /// <summary>
        /// Dowload file
        /// </summary>
        /// <returns></returns>
        public async Task DowloadFile()
        {
            try
            {
                await _loading.ShowAsync();

                var model = new DownloadModel()
                {
                    FileId = S3File.EncryptedId,
                    Password = S3File.Password,
                    UpdSyainCd = new ClaimModel().SyainCdSeq,
                    UpdPrgID = ""
                };

                var result = await _sharedLibrariesApi.DownloadFileAsync(model);
                if (result != null)
                {
                    var extension = Path.GetExtension(S3File.Name);
                    string myExportString = result.Content;
                    await _jSRuntime.InvokeVoidAsync("downloadFileClientSide", myExportString, extension.Replace(".", string.Empty).ToLower(), S3File.Name.Replace(extension, string.Empty) + "_");
                }
                IsShowPopupDowload = false;

                await _loading.HideAsync();
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                _errorModalService.HandleError(ex);
                IsShowPopupDowload = false;
                await _loading.HideAsync();
            }
        }

        /// <summary>
        /// Copy url
        /// </summary>
        /// <returns></returns>
        public async Task CopyUrl()
        {
            await _jSRuntime.InvokeVoidAsync("copyText", $"copy-{("url")}-{S3File.Name}");
        }

        /// <summary>
        /// Check value input is text or number
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private bool IsNumberic(string input)
        {
            return input.All(char.IsNumber);
        }

        protected async void ClearFormSeach()
        {
            try
            {
                await ToggleLoading(true);
                ActiveV = (int)ViewMode.Medium;
                if (!string.IsNullOrEmpty(InvoiceYearMoth))
                    searchModel.InvoiceYearMonth = DateTime.ParseExact(InvoiceYearMoth, Formats.yyyyMM, null).AddMonths(1);
                else
                    searchModel.InvoiceYearMonth = DateTime.Now;
                searchModel.BillOffice = billOfficeReceipt.FirstOrDefault(x => x.EigyoCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().EigyoCdSeq);

                searchModel.StaInvoiceOutNum = "";
                searchModel.StaInvoiceSerNum = "";
                searchModel.EndInvoiceOutNum = "";
                searchModel.EndInvoiceSerNum = "";

                //searchModel.BillAddressFrom = null;
                //searchModel.BillAddressTo = null;
                searchModel.CustomerModelFrom = new CustomerModel();
                searchModel.CustomerModelTo = new CustomerModel();
                DefaultValueFrom = new DefaultCustomerData();
                DefaultValueTo = new DefaultCustomerData();

                isGyosyaValid = isTokiskValid = isTokistValid = true;

                searchModel.StaInvoicingDate = null;
                searchModel.EndInvoicingDate = null;

                //init data for 請求先
                var result = await _service.GetCommonListItems(searchModel);
                billAddressReceipt = result.BillAddressReceipts;

                searchModel.BillAddressReceipt = billAddressReceipt.FirstOrDefault();
                SelectedBillAddressReceipt = billAddressReceipt.FirstOrDefault();

                ////init caculate data
                SelectedInvoices = new List<Invoice>();
                SelectedBillingAmount = "0";
                SelectedTaxAmount = "0";
                SelectedFeeAmount = "0";

                await _filterConditionService.DeleteCustomFilerCondition(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, 0, FormFilterName.ReceiptOutput);
                await OnSearch(true);
                await ToggleLoading(false);
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                _errorModalService.HandleError(ex);
            }

        }

        private async Task ToggleLoading(bool value)
        {
            IsLoading = value;
            await InvokeAsync(StateHasChanged);
            await Task.Delay(100);
        }

        /// <summary>
        /// when checkbox all is checked
        /// </summary>
        /// <param name="newValue">check or uncheck</param>
        protected void CheckedItemAllChanged(bool newValue)
        {
            try
            {
                if (DataSourcePaging.Any())
                {
                    foreach (var item in DataSourcePaging)
                    {
                        CheckedValueGridChanged(item, newValue, true);
                    }
                    CaculateData();
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                _errorModalService.HandleError(ex);
            }
        }

        /// <summary>
        /// caculate to show 選択請求額
        /// </summary>
        private void CaculateData()
        {
            long selectedBillingAmount = 0;
            long selectedTaxAmount = 0;
            long selectedFeeAmount = 0;
            long tmpSelectedBillingAmount = 0;
            long tmpSelectedTaxAmount = 0;
            long tmpSelectedFeeAmount = 0;

            SelectedInvoices = SelectedInvoices.Where(x => x.Checked).ToList();
            foreach (var item in SelectedInvoices)
            {
                CommonUtil.NumberTryParse(item.ThisBillingAmount, out tmpSelectedBillingAmount);
                selectedBillingAmount += tmpSelectedBillingAmount;
                CommonUtil.NumberTryParse(item.ThisTaxAmount, out tmpSelectedTaxAmount);
                selectedTaxAmount += tmpSelectedTaxAmount;
                CommonUtil.NumberTryParse(item.ThisFeeAmount, out tmpSelectedFeeAmount);
                selectedFeeAmount += tmpSelectedFeeAmount;
            }
            SelectedBillingAmount = $"￥{selectedBillingAmount.ToString("#,##0")}";
            SelectedTaxAmount = $"￥{selectedTaxAmount.ToString("#,##0")}";
            SelectedFeeAmount = $"￥{selectedFeeAmount.ToString("#,##0")}";
            searchModel.SeiOutSeqSeiRen = string.Join(",", SelectedInvoices.Select(r => r.ListInvoiceNo)).Replace("-", "");
        }

        /// <summary>
        /// press check row
        /// </summary>
        /// <param name="invoice">invoice select</param>
        /// <param name="newValue">check or uncheck</param>
        /// <param name="isCheckAll">is check all</param>
        protected void CheckedValueGridChanged(Invoice invoice, bool newValue, bool isCheckAll)
        {
            //check item selecting is added to selected invoice
            var data = SelectedInvoices?.FirstOrDefault(x => x.SeiOutSeq == invoice.SeiOutSeq && x.SeiRen == invoice.SeiRen);
            //if not added to list then added
            if (data == null)
                SelectedInvoices.Add(invoice);
            else
            {
                //if check then uncheck and otherwise
                if (invoice.Checked && data.Checked && isCheckAll && newValue)
                {
                    data.Checked = (data != null) && newValue;
                    return;
                }
                data.Checked = (data != null) && newValue;
            }
            invoice.Checked = newValue;
            ItemCheckAll = DataSourcePaging.All(x => SelectedInvoices.Any(y => x.SeiOutSeq == x.SeiOutSeq && y.SeiRen == x.SeiRen && x.Checked));

            if (!isCheckAll)
            {
                CaculateData();
                StateHasChanged();
            }
        }

        protected async Task OnChangePage(int page)
        {
            await ToggleLoading(true);
            CurrentPage = page;
            await OnSearch(false);
            if (DataSource.Any())
            {
                foreach (var item in DataSourcePaging)
                {
                    var data = SelectedInvoices.Where(x => x.SeiOutSeq == item.SeiOutSeq && x.SeiRen == item.SeiRen).FirstOrDefault();
                    if (data != null)
                    {
                        item.Checked = data.Checked;
                    }
                }
                ItemCheckAll = DataSourcePaging.All(x => SelectedInvoices.Any(y => (y.SeiOutSeq == x.SeiOutSeq) && (y.SeiRen == x.SeiRen) && x.Checked));
            }
            await ToggleLoading(false);
        }

        /// <summary>
        /// when change reload grid
        /// </summary>
        /// <param name="value">current selected</param>
        /// <returns></returns>
        protected async Task OnChangeBillAddressReceipt(BillAddressReceipt value)
        {
            await ToggleLoading(true);
            CheckedItemAllChanged(false);
            SelectedBillAddressReceipt = value;
            searchModel.BillAddressReceipt = value;
            await OnSearch(false);
            await ToggleLoading(false);
        }

        /// <summary>
        /// search with condition
        /// </summary>
        /// <param name="isFirst">first load close form search</param>
        /// <returns></returns>
        public async Task OnSearch(bool isFirst)
        {
            DataSource = await _service.GetInvoiceListData(searchModel);

            if (!DataSource.Any())
            {
                IsDataNotFound = true;
                DataSource = new List<Invoice>();
                DataSourcePaging = new List<Invoice>();
                ItemCheckAll = false;
            }
            else if (DataSource.Any())
            {
                IsDataNotFound = false;
                DataSourcePaging = DataSource.Skip(CurrentPage * ItemPerPage).Take(ItemPerPage).ToList();
            }
            if (isFirst)
                IsDataNotFound = false;
        }

        private async Task<Image> GetReportImage()
        {
            try
            {
                var fileId = await _billPrintService.GetCompanyFileId();
                if (!string.IsNullOrWhiteSpace(fileId) && !string.IsNullOrEmpty(fileId))
                {
                    var file = await _s3Service.DownloadFileAsync(new DownloadModel()
                    {
                        FileId = fileId,
                        Password = string.Empty,
                        UpdSyainCd = new ClaimModel().SyainCdSeq,
                        UpdPrgID = CommonConst.UpdPrgID
                    });
                    if (file != null)
                    {
                        var byteArr = Convert.FromBase64String(file.Content);
                        using (var mem = new MemoryStream(byteArr))
                        {
                            return Image.FromStream(mem);
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        protected async Task OnModelChanged(string propertyName, dynamic val, bool isFrom)
        {
            var propertyFromInfo = searchModel.CustomerModelFrom.GetType().GetProperty(propertyName);
            var propertyToInfo = searchModel.CustomerModelTo.GetType().GetProperty(propertyName);

            if (propertyFromInfo != null && isFrom)
                propertyFromInfo.SetValue(searchModel.CustomerModelFrom, val);
            if (propertyToInfo != null && !isFrom)
                propertyToInfo.SetValue(searchModel.CustomerModelTo, val);

            if (IsFirstRender && !isFrom && propertyName == nameof(searchModel.CustomerModelFrom.SelectedTokiSt))
                await GetBillAddressReceipts();
            else if (IsFirstRender)
                return;

            if (propertyFromInfo != null && isFrom)
                propertyFromInfo.SetValue(searchModel.CustomerModelFrom, val);
            if (propertyToInfo != null && !isFrom)
                propertyToInfo.SetValue(searchModel.CustomerModelTo, val);

            if (searchModel.CustomerModelFrom?.SelectedGyosya?.GyosyaCd > searchModel.CustomerModelTo?.SelectedGyosya?.GyosyaCd)
                isGyosyaValid = false;
            else
                isGyosyaValid = true;

            if ((searchModel.CustomerModelFrom?.SelectedGyosya?.GyosyaCd == searchModel.CustomerModelTo?.SelectedGyosya?.GyosyaCd) && (searchModel.CustomerModelFrom?.SelectedTokisk?.TokuiCd > searchModel.CustomerModelTo?.SelectedTokisk?.TokuiCd))
                isTokiskValid = false;
            else
                isTokiskValid = true;

            if ((searchModel.CustomerModelFrom?.SelectedGyosya?.GyosyaCd == searchModel.CustomerModelTo?.SelectedGyosya?.GyosyaCd) && (searchModel.CustomerModelFrom?.SelectedTokisk?.TokuiCd == searchModel.CustomerModelTo?.SelectedTokisk?.TokuiCd)
            && (searchModel.CustomerModelFrom?.SelectedTokiSt?.SitenCd > searchModel.CustomerModelTo?.SelectedTokiSt?.SitenCd))
                isTokistValid = false;
            else
                isTokistValid = true;

            if (editFormContext.Validate() && isGyosyaValid && isTokiskValid && isTokistValid)
            {
                if (propertyName == nameof(searchModel.CustomerModelFrom.SelectedTokiSt))
                {
                    var keyValueFilterPairs = _generateFilterValueDictionaryService.GenerateForReceiptOutput(searchModel).Result;
                    await _filterConditionService.SaveFilterCondtion(keyValueFilterPairs, FormFilterName.ReceiptOutput, 0, new ClaimModel().SyainCdSeq);
                    await GetBillAddressReceipts();
                }
            }

            StateHasChanged();
        }
        protected async Task GetBillAddressReceipts()
        {
            var result = await _service.GetCommonListItems(searchModel);
            billAddressReceipt = result?.BillAddressReceipts;
            searchModel.BillAddressReceipt = billAddressReceipt.FirstOrDefault();
            SelectedBillAddressReceipt = billAddressReceipt.FirstOrDefault();
            await OnSearch(false);
        }
        #endregion Function
    }
}
