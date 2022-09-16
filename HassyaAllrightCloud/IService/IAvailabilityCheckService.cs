using HassyaAllrightCloud.Commons;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using StoredProcedureEFCore;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using HassyaAllrightCloud.Commons.Extensions;

namespace HassyaAllrightCloud.IService
{
    public interface IAvailabilityCheckService
    {
        Task<List<ItemBus>> GetBusData(DateTime st, DateTime end);
        Task<List<KyoSHeDatabyDate>> GetDriver(List<DateTime> daysOf2Weeks, List<int> branchlst, List<BranchChartData> branchchart);
        DriverInfomation CaculateDriverInfo(DateTime date, List<KyoSHeDatabyDate> kyoSHeDatabyDatelst);
        Task<List<BusDataType>> GetBusList(List<DateTime> daysOf2Weeks, List<int> comlst, List<int> branchlst, List<ItemBus> buslines, List<BranchChartData> branchchart);
        BusInfomation CaculateBusInfo(DateTime date, List<BusDataType> busnames, List<ItemBus> buslines, List<int> branchlst);
        Task<List<BusAllocation>> GetBusAllocation(DateTime from, DateTime to, int tenantCdSeq, CancellationToken token);
        Task<List<BusData>> GetBusData(DateTime from, DateTime to, int tenantCdSeq, string eigyos, string compny, CancellationToken token);
        Task<List<ShuriData>> GetShuri(DateTime from, DateTime to, CancellationToken token);
        BusInfomation CaculateBusInfo(DateTime day, List<BusAllocation> busAllocations, List<BusData> busData, List<ShuriData> shuri);
        Task<List<EmployeeData>> GetEmployeeData(DateTime from, DateTime to, int tenantCdSeq, string eigyos, CancellationToken token);
        Task<List<WorkHolidayData>> GetWorkHolidayData(DateTime from, DateTime to, int tenantCdSeq, string syainCdSeq, CancellationToken token);
        Task<List<StaffInfo>> GetStaffData(DateTime from, DateTime to, int tenantCdSeq, CancellationToken token);
        DriverInfomation CaculateDriverInfo(DateTime day, List<EmployeeData> employees, List<WorkHolidayData> workHolidayData, List<StaffInfo> staffs);
    }

    public class AvailabilityCheckService : IAvailabilityCheckService
    {
        private IBusBookingDataListService BusBookingDataService { get; set; }
        private ITPM_TokiskDataListService TPM_TokiskDataService { get; set; }
        private ITPM_KyoSHeDataListService TPM_KyoSHeDataService { get; set; }
        private BusScheduleHelper BusScheduleHelper { get; set; }
        private IBusDataListService BusDataService { get; set; }
        private KobodbContext _context { get; set; }
        public AvailabilityCheckService(
            IBusBookingDataListService busBookingDataService,
            ITPM_TokiskDataListService tPM_TokiskDataService,
            ITPM_KyoSHeDataListService tPM_KyoSHeDataService,
            BusScheduleHelper busScheduleHelper,
            IBusDataListService busDataService,
            KobodbContext context)
        {
            BusBookingDataService = busBookingDataService;
            TPM_TokiskDataService = tPM_TokiskDataService;
            TPM_KyoSHeDataService = tPM_KyoSHeDataService;
            BusScheduleHelper = busScheduleHelper;
            BusDataService = busDataService;
            _context = context;
        }

        public BusInfomation CaculateBusInfo(DateTime date, List<BusDataType> busNames, List<ItemBus> busLines, List<int> branchlst)
        {
            string datestr = date.ToString(DateTimeFormat.yyyyMMdd);
            int countbusname = busNames.Where(x => x.IsOutOfDate == false && x.NinkaKbn != 7).Select(x => x.BusID).Count() + busNames.Where(x => x.IsOutOfDate && x.IsHasBooking & x.IsActive & x.NinkaKbn != 7).Select(x => x.BusID).Count();
            var countbusnameLarge = busNames.Where(x => x.IsOutOfDate == false && x.NinkaKbn != 7 && x.KataKbn == 1).Select(x => x.BusID).Count() + busNames.Where(x => x.IsOutOfDate && x.IsHasBooking & x.IsActive & x.NinkaKbn != 7 && x.KataKbn == 1).Select(x => x.BusID).Count();
            var countbusnameMedium = busNames.Where(x => x.IsOutOfDate == false && x.NinkaKbn != 7 && x.KataKbn == 2).Select(x => x.BusID).Count() + busNames.Where(x => x.IsOutOfDate && x.IsHasBooking & x.IsActive & x.NinkaKbn != 7 && x.KataKbn == 2).Select(x => x.BusID).Count();
            var countbusnameSmall = busNames.Where(x => x.IsOutOfDate == false && x.NinkaKbn != 7 && x.KataKbn == 3).Select(x => x.BusID).Count() + busNames.Where(x => x.IsOutOfDate && x.IsHasBooking & x.IsActive & x.NinkaKbn != 7 && x.KataKbn == 3).Select(x => x.BusID).Count();

            var countbusLarge = busLines.Where(t => t.KSKbn == 2 && string.Compare(t.StartDateDefault, datestr) <= 0 && string.Compare(t.EndDateDefault, datestr) >= 0 && branchlst.Contains(t.BranchId) && t.Syasyu_KataKbn == 1 && t.SyaRyo_NinKaKbn != 7).Select(x => x.BusLine).Distinct().Count();
            var countbusMedium = busLines.Where(t => t.KSKbn == 2 && string.Compare(t.StartDateDefault, datestr) <= 0 && string.Compare(t.EndDateDefault, datestr) >= 0 && branchlst.Contains(t.BranchId) && t.Syasyu_KataKbn == 2 && t.SyaRyo_NinKaKbn != 7).Select(x => x.BusLine).Distinct().Count();
            var countbusSmall = busLines.Where(t => t.KSKbn == 2 && string.Compare(t.StartDateDefault, datestr) <= 0 && string.Compare(t.EndDateDefault, datestr) >= 0 && branchlst.Contains(t.BranchId) && t.Syasyu_KataKbn == 3 && t.SyaRyo_NinKaKbn != 7).Select(x => x.BusLine).Distinct().Count();

            return new BusInfomation()
            {
                InUseLargeBusCount = countbusLarge,
                InUseMediumBusCount = countbusMedium,
                InUseSmallBusCount = countbusSmall,
                LargeBusCount = countbusnameLarge,
                MediumBusCount = countbusnameMedium,
                SmallBusCount = countbusnameSmall,
                DateSelected = date
            };
        }

        public async Task<List<BusDataType>> GetBusList(List<DateTime> daysOf2Weeks,
            List<int> comlst, List<int> branchlst, List<ItemBus> buslines, List<BranchChartData> branchchart)
        {
            DateTime startDateFilter = daysOf2Weeks.First();
            DateTime endDateFilter = daysOf2Weeks.Last();
            List<BusDataType> busnames = new List<BusDataType>();
            List<BusInfoData> bus = new List<BusInfoData>();
            bus = await BusDataService.Getbus(daysOf2Weeks.First(), new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, comlst.ToArray(), branchlst.ToArray(), new ClaimModel().TenantID);
            BusInfoData itemBusTemp = new BusInfoData();

            List<BusInfoData> listBusTemp = new List<BusInfoData>();

            foreach (var item in bus)
            {
                if (item.StaYmd.CompareTo(startDateFilter.ToString("yyyyMMdd")) <= 0
                    && item.EndYmd.CompareTo(startDateFilter.ToString("yyyyMMdd")) >= 0
                    && item.EndYmd.CompareTo(endDateFilter.ToString("yyyyMMdd")) <= 0)
                {
                    listBusTemp.Add(item);
                }
                if (item.StaYmd.CompareTo(startDateFilter.ToString("yyyyMMdd")) >= 0
                    && item.EndYmd.CompareTo(endDateFilter.ToString("yyyyMMdd")) >= 0
                    && item.StaYmd.CompareTo(endDateFilter.ToString("yyyyMMdd")) <= 0)
                {
                    listBusTemp.Add(item);
                }
                if (item.StaYmd.CompareTo(startDateFilter.ToString("yyyyMMdd")) >= 0
                    && item.EndYmd.CompareTo(endDateFilter.ToString("yyyyMMdd")) <= 0)
                {
                    listBusTemp.Add(item);
                }
                if (item.StaYmd.CompareTo(startDateFilter.ToString("yyyyMMdd")) <= 0
                    && item.EndYmd.CompareTo(endDateFilter.ToString("yyyyMMdd")) >= 0)
                {
                    listBusTemp.Add(item);
                }
            }
            bus = listBusTemp.Distinct().ToList();
            foreach (var i in bus)
            {
                if (!busnames.Where(t => t.BusID == i.SyaRyoCdSeq.ToString()).Any())
                {
                    BusDataType busItem = new BusDataType();
                    busItem.BusID = i.SyaRyoCdSeq.ToString();
                    busItem.SyaSyuNm = i.SyaSyuNm;
                    busItem.SyaRyoNm = i.SyaRyoNm;
                    busItem.RyakuNm = i.RyakuNm;
                    busItem.EigyoNm = i.EigyoNm;
                    busItem.KariSyaRyoNm = i.KariSyaRyoNm;
                    //busItem.BusHeight = 2;
                    busItem.BusVehicle = 0;
                    busItem.BusBranchID = i.EigyoCdSeq;
                    busItem.BusCompanyID = i.CompanyCdSeq;
                    busItem.SyaSyuCd = i.SyaSyuCd;
                    busItem.EigyoCd = i.EigyoCd;
                    busItem.SyaRyoCd = i.SyaRyoCd;
                    busItem.TenkoNo = i.TenkoNo;
                    if (bus.Where(t => t.SyaRyoCdSeq == i.SyaRyoCdSeq).Count() >= 2)
                    {
                        busItem.StaYmd = bus.OrderBy(t => t.EndYmd).Where(t => t.SyaRyoCdSeq == i.SyaRyoCdSeq).First().StaYmd;
                        busItem.EndYmd = bus.OrderBy(t => t.EndYmd).Where(t => t.SyaRyoCdSeq == i.SyaRyoCdSeq).Last().EndYmd;
                    }
                    else
                    {
                        busItem.StaYmd = i.StaYmd;
                        busItem.EndYmd = i.EndYmd;
                    }
                    busItem.NinkaKbn = i.NinkaKbn;
                    busItem.TeiCnt = i.TeiCnt;
                    busItem.KataKbn = i.KataKbn;
                    busItem.LockYmd = i.LockYmd;
                    busItem.SyaSyuCdSeq = i.SyaSyuCdSeq;
                    busnames.Add(busItem);
                }
            }

            if (buslines.Count != 0)
            {
                foreach (var item in buslines)
                {
                    if (item.BusLine != "0" && item.KSKbn == 2 && busnames.Where(x => x.BusID == item.BusLine).Count() > 0)
                    {
                        int brandid = busnames.Where(x => x.BusID == item.BusLine).First().BusBranchID;
                        item.BranchId = brandid;
                        item.CompanyId = branchchart.Where(a => a.EigyoCdSeq == brandid).First().CompanyCdSeq;

                        // check table lock
                        string lockymd = null;
                        if ((lockymd = busnames.Where(x => x.BusID == item.BusLine).First().LockYmd) != null
                            && DateTime.TryParseExact(lockymd, "yyyyMMdd", null, DateTimeStyles.None, out DateTime lockDate)
                            && DateTime.TryParseExact(item.SeiTaiYmd, "yyyyMMdd", null, DateTimeStyles.None, out DateTime seiTaiDate))
                        {
                            if (seiTaiDate < lockDate)
                            {
                                item.AllowDrop = false;
                                item.AllowEdit = false;
                            }
                        }
                    }
                }
            }

            UpdateBusList(buslines, busnames, daysOf2Weeks.First());

            return busnames;
        }

        private void UpdateBusList(List<ItemBus> buslines, List<BusDataType> busnames, DateTime startDate)
        {
            List<string> buslineid = buslines.Select(t => t.BusLine).ToList();
            foreach (var i in busnames)
            {
                if (DateTime.Compare(startDate, DateTime.ParseExact(i.StaYmd, "yyyyMMdd", CultureInfo.InvariantCulture)) >= 0 && DateTime.Compare(startDate, DateTime.ParseExact(i.EndYmd, "yyyyMMdd", CultureInfo.InvariantCulture)) < 0)
                {
                    i.IsOutOfDate = false;
                }
                else
                {
                    i.IsOutOfDate = true;
                }
                if (buslineid.ToArray().Contains(i.BusID))
                {
                    if (busnames.Where(t => t.BusID == i.BusID).Count() > 1)
                    {
                        if (i.IsOutOfDate == true)
                        {
                            i.IsHasBooking = false;
                        }
                        else
                        {
                            i.IsHasBooking = true;
                        }
                    }
                    else
                    { i.IsHasBooking = true; }
                }
                else
                {
                    i.IsHasBooking = false;
                }
            }

            List<string> busid = busnames.Where(x => x.IsOutOfDate == false).Select(x => x.BusID).ToList();
            List<BusDataType> busnameexpried = busnames.Where(x => x.IsOutOfDate && x.IsHasBooking).ToList();
            foreach (var i in busnameexpried)
            {
                if (busid.ToArray().Contains(i.BusID))
                {
                    i.IsActive = true;
                }
                else
                {
                    if (busnameexpried.Where(x => x.BusID == i.BusID).Count() > 1)
                    {
                        i.IsActive = true;
                        busnameexpried.Where(x => x.BusID == i.BusID).First().IsActive = true;
                    }
                    else
                    {
                        i.IsActive = true;
                    }
                }
            }
        }

        public DriverInfomation CaculateDriverInfo(DateTime date, List<KyoSHeDatabyDate> kyoSHeDatabyDatelst)
        {
            int countBusdriverLarge = 0, countBusdriverMedium = 0, countBusdriverSmall = 0, countBusdriveroffLarge = 0, countBusdriveroffMedium = 0, countBusdriveroffSmall = 0;
            if (kyoSHeDatabyDatelst != null)
            {

                if (kyoSHeDatabyDatelst.Count != 0 && kyoSHeDatabyDatelst.Where(t => t.date == date).Count() > 0)
                {
                    var listKyoSHebyDateitem = kyoSHeDatabyDatelst.Where(t => t.date == date).First().KyoSHelst;

                    countBusdriverLarge = listKyoSHebyDateitem.Where(t => (t.SyokumuCdSeq == 1 || t.SyokumuCdSeq == 2) && t.BigTypeDrivingFlg == 1).Count();
                    countBusdriverMedium = listKyoSHebyDateitem.Where(t => (t.SyokumuCdSeq == 1 || t.SyokumuCdSeq == 2) && t.MediumTypeDrivingFlg == 1).Count();
                    countBusdriverSmall = listKyoSHebyDateitem.Where(t => (t.SyokumuCdSeq == 1 || t.SyokumuCdSeq == 2) && t.SmallTypeDrivingFlg == 1).Count();
                }

                if (kyoSHeDatabyDatelst.Count != 0 && kyoSHeDatabyDatelst.Where(t => t.date == date).Select(t => t.Kobandriverlst).ToList().Count() > 0)
                {
                    countBusdriveroffLarge = kyoSHeDatabyDatelst.Where(t => t.date == date).First().Kobandriverlst.Where(t => t.BigTypeDrivingFlg == 1).Count();
                    countBusdriveroffMedium = kyoSHeDatabyDatelst.Where(t => t.date == date).First().Kobandriverlst.Where(t => t.MediumTypeDrivingFlg == 1).Count();
                    countBusdriveroffSmall = kyoSHeDatabyDatelst.Where(t => t.date == date).First().Kobandriverlst.Where(t => t.SmallTypeDrivingFlg == 1).Count();
                }
            }

            return new DriverInfomation()
            {
                AbsenceLargeDriverCount = countBusdriveroffLarge,
                AbsenceMediumDriverCount = countBusdriveroffMedium,
                AbsenceSmallDriverCount = countBusdriveroffSmall,
                LargeDriverCount = countBusdriverLarge,
                MediumDriverCount = countBusdriverMedium,
                SmallDriverCount = countBusdriverSmall,
                DateSelected = date
            };
        }

        public async Task<List<KyoSHeDatabyDate>> GetDriver(List<DateTime> daysOf2Weeks, List<int> branchlst, List<BranchChartData> branchchart)
        {
            List<KyoSHeDatabyDate> KyoSHeDatabyDatelst = new List<KyoSHeDatabyDate>();
            int d = daysOf2Weeks.Count;
            foreach (var day in daysOf2Weeks)
            {
                KyoSHeDatabyDate KyoSHeDatabyDateitem = new KyoSHeDatabyDate();
                KyoSHeDatabyDateitem.date = day;
                List<TPM_KyoSHeData> KyoSHeData = await TPM_KyoSHeDataService.Getdata(day, branchlst.ToArray(), new ClaimModel().TenantID);
                foreach (var item in KyoSHeData)
                {
                    KyoSHeData.Where(t => t.SyainCdSeq == item.SyainCdSeq && t.EndYmd == item.EndYmd).First().CompanyCdSeq = branchchart.Where(a => a.EigyoCdSeq == item.EigyoCdSeq).First().CompanyCdSeq;
                }
                KyoSHeDatabyDateitem.KyoSHelst = KyoSHeData;
                int[] iddriver = { 1, 2 };
                int[] idguider = { 3, 4 };
                List<int> SyainCdSeqdriverslt = KyoSHeDatabyDateitem.KyoSHelst.Where(t => iddriver.Contains(t.SyokumuCdSeq)).Select(t => t.SyainCdSeq).ToList();
                List<int> SyainCdSeqguiderslt = KyoSHeDatabyDateitem.KyoSHelst.Where(t => idguider.Contains(t.SyokumuCdSeq)).Select(t => t.SyainCdSeq).ToList();
                SyainCdSeqdriverslt = KyoSHeDatabyDateitem.KyoSHelst.Where(t => iddriver.Contains(t.SyokumuCdSeq)).Select(t => t.SyainCdSeq).ToList();
                SyainCdSeqguiderslt = KyoSHeDatabyDateitem.KyoSHelst.Where(t => idguider.Contains(t.SyokumuCdSeq)).Select(t => t.SyainCdSeq).ToList();
                KyoSHeDatabyDateitem.Kikyujdriverlst = await TPM_KyoSHeDataService.Getdataoff(day, SyainCdSeqdriverslt.ToArray());
                KyoSHeDatabyDateitem.Kikyujguiderlst = await TPM_KyoSHeDataService.Getdataoff(day, SyainCdSeqguiderslt.ToArray());
                KyoSHeDatabyDateitem.Kobandriverlst = await TPM_KyoSHeDataService.GetdataKoban(day, SyainCdSeqdriverslt.ToArray());
                KyoSHeDatabyDatelst.Add(KyoSHeDatabyDateitem);
            }
            return KyoSHeDatabyDatelst;
        }

        public async Task<List<ItemBus>> GetBusData(DateTime st, DateTime end)
        {
            List<BusBookingData> busBookingData = new List<BusBookingData>();
            List<TokiskChartData> tokisk = new List<TokiskChartData>();
            List<ItemBus> buslines = new List<ItemBus>();
            busBookingData.Clear();
            busBookingData = await BusBookingDataService.Getbusdatabooking(st, end, 1, new ClaimModel().TenantID);

            foreach (BusBookingData i in busBookingData)
            {
                tokisk = await TPM_TokiskDataService.getdata(i.Unkobi_HaiSYmd);
                ItemBus newBusbookingdata = new ItemBus();

                newBusbookingdata.BookingId = i.Haisha_UkeNo;
                newBusbookingdata.haUnkRen = i.Haisha_UnkRen;
                newBusbookingdata.TeiDanNo = i.Haisha_TeiDanNo;
                newBusbookingdata.BunkRen = i.Haisha_BunkRen;
                newBusbookingdata.HenKai = i.Haisha_HenKai;
                newBusbookingdata.Id = i.Haisha_GoSya;
                newBusbookingdata.UkeCd = i.Yyksho_UkeCd;
                newBusbookingdata.BusLine = i.Haisha_HaiSSryCdSeq.ToString();
                newBusbookingdata.StartDate = i.Haisha_HaiSYmd;
                newBusbookingdata.TimeStart = int.Parse(i.Haisha_HaiSTime);

                DateTime enddatetime = DateTime.ParseExact(i.Haisha_TouYmd, "yyyyMMdd", CultureInfo.InvariantCulture);
                DateTime enddatetimemax = DateTime.ParseExact(i.Unkobi_TouYmd, "yyyyMMdd", CultureInfo.InvariantCulture);
                string enddatest = i.Haisha_TouYmd;
                string endtime = i.Haisha_TouChTime;

                if (BusScheduleHelper.ConvertTime(i.Haisha_TouChTime).Days >= 1)
                {
                    enddatest = enddatetime.AddDays(BusScheduleHelper.ConvertTime(i.Haisha_TouChTime).Days).ToString("yyyyMMdd");
                    endtime = BusScheduleHelper.ConvertTime(i.Haisha_TouChTime).Hours.ToString("D2") + BusScheduleHelper.ConvertTime(i.Haisha_TouChTime).Minutes.ToString("D2");
                }
                else
                {
                    enddatest = i.Haisha_TouYmd;
                    endtime = i.Haisha_TouChTime;
                }
                newBusbookingdata.EndDate = enddatest;
                newBusbookingdata.TimeEnd = int.Parse(endtime);
                //update line caculation time gray
                DateTime enddatetimegray = DateTime.ParseExact(i.Haisha_KikYmd, "yyyyMMdd", CultureInfo.InvariantCulture);
                string enddatestrgray = i.Haisha_KikYmd;
                string endtimegray = i.Haisha_KikTime;
                if (BusScheduleHelper.ConvertTime(i.Haisha_KikTime).Days >= 1)
                {
                    enddatestrgray = enddatetimegray.AddDays(BusScheduleHelper.ConvertTime(i.Haisha_KikTime).Days).ToString("yyyyMMdd");
                    endtimegray = BusScheduleHelper.ConvertTime(i.Haisha_KikTime).Hours.ToString("D2") + BusScheduleHelper.ConvertTime(i.Haisha_KikTime).Minutes.ToString("D2");
                }
                else
                {
                    enddatestrgray = i.Haisha_KikYmd;
                    endtimegray = i.Haisha_KikTime;
                }
                newBusbookingdata.StartDateDefault = i.Haisha_SyuKoYmd;
                newBusbookingdata.EndDateDefault = enddatestrgray;
                newBusbookingdata.TimeStartDefault = int.Parse(i.Haisha_SyuKoTime);
                newBusbookingdata.TimeEndDefault = int.Parse(endtimegray);
                newBusbookingdata.JyoSyaJin = i.Haisha_JyoSyaJin;

                if (i.Haisha_DrvJin > 1)
                {
                    newBusbookingdata.ColorLine += " border-black";
                }
                newBusbookingdata.Status = 1;
                newBusbookingdata.CCSStyle = "";
                newBusbookingdata.Top = 0.3125;
                newBusbookingdata.Height = 2;
                newBusbookingdata.Name = "";
                newBusbookingdata.DanTaNm = i.Unkobi_DanTaNm;
                newBusbookingdata.IkNm = i.Haisha_IkNm;
                newBusbookingdata.TokuiNm = i.TokiSt_RyakuNm;
                newBusbookingdata.NumberDriver = i.Haisha_DrvJin;
                newBusbookingdata.NumberGuider = i.Haisha_GuiSu;

                newBusbookingdata.MinDate = i.Unkobi_HaiSYmd;
                newBusbookingdata.MinTime = int.Parse(i.Unkobi_HaiSTime);
                if (BusScheduleHelper.ConvertTime(i.Unkobi_TouChTime).Days >= 1)
                {
                    newBusbookingdata.Maxdate = enddatetimemax.AddDays(BusScheduleHelper.ConvertTime(i.Unkobi_TouChTime).Days).ToString("yyyyMMdd");
                    newBusbookingdata.MaxTime = int.Parse(BusScheduleHelper.ConvertTime(i.Unkobi_TouChTime).Hours.ToString("D2") + BusScheduleHelper.ConvertTime(i.Unkobi_TouChTime).Minutes.ToString("D2"));
                }
                else
                {
                    newBusbookingdata.Maxdate = i.Unkobi_TouYmd;
                    newBusbookingdata.MaxTime = int.Parse(i.Unkobi_TouChTime);
                }

                newBusbookingdata.HasYmd = i.Haisha_HaiSYmd;
                newBusbookingdata.Zeiritsu = i.Yyksho_Zeiritsu;
                newBusbookingdata.BookingType = i.Yyksho_YoyaKbnSeq;
                newBusbookingdata.KSKbn = i.Haisha_KSKbn;
                newBusbookingdata.YouTblSeq = i.Haisha_YouTblSeq;
                newBusbookingdata.SyaSyu_SyaSyuNm = i.SyaSyu_SyaSyuNm;
                newBusbookingdata.BusLineType = i.Haisha_UkeNo.ToString() + i.Haisha_TeiDanNo.ToString();
                if (tokisk.Where(t => t.Tokisk_TokuiSeq == i.Yousha_YouCdSeq && t.TokiSt_SitenCdSeq == i.Yousha_YouSitCdSeq).ToList().Count > 0)
                {
                    newBusbookingdata.Tokisk_YouSRyakuNm = tokisk.First(t => t.Tokisk_TokuiSeq == i.Yousha_YouCdSeq).Tokisk_RyakuNm + " " + tokisk.First(t => t.TokiSt_SitenCdSeq == i.Yousha_YouSitCdSeq).TokiSt_RyakuNm;
                    newBusbookingdata.Tokisk_SitenCdSeq = tokisk.First(t => t.TokiSt_SitenCdSeq == i.Yousha_YouSitCdSeq).TokiSt_SitenCdSeq;
                }

                newBusbookingdata.TokiSk_RyakuNm = i.TokiSk_RyakuNm;
                newBusbookingdata.TokiSt_RyakuNm = i.TokiSt_RyakuNm;
                newBusbookingdata.Shuri_ShuriTblSeq = 0;
                newBusbookingdata.SyaSyuRen = i.YykSyu_SyaSyuRen;
                newBusbookingdata.BranchId = 0;
                newBusbookingdata.CompanyId = 0;
                newBusbookingdata.BunKSyuJyn = i.Haisha_BunKSyuJyn;
                newBusbookingdata.SeiTaiYmd = i.Yyksho_SeiTaiYmd;
                if (i.Haisha_BunKSyuJyn == 0)
                {
                    newBusbookingdata.CanBeDeleted = false;
                }
                if (i.Haisha_HaiSKbn == 2 || (i.Haisha_KSKbn == 1 && i.Haisha_YouTblSeq > 0) || (i.Haisha_KSKbn == 1 && i.Haisha_YouTblSeq == 0))
                {
                    newBusbookingdata.CanSimpledispatch = false;
                }
                newBusbookingdata.Unkobi_StartYmd = i.Unkobi_HaiSYmd;
                newBusbookingdata.Syasyu_KataKbn = i.Syasyu_KataKbn;
                newBusbookingdata.HaiSsryCdSeq = i.Haisha_HaiSSryCdSeq;
                newBusbookingdata.SyuKoYmd = i.Haisha_SyuKoYmd;
                newBusbookingdata.KikYmd = i.Haisha_KikYmd;
                buslines.Add(newBusbookingdata);
            }
            return buslines;
        }

        public async Task<List<BusAllocation>> GetBusAllocation(DateTime from, DateTime to, int tenantCdSeq, CancellationToken token)
        {
            var result = new List<BusAllocation>();
            await _context.LoadStoredProc("PK_dBusAllocation_R")
                .AddParam("@From", from.ToString(DateTimeFormat.yyyyMMdd))
                .AddParam("@To", to.ToString(DateTimeFormat.yyyyMMdd))
                .AddParam("@TenantCdSeq", tenantCdSeq)
                .ExecAsync(async e => { result = await e.ToListAsync<BusAllocation>(); }, token);
            return result;
        }

        public async Task<List<BusData>> GetBusData(DateTime from, DateTime to, int tenantCdSeq, string eigyos, string compny, CancellationToken token)
        {
            var result = new List<BusData>();
            await _context.LoadStoredProc("PK_dBusData_R")
                .AddParam("@From", from.ToString(DateTimeFormat.yyyyMMdd))
                .AddParam("@To", to.ToString(DateTimeFormat.yyyyMMdd))
                .AddParam("@TenantCdSeq", tenantCdSeq)
                .AddParam("@Eigyos", eigyos)
                .AddParam("@Compny", compny)
                .ExecAsync(async e => { result = await e.ToListAsync<BusData>(); }, token);
            return result;
        }

        public async Task<List<ShuriData>> GetShuri(DateTime from, DateTime to, CancellationToken token)
        {
            var f = from.ToString(DateTimeFormat.yyyyMMdd);
            var t = to.ToString(DateTimeFormat.yyyyMMdd);
            return await _context.TkdShuri.Where(e => e.SiyoKbn == 1 && t.CompareTo(e.ShuriSymd) >= 0 && f.CompareTo(e.ShuriEymd) <= 0).Select(e =>
            new ShuriData()
            {
                SyaRyoCdSeq = e.SyaRyoCdSeq,
                ShuriSYmd = e.ShuriSymd,
                ShuriEYmd = e.ShuriEymd
            }).ToListAsync(token);
        }

        public BusInfomation CaculateBusInfo(DateTime day, List<BusAllocation> busAllocations, List<BusData> busData, List<ShuriData> repairData)
        {
            var d = day.ToString(DateTimeFormat.yyyyMMdd);
            var inUseBus = busAllocations.Where(e => e.SyuKoYmd.CompareTo(d) <= 0 && e.KikYmd.CompareTo(d) >= 0).Select(e => new { e.HaiSSryCdSeq, e.KataKbn }).DistinctBy(e => e.HaiSSryCdSeq);
            var busRepair = repairData.Where(e => e.ShuriSYmd.CompareTo(d) <= 0 && e.ShuriEYmd.CompareTo(d) >= 0).Select(e => e.SyaRyoCdSeq);
            var busCount = busData.Where(e => e.StaYmd.CompareTo(d) <= 0 && e.EndYmd.CompareTo(d) >= 0 && !busRepair.Contains(e.SyaRyoCdSeq));

            return new BusInfomation()
            {
                InUseLargeBusCount = inUseBus.Count(e => e.KataKbn == 1),
                InUseMediumBusCount = inUseBus.Count(e => e.KataKbn == 2),
                InUseSmallBusCount = inUseBus.Count(e => e.KataKbn == 3),
                LargeBusCount = busCount.Count(e => e.KataKbn == 1),
                MediumBusCount = busCount.Count(e => e.KataKbn == 2),
                SmallBusCount = busCount.Count(e => e.KataKbn == 3),
                DateSelected = day
            };
        }

        public async Task<List<EmployeeData>> GetEmployeeData(DateTime from, DateTime to, int tenantCdSeq, string eigyos, CancellationToken token)
        {
            var result = new List<EmployeeData>();
            await _context.LoadStoredProc("PK_dEmployeeData_R")
                .AddParam("@From", from.ToString(DateTimeFormat.yyyyMMdd))
                .AddParam("@To", to.ToString(DateTimeFormat.yyyyMMdd))
                .AddParam("@Eigyos", eigyos)
                .AddParam("@TenantCdSeq", tenantCdSeq)
                .ExecAsync(async e => { result = await e.ToListAsync<EmployeeData>(); }, token);
            return result;
        }

        public async Task<List<WorkHolidayData>> GetWorkHolidayData(DateTime from, DateTime to, int tenantCdSeq, string syainCdSeq, CancellationToken token)
        {
            var result = new List<WorkHolidayData>();
            await _context.LoadStoredProc("PK_dWorkHolidayData_R")
                .AddParam("@From", from.ToString(DateTimeFormat.yyyyMMdd))
                .AddParam("@To", to.ToString(DateTimeFormat.yyyyMMdd))
                .AddParam("@TenantCdSeq", tenantCdSeq)
                .AddParam("@SyainCdSeq", syainCdSeq)
                .ExecAsync(async e => { result = await e.ToListAsync<WorkHolidayData>(); }, token);
            return result;
        }

        public async Task<List<StaffInfo>> GetStaffData(DateTime from, DateTime to, int tenantCdSeq, CancellationToken token)
        {
            var result = new List<StaffInfo>();
            await _context.LoadStoredProc("PK_dStaffData_R")
                .AddParam("@From", from.ToString(DateTimeFormat.yyyyMMdd))
                .AddParam("@To", to.ToString(DateTimeFormat.yyyyMMdd))
                .AddParam("@TenantCdSeq", tenantCdSeq)
                .ExecAsync(async e => { result = await e.ToListAsync<StaffInfo>(); }, token);
            return result;
        }

        public DriverInfomation CaculateDriverInfo(DateTime day, List<EmployeeData> employees, List<WorkHolidayData> workHolidayData, List<StaffInfo> staffs)
        {
            var d = day.ToString(DateTimeFormat.yyyyMMdd);
            var driverOff = staffs.Where(e => e.SyuKoYmd.CompareTo(d) <= 0 && e.KikYmd.CompareTo(d) >= 0);
            var workHoliday = workHolidayData.Where(e => e.KinKyuSYmd.CompareTo(d) <= 0 && e.KinKyuEYmd.CompareTo(d) >= 0).Select(e => e.SyainCdSeq);
            var driverCount = employees.Where(e => e.StaYmd.CompareTo(d) <= 0 && e.EndYmd.CompareTo(d) >= 0 && !workHoliday.Contains(e.SyainCdSeq));
            return new DriverInfomation()
            {
                AbsenceLargeDriverCount = driverOff.Where(e => e.KataKbn == 1).Sum(e => e.DrvJin),
                AbsenceMediumDriverCount = driverOff.Where(e => e.KataKbn == 2).Sum(e => e.DrvJin),
                AbsenceSmallDriverCount = driverOff.Where(e => e.KataKbn == 3).Sum(e => e.DrvJin),
                LargeDriverCount = driverCount.Count(e => e.BigTypeDrivingFlg == 1),
                MediumDriverCount = driverCount.Count(e => e.MediumTypeDrivingFlg == 1),
                SmallDriverCount = driverCount.Count(e => e.SmallTypeDrivingFlg == 1),
                DateSelected = day
            };
        }
    }
}
