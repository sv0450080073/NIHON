using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Application.SimpleQuotationReport.Queries;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using HassyaAllrightCloud.Reports.DataSource;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface ISimpleQuotationService : IReportService
    {
        Task<XtraReport> GetReport(SimpleQuotationReportFilter prviewParam);
        Task<List<BookingKeyData>> GetBookingKeyListAsync(SimpleQuotationData param);
        Task<List<SimpleQuotationDataReport>> GetSimpleQuotationListReportAsync(SimpleQuotationReportFilter param);
        Dictionary<string, string> GetFieldValues(SimpleQuotationData data);
        void ApplyFilter(ref SimpleQuotationData data, Dictionary<string, string> filterValues);
        Task<bool> ExportAsPDF(SimpleQuotationReportFilter param, IJSRuntime JSRuntime);
        Task<bool> ExportQuotationJourneyAsPDF(SimpleQuotationReportFilter param, IJSRuntime JSRuntime);
        Task<SimpleQuotationReportFilter> SetParamForSimpleQuotation(string UkeNo);
        Task<SimpleQuotationReportFilter> SetParamForQuotationJourney(string UkeNo);
    }

    public class SimpleQuotationService : ISimpleQuotationService
    {
        private readonly IMediator _mediatR;
        private readonly ILogger<SimpleQuotationService> _logger;

        public SimpleQuotationService(IMediator mediatR, ILogger<SimpleQuotationService> logger)
        {
            _mediatR = mediatR;
            _logger = logger;
        }

        public async Task<List<SimpleQuotationDataReport>> GetSimpleQuotationListReportAsync(SimpleQuotationReportFilter param)
        {
            try
            {
                var result = await _mediatR.Send(new GetSimpleQuotationReportQuery(
                    param.BookingKeyList,
                    param.TenantId,
                    param.IsWithJourney,
                    param.IsDisplayMinMaxPrice
                ));
                if (param.IsWithJourney)
                {
                    int maxLengthDantaNmDisplay = param.ReportType switch
                    {
                        QuotationReportType.JourneyHorizontal => 28,
                        QuotationReportType.JourneyVertical => 18,
                        _ => 0
                    };
                    foreach (var item in result)
                    {
                        item.HeaderData.DantaNm =
                            item.HeaderData.DantaNm.Length > maxLengthDantaNmDisplay
                            ? $"{item.HeaderData.DantaNm.Substring(0, maxLengthDantaNmDisplay)}    様 御一行"
                            : $"{item.HeaderData.DantaNm}    様 御一行";
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<List<BookingKeyData>> GetBookingKeyListAsync(SimpleQuotationData param)
        {
            try
            {
                var result = await _mediatR.Send(new GetSimpleQuotationReportPagedKeysQuery(param));
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<XtraReport> PreviewReport(string queryParams)
        {
            try
            {
                var prviewParam = EncryptHelper.DecryptFromUrl<SimpleQuotationReportFilter>(queryParams);
                return await GetReport(prviewParam);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new XtraReport();
            }
        }

        /// <summary>
        /// Get report template and data using filter params
        /// </summary>
        /// <param name="prviewParam">Filter data</param>
        /// <returns>XtraReport match with filter params</returns>
        public async Task<XtraReport> GetReport(SimpleQuotationReportFilter prviewParam)
        {
            try
            {
                ObjectDataSource dataSource = new ObjectDataSource();
                var data = await GetSimpleQuotationListReportAsync(prviewParam);
                if (prviewParam.IsWithJourney)
                {
                    int bodyReportRowPerPage = prviewParam.ReportType switch
                    {
                        QuotationReportType.JourneyHorizontal => 13,
                        QuotationReportType.JourneyVertical => 20,
                        _ => 0
                    };
                    HandlePagingQuotationJourney(ref data, bodyReportRowPerPage);
                }

                XtraReport report = prviewParam.ReportType switch
                {
                    QuotationReportType.JourneyHorizontal => new Reports.ReportTemplate.QuotationWithJourney.QuotationWithJourneyHorizontal(),
                    QuotationReportType.JourneyVertical => new Reports.ReportTemplate.QuotationWithJourney.QuotationWithJourneyVertical(),
                    _ => new Reports.ReportTemplate.SimpleQuotation.SimpleQuotation(),
                };
                Parameter param = new Parameter()
                {
                    Name = "data",
                    Type = typeof(List<SimpleQuotationDataReport>),
                    Value = data
                };
                dataSource.Name = "objectDataSource1";
                dataSource.DataSource = typeof(SimpleQuotationReportDataSource);
                dataSource.Constructor = new ObjectConstructorInfo(param);
                dataSource.DataMember = "_data";
                report.DataSource = dataSource;

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Paging using model for QuotationJourney report / Kotei and Tehai list
        /// <param name="reportDataList">Data list report use for pagging</param>
        /// <param name="bodyReportRowPerPage">Body rows in horizontal and vertical is different</param>
        /// </summary>
        private void HandlePagingQuotationJourney(ref List<SimpleQuotationDataReport> reportDataList, int bodyReportRowPerPage)
        {
            const int footerReportRowPerPage = 11;

            var addedPages = new List<SimpleQuotationDataReport>();
            foreach (var rpData in reportDataList)
            {
                var maxNittei = Math.Max(
                    rpData.KoteiDataList.DefaultIfEmpty(new SimpleQuotationDataReport.BodyJourney()).Max(k => k.Nittei),
                    rpData.TehaiDataList.DefaultIfEmpty(new SimpleQuotationDataReport.BodyJourney()).Max(k => k.Nittei)
                );
                for (int iNittei = 1; iNittei <= maxNittei; iNittei++)
                {
                    var koteiListInDate = rpData.KoteiDataList.Where(k => k.Nittei == iNittei).ToList();
                    var tehaiListInDate = rpData.TehaiDataList.Where(k => k.Nittei == iNittei).ToList();
                    var lMin = koteiListInDate.Count < tehaiListInDate.Count
                        ? koteiListInDate
                        : tehaiListInDate;
                    var lMax = koteiListInDate.Count > tehaiListInDate.Count
                        ? koteiListInDate
                        : tehaiListInDate;
                    if (lMin.Count == 0 && lMax.Count > 0)
                    {
                        lMax.ForEach(_ => _.IsHideDate = true);
                        lMax[0].IsHideDate = false;
                        rpData.BodyJourneyDataList.AddRange(lMax);
                        continue;
                    }
                    for (int i = 0; i < lMax.Count; i++)
                    {
                        rpData.BodyJourneyDataList.Add(
                            new SimpleQuotationDataReport.BodyJourney()
                            {
                                HaiSYmd = lMax[i].HaiSYmd,
                                UkeNo = lMax[i].UkeNo,
                                UnkRen = lMax[i].UnkRen,
                                Nittei = lMax[i].Nittei,
                                Koutei = lMax[i].Koutei ?? (i >= lMin.Count ? string.Empty : lMin[i].Koutei),
                                TehNm = lMax[i].TehNm ?? (i >= lMin.Count ? string.Empty : lMin[i].TehNm),
                                IsHideDate = i != 0
                            }
                        );
                    }
                }

                var totalPagePerKey = Math.Max(
                    rpData.BodyJourneyDataList.Count / bodyReportRowPerPage + (rpData.BodyJourneyDataList.Count % bodyReportRowPerPage == 0 ? 0 : 1),
                    rpData.BodyDataList.Count / footerReportRowPerPage + (rpData.BodyDataList.Count % footerReportRowPerPage == 0 ? 0 : 1)
                );
                rpData.BodyJourneyDataList.AddRange(
                    Enumerable.Repeat(new SimpleQuotationDataReport.BodyJourney(), totalPagePerKey * bodyReportRowPerPage - rpData.BodyJourneyDataList.Count)
                );
                rpData.BodyDataList.AddRange(
                    Enumerable.Repeat(new SimpleQuotationDataReport.Body(isEmpty: true), totalPagePerKey * footerReportRowPerPage - rpData.BodyDataList.Count)
                );

                if (totalPagePerKey > 1)
                {
                    rpData.TotalPage = totalPagePerKey;
                    for (int index = 2; index <= totalPagePerKey; index++)
                    {
                        addedPages.Add(
                            new SimpleQuotationDataReport()
                            {
                                FarePrice = rpData.FarePrice,
                                FareTax = rpData.FareTax,
                                TaxIncludePrice = rpData.TaxIncludePrice,
                                MitTotal = rpData.MitTotal,
                                TotalSyaRyoSyo = rpData.TotalSyaRyoSyo,
                                Total = rpData.Total,
                                GuiderCost = rpData.GuiderCost,
                                FutaiCost = rpData.FutaiCost,
                                TsumiCost = rpData.TsumiCost,
                                TehaiCost = rpData.TehaiCost,
                                TransportCost = rpData.TransportCost,

                                HeaderData = rpData.HeaderData,
                                TotalPage = totalPagePerKey,
                                CurrentPage = index,
                                FooterCarCountList = rpData.FooterCarCountList,
                                FooterData = rpData.FooterData,
                                BodyDataList = new List<SimpleQuotationDataReport.Body>(rpData.BodyDataList.Skip(footerReportRowPerPage).Take(footerReportRowPerPage)),
                                BodyJourneyDataList = new List<SimpleQuotationDataReport.BodyJourney>(rpData.BodyJourneyDataList.Skip(bodyReportRowPerPage).Take(bodyReportRowPerPage)),
                            }
                        );
                        rpData.BodyDataList.RemoveRange(footerReportRowPerPage, footerReportRowPerPage);
                        rpData.BodyJourneyDataList.RemoveRange(bodyReportRowPerPage, bodyReportRowPerPage);
                    }
                }
            }
            reportDataList.AddRange(addedPages);
            reportDataList = reportDataList.OrderBy(_ => _.HeaderData.UkeNo).ThenBy(_ => _.HeaderData.UnkRen).ToList();
        }

        public Dictionary<string, string> GetFieldValues(SimpleQuotationData data)
        {
            try
            {
                return new Dictionary<string, string>
                {
                    [nameof(data.UkeCdFrom)] = data.UkeCdFrom,
                    [nameof(data.UkeCdTo)] = data.UkeCdTo,
                    [nameof(data.StartPickupDate)] = data.StartPickupDate?.ToString("yyyyMMdd") ?? string.Empty,
                    [nameof(data.EndPickupDate)] = data.EndPickupDate?.ToString("yyyyMMdd") ?? string.Empty,
                    [nameof(data.StartArrivalDate)] = data.StartArrivalDate?.ToString("yyyyMMdd") ?? string.Empty,
                    [nameof(data.EndArrivalDate)] = data.EndArrivalDate?.ToString("yyyyMMdd") ?? string.Empty,
                    //[nameof(data.BookingTypeStart)] = data.BookingTypeStart?.YoyaKbnSeq.ToString() ?? string.Empty,
                    //[nameof(data.BookingTypeEnd)] = data.BookingTypeEnd?.YoyaKbnSeq.ToString() ?? string.Empty,
                    [nameof(data.YoyakuFrom)] = data.YoyakuFrom?.YoyaKbnSeq.ToString() ?? string.Empty,
                    [nameof(data.YoyakuTo)] = data.YoyakuTo?.YoyaKbnSeq.ToString() ?? string.Empty,
                    [nameof(data.BranchStart)] = data.BranchStart?.EigyoCdSeq.ToString() ?? string.Empty,
                    [nameof(data.BranchEnd)] = data.BranchEnd?.EigyoCdSeq.ToString() ?? string.Empty,
                    //[nameof(data.CustomerStart)] = data.CustomerStart != null ? $"{data.CustomerStart.TokuiSeq}-{data.CustomerStart.SitenCdSeq}" : string.Empty,
                    //[nameof(data.CustomerEnd)] = data.CustomerEnd != null ? $"{data.CustomerEnd.TokuiSeq}-{data.CustomerEnd.SitenCdSeq}" : string.Empty,
                    [nameof(data.GyosyaShiireSakiFrom)] = data.GyosyaShiireSakiFrom?.GyosyaCdSeq.ToString() ?? string.Empty,
                    [nameof(data.GyosyaShiireSakiTo)] = data.GyosyaShiireSakiTo?.GyosyaCdSeq.ToString() ?? string.Empty,
                    [nameof(data.TokiskShiireSakiFrom)] = data.TokiskShiireSakiFrom?.TokuiSeq.ToString() ?? string.Empty,
                    [nameof(data.TokiskShiireSakiTo)] = data.TokiskShiireSakiTo?.TokuiSeq.ToString() ?? string.Empty,
                    [nameof(data.TokiStShiireSakiFrom)] = data.TokiStShiireSakiFrom?.SitenCdSeq.ToString() ?? string.Empty,
                    [nameof(data.TokiStShiireSakiTo)] = data.TokiStShiireSakiTo?.SitenCdSeq.ToString() ?? string.Empty,

                    [nameof(data.ExportType)] = data.ExportType.ToString(),
                    [nameof(data.OutputOrientation)] = data.OutputOrientation.ToString(),
                    [nameof(data.Fare)] = Convert.ToByte(data.Fare).ToString()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new Dictionary<string, string>();
            }
        }

        public void ApplyFilter(ref SimpleQuotationData data, Dictionary<string, string> filterValues)
        {
            try
            {
                string outValueString = string.Empty;
                DateTime dt = new DateTime();
                var dataPropList = data
                    .GetType()
                    .GetProperties()
                    .Where(d => d.CanWrite && d.CanRead)
                    .ToList();
                foreach (var dataProp in dataPropList)
                {
                    if (filterValues.TryGetValue(dataProp.Name, out outValueString))
                    {
                        var type = typeof(Nullable<DateTime>);
                        var rootType = dataProp.PropertyType;
                        var isEqual = type == rootType;
                        if (dataProp.PropertyType != typeof(Nullable<DateTime>) && dataProp.PropertyType.IsGenericType || dataProp.PropertyType.IsClass && dataProp.PropertyType != typeof(string))
                        {
                            continue;
                        }
                        dynamic setValue = null;
                        if (dataProp.PropertyType.IsEnum)
                        {
                            setValue = Enum.Parse(dataProp.PropertyType, outValueString);
                        }
                        else if (dataProp.PropertyType == typeof(Nullable<DateTime>)
                            || dataProp.PropertyType == typeof(DateTime))
                        {
                            if (DateTime.TryParseExact(outValueString, "yyyyMMdd", null, DateTimeStyles.None, out dt))
                            {
                                setValue = dt;
                            }
                            else
                            {
                                setValue = null;
                            }
                        }
                        else if (dataProp.PropertyType == typeof(string))
                        {
                            setValue = outValueString;
                        }

                        dataProp.SetValue(data, setValue);
                    }
                }

                if (filterValues.ContainsKey(nameof(data.Fare)))
                {
                    data.Fare = int.TryParse(filterValues[nameof(data.Fare)], out int outValue) ? Convert.ToBoolean(outValue) : false;
                }
                //if (filterValues.ContainsKey(nameof(data.BookingTypeStart)))
                //{
                //    data.BookingTypeStart = int.TryParse(filterValues[nameof(data.BookingTypeStart)], out int outValue)
                //                ? new ReservationData() { YoyaKbnSeq = outValue }
                //                : default(ReservationData); 
                //}
                //if (filterValues.ContainsKey(nameof(data.BookingTypeEnd)))
                //{
                //    data.BookingTypeEnd = int.TryParse(filterValues[nameof(data.BookingTypeEnd)], out int outValue)
                //                ? new ReservationData() { YoyaKbnSeq = outValue }
                //                : default(ReservationData); 
                //}
                if (filterValues.ContainsKey(nameof(data.YoyakuFrom)))
                {
                    data.YoyakuFrom = int.TryParse(filterValues[nameof(data.YoyakuFrom)], out int outValue)
                                ? new ReservationClassComponentData() { YoyaKbnSeq = outValue }
                                : default(ReservationClassComponentData);
                }
                if (filterValues.ContainsKey(nameof(data.YoyakuTo)))
                {
                    data.YoyakuTo = int.TryParse(filterValues[nameof(data.YoyakuTo)], out int outValue)
                                ? new ReservationClassComponentData() { YoyaKbnSeq = outValue }
                                : default(ReservationClassComponentData);
                }
                if (filterValues.ContainsKey(nameof(data.BranchStart)))
                {
                    data.BranchStart = int.TryParse(filterValues[nameof(data.BranchStart)], out int outValue)
                                ? new LoadSaleBranch() { EigyoCdSeq = outValue }
                                : default(LoadSaleBranch);
                }
                if (filterValues.ContainsKey(nameof(data.BranchEnd)))
                {
                    data.BranchEnd = int.TryParse(filterValues[nameof(data.BranchEnd)], out int outValue)
                                ? new LoadSaleBranch() { EigyoCdSeq = outValue }
                                : default(LoadSaleBranch);
                }
                if (filterValues.ContainsKey(nameof(data.BranchEnd)))
                {
                    data.BranchEnd = int.TryParse(filterValues[nameof(data.BranchEnd)], out int outValue)
                                ? new LoadSaleBranch() { EigyoCdSeq = outValue }
                                : default(LoadSaleBranch);
                }

                if (filterValues.ContainsKey(nameof(data.GyosyaShiireSakiFrom)))
                {
                    data.GyosyaShiireSakiFrom = int.TryParse(filterValues[nameof(data.GyosyaShiireSakiFrom)], out int outValue)
                               ? new CustomerComponentGyosyaData() { GyosyaCdSeq = outValue }
                               : default(CustomerComponentGyosyaData);
                }

                if (filterValues.ContainsKey(nameof(data.GyosyaShiireSakiTo)))
                {
                    data.GyosyaShiireSakiTo = int.TryParse(filterValues[nameof(data.GyosyaShiireSakiTo)], out int outValue)
                               ? new CustomerComponentGyosyaData() { GyosyaCdSeq = outValue }
                               : default(CustomerComponentGyosyaData);
                }

                if (filterValues.ContainsKey(nameof(data.TokiskShiireSakiFrom)))
                {
                    data.TokiskShiireSakiFrom = int.TryParse(filterValues[nameof(data.TokiskShiireSakiFrom)], out int outValue)
                               ? new CustomerComponentTokiskData() { TokuiSeq = outValue }
                               : default(CustomerComponentTokiskData);
                }

                if (filterValues.ContainsKey(nameof(data.TokiskShiireSakiTo)))
                {
                    data.TokiskShiireSakiTo = int.TryParse(filterValues[nameof(data.TokiskShiireSakiTo)], out int outValue)
                               ? new CustomerComponentTokiskData() { TokuiSeq = outValue }
                               : default(CustomerComponentTokiskData);
                }

                if (filterValues.ContainsKey(nameof(data.TokiStShiireSakiFrom)))
                {
                    data.TokiStShiireSakiFrom = int.TryParse(filterValues[nameof(data.TokiStShiireSakiFrom)], out int outValue)
                               ? new CustomerComponentTokiStData() { SitenCdSeq = outValue }
                               : default(CustomerComponentTokiStData);
                }

                if (filterValues.ContainsKey(nameof(data.TokiStShiireSakiTo)))
                {
                    data.TokiStShiireSakiTo = int.TryParse(filterValues[nameof(data.TokiStShiireSakiTo)], out int outValue)
                               ? new CustomerComponentTokiStData() { SitenCdSeq = outValue }
                               : default(CustomerComponentTokiStData);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                data = new SimpleQuotationData(data.TenantId, data.UserLoginId);
            }
        }

        public async Task<bool> ExportAsPDF(SimpleQuotationReportFilter param, IJSRuntime JSRuntime)
        {
            if (param.BookingKeyList == null)
                return true;
            var rpData = await GetSimpleQuotationListReportAsync(param);
            if (rpData != null && rpData.Any())
            {
                var report = new Reports.ReportTemplate.SimpleQuotation.SimpleQuotation();
                report.DataSource = rpData;

                await new System.Threading.Tasks.TaskFactory().StartNew(() =>
                {
                    report.CreateDocument();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        report.ExportToPdf(ms);
                        byte[] exportedFileBytes = ms.ToArray();
                        string myExportString = Convert.ToBase64String(exportedFileBytes);
                        JSRuntime.InvokeVoidAsync("downloadFileClientSide", myExportString, "pdf", "SimpleQuotationReport");
                    }
                });
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<SimpleQuotationReportFilter> SetParamForSimpleQuotation(string UkeNo)
        {
            // For SimpleQuotation
            SimpleQuotationData FilterData = new SimpleQuotationData(new HassyaAllrightCloud.Domain.Dto.ClaimModel().TenantID, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq);
            FilterData.UkeCdFrom = UkeNo;
            FilterData.UkeCdTo = UkeNo;
            FilterData.StartArrivalDate = null;
            FilterData.EndArrivalDate = null;
            FilterData.StartPickupDate = null;
            FilterData.EndPickupDate = null;
            var bookingKeys = await GetBookingKeyListAsync(FilterData);
            SimpleQuotationReportFilter param = null;
            if (bookingKeys != null && bookingKeys.Any())
            {
                param = new SimpleQuotationReportFilter(bookingKeys, new HassyaAllrightCloud.Domain.Dto.ClaimModel().TenantID, FilterData.Fare);
            }
            return param;
        }
        public async Task<SimpleQuotationReportFilter> SetParamForQuotationJourney(string UkeNo)
        {
            // For SimpleQuotation
            SimpleQuotationData FilterData = new SimpleQuotationData(new HassyaAllrightCloud.Domain.Dto.ClaimModel().TenantID, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq);
            FilterData.UkeCdFrom = UkeNo;
            FilterData.UkeCdTo = UkeNo;
            FilterData.StartArrivalDate = null;
            FilterData.EndArrivalDate = null;
            FilterData.StartPickupDate = null;
            FilterData.EndPickupDate = null;
            var bookingKeys = await GetBookingKeyListAsync(FilterData);
            SimpleQuotationReportFilter param = null;
            if (bookingKeys != null && bookingKeys.Any())
            {
                 param = new SimpleQuotationReportFilter(
                        bookingKeys,
                        new HassyaAllrightCloud.Domain.Dto.ClaimModel().TenantID,
                        FilterData.Fare,
                        FilterData.OutputOrientation == OutputOrientation.Horizontal
                            ? QuotationReportType.JourneyHorizontal
                            : QuotationReportType.JourneyVertical
                    );
            }
            
            return param;
        }

        public async Task<bool> ExportQuotationJourneyAsPDF(SimpleQuotationReportFilter param, IJSRuntime JSRuntime)
        {
            try
            {
                if (param.BookingKeyList == null)
                    return true;
                DevExpress.XtraReports.UI.XtraReport report = await GetReport(param);

                await new System.Threading.Tasks.TaskFactory().StartNew(() =>
                {
                    report.CreateDocument();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        report.ExportToPdf(ms);
                        byte[] exportedFileBytes = ms.ToArray();
                        string myExportString = Convert.ToBase64String(exportedFileBytes);
                        JSRuntime.InvokeVoidAsync("downloadFileClientSide", myExportString, "pdf", "QuotationWithJourneyReport");
                    }
                });
                return false;
            }
            catch (Exception ex)
            {
                return true;
                throw ex;
            }
            
        }
    }
}