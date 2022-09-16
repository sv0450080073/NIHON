using HassyaAllrightCloud.Commons.Helpers;
using System;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class MinMaxGridData // GRID 
    {
        public int rowID { get; set; }
        public DateTime DateofScheduler { get; set; } //日付	
        public int KmRunning { get; set; }
        public BookingInputHelper.MyDate BusInspectionStartDate { get; set; }
        public BookingInputHelper.MyDate BusInspectionEndDate { get; set; }
        public void update()
        {
            // Update for specialrunning time in row
            if (BusInspectionEndDate.inpTime.myHour != -1 &&
                BusInspectionEndDate.inpTime.myMinute != -1 &&
                BusInspectionStartDate.inpTime.myHour != -1 &&
                    BusInspectionStartDate.inpTime.myMinute != -1)
            {
                var tmpTimeRunning = BusInspectionEndDate - BusInspectionStartDate;
                if (tmpTimeRunning.myHour < 5)
                {
                    TimeRunning.myHour = 5;
                    TimeRunning.myMinute = 00;
                }
                else
                {
                    TimeRunning.myHour = tmpTimeRunning.myHour;
                    TimeRunning.myMinute = tmpTimeRunning.myMinute;
                }
            }

            // update for Erlywork or Mid9work
            BookingInputHelper.Scheduler sche = new BookingInputHelper.Scheduler(BusInspectionStartDate, BusInspectionEndDate);
            TimeSpan r = sche.getMid9EarlyDuration();
            SpecialTimeRunning.myHour = r.Hours + r.Days * 24;
            SpecialTimeRunning.myMinute = r.Minutes;
        }

        public void Update(bool changeIsPreviousDay)
        {
            // Update for specialrunning time in row
            BookingInputHelper.MyDate changeBusInspectionStartDate = new BookingInputHelper.MyDate(BusInspectionStartDate, 0, changeIsPreviousDay);
            if (BusInspectionEndDate.inpTime.myHour != -1 &&
                BusInspectionEndDate.inpTime.myMinute != -1 &&
                changeBusInspectionStartDate.inpTime.myHour != -1 &&
                    changeBusInspectionStartDate.inpTime.myMinute != -1)
            {
                var tmpTimeRunning = BusInspectionEndDate - changeBusInspectionStartDate;
                if (tmpTimeRunning.myHour < 5)
                {
                    TimeRunning.myHour = 5;
                    TimeRunning.myMinute = 00;
                }
                else
                {
                    TimeRunning.myHour = tmpTimeRunning.myHour;
                    TimeRunning.myMinute = tmpTimeRunning.myMinute;
                }
            }
            // update for Erlywork or Mid9work
            BookingInputHelper.Scheduler sche = new BookingInputHelper.Scheduler(changeBusInspectionStartDate, BusInspectionEndDate);
            TimeSpan r = sche.getMid9EarlyDuration();
            SpecialTimeRunning.myHour = r.Hours + r.Days * 24;
            SpecialTimeRunning.myMinute = r.Minutes;
        }

        public BookingInputHelper.MyTime TimeRunning { get; set; } = new BookingInputHelper.MyTime(0, 0);

        public int ExactKmRunning { get; set; } = 0;

        public BookingInputHelper.MyTime ExactTimeRunning { get; set; } = new BookingInputHelper.MyTime(0, 0);

        public BookingInputHelper.MyTime SpecialTimeRunning { get; set; } = new BookingInputHelper.MyTime(0, 0);
        public bool isChangeDriver { get; set; } = false; // false
        public int KmRunningwithChgDriver { get; set; } = 0;
        public BookingInputHelper.MyTime TimeRunningwithChgDriver { get; set; } = new BookingInputHelper.MyTime(0, 0);
        public BookingInputHelper.MyTime SpecialTimeRunningwithChgDriver { get; set; } = new BookingInputHelper.MyTime(0, 0);

        public MinMaxGridData()
        {

        }
        public MinMaxGridData(int _rowID, DateTime _dateofScheduler)
        {
            this.rowID = _rowID;
            DateofScheduler = _dateofScheduler;
            BusInspectionStartDate = new BookingInputHelper.MyDate(_dateofScheduler, new BookingInputHelper.MyTime());
            BusInspectionEndDate = new BookingInputHelper.MyDate(_dateofScheduler, new BookingInputHelper.MyTime());
        }
        public MinMaxGridData(MinMaxGridData old)
        {
            this.rowID = old.rowID;
            this.DateofScheduler = old.DateofScheduler;
            this.KmRunning = old.KmRunning;
            this.BusInspectionStartDate = new BookingInputHelper.MyDate(old.BusInspectionStartDate);
            this.BusInspectionEndDate = new BookingInputHelper.MyDate(old.BusInspectionEndDate);
            this.TimeRunning = new BookingInputHelper.MyTime(old.TimeRunning);
            this.ExactKmRunning = old.ExactKmRunning;
            this.ExactTimeRunning = new BookingInputHelper.MyTime(old.ExactTimeRunning);
            this.SpecialTimeRunning = new BookingInputHelper.MyTime(old.SpecialTimeRunning);
            this.isChangeDriver = old.isChangeDriver;
            this.KmRunningwithChgDriver = old.KmRunningwithChgDriver;
            this.TimeRunningwithChgDriver = new BookingInputHelper.MyTime(old.TimeRunningwithChgDriver);
            this.SpecialTimeRunningwithChgDriver = new BookingInputHelper.MyTime(old.SpecialTimeRunningwithChgDriver);
        }

        public string KmRunningStr => KmRunning.ToString();
        public string ExactKmRunningStr => ExactKmRunning.ToString();
        public string KmRunningwithChgDriverStr => KmRunningwithChgDriver.ToString();
    }
}
