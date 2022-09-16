using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Commons;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.IService;
using HassyaAllrightCloud.Pages.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ReportTemplate = HassyaAllrightCloud.Reports.ReportTemplate.TransportActualResult.TransportActualResult;

namespace HassyaAllrightCloud.Pages
{
    public class TransportActualResultBase : ComponentBase, IDisposable
    {
        [Inject] protected IStringLocalizer<TransportActualResult> _lang { get; set; }
        [Inject] protected ITransportationSummaryService _transportationSummaryService { get; set; }
        [Inject] protected ITransportActualResultService _service { get; set; }
        [Inject] protected IJSRuntime _jSRuntime { get; set; }
        [Inject] private IReportLoadingService _reportLoading { get; set; }
        [Inject] private IFilterCondition _filterService { get; set; }
        [Inject] private IErrorHandlerService _errService { get; set; }
        protected EditContext searchForm { get; set; }
        protected TransportActualResultSearchModel searchModel { get; set; }
        protected bool isDataNotFound { get; set; }
        protected bool isEnable { get; set; }
        protected IEnumerable<CompanyListItem> companies { get; set; } = new List<CompanyListItem>();
        protected IEnumerable<EigyoListItem> eigyoList { get; set; } = new List<EigyoListItem>();
        protected IEnumerable<CodeKbDataItem> shippingList { get; set; } = new List<CodeKbDataItem>();
        protected CancellationTokenSource source = new CancellationTokenSource();
        protected ReportLoading reportLoading;
        protected Dictionary<string, string> LangDic = new Dictionary<string, string>();
        protected override async Task OnInitializedAsync()
        {
            try
            {
                LangDic = _lang.GetAllStrings().ToDictionary(l => l.Name, l => l.Value);
                companies = await _transportationSummaryService.GetCompanyListItems(new ClaimModel().CompanyID);
                eigyoList = await _transportationSummaryService.GetEigyoListItems(new ClaimModel().CompanyID, new ClaimModel().TenantID);
                shippingList = await _service.GetCodeKb(new ClaimModel().TenantID);

                searchModel = new TransportActualResultSearchModel();
                searchModel.OutputType = OutputType.Preview;

                searchModel.ProcessingYear = DateTime.Now;
                searchModel.Company = companies.FirstOrDefault();

                searchModel = await BuildSearchModel(searchModel);
                searchForm = new EditContext(searchModel);

                isEnable = searchForm.Validate();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                await _jSRuntime.InvokeVoidAsync("EnterTab", ".enterField");
                if (firstRender)
                    await _jSRuntime.InvokeVoidAsync("focus", "#input-focus-onload");
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected async Task ProcessingYearChanged(DateTime? val)
        {
            try
            {
                searchModel.ProcessingYear = val;
                isEnable = searchForm.Validate();
                if (isEnable)
                    await SaveSearchModel(searchModel);
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected async Task OnResetSearchData()
        {
            try
            {
                searchModel.ProcessingYear = DateTime.Now;
                searchModel.Company = companies.FirstOrDefault();
                searchModel.EigyoFrom = null;
                searchModel.EigyoTo = null;
                searchModel.ShippingFrom = null;
                searchModel.ShippingTo = null;
                searchModel.OutputType = OutputType.Preview;

                await _filterService.DeleteCustomFilerCondition(new ClaimModel().SyainCdSeq, 0, FormFilterName.TransportActualResult);

                StateHasChanged();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        private void PreViewReport(ReportSearchModel searchModel)
        {
            try
            {
                var uri = $"TransportActualResultPreview?Params={EncryptHelper.EncryptToUrl(searchModel)}";
                _jSRuntime.InvokeVoidAsync("open", uri, "_blank");
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected async Task OnSubmit()
        {
            var reportId = Guid.Empty;
            try
            {
                isEnable = searchForm.Validate();
                if (isEnable)
                {
                    var reportModel = FormSearchModel2ReportSearchModel();
                    reportId = Guid.NewGuid();
                    reportLoading.InitReportLoading(reportId);
                    _reportLoading.Start(reportId);
                    var reportDataSource = await _service.GetReportData(reportModel, source.Token);
                    _reportLoading.Stop(reportId);
                    if (reportDataSource == null || !reportDataSource.Any())
                    {
                        isDataNotFound = true;
                        StateHasChanged();
                    }
                    else
                    {
                        isDataNotFound = false;
                        StateHasChanged();

                        await SaveSearchModel(searchModel);

                        if (searchModel.OutputType == OutputType.Preview)
                        {
                            PreViewReport(reportModel);
                        }
                        else
                        {
                            switch (searchModel.OutputType)
                            {
                                case OutputType.ExportPdf:
                                case OutputType.ExportExcel:
                                    await DownloadReport(reportDataSource);
                                    break;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
                if(reportId != Guid.Empty) _reportLoading.Stop(reportId);
            }
        }

        private ReportSearchModel FormSearchModel2ReportSearchModel()
        {
            return new ReportSearchModel()
            {
                Company = searchModel.Company.CompanyCd,
                CurrentTenantId = new ClaimModel().TenantID,
                EigyoFrom = searchModel.EigyoFrom?.EigyoCd,
                EigyoTo = searchModel.EigyoTo?.EigyoCd,
                ProcessingYear = searchModel.ProcessingYear.Value.Year,
                ShippingFrom = searchModel.ShippingFrom?.CodeKbn,
                ShippingTo = searchModel.ShippingTo?.CodeKbn
            };
        }

        private async Task DownloadReport(List<TransportActualResultReportData> dataSource)
        {
            XtraReport report = new ReportTemplate();

            report.DataSource = dataSource;

            if (searchModel.OutputType == OutputType.ExportExcel)
            {
                report.ExportOptions.Xlsx.ExportMode = XlsxExportMode.SingleFilePageByPage;
                report.BeforePrint += (sender, e) =>
                {
                    (sender as ReportTemplate).PrintingSystem.XlSheetCreated += PrintingSystem_XlsxDocumentCreated;
                };
                void PrintingSystem_XlsxDocumentCreated(object sender, XlSheetCreatedEventArgs e)
                {
                    e.SheetName = dataSource[e.Index].UnsouKbnNm;
                }
            }

            using (MemoryStream ms = new MemoryStream())
            {
                report.CreateDocument();
                if (searchModel.OutputType == OutputType.ExportExcel) report.ExportToXlsx(ms);
                else report.ExportToPdf(ms);
                byte[] exportedFileBytes = ms.ToArray();
                string myExportString = Convert.ToBase64String(exportedFileBytes);
                await _jSRuntime.InvokeVoidAsync("downloadFileClientSide", myExportString, searchModel.OutputType == OutputType.ExportExcel ? "xlsx" : "pdf", "TransportActualResultReport");
            }
        }

        protected async Task EigyoChanged(EigyoListItem val, bool isFrom)
        {
            try
            {
                if (isFrom)
                    searchModel.EigyoFrom = val;
                else
                    searchModel.EigyoTo = val;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected async Task CompanyChanged(CompanyListItem val)
        {
            try
            {
                searchModel.Company = val;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected async Task ShippingChanged(CodeKbDataItem val, bool isFrom)
        {
            try
            {
                if (isFrom)
                    searchModel.ShippingFrom = val;
                else
                    searchModel.ShippingTo = val;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected async Task OutputTypeChanged(OutputType type)
        {
            searchModel.OutputType = type;
        }

        private async Task SaveSearchModel(TransportActualResultSearchModel model)
        {
            var filers = GetSearchFormModel(model);
            await _filterService.SaveFilterCondtion(filers, FormFilterName.TransportActualResult, 0, new ClaimModel().SyainCdSeq);
        }

        private Dictionary<string, string> GetSearchFormModel(TransportActualResultSearchModel model)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            if (model == null) return result;

            result.Add(nameof(TransportActualResultSearchModel.OutputType), ((int)model.OutputType).ToString());
            result.Add(nameof(TransportActualResultSearchModel.ProcessingYear), model.ProcessingYear.Value.ToString(DateTimeFormat.yyyyMMdd));
            result.Add(nameof(TransportActualResultSearchModel.Company), model.Company?.CompanyCdSeq.ToString() ?? string.Empty);
            result.Add(nameof(TransportActualResultSearchModel.EigyoFrom), model.EigyoFrom?.EigyoCdSeq.ToString() ?? string.Empty);
            result.Add(nameof(TransportActualResultSearchModel.EigyoTo), model.EigyoTo?.EigyoCdSeq.ToString() ?? string.Empty);
            result.Add(nameof(TransportActualResultSearchModel.ShippingFrom), model.ShippingFrom?.CodeKbn ?? string.Empty);
            result.Add(nameof(TransportActualResultSearchModel.ShippingTo), model.ShippingTo?.CodeKbn ?? string.Empty);

            return result;
        }

        private async Task<TransportActualResultSearchModel> BuildSearchModel(TransportActualResultSearchModel model)
        {
            var filters = await _filterService.GetFilterCondition(FormFilterName.TransportActualResult, 0, new ClaimModel().SyainCdSeq);

            foreach (var item in filters)
            {
                var propertyInfo = model.GetType().GetProperty(item.ItemNm);
                if (propertyInfo != null && !string.IsNullOrEmpty(item.JoInput))
                {
                    switch (item.ItemNm)
                    {
                        case nameof(TransportActualResultSearchModel.OutputType):
                            var type = (OutputType)(int.Parse(item.JoInput));
                            propertyInfo.SetValue(model, type);
                            break;
                        case nameof(TransportActualResultSearchModel.ProcessingYear):
                            var selectedDate = DateTime.ParseExact(item.JoInput, DateTimeFormat.yyyyMMdd, CultureInfo.InvariantCulture);
                            propertyInfo.SetValue(model, selectedDate);
                            break;
                        case nameof(TransportActualResultSearchModel.Company):
                            var selectedComCdSeq = int.Parse(item.JoInput);
                            var selectedCom = companies.FirstOrDefault(e => e.CompanyCdSeq == selectedComCdSeq);
                            propertyInfo.SetValue(model, selectedCom);
                            break;
                        case nameof(TransportActualResultSearchModel.EigyoFrom):
                        case nameof(TransportActualResultSearchModel.EigyoTo):
                            var selectedEigyoCdSeq = int.Parse(item.JoInput);
                            var selectedEigyo = eigyoList.FirstOrDefault(e => e.EigyoCdSeq == selectedEigyoCdSeq);
                            propertyInfo.SetValue(model, selectedEigyo);
                            break;
                        case nameof(TransportActualResultSearchModel.ShippingFrom):
                        case nameof(TransportActualResultSearchModel.ShippingTo):
                            var selectedShipping = shippingList.FirstOrDefault(e => e.CodeKbn == item.JoInput);
                            propertyInfo.SetValue(model, selectedShipping);
                            break;
                    }
                }
            }

            return model;
        }

        public void Dispose()
        {
            try
            {
                source.Cancel();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }
    }
}
