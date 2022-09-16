using DevExpress.CodeParser;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.IService;
using HassyaAllrightCloud.Pages.Components.TransportDailyReport;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Entities;

namespace HassyaAllrightCloud.Pages
{
    public class TransportDailyReportBase : ComponentBase
    {
        [Inject]
        protected IStringLocalizer<TransportDailyReport> _lang { get; set; }
        [Inject]
        protected ITransportDailyReportService transportDailyReportService { get; set; }
        [Inject]
        protected IWebHostEnvironment hostingEnvironment { get; set; }
        [Inject]
        protected IJSRuntime jsRuntime { get; set; }

        public EditContext searchForm { get; set; }
        public TransportDailyReportSearchParams searchParams = new TransportDailyReportSearchParams();
        public List<TransportDropDown> listOutputWithHeader { get; set; } = new List<TransportDropDown>();
        public List<TransportDropDown> listKukuriKbn { get; set; } = new List<TransportDropDown>();
        public List<TransportDropDown> listKugiriCharType { get; set; } = new List<TransportDropDown>();
        public List<TransportDropDown> listPageSize { get; set; } = new List<TransportDropDown>();
        public List<TransportDropDown> listAggregation { get; set; } = new List<TransportDropDown>();
        public List<CompanySearchData> listCompany { get; set; } = new List<CompanySearchData>();
        public List<EigyoSearchData> listEigyo { get; set; } = new List<EigyoSearchData>();
        public byte fontSize { get; set; } = 2;
        public bool isDisableCSV { get; set; } = true;
        public bool isDataNotFound { get; set; } = false;
        public ListData list = new ListData();
        public bool isLoading { get; set; } = true;

        [Inject]
        protected IErrorHandlerService errorModalService { get; set; }
        [Inject]
        protected IReportLayoutSettingService _reportLayoutSettingService { get; set; }
        public Dictionary<string, string> LangDic = new Dictionary<string, string>();
        [Inject]
        protected IFilterCondition FilterConditionService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var dataLang = _lang.GetAllStrings();
            LangDic = dataLang.ToDictionary(l => l.Name, l => l.Value);

            var conditions = await FilterConditionService.GetFilterCondition(FormFilterName.TransportDailyReport, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq);

            await OnInitData(conditions);
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                jsRuntime.InvokeVoidAsync("EnterTab");
            }
            catch(Exception ex)
            {
                errorModalService.HandleError(ex);
            }
            return base.OnAfterRenderAsync(firstRender);
        }

        private async Task OnInitData(List<TkdInpCon> conditions)
        {
            try
            {
                listOutputWithHeader.AddRange(new List<TransportDropDown>() {
                    new TransportDropDown(){ Value = 0, Text = _lang["output_withheader"] },
                    new TransportDropDown(){ Value = 1, Text = _lang["output_notwithheader"] }
                });

                listKukuriKbn.AddRange(new List<TransportDropDown>() {
                    new TransportDropDown(){ Value = 0, Text = _lang["output_enclose"] },
                    new TransportDropDown(){ Value = 1, Text = _lang["output_notenclose"] }
                });

                listKugiriCharType.AddRange(new List<TransportDropDown>() {
                    new TransportDropDown(){ Value = 0, Text = _lang["output_withtab"] },
                    new TransportDropDown(){ Value = 1, Text = _lang["output_withsemicolon"] },
                    new TransportDropDown(){ Value = 2, Text = _lang["output_withcomma"] }
                });

                listPageSize.AddRange(new List<TransportDropDown>()
                {
                    new TransportDropDown(){ Value = 0, Text = "A4" },
                    new TransportDropDown(){ Value = 1, Text = "A3" },
                    new TransportDropDown(){ Value = 2, Text = "B4" },
                });

                listAggregation.AddRange(new List<TransportDropDown>()
                {
                    new TransportDropDown(){ Value = 1, Text = _lang["general_transportation"] },
                    new TransportDropDown(){ Value = 2, Text = _lang["specific_transportation"] },
                    new TransportDropDown(){ Value = 3, Text = _lang["special_transportation"] },
                });

                var taskCompany = transportDailyReportService.GetListCompanyForSearch(searchParams.CompanyCdSeq);
                var taskEigyo = transportDailyReportService.GetListEigyoForSearch(searchParams.TenantCdSeq, searchParams.CompanyCdSeq);

                await Task.WhenAll(taskCompany, taskEigyo);
                listCompany = taskCompany.Result;
                listEigyo = taskEigyo.Result;

                searchForm = new EditContext(searchParams);

                await OnInitSearchData(conditions);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
                isLoading = false;
                StateHasChanged();
            }
        }

        protected async Task OnHandleChanged(dynamic value, string propName)
        {
            try
            {
                isLoading = true;
                await Task.Delay(100);
                await InvokeAsync(StateHasChanged);
                var classType = searchParams.GetType();
                var prop = classType.GetProperty(propName);

                switch (propName)
                {
                    case "selectedDate":
                        prop.SetValue(searchParams, value as DateTime?, null);
                        break;
                    case "selectedCompany":
                        prop.SetValue(searchParams, value as CompanySearchData, null);
                        break;
                    case "selectedEigyoFrom":
                    case "selectedEigyoTo":
                        prop.SetValue(searchParams, value as EigyoSearchData, null);
                        break;
                    case "aggregation":
                        prop.SetValue(searchParams, value as TransportDropDown, null);
                        break;
                }
                if (searchForm.Validate())
                {
                    await list.OnSearch(false, false);
                }
                else
                {
                    isDataNotFound = false;
                    await list.ClearData();
                }
                if(list.listDataDisplay.Any())
                    await SaveConditions();
                isLoading = false;
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
                isLoading = false;
            }
            StateHasChanged();
        }

        protected async Task OnSetFontSize(byte value)
        {
            try
            {
                fontSize = value;
                searchParams.fontSize = value;
                if (list.listDataDisplay.Any())
                    await SaveConditions();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected async Task OnChangeOutputSetting(byte value)
        {
            try
            {
                searchParams.OutputSetting = value;
                if (searchParams.OutputSetting == 4)
                {
                    isDisableCSV = false;
                }
                else
                {
                    isDisableCSV = true;
                }
                if (list.listDataDisplay.Any())
                    await SaveConditions();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected async Task OnChangeOutputCategory(byte value)
        {
            try
            {
                isLoading = true;
                await Task.Delay(100);
                await InvokeAsync(StateHasChanged);
                searchParams.OutputCategory = value;
                list.InitTotal2();
                await list.OnSearch(false, false);
                if (list.listDataDisplay.Any())
                    await SaveConditions();
                isLoading = false;
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
                isLoading = false;
            }
            StateHasChanged();
        }

        protected async Task OnChangeTotalType(byte value)
        {
            try
            {
                searchParams.TotalType = value;
                if (list.listDataDisplay.Any())
                    await SaveConditions();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected void DataNotFound(bool value)
        {
            isDataNotFound = value;
        }

        protected async Task OnInitSearchData(List<TkdInpCon> conditions = null)
        {
            isLoading = true;
            await Task.Delay(100);
            await InvokeAsync(StateHasChanged);
            if(conditions == null)
            {
                SetInit();
            }
            else
            {
                if (conditions.Any())
                {
                    searchParams.OutputSetting = byte.Parse(conditions.FirstOrDefault(_ => _.ItemNm == nameof(searchParams.OutputSetting))?.JoInput ?? "1");
                    searchParams.OutputCategory = byte.Parse(conditions.FirstOrDefault(_ => _.ItemNm == nameof(searchParams.OutputCategory))?.JoInput ?? "1");
                    searchParams.TotalType = byte.Parse(conditions.FirstOrDefault(_ => _.ItemNm == nameof(searchParams.TotalType))?.JoInput ?? "0");
                    searchParams.csvHeader = listOutputWithHeader.FirstOrDefault(_ => _.Value == byte.Parse(conditions.FirstOrDefault(_ => _.ItemNm == nameof(searchParams.csvHeader))?.JoInput ?? "0"));
                    searchParams.csvEnclose = listKukuriKbn.FirstOrDefault(_ => _.Value == byte.Parse(conditions.FirstOrDefault(_ => _.ItemNm == nameof(searchParams.csvEnclose))?.JoInput ?? "0"));
                    searchParams.csvSpace = listKugiriCharType.FirstOrDefault(_ => _.Value == byte.Parse(conditions.FirstOrDefault(_ => _.ItemNm == nameof(searchParams.csvSpace))?.JoInput ?? "0"));
                    searchParams.pageSize = listPageSize.FirstOrDefault(_ => _.Value == byte.Parse(conditions.FirstOrDefault(_ => _.ItemNm == nameof(searchParams.pageSize))?.JoInput ?? "0"));
                    searchParams.aggregation = listAggregation.FirstOrDefault(_ => _.Value == byte.Parse(conditions.FirstOrDefault(_ => _.ItemNm == nameof(searchParams.aggregation))?.JoInput ?? "0"));
                    searchParams.selectedCompany = listCompany.FirstOrDefault(_ => _.CompanyCdSeq == int.Parse(conditions.FirstOrDefault(_ => _.ItemNm == nameof(searchParams.selectedCompany))?.JoInput ?? "0"));
                    searchParams.selectedEigyoFrom = listEigyo.FirstOrDefault(_ => _.EigyoCdSeq == int.Parse(conditions.FirstOrDefault(_ => _.ItemNm == nameof(searchParams.selectedEigyoFrom))?.JoInput ?? "0"));
                    searchParams.selectedEigyoTo = listEigyo.FirstOrDefault(_ => _.EigyoCdSeq == int.Parse(conditions.FirstOrDefault(_ => _.ItemNm == nameof(searchParams.selectedEigyoTo))?.JoInput ?? "0"));
                    var date = conditions.FirstOrDefault(_ => _.ItemNm == nameof(searchParams.selectedDate));
                    searchParams.selectedDate = date == null ? DateTime.Now : DateTime.Parse(date.JoInput);
                    searchParams.fontSize = byte.Parse(conditions.FirstOrDefault(_ => _.ItemNm == nameof(searchParams.fontSize))?.JoInput ?? ((byte)ViewMode.Medium).ToString());
                    fontSize = searchParams.fontSize;
                }
                else
                {
                    SetInit();
                }
            }

            if(list.searchParams != null)
            {
                await list.OnSearch(false, false);
            }

            if (conditions == null && list.listDataDisplay.Any())
            {
                await FilterConditionService.DeleteCustomFilerCondition(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, 0, FormFilterName.TransportDailyReport);
            }

            isLoading = false;
            StateHasChanged();
        }

        private void SetInit()
        {
            searchParams.OutputSetting = 1;
            searchParams.OutputCategory = 1;
            searchParams.TotalType = 0;
            searchParams.csvHeader = listOutputWithHeader[0];
            searchParams.csvEnclose = listKukuriKbn[0];
            searchParams.csvSpace = listKugiriCharType[0];
            searchParams.pageSize = listPageSize[0];
            searchParams.aggregation = listAggregation[0];
            searchParams.selectedCompany = null;
            searchParams.selectedEigyoFrom = null;
            searchParams.selectedEigyoTo = null;
            searchParams.selectedDate = DateTime.Now;
            fontSize = (byte)ViewMode.Medium;
            searchParams.fontSize = (byte)ViewMode.Medium;
            searchForm.Validate();
        }

        protected async Task OnExport()
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
                errorModalService.HandleError(ex);
            }
        }

        private async Task OnExportCsv()
        {
            try
            {
                var listData = await transportDailyReportService.GetListTransportDailyReport(searchParams);
                if (listData.Count > 0)
                {
                    listData.ForEach(e => FormatCsvData(e));
                    var dt = listData.ToDataTable<TransportDailyReportData>();
                    while (dt.Columns.Count > 71)
                    {
                        dt.Columns.RemoveAt(71);
                    }
                    SetTableHeader(dt);
                    string path = string.Format("{0}/csv/{1}.csv", hostingEnvironment.WebRootPath, Guid.NewGuid());

                    bool isWithHeader = searchParams.csvHeader.Value == 0 ? true : false;
                    bool isEnclose = searchParams.csvEnclose.Value == 0 ? true : false;
                    string space = searchParams.csvSpace.Value == 0 ? "\t" : searchParams.csvSpace.Value == 1 ? ";" : ",";

                    var result = CsvHelper.ExportDatatableToCsv(dt, path, false, isWithHeader, isEnclose, space);
                    await new System.Threading.Tasks.TaskFactory().StartNew(() =>
                    {
                        string myExportString = Convert.ToBase64String(result);
                        jsRuntime.InvokeVoidAsync("downloadFileClientSide", myExportString, "csv", "TransportDailyReport");
                    });
                }
                else
                {
                    isDataNotFound = true;
                }
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        private void SetTableHeader(DataTable table)
        {
            List<string> listHeader = new List<string>() { "営業所コード", "営業所名", "営業所略名", "車輌コード", "車号", "定員",
                "業者コード", "得意先コード", "支店コード", "得意先名", "支店名", "得意先略名", "支店略名", "団体名", "団体名２",
                "行き先名", "日泊", "出庫年月日", "出庫時間", "配車年月日", "配車時間", "到着年月日", "到着時間", "帰庫年月日",
                "帰庫時間", "運賃", "消費税率", "消費税", "手数料率", "手数料", "差引収入", "乗車人員", "ﾌﾟﾗｽ人員", "実車一般キロ",
                "実車高速キロ", "回送一般キロ", "回送高速キロ", "その他キロ", "総走行キロ", "燃料１コード", "燃料１名", "燃料１略名", "燃料１",
                "燃料２コード", "燃料２名", "燃料２略名", "燃料２", "燃料３コード", "燃料３名", "燃料３略名", "燃料３", "職務区分コード１",
                "職務区分名１", "乗務員１コード", "乗務員１名", "職務区分コード２", "職務区分名２", "乗務員２コード", "乗務員２名", "職務区分コード３",
                "職務区分名３", "乗務員３コード", "乗務員３名", "職務区分コード４", "職務区分名４", "乗務員４コード", "乗務員４名", "職務区分コード５",
                "職務区分名５", "乗務員５コード", "乗務員５名"};
            for (int i = 0; i < table.Columns.Count; i++)
            {
                table.Columns[i].ColumnName = listHeader[i];
            }
        }

        private void FormatCsvData(TransportDailyReportData item)
        {
            item.EigyoCd = string.IsNullOrEmpty(item.EigyoCd)? "00000" : item.EigyoCd.PadLeft(5, '0');
            item.SyaRyoCd = item.SyaRyoCd.PadLeft(5, '0');
            item.GyosyaCd = item.GyosyaCd.PadLeft(3, '0');
            item.TokuiCd = item.TokuiCd.PadLeft(4, '0');
            item.SitenCd = item.SitenCd.PadLeft(4, '0');
            item.Zeiritsu = decimal.Parse(item.Zeiritsu).ToString("N2");
            item.TesuRitu = decimal.Parse(item.TesuRitu).ToString("N");
            item.Total_JisaIPKm = decimal.Parse(item.Total_JisaIPKm).ToString("N2");
            item.Total_JisaKSKm = decimal.Parse(item.Total_JisaKSKm).ToString("N2");
            item.Total_KisoIPKm = decimal.Parse(item.Total_KisoIPKm).ToString("N2");
            item.Total_KisoKSKm = decimal.Parse(item.Total_KisoKSKm).ToString("N2");
            item.Total_OthKm = decimal.Parse(item.Total_OthKm).ToString("N2");
            item.Total_TotalKm = decimal.Parse(item.Total_TotalKm).ToString("N2");
            item.Nenryo1Cd = string.IsNullOrEmpty(item.Nenryo1Cd) ? "00" : item.Nenryo1.PadLeft(2, '0');
            item.Nenryo1 = decimal.Parse(item.Nenryo1).ToString("N2");
            item.Nenryo2Cd = string.IsNullOrEmpty(item.Nenryo2Cd) ? "00" : item.Nenryo2.PadLeft(2, '0');
            item.Nenryo2 = decimal.Parse(item.Nenryo2).ToString("N2");
            item.Nenryo3Cd = string.IsNullOrEmpty(item.Nenryo3Cd) ? "00" : item.Nenryo3.PadLeft(2, '0');
            item.Nenryo3 = decimal.Parse(item.Nenryo3).ToString("N2");
            item.SyainCd1 = item.SyainCd1.Trim().PadLeft(10, '0');
            item.SyainCd2 = item.SyainCd2.Trim().PadLeft(10, '0');
            item.SyainCd3 = item.SyainCd3.Trim().PadLeft(10, '0');
            item.SyainCd4 = item.SyainCd4.Trim().PadLeft(10, '0');
            item.SyainCd5 = item.SyainCd5.Trim().PadLeft(10, '0');
        }

        private async Task OnExportPdf(byte type)
        {
            var data = await transportDailyReportService.GetDataPDF(searchParams);
            if(data.Count > 0)
            {
                if (type == 2)
                {
                    XtraReport report = null;
                    if (searchParams.pageSize.Value == 0)
                    {
                        report = new Reports.TransportDailyReportA4();
                    }
                    else if (searchParams.pageSize.Value == 1)
                    {
                        report = new Reports.TransportDailyReportA3();
                    }
                    else
                    {
                        report = new Reports.TransportDailyReportB4();
                    }
                    //XtraReport report = await _reportLayoutSettingService.GetCurrentTemplate(ReportIdForSetting.TransportDaily, BaseNamespace.TransportDailyReportTemplate, new ClaimModel().TenantID, searchParams.pageSize.Value);
                    report.DataSource = data;
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
                }
                else
                {
                    var searchString = EncryptHelper.EncryptToUrl(searchParams);
                    jsRuntime.InvokeVoidAsync("open", "transportdailyreportpreview?searchString=" + searchString, "_blank");
                }
            }
            else
            {
                isDataNotFound = true;
            }
        }

        protected async Task OnClickLayoutSetting(byte type)
        {
            try
            {
                if (type == 0)
                {
                    isLoading = true;
                    await Task.Delay(100);
                    await InvokeAsync(StateHasChanged);
                    await list.OnSave();
                    isLoading = false;
                }
                else
                {
                    
                }
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
                isLoading = false;
            }
            StateHasChanged();
        }

        protected async Task OnChangeOutputSetting(TransportDropDown item, string propName)
        {
            switch (propName)
            {
                case nameof(TransportDailyReportSearchParams.pageSize):
                    searchParams.pageSize = item;
                    break;
                case nameof(TransportDailyReportSearchParams.csvHeader):
                    searchParams.csvHeader = item;
                    break;
                case nameof(TransportDailyReportSearchParams.csvEnclose):
                    searchParams.csvEnclose = item;
                    break;
                case nameof(TransportDailyReportSearchParams.csvSpace):
                    searchParams.csvSpace = item;
                    break;
            }
            if (list.listDataDisplay.Any())
                await SaveConditions();
            StateHasChanged();
        }

        protected async Task SaveConditions()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            dict.Add(nameof(TransportDailyReportSearchParams.OutputCategory), searchParams.OutputCategory.ToString());
            dict.Add(nameof(TransportDailyReportSearchParams.selectedDate), searchParams.selectedDate.ToString());
            dict.Add(nameof(TransportDailyReportSearchParams.selectedCompany), searchParams.selectedCompany?.CompanyCdSeq.ToString() ?? "0");
            dict.Add(nameof(TransportDailyReportSearchParams.selectedEigyoFrom), searchParams.selectedEigyoFrom?.EigyoCdSeq.ToString() ?? "0");
            dict.Add(nameof(TransportDailyReportSearchParams.selectedEigyoTo), searchParams.selectedEigyoTo?.EigyoCdSeq.ToString() ?? "0");
            dict.Add(nameof(TransportDailyReportSearchParams.aggregation), searchParams.aggregation?.Value.ToString() ?? "0");
            dict.Add(nameof(TransportDailyReportSearchParams.fontSize), searchParams.fontSize.ToString());
            dict.Add(nameof(TransportDailyReportSearchParams.TotalType), searchParams.TotalType.ToString());
            dict.Add(nameof(TransportDailyReportSearchParams.OutputSetting), searchParams.OutputSetting.ToString());
            dict.Add(nameof(TransportDailyReportSearchParams.pageSize), searchParams.pageSize.Value.ToString());
            dict.Add(nameof(TransportDailyReportSearchParams.csvHeader), searchParams.csvHeader.Value.ToString());
            dict.Add(nameof(TransportDailyReportSearchParams.csvEnclose), searchParams.csvEnclose.Value.ToString());
            dict.Add(nameof(TransportDailyReportSearchParams.csvSpace), searchParams.csvSpace.Value.ToString());

            await FilterConditionService.SaveFilterCondtion(dict, FormFilterName.TransportDailyReport, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq);
        }
    }
}
