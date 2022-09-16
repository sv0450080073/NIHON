using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using SharedLibraries.UI.Models;

namespace HassyaAllrightCloud.Pages.Components.RevenueSummary
{
    public class TransportationMonthlyReportBase : ComponentBase
    {
        [Inject] protected IStringLocalizer<TransportationMonthlyReport> _lang { get; set; }
        [Inject] protected IStringLocalizer<Pages.RevenueSummary> _baseLang { get; set; }
        [Inject] protected IJSRuntime _jSRuntime { get; set; }
        [Inject] protected IRevenueSummaryService _revenueSummaryService { get; set; }
        [Inject] protected ILoadingService _loading { get; set; }
        [Inject] private IErrorHandlerService _errService { get; set; }
        [Parameter] public TransportationRevenueSearchModel Model { get; set; }
        [Parameter] public int GridSize { get; set; }
        [Parameter] public bool IsLoadOnInit { get; set; }
        [Parameter] public EventCallback<bool> DataChanged { get; set; }
        protected List<MonthlyRevenueItem> actualRevenueItems { get; set; } = new List<MonthlyRevenueItem>();
        private const int DefaultPage = 0;
        protected Pagination paging;
        protected byte itemPerPage { get; set; } = Common.DefaultPageSize;
        protected int currentPage { get; set; } = DefaultPage;
        protected MonthlyRevenueSearchModel searchModel { get; set; } = new MonthlyRevenueSearchModel();
        protected List<MonthlyRevenueItem> monthlyRevenueItems { get; set; } = new List<MonthlyRevenueItem>();
        protected List<EigyoListItem> eigyoListItems { get; set; } = new List<EigyoListItem>();
        protected List<SummaryResult> summaryResult { get; set; } = new List<SummaryResult>();
        protected HeaderTemplate Header;
        protected BodyTemplate Body;
        private const string WhiteSpaceCss = "white-space-pre-line";
        private const string VerticalAlignCss = "vertical-align-baseline";
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await _jSRuntime.InvokeVoidAsync("initSelectableRow");
        }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                InitTable();
                if (IsLoadOnInit)
                {
                    await _loading.ShowAsync();
                    ClearCurrentData();
                    await InitControls(Model);
                    if (eigyoListItems.Any())
                        await GetGridData(searchModel);
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

        private void InitTable()
        {
            Header = new HeaderTemplate()
            {
                Rows = new List<RowHeaderTemplate>()
                {
                    new RowHeaderTemplate()
                    {
                        Columns = new List<ColumnHeaderTemplate>()
                        {
                            new ColumnHeaderTemplate(){ ColName = _lang["no_col"], RowSpan = 2 },
                            new ColumnHeaderTemplate(){ ColName = _lang["sales_date_col"], RowSpan = 2 },
                            new ColumnHeaderTemplate(){ ColName = _lang["fares_including_tax_col"], RowSpan = 2 },
                            new ColumnHeaderTemplate(){ ColName = _lang["mercenary_name_col"], RowSpan = 2 },
                            new ColumnHeaderTemplate(){ ColName = $"{_lang["mercenary_col"]} \n {_lang["number_of_units_col"]}", RowSpan = 2, Width = 200, CssClass =  WhiteSpaceCss},
                            new ColumnHeaderTemplate(){ ColName = $"{_lang["mercenary_items_col"]} \n {_lang["total_col"]}", RowSpan = 2, CssClass = WhiteSpaceCss },
                            new ColumnHeaderTemplate(){ ColName = $"{_lang["car_hire_col"]} \n {_lang["total_col"]}", RowSpan = 2, CssClass = WhiteSpaceCss },
                            new ColumnHeaderTemplate(){ ColName = $"{_lang["own_item_col"]} \n {_lang["total_col"]}", RowSpan = 2, CssClass = WhiteSpaceCss },
                            new ColumnHeaderTemplate(){ ColName = $"{_lang["guide_fee_col"]} \n {_lang["total_col"]}", RowSpan = 2, CssClass = WhiteSpaceCss },
                            new ColumnHeaderTemplate(){ ColName = $"{_lang["other_incidentals_col"]} \n {_lang["total_col"]}", RowSpan = 2, CssClass = WhiteSpaceCss, Width = 120 },
                            new ColumnHeaderTemplate(){ ColName = $"{_lang["cancellation_charge_col"]} \n {_lang["total_col"]}", RowSpan = 2, CssClass = WhiteSpaceCss, Width = 140  },
                            new ColumnHeaderTemplate(){ ColName = _lang["fare_profit_loss_col"], RowSpan = 2 },

                            new ColumnHeaderTemplate(){ ColName = _lang["merchandise_item_col"], ColSpan = 3, Width = 300 },

                            new ColumnHeaderTemplate(){ ColName = _lang["car_hire_col1"], ColSpan = 3, Width = 300 },

                            new ColumnHeaderTemplate(){ ColName = _lang["company_items_col"], ColSpan = 4, Width = 400 },

                            new ColumnHeaderTemplate(){ ColName = _lang["guide_fee_col1"], ColSpan = 3, Width = 300 },

                            new ColumnHeaderTemplate(){ ColName = _lang["other_incidentals_col"], ColSpan = 3, Width = 300 },

                            new ColumnHeaderTemplate(){ ColName = _lang["cancellation_charge_col"], ColSpan = 2, Width = 200 }
                        }
                    },
                    new RowHeaderTemplate()
                    {
                        Columns = new List<ColumnHeaderTemplate>()
                        {
                            new ColumnHeaderTemplate(){ ColName = _lang["fare_col"] },
                            new ColumnHeaderTemplate(){ ColName = _lang["consumption_tax_col"] },
                            new ColumnHeaderTemplate(){ ColName = _lang["fee_amount_col"] },

                            new ColumnHeaderTemplate(){ ColName = _lang["amount_generated_col"] },
                            new ColumnHeaderTemplate(){ ColName = _lang["consumption_tax_col"] },
                            new ColumnHeaderTemplate(){ ColName = _lang["fee_amount_col"] },

                            new ColumnHeaderTemplate(){ ColName = _lang["fare_col"] },
                            new ColumnHeaderTemplate(){ ColName = _lang["number_of_units_col"] },
                            new ColumnHeaderTemplate(){ ColName = _lang["consumption_tax_col"] },
                            new ColumnHeaderTemplate(){ ColName = _lang["fee_amount_col"] },

                            new ColumnHeaderTemplate(){ ColName = _lang["sales_amount_col"] },
                            new ColumnHeaderTemplate(){ ColName = _lang["consumption_tax_col"] },
                            new ColumnHeaderTemplate(){ ColName = _lang["fee_amount_col"] },

                            new ColumnHeaderTemplate(){ ColName = _lang["sales_amount_col"] },
                            new ColumnHeaderTemplate(){ ColName = _lang["consumption_tax_col"] },
                            new ColumnHeaderTemplate(){ ColName = _lang["fee_amount_col"] },

                            new ColumnHeaderTemplate(){ ColName = _lang["amount_of_money_col"] },
                            new ColumnHeaderTemplate(){ ColName = _lang["consumption_tax_col"] },
                        }
                    }
                }
            };

            Body = new BodyTemplate()
            {
                Rows = new List<RowBodyTemplate>()
                {
                    new RowBodyTemplate()
                    {
                        Columns = new List<ColumnBodyTemplate>()
                        {
                            new ColumnBodyTemplate(){ AlignCol = AlignColEnum.Center, DisplayFieldName = nameof(MonthlyRevenueItem.No), CustomCss = VerticalAlignCss},
                            new ColumnBodyTemplate(){ AlignCol = AlignColEnum.Center, DisplayFieldName = nameof(MonthlyRevenueItem.UriYmd), CustomCss = VerticalAlignCss, CustomTextFormatDelegate = e => e.ToString().Substring(e.ToString().Length - 2)},
                            new ColumnBodyTemplate(){ AlignCol = AlignColEnum.Right, DisplayFieldName = nameof(MonthlyRevenueItem.KeiKin), CustomCss = VerticalAlignCss, CustomTextFormatDelegate = e => KoboGridHelper.AddCommasForNumber(e)},
                            new ColumnBodyTemplate(){ AlignCol = AlignColEnum.Left, Control = new MultiLineControl<MonthlyRevenueItem> { MutilineText = GetYouRyakuNmAndYouSitRyakuNm } },
                            new ColumnBodyTemplate(){ AlignCol = AlignColEnum.Right, Control = new MultiLineControl<MonthlyRevenueItem>{ MutilineText  = GetYouSyaSyuDai } },
                            new ColumnBodyTemplate(){ AlignCol = AlignColEnum.Right, Control = new MultiLineControl<MonthlyRevenueItem>{ MutilineText  = GetYouG } },
                            new ColumnBodyTemplate(){ AlignCol = AlignColEnum.Right, Control = new MultiLineControl<MonthlyRevenueItem>{ MutilineText  = GetYouFutG } },
                            new ColumnBodyTemplate(){ AlignCol = AlignColEnum.Right, DisplayFieldName = nameof(MonthlyRevenueItem.JisSyaRyoSum), CustomCss = VerticalAlignCss, CustomTextFormatDelegate = KoboGridHelper.AddCommasForNumber},
                            new ColumnBodyTemplate(){ AlignCol = AlignColEnum.Right, DisplayFieldName = nameof(MonthlyRevenueItem.GaiSyaRyoSum), CustomCss = VerticalAlignCss, CustomTextFormatDelegate = KoboGridHelper.AddCommasForNumber},
                            new ColumnBodyTemplate(){ AlignCol = AlignColEnum.Right, DisplayFieldName = nameof(MonthlyRevenueItem.EtcSyaRyoSum), CustomCss = VerticalAlignCss, CustomTextFormatDelegate = KoboGridHelper.AddCommasForNumber},
                            new ColumnBodyTemplate(){ AlignCol = AlignColEnum.Right, DisplayFieldName = nameof(MonthlyRevenueItem.CanSum), CustomCss = VerticalAlignCss, CustomTextFormatDelegate = KoboGridHelper.AddCommasForNumber},
                            new ColumnBodyTemplate(){ AlignCol = AlignColEnum.Right, DisplayFieldName = nameof(MonthlyRevenueItem.UntSoneki), CustomCss = VerticalAlignCss, CustomTextFormatDelegate = KoboGridHelper.AddCommasForNumber},

                            new ColumnBodyTemplate(){ AlignCol = AlignColEnum.Right, Control = new MultiLineControl<MonthlyRevenueItem>{ MutilineText  = GetYouSyaRyoUnc } },
                            new ColumnBodyTemplate(){ AlignCol = AlignColEnum.Right, Control = new MultiLineControl<MonthlyRevenueItem>{ MutilineText  = GetYouSyaRyoSyo } },
                            new ColumnBodyTemplate(){ AlignCol = AlignColEnum.Right, Control = new MultiLineControl<MonthlyRevenueItem>{ MutilineText  = GetYouSyaRyoTes } },

                            new ColumnBodyTemplate(){ AlignCol = AlignColEnum.Right, Control = new MultiLineControl<MonthlyRevenueItem>{ MutilineText  = GetYfuUriGakKin} },
                            new ColumnBodyTemplate(){ AlignCol = AlignColEnum.Right, Control = new MultiLineControl<MonthlyRevenueItem>{ MutilineText  = GetYfuSyaRyoSyo} },
                            new ColumnBodyTemplate(){ AlignCol = AlignColEnum.Right, Control = new MultiLineControl<MonthlyRevenueItem>{ MutilineText  = GetYfuSyaRyoTes} },

                            new ColumnBodyTemplate(){ AlignCol = AlignColEnum.Right, DisplayFieldName = nameof(MonthlyRevenueItem.JisSyaRyoUnc), CustomCss = VerticalAlignCss, CustomTextFormatDelegate = KoboGridHelper.AddCommasForNumber},
                            new ColumnBodyTemplate(){ AlignCol = AlignColEnum.Right, DisplayFieldName = nameof(MonthlyRevenueItem.JisSyaSyuDai), CustomCss = VerticalAlignCss, CustomTextFormatDelegate = KoboGridHelper.AddCommasForNumber},
                            new ColumnBodyTemplate(){ AlignCol = AlignColEnum.Right, DisplayFieldName = nameof(MonthlyRevenueItem.JisSyaRyoSyo), CustomCss = VerticalAlignCss, CustomTextFormatDelegate = KoboGridHelper.AddCommasForNumber},
                            new ColumnBodyTemplate(){ AlignCol = AlignColEnum.Right, DisplayFieldName = nameof(MonthlyRevenueItem.JisSyaRyoTes), CustomCss = VerticalAlignCss, CustomTextFormatDelegate = KoboGridHelper.AddCommasForNumber},

                            new ColumnBodyTemplate(){ AlignCol = AlignColEnum.Right, DisplayFieldName = nameof(MonthlyRevenueItem.GaiUriGakKin), CustomCss = VerticalAlignCss, CustomTextFormatDelegate = KoboGridHelper.AddCommasForNumber},
                            new ColumnBodyTemplate(){ AlignCol = AlignColEnum.Right, DisplayFieldName = nameof(MonthlyRevenueItem.GaiSyaRyoSyo), CustomCss = VerticalAlignCss, CustomTextFormatDelegate = KoboGridHelper.AddCommasForNumber},
                            new ColumnBodyTemplate(){ AlignCol = AlignColEnum.Right, DisplayFieldName = nameof(MonthlyRevenueItem.GaiSyaRyoTes), CustomCss = VerticalAlignCss, CustomTextFormatDelegate = KoboGridHelper.AddCommasForNumber},

                            new ColumnBodyTemplate(){ AlignCol = AlignColEnum.Right, DisplayFieldName = nameof(MonthlyRevenueItem.EtcUriGakKin), CustomCss = VerticalAlignCss, CustomTextFormatDelegate = KoboGridHelper.AddCommasForNumber},
                            new ColumnBodyTemplate(){ AlignCol = AlignColEnum.Right, DisplayFieldName = nameof(MonthlyRevenueItem.EtcSyaRyoSyo), CustomCss = VerticalAlignCss, CustomTextFormatDelegate = KoboGridHelper.AddCommasForNumber},
                            new ColumnBodyTemplate(){ AlignCol = AlignColEnum.Right, DisplayFieldName = nameof(MonthlyRevenueItem.EtcSyaRyoTes), CustomCss = VerticalAlignCss, CustomTextFormatDelegate = KoboGridHelper.AddCommasForNumber},

                            new ColumnBodyTemplate(){ AlignCol = AlignColEnum.Right, DisplayFieldName = nameof(MonthlyRevenueItem.CanUnc), CustomCss = VerticalAlignCss, CustomTextFormatDelegate = KoboGridHelper.AddCommasForNumber},
                            new ColumnBodyTemplate(){ AlignCol = AlignColEnum.Right, DisplayFieldName = nameof(MonthlyRevenueItem.CanSyoG), CustomCss = VerticalAlignCss, CustomTextFormatDelegate = KoboGridHelper.AddCommasForNumber},
                        }
                    }
                }
            };
        }

        private IEnumerable<string> GetYfuSyaRyoTes(MonthlyRevenueItem item)
        {
            if (item != null && item.DetailItems != null && item.DetailItems.Any())
            {
                foreach (var sub in item.DetailItems)
                {
                    yield return sub.YfuSyaRyoTes.AddCommas();
                    yield return string.Empty;
                }
            }
        }
        private IEnumerable<string> GetYfuSyaRyoSyo(MonthlyRevenueItem item)
        {
            if (item != null && item.DetailItems != null && item.DetailItems.Any())
            {
                foreach (var sub in item.DetailItems)
                {
                    yield return sub.YfuSyaRyoSyo.AddCommas();
                    yield return string.Empty;
                }
            }
        }
        private IEnumerable<string> GetYfuUriGakKin(MonthlyRevenueItem item)
        {
            if (item != null && item.DetailItems != null && item.DetailItems.Any())
            {
                foreach (var sub in item.DetailItems)
                {
                    yield return sub.YfuUriGakKin.AddCommas();
                    yield return string.Empty;
                }
            }
        }
        private IEnumerable<string> GetYouSyaRyoTes(MonthlyRevenueItem item)
        {
            if (item != null && item.DetailItems != null && item.DetailItems.Any())
            {
                foreach (var sub in item.DetailItems)
                {
                    yield return sub.YouSyaRyoTes.AddCommas();
                    yield return string.Empty;
                }
            }
        }
        private IEnumerable<string> GetYouSyaRyoSyo(MonthlyRevenueItem item)
        {
            if (item != null && item.DetailItems != null && item.DetailItems.Any())
            {
                foreach (var sub in item.DetailItems)
                {
                    yield return sub.YouSyaRyoSyo.AddCommas();
                    yield return string.Empty;
                }
            }
        }
        private IEnumerable<string> GetYouSyaRyoUnc(MonthlyRevenueItem item)
        {
            if (item != null && item.DetailItems != null && item.DetailItems.Any())
            {
                foreach (var sub in item.DetailItems)
                {
                    yield return sub.YouSyaRyoUnc.AddCommas();
                    yield return string.Empty;
                }
            }
        }
        private IEnumerable<string> GetYouFutG(MonthlyRevenueItem item)
        {
            if (item != null && item.DetailItems != null && item.DetailItems.Any())
            {
                foreach (var sub in item.DetailItems)
                {
                    yield return sub.YouFutG.AddCommas();
                    yield return string.Empty;
                }
            }
        }
        private IEnumerable<string> GetYouG(MonthlyRevenueItem item)
        {
            if (item != null && item.DetailItems != null && item.DetailItems.Any())
            {
                foreach (var sub in item.DetailItems)
                {
                    yield return sub.YouG.AddCommas();
                    yield return string.Empty;
                }
            }
        }
        private IEnumerable<string> GetYouSyaSyuDai(MonthlyRevenueItem item)
        {
            if (item != null && item.DetailItems != null && item.DetailItems.Any())
            {
                foreach (var sub in item.DetailItems)
                {
                    yield return sub.YouSyaSyuDai.AddCommas();
                    yield return string.Empty;
                }
            }
        }
        private IEnumerable<string> GetYouRyakuNmAndYouSitRyakuNm(MonthlyRevenueItem item)
        {
            if (item != null && item.DetailItems != null && item.DetailItems.Any())
            {
                foreach (var sub in item.DetailItems)
                {
                    yield return sub.YouRyakuNm;
                    yield return sub.YouSitRyakuNm;
                }
            }
        }

        private void ClearCurrentData()
        {
            if (eigyoListItems != null) eigyoListItems.Clear();
            if (monthlyRevenueItems != null) monthlyRevenueItems.Clear();
            if (summaryResult != null) summaryResult.Clear();
            if (actualRevenueItems != null) actualRevenueItems.Clear();
            currentPage = DefaultPage;
            itemPerPage = Common.DefaultPageSize;
            StateHasChanged();
        }

        /// <summary>
        /// Init controls for current page
        /// </summary>
        /// <returns></returns>
        private async Task InitControls(TransportationRevenueSearchModel model)
        {
            if (model == null)
                model = new TransportationRevenueSearchModel();

            eigyoListItems = await _revenueSummaryService.GetEigyoListForMonthlyRevenueReport(model);
            await DataChanged.InvokeAsync(eigyoListItems.Any());
            if (!eigyoListItems.Any()) eigyoListItems = new List<EigyoListItem>();
            searchModel = new MonthlyRevenueSearchModel();
            searchModel.TesukomiKbn = _revenueSummaryService.GetTesuInKbnItems().Find(o => o.Value == (int)model.TesuInKbn).Text;
            searchModel.Eigyo = eigyoListItems.FirstOrDefault();
            searchModel.RevenueSearchModel = model;
            searchModel.UriYm = model.UriYmdFrom.Substring(0, model.UriYmdFrom.Length - 2).AddSlash2YYYYMM();
        }

        /// <summary>
        /// Get data for 2 grids
        /// </summary>
        /// <returns></returns>
        private async Task GetGridData(MonthlyRevenueSearchModel model)
        {
            var result = await _revenueSummaryService.GetMonthlyRevenueData(model);
            if (result != null)
            {
                monthlyRevenueItems = result.MonthlyRevenueItems;
                summaryResult = result.SummaryResult;
            }
            else
            {
                monthlyRevenueItems = new List<MonthlyRevenueItem>();
                summaryResult = new List<SummaryResult>();
            }
            RebindGridData();
        }

        /// <summary>
        /// Trigger when Eigyo combobox changed 
        /// </summary>
        /// <param name="val"></param>
        /// <param name="isPre"></param>
        /// <returns></returns>
        protected async Task EigyoListItemsChanged(EigyoListItem val, bool? isPre = null)
        {
            try
            {
                await _loading.ShowAsync();
                if (isPre == null)
                {
                    searchModel.Eigyo = val;
                    await GetGridData(searchModel);
                }
                else
                {
                    if (isPre == true)
                    {
                        var preIndex = GetPreNextIndex(true, eigyoListItems, searchModel.Eigyo);
                        if (preIndex != -1)
                        {
                            searchModel.Eigyo = eigyoListItems[preIndex];
                            await GetGridData(searchModel);
                        }
                    }
                    else
                    {
                        var nextIndex = GetPreNextIndex(false, eigyoListItems, searchModel.Eigyo);
                        if (nextIndex != -1)
                        {
                            searchModel.Eigyo = eigyoListItems[nextIndex];
                            await GetGridData(searchModel);
                        }
                    }
                }
                StateHasChanged();
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

        /// <summary>
        /// Get pre - next index
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isPre"></param>
        /// <param name="list"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        private int GetPreNextIndex<T>(bool isPre, List<T> list, T val)
        {
            var index = list.IndexOf(val);

            return isPre ? (index > 0 ? index - 1 : -1) : (index < list.Count - 1 ? index + 1 : -1);
        }

        public async Task<bool> Reload(TransportationRevenueSearchModel model)
        {
            try
            {
                await _loading.ShowAsync();
                Model = model;
                ClearCurrentData();
                await InitControls(Model);
                if (eigyoListItems.Any())
                    await GetGridData(searchModel);
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
            return monthlyRevenueItems.Any();
        }

        // Add for paging
        private void RebindGridData()
        {
            actualRevenueItems = monthlyRevenueItems.Skip(currentPage * itemPerPage).Take(itemPerPage).ToList();
            //Re-Caculate Summary Result
            var commonResult = new SummaryResult();
            commonResult.MesaiKbn = 1;
            commonResult.SFutSyaRyoSyo = actualRevenueItems.Sum(e => e.SFutSyaRyoSyo);
            commonResult.SFutSyaRyoTes = actualRevenueItems.Sum(e => e.SFutSyaRyoTes);
            commonResult.SFutUriGakKin = actualRevenueItems.Sum(e => e.SFutUriGakKin);
            commonResult.SJisSyaRyoSyo = actualRevenueItems.Sum(e => e.SJisSyaRyoSyo);
            commonResult.SJisSyaRyoTes = actualRevenueItems.Sum(e => e.SJisSyaRyoTes);
            commonResult.SJisSyaRyoUnc = actualRevenueItems.Sum(e => e.SJisSyaRyoUnc);
            commonResult.SJyuSyaRyoRui = actualRevenueItems.Sum(e => e.SJyuSyaRyoRui);
            commonResult.SJyuSyaRyoSyo = actualRevenueItems.Sum(e => e.SJyuSyaRyoSyo);
            commonResult.SJyuSyaRyoTes = actualRevenueItems.Sum(e => e.SJyuSyaRyoTes);
            commonResult.SJyuSyaRyoUnc = actualRevenueItems.Sum(e => e.SJyuSyaRyoUnc);
            commonResult.SSoneki = actualRevenueItems.Sum(e => e.SSoneki);
            commonResult.SYfuSyaRyoSyo = actualRevenueItems.Sum(e => e.SYfuSyaRyoSyo);
            commonResult.SYfuSyaRyoTes = actualRevenueItems.Sum(e => e.SYfuSyaRyoTes);
            commonResult.SYfuUriGakKin = actualRevenueItems.Sum(e => e.SYfuUriGakKin);
            commonResult.SYoushaSyo = actualRevenueItems.Sum(e => e.SYoushaSyo);
            commonResult.SYoushaTes = actualRevenueItems.Sum(e => e.SYoushaTes);
            commonResult.SYoushaUnc = actualRevenueItems.Sum(e => e.SYoushaUnc);
            var removeItem = summaryResult.FirstOrDefault(e => e.MesaiKbn == 1);
            if (removeItem != null)
                summaryResult.Remove(removeItem);
            summaryResult.Add(commonResult);
            summaryResult = summaryResult.OrderBy(e => e.MesaiKbn).ToList();
            StateHasChanged();
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
