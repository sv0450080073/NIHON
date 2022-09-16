using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Application.CancelListReport.Queries;
using HassyaAllrightCloud.Application.Staff.Queries;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Reports.DataSource;
using MediatR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
namespace HassyaAllrightCloud.IService
{
    public interface ICancelListReportService : IReportService
    {
        Task<List<CancelListSearchData>> SearchCancelListByCondition(CancelListData searchOption, int tenantId);
        Task<List<CancelListReportData>> GetCancelListReport(CancelListReportSearchParams prs);
        Task<List<CancelListReportPagedData>> GetCancelListReportPaged(CancelListReportSearchParams prs);
        Dictionary<string, string> GetFieldValues(CancelListData data);
        void ApplyFilter(ref CancelListData data, Dictionary<string, string> filterValues, List<ReservationClassComponentData> BookingTypes, List<CustomerComponentGyosyaData> ListGyosya, List<CustomerComponentTokiskData> TokiskData, List<CustomerComponentTokiStData> TokiStData);
    }

    public class CancelListReportService : ICancelListReportService
    {
        private readonly IMediator _mediatR;
        private readonly ITPM_CodeSyService _codeSyService;
        public CancelListReportService(IMediator mediatR, ITPM_CodeSyService codeSyService)
        {
            _mediatR = mediatR;
            _codeSyService = codeSyService;
        }

        public async Task<List<CancelListSearchData>> SearchCancelListByCondition(CancelListData searchOption, int tenantId)
        {
            return await _mediatR.Send(new SearchCancelBookingQuery(searchOption, _codeSyService, tenantId));
        }

        public async Task<List<CancelListReportPagedData>> GetCancelListReportPaged(CancelListReportSearchParams prs)
        {
            var currentStaffLogin = await _mediatR.Send(new GetStaffByStaffIdQuery(prs.UserLoginId, prs.TenantId));
            var reportData = await _mediatR.Send(new GetCancelListReportQuery(prs.BookingKeys, prs.TenantId, _codeSyService));

            return PaggingReportData(prs, reportData, prs.SearchCondition.BreakPage.Option, currentStaffLogin.FirstOrDefault(), 18);
        }

        public async Task<List<CancelListReportData>> GetCancelListReport(CancelListReportSearchParams prs)
        {
            var reportData = await _mediatR.Send(new GetCancelListReportQuery(prs.BookingKeys, prs.TenantId, _codeSyService));

            if (reportData.Any())
            {
                var handleRepeat = (HandleRepeatItem(reportData).Select(_ => _.Item3)).Aggregate((result, item) => result.Concat(item).ToList());

                return ReOrderReportData(handleRepeat, prs.SearchCondition.Sort, BreakReportPage.None);
            }

            return new List<CancelListReportData>();
        }

        public async Task<XtraReport> PreviewReport(string queryParams)
        {
            XtraReport report = new XtraReport();
            ObjectDataSource dataSource = new ObjectDataSource();
            var prviewParam = EncryptHelper.DecryptFromUrl<CancelListReportSearchParams>(queryParams);

            switch (prviewParam.SearchCondition.PaperSize)
            {
                case PaperSize.A3:
                    report = new Reports.ReportTemplate.Kyanseru.KyanseruA3();
                    break;
                case PaperSize.A4:
                    report = new Reports.ReportTemplate.Kyanseru.Kyanseru();
                    break;
                case PaperSize.B4:
                    report = new Reports.ReportTemplate.Kyanseru.KyanseruB4();
                    break;
                default:
                    break;
            }

            var data = await GetCancelListReportPaged(prviewParam);
            Parameter param = new Parameter()
            {
                Name = "data",
                Type = typeof(List<CancelListReportPagedData>),
                Value = data
            };
            dataSource.Name = "objectDataSource1";
            dataSource.DataSource = typeof(CancelListReportDataSource);
            dataSource.Constructor = new ObjectConstructorInfo(param);
            dataSource.DataMember = "_data";
            report.DataSource = dataSource;
            return report;
        }


        #region Helper
        private List<CancelListReportPagedData> PaggingReportData(CancelListReportSearchParams prs, List<CancelListReportData> reportDatas, BreakReportPage breakMode, LoadStaff currentStaff, int itemPerPage = 20)
        {
            if (reportDatas is null)
                throw new ArgumentNullException(nameof(reportDatas));

            var searchCondition = prs.SearchCondition;

            reportDatas = ReOrderReportData(reportDatas, prs.SearchCondition.Sort, breakMode);

            var groupDisplay = GroupReportData(prs.SearchCondition, reportDatas, breakMode, currentStaff);

            var paged = new List<CancelListReportPagedData>();
            bool isFirst = true;

            foreach (var item in groupDisplay)
            {
                var grouped = HandleRepeatItem(item.ReportDatas);

                isFirst = true;
                foreach (var uke in grouped)
                {
                    int space = 0;

                    if (!isFirst)
                    {
                        space = itemPerPage - paged.ElementAt(paged.Count - 1).ReportDatas.Count();
                    }

                    if (space >= uke.Item3.Count())
                    {
                        paged.ElementAt(paged.Count - 1).ReportDatas.AddRange(uke.Item3);
                    }
                    else
                    {
                        if (!isFirst) //add empty record in group list
                        {
                            paged.ElementAt(paged.Count - 1).ReportDatas.AddRange(Enumerable.Repeat(new CancelListReportData() { IsEmptyObject = true }, space));
                        }
                        isFirst = false;

                        for (int pageIndex = 1; ; pageIndex++)
                        {
                            var pagedItem = PagedList<CancelListReportData>.ToPagedList(uke.Item3.AsQueryable(), pageIndex, itemPerPage);

                            int currentPageCount = pagedItem.Count;
                            if (currentPageCount == itemPerPage)
                            {
                                var newPage = new CancelListReportPagedData();
                                newPage.SimpleCloneProperties(item);
                                newPage.ReportDatas = pagedItem;

                                paged.Add(newPage);
                            }
                            else
                            {
                                if (currentPageCount == 0)
                                {
                                    break;
                                }
                                else
                                {
                                    var newPage = new CancelListReportPagedData();
                                    newPage.SimpleCloneProperties(item);
                                    newPage.ReportDatas = pagedItem;

                                    paged.Add(newPage);
                                    break;
                                }
                            }
                        }
                    }

                    var recordValids = paged.Last()?.ReportDatas?.Where(_ => !_.IsEmptyObject && !_.IsReplaceItem) ?? new List<CancelListReportData>();
                    paged.ElementAt(paged.Count - 1).PageSummary = new PageSummaryData
                    {
                        SumCancelFee = recordValids.Sum(d => int.Parse(d.CancelFee)).ToString(),
                        SumCancelTax = recordValids.Sum(d => int.Parse(d.CancelTax)).ToString(),
                        SumBusFee = recordValids.Sum(d => int.Parse(d.UntKin)).ToString(),
                        SumBusTax = recordValids.Sum(d => int.Parse(d.ZeiRui)).ToString(),
                        SumBusCharge = recordValids.Sum(d => int.Parse(d.TesuRyoG)).ToString(),
                    };
                }
                //add empty record in last page in group list
                paged.ElementAt(paged.Count - 1).ReportDatas.AddRange(Enumerable.Repeat(new CancelListReportData() { IsEmptyObject = true }, itemPerPage - paged.ElementAt(paged.Count - 1).ReportDatas.Count));
            }

            return paged;
        }

        private List<CancelListReportData> ReOrderReportData(List<CancelListReportData> reportDatas, SortCancel order, BreakReportPage breakMode)
        {
            switch (breakMode)
            {
                case BreakReportPage.Customer:
                    switch (order)
                    {
                        case SortCancel.Customer:
                            return reportDatas;
                        case SortCancel.CancellationDate:
                            return reportDatas.OrderBy(_ => _.TokuTokuiCd)
                                             .ThenBy(_ => _.CancelYmd)
                                             .ThenBy(_ => _.UkeCd)
                                             .ThenBy(_ => _.UnkRen)
                                             .AsEnumerable()
                                             .ToList();
                        case SortCancel.VehicleDeliveryDate:
                            return reportDatas.OrderBy(_ => _.TokuTokuiCd)
                                             .ThenBy(_ => _.HaiSYmd)
                                             .ThenBy(_ => _.UkeCd)
                                             .ThenBy(_ => _.UnkRen)
                                             .AsEnumerable()
                                             .ToList();
                        default:
                            throw new ArgumentOutOfRangeException(nameof(breakMode));
                    }
                case BreakReportPage.None:
                    switch (order)
                    {
                        case SortCancel.Customer:
                            return reportDatas.OrderBy(_ => _.TokuTokuiCd)
                                              .ThenBy(_ => _.UkeCd)
                                              .ThenBy(_ => _.UnkRen)
                                              .AsEnumerable()
                                              .ToList();
                        case SortCancel.CancellationDate:
                            return reportDatas.OrderBy(_ => _.CancelYmd)
                                             .ThenBy(_ => _.UkeCd)
                                             .ThenBy(_ => _.UnkRen)
                                             .AsEnumerable()
                                             .ToList();
                        case SortCancel.VehicleDeliveryDate:
                            return reportDatas.OrderBy(_ => _.HaiSYmd)
                                             .ThenBy(_ => _.UkeCd)
                                             .ThenBy(_ => _.UnkRen)
                                             .AsEnumerable()
                                             .ToList();
                        default:
                            throw new ArgumentOutOfRangeException(nameof(breakMode));
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(breakMode));
            }
        }

        private List<CancelListReportPagedData> GroupReportData(CancelListData searchCondition, List<CancelListReportData> reportDatas, BreakReportPage breakMode, LoadStaff currentStaff)
        {
            List<CancelListReportPagedData> groupDisplay = new List<CancelListReportPagedData>();
            switch (breakMode)
            {
                case BreakReportPage.Customer:
                    return reportDatas.GroupBy(_ => new { _.TokuTokuiCd, _.ToshiSitenCd })
                                              .Select(_ => new CancelListReportPagedData
                                              {
                                                  IsNormalPagging = false,
                                                  CancelDateTypeDetail = $"{searchCondition.DateTypeText}： {searchCondition.StartDate.ToString("yyyy/MM/dd")} ～ {searchCondition.EndDate.ToString("yyyy/MM/dd")}",
                                                  CustomerInfo = $"{($"{searchCondition.GyosyaTokuiSakiFrom.GyosyaCd.ToString("D3")} : {searchCondition.GyosyaTokuiSakiFrom.GyosyaNm} {searchCondition.TokiskTokuiSakiFrom.TokuiCd.ToString("D4")} : {searchCondition.TokiskTokuiSakiFrom.RyakuNm} {searchCondition.TokiStTokuiSakiFrom.SitenCd.ToString("D4")} : {searchCondition.TokiStTokuiSakiFrom.RyakuNm}")} ～ {($"{searchCondition.GyosyaTokuiSakiTo.GyosyaCd.ToString("D3")} : {searchCondition.GyosyaTokuiSakiTo.GyosyaNm} {searchCondition.TokiskTokuiSakiTo.TokuiCd.ToString("D4")} : {searchCondition.TokiskTokuiSakiTo.RyakuNm} {searchCondition.TokiStTokuiSakiTo.SitenCd.ToString("D4")} : {searchCondition.TokiStTokuiSakiTo.RyakuNm}")}",
                                                  CompanyInfo = $"{searchCondition.Company.CompanyInfo}",
                                                  SortInfo = $"{searchCondition.SortText}",
                                                  PaggingTypeInfo = $"{searchCondition.BreakPage.DisplayName}",
                                                  BookingTypeInfo = $"{(searchCondition.YoyakuFrom == null ? "" : searchCondition.YoyakuFrom.YoyaKbnNm)} ～ {(searchCondition.YoyakuTo == null ? "" : searchCondition.YoyakuTo.YoyaKbnNm)}",
                                                  UkeCdInfo = $"{searchCondition.UkeCdFrom} ～ {searchCondition.UkeCdTo}",
                                                  StaffInfo = $"{searchCondition.StaffStart.StaffText} ～ {searchCondition.StaffEnd.StaffText}",
                                                  CancelStaffInfo = $"{searchCondition.CancelStaffStart.StaffText} ～ {searchCondition.CancelStaffEnd.StaffText}",
                                                  CancelFeeInfo = searchCondition.CancelCharge.DisplayName,
                                                  BranchInfo = $"{searchCondition.BranchStart.BranchText} ～ {searchCondition.BranchEnd.BranchText}",
                                                  PrintedStaffCD = int.Parse(currentStaff.SyainCd).ToString("D10"),
                                                  PrintedStaffName = currentStaff.SyainNm,
                                                  ReportDatas = _.ToList(),
                                                  AllPageSummary = new PageSummaryData
                                                  {
                                                      SumCancelFee = _.Distinct().Sum(d => int.Parse(d.CancelFee)).ToString(),
                                                      SumCancelTax = _.Distinct().Sum(d => int.Parse(d.CancelTax)).ToString(),
                                                      SumBusFee = _.Distinct().Sum(d => int.Parse(d.UntKin)).ToString(),
                                                      SumBusTax = _.Distinct().Sum(d => int.Parse(d.ZeiRui)).ToString(),
                                                      SumBusCharge = _.Distinct().Sum(d => int.Parse(d.TesuRyoG)).ToString(),
                                                  }
                                              }).ToList();
                case BreakReportPage.None:
                    return new List<CancelListReportPagedData>
                    {
                        new CancelListReportPagedData
                        {
                            IsNormalPagging = true,
                            CancelDateTypeDetail = $"{searchCondition.DateTypeText}： {searchCondition.StartDate.ToString("yyyy/MM/dd")} ～ {searchCondition.EndDate.ToString("yyyy/MM/dd")}",
                            CustomerInfo = $"{($"{searchCondition.GyosyaTokuiSakiFrom.GyosyaCd.ToString("D3")} : {searchCondition.GyosyaTokuiSakiFrom.GyosyaNm} {searchCondition.TokiskTokuiSakiFrom.TokuiCd.ToString("D4")} : {searchCondition.TokiskTokuiSakiFrom.RyakuNm} {searchCondition.TokiStTokuiSakiFrom.SitenCd.ToString("D4")} : {searchCondition.TokiStTokuiSakiFrom.RyakuNm}")} ～ {($"{searchCondition.GyosyaTokuiSakiTo.GyosyaCd.ToString("D3")} : {searchCondition.GyosyaTokuiSakiTo.GyosyaNm} {searchCondition.TokiskTokuiSakiTo.TokuiCd.ToString("D4")} : {searchCondition.TokiskTokuiSakiTo.RyakuNm} {searchCondition.TokiStTokuiSakiTo.SitenCd.ToString("D4")} : {searchCondition.TokiStTokuiSakiTo.RyakuNm}")}",
                            CompanyInfo = $"{searchCondition.Company.CompanyInfo}",
                            SortInfo = $"{searchCondition.SortText}",
                            PaggingTypeInfo = $"{searchCondition.BreakPage.DisplayName}",
                            BookingTypeInfo = $"{(searchCondition.YoyakuFrom == null ?  "" :searchCondition.YoyakuFrom.YoyaKbnNm)} ～ {(searchCondition.YoyakuTo == null ?  "" :searchCondition.YoyakuTo.YoyaKbnNm)}",
                            UkeCdInfo = $"{searchCondition.UkeCdFrom} ～ {searchCondition.UkeCdTo}",
                            StaffInfo = $"{searchCondition.StaffStart.StaffText} ～ {searchCondition.StaffEnd.StaffText}",
                            CancelStaffInfo = $"{searchCondition.CancelStaffStart.StaffText} ～ {searchCondition.CancelStaffEnd.StaffText}",
                            CancelFeeInfo = searchCondition.CancelCharge.DisplayName,
                            BranchInfo = $"{searchCondition.BranchStart.BranchText} ～ {searchCondition.BranchEnd.BranchText}",
                            PrintedStaffCD = int.Parse(currentStaff.SyainCd).ToString("D10"),
                            PrintedStaffName = currentStaff.SyainNm,
                            ReportDatas = reportDatas,
                            AllPageSummary = new PageSummaryData
                            {
                                SumCancelFee = reportDatas.Distinct().Sum(d => int.Parse(d.CancelFee)).ToString(),
                                SumCancelTax = reportDatas.Distinct().Sum(d => int.Parse(d.CancelTax)).ToString(),
                                SumBusFee = reportDatas.Distinct().Sum(d => int.Parse(d.UntKin)).ToString(),
                                SumBusTax = reportDatas.Distinct().Sum(d => int.Parse(d.ZeiRui)).ToString(),
                                SumBusCharge = reportDatas.Distinct().Sum(d => int.Parse(d.TesuRyoG)).ToString(),
                            }
                        }
                    };
                default:
                    throw new ArgumentOutOfRangeException(nameof(breakMode));
            }
        }

        private List<Tuple<string, int, List<CancelListReportData>>> HandleRepeatItem(List<CancelListReportData> reportDatas)
        {
            var grouped = reportDatas
                    .GroupBy(_ => new { _.UkeCd, _.UnkRen })
                    .OrderBy(_ => _.Key.UkeCd)
                    .ThenBy(_ => _.Key.UnkRen)
                    .Select(_ => new
                    {
                        UkeCd = _.Key.UkeCd,
                        UnkRen = _.Key.UnkRen,
                        Data = _.ToList()
                    }).ToList();

            grouped.ForEach(item =>
            {
                item.Data.ForEach(_ => {
                    _.IsReplaceItem = true;
                });

                item.Data.First().IsReplaceItem = false;
            });

            return grouped.Select(_=> new Tuple<string, int, List<CancelListReportData>>(_.UkeCd, _.UnkRen, _.Data)).ToList();
        }
        #endregion

        public Dictionary<string, string> GetFieldValues(CancelListData data)
        {
            return new Dictionary<string, string>
            {
                [nameof(data.UkeCdFrom)] = data.UkeCdFrom,
                [nameof(data.UkeCdTo)] = data.UkeCdTo,
                [nameof(data.StartDate)] = data.StartDate.ToString("yyyyMMdd"),
                [nameof(data.EndDate)] = data.EndDate.ToString("yyyyMMdd"),
                [nameof(data.DateType)] = data.DateType.ToString(),
                [nameof(data.Sort)] = data.Sort.ToString(),
                [nameof(data.YoyakuFrom)] = data.YoyakuFrom.YoyaKbnSeq.ToString(),
                [nameof(data.YoyakuTo)] = data.YoyakuTo.YoyaKbnSeq.ToString(),
                [nameof(data.Company)] = data.Company.CompanyCdSeq.ToString(),
                [nameof(data.CancelBookingType)] = data.CancelBookingType.CodeKb_CodeKbnSeq.ToString(),
                [nameof(data.CancelCharge)] = data.CancelCharge.Option.ToString(),
                [nameof(data.BreakPage)] = data.BreakPage.Option.ToString(),
                [nameof(data.BranchStart)] = data.BranchStart?.EigyoCdSeq.ToString() ?? "",
                [nameof(data.BranchEnd)] = data.BranchEnd?.EigyoCdSeq.ToString() ?? "",
                [nameof(data.GyosyaTokuiSakiFrom)] = data.GyosyaTokuiSakiFrom != null ? data.GyosyaTokuiSakiFrom.GyosyaCdSeq.ToString() : "0",
                [nameof(data.GyosyaTokuiSakiTo)] = data.GyosyaTokuiSakiTo != null ? data.GyosyaTokuiSakiTo.GyosyaCdSeq.ToString() : "0",
                [nameof(data.TokiskTokuiSakiFrom)] = data.TokiskTokuiSakiFrom != null ? data.TokiskTokuiSakiFrom.TokuiSeq.ToString() : "0",
                [nameof(data.TokiskTokuiSakiTo)] = data.TokiskTokuiSakiTo  != null ? data.TokiskTokuiSakiTo.TokuiSeq.ToString(): "0",
                [nameof(data.TokiStTokuiSakiFrom)] = data.TokiStTokuiSakiFrom != null ? data.TokiStTokuiSakiFrom.SitenCdSeq.ToString(): "0",
                [nameof(data.TokiStTokuiSakiTo)] = data.TokiStTokuiSakiTo  != null ? data.TokiStTokuiSakiTo.SitenCdSeq.ToString(): "0",
                [nameof(data.GyosyaShiireSakiFrom)] = data.GyosyaShiireSakiFrom != null ? data.GyosyaShiireSakiFrom.GyosyaCdSeq.ToString(): "0",
                [nameof(data.GyosyaShiireSakiTo)] = data.GyosyaShiireSakiTo  != null ? data.GyosyaShiireSakiTo.GyosyaCdSeq.ToString(): "0",
                [nameof(data.TokiskShiireSakiFrom)] = data.TokiskShiireSakiFrom  != null ? data.TokiskShiireSakiFrom.TokuiSeq.ToString(): "0",
                [nameof(data.TokiskShiireSakiTo)] = data.TokiskShiireSakiTo  != null ? data.TokiskShiireSakiTo.TokuiSeq.ToString(): "0",
                [nameof(data.TokiStShiireSakiFrom)] = data.TokiStShiireSakiFrom  != null ? data.TokiStShiireSakiFrom.SitenCdSeq.ToString(): "0",
                [nameof(data.TokiStShiireSakiTo)] = data.TokiStShiireSakiTo  != null ? data.TokiStShiireSakiTo.SitenCdSeq.ToString(): "0",
                [nameof(data.StaffStart)] = data.StaffStart.SyainCdSeq.ToString(),
                [nameof(data.StaffEnd)] = data.StaffEnd.SyainCdSeq.ToString(),
                [nameof(data.CancelStaffStart)] = data.CancelStaffStart.SyainCdSeq.ToString(),
                [nameof(data.CancelStaffEnd)] = data.CancelStaffEnd.SyainCdSeq.ToString(),
                [nameof(data.ExportType)] = data.ExportType.ToString(),
                [nameof(data.PaperSize)] = data.PaperSize.ToString(),
                [nameof(data.CsvConfigOption.Header)] = data.CsvConfigOption.Header.Option.ToString(),
                [nameof(data.CsvConfigOption.GroupSymbol)] = data.CsvConfigOption.GroupSymbol.Option.ToString(),
                [nameof(data.CsvConfigOption.Delimiter)] = data.CsvConfigOption.Delimiter.Option.ToString(),
                [nameof(data.CsvConfigOption.DelimiterSymbol)] = data.CsvConfigOption.DelimiterSymbol ?? string.Empty,
                [nameof(data.Size)] = data.Size.ToString(),
            };
        }

        public void ApplyFilter(ref CancelListData data, Dictionary<string, string> filterValues, List<ReservationClassComponentData> BookingTypes, List<CustomerComponentGyosyaData> ListGyosya, List<CustomerComponentTokiskData> TokiskData, List<CustomerComponentTokiStData> TokiStData)
        {
            CustomerComponentGyosyaData GyosyaTokuiFrom = new CustomerComponentGyosyaData();
            CustomerComponentGyosyaData GyosyaTokuiTo = new CustomerComponentGyosyaData();
            CustomerComponentTokiskData TokiskTokuiFrom = new CustomerComponentTokiskData();
            CustomerComponentTokiskData TokiskTokuiTo = new CustomerComponentTokiskData();

            CustomerComponentGyosyaData GyosyaShiireFrom = new CustomerComponentGyosyaData();
            CustomerComponentGyosyaData GyosyaShiireTo = new CustomerComponentGyosyaData();
            CustomerComponentTokiskData TokiskShiireFrom = new CustomerComponentTokiskData();
            CustomerComponentTokiskData TokiskShiireTo = new CustomerComponentTokiskData();
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
                    if (dataProp.PropertyType.IsGenericType || dataProp.PropertyType.IsClass && dataProp.PropertyType != typeof(string))
                    {
                        continue;
                    }
                    dynamic setValue = null;
                    if (dataProp.PropertyType.IsEnum)
                    {
                        setValue = Enum.Parse(dataProp.PropertyType, outValueString);
                    }
                    else if (dataProp.PropertyType == typeof(DateTime))
                    {
                        if (DateTime.TryParseExact(outValueString, "yyyyMMdd", null, DateTimeStyles.None, out dt))
                        {
                            setValue = dt;
                        }
                    }
                    else if (dataProp.PropertyType == typeof(string))
                    {
                        setValue = outValueString;
                    }

                    dataProp.SetValue(data, setValue);
                }
            }

            if (filterValues.ContainsKey(nameof(data.YoyakuFrom)))
            {
                if (int.TryParse(filterValues[nameof(data.YoyakuFrom)], out int outValue))
                {
                    data.YoyakuFrom = BookingTypes.Where(x => x.YoyaKbnSeq == outValue).FirstOrDefault();
                } 
            }
            if (filterValues.ContainsKey(nameof(data.YoyakuTo)))
            {
                if (int.TryParse(filterValues[nameof(data.YoyakuTo)], out int outValue))
                {
                    data.YoyakuTo = BookingTypes.Where(x => x.YoyaKbnSeq == outValue).FirstOrDefault();
                } 
            }

            if (filterValues.ContainsKey(nameof(data.Company)))
            {
                if (int.TryParse(filterValues[nameof(data.Company)], out int outValue))
                {
                    data.Company = new CompanyData() { CompanyCdSeq = outValue };
                } 
            }
            if (filterValues.ContainsKey(nameof(data.CancelBookingType)))
            {
                if (int.TryParse(filterValues[nameof(data.CancelBookingType)], out int outValue))
                {
                    data.CancelBookingType = new TPM_CodeKbData() { CodeKb_CodeKbnSeq = outValue };
                } 
            }
            if (filterValues.ContainsKey(nameof(data.BranchStart)))
            {
                if (int.TryParse(filterValues[nameof(data.BranchStart)], out int outValue))
                {
                    data.BranchStart = new LoadSaleBranch() { EigyoCdSeq = outValue };
                } 
            }
            if (filterValues.ContainsKey(nameof(data.BranchEnd)))
            {
                if (int.TryParse(filterValues[nameof(data.BranchEnd)], out int outValue))
                {
                    data.BranchEnd = new LoadSaleBranch() { EigyoCdSeq = outValue };
                } 
            }
            if (filterValues.ContainsKey(nameof(data.GyosyaTokuiSakiFrom)))
            {
                if (int.TryParse(filterValues[nameof(data.GyosyaTokuiSakiFrom)], out int outValue))
                {
                    data.GyosyaTokuiSakiFrom = ListGyosya.FirstOrDefault(_ => _.GyosyaCdSeq == outValue);
                    GyosyaTokuiFrom = ListGyosya.FirstOrDefault(_ => _.GyosyaCdSeq == outValue);
                }
                else
                {
                    data.GyosyaTokuiSakiFrom = null;
                }
            }
            if (filterValues.ContainsKey(nameof(data.GyosyaTokuiSakiTo)))
            {
                if (int.TryParse(filterValues[nameof(data.GyosyaTokuiSakiTo)], out int outValue))
                {
                    data.GyosyaTokuiSakiTo = ListGyosya.FirstOrDefault(_ => _.GyosyaCdSeq == outValue);
                    GyosyaTokuiTo = ListGyosya.FirstOrDefault(_ => _.GyosyaCdSeq == outValue);
                }
                else
                {
                    data.GyosyaTokuiSakiTo = null;
                }
            }
            if (data.GyosyaTokuiSakiFrom != null && filterValues.ContainsKey(nameof(data.TokiskTokuiSakiFrom)))
            {
                if (int.TryParse(filterValues[nameof(data.TokiskTokuiSakiFrom)], out int outValue))
                {
                    List<CustomerComponentTokiskData> LstTokisk = new List<CustomerComponentTokiskData>();
                    LstTokisk = TokiskData.Where(_ => _.GyosyaCdSeq == (GyosyaTokuiFrom?.GyosyaCdSeq ?? -1)).ToList();
                    data.TokiskTokuiSakiFrom = LstTokisk.FirstOrDefault(_ => _.TokuiSeq == outValue);
                    TokiskTokuiFrom = LstTokisk.FirstOrDefault(_ => _.TokuiSeq == outValue);
                }
                else
                {
                    data.TokiskTokuiSakiFrom = null;
                }
            }
            if (data.GyosyaTokuiSakiFrom != null && filterValues.ContainsKey(nameof(data.TokiskTokuiSakiTo)))
            {
                if (int.TryParse(filterValues[nameof(data.TokiskTokuiSakiTo)], out int outValue))
                {
                    List<CustomerComponentTokiskData> LstTokisk = new List<CustomerComponentTokiskData>();
                    LstTokisk = TokiskData.Where(_ => _.GyosyaCdSeq == (GyosyaTokuiTo?.GyosyaCdSeq ?? -1)).ToList();
                    data.TokiskTokuiSakiTo = LstTokisk.FirstOrDefault(_ => _.TokuiSeq == outValue);
                    TokiskTokuiTo = LstTokisk.FirstOrDefault(_ => _.TokuiSeq == outValue);
                }
                else
                {
                    data.TokiskTokuiSakiTo = null;
                }
            }
            if (filterValues.ContainsKey(nameof(data.TokiStTokuiSakiFrom)) && data.TokiskTokuiSakiFrom != null)
            {
                if (int.TryParse(filterValues[nameof(data.TokiStTokuiSakiFrom)], out int outValue))
                {
                    List<CustomerComponentTokiStData> LstTokiSt = new List<CustomerComponentTokiStData>();
                    LstTokiSt = TokiStData.Where(_ => _.TokuiSeq == (TokiskTokuiFrom?.TokuiSeq ?? -1)).ToList();
                    data.TokiStTokuiSakiFrom = LstTokiSt.FirstOrDefault(_ => _.SitenCdSeq == outValue);
                }
                else
                {
                    data.TokiStTokuiSakiFrom = null;
                }
            }
            if (filterValues.ContainsKey(nameof(data.TokiStTokuiSakiTo)) && data.TokiskTokuiSakiTo != null)
            {
                if (int.TryParse(filterValues[nameof(data.TokiStTokuiSakiTo)], out int outValue))
                {
                    List<CustomerComponentTokiStData> LstTokiSt = new List<CustomerComponentTokiStData>();
                    LstTokiSt = TokiStData.Where(_ => _.TokuiSeq == (TokiskTokuiTo?.TokuiSeq ?? -1)).ToList();
                    data.TokiStTokuiSakiTo = LstTokiSt.FirstOrDefault(_ => _.SitenCdSeq == outValue);
                }
                else
                {
                    data.TokiStTokuiSakiTo = null;
                }
            }
            if (filterValues.ContainsKey(nameof(data.GyosyaShiireSakiFrom)))
            {
                if (int.TryParse(filterValues[nameof(data.GyosyaShiireSakiFrom)], out int outValue))
                {
                    data.GyosyaShiireSakiFrom = ListGyosya.FirstOrDefault(_ => _.GyosyaCdSeq == outValue);
                    GyosyaShiireFrom = ListGyosya.FirstOrDefault(_ => _.GyosyaCdSeq == outValue);
                }
                else
                {
                    data.GyosyaShiireSakiFrom = null;
                }
            }
            if (filterValues.ContainsKey(nameof(data.GyosyaShiireSakiTo)))
            {
                if (int.TryParse(filterValues[nameof(data.GyosyaShiireSakiTo)], out int outValue))
                {
                    data.GyosyaShiireSakiTo = ListGyosya.FirstOrDefault(_ => _.GyosyaCdSeq == outValue);
                    GyosyaShiireTo = ListGyosya.FirstOrDefault(_ => _.GyosyaCdSeq == outValue);
                }
                else
                {
                    data.GyosyaShiireSakiTo = null;
                }
            }
            if (data.GyosyaShiireSakiFrom != null && filterValues.ContainsKey(nameof(data.TokiskShiireSakiFrom)))
            {
                if (int.TryParse(filterValues[nameof(data.TokiskShiireSakiFrom)], out int outValue))
                {
                    List<CustomerComponentTokiskData> LstTokisk = new List<CustomerComponentTokiskData>();
                    LstTokisk = TokiskData.Where(_ => _.GyosyaCdSeq == (GyosyaShiireFrom?.GyosyaCdSeq ?? -1)).ToList();
                    data.TokiskShiireSakiFrom = LstTokisk.FirstOrDefault(_ => _.TokuiSeq == outValue);
                    TokiskShiireFrom = LstTokisk.FirstOrDefault(_ => _.TokuiSeq == outValue);
                }
                else
                {
                    data.TokiskShiireSakiFrom = null;
                }
            }
            if (data.GyosyaShiireSakiFrom != null && filterValues.ContainsKey(nameof(data.TokiskShiireSakiTo)))
            {
                if (int.TryParse(filterValues[nameof(data.TokiskShiireSakiTo)], out int outValue))
                {
                    List<CustomerComponentTokiskData> LstTokisk = new List<CustomerComponentTokiskData>();
                    LstTokisk = TokiskData.Where(_ => _.GyosyaCdSeq == (GyosyaShiireTo?.GyosyaCdSeq ?? -1)).ToList();
                    data.TokiskShiireSakiTo = LstTokisk.FirstOrDefault(_ => _.TokuiSeq == outValue);
                    TokiskShiireTo = LstTokisk.FirstOrDefault(_ => _.TokuiSeq == outValue);
                }
                else
                {
                    data.TokiskShiireSakiTo = null;
                }
            }
            if (filterValues.ContainsKey(nameof(data.TokiStShiireSakiFrom)) && data.TokiskShiireSakiFrom != null)
            {
                if (int.TryParse(filterValues[nameof(data.TokiStShiireSakiFrom)], out int outValue))
                {
                    List<CustomerComponentTokiStData> LstTokiSt = new List<CustomerComponentTokiStData>();
                    LstTokiSt = TokiStData.Where(_ => _.TokuiSeq == (TokiskShiireFrom?.TokuiSeq ?? -1)).ToList();
                    data.TokiStShiireSakiFrom = LstTokiSt.FirstOrDefault(_ => _.SitenCdSeq == outValue);
                }
                else
                {
                    data.TokiStShiireSakiFrom = null;
                }
            }
            if (filterValues.ContainsKey(nameof(data.TokiStShiireSakiTo)) && data.TokiskShiireSakiTo != null)
            {
                if (int.TryParse(filterValues[nameof(data.TokiStShiireSakiTo)], out int outValue))
                {
                    List<CustomerComponentTokiStData> LstTokiSt = new List<CustomerComponentTokiStData>();
                    LstTokiSt = TokiStData.Where(_ => _.TokuiSeq == (TokiskShiireTo?.TokuiSeq ?? -1)).ToList();
                    data.TokiStShiireSakiTo = LstTokiSt.FirstOrDefault(_ => _.SitenCdSeq == outValue);
                }
                else
                {
                    data.TokiStShiireSakiTo = null;
                }
            }
            if (filterValues.ContainsKey(nameof(data.StaffStart)))
            {
                if (int.TryParse(filterValues[nameof(data.StaffStart)], out int outValue))
                {
                    data.StaffStart = new LoadStaff() { SyainCdSeq = outValue };
                } 
            }
            if (filterValues.ContainsKey(nameof(data.StaffEnd)))
            {
                if (int.TryParse(filterValues[nameof(data.StaffEnd)], out int outValue))
                {
                    data.StaffEnd = new LoadStaff() { SyainCdSeq = outValue };
                } 
            }
            if (filterValues.ContainsKey(nameof(data.CancelStaffStart)))
            {
                if (int.TryParse(filterValues[nameof(data.CancelStaffStart)], out int outValue))
                {
                    data.CancelStaffStart = new LoadStaff() { SyainCdSeq = outValue };
                } 
            }
            if (filterValues.ContainsKey(nameof(data.CancelStaffEnd)))
            {
                if (int.TryParse(filterValues[nameof(data.CancelStaffEnd)], out int outValue))
                {
                    data.CancelStaffEnd = new LoadStaff() { SyainCdSeq = outValue };
                } 
            }
            if (filterValues.ContainsKey(nameof(data.Size)))
            {
                if (int.TryParse(filterValues[nameof(data.Size)], out int outValue))
                {
                    data.Size = outValue;
                } 
            }

            if (filterValues.ContainsKey(nameof(data.CancelCharge)))
            {
                data.CancelCharge = new SelectedOption<ConfirmAction>() { Option = Enum.Parse<ConfirmAction>(filterValues[nameof(data.CancelCharge)]) }; 
            }
            if (filterValues.ContainsKey(nameof(data.BreakPage)))
            {
                data.BreakPage = new SelectedOption<BreakReportPage>() { Option = Enum.Parse<BreakReportPage>(filterValues[nameof(data.BreakPage)]) }; 
            }

            if (filterValues.ContainsKey(nameof(data.CsvConfigOption.Header)))
            {
                data.CsvConfigOption.Header = new SelectedOption<CSV_Header>() { Option = Enum.Parse<CSV_Header>(filterValues[nameof(data.CsvConfigOption.Header)]) }; 
            }
            if (filterValues.ContainsKey(nameof(data.CsvConfigOption.GroupSymbol)))
            {
                data.CsvConfigOption.GroupSymbol = new SelectedOption<CSV_Group>() { Option = Enum.Parse<CSV_Group>(filterValues[nameof(data.CsvConfigOption.GroupSymbol)]) }; 
            }
            if (filterValues.ContainsKey(nameof(data.CsvConfigOption.Delimiter)))
            {
                data.CsvConfigOption.Delimiter = new SelectedOption<CSV_Delimiter>() { Option = Enum.Parse<CSV_Delimiter>(filterValues[nameof(data.CsvConfigOption.Delimiter)]) }; 
            }
            if (filterValues.ContainsKey(nameof(data.CsvConfigOption.DelimiterSymbol)))
            {
                data.CsvConfigOption.DelimiterSymbol = filterValues[nameof(data.CsvConfigOption.DelimiterSymbol)]; 
            }
        }
    }
}
