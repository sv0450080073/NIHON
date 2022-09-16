using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.IService;
using HassyaAllrightCloud.Commons.Constants;
using Microsoft.Extensions.Localization;
using HassyaAllrightCloud.Commons;
using System.Globalization;

namespace HassyaAllrightCloud.Pages
{
    public class VehicleSchedulerMobileBase : ComponentBase
    {
        [Parameter]
        public string SyaRyoCdSeq { get; set; } = string.Empty;

        private const string DayTrip = "#2488FD";
        private const string OverNight = "#F57C01";
        private const string Repair = "#9475CC";

        public DateTime selectedMonth { get; set; } = DateTime.Now;
        public List<CalendarItem> listDate { get; set; } = new List<CalendarItem>();
        public List<int> lastItem { get; set; } = new List<int>() { 6, 13, 20, 27, 34, 41 };
        public CalendarItem selectedDate { get; set; }
        public bool IsToday { get; set; }
        public bool IsYesterday { get; set; }
        public bool IsTomorrow { get; set; }

        public VehicleSchedulerEigyoData Eigyo { get; set; }
        public List<VehicleSchedulerSyaRyoData> ListSyaRyo { get; set; } = new List<VehicleSchedulerSyaRyoData>();
        public VehicleSchedulerSyaRyoData selectedSyaRyo { get; set; }
        public List<VehicleAllocationData> listVehicleAllocation { get; set; } = new List<VehicleAllocationData>();
        public List<VehicleRepairData> listVehicleRepair { get; set; } = new List<VehicleRepairData>();
        public List<VehicleSchedulerMobileData> Data { get; set; } = new List<VehicleSchedulerMobileData>();

        [Inject]
        protected IVehicleSchedulerMobileService _service { get; set; }
        [Inject]
        protected IStringLocalizer<VehicleSchedulerMobile> Lang { get; set; }
        [Inject]
        protected IErrorHandlerService errorModalService { get; set; }
        [Inject]
        protected IFilterCondition _filterConditionService { get; set; }
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        public VehicleSchedulerMobileData PopupData { get; set; }
        public bool IsShow { get; set; } = false;
        public bool isLoading { get; set; } = true;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(SyaRyoCdSeq) || string.IsNullOrWhiteSpace(SyaRyoCdSeq))
                {
                    NavigationManager.NavigateTo("/vehicleavailabilityconfirmationmobile");
                }
                else
                {
                    int syaRyoCdSeq = int.Parse(SyaRyoCdSeq);
                    Eigyo = await _service.GetEigyoData(syaRyoCdSeq);
                    ListSyaRyo = await _service.GetListSyaRyoData(Eigyo.EigyoCdSeq);
                    selectedSyaRyo = ListSyaRyo.FirstOrDefault(_ => _.SyaRyoCdSeq == syaRyoCdSeq);
                    await OnGetData();
                    GetListDate(selectedMonth.Year, selectedMonth.Month);
                    isLoading = false;
                    StateHasChanged();
                }
            }
            catch(Exception ex)
            {
                errorModalService.HandleError(ex);
                isLoading = false;
                StateHasChanged();
            }
        }

        private async Task OnGetData()
        {
            string startYmd = (new DateTime(selectedMonth.Year, selectedMonth.Month, 1)).ToString(CommonConstants.FormatYMD);
            string endYmd = (new DateTime(selectedMonth.Year, selectedMonth.Month, DateTime.DaysInMonth(selectedMonth.Year, selectedMonth.Month))).ToString(CommonConstants.FormatYMD);
            var taskVehicleAllocation = _service.GetListVehicleAllocation(startYmd, endYmd, selectedSyaRyo.SyaRyoCdSeq, new HassyaAllrightCloud.Domain.Dto.ClaimModel().TenantID);
            var taskVehicleRepair = _service.GetListVehicleRepair(startYmd, endYmd, selectedSyaRyo.SyaRyoCdSeq);
            await Task.WhenAll(taskVehicleAllocation, taskVehicleRepair);
            listVehicleAllocation = taskVehicleAllocation.Result.OrderBy(_ => _.SyuKoYmd).ThenBy(_ => _.SyuKoTime).ToList();
            listVehicleRepair = taskVehicleRepair.Result.OrderBy(_ => _.ShuriSYmd).ThenBy(_ => _.ShuriSTime).ToList();
        }

        private void GetListDate(int year, int month)
        {
            listDate.Clear();
            var startDate = GetStartDate(year, month);
            for (int i = 0; i < 42; i++)
            {
                string status = string.Empty;
                var startDateString = startDate.AddDays(i).ToString(CommonConstants.FormatYMD);
                var allocation = listVehicleAllocation.FirstOrDefault(_ => _.SyuKoYmd.CompareTo(startDateString) < 1 && _.KikYmd.CompareTo(startDateString) > -1);
                var repair = listVehicleRepair.FirstOrDefault(_ => _.ShuriSYmd.CompareTo(startDateString) < 1 && _.ShuriEYmd.CompareTo(startDateString) > -1);
                if (repair != null)
                {
                    if (allocation != null && allocation.SyuKoYmd.CompareTo(repair.ShuriSYmd) < 1 && allocation.SyuKoTime.CompareTo(repair.ShuriSYmd) < 1)
                    {
                        status = GetStatus(allocation);
                    }
                    else
                    {
                        status = Repair;
                    }
                }
                else if (allocation != null)
                {
                    status = GetStatus(allocation);
                }

                listDate.Add(new CalendarItem() { DisplayData = startDate.AddDays(i), IsDisable = CheckDisable(startDate.AddDays(i)), Status = status });
            }
        }

        private string GetStatus(VehicleAllocationData allocation)
        {
            if (allocation.SyuKoYmd == allocation.KikYmd) return DayTrip;
            else return OverNight;
        }

        private bool CheckDisable(DateTime date)
        {
            if (date.Month < selectedMonth.Month || date.Month > selectedMonth.Month) return true;
            return false;
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

        protected void OnSelectDate(CalendarItem item)
        {
            try
            {
                if (!item.IsDisable)
                {
                    Data.Clear();
                    selectedDate = item;
                    var startDateString = selectedDate.DisplayData.ToString(CommonConstants.FormatYMD);
                    OnSetDateText();
                    var Allocations = listVehicleAllocation.Where(_ => _.KSKbn == 2 && _.NinkaKbn != 7
                                                                    && _.SyuKoYmd.CompareTo(startDateString) < 1 && _.KikYmd.CompareTo(startDateString) > -1).ToList();
                    var Repairs = listVehicleRepair.Where(_ => _.ShuriSYmd.CompareTo(startDateString) < 1 && _.ShuriEYmd.CompareTo(startDateString) > -1).ToList();

                    foreach (var allocation in Allocations)
                    {
                        VehicleSchedulerMobileData data = new VehicleSchedulerMobileData();
                        data.UkeCd = allocation.UkeCd;
                        data.UkeNo = allocation.UkeNo;
                        data.Name = allocation.DanTaNm;
                        data.Customer = string.Format("{0} ～ {1}", allocation.TokiskNm, allocation.TokiStNm);
                        data.DispatchNote = allocation.BikoNm;
                        data.PopupYmd = string.Format("{0} {1}", 
                            DateTime.ParseExact(string.Format("{0} {1}", allocation.SyuKoYmd, allocation.SyuKoTime), DateTimeFormat.yyyyMMddHHmm, CultureInfo.InvariantCulture).ToString(DateTimeFormat.yyyyMMddSlash_HHmm_ddd),
                            DateTime.ParseExact(string.Format("{0} {1}", allocation.KikYmd, allocation.KikTime), DateTimeFormat.yyyyMMddHHmm, CultureInfo.InvariantCulture).ToString(DateTimeFormat.yyyyMMddSlash_HHmm_ddd));
                        data.Type = (byte)(allocation.SyuKoYmd == allocation.KikYmd ? 1 : 2);
                        if (allocation.SyuKoYmd == allocation.KikYmd) data.Status = DayTrip;
                        else data.Status = OverNight;
                        if (allocation.SyuKoYmd.CompareTo(startDateString) < 0 && allocation.KikYmd.CompareTo(startDateString) > 0)
                        {
                            data.StartTime = Lang["starttime"];
                        }
                        else if (allocation.SyuKoYmd.CompareTo(startDateString) == 0 && allocation.KikYmd.CompareTo(startDateString) > 0)
                        {
                            data.StartTime = allocation.SyuKoYmd == startDateString ? allocation.SyuKoTime.Insert(2, ":") : "00:00";
                        }
                        else if (allocation.SyuKoYmd.CompareTo(startDateString) < 0 && allocation.KikYmd.CompareTo(startDateString) == 0)
                        {
                            data.EndTime = allocation.KikYmd == startDateString ? allocation.KikTime.Insert(2, ":") : "23:59";
                        }
                        else if (allocation.SyuKoYmd.CompareTo(startDateString) == 0 && allocation.KikYmd.CompareTo(startDateString) == 0)
                        {
                            data.StartTime = allocation.SyuKoYmd == startDateString ? allocation.SyuKoTime.Insert(2, ":") : "00:00";
                            data.EndTime = allocation.KikYmd == startDateString ? allocation.KikTime.Insert(2, ":") : "23:59";
                        }
                        data.Ymd = string.Format("{0} ～ {1}", allocation.SyuKoYmd.Insert(4, "/").Insert(7, "/"), allocation.KikYmd.Insert(4, "/").Insert(7, "/"));
                        Data.Add(data);
                    }

                    foreach (var repair in Repairs)
                    {
                        VehicleSchedulerMobileData data = new VehicleSchedulerMobileData();
                        data.Name = repair.SyuRiCd_RyakuNm;
                        data.EigyoName = repair.Eigyos_RyakuNm;
                        data.SyaRyoName = repair.SyaRyoNm;
                        data.DispatchNote = repair.BikoNm;
                        data.PopupYmd = string.Format("{0} ～ {1}",
                            DateTime.ParseExact(string.Format("{0} {1}", repair.ShuriSYmd, repair.ShuriSTime), DateTimeFormat.yyyyMMddHHmm, CultureInfo.InvariantCulture).ToString(DateTimeFormat.yyyyMMddSlash_HHmm_ddd),
                            DateTime.ParseExact(string.Format("{0} {1}", repair.ShuriEYmd, repair.ShuriETime), DateTimeFormat.yyyyMMddHHmm, CultureInfo.InvariantCulture).ToString(DateTimeFormat.yyyyMMddSlash_HHmm_ddd));
                        data.Type = 0;
                        data.Status = Repair;
                        if (repair.ShuriSYmd.CompareTo(startDateString) < 0 && repair.ShuriEYmd.CompareTo(startDateString) > 0)
                        {
                            data.StartTime = Lang["starttime"];
                        }
                        else if (repair.ShuriSYmd.CompareTo(startDateString) == 0 && repair.ShuriEYmd.CompareTo(startDateString) > 0)
                        {
                            data.StartTime = repair.ShuriSYmd == startDateString ? repair.ShuriSTime.Insert(2, ":") : "00:00";
                        }
                        else if (repair.ShuriSYmd.CompareTo(startDateString) < 0 && repair.ShuriEYmd.CompareTo(startDateString) == 0)
                        {
                            data.EndTime = repair.ShuriEYmd == startDateString ? repair.ShuriETime.Insert(2, ":") : "23:59";
                        }
                        else if (repair.ShuriSYmd.CompareTo(startDateString) == 0 && repair.ShuriEYmd.CompareTo(startDateString) == 0)
                        {
                            data.StartTime = repair.ShuriSYmd == startDateString ? repair.ShuriSTime.Insert(2, ":") : "00:00";
                            data.EndTime = repair.ShuriEYmd == startDateString ? repair.ShuriETime.Insert(2, ":") : "23:59";
                        }
                        data.Ymd = string.Format("{0} ～ {1}", repair.ShuriSYmd.Insert(4, "/").Insert(7, "/"), repair.ShuriEYmd.Insert(4, "/").Insert(7, "/"));
                        Data.Add(data);
                    }

                    Data = Data.OrderBy(_ => _.StartTime).ThenBy(_ => _.Type).ToList();
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected async Task NextPrevDate(byte type)
        {
            try
            {
                var index = listDate.IndexOf(selectedDate);
                if (type == 1 ? (index < listDate.Count && !listDate[index + 1].IsDisable) : (index > 0 && !listDate[index - 1].IsDisable))
                {
                    selectedDate = type == 1 ? listDate[index + 1] : listDate[index - 1];
                    OnSetDateText();
                    OnSelectDate(selectedDate);
                }
                else
                {
                    await ShowLoading();
                    if (type == 0)
                    {
                        selectedMonth = selectedMonth.AddMonths(-1);
                    }
                    else
                    {
                        selectedMonth = selectedMonth.AddMonths(1);
                    }
                    await OnGetData();
                    GetListDate(selectedMonth.Year, selectedMonth.Month);
                    if(type == 0)
                    {
                        selectedDate = listDate.FirstOrDefault(_ => _.DisplayData.Day == DateTime.DaysInMonth(selectedMonth.Year, selectedMonth.Month) && _.DisplayData.Month == selectedMonth.Month); 
                    }
                    else
                    {
                        selectedDate = listDate.FirstOrDefault(_ => _.DisplayData.Day == 1);
                    }
                    OnSelectDate(selectedDate);
                }
                isLoading = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        private void OnSetDateText()
        {
            IsToday = false;
            IsYesterday = false;
            IsTomorrow = false;
            if (selectedDate.DisplayData == DateTime.Today) IsToday = true;
            else if (selectedDate.DisplayData == DateTime.Today.AddDays(-1)) IsYesterday = true;
            else if (selectedDate.DisplayData == DateTime.Today.AddDays(1)) IsTomorrow = true;
        }

        protected async Task NextPrevMonth(byte type)
        {
            try
            {
                await ShowLoading();
                selectedMonth = type == 1 ? selectedMonth.AddMonths(1) : selectedMonth.AddMonths(-1);
                await OnGetData();
                GetListDate(selectedMonth.Year, selectedMonth.Month);
                isLoading = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
                isLoading = false;
                StateHasChanged();
            }
        }

        protected async Task OnChangeSyaRyo(VehicleSchedulerSyaRyoData item)
        {
            try
            {
                selectedSyaRyo = item;
                await OnRefresh();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected async Task NextPrevSyaRyo(byte type)
        {
            try
            {
                var index = ListSyaRyo.IndexOf(selectedSyaRyo);
                if (type == 1 ? index < ListSyaRyo.Count : index > 0)
                {
                    selectedSyaRyo = type == 1 ? ListSyaRyo[index + 1] : ListSyaRyo[index - 1];
                    await OnRefresh();
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected async Task OnSetCurrentDate()
        {
            try
            {
                await ShowLoading();
                if (selectedMonth.Month != DateTime.Today.Month || selectedMonth.Year != DateTime.Today.Year)
                {
                    selectedMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    await OnGetData();
                    GetListDate(selectedMonth.Year, selectedMonth.Month);
                }
                selectedDate = listDate.FirstOrDefault(_ => _.DisplayData == DateTime.Today);
                OnSetDateText();
                isLoading = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
                isLoading = false;
                StateHasChanged();
            }
        }

        protected async Task OnRefresh()
        {
            try
            {
                await ShowLoading();
                await OnGetData();
                GetListDate(selectedMonth.Year, selectedMonth.Month);
                Data.Clear();
                selectedDate = null;
                isLoading = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
                isLoading = false;
                StateHasChanged();
            }
        }

        protected void OnNavigate()
        {
            NavigationManager.NavigateTo(string.Format("reservationmobile?SyaRyoCdSeq={0}", selectedSyaRyo.SyaRyoCdSeq));
        }

        protected void OnOpenPopup(VehicleSchedulerMobileData item)
        {
            PopupData = item;
            IsShow = true;
            StateHasChanged();
        }

        protected async Task OnClosePopup(bool value = false)
        {
            if (PopupData.IsCancel)
            {
                Data.Remove(PopupData);
                await OnGetData();
                GetListDate(selectedMonth.Year, selectedMonth.Month);
            }
            PopupData = null;
            IsShow = value;
            StateHasChanged();
        }

        private async Task ShowLoading()
        {
            isLoading = true;
            await Task.Delay(100);
            await InvokeAsync(StateHasChanged);
        }
    }

    public class CalendarItem
    {
        public DateTime DisplayData { get; set; }
        public string Status { get; set; }
        public bool IsDisable { get; set; } = false;
    }
}
