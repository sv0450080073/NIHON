using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Microsoft.Extensions.Localization;
using HassyaAllrightCloud.IService;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons;
using System.Globalization;
using System.Threading;

namespace HassyaAllrightCloud.Pages
{
    public class AvailabilityCheckBase : ComponentBase, IDisposable
    {
        [Inject] IJSRuntime jSRuntime { get; set; }
        [Inject] protected IStringLocalizer<AvailabilityCheck> lang { get; set; }
        [Inject] private ITPM_EigyosDataListService TPM_EigyosDataService { get; set; }
        [Inject] private ILoadingService loading { get; set; }
        [Inject] private IAvailabilityCheckService _service { get; set; }
        [Inject] protected NavigationManager _navigationManager { get; set; }
        [Inject] private IErrorHandlerService _errService { get; set; }
        [Inject] private IFilterCondition _filterService { get; set; }
        [Inject] private ITransportationSummaryService _transportationSummaryService { get; set; }
        protected string CustomStyle { get; set; }
        // Clicked Bus Info
        protected BusInfomation Sbi { get; set; }
        // Selected Driver Info
        protected DriverInfomation Sdi { get; set; }
        protected List<DateTime> DaysOf2Weeks { get; set; } = new List<DateTime>();
        protected IEnumerable<CompanyListItem> companyListItems { get; set; } = new List<CompanyListItem>();
        protected List<EigyoListItem> eigyoListItems { get; set; } = new List<EigyoListItem>();
        protected List<BusInfomation> BusInfos { get; set; } = new List<BusInfomation>();
        protected List<DriverInfomation> DriverInfos { get; set; } = new List<DriverInfomation>();
        private bool isClickCol = false;
        protected DateTime SelectedDate { get; set; }
        private CancellationTokenSource source { get; set; } = new CancellationTokenSource();

        [JSInvokable]
        public void InvokeClickOutside()
        {
            try
            {
                if (!isClickCol)
                {
                    CustomStyle = string.Empty;
                    Sdi = null;
                    Sbi = null;
                }
                else
                {
                    isClickCol = false;
                }

                StateHasChanged();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var filters = await _filterService.GetFilterCondition(FormFilterName.AvailabilityCheck, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq);
                if (filters != null && filters.Any())
                {
                    var date = filters.FirstOrDefault()?.JoInput;
                    if (date != null)
                    {
                        var newDate = DateTime.ParseExact(date, DateTimeFormat.yyyyMMdd, CultureInfo.InvariantCulture);
                        GetDaysOf2Weeks(newDate);
                    }
                }
                else
                    GetDaysOf2Weeks(DateTime.Now);

                await GetCompany();
                await GetBranch();
                await Reload();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                if (firstRender)
                {
                    await jSRuntime.InvokeVoidAsync("initClickOutSide", "availability-check-detail-popup", DotNetObjectReference.Create(this));
                }
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        private async Task GetCompany()
        {
            companyListItems = await _transportationSummaryService.GetCompanyListItems(0);
        }

        private async Task GetBranch()
        {
            foreach (var i in companyListItems)
            {
                var lst = await _transportationSummaryService.GetEigyoListItems(i.CompanyCdSeq, new ClaimModel().TenantID);
                eigyoListItems.AddRange(lst);
            }
        }

        private void ShowPopupForDriver(DriverInfomation driver)
        {
            Sdi = driver;
            Sbi = null;
            isClickCol = true;
        }

        private void ShowPopupForBus(BusInfomation bus)
        {
            Sbi = bus;
            Sdi = null;
            isClickCol = true;
        }

        protected string GetHeightForBus(BusInfomation busInfo)
        {
            try
            {
                var total = busInfo.LargeBusCount + busInfo.MediumBusCount + busInfo.SmallBusCount;
                var empty = total - (busInfo.InUseLargeBusCount + busInfo.InUseMediumBusCount + busInfo.InUseSmallBusCount);
                return total == 0 ? "0%" : string.Format("{0}%", Math.Round(empty * 100.0 / total));
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
                return string.Empty;
            }
        }

        protected string GetHeightForDriver(DriverInfomation driver)
        {
            try
            {
                return driver.LargeDriverCount == 0 ? "0%" : string.Format("{0}%", Math.Round((driver.LargeDriverCount - driver.AbsenceLargeDriverCount) * 100.0 / driver.LargeDriverCount));
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
                return string.Empty;
            }
        }

        protected async Task ShowPopup(MouseEventArgs args, BusInfomation bus, DriverInfomation driver)
        {
            try
            {
                if (bus != null)
                    ShowPopupForBus(bus);
                else if (driver != null)
                    ShowPopupForDriver(driver);
                else return;

                // Hard code ShowPopupForBuswidth popup = 130px
                var viewWidth = await jSRuntime.InvokeAsync<int>("getViewWidth");
                if (args.ClientX + 130 > viewWidth)
                {
                    CustomStyle = $"left: {args.ClientX - 130}px; top: {args.ClientY}px; display: block;";
                }
                else
                {
                    CustomStyle = $"left: {args.ClientX}px; top: {args.ClientY}px; display: block;";
                }
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        private void GetDaysOf2Weeks(DateTime selectedDate)
        {
            SelectedDate = selectedDate;
            DaysOf2Weeks.Clear();
            for (var i = 0; i < 14; i++)
            {
                DaysOf2Weeks.Add(selectedDate.AddDays(i));
            }
        }
        protected string GetDayClass(DateTime date)
        {
            try
            {
                return date.DayOfWeek == DayOfWeek.Saturday ? "saturday-color" : date.DayOfWeek == DayOfWeek.Sunday ? "sunday-color" : string.Empty;
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
                return string.Empty;
            }
        }

        protected async Task PreviousClick()
        {
            try
            {
                GetDaysOf2Weeks(DaysOf2Weeks.First().AddDays(-14));
                await Reload();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }
        private string GetCompnyCdSeq() => companyListItems == null || !companyListItems.Any() ? string.Empty : string.Join(',', companyListItems.Select(c => c.CompanyCdSeq));
        private string GetEiygoCdSeq() => eigyoListItems == null || !eigyoListItems.Any() ? string.Empty : string.Join(',', eigyoListItems.Select(c => c.EigyoCdSeq));
        private string GetSyainCdSeq(IEnumerable<int> seqs) => seqs == null || seqs.Any() ? string.Empty : string.Join(',', seqs);

        private async Task Reload()
        {
            try
            {
                await loading.ShowAsync();
                CustomStyle = string.Empty;

                var eiygos = GetEiygoCdSeq();
                var tenantId = new ClaimModel().TenantID;

                var busAllocations = await _service.GetBusAllocation(DaysOf2Weeks.First(), DaysOf2Weeks.Last(), tenantId, source.Token);
                var busData = await _service.GetBusData(DaysOf2Weeks.First(), DaysOf2Weeks.Last(), tenantId, eiygos, GetCompnyCdSeq(), source.Token);
                var repairData = await _service.GetShuri(DaysOf2Weeks.First(), DaysOf2Weeks.Last(), source.Token);

                var employees = await _service.GetEmployeeData(DaysOf2Weeks.First(), DaysOf2Weeks.Last(), tenantId, eiygos, source.Token);
                var workHolidayData = await _service.GetWorkHolidayData(DaysOf2Weeks.First(), DaysOf2Weeks.Last(), tenantId, GetSyainCdSeq(employees.Select(e => e.SyainCdSeq)), source.Token);
                var staffs = await _service.GetStaffData(DaysOf2Weeks.First(), DaysOf2Weeks.Last(), tenantId, source.Token);
                BusInfos.Clear();
                foreach (var day in DaysOf2Weeks)
                {
                    BusInfos.Add(_service.CaculateBusInfo(day, busAllocations, busData, repairData));
                }
                DriverInfos.Clear();
                foreach (var day in DaysOf2Weeks)
                {
                    DriverInfos.Add(_service.CaculateDriverInfo(day, employees, workHolidayData, staffs));
                }

                await InvokeAsync(StateHasChanged);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                await loading.HideAsync();
            }
        }
        protected async Task NextClick()
        {
            try
            {
                GetDaysOf2Weeks(DaysOf2Weeks.Last().AddDays(1));
                await Reload();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }
        protected async Task CurrentDateClick()
        {
            try
            {
                GetDaysOf2Weeks(DateTime.Now);
                await Reload();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected async Task SelectedDateChanged(DateTime date)
        {
            try
            {
                GetDaysOf2Weeks(date);
                await SaveSearchModel(date);
                await Reload();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected async Task RefreshClick()
        {
            try
            {
                await Reload();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected async Task MouseDown()
        {
            try
            {
                await jSRuntime.InvokeVoidAsync("loadPageScript", "vehicleavailabilityconfirmationmobile", "mouseDown");
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected async Task MouseUp(DateTime dateTime)
        {
            try
            {
                await jSRuntime.InvokeVoidAsync("loadPageScript", "vehicleavailabilityconfirmationmobile", "mouseUp", DotNetObjectReference.Create(this), dateTime);
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }


        private async Task SaveSearchModel(DateTime date)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add(nameof(SelectedDate), SelectedDate.ToString(DateTimeFormat.yyyyMMdd));
            await _filterService.SaveFilterCondtion(result, FormFilterName.AvailabilityCheck, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq);
        }


        [JSInvokable]
        public void RedirectVehicleAvailabilityConfirmation(DateTime dateTime)
        {
            try
            {
                _navigationManager.NavigateTo(string.Format("/vehicleavailabilityconfirmationmobile?startDateParam={0}", dateTime.ToString(Formats.yyyyMMdd)));
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        public void Dispose()
        {
            source.Cancel();
        }
    }
}
