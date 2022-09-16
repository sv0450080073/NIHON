using HassyaAllrightCloud.Commons.Extensions;
using System;
using System.Globalization;
using System.Linq;
using System.Numerics;

namespace HassyaAllrightCloud.Commons.Helpers
{
    public class CommonHelper
    {
        public static string ConvertStringToTimeString(string time)
        {
            if (time.Trim() != "" && time != null)
            {
                var hours = int.Parse(time.Substring(0, 2));
                var minutes = int.Parse(time.Substring(2, 2));
                var result = new TimeSpan(hours, minutes, 0);
                return result.ToString();
            }
            else
            {
                var result = new TimeSpan(0, 0, 0);
                return result.ToString();
            }
        }

        public static string FormatCodeNumber(string code, int zeroNumber, long limit)
        {
            if (string.IsNullOrEmpty(code))
            {
                return "0";
            }
            else
            {
                if (int.Parse(code) > limit)
                {
                    return code.ToString();
                }
                return code.PadLeft(zeroNumber, '0');
            }
        }

        //convert string yyyymmdd to yyyy-mm-dd
        public static string ConvertStringToDateString(string dateString)
        {
            if (dateString == null)
            {
                return dateString;
            }

            var year = dateString.Substring(0, 4);
            var month = dateString.Substring(4, 2);
            var date = dateString.Substring(6, 2);
            return $"{year}-{month}-{date}";
        }

        //convert string yyyymmdd to yy/mm/dd
        public static string ConvertStringToDateStringV2(string dateString)
        {
            if (dateString == null)
            {
                return dateString;
            }

            var year = dateString.Substring(0, 4).Substring(2, 2);
            var month = dateString.Substring(4, 2);
            var date = dateString.Substring(6, 2);
            return $"{year}/{month}/{date}";
        }

        /// <summary>
        /// Get Kotei/Tehai date by nittei and tomKbn flag
        /// </summary>
        /// <param name="haishaYmd"></param>
        /// <param name="touYmd"></param>
        /// <param name="nittei"></param>
        /// <param name="tomKbn"></param>
        /// <returns>Date of kotei/tehai</returns>
        public static DateTime GetKoteiTehaiDate(string haishaYmd, string touYmd, int nittei, int tomKbn)
        {
            DateTime hy = DateTime.ParseExact(haishaYmd, "yyyyMMdd", CultureInfo.CurrentCulture);
            DateTime ty = DateTime.ParseExact(touYmd, "yyyyMMdd", CultureInfo.CurrentCulture);

            switch (tomKbn)
            {
                case 1:  
                    hy = hy.AddDays(nittei - 1);
                    break;
                case 2:  
                    hy = hy.AddDays(-1);
                    break;
                case 3:
                    int days = (ty - hy).Days;
                    hy = hy.AddDays(days + 1);
                    break;
                default:
                    return DateTime.Now;
            }

            return hy;
        }
        public static TModel SimpleCloneModel<TModel>(TModel dataSource) where TModel : class, new()
        {
            TModel dataDest = new TModel();

            dataDest.SimpleCloneProperties(dataSource);

            return dataDest;
        }

        /// <summary>
        /// Check in range for multiple id. When input 0 to left/right value => always true if compare with that value.
        /// <para>Example: IsBetween(new {A = 0, B = 0}, new {A = 0, B = 0}, new {A = 0, B = 0}) </para>
        /// </summary>
        /// <param name="left">Left value</param>
        /// <param name="right">Right value</param>
        /// <param name="compareValue">Value to check in range</param>
        /// <returns><c>true</c> if value is between range, otherwise <c>false</c></returns>
        public static bool IsBetween(object left, object right, object compareValue)
        {
            try
            {
                BigInteger pLeft = BigInteger.Parse(ConverToString(left));
                BigInteger pRight = BigInteger.Parse(ConverToString(right));
                BigInteger pCompare = BigInteger.Parse(ConverToString(compareValue));

                bool result = true;

                if (!pLeft.IsZero)
                    result = pCompare >= pLeft;
                if (!pRight.IsZero)
                    result = pCompare <= pRight;

                return result;
            }
            catch (Exception ex)
            {
                ex = ex ?? ex;

                return false;
            }

            string ConverToString(object input)
            {
                var fields = input.GetType().GetFields();
                var props = input.GetType().GetProperties();

                if (fields.Any())
                {
                    dynamic temp = input;
                    return $"{Convert.ToInt64(temp.Item1):D10}{Convert.ToInt64(temp.Item2):D10}";
                }

                string result = string.Empty;
                foreach (var item in props)
                {
                    var val = input.GetType().GetProperty(item.Name).GetValue(input, null);

                    result += $"{Convert.ToInt64(val):D10}";
                }

                return result;
            }
        }

        public static DateTime? ConvertToDateTime (dynamic date)
        {
            if (date == null)
                return null;
            if (date is DateTime)
                return date;
            if(date is string)
            {
                string[] specialCharacters = new string[] { ".", "/" };

                var dateString = ((string)date).Normalize(System.Text.NormalizationForm.FormKC);
                var specialCharacter = specialCharacters.FirstOrDefault(x => dateString.Contains(x));
                if (specialCharacter != null)
                    dateString = dateString.Replace(specialCharacter, "");
                if (DateTime.TryParseExact(dateString, "yyyyMMdd",
                                   CultureInfo.InvariantCulture,
                                   DateTimeStyles.None,
                                   out DateTime dateValue))
                    return DateTime.ParseExact(dateString, "yyyyMMdd", CultureInfo.InvariantCulture);
            }
            return null;
        }
    }
}
