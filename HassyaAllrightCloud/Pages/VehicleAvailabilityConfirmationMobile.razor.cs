using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Pages
{
    public class VehicleAvailabilityConfirmationMobileBase : ComponentBase
    {
        [Parameter]
        public string startDateParam { get; set; }
        [Inject]
        protected IVehicleAvailabilityConfirmationMobileService _service { get; set; }
        [Inject]
        protected IStringLocalizer<VehicleAvailabilityConfirmationMobile> _lang { get; set; }
        [Inject]
        protected IJSRuntime _jSRuntime { get; set; }
        [Inject]
        protected IErrorHandlerService _errorModalService { get; set; }
        [Inject]
        private IAvailabilityCheckService _availabilityCheckService { get; set; }
        [Inject]
        private ITransportationSummaryService _transportationSummaryService { get; set; }
        [Inject]
        protected IGenerateFilterValueDictionary _generateFilterValueDictionaryService { get; set; }
        [Inject]
        protected IFilterCondition _filterConditionService { get; set; }
        [Inject]
        protected NavigationManager _navigationManager { get; set; }
        [Inject]
        private ILoadingService _loading { get; set; }

        public DateTime selectedMonth;
        public List<DateTime> listDate { get; set; } = new List<DateTime>();
        public List<int> lastItem { get; set; } = new List<int>() { 6, 13, 20, 27, 34, 41 };
        public DateTime selectedDate { get; set; }
        public bool IsToday { get; set; }
        public bool IsYesterday { get; set; }
        public bool IsTomorrow { get; set; }
        protected bool isFirstRender = true;
        protected BusType busTypeSelected;
        protected List<BusType> busTypes = new List<BusType>();
        protected int EmptyLargeDriverCount { get; set; }
        protected int AbsenceLargeDriverCount { get; set; }
        protected int LargeDriverCount { get; set; }
        public List<VehicleSchedulerMobileData> Data { get; set; } = new List<VehicleSchedulerMobileData>();
        protected List<DriverInfomation> DriverInfos { get; set; } = new List<DriverInfomation>();
        protected List<DateTime> DaysOfMonth { get; set; } = new List<DateTime>();
        protected List<BusAllocationDatas> BusInfos { get; set; } = new List<BusAllocationDatas>();
        protected List<EigyoListItem> eigyoListItems { get; set; } = new List<EigyoListItem>();
        private CancellationTokenSource source { get; set; } = new CancellationTokenSource();
        protected IEnumerable<CompanyListItem> companyListItems { get; set; } = new List<CompanyListItem>();
        protected List<GroupBusInfo> groupBusInfo = new List<GroupBusInfo>();
        protected List<BusData> busDatas = new List<BusData>();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (isFirstRender)
            {
                await _jSRuntime.InvokeVoidAsync("loadPageScript", "vehicleavailabilityconfirmationmobile", "fadeToggleVhScheduleMB");
                await InvokeAsync(StateHasChanged);
                isFirstRender = false;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await _loading.ShowAsync();
                busTypes = (await _service.GetBusTypeListItems()).ToList();
                var filterValues = _filterConditionService.GetFilterCondition(FormFilterName.VehicleAvailabilityConfirmationMobile, 0, new ClaimModel().SyainCdSeq).Result;
                if (filterValues.Any())
                {
                    busTypeSelected = filterValues.FirstOrDefault(x => x.ItemNm == nameof(FormSearch.BusType)) != null ? busTypes.FirstOrDefault(x => x.SyaSyuCdSeq == (int.Parse(filterValues.FirstOrDefault(x => x.ItemNm == nameof(FormSearch.BusType)).JoInput))) : new BusType();
                    selectedDate = string.IsNullOrEmpty(startDateParam) ? (filterValues.FirstOrDefault(x => x.ItemNm == nameof(FormSearch.SelectedDate)) != null ?
                                               DateTime.ParseExact(filterValues.FirstOrDefault(x => x.ItemNm == nameof(FormSearch.SelectedDate))?.JoInput, Formats.yyyyMMdd, null) : DateTime.Now) :
                                               DateTime.ParseExact(startDateParam, Formats.yyyyMMdd, new CultureInfo("ja-JP"));
                }
                else
                {
                    if (string.IsNullOrEmpty(startDateParam))
                        selectedDate = DateTime.Now;
                    else
                        selectedDate = DateTime.ParseExact(startDateParam, Formats.yyyyMMdd, new CultureInfo("ja-JP"));
                    busTypeSelected = new BusType();
                }
                selectedMonth = selectedDate;
                OnSetDateText();
                GetListDate(selectedDate.Year, selectedDate.Month);
                companyListItems = await _transportationSummaryService.GetCompanyListItems(0);
                await GetBranch();
                await GetDriverInfo(true);
                busTypes.Insert(0, null);
                await _loading.HideAsync();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                _errorModalService.HandleError(ex);
                StateHasChanged();
            }
        }

        protected async Task PrepareData()
        {
            var eiygos = GetEiygoCdSeq();
            var tenantId = new ClaimModel().TenantID;
            DaysOfMonth = GetDaysOfMonth(selectedDate.Year, selectedDate.Month);

            var busAllocations = await _availabilityCheckService.GetBusAllocation(DaysOfMonth.First(), DaysOfMonth.Last(), tenantId, source.Token);
            var busData = await _availabilityCheckService.GetBusData(DaysOfMonth.First(), DaysOfMonth.Last(), tenantId, eiygos, GetCompnyCdSeq(), source.Token);
            var repairData = await _availabilityCheckService.GetShuri(DaysOfMonth.First(), DaysOfMonth.Last(), source.Token);

            var employees = await _availabilityCheckService.GetEmployeeData(DaysOfMonth.First(), DaysOfMonth.Last(), tenantId, eiygos, source.Token);
            var workHolidayData = await _availabilityCheckService.GetWorkHolidayData(DaysOfMonth.First(), DaysOfMonth.Last(), tenantId, GetSyainCdSeq(employees.Select(e => e.SyainCdSeq)), source.Token);
            var staffs = await _availabilityCheckService.GetStaffData(DaysOfMonth.First(), DaysOfMonth.Last(), tenantId, source.Token);
            BusInfos.Clear();
            foreach (var day in DaysOfMonth)
            {
                BusInfos.Add(await _service.CaculateBusInfo(day, busAllocations, busData, repairData));
            }
            DriverInfos.Clear();
            foreach (var day in DaysOfMonth)
            {
                DriverInfos.Add(_availabilityCheckService.CaculateDriverInfo(day, employees, workHolidayData, staffs));
            }
        }

        private async Task GetDriverInfo(bool isFirstLoad)
        {
            var busInfo = BusInfos.FirstOrDefault(_ => _.DateSelected.Date == selectedDate.Date);
            if (busInfo == null)
            {
                await PrepareData();
                busInfo = BusInfos.FirstOrDefault(_ => _.DateSelected.Date == selectedDate.Date);
            }

            groupBusInfo.Clear();
            if (busTypeSelected?.SyaSyuCdSeq > 0)
                busDatas = busInfo.BusDatas.Where(x => x.SyaSyuCdSeq == busTypeSelected.SyaSyuCdSeq).ToList();
            else
                busDatas = busInfo.BusDatas.ToList();

            foreach (var group in busDatas.GroupBy(x => x.SyaSyuCdSeq))
            {
                var busDetails = new List<BusDetail>();
                foreach (var item in group)
                {
                    if (busInfo.BusAllocationsSeqs.Any(_ => _ == item.SyaRyoCdSeq))
                    {
                        busDetails.Add(new BusDetail
                        {
                            BusTypeNameDetail = item.SyaRyoNm,
                            Status = "✖",
                            isAvailable = false,
                            //SyaRyoCdSeq = item.BusID
                        });
                    }
                    else
                    {
                        busDetails.Add(new BusDetail
                        {
                            BusTypeNameDetail = item.SyaRyoNm,
                            Status = "○",
                            isAvailable = true,
                            //SyaRyoCdSeq = item.BusID
                        });
                    }
                }
                groupBusInfo.Add(new GroupBusInfo
                {
                    BusTypeName = group.FirstOrDefault().SyaSyuNm,
                    BusCount = group.Count(),
                    InUseBusCount = $"{group.Count() - busDetails.Where(x => x.isAvailable).Count()}台",
                    UnUseBusCount = busDetails.Where(x => x.isAvailable).Count().ToString(),
                    BusDetails = busDetails
                });
            }

            AbsenceLargeDriverCount = DriverInfos[selectedDate.Day - 1].AbsenceLargeDriverCount;
            LargeDriverCount = DriverInfos[selectedDate.Day - 1].LargeDriverCount;
            EmptyLargeDriverCount = LargeDriverCount - AbsenceLargeDriverCount;

            var searchModel = new FormSearch
            {
                BusType = busTypeSelected,
                SelectedDate = selectedDate
            };
            if (!isFirstLoad)
            {
                var keyValueFilterPairs = _generateFilterValueDictionaryService.GenerateForVehicleAvailabilityConfirmationMobile(searchModel).Result;
                await _filterConditionService.SaveFilterCondtion(keyValueFilterPairs, FormFilterName.VehicleAvailabilityConfirmationMobile, 0, new ClaimModel().SyainCdSeq);
            }
            await InvokeAsync(StateHasChanged);
        }

        private void GetListDate(int year, int month)
        {
            listDate.Clear();
            var startDate = GetStartDate(year, month);
            for (int i = 0; i < 42; i++)
            {
                listDate.Add(startDate.AddDays(i));
            }
        }

        private DateTime GetStartDate(int year, int month)
        {
            DateTime date = new DateTime(year, month, 1);
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                    return date.AddDays(-6);
                case DayOfWeek.Friday:
                    return date.AddDays(-5);
                case DayOfWeek.Thursday:
                    return date.AddDays(-4);
                case DayOfWeek.Wednesday:
                    return date.AddDays(-3);
                case DayOfWeek.Tuesday:
                    return date.AddDays(-2);
                case DayOfWeek.Monday:
                    return date.AddDays(-1);
                default:
                    return date;
            }
        }

        protected async Task BusTypeChanged(BusType type)
        {
            try
            {
                busTypeSelected = type;
                await GetDriverInfo(false);
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                _errorModalService.HandleError(ex);
            }
        }

        protected async Task OnSetCurrentDate()
        {
            try
            {
                await _loading.ShowAsync();
                selectedDate = listDate.FirstOrDefault(_ => _ == DateTime.Today);
                await GetDriverInfo(false);
                if (selectedMonth.Month != DateTime.Today.Month || selectedMonth.Year != DateTime.Today.Year)
                {
                    selectedMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    GetListDate(selectedMonth.Year, selectedMonth.Month);
                }

                var searchModel = new FormSearch
                {
                    BusType = busTypeSelected,
                    SelectedDate = selectedDate
                };
                OnSetDateText();
                await _loading.HideAsync();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                _errorModalService.HandleError(ex);
                StateHasChanged();
            }
        }

        protected async Task NextPrevDate(byte type)
        {
            try
            {
                await _loading.ShowAsync();
                if (type == 1)
                    selectedDate = selectedDate.AddDays(1);
                else
                    selectedDate = selectedDate.AddDays(-1);

                await GetDriverInfo(false);
                selectedMonth = selectedDate;
                GetListDate(selectedMonth.Year, selectedMonth.Month);
                OnSetDateText();
                await _loading.HideAsync();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                _errorModalService.HandleError(ex);
            }
        }

        protected async Task OnSelectDate(DateTime item)
        {
            try
            {
                await _loading.ShowAsync();
                selectedDate = item;
                selectedMonth = item;
                await GetDriverInfo(false);
                OnSetDateText();
                await _loading.HideAsync();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                _errorModalService.HandleError(ex);
            }
        }

        private void OnSetDateText()
        {
            IsToday = false;
            IsYesterday = false;
            IsTomorrow = false;
            if (selectedDate == DateTime.Today) IsToday = true;
            else if (selectedDate == DateTime.Today.AddDays(-1)) IsYesterday = true;
            else if (selectedDate == DateTime.Today.AddDays(1)) IsTomorrow = true;
        }

        private List<DateTime> GetDaysOfMonth(int year, int month)
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(year, month))  // Days: 1, 2 ... 31 etc.
                             .Select(day => new DateTime(year, month, day)) // Map each day to a date
                             .ToList(); // Load dates into a list
        }

        protected async Task NextPrevMonth(byte type)
        {
            try
            {
                selectedMonth = type == 1 ? selectedMonth.AddMonths(1) : selectedMonth.AddMonths(-1);
                GetListDate(selectedMonth.Year, selectedMonth.Month);
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                _errorModalService.HandleError(ex);
                StateHasChanged();
            }
        }

        private async Task GetBranch()
        {
            foreach (var i in companyListItems)
            {
                var lst = await _transportationSummaryService.GetEigyoListItems(i.CompanyCdSeq, new ClaimModel().TenantID);
                eigyoListItems.AddRange(lst);
            }
        }

        protected void OnNavigate(BusDetail item)
        {
            try
            {
                _navigationManager.NavigateTo(string.Format("vehicleschedulermobile?SyaRyoCdSeq={0}", item.SyaRyoCdSeq));
            }
            catch (Exception ex)
            {
                _errorModalService.HandleError(ex);
            }
        }

        protected async Task OnRefresh()
        {
            try
            {
                await _filterConditionService.DeleteCustomFilerCondition(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, 0, FormFilterName.VehicleAvailabilityConfirmationMobile);
                await OnInitializedAsync();
            }
            catch (Exception ex)
            {
                _errorModalService.HandleError(ex);
                StateHasChanged();
            }
        }

        private string GetEiygoCdSeq() => eigyoListItems == null || !eigyoListItems.Any() ? string.Empty : string.Join(',', eigyoListItems.Select(c => c.EigyoCdSeq));
        private string GetCompnyCdSeq() => companyListItems == null || !companyListItems.Any() ? string.Empty : string.Join(',', companyListItems.Select(c => c.CompanyCdSeq));
        private string GetSyainCdSeq(IEnumerable<int> seqs) => seqs == null || seqs.Any() ? string.Empty : string.Join(',', seqs);
    }
}
