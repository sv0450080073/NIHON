using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Commons.Helpers.ExcelExport
{
    public enum ExcelTemplateMode
    {
        OneDayQuarter,
        OneDayOneHour,
        OneDayThreeHour,
        OneDaySixHour,
        ThreeDayThreeHour,
        ThreeDaySixHour,
        OneWeekOneDay,
        OneMonthOneDay,
        FourSegment
    }

    public class ExcelExportScheduleHelper
    {
        public static ExcelPackage GenerateBusSchedule(DateTime selectedDate, ConfigBusSchedule Param, List<BusDataType> busNames, List<ItemBus> itemBuses, List<TKD_ShuriData> shuriDatas, Dictionary<string, string> colorRef)
        {
            //TODO base on Param to sort busNames + display text of bus lines
            busNames = busNames.OrderBy(b => b.SyaSyuCd).ThenBy(b => b.SyaRyoCd).ToList();
             var package = CreateExcelPackageTemplate(ExcelTemplateMode.OneDayQuarter, selectedDate, busNames, itemBuses, shuriDatas, colorRef);
            if(Param.ActiveP == (int)DayMode.OneDay && Param.ActiveCPT == (int)TimeMode.OneHour)
            {
                package = CreateExcelPackageTemplate(ExcelTemplateMode.OneDayOneHour, selectedDate, busNames, itemBuses, shuriDatas, colorRef);
            }    
            return package;
        }

        /// <summary>
        /// Generate Excel package
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="selectedDate"></param>
        /// <param name="busNames"></param>
        /// <param name="itemBuses"></param>
        /// <param name="shuriDatas"></param>
        /// <param name="colorRef"></param>
        /// <returns></returns>
        private static ExcelPackage CreateExcelPackageTemplate(ExcelTemplateMode mode, DateTime selectedDate, List<BusDataType> busNames, List<ItemBus> itemBuses, List<TKD_ShuriData> shuriDatas, Dictionary<string, string> colorRef)
        {
            DateTime start = selectedDate;
            DateTime end = selectedDate;
            int firstRow = 0;
            int lastColumn = 0;
            int minutesPerBlock = 0;
            ExcelPackage package = new ExcelPackage();
            package.Workbook.Properties.Title = "Bus schedule";
            package.Workbook.Properties.Author = "Kashikiri";
            package.Workbook.Properties.Subject = "Bus schedule";
            package.Workbook.Properties.Keywords = "Bus schedule";
            ExcelWorksheet sheet = package.Workbook.Worksheets.Add("Schedule");
            sheet.Column(1).Width = ExcelConst.BusNameWidth;
            switch (mode)
            {
                case ExcelTemplateMode.OneDayQuarter:
                    GenerateOneDayQuarterHeader(ref sheet, selectedDate);
                    start = selectedDate;
                    end = selectedDate;
                    firstRow = 6;
                    lastColumn = 1 + (24 * 4);
                    minutesPerBlock = 15;
                    sheet.View.ZoomScale = 80;
                    GenerateBusLineData(ref sheet, start, end, busNames, itemBuses, shuriDatas, colorRef, firstRow, lastColumn, minutesPerBlock, ExcelConst.SmallBlockWidth);
                    break;
                case ExcelTemplateMode.OneDayOneHour:
                    start = selectedDate;
                    end = selectedDate;
                    firstRow = 3;
                    lastColumn = 1 + 24;
                    minutesPerBlock = 60;
                    sheet.View.ZoomScale = 80;
                    GenerateOneDayOneHourHeader(ref sheet, selectedDate);
                    GenerateBusLineData(ref sheet, start, end, busNames, itemBuses, shuriDatas, colorRef, firstRow, lastColumn, minutesPerBlock, ExcelConst.MediumBlockWidth);
                    break;
                case ExcelTemplateMode.OneDayThreeHour:
                    break;
                case ExcelTemplateMode.OneDaySixHour:
                    break;
                case ExcelTemplateMode.ThreeDayThreeHour:
                    break;
                case ExcelTemplateMode.ThreeDaySixHour:
                    break;
                case ExcelTemplateMode.OneWeekOneDay:
                    break;
                case ExcelTemplateMode.OneMonthOneDay:
                    break;
                case ExcelTemplateMode.FourSegment:
                    break;
                default:
                    break;
            }
            
            return package;
        }

        private static void GenerateBusLineData(ref ExcelWorksheet sheet, DateTime startViewDate, DateTime endViewDate, List<BusDataType> busNames, List<ItemBus> itemBuses, List<TKD_ShuriData> shuriDatas, Dictionary<string, string> colorRef, int firstRow, int lastColumn, int minutesPerBlock, double excelConst)
        {
            //Mapping ItemBus to BusJob
            var busMap = itemBuses.GroupBy(i => i.BusLine).ToDictionary(g => g.Key, g => g.OrderBy(i => i.StartDate).ThenBy(i => i.TimeStart).AsEnumerable());
            var jobMap = new Dictionary<string, List<BusJob>>();
            foreach (var item in busMap)
            {
                var jobs = new List<BusJob>();
                foreach (var job in item.Value)
                {
                    var busJob = new BusJob(minutesPerBlock, startViewDate, endViewDate, 1, job, colorRef , excelConst);
                    busJob.Stack = IdentifyStack(busJob, jobs);
                    jobs.Add(busJob);
                }
                jobMap[item.Key] = jobs;
            }
            //Draw bus lines
            for (int row = firstRow; row < busNames.Count + firstRow; row++)
            {
                var bus = busNames.ElementAt(row - firstRow);
                sheet.Cells[row, 1].Value = bus.BusName;
                sheet.Cells[row, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                var jobList = jobMap.ContainsKey(bus.BusID) ? jobMap[bus.BusID] : new List<BusJob>();
                var height = jobList.Count > 0 ? jobList.Max(j => j.Stack) : 1;
                sheet.Row(row).Height = ExcelConst.BusHeight * height;
                foreach (var job in jobList.Where(j => j.IsVisible))
                {
                    ExcelShape shapeDefault = sheet.Drawings.AddShape($"{job.BusID}-{job.BookingID}-{job.TeiDanNo}-{job.BunkRen}-default", eShapeStyle.Rect);
                    shapeDefault.SetPosition(row - 1, job.RowOffset, job.PrepareColumnIndex + 1, job.PrepareColumnOffset);
                    shapeDefault.SetSize(job.PrepareLenght, ExcelHelper.HeightToPixel(ExcelConst.BusHeight));
                    shapeDefault.Fill.Color = Color.Gray;
                    ExcelShape shape = sheet.Drawings.AddShape($"{job.BusID}-{job.BookingID}-{job.TeiDanNo}-{job.BunkRen}", eShapeStyle.Rect);
                    shape.SetPosition(row - 1, job.RowOffset, job.BusColumnIndex + 1, job.BusColumnOffset);
                    shape.SetSize(job.BusLenght, ExcelHelper.HeightToPixel(ExcelConst.BusHeight));
                    shape.Fill.Color = job.LineColor;
                    shape.Text = $"{job.TitleLabel}  {job.LineLabel}";
                }
            }

            //TODO mapping bus repair to busjob
            //TODO draw bus repair
            //TODO add company at bottom of file
            //Add borders all cells
            var lastRow = firstRow - 1 + busNames.Count;//TODO + rows from repair buses
            BorderAllCells(sheet, lastRow, lastColumn);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="busNames"></param>
        /// <param name="firstRow"></param>
        /// <param name="lastColumn"></param>
        private static void BorderAllCells(ExcelWorksheet sheet, int lastRow, int lastColumn)
        {
            var range = sheet.Cells[1, 1, lastRow, lastColumn];
            range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        }

        /// <summary>
        /// Identify stack of each bus line if buses are conflicted stack will be increased
        /// </summary>
        /// <param name="busJob"></param>
        /// <param name="jobs"></param>
        /// <returns></returns>
        private static int IdentifyStack(BusJob busJob, List<BusJob> jobs)
        {
            int stack = 1;
            var existingStack = jobs.Where(j => (j.StartDateTime <= busJob.StartDateTime && j.EndDateTime >= busJob.StartDateTime)
            || (j.StartDateTime <= busJob.EndDateTime && j.EndDateTime >= busJob.EndDateTime)).Select(j => j.Stack);
            foreach (var s in existingStack)
            {
                if (stack == s)
                {
                    stack++;
                }
            }
            return stack;
        }

        /// <summary>
        /// Generate header of OneDayQuarter
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="selectedDate"></param>
        private static void GenerateOneDayQuarterHeader(ref ExcelWorksheet sheet, DateTime selectedDate)
        {
            var exportDateRow = 1;
            var titleRow = 2;
            int dateRow = 3;
            int hourRow = 4;
            int minuteRow = 5;
            var titleValue = "車 輌 別 線 引(１日 １５分)";
            var exportDateHeader = sheet.Cells[exportDateRow, 1, exportDateRow, 1 + (24 * 4)];
            exportDateHeader.Merge = true;
            exportDateHeader.Value = DateTime.Now.ToString(ExcelConst.DisplayDateTimeFormat);
            exportDateHeader.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
            var titleHeader = sheet.Cells[titleRow, 1, titleRow, 1 + (24 * 4)];
            titleHeader.Merge = true;
            titleHeader.Value = titleValue;
            titleHeader.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            var dateHeader = sheet.Cells[dateRow, 2, dateRow, 1 + (24 * 4)];
            dateHeader.Merge = true;
            dateHeader.Value = selectedDate.ToString(ExcelConst.DisplayDateFormat);
            dateHeader.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            for (int i = 1; i <= 24; i++)
            {
                var hourHeader = sheet.Cells[hourRow, i * 4 - 2, hourRow, i * 4 + 1];
                hourHeader.Merge = true;
                hourHeader.Value = i - 1;
                hourHeader.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                for (int j = 1; j <= 4; j++)
                {
                    var minCol = i * 4 + j - 3;
                    var minuteHeader = sheet.Cells[minuteRow, minCol, minuteRow, minCol];
                    minuteHeader.Value = 15 * (j - 1);
                    sheet.Column(minCol).Width = ExcelConst.SmallBlockWidth;
                    minuteHeader.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                }
            }
        }

        /// <summary>
        /// Generate header of OneDayOneHour
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="selectedDate"></param>
        private static void GenerateOneDayOneHourHeader(ref ExcelWorksheet sheet, DateTime selectedDate)
        {
            var dateHeader = sheet.Cells[1, 2, 1, 1 + (24)];
            dateHeader.Merge = true;
            dateHeader.Value = selectedDate.ToString();
            dateHeader.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            for (int i = 1; i <= 24; i++)
            {
                var hourCol = 1 + i;
                var hourHeader = sheet.Cells[2, hourCol, 2, hourCol];
                hourHeader.Merge = true;
                hourHeader.Value = i - 1;
                hourHeader.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                sheet.Column(hourCol).Width = ExcelConst.MediumBlockWidth;
            }
        }

        /// <summary>
        /// Convert minutes to pixels base on minutes per block and block's width
        /// </summary>
        /// <param name="minutes"></param>
        /// <param name="minutesPerBlock"></param>
        /// <param name="blockWidth"></param>
        /// <returns></returns>
        public static int MinutesToPixel(double minutes, int minutesPerBlock, double blockWidth)
        {
            double width = minutes / minutesPerBlock * blockWidth;
            return ExcelHelper.WidthToPixel(width);
        }
    }
}
