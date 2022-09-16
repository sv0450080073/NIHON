using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace HassyaAllrightCloud.Commons.Helpers
{
    public class BookingInputHelper
    {
        // Because in system the input Time will larger than 23:59 so 
        // Define MyTime that will be solve this issue 
        // Example if user input 25:30, we use as "new MyTime(25,30)"
        public partial class MyTime
        {
            public int myHour { get; set; } = -1; 
            public int myMinute { get; set; } = -1;
            public MyTime(){ }
            public MyTime(int h, int m)
            {
                this.myHour = h;
                this.myMinute = m;
            }
            public MyTime(MyTime obj)
            {
                this.myHour = obj.myHour;
                this.myMinute = obj.myMinute;
            }
            public static Boolean operator >(MyTime a, MyTime b)
            {
                int m1 = (a.myHour * 60) + (a.myMinute);
                int m2 = (b.myHour * 60) + (b.myMinute);
                return (m1 > m2);
            }
            public static Boolean operator <(MyTime a, MyTime b)
            {
                int m1 = (a.myHour * 60) + (a.myMinute);
                int m2 = (b.myHour * 60) + (b.myMinute);
                return (m1 < m2);
            }
            public static Boolean operator >=(MyTime a, MyTime b)
            {
                int m1 = (a.myHour * 60) + (a.myMinute);
                int m2 = (b.myHour * 60) + (b.myMinute);
                return (m1 >= m2);
            }
            public static Boolean operator <=(MyTime a, MyTime b)
            {
                int m1 = (a.myHour * 60) + (a.myMinute);
                int m2 = (b.myHour * 60) + (b.myMinute);
                return (m1 <= m2);
            }
            public void SetNewValue(MyTime newValue)
            {
                this.myHour = newValue.myHour;
                this.myMinute = newValue.myMinute;
            }

            public static MyTime operator +(MyTime a, MyTime b)
            {
                int h = (a.myHour == -1 ? 0 : a.myHour) + (b.myHour == -1 ? 0 : b.myHour);
                int m = (a.myMinute == -1 ? 0 : a.myMinute) + (b.myMinute == -1 ? 0 : b.myMinute);
                return new MyTime(h + (m / 60), m % 60);
            }
            public static MyTime operator -(MyTime a, MyTime b)
            {
                int h = (a.myHour == -1 ? 0 : a.myHour) - (b.myHour == -1 ? 0 : b.myHour);
                int m = (a.myMinute == -1 ? 0 : a.myMinute) - (b.myMinute == -1 ? 0 : b.myMinute);
                if (m < 0)
                {
                    h -= 1;
                    m = 60 + m;
                }
                return new MyTime(h, m);
            }
            /// <summary>
            /// Convert time to minute, accept negative time
            /// </summary>
            /// <returns>converted hour and minute to all minute</returns>
            public int TotalMinutes()
            {
                return myHour * 60 + myMinute;
            }

            // Return string with format as hh:mm
            // Sample MyTime(25,30).ToString() => return "25:30"
            public override string ToString()
            {
                if (this.myHour == -1 || this.myMinute == -1) { return "--:--"; }
                return string.Format("{0:00}:{1:00}", this.myHour, this.myMinute);
            }
            private string str;
            public string Str
            {
                get
                {
                    this.str = this.ToString();
                    return this.str;
                }
                set
                {
                    MyTime tmp = MyTime.Parse(value);
                    this.myHour = tmp.myHour;
                    this.myMinute = tmp.myMinute;
                }
            }

            public static MyTime Parse(string str)
            {
                return new MyTime(int.Parse(str.Split(":")[0]), int.Parse(str.Split(":")[1]));
            }

            public static Boolean TryParse(string str, out MyTime outPutObject)
            {
                try
                {
                    str = CommonUtil.MyTimeFormat(str.ToString()); // ouput with format ##:##
                    outPutObject = MyTime.Parse(str);
                    if (outPutObject.myMinute < 0 || outPutObject.myMinute > 59)
                    {
                        outPutObject = null;
                        return false;
                    }
                    return true;
                }
                catch{
                    outPutObject = null;
                    return false;
                }
            }

            // Return string with format as hhmm
            // Sample MyTime(25,30).ToString() => return "2530"
            public string ToStringWithoutDelimiter()
            {
                if (this.myHour == -1 || this.myMinute == -1) { return "----"; }
                return string.Format("{0:00}{1:00}", this.myHour, this.myMinute);
            }
        };

        public partial class MyDate
        {
            public bool isPreviousDay { get; set; }
            public DateTime inpDate { get; set; }
            public MyTime inpTime { get; set; }
            public MyDate() { }
            public MyDate(MyDate _myDate) : this(_myDate, 0)
            {
            }
            public MyDate(MyDate _myDate, int _addMoreMinute)
            {
                this.inpDate = new DateTime(_myDate.inpDate.Year, _myDate.inpDate.Month, _myDate.inpDate.Day, 0, 0, 0);
                this.inpTime = new MyTime(_myDate.inpTime.myHour, _myDate.inpTime.myMinute);
                this.isPreviousDay = _myDate.isPreviousDay;
                this.addMinutes(_addMoreMinute);
            }

            public MyDate(MyDate _myDate, int _addMoreMinute, bool _isPreviousDay)
            {
                this.inpDate = new DateTime(_myDate.inpDate.Year, _myDate.inpDate.Month, _myDate.inpDate.Day, 0, 0, 0);
                this.inpTime = new MyTime(_myDate.inpTime.myHour, _myDate.inpTime.myMinute);
                this.isPreviousDay = _isPreviousDay;
                this.addMinutes(_addMoreMinute);
            }

            public MyDate(DateTime _inpDate, MyTime _inpTime) : this(_inpDate, _inpTime, false)
            {

            }
            private MyDate(DateTime _inpDate, MyTime _inpTime, bool _isPreviousDay)
            {
                this.inpDate = new DateTime(_inpDate.Year, _inpDate.Month, _inpDate.Day, 0, 0, 0);
                this.inpTime = _inpTime;
                this.isPreviousDay = _isPreviousDay;
            }

            // Return string of object as format yyyy/MM/dd hh:mm
            // Sample: new MyDate(DateTime.Now(), 25, 30) => 2020/04/01 25:30
            public override string ToString()
            {
                if (inpDate != null && inpTime != null)
                {
                    return string.Format("{0} {1:00}:{2:00}", inpDate.ToString("yyyy/MM/dd"), inpTime.myHour, inpTime.myMinute);
                }
                else
                {
                    return "";
                }
            }

            public DateTime ConvertedDate
            {
                get
                {
                    try
                    {
                        DateTime convDate = inpDate.AddHours(inpTime.myHour).AddMinutes(inpTime.myMinute);
                        if (isPreviousDay == true)
                        {
                            convDate = convDate.AddDays(-1);
                        }
                        return convDate;
                    }
                    catch
                    {
                        return DateTime.Today;
                    }
                }
            }

            public MyTime ConvertedTime
            {
                get
                {
                    DateTime dt = this.ConvertedDate;
                    return new MyTime(dt.Hour, dt.Minute);
                }
            }

            private void addMinutes(int _minute)
            {
                if (_minute >= 0)
                {
                    int min = this.inpTime.myMinute + _minute;
                    this.inpTime.myMinute = min % 60;
                    this.inpTime.myHour = this.inpTime.myHour + (min / 60);
                }
                else
                {
                    DateTime tmp = this.ConvertedDate.AddMinutes(_minute);
                    if (!(tmp.Day == this.inpDate.Day))
                    {
                        this.isPreviousDay = true;
                        this.inpDate = new DateTime(tmp.Year, tmp.Month, tmp.Day);
                        this.inpDate = this.inpDate.AddDays(1);
                    }
                    else
                    {
                        this.inpDate = new DateTime(tmp.Year, tmp.Month, tmp.Day);
                    }
                    this.inpTime.myHour = tmp.Hour;
                    this.inpTime.myMinute = tmp.Minute;
                }
            }
            public static MyTime operator -(MyDate s1, MyDate s2)
            {
                MyTime mt = new MyTime();
                if ((s1 != null) && (s2 != null) &&
                    s1.inpTime.myHour != -1 && s1.inpTime.myMinute != -1 &&
                    s2.inpTime.myHour != -1 && s2.inpTime.myMinute != -1 &&
                    (s1.ConvertedDate >= s2.ConvertedDate))
                {
                    TimeSpan r = s1.ConvertedDate.Subtract(s2.ConvertedDate);
                    mt.myHour = r.Hours + r.Days * 24;
                    mt.myMinute = r.Minutes;
                }
                return mt;
            }
        };

        public class Scheduler
        {
            private MyDate start;
            private MyDate end;
            public Scheduler(MyDate _start, MyDate _end)
            {
                this.start = _start;
                this.end = _end;
            }
            public int getRowforGrid()
            {
                DateTime starTmp = new DateTime(start.inpDate.Year, start.inpDate.Month, start.inpDate.Day, 0, 0, 0);
                DateTime endTmp = new DateTime(end.inpDate.Year, end.inpDate.Month, end.inpDate.Day, 0, 0, 0);
                return (endTmp - starTmp).Days + 1;
            }

            public TimeSpan getMid9EarlyDuration()
            {
                if (start.inpTime.myHour == -1 || start.inpTime.myHour == -1 || end.inpTime.myHour == -1 || end.inpTime.myHour == -1)
                {
                    return TimeSpan.Zero;
                }
                DateTime s = start.ConvertedDate;
                DateTime e = end.ConvertedDate;

                DateTime ss = new DateTime(s.Year, s.Month, s.Day, 0, 0, 0);
                DateTime ee = new DateTime(e.Year, e.Month, e.Day, 0, 0, 0);
                int h = s.Hour;
                int m = s.Minute;

                TimeSpan totalTime = TimeSpan.Zero;
                TimeSpan additionTime = TimeSpan.Zero;
                while (ss.AddDays(1) <= ee)
                {
                    additionTime = Scheduler.getMid9EarlyDurationInDay(new DateTime(ss.Year, ss.Month, ss.Day, h, m, 0), ss.AddDays(1));
                    // ss.AddDays(1) = 00:00:00 ngay hom sau cua ss (Y nghia cua no tuong duong voi 24:00:00 cua ss)

                    // Console.WriteLine(string.Format("Mid9Early in {0}: {1}", ss.ToString("yyyy-MM-dd"), additionTime.ToString()));

                    ss = ss.AddDays(1);
                    h = 0;
                    m = 0;
                    totalTime = totalTime + additionTime;

                }
                additionTime = Scheduler.getMid9EarlyDurationInDay(new DateTime(ss.Year, ss.Month, ss.Day, h, m, 0), e);
                // Console.WriteLine(string.Format("Mid9Early in {0}: {1}", ss.ToString("yyyy-MM-dd"), additionTime.ToString()));
                if (additionTime.TotalSeconds < 0)
                {
                    // Duration can not be negative
                    additionTime = new TimeSpan(0);
                }
                return totalTime + additionTime;
            }

            // Get duration from 22:00~05:00 It mean [00:00~05:00, 22:00~24:00]
            public static TimeSpan getMid9EarlyDurationInDay(DateTime s, DateTime e)
            {
                DateTime Early = new DateTime(s.Year, s.Month, s.Day, 5, 0, 0);
                DateTime Midnight = new DateTime(s.Year, s.Month, s.Day, 22, 0, 0);
                if (DateTime.Compare(e, Early) <= 0)
                { //  s,e < 5h
                    return e - s;
                }
                else
                { // e > 5h 
                    if (DateTime.Compare(e, Midnight) <= 0) // e < 22h
                    {
                        if (DateTime.Compare(s, Early) <= 0)
                        { // s < 5h
                            return Early - s;
                        }
                        else
                        {
                            return new TimeSpan(0, 0, 0);
                        }
                    }
                    else
                    { // e > 22h
                        TimeSpan t1, t2;
                        if (DateTime.Compare(s, Early) <= 0) // s < 5h
                        {
                            t1 = Early - s;
                            t2 = e - Midnight;
                            return t1 + t2;
                        }
                        else if (DateTime.Compare(s, Midnight) > 0)
                        { // s > 22h 
                            return e - s;
                        }
                        else
                        {
                            return e - Midnight;
                        }
                    }
                }
            }
        }

        public static BookingInputHelper.MyTime Round(BookingInputHelper.MyTime inpTime)
        {
            if (inpTime.myMinute < 30)
            {
                return new BookingInputHelper.MyTime(inpTime.myHour, 0);
            }
            else
            {
                return new BookingInputHelper.MyTime(inpTime.myHour + 1, 0);
            }
        }
        public static int Round(int km)
        {
            if(km % 10 == 0)
            {
                return km;
            }else {
                return (km -(km%10) + 10);
            }
        }

        public static Dictionary<RoundSettings, Func<decimal, decimal>> RoundHelper = new Dictionary<RoundSettings, Func<decimal, decimal>>
        {
            [RoundSettings.Ceiling] = (d) => Math.Ceiling(d),
            [RoundSettings.Floor] = (d) => Math.Floor(d),
            [RoundSettings.Round] = (d) => Math.Round(d, MidpointRounding.AwayFromZero),
        };

        public static Dictionary<RoundTaxAmountType, Func<decimal, decimal>> RoundTaxAmountHelper = new Dictionary<RoundTaxAmountType, Func<decimal, decimal>>
        {
            [RoundTaxAmountType.RoundUp] = (d) => Math.Ceiling(d),
            [RoundTaxAmountType.Truncate] = (d) => Math.Truncate(d),
            [RoundTaxAmountType.Rounding] = (d) => Math.Round(d, 0, MidpointRounding.AwayFromZero),
        };
        /// <summary>
        /// SyuKo - LeaveGarage, HaiS - Start, SyuPa - Begin running, Tou - End, Kik - Return Garage
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Dictionary<CommandMode, List<TkdHaisha>> ArrangeHaishaDaily(Dictionary<CommandMode, List<TkdHaisha>> source)
        {
            var result = new Dictionary<CommandMode, List<TkdHaisha>>();
            result[CommandMode.Create] = new List<TkdHaisha>();
            result[CommandMode.Update] = new List<TkdHaisha>();
            result[CommandMode.Delete] = source.GetValueOrDefault(CommandMode.Delete) ?? new List<TkdHaisha>();
            var createResult = SplitHaishaDaily(source[CommandMode.Create], false);
            result[CommandMode.Create].AddRange(createResult[CommandMode.Create]);
            if (source.ContainsKey(CommandMode.Update))
            {
                var updateResult = SplitHaishaDaily(source[CommandMode.Update], true);
                result[CommandMode.Create].AddRange(updateResult[CommandMode.Create]);
                result[CommandMode.Update].AddRange(updateResult[CommandMode.Update]);
            }
            return result;
        }

        private static Dictionary<CommandMode, List<TkdHaisha>> SplitHaishaDaily(List<TkdHaisha> source, bool isUpdate)
        {
            var result = new Dictionary<CommandMode, List<TkdHaisha>>();
            result[CommandMode.Create] = new List<TkdHaisha>();
            result[CommandMode.Update] = new List<TkdHaisha>();
            var updateList = new List<TkdHaisha>();
            var createList = new List<TkdHaisha>();
            var firstHourOfDay = "0000";
            var lastHourOfDay = "2359";
            foreach (var haisha in source)
            {
                var startDate = DateTime.ParseExact(haisha.HaiSymd, "yyyyMMdd", new CultureInfo("ja-JP"));
                var endDate = DateTime.ParseExact(haisha.TouYmd, "yyyyMMdd", new CultureInfo("ja-JP"));
                if (isUpdate)
                {
                    updateList.Add(haisha);
                }
                else
                {
                    createList.Add(haisha);
                }
                
                var numOfDays = (endDate - startDate).Days + 1;
                if (numOfDays > 1)
                {
                    var originHaisha = new TkdHaisha();
                    originHaisha.SimpleCloneProperties(haisha);
                    haisha.HenKai = (short)(haisha.HenKai + 1);
                    SetValueForSplitHaisha(haisha, originHaisha, 0, numOfDays, startDate, endDate, firstHourOfDay, lastHourOfDay);
                    for (int i = 1; i < numOfDays; i++)
                    {
                        var newHaisha = new TkdHaisha();
                        newHaisha.SimpleCloneProperties(originHaisha);
                        newHaisha.HenKai = 0;
                        SetValueForSplitHaisha(newHaisha, originHaisha, i, numOfDays, startDate, endDate, firstHourOfDay, lastHourOfDay);
                        createList.Add(newHaisha);
                    }
                }
            }
            result[CommandMode.Update].AddRange(updateList);
            result[CommandMode.Create].AddRange(createList);
            return result;
        }

        /// <summary>
        /// Filter list Vehical with KataKbn and SyaSyuCdSeq
        /// #6625 2021/03/09 Vehicle which NinkaKbn=7 will not able to be auto assigned
        /// </summary>
        /// <param name="listVehicle">Data source will be filter</param>
        /// <param name="KataKbn">KataKbn = 型; Example data: 1(Big); 2(Medium); 3(Small)</param>
        /// <param name="SyaSyuCdSeq">SyaSyuCdSeq = 車種; Example data 2:(ｽｰﾊﾟｰﾊｲﾃﾞｯｶｰ); 3(Ford); 4(Fortuner); 5(CRV)...</param>
        /// <returns>List <see cref="Vehical"/> after filter</returns>
        public static List<Vehical> FilterVehical(List<Vehical> vehicles, int kataKbn, int syaSyuCdSeq)
        {
            if (vehicles is null)
                throw new ArgumentNullException(nameof(vehicles));

            if (syaSyuCdSeq == 0) // Not filter SyaSyuCdSeq 車種
            {
                return (from e in vehicles
                        where (e.KataKbn == kataKbn && e.VehicleModel.NinkaKbn != 7)
                        select new Vehical { EigyoCdSeq = e.EigyoCdSeq, KataKbn = e.KataKbn, VehicleModel = e.VehicleModel }).ToList();
            }
            else
            {
                return (from e in vehicles
                        where (e.KataKbn == kataKbn && e.VehicleModel.SyaSyuCdSeq == syaSyuCdSeq && e.VehicleModel.NinkaKbn != 7)
                        select new Vehical { EigyoCdSeq = e.EigyoCdSeq, KataKbn = e.KataKbn, VehicleModel = e.VehicleModel }).ToList();
            }
        }

        /// <summary>
        /// Get master vehicles and filter follow config options in vehicleGridData
        /// </summary>
        /// <param name="vehicleGridData"></param>
        /// <param name="vehicals"></param>
        /// <returns>List master vehicles after filter</returns>
        public static List<Vehical> GetMasterVehicals(VehicleGridData vehicleGridData, List<Vehical> vehicals)
        {
            List<Vehical> masterVehicle = new List<Vehical>();

            if (vehicleGridData.PriorityAutoAssignBranch == null)
            {
                masterVehicle = BookingInputHelper.FilterVehical(vehicals, int.Parse(vehicleGridData.busTypeData.Katakbn), vehicleGridData.busTypeData.SyaSyuCdSeq);
            }
            else
            {
                var vehiclesByBranch = vehicals
                    .Where(_ => _.EigyoCdSeq == vehicleGridData.PriorityAutoAssignBranch.EigyoCdSeq)
                    .ToList();

                masterVehicle = BookingInputHelper.FilterVehical(vehiclesByBranch,
                    int.Parse(vehicleGridData.busTypeData.Katakbn), vehicleGridData.busTypeData.SyaSyuCdSeq);
            }

            return masterVehicle;
        }

        private static void SetValueForSplitHaisha(TkdHaisha haisha, TkdHaisha original, int index, int numOfDays, DateTime start, DateTime end, string firstHour, string lastHour)
        {
            int SyaRyoUncFloor = original.SyaRyoUnc / numOfDays;
            int SyaRyoUncOdd = original.SyaRyoUnc % numOfDays;
            int SyaRyoSyoFloor = original.SyaRyoSyo / numOfDays;
            int SyaRyoSyoOdd = original.SyaRyoSyo % numOfDays;
            int SyaRyoTesFloor = original.SyaRyoTes / numOfDays;
            int SyaRyoTesOdd = original.SyaRyoTes % numOfDays;
            int YoushaUncFloor = original.YoushaUnc / numOfDays;
            int YoushaUncOdd = original.YoushaUnc % numOfDays;
            int YoushaSyoFloor = original.YoushaSyo / numOfDays;
            int YoushaSyoOdd = original.YoushaSyo % numOfDays;
            int YoushaTesFloor = original.YoushaTes / numOfDays;
            int YoushaTesOdd = original.YoushaTes % numOfDays;
            if (index != 0)
            {
                haisha.SyuKoYmd = start.AddDays(index).ToString("yyyyMMdd");
                haisha.SyuKoTime = firstHour;
                haisha.HaiSymd = start.AddDays(index).ToString("yyyyMMdd");
                haisha.HaiStime = firstHour;
            }
            if (index != numOfDays -1)
            {
                haisha.TouYmd = start.AddDays(index).ToString("yyyyMMdd");
                haisha.TouChTime = lastHour;
                haisha.KikYmd = start.AddDays(index).ToString("yyyyMMdd");
                haisha.KikTime = lastHour;
            }
            haisha.BunkRen = (short)(index + 1);
            haisha.BunKsyuJyn = (short)(index + 1);
            haisha.SyaRyoUnc = SyaRyoUncOdd > index ? (SyaRyoUncFloor + 1) : SyaRyoUncFloor;
            haisha.SyaRyoSyo = SyaRyoSyoOdd > index ? (SyaRyoSyoFloor + 1) : SyaRyoSyoFloor;
            haisha.SyaRyoTes = SyaRyoTesOdd > index ? (SyaRyoTesFloor + 1) : SyaRyoTesFloor;
            haisha.YoushaUnc = YoushaUncOdd > index ? (YoushaUncFloor + 1) : YoushaUncFloor;
            haisha.YoushaSyo = YoushaSyoOdd > index ? (YoushaSyoFloor + 1) : YoushaSyoFloor;
            haisha.YoushaTes = YoushaTesOdd > index ? (YoushaTesFloor + 1) : YoushaTesFloor;
        }

        public static List<BookingDisableEditState> CheckEditable(TkdYyksho yyksho, TkdLockTable lockTable)
        {
            try
            {
                var result = new List<BookingDisableEditState>();
                if (yyksho.NyuKinKbn != 1 || yyksho.NcouKbn != 1)
                {
                    result.Add(BookingDisableEditState.PaidOrCoupon);
                }

                if (lockTable != null && DateTime.TryParseExact(lockTable.LockYmd, "yyyyMMdd", null, DateTimeStyles.None, out DateTime lockDate)
                            && DateTime.TryParseExact(yyksho.SeiTaiYmd, "yyyyMMdd", null, DateTimeStyles.None, out DateTime seiTaiDate))
                {
                    if (seiTaiDate < lockDate)
                    {
                        result.Add(BookingDisableEditState.Locked);
                    }
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
