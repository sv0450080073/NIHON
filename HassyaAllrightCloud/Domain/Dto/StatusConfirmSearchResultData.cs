using HassyaAllrightCloud.Commons.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using static HassyaAllrightCloud.Commons.Helpers.BookingInputHelper;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class BusViewData
    {
        public string BusName { get; set; }
        public string BusType { get; set; }
        public int Daisu { get; set; }
    }

    public class StatusConfirmSearchResultData
    {
        public int No { get; set; }
        public bool IsChecked { get; set; } = false;
        public int CountKak { get; set; }
        public string Ukeno { get; set; }
        public int UnkRen { get; set; }
        public string TokuiSaki { get; set; }
        public string TokuiStaff { get; set; }
        public string ShiireSaki { get; set; }
        public int ConfirmedTime { get; set; }
        private DateTime _confirmedYmd = DateTime.Now;
        public string ConfirmedYmd
        {
            get
            {
                if (DateTime.MinValue.CompareTo(_confirmedYmd) == 0)
                    return string.Empty;
                return _confirmedYmd.ToString("yyyy/MM/dd");
            }
            set => DateTime.TryParseExact(value, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _confirmedYmd);
        }
        private DateTime _fixedYmd = DateTime.Now;
        public string FixedYmd
        {
            get 
            {
                if (DateTime.MinValue.CompareTo(_fixedYmd) == 0)
                    return string.Empty;
                return _fixedYmd.ToString("yyyy/MM/dd");
            }
            set => DateTime.TryParseExact(value, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _fixedYmd);
        }
        public int ConfirmedPerson { get; set; }
        public string ConfirmedBy { get; set; }
        public string NoteContent { get; set; }
        public bool Saikou { get; set; }
        public bool SumDai { get; set; }
        public bool SumAmount { get; set; }
        public bool ScheduledDate { get; set; }
        public string DanTaiName { get; set; }
        public string KanjiName { get; set; }
        public string DestinationName { get; set; }
        private DateTime _haishaYmd = DateTime.Now;
        public string HaishaYmd 
        {
            get => _haishaYmd.ToString("yyyy/MM/dd");
            set => _haishaYmd = DateTime.ParseExact(value, "yyyyMMdd", CultureInfo.InvariantCulture);
        }
        private MyTime _haishaTime = new MyTime();
        public string HaishaTime 
        { 
            get => _haishaTime.Str; 
            set
            {
                if (ValidationHelper.ValidationInputTimeText(value))
                {

                    _haishaTime = new MyTime
                    {
                        Str = CommonUtil.ConvertMyTimeStrToDefaultFormat(value)
                    };
                }
                else
                {
                    _haishaTime = new MyTime();
                }
            }
        }
        public string HaiSNm { get; set; }
        private DateTime _touYmd = DateTime.Now;
        public string TouYmd
        {
            get => _touYmd.ToString("yyyy/MM/dd");
            set => _touYmd = DateTime.ParseExact(value, "yyyyMMdd", CultureInfo.InvariantCulture);
        }
        private MyTime _touTime = new MyTime();
        public string TouTime
        {
            get => _touTime.Str;
            set
            {
                if (ValidationHelper.ValidationInputTimeText(value))
                {

                    _touTime = new MyTime
                    {
                        Str = CommonUtil.ConvertMyTimeStrToDefaultFormat(value)
                    };
                }
                else
                {
                    _touTime = new MyTime();
                }
            }
        }
        public string TouNm { get; set; } 
        public string PassengerQuantity { get; set; }
        public string PlusPassenger { get; set; }
        public string HaishaInfoStrRow1 => string.Format("{0} {1} {2}", HaishaYmd, HaishaTime, HaiSNm);
        public string HaishaInfoStrRow2 => string.Format("{0} {1} {2}", TouYmd, TouTime, TouNm);
        public string HaishaInfoStrRow3 => string.Format("{0} + {1}", PassengerQuantity, PlusPassenger);
        //public string 
        #region Temp proprety convert to list BusViewData. Ignore this property
        /// <summary>
        /// Temp proprety convert to list BusViewData. Ignore this property
        /// </summary>
        public string BusName { get; set; }
        /// <summary>
        /// Temp proprety convert to list BusViewData. Ignore this property
        /// </summary>
        public string BusType { get; set; }
        /// <summary>
        /// Temp proprety convert to list BusViewData. Ignore this property
        /// </summary>
        public int Daisu { get; set; }
        #endregion
        public List<BusViewData> BusViewDatas { get; set; } = new List<BusViewData>();
        public int _busFee = 0;
        public string BusFee { 
            get => _busFee.ToString("N0");
            set => int.TryParse(value, out _busFee);  
        }
        public int _busTaxAmount = 0;
        public string BusTaxAmount {
            get => _busTaxAmount.ToString("N0");
            set => int.TryParse(value, out _busTaxAmount); 
        }
        private float _busTaxRate = 0f;
        public string BusTaxRate { 
            get => String.Format("{0:P1}", _busTaxRate / 100); 
            set => float.TryParse(value, out _busTaxRate); 
        }
        public int _busCharge = 0;
        public string BusCharge { 
            get => _busCharge.ToString("N0"); 
            set => int.TryParse(value, out _busCharge); 
        }
        private float _busChargeRate = 0f;
        public string BusChargeRate { 
            get => String.Format("{0:P1}", _busChargeRate / 100); 
            set => float.TryParse(value, out _busChargeRate); 
        }
        public int _guideFee = 0;
        public string GuideFee { 
            get => _guideFee.ToString("N0"); 
            set => int.TryParse(value, out _guideFee); 
        }
        public int _guideTax = 0;
        public string GuideTax {
            get => _guideTax.ToString("N0");
            set => int.TryParse(value, out _guideTax);
        }
        public int _guideCharge = 0;
        public string GuideCharge {
            get => _guideCharge.ToString("N0");
            set => int.TryParse(value, out _guideCharge);
        }
        public string ReceivedBranch { get; set; }
        public string ReceivedBy { get; set; }
        public string InputBy { get; set; } 
        public string BookingType { get; set; } 
        private DateTime _receivedYmd = DateTime.Now;
        public string ReceivedYmd
        {
            get => _receivedYmd.ToString("yyyyMMdd");
            set => _receivedYmd = DateTime.ParseExact(value, "yyyyMMdd", CultureInfo.InvariantCulture);
        }
        private int _bookingNo = 0;
        public string BookingNo 
        { 
            get => _bookingNo.ToString("D10"); 
            set => int.TryParse(value, out _bookingNo); 
        }
        public bool Guide { get; set; }
        public bool Kotei { get; set; }
        public bool TsuMi { get; set; }
        public bool Tehai { get; set; }
        public bool Biko { get; set; }
        public bool Futai { get; set; }

        public override bool Equals(object obj)
        {
            try
            {
                var o = ((StatusConfirmSearchResultData)obj);
                return this.BookingNo == o.BookingNo && this.UnkRen == o.UnkRen;
            }
            catch
            {
                return false;
            }
        }
        public override int GetHashCode()
        {
            //Get the ID hash code value
            int IDHashCode = this.BookingNo.GetHashCode();
            int UnkRenHashCode = this.UnkRen.GetHashCode();
            //Get the string HashCode Value
            return IDHashCode ^ UnkRenHashCode;
        }
    }
}
