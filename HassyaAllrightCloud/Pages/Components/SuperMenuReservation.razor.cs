using HassyaAllrightCloud.Infrastructure.Services;
using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using HassyaAllrightCloud.Pages.Components.CommonComponents;
using HassyaAllrightCloud.IService.CommonComponents;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using System.Data;
using System.Text;
using Microsoft.JSInterop;
using Microsoft.Extensions.Localization;
using BlazorContextMenu;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Pages.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Forms;
using System.IO;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Commons.Constants;
using SharedLibraries.UI.Models;
using HassyaAllrightCloud.Domain.Dto.BillPrint;
using System.Text;
using System.Globalization;
using SharedLibraries.UI.Models;

namespace HassyaAllrightCloud.Pages.Components
{
    public class SuperMenuReservationBase : ComponentBase
    {
        #region Inject
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected IBlazorContextMenuService blazorContextMenuService { get; set; }
        [Inject] protected CustomNavigation NavigationManager { get; set; }
        [Inject] protected IHyperDataService HyperDataService { get; set; }
        [Inject] protected IStringLocalizer<SuperMenu> Lang { get; set; }
        [Inject] protected IFilterCondition FilterConditionService { get; set; }
        [Inject] protected AppSettingsService AppSettingsService { get; set; }
        [Inject] protected IGenerateFilterValueDictionary GenerateFilterValueDictionaryService { get; set; }
        [Inject] protected IGridLayoutService GridLayoutService { get; set; }
        [Inject] protected IBookingTypeListService YoyKbnService { get; set; }
        [Inject] protected IHikiukeshoReportService HikiukeshoReportService { get; set; }
        [Inject] protected IAdvancePaymentDetailsService advancePaymentDetailsService { get; set; }
        [Inject] protected IReportLayoutSettingService reportLayoutSettingService { get; set; }
        [Inject] protected ISimpleQuotationService simpleQuotationServic { get; set; }
        [Inject] protected ILoadingService _loadingService { get; set; }
        [Inject] protected IErrorHandlerService errorModalService { get; set; }
        [Inject] protected IBusCoordinationReportService BusCoordinationReportService { get; set; }
        [Inject] protected IUnkoushijishoReportService UnkoushijishoReportService { get; set; }
        [Inject] protected IFareFeeCorrectionService _fareFeeCorrectionService { get; set; }
        [Inject] protected IHaitaCheckService HaitaCheckService { get; set; }
        #endregion
        [Parameter] public List<SuperMenuReservationData> GridDatas { get; set; }
        [Parameter] public SuperMenuReservationTotalGridData GridTotalDatas { get; set; }
        [Parameter] public int ActiveV { get; set; }
        [Parameter] public byte RecordsPerPage { get; set; } = (byte)25;
        [Parameter] public HyperFormData hyperData { get; set; }
        [Parameter] public EventCallback<int> ChangeState { get; set; }
        [Parameter] public EventCallback<bool> ValueCheckHaita { get; set; }
        [Parameter] public int FirstPageSelect { get; set; }
        [Parameter] public EventCallback<int> FirstPageSelectChanged { get; set; }
        [Parameter] public EventCallback<byte> RecordsPerPageChanged { get; set; }
        [Parameter] public bool isChangePageNumber { get; set; }
        [Parameter] public Pagination paging { get; set; } = new Pagination();
        [Parameter] public bool isInitComplete { get; set; } = false;
        [Parameter] public bool isFirstInit { get; set; } = true;
        [Parameter] public bool loading { get; set; }
        [Parameter] public EventCallback<bool> loadingChanged { get; set; }
        [Parameter] public bool isChangeValue { get; set; } = false;
        [Parameter] public EventCallback<bool> isChangeValueChanged { get; set; }
        [Parameter] public bool isReloadTotal { get; set; } = false;
        [Parameter] public EventCallback<bool> isReloadTotalChanged { get; set; }
        public List<ReservationDataToCheck> ReservertionData;
        public SuperMenuReservationTotalData CurrentTotal { get; set; }
        public string CurrentTotalContent;
        public int MaxPageCount = 5;
        public int CurrentPage = 1;
        public int NumberOfPage;
        public List<SuperMenuReservationData> GridDisplay { get; set; }
        public List<SuperMenuReservationData> CheckedItems { get; set; } = new List<SuperMenuReservationData>();
        public bool isClickRow { get; set; } = false;
        public int LastXClicked { get; set; }
        public int LastYClicked { get; set; }
        public int? CurrentClick { get; set; } = null;
        public int? CurrentScroll { get; set; }
        public bool ShowPopup = false;
        public Dictionary<string, List<string>> InfoMessage = new Dictionary<string, List<string>>();
        public string Comma = "、";
        public bool ShowComfirmDeletePopup = false;
        protected ShowCheckboxArgs<SuperMenuReservationData> ShowCheckboxOptions { get; set; } = new ShowCheckboxArgs<SuperMenuReservationData>()
        {
            RowIdentifier = (checkedItem, item) => checkedItem.No == item.No,
            Disable = (item) => false
        };
        protected List<SuperMenuReservationData> DataItems { get; set; } = new List<SuperMenuReservationData>();
        protected HeaderTemplate Header { get; set; }
        protected BodyTemplate Body { get; set; }
        public string UkeCdParam { get; set; } = "0";
        public string currentUkeNo = "";
        public short currentunkRen;
        public Int16 currentBunkRen = 0;
        public BookingFormData CurrentBookingData = new BookingFormData();
        protected bool PopupFutai { get; set; }
        protected bool PopupJourney { get; set; } = false;
        protected bool PopupTehai { get; set; } = false;
        protected bool PopupFareFeeCorrection { get; set; } = false;
        protected bool PopupVehicleAllocationInput { get; set; } = false;
        protected bool PopupUploadFile { get; set; } = false;
        protected bool IsOpenConfirmTab { get; set; } = false;
        protected bool PopupTsumi { get; set; }
        protected bool IsOpenCancelTab { get; set; } = false;
        protected bool IsOpenEditYykshoBikoNm { get; set; } = false;
        protected bool IsOpenEditUnkobiBikoNm { get; set; } = false;
        protected bool DataNotExistPopup { get; set; } = false;
        protected string NoDataType { get; set; } = "";
        public bool isLoadTotal = true;
        public bool isFirstRender = true;
        protected override void OnInitialized()
        {
            try
            {
                CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
                if (cultureInfo.Name != "ja-JP")
                {
                    Comma = ", ";
                }
                GenrateGrid();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected override void OnAfterRender(bool firstRender)
        {
            try
            {
                if (firstRender)
                {
                    StateHasChanged();
                    JSRuntime.InvokeVoidAsync("handleSelectByKeyUp", DotNetObjectReference.Create(this));
                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected override void OnParametersSet()
        {
            try
            {
                if (isInitComplete)
                {
                    if ((isChangeValue && isFirstInit == false) && (isLoadTotal == false && isReloadTotal == false))
                    {
                        changeData();
                        if (loading)
                        {
                            loading = false;
                            loadingChanged.InvokeAsync(loading);
                        }
                        isChangeValue = false;
                        isChangeValueChanged.InvokeAsync(isChangeValue);
                    }
                    if (isLoadTotal || isReloadTotal)
                    {
                        changeData();
                        isLoadTotal = false;
                        isReloadTotal = false;
                        isReloadTotalChanged.InvokeAsync(isReloadTotal);
                    }
                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }
        protected void changeData()
        {
            JSRuntime.InvokeVoidAsync("loadPageScript", "hyperMenuPage");
            CurrentTotal = GridTotalDatas.Order;
            CurrentTotalContent = Lang["Order"];
            NumberOfPage = (GridDatas.Count() + RecordsPerPage - 1) / RecordsPerPage;
            FirstLoad();
        }

        protected void CheckedChange(CheckedChangeEventArgs<SuperMenuReservationData> checkedItems)
        {
            try
            {
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        public static Func<object, string> CustomRowCss = (item) =>
        {
            var data = item as SuperMenuReservationData;
            var cssClass = "";
            SuperMenuColorPattern result;
            if (data == null)
                return cssClass;
            if (data.SihKbn > 1)
            {
                result = SuperMenuColorPattern.Payment;
            }
            else if (data.SCouKbn > 1)
            {
                result = SuperMenuColorPattern.Support;
            }
            else if (data.NyuKinKbn > 1)
            {
                result = SuperMenuColorPattern.Deposit;
            }
            else if (data.NCouKbn > 1)
            {
                result = SuperMenuColorPattern.Enter;
            }
            else if (data.NippoKbn > 1)
            {
                result = SuperMenuColorPattern.DailyReport;
            }
            else if (data.YouKbn > 1)
            {
                result = SuperMenuColorPattern.Mercenary;
            }
            else if (data.HaiSKbn > 1)
            {
                result = SuperMenuColorPattern.Dispatch;
            }
            else if (data.HaiIKbn > 1)
            {
                result = SuperMenuColorPattern.Manning;
            }
            else if (data.GuiWNin != 0)
            {
                result = SuperMenuColorPattern.Allocation;
            }
            else if (data.KaktYmd != null && data.KaktYmd.Trim().Length > 0)
            {
                result = SuperMenuColorPattern.Confirmed;
            }
            else if (data.KaknKais != 0)
            {
                result = SuperMenuColorPattern.Confirmation;
            }
            else if (data.KSKbn > 1)
            {
                result = SuperMenuColorPattern.TemporaryBus;
            }
            else if (data.KHinKbn > 1)
            {
                result = SuperMenuColorPattern.TemporaryDistribution;
            }
            else
            {
                result = SuperMenuColorPattern.Unprovisioned;
            }
            cssClass = "grid-color-" + ((int)result).ToString();
            return cssClass;
        };

        protected async Task RowClick(RowClickEventArgs<SuperMenuReservationData> args)
        {
            try
            {
                SuperMenuReservationData SelectedItem = args.SelectedItem;
                LastXClicked = Convert.ToInt32(args.Event.ClientX);
                LastYClicked = Convert.ToInt32(args.Event.ClientY);
                CurrentScroll = null;
                if (!args.Event.ShiftKey && !args.Event.CtrlKey)
                {
                    if (CheckedItems.Count > 1)
                    {
                        await blazorContextMenuService.HideMenu("gridRowClickMenu");
                        await blazorContextMenuService.ShowMenu("gridRowsClickMenu", LastXClicked, LastYClicked);
                    }
                    else
                    {
                        CheckedItems = new List<SuperMenuReservationData>() { SelectedItem };
                        await JSRuntime.InvokeVoidAsync("SetPositionForMenuContext", LastYClicked, DotNetObjectReference.Create(this));
                    }
                }
                else
                {
                    isClickRow = true;
                    await blazorContextMenuService.HideMenu("gridRowClickMenu");
                    if (args.Event.CtrlKey)
                    {
                        if (!CheckedItems.Any(item => item.No == SelectedItem.No))
                        {
                            CheckedItems.Add(DataItems.FirstOrDefault(item => item.No == SelectedItem.No));
                        }
                        else
                        {
                            CheckedItems.RemoveAll(item => item.No == SelectedItem.No);
                            if (CheckedItems.Count == 0)
                            {
                                CurrentClick = null;
                            }
                            else
                            {
                                CurrentClick = CheckedItems.Max(item => item.No);
                            }
                        }
                    }
                    else
                    {
                        int BeginIndex = Math.Min(SelectedItem.No, (int)CurrentClick);
                        int EndIndex = Math.Max(SelectedItem.No, (int)CurrentClick);
                        for (int IndexToBeAdd = BeginIndex; IndexToBeAdd <= EndIndex; IndexToBeAdd++)
                        {
                            if (!CheckedItems.Any(item => item.No == IndexToBeAdd))
                            {
                                CheckedItems.Add(DataItems.FirstOrDefault(item => item.No == IndexToBeAdd));
                            }
                        }
                    }
                }

                CurrentClick = SelectedItem.No;
                InvokeAsync(StateHasChanged).Wait();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        [JSInvokable]
        public async Task positionGetComplete(int y)
        {
            try
            {
                if (y > 0)
                    LastYClicked = y;
                await blazorContextMenuService.ShowMenu("gridRowClickMenu", LastXClicked, LastYClicked);
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        public async void SaveGridLayout()
        {
            try
            {
                await _loadingService.ShowAsync();
                await Task.Run(() =>
                {
                    var headerColumns = Header.Rows.Count > 0 ? Header.Rows[0].Columns : new List<ColumnHeaderTemplate>();
                    List<TkdGridLy> gridLayouts = new List<TkdGridLy>();
                    if (headerColumns.Count > 0)
                    {
                        for (int i = 0; i < headerColumns.Count; i++)
                        {
                            string itemName = headerColumns[i].CodeName;
                            gridLayouts.Add(new TkdGridLy()
                            {
                                DspNo = i,
                                FormNm = FormFilterName.SuperMenuReservation,
                                FrozenCol = 0,
                                GridNm = SuperMenuType1GridHeaderNameConstants.GridName,
                                ItemNm = itemName,
                                SyainCdSeq = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq,
                                UpdPrgId = Common.UpdPrgId,
                                UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq,
                                UpdYmd = DateTime.Now.ToString().Substring(0, 10).Replace("/", string.Empty),
                                UpdTime = DateTime.Now.ToString().Substring(11).Replace(":", string.Empty),
                                Width = headerColumns[i].Width
                            });
                        }
                    }

                    var isSaved = GridLayoutService.SaveGridLayout(gridLayouts).Result;
                    InvokeAsync(StateHasChanged).Wait();
                });
                await _loadingService.HideAsync();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        public async void InitGridLayout()
        {
            try
            {
                await _loadingService.ShowAsync();
                await Task.Run(() =>
                {
                    List<TkdGridLy> tkdGridLies = new List<TkdGridLy>();
                    RenderGridBySavedLayout(tkdGridLies);
                    DeleteSavedGridLayout();
                    InvokeAsync(StateHasChanged).Wait();
                });
                await _loadingService.HideAsync();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected void DeleteSavedGridLayout()
        {
            try
            {
                var isDeleted = GridLayoutService.DeleteSavedGridLayout(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, FormFilterName.SuperMenuReservation, SuperMenuType1GridHeaderNameConstants.GridName).Result;
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected void GenrateGrid()
        {
            try
            {
                List<TkdGridLy> tkdGridLies = GridLayoutService.GetGridLayout(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, FormFilterName.SuperMenuReservation, SuperMenuType1GridHeaderNameConstants.GridName).Result;
                RenderGridBySavedLayout(tkdGridLies);
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected void RenderGridBySavedLayout(List<TkdGridLy> gridlayouts)
        {
            try
            {
                HeaderTemplate headerTemplate = new HeaderTemplate();
                headerTemplate.Rows = new List<RowHeaderTemplate>();
                headerTemplate.StickyCount = 1;
                BodyTemplate bodyTemplate = new BodyTemplate();
                bodyTemplate.Rows = new List<RowBodyTemplate>();
                bodyTemplate.CustomCssDelegate = CustomRowCss;
                for (int i = 0; i < 3; i++)
                {
                    headerTemplate.Rows.Add(new RowHeaderTemplate());
                    bodyTemplate.Rows.Add(new RowBodyTemplate());
                }
                for (int i = 0; i < 3; i++)
                {
                    headerTemplate.Rows[i].Columns = new List<ColumnHeaderTemplate>();
                    bodyTemplate.Rows[i].Columns = new List<ColumnBodyTemplate>();
                }
                int CurrentIndex = 0;
                while (true)
                {
                    // No
                    if ((gridlayouts.Count == 0 && CurrentIndex == 0) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == SuperMenuType1GridHeaderNameConstants.NoItemNm))
                    {
                        headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["No"], RowSpan = 3, CodeName = SuperMenuType1GridHeaderNameConstants.NoItemNm, Width = gridlayouts.Count == 0 ? 50 : gridlayouts[CurrentIndex].Width });

                        bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.No), RowSpan = 3, AlignCol = AlignColEnum.Center });
                    }

                    // 記号
                    if ((gridlayouts.Count == 0 && CurrentIndex == 1) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == SuperMenuType1GridHeaderNameConstants.MarkItemNm))
                    {
                        headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridMark"], CodeName = SuperMenuType1GridHeaderNameConstants.MarkItemNm, RowSpan = 3, Width = gridlayouts.Count == 0 ? 50 : gridlayouts[CurrentIndex].Width });

                        bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.SymbolInString), RowSpan = 3, AlignCol = AlignColEnum.Center });
                    }

                    // 得意先, 支店名, 担当者
                    if ((gridlayouts.Count == 0 && CurrentIndex == 2) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == SuperMenuType1GridHeaderNameConstants.CustomerItemNm))
                    {
                        headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridCustomer"], CodeName = SuperMenuType1GridHeaderNameConstants.CustomerItemNm, RowSpan = 1, Width = gridlayouts.Count == 0 ? 150 : gridlayouts[CurrentIndex].Width });
                        headerTemplate.Rows[1].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridBranch"], RowSpan = 1 });
                        headerTemplate.Rows[2].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridCustomerStaff"], RowSpan = 1 });

                        bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.Customer), RowSpan = 1 });
                        bodyTemplate.Rows[1].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.Branch), RowSpan = 1 });
                        bodyTemplate.Rows[2].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.PersonInCharge), RowSpan = 1 });
                    }

                    // 団体名, 幹事名, 行先名
                    if ((gridlayouts.Count == 0 && CurrentIndex == 3) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == SuperMenuType1GridHeaderNameConstants.OrganizationItemNm))
                    {
                        headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridGroup"], CodeName = SuperMenuType1GridHeaderNameConstants.OrganizationItemNm, RowSpan = 1, Width = gridlayouts.Count == 0 ? 200 : gridlayouts[CurrentIndex].Width });
                        headerTemplate.Rows[1].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridGroupStaff"], RowSpan = 1 });
                        headerTemplate.Rows[2].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridDestination"], RowSpan = 1 });

                        bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.Organization), RowSpan = 1 });
                        bodyTemplate.Rows[1].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.Secretary), RowSpan = 1 });
                        bodyTemplate.Rows[2].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.Destination), RowSpan = 1 });
                    }

                    // 配車, 到着, 運転手数/ガイド数
                    if ((gridlayouts.Count == 0 && CurrentIndex == 4) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == SuperMenuType1GridHeaderNameConstants.DispatchItemNm))
                    {
                        headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridDispatch"], CodeName = SuperMenuType1GridHeaderNameConstants.DispatchItemNm, RowSpan = 1, Width = gridlayouts.Count == 0 ? 200 : gridlayouts[CurrentIndex].Width });
                        headerTemplate.Rows[1].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridArrival"], RowSpan = 1 });
                        headerTemplate.Rows[2].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridDriverGuiderNumber"], RowSpan = 1 });

                        bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.Dispatch), RowSpan = 1 });
                        bodyTemplate.Rows[1].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.Arrival), RowSpan = 1 });
                        bodyTemplate.Rows[2].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.NumberOfDriverNumberOfGuideString), RowSpan = 1 });
                    }

                    // 車種/台数
                    if ((gridlayouts.Count == 0 && CurrentIndex == 5) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == SuperMenuType1GridHeaderNameConstants.CarTypeItemNm))
                    {
                        headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridBusType"], CodeName = SuperMenuType1GridHeaderNameConstants.CarTypeItemNm, RowSpan = 3, Width = gridlayouts.Count == 0 ? 300 : gridlayouts[CurrentIndex].Width });

                        bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { Control = new MultiLineControl<SuperMenuReservationData> { MutilineText = GetBusesInfo }, RowSpan = 3 });
                    }

                    // 運賃
                    if ((gridlayouts.Count == 0 && CurrentIndex == 6) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == SuperMenuType1GridHeaderNameConstants.FareItemNm))
                    {
                        headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridFare"], CodeName = SuperMenuType1GridHeaderNameConstants.FareItemNm, RowSpan = 1, ColSpan = 2, Width = gridlayouts.Count == 0 ? 150 : gridlayouts[CurrentIndex].Width });
                        headerTemplate.Rows[1].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridFareTax"], RowSpan = 1, ColSpan = 2 });
                        headerTemplate.Rows[2].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridFareCommission"], RowSpan = 1, ColSpan = 2 });

                        bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.Fare), RowSpan = 1, ColSpan = 2, CustomTextFormatDelegate = KoboGridHelper.ToFormatC, AlignCol = AlignColEnum.Right });
                        bodyTemplate.Rows[1].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.TaxRate), RowSpan = 1, CustomTextFormatDelegate = KoboGridHelper.ToFormatPercent, AlignCol = AlignColEnum.Right });
                        bodyTemplate.Rows[1].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.FareTax), RowSpan = 1, CustomTextFormatDelegate = KoboGridHelper.ToFormatC, AlignCol = AlignColEnum.Right });
                        bodyTemplate.Rows[2].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.FeeTaxRate), RowSpan = 1, CustomTextFormatDelegate = KoboGridHelper.ToFormatPercent, AlignCol = AlignColEnum.Right });
                        bodyTemplate.Rows[2].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.FareFee), RowSpan = 1, CustomTextFormatDelegate = KoboGridHelper.ToFormatC, AlignCol = AlignColEnum.Right });
                    }

                    // 上限(料金), 下限(料金), 指数
                    if ((gridlayouts.Count == 0 && CurrentIndex == 7) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == SuperMenuType1GridHeaderNameConstants.LimitItemNm))
                    {
                        headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridMaxAmount"], CodeName = SuperMenuType1GridHeaderNameConstants.LimitItemNm, RowSpan = 1, Width = gridlayouts.Count == 0 ? 150 : gridlayouts[CurrentIndex].Width });
                        headerTemplate.Rows[1].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridMinAmount"], RowSpan = 1 });
                        headerTemplate.Rows[2].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridPriceIndex"], RowSpan = 1 });

                        bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.MaxFee), RowSpan = 1, ColSpan = 1, CustomTextFormatDelegate = KoboGridHelper.ToFormatC, AlignCol = AlignColEnum.Right });
                        bodyTemplate.Rows[1].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.MinFee), RowSpan = 1, CustomTextFormatDelegate = KoboGridHelper.ToFormatC, AlignCol = AlignColEnum.Right });
                        bodyTemplate.Rows[2].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.UnitPriceIndex), RowSpan = 1, CustomTextFormatDelegate = KoboGridHelper.Numeric, AlignCol = AlignColEnum.Right });
                    }

                    // ガイド料
                    if ((gridlayouts.Count == 0 && CurrentIndex == 8) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == SuperMenuType1GridHeaderNameConstants.GuideFeeItemNm))
                    {
                        headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridGuideFee"], RowSpan = 1, CodeName = SuperMenuType1GridHeaderNameConstants.GuideFeeItemNm, Width = gridlayouts.Count == 0 ? 150 : gridlayouts[CurrentIndex].Width });
                        headerTemplate.Rows[1].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridGuideFeeTax"], RowSpan = 1 });
                        headerTemplate.Rows[2].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridGuideFeeCommission"], RowSpan = 1 });

                        bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.Guide), RowSpan = 1, ColSpan = 1, CustomTextFormatDelegate = KoboGridHelper.ToFormatC, AlignCol = AlignColEnum.Right });
                        bodyTemplate.Rows[1].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.GuideTax), RowSpan = 1, CustomTextFormatDelegate = KoboGridHelper.ToFormatC, AlignCol = AlignColEnum.Right });
                        bodyTemplate.Rows[2].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.GuideFee), RowSpan = 1, CustomTextFormatDelegate = KoboGridHelper.ToFormatC, AlignCol = AlignColEnum.Right });
                    }

                    // その他付帯
                    if ((gridlayouts.Count == 0 && CurrentIndex == 9) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == SuperMenuType1GridHeaderNameConstants.IncidentalItemNm))
                    {
                        headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridOtherCharge"], CodeName = SuperMenuType1GridHeaderNameConstants.IncidentalItemNm, RowSpan = 1, Width = gridlayouts.Count == 0 ? 150 : gridlayouts[CurrentIndex].Width });
                        headerTemplate.Rows[1].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridOtherChargeTax"], RowSpan = 1 });
                        headerTemplate.Rows[2].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridOtherChargeCommission"], RowSpan = 1 });

                        bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.Other), RowSpan = 1, ColSpan = 1, CustomTextFormatDelegate = KoboGridHelper.ToFormatC, AlignCol = AlignColEnum.Right });
                        bodyTemplate.Rows[1].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.OtherTax), RowSpan = 1, CustomTextFormatDelegate = KoboGridHelper.ToFormatC, AlignCol = AlignColEnum.Right });
                        bodyTemplate.Rows[2].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.OtherFee), RowSpan = 1, CustomTextFormatDelegate = KoboGridHelper.ToFormatC, AlignCol = AlignColEnum.Right });
                    }

                    // 傭車台数
                    if ((gridlayouts.Count == 0 && CurrentIndex == 10) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == SuperMenuType1GridHeaderNameConstants.BusQuantityItemNm))
                    {
                        headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridYousyaNumber"], CodeName = SuperMenuType1GridHeaderNameConstants.BusQuantityItemNm, RowSpan = 3, Width = gridlayouts.Count == 0 ? 150 : gridlayouts[CurrentIndex].Width });

                        bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.NumberOfHiredBusInString), RowSpan = 3, AlignCol = AlignColEnum.Right });
                    }

                    // 傭車運賃
                    if ((gridlayouts.Count == 0 && CurrentIndex == 11) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == SuperMenuType1GridHeaderNameConstants.BusFareItemNm))
                    {
                        headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridYousyaFare"], CodeName = SuperMenuType1GridHeaderNameConstants.BusFareItemNm, RowSpan = 1, Width = gridlayouts.Count == 0 ? 150 : gridlayouts[CurrentIndex].Width });
                        headerTemplate.Rows[1].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridYousyaTax"], RowSpan = 1 });
                        headerTemplate.Rows[2].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridYousyaFareCommission"], RowSpan = 1 });

                        bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.HiredBusFare), RowSpan = 1, ColSpan = 1, CustomTextFormatDelegate = KoboGridHelper.ToFormatC, AlignCol = AlignColEnum.Right });
                        bodyTemplate.Rows[1].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.HiredBusFareTax), RowSpan = 1, CustomTextFormatDelegate = KoboGridHelper.ToFormatC, AlignCol = AlignColEnum.Right });
                        bodyTemplate.Rows[2].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.HiredBusFareFee), RowSpan = 1, CustomTextFormatDelegate = KoboGridHelper.ToFormatC, AlignCol = AlignColEnum.Right });
                    }

                    // 傭車ガイド料
                    if ((gridlayouts.Count == 0 && CurrentIndex == 12) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == SuperMenuType1GridHeaderNameConstants.BusGuildFeeItemNm))
                    {
                        headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridYousyaGuideFee"], CodeName = SuperMenuType1GridHeaderNameConstants.BusGuildFeeItemNm, RowSpan = 1, Width = gridlayouts.Count == 0 ? 150 : gridlayouts[CurrentIndex].Width });
                        headerTemplate.Rows[1].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridYousyaGuideFeeTax"], RowSpan = 1 });
                        headerTemplate.Rows[2].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridYousyaGuideFeeCommission"], RowSpan = 1 });

                        bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.HiredBusGuide), RowSpan = 1, ColSpan = 1, CustomTextFormatDelegate = KoboGridHelper.ToFormatC, AlignCol = AlignColEnum.Right });
                        bodyTemplate.Rows[1].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.HiredBusGuideTax), RowSpan = 1, CustomTextFormatDelegate = KoboGridHelper.ToFormatC, AlignCol = AlignColEnum.Right });
                        bodyTemplate.Rows[2].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.HiredBusGuideFee), RowSpan = 1, CustomTextFormatDelegate = KoboGridHelper.ToFormatC, AlignCol = AlignColEnum.Right });
                    }

                    // その他付帯
                    if ((gridlayouts.Count == 0 && CurrentIndex == 13) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == SuperMenuType1GridHeaderNameConstants.BusIncidentalItemNm))
                    {
                        headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridYousyaOtherCharge"], CodeName = SuperMenuType1GridHeaderNameConstants.BusIncidentalItemNm, RowSpan = 1, Width = gridlayouts.Count == 0 ? 150 : gridlayouts[CurrentIndex].Width });
                        headerTemplate.Rows[1].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridYousyaOtherChargeTax"], RowSpan = 1 });
                        headerTemplate.Rows[2].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridYousyaOtherChargeCommission"], RowSpan = 1 });

                        bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.HiredBusOther), RowSpan = 1, ColSpan = 1, CustomTextFormatDelegate = KoboGridHelper.ToFormatC, AlignCol = AlignColEnum.Right });
                        bodyTemplate.Rows[1].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.HiredBusOtherTax), RowSpan = 1, CustomTextFormatDelegate = KoboGridHelper.ToFormatC, AlignCol = AlignColEnum.Right });
                        bodyTemplate.Rows[2].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.HiredBusOtherFee), RowSpan = 1, CustomTextFormatDelegate = KoboGridHelper.ToFormatC, AlignCol = AlignColEnum.Right });
                    }

                    // 乗車人員, プラス人員
                    if ((gridlayouts.Count == 0 && CurrentIndex == 14) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == SuperMenuType1GridHeaderNameConstants.BusPersonItemNm))
                    {
                        headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridBoardingPerson"], CodeName = SuperMenuType1GridHeaderNameConstants.BusPersonItemNm, RowSpan = 1, Width = gridlayouts.Count == 0 ? 150 : gridlayouts[CurrentIndex].Width });
                        headerTemplate.Rows[1].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridPlusPerson"], RowSpan = 1 });
                        headerTemplate.Rows[2].Columns.Add(new ColumnHeaderTemplate() { ColName = "", RowSpan = 1 });

                        bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.PersonInString), RowSpan = 1, ColSpan = 1, AlignCol = AlignColEnum.Right });
                        bodyTemplate.Rows[1].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.PersonPlusString), RowSpan = 1, AlignCol = AlignColEnum.Right });
                        bodyTemplate.Rows[2].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.EmptyString), RowSpan = 1 });
                    }

                    // 請求区分, 請求年月日
                    if ((gridlayouts.Count == 0 && CurrentIndex == 15) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == SuperMenuType1GridHeaderNameConstants.BillItemNm))
                    {
                        headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridBillingCategory"], CodeName = SuperMenuType1GridHeaderNameConstants.BillItemNm, RowSpan = 1, Width = gridlayouts.Count == 0 ? 150 : gridlayouts[CurrentIndex].Width });
                        headerTemplate.Rows[1].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridBillingDate"], RowSpan = 1 });
                        headerTemplate.Rows[2].Columns.Add(new ColumnHeaderTemplate() { ColName = "", RowSpan = 1 });

                        bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.BillClassification), RowSpan = 1, ColSpan = 1 });
                        bodyTemplate.Rows[1].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.BillDate), RowSpan = 1, CustomTextFormatDelegate = KoboGridHelper.FormatYYYYMMDDDelegate });
                        bodyTemplate.Rows[2].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.EmptyString), RowSpan = 1 });
                    }

                    // 請求書出力年月日
                    if ((gridlayouts.Count == 0 && CurrentIndex == 16) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == SuperMenuType1GridHeaderNameConstants.BillOutputDateItemNm))
                    {
                        headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridBillingExportDate"], CodeName = SuperMenuType1GridHeaderNameConstants.BillOutputDateItemNm, RowSpan = 3, Width = gridlayouts.Count == 0 ? 150 : gridlayouts[CurrentIndex].Width });

                        bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.BillOutputDate), RowSpan = 3, CustomTextFormatDelegate = KoboGridHelper.FormatYYYYMMDDDelegate });
                    }

                    // 運送引受書出力年月日
                    if ((gridlayouts.Count == 0 && CurrentIndex == 17) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == SuperMenuType1GridHeaderNameConstants.DeliveryReceiptOutputDateItemNm))
                    {
                        headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridUnsoHikiukeshoExportDate"], CodeName = SuperMenuType1GridHeaderNameConstants.DeliveryReceiptOutputDateItemNm, RowSpan = 3, Width = gridlayouts.Count == 0 ? 150 : gridlayouts[CurrentIndex].Width });

                        bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.DeliveryReceiptOutputDate), RowSpan = 3, CustomTextFormatDelegate = KoboGridHelper.FormatYYYYMMDDDelegate });
                    }

                    // 受付営業所, 営業担当, 入力担当
                    if ((gridlayouts.Count == 0 && CurrentIndex == 18) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == SuperMenuType1GridHeaderNameConstants.ReceptionItemNm))
                    {
                        headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridRegistrationOffice"], CodeName = SuperMenuType1GridHeaderNameConstants.ReceptionItemNm, RowSpan = 1, Width = gridlayouts.Count == 0 ? 150 : gridlayouts[CurrentIndex].Width });
                        headerTemplate.Rows[1].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridServiceStaff"], RowSpan = 1 });
                        headerTemplate.Rows[2].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridInputStaff"], RowSpan = 1 });

                        bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.ReceptOffice), RowSpan = 1, ColSpan = 1 });
                        bodyTemplate.Rows[1].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.SalesStaff), RowSpan = 1 });
                        bodyTemplate.Rows[2].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.InputStaff), RowSpan = 1 });
                    }

                    // 予約区分, 受付日, 受付番号
                    if ((gridlayouts.Count == 0 && CurrentIndex == 19) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == SuperMenuType1GridHeaderNameConstants.ReservationItemNm))
                    {
                        headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridReservationClassification"], CodeName = SuperMenuType1GridHeaderNameConstants.ReservationItemNm, RowSpan = 1, Width = gridlayouts.Count == 0 ? 150 : gridlayouts[CurrentIndex].Width });
                        headerTemplate.Rows[1].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridReceiptDate"], RowSpan = 1 });
                        headerTemplate.Rows[2].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridReceiptNumber"], RowSpan = 1 });

                        bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.ReserveClassification), RowSpan = 1, ColSpan = 1 });
                        bodyTemplate.Rows[1].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.ReceptionDate), RowSpan = 1, CustomTextFormatDelegate = KoboGridHelper.FormatYYYYMMDDDelegate });
                        bodyTemplate.Rows[2].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.ReceiptNumber), RowSpan = 1 });
                    }

                    // 入力表示
                    if ((gridlayouts.Count == 0 && CurrentIndex == 20) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == SuperMenuType1GridHeaderNameConstants.InputDisplayItemNm))
                    {
                        headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["InputDisplay"], RowSpan = 2, ColSpan = 5, CodeName = SuperMenuType1GridHeaderNameConstants.InputDisplayItemNm, Width = gridlayouts.Count == 0 ? 250 : gridlayouts[CurrentIndex].Width });
                        headerTemplate.Rows[2].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridTimeScheduleMark"], RowSpan = 1 });
                        headerTemplate.Rows[2].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridLoadingGoodsMark"], RowSpan = 1 });
                        headerTemplate.Rows[2].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridArrangementMark"], RowSpan = 1 });
                        headerTemplate.Rows[2].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridNoteMark"], RowSpan = 1 });
                        headerTemplate.Rows[2].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["GridOtherChargeMark"], RowSpan = 1 });

                        bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.JourneyString), RowSpan = 3, ColSpan = 1, AlignCol = AlignColEnum.Center });
                        bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.LoadString), RowSpan = 3, ColSpan = 1, AlignCol = AlignColEnum.Center });
                        bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.ArrangeString), RowSpan = 3, ColSpan = 1, AlignCol = AlignColEnum.Center });
                        bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.RemarksString), RowSpan = 3, ColSpan = 1, AlignCol = AlignColEnum.Center });
                        bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuReservationData.IncidentalString), RowSpan = 3, ColSpan = 1, AlignCol = AlignColEnum.Center });
                        if (gridlayouts.Count == 0)
                        {
                            break;
                        }
                    }
                    CurrentIndex++;
                    if (gridlayouts.Count != 0 && CurrentIndex >= gridlayouts.Count)
                    {
                        break;
                    }
                }
                Header = headerTemplate;
                Body = bodyTemplate;
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected IEnumerable<string> GetBusesInfo(SuperMenuReservationData item)
        {
            if (item != null)
            {
                foreach (BusInfo busInfo in item.BusesInfo)
                {
                    yield return busInfo.BusType + "/" + busInfo.NumberOfBuses + Lang["BusUnit"];
                }
            }
        }

        protected void ChangeTotal()
        {
            try
            {
                if (CurrentTotalContent == Lang["Order"])
                {
                    CurrentTotalContent = Lang["Company"];
                    CurrentTotal = GridTotalDatas.Company;
                }
                else if (CurrentTotalContent == Lang["Company"])
                {
                    CurrentTotalContent = Lang["Bus"];
                    CurrentTotal = GridTotalDatas.Bus;
                }
                else
                {
                    CurrentTotalContent = Lang["Order"];
                    CurrentTotal = GridTotalDatas.Order;
                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected int[] GetPagination()
        {
            try
            {
                if (NumberOfPage <= MaxPageCount)
                {
                    return Enumerable.Range(0, NumberOfPage).ToArray();
                }
                else
                {
                    int BeginIndex = ((int)Math.Floor(CurrentPage * 1.0 / MaxPageCount)) * MaxPageCount;
                    int Count = Math.Min(MaxPageCount, NumberOfPage - BeginIndex);
                    return Enumerable.Range(BeginIndex, Count).ToArray();
                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
                return new int[] { };
            }
        }

        protected async void SelectPage(int index)
        {
            try
            {
                await _loadingService.ShowAsync();
                List<SuperMenuReservationData> TempGridData = GridDatas.GetRange(index * RecordsPerPage, Math.Min(RecordsPerPage, GridDatas.Count() - index * RecordsPerPage));
                CurrentPage = index;
                if (CurrentPage != FirstPageSelect)
                {
                    FirstPageSelect = CurrentPage;
                    await FirstPageSelectChanged.InvokeAsync(FirstPageSelect);
                }

                if (TempGridData.Count() > 0 && (TempGridData[0] == null || isChangePageNumber))
                {
                    GetPageData(index).Wait();
                    JSRuntime.InvokeVoidAsync("scrollToTop");
                }
                else
                {
                    GridDisplay = TempGridData;
                    DataItems = GridDisplay;
                    if (index == 0 && paging != null)
                    {
                        paging.TotalCount = GridDatas.Count();
                        paging.currentPage = index;
                    }
                    StandardizedDataForGrid(index);
                }
                InvokeAsync(StateHasChanged).Wait();
                await _loadingService.HideAsync();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected void FirstLoad()
        {
            try
            {
                int index = 0; ;
                List<SuperMenuReservationData> TempGridData = GridDatas.GetRange(index * RecordsPerPage, Math.Min(RecordsPerPage, GridDatas.Count() - index * RecordsPerPage));
                GridDisplay = TempGridData;
                DataItems = GridDisplay;
                StandardizedDataForGrid(index);
                CurrentPage = index;
                if (index == 0 && paging != null)
                {
                    paging.TotalCount = GridDatas.Count();
                    paging.currentPage = index;
                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup(Lang["Error"], ex.GetOriginalException()?.Message);
            }
        }

        protected async Task GetPageData(int PageNo)
        {
            try
            {
                if (isChangePageNumber)
                {
                    isChangePageNumber = false;
                    FirstPageSelect = 0;
                    PageNo = 0;
                    paging.currentPage = PageNo;
                }
                PageNo = paging.currentPage;
                int OffSet = PageNo * RecordsPerPage;
                List<SuperMenuReservationData> PageData = await HyperDataService.GetSuperMenuReservationData(hyperData, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, new ClaimModel().TenantID, OffSet, RecordsPerPage);
                if (GridDatas.Count >= PageData.Count())
                {
                    GridDatas.RemoveRange(OffSet, PageData.Count());
                    GridDatas.InsertRange(OffSet, PageData);
                }
                else
                {
                    GetPageData(PageNo);
                }
                GridDisplay = PageData;
                CurrentPage = PageNo;
                DataItems = GridDisplay;
                StandardizedDataForGrid(PageNo);
                paging.TotalCount = GridDatas.Count();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected void StandardizedDataForGrid(int index)
        {
            try
            {
                int count = index * RecordsPerPage + 1;
                if (GridDisplay.Count > 0 && GridDisplay[0] != null)
                {
                    foreach (var item in GridDisplay)
                    {
                        if (item != null)
                        {
                            item.No = count;
                            item.NumberOfHiredBusInString = item.NumberOfHiredBus.ToString() + " " + Lang["BusUnit"];
                            item.PersonInString = item.Person.ToString() + " " + Lang["PeopleUnit"];
                            item.ArrangeString = item.Arrange ? "⚪" : string.Empty;
                            item.RemarksString = item.Remarks ? "⚪" : string.Empty;
                            item.JourneyString = item.Journey ? "⚪" : string.Empty;
                            item.LoadString = item.Load ? "⚪" : string.Empty;
                            item.IncidentalString = item.Incidental ? "⚪" : string.Empty;
                            item.PersonPlusString = item.PersonPlus + Lang["PeopleUnit"];
                            item.NumberOfDriverNumberOfGuideString = Lang["Driver"] + item.NumberOfDrivers.ToString() + Lang["DriverUnit"] + "/" + Lang["Guide"] + item.NumberOfGuides.ToString() + Lang["DriverUnit"];
                            item.EmptyString = string.Empty;
                            count++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        async Task HandleMouseWheel()
        {
            try
            {
                await blazorContextMenuService.HideMenu("gridRowClickMenu");
                await blazorContextMenuService.HideMenu("gridRowsClickMenu");
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        [JSInvokable]
        public async Task<int> OnKeyDown(int KeyCode, bool IsShift)
        {
            try
            {
                int IndexResult = -1;
                if (CheckedItems.Count == 0)
                {
                    return IndexResult;
                }
                if (KeyCode == 38 && !IsShift && CheckedItems.Count == 1 && CheckedItems[0].No > GridDisplay[0].No)
                {
                    CheckedItems = new List<SuperMenuReservationData>() { DataItems.FirstOrDefault(item => item.No == CheckedItems[0].No - 1) };
                    CurrentClick = CheckedItems[0].No;
                    IndexResult = CheckedItems[0].No;
                }
                if (KeyCode == 40 && !IsShift && CheckedItems.Count == 1 && CheckedItems[0].No < GridDisplay[GridDisplay.Count - 1].No)
                {
                    CheckedItems = new List<SuperMenuReservationData>() { DataItems.FirstOrDefault(item => item.No == CheckedItems[0].No + 1) };
                    CurrentClick = CheckedItems[0].No;
                    IndexResult = CheckedItems[0].No;
                }
                if (isClickRow || (IsShift && (KeyCode == 38 || KeyCode == 40)))
                {
                    isClickRow = false;
                    if (KeyCode == 38 && IsShift)
                    {
                        int NextIndex = CurrentScroll == null ? (int)CurrentClick - 1 : (int)CurrentScroll - 1;
                        if (NextIndex >= GridDisplay[0].No)
                        {
                            CurrentScroll = NextIndex;
                            IndexResult = (int)CurrentScroll;
                        }
                        if (CurrentScroll >= (int)CurrentClick)
                        {
                            CheckedItems.RemoveAll(item => item.No == CurrentScroll + 1);
                        }
                        else if (!CheckedItems.Any(item => item.No == (int)CurrentScroll))
                        {
                            CheckedItems.Add(DataItems.FirstOrDefault(item => item.No == (int)CurrentScroll));
                        }
                    }
                    else if (KeyCode == 40 && IsShift)
                    {
                        int NextIndex = CurrentScroll == null ? (int)CurrentClick + 1 : (int)CurrentScroll + 1;
                        if (NextIndex < GridDisplay[GridDisplay.Count - 1].No)
                        {
                            CurrentScroll = NextIndex;
                            IndexResult = (int)CurrentScroll;
                        }
                        if (CurrentScroll <= (int)CurrentClick)
                        {
                            CheckedItems.RemoveAll(item => item.No == CurrentScroll - 1);
                        }
                        else if (!CheckedItems.Any(item => item.No == (int)CurrentScroll))
                        {
                            CheckedItems.Add(DataItems.FirstOrDefault(item => item.No == (int)CurrentScroll));
                        }

                    }
                }
                StateHasChanged();
                await blazorContextMenuService.HideMenu("gridRowClickMenu");
                await blazorContextMenuService.HideMenu("gridRowsClickMenu");
                return IndexResult;
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
                return -1;
            }
        }

        [JSInvokable]
        public async Task KeyUpComplete()
        {
            try
            {
                if (CheckedItems.Count == 0)
                {
                    return;
                }
                string MenuNameToShow = "gridRowClickMenu";
                string MenuNameToHide = "gridRowsClickMenu";
                if (CheckedItems.Count > 1)
                {
                    string temp = MenuNameToShow;
                    MenuNameToShow = MenuNameToHide;
                    MenuNameToHide = temp;
                }
                await blazorContextMenuService.HideMenu(MenuNameToHide);
                await blazorContextMenuService.ShowMenu(MenuNameToShow, LastXClicked, LastYClicked);
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected void ComfirmCancelReservation()
        {
            ShowComfirmDeletePopup = true;
        }

        protected async void CancelReservation()
        {
            try
            {
                ShowComfirmDeletePopup = false;
                var canceled = false;
                await _loadingService.ShowAsync();
                bool isHaita = await HaitaCheck(CheckedItems);
                if (!isHaita)
                {
                    await AlertHaita();
                    await _loadingService.HideAsync();
                    return;
                }
                await Task.Run(() =>
                {
                    InvokeAsync(StateHasChanged).Wait();
                    GetDataReservationToCheck().Wait();
                    if (InfoMessage.ContainsKey(""))
                    {
                        ShowPopup = HyperDataService.CancelRevervation(InfoMessage[""], ReservertionData).Result;
                        canceled = ShowPopup;
                    }
                    else
                    {
                        ShowPopup = true;
                        canceled = true;
                    }
                    InvokeAsync(StateHasChanged);
                });
                await _loadingService.HideAsync();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected async void ConfirmReservation()
        {
            try
            {
                await _loadingService.ShowAsync();
                StateHasChanged();
                bool isHaita = await HaitaCheck(CheckedItems);
                if (!isHaita)
                {
                    await AlertHaita();
                    await _loadingService.HideAsync();
                    return;
                }
                var confirm = false;
                List<string> SelectedReceiptNumber = new List<string>();
                foreach (SuperMenuReservationData SelectRow in CheckedItems)
                {
                    SelectedReceiptNumber.Add(SelectRow.UkeNo);
                }
                confirm = await HyperDataService.ConfirmRevervation(SelectedReceiptNumber);
                await ChangeState.InvokeAsync(CurrentPage);
                await _loadingService.HideAsync();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected async Task AlertHaita()
        {
            PopupUploadFile = false;
            await ValueCheckHaita.InvokeAsync(false);
        }

        protected async Task<bool> HaitaCheck(List<SuperMenuReservationData> DatasToCheck)
        {
            List<HaitaModelCheck> HaitaModelsToCheck = new List<HaitaModelCheck>();
            foreach (SuperMenuReservationData DataToCheck in DatasToCheck)
            {
                HaitaModelsToCheck.Add(new HaitaModelCheck()
                {
                    TableName = "TKD_Yyksho",
                    CurrentUpdYmdTime = DataToCheck.YykshoUpdYmdTime,
                    PrimaryKeys = new List<PrimaryKeyToCheck>(new PrimaryKeyToCheck[] { new PrimaryKeyToCheck() { PrimaryKey = "UkeNo =", Value = "'" + DataToCheck.UkeNo + "'" } })
                });
            }
            return await HaitaCheckService.GetHaitaCheck(HaitaModelsToCheck);
        }

        protected async Task GetDataReservationToCheck()
        {
            try
            {
                InfoMessage = new Dictionary<string, List<string>>();
                List<string> SelectedReceiptNumber = new List<string>();
                foreach (SuperMenuReservationData SelectRow in CheckedItems)
                {
                    SelectedReceiptNumber.Add(SelectRow.UkeNo);
                }
                ReservertionData = await HyperDataService.GetDataReservationToCheck(SelectedReceiptNumber);
                CheckReservationToBeCancel();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected void CheckReservationToBeCancel()
        {
            try
            {
                foreach (ReservationDataToCheck Data in ReservertionData)
                {
                    string Error = "";
                    if (Data.CompanySeq != new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID)
                    {
                        Error = Lang["BelongToOtherCompanyMessage"];
                    }
                    else if (Data.NippoKbn != 1)
                    {
                        Error = Lang["DailyReportEnteredMessage"];
                    }
                    else if (Data.DepositClassificationStatus)
                    {
                        Error = Lang["DepositEnteredMessage"];
                    }
                    else if (Data.PaymentClassificationStatus)
                    {
                        Error = Lang["BI_T026"];
                    }
                    else if (Data.ClosedStatus)
                    {
                        Error = Lang["ClosedMessage"];
                    }
                    if (!InfoMessage.ContainsKey(Error))
                    {
                        InfoMessage.Add(Error, new List<string>());
                    }
                    InfoMessage[Error].Add(Data.ReceiptNumber);
                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected async void ItemClick(ItemClickEventArgs e)
        {
            try
            {
                string baseUrl = AppSettingsService.GetBaseUrl() + "";
                string url = "";
                string UkeNo = CheckedItems[0].UkeNo;
                ObjectPram temp = new ObjectPram();
                temp.key = UkeNo;
                string UkeCdPram = EncryptHelper.EncryptToUrl(temp);
                List<string> SelectedReceiptNumber = new List<string>();
                List<string> SelectedUnkRen = new List<string>();
                foreach (SuperMenuReservationData SelectRow in CheckedItems)
                {
                    SelectedReceiptNumber.Add(SelectRow.UkeNo);
                    SelectedUnkRen.Add(SelectRow.UnkRen.ToString());
                }
                string resultUkeNo = "";
                resultUkeNo = getInfoListUkeNo(CheckedItems);
                OutDataTable lstPrint = new OutDataTable { UkeNo = UkeNo, supOutSiji = 2 };
                switch (e.MenuItem.Id)
                {
                    case "menuitem-reservation-single-copy":
                        url = baseUrl + "/bookinginput";
                        url = url + string.Format("/?UkeCd={0}&action=copy", UkeNo.Substring(5));
                        await JSRuntime.InvokeVoidAsync("open", url, "_blank");
                        break;
                    case "menuitem-reservation-calendar-copy":
                        url = baseUrl + string.Format("/bookinginputmulticopy/?UkeCd={0}", UkeNo.Substring(5));
                        await JSRuntime.InvokeVoidAsync("open", url, "_blank");
                        break;
                    case "download_report_under_writing_arriage":
                        await _loadingService.ShowAsync();
                        NoDataType = "TransportationReport1";
                        DataNotExistPopup = HikiukeshoReportService.transportationContractReport(OptionReport.Download.ToString(), SelectedReceiptNumber, SelectedUnkRen, "1", JSRuntime, YoyKbnService, 1).Result;
                        await _loadingService.HideAsync();
                        break;
                    case "preview_report_under_writing_arriage":
                        NoDataType = "TransportationReport1";
                        DataNotExistPopup = HikiukeshoReportService.transportationContractReport(OptionReport.Preview.ToString(), SelectedReceiptNumber, SelectedUnkRen, "1", JSRuntime, YoyKbnService, 1).Result;
                        break;
                    case "download_report_under_writing_transportation":
                        await _loadingService.ShowAsync();
                        NoDataType = "TransportationReport2";
                        DataNotExistPopup = HikiukeshoReportService.transportationContractReport(OptionReport.Download.ToString(), SelectedReceiptNumber, SelectedUnkRen, "2", JSRuntime, YoyKbnService, 1).Result;
                        await _loadingService.HideAsync();
                        break;
                    case "preview_report_under_writing_transportation":
                        NoDataType = "TransportationReport2";
                        DataNotExistPopup = HikiukeshoReportService.transportationContractReport(OptionReport.Preview.ToString(), SelectedReceiptNumber, SelectedUnkRen, "2", JSRuntime, YoyKbnService, 1).Result;
                        break;
                    case "download_advance_statement":
                        await _loadingService.ShowAsync();
                        NoDataType = "AdvanceReport";
                        DataNotExistPopup = advancePaymentDetailsService.AdvancePaymentDetailReport(OptionReport.Download.ToString(), SelectedReceiptNumber, JSRuntime, reportLayoutSettingService).Result;
                        await _loadingService.HideAsync();
                        break;
                    case "preview_advance_statement":
                        NoDataType = "AdvanceReport";
                        DataNotExistPopup = advancePaymentDetailsService.AdvancePaymentDetailReport(OptionReport.Preview.ToString(), SelectedReceiptNumber, JSRuntime, reportLayoutSettingService).Result;
                        break;
                    case "menuitem-deposit-coupon-input":
                        url = baseUrl + "/depositcoupon";
                        url = url + string.Format("/?UkeCdParam={0}", UkeCdPram);
                        JSRuntime.InvokeVoidAsync("open", url, "_blank");
                        break;
                    case "menuitem-coupon-settlement-input":
                        url = baseUrl + "/couponpayment";
                        url = url + string.Format("/?UkeCdParam={0}", UkeCdPram);
                        JSRuntime.InvokeVoidAsync("open", url, "_blank");
                        break;
                    case "menuitem-busallocation":
                        url = baseUrl + "/busallocation";
                        url = url + string.Format("/?date={0}&bookingid={1}", CheckedItems[0].HaiSYmd, UkeNo.Substring(5));
                        JSRuntime.InvokeVoidAsync("open", url, "_blank");
                        break;
                    case "menuitem-partnerbookinginput":
                        url = baseUrl + "/partnerbookinginput";
                        url = url + string.Format("/?UkeCd={0}&UnkRen={1}", CheckedItems[0].UkeNo.Substring(5), CheckedItems[0].UnkRen);
                        JSRuntime.InvokeVoidAsync("open", url, "_blank");
                        break;
                    case "download_report_multi_under_writing_arriage":
                        await _loadingService.ShowAsync();
                        NoDataType = "TransportationReport1";
                        DataNotExistPopup = HikiukeshoReportService.transportationContractReport(OptionReport.Download.ToString(), SelectedReceiptNumber, SelectedUnkRen, "1", JSRuntime, YoyKbnService, 1).Result;
                        await _loadingService.HideAsync();
                        break;
                    case "preview_report_multi_under_writing_arriage":
                        NoDataType = "TransportationReport1";
                        DataNotExistPopup = HikiukeshoReportService.transportationContractReport(OptionReport.Preview.ToString(), SelectedReceiptNumber, SelectedUnkRen, "1", JSRuntime, YoyKbnService, 1).Result;
                        break;
                    case "download_multi_report_under_writing_transportation":
                        await _loadingService.ShowAsync();
                        NoDataType = "TransportationReport2";
                        DataNotExistPopup = HikiukeshoReportService.transportationContractReport(OptionReport.Download.ToString(), SelectedReceiptNumber, SelectedUnkRen, "2", JSRuntime, YoyKbnService, 1).Result;
                        await _loadingService.HideAsync();
                        break;
                    case "preview_multi_report_under_writing_transportation":
                        NoDataType = "TransportationReport2";
                        DataNotExistPopup = HikiukeshoReportService.transportationContractReport(OptionReport.Preview.ToString(), SelectedReceiptNumber, SelectedUnkRen, "2", JSRuntime, YoyKbnService, 1).Result;
                        break;
                    case "download_multi_advance_statement":
                        await _loadingService.ShowAsync();
                        NoDataType = "TransportationReport2";
                        DataNotExistPopup = advancePaymentDetailsService.AdvancePaymentDetailReport(OptionReport.Download.ToString(), SelectedReceiptNumber, JSRuntime, reportLayoutSettingService).Result;
                        await _loadingService.HideAsync();
                        break;
                    case "preview_multi_advance_statement":
                        NoDataType = "TransportationReport2";
                        DataNotExistPopup = advancePaymentDetailsService.AdvancePaymentDetailReport(OptionReport.Preview.ToString(), SelectedReceiptNumber, JSRuntime, reportLayoutSettingService).Result;
                        break;
                    case "preview_invoice":
                        OutDataTableModel outDataTableModel = new OutDataTableModel() { outDataTables = new List<OutDataTable>() { lstPrint } };
                        var billParams = EncryptHelper.EncryptToUrl(outDataTableModel);
                        url = baseUrl + "/billprint";
                        url = url + string.Format("/?lstInfo={0}", billParams);
                        JSRuntime.InvokeVoidAsync("open", url, "_blank");
                        break;
                    case "booking-input-edit":
                        url = baseUrl + "/bookinginput";
                        url = url + string.Format("/?UkeCd={0}", UkeNo.Substring(5));
                        JSRuntime.InvokeVoidAsync("open", url, "_blank");
                        break;
                    case "menuitem-reservation-cancellation":
                        if (!string.IsNullOrEmpty(UkeNo))
                        {
                            IsOpenCancelTab = true;
                        }
                        break;
                    case "download_report_operating_instructions":
                        await _loadingService.ShowAsync();
                        UnkoushijishoReportService.ExportReportAsPdf(UkeNo, 1, CheckedItems[0].UnkRen, 1, JSRuntime);
                        await _loadingService.HideAsync();
                        break;
                    case "preview_report_operating_instructions":
                        url = baseUrl + "/OperatingInstructionReportPreviewNew";
                        url = url + string.Format("/?Option={0}&BookingID={1}&TeiDanNo={2}&UnkRen={3}&BunkRen={4}&Mode={5}&IsLoadDefault={6}", OptionReport.Preview, UkeNo, 1, CheckedItems[0].UnkRen, 1, 1, 1);
                        JSRuntime.InvokeVoidAsync("open", url, "_blank");
                        break;
                    case "download_report_flight_record_book":
                        await _loadingService.ShowAsync();
                        UnkoushijishoReportService.ExportReportdriAsPdf(UkeNo, 1, CheckedItems[0].UnkRen, 1, JSRuntime);
                        await _loadingService.HideAsync();
                        break;
                    case "preview_report_flight_record_book":
                        url = baseUrl + "/OperatingInstructionReportPreviewNew";
                        url = url + string.Format("/?Option={0}&BookingID={1}&TeiDanNo={2}&UnkRen={3}&BunkRen={4}&Mode={5}&IsLoadDefault={6}", OptionReport.Preview, UkeNo, 1, CheckedItems[0].UnkRen, 1, 2, 1);
                        JSRuntime.InvokeVoidAsync("open", url, "_blank");
                        break;
                    case "menuitem-confirmation":
                        IsOpenConfirmTab = true;
                        return;
                    case "menuitem-edityykshobikonm":
                        IsOpenEditYykshoBikoNm = true;
                        return;
                    case "menuitem-editunkobibikonm":
                        IsOpenEditUnkobiBikoNm = true;
                        return;
                    case "download_multi_report_operating_instructions":
                        await _loadingService.ShowAsync();
                        UnkoushijishoReportService.ExportReportdriUkelistAsPdf(1, resultUkeNo, 1, JSRuntime);
                        await _loadingService.HideAsync();
                        break;
                    case "preview_multi_report_operating_instructions":
                        url = baseUrl + "/OperatingInstructionReportPreviewNew";
                        url = url + string.Format("/?Option={0}&UkenoList={1}&FormOutput={2}&Mode={3}&IsLoadDefault={4}", OptionReport.Preview, resultUkeNo, 1, 1, 1);
                        JSRuntime.InvokeVoidAsync("open", url, "_blank");
                        break;
                    case "download_multi_report_flight_record_book":
                        await _loadingService.ShowAsync();
                        UnkoushijishoReportService.ExportReportdriUkelistAsPdf(2, resultUkeNo, 1, JSRuntime);
                        await _loadingService.HideAsync();
                        break;
                    case "preview_multi_report_flight_record_book":
                        url = baseUrl + "/OperatingInstructionReportPreviewNew";
                        url = url + string.Format("/?Option={0}&UkenoList={1}&FormOutput={2}&Mode={3}&IsLoadDefault={4}", OptionReport.Preview, resultUkeNo, 1, 2, 1);
                        JSRuntime.InvokeVoidAsync("open", url, "_blank");
                        break;
                    case "download_simple_quotation":
                        await _loadingService.ShowAsync();
                        NoDataType = "SimpleQuotation";
                        SimpleQuotationReportFilter param = await simpleQuotationServic.SetParamForSimpleQuotation(UkeNo.Substring(5));
                        if (param != null)
                        {
                            DataNotExistPopup = await simpleQuotationServic.ExportAsPDF(param, JSRuntime);
                        }
                        else
                        {
                            DataNotExistPopup = true;
                        }
                        await _loadingService.HideAsync();
                        break;
                    case "preview_simple_quotation":
                        NoDataType = "SimpleQuotation";
                        SimpleQuotationReportFilter param1 = await simpleQuotationServic.SetParamForSimpleQuotation(UkeNo.Substring(5));
                        DataNotExistPopup = (param1 == null || param1.BookingKeyList == null) ? true : false;
                        if (!DataNotExistPopup)
                        {
                            var searchString = EncryptHelper.EncryptToUrl(param1);
                            JSRuntime.InvokeVoidAsync("open", "SimpleQuotationReportPreview?searchString=" + searchString, "_blank");
                        }
                        break;
                    case "download_quotation_itinerary":
                        await _loadingService.ShowAsync();
                        NoDataType = "SimpleQuotation";
                        SimpleQuotationReportFilter param2 = await simpleQuotationServic.SetParamForQuotationJourney(UkeNo.Substring(5));
                        if (param2 == null)
                        {
                            DataNotExistPopup = true;
                        }
                        else
                        {
                            DataNotExistPopup = await simpleQuotationServic.ExportAsPDF(param2, JSRuntime);
                        }
                        await _loadingService.HideAsync();
                        break;
                    case "preview_quotation_itinerary":
                        NoDataType = "SimpleQuotation";
                        SimpleQuotationReportFilter param3 = await simpleQuotationServic.SetParamForQuotationJourney(UkeNo.Substring(5));
                        DataNotExistPopup = (param3 == null || param3.BookingKeyList == null) ? true : false;
                        if (!DataNotExistPopup)
                        {
                            var searchString1 = EncryptHelper.EncryptToUrl(param3);
                            JSRuntime.InvokeVoidAsync("open", "SimpleQuotationReportPreview?searchString=" + searchString1, "_blank");
                        }
                        break;
                }
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        public async void ItemClickBusCooradinationReport(ItemClickEventArgs e)
        {
            try
            {
                string baseUrl = AppSettingsService.GetBaseUrl();
                BusCoordinationSearchParam report = new BusCoordinationSearchParam();
                string url = baseUrl + "";
                switch (e.MenuItem.Id)
                {
                    case "BusCoooradination_Preview":
                        url = url + string.Format("/buscoordinationreportpreview/?Option={0}&UnkobiDate={1}&Ukeno={2}&UnkRen={3}&IsLoadDefault=1", OptionReport.Preview, CheckedItems[0].HaiSYmd, CheckedItems[0].UkeNo.Substring(5), CheckedItems[0].UnkRen.ToString());
                        JSRuntime.InvokeVoidAsync("open", url, "_blank");
                        break;
                    case "BusCoooradination_Download":
                        await _loadingService.ShowAsync();
                        BusCoordinationReportService.ExportPdfFiles(CheckedItems[0].HaiSYmd, CheckedItems[0].UkeNo.Substring(5), CheckedItems[0].UnkRen.ToString(), JSRuntime);
                        await _loadingService.HideAsync();
                        break;
                }
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                await _loadingService.HideAsync();
                await InvokeAsync(StateHasChanged);
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected string getInfoListUkeNo(List<SuperMenuReservationData> _checkedItems)
        {
            string resultUkeNo = "";
            _checkedItems = _checkedItems.DistinctBy(p => new { p.UkeNo, p.UnkRen }).ToList();
            foreach (SuperMenuReservationData item in _checkedItems)
            {
                resultUkeNo += item.UkeNo + item.UnkRen.ToString("000") + ",";
            }
            if (!string.IsNullOrEmpty(resultUkeNo))
            {
                resultUkeNo = resultUkeNo.Substring(0, resultUkeNo.LastIndexOf(","));
            }
            return resultUkeNo;
        }

        public async void MultiItemClickBusCooradinationReport(ItemClickEventArgs e)
        {
            try
            {
                string resultUkeNo = "";
                resultUkeNo = getInfoListUkeNo(CheckedItems);
                string baseUrl = AppSettingsService.GetBaseUrl();
                BusCoordinationSearchParam report = new BusCoordinationSearchParam();
                string searchString = EncryptHelper.EncryptToUrl(report);
                string url = baseUrl + "";
                switch (e.MenuItem.Id)
                {
                    case "BusCoooradination_multi_Preview":
                        url = url + string.Format("/buscoordinationreportpreview/?Option={0}&UkenoList={1}&FormOutput={2}&IsLoadDefault=1", OptionReport.Preview, resultUkeNo, "1");
                        JSRuntime.InvokeVoidAsync("open", url, "_blank");
                        break;
                    case "BusCoooradination_multi_Download":
                        await _loadingService.ShowAsync();
                        BusCoordinationReportService.ExportPdfFilesbyUkenoLst(resultUkeNo, 1, JSRuntime);
                        await _loadingService.HideAsync();
                        break;
                }
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                await _loadingService.HideAsync();
                await InvokeAsync(StateHasChanged);
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected void OpenFutai()
        {
            try
            {
                currentUkeNo = CheckedItems[0].UkeNo;
                PopupFutai = true;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected void OpenJourneys()
        {
            try
            {
                currentUkeNo = CheckedItems[0].UkeNo;
                currentunkRen = (short)CheckedItems[0].UnkRen;
                PopupJourney = true;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected void OpenTehai()
        {
            try
            {
                getCurrentUkeCd();
                PopupTehai = true;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected void OpenTsumi()
        {
            try
            {
                getCurrentUkeCd();
                PopupTsumi = true;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        public void OpenBusAllocation(ItemClickEventArgs e)
        {
            string baseUrl = AppSettingsService.GetBaseUrl();
            string url = baseUrl + "/busallocation";
            switch (e.MenuItem.Id)
            {
                case "menuitem-temporary-car-input":
                    url = url + string.Format("/?date={0}&bookingid={1}&HaiSKbn={2}", CheckedItems[0].HaiSYmd, CheckedItems[0].UkeNo.Substring(5), CheckedItems[0].HaiSKbn);
                    JSRuntime.InvokeVoidAsync("open", url, "_blank");
                    break;
                case "menuitem-busallocation":
                    url = url + string.Format("/?date={0}&bookingid={1}", CheckedItems[0].HaiSYmd, CheckedItems[0].UkeNo.Substring(5));
                    JSRuntime.InvokeVoidAsync("open", url, "_blank");
                    break;
            }
        }

        protected async Task OpenFareFeeCorrection()
        {
            try
            {
                getCurrentUkeCd();
                PopupFareFeeCorrection = true;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        public void OpenUploadFile()
        {
            try
            {
                getCurrentUkeCd();
                CurrentBookingData.UkeNo = currentUkeNo;
                CurrentBookingData.UnkRen = currentunkRen;
                CurrentBookingData.UnkobiFileUpdYmdTime = string.IsNullOrWhiteSpace(CheckedItems[0].UnkobiFileUpdYmdTime) ? null : CheckedItems[0].UnkobiFileUpdYmdTime;
                PopupUploadFile = true;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected async Task NavigateToDailyBatchCopy()
        {
            try
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                StringBuilder sb = new StringBuilder();
                foreach (var item in CheckedItems)
                {
                    sb.Append(item.UkeNo + ",");
                }
                dict.Add("listUkeNo", sb.ToString());
                await JSRuntime.InvokeVoidAsync("open", "dailybatchcopy?searchString=" + EncryptHelper.EncryptToUrl(dict), "_blank");
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected void getCurrentUkeCd()
        {
            try
            {
                currentUkeNo = CheckedItems[0].UkeNo;
                currentunkRen = (short)CheckedItems[0].UnkRen;
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected async void refreshGrid()
        {
            try
            {
                if (PopupFutai)
                    PopupFutai = false;
                if (PopupTehai)
                    PopupTehai = false;
                if (PopupJourney)
                    PopupJourney = false;
                if (PopupTsumi)
                    PopupTsumi = false;
                if (PopupFareFeeCorrection)
                    PopupFareFeeCorrection = false;
                if (ShowPopup)
                    ShowPopup = false;
                if (PopupUploadFile)
                {
                    PopupUploadFile = false;
                }
                StateHasChanged();
                await _loadingService.ShowAsync();
                await ChangeState.InvokeAsync(CurrentPage);
                await _loadingService.HideAsync();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected async void ChangePageSize(byte value)
        {
            try
            {
                isChangePageNumber = true;
                RecordsPerPage = value;
                await RecordsPerPageChanged.InvokeAsync(RecordsPerPage);
                await FirstPageSelectChanged.InvokeAsync(FirstPageSelect);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected void OnCloseFareFeeCorrection(bool value)
        {
            PopupFareFeeCorrection = value;
            refreshGrid();
        }
    }
}
