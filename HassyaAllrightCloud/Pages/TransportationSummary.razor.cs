using HassyaAllrightCloud.Commons;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Pages
{
    public class TransportationSummaryBase : ComponentBase
    {
        [Inject] protected IStringLocalizer<TransportationSummary> _lang { get; set; }
        [Inject] protected ITransportationSummaryService _service { get; set; }
        [Inject] private IJSRuntime _jSRuntime { get; set; }
        [Inject] private IErrorHandlerService _errService { get; set; }
        [Inject] protected IFilterCondition _filterService { get; set; }
        [Inject] private ILoadingService _loadingService { get; set; }

        protected TransportationSummarySearchModel searchModel { get; set; }
        protected EditContext editFormContext { get; set; }
        protected int gridSizeClass { get; set; } = (int)ViewMode.Medium;
        protected IEnumerable<TransportationSummaryItem> summaryList = new List<TransportationSummaryItem>();
        protected IEnumerable<EigyoListItem> eigyoList = new List<EigyoListItem>();
        protected IEnumerable<CompanyListItem> companyList = new List<CompanyListItem>();
        protected bool isValidCompanyCode { get; set; } = true;
        protected Dictionary<string, string> LangDic = new Dictionary<string, string>();
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

        /// <summary>
        /// Trigger when component init
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                await _loadingService.ShowAsync();
                var dataLang = _lang.GetAllStrings();
                LangDic = dataLang.ToDictionary(l => l.Name, l => l.Value);

                searchModel = new TransportationSummarySearchModel();
                searchModel.ProcessingDate = DateTime.Now;

                companyList = await _service.GetCompanyListItems(new ClaimModel().CompanyID);
                searchModel.Company = companyList.FirstOrDefault();

                eigyoList = await _service.GetEigyoListItems(new ClaimModel().CompanyID, new ClaimModel().TenantID);

                searchModel = await BuildSearchModel(searchModel);

                editFormContext = new EditContext(searchModel);

                await GetTransportationSummary(true);
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
            finally
            {
                await _loadingService.HideAsync();
            }
        }

        /// <summary>
        /// Get Transportation summary
        /// </summary>
        /// <returns></returns>
        protected async Task Process()
        {
            try
            {
                await _loadingService.ShowAsync();

                await GetTransportationSummary(false);
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
            finally
            {
                await _loadingService.HideAsync();
            }
        }

        private async Task GetTransportationSummary(bool getOnly)
        {
            if (editFormContext.Validate())
            {
                summaryList = await _service.GetTableRowModels(searchModel, getOnly);
                if (summaryList == null)
                    summaryList = new List<TransportationSummaryItem>();
                else
                    await SaveSearchModel(searchModel);
            }
        }

        /// <summary>
        /// Trigger when company changed
        /// </summary>
        /// <param name="selected"></param>
        /// <returns></returns>
        protected async Task CompanyChanged(CompanyListItem selected)
        {
            try
            {
                await _loadingService.ShowAsync();
                if (selected != null && await _service.IsValidCompanyCode(selected.CompanyCdSeq))
                {
                    isValidCompanyCode = true;
                    searchModel.Company = selected;
                }
                else
                {
                    isValidCompanyCode = false;
                    searchModel.Company = null;
                }
                await GetTransportationSummary(true);
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
            finally
            {
                await _loadingService.HideAsync();
            }
        }

        /// <summary>
        /// Trigger when Eigyo changed
        /// </summary>
        /// <param name="item"></param>
        /// <param name="isEigyoFromChanged"></param>
        /// <returns></returns>
        protected async Task EigyoChanged(EigyoListItem item, bool isEigyoFromChanged)
        {
            try
            {
                await _loadingService.ShowAsync();
                if (isEigyoFromChanged)
                    searchModel.EigyoFrom = item;
                else
                    searchModel.EigyoTo = item;
                await GetTransportationSummary(true);
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
            finally
            {
                await _loadingService.HideAsync();
            }
        }
        protected void TriggerTabChange()
        {
            StateHasChanged();
        }

        private async Task<TransportationSummarySearchModel> BuildSearchModel(TransportationSummarySearchModel model)
        {
            var filters = await _filterService.GetFilterCondition(FormFilterName.TransportationSummary, 0, new ClaimModel().SyainCdSeq);
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
                        case nameof(TransportationSummarySearchModel.Company):
                            var comCdSeq = int.Parse(item.JoInput);
                            if (comCdSeq != 0)
                            {
                                var selectedCom = companyList.FirstOrDefault(e => e.CompanyCdSeq == comCdSeq);
                                if (selectedCom != null)
                                    propertyInfo.SetValue(model, selectedCom);
                            }
                            break;
                        case nameof(TransportationSummarySearchModel.EigyoFrom):
                        case nameof(TransportationSummarySearchModel.EigyoTo):
                            var eigyoSeq = int.Parse(item.JoInput);
                            if (eigyoSeq != 0)
                            {
                                var selectedEigyo = eigyoList.FirstOrDefault(e => e.EigyoCdSeq == eigyoSeq);
                                if (selectedEigyo != null)
                                    propertyInfo.SetValue(model, selectedEigyo);
                            }
                            break;
                        case nameof(TransportationSummarySearchModel.ProcessingDate):
                            propertyInfo.SetValue(model, DateTime.ParseExact(item.JoInput, DateTimeFormat.yyyyMMdd, CultureInfo.InvariantCulture));
                            break;
                    }
                }
            }
            return model;
        }

        private async Task SaveSearchModel(TransportationSummarySearchModel model)
        {
            var filers = GetSearchFormModel(model);
            await _filterService.SaveFilterCondtion(filers, FormFilterName.TransportationSummary, 0, new ClaimModel().SyainCdSeq);
        }

        private Dictionary<string, string> GetSearchFormModel(TransportationSummarySearchModel model)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            if (model == null) return result;
            result.Add(nameof(TransportationSummarySearchModel.ProcessingDate), model.ProcessingDate.ToString(DateTimeFormat.yyyyMMdd));
            result.Add(nameof(TransportationSummarySearchModel.Company), model.Company?.CompanyCdSeq.ToString() ?? string.Empty);

            result.Add(nameof(TransportationSummarySearchModel.EigyoFrom), model.EigyoFrom?.EigyoCdSeq.ToString() ?? string.Empty);
            result.Add(nameof(TransportationSummarySearchModel.EigyoTo), model.EigyoTo?.EigyoCdSeq.ToString() ?? string.Empty);

            return result;
        }

        protected async Task DateChanged(DateTime date)
        {
            try
            {
                await _loadingService.ShowAsync();
                searchModel.ProcessingDate = date;
                await GetTransportationSummary(true);
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
            finally
            {
                await _loadingService.HideAsync();
            }
        }
    }
}
