using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.IService;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Commons.Constants;

namespace HassyaAllrightCloud.Application.StatusConfirmationReport.Queries
{
    public class SearchStatusConfirmationsByTenanIdQuery : IRequest<StatusConfirmationPagedSearch>
    {
        private readonly ITPM_CodeSyService _codeSyService;
        private readonly StatusConfirmationData _searchOption;
        private readonly int _tenantId;
        private readonly int _skip;
        private readonly byte _take;

        public SearchStatusConfirmationsByTenanIdQuery(ITPM_CodeSyService codeSyService,
            StatusConfirmationData option,
            int tenantId,
            int skip,
            byte take)
        {
            _codeSyService = codeSyService;
            _searchOption = option;
            _tenantId = tenantId;
            _skip = skip;
            _take = take;
        }

        public class Handler : IRequestHandler<SearchStatusConfirmationsByTenanIdQuery, StatusConfirmationPagedSearch>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<SearchStatusConfirmationsByTenanIdQuery> _logger;
            public Handler(KobodbContext context, ILogger<SearchStatusConfirmationsByTenanIdQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            private object StatusToValue(ConfirmStatus status)
            {
                return status switch
                {
                    ConfirmStatus.Unknown => System.DBNull.Value,
                    ConfirmStatus.Confirm => 1,
                    ConfirmStatus.UnConfirmed => 0,
                    _ => System.DBNull.Value
                };
            }

            public async Task<StatusConfirmationPagedSearch> Handle(SearchStatusConfirmationsByTenanIdQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    int totalRows = 0;
                    var resultList = new List<StatusConfirmSearchResultData>();
                    var totalSummary = new PageSummaryData();
                    string customerStr;
                    string customerEnd;
                    if (request._searchOption.GyosyaTokuiSakiFrom == null || request._searchOption.GyosyaTokuiSakiFrom.GyosyaCdSeq == 0)
                    {
                        customerStr = "";
                    }
                    else
                    {
                        customerStr = request._searchOption.GyosyaTokuiSakiFrom.GyosyaCd.ToString("D3") + (request._searchOption.TokiskTokuiSakiFrom == null ? "0000" : request._searchOption.TokiskTokuiSakiFrom.TokuiCd.ToString("D4")) + (request._searchOption.TokiStTokuiSakiFrom == null ? "0000" : request._searchOption.TokiStTokuiSakiFrom.SitenCd.ToString("D4"));
                    }
                    if (request._searchOption.GyosyaTokuiSakiTo == null || request._searchOption.GyosyaTokuiSakiTo.GyosyaCdSeq == 0)
                    {
                        customerEnd = "";
                    }
                    else
                    {
                        customerEnd = request._searchOption.GyosyaTokuiSakiTo.GyosyaCd.ToString("D3") + (request._searchOption.TokiskTokuiSakiTo == null ? "9999" : request._searchOption.TokiskTokuiSakiTo.TokuiCd.ToString("D4")) + (request._searchOption.TokiStTokuiSakiTo == null ? "9999" : request._searchOption.TokiStTokuiSakiTo.SitenCd.ToString("D4"));
                    }


                    var connection = _context.Database.GetDbConnection();
                    SqlCommand command = new SqlCommand();
                    command.Connection = (SqlConnection)connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Pro_SearchStatusConfirmation_R";
                    command.Parameters.AddWithValue("@tenantId", request._tenantId);
                    command.Parameters.AddWithValue("@startDate", request._searchOption.StartDate.ToString("yyyyMMdd"));
                    command.Parameters.AddWithValue("@endDate", request._searchOption.EndDate.ToString("yyyyMMdd"));
                    command.Parameters.AddWithValue("@company", request._searchOption.SelectedCompany.CompanyCdSeq);
                    command.Parameters.AddWithValue("@branchStart", request._searchOption.BranchStart.EigyoCd);
                    command.Parameters.AddWithValue("@branchEnd", request._searchOption.BranchEnd.EigyoCd);
                    command.Parameters.AddWithValue("@customerStart", customerStr);
                    command.Parameters.AddWithValue("@customerEnd", customerEnd);
                    command.Parameters.AddWithValue("@isFixed", request._searchOption.FixedStatus == ConfirmStatus.Fixed ? 1 : 0);
                    command.Parameters.AddWithValue("@isConfirm", request._searchOption.ConfirmedStatus == ConfirmStatus.Confirmed ? 1 : 0);
                    command.Parameters.AddWithValue("@numberOfConfirm", (int)request._searchOption.ConfirmedTimes.Option);
                    command.Parameters.AddWithValue("@saikFlg", StatusToValue(request._searchOption.Saikou.Option));
                    command.Parameters.AddWithValue("@daiSuFlg", StatusToValue(request._searchOption.SumDai.Option));
                    command.Parameters.AddWithValue("@kingFlg", StatusToValue(request._searchOption.Ammount.Option));
                    command.Parameters.AddWithValue("@nitteiFlg", StatusToValue(request._searchOption.ScheduleDate.Option));
                    command.Parameters.AddWithValue("@skip", request._skip);
                    command.Parameters.AddWithValue("@take", request._take);

                    await command.Connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        await reader.ReadAsync();
                        totalRows = Convert.ToInt32(reader["Count"]);
                        totalSummary._sumBusFee = Convert.ToInt64(reader["SumBusFee"]);
                        totalSummary._sumBusTax = Convert.ToInt64(reader["SumBusTax"]);
                        totalSummary._sumBusCharge = Convert.ToInt64(reader["SumBusCharge"]);
                        totalSummary._sumGuideFee = Convert.ToInt64(reader["SumGuideFee"]);
                        totalSummary._sumGuideTax = Convert.ToInt64(reader["SumGuideTax"]);
                        totalSummary._sumGuideCharge = Convert.ToInt64(reader["SumGuideCharge"]);

                        await reader.NextResultAsync();
                        while (await reader.ReadAsync())
                        {
                            var newItem = new StatusConfirmSearchResultData();
                            //newItem.No = Convert.ToInt32(reader["No"]);
                            newItem.Ukeno = reader["UkeNo"].ToString();
                            newItem.UnkRen = Convert.ToInt32(reader["UnkRen"]);
                            newItem.BusName = reader["BusName"].ToString();
                            newItem.BusType = reader["Bustype"].ToString();
                            newItem.Daisu = Convert.ToInt32(reader["Daisu"]);
                            newItem.TokuiStaff = reader["TokuiStaff"].ToString();
                            newItem.ShiireSaki = reader["ShiireSaki"].ToString();
                            newItem.ConfirmedTime = Convert.ToInt32(reader["ConfirmedTime"]);
                            newItem.DanTaiName = reader["DanTaiName"].ToString();
                            newItem.DestinationName = reader["DestinationName"].ToString();
                            newItem.KanjiName = reader["KanjiName"].ToString();
                            newItem.TokuiSaki = reader["TokuiSaki"].ToString();
                            newItem.ConfirmedYmd = reader["ConfirmedYmd"].ToString();
                            newItem.FixedYmd = reader["FixedYmd"].ToString();
                            newItem.ConfirmedPerson = Convert.ToInt32(reader["ConfirmedPerson"]);
                            newItem.ConfirmedBy = reader["ConfirmedBy"].ToString();
                            newItem.NoteContent = reader["NoteContent"].ToString();
                            newItem.Saikou = Convert.ToInt32(reader["Saikou"]) == 1;
                            newItem.SumDai = Convert.ToInt32(reader["SumDai"]) == 1;
                            newItem.SumAmount = Convert.ToInt32(reader["SumAmount"]) == 1;
                            newItem.ScheduledDate = Convert.ToInt32(reader["ScheduledDate"]) == 1;
                            newItem.HaishaYmd = reader["HaishaYmd"].ToString();
                            newItem.HaishaTime = reader["HaishaTime"].ToString();
                            newItem.HaiSNm = reader["HaiSNm"].ToString();
                            newItem.TouYmd = reader["TouYmd"].ToString();
                            newItem.TouTime = reader["TouTime"].ToString();
                            newItem.TouNm = reader["TouNm"].ToString();
                            newItem.PassengerQuantity = reader["PassengerQuantity"].ToString();
                            newItem.PlusPassenger = reader["PlusPassenger"].ToString();
                            newItem.BusFee = reader["BusFee"].ToString();
                            newItem.BusTaxAmount = reader["BusTaxAmount"].ToString();
                            newItem.BusTaxRate = reader["BusTaxRate"].ToString();
                            newItem.BusCharge = reader["BusCharge"].ToString();
                            newItem.BusChargeRate = reader["BusChargeRate"].ToString();
                            newItem.GuideFee = reader["GuideFee"].ToString();
                            newItem.GuideTax = reader["GuideTax"].ToString();
                            newItem.GuideCharge = reader["GuideCharge"].ToString();
                            newItem.ReceivedBranch = reader["ReceivedBranch"].ToString();
                            newItem.ReceivedBy = reader["ReceivedBy"].ToString();
                            newItem.InputBy = reader["InputBy"].ToString();
                            newItem.BookingType = reader["BookingType"].ToString();
                            newItem.ReceivedYmd = reader["ReceivedYmd"].ToString();
                            newItem.BookingNo = reader["BookingNo"].ToString();
                            newItem.Guide = Convert.ToInt32(reader["Guide"]) > 0;
                            newItem.Kotei = Convert.ToInt32(reader["Kotei"]) > 0;
                            newItem.TsuMi = Convert.ToInt32(reader["TsuMi"]) > 0;
                            newItem.Tehai = Convert.ToInt32(reader["Tehai"]) > 0;
                            newItem.Futai = Convert.ToInt32(reader["Futai"]) > 0;

                            resultList.Add(newItem);
                        }
                    }
                    
                    await command.Connection.CloseAsync();
                    var uniqueBooking =
                        (from ac in resultList
                         group ac by new { ac.BookingNo, ac.UnkRen } into av
                         select new
                         {
                             av.Key.BookingNo,
                             av.Key.UnkRen,
                             ListData = av.ToList()
                         })
                         .OrderBy(_ => _.BookingNo)
                         .ThenBy(_ => _.UnkRen)
                         .ToList();

                    // This step collapsible multiple booking into one record.
                    var uniqueBkDetail =
                        (from ub in uniqueBooking
                         join ax in resultList.Distinct() on new { ub.BookingNo, ub.UnkRen } equals new { ax.BookingNo, ax.UnkRen }
                         let temp = ax.BusViewDatas = ub.ListData.Select(_ => new BusViewData
                         {
                             BusName = _.BusName,
                             BusType = _.BusType,
                             Daisu = _.Daisu
                         }).ToList()
                         select ax
                        ).OrderBy(_ => _.BookingNo).ToList().Select((s, index) => { s.No = index + 1 + request._skip; return s; }).ToList();
                    return new StatusConfirmationPagedSearch()
                    {
                        TotalItems = totalRows,
                        DataList = uniqueBkDetail,
                        TotalSummary = totalSummary
                    };
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    return new StatusConfirmationPagedSearch();
                }
            }
        }
    }
}
