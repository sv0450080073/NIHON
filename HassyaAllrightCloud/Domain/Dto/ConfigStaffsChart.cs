using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class ConfigStaffsChart:ICloneable
    {
        public string Date { get; set; }
        public int Type { get; set; }
        public int Grouping { get; set; }
        public int DisplayRange { get; set; }
        public int TimeRange { get; set; }
        public int OrderbyList { get; set; }
        public int OrderbyName { get; set; }
        public int DisplayedCharacters { get; set; }
        public int DisplayedLineHeight { get; set; }
        public int BookingCategory { get; set; }
        public int Duties { get; set; }
        public int Mode { get; set; }
        public int Number_of_days { get; set; }
        public string DepartureDateStart { get; set; }
        public string DepartureTimeStart { get; set; } = "0000";
        public string DepartureDateEnd { get; set; }
        public string DepartureTimeEnd { get; set; } = "2359";
        public string ArrivalDateStart { get; set; }
        public string ArrivalTimeStart { get; set; } = "0000";
        public string ArrivalDateEnd { get; set; }
        public string ArrivalTimeEnd { get; set; } = "2359";
        public string DeliveryDateStart { get; set; }
        public string DeliveryTimeStart { get; set; } = "0000";
        public string DeliveryDateEnd { get; set; }
        public string DeliveryTimeEnd { get; set; } = "2359";
        public string ReturnDateStart { get; set; }
        public string ReturnTimeStart { get; set; } = "0000";
        public string ReturnDateEnd { get; set; }
        public string ReturnTimeEnd { get; set; } = "2359";
        public DateTime DepartureDateTimeStart { get
            {
                    return ParseStringToDateTime(DepartureDateStart, DepartureTimeStart);
            } 
        }
        public DateTime DepartureDateTimeEnd
        {
            get
            {
                return ParseStringToDateTime(DepartureDateEnd, DepartureTimeEnd);
            }
        }
        public DateTime ArrivalDateTimeStart
        {
            get
            {
                return ParseStringToDateTime(ArrivalDateStart, ArrivalTimeStart);
            }
        }
        public DateTime ArrivalDateTimeEnd
        {
            get
            {
                return ParseStringToDateTime(ArrivalDateEnd, ArrivalTimeEnd);
            }
        }
        public DateTime DeliveryDateTimeStart
        {
            get
            {
                return ParseStringToDateTime(DeliveryDateStart, DeliveryTimeStart);
            }
        }
        public DateTime DeliveryDateTimeEnd
        {
            get
            {
                return ParseStringToDateTime(DeliveryDateEnd, DeliveryTimeEnd);
            }
        }
        public DateTime ReturnDateTimeStart
        {
            get
            {
                return ParseStringToDateTime(ReturnDateStart, ReturnTimeStart);
            }
        }
        public DateTime ReturnDateTimeEnd
        {
            get
            {
                return ParseStringToDateTime(ReturnDateEnd, ReturnTimeEnd);
            }
        }
        public string ReservationClassification { get; set; }="0";
        public string AffiliationOfficeFrom { get; set; }="0";
        public string AffiliationOfficeTo { get; set; }="0";
        public string Driver{ get; set; }="1";
        public string Guide{ get; set; }="1";
        public string ListCom { get; set; }
        public string ListBrch { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        private DateTime ParseStringToDateTime(string dateTime, string Time)
        {
            DateTime result;
            DateTime.TryParseExact(dateTime + Time, "yyyyMMddHHmm", new CultureInfo("ja-JP"), DateTimeStyles.None, out result);
            return result;
        }
    }
}

