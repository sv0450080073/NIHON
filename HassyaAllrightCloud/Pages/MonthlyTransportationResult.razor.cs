using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Pages
{
    public class MonthlyTransportationResultBase : ComponentBase
    {
        [Inject]
        protected IJSRuntime _jSRuntime { get; set; }
        [Inject]
        protected IStringLocalizer<MonthlyTransportationResult> _lang { get; set; }
        [Inject]
        protected IMonthlyTransportationService _service { get; set; }
        [Inject]
        protected NavigationManager _navigationManager { get; set; }
        [Inject]
        protected IJSRuntime _jsRuntime { get; set; }
        [Inject]
        protected IErrorHandlerService errorModalService { get; set; }
        [Inject]
        protected IGenerateFilterValueDictionary _generateFilterValueDictionaryService { get; set; }
        [Inject]
        protected IFilterCondition _filterConditionService { get; set; }
        [Inject]
        protected IReportLayoutSettingService _reportLayoutSettingService { get; set; }
        protected MonthlyTransportationFormSearch searchModel { get; set; }
        protected EditContext editFormContext { get; set; }
        protected List<CompanyItem> companys = new List<CompanyItem>();
        protected List<EigyoItem> eigyoFrom = new List<EigyoItem>();
        protected List<EigyoItem> eigyoTo = new List<EigyoItem>();
        protected List<ShippingItem> shippingFrom = new List<ShippingItem>();
        protected List<ShippingItem> shippingTo = new List<ShippingItem>();
        public List<string> ErrorMessage = new List<string>();
        public Dictionary<string, string> LangDic = new Dictionary<string, string>();
        protected bool IsReadOnly { get; set; } = false;
        protected SearchParam searchParams { get; set; }
        public bool isLoading { get; set; }
        protected bool IsShow { get; set; } = false;
        protected bool isFirstRender { get; set; } = true;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var commonValue = await _service.GetCommonListItems();
                var dataLang = _lang.GetAllStrings();
                IsReadOnly = false;
                ErrorMessage = new List<string>();
                companys = commonValue.Companys.ToList();
                eigyoFrom = commonValue.Eigyos.ToList();
                eigyoFrom.Insert(0, null);
                eigyoTo = commonValue.Eigyos.ToList();
                eigyoTo.Insert(0, null);
                shippingFrom = commonValue.Shippings.ToList();
                shippingFrom.Insert(0, null);
                shippingTo = commonValue.Shippings.ToList();
                shippingTo.Insert(0, null);
                searchModel = new MonthlyTransportationFormSearch();
                //init value
                var filterValues = _filterConditionService.GetFilterCondition(FormFilterName.MonthlyTransportationResult, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq).Result;
                if (filterValues.Any())
                {
                    searchModel.OutputInstructionMode = filterValues.FirstOrDefault(x => x.ItemNm == nameof(MonthlyTransportationFormSearch.OutputInstructionMode)) == null ? 1 : int.Parse(filterValues.FirstOrDefault(x => x.ItemNm == nameof(MonthlyTransportationFormSearch.OutputInstructionMode)).JoInput);
                    searchModel.ProcessingDate = DateTime.ParseExact(filterValues.FirstOrDefault(x => x.ItemNm == nameof(MonthlyTransportationFormSearch.ProcessingDate)).JoInput, Formats.yyyyMMdd, null);
                    searchModel.EigyoFrom = commonValue.Eigyos.FirstOrDefault(x => x.EigyoCdSeq == (filterValues.FirstOrDefault(x => x.ItemNm == nameof(MonthlyTransportationFormSearch.EigyoFrom)) == null ? 1 : int.Parse(filterValues.FirstOrDefault(x => x.ItemNm == nameof(MonthlyTransportationFormSearch.EigyoFrom)).JoInput)));
                    searchModel.EigyoTo = commonValue.Eigyos.FirstOrDefault(x => x.EigyoCdSeq == (filterValues.FirstOrDefault(x => x.ItemNm == nameof(MonthlyTransportationFormSearch.EigyoTo)) == null ? 0 : int.Parse(filterValues.FirstOrDefault(x => x.ItemNm == nameof(MonthlyTransportationFormSearch.EigyoTo)).JoInput)));
                    searchModel.ShippingFrom = commonValue.Shippings.FirstOrDefault(x => x.CodeKbnSeq == (filterValues.FirstOrDefault(x => x.ItemNm == nameof(MonthlyTransportationFormSearch.ShippingFrom)) == null ? 1 : int.Parse(filterValues.FirstOrDefault(x => x.ItemNm == nameof(MonthlyTransportationFormSearch.ShippingFrom)).JoInput)));
                    searchModel.ShippingTo = commonValue.Shippings.FirstOrDefault(x => x.CodeKbnSeq == (filterValues.FirstOrDefault(x => x.ItemNm == nameof(MonthlyTransportationFormSearch.ShippingTo)) == null ? 0 : int.Parse(filterValues.FirstOrDefault(x => x.ItemNm == nameof(MonthlyTransportationFormSearch.ShippingTo)).JoInput)));
                    searchModel.Company = companys.FirstOrDefault();
                }
                else
                {
                    searchModel.OutputInstructionMode = (int)OutputInstruction.Show;
                    searchModel.ProcessingDate = DateTime.Now;
                    searchModel.Company = companys.FirstOrDefault();
                    searchModel.ShippingFrom = commonValue.Shippings.FirstOrDefault();
                    searchModel.EigyoFrom = commonValue.Eigyos.FirstOrDefault();
                }
                PrepareInit();
                CheckDisable(searchModel.OutputInstructionMode);
                editFormContext = new EditContext(searchModel);
                LangDic = dataLang.ToDictionary(l => l.Name, l => l.Value);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected async Task BtnStart()
        {
            try
            {
                if (editFormContext.Validate())
                {
                    var keyValueFilterPairs = _generateFilterValueDictionaryService.GenerateForMonthlyTransportationResult(searchModel).Result;
                    if (editFormContext.Validate())
                        await _filterConditionService.SaveFilterCondtion(keyValueFilterPairs, FormFilterName.MonthlyTransportationResult, 0, new ClaimModel().SyainCdSeq);

                    if (searchModel?.OutputInstructionMode == (int)OutputInstruction.Show)
                    {
                        var searchString = EncryptHelper.EncryptToUrl(searchParams);
                        _navigationManager.NavigateTo("monthlyTransportationResultInput?searchString=" + searchString);
                    }
                    else
                    {
                        var result = await _service.GetJitHouDataReport(searchParams);
                        IsShow = result.Any();
                        if (!IsShow)
                            IsShow = true;
                        else
                        {
                            if (searchModel?.OutputInstructionMode == (int)OutputInstruction.Preview)
                            {
                                await OnPreviewReport();
                            }
                            else
                            {
                                var report = new Reports.JitHouReport();
                                report.DataSource = result;
                                await new System.Threading.Tasks.TaskFactory().StartNew(() =>
                                {
                                    report.CreateDocument();
                                    using (MemoryStream ms = new MemoryStream())
                                    {
                                        report.ExportToPdf(ms);
                                        byte[] exportedFileBytes = ms.ToArray();
                                        string myExportString = Convert.ToBase64String(exportedFileBytes);
                                        _jSRuntime.InvokeVoidAsync("downloadFileClientSide", myExportString, "pdf", "月別輸送実績");
                                    }
                                });
                            }
                            IsShow = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        private async Task OnPreviewReport()
        {
            try
            {
                var searchString = EncryptHelper.EncryptToUrl(searchParams);
                await _jSRuntime.InvokeVoidAsync("openNewUrlInNewTab", string.Format("jithoureportpreview?searchString={0}", searchString));
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected async void UpdateFormValue(string propertyName, dynamic value)
        {
            try
            {
                await ToggleLoading(true);
                SetValue(propertyName, value);
                CheckDisable(searchModel.OutputInstructionMode);
                await ToggleLoading(false);
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        private async void SetValue(string propertyName, dynamic value)
        {
            var propertyInfo = searchModel.GetType().GetProperty(propertyName);
            propertyInfo.SetValue(searchModel, value, null);
            PrepareInit();
            await InvokeAsync(StateHasChanged);
        }

        private void PrepareInit()
        {
            var processingDate = searchModel?.ProcessingDate.ToString(Formats.yyyyMM);
            searchParams = new SearchParam
            {
                CompnyCd = searchModel?.Company?.CompanyCd ?? 0,
                CompanyCdSeq = searchModel?.Company?.CompanyCdSeq ?? 0,
                StrDate = processingDate,
                StrEigyoCd = searchModel?.EigyoFrom?.EigyoCd ?? 0,
                EndEigyoCd = searchModel?.EigyoTo?.EigyoCd ?? 0,
                StrUnsouKbn = searchModel?.ShippingFrom?.CodeKbn ?? 0,
                EndUnsouKbn = searchModel?.ShippingTo?.CodeKbn ?? 0,
                TenantCdSeq = new ClaimModel().TenantID,
                CompanyName = searchModel?.Company?.RyakuNm,
                StrEigyoCdSeq = searchModel.EigyoFrom.EigyoCdSeq,
            };
        }

        protected void CheckDisable(int mode)
        {
            if (mode == (int)OutputInstruction.Show)
            {
                IsReadOnly = true;
                searchModel.EigyoTo = null;
                searchModel.ShippingTo = null;
                ErrorMessage = new List<string>();
            }
            else
                IsReadOnly = false;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (isFirstRender)
            {
                await _jsRuntime.InvokeVoidAsync("focus", ".focus-first-item");
                await _jsRuntime.InvokeVoidAsync("EnterTab", ".monthlytransportation-result");
                isFirstRender = false;
            }
        }

        protected async void ClearFormSeach()
        {
            await ToggleLoading(true);
            await _filterConditionService.DeleteCustomFilerCondition(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, 0, FormFilterName.MonthlyTransportationResult);
            await OnInitializedAsync();
            await ToggleLoading(false);
        }
        private async Task ToggleLoading(bool value)
        {
            isLoading = value;
            await InvokeAsync(StateHasChanged);
            await Task.Delay(100);
        }
    }
}
