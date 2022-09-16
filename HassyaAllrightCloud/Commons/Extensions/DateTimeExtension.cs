using DevExpress.Office.Utils;
using System;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Commons.Extensions
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// Check if this date is in the two specific date or not
        /// </summary>
        /// <param name="date">Date need to check</param>
        /// <param name="minDate">Date in start of range</param>
        /// <param name="maxDate">Date in end of range</param>
        /// <returns>true if in range</returns>
        public static bool IsInRange(this DateTime date, DateTime minDate, DateTime maxDate)
        {
            if (date >= minDate && date <= maxDate)
            {
                return true;
            }
            return false;
        }

        public static void AddRange(this List<DateTime> dateTimeList, DateTime startDate, DateTime endDate)
        {
            for (; startDate <= endDate; startDate = startDate.AddDays(1))
            {
                dateTimeList.Add(startDate);
            }
        }
    }
}
