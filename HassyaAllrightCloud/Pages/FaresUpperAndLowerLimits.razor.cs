using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Generic;
using System;
using System.Linq;
using HassyaAllrightCloud.Pages.Components;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Commons.Helpers;
using System.IO;
using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Commons.Extensions;
using System.Data;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;
using Newtonsoft.Json;

namespace HassyaAllrightCloud.Pages
{
    public class FaresUpperAndLowerLimitsBase : ComponentBase
    {
        #region Inject
        [Inject]
        protected IStringLocalizer<FaresUpperAndLowerLimits> _lang { get; set; }
        [Inject]
        protected IFaresUpperAndLowerLimitsService _service { get; set; }
        [Inject]
        private ILoadingService _loading { get; set; }
        [Inject]
        protected IJSRuntime _jsRuntime { get; set; }
        [Inject]
        protected IWebHostEnvironment _hostingEnvironment { get; set; }
        [Inject]
        protected IErrorHandlerService _errorModalService { get; set; }
        [Inject]
        protected IGenerateFilterValueDictionary _generateFilterValueDictionaryService { get; set; }
        [Inject]
        protected IFilterCondition _filterConditionService { get; set; }
        [Parameter]
        public EventCallback<MouseEventArgs> TogglePopup { get; set; }
        #endregion Inject

        #region Property
        protected List<SaleOffice> saleOffice = new List<SaleOffice>();
        protected List<Cause> lstCauses = new List<Cause>();
        protected List<SalePersonInCharge> salePersonInCharge = new List<SalePersonInCharge>();
        protected List<string> formSetting = new List<string>();
        protected List<string> outputWithHeader = new List<string>();
        protected List<string> kukuriKbn = new List<string>();
        protected List<string> kugiriCharType = new List<string>();
        protected List<FaresUpperAndLowerLimitGrid> DataSourcePaging { get; set; } = new List<FaresUpperAndLowerLimitGrid>();
        protected List<FaresUpperAndLowerLimitGrid> DataSource { get; set; } = new List<FaresUpperAndLowerLimitGrid>();
        public Dictionary<string, string> _langDic = new Dictionary<string, string>();
        protected FaresUpperAndLowerLimitGrid faresUpperAndLowerLimitItemSelect;
        protected EditContext editFormContext { get; set; }
        protected int activeTabIndex = 0;
        protected int ActiveTabIndex { get => activeTabIndex; set { activeTabIndex = value; StateHasChanged(); } }
        protected FaresUpperAndLowerLimitsFormSearch searchModel = new FaresUpperAndLowerLimitsFormSearch();
        protected bool isFirstRender { get; set; } = true;
        protected int ActiveV { get; set; }
        protected int ActiveL { get; set; }
        protected List<SaleOffice> saleOffices = new List<SaleOffice>();
        public byte itemPerPage { get; set; } = Common.DefaultPageSize;
        public int currentPage { get; set; } = 0;
        protected Pagination paging = new Pagination();
        protected CommonListCombobox CommonValue = new CommonListCombobox();
        protected bool PopupVisible = false;
        protected short causeReason;
        protected string otherReasonInput = "";
        protected bool PopupDeleteVisible = false;
        protected bool isDataNotFound { get; set; } = false;
        protected int IndexItem { get; set; }
        #endregion Property
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

        /// <summary>
        /// 受付番号 only input number client side
        /// </summary>
        /// <param name="firstRender"></param>
        /// <returns></returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (isFirstRender)
            {
                await LoadJS();
                isFirstRender = false;
            }
        }

        public   async Task LoadJS()
        {
            await _jsRuntime.InvokeVoidAsync("setEventforCodeNumberFieldV2", ".code-number-ser", true, 10);
            await _jsRuntime.InvokeVoidAsync("EnterTab");
            await InvokeAsync(StateHasChanged);
        }

        protected async Task UpdateFormValue(string propertyName, dynamic value)
        {
            try
            {
                await _loading.ShowAsync();
                SetValue(propertyName, value);
                await _loading.HideAsync();
            }
            catch (Exception ex)
            {
                _errorModalService.HandleError(ex);
            }
        }

        protected async Task OnUpdateOtherReason(string value)
        {
            otherReasonInput = value.TruncateWithMaxLength(200);
            await InvokeAsync(StateHasChanged);
        }

        private async void SetValue(string propertyName, dynamic value)
        {
            var isNumber = int.TryParse(value.ToString(), out int n);
            var propertyInfo = searchModel.GetType().GetProperty(propertyName);
            // if 受付番号 is't number, do nothing
            if ((propertyName == nameof(searchModel.ReservationNumberStart) || propertyName == nameof(searchModel.ReservationNumberEnd)) && !isNumber)
            {
                await InvokeAsync(StateHasChanged);
                return;
            }
            else if ((propertyName == nameof(searchModel.ReservationNumberStart) || propertyName == nameof(searchModel.ReservationNumberEnd)) && isNumber && value.ToString() != "")
                value = string.Format("{0:D10}", int.Parse(((string)value).TruncateWithMaxLength(10)));

            propertyInfo.SetValue(searchModel, value, null);
            if (editFormContext.Validate())
            {
                var keyValueFilterPairs = _generateFilterValueDictionaryService.GenerateForFaresUpperAndLowerLimits(searchModel).Result;
                await _filterConditionService.SaveFilterCondtion(keyValueFilterPairs, FormFilterName.FaresUpperAndLowerLimits, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq);
                //only search when change data in tab 条件設定
                if (propertyName != nameof(searchModel.FormSetting) && propertyName != nameof(searchModel.OutputWithHeader) && propertyName != nameof(searchModel.KukuriKbn)
                 && propertyName != nameof(searchModel.ActiveL) && propertyName != nameof(searchModel.ActiveV) && propertyName != nameof(searchModel.OutputSetting) && propertyName != nameof(searchModel.KugiriCharType))
                    await OnSearch(false);
                await InvokeAsync(StateHasChanged);
            }
        }

        /// <summary>
        /// Change 表示設定,レイアウト設定
        /// </summary>
        /// <param name="e"></param>
        /// <param name="number">value selected</param>
        /// <param name="mode">表示設定 or レイアウト設定</param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected async Task ChangeMode(MouseEventArgs e, int number, int mode, int value)
        {
            try
            {
                if (mode == (int)ModeChangeV.ViewMode)
                {
                    ActiveV = number;
                    await UpdateFormValue(nameof(searchModel.ActiveV), ActiveV);
                }
                else if (mode == (int)ModeChangeV.LayoutMode)
                {
                    ActiveL = number;
                    await UpdateFormValue(nameof(searchModel.ActiveL), ActiveL);
                }
                else if (mode == (int)ModeChangeV.OutputInstruction)
                {
                    searchModel.OutputSetting = (OutputInstruction)value;
                    await UpdateFormValue(nameof(searchModel.OutputSetting), searchModel.OutputSetting);
                }
            }
            catch (Exception ex)
            {
                _errorModalService.HandleError(ex);
            }
        }

        /// <summary>
        /// export data
        /// </summary>
        /// <returns></returns>
        protected async Task BtnStart()
        {
            try
            {
                if (editFormContext.Validate() && !isDataNotFound)
                    if (searchModel.OutputSetting == OutputInstruction.Preview)
                    {
                        var searchString = EncryptHelper.EncryptToUrl(searchModel);
                        await _jsRuntime.InvokeVoidAsync("open", "faresupperandlowerlimitpreview?searchString=" + searchString, "_blank");
                    }
                    else if (searchModel.OutputSetting == OutputInstruction.Csv)
                    {
                        var listData = await _service.GetDataCsv(searchModel);
                        var dt = listData.ToDataTable<FaresUpperAndLowerLimitCsv>();
                        while (dt.Columns.Count > 60)
                        {
                            dt.Columns.RemoveAt(60);
                        }
                        SetTableHeader(dt);
                        string path = string.Format("{0}/csv/{1}.csv", _hostingEnvironment.WebRootPath, Guid.NewGuid());

                        bool isWithHeader = searchModel.OutputWithHeader == EnumHelper.GetDescription<OutputWithHeader>(OutputWithHeader.Output) ? true : false;
                        bool isEnclose = searchModel.KukuriKbn == EnumHelper.GetDescription<KukuriKbn>(KukuriKbn.EncloseIn) ? true : false;
                        string space = searchModel.KugiriCharType == EnumHelper.GetDescription<KugiriCharType>(KugiriCharType.Tab) ? "\t" : (searchModel.KugiriCharType == EnumHelper.GetDescription<KugiriCharType>(KugiriCharType.Semicolon) ? ";" : ",");

                        var result = CsvHelper.ExportDatatableToCsv(dt, path, true, isWithHeader, isEnclose, space);
                        await new System.Threading.Tasks.TaskFactory().StartNew(() =>
                        {
                            string myExportString = Convert.ToBase64String(result);
                            _jsRuntime.InvokeVoidAsync("downloadFileClientSide", myExportString, "csv", "FaresUpperAndLowerLimit");
                        });
                    }
                    else
                    {
                        var data = await _service.GetDataReport(searchModel);
                        if (data != null)
                        {
                            XtraReport report = null;
                            if (searchModel.FormSetting == FormSetting.A4.ToString())
                                report = new Reports.ReportTemplate.FaresUpperAndLowerLimits.FaresUpperAndLowerLimitsA4();
                            else if (searchModel.FormSetting == FormSetting.B4.ToString())
                                report = new Reports.ReportTemplate.FaresUpperAndLowerLimits.FaresUpperAndLowerLimitsB4();
                            else if (searchModel.FormSetting == FormSetting.A3.ToString())
                                report = new Reports.ReportTemplate.FaresUpperAndLowerLimits.FaresUpperAndLowerLimitsA3();
                            var dataReport = new List<FaresUpperAndLowerLimitReport>();
                            dataReport.Add(data);
                            report.DataSource = dataReport;

                            await new System.Threading.Tasks.TaskFactory().StartNew(() =>
                            {
                                report.CreateDocument();
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    report.ExportToPdf(ms);

                                    byte[] exportedFileBytes = ms.ToArray();
                                    string myExportString = Convert.ToBase64String(exportedFileBytes);
                                    _jsRuntime.InvokeVoidAsync("downloadFileClientSide", myExportString, "pdf", "FaresUpperAndLowerLimit");

                                }
                            });
                        }
                    }
            }
            catch (Exception ex)
            {
                _errorModalService.HandleError(ex);
            }
        }

        protected void SetTableHeader(DataTable table)
        {
            List<string> listHeader = new List<string>() { "受付番号","配車年月日","到着年月日","車輌名",
                                                           "計画の下限金額","計画の上限金額","実績の下限金額","実績の上限金額"
                                                           ,"請求額","原因","計画の走行キロ（Km）","計画の時間（時間","実績の走行キロ（Km)"
                                                           ,"実績の時間（時間)","交代運転者","特殊車両","身障者割引","学校割引", };
            for (int i = 0; i < table.Columns.Count; i++)
            {
                table.Columns[i].ColumnName = listHeader[i];
            }
        }

        public async Task OnSearch(bool isFirst)
        {
            var result = await _service.GetFaresUpperAndLowerLimitsList(searchModel);
            int index = 1;
            DataSource = result.Select(x => new FaresUpperAndLowerLimitGrid
            {
                GridNo = index++,
                GridReservationNumber = x.UkeNo.Substring(5, 10),
                GridOperationYmd = $"{DateTime.ParseExact(x.HaiSYmd, Formats.yyyyMMdd, null).ToString(Commons.DateTimeFormat.yyyyMMddSlash) } ～ {DateTime.ParseExact(x.TouYmd, Formats.yyyyMMdd, null).ToString(Commons.DateTimeFormat.yyyyMMddSlash)}",
                GridVehicleName = x.SyaryoNm,
                GridPlan = "計画",
                GridActualResult = "実績",
                GridPlanMinAmount = x?.MitsumoriSumMinAmount.ToString("#,##0"),
                GridPlanMaxAmount = x?.MitsumoriSumMaxAmount.ToString("#,##0"),
                GridActualMinAmount = x?.JissekiSumMinAmount.ToString("#,##0"),
                GridActualMaxAmount = x?.JissekiSumMaxAmount.ToString("#,##0"),
                GridBillingAmount = x?.SeikyuGaku.ToString("#,##0"),
                GridCause = string.IsNullOrEmpty(x.CauseNm) ? "" : x.CauseNm,
                GridPlanRunningKilomet = x?.RunningKmSum.ToString("#,##0.#0"),
                GridPlanTotalTime = $"{Convert.ToInt32(x.RestraintTimeSum.Trim()).ToString("D4").Substring(0, 2)}:{Convert.ToInt32(x.RestraintTimeSum.Trim()).ToString("D4").Substring(2, 2)}",
                GridActualRunningKilomet = (x?.EndMeter - x?.StMeter)?.ToString("#,##0.#0"),
                GridActualTotalTime = $"{CaculateTime(x.KoskuTime, x.InspectionTime).Item1}:{CaculateTime(x.KoskuTime, x.InspectionTime).Item2}",
                GridChangedDriver = x.ChangeDriverFeeFlag == 1 ? "〇" : "",
                GridSpecialVehicle = x.SpecialFlg == 1 ? "〇" : "",
                GridDisabledPersonDiscount = x?.DisabledPersonDiscount == 1 ? "〇" : "",
                GridSchoolDiscount = x?.SchoolDiscount == 1 ? "〇" : "",
                CssClass = (x?.SeikyuGaku > x?.JissekiSumMaxAmount ? "fa fa-caret-up" : (x?.SeikyuGaku < x?.JissekiSumMinAmount ? "fa fa-caret-down" : "")),
                UnkRenGrid = x?.UnkRen ?? 0,
                SyaSyuRenGrid = x?.SyaSyuRen ?? 0,
                TeiDanNoGrid = x?.TeiDanNo ?? 0,
                BunkRenGrid = x?.BunkRen ?? 0
            }).ToList();

            if (DataSource.Any())
            {
                isDataNotFound = false;
                DataSourcePaging = DataSource.Skip(currentPage * itemPerPage).Take(itemPerPage).ToList();
            }
            else
            {
                isDataNotFound = true;
                DataSource = new List<FaresUpperAndLowerLimitGrid>();
                DataSourcePaging = new List<FaresUpperAndLowerLimitGrid>();
            }
            if (isFirst)
                isDataNotFound = false;
        }

        protected async Task InitData()
        {
            var dataLang = _lang.GetAllStrings();
            var filterValues = _filterConditionService.GetFilterCondition(FormFilterName.FaresUpperAndLowerLimits, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq).Result;

            paging.currentPage = 0;
            currentPage = 0;
            IndexItem = 0;
            CommonValue = await _service.GetCommonCombobox();
            formSetting = EnumHelper.GetDescriptions<FormSetting>().ToList();
            outputWithHeader = EnumHelper.GetDescriptions<OutputWithHeader>().ToList();
            kukuriKbn = EnumHelper.GetDescriptions<KukuriKbn>().ToList();
            kugiriCharType = EnumHelper.GetDescriptions<KugiriCharType>().ToList();
            saleOffice = CommonValue.SaleOffices;
            salePersonInCharge = CommonValue.SalePersonInCharges;
            _langDic = dataLang.ToDictionary(l => l.Name, l => l.Value);

            if (filterValues.Any())
            {
                searchModel.DateClassification = (DateClassification)int.Parse(filterValues.FirstOrDefault(x => x.ItemNm == nameof(FaresUpperAndLowerLimitsFormSearch.DateClassification))?.JoInput);
                searchModel.OutputStartDate = filterValues.FirstOrDefault(x => x.ItemNm == nameof(FaresUpperAndLowerLimitsFormSearch.OutputStartDate)) != null ? DateTime.ParseExact(filterValues.FirstOrDefault(x => x.ItemNm == nameof(FaresUpperAndLowerLimitsFormSearch.OutputStartDate))?.JoInput, Formats.yyyyMMdd, null) : (DateTime?)null;
                searchModel.OutputEndDate = filterValues.FirstOrDefault(x => x.ItemNm == nameof(FaresUpperAndLowerLimitsFormSearch.OutputEndDate)) != null ? DateTime.ParseExact(filterValues.FirstOrDefault(x => x.ItemNm == nameof(FaresUpperAndLowerLimitsFormSearch.OutputEndDate))?.JoInput, Formats.yyyyMMdd, null) : (DateTime?)null;
                searchModel.SaleOffice = saleOffice.FirstOrDefault(x => x.EigyoCdSeq == (filterValues.FirstOrDefault(x => x.ItemNm == nameof(FaresUpperAndLowerLimitsFormSearch.SaleOffice)) == null ? 0 : int.Parse(filterValues.FirstOrDefault(x => x.ItemNm == nameof(FaresUpperAndLowerLimitsFormSearch.SaleOffice))?.JoInput)));
                searchModel.SalePersonInCharge = salePersonInCharge.FirstOrDefault(x => x.SyainCdSeq == (filterValues.FirstOrDefault(x => x.ItemNm == nameof(FaresUpperAndLowerLimitsFormSearch.SalePersonInCharge)) == null ? 0 : int.Parse(filterValues.FirstOrDefault(x => x.ItemNm == nameof(FaresUpperAndLowerLimitsFormSearch.SalePersonInCharge))?.JoInput.Split(',')[0]))
                                                                             && x.EigyoCdSeq == (filterValues.FirstOrDefault(x => x.ItemNm == nameof(FaresUpperAndLowerLimitsFormSearch.SalePersonInCharge)) == null ? 0 : int.Parse(filterValues.FirstOrDefault(x => x.ItemNm == nameof(FaresUpperAndLowerLimitsFormSearch.SalePersonInCharge))?.JoInput.Split(',')[1])));
                searchModel.Range = filterValues.FirstOrDefault(x => x.ItemNm == nameof(FaresUpperAndLowerLimitsFormSearch.Range)) == null ? (int)PlanningScope.OutOfRange : int.Parse(filterValues.FirstOrDefault(x => x.ItemNm == nameof(FaresUpperAndLowerLimitsFormSearch.Range)).JoInput);
                searchModel.ItemOutOfRange = (ItemOutOfRange)int.Parse(filterValues.FirstOrDefault(x => x.ItemNm == nameof(FaresUpperAndLowerLimitsFormSearch.ItemOutOfRange))?.JoInput);
                searchModel.CauseInput = filterValues.FirstOrDefault(x => x.ItemNm == nameof(FaresUpperAndLowerLimitsFormSearch.CauseInput)) == null ? (int)CauseInput.Input : int.Parse(filterValues.FirstOrDefault(x => x.ItemNm == nameof(FaresUpperAndLowerLimitsFormSearch.CauseInput)).JoInput);
                searchModel.ChooseCause = (ChooseCause)int.Parse(filterValues.FirstOrDefault(x => x.ItemNm == nameof(FaresUpperAndLowerLimitsFormSearch.ChooseCause))?.JoInput);
                searchModel.ReservationNumberStart = !string.IsNullOrEmpty(filterValues.FirstOrDefault(x => x.ItemNm == nameof(FaresUpperAndLowerLimitsFormSearch.ReservationNumberStart))?.JoInput) ? filterValues.FirstOrDefault(x => x.ItemNm == nameof(FaresUpperAndLowerLimitsFormSearch.ReservationNumberStart))?.JoInput?.PadLeft(10, '0') : "";
                searchModel.ReservationNumberEnd = !string.IsNullOrEmpty(filterValues.FirstOrDefault(x => x.ItemNm == nameof(FaresUpperAndLowerLimitsFormSearch.ReservationNumberEnd))?.JoInput) ? filterValues.FirstOrDefault(x => x.ItemNm == nameof(FaresUpperAndLowerLimitsFormSearch.ReservationNumberEnd))?.JoInput?.PadLeft(10, '0') : "";
                ActiveV = int.Parse(filterValues.FirstOrDefault(x => x.ItemNm == nameof(FaresUpperAndLowerLimitsFormSearch.ActiveV))?.JoInput ?? ((int)ViewMode.Large).ToString());
                ActiveL = int.Parse(filterValues.FirstOrDefault(x => x.ItemNm == nameof(FaresUpperAndLowerLimitsFormSearch.ActiveL))?.JoInput ?? ((int)LayoutMode.Save).ToString());
                searchModel.OutputSetting = (OutputInstruction)int.Parse(filterValues.FirstOrDefault(x => x.ItemNm == nameof(FaresUpperAndLowerLimitsFormSearch.OutputSetting))?.JoInput);
                searchModel.FormSetting = formSetting.FirstOrDefault(x => x == filterValues.FirstOrDefault(x => x.ItemNm == nameof(FaresUpperAndLowerLimitsFormSearch.FormSetting))?.JoInput);
                searchModel.OutputWithHeader = outputWithHeader.FirstOrDefault(x => x == filterValues.FirstOrDefault(x => x.ItemNm == nameof(FaresUpperAndLowerLimitsFormSearch.OutputWithHeader))?.JoInput);
                searchModel.KukuriKbn = kukuriKbn.FirstOrDefault(x => x == filterValues.FirstOrDefault(x => x.ItemNm == nameof(FaresUpperAndLowerLimitsFormSearch.KukuriKbn))?.JoInput);
                searchModel.KugiriCharType = kugiriCharType.FirstOrDefault(x => x == filterValues.FirstOrDefault(x => x.ItemNm == nameof(FaresUpperAndLowerLimitsFormSearch.KugiriCharType))?.JoInput);
            }
            else
            {
                ActiveV = (int)ViewMode.Medium;
                ActiveL = (int)LayoutMode.Save;
                searchModel = new FaresUpperAndLowerLimitsFormSearch();
                searchModel.FormSetting = formSetting.FirstOrDefault();
                searchModel.OutputWithHeader = outputWithHeader.FirstOrDefault();
                searchModel.KukuriKbn = kukuriKbn.FirstOrDefault();
                searchModel.KugiriCharType = kugiriCharType.FirstOrDefault();
                searchModel.Range = (int)PlanningScope.OutOfRange;
                searchModel.CauseInput = (int)CauseInput.Input;
                searchModel.DateClassification = DateClassification.BackToGarageDate;
                searchModel.ChooseCause = ChooseCause.AllOfCause;
                searchModel.OutputSetting = OutputInstruction.Preview;
                searchModel.ItemOutOfRange = ItemOutOfRange.BothOfItem;
            }
            saleOffice.Insert(0, null);
            salePersonInCharge.Insert(0, null);
            editFormContext = new EditContext(searchModel);
        }

        protected async void ClearFormSeach()
        {
            await _loading.ShowAsync();
            ActiveV = (int)ViewMode.Medium;
            ActiveL = (int)LayoutMode.Save;
            searchModel.FormSetting = formSetting.FirstOrDefault();
            searchModel.OutputWithHeader = outputWithHeader.FirstOrDefault();
            searchModel.KukuriKbn = kukuriKbn.FirstOrDefault();
            searchModel.KugiriCharType = kugiriCharType.FirstOrDefault();
            searchModel.Range = (int)PlanningScope.OutOfRange;
            searchModel.CauseInput = (int)CauseInput.Input;
            searchModel.DateClassification = DateClassification.BackToGarageDate;
            searchModel.ChooseCause = ChooseCause.AllOfCause;
            searchModel.OutputSetting = OutputInstruction.Preview;
            searchModel.ItemOutOfRange = ItemOutOfRange.BothOfItem;
            searchModel.OutputStartDate = null;
            searchModel.OutputEndDate = null;
            searchModel.ReservationNumberStart = "";
            searchModel.ReservationNumberEnd = "";
            searchModel.SaleOffice = null;
            searchModel.SalePersonInCharge = null;
            editFormContext.Validate();
            await _filterConditionService.DeleteCustomFilerCondition(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, 0, FormFilterName.FaresUpperAndLowerLimits);
            await OnSearch(false);
            await _loading.HideAsync();
        }

        protected async Task OnChangePage(int page)
        {
            currentPage = page;
            await OnSearch(false);
        }

        protected async Task OnRowDoubleClick(FaresUpperAndLowerLimitGrid item)
        {
            try
            {
                if (ulong.Parse(item.GridBillingAmount, NumberStyles.AllowThousands, new CultureInfo("en-au")) > ulong.Parse(item.GridActualMaxAmount, NumberStyles.AllowThousands)
                    || ulong.Parse(item.GridBillingAmount, NumberStyles.AllowThousands, new CultureInfo("en-au")) < ulong.Parse(item.GridActualMinAmount, NumberStyles.AllowThousands, new CultureInfo("en-au")))
                {
                    faresUpperAndLowerLimitItemSelect = item;
                    var tkdMaxMinFareFeeCause = new TkdMaxMinFareFeeCauseParam
                    {
                        UkeNo = $"{new ClaimModel().TenantID.ToString("D5")}{faresUpperAndLowerLimitItemSelect.GridReservationNumber}",
                        UnkRen = faresUpperAndLowerLimitItemSelect.UnkRenGrid,
                        BunkRen = faresUpperAndLowerLimitItemSelect.BunkRenGrid,
                        TeiDanNo = faresUpperAndLowerLimitItemSelect.TeiDanNoGrid,
                    };
                    var causeItem = await _service.GetFirstOrDefaultTKDMaxMinFareFeeCause(tkdMaxMinFareFeeCause);
                    if (causeItem != null)
                    {
                        causeReason = causeItem?.UpperLowerCauseKbn ?? 1;
                        otherReasonInput = causeItem?.OtherCauseDetail;
                    }
                    else
                    {
                        causeReason = 1;
                        otherReasonInput = "";
                    }
                    lstCauses = await _service.GetCauseCombobox();
                    PopupVisible = true;
                    IndexItem = item.GridNo;
                }
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                _errorModalService.HandleError(ex);
            }
        }

        protected async Task OnTogglePopup(bool isDelete)
        {
            try
            {
                if (isDelete)
                {
                    PopupVisible = false;
                    await OnSaveOrUpdateCause(false);
                    await TogglePopup.InvokeAsync(new MouseEventArgs() { Type = "search" });
                }
                PopupDeleteVisible = !PopupDeleteVisible;
            }
            catch (Exception ex)
            {
                _errorModalService.HandleError(ex);
            }
        }

        protected async Task OnSaveOrUpdateCause(bool isSave = true)
        {
            try
            {
                await _loading.ShowAsync();
                PopupVisible = false;
                var causeItem = new TkdMaxMinFareFeeCause
                {
                    UkeNo = $"{new ClaimModel().TenantID.ToString("D5")}{faresUpperAndLowerLimitItemSelect.GridReservationNumber}",
                    UnkRen = faresUpperAndLowerLimitItemSelect.UnkRenGrid,
                    TeiDanNo = faresUpperAndLowerLimitItemSelect.TeiDanNoGrid,
                    BunkRen = faresUpperAndLowerLimitItemSelect.BunkRenGrid,
                    SyaSyuRen = faresUpperAndLowerLimitItemSelect.SyaSyuRenGrid,
                    UpperLowerCauseKbn = causeReason,
                    OtherCauseDetail = otherReasonInput,
                    SiyoKbn = CommonConstants.SiyoKbn,
                    UpdYmd = DateTime.Now.ToString(Formats.yyyyMMdd),
                    UpdTime = DateTime.Now.ToString(Formats.HHmmss),
                    UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq,
                    UpdPrgId = Common.UpdPrgId
                };
                await _service.SaveOrUpdateCause(causeItem, isSave);
                await OnSearch(false);
                await _loading.HideAsync();
            }
            catch (Exception ex)
            {
                _errorModalService.HandleError(ex);
            }
        }

        /// <summary>
        /// Caculate to show 時間（時間）
        /// </summary>
        /// <param name="time1">Time1</param>
        /// <param name="time2">Time1</param>
        /// <returns></returns>
        private Tuple<string, string> CaculateTime(string time1, string time2)
        {
            var hours = Convert.ToInt32(time1.Substring(0, 2)) + Convert.ToInt32(time2.Substring(0, 2));
            var minutes = Convert.ToInt32(time1.Substring(1, 2)) + Convert.ToInt32(time2.Substring(1, 2));
            var sumHours = (Convert.ToInt32(hours) + (Convert.ToInt32(minutes) / 60)).ToString();
            var sumMinutes = (Convert.ToInt32(minutes) % 60).ToString();
            if (sumHours.Length == 1)
                sumHours = sumHours.PadLeft(2, '0');
            if (sumMinutes.Length == 1)
                sumMinutes = sumHours.PadLeft(2, '0');
            return new Tuple<string, string>(sumHours, sumMinutes);
        }
    }
}
