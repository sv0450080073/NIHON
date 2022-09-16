using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Commons;
using HassyaAllrightCloud.Pages.Components.RevenueSummary;
using HassyaAllrightCloud.Commons.Helpers;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using HassyaAllrightCloud.Commons.Extensions;
using System.Globalization;
using HassyaAllrightCloud.Pages.Components.CommonComponents;
using HassyaAllrightCloud.IService.CommonComponents;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;

namespace HassyaAllrightCloud.Pages
{
    public class RevenueSummaryBase : ComponentBase
    {
        [CascadingParameter] protected ClaimModel ClaimModel { get; set; }
        [Inject] protected IStringLocalizer<RevenueSummary> _lang { get; set; }
        [Inject] private IRevenueSummaryService _service { get; set; }
        [Inject] protected NavigationManager _navigationManager { get; set; }
        [Inject] protected IJSRuntime _jSRuntime { get; set; }
        [Inject] protected ITransportationSummaryService _transportationSummaryService { get; set; }
        [Inject] protected IWebHostEnvironment _webHostEnvironment { get; set; }
        [Inject] protected ILoadingService _loading { get; set; }
        [Inject] protected IErrorHandlerService errorModalService { get; set; }
        [Inject] protected IFilterCondition _filterService { get; set; }
        [Inject] protected IReservationClassComponentService _reservationService { get; set; }
        protected FormSearchModel Model { get; set; } = new FormSearchModel();
        protected TransportationRevenueSearchModel RevenueSearchModel { get; set; }
        protected List<ComboboxBaseItem> showHeaderOptions { get; set; }
        protected List<ComboboxBaseItem> kukuriKbnItems { get; set; }
        protected List<ComboboxBaseItem> separatorOptions { get; set; }
        protected IEnumerable<CompanyListItem> companyListItems { get; set; } = new List<CompanyListItem>();
        protected IEnumerable<EigyoListItem> eigyoListItems { get; set; } = new List<EigyoListItem>();
        protected List<ComboboxBaseItem> tesuInKbnItems { get; set; }
        protected IEnumerable<ComboboxBaseItem> pageSizeListItems { get; set; }
        protected EditContext editFormContext { get; set; }
        protected bool showError { get; set; }
        protected int gridSizeClass { get; set; } = (int)ViewMode.Medium;
        protected bool isFirstLoad { get; set; }
        protected TransportationDailyReport TransportationDailyReport { get; set; }
        protected TransportationMonthlyReport TransportationMonthlyReport { get; set; }
        protected Dictionary<string, string> LangDic = new Dictionary<string, string>();
        protected Components.ReportLoading reportLoading;
        private bool dataExist { get; set; }
        public List<ReservationClassComponentData> ListReservationClass { get; set; } = new List<ReservationClassComponentData>();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                await _jSRuntime.InvokeVoidAsync("setEventforCodeNumberField");
                await _jSRuntime.InvokeVoidAsync("EnterTab", ".enterField");
                if (firstRender)
                    await _jSRuntime.InvokeVoidAsync("focus", "#input-focus-onload");
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                isFirstLoad = true;
                var dataLang = _lang.GetAllStrings();
                LangDic = dataLang.ToDictionary(l => l.Name, l => l.Value);
                await InitiateControls();
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected void TriggerTabChange()
        {
            try
            {
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected async Task ClearBtnOnClick()
        {
            try
            {
                Model.UkeNoFrom = string.Empty;
                Model.UkeNoTo = string.Empty;
                Model.YoyaKbnFrom = null;
                Model.YoyaKbnTo = null;
                Model.EigyoFrom = null;
                Model.EigyoTo = null;
                Model.Company = companyListItems.FirstOrDefault(e => e.CompanyCdSeq == new ClaimModel().CompanyID);
                Model.EigyoKbn = EigyoKbnEnum.BillingOffice;
                Model.DailyTable = true;
                Model.TotalTable = false;
                Model.OutputType = OutputType.Preview;
                gridSizeClass = (int)ViewMode.Medium;
                Model.PageSize = pageSizeListItems.FirstOrDefault();
                Model.OutputWithHeader = showHeaderOptions.FirstOrDefault();
                Model.KukuriKbn = kukuriKbnItems.FirstOrDefault();
                Model.KugiriCharType = separatorOptions.FirstOrDefault();
                Model.UriYmdFrom = DateTime.Now;
                Model.UriYmdTo = DateTime.Now;
                showError = false;

                dataExist = await ReloadReportData();
                if (dataExist)
                    await _filterService.DeleteCustomFilerCondition(new ClaimModel().SyainCdSeq, 0, FormFilterName.RevenueSummary);
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected async Task UpdateFormModel(string propertyName, dynamic value)
        {
            try
            {
                showError = false;
                var propertyInfo = Model.GetType().GetProperty(propertyName);
                if (propertyName == nameof(Model.UkeNoFrom) || propertyName == nameof(Model.UkeNoTo))
                {
                    string val = (string)value;
                    if (val.Length > 10) val = val.Substring(0, 10);
                    val = val.Normalize(NormalizationForm.FormKD);
                    long result;
                    if (string.IsNullOrEmpty(val) || long.TryParse(val, out result))
                        propertyInfo.SetValue(Model, val);
                    StateHasChanged();
                }
                else
                    propertyInfo.SetValue(Model, value);
                if (propertyName == nameof(Model.TotalTable) || propertyName == nameof(Model.DailyTable))
                {
                    if (propertyName == nameof(Model.TotalTable)) Model.DailyTable = false;
                    if (propertyName == nameof(Model.DailyTable)) Model.TotalTable = false;
                }

                if (editFormContext.Validate())
                {
                    if (propertyName != nameof(Model.OutputWithHeader)
                    && propertyName != nameof(Model.KukuriKbn)
                    && propertyName != nameof(Model.KugiriCharType)
                    && propertyName != nameof(Model.PageSize))
                    {
                        dataExist = await ReloadReportData();
                    }

                    if (dataExist)
                    {
                        await SaveSearchModel(Model);
                    }
                }

                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected async Task ChangeGridSize(ViewMode mode)
        {
            try
            {
                gridSizeClass = (int)mode;
                if (editFormContext.Validate() && dataExist)
                {
                    await SaveSearchModel(Model);
                }
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected async Task ChangeOuputType(OutputType type)
        {
            try
            {
                Model.OutputType = type;
                if (editFormContext.Validate() && dataExist)
                {
                    await SaveSearchModel(Model);
                }
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
                var model = GetRevenueSearchModel(Model);
                var isEmpty = await PreCheck(model);
                if (!isEmpty)
                {
                    if (Model.OutputType == OutputType.Preview)
                    {
                        PreViewReport(model, Model.DailyTable);
                    }
                    else
                    {
                        model.ReportId = Guid.NewGuid();
                        reportLoading.InitReportLoading(model.ReportId);
                        switch (Model.OutputType)
                        {
                            case OutputType.CSV:
                                await ExportToCSV(model, Model.DailyTable);
                                break;
                            case OutputType.ExportPdf:
                                if (Model.DailyTable)
                                    await DownloadDailyReport(model);
                                else
                                    await DownloadMonthlyReport(model);
                                break;
                        }
                    }
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        private async Task<FormSearchModel> BuildSearchModel(FormSearchModel model)
        {
            var filters = await _filterService.GetFilterCondition(FormFilterName.RevenueSummary, 0, new ClaimModel().SyainCdSeq);

            foreach (var item in filters)
            {
                if (item.ItemNm == nameof(gridSizeClass))
                {
                    gridSizeClass = int.Parse(item.JoInput);
                    continue;
                }
                var propertyInfo = model.GetType().GetProperty(item.ItemNm);
                if (propertyInfo != null && !string.IsNullOrEmpty(item.JoInput))
                {
                    switch (item.ItemNm)
                    {
                        case nameof(FormSearchModel.EigyoKbn):
                        case nameof(FormSearchModel.OutputType):
                            propertyInfo.SetValue(model, int.Parse(item.JoInput));
                            break;
                        case nameof(FormSearchModel.UkeNoFrom):
                            propertyInfo.SetValue(model, item.JoInput);
                            break;
                        case nameof(FormSearchModel.UkeNoTo):
                            propertyInfo.SetValue(model, item.JoInput);
                            break;
                        case nameof(FormSearchModel.UriYmdFrom):
                        case nameof(FormSearchModel.UriYmdTo):
                            propertyInfo.SetValue(model, DateTime.ParseExact(item.JoInput, DateTimeFormat.yyyyMMdd, CultureInfo.InvariantCulture));
                            break;
                        case nameof(FormSearchModel.YoyaKbnFrom):
                        case nameof(FormSearchModel.YoyaKbnTo):
                            var yoSeq = int.Parse(item.JoInput);
                            if (yoSeq != 0)
                            {
                                var selectedYo = ListReservationClass.FirstOrDefault(e => e.YoyaKbnSeq == yoSeq);
                                if (selectedYo != null)
                                    propertyInfo.SetValue(model, selectedYo);
                            }
                            break;
                        case nameof(FormSearchModel.Company):
                            var comSeq = int.Parse(item.JoInput);
                            if (comSeq != 0)
                            {
                                var selectedCom = companyListItems.FirstOrDefault(e => e.CompanyCdSeq == comSeq);
                                if (selectedCom != null)
                                    propertyInfo.SetValue(model, selectedCom);
                            }
                            break;
                        case nameof(FormSearchModel.TesuInKbn):
                            var tesuVal = int.Parse(item.JoInput);
                            if (tesuVal != 0)
                            {
                                var selectedTesu = tesuInKbnItems.FirstOrDefault(e => e.Value == tesuVal);
                                if (selectedTesu != null)
                                    propertyInfo.SetValue(model, selectedTesu);
                            }
                            break;
                        case nameof(FormSearchModel.EigyoFrom):
                        case nameof(FormSearchModel.EigyoTo):
                            var eigyoSeq = int.Parse(item.JoInput);
                            if (eigyoSeq != 0)
                            {
                                var selectedEigyo = eigyoListItems.FirstOrDefault(e => e.EigyoCdSeq == eigyoSeq);
                                if (selectedEigyo != null)
                                    propertyInfo.SetValue(model, selectedEigyo);
                            }
                            break;
                        case nameof(FormSearchModel.DailyTable):
                        case nameof(FormSearchModel.TotalTable):
                            propertyInfo.SetValue(model, bool.Parse(item.JoInput));
                            break;
                        case nameof(FormSearchModel.PageSize):
                            var pageVal = int.Parse(item.JoInput);
                            if (pageVal != 0)
                            {
                                var selectedPageSize = pageSizeListItems.FirstOrDefault(e => e.Value == pageVal);
                                if (selectedPageSize != null)
                                    propertyInfo.SetValue(model, selectedPageSize);
                            }
                            break;
                        case nameof(FormSearchModel.OutputWithHeader):
                            var outputVal = int.Parse(item.JoInput);
                            if (outputVal != 0)
                            {
                                var selectedOutput = showHeaderOptions.FirstOrDefault(e => e.Value == outputVal);
                                if (selectedOutput != null)
                                    propertyInfo.SetValue(model, selectedOutput);
                            }
                            break;
                        case nameof(FormSearchModel.KukuriKbn):
                            var kuVal = int.Parse(item.JoInput);
                            if (kuVal != 0)
                            {
                                var selectedKu = kukuriKbnItems.FirstOrDefault(e => e.Value == kuVal);
                                if (selectedKu != null)
                                    propertyInfo.SetValue(model, selectedKu);
                            }
                            break;
                        case nameof(FormSearchModel.KugiriCharType):
                            var kugiVal = int.Parse(item.JoInput);
                            if (kugiVal != 0)
                            {
                                var selectedKugi = separatorOptions.FirstOrDefault(e => e.Value == kugiVal);
                                if (selectedKugi != null)
                                    propertyInfo.SetValue(model, selectedKugi);
                            }
                            break;
                    }
                }
            }

            return model;
        }

        private async Task SaveSearchModel(FormSearchModel model)
        {
            var filers = GetSearchFormModel(model);
            await _filterService.SaveFilterCondtion(filers, FormFilterName.RevenueSummary, 0, new ClaimModel().SyainCdSeq);
        }

        private Dictionary<string, string> GetSearchFormModel(FormSearchModel model)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            if (model == null) return result;

            result.Add(nameof(FormSearchModel.EigyoKbn), ((int)model.EigyoKbn).ToString());

            result.Add(nameof(FormSearchModel.UkeNoFrom), string.IsNullOrEmpty(model.UkeNoFrom) ? string.Empty : model.UkeNoFrom);
            result.Add(nameof(FormSearchModel.UkeNoTo), string.IsNullOrEmpty(model.UkeNoTo) ? string.Empty : model.UkeNoTo);

            result.Add(nameof(FormSearchModel.UriYmdFrom), model.UriYmdFrom.ToString(DateTimeFormat.yyyyMMdd));
            result.Add(nameof(FormSearchModel.UriYmdTo), model.UriYmdTo.ToString(DateTimeFormat.yyyyMMdd));

            result.Add(nameof(FormSearchModel.YoyaKbnFrom), model.YoyaKbnFrom?.YoyaKbnSeq.ToString() ?? string.Empty);
            result.Add(nameof(FormSearchModel.YoyaKbnTo), model.YoyaKbnTo?.YoyaKbnSeq.ToString() ?? string.Empty);

            result.Add(nameof(FormSearchModel.Company), model.Company?.CompanyCdSeq.ToString() ?? string.Empty);

            result.Add(nameof(FormSearchModel.TesuInKbn), model.TesuInKbn?.Value.ToString() ?? string.Empty);

            result.Add(nameof(FormSearchModel.EigyoFrom), model.EigyoFrom?.EigyoCdSeq.ToString() ?? string.Empty);
            result.Add(nameof(FormSearchModel.EigyoTo), model.EigyoTo?.EigyoCdSeq.ToString() ?? string.Empty);

            result.Add(nameof(FormSearchModel.DailyTable), model.DailyTable.ToString());
            result.Add(nameof(FormSearchModel.TotalTable), model.TotalTable.ToString());

            result.Add(nameof(gridSizeClass), gridSizeClass.ToString());

            result.Add(nameof(FormSearchModel.OutputType), ((int)model.OutputType).ToString());

            result.Add(nameof(FormSearchModel.PageSize), Model.PageSize?.Value.ToString() ?? string.Empty);

            result.Add(nameof(FormSearchModel.OutputWithHeader), Model.OutputWithHeader?.Value.ToString() ?? string.Empty);
            result.Add(nameof(FormSearchModel.KukuriKbn), Model.KukuriKbn?.Value.ToString() ?? string.Empty);
            result.Add(nameof(FormSearchModel.KugiriCharType), Model.KugiriCharType?.Value.ToString() ?? string.Empty);

            return result;
        }

        private void PreViewReport(TransportationRevenueSearchModel searchModel, bool isDailyReport)
        {
            searchModel.IsDailyReport = isDailyReport;
            var uri = $"RevenueSummaryReportPreview?Params={EncryptHelper.EncryptToUrl(searchModel)}";
            _jSRuntime.InvokeVoidAsync("open", uri, "_blank");
        }

        private async Task DownloadDailyReport(TransportationRevenueSearchModel searchModel)
        {
            var reportDataSource = await _service.GetDailyRevenueReportData(searchModel);
            var report = _service.GetRevenueReportInstanceByPageSize(searchModel.PageSize, true);
            report.DataSource = reportDataSource;

            report.CreateDocument();
            using (MemoryStream ms = new MemoryStream())
            {
                report.CreateDocument();
                report.ExportToPdf(ms);
                byte[] exportedFileBytes = ms.ToArray();
                string myExportString = Convert.ToBase64String(exportedFileBytes);
                await _jSRuntime.InvokeVoidAsync("downloadFileClientSide", myExportString, "pdf", "TrasportationRevenueReport");
            }
        }

        private async Task DownloadMonthlyReport(TransportationRevenueSearchModel searchModel)
        {
            var reportDataSource = await _service.GetMonthlyRevenueReportData(searchModel);
            var report = _service.GetRevenueReportInstanceByPageSize(searchModel.PageSize, false);
            report.DataSource = reportDataSource;

            await new TaskFactory().StartNew(() =>
            {
                report.CreateDocument();
                using (MemoryStream ms = new MemoryStream())
                {
                    report.CreateDocument();
                    report.ExportToPdf(ms);
                    byte[] exportedFileBytes = ms.ToArray();
                    string myExportString = Convert.ToBase64String(exportedFileBytes);
                    _jSRuntime.InvokeVoidAsync("downloadFileClientSide", myExportString, "pdf", "TrasportationRevenueReport");
                }
            });
        }

        private async Task ExportToCSV(TransportationRevenueSearchModel searchModel, bool isDailyReport)
        {
            StringBuilder reportDataSource;
            if (isDailyReport)
                reportDataSource = await _service.GetDailyRevenueCSVReportData(searchModel);
            else
                reportDataSource = await _service.GetMonthlyRevenueCSVReportData(searchModel);
            var filePath = $"{_webHostEnvironment.WebRootPath}/csv/{Guid.NewGuid()}.csv";
            using (StreamWriter file = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                file.Write(reportDataSource.ToString());
            }
            byte[] exportedFileBytes = File.ReadAllBytes(filePath);
            File.Delete(filePath);
            string myExportString = Convert.ToBase64String(exportedFileBytes);
            await _jSRuntime.InvokeVoidAsync("downloadFileClientSide", myExportString, "csv", isDailyReport ? "TrasportationRevenueReport" : "MonthlyRevenueReport");
        }

        private async Task<bool> ReloadReportData()
        {
            RevenueSearchModel = GetRevenueSearchModel(Model);
            if (Model.DailyTable)
                return await TransportationDailyReport.Reload(RevenueSearchModel);
            else
                return await TransportationMonthlyReport.Reload(RevenueSearchModel);
        }

        protected void DataChanged(bool hasData)
        {
            try
            {
                if (!isFirstLoad)
                {
                    showError = !hasData;
                    dataExist = hasData;
                    StateHasChanged();
                }
                else
                    isFirstLoad = false;
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        private TransportationRevenueSearchModel GetRevenueSearchModel(FormSearchModel model)
        {
            var currentTenantId = new ClaimModel().TenantID;
            return new TransportationRevenueSearchModel()
            {
                UriYmdFrom = model.UriYmdFrom.ToString(DateTimeFormat.yyyyMMdd),
                UriYmdTo = model.UriYmdTo.ToString(DateTimeFormat.yyyyMMdd),
                TesuInKbn = (TesuInKbnEnum)model.TesuInKbn.Value,
                YoyaKbnFrom = model.YoyaKbnFrom?.YoyaKbn ?? 0,
                YoyaKbnTo = model.YoyaKbnTo?.YoyaKbn ?? 0,
                Company = model.Company?.CompanyCd ?? 0,
                EigyoFrom = model.EigyoFrom?.EigyoCd ?? 0,
                EigyoTo = model.EigyoTo?.EigyoCd ?? 0,
                EigyoNmFrom = model.EigyoFrom?.RyakuNm ?? string.Empty,
                EigyoNmTo = model.EigyoTo?.RyakuNm ?? string.Empty,
                UkeNoFrom = long.TryParse(model.UkeNoFrom, out long ukeNoFrom) ? $"{currentTenantId:00000}{ukeNoFrom:0000000000}" : string.Empty,
                UkeNoTo = long.TryParse(model.UkeNoTo, out long ukeNoTo) ? $"{currentTenantId:00000}{ukeNoTo:0000000000}" : string.Empty,
                OutputWithHeader = (ShowHeaderEnum)model.OutputWithHeader.Value,
                KukuriKbn = (WrapContentEnum)model.KukuriKbn.Value,
                KugiriCharType = (SeperatorEnum)model.KugiriCharType.Value,
                EigyoKbn = model.EigyoKbn,
                TenantCdSeq = currentTenantId,
                PageSize = (PageSize)model.PageSize.Value,
                IsDailyReport = model.DailyTable
            };
        }

        private async Task<bool> PreCheck(TransportationRevenueSearchModel model)
        {
            await _loading.ShowAsync();
            if (Model.DailyTable)
            {
                var items = await _service.GetEigyoListForDailyRevenueReport(model);
                showError = !items.Any();
            }
            else
            {
                var items = await _service.GetEigyoListForMonthlyRevenueReport(model);
                showError = !items.Any();
            }
            await _loading.HideAsync();
            return showError;
        }

        private async Task InitiateControls()
        {
            ListReservationClass = await _reservationService.GetListReservationClass();
            showHeaderOptions = _service.GetShowHeaderOptions();
            Model.OutputWithHeader = showHeaderOptions.First();
            kukuriKbnItems = _service.GetKukuriKbnItems();
            Model.KukuriKbn = kukuriKbnItems.First();
            separatorOptions = _service.GetSeparatorOptions();
            Model.KugiriCharType = separatorOptions.Last();
            Model.UriYmdFrom = DateTime.Now;
            Model.UriYmdTo = DateTime.Now;
            tesuInKbnItems = _service.GetTesuInKbnItems();
            Model.TesuInKbn = tesuInKbnItems.First();
            pageSizeListItems = _service.GetPageSizes();
            Model.PageSize = pageSizeListItems.FirstOrDefault();
            companyListItems = await _transportationSummaryService.GetCompanyListItems(0);
            Model.Company = companyListItems.FirstOrDefault(e => e.CompanyCdSeq == new ClaimModel().CompanyID);
            eigyoListItems = await _transportationSummaryService.GetEigyoListItems(new ClaimModel().CompanyID, new ClaimModel().TenantID);
            Model.EigyoKbn = EigyoKbnEnum.BillingOffice;
            Model.OutputType = OutputType.Preview;
            Model.DailyTable = true;
            // Get filters from DB
            Model = await BuildSearchModel(Model);
            editFormContext = new EditContext(Model);
            RevenueSearchModel = GetRevenueSearchModel(Model);
        }
    }
}
