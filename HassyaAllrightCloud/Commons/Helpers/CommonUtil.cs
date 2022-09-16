using HassyaAllrightCloud.Commons.Constants;
using DevExpress.Charts.Native;
using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace HassyaAllrightCloud.Commons.Helpers
{
    public class CommonUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetNumbers(string input)
        {
            return new string(input.Where(c => char.IsDigit(c)).ToArray());
        }

        /// <summary>
        ///         /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string CurencyFormat(object obj)
        {
            if (typeof(string) == obj.GetType())
            {
                obj = long.Parse(obj.ToString());
            }
            return string.Format("{0:#,0}", obj);
        }

        public static string TaxRateFormat(string taxValueString)
        {
            if (float.TryParse(taxValueString, out float taxValue))
            {
                return taxValue.ToString("F1");
            }
            return "-1";
        }

        /// <summary>
        /// Remask input string
        /// </summary>
        /// <param name="str"></param>
        /// <returns>String  is: X => 0X:00  |  XX => XX:00  |  XXX => 0X:XX  |  XXXX => XX:XX</returns>
        public static string MyTimeFormat(string str)
        {
            if (str.IndexOf(":") == -1)
            {
                switch (str.Length)
                {
                    case 1:
                        return String.Format("0{0}:00", str);
                    case 2:
                        return String.Format("{0}:00", str);
                    case 3:
                        return String.Format("0{0}:{1}{2}", str[0], str[1], str[2]);
                    case 4:
                        return String.Format("{0}{1}:{2}{3}", str[0], str[1], str[2], str[3]);
                }
            }
            return str;
        }

        /// <summary>
        /// Convert <see cref="BookingInputHelper.MyTime"/> as string to default format.
        /// </summary>
        /// <param name="time">Time as string will converted</param>
        /// <returns>
        /// String  is: X => 0X:00  |  XX => XX:00  |  XXX => 0X:XX  |  XXXX => XX:XX
        /// <para>String  is: X:X => 0X:0X  |  XX:X => XX:X0  |  X:XX => 0X:XX  |  XX:XX => XX:XX</para>
        /// </returns>
        public static string ConvertMyTimeStrToDefaultFormat(string time)
        {
            if (string.IsNullOrEmpty(time))
                return time;
            string maskChar = ":";
            time = time.ToString().Normalize(System.Text.NormalizationForm.FormKC);
            string timeFormated = string.Empty;

            if (time.Contains(":"))
            {
                string tempTime = time.Replace(":", String.Empty);

                bool isInFormat = int.TryParse(tempTime, out int timeValue);
                if (isInFormat)
                {
                    string[] spl = time.Split(":");

                    if (!string.IsNullOrEmpty(spl[0]) && !string.IsNullOrEmpty(spl[1]))
                    {
                        timeFormated = int.Parse(spl[0]).ToString("00") + maskChar + int.Parse(spl[1]).ToString("00");
                    }
                }
            }
            else
            {
                bool isInFormat = int.TryParse(time, out int timeValue);
                if (isInFormat)
                {

                    switch (time.Length)
                    {
                        case 1:
                            timeFormated = $"0{time}{maskChar}00";
                            break;
                        case 2:
                            timeFormated = $"{time}{maskChar}00";
                            break;
                        case 3:
                            timeFormated = $"0{time[0]}{maskChar}{time[1]}{time[2]}";
                            break;
                        case 4:
                            timeFormated = $"{time[0]}{time[1]}{maskChar}{time[2]}{time[3]}";
                            break;
                    }
                }
            }
            return timeFormated;
        }

        /// <summary>
        /// Convert time string to new format specified.
        /// </summary>
        /// <param name="time">Time string will be apply</param>
        /// <param name="format">Format of time string. Example: <see cref="Formats.HHmm"/></param>
        /// <returns>
        /// Time as string with new format
        /// </returns>
        public static string MyTimeFormat(string time, string format)
        {
            string validTime = ConvertMyTimeStrToDefaultFormat(time);

            if (!string.IsNullOrEmpty(validTime) && !string.IsNullOrEmpty(format))
            {
                if (format == Formats.HHmm)
                {
                    validTime = validTime.Replace(":", String.Empty);
                }
            }

            return validTime;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="tmp"></param>
        /// <returns></returns>
        public static Boolean NumberTryParse(string str, out int tmp)
        {
            str = str.Replace(",", "");
            if (int.TryParse(str.ToString(), out tmp) ||
                int.TryParse(str.ToString().Normalize(System.Text.NormalizationForm.FormKC), out tmp)) // Convert from Full-width to Haft-width
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="tmp"></param>
        /// <returns></returns>
        public static Boolean NumberTryParse(string str, out long tmp)
        {
            str = str.Replace(",", "");
            if (long.TryParse(str.ToString(), out tmp) ||
                long.TryParse(str.ToString().Normalize(System.Text.NormalizationForm.FormKC), out tmp)) // Convert from Full-width to Haft-width
            {
                return true;
            }
            return false;
        }

        public static string FormatCodeNumber(string code, int? length = null)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return "";
            }
            else
            {
                return length == null ? code.PadLeft(10, '0') : code.PadLeft((int)length, '0');
            }
        }

        public static decimal? CheckNullAndPercentage(Nullable<decimal> value1 = 0, Nullable<decimal> value2 = 0)
        {
            try
            {
                return ((value1 / value2 * 100) == 0 ? null : (value1 / value2 * 100));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static T CheckNullAndSum<T>(T number1, T number2, T number3)
        {
            dynamic a = number1;
            dynamic b = number2;
            dynamic c = number3;
            return ((a ?? 0) + (b ?? 0) + (c ?? 0) == 0 ? null : (a ?? 0) + (b ?? 0) + (c ?? 0));
        }

        public static decimal? CheckNullAndCaculate(Nullable<decimal> value1 = 0, Nullable<decimal> value2 = 0)
        {
            try
            {
                return ((value1 / value2) == 0 ? null : (value1 / value2));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string CurrentYYYYMMDD => $"{DateTime.Now.Year:D4}{DateTime.Now.Month:D2}{DateTime.Now.Day:D2}";
        public static string CurrentHHMMSS => $"{DateTime.Now.Hour:D2}{DateTime.Now.Minute:D2}{DateTime.Now.Second:D2}";
    }
}
