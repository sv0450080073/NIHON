using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Application.AccessoryFeeListReport.Query;
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
using HassyaAllrightCloud.IService.CommonComponents;

namespace HassyaAllrightCloud.IService
{
    public interface IAccessoryFeeListReportService : IReportService
    {
        Task<List<LoadFutaiType>> GetFutaiTypeByTenantIdAsync(int tenantId);

        Task<List<AccessoryFeeListReportData>> GetAccessoryFeeListReportAsync(AccessoryFeeListReportSearchParams prs);
        Task<List<AccessoryFeeListReportData>> GetAccessoryFeeListReportCsvAsync(AccessoryFeeListReportSearchParams prs);
        Task<List<AccessoryFeeListReportPagedData>> GetAccessoryFeeListReportPagedAsync(AccessoryFeeListReportSearchParams prs);
        Dictionary<string, string> GetFieldValues(AccessoryFeeListData data);
        void ApplyFilter(ref AccessoryFeeListData data, Dictionary<string, string> filterValues, List<ReservationClassComponentData> listReservation,
            List<CustomerComponentGyosyaData> listGyosya, List<CustomerComponentTokiskData> listTokisk, List<CustomerComponentTokiStData> listTokist);
    }

    public class AccessoryFeeListReportService : IAccessoryFeeListReportService
    {
        private readonly ITPM_CodeSyService _codeSyuService;
        private readonly IMediator _mediatR;
        private readonly IReservationClassComponentService _service;

        public AccessoryFeeListReportService(ITPM_CodeSyService codeSyuService, IMediator mediatR, IReservationClassComponentService service)
        {
            _codeSyuService = codeSyuService;
            _mediatR = mediatR;
            _service = service;
        }

        public async Task<List<LoadFutaiType>> GetFutaiTypeByTenantIdAsync(int tenantId)
        {
            return await _mediatR.Send(new GetLoadFutaiTypeQuery(_codeSyuService, tenantId));
        }

        public async Task<List<AccessoryFeeListReportData>> GetAccessoryFeeListReportAsync(AccessoryFeeListReportSearchParams prs)
        {
            return await _mediatR.Send(new GetAccessoryFeeListReportQuery(prs.SearchCondition, prs.TenantId, _codeSyuService));
        }

        public async Task<List<AccessoryFeeListReportPagedData>> GetAccessoryFeeListReportPagedAsync(AccessoryFeeListReportSearchParams prs)
        {
            var currentStaffLogin = await _mediatR.Send(new GetStaffByStaffIdQuery(prs.UserLoginId, prs.TenantId));
            var rpDatas = await GetAccessoryFeeListReportAsync(prs);

            return PaggingReportData(prs, rpDatas, currentStaffLogin.FirstOrDefault());
        }

        public async Task<List<AccessoryFeeListReportData>> GetAccessoryFeeListReportCsvAsync(AccessoryFeeListReportSearchParams prs)
        {
            var rpDatas = await GetAccessoryFeeListReportAsync(prs);

            foreach (var (item, index) in rpDatas.WithIndex())
            {
                item.No = index + 1;
            }

            return rpDatas;
        }

        public async Task<XtraReport> PreviewReport(string queryParams)
        {
            XtraReport report = new XtraReport();
            ObjectDataSource dataSource = new ObjectDataSource();
            var prviewParam = EncryptHelper.DecryptFromUrl<AccessoryFeeListReportSearchParams>(queryParams);
            var bookingTypes = await _service.GetListReservationClass();
            prviewParam.SearchCondition.BookingTypes = bookingTypes;
            if (prviewParam.SearchCondition.BookingTypeStart != null)
                prviewParam.SearchCondition.BookingTypes = prviewParam.SearchCondition.BookingTypes.Where(_ => _.YoyaKbn >= prviewParam.SearchCondition.BookingTypeStart.YoyaKbn).ToList();
            if (prviewParam.SearchCondition.BookingTypeEnd != null)
                prviewParam.SearchCondition.BookingTypes = prviewParam.SearchCondition.BookingTypes.Where(_ => _.YoyaKbn <= prviewParam.SearchCondition.BookingTypeEnd.YoyaKbn).ToList();

            switch (prviewParam.SearchCondition.PaperSize)
            {
                case PaperSize.A3:
                    report = new Reports.ReportTemplate.AccessoryFeeList.AccessoryFeeListA3();
                    break;
                case PaperSize.A4:
                    report = new Reports.ReportTemplate.AccessoryFeeList.AccessoryFeeListA4();
                    break;
                case PaperSize.B4:
                    report = new Reports.ReportTemplate.AccessoryFeeList.AccessoryFeeListB4();
                    break;
                default:
                    break;
            }

            var data = await GetAccessoryFeeListReportPagedAsync(prviewParam);

            Parameter param = new Parameter()
            {
                Name = "data",
                Type = typeof(List<AccessoryFeeListReportPagedData>),
                Value = data
            };
            dataSource.Name = "objectDataSource1";
            dataSource.DataSource = typeof(AccessoryFeeListReportDataSource);
            dataSource.Constructor = new ObjectConstructorInfo(param);
            dataSource.DataMember = "_data";
            report.DataSource = dataSource;
            return report;
        }

        #region Helper
        private List<AccessoryFeeListReportPagedData> PaggingReportData(AccessoryFeeListReportSearchParams prs, List<AccessoryFeeListReportData> reportDatas, LoadStaff currentStaff, int itemPerPage = 30)
        {
            if(reportDatas is null)
                throw new ArgumentNullException(nameof(reportDatas));

            var groupDisplay = GroupReportData(prs.SearchCondition, reportDatas, currentStaff);

            var paged = new List<AccessoryFeeListReportPagedData>();

            foreach (var item in groupDisplay)
            {
                var pageds = PagedList<AccessoryFeeListReportData>.GetAllPageds(item.ReportDatas.AsQueryable(), itemPerPage);

                foreach(var (page, index) in pageds.WithIndex())
                {
                    AccessoryFeeListReportPagedData newPage = new AccessoryFeeListReportPagedData();
                    newPage.SimpleCloneProperties(item);

                    foreach (var (itemPage, pi) in page.WithIndex()) // assign no for item
                    {
                        itemPage.No = ((index * 30) + pi + 1);
                    }

                    newPage.ReportDatas = page;

                    paged.Add(newPage);
                }

                if (pageds.Any())
                {
                    paged.ElementAt(paged.Count - 1).ReportDatas.AddRange(Enumerable.Repeat(new AccessoryFeeListReportData { IsEmptyObject = true }, itemPerPage - paged.ElementAt(paged.Count - 1).ReportDatas.Count()));
                }
            }

            return paged;
        }

        private List<AccessoryFeeListReportPagedData> GroupReportData(AccessoryFeeListData searchCondition, List<AccessoryFeeListReportData> reportDatas, LoadStaff currentStaff)
        {
            List<AccessoryFeeListReportPagedData> groupDisplay = new List<AccessoryFeeListReportPagedData>();
            string CustomerInfoStart = string.Empty;
            string CustomerInfoEnd = string.Empty;
            switch (searchCondition.BreakPage.Option)
            {
                case BreakReportPage.Customer:
                    if (searchCondition.SelectedGyosyaStart != null && searchCondition.SelectedTokiskStart != null && searchCondition.SelectedTokistStart != null)
                    {
                        CustomerInfoStart = $"{searchCondition.SelectedTokiskStart.TokuiCd:D4}-{searchCondition.SelectedTokistStart.SitenCd:D4}-{searchCondition.SelectedGyosyaStart.GyosyaCd:D3}：{searchCondition.SelectedTokiskStart.RyakuNm} {searchCondition.SelectedTokistStart.RyakuNm}";
                    }
                    if (searchCondition.SelectedGyosyaEnd != null && searchCondition.SelectedTokiskEnd != null && searchCondition.SelectedTokistEnd != null)
                    {
                        CustomerInfoStart = $"{searchCondition.SelectedTokiskEnd.TokuiCd:D4}-{searchCondition.SelectedTokistEnd.SitenCd:D4}-{searchCondition.SelectedGyosyaEnd.GyosyaCd:D3}：{searchCondition.SelectedTokiskEnd.RyakuNm} {searchCondition.SelectedTokistEnd.RyakuNm}";
                    }
                    return reportDatas.GroupBy(_ => new { _.TokuTokuiCd, _.ToshiSitenCd})
                                              .Select(_ => new AccessoryFeeListReportPagedData
                                              {
                                                  IsNormalPagging = false,
                                                  DateTypeSearchInfo = $"{searchCondition.DateTypeText}： {searchCondition.StartDate.ToString("yyyy/MM/dd")} ～ {searchCondition.EndDate.ToString("yyyy/MM/dd")}",
                                                  CustomerInfo = $"{CustomerInfoStart} ～ {CustomerInfoEnd}",
                                                  CompanyInfo = $"{searchCondition.Company?.CompanyInfo ?? string.Empty}",
                                                  PagingTypeInfo = $"{searchCondition.BreakPage.DisplayName}",
                                                  BookingTypeInfo = $"{searchCondition.BookingTypeStart?.YoyaKbnNm ?? string.Empty} ～ {searchCondition.BookingTypeEnd?.YoyaKbnNm ?? string.Empty}",
                                                  UkeCdInfo = $"{searchCondition.UkeCdFrom} ～ {searchCondition.UkeCdTo}",
                                                  BranchInfo = $"{searchCondition.BranchStart?.BranchText ?? string.Empty} ～ {searchCondition.BranchEnd?.BranchText ?? string.Empty}",
                                                  PrintedStaffCD = int.Parse(currentStaff.SyainCd).ToString("D10"),
                                                  PrintedStaffName = currentStaff.SyainNm,
                                                  PrintedStaffCompanyName = currentStaff.CompanyName,
                                                  ExportTypeInfo = searchCondition.ReportType.DisplayName,
                                                  InvoiceTypeInfo = searchCondition.InvoiceType.DisplayName,
                                                  FutaiInfo = $"{searchCondition.FutaiStart?.FutaiText ?? string.Empty} ～ {searchCondition.FutaiEnd?.FutaiText ?? string.Empty}",
                                                  FutaiTypeInfo = (searchCondition.FutaiFeeTypes.Count == 0 || searchCondition.FutaiFeeTypes.Any(_ => _.IsSelectedAll)) ? Constants.SelectedAll : searchCondition.FutaiFeeTypes.Where(_ => !_.IsSelectedAll).Select(_ => _.CodeKbnNm).Aggregate((result, item) => result + "; " + item),
                                                  ReportDatas = _.Concat(new List<AccessoryFeeListReportData> {
                                                      new AccessoryFeeListReportData
                                                      {
                                                          SumSuryo = _.ToList().Sum(_=> int.Parse(_.Suryo)),
                                                          SumUriGakKin = _.ToList().Sum(_ => int.Parse(_.UriGakKin)),
                                                          SumSyohizei = _.ToList().Sum(_ => int.Parse(_.SyaRyoSyo)),
                                                          SumTesu = _.ToList().Sum(_ => int.Parse(_.SyaRyoTes)),
                                                          IsSumRow = true
                                                      }
                                                  }).ToList(), //this step sum all record and add sum row to last of the list
                                              }).ToList();
                case BreakReportPage.None:
                    if (searchCondition.SelectedGyosyaStart != null && searchCondition.SelectedTokiskStart != null && searchCondition.SelectedTokistStart != null)
                    {
                        CustomerInfoStart = $"{searchCondition.SelectedTokiskStart.TokuiCd.ToString("D4")}：{searchCondition.SelectedTokiskStart.TokuiNm} {searchCondition.SelectedTokistStart.SitenCd.ToString("D4")}：{searchCondition.SelectedTokistStart.SitenNm}";
                    }
                    if (searchCondition.SelectedGyosyaEnd != null && searchCondition.SelectedTokiskEnd != null && searchCondition.SelectedTokistEnd != null)
                    {
                        CustomerInfoStart = $"{searchCondition.SelectedTokiskEnd.TokuiCd.ToString("D4")}：{searchCondition.SelectedTokiskEnd.TokuiNm} {searchCondition.SelectedTokistEnd.SitenCd.ToString("D4")}：{searchCondition.SelectedTokistEnd.SitenNm}";
                    }
                    return new List<AccessoryFeeListReportPagedData>
                    {
                        new AccessoryFeeListReportPagedData
                        {
                            IsNormalPagging = true,
                            DateTypeSearchInfo = $"{searchCondition.DateTypeText}： {searchCondition.StartDate.ToString("yyyy/MM/dd")} ～ {searchCondition.EndDate.ToString("yyyy/MM/dd")}",
                            CustomerInfo = $"{CustomerInfoStart} ～ {CustomerInfoStart}",
                            CompanyInfo = $"{searchCondition.Company?.CompanyInfo ?? string.Empty}",
                            PagingTypeInfo = $"{searchCondition.BreakPage.DisplayName}",
                            BookingTypeInfo = $"{searchCondition.BookingTypeStart?.YoyaKbnNm ?? string.Empty} ～ {searchCondition.BookingTypeEnd?.YoyaKbnNm ?? string.Empty}",
                            UkeCdInfo = $"{searchCondition.UkeCdFrom} ～ {searchCondition.UkeCdTo}",
                            BranchInfo = $"{searchCondition.BranchStart?.BranchText ?? string.Empty} ～ {searchCondition.BranchEnd?.BranchText ?? string.Empty}",
                            PrintedStaffCD = int.Parse(currentStaff.SyainCd).ToString("D10"),
                            PrintedStaffName = currentStaff.SyainNm,
                            PrintedStaffCompanyName = currentStaff.CompanyName,
                            ExportTypeInfo = searchCondition.ReportType.DisplayName,
                            InvoiceTypeInfo = searchCondition.InvoiceType.DisplayName,
                            FutaiInfo = $"{searchCondition.FutaiStart?.FutaiText ?? string.Empty} ～ {searchCondition.FutaiEnd?.FutaiText ?? string.Empty}",
                            FutaiTypeInfo = (searchCondition.FutaiFeeTypes.Count == 0 || searchCondition.FutaiFeeTypes.Any(_ => _.IsSelectedAll)) ? Constants.SelectedAll : searchCondition.FutaiFeeTypes.Where(_ => !_.IsSelectedAll).Select(_ => _.CodeKbnNm).Aggregate((result, item) => result + "; " + item),
                            ReportDatas = reportDatas.Concat(new List<AccessoryFeeListReportData> { 
                                new AccessoryFeeListReportData 
                                { 
                                    SumSuryo = reportDatas.ToList().Sum(_=> int.Parse(_.Suryo)), 
                                    SumUriGakKin = reportDatas.ToList().Sum(_ => int.Parse(_.UriGakKin)), 
                                    SumSyohizei = reportDatas.ToList().Sum(_ => int.Parse(_.SyaRyoSyo)),
                                    SumTesu = reportDatas.ToList().Sum(_ => int.Parse(_.SyaRyoTes)),
                                    IsSumRow = true 
                                } 
                            }).ToList(), //this step sum all record and add sum row to last of the list
                        }
                    };
                default:
                    throw new ArgumentOutOfRangeException(nameof(searchCondition.BreakPage.Option));
            }
        }

        #endregion

        public Dictionary<string, string> GetFieldValues(AccessoryFeeListData data)
        {
            return new Dictionary<string, string>
            {
                [nameof(data.UkeCdFrom)] = data.UkeCdFrom,
                [nameof(data.UkeCdTo)] = data.UkeCdTo,
                [nameof(data.StartDate)] = data.StartDate.ToString("yyyyMMdd"),
                [nameof(data.EndDate)] = data.EndDate.ToString("yyyyMMdd"),
                [nameof(data.DateType)] = data.DateType.ToString(),
                //[nameof(data.BookingTypes)] = string.Join('-', data.BookingTypes.Select(_ => _.YoyaKbnSeq)),
                [nameof(data.BookingTypeStart)] = data.BookingTypeStart?.YoyaKbnSeq.ToString() ?? string.Empty,
                [nameof(data.BookingTypeEnd)] = data.BookingTypeEnd?.YoyaKbnSeq.ToString() ?? string.Empty,
                [nameof(data.FutaiFeeTypes)] = string.Join('-', data.FutaiFeeTypes.Select(_ => _.CodeKbnSeq)),
                [nameof(data.Company)] = data.Company?.CompanyCdSeq.ToString() ?? string.Empty,
                [nameof(data.BreakPage)] = data.BreakPage.Option.ToString(),
                [nameof(data.BranchStart)] = data.BranchStart?.EigyoCdSeq.ToString() ?? string.Empty,
                [nameof(data.BranchEnd)] = data.BranchEnd?.EigyoCdSeq.ToString() ?? string.Empty,
                [nameof(data.SelectedGyosyaStart)] = data.SelectedGyosyaStart?.GyosyaCdSeq.ToString() ?? string.Empty,
                [nameof(data.SelectedGyosyaEnd)] = data.SelectedGyosyaEnd?.GyosyaCdSeq.ToString() ?? string.Empty,
                [nameof(data.SelectedTokiskStart)] = data.SelectedTokiskStart?.TokuiSeq.ToString() ?? string.Empty,
                [nameof(data.SelectedTokiskEnd)] = data.SelectedTokiskEnd?.TokuiSeq.ToString() ?? string.Empty,
                [nameof(data.SelectedTokistStart)] = data.SelectedTokistStart?.SitenCdSeq.ToString() ?? string.Empty,
                [nameof(data.SelectedTokistEnd)] = data.SelectedTokistEnd?.SitenCdSeq.ToString() ?? string.Empty,
                [nameof(data.FutaiStart)] = data.FutaiStart?.FutaiCdSeq.ToString() ?? string.Empty,
                [nameof(data.FutaiEnd)] = data.FutaiEnd?.FutaiCdSeq.ToString() ?? string.Empty,
                [nameof(data.ExportType)] = data.ExportType.ToString(),
                [nameof(data.PaperSize)] = data.PaperSize.ToString(),
                [nameof(data.InvoiceType)] = data.InvoiceType.Option.ToString(),
                [nameof(data.ReportType)] = data.ReportType.Option.ToString(),
                [nameof(data.CsvConfigOption.Header)] = data.CsvConfigOption.Header.Option.ToString(),
                [nameof(data.CsvConfigOption.GroupSymbol)] = data.CsvConfigOption.GroupSymbol.Option.ToString(),
                [nameof(data.CsvConfigOption.Delimiter)] = data.CsvConfigOption.Delimiter.Option.ToString(),
                [nameof(data.CsvConfigOption.DelimiterSymbol)] = data.CsvConfigOption.DelimiterSymbol ?? string.Empty,
            };
        }

        public void ApplyFilter(ref AccessoryFeeListData data, Dictionary<string, string> filterValues, List<ReservationClassComponentData> listReservation,
            List<CustomerComponentGyosyaData> listGyosya, List<CustomerComponentTokiskData> listTokisk, List<CustomerComponentTokiStData> listTokist)
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

            //var yoyaKbnSeqList = string.IsNullOrEmpty(filterValues[nameof(data.BookingTypes)]) ? null : filterValues[nameof(data.BookingTypes)].Split('-').Select(_ => int.Parse(_));
            //data.BookingTypes = yoyaKbnSeqList?.Select(_ => new ReservationData() { YoyaKbnSeq = _, IsSelectedAll = _ == 0 }).ToList() ?? new List<ReservationData>();
            var futGuiKbns = string.IsNullOrEmpty(filterValues[nameof(data.FutaiFeeTypes)]) ? null : filterValues[nameof(data.FutaiFeeTypes)].Split('-').Select(_ => int.Parse(_));
            data.FutaiFeeTypes = futGuiKbns?.Select(_ => new LoadFutaiType { CodeKbnSeq = _, IsSelectedAll = _ == 0 } ).ToList() ?? new List<LoadFutaiType>();
            if (int.TryParse(filterValues[nameof(data.BookingTypeStart)], out int outValue))
            {
                data.BookingTypeStart = listReservation.FirstOrDefault(_ => _.YoyaKbnSeq == outValue);
            }
            if (int.TryParse(filterValues[nameof(data.BookingTypeEnd)], out outValue))
            {
                data.BookingTypeEnd = listReservation.FirstOrDefault(_ => _.YoyaKbnSeq == outValue);
            }
            if (int.TryParse(filterValues[nameof(data.Company)], out  outValue))
            {
                data.Company = new CompanyData() { CompanyCdSeq = outValue };
            }
            if (int.TryParse(filterValues[nameof(data.BranchStart)], out outValue))
            {
                data.BranchStart = new LoadSaleBranch() { EigyoCdSeq = outValue };
            }
            if (int.TryParse(filterValues[nameof(data.BranchEnd)], out outValue))
            {
                data.BranchEnd = new LoadSaleBranch() { EigyoCdSeq = outValue };
            }

            if (int.TryParse(filterValues[nameof(data.SelectedGyosyaStart)], out outValue))
            {
                data.SelectedGyosyaStart = listGyosya.FirstOrDefault(_ => _.GyosyaCdSeq == outValue);
            }
            if (int.TryParse(filterValues[nameof(data.SelectedGyosyaEnd)], out outValue))
            {
                data.SelectedGyosyaEnd = listGyosya.FirstOrDefault(_ => _.GyosyaCdSeq == outValue);
            }

            if (int.TryParse(filterValues[nameof(data.SelectedTokiskStart)], out outValue))
            {
                var gyosyaCdSeq = data.SelectedGyosyaStart.GyosyaCdSeq;
                data.SelectedTokiskStart = listTokisk.FirstOrDefault(_ => _.GyosyaCdSeq == gyosyaCdSeq && _.TokuiSeq == outValue);
            }
            if (int.TryParse(filterValues[nameof(data.SelectedTokiskEnd)], out outValue))
            {
                var gyosyaCdSeq = data.SelectedGyosyaEnd.GyosyaCdSeq;
                data.SelectedTokiskEnd = listTokisk.FirstOrDefault(_ => _.GyosyaCdSeq == gyosyaCdSeq && _.TokuiSeq == outValue);
            }

            if (int.TryParse(filterValues[nameof(data.SelectedTokistStart)], out outValue))
            {
                var tokuiSeq = data.SelectedTokiskStart.TokuiSeq;
                data.SelectedTokistStart = listTokist.FirstOrDefault(_ => _.TokuiSeq == tokuiSeq && _.SitenCdSeq == outValue);
            }
            if (int.TryParse(filterValues[nameof(data.SelectedTokistEnd)], out outValue))
            {
                var tokuiSeq = data.SelectedTokiskEnd.TokuiSeq;
                data.SelectedTokistEnd = listTokist.FirstOrDefault(_ => _.TokuiSeq == tokuiSeq && _.SitenCdSeq == outValue);
            }

            if (int.TryParse(filterValues[nameof(data.FutaiStart)], out outValue))
            {
                data.FutaiStart = new LoadFutai() { FutaiCdSeq = outValue, IsSelectedAll = (outValue == 0) };
            }
            if (int.TryParse(filterValues[nameof(data.FutaiEnd)], out outValue))
            {
                data.FutaiEnd = new LoadFutai() { FutaiCdSeq = outValue, IsSelectedAll = (outValue == 0)  };
            }

            data.InvoiceType = new SelectedOption<InvoiceTypeOption>() { Option = Enum.Parse<InvoiceTypeOption>(filterValues[nameof(data.InvoiceType)]) };
            data.BreakPage = new SelectedOption<BreakReportPage>() { Option = Enum.Parse<BreakReportPage>(filterValues[nameof(data.BreakPage)]) };
            data.ReportType = new SelectedOption<ReportType>() { Option = Enum.Parse<ReportType>(filterValues[nameof(data.ReportType)]) };

            data.CsvConfigOption.Header = new SelectedOption<CSV_Header>() { Option = Enum.Parse<CSV_Header>(filterValues[nameof(data.CsvConfigOption.Header)]) };
            data.CsvConfigOption.GroupSymbol = new SelectedOption<CSV_Group>() { Option = Enum.Parse<CSV_Group>(filterValues[nameof(data.CsvConfigOption.GroupSymbol)]) };
            data.CsvConfigOption.Delimiter = new SelectedOption<CSV_Delimiter>() { Option = Enum.Parse<CSV_Delimiter>(filterValues[nameof(data.CsvConfigOption.Delimiter)]) };
            data.CsvConfigOption.DelimiterSymbol = filterValues[nameof(data.CsvConfigOption.DelimiterSymbol)];
        }
    }
}
