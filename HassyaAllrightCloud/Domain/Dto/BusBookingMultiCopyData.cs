using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Entities;
using System;
using System.Collections.Generic;
using static HassyaAllrightCloud.Commons.Helpers.BookingInputHelper;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class BusBookingMultiCopyData
    {
        public BusBookingMultiCopyData()
        {
            CopyType = BookingMultiCopyType.Reservation;
            BookingDataChangedList = new List<MultiCopyDataGrid>();
            DispatchTime = new MyTime(0, 0);
            ArrivalTime = new MyTime(0, 0);
            IsApplyAll = false;
            BookingDataToCopy = new BookingFormData();
        }

        public MyTime DispatchTime { get; set; }
        public MyTime ArrivalTime { get; set; }
        public bool IsApplyAll { get; set; }

        public BookingMultiCopyType CopyType { get; set; }

        public BookingFormData BookingDataToCopy { get; set; }

        public List<MultiCopyDataGrid> BookingDataChangedList { get; set; }

        public TkdYyksho YykshoCopyData { get; set; }
        public TkdUnkobi UnkobiCopyData { get; set; }
        public List<TkdMishum> MishumCopyDataList { get; set; }
        public List<TkdYykSyu> YykSyuCopyDataList { get; set; }
        public List<TkdHaisha> HaishaCopyDataList { get; set; }
        public List<TkdBookingMaxMinFareFeeCalc> BookingFareFeeCopyDataList { get; set; }
        public List<TkdBookingMaxMinFareFeeCalcMeisai> BookingFareFeeMeisaiCopyDataList { get; set; }
        public List<TkdKotei> KoteiList { get; set; }
        public List<TkdKoteik> KoteiKList { get; set; }
        public List<TkdTehai> TehaiList { get; set; }
        public List<TkdFutTum> FutaiList { get; set; }
        public List<TkdMfutTu> MFutaiList { get; set; }
        public List<TkdFutTum> TsumiList { get; set; }
        public List<TkdMfutTu> MTsumiList { get; set; }

        public bool IsCopyBusRoute { get; set; }
        public bool IsCopyArrangement { get; set; }
        public bool IsCopyIncludedGoods { get; set; }
        public bool IsCopyIncludedServices { get; set; }

        public double NumberOfDateChanged { get; set; }

        public bool IsDoneLoadCopyData
        {
            get
            {
                return YykshoCopyData != null
                    && UnkobiCopyData != null
                    && MishumCopyDataList != null
                    && YykSyuCopyDataList != null
                    && HaishaCopyDataList != null
                    && BookingFareFeeCopyDataList != null
                    && BookingFareFeeMeisaiCopyDataList != null;
            }
        }
    }

    public class MultiCopyDataGrid
    {
        public MultiCopyDataGrid(MyTime startTime, MyTime endTime)
        {
            _startTime = new MyTime(startTime.myHour, startTime.myMinute);
            _endTime = new MyTime(endTime.myHour, endTime.myMinute);
        }

        public MultiCopyDataGrid() : this(new MyTime(0, 0), new MyTime(0, 0))
        {

        }

        private MyTime _startTime;
        private MyTime _endTime;

        public int Index { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public MyTime StartTime
        {
            get
            {
                return _startTime;
            }
            set
            {
                _startTime.myHour = value.myHour;
                _startTime.myMinute = value.myMinute;
            }
        }
        public MyTime EndTime
        {
            get
            {
                return _endTime;
            }
            set
            {
                _endTime.myHour = value.myHour;
                _endTime.myMinute = value.myMinute;
            }
        }
        public DateTime InvoiceDate { get; set; }
        public DateTime InvoiceMonth { get; set; }
    }
}
