using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HassyaAllrightCloud.Domain.Dto;

namespace HassyaAllrightCloud.Commons.Helpers
{
    public class BusScheduleHelper
    {
        public TimeSpan ConvertTime(string time)
        {
            if(time.Trim()!=""&&time!=null)
            {
                var hours = Int32.Parse(time.Substring(0, 2));
                var minutes = Int32.Parse(time.Substring(2, 2));
                var result = new TimeSpan(hours, minutes, 0);
                return result;
            }
            else
            {
                var result = new TimeSpan(0, 0, 0);
                return result;
            }   
           
        }
        public bool CheckTimeinDay(string time)
        {
            int hours = Int32.Parse(time.Substring(0, 2));
            int minutes = Int32.Parse(time.Substring(2, 2));
            if(hours>=24 ||minutes>59)
            {
                return false;
            }
            else
            return true;
        }
        public double calwidth(double Widths, string startdate, string starttime, string enddate, string endtime, int mode)
        {
            DateTime startdatene;
            DateTime.TryParseExact(startdate + starttime,
                           "yyyyMMddHHmm",
                           CultureInfo.CurrentCulture,
                           DateTimeStyles.None,
                           out startdatene);

            DateTime enddatene;
            DateTime.TryParseExact(enddate + endtime,
                           "yyyyMMddHHmm",
                           CultureInfo.CurrentCulture,
                           DateTimeStyles.None,
                           out enddatene);
            double totalsecond = (enddatene - startdatene).TotalSeconds;
            double pxpersecond = Widths / mode / 86400;
            return totalsecond * pxpersecond;
        }

        public double calleft(double Widths, string startdate, string starttime, int mode, DateTime defaultdate)
        {
            DateTime startdatene;
            DateTime.TryParseExact(startdate + starttime,
                           "yyyyMMddHHmm",
                           CultureInfo.CurrentCulture,
                           DateTimeStyles.None,
                           out startdatene);
            double totalsecond = (startdatene - defaultdate).TotalSeconds;
            double pxpersecond = Widths / mode / 86400;
            return totalsecond * pxpersecond;
        }
        public DateTime convertwidthtodate(double Widths, double width, int modes, DateTime defaultdate)
        {
            double pxpersecond = Widths / modes / 86400;
            double seconds = width / pxpersecond;
            return defaultdate.AddSeconds(seconds);
        }

        public bool compairdate(List<DatetimeData> listdate ,DatetimeData date)
        {
            bool checkdate= false;
            DatetimeData datefirst = listdate.First();
            DatetimeData datelast = listdate.Last();
            if (listdate.Count==1)
            {
                if(datefirst.DateStart>=date.DateEnd||datefirst.DateEnd<=date.DateStart)
                {
                    checkdate = true;
                }
            }
            else {
                for(int i=0;i<=listdate.Count();i++)
                {
                    for(int j=i+1;j<=listdate.Count()-1;j++)
                    {
                        DatetimeData date1= listdate.ElementAt(i);
                        DatetimeData date2 = listdate.ElementAt(j);
                        if((date1.DateStart>=date.DateEnd && datefirst.DateStart>=date.DateEnd)||(date2.DateEnd<=date.DateStart && datelast.DateEnd<=date.DateStart)||(date1.DateEnd<=date.DateStart&&date.DateEnd<=date2.DateStart))
                        {
                             checkdate = true;
                        }
                    }
                }
            }
            return checkdate;
        }

    }
}
