using DevExpress.Blazor.Internal;
using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Application.StatusConfirmationReport.Queries;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using HassyaAllrightCloud.Reports.DataSource;
using MediatR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IStatusConfirmationService : IReportService
    {
        /// <summary>
        /// Get list status confirm for check list
        /// </summary>
        /// <param name="searchOption">Option for filter list</param>
        /// <returns>List of <see cref="BookingKeyData"/></returns>
        Task<List<BookingKeyData>> GetStatusConfirmCheckListAsync(StatusConfirmationData searchOption);
        /// <summary>
        /// Search status confirmation by tenantId
        /// </summary>
        /// <param name="searchOption">Option for filter list</param>
        /// <returns>List of <see cref="StatusConfirmSearchResultData"/></returns>
        Task<StatusConfirmationPagedSearch> SearchStatusConfirmDataByTenanIdAsync(
            StatusConfirmationData searchOption,
            int tenantId,
            int skip,
            byte take
        );
        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectedList"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        Task<List<StatusConfirmationReportData>> GetStatusConfirmationReportInfoAsync(List<BookingKeyData> selectedList, int tenantId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectedList"></param>
        /// <param name="tenantId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="itemPerPage"></param>
        /// <returns></returns>
        Task<List<StatusConfirmationReportPaged>> GetStatusConfirmationReportPagedInfoAsync(List<BookingKeyData> selectedList, int tenantId, DateTime startDate, DateTime endDate, int itemPerPage = 20);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportData"></param>
        /// <returns></returns>
        Dictionary<string, string> GetFieldValues(StatusConfirmationData reportData);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportData"></param>
        /// <param name="filterValues"></param>
        StatusConfirmationData ApplyFilter(Dictionary<string, string> filterValues, List<SelectedOption<CSV_Header>> CsvHeaderOptions
            , List<SelectedOption<ConfirmStatus>> ConfirmStatusOptions, List<SelectedOption<NumberOfConfirmed>> ConfirmedTimeOptions
            , List<LoadSaleBranch> Branches , List<CompanyData> Companies, List<CustomerComponentGyosyaData> ListGyosya
            , List<CustomerComponentTokiskData> TokiskData, List<CustomerComponentTokiStData> TokiStData
            , List<SelectedOption<CSV_Group>> CsvGroupSymbolOptions, List<SelectedOption<CSV_Delimiter>> CsvDelimiterOptions);
    }

    public class StatusConfirmationService : IStatusConfirmationService
    {
        private readonly IMediator _mediatR;
        private readonly ITPM_CodeSyService _codeSyService;

        public StatusConfirmationService(IMediator mediatR, 
            ITPM_CodeSyService codeSyService)
        {
            _mediatR = mediatR;
            _codeSyService = codeSyService;
        }

        public async Task<List<StatusConfirmationReportData>> GetStatusConfirmationReportInfoAsync(List<BookingKeyData> selectedList, int tenantId)
        {
            return await _mediatR.Send(new GetStatusConfirmationReportDataQuery(selectedList, tenantId));
        }

        public async Task<List<StatusConfirmationReportPaged>> GetStatusConfirmationReportPagedInfoAsync(List<BookingKeyData> selectedList, int tenantId, DateTime startDate, DateTime endDate, int itemPerPage = 20)
        {
            //DateTime reportedDate = DateTime.Now;
            List<StatusConfirmationReportData> reportResult = await GetStatusConfirmationReportInfoAsync(selectedList, tenantId);

            var groupDisplay = reportResult
                .GroupBy(_ => new { _.TokiskTokuiCd, _.TokiStSitenCd, _.EigyoCd })
                .Select(_ => new StatusConfirmationReportPaged
                {
                    SitenRyakuNm = _?.First()?.SitenRyakuNm,
                    TokuiRyakuNm = _?.First()?.TokuiRyakuNm,
                    EigyosRyakuNm = _?.First()?.EigyosRyakuNm,
                    //ReportDate = reportedDate.ToString("yyyy/MM/dd HH:mm"),
                    StartDate = startDate.ToString("yyyy/MM/dd"),
                    EndDate = endDate.ToString("yyyy/MM/dd"),
                    PagedData = _.ToList()
                }).ToList();

            var paged = new List<StatusConfirmationReportPaged>();

            foreach (var item in groupDisplay)
            {
                var result = HandleRepeatItem(item.PagedData);

                for (int pageIndex = 1; ; pageIndex++)
                {
                    var pagedItem = PagedList<StatusConfirmationReportData>.ToPagedList(result.AsQueryable(), pageIndex, itemPerPage);

                    int currentPageCount = pagedItem.Count;
                    if (currentPageCount == itemPerPage)
                    {
                        var newPage = new StatusConfirmationReportPaged();
                        newPage.SimpleCloneProperties(item);
                        newPage.PagedData = pagedItem;

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
                            var newPage = new StatusConfirmationReportPaged();
                            newPage.SimpleCloneProperties(item);
                            newPage.PagedData = pagedItem;

                            paged.Add(newPage);
                            break;
                        }
                    }
                }
            }

            return paged;

            ReportData HandleRepeatItem<ReportData>(ReportData reportData) 
                where ReportData : List<StatusConfirmationReportData>
            {
                var result = reportData
                  .GroupBy(_ => new { _.UkeNo, _.UnkRen })
                  .OrderBy(_ => _.Key.UkeNo)
                  .ThenBy(_ => _.Key.UnkRen)
                  .Select(_ => new
                  {
                      UkeNo = _.Key.UkeNo,
                      UnkRen = _.Key.UnkRen,
                      Data = _.ToList()
                  }).ToList();

                var finalResult = new List<StatusConfirmationReportData>();

                result.ForEach(item =>
                {
                    item.Data.ForEach(_ =>
                    {
                        _.IsReplaceItem = true;
                    });

                    item.Data.First().IsReplaceItem = false;
                    finalResult.AddRange(item.Data);
                });

                return (ReportData)finalResult;
            }
        }

        public async Task<List<BookingKeyData>> GetStatusConfirmCheckListAsync(StatusConfirmationData searchOption)
        {
            return await _mediatR.Send(new GetStatusConfirmCheckListQuery(searchOption));
        }

        public async Task<XtraReport> PreviewReport(string queryParams)
        {
            XtraReport report = new XtraReport();
            ObjectDataSource dataSource = new ObjectDataSource();
            var prviewParam = EncryptHelper.DecryptFromUrl<StatusConfirmationPreviewReportParam>(queryParams);

            switch (prviewParam.PaperSize)
            {
                case Commons.Constants.PaperSize.A3:
                    report = new Reports.ReportTemplate.KakuninListReport.KakuninListReportA3();
                    break;
                case Commons.Constants.PaperSize.A4:
                    report = new Reports.ReportTemplate.KakuninListReport.KakuninList();
                    break;
                case Commons.Constants.PaperSize.B4:
                    report = new Reports.ReportTemplate.KakuninListReport.KakuninListReportB4();
                    break;
                default:
                    break;
            }

            var data = await GetStatusConfirmationReportPagedInfoAsync(prviewParam.SelectedList, prviewParam.TenantId, prviewParam.StartDate, prviewParam.EndDate, prviewParam.ItemPerPage);
            Parameter param = new Parameter()
            {
                Name = "data",
                Type = typeof(List<StatusConfirmationReportPaged>),
                Value = data
            };
            dataSource.Name = "objectDataSource3";
            dataSource.DataSource = typeof(StatusConfirmationReportDataSource);
            dataSource.Constructor = new ObjectConstructorInfo(param);
            dataSource.DataMember = "_data";
            report.DataSource = dataSource;
            return report;
        }

        public async Task<StatusConfirmationPagedSearch> SearchStatusConfirmDataByTenanIdAsync(
            StatusConfirmationData searchOption,
            int tenantId,
            int skip,
            byte take)
        {
            return await _mediatR.Send(new SearchStatusConfirmationsByTenanIdQuery(_codeSyService, searchOption, tenantId, skip, take));
        }

        public Dictionary<string, string> GetFieldValues(StatusConfirmationData confirmData)
        {
            return new Dictionary<string, string>
            {
                [nameof(confirmData.StartDate)] = confirmData.StartDate.ToString("yyyyMMdd"),
                [nameof(confirmData.EndDate)] = confirmData.EndDate.ToString("yyyyMMdd"),
                [nameof(confirmData.SelectedCompany)] = confirmData.SelectedCompany.CompanyCdSeq.ToString(),
                [nameof(confirmData.BranchStart)] = confirmData.BranchStart.EigyoCdSeq.ToString(),
                [nameof(confirmData.BranchEnd)] = confirmData.BranchEnd.EigyoCdSeq.ToString(),
                [nameof(confirmData.GyosyaTokuiSakiFrom)] = confirmData.GyosyaTokuiSakiFrom != null ? confirmData.GyosyaTokuiSakiFrom.GyosyaCdSeq.ToString() : "0",
                [nameof(confirmData.GyosyaTokuiSakiTo)] = confirmData.GyosyaTokuiSakiTo != null ? confirmData.GyosyaTokuiSakiTo.GyosyaCdSeq.ToString() : "0",
                [nameof(confirmData.TokiskTokuiSakiFrom)] = confirmData.TokiskTokuiSakiFrom != null ? confirmData.TokiskTokuiSakiFrom.TokuiSeq.ToString() : "0",
                [nameof(confirmData.TokiskTokuiSakiTo)] = confirmData.TokiskTokuiSakiTo != null ? confirmData.TokiskTokuiSakiTo.TokuiSeq.ToString() : "0",
                [nameof(confirmData.TokiStTokuiSakiFrom)] = confirmData.TokiStTokuiSakiFrom != null ? confirmData.TokiStTokuiSakiFrom.SitenCdSeq.ToString() : "0",
                [nameof(confirmData.TokiStTokuiSakiTo)] = confirmData.TokiStTokuiSakiTo != null ? confirmData.TokiStTokuiSakiTo.SitenCdSeq.ToString() : "0",
                [nameof(confirmData.ConfirmedStatus)] = confirmData.ConfirmedStatus.ToString(),
                [nameof(confirmData.FixedStatus)] = confirmData.FixedStatus.ToString(),
                [nameof(confirmData.ConfirmedTimes)] = confirmData.ConfirmedTimes.Option.ToString(),
                [nameof(confirmData.Saikou)] = confirmData.Saikou.Option.ToString(),
                [nameof(confirmData.SumDai)] = confirmData.SumDai.Option.ToString(),
                [nameof(confirmData.Ammount)] = confirmData.Ammount.Option.ToString(),
                [nameof(confirmData.ScheduleDate)] = confirmData.ScheduleDate.Option.ToString(),
                [nameof(confirmData.ExportType)] = confirmData.ExportType.ToString(),
                [nameof(confirmData.PaperSize)] = confirmData.PaperSize.ToString(),
                [nameof(confirmData.CsvConfigOption.Header)] = confirmData.CsvConfigOption.Header.Option.ToString(),
                [nameof(confirmData.CsvConfigOption.GroupSymbol)] = confirmData.CsvConfigOption.GroupSymbol.Option.ToString(),
                [nameof(confirmData.CsvConfigOption.Delimiter)] = confirmData.CsvConfigOption.Delimiter.Option.ToString(),
                [nameof(confirmData.CsvConfigOption.DelimiterSymbol)] = confirmData.CsvConfigOption.DelimiterSymbol ?? string.Empty,
                [nameof(confirmData.Size)] = confirmData.Size.ToString(),
            };
        }

        public StatusConfirmationData ApplyFilter(Dictionary<string, string> filterValues, List<SelectedOption<CSV_Header>> CsvHeaderOptions
            , List<SelectedOption<ConfirmStatus>> ConfirmStatusOptions, List<SelectedOption<NumberOfConfirmed>> ConfirmedTimeOptions
            , List<LoadSaleBranch> Branches, List<CompanyData> Companies, List<CustomerComponentGyosyaData> ListGyosya
            , List<CustomerComponentTokiskData> TokiskData, List<CustomerComponentTokiStData> TokiStData
            , List<SelectedOption<CSV_Group>> CsvGroupSymbolOptions, List<SelectedOption<CSV_Delimiter>> CsvDelimiterOptions)
        {
            StatusConfirmationData confirmData = new StatusConfirmationData();
            CustomerComponentGyosyaData GyoSyaFrom = new CustomerComponentGyosyaData();
            CustomerComponentGyosyaData GyoSyaTo = new CustomerComponentGyosyaData();
            CustomerComponentTokiskData TokuiFrom = new CustomerComponentTokiskData();
            CustomerComponentTokiskData TokuiTo = new CustomerComponentTokiskData();
            string outValueString = string.Empty;
            DateTime dt = new DateTime();
            var dataPropList = confirmData
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

                    dataProp.SetValue(confirmData, setValue);
                }
            }

            if (filterValues.ContainsKey(nameof(confirmData.Size)))
            {
                if (int.TryParse(filterValues[nameof(confirmData.Size)], out int outValue))
                {
                    confirmData.Size = outValue;
                }
            }
            if (filterValues.ContainsKey(nameof(confirmData.SelectedCompany)))
            {
                if (int.TryParse(filterValues[nameof(confirmData.SelectedCompany)], out int outValue))
                {
                    confirmData.SelectedCompany = Companies.FirstOrDefault(_ => _ != null && _.CompanyCdSeq != 0 && _.CompanyCdSeq == outValue);
                }
                else
                {
                    confirmData.SelectedCompany = null;
                }
            }
            if (filterValues.ContainsKey(nameof(confirmData.BranchStart)))
            {
                if (int.TryParse(filterValues[nameof(confirmData.BranchStart)], out int outValue))
                {
                    confirmData.BranchStart = Branches.FirstOrDefault(_ => _ != null && _.EigyoCdSeq != 0 && _.EigyoCdSeq == outValue);
                }
                else
                {
                    confirmData.BranchStart = null;
                }
            }
            if (filterValues.ContainsKey(nameof(confirmData.BranchEnd)))
            {
                if (int.TryParse(filterValues[nameof(confirmData.BranchEnd)], out int outValue))
                {
                    confirmData.BranchEnd = Branches.FirstOrDefault(_ => _ != null && _.EigyoCdSeq != 0 && _.EigyoCdSeq == outValue);
                }
                else
                {
                    confirmData.BranchStart = null;
                }
            }
            if (filterValues.ContainsKey(nameof(confirmData.GyosyaTokuiSakiFrom)))
            {
                if (int.TryParse(filterValues[nameof(confirmData.GyosyaTokuiSakiFrom)], out int outValue))
                {
                    confirmData.GyosyaTokuiSakiFrom = ListGyosya.FirstOrDefault(_ => _.GyosyaCdSeq == outValue);
                    GyoSyaFrom = ListGyosya.FirstOrDefault(_ => _.GyosyaCdSeq == outValue);
                }
                else
                {
                    confirmData.GyosyaTokuiSakiFrom = null;
                }
            }
            if (filterValues.ContainsKey(nameof(confirmData.GyosyaTokuiSakiTo)))
            {
                if (int.TryParse(filterValues[nameof(confirmData.GyosyaTokuiSakiTo)], out int outValue))
                {
                    confirmData.GyosyaTokuiSakiTo = ListGyosya.FirstOrDefault(_ => _.GyosyaCdSeq == outValue);
                    GyoSyaTo = ListGyosya.FirstOrDefault(_ => _.GyosyaCdSeq == outValue);
                }
                else
                {
                    confirmData.GyosyaTokuiSakiTo = null;
                }
            }
            if (confirmData.GyosyaTokuiSakiFrom != null && filterValues.ContainsKey(nameof(confirmData.TokiskTokuiSakiFrom)))
            {
                if (int.TryParse(filterValues[nameof(confirmData.TokiskTokuiSakiFrom)], out int outValue))
                {
                    List<CustomerComponentTokiskData> LstTokisk = new List<CustomerComponentTokiskData>();
                    LstTokisk = TokiskData.Where(_ => _.GyosyaCdSeq == (GyoSyaFrom?.GyosyaCdSeq ?? -1)).ToList();
                    confirmData.TokiskTokuiSakiFrom = LstTokisk.FirstOrDefault(_ => _.TokuiSeq == outValue);
                    TokuiFrom = LstTokisk.FirstOrDefault(_ => _.TokuiSeq == outValue);
                }
                else
                {
                    confirmData.TokiskTokuiSakiFrom = null;
                }
            }
            if (confirmData.GyosyaTokuiSakiFrom != null && filterValues.ContainsKey(nameof(confirmData.TokiskTokuiSakiTo)))
            {
                if (int.TryParse(filterValues[nameof(confirmData.TokiskTokuiSakiTo)], out int outValue))
                {
                    List<CustomerComponentTokiskData> LstTokisk = new List<CustomerComponentTokiskData>();
                    LstTokisk = TokiskData.Where(_ => _.GyosyaCdSeq == (GyoSyaTo?.GyosyaCdSeq ?? -1)).ToList();
                    confirmData.TokiskTokuiSakiTo = LstTokisk.FirstOrDefault(_ => _.TokuiSeq == outValue);
                    TokuiTo = LstTokisk.FirstOrDefault(_ => _.TokuiSeq == outValue);
                }
                else
                {
                    confirmData.TokiskTokuiSakiTo = null;
                }
            }
            if (filterValues.ContainsKey(nameof(confirmData.TokiStTokuiSakiFrom)) && confirmData.TokiskTokuiSakiFrom != null)
            {
                if (int.TryParse(filterValues[nameof(confirmData.TokiStTokuiSakiFrom)], out int outValue))
                {
                    List<CustomerComponentTokiStData> LstTokiSt = new List<CustomerComponentTokiStData>();
                    LstTokiSt = TokiStData.Where(_ => _.TokuiSeq == (TokuiFrom?.TokuiSeq ?? -1)).ToList();
                    confirmData.TokiStTokuiSakiFrom = LstTokiSt.FirstOrDefault(_ => _.SitenCdSeq == outValue);
                }
                else
                {
                    confirmData.TokiStTokuiSakiFrom = null;
                }
            }
            if (filterValues.ContainsKey(nameof(confirmData.TokiStTokuiSakiTo)) && confirmData.TokiskTokuiSakiTo != null)
            {
                if (int.TryParse(filterValues[nameof(confirmData.TokiStTokuiSakiTo)], out int outValue))
                {
                    List<CustomerComponentTokiStData> LstTokiSt = new List<CustomerComponentTokiStData>();
                    LstTokiSt = TokiStData.Where(_ => _.TokuiSeq == (TokuiTo?.TokuiSeq ?? -1)).ToList();
                    confirmData.TokiStTokuiSakiTo = LstTokiSt.FirstOrDefault(_ => _.SitenCdSeq == outValue);
                }
                else
                {
                    confirmData.TokiStTokuiSakiTo = null;
                }
            }
            if (filterValues.ContainsKey(nameof(confirmData.ConfirmedTimes)))
            {
                confirmData.ConfirmedTimes = ConfirmedTimeOptions.SingleOrDefault(c => c.Option == Enum.Parse<NumberOfConfirmed>(filterValues[nameof(confirmData.ConfirmedTimes)]));
            }
            if (filterValues.ContainsKey(nameof(confirmData.Saikou)))
            {
                confirmData.Saikou = ConfirmStatusOptions.SingleOrDefault(c => c.Option == Enum.Parse<ConfirmStatus>(filterValues[nameof(confirmData.Saikou)]));
            }
            if (filterValues.ContainsKey(nameof(confirmData.SumDai)))
            {
                confirmData.SumDai = ConfirmStatusOptions.SingleOrDefault(c => c.Option == Enum.Parse<ConfirmStatus>(filterValues[nameof(confirmData.SumDai)])) ;
            }
            if (filterValues.ContainsKey(nameof(confirmData.Ammount)))
            {
                confirmData.Ammount = ConfirmStatusOptions.SingleOrDefault(c => c.Option == Enum.Parse<ConfirmStatus>(filterValues[nameof(confirmData.Ammount)]));
            }
            if (filterValues.ContainsKey(nameof(confirmData.ScheduleDate)))
            {
                confirmData.ScheduleDate = ConfirmStatusOptions.SingleOrDefault(c => c.Option == Enum.Parse<ConfirmStatus>(filterValues[nameof(confirmData.ScheduleDate)]));
            }
            if (filterValues.ContainsKey(nameof(confirmData.CsvConfigOption.Header)))
            {
                confirmData.CsvConfigOption.Header = CsvHeaderOptions.SingleOrDefault(c => c.Option == Enum.Parse<CSV_Header>(filterValues[nameof(confirmData.CsvConfigOption.Header)]));
            }
            if (filterValues.ContainsKey(nameof(confirmData.CsvConfigOption.GroupSymbol)))
            {
                confirmData.CsvConfigOption.GroupSymbol = CsvGroupSymbolOptions.SingleOrDefault(c => c.Option == Enum.Parse<CSV_Group>(filterValues[nameof(confirmData.CsvConfigOption.GroupSymbol)]));
            }
            if (filterValues.ContainsKey(nameof(confirmData.CsvConfigOption.Delimiter)))
            {
                confirmData.CsvConfigOption.Delimiter = CsvDelimiterOptions.SingleOrDefault(c => c.Option == Enum.Parse<CSV_Delimiter>(filterValues[nameof(confirmData.CsvConfigOption.Delimiter)]));
            }
            if (filterValues.ContainsKey(nameof(confirmData.CsvConfigOption.DelimiterSymbol)))
            {
                confirmData.CsvConfigOption.DelimiterSymbol = filterValues[nameof(confirmData.CsvConfigOption.DelimiterSymbol)];
            }
            return confirmData;
        }
    }
}
