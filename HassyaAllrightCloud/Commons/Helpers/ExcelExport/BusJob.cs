using HassyaAllrightCloud.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Commons.Helpers.ExcelExport
{
    public class BusJob
    {
        public string BusID { get; set; }
        public string BookingID { get; set; }
        public short TeiDanNo { get; set; }
        public short BunkRen { get; set; }
        public int Stack { get; set; } = 1;
        public int PrepareLenght { get; set; }
        public int PrepareColumnOffset { get; set; }
        public int PrepareColumnIndex { get; set; }
        public int BusLenght { get; set; }
        public int RowOffset { get => ExcelHelper.HeightToPixel((Stack * ExcelConst.BusLineHeight) - (1 * ExcelConst.BusHeight)); }
        public int BusColumnIndex { get; set; }
        public int BusColumnOffset { get; set; }
        public string LineLabel { get; set; }
        public string TitleLabel { get; set; }
        public System.Drawing.Color LineColor { get; set; }
        public bool IsVisible { get; set; } = false;
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        
        /// <summary>
        /// Parse Bus line into Bus Job item
        /// </summary>
        /// <param name="minutePerBlock"></param>
        /// <param name="startViewDate"></param>
        /// <param name="endViewDate"></param>
        /// <param name="stack"></param>
        /// <param name="itemBus"></param>
        /// <param name="colorRef"></param>
        public BusJob(int minutePerBlock, DateTime startViewDate, DateTime endViewDate, int stack, ItemBus itemBus, Dictionary<string, string> colorRef, double excelConst)
        {
            DateTime busStartDate, busEndDate, garageLeaveDate, garageReturnDate;
            DateTime.TryParseExact(itemBus.StartDate + itemBus.TimeStart.ToString("D4"), ExcelConst.DateTimeFormat, null, System.Globalization.DateTimeStyles.None, out busStartDate);
            DateTime.TryParseExact(itemBus.EndDate + itemBus.TimeEnd.ToString("D4"), ExcelConst.DateTimeFormat, null, System.Globalization.DateTimeStyles.None, out busEndDate);
            DateTime.TryParseExact(itemBus.StartDateDefault + itemBus.TimeStartDefault.ToString("D4"), ExcelConst.DateTimeFormat, null, System.Globalization.DateTimeStyles.None, out garageLeaveDate);
            DateTime.TryParseExact(itemBus.EndDateDefault + itemBus.TimeEndDefault.ToString("D4"), ExcelConst.DateTimeFormat, null, System.Globalization.DateTimeStyles.None, out garageReturnDate);
            var start = new DateTime(startViewDate.Year, startViewDate.Month, startViewDate.Day, 0, 0, 0);
            var end = new DateTime(endViewDate.Year, endViewDate.Month, endViewDate.Day, 23, 59, 0);
            var title = $"{busStartDate.ToString(ExcelConst.TimeFormat)} - {busEndDate.ToString(ExcelConst.TimeFormat)}";
            if (garageLeaveDate < end && garageReturnDate > start)
            {
                busStartDate = busStartDate < start ? start : busStartDate;
                busEndDate = busEndDate > end ? end : busEndDate;
                garageLeaveDate = garageLeaveDate < start ? start : garageLeaveDate;
                garageReturnDate = garageReturnDate > end ? end : garageReturnDate;
                var busLenghtMinutes = (busEndDate - busStartDate).TotalMinutes;
                var busPrepareMinutes = (garageReturnDate - garageLeaveDate).TotalMinutes;
                var busOffsetMinutes = (busStartDate - startViewDate).TotalMinutes;
                var busPrepareOffsetMinutes = (garageLeaveDate - startViewDate).TotalMinutes;
                this.Stack = stack;
                this.BusLenght = ExcelExportScheduleHelper.MinutesToPixel(busLenghtMinutes, minutePerBlock, excelConst);
                this.PrepareLenght = ExcelExportScheduleHelper.MinutesToPixel(busPrepareMinutes, minutePerBlock, excelConst);
                this.BusColumnIndex = (int)Math.Floor(busOffsetMinutes / minutePerBlock);
                this.PrepareColumnIndex = (int)Math.Floor(busPrepareOffsetMinutes / minutePerBlock);
                var offsetMinutes = busOffsetMinutes - (this.BusColumnIndex * minutePerBlock);
                var prepareoffsetMinutes = busPrepareOffsetMinutes - (this.PrepareColumnIndex * minutePerBlock);
                this.BusColumnOffset = ExcelExportScheduleHelper.MinutesToPixel(offsetMinutes, minutePerBlock, excelConst);
                this.PrepareColumnOffset = ExcelExportScheduleHelper.MinutesToPixel(prepareoffsetMinutes, minutePerBlock, excelConst);
                //this.RowOffset = ExcelExportScheduleHelper.HeightToPixel((stack * ExcelConfig.BusLineHeight) - (1 * ExcelConfig.BusHeight));
                this.LineLabel = Regex.Replace(itemBus.Text, "<.*?>", String.Empty);
                this.TitleLabel = title;
                this.LineColor = itemBus.ColorLine=="color-9"? ColorTranslator.FromHtml("#F4D9F4"):colorRef.ContainsKey(itemBus.ColorLine) ? ColorTranslator.FromHtml(colorRef[itemBus.ColorLine]) : ColorTranslator.FromHtml("#000000");
                this.BusID = itemBus.BusLine;
                this.BookingID = itemBus.BookingId;
                this.TeiDanNo = itemBus.TeiDanNo;
                this.BunkRen = itemBus.BunkRen;
                this.IsVisible = true;
                this.StartDateTime = garageLeaveDate;
                this.EndDateTime = garageReturnDate;
            }
        }

        /// <summary>
        /// TODO Update logic for repair buses
        /// </summary>
        /// <param name="minutePerBlock"></param>
        /// <param name="selectedDate"></param>
        /// <param name="stack"></param>
        /// <param name="shuriData"></param>
        public BusJob(int minutePerBlock, DateTime selectedDate, int stack, TKD_ShuriData shuriData)
        {
            DateTime repairStartDate, repairEndDate;
            DateTime.TryParseExact(shuriData.Shuri_ShuriSYmd + shuriData.Shuri_ShuriSTime, ExcelConst.DateTimeFormat, null, System.Globalization.DateTimeStyles.None, out repairStartDate);
            DateTime.TryParseExact(shuriData.Shuri_ShuriEYmd + shuriData.Shuri_ShuriETime, ExcelConst.DateTimeFormat, null, System.Globalization.DateTimeStyles.None, out repairEndDate);
            var busLenghtMinutes = (repairEndDate - repairStartDate).TotalMinutes;
            var busOffsetMinutes = (repairStartDate - selectedDate).TotalMinutes;
            this.Stack = stack;
            this.BusLenght = ExcelExportScheduleHelper.MinutesToPixel(busLenghtMinutes, minutePerBlock, ExcelConst.SmallBlockWidth);
            this.PrepareLenght = 0;
            //this.RowOffset = ExcelExportScheduleHelper.MinutesToPixel(busOffsetMinutes, minutePerBlock, ExcelConfig.SmallBlockWidth);
            this.PrepareColumnOffset = 0;
            this.BusColumnOffset = ExcelHelper.HeightToPixel((stack * ExcelConst.BusLineHeight) - (stack * ExcelConst.BusHeight));
            this.LineLabel = shuriData.Text;
            this.TitleLabel = $"{repairStartDate.ToString(ExcelConst.TimeFormat)} - {repairEndDate.ToString(ExcelConst.TimeFormat)}";
            this.BusID = shuriData.Shuri_SyaRyoCdSeq.ToString();
        }
    }
}
