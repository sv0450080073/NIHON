using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.IService;
using HassyaAllrightCloud.Pages.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Pages
{
    public class VehicleDailyReportListBase : ComponentBase
    {
        [Inject]
        protected IStringLocalizer<VehicleDailyReportList> _lang { get; set; }
        [Inject]
        protected IJSRuntime jsRuntime { get; set; }
        [Inject]
        protected IVehicleDailyReportService vehicleDailyReportService { get; set; }
        [Inject]
        protected NavigationManager navigationManager { get; set; }

        [Parameter]
        public VehicleDailyReportSearchParam searchParams { get; set; }
        [Parameter]
        public byte fontSize { get; set; }
        [Parameter]
        public EventCallback<bool> DataNotFound { get; set; }

        public List<VehicleDailyReportModel> listData { get; set; } = new List<VehicleDailyReportModel>();
        public List<VehicleDailyReportModel> listDataDate { get; set; } = new List<VehicleDailyReportModel>();
        public List<CurrentBus> listCurrentBus { get; set; } = new List<CurrentBus>();
        public List<string> listUnkYmd { get; set; } = new List<string>();
        public List<VehicleDailyReportModel> listDataDisplay { get; set; } = new List<VehicleDailyReportModel>();
        public List<VehicleDailyReportChildModel> listChild { get; set; } = new List<VehicleDailyReportChildModel>();

        public byte itemPerPage { get; set; } = 25;
        public int currentPage { get; set; } = 0;
        //public int totalPage { get; set; } = 0;
        public Pagination paging = new Pagination();
        public bool isShowPopup { get; set; }
        public VehicleDailyReportModel selectedItem { get; set; }
        public bool isLoading { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await OnSearch(false, true);
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            jsRuntime.InvokeVoidAsync("scrollToTop");
            jsRuntime.InvokeVoidAsync("resizeOneColumn");
            jsRuntime.InvokeVoidAsync("selectedRow");
            return base.OnAfterRenderAsync(firstRender);
        }

        private async Task InitData()
        {
            if (searchParams.OutputKbn.Value == 0)
            {
                searchParams.selectedUnkYmd = string.Empty;
                listCurrentBus = await vehicleDailyReportService.GetListBusForSearch(searchParams);
                if (listCurrentBus.Count > 0)
                {
                    searchParams.selectedCurrentBus = listCurrentBus[0];
                    searchParams.SyaRyoCdSeq = searchParams.selectedCurrentBus.SyaRyoCdSeq;
                }
                else
                {
                    searchParams.selectedCurrentBus = null;
                    searchParams.SyaRyoCdSeq = 0;
                }
            }
            else
            {
                searchParams.selectedCurrentBus = null;
                searchParams.SyaRyoCdSeq = 0;
            }
        }

        public async Task OnClearSearch(VehicleDailyReportSearchParam input)
        {
            searchParams = input;
            await OnSearch(false, true);
        }

        private async Task ToggleLoading(bool value)
        {
            isLoading = value;
            await InvokeAsync(StateHasChanged);
            await Task.Delay(100);
        }

        public async Task OnSearch(bool isChangePage = true, bool isInitData = false, bool isBusChange = false, bool isYmdChange = true)
        {
            await ToggleLoading(true);
            if (!isChangePage)
            {
                var temp = searchParams.OutputSetting;
                searchParams.OutputSetting = 0;
                if(isInitData)
                    await InitData();

                if(searchParams.OutputKbn.Value == 0)
                {
                    listDataDate = new List<VehicleDailyReportModel>();
                }

                listUnkYmd = new List<string>();
                listData = await vehicleDailyReportService.GetListVehicleDailyReport(searchParams);

                for (int i = 0; i < listData.Count; i++)
                {
                    if(listData[i].UnkYmd == null)
                    {
                        listData[i].UnkYmd = listData[i].SyukoYmd;
                        AddListUnkYmd(listData[i].SyukoYmd);
                    }
                    else
                    {
                        AddListUnkYmd(listData[i].UnkYmd);
                    }
                    var start = DateTime.ParseExact(listData[i].SyukoYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture);
                    var end = DateTime.ParseExact(listData[i].KikYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture);
                    var rangeDate = Enumerable.Range(0, 1 + end.Subtract(start).Days).Select(offset => start.AddDays(offset).ToString(CommonConstants.FormatYMD)).ToList();
                    var tempCount = i;
                    foreach (var date in rangeDate)
                    {
                        AddListUnkYmd(date);
                        var count = 0;
                        if(searchParams.OutputKbn.Value == 0)
                        {
                            count = listData.Count(_ => _.UnkYmd == date);
                        }
                        else
                        {
                            count = listData.Count(_ => _.UnkYmd == date && _.SyaRyoCdSeq == listData[i].SyaRyoCdSeq);
                        }

                        if (count == 0)
                        {
                            tempCount = tempCount + 1;
                            listData.Insert(tempCount, createDefaultModel(date, listData[i]));
                        }
                    }
                    i = tempCount;
                }

                if(searchParams.OutputKbn.Value == 0)
                    listData = listData.OrderBy(_ => _.UnkYmd).ToList();
                listUnkYmd.Sort();

                if(searchParams.ScheduleYmdStart != null)
                {
                    listData = listData.Where(_ => _.UnkYmd.CompareTo(searchParams.ScheduleYmdStart.Value.ToString(CommonConstants.FormatYMD)) > -1).ToList();
                    listUnkYmd = listUnkYmd.Where(_ => _.Replace("/", "").CompareTo(searchParams.ScheduleYmdStart.Value.ToString(CommonConstants.FormatYMD)) > -1).ToList();
                }
                if(searchParams.ScheduleYmdEnd != null)
                {
                    listData = listData.Where(_ => _.UnkYmd.CompareTo(searchParams.ScheduleYmdEnd.Value.ToString(CommonConstants.FormatYMD)) < 1).ToList();
                    listUnkYmd = listUnkYmd.Where(_ => _.Replace("/", "").CompareTo(searchParams.ScheduleYmdEnd.Value.ToString(CommonConstants.FormatYMD)) < 1).ToList();
                }

                if (isYmdChange)
                {
                    if (listUnkYmd.Count > 0)
                    {
                        searchParams.selectedUnkYmd = listUnkYmd[0];
                    }
                    else
                    {
                        searchParams.selectedUnkYmd = null;
                    }
                }

                //totalPage = (int)Math.Ceiling(listData.Count * 1.0 / itemPerPage);
                paging.currentPage = 0;
                currentPage = 0;
                searchParams.OutputSetting = temp;
            }
            if(listData.Count > 0)
            {
                if(searchParams.OutputKbn.Value == 1)
                {
                    if (!isChangePage)
                    {
                        listDataDate = listData.ToList();
                    }
                    listData = listDataDate.Where(_ => _.UnkYmd == searchParams.selectedUnkYmd.Replace("/", "")).ToList();
                    //totalPage = (int)Math.Ceiling(listData.Count * 1.0 / itemPerPage);
                }
                listDataDisplay = listData.Skip(currentPage * itemPerPage).Take(itemPerPage).ToList();
                await DataNotFound.InvokeAsync(false);
            }
            else
            {
                listDataDisplay = new List<VehicleDailyReportModel>();
                await DataNotFound.InvokeAsync(true);
            }
            InitChildList();
            //isLoading = false;
            StateHasChanged();
            await ToggleLoading(false);
        }

        private void AddListUnkYmd(string date)
        {
            if (searchParams.OutputKbn.Value == 1 && !listUnkYmd.Contains(date.Insert(4, "/").Insert(7, "/")))
            {
                listUnkYmd.Add(date.Insert(4, "/").Insert(7, "/"));
            }
        }

        private VehicleDailyReportModel createDefaultModel(string date, VehicleDailyReportModel baseModel)
        {
            var model = new VehicleDailyReportModel()
            {
                UnkYmd = date,
                SyaryoNm = baseModel.SyaryoNm,
                DanTaNm = baseModel.DanTaNm,
                IkNm = baseModel.IkNm,
                TokuiRyakuNm = baseModel.TokuiRyakuNm,
                SitenRyakuNm = baseModel.SitenRyakuNm,
                HaiSYmd = baseModel.HaiSYmd,
                TouYmd = baseModel.TouYmd,
                SyukoYmd = baseModel.SyukoYmd,
                KikYmd = baseModel.KikYmd,
                Haisha_SyukoTime = baseModel.Haisha_SyukoTime,
                Haisha_KikTime = baseModel.Haisha_KikTime,
                Shabni_SyukoTime = baseModel.Shabni_SyukoTime,
                Shabni_KikTime = baseModel.Shabni_KikTime,
                JyoSyaJin = 0,
                PlusJin = 0,
                JisaIPKm = 0,
                JisaKSKm = 0,
                KisoIPKm = 0,
                KisoKOKm = 0,
                OthKm = 0,
                StMeter = 0,
                EndMeter = 0,
                NenryoRyakuNm1 = baseModel.NenryoRyakuNm1,
                NenryoRyakuNm2 = baseModel.NenryoRyakuNm2,
                NenryoRyakuNm3 = baseModel.NenryoRyakuNm3,
                Nenryo1 = 0,
                Nenryo2 = 0,
                Nenryo3 = 0,
                YoyaKbnNm = baseModel.YoyaKbnNm,
                UkeNo = baseModel.UkeNo,
                UnkRen = baseModel.UnkRen,
                TeiDanNo = baseModel.TeiDanNo,
                BunkRen = baseModel.BunkRen,
                SyainNm1 = baseModel.SyainNm1,
                SyainNm2 = baseModel.SyainNm2,
                SyainNm3 = baseModel.SyainNm3,
                SyainNm4 = baseModel.SyainNm4,
                SyainNm5 = baseModel.SyainNm5,
                DanTaNm2 = baseModel.DanTaNm2,
                SyaRyoCd = baseModel.SyaRyoCd,
                NenryoCd1Seq = baseModel.NenryoCd1Seq,
                NenryoCd2Seq = baseModel.NenryoCd2Seq,
                NenryoCd3Seq = baseModel.NenryoCd3Seq
            };
            return model;
        }

        private void InitChildList()
        {
            listChild = new List<VehicleDailyReportChildModel>();
            var temp = new VehicleDailyReportChildModel();
            var tempTotal = new VehicleDailyReportChildModel();
            if (listData.Count > 0)
            {
                temp.Text = "頁計";
                listDataDisplay.ForEach(e =>
                {
                    temp.NumberOfTrips += e.UnkKai;
                    temp.BoardingPersonnel += e.JyoSyaJin;
                    temp.PlusPersonnel += e.PlusJin;
                    temp.ActualKmGeneral += e.JisaIPKm;
                    temp.ActualKmHighSpeed += e.JisaKSKm;
                    temp.ForwardingKmGeneral += e.KisoIPKm;
                    temp.ForwardingKmHighSpeed += e.KisoKOKm;
                    temp.OtherKm += e.OthKm;
                    temp.TotalMile = temp.TotalMile + (e.EndMeter - e.StMeter);
                    temp.Fuel1 += e.Nenryo1;
                    temp.Fuel2 += e.Nenryo2;
                    temp.Fuel3 += e.Nenryo3;
                });

                tempTotal.Text = "累計";
                listData.ForEach(e =>
                {
                    tempTotal.NumberOfTrips += e.UnkKai;
                    tempTotal.BoardingPersonnel += e.JyoSyaJin;
                    tempTotal.PlusPersonnel += e.PlusJin;
                    tempTotal.ActualKmGeneral += e.JisaIPKm;
                    tempTotal.ActualKmHighSpeed += e.JisaKSKm;
                    tempTotal.ForwardingKmGeneral += e.KisoIPKm;
                    tempTotal.ForwardingKmHighSpeed += e.KisoKOKm;
                    tempTotal.OtherKm += e.OthKm;
                    tempTotal.TotalMile = tempTotal.TotalMile + (e.EndMeter - e.StMeter);
                    tempTotal.Fuel1 += e.Nenryo1;
                    tempTotal.Fuel2 += e.Nenryo2;
                    tempTotal.Fuel3 += e.Nenryo3;
                });
            }
            listChild.Add(temp);
            listChild.Add(tempTotal);
        }

        protected async Task OnCurrentBusChanged(CurrentBus e)
        {
            paging.currentPage = 0;
            currentPage = 0;
            searchParams.selectedCurrentBus = e;
            searchParams.SyaRyoCdSeq = searchParams.selectedCurrentBus.SyaRyoCdSeq;
            await OnSearch(false, false, true);
        }

        protected async Task OnUnkYmdChanged(string e)
        {
            paging.currentPage = 0;
            currentPage = 0;
            searchParams.selectedUnkYmd = e;
            listData = listDataDate.Where(_ => _.UnkYmd == searchParams.selectedUnkYmd.Replace("/", "")).ToList();
            listDataDisplay = listData.Skip(currentPage * itemPerPage).Take(itemPerPage).ToList();
            await DataNotFound.InvokeAsync(false);
            InitChildList();
            StateHasChanged();
            //await OnSearch(false);
        }

        protected async Task OnChangedItem(int type)
        {
            if (searchParams.OutputKbn.Value == 0)
            {
                if (listCurrentBus.Count > 0)
                {
                    var index = listCurrentBus.IndexOf(searchParams.selectedCurrentBus);
                    if (type == 0)
                    {
                        if (index - 1 > -1)
                        {
                            await OnCurrentBusChanged(listCurrentBus[index - 1]);
                        }
                        else
                        {
                            await OnCurrentBusChanged(listCurrentBus[listCurrentBus.Count - 1]);
                        }
                    }
                    else
                    {
                        if (index + 1 < listCurrentBus.Count)
                        {
                            await OnCurrentBusChanged(listCurrentBus[index + 1]);
                        }
                        else
                        {
                            await OnCurrentBusChanged(listCurrentBus[0]);
                        }
                    }
                }
            }
            else
            {
                if (listUnkYmd.Count > 0)
                {
                    var index = listUnkYmd.IndexOf(searchParams.selectedUnkYmd);
                    if (type == 0)
                    {
                        if (index - 1 > -1)
                        {
                            await OnUnkYmdChanged(listUnkYmd[index - 1]);
                        }
                        else
                        {
                            await OnUnkYmdChanged(listUnkYmd[listUnkYmd.Count - 1]);
                        }
                    }
                    else
                    {
                        if (index + 1 < listUnkYmd.Count)
                        {
                            await OnUnkYmdChanged(listUnkYmd[index + 1]);
                        }
                        else
                        {
                            await OnUnkYmdChanged(listUnkYmd[0]);
                        }
                    }
                }
            }
        }

        protected async Task OnChangePage(int page)
        {
            currentPage = page;
            await OnSearch();
        }

        protected bool OnCheckDisable(byte type)
        {
            if (type == 0)
            {
                if (searchParams.OutputKbn.Value == 0 && (listCurrentBus.Count == 0 || listCurrentBus.Count == 1 || listCurrentBus.IndexOf(searchParams.selectedCurrentBus) == 0))
                {
                    return true;
                }
                if (searchParams.OutputKbn.Value == 1 && (listUnkYmd.Count == 0 || listUnkYmd.Count == 1 || listUnkYmd.IndexOf(searchParams.selectedUnkYmd) == 0))
                {
                    return true;
                }
                return false;
            }
            else
            {
                if (searchParams.OutputKbn.Value == 0 && (listCurrentBus.Count == 0 || listCurrentBus.Count == 1 || listCurrentBus.IndexOf(searchParams.selectedCurrentBus) == listCurrentBus.Count - 1))
                {
                    return true;
                }
                if (searchParams.OutputKbn.Value == 1 && (listUnkYmd.Count == 0 || listUnkYmd.Count == 1 || listUnkYmd.IndexOf(searchParams.selectedUnkYmd) == listUnkYmd.Count - 1))
                {
                    return true;
                }
                return false;
            }
        }

		protected async Task OnRowDoubleClick(VehicleDailyReportModel item)
        {
            selectedItem = item;
            await TogglePopup(new MouseEventArgs());
        }

        protected async Task TogglePopup(MouseEventArgs e)
        {
            isShowPopup = !isShowPopup;
            if (isShowPopup) jsRuntime.InvokeVoidAsync("hidePageScroll");
            else jsRuntime.InvokeVoidAsync("showPageScroll");
            if (!isShowPopup && e.Type == "search")
                await OnSearch(false, false, false, false);
        }

        protected void OnChangeItemPerPage(byte _itemPerPage)
        {
            itemPerPage = _itemPerPage;
            StateHasChanged();
        }
    }
}
