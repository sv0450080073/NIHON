using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.SubContractorStatus;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.SubContractorStatusReport.Queries
{
    public class SearchSubContractorStatusQuery : IRequest<SubContractorStatusSearchPaged>
    {
        private readonly SubContractorStatusData _searchOption;
        private readonly int _tenantId;
        private readonly int _page;
        private readonly int _itemPerPage;

        public SearchSubContractorStatusQuery(SubContractorStatusData searchOption, int tenantId, int page, int itemPerPage)
        {
            _searchOption = searchOption ?? throw new ArgumentNullException(nameof(searchOption));
            _tenantId = tenantId;
            _page = page;
            _itemPerPage = itemPerPage;
        }

        public class Handler : IRequestHandler<SearchSubContractorStatusQuery, SubContractorStatusSearchPaged>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<SearchSubContractorStatusQuery> _logger;
            public Handler(KobodbContext context, ILogger<SearchSubContractorStatusQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<SubContractorStatusSearchPaged> Handle(SearchSubContractorStatusQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var condition = request._searchOption;
                    var result = new List<SubContractorStatusSearchResultData>();
                    var summary = new SubContractorStatusSummaryData();

                    var connection = _context.Database.GetDbConnection();
                    SqlCommand command = new SqlCommand();
                    command.Connection = (SqlConnection)connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Pro_SubContractorStatus_R";
                    command.Parameters.AddWithValue("@startDate", condition.StartDate.ToString("yyyyMMdd"));
                    command.Parameters.AddWithValue("@endDate", condition.EndDate.ToString("yyyyMMdd"));
                    command.Parameters.AddWithValue("@dateType", condition.DateType == DateType.Dispatch ? 1 : (condition.DateType == DateType.Arrival ? 2 : 3));
                    //command.Parameters.AddWithValue("@sitenTo", condition.CustomerEnd?.SitenCd ?? int.MaxValue);
                    command.Parameters.AddWithValue("@gyosyaFrom", condition?.SelectedGyosyaFrom?.GyosyaCd ?? 0);
                    command.Parameters.AddWithValue("@gyosyaTo", condition?.SelectedGyosyaTo?.GyosyaCd ?? 999);
                    command.Parameters.AddWithValue("@tokuiFrom", condition?.SelectedTokiskFrom?.TokuiCd ?? 0);
                    command.Parameters.AddWithValue("@tokuiTo", condition?.SelectedTokiskTo?.TokuiCd ?? 9999);
                    command.Parameters.AddWithValue("@sitenFrom", condition?.SelectedTokiStFrom?.SitenCd ?? 0);
                    command.Parameters.AddWithValue("@sitenTo", condition?.SelectedTokiStTo?.SitenCd ?? 9999);
                    command.Parameters.AddWithValue("@bookingTypeFrom", condition.RegistrationTypeFrom?.YoyaKbn ?? 0);
                    command.Parameters.AddWithValue("@bookingTypeTo", condition.RegistrationTypeTo?.YoyaKbn ?? 99);
                    command.Parameters.AddWithValue("@companyIds", string.Join('-', condition.Companies.Where(_ => _ != null).Select(_ => _.CompanyCdSeq)));
                    command.Parameters.AddWithValue("@brandStart", condition.BranchStart?.EigyoCd ?? 0);
                    command.Parameters.AddWithValue("@brandEnd", condition.BranchEnd?.EigyoCd ?? int.MaxValue);
                    command.Parameters.AddWithValue("@staffFrom", condition.StaffStart?.SyainCd ?? "0");
                    command.Parameters.AddWithValue("@staffTo", condition.StaffEnd?.SyainCd ?? $"{int.MaxValue}");
                    //command.Parameters.AddWithValue("@bookingTypes", string.Join('-', YoyKbnHelper.GetListYoyKbnFromTo(condition.BookingTypeStart, condition.BookingTypeEnd, condition.BookingTypes.Where(_ => _ != null).ToList()).Select(_ => _.YoyaKbnSeq)));
                    command.Parameters.AddWithValue("@ukeCdFrom", condition._ukeCdFrom == -1 ? "1" : condition.UkeCdFrom);
                    command.Parameters.AddWithValue("@ukeCdTo", condition._ukeCdTo == -1 ? int.MaxValue.ToString() : condition.UkeCdTo);
                    command.Parameters.AddWithValue("@jitaFlg", (int)condition.OwnCompanyType.Option);
                    command.Parameters.AddWithValue("@tenantId", request._tenantId);
                    command.Parameters.AddWithValue("@isSearch", 1);
                    command.Parameters.AddWithValue("@group", (int)condition.Group.Option);
                    command.Parameters.AddWithValue("@outputOrder", (int)condition.OutputOrder.Option);
                    command.Parameters.AddWithValue("@itemPerPage", request._itemPerPage);
                    command.Parameters.AddWithValue("@page", request._page);

                    await command.Connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var busMoney = MapTableToObjectHelper.ConvertToListObject<SubContractorStatusSummaryData>(reader, 1);
                        await reader.NextResultAsync();
                        var busLoanMoney = MapTableToObjectHelper.ConvertToListObject<SubContractorStatusSummaryData>(reader, 1);
                        summary = MergeSummaryInfo((await busMoney).FirstOrDefault(), (await busLoanMoney).FirstOrDefault());

                        await reader.NextResultAsync();
                        result = await MapTableToObjectHelper.ConvertToListObject<SubContractorStatusSearchResultData>(reader);
                    };

                    await command.Connection.CloseAsync();

                    return new SubContractorStatusSearchPaged
                    {
                        PageData = CollapseRecords(result).Select((item, index) =>
                                                            {
                                                                item.No = ((request._page - 1) * request._itemPerPage) + index + 1;
                                                                return item;
                                                            }).ToList(),
                        Summary = summary,
                        TotalRecord = int.Parse(summary?.TotalRecords ?? "0"),
                    };
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);

                    return new SubContractorStatusSearchPaged();
                }
            }

            private SubContractorStatusSummaryData MergeSummaryInfo(SubContractorStatusSummaryData obj1, SubContractorStatusSummaryData obj2)
            {
                return new SubContractorStatusSummaryData
                {
                    TotalRecords = obj1?.TotalRecords.ToString(),

                    TotalSyaRyoUnc = obj1?._totalSyaRyoUnc.ToString(),
                    TotalZeiRui = obj1?._totalZeiRui.ToString(),
                    TotalTesuRyoG = obj1?._totalTesuRyoG.ToString(),

                    TotalGuideFee = obj1?._totalGuideFee.ToString(),
                    TotalGuideTax = obj1?._totalGuideTax.ToString(),
                    TotalUnitGuiderFee = obj1?._totalUnitGuiderFee.ToString(),

                    TotalIncidentalFee = obj1?._totalIncidentalFee.ToString(),
                    TotalIncidentalTax = obj1?._totalIncidentalTax.ToString(),
                    TotalIncidentalCharge = obj1?._totalIncidentalCharge.ToString(),

                    TotalYoushaUnc = obj2?._totalYoushaUnc.ToString(),
                    TotalYoushaSyo = obj2?._totalYoushaSyo.ToString(),
                    TotalYoushaTes = obj2?._totalYoushaTes.ToString(),

                    TotalYouFutTumGuiKin = obj2?._totalYouFutTumGuiKin.ToString(),
                    TotalYouFutTumGuiTax = obj2?._totalYouFutTumGuiTax.ToString(),
                    TotalYouFutTumGuiTes = obj2?._totalYouFutTumGuiTes.ToString(),

                    TotalYouFutTumKin = obj2?._totalYouFutTumKin.ToString(),
                    TotalYouFutTumTax = obj2?._totalYouFutTumTax.ToString(),
                    TotalYouFutTumTes = obj2?._totalYouFutTumTes.ToString(),
                };
            }

            private List<SubContractorStatusSearchResultData> CollapseRecords(List<SubContractorStatusSearchResultData> source)
            {
                var uniqueBooking =
                        (from ac in source
                         group ac by new { ac.UkeCd, ac.UnkRen, ac.YouTokuiSeq, ac.YouSitenCdSeq } into av
                         select new
                         {
                             av.Key.UkeCd,
                             av.Key.UnkRen,
                             av.Key.YouTokuiSeq,
                             av.Key.YouSitenCdSeq,
                             ListData = av
                         });

                // This step collapsible multiple booking into one record.
                var result =
                    (from ub in uniqueBooking
                     join ax in source.DistinctBy(_ => new { _.UkeCd, _.UnkRen, _.YouTokuiSeq, _.YouSitenCdSeq })
                     on new { ub.UkeCd, ub.UnkRen, ub.YouTokuiSeq, ub.YouSitenCdSeq }
                     equals new { ax.UkeCd, ax.UnkRen, ax.YouTokuiSeq, ax.YouSitenCdSeq }
                     let temp0 = ax.GoSyas = ub.ListData.Select(_ => _.HAISHA_GoSya).ToList()
                     let temp1 = ax.BusScheduleInfos = ub.ListData.Select(item => new BusScheduleInfo
                     {
                         H_HaiSBinNm = item.H_HaiSBinNm,
                         H_HaiSKouKNm = item.H_HaiSKouKNm,
                         H_HaiSNm = item.H_HaiSNm,
                         H_HaiSSetTime = item.H_HaiSSetTime,
                         H_HaiSTime = item.H_HaiSTime,
                         H_HaiSYmd = item.H_HaiSYmd,
                         H_TouBinNm = item.H_TouBinNm,
                         H_TouChTime = item.H_TouChTime,
                         H_TouSKouKNm = item.H_TouSKouKNm,
                         H_TouNm = item.H_TouNm,
                         H_TouSetTime = item.H_TouSetTime,
                         H_TouYmd = item.H_TouYmd,
                     }).ToList()
                     let temp2 = ax.TaxFeeInfos = ub.ListData.Select(item => new TaxFeeInfo
                     {
                         YoushaSyo = item.YoushaSyo,
                         YoushaTes = item.YoushaTes,
                         YoushaUnc = item.YoushaUnc,
                         YouTesuRitu = item.YouTesuRitu,
                         YouZeiritsu = item.YouZeiritsu,
                     }).ToList()
                     select ax
                    )
                    .ToList();

                return result;
            }
        }
    }
}
