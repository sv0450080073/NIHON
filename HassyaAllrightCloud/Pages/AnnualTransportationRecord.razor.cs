using HassyaAllrightCloud.Commons.Constants;
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
    public class AnnualTransportationRecordBase : ComponentBase
    {
        [Inject]
        protected IJSRuntime _jSRuntime { get; set; }
        [Inject]
        protected IStringLocalizer<AnnualTransportationRecord> _lang { get; set; }
        [Inject]
        protected IMonthlyTransportationAnnualService _serviceAnnual { get; set; }
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
        protected AnnualTransportationRecordFormSearch searchModel { get; set; }
        protected EditContext editFormContext { get; set; }
        protected List<CompanyItem> companys = new List<CompanyItem>();
        protected List<EigyoItem> eigyoFrom = new List<EigyoItem>();
        protected List<EigyoItem> eigyoTo = new List<EigyoItem>();
        protected List<ShippingItem> shippingFrom = new List<ShippingItem>();
        protected List<ShippingItem> shippingTo = new List<ShippingItem>();
        public bool isLoading { get; set; }
        public Dictionary<string, string> LangDic = new Dictionary<string, string>();
        protected SearchParam searchParams { get; set; }
        protected bool IsShow { get; set; } = false;
        protected bool isFirstRender { get; set; } = true;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await ToggleLoading(true);
                var commonValue = await _service.GetCommonListItems();
                var dataLang = _lang.GetAllStrings();
                companys = commonValue.Companys.ToList();
                eigyoFrom = commonValue.Eigyos.ToList();
                eigyoFrom.Insert(0, null);
                eigyoTo = commonValue.Eigyos.ToList();
                eigyoTo.Insert(0, null);
                shippingFrom = commonValue.Shippings.ToList();
                shippingFrom.Insert(0, null);
                shippingTo = commonValue.Shippings.ToList();
                shippingTo.Insert(0, null);
                //init value
                searchModel = new AnnualTransportationRecordFormSearch();
                var filterValues = _filterConditionService.GetFilterCondition(FormFilterName.AnnualTransportationRecord, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq).Result;
                if (filterValues.Any())
                {
                    searchModel.OutputInstructionMode = filterValues.FirstOrDefault(x => x.ItemNm == nameof(AnnualTransportationRecordFormSearch.OutputInstructionMode)) == null ? 1 : int.Parse(filterValues.FirstOrDefault(x => x.ItemNm == nameof(AnnualTransportationRecordFormSearch.OutputInstructionMode)).JoInput);
                    searchModel.ProcessingDateFrom = DateTime.ParseExact(filterValues.FirstOrDefault(x => x.ItemNm == nameof(AnnualTransportationRecordFormSearch.ProcessingDateFrom)).JoInput, Formats.yyyyMMdd, null);
                    searchModel.ProcessingDateTo = DateTime.ParseExact(filterValues.FirstOrDefault(x => x.ItemNm == nameof(AnnualTransportationRecordFormSearch.ProcessingDateTo)).JoInput, Formats.yyyyMMdd, null);
                    searchModel.EigyoFrom = commonValue.Eigyos.FirstOrDefault(x => x.EigyoCdSeq == (filterValues.FirstOrDefault(x => x.ItemNm == nameof(AnnualTransportationRecordFormSearch.EigyoFrom)) == null ? 1 : int.Parse(filterValues.FirstOrDefault(x => x.ItemNm == nameof(AnnualTransportationRecordFormSearch.EigyoFrom)).JoInput)));
                    searchModel.EigyoTo = commonValue.Eigyos.FirstOrDefault(x => x.EigyoCdSeq == (filterValues.FirstOrDefault(x => x.ItemNm == nameof(AnnualTransportationRecordFormSearch.EigyoTo)) == null ? 0 : int.Parse(filterValues.FirstOrDefault(x => x.ItemNm == nameof(AnnualTransportationRecordFormSearch.EigyoTo)).JoInput)));
                    searchModel.ShippingFrom = commonValue.Shippings.FirstOrDefault(x => x.CodeKbnSeq == (filterValues.FirstOrDefault(x => x.ItemNm == nameof(AnnualTransportationRecordFormSearch.ShippingFrom)) == null ? 1 : int.Parse(filterValues.FirstOrDefault(x => x.ItemNm == nameof(AnnualTransportationRecordFormSearch.ShippingFrom)).JoInput)));
                    searchModel.ShippingTo = commonValue.Shippings.FirstOrDefault(x => x.CodeKbnSeq == (filterValues.FirstOrDefault(x => x.ItemNm == nameof(AnnualTransportationRecordFormSearch.ShippingTo)) == null ? 0 : int.Parse(filterValues.FirstOrDefault(x => x.ItemNm == nameof(AnnualTransportationRecordFormSearch.ShippingTo)).JoInput)));
                }
                else
                    searchModel.OutputInstructionMode = (int)OutputInstruction.Preview;
                searchModel.Company = companys.FirstOrDefault();

                SetParamModel();
                LangDic = dataLang.ToDictionary(l => l.Name, l => l.Value);
                editFormContext = new EditContext(searchModel);
                await ToggleLoading(false);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        private void SetParamModel()
        {
            var processingDateFrom = searchModel?.ProcessingDateFrom.ToString(Formats.yyyyMM);
            var processingDateTo = searchModel?.ProcessingDateTo.ToString(Formats.yyyyMM);
            searchParams = new SearchParam
            {
                CompnyCd = searchModel?.Company?.CompanyCd ?? 0,
                StrDate = processingDateFrom,
                EndDate = processingDateTo,
                StrEigyoCd = searchModel?.EigyoFrom?.EigyoCd ?? 0,
                EndEigyoCd = searchModel?.EigyoTo?.EigyoCd ?? 0,
                StrUnsouKbn = searchModel?.ShippingFrom?.CodeKbn ?? 0,
                EndUnsouKbn = searchModel?.ShippingTo?.CodeKbn ?? 0,
                TenantCdSeq = new ClaimModel().TenantID,
                CompanyName = searchModel?.Company?.RyakuNm
            };
        }

        protected async Task BtnStart()
        {
            try
            {
                if (editFormContext.Validate())
                {
                    await ToggleLoading(true);

                    var keyValueFilterPairs = _generateFilterValueDictionaryService.GenerateForAnnualTransportationRecord(searchModel).Result;
                    if (editFormContext.Validate())
                        await _filterConditionService.SaveFilterCondtion(keyValueFilterPairs, FormFilterName.AnnualTransportationRecord, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq);

                    var result = await _serviceAnnual.GetJitHouAnnualDataReport(searchParams);
                    if (!result.Any())
                        IsShow = true;
                    else
                    {
                        if (searchModel?.OutputInstructionMode == (int)OutputInstruction.Preview)
                        {
                            var searchString = EncryptHelper.EncryptToUrl(searchParams);
                            await _jSRuntime.InvokeVoidAsync("openNewUrlInNewTab", string.Format("jitHouannualreportpreview?searchString={0}", searchString));
                        }
                        else
                        {
                            var report = new Reports.JitHouAnnualReport();
                            report.DataSource = result;
                            await new TaskFactory().StartNew(() =>
                            {
                                report.CreateDocument();
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    report.ExportToPdf(ms);
                                    byte[] exportedFileBytes = ms.ToArray();
                                    string myExportString = Convert.ToBase64String(exportedFileBytes);
                                    _jSRuntime.InvokeVoidAsync("downloadFileClientSide", myExportString, "pdf", "年間輸送実績");
                                }
                            });
                        }
                        IsShow = false;
                    }
                }
                await InvokeAsync(StateHasChanged);
                await ToggleLoading(false);
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
                if (editFormContext.Validate())
                    SetValue(propertyName, value);
                await InvokeAsync(StateHasChanged);
                await ToggleLoading(false);
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
            var processingDateFrom = searchModel?.ProcessingDateFrom.ToString(Formats.yyyyMM);
            var processingDateTo = searchModel?.ProcessingDateTo.ToString(Formats.yyyyMM);
            SetParamModel();
            await InvokeAsync(StateHasChanged);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (isFirstRender)
            {
                await _jsRuntime.InvokeVoidAsync("focus", ".focus-first-item");
                await _jsRuntime.InvokeVoidAsync("EnterTab");
                isFirstRender = false;
            }
        }

        protected async void ClearFormSeach()
        {
            await _filterConditionService.DeleteCustomFilerCondition(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, 0, FormFilterName.AnnualTransportationRecord);
            await OnInitializedAsync();
        }

        private async Task ToggleLoading(bool value)
        {
            isLoading = value;
            await InvokeAsync(StateHasChanged);
            await Task.Delay(100);
        }
    }
}
