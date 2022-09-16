using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Commons.Extensions
{
    public static class StringExtensions
    {
        public static bool IsIntLargerThanZero(this string value)
        {
            return Int32.TryParse(value, out int val) && val > 0;
        }

        public static string TruncateWithMaxLength(this string value, int maxLength)
        {
            Encoding asc = Encoding.UTF8;
            var byteCount = 0;
            var result = new StringBuilder();
            foreach (char c in value.ToArray())
            {
                var charByte = asc.GetBytes(c.ToString()).Length == 1 ? 1 : 2;
                byteCount += charByte;
                if (byteCount <= maxLength)
                    result.Append(c);
                else break;
            }
            return result.ToString();
        }

        public static bool IsMaxLengthValid(this string val, int maxLength)
        {
            Encoding asc = Encoding.UTF8;
            var byteCount = 0;
            foreach (char c in val.ToArray())
            {
                var charByte = asc.GetBytes(c.ToString()).Length == 3 ? 2 : 1;
                byteCount += charByte;
                if (byteCount > maxLength)
                    return false;
            }
            return true;
        }

        public static string AddSlash2YYYYMM(this string val)
        {
            return !string.IsNullOrWhiteSpace(val) && !string.IsNullOrEmpty(val) && val.Length == 6 ? val.Insert(4, "/") : val;
        }

        public static string AddSlash2YYYYMMDD(this string val)
        {
            return !string.IsNullOrWhiteSpace(val) && !string.IsNullOrEmpty(val) && val.Length == 8 ? val.Insert(4, "/").Insert(7, "/") : val;
        }

        public static string AddSlash2YYMMDD(this string val)
        {
            if (string.IsNullOrWhiteSpace(val) || string.IsNullOrEmpty(val)) return val;
            else if (val.Length == 6) return val.Insert(2, "/").Insert(5, "/");
            else if (val.Length == 8) return val.Substring(2, val.Length - 2).Insert(2, "/").Insert(5, "/"); else return val;
        }

        public static string AddColon2HHMMSS(this string val)
        {
            return !string.IsNullOrWhiteSpace(val) && !string.IsNullOrEmpty(val) && val.Length == 6 ? val.Insert(2, ":").Insert(5, ":") : val;
        }

        public static string AddColon2HHMM(this string val)
        {
            if (string.IsNullOrWhiteSpace(val) || string.IsNullOrEmpty(val)) return val;
            val = val.Trim();
            if (val.Length == 4) return val.Insert(2, ":");
            else if (val.Length == 6) return val.Substring(0, val.Length - 2).Insert(2, ":");
            else return val;
        }

        public static string AddCommas(this int val) => $"{val:#,##0}";
        public static string AddCommas(this long val) => $"{val:#,##0}";

        public static string AddCommas(this string val)
        {
            if(!string.IsNullOrWhiteSpace(val) && !string.IsNullOrEmpty(val))
            {
                string temp = val;
                var decimalValue = string.Empty;
                if (val.Contains('.'))
                {
                    decimalValue = val.Substring(val.IndexOf('.'));
                    temp = val.Substring(0, val.IndexOf('.'));
                }
                
                return long.Parse(temp).AddCommas() + decimalValue;
            }
            return val;
        }

        public static string AddCommasIntoString(this string val)
        {
            if (!string.IsNullOrWhiteSpace(val) && !string.IsNullOrEmpty(val))
            {
                string temp = val;
                var decimalValue = string.Empty;
                if (val.Contains('.'))
                {
                    decimalValue = val.Substring(val.IndexOf('.'));
                    temp = val.Substring(0, val.IndexOf('.'));
                }
                var valInLong = long.Parse(temp);
                return valInLong.ToString("#,##0") + decimalValue;

            }
            return val;
        }

        public static bool IsNullOrEmptyOrWhiteSpace(this string val)
        {
            return string.IsNullOrEmpty(val) || string.IsNullOrWhiteSpace(val);
        }
    }
}
