using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.SubContractorStatus;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace HassyaAllrightCloud.Commons.Helpers
{
    public class KoboGridHelper
    {
        public static Func<object, string> FormatYYYYMMDDDelegate = (val) => val is string ? (val as string).AddSlash2YYYYMMDD() : string.Empty;
        public static Func<object, string> AddCommasForNumber = (val) => val != null ? val.ToString().AddCommas() : string.Empty;
        public static Func<object, string> PadLeft5 = (val) => val == null ? string.Empty : val.ToString().PadLeft(5, '0');
        public static Func<object, string> ToFormatC = (val) => string.Format("{0:C}", val);
        public static Func<object, string> ToFormatPercent = (val) => string.Format("{0}%", val);
        public static Func<object, string> ConvertDateTimeToYYMMDD = (val) => string.Format("{0:yy/MM/dd}", val);
        public static Func<object, string> AddCommasForLongNumber = (val) => val.ToString().AddCommasIntoString();
        public static Func<object, string> Numeric = (val) => string.Format("{0:0.0}", val);
        public static Func<object, string> ToFormatString = (val) => string.Format("{0:0000000000}", val);
        public static Func<object, string> ToFormatDateTime = (val) => string.Format("{0:yyyy/MM/dd（ddd）HH:mm}", val);
        public static Func<object, string> ToFormatPerson = (val) => string.Format("{0} 人", val);
        public static Func<object, string> ToFormatPlusPerson = (val) => string.Format("+{0} 人", val);
        public static Func<object, string> MarkedBoolean = (val) => Convert.ToBoolean(val) ? "⚪" : string.Empty;
        public static Func<object, string> RemoveTenantCdFromUkeNo = (val) => ((string)val).Length == 15 ? ((string)val).Remove(0, 5) : (string)val;
        public static Func<object, string> BreakListBusViewDatas_BusName = (val) =>
        {
            string result = string.Empty;
            var listData = val as List<BusViewData>;
            if (listData != null && listData.Any())
            {
                foreach (var item in listData)
                {
                    result += "\n" + item.BusName;
                }
                result = result.Substring(1);
            }
            return result;
        };
        public static Func<object, string> BreakListBusViewDatas_BusType = (val) =>
        {
            string result = string.Empty;
            var listData = val as List<BusViewData>;
            if (listData != null && listData.Any())
            {
                foreach (var item in listData)
                {
                    result += "\n" + item.BusType;
                }
                result = result.Substring(1);
            }
            return result;
        };
        public static Func<object, string> BreakListBusViewDatas_Daisu = (val) =>
        {
            string result = string.Empty;
            var listData = val as List<BusViewData>;
            if (listData != null && listData.Any())
            {
                foreach (var item in listData)
                {
                    result += "\n" + item.Daisu;
                }
                result = result.Substring(1);
            }
            return result;
        };
        public static Func<object, string> BreakListBusViewDatas_Daisu_WithSuffix = (val) =>
        {
            string result = string.Empty;
            var listData = val as List<BusViewData>;
            if (listData != null && listData.Any())
            {
                foreach (var item in listData)
                {
                    result += "\n" + item.Daisu + " 台";
                }
                result = result.Substring(1);
            }
            return result;
        };

        public static Func<object, string> BreakListBusScheduleInfo = (val) =>
        {
            string result = string.Empty;
            var datas = val as List<BusScheduleInfo>;

            if(datas?.Any() ?? false)
            {
                foreach (var item in datas)
                {
                    result += "\n";
                    result += (item.VehicleDispatch);
                    result += "\n";
                    result += (item.VehicleDispatchOther);
                    result += "\n";
                    result += (item.ArrivalConnection);
                    result += "\n";
                    result += (item.ArrivalConnectionOther);
                }
            }

            return result.Substring(1);
        };

        public static Func<object, string> BreakListBusScheduleInfoNo = (val) =>
        {
            string result = string.Empty;
            var stts = val as List<string>;

            if (stts?.Any() ?? false)
            {
                foreach (var item in stts)
                {
                    result += "\n";
                    result += "\n";
                    result += $"{item.Trim()}";
                    result += "\n";
                    result += "\n";
                }
            }
            return result.Substring(1);
        };

        public static Func<object, string> BreakListTaxFeeInfo = val =>
        {
            string result = string.Empty;
            var datas = val as List<TaxFeeInfo>;

            if (datas?.Any() ?? false)
            {
                foreach (var item in datas)
                {
                    result += "\n";
                    result += $"{item.YoushaUnc}";
                    result += "\n";
                    result += $"{item.YoushaSyo}";
                    result += "\n";
                    result += $"{item.YoushaTes}";
                    result += "\n";
                }
            }

            return result.Substring(1);
        };

        public static Func<object, string> BreakListTaxRateInfo = val =>
        {
            string result = string.Empty;
            var datas = val as List<TaxFeeInfo>;

            if (datas?.Any() ?? false)
            {
                foreach (var item in datas)
                {
                    result += "\n";
                    result += $"\t";
                    result += "\n";
                    result += $"{item.YouZeiritsu}";
                    result += "\n";
                    result += $"{item.YouTesuRitu}";
                    result += "\n";
                }
            }

            return result.Substring(2);
        };

        public static Func<object, string> FormatDateYYYYMMDD_DOW = (val) => string.IsNullOrEmpty(val.ToString().Replace(" ", string.Empty)) ? string.Empty : DateTime.ParseExact(val.ToString(), CommonConstants.FormatYMD, CultureInfo.InvariantCulture).ToString(CommonConstants.FormatYMDWithSlash);
        public static Func<object, string> FormatHHMMSSDelegate = (val) => val is string ? (val as string).AddColon2HHMMSS() : string.Empty;
        public static Func<object, string> PadLeft10 = (val) => val != null ? val.ToString().PadLeft(10) : string.Empty;
    }
}
