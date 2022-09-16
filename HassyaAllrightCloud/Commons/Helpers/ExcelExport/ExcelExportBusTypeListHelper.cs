using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;


namespace HassyaAllrightCloud.Commons.Helpers.ExcelExport
{
    public class ExcelExportBusTypeListHelper
    {
        public enum ExcelTemplateMode
        {
            NotGroup,
            BranchGroup,
            CompanyGroup
        }
        public enum DayOff
        {
            sunday,
            saturday,
            off
        }
       


        public static ExcelPackage GenerateBusTypeList(BusTypeListData searchParam, 
            List<BusTypeItemDataReport> busTypeList, List<BusTypeDetailDataReport> busTypeDataRowList, List<NumberVehicleOfBusUnAsign> busUnAsignDataRowList, List<NumberVehicleOfBusUnAsign> busHiringDataRowList, List<BusTypeDetailDataReport> totalBusTypeDataRowList, List<BusTypeDetailDataReport> totalEmployeeDataRowList, List<BusTypeItemDataReport> comBusTypeNormalList, List<BusTypeItemDataReport> branchBusTypeNormalList, List<CalendarReport> calendarReportList, List<BusTypeItemDataReport> numberOfBusTypeByGroup)
        {
            try
            {
                var package = CreateExcelPackageTemplate(ExcelTemplateMode.NotGroup, searchParam, busTypeList, busTypeDataRowList, busUnAsignDataRowList, busHiringDataRowList, totalBusTypeDataRowList, totalEmployeeDataRowList, comBusTypeNormalList, branchBusTypeNormalList, calendarReportList, numberOfBusTypeByGroup);
                return package;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private static ExcelPackage CreateExcelPackageTemplate(ExcelTemplateMode mode, BusTypeListData searchParam, List<BusTypeItemDataReport> busTypeList, List<BusTypeDetailDataReport> busTypeDataRowList, List<NumberVehicleOfBusUnAsign> busUnAsignDataRowList, List<NumberVehicleOfBusUnAsign> busHiringDataRowList, List<BusTypeDetailDataReport> totalBusTypeDataRowList, List<BusTypeDetailDataReport> totalEmployeeDataRowList, List<BusTypeItemDataReport> comBusTypeNormalList, List<BusTypeItemDataReport> branchBusTypeNormalList, List<CalendarReport> calendarReportList, List<BusTypeItemDataReport> numberOfBusTypeByGroup)
        {
            try
            {
                DateTime startDate = searchParam.StartDate;
                DateTime end = searchParam.StartDate;
                int firstRow = 0;
                int lastColumn = 0;
                ExcelPackage package = new ExcelPackage();
                package.Workbook.Properties.Title = "BusTypeList";
                package.Workbook.Properties.Author = "Kashikiri";
                package.Workbook.Properties.Subject = "BusTypeList";
                package.Workbook.Properties.Keywords = "BusTypeList";
                ExcelWorksheet sheet = package.Workbook.Worksheets.Add("BusTypeList");
                sheet.Column(1).Width = ExcelConst.BusNameWidth;
                startDate = startDate;
                end = startDate;
                firstRow = 5;
                lastColumn = 32;
                sheet.View.ZoomScale = 80;
                int rowcurrent = 4;
                switch (searchParam.GroupMode)
                {
                    case GroupMode.All:
                        GenerateTitle(ref sheet, searchParam);
                        GenerateTaleHeader(ref sheet, searchParam, ref rowcurrent, GroupMode.All, null, calendarReportList);
                        GenerateBusTypeNotGroupData(ref sheet, busTypeList, busTypeDataRowList, busUnAsignDataRowList, busHiringDataRowList, totalBusTypeDataRowList, totalEmployeeDataRowList, lastColumn, ref rowcurrent, numberOfBusTypeByGroup,  searchParam);
                        break;
                    case GroupMode.Company:
                        GenerateTitle(ref sheet, searchParam);
                        foreach (var com in comBusTypeNormalList)
                        {
                            GenerateLineNameGroup(ref sheet, "#E9ECEF", com.COM_RyakuNm, ref rowcurrent);
                            GenerateTaleHeader(ref sheet, searchParam, ref rowcurrent, GroupMode.Company, com, calendarReportList);
                            GenerateBusTypeCompanyGroupData(ref sheet, busTypeList, busTypeDataRowList, busUnAsignDataRowList, busHiringDataRowList, totalBusTypeDataRowList, totalEmployeeDataRowList, com, lastColumn, ref rowcurrent, numberOfBusTypeByGroup,  searchParam);
                        }
                        break;
                    case GroupMode.Branch:
                        GenerateTitle(ref sheet, searchParam);
                        foreach (var branch in branchBusTypeNormalList)
                        {
                            GenerateLineNameGroup(ref sheet, "#e9ecef", branch.Ei_RyakuNm, ref rowcurrent);
                            GenerateTaleHeader(ref sheet, searchParam, ref rowcurrent, GroupMode.Branch, branch, calendarReportList);
                            GenerateBusTypeBranchGroupData(ref sheet, busTypeList, busTypeDataRowList, busUnAsignDataRowList, busHiringDataRowList, totalBusTypeDataRowList, totalEmployeeDataRowList, branch, lastColumn, ref rowcurrent, numberOfBusTypeByGroup,  searchParam);
                        }
                        break;
                    default:
                        break;
                }
                GenerateFooterTable(ref sheet, searchParam, ref rowcurrent);
                return package;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private static void GenerateTitle(ref ExcelWorksheet sheet, BusTypeListData searchParam)
        {
            try
            {
                var titleRow = 2;
                int dateRow = 2;
                int space = 3;
                var titleValue = "車　種　別　台　数　表　示";
                var titleHeader = sheet.Cells[titleRow, 2, titleRow, 28];
                titleHeader.Merge = true;
                titleHeader.Value = titleValue;
                titleHeader.Style.Font.Size = 16;
                titleHeader.Style.Font.Bold = true;
                titleHeader.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                var dateCurrent = sheet.Cells[dateRow, 1];
                dateCurrent.Value = searchParam.StartDate.ToString(ExcelConst.DisplayDateFormat) + "より";
                dateCurrent.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                var dateSearch = sheet.Cells[dateRow, 29, dateRow, 32];
                dateSearch.Merge = true;
                dateSearch.Value = DateTime.Now.ToString(ExcelConst.DisplayDateTimeFormat);
                dateSearch.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                var spaceRow = sheet.Cells[space, 1, space, 32];
                spaceRow.Merge = true;
            }
            catch (Exception ex)
            {

            }
        }
        private static void GenerateTaleHeader(ref ExcelWorksheet sheet, BusTypeListData searchParam, ref int rowCurrent, GroupMode groupMode, BusTypeItemDataReport groupComOrBranchItem, List<CalendarReport> calendarReportList)
        {
            try
            {
                CultureInfo ci = new CultureInfo("ja-JP");
                var headername = sheet.Cells[rowCurrent, 1, rowCurrent, 1];
                headername.Merge = true;
                headername.Value = "車種名";
                headername.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                headername.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                headername.Style.Fill.PatternType = ExcelFillStyle.Solid;
                Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#F5F5F5");
                headername.Style.Fill.BackgroundColor.SetColor(colFromHex);
                sheet.Row(rowCurrent).Height = 40;
                // sheet.Row(rowCurrent).CustomHeight = false;
                for (var i = 1; i <= searchParam.numberDay; i++)
                {
                    DateTime date = searchParam.StartDate.AddDays(i - 1);
                    var hourHeader = sheet.Cells[rowCurrent, i + 1, rowCurrent, i + 1];
                    hourHeader.Merge = true;
                    var valueCell = date.ToString("MM月dd", ci) + "\n " + date.ToString("(ddd)", ci);
                    hourHeader.Value = valueCell;
                    hourHeader.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    hourHeader.Style.Fill.PatternType = ExcelFillStyle.Solid;                  
                    hourHeader.Style.Fill.BackgroundColor.SetColor(colFromHex);
                    hourHeader.Style.WrapText = true;         
                    if (date.DayOfWeek.ToString("G").ToLower() == DayOff.sunday.ToString())
                    {
                        SetColorColumnBusName(sheet, rowCurrent, i + 1, "#FFF0F5", BusMode.TableHeader);
                        hourHeader.Style.Font.Color.SetColor(Color.Red);
                    }
                    else if (date.DayOfWeek.ToString("G").ToLower() == DayOff.saturday.ToString())
                    {
                        SetColorColumnBusName(sheet, rowCurrent, i + 1, "#F0F8FF", BusMode.TableHeader);
                        hourHeader.Style.Font.Color.SetColor(Color.Blue);
                    }
                    if (groupMode == GroupMode.Branch || groupMode == GroupMode.Company)
                    {
                        if (groupComOrBranchItem != null)
                        {
                            if (IsDayOff(date, calendarReportList, groupComOrBranchItem))
                            {
                                SetColorColumnBusName(sheet, rowCurrent, i + 1, "#FFF0F5", BusMode.TableHeader);
                                hourHeader.Style.Font.Color.SetColor(Color.Red);
                            }
                        }
                    }
                    else if (groupMode == GroupMode.All)
                    {
                        if (IsDayOff(date, calendarReportList, groupComOrBranchItem))
                        {
                            SetColorColumnBusName(sheet, rowCurrent, i + 1, "#FFF0F5", BusMode.TableHeader);
                            hourHeader.Style.Font.Color.SetColor(Color.Red);
                        }
                    }
                    //todo notfroup 
                    var hourCol = 1 + i;
                }
                rowCurrent += 1;
            }
            catch (Exception ex)
            {

            }
        }
        private static void GenerateLineNameGroup(ref ExcelWorksheet sheet, string colorLine, string lineName, ref int rowCurrent)
        {
            try
            {
                Color colFromHex = System.Drawing.ColorTranslator.FromHtml(colorLine);
                CultureInfo ci = new CultureInfo("ja-JP");
                var lineGroup = sheet.Cells[rowCurrent, 1, rowCurrent, 31];
                lineGroup.Merge = true;
                lineGroup.Value = lineName;
                lineGroup.Style.Fill.PatternType = ExcelFillStyle.Solid;
                lineGroup.Style.Fill.BackgroundColor.SetColor(colFromHex);
                lineGroup.Style.Font.Bold = true;
                lineGroup.Style.Font.Size = 13;
                lineGroup.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                rowCurrent += 1;
            }
            catch (Exception ex)
            {

            }
        }
        private static void GenerateFooterTable(ref ExcelWorksheet sheet, BusTypeListData searchParam, ref int rowCurrent)
        {
            try
            {
                var footer = sheet.Cells[rowCurrent, 25, rowCurrent, 32];
                footer.Merge = true;
                footer.Value = searchParam.SyainCd + "   " + searchParam.SyainNm;
                footer.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                rowCurrent += 1;
            }
            catch (Exception ex)
            {

            }
        }
        //Excel BusTypeNormal
        private static void GenerateBusTypeNotGroupData(ref ExcelWorksheet sheet, List<BusTypeItemDataReport> busTypeList, List<BusTypeDetailDataReport> busTypeDataRowList, List<NumberVehicleOfBusUnAsign> busUnAsignDataRowList, List<NumberVehicleOfBusUnAsign> busHiringDataRowList, List<BusTypeDetailDataReport> totalBusTypeDataRowList, List<BusTypeDetailDataReport> totalEmployeeDataRowList, int lastColumn, ref int rowCurrent, List<BusTypeItemDataReport> numberOfBusTypeByGroup, BusTypeListData searchParam)
        {
            try
            {
                //bustype normal 
                int rowBusNormal = rowCurrent;
                for (int row = rowBusNormal; row < busTypeList.Count + rowBusNormal; row++)
                {
                    var busType = busTypeList.ElementAt(row - rowBusNormal);
                    var numberBusType = numberOfBusTypeByGroup.Where(x => x.SyaSyuCdSeq == busType.SyaSyuCdSeq).FirstOrDefault();
                    sheet.Cells[row, 1].Value = busType.SyaSyuNm + "(" + numberBusType.CountNumberOfVehicle + ")";
                    SetColorColumnBusName(sheet, row, 1, "#BBDeFB", BusMode.BusMormal);
                    if (busType != null)
                    {
                        var numberVehicleByBusTypeData = busTypeDataRowList.Where(x => x.SyaSyuCdSeq == busType.SyaSyuCdSeq).ToList();
                        if (numberVehicleByBusTypeData.Any())
                        {
                            for (int i = 1; i <= searchParam.numberDay; i++)
                            {
                                var numberVerhicleItem = numberVehicleByBusTypeData[i - 1];

                                sheet.Cells[row, i + 1].Value = numberVerhicleItem.NumberText;
                                if (IsNumberBusGreaterThanNumberBusType(numberBusType, numberVerhicleItem.Number))
                                {
                                    sheet.Cells[row, i + 1].Style.Font.Color.SetColor(Color.Red);
                                }
                                sheet.Cells[row, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            }
                        }
                        else
                        {
                            for (int i = 1; i <= searchParam.numberDay; i++)
                            {
                                sheet.Cells[row, i + 1].Value = "";
                                sheet.Cells[row, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            }
                        }

                    }
                    rowCurrent = row;
                }
                //busUnAssign  has 3 item 
                int rowBusUnAsignStart = rowCurrent + 1;
                for (int rowBusUnAsign = rowBusUnAsignStart; rowBusUnAsign < rowBusUnAsignStart + 3; rowBusUnAsign++)
                {
                    int kataKbn = rowBusUnAsign - rowBusUnAsignStart + 1;
                    var strBusTypeName = "";
                    strBusTypeName = rowBusUnAsign - rowBusUnAsignStart == 0 ? "☆未仮車（大型）" : rowBusUnAsign - rowBusUnAsignStart == 1 ? "☆未仮車（中型）" : "☆未仮車（小型）";
                    sheet.Cells[rowBusUnAsign, 1].Value = strBusTypeName;
                    SetColorColumnBusName(sheet, rowBusUnAsign, 1, "#0E25D7", BusMode.BusUnAsign);
                    var busUnSignByKataKbn = busUnAsignDataRowList.Where(x => x.KataKbn == kataKbn).OrderBy(x => x.SyuKoYmd).ToList();
                    if (busUnSignByKataKbn.Any())
                    {
                        for (int i = 1; i <= searchParam.numberDay; i++)
                        {
                            var numberVerhicleItem = busUnSignByKataKbn[i - 1];
                            sheet.Cells[rowBusUnAsign, i + 1].Value = numberVerhicleItem.NumberOfVehicleText;
                            sheet.Cells[rowBusUnAsign, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }
                    }
                    else
                    {
                        for (int i = 1; i <= searchParam.numberDay; i++)
                        {
                            sheet.Cells[rowBusUnAsign, i + 1].Value = "";
                            sheet.Cells[rowBusUnAsign, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }
                    }
                    rowCurrent = rowBusUnAsign;
                }
                //busHiring
                int rowBusHiringStart = rowCurrent + 1;
                for (int rowBusHiring = rowBusHiringStart; rowBusHiring < rowBusHiringStart + 1; rowBusHiring++)
                {
                    var strBusTypeName = "○傭車";
                    sheet.Cells[rowBusHiring, 1].Value = strBusTypeName;
                    SetColorColumnBusName(sheet, rowBusHiring, 1, "#0E25D7", BusMode.BusHiring);
                    for (int i = 1; i <= searchParam.numberDay; i++)
                    {
                        if (busHiringDataRowList.Any())
                        {
                            var numberVerhicleItem = busHiringDataRowList[i - 1];
                            sheet.Cells[rowBusHiring, i + 1].Value = numberVerhicleItem.NumberOfVehicleText;
                            sheet.Cells[rowBusHiring, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }
                        else
                        {
                            sheet.Cells[rowBusHiring, i + 1].Value = "";
                            sheet.Cells[rowBusHiring, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }

                    }
                    rowCurrent = rowBusHiring;
                }
                //Total bus by katabn
                int rowBusTotalStart = rowCurrent + 1;
                for (int rowBusTotal = rowBusTotalStart; rowBusTotal < rowBusTotalStart + 3; rowBusTotal++)
                {
                    int kataKbn = rowBusTotal - rowBusTotalStart + 1;
                    var strBusTypeName = "";
                    strBusTypeName = rowBusTotal - rowBusTotalStart == 0 ? "●合計（大型）" : rowBusTotal - rowBusTotalStart == 1 ? "●合計（中型）" : "●合計（小型）";
                    sheet.Cells[rowBusTotal, 1].Value = strBusTypeName;
                    SetColorColumnBusName(sheet, rowBusTotal, 1, "#FFB74E", BusMode.BusMormal);
                    var busTotalByKataKbn = totalBusTypeDataRowList.Where(x => x.KataKbn == kataKbn).OrderBy(x => x.SyuKoYmd).ToList();
                    if (busTotalByKataKbn.Any())
                    {
                        for (int i = 1; i <= searchParam.numberDay; i++)
                        {
                            var numberVerhicleItem = busTotalByKataKbn[i - 1];
                            sheet.Cells[rowBusTotal, i + 1].Value = numberVerhicleItem.NumberText;
                            sheet.Cells[rowBusTotal, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }
                    }
                    else
                    {
                        for (int i = 1; i <= searchParam.numberDay; i++)
                        {
                            sheet.Cells[rowBusTotal, i + 1].Value = "";
                            sheet.Cells[rowBusTotal, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }
                    }
                    rowCurrent = rowBusTotal;
                }
                //driver and gui 
                int rowBusTotalEmployeeStart = rowCurrent + 1;
                for (int rowEmployeeTotal = rowBusTotalEmployeeStart; rowEmployeeTotal < rowBusTotalEmployeeStart + 2; rowEmployeeTotal++)
                {
                    int isDriver = rowEmployeeTotal - rowBusTotalEmployeeStart + 1;
                    var strBusTypeName = "";
                    strBusTypeName = rowEmployeeTotal - rowBusTotalEmployeeStart == 0 ? "●合計（運転手数）" : "●合計（ガイド数）";
                    sheet.Cells[rowEmployeeTotal, 1].Value = strBusTypeName;
                    SetColorColumnBusName(sheet, rowEmployeeTotal, 1, "#FFB74E", BusMode.BusMormal);
                    var totalEmployee = totalEmployeeDataRowList.Where(x => x.IsDriver == isDriver).OrderBy(x => x.SyuKoYmd).ToList();

                    if (totalEmployee.Any())
                    {
                        for (int i = 1; i <= searchParam.numberDay; i++)
                        {
                            var numberVerhicleItem = totalEmployee[i - 1];
                            sheet.Cells[rowEmployeeTotal, i + 1].Value = numberVerhicleItem.NumberText;
                            sheet.Cells[rowEmployeeTotal, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }

                    }
                    else
                    {
                        for (int i = 1; i <= searchParam.numberDay; i++)
                        {
                            sheet.Cells[rowEmployeeTotal, i + 1].Value = "";
                            sheet.Cells[rowEmployeeTotal, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }
                    }


                    rowCurrent = rowEmployeeTotal;
                }

                var lastRow = rowCurrent - 1;
                BorderAllCells(sheet, rowCurrent, lastColumn);
                rowCurrent += 1;
            }
            catch (Exception ex)
            {

            }
        }
        //Excel BusType Group Company 
        private static void GenerateBusTypeCompanyGroupData(ref ExcelWorksheet sheet, List<BusTypeItemDataReport> busTypeList, List<BusTypeDetailDataReport> busTypeDataRowList, List<NumberVehicleOfBusUnAsign> busUnAsignDataRowList, List<NumberVehicleOfBusUnAsign> busHiringDataRowList, List<BusTypeDetailDataReport> totalBusTypeDataRowList, List<BusTypeDetailDataReport> totalEmployeeDataRowList, BusTypeItemDataReport comBusTypeItem, int lastColumn, ref int rowCurrent, List<BusTypeItemDataReport> numberOfBusTypeByGroup, BusTypeListData searchParam)
        {
            try
            {
                var busTypeByComSeq = busTypeDataRowList.Where(x => x.CompanyCdSeq == comBusTypeItem.CompanyCdSeq).ToList();
                var busUnAsignByComSeq = busUnAsignDataRowList.Where(x => x.CompanyCdSeq == comBusTypeItem.CompanyCdSeq).ToList();
                var busHiringByComSeq = busHiringDataRowList.Where(x => x.CompanyCdSeq == comBusTypeItem.CompanyCdSeq).ToList();
                var totalBusTypeByComSeq = totalBusTypeDataRowList.Where(x => x.CompanyCdSeq == comBusTypeItem.CompanyCdSeq).ToList();
                var totalEmployeeByComSeq = totalEmployeeDataRowList.Where(x => x.CompanyCdSeq == comBusTypeItem.CompanyCdSeq).ToList();
                //bustype normal 
                int rowbusNormal = rowCurrent;
                for (int row = rowbusNormal; row < busTypeList.Count + rowbusNormal; row++)
                {
                    var busType = busTypeList.ElementAt(row - rowbusNormal);
                    var numberBusType = numberOfBusTypeByGroup.Where(x => x.SyaSyuCdSeq == busType.SyaSyuCdSeq && x.CompanyCdSeq== comBusTypeItem.CompanyCdSeq).FirstOrDefault();
                    int valueBusType = 0;
                    valueBusType = numberBusType?.CountNumberOfVehicle == null ? 0 : numberBusType.CountNumberOfVehicle;
                    sheet.Cells[row, 1].Value = busType.SyaSyuNm + "("+ valueBusType + ")";                  
                    SetColorColumnBusName(sheet, row, 1, "#BBDeFB", BusMode.BusMormal);
                    if (busType != null)
                    {
                        var numberVehicleByBusTypeData = busTypeByComSeq.Where(x => x.SyaSyuCdSeq == busType.SyaSyuCdSeq).ToList();
                        if (numberVehicleByBusTypeData.Any())
                        {
                            for (int i = 1; i <= searchParam.numberDay; i++)
                            {
                                var numberVerhicleItem = numberVehicleByBusTypeData[i - 1];
                                sheet.Cells[row, i + 1].Value = numberVerhicleItem.NumberText;
                                sheet.Cells[row, i + 1].Value = numberVerhicleItem.NumberText;
                                if (IsNumberBusGreaterThanNumberBusType(numberBusType, numberVerhicleItem.Number))
                                {
                                    sheet.Cells[row, i + 1].Style.Font.Color.SetColor(Color.Red);
                                }

                                sheet.Cells[row, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            }
                        }
                        else
                        {
                            for (int i = 1; i <= searchParam.numberDay; i++)
                            {
                                sheet.Cells[row, i + 1].Value = "";
                                sheet.Cells[row, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            }
                        }

                    }
                    rowCurrent = row;
                }
                //busUnAssign  has 3 item 
                int rowBusUnAsignStart = rowCurrent + 1;
                for (int rowBusUnAsign = rowBusUnAsignStart; rowBusUnAsign < rowBusUnAsignStart + 3; rowBusUnAsign++)
                {
                    int kataKbn = rowBusUnAsign - rowBusUnAsignStart + 1;
                    var strBusTypeName = "";
                    strBusTypeName = rowBusUnAsign - rowBusUnAsignStart == 0 ? "☆未仮車（大型）" : rowBusUnAsign - rowBusUnAsignStart == 1 ? "☆未仮車（中型）" : "☆未仮車（小型）";
                    sheet.Cells[rowBusUnAsign, 1].Value = strBusTypeName;
                    SetColorColumnBusName(sheet, rowBusUnAsign, 1, "#0E25D7", BusMode.BusUnAsign);
                    var busUnSignByKataKbn = busUnAsignByComSeq.Where(x => x.KataKbn == kataKbn).OrderBy(x => x.SyuKoYmd).ToList();
                    if (busUnSignByKataKbn.Any())
                    {
                        for (int i = 1; i <= searchParam.numberDay; i++)
                        {
                            var numberVerhicleItem = busUnSignByKataKbn[i - 1];
                            sheet.Cells[rowBusUnAsign, i + 1].Value = numberVerhicleItem.NumberOfVehicleText;
                            sheet.Cells[rowBusUnAsign, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }
                    }
                    else
                    {
                        for (int i = 1; i <= searchParam.numberDay; i++)
                        {
                            sheet.Cells[rowBusUnAsign, i + 1].Value = "";
                            sheet.Cells[rowBusUnAsign, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }
                    }

                    rowCurrent = rowBusUnAsign;
                }
                //busHiring
                int rowBusHiringStart = rowCurrent + 1;
                for (int rowBusHiring = rowBusHiringStart; rowBusHiring < rowBusHiringStart + 1; rowBusHiring++)
                {
                    var strBusTypeName = "○傭車";
                    sheet.Cells[rowBusHiring, 1].Value = strBusTypeName;
                    SetColorColumnBusName(sheet, rowBusHiring, 1, "#0E25D7", BusMode.BusHiring);
                    for (int i = 1; i <= searchParam.numberDay; i++)
                    {
                        if (busHiringDataRowList.Any())
                        {
                            var numberVerhicleItem = busHiringByComSeq[i - 1];
                            sheet.Cells[rowBusHiring, i + 1].Value = numberVerhicleItem.NumberOfVehicleText;
                            sheet.Cells[rowBusHiring, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }
                        else
                        {
                            sheet.Cells[rowBusHiring, i + 1].Value = "";
                            sheet.Cells[rowBusHiring, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }

                    }
                    rowCurrent = rowBusHiring;
                }
                //Total bus by katabn
                int rowBusTotalStart = rowCurrent + 1;
                for (int rowBusTotal = rowBusTotalStart; rowBusTotal < rowBusTotalStart + 3; rowBusTotal++)
                {
                    int kataKbn = rowBusTotal - rowBusTotalStart + 1;
                    var strBusTypeName = "";
                    strBusTypeName = rowBusTotal - rowBusTotalStart == 0 ? "●合計（大型）" : rowBusTotal - rowBusTotalStart == 1 ? "●合計（中型）" : "●合計（小型）";
                    sheet.Cells[rowBusTotal, 1].Value = strBusTypeName;
                    SetColorColumnBusName(sheet, rowBusTotal, 1, "#FFB74E", BusMode.BusMormal);
                    var busTotalByKataKbn = totalBusTypeByComSeq.Where(x => x.KataKbn == kataKbn).OrderBy(x => x.SyuKoYmd).ToList();
                    if (busTotalByKataKbn.Any())
                    {
                        for (int i = 1; i <= searchParam.numberDay; i++)
                        {
                            var numberVerhicleItem = busTotalByKataKbn[i - 1];
                            sheet.Cells[rowBusTotal, i + 1].Value = numberVerhicleItem.NumberText;
                            sheet.Cells[rowBusTotal, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }
                    }
                    else
                    {
                        for (int i = 1; i <= searchParam.numberDay; i++)
                        {
                            sheet.Cells[rowBusTotal, i + 1].Value = "";
                            sheet.Cells[rowBusTotal, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }
                    }

                    rowCurrent = rowBusTotal;
                }
                //driver and gui 
                int rowBusTotalEmployeeStart = rowCurrent + 1;
                for (int rowEmployeeTotal = rowBusTotalEmployeeStart; rowEmployeeTotal < rowBusTotalEmployeeStart + 2; rowEmployeeTotal++)
                {
                    int isDriver = rowEmployeeTotal - rowBusTotalEmployeeStart + 1;
                    var strBusTypeName = "";
                    strBusTypeName = rowEmployeeTotal - rowBusTotalEmployeeStart == 0 ? "●合計（運転手数）" : "●合計（ガイド数）";
                    sheet.Cells[rowEmployeeTotal, 1].Value = strBusTypeName;
                    SetColorColumnBusName(sheet, rowEmployeeTotal, 1, "#FFB74E", BusMode.BusMormal);
                    var totalEmployee = totalEmployeeByComSeq.Where(x => x.IsDriver == isDriver).OrderBy(x => x.SyuKoYmd).ToList();
                    if (totalEmployee.Any())
                    {
                        for (int i = 1; i <= searchParam.numberDay; i++)
                        {
                            var numberVerhicleItem = totalEmployee[i - 1];
                            sheet.Cells[rowEmployeeTotal, i + 1].Value = numberVerhicleItem.NumberText;
                            sheet.Cells[rowEmployeeTotal, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        }
                    }
                    else
                    {
                        for (int i = 1; i <= searchParam.numberDay; i++)
                        {
                            sheet.Cells[rowEmployeeTotal, i + 1].Value = "";
                            sheet.Cells[rowEmployeeTotal, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        }
                    }
                    rowCurrent = rowEmployeeTotal;
                }
                //set rowcurrent rowCurrent++
                rowCurrent += 1;
                var lastRow = rowCurrent - 1;
                BorderAllCells(sheet, lastRow, lastColumn);
            }
            catch (Exception ex)
            {

            }
        }
        //Excel Bustype Group Branch 
        private static void GenerateBusTypeBranchGroupData(ref ExcelWorksheet sheet, List<BusTypeItemDataReport> busTypeList, List<BusTypeDetailDataReport> busTypeDataRowList, List<NumberVehicleOfBusUnAsign> busUnAsignDataRowList, List<NumberVehicleOfBusUnAsign> busHiringDataRowList, List<BusTypeDetailDataReport> totalBusTypeDataRowList, List<BusTypeDetailDataReport> totalEmployeeDataRowList, BusTypeItemDataReport branchBusTypeNormalItem, int lastColumn, ref int rowCurrent, List<BusTypeItemDataReport> numberOfBusTypeByGroup, BusTypeListData searchParam)
        {
            try
            {
                var busTypeByBranchSeq = busTypeDataRowList.Where(x => x.EigyoCdSeq == branchBusTypeNormalItem.EigyoCdSeq).ToList();
                var busUnAsignByBranchSeq = busUnAsignDataRowList.Where(x => x.UkeEigCdSeq == branchBusTypeNormalItem.EigyoCdSeq).ToList();
                var busHiringByBranchSeq = busHiringDataRowList.Where(x => x.UkeEigCdSeq == branchBusTypeNormalItem.EigyoCdSeq).ToList();
                var totalBusTypeByBranchSeq = totalBusTypeDataRowList.Where(x => x.EigyoCdSeq == branchBusTypeNormalItem.EigyoCdSeq).ToList();
                var totalEmployeeByBranchSeq = totalEmployeeDataRowList.Where(x => x.EigyoCdSeq == branchBusTypeNormalItem.EigyoCdSeq).ToList();
                //bustype normal 
                int rowbusNormal = rowCurrent;
                for (int row = rowbusNormal; row < busTypeList.Count + rowbusNormal; row++)
                {
                    var busType = busTypeList.ElementAt(row - rowbusNormal);
                    var numberBusType = numberOfBusTypeByGroup.Where(x => x.SyaSyuCdSeq == busType.SyaSyuCdSeq && x.EigyoCdSeq== branchBusTypeNormalItem.EigyoCdSeq).FirstOrDefault();
                    int valueBusType = 0;
                    valueBusType = numberBusType?.CountNumberOfVehicle == null ? 0 : numberBusType.CountNumberOfVehicle;
                    sheet.Cells[row, 1].Value = busType.SyaSyuNm + "(" + valueBusType+ ")";
                    SetColorColumnBusName(sheet, row, 1, "#BBDeFB", BusMode.BusMormal);
                    if (busType != null)
                    {
                        var numberVehicleByBusTypeData = busTypeByBranchSeq.Where(x => x.SyaSyuCdSeq == busType.SyaSyuCdSeq).ToList();
                        if (numberVehicleByBusTypeData.Any())
                        {
                            for (int i = 1; i <= searchParam.numberDay; i++)
                            {
                                var numberVerhicleItem = numberVehicleByBusTypeData[i - 1];
                                sheet.Cells[row, i + 1].Value = numberVerhicleItem.NumberText;
                                if (IsNumberBusGreaterThanNumberBusType(numberBusType, numberVerhicleItem.Number))
                                {
                                    sheet.Cells[row, i + 1].Style.Font.Color.SetColor(Color.Red);
                                }
                                sheet.Cells[row, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            }
                        }
                        else
                        {
                            for (int i = 1; i <= searchParam.numberDay; i++)
                            {
                                sheet.Cells[row, i + 1].Value = "";
                                sheet.Cells[row, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            }
                        }

                    }
                    rowCurrent = row;
                }
                //busUnAssign  has 3 item 
                int rowBusUnAsignStart = rowCurrent + 1;
                for (int rowBusUnAsign = rowBusUnAsignStart; rowBusUnAsign < rowBusUnAsignStart + 3; rowBusUnAsign++)
                {
                    int kataKbn = rowBusUnAsign - rowBusUnAsignStart + 1;
                    var strBusTypeName = "";
                    strBusTypeName = rowBusUnAsign - rowBusUnAsignStart == 0 ? "☆未仮車（大型）" : rowBusUnAsign - rowBusUnAsignStart == 1 ? "☆未仮車（中型）" : "☆未仮車（小型）";
                    sheet.Cells[rowBusUnAsign, 1].Value = strBusTypeName;
                    SetColorColumnBusName(sheet, rowBusUnAsign, 1, "#0E25D7", BusMode.BusUnAsign);
                    var busUnSignByKataKbn = busUnAsignByBranchSeq.Where(x => x.KataKbn == kataKbn).OrderBy(x => x.SyuKoYmd).ToList();
                    if (busUnSignByKataKbn.Any())
                    {
                        for (int i = 1; i <= searchParam.numberDay; i++)
                        {
                            var numberVerhicleItem = busUnSignByKataKbn[i - 1];
                            sheet.Cells[rowBusUnAsign, i + 1].Value = numberVerhicleItem.NumberOfVehicleText;
                            sheet.Cells[rowBusUnAsign, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }
                    }
                    else
                    {
                        for (int i = 1; i <= searchParam.numberDay; i++)
                        {
                            sheet.Cells[rowBusUnAsign, i + 1].Value = "";
                            sheet.Cells[rowBusUnAsign, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }
                    }

                    rowCurrent = rowBusUnAsign;
                }
                //busHiring
                int rowBusHiringStart = rowCurrent + 1;
                for (int rowBusHiring = rowBusHiringStart; rowBusHiring < rowBusHiringStart + 1; rowBusHiring++)
                {
                    var strBusTypeName = "○傭車";
                    sheet.Cells[rowBusHiring, 1].Value = strBusTypeName;
                    SetColorColumnBusName(sheet, rowBusHiring, 1, "#0E25D7", BusMode.BusHiring);
                    for (int i = 1; i <= searchParam.numberDay; i++)
                    {
                        if (busHiringDataRowList.Any())
                        {
                            var numberVerhicleItem = busHiringByBranchSeq[i - 1];
                            sheet.Cells[rowBusHiring, i + 1].Value = numberVerhicleItem.NumberOfVehicleText;
                            sheet.Cells[rowBusHiring, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }
                        else
                        {
                            sheet.Cells[rowBusHiring, i + 1].Value = "";
                            sheet.Cells[rowBusHiring, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }

                    }
                    rowCurrent = rowBusHiring;
                }
                //Total bus by katabn
                int rowBusTotalStart = rowCurrent + 1;
                for (int rowBusTotal = rowBusTotalStart; rowBusTotal < rowBusTotalStart + 3; rowBusTotal++)
                {
                    int kataKbn = rowBusTotal - rowBusTotalStart + 1;
                    var strBusTypeName = "";
                    strBusTypeName = rowBusTotal - rowBusTotalStart == 0 ? "●合計（大型）" : rowBusTotal - rowBusTotalStart == 1 ? "●合計（中型）" : "●合計（小型）";
                    sheet.Cells[rowBusTotal, 1].Value = strBusTypeName;
                    SetColorColumnBusName(sheet, rowBusTotal, 1, "#FFB74E", BusMode.BusMormal);
                    var busTotalByKataKbn = totalBusTypeByBranchSeq.Where(x => x.KataKbn == kataKbn).OrderBy(x => x.SyuKoYmd).ToList();
                    if (busTotalByKataKbn.Any())
                    {
                        for (int i = 1; i <= searchParam.numberDay; i++)
                        {
                            var numberVerhicleItem = busTotalByKataKbn[i - 1];
                            sheet.Cells[rowBusTotal, i + 1].Value = numberVerhicleItem.NumberText;
                            sheet.Cells[rowBusTotal, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }
                    }
                    else
                    {
                        for (int i = 1; i <= searchParam.numberDay; i++)
                        {
                            sheet.Cells[rowBusTotal, i + 1].Value = "";
                            sheet.Cells[rowBusTotal, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }
                    }

                    rowCurrent = rowBusTotal;
                }
                //driver and gui 
                int rowBusTotalEmployeeStart = rowCurrent + 1;
                for (int rowEmployeeTotal = rowBusTotalEmployeeStart; rowEmployeeTotal < rowBusTotalEmployeeStart + 2; rowEmployeeTotal++)
                {
                    int isDriver = rowEmployeeTotal - rowBusTotalEmployeeStart + 1;
                    var strBusTypeName = "";
                    strBusTypeName = rowEmployeeTotal - rowBusTotalEmployeeStart == 0 ? "●合計（運転手数）" : "●合計（ガイド数）";
                    sheet.Cells[rowEmployeeTotal, 1].Value = strBusTypeName;
                    SetColorColumnBusName(sheet, rowEmployeeTotal, 1, "#FFB74E", BusMode.BusMormal);
                    var totalEmployee = totalEmployeeByBranchSeq.Where(x => x.IsDriver == isDriver).OrderBy(x => x.SyuKoYmd).ToList();
                    if (totalEmployee.Any())
                    {
                        for (int i = 1; i <= searchParam.numberDay; i++)
                        {
                            var numberVerhicleItem = totalEmployee[i - 1];
                            sheet.Cells[rowEmployeeTotal, i + 1].Value = numberVerhicleItem.NumberText;
                            sheet.Cells[rowEmployeeTotal, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }
                    }
                    else
                    {
                        for (int i = 1; i <= searchParam.numberDay; i++)
                        {

                            sheet.Cells[rowEmployeeTotal, i + 1].Value = "";
                            sheet.Cells[rowEmployeeTotal, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }
                    }
                    rowCurrent = rowEmployeeTotal;
                }
                rowCurrent += 1;
                var lastRow = rowCurrent - 1;
                BorderAllCells(sheet, lastRow, lastColumn);
            }
            catch (Exception ex)
            {

            }
        }
        private static void SetColorColumnBusName(ExcelWorksheet sheet, int row, int cell, string hexColor, BusMode busMode)
        {
            try
            {
                Color colFromHex = System.Drawing.ColorTranslator.FromHtml(hexColor);
                sheet.Cells[row, cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells[row, cell].Style.Fill.BackgroundColor.SetColor(colFromHex);
                sheet.Cells[row, cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                if (busMode == BusMode.BusUnAsign || busMode == BusMode.BusHiring)
                {
                    sheet.Cells[row, cell].Style.Font.Color.SetColor(Color.White);
                }
                if (busMode == BusMode.TableHeader)
                {
                    sheet.Cells[row, cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }
            }
            catch (Exception ex)
            {

            }
        }
        private static void BorderAllCells(ExcelWorksheet sheet, int lastRow, int lastColumn)
        {
            try
            {
                var range = sheet.Cells[4, 1, lastRow, lastColumn];
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            }
            catch (Exception ex)
            {

            }

        }
        public static bool IsDayOff(DateTime dayCurrent, List<CalendarReport> calendarReportList, BusTypeItemDataReport groupComOrBranchItem)
        {
            bool result = false;
            try
            {
                if (calendarReportList != null && calendarReportList.Any())
                {
                    var dayOffByCom = new List<CalendarReport>();
                    if(groupComOrBranchItem !=null)
                    {
                        dayOffByCom = calendarReportList.Where(x => x.CompanyCdSeq == groupComOrBranchItem.CompanyCdSeq || x.CompanyCdSeq == 0).ToList();
                    }
                    else
                    {
                        dayOffByCom = calendarReportList.ToList();
                    }               
                    if (dayOffByCom.Any())
                    {
                        var dataOff = dayOffByCom.Where(x => x.CalenYmd == dayCurrent.ToString("yyyyMMdd")).ToList();
                        result = dataOff.Any() ? true : false;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return result;
            }

        }
        public static bool IsNumberBusGreaterThanNumberBusType(BusTypeItemDataReport numberOfBusTypeByGroupItem, int numberBus)
        {
            bool result = false;
            try
            {
                if (numberOfBusTypeByGroupItem != null)
                {
                    if (numberBus > numberOfBusTypeByGroupItem.CountNumberOfVehicle)
                    {
                        result = true;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return result;
            }

        }
       

    }
}
