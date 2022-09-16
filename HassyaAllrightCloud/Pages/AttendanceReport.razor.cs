using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Commons;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
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
using HassyaAllrightCloud.IService.CommonComponents;

namespace HassyaAllrightCloud.Pages
{
    public class AttendanceReportBase : ComponentBase, IDisposable
    {
        [Inject] protected IStringLocalizer<AttendanceReport> _lang { get; set; }
        [Inject] protected ITransportationSummaryService _transportationSummaryService { get; set; }
        [Inject] protected IAttendanceReportService _service { get; set; }
        [Inject] protected IRevenueSummaryService _revenueSummaryService { get; set; }
        [Inject] protected IJSRuntime _jSRuntime { get; set; }
        [Inject] private IReportLoadingService _reportLoading { get; set; }
        [Inject] private IFilterCondition _filterService { get; set; }
        [Inject] private IErrorHandlerService _errService { get; set; }
        [Inject] private IReportLayoutSettingService _reportLayoutSettingService { get; set; }
        [Inject] protected IReservationClassComponentService _reservationService { get; set; }
        protected EditContext searchForm { get; set; }
        protected AttendanceReportSearchModel searchModel { get; set; }
        protected bool isDataNotFound { get; set; }
        protected List<CompanyData> companies { get; set; } = new List<CompanyData>();
        protected List<PageSizeItem> pageSizes { get; set; } = new List<PageSizeItem>();
        protected IEnumerable<EigyoListItem> eigyoList { get; set; } = new List<EigyoListItem>();
        protected CancellationTokenSource source = new CancellationTokenSource();
        protected ReportLoading reportLoading;
        protected Dictionary<string, string> LangDic = new Dictionary<string, string>();
        public List<ReservationClassComponentData> ListReservationClass { get; set; } = new List<ReservationClassComponentData>();
        protected override async Task OnInitializedAsync()
        {
            try
            {
                LangDic = _lang.GetAllStrings().ToDictionary(l => l.Name, l => l.Value);
                companies = await _service.GetCompanyListItems(new ClaimModel().TenantID);
                eigyoList = await _transportationSummaryService.GetEigyoListItems(new ClaimModel().CompanyID, new ClaimModel().TenantID);
                ListReservationClass = await _reservationService.GetListReservationClass();
                pageSizes.Add(new PageSizeItem()
                {
                    PageSize = PageSize.A4,
                    Name = _lang["a4"]
                });
                pageSizes.Add(new PageSizeItem()
                {
                    PageSize = PageSize.A3,
                    Name = _lang["a3"]
                });
                pageSizes.Add(new PageSizeItem()
                {
                    PageSize = PageSize.B4,
                    Name = _lang["b4"]
                });
                searchModel = new AttendanceReportSearchModel();
                searchModel.Company = companies.FirstOrDefault(e => e.CompanyCd == new ClaimModel().CompanyID) ?? companies.FirstOrDefault();
                searchModel.OutputType = OutputType.Preview;
                searchModel.PageSize = pageSizes.FirstOrDefault();

                searchModel.ProcessingDate = DateTime.Now;

                searchModel = await BuildSearchModel(searchModel);
                searchForm = new EditContext(searchModel);
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

        protected void ProcessingDateChanged(DateTime val)
        {
            try
            {
                searchModel.ProcessingDate = val;
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected void OnResetSearchData()
        {
            try
            {
                searchModel.ProcessingDate = DateTime.Now;
                searchModel.Company = companies.FirstOrDefault();
                searchModel.EigyoFrom = null;
                searchModel.EigyoTo = null;
                searchModel.RegistrationTypeFrom = null;
                searchModel.RegistrationTypeTo = null;
                searchModel.OutputType = OutputType.Preview;
                searchModel.PageSize = pageSizes.FirstOrDefault();
                searchForm.Validate();
                _filterService.DeleteCustomFilerCondition(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, 0, FormFilterName.AttendanceReport);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        private void PreViewReport(AttendanceReportModel searchModel)
        {
            var uri = $"AttendanceReportPreview?Params={EncryptHelper.EncryptToUrl(searchModel)}";
            _jSRuntime.InvokeVoidAsync("open", uri, "_blank");
        }

        protected async Task OnSubmit()
        {
            var reportId = Guid.Empty;
            try
            {
                if (searchForm.Validate())
                {
                    var reportModel = FormSearchModel2ReportSearchModel();

                    reportId = Guid.NewGuid();
                    reportLoading.InitReportLoading(reportId);
                    _reportLoading.Start(reportId);
                    var reportDataSource = await _service.GetReportData(reportModel, source.Token);
                    _reportLoading.Stop(reportId);

                    isDataNotFound = reportDataSource == null || !reportDataSource.Any();
                    StateHasChanged();

                    if (!isDataNotFound)
                    {
                        await SaveSearchModel(searchModel);
                        if (searchModel.OutputType == OutputType.Preview)
                            PreViewReport(reportModel);
                        else
                            await DownloadReport(reportDataSource); // Export 2 pdf
                    }
                }
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
                if (reportId != Guid.Empty)
                    _reportLoading.Stop(reportId);
            }
        }

        private AttendanceReportModel FormSearchModel2ReportSearchModel() => new AttendanceReportModel()
        {
            CompanyCdSeq = searchModel.Company.CompanyCdSeq,
            CurrentTenantId = new ClaimModel().TenantID,
            EigyoFrom = searchModel.EigyoFrom?.EigyoCd ?? 0,
            EigyoTo = searchModel.EigyoTo?.EigyoCd ?? 0,
            ProcessingDate = searchModel.ProcessingDate.ToString(DateTimeFormat.yyyyMMdd),
            RegistrationTypeSortFrom = searchModel.RegistrationTypeFrom?.YoyaKbn ?? 0,
            RegistrationTypeSortTo = searchModel.RegistrationTypeTo?.YoyaKbn ?? 0,
            PageSize = searchModel.PageSize.PageSize
        };

        private async Task DownloadReport(List<AttendanceReportPage> dataSource)
        {
            var report = await _reportLayoutSettingService.GetCurrentTemplate(ReportIdForSetting.Attendancereport, BaseNamespace.Attendancereport, 
                new ClaimModel().TenantID, new ClaimModel().EigyoCdSeq, (byte)searchModel.PageSize.PageSize);
            report.DataSource = dataSource;
            using (MemoryStream ms = new MemoryStream())
            {
                report.CreateDocument();
                report.ExportToPdf(ms);
                byte[] exportedFileBytes = ms.ToArray();
                string myExportString = Convert.ToBase64String(exportedFileBytes);
                await _jSRuntime.InvokeVoidAsync("downloadFileClientSide", myExportString, "pdf", "AttendanceReport");
            }
        }

        protected void EigyoChanged(EigyoListItem val, bool isFrom)
        {
            try
            {
                if (isFrom)
                    searchModel.EigyoFrom = val;
                else
                    searchModel.EigyoTo = val;
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected void CompanyChanged(CompanyData val)
        {
            try
            {
                searchModel.Company = val;
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected void RegistrationTypeChanged(ReservationClassComponentData val, bool isFrom)
        {
            try
            {
                if (isFrom)
                    searchModel.RegistrationTypeFrom = val;
                else
                    searchModel.RegistrationTypeTo = val;
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected void OutputTypeChanged(OutputType type)
        {
            try
            {
                searchModel.OutputType = type;
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected void PageSizeChanged(PageSizeItem item)
        {
            try
            {
                searchModel.PageSize = item;
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        private async Task SaveSearchModel(AttendanceReportSearchModel model)
        {
            var filers = GetSearchFormModel(model);
            await _filterService.SaveFilterCondtion(filers, FormFilterName.AttendanceReport, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq);
        }

        private Dictionary<string, string> GetSearchFormModel(AttendanceReportSearchModel model)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            if (model == null) return result;

            result.Add(nameof(AttendanceReportSearchModel.OutputType), ((int)model.OutputType).ToString());
            result.Add(nameof(AttendanceReportSearchModel.PageSize), ((int?)model.PageSize?.PageSize)?.ToString() ?? string.Empty);
            result.Add(nameof(AttendanceReportSearchModel.Company), model.Company?.CompanyCdSeq.ToString() ?? string.Empty);
            result.Add(nameof(AttendanceReportSearchModel.ProcessingDate), model.ProcessingDate.ToString(DateTimeFormat.yyyyMMdd));
            result.Add(nameof(AttendanceReportSearchModel.EigyoFrom), model.EigyoFrom?.EigyoCdSeq.ToString() ?? string.Empty);
            result.Add(nameof(AttendanceReportSearchModel.EigyoTo), model.EigyoTo?.EigyoCdSeq.ToString() ?? string.Empty);
            result.Add(nameof(AttendanceReportSearchModel.RegistrationTypeFrom), model.RegistrationTypeFrom?.YoyaKbnSeq.ToString() ?? string.Empty);
            result.Add(nameof(AttendanceReportSearchModel.RegistrationTypeTo), model.RegistrationTypeTo?.YoyaKbnSeq.ToString() ?? string.Empty);

            return result;
        }

        private async Task<AttendanceReportSearchModel> BuildSearchModel(AttendanceReportSearchModel model)
        {
            var filters = await _filterService.GetFilterCondition(FormFilterName.AttendanceReport, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq);

            foreach (var item in filters)
            {
                var propertyInfo = model.GetType().GetProperty(item.ItemNm);
                if (propertyInfo != null && !string.IsNullOrEmpty(item.JoInput))
                {
                    switch (item.ItemNm)
                    {
                        case nameof(AttendanceReportSearchModel.OutputType):
                            var type = (OutputType)(int.Parse(item.JoInput));
                            propertyInfo.SetValue(model, type);
                            break;
                        case nameof(AttendanceReportSearchModel.PageSize):
                            var page = (PageSize)(int.Parse(item.JoInput));
                            var selectedPage = pageSizes.FirstOrDefault(e => e.PageSize == page);
                            propertyInfo.SetValue(model, selectedPage);
                            break;
                        case nameof(AttendanceReportSearchModel.Company):
                            var selectedComCdSeq = int.Parse(item.JoInput);
                            var selectedCom = companies.FirstOrDefault(e => e.CompanyCdSeq == selectedComCdSeq);
                            propertyInfo.SetValue(model, selectedCom);
                            break;
                        case nameof(AttendanceReportSearchModel.ProcessingDate):
                            var selectedDate = DateTime.ParseExact(item.JoInput, DateTimeFormat.yyyyMMdd, CultureInfo.InvariantCulture);
                            propertyInfo.SetValue(model, selectedDate);
                            break;
                        case nameof(AttendanceReportSearchModel.EigyoFrom):
                        case nameof(AttendanceReportSearchModel.EigyoTo):
                            var selectedEigyoCdSeq = int.Parse(item.JoInput);
                            var selectedEigyo = eigyoList.FirstOrDefault(e => e.EigyoCdSeq == selectedEigyoCdSeq);
                            propertyInfo.SetValue(model, selectedEigyo);
                            break;
                        case nameof(AttendanceReportSearchModel.RegistrationTypeFrom):
                        case nameof(AttendanceReportSearchModel.RegistrationTypeTo):
                            var selectedTypeCdSeq = int.Parse(item.JoInput);
                            var selectedType = ListReservationClass.FirstOrDefault(e => e.YoyaKbnSeq == selectedTypeCdSeq);
                            propertyInfo.SetValue(model, selectedType);
                            break;
                    }
                }
            }

            return model;
        }

        public void Dispose()
        {
            source.Cancel();
        }
    }
}
