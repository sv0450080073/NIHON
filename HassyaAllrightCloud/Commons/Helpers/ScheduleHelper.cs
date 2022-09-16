using System;
using System.Collections.Generic;
using System.Globalization;

namespace HassyaAllrightCloud.Commons.Helpers
{
    public class ScheduleSelectorModel
    {
        public ScheduleSelectorModel()
        {
            TomKbn = 1;
        }

        public string DateString
        {
            set
            {
                if (DateTime.TryParseExact(value, "yyyyMMdd", null, DateTimeStyles.None, out DateTime tempDt))
                {
                    Date = tempDt;
                }
            }
        }
        public DateTime Date { get; set; }
        public short Nittei { get; set; }
        public bool IsPreviousDay { get => TomKbn == 2; }
        public bool IsAfterDay { get => TomKbn == 3; }
        public byte TomKbn { get; set; }

        public string Textbun => IsPreviousDay ? "前泊" : IsAfterDay ? "後泊" : "通常";

        public string GetPrefixString()
        {
            return $"{Textbun}{Nittei}日目";
        }

        public string GetDateString()
        {
            return Date.ToString("yyyy/MM/dd (ddd)");
        }

        public string Text
        {
            get
            {
                if (IsPreviousDay) return Date.ToString($"前泊{1}日目 yyyy/MM/dd (ddd)");
                else if (IsAfterDay) return Date.ToString($"後泊{1}日目 yyyy/MM/dd (ddd)");
                else return Date.ToString($"通常{Nittei}日目 yyyy/MM/dd (ddd)");
            }
        }
    }

    public class ScheduleHelper
    {
        public static List<ScheduleSelectorModel> GetScheduleSelectorList(DateTime startDate, DateTime endDate, bool isPreviousDate, bool isAfterDate)
        {
            var result = new List<ScheduleSelectorModel>();
            if (isPreviousDate)
            {
                result.Add(new ScheduleSelectorModel()
                {
                    Date = startDate.AddDays(-1),
                    TomKbn = 2,
                    Nittei = 1
                });
            }
            short nittei = 1;
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                result.Add(new ScheduleSelectorModel()
                {
                    Date = date,
                    TomKbn = 1,
                    Nittei = nittei++,
                });
            }
            if (isAfterDate)
            {
                result.Add(new ScheduleSelectorModel()
                {
                    Date = endDate.AddDays(1),
                    TomKbn = 3,
                    Nittei = 1
                });
            }
            return result;
        }
    }
}
