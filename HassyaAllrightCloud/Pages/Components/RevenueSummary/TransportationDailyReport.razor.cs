using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.IService;
using HassyaAllrightCloud.Reports.ReportTemplate.TransportationRevenue;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Pages.Components.RevenueSummary
{
    public class TransportationDailyReportBase : ComponentBase
    {
        [Inject] protected IStringLocalizer<TransportationDailyReport> _lang { get; set; }
        [Inject] protected IStringLocalizer<Pages.RevenueSummary> _baseLang { get; set; }
        [Inject] protected IJSRuntime _jSRuntime { get; set; }
        [Inject] protected IRevenueSummaryService _revenueSummaryService { get; set; }
        [Inject] protected ILoadingService _loading { get; set; }
        [Inject] private IErrorHandlerService _errService { get; set; }
        [Parameter] public TransportationRevenueSearchModel Model { get; set; }
        [Parameter] public int GridSize { get; set; }
        [Parameter] public bool IsLoadOnInit { get; set; }
        [Parameter] public EventCallback<bool> DataChanged { get; set; }
        protected List<string> uriYmdList { get; set; } = new List<string>();
        protected DailyRevenueSearchModel searchModel { get; set; } = new DailyRevenueSearchModel();
        protected List<DailyRevenueItem> dailyRevenueItems { get; set; } = new List<DailyRevenueItem>();
        protected List<DailyRevenueItem> actualRevenueItems { get; set; } = new List<DailyRevenueItem>();
        protected List<EigyoListItem> eigyoListItems { get; set; } = new List<EigyoListItem>();
        protected List<SummaryResult> summaryResult { get; set; } = new List<SummaryResult>();
        private const int DefaultPage = 0;
        protected byte itemPerPage { get; set; } = Common.DefaultPageSize;
        protected int currentPage { get; set; } = DefaultPage;
        protected Pagination paging;
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await _jSRuntime.InvokeVoidAsync("initSelectableRow");
        }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                if (IsLoadOnInit)
                {
                    await _loading.ShowAsync();
                    ClearCurrentData();
                    await InitControls(Model);
                    if (eigyoListItems.Any() && uriYmdList.Any())
                        await GetResultGridData(searchModel);
                    await InvokeAsync(StateHasChanged);
                }
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
            finally
            {
                await _loading.HideAsync();
            }
        }

        public async Task<bool> Reload(TransportationRevenueSearchModel model)
        {
            try
            {
                await _loading.ShowAsync();
                Model = model;

                ClearCurrentData();

                await InitControls(Model);
                if (eigyoListItems.Any() && uriYmdList.Any())
                    await GetResultGridData(searchModel);
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
            finally
            {
                await _loading.HideAsync();
            }
            return dailyRevenueItems.Any();
        }

        private void ClearCurrentData()
        {
            if (eigyoListItems != null) eigyoListItems.Clear();
            if (uriYmdList != null) uriYmdList.Clear();
            if (dailyRevenueItems != null) dailyRevenueItems.Clear();
            if (summaryResult != null) summaryResult.Clear();
            if (actualRevenueItems != null) actualRevenueItems.Clear();
            currentPage = DefaultPage;
            itemPerPage = Common.DefaultPageSize;

            StateHasChanged();
        }

        private async Task InitControls(TransportationRevenueSearchModel model)
        {
            searchModel.RevenueSearchModel = model;

            eigyoListItems = await _revenueSummaryService.GetEigyoListForDailyRevenueReport(model);
            await DataChanged.InvokeAsync(eigyoListItems.Any());
            searchModel.Eigyo = eigyoListItems.FirstOrDefault();

            if (eigyoListItems.Any())
            {
                uriYmdList = await _revenueSummaryService.GetUriYmdForDailyRevenueReport(searchModel);
                searchModel.UriYmd = uriYmdList.FirstOrDefault();
            }
            else
            {
                uriYmdList.Clear();
            }

            var options = _revenueSummaryService.GetTesuInKbnItems();
            searchModel.TesukomiKbn = options.Find(o => o.Value == (int)model.TesuInKbn).Text;
        }

        private async Task GetResultGridData(DailyRevenueSearchModel model)
        {
            var result = await _revenueSummaryService.GetDailyRevenueData(model);
            dailyRevenueItems = result.DailyRevenueItems;
            summaryResult = result.SummaryResult;
            RebindGridData();
        }

        private void RebindGridData()
        {
            actualRevenueItems = dailyRevenueItems.Skip(currentPage * itemPerPage).Take(itemPerPage).ToList();
            StateHasChanged();
        }

        private async Task GetUriYmdListAndRebindGrid()
        {
            uriYmdList = await _revenueSummaryService.GetUriYmdForDailyRevenueReport(searchModel);
            searchModel.UriYmd = uriYmdList.FirstOrDefault();
            if (searchModel.UriYmd == null)
            {
                if (uriYmdList != null) uriYmdList.Clear();
                if (dailyRevenueItems != null) dailyRevenueItems.Clear();
                if (summaryResult != null) summaryResult.Clear();
                if (actualRevenueItems != null) actualRevenueItems.Clear();
            }
            else
                await GetResultGridData(searchModel);
        }

        protected async Task EigyoListItemsChanged(EigyoListItem val, bool? isPre = null)
        {
            try
            {
                await _loading.ShowAsync();
                if (isPre == null)
                {
                    searchModel.Eigyo = val;
                    await GetUriYmdListAndRebindGrid();
                }
                else
                {
                    if (isPre == true)
                    {
                        var preIndex = GetPreNextIndex(true, eigyoListItems, searchModel.Eigyo);
                        if (preIndex != -1)
                        {
                            searchModel.Eigyo = eigyoListItems[preIndex];
                            await GetUriYmdListAndRebindGrid();
                        }
                    }
                    else
                    {
                        var nextIndex = GetPreNextIndex(false, eigyoListItems, searchModel.Eigyo);
                        if (nextIndex != -1)
                        {
                            searchModel.Eigyo = eigyoListItems[nextIndex];
                            await GetUriYmdListAndRebindGrid();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
            finally
            {
                await _loading.HideAsync();
                StateHasChanged();
            }
        }
        private int GetPreNextIndex<T>(bool isPre, List<T> list, T val)
        {
            var index = list.IndexOf(val);

            return isPre ? (index > 0 ? index - 1 : -1) : (index < list.Count - 1 ? index + 1 : -1);
        }
        protected async Task UriYmdChanged(string val, bool? isPre = null)
        {
            try
            {
                await _loading.ShowAsync();
                if (isPre == null)
                {
                    searchModel.UriYmd = val;
                    await GetResultGridData(searchModel);
                }
                else
                {
                    if (isPre == true)
                    {
                        var preIndex = GetPreNextIndex(true, uriYmdList, searchModel.UriYmd);
                        if (preIndex != -1)
                        {
                            searchModel.UriYmd = uriYmdList[preIndex];
                            await GetResultGridData(searchModel);
                        }
                    }
                    else
                    {
                        var nextIndex = GetPreNextIndex(false, uriYmdList, searchModel.UriYmd);
                        if (nextIndex != -1)
                        {
                            searchModel.UriYmd = uriYmdList[nextIndex];
                            await GetResultGridData(searchModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
            finally
            {
                await _loading.HideAsync();
                StateHasChanged();
            }
        }

        protected void OnChangePage(int page)
        {
            try
            {
                currentPage = page;
                RebindGridData();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected void OnChangeItemPerPage(byte _itemPerPage)
        {
            try
            {
                itemPerPage = _itemPerPage;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }
    }
}
