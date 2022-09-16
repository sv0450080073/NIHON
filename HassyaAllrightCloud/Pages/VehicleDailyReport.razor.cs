using DevExpress.Blazor;
using DevExpress.CodeParser;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using HassyaAllrightCloud.IService;
using HassyaAllrightCloud.Pages.Components.CommonComponents;
using HassyaAllrightCloud.Reports.DataSource;
using HassyaAllrightCloud.Reports.ReportFactory;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Pages
{
    public class VehicleDailyReportBase : ComponentBase
    {
        [Inject]
        protected IStringLocalizer<VehicleDailyReport> _lang { get; set; }
        [Inject]
        protected IJSRuntime jsRuntime { get; set; }
        [Inject]
        protected IVehicleDailyReportService vehicleDailyReportService { get; set; }
        [Inject]
        protected NavigationManager navigationManager { get; set; }
        [Inject]
        protected IWebHostEnvironment hostingEnvironment { get; set; }
        [Inject]
        protected IReportLayoutSettingService _reportLayoutSettingService { get; set; }
        [Inject]
        protected IErrorHandlerService errorModalService { get; set; }
        [Inject]
        IFilterCondition FilterConditionService { get; set; }
        [Inject]
        IGetFilterDataService GetFilterDataService { get; set; }
        [Inject]
        IGenerateFilterValueDictionary GenerateFilterValueDictionaryService { get; set; }

        public Dictionary<string, string> LangDic = new Dictionary<string, string>();

        public EditContext searchForm { get; set; }
        public VehicleDailyReportSearchParam searchParams { get; set; } = new VehicleDailyReportSearchParam();
        public List<VehicleSearchDropdown> listOutputSetting { get; set; } = new List<VehicleSearchDropdown>();
        public List<VehicleSearchDropdown> listOutputWithHeader { get; set; } = new List<VehicleSearchDropdown>();
        public List<VehicleSearchDropdown> listKukuriKbn { get; set; } = new List<VehicleSearchDropdown>();
        public List<VehicleSearchDropdown> listKugiriCharType { get; set; } = new List<VehicleSearchDropdown>();
        public List<VehicleSearchDropdown> listOutputKbn { get; set; } = new List<VehicleSearchDropdown>();
        public List<BusSaleBranchSearch> listBusSaleBranch { get; set; } = new List<BusSaleBranchSearch>();
        public List<BusCodeSearch> listBusCode { get; set; } = new List<BusCodeSearch>();
        //public List<ReservationSearch> listReservation { get; set; } = new List<ReservationSearch>();
        public List<string> listCompany { get; set; } = new List<string>();
        public bool isDisableCsv { get; set; } = true;
        public bool isDataNotFound { get; set; } = false;
        public bool isDisableButton { get; set; } = false;
        public byte fontSize { get; set; } = (byte)ViewMode.Medium;
        public VehicleDailyReportList list = new VehicleDailyReportList();
        public bool isLoading { get; set; }
        Dictionary<string, string> keyValueFilterPairs = new Dictionary<string, string>();
        public bool isInitFinished { get; set; } = false;

        public ReservationClassComponentBase component { get; set; } = new ReservationClassComponentBase();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                searchForm = new EditContext(searchParams);
                var dataLang = _lang.GetAllStrings();
                LangDic = dataLang.ToDictionary(l => l.Name, l => l.Value);

                await OnInitDataAsync();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
				if (firstRender)
            	{
                	jsRuntime.InvokeVoidAsync("focusFirstItem");
            	}
                jsRuntime.InvokeVoidAsync("setEventforCodeNumberFieldV2", ".code-number", true, 10, true);
                jsRuntime.InvokeVoidAsync("EnterTab");
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            return base.OnAfterRenderAsync(firstRender);
        }

        protected void OnTabChanged()
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

        private async Task OnInitDataAsync()
        {
            try
            {
                searchParams.OutputSetting = 1;

                listOutputWithHeader.AddRange(new List<VehicleSearchDropdown>() {
                new VehicleSearchDropdown(){ Value = 0, Text = _lang["output_withheader"] },
                new VehicleSearchDropdown(){ Value = 1, Text = _lang["output_notwithheader"] }
            });
                searchParams.OutputWithHeader = listOutputWithHeader[0];

                listKukuriKbn.AddRange(new List<VehicleSearchDropdown>() {
                new VehicleSearchDropdown(){ Value = 0, Text = _lang["output_enclose"] },
                new VehicleSearchDropdown(){ Value = 1, Text = _lang["output_notenclose"] }
            });
                searchParams.KukuriKbn = listKukuriKbn[0];

                listKugiriCharType.AddRange(new List<VehicleSearchDropdown>() {
                new VehicleSearchDropdown(){ Value = 0, Text = _lang["output_withtab"] },
                new VehicleSearchDropdown(){ Value = 1, Text = _lang["output_withsemicolon"] },
                new VehicleSearchDropdown(){ Value = 2, Text = _lang["output_withcomma"] }
            });
                searchParams.KugiriCharType = listKugiriCharType[0];

                listOutputKbn.AddRange(new List<VehicleSearchDropdown>()
            {
                new VehicleSearchDropdown(){ Value = 0, Text = _lang["output_bycode"] },
                new VehicleSearchDropdown(){ Value = 1, Text = _lang["output_bydate"] },
            });
                searchParams.OutputKbn = listOutputKbn[0];

                searchParams.PaperSize = new VehicleSearchDropdown()
                {
                    Text = _lang["A4"],
                    Value = (byte)PaperSize.A4
                };

                //listReservation = await vehicleDailyReportService.GetListReservation();
                //listReservation = listReservation.OrderBy(_ => _.PriorityNum).ToList();
                //listReservation.Insert(0, new ReservationSearch() { YoyaKbnNm = Constants.SelectedAll, IsSelectedAll = true });
                searchParams.Company = await vehicleDailyReportService.GetCompanyName(searchParams.TenantCdSeq, searchParams.CompanyCdSeq);
                listCompany.Add(searchParams.Company);
                listBusSaleBranch = await vehicleDailyReportService.GetListBusSaleBranch(searchParams.TenantCdSeq);
                listBusCode = await vehicleDailyReportService.GetListBusCode();
                searchParams = BuildSearchModel(searchParams).Result;
                //await list.OnSearch(false, true);
                isInitFinished = true;
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        private async Task<VehicleDailyReportSearchParam> BuildSearchModel(VehicleDailyReportSearchParam model)
        {
            var filters = await FilterConditionService.GetFilterCondition(FormFilterName.VehicleDailyReport, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq);
            if (filters.Count() == 0)
            {
                return model;
            }
            else
            {
                foreach (var item in filters)
                {
                    var propertyInfo = model.GetType().GetProperty(item.ItemNm);
                    if (propertyInfo != null && !string.IsNullOrEmpty(item.JoInput))
                    {
                        switch (item.ItemNm)
                        {
                            case nameof(VehicleDailyReportSearchParam.OutputSetting):
                                propertyInfo.SetValue(model, byte.TryParse(item.JoInput, out byte outputSetting) ? (byte)outputSetting : (byte)1);
                                break;
                            case nameof(VehicleDailyReportSearchParam.OutputWithHeader):
                                var selectedOutputWithHeader = listOutputWithHeader.FirstOrDefault(x => x.Value == (byte.TryParse(item.JoInput, out byte value) ? (byte)value : (byte)0));
                                propertyInfo.SetValue(model, selectedOutputWithHeader);
                                break;
                            case nameof(VehicleDailyReportSearchParam.KukuriKbn):
                                var selectedKukuriKbn = listKukuriKbn.FirstOrDefault(x => x.Value == (byte.TryParse(item.JoInput, out byte value) ? (byte)value : (byte)0));
                                propertyInfo.SetValue(model, selectedKukuriKbn);
                                break;
                            case nameof(VehicleDailyReportSearchParam.KugiriCharType):
                                var selectedKugiriCharType = listKugiriCharType.FirstOrDefault(x => x.Value == (byte.TryParse(item.JoInput, out byte value) ? (byte)value : (byte)0));
                                propertyInfo.SetValue(model, selectedKugiriCharType);
                                break;
                            case nameof(VehicleDailyReportSearchParam.ScheduleYmdStart):
                            case nameof(VehicleDailyReportSearchParam.ScheduleYmdEnd):
                                propertyInfo.SetValue(model, DateTime.TryParseExact(item.JoInput, "yyyyMMdd", new CultureInfo("ja-JP"), DateTimeStyles.None, out DateTime selectedDate) ? selectedDate : (DateTime?)null);
                                break;

                            case nameof(VehicleDailyReportSearchParam.selectedBusSaleStart):
                            case nameof(VehicleDailyReportSearchParam.selectedBusSaleEnd):
                                var selectedEigyoCdSeq = int.TryParse(item.JoInput, out int selectedE) ? selectedE : 0;
                                var selectedEigyo = listBusSaleBranch.FirstOrDefault(e => e.EigyoCdSeq == selectedEigyoCdSeq);
                                propertyInfo.SetValue(model, selectedEigyo);
                                break;
                            case nameof(VehicleDailyReportSearchParam.selectedBusCodeStart):
                            case nameof(VehicleDailyReportSearchParam.selectedBusCodeEnd):
                                var selectedSyaryoCdSeq = int.TryParse(item.JoInput, out int selectedS) ? selectedS : 0;
                                var selectedSyaryo = listBusCode.FirstOrDefault(e => e.SyaRyoCdSeq == selectedSyaryoCdSeq);
                                propertyInfo.SetValue(model, selectedSyaryo);
                                break;
                            case nameof(VehicleDailyReportSearchParam.ReceptionStart):
                            case nameof(VehicleDailyReportSearchParam.ReceptionEnd):
                                var ukeNo = int.TryParse(item.JoInput, out int selectedU) ? $"{selectedU:0000000000}" : "";
                                propertyInfo.SetValue(model, ukeNo);
                                break;
                            case nameof(VehicleDailyReportSearchParam.selectedReservationStart):
                                var selectedReservationStart = component.ListReservationClass.FirstOrDefault(_ => _.YoyaKbnSeq == int.Parse(item.JoInput));
                                propertyInfo.SetValue(model, selectedReservationStart);
                                break;
                            case nameof(VehicleDailyReportSearchParam.selectedReservationEnd):
                                var selectedReservationEnd = component.ListReservationClass.FirstOrDefault(_ => _.YoyaKbnSeq == int.Parse(item.JoInput));
                                propertyInfo.SetValue(model, selectedReservationEnd);
                                break;
                            case nameof(VehicleDailyReportSearchParam.OutputKbn):
                                var selectedOutputKbn = listOutputKbn.FirstOrDefault(x => x.Value == (byte.TryParse(item.JoInput, out byte value) ? (byte)value : (byte)0));
                                propertyInfo.SetValue(model, selectedOutputKbn);
                                break;
                            case nameof(searchParams.fontSize):
                                searchParams.fontSize = string.IsNullOrEmpty(item.JoInput) ? (byte)ViewMode.Medium : byte.Parse(item.JoInput);
                                break;
                        }
                    }
                }
            }
            return model;
        }

        protected void SaveSearchFilter()
        {
            keyValueFilterPairs = GenerateFilterValueDictionaryService.GenerateForVehicleDailyReport(searchParams).Result;
            FilterConditionService.SaveFilterCondtion(keyValueFilterPairs, FormFilterName.VehicleDailyReport, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq).Wait();
        }

        protected async Task OnHandleChanged(dynamic value, string propName)
        {
            try
            {
                var classType = searchParams.GetType();
                var prop = classType.GetProperty(propName);

                switch (propName)
                {
                    case "ReceptionStart":
                    case "ReceptionEnd":
                        string temp = Convert.ToString(value).Normalize(NormalizationForm.FormKC);
                        if (string.IsNullOrEmpty(value))
                        {
                            prop.SetValue(searchParams, string.Empty, null);
                        }
                        else
                        {
                            if (long.TryParse(temp, out long v) && v > 0)
                            {
                                prop.SetValue(searchParams, temp.PadLeft(10, '0'), null);
                            }
                            else
                            {
                                temp = prop.GetValue(searchParams).ToString();
                                prop.SetValue(searchParams, temp, null);
                            }
                        }
                        break;
                    case "ScheduleYmdStart":
                    case "ScheduleYmdEnd":
                        prop.SetValue(searchParams, value as DateTime?, null);
                        break;
                    case "selectedBusSaleStart":
                    case "selectedBusSaleEnd":
                        prop.SetValue(searchParams, value as BusSaleBranchSearch, null);
                        break;
                    case "selectedBusCodeStart":
                    case "selectedBusCodeEnd":
                        prop.SetValue(searchParams, value as BusCodeSearch, null);
                        break;
                    case "selectedReservationStart":
                    case "selectedReservationEnd":
                        prop.SetValue(searchParams, value as ReservationClassComponentData, null);
                        break;
                    case "OutputKbn":
                        prop.SetValue(searchParams, value as VehicleSearchDropdown, null);
                        break;
                }
                if (searchForm.Validate())
                {
                    await list.OnSearch(false, true);
                    SaveSearchFilter();
                }
                else
                {
                    list.listData = new List<VehicleDailyReportModel>();
                    list.listDataDisplay = new List<VehicleDailyReportModel>();
                    list.listChild = new List<VehicleDailyReportChildModel>();
                    list.listCurrentBus = new List<CurrentBus>();
                    list.listUnkYmd = new List<string>();
                    searchParams.selectedCurrentBus = null;
                    searchParams.SyaRyoCdSeq = 0;
                    list.paging.Reset();
                }
                StateHasChanged();
                CheckDisableButton();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        private async Task ToggleLoading(bool value)
        {
            try
            {
                isLoading = value;
                await InvokeAsync(StateHasChanged);
                await Task.Delay(100);
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected async Task OnResetSearchData()
        {
            try
            {

                var temp = searchParams.Company;
                searchParams = new VehicleDailyReportSearchParam();
                searchParams.OutputSetting = 1;
                searchParams.ScheduleYmdStart = null;
                searchParams.ScheduleYmdEnd = null;
                searchParams.ReceptionStart = string.Empty;
                searchParams.ReceptionEnd = string.Empty;
                searchParams.selectedBusCodeStart = null;
                searchParams.selectedBusCodeEnd = null;
                searchParams.selectedBusSaleStart = null;
                searchParams.selectedBusSaleEnd = null;
                searchParams.selectedReservationStart = null;
                searchParams.selectedReservationEnd = null;
                searchParams.OutputWithHeader = listOutputWithHeader[0];
                searchParams.KukuriKbn = listKukuriKbn[0];
                searchParams.KugiriCharType = listKugiriCharType[0];
                searchParams.OutputKbn = listOutputKbn[0];
                searchParams.EndYmd = string.Empty;
                searchParams.StrYmd = string.Empty;
                searchParams.Company = temp;
                searchForm = new EditContext(searchParams);
                StateHasChanged();
                CheckDisableButton();
                await list.OnClearSearch(searchParams);
                SaveSearchFilter();

            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected async Task OnNavigate()
        {
            try
            {

                if (searchForm.Validate())
                {
                    if (searchParams.OutputSetting == 1)
                    {
                        await OnExportPdf(0);
                    }
                    //else if (searchParams.OutputSetting == 2)
                    //{
                    //    await OnExportPdf(1);
                    //}
                    else if (searchParams.OutputSetting == 3)
                    {
                        await OnExportPdf(2);
                    }
                    else if (searchParams.OutputSetting == 4)
                    {
                        await OnExportCsv();
                    }
                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        private async Task OnExportCsv()
        {
            try
            {
                await ToggleLoading(true);
                var searchParam = (VehicleDailyReportSearchParam)searchParams.Clone();
                searchParam.SyaRyoCdSeq = 0;
                searchParam.selectedUnkYmd = string.Empty;
                var listData = await vehicleDailyReportService.GetListVehicleDailyReport(searchParam);
                if (listData.Count > 0)
                {
                    var dt = listData.ToDataTable<VehicleDailyReportModel>();
                    while (dt.Columns.Count > 60)
                    {
                        dt.Columns.RemoveAt(60);
                    }
                    SetTableHeader(dt);
                    string path = string.Format("{0}/csv/{1}.csv", hostingEnvironment.WebRootPath, Guid.NewGuid());

                    bool isWithHeader = searchParams.OutputWithHeader.Value == 0 ? true : false;
                    bool isEnclose = searchParams.KukuriKbn.Value == 0 ? true : false;
                    string space = searchParams.KugiriCharType.Value == 0 ? "\t" : searchParams.KugiriCharType.Value == 1 ? ";" : ",";

                    var result = CsvHelper.ExportDatatableToCsv(dt, path, true, isWithHeader, isEnclose, space);
                    await new System.Threading.Tasks.TaskFactory().StartNew(() =>
                    {
                        string myExportString = Convert.ToBase64String(result);
                        jsRuntime.InvokeVoidAsync("downloadFileClientSide", myExportString, "csv", "VehicleDailyReport");
                    });
                }
                await ToggleLoading(false);
                SaveSearchFilter();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
                await ToggleLoading(false);
            }
        }

        private void SetTableHeader(DataTable table)
        {
            try
            {

                List<string> listHeader = new List<string>() { "車輛コード", "車号", "車種コード", "車種コード名称", "型区分", "型区分名",
                "運行年月日", "団体名", "団体名２", "行先名", "得意先業者コード", "得意先コード", "支店コード", "得意先業者名", "得意先名",
                "得意先支店名", "得意先略名", "得意先支店略名", "配車年月日", "到着年月日", "出庫日付", "出庫時間", "帰庫日付", "帰庫時間",
                "受付番号", "運行連番", "予約区分", "予約区分名", "始メーター", "終メーター", "実車_一般", "実車_高速", "回送_一般", "回送_高速",
                "その他キロ", "総走行キロ", "乗車人員", "プラス人員", "燃料１コード", "燃料１名称", "燃料１略名", "燃料１数量", "燃料２コード",
                "燃料２名称", "燃料２略名", "燃料２数量", "燃料３コード", "燃料３名称", "燃料３略名", "燃料３数量", "乗務員１コード", "乗務員１氏名",
                "乗務員２コード", "乗務員２氏名", "乗務員３コード", "乗務員３氏名", "乗務員４コード", "乗務員４氏名", "乗務員５コード", "乗務員５氏名" };
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

        private async Task OnExportPdf(byte type)
        {
            try
            {
                await ToggleLoading(true);
                var data = await vehicleDailyReportService.GetPDFData(searchParams);
                if (data.Count > 0)
                {
                    if (type == 0)
                    {
                        await ToggleLoading(false);
                        var searchString = EncryptHelper.EncryptToUrl(searchParams);
                        jsRuntime.InvokeVoidAsync("open", "vehicledailyreportpreview?searchString=" + searchString, "_blank");
                    }
                    else
                    {
                        XtraReport report = await _reportLayoutSettingService.GetCurrentTemplate(ReportIdForSetting.VehicleDaily, BaseNamespace.VehicleDailyReportTemplate, new ClaimModel().TenantID, new ClaimModel().EigyoCdSeq, searchParams.PaperSize.Value);
                        report.DataSource = data;
                        await new System.Threading.Tasks.TaskFactory().StartNew(async () =>
                        {
                            await new System.Threading.Tasks.TaskFactory().StartNew(() =>
                            {
                                report.CreateDocument();
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    //if (type == 1)
                                    //{
                                    //    PrintToolBase tool = new PrintToolBase(report.PrintingSystem);
                                    //    tool.Print();
                                    //    return;
                                    //}
                                    report.ExportToPdf(ms);

                                    byte[] exportedFileBytes = ms.ToArray();
                                    string myExportString = Convert.ToBase64String(exportedFileBytes);
                                    jsRuntime.InvokeVoidAsync("downloadFileClientSide", myExportString, "pdf", "VehicleDailyReport");
                                }
                            });
                        });
    
                        await ToggleLoading(false);
                        SaveSearchFilter();
                    }
                }
                else
                {
                    await ToggleLoading(false);
                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
                await ToggleLoading(false);
            }
        }

        protected void CheckDisableButton()
        {
            try
            {

                searchForm.Validate();
                if (searchForm.GetValidationMessages().Count() > 0)
                {
                    isDisableButton = true;
                }
                else
                {
                    isDisableButton = false;
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected void OnSetOutputSetting(byte value)
        {
            try
            {

                searchParams.OutputSetting = value;
                if (searchParams.OutputSetting == 4)
                {
                    isDisableCsv = false;
                }
                else
                {
                    isDisableCsv = true;
                }
                SaveSearchFilter();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected void OnSetFontSize(byte value)
        {
            searchParams.fontSize = value;
            SaveSearchFilter();
            StateHasChanged();
        }

        protected void DataNotFound(bool value)
        {
            isDataNotFound = value;
        }

        protected void OnPaperSizeSetting(byte value)
        {
            try
            {
                searchParams.PaperSize = new VehicleSearchDropdown { Value = value };
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected async Task OnClickLayoutSetting(byte type)
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
    }
}
