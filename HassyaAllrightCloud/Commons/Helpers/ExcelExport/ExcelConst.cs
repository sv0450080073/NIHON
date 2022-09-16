using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Commons.Helpers.ExcelExport
{
    public static class ExcelConst
    {
        public const double PixelPerWidth = 7;
        public const double PixelPerHeight = 1.3;
        //public const double SmallBlockWidth = 7;
        public const double SmallBlockWidth = 3;
        public const double MediumBlockWidth = 10;
        public const double BigBlockWidth = 40;
        //public const double BusNameWidth = 80;
        public const double BusNameWidth = 30;
        public const double BusLineHeight = 20;
        public const double BusHeight = 20;
        public const string DateTimeFormat = "yyyyMMddHHmm";
        public const string TimeFormat = "HH:mm";
        public const string DisplayDateFormat = "yyyy/MM/dd";
        public const string DisplayDateTimeFormat = "yyyy/MM/dd HH:mm";
    }

    public static class ExcelHelper
    {
        /// <summary>
        /// from excel width to excel pixel
        /// </summary>
        /// <param name="width"></param>
        /// <returns></returns>
        public static int WidthToPixel(double width)
        {
            return (int)Math.Ceiling(width * ExcelConst.PixelPerWidth);
        }

        /// <summary>
        /// from excel height to excel pixel
        /// </summary>
        /// <param name="height"></param>
        /// <returns></returns>
        public static int HeightToPixel(double height)
        {
            return (int)Math.Ceiling(height * ExcelConst.PixelPerHeight);
        }
    }
}
