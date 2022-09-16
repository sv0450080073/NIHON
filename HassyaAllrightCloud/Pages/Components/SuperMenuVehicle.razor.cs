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
namespace HassyaAllrightCloud.Pages.Components
{
    public class SuperMenuVehicleBase : ComponentBase
    {
        #region parameter
        [Parameter] public List<SuperMenuVehicleData> GridDatas { get; set; }
        [Parameter] public SuperMenuVehicleTotalGridData GridTotalDatas { get; set; }
        [Parameter] public int ActiveV { get; set; }
        [Parameter] public byte RecordsPerPage { get; set; } = (byte)25;
        [Parameter] public HyperFormData hyperData { get; set; }
        [Parameter] public EventCallback<int> ChangeState { get; set; }
        [Parameter] public int FirstPageSelect { get; set; }
        [Parameter] public EventCallback<int> FirstPageSelectChanged { get; set; }
        [Parameter] public EventCallback<byte> RecordsPerPageChanged { get; set; }
        [Parameter] public bool isChangePageNumber { get; set; }
        [Parameter] public Pagination paging { get; set; } = new Pagination();
        [Parameter] public EventCallback<bool> ValueCheckHaita { get; set; }
        [Parameter] public bool isInitComplete { get; set; } = false;
        [Parameter] public bool isFirstInit { get; set; } = true;
        [Parameter] public bool isChangeValue { get; set; } = false;
        [Parameter] public EventCallback<bool> isChangeValueChanged { get; set; }
        [Parameter] public bool loading { get; set; }
        [Parameter] public EventCallback<bool> loadingChanged { get; set; }
        [Parameter] public bool isReloadTotal { get; set; } = false;
        [Parameter] public EventCallback<bool> isReloadTotalChanged { get; set; }
        #endregion

        #region Inject
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected IBlazorContextMenuService blazorContextMenuService { get; set; }
        [Inject] protected CustomNavigation NavigationManager { get; set; }
        [Inject] protected IHyperDataService HyperDataService { get; set; }
        [Inject] protected IStringLocalizer<SuperMenu> Lang { get; set; }
        [Inject] protected IErrorHandlerService errorModalService { get; set; }
        [Inject] protected AppSettingsService AppSettingsService { get; set; }
        [Inject] protected IHikiukeshoReportService HikiukeshoReportService { get; set; }
        [Inject] protected IAdvancePaymentDetailsService advancePaymentDetailsService { get; set; }
        [Inject] protected IReportLayoutSettingService reportLayoutSettingService { get; set; }
        [Inject] protected ISimpleQuotationService simpleQuotationServic { get; set; }
        [Inject] protected ILoadingService _loadingService { get; set; }
        [Inject] protected IBusCoordinationReportService BusCoordinationReportService { get; set; }
        [Inject] protected IUnkoushijishoReportService UnkoushijishoReportService { get; set; }
        [Inject] protected IFareFeeCorrectionService _fareFeeCorrectionService { get; set; }
        [Inject] protected IBookingTypeListService YoyKbnService { get; set; }
        [Inject] protected IVenderRequestService VenderRequestService { get; set; }
        #endregion

        #region variable
        public List<SuperMenuVehicleData> GridDisplay { get; set; }
        public SuperMenuVehicleTotalData CurrentTotal { get; set; } = new SuperMenuVehicleTotalData();
        public string CurrentTotalContent;
        public List<int> CurrentRow = new List<int>();
        public bool isClickRow { get; set; } = false;
        public int LastXClicked { get; set; }
        public int LastYClicked { get; set; }
        public int? CurrentClick { get; set; } = null;
        public int? CurrentScroll { get; set; }
        public int MaxPageCount = 5;
        public int CurrentPage = 0;
        public int CurrentPageTemp = 1;
        public int NumberOfPage;
        public string Comma = "、";
        public bool ShowPopup = false;
        public Dictionary<string, List<string>> InfoMessage = new Dictionary<string, List<string>>();
        public List<ReservationDataToCheck> ReservertionData;
        protected HeaderTemplate Header { get; set; }
        protected BodyTemplate Body { get; set; }
        protected List<SuperMenuVehicleData> DataItems { get; set; } = new List<SuperMenuVehicleData>();
        protected ShowCheckboxArgs<SuperMenuVehicleData> ShowCheckboxOptions { get; set; } = new ShowCheckboxArgs<SuperMenuVehicleData>()
        {
            RowIdentifier = (checkedItem, item) => checkedItem.No == item.No,
            Disable = (item) => false
        };
        public List<SuperMenuVehicleData> CheckedItems { get; set; } = new List<SuperMenuVehicleData>();
        protected List<HaiTaParam> haiTaParamInits = new List<HaiTaParam>();
        public VehicleDailyReportModel selectedItemVehicleDaily { get; set; }

        protected bool PopupFutai { get; set; }
        protected bool PopupJourney { get; set; } = false;
        protected bool PopupTehai { get; set; } = false;
        protected bool PopupFareFeeCorrection { get; set; } = false;
        protected bool PopupVehicleDailyReportInput { get; set; } = false;
        protected bool PopupVehicleAllocationInput { get; set; } = false;
        protected bool IsOpenConfirmTab { get; set; } = false;
        protected bool PopupTsumi { get; set; }
        protected bool IsOpenCancelTab { get; set; } = false;
        protected bool IsOpenEditYykshoBikoNm { get; set; } = false;
        protected bool IsOpenEditUnkobiBikoNm { get; set; } = false;
        protected bool DataNotExistPopup { get; set; } = false;
        protected string NoDataType { get; set; } = "";
        public string UkeCdParam { get; set; } = "0";
        public string currentUkeNo = "";
        public short currentunkRen;
        public Int16 currentBunkRen = 0;
        public bool ShowComfirmDeletePopup = false;
        public bool isFirstRender = true;
        public bool isLoadTotal = true;
        #endregion

        #region Function
        protected override void OnInitialized()
        {
            try
            {
                InitBody();
                InitHeader();
                CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
                if (cultureInfo.Name != "ja-JP")
                {
                    Comma = ", ";
                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup(Lang["Error"], ex.GetOriginalException()?.Message);
            }

        }
        protected void InitBody()
        {
            try
            {
                Body = new BodyTemplate()
                {
                    Rows = new List<RowBodyTemplate>()
{
                    new RowBodyTemplate()
                    {
                        Columns = new List<ColumnBodyTemplate>()
{
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.No), RowSpan = 3,  AlignCol = AlignColEnum.Center },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.Symbol), RowSpan = 3, AlignCol = AlignColEnum.Center, Control = new MultiLineControl<SuperMenuVehicleData> { MutilineText = GetSymbol } },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.Customer)},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.Organization) },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.Dispatch)},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.OfficeAddress)},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.Crew), RowSpan = 3, Control = new MultiLineControl<SuperMenuVehicleData> { MutilineText = GetCrew } },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.Fare), AlignCol = AlignColEnum.Right, CustomTextFormatDelegate = KoboGridHelper.ToFormatC},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.Guide), AlignCol = AlignColEnum.Right, CustomTextFormatDelegate = KoboGridHelper.ToFormatC},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.Other), AlignCol = AlignColEnum.Right, CustomTextFormatDelegate = KoboGridHelper.ToFormatC},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.PersonString), AlignCol = AlignColEnum.Right },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.ExitingDate), CustomTextFormatDelegate = KoboGridHelper.FormatYYYYMMDDDelegate },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.InServiceKilo), AlignCol = AlignColEnum.Right },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.ForwardingKilo), AlignCol = AlignColEnum.Right },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.OtherKilo), RowSpan = 3, AlignCol = AlignColEnum.Right },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.TotalKilo), RowSpan = 3, AlignCol = AlignColEnum.Right  },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.Fuel1Name) },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.Fuel2Name) },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.Fuel3Name) },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.RegistrationOffice) },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.ReserveClassification) },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.JourneyString), RowSpan = 3, ColSpan = 1, AlignCol = AlignColEnum.Center },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.LoadString), RowSpan = 3, ColSpan = 1, AlignCol = AlignColEnum.Center },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.ArrangeString), RowSpan = 3, ColSpan = 1, AlignCol = AlignColEnum.Center },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.RemarksString), RowSpan = 3, ColSpan = 1, AlignCol = AlignColEnum.Center },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.IncidentalString), RowSpan = 3, ColSpan = 1, AlignCol = AlignColEnum.Center },
                        }
                    },
                     new RowBodyTemplate()
                    {
                        Columns = new List<ColumnBodyTemplate>()
{
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.Branch) },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.Organization2) },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.Arrival)},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.BusName) },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.FareTax), AlignCol = AlignColEnum.Right, CustomTextFormatDelegate = KoboGridHelper.ToFormatC },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.GuideTax), AlignCol = AlignColEnum.Right, CustomTextFormatDelegate = KoboGridHelper.ToFormatC },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.OtherTax), AlignCol = AlignColEnum.Right, CustomTextFormatDelegate = KoboGridHelper.ToFormatC },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.PersonPlusString), AlignCol = AlignColEnum.Right },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.EnteringDate),CustomTextFormatDelegate = KoboGridHelper.FormatYYYYMMDDDelegate },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.InServiceHighSpeed), AlignCol = AlignColEnum.Right },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.ForwardingeHighSpeed), AlignCol = AlignColEnum.Right },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.Fuel1Value), AlignCol = AlignColEnum.Right },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.Fuel2Value), AlignCol = AlignColEnum.Right },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.Fuel3Value), AlignCol = AlignColEnum.Right },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.SalesStaff) },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.ReceptionDate),CustomTextFormatDelegate = KoboGridHelper.FormatYYYYMMDDDelegate }
                        }
                    },
                     new RowBodyTemplate()
                    {
                        Columns = new List<ColumnBodyTemplate>()
{
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.PersonInCharge) },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.Destination) },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.NumberOfDriverString) },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.BusNo) },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.FareFee), AlignCol = AlignColEnum.Right,CustomTextFormatDelegate = KoboGridHelper.ToFormatC },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.GuideFee), AlignCol = AlignColEnum.Right,CustomTextFormatDelegate = KoboGridHelper.ToFormatC },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.OtherFee), AlignCol = AlignColEnum.Right,CustomTextFormatDelegate = KoboGridHelper.ToFormatC },
                            new ColumnBodyTemplate()  { DisplayFieldName = nameof(SuperMenuVehicleData.EmptyString) },
                            new ColumnBodyTemplate()  { DisplayFieldName = nameof(SuperMenuVehicleData.EmptyString) },
                            new ColumnBodyTemplate()  { DisplayFieldName = nameof(SuperMenuVehicleData.EmptyString) },
                            new ColumnBodyTemplate()  { DisplayFieldName = nameof(SuperMenuVehicleData.EmptyString) },
                            new ColumnBodyTemplate()  { DisplayFieldName = nameof(SuperMenuVehicleData.EmptyString) },
                            new ColumnBodyTemplate()  { DisplayFieldName = nameof(SuperMenuVehicleData.EmptyString) },
                            new ColumnBodyTemplate()  { DisplayFieldName = nameof(SuperMenuVehicleData.EmptyString) },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.InputStaff) },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(SuperMenuVehicleData.ReceiptNumber) }
                        }
                    }

                }
                };
                Body.CustomCssDelegate = CustomRowCss;
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }

        }
        protected IEnumerable<string> GetSymbol(SuperMenuVehicleData item)
        {
            if (item != null)
            {
                foreach (string i_Symbol in item.Symbol)
                {
                    yield return Lang[i_Symbol];
                }
            }
        }
        protected IEnumerable<string> GetCrew(SuperMenuVehicleData item)
        {
            if (item != null && item.Crew.Split(",").Length > 1)
            {
                foreach (string i_Crew in item.Crew.Split(","))
                {
                    yield return i_Crew;
                }
            }
            else
            {
                yield return item.Crew;
            }
        }
        /// <summary>
        /// Set color for grid
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Func<object, string> CustomRowCss = (item) =>
        {
            var data = item as SuperMenuVehicleData;
            var cssClass = "";
            SuperMenuColorPattern result;
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
        protected void InitHeader()
        {
            try
            {
                Header = new HeaderTemplate()
                {
                    StickyCount = 1,
                    Rows = new List<RowHeaderTemplate>()
{
                    new RowHeaderTemplate()
                    {
                        Columns = new List<ColumnHeaderTemplate>()
{
                            new ColumnHeaderTemplate() { ColName = Lang["No"], RowSpan = 3,Width = 50 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridMark"], RowSpan = 3,Width = 50 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridCustomer"], RowSpan = 1,Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridGroup"], RowSpan = 1,Width = 200 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridDispatch"], RowSpan = 1,Width = 250 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridOffice"], RowSpan = 1,Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridCrew"], RowSpan = 3,Width = 120 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridFare"], RowSpan = 1,Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridGuideFee"], RowSpan = 1,Width = 120 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridOtherCharge"], RowSpan = 1,Width = 120 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridBoardingPerson"], RowSpan = 1,Width = 100 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridExitingDate"], RowSpan = 1,Width = 100 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridInServiceKilo"], RowSpan = 1,Width = 80 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridIForwardingKilo"], RowSpan = 1,Width = 80 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridOtherKilo"], RowSpan = 3,Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridTotalKilo"], RowSpan = 3,Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridFuel2Name"], RowSpan = 3,Width = 80 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridFuel2Name"], RowSpan = 3,Width = 80 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridFuel3Name"], RowSpan = 3,Width = 80 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridRegistrationOffice"], RowSpan = 1,Width = 100 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridReservationClassification"], RowSpan = 1,Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["InputDisplay"], RowSpan = 2, ColSpan = 5 ,Width = 250},

                        }
                    },
                    new RowHeaderTemplate()
                    {
                        Columns = new List<ColumnHeaderTemplate>()
{
                            new ColumnHeaderTemplate() { ColName = Lang["GridBranch"], RowSpan = 1,Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridGroup2Name"], RowSpan = 1,Width = 200 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridArrival"], RowSpan = 1, Width = 250 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridBusName"], RowSpan = 1, Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridFareTax"], RowSpan = 1, Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridGuideFeeTax"], RowSpan = 1,Width = 120 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridOtherChargeTax"], RowSpan = 1,Width = 120 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridPlusPerson"], RowSpan = 1, Width = 100 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridEnteringDate"], RowSpan = 1, Width = 100 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridInServiceHighSpeed"], RowSpan = 1, Width = 80 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridInServiceHighSpeed"], RowSpan = 1, Width = 80 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridServiceStaff"], RowSpan = 1, Width = 100 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridReceiptDate"], RowSpan = 1, Width = 150 }
                        }
                    },
                    new RowHeaderTemplate()
                    {
                        Columns = new List<ColumnHeaderTemplate>()
{
                            new ColumnHeaderTemplate() { ColName = Lang["GridCustomerStaff"], RowSpan = 1,Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridDestination"], RowSpan = 1,Width = 200 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridDriverGuiderNumber"], RowSpan = 1, Width = 250 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridBusNo"], RowSpan = 1, Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridFareCommission"], RowSpan = 1, Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridGuideFeeCommission"], RowSpan = 1, Width = 120 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridOtherChargeCommission"], RowSpan = 1,Width = 120 },
                            new ColumnHeaderTemplate() { ColName = "", RowSpan = 1, Width = 100 },
                            new ColumnHeaderTemplate() { ColName = "", RowSpan = 1, Width = 100 },
                            new ColumnHeaderTemplate() { ColName = "", RowSpan = 1, Width = 80 },
                            new ColumnHeaderTemplate() { ColName = "", RowSpan = 1, Width = 80 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridInputStaff"], RowSpan = 1, Width = 100 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridReceiptNumber"], RowSpan = 1, Width = 150 },
                            new ColumnHeaderTemplate() { ColName = Lang["GridTimeScheduleMark"], RowSpan = 1 ,Width = 50},
                            new ColumnHeaderTemplate() { ColName = Lang["GridLoadingGoodsMark"], RowSpan = 1 ,Width = 50},
                            new ColumnHeaderTemplate() { ColName = Lang["GridArrangementMark"], RowSpan = 1 ,Width = 50},
                            new ColumnHeaderTemplate() { ColName = Lang["GridNoteMark"], RowSpan = 1 ,Width = 50},
                            new ColumnHeaderTemplate() { ColName = Lang["GridOtherChargeMark"], RowSpan = 1 ,Width = 50}
                        }
                    }
                }
                };
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
                errorModalService.ShowErrorPopup(Lang["Error"], ex.GetOriginalException()?.Message);
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
                errorModalService.ShowErrorPopup(Lang["Error"], ex.GetOriginalException()?.Message);
            }

        }
        protected void changeData()
        {
            JSRuntime.InvokeVoidAsync("loadPageScript", "hyperMenuPage");
            CurrentTotal = GridTotalDatas.Order;
            CurrentTotalContent = Lang["Order"];
            NumberOfPage = (GridDatas.Count() + RecordsPerPage - 1) / RecordsPerPage;
            FirstLoad(true);
            //haitacheck
            haiTaParamInits = GridDatas?.Select(_ => new HaiTaParam
            {
                UpdYmd = _?.UpdYmd,
                UpdTime = _?.UpdTime,
                UkeNo = _?.UkeNo
            }).ToList();
        }

        protected async Task HandleMouseWheel()
        {
            try
            {
                await blazorContextMenuService.HideMenu("gridRowClickMenu");
                await blazorContextMenuService.HideMenu("gridRowsClickMenu");
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup(Lang["Error"], ex.GetOriginalException()?.Message);
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
                    CheckedItems = new List<SuperMenuVehicleData>() { DataItems.FirstOrDefault(item => item.No == CheckedItems[0].No - 1) };
                    CurrentClick = CheckedItems[0].No;
                    IndexResult = CheckedItems[0].No;
                }
                if (KeyCode == 40 && !IsShift && CheckedItems.Count == 1 && CheckedItems[0].No < GridDisplay[GridDisplay.Count - 1].No)
                {
                    CheckedItems = new List<SuperMenuVehicleData>() { DataItems.FirstOrDefault(item => item.No == CheckedItems[0].No + 1) };
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

        /// <summary>
        /// Set color for grid
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected int GetColorPattern(SuperMenuVehicleData data)
        {
            try
            {
                SuperMenuColorPattern result;
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
                return (int)result;
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup(Lang["Error"], ex.GetOriginalException()?.Message);
                return 0;
            }

        }

        protected async Task HandleMouseDown(MouseEventArgs e, int index)
        {
            try
            {
                LastXClicked = Convert.ToInt32(e.ClientX);
                LastYClicked = Convert.ToInt32(e.ClientY);
                CurrentScroll = null;
                if (!e.ShiftKey && !e.CtrlKey)
                {
                    if (CheckedItems.Count > 1)
                    {
                        await blazorContextMenuService.HideMenu("gridRowClickMenu");
                        await blazorContextMenuService.ShowMenu("gridRowsClickMenu", LastXClicked, LastYClicked);
                    }
                    else
                    {
                        CurrentRow = new List<int>(new int[] { index });
                        await blazorContextMenuService.ShowMenu("gridRowClickMenu", LastXClicked, LastYClicked);
                    }
                }
                else
                {
                    isClickRow = true;
                    await blazorContextMenuService.HideMenu("gridRowClickMenu");
                    if (e.CtrlKey)
                    {
                        if (!CurrentRow.Contains(index))
                        {
                            CurrentRow.Add(index);
                        }
                        else
                        {
                            CurrentRow.RemoveAll(p => p == index);
                            if (CurrentRow.Count() == 0)
                            {
                                CurrentClick = null;
                            }
                            else
                            {
                                CurrentClick = CurrentRow.Max();
                            }
                        }
                    }
                    else
                    {
                        int BeginIndex = Math.Min(index, (int)CurrentClick);
                        int EndIndex = Math.Max(index, (int)CurrentClick);
                        for (int IndexToBeAdd = BeginIndex; IndexToBeAdd <= EndIndex; IndexToBeAdd++)
                        {
                            if (!CurrentRow.Contains(IndexToBeAdd))
                            {
                                CurrentRow.Add(IndexToBeAdd);
                            }
                        }
                    }
                }
                CurrentClick = index;
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup(Lang["Error"], ex.GetOriginalException()?.Message);
            }

        }

        /// <summary>
        /// Change label grid total
        /// </summary>
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
                errorModalService.ShowErrorPopup(Lang["Error"], ex.GetOriginalException()?.Message);
            }
        }

        /// <summary>
        /// Select page
        /// </summary>
        /// <param name="index"></param>
        protected async void SelectPage(int index)
        {
            try
            {
                await _loadingService.ShowAsync();
                // CurrentRow = new List<int>();
                List<SuperMenuVehicleData> TempGridData = GridDatas.GetRange(index * RecordsPerPage, Math.Min(RecordsPerPage, GridDatas.Count() - index * RecordsPerPage));
                CurrentPageTemp = index;
                if (CurrentPageTemp != FirstPageSelect)
                {
                    FirstPageSelect = CurrentPageTemp;
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
                    StandardizedDataForGrid(index);
                    CurrentPage = index;
                    if (index == 0 && paging != null)
                    {
                        paging.TotalCount = GridDatas.Count();
                        paging.currentPage = index;
                    }
                    InvokeAsync(StateHasChanged).Wait();
                }
                await _loadingService.HideAsync();
                InvokeAsync(StateHasChanged).Wait();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup(Lang["Error"], ex.GetOriginalException()?.Message);
            }
        }
        protected async void FirstLoad(bool isFirstLoad)
        {
            try
            {
                if (!isFirstLoad)
                {
                    await _loadingService.ShowAsync();
                }
                int index = 0; ;
                // CurrentRow = new List<int>();
                List<SuperMenuVehicleData> TempGridData = GridDatas.GetRange(index * RecordsPerPage, Math.Min(RecordsPerPage, GridDatas.Count() - index * RecordsPerPage));
                GridDisplay = TempGridData;
                DataItems = GridDisplay;
                StandardizedDataForGrid(index);
                CurrentPage = index;
                if (index == 0 && paging != null)
                {
                    paging.TotalCount = GridDatas.Count();
                    paging.currentPage = index;
                }
                if (!isFirstLoad)
                {
                    await _loadingService.HideAsync();
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
                List<SuperMenuVehicleData> PageData = await HyperDataService.GetSuperMenuVehicleData(hyperData, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, new ClaimModel().TenantID, OffSet, RecordsPerPage);
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
                DataItems = GridDisplay;
                CurrentPage = PageNo;
                StandardizedDataForGrid(PageNo);
                paging.TotalCount = GridDatas.Count();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup(Lang["Error"], ex.GetOriginalException()?.Message);
            }

        }

        protected void StandardizedDataForGrid(int index)
        {
            try
            {
                int count = index * RecordsPerPage + 1;
                if (GridDisplay.Count > 0 && GridDisplay[0] != null)
                {
                    foreach (var data in GridDisplay.Select((value, i) => new { i, value }))
                    {

                    }
                    foreach (var item in GridDisplay)
                    {
                        if (item != null)
                        {
                            item.No = count;
                            foreach (var symbol in item.Symbol)
                            {
                                item.SymbolInString = Lang[symbol];
                            }
                            item.PersonString = item.Person.ToString() + " " + @Lang["PeopleUnit"];
                            item.ArrangeString = item.Arrange ? "⚪" : string.Empty;
                            item.RemarksString = item.Remarks ? "⚪" : string.Empty;
                            item.JourneyString = item.Journey ? "⚪" : string.Empty;
                            item.LoadString = item.Load ? "⚪" : string.Empty;
                            item.IncidentalString = item.Incidental ? "⚪" : string.Empty;
                            item.PersonPlusString = item.PersonPlus + Lang["PeopleUnit"];
                            item.NumberOfDriverString = Lang["Driver"] + item.NumberOfDrivers + Lang["DriverUnit"] + "/" + Lang["Guide"] + item.NumberOfGuides + Lang["DriverUnit"];
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

        /// <summary>
        /// Get pagination for table
        /// </summary>
        /// <returns></returns>
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
                errorModalService.ShowErrorPopup(Lang["Error"], ex.GetOriginalException()?.Message);
                return Enumerable.Range(0, NumberOfPage).ToArray();
            }
        }

        protected void EditReservationForm()
        {
            try
            {
                string baseUrl = AppSettingsService.GetBaseUrl() + "";
                string url = "";
                string UkeCd = CheckedItems[0].UkeNo.Substring(5, 10);
                url = baseUrl + "/bookinginput";
                url = url + string.Format("/?UkeCd={0}", UkeCd);
                JSRuntime.InvokeVoidAsync("open", url, "_blank");
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup(Lang["Error"], ex.GetOriginalException()?.Message);
            }
        }

        protected async void CancelReservation()
        {
            try
            {
                ShowComfirmDeletePopup = false;
                var canceled = false;
                await _loadingService.ShowAsync();
                var isValid = await HaiTaCheck();
                if (isValid)
                {
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
                    JSRuntime.InvokeVoidAsync("hidePageScroll");
                    await _loadingService.HideAsync();

                }
                await _loadingService.HideAsync();
                await ValueCheckHaita.InvokeAsync(isValid);
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup(Lang["Error"], ex.GetOriginalException()?.Message);
            }
        }

        protected async void ConfirmReservation()
        {
            try
            {
                await _loadingService.ShowAsync();
                StateHasChanged();
                var isValid = await HaiTaCheck();
                if (isValid)
                {
                    var confirm = false;
                    List<string> SelectedReceiptNumber = new List<string>();
                    foreach (SuperMenuVehicleData SelectRow in CheckedItems)
                    {
                        SelectedReceiptNumber.Add(SelectRow.UkeNo);
                    }
                    confirm = await HyperDataService.ConfirmRevervation(SelectedReceiptNumber);
                    await ChangeState.InvokeAsync(CurrentPage);
                    await _loadingService.HideAsync();
                }
                await ValueCheckHaita.InvokeAsync(isValid);
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected async Task<bool> HaiTaCheck()
        {
            var haiTaParamSelecteds = haiTaParamInits.Where(x => CheckedItems.Any(y => y.UkeNo.Equals(x.UkeNo))).ToList();
            return await HyperDataService.HaitaCheck(haiTaParamSelecteds);
        }

        protected async Task GetDataReservationToCheck()
        {
            try
            {
                InfoMessage = new Dictionary<string, List<string>>();
                List<string> SelectedReceiptNumber = new List<string>();
                foreach (SuperMenuVehicleData SelectRow in CheckedItems)
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

        protected string GetCrew()
        {
            return "";
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

        protected async Task RowClick(RowClickEventArgs<SuperMenuVehicleData> args)
        {
            try
            {
                SuperMenuVehicleData SelectedItem = args.SelectedItem;
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
                        CheckedItems = new List<SuperMenuVehicleData>() { SelectedItem };
                        await JSRuntime.InvokeVoidAsync("loadPageScript", "hyperMenuPage", "SetPositionForMenuContext", LastYClicked, DotNetObjectReference.Create(this));
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

        protected void CheckedChange(CheckedChangeEventArgs<SuperMenuVehicleData> checkedItems)
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
                JSRuntime.InvokeVoidAsync("showPageScroll");
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
                foreach (SuperMenuVehicleData SelectRow in CheckedItems)
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
                        JSRuntime.InvokeVoidAsync("open", url, "_blank");
                        break;
                    case "menuitem-reservation-calendar-copy":
                        url = baseUrl + string.Format("/bookinginputmulticopy/?UkeCd={0}", UkeNo.Substring(5));
                        JSRuntime.InvokeVoidAsync("open", url, "_blank");
                        break;
                    case "menuitem-confirmation":
                        IsOpenConfirmTab = true;
                        JSRuntime.InvokeVoidAsync("hidePageScroll");
                        return;
                    case "menuitem-edityykshobikonm":
                        IsOpenEditYykshoBikoNm = true;
                        JSRuntime.InvokeVoidAsync("hidePageScroll");
                        return;
                    case "menuitem-editunkobibikonm":
                        IsOpenEditUnkobiBikoNm = true;
                        JSRuntime.InvokeVoidAsync("hidePageScroll");
                        return;
                    case "menuitem-partnerbookinginput":
                        url = baseUrl + "/partnerbookinginput";
                        url = url + string.Format("/?UkeCd={0}&UnkRen={1}", CheckedItems[0].UkeNo.Substring(5), CheckedItems[0].UnkRen);
                        JSRuntime.InvokeVoidAsync("open", url, "_blank");
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
                    case "download_advance_statement":
                        await _loadingService.ShowAsync();
                        NoDataType = "AdvanceReport";
                        DataNotExistPopup = await advancePaymentDetailsService.AdvancePaymentDetailReport(OptionReport.Download.ToString(), SelectedReceiptNumber, JSRuntime, reportLayoutSettingService);
                        await _loadingService.HideAsync();
                        break;
                    case "preview_advance_statement":
                        NoDataType = "AdvanceReport";
                        DataNotExistPopup = await advancePaymentDetailsService.AdvancePaymentDetailReport(OptionReport.Preview.ToString(), SelectedReceiptNumber, JSRuntime, reportLayoutSettingService);
                        break; ///
                    case "download_report_car_request_form":
                        await _loadingService.ShowAsync();
                        List<VenderRequestReportData> listResult = new List<VenderRequestReportData>();
                        VenderRequestFormData searchParam1 = new VenderRequestFormData();
                        (DataNotExistPopup, listResult, searchParam1) = await VenderRequestService.SetParamAndCheckData(UkeNo.Substring(5), CheckedItems[0].HaiSYmdInfoUnkobi, CheckedItems[0].UnkRen.ToString());
                        if (!DataNotExistPopup)
                        {
                            VenderRequestService.ExportPDF(listResult, JSRuntime);
                        }
                        await _loadingService.HideAsync();
                        break;
                    case "preview_report_car_request_form":
                        List<VenderRequestReportData> listResult1 = new List<VenderRequestReportData>();
                        VenderRequestFormData searchParam = new VenderRequestFormData();
                        (DataNotExistPopup, listResult1, searchParam) = await VenderRequestService.SetParamAndCheckData(UkeNo.Substring(5), CheckedItems[0].HaiSYmdInfoUnkobi, CheckedItems[0].UnkRen.ToString());
                        if (!DataNotExistPopup)
                        {
                            searchParam.OutputSetting = OutputInstruction.Preview;
                            string PreviewReportUrl = EncryptHelper.EncryptToUrl(searchParam);
                            url = baseUrl + "/VenderRequestFormPreview";
                            url = url + string.Format("/?PreviewReportUrl={0}", PreviewReportUrl);
                            await new System.Threading.Tasks.TaskFactory().StartNew(() =>
                            {
                                JSRuntime.InvokeVoidAsync("open", url, "_blank");
                            });
                        }
                        break;
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
                    case "menuitem-reservation-cancellation":
                        if (!string.IsNullOrEmpty(UkeNo))
                        {
                            IsOpenCancelTab = true;
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

        protected string getInfoListUkeNo(List<SuperMenuVehicleData> _checkedItems)
        {
            string resultUkeNo = "";
            _checkedItems = _checkedItems.DistinctBy(p => new { p.UkeNo, p.UnkRen }).ToList();
            foreach (SuperMenuVehicleData item in _checkedItems)
            {
                resultUkeNo += item.UkeNo + item.UnkRen.ToString("000") + ",";
            }
            if (!string.IsNullOrEmpty(resultUkeNo))
            {
                resultUkeNo = resultUkeNo.Substring(0, resultUkeNo.LastIndexOf(","));
            }
            return resultUkeNo;
        }

        protected void OpenFutai()
        {
            try
            {
                getCurrentUkeCd();
                PopupFutai = true;
                JSRuntime.InvokeVoidAsync("hidePageScroll");
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
                getCurrentUkeCd();
                PopupJourney = true;
                JSRuntime.InvokeVoidAsync("hidePageScroll");
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
                JSRuntime.InvokeVoidAsync("hidePageScroll");
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
                JSRuntime.InvokeVoidAsync("hidePageScroll");
                StateHasChanged();
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

        public void OpenBusAllocation(ItemClickEventArgs e)
        {
            string baseUrl = AppSettingsService.GetBaseUrl();
            string url = baseUrl + "/busallocation";
            switch (e.MenuItem.Id)
            {
                case "menuitem-temporary-car-input":
                    url = url + string.Format("/?date={0}&bookingid={1}&HaiSKbn={2}", CheckedItems[0].HaiSYmdInfoUnkobi, CheckedItems[0].UkeNo.Substring(5), CheckedItems[0].HaiSKbn);
                    JSRuntime.InvokeVoidAsync("open", url, "_blank");
                    break;
                case "menuitem-busallocation":
                    url = url + string.Format("/?date={0}&bookingid={1}", CheckedItems[0].HaiSYmdInfoUnkobi, CheckedItems[0].UkeNo.Substring(5));
                    JSRuntime.InvokeVoidAsync("open", url, "_blank");
                    break;
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
                        url = url + string.Format("/buscoordinationreportpreview/?Option={0}&UnkobiDate={1}&Ukeno={2}&UnkRen={3}&IsLoadDefault=1", OptionReport.Preview, CheckedItems[0].HaiSYmdInfoUnkobi, CheckedItems[0].UkeNo.Substring(5), CheckedItems[0].UnkRen.ToString());
                        JSRuntime.InvokeVoidAsync("open", url, "_blank");
                        break;
                    case "BusCoooradination_Download":
                        await _loadingService.ShowAsync();
                        BusCoordinationReportService.ExportPdfFiles(CheckedItems[0].HaiSYmdInfoUnkobi, CheckedItems[0].UkeNo.Substring(5), CheckedItems[0].UnkRen.ToString(), JSRuntime);
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

        protected async Task OpenFareFeeCorrection()
        {
            try
            {
                getCurrentUkeCd();
                PopupFareFeeCorrection = true;
                JSRuntime.InvokeVoidAsync("hidePageScroll");
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected void OpenVehicleDailyReportInput()
        {
            try
            {
                selectedItemVehicleDaily = new VehicleDailyReportModel();
                selectedItemVehicleDaily.UkeNo = CheckedItems[0].UkeNo;
                selectedItemVehicleDaily.UnkRen = CheckedItems[0].UnkRen;
                selectedItemVehicleDaily.TeiDanNo = CheckedItems[0].TeiDanNo;
                selectedItemVehicleDaily.BunkRen = CheckedItems[0].BunkRen;
                selectedItemVehicleDaily.UnkYmd = CheckedItems[0].ExitingDate;
                selectedItemVehicleDaily.SyukoYmd = CheckedItems[0].ExitingDate;
                selectedItemVehicleDaily.KikYmd = CheckedItems[0].EnteringDate;
                selectedItemVehicleDaily.NenryoCd1Seq = CheckedItems[0].NenryoCd1Seq;
                selectedItemVehicleDaily.NenryoCd2Seq = CheckedItems[0].NenryoCd2Seq;
                selectedItemVehicleDaily.NenryoCd3Seq = CheckedItems[0].NenryoCd3Seq;
                selectedItemVehicleDaily.SyaRyoCd = CheckedItems[0].SyaRyoCd;
                selectedItemVehicleDaily.SyaryoNm = CheckedItems[0].SyaRyoNm;
                selectedItemVehicleDaily.HaiSYmd = CheckedItems[0].HaiSYmd;
                selectedItemVehicleDaily.TouYmd = CheckedItems[0].TouYmd;
                TogglePopup();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        protected void TogglePopup()
        {
            PopupVehicleDailyReportInput = !PopupVehicleDailyReportInput;
            if (PopupVehicleDailyReportInput) JSRuntime.InvokeVoidAsync("hidePageScroll");
            else JSRuntime.InvokeVoidAsync("showPageScroll");
            if (!PopupVehicleDailyReportInput)
                refreshGrid(); ;
        }

        protected void ComfirmCancelReservation()
        {
            ShowComfirmDeletePopup = true;
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

        protected void OnCloseFareFeeCorrection(bool value)
        {
            PopupFareFeeCorrection = value;
            refreshGrid();
        }
        #endregion
    }
}
