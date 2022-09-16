using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.IService;
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

namespace HassyaAllrightCloud.Application.AccessoryFeeListReport.Query
{
    public class GetAccessoryFeeListReportQuery : IRequest<List<AccessoryFeeListReportData>>
    {
        private AccessoryFeeListData _searchCondition;
        private readonly int _tenantId;
        private readonly ITPM_CodeSyService _codeSyService;

        public GetAccessoryFeeListReportQuery(AccessoryFeeListData searchCondition, int tenantId, ITPM_CodeSyService codeSyService)
        {
            _searchCondition = searchCondition;
            _tenantId = tenantId;
            _codeSyService = codeSyService;
        }

        public class Handler : IRequestHandler<GetAccessoryFeeListReportQuery, List<AccessoryFeeListReportData>>
        {
            private KobodbContext _context;
            private ILogger<GetAccessoryFeeListReportQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetAccessoryFeeListReportQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<List<AccessoryFeeListReportData>> Handle(GetAccessoryFeeListReportQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var condition = request._searchCondition;

                    var connection = _context.Database.GetDbConnection();
                    SqlCommand command = new SqlCommand();
                    command.Connection = (SqlConnection)connection;
                    command.CommandType = CommandType.StoredProcedure;

                    switch (condition.ReportType.Option)
                    {
                        case ReportType.Detail:
                            command.CommandText = "Pro_AccessoryFeeListDetail_R";
                            break;
                        case ReportType.Summary:
                            command.CommandText = "Pro_AccessoryFeeListSummary_R";
                            break;
                        default:
                            command.CommandText = "Pro_AccessoryFeeListDetail_R";
                            break;
                    }
                    var listReservation = condition.BookingTypes.ToList();
                    if (condition.BookingTypeStart != null)
                        listReservation = listReservation.Where(_ => _.YoyaKbn >= condition.BookingTypeStart.YoyaKbn).ToList();
                    if (condition.BookingTypeEnd != null)
                        listReservation = listReservation.Where(_ => _.YoyaKbn <= condition.BookingTypeEnd.YoyaKbn).ToList();
                    command.Parameters.AddWithValue("@startDate", condition.StartDate.ToString("yyyyMMdd"));
                    command.Parameters.AddWithValue("@endDate", condition.EndDate.ToString("yyyyMMdd"));
                    command.Parameters.AddWithValue("@dateType", condition.DateType == DateType.Dispatch ? 1 : (condition.DateType == DateType.Arrival ? 2 : 3));
                    command.Parameters.AddWithValue("@customerFrom", $"{condition.SelectedGyosyaStart?.GyosyaCd ?? 0:000}{condition.SelectedTokiskStart?.TokuiCd ?? 0:0000}{condition.SelectedTokistStart?.SitenCd ?? 0:0000}");
                    command.Parameters.AddWithValue("@customerTo", $"{condition.SelectedGyosyaEnd?.GyosyaCd ?? 999:000}{condition.SelectedTokiskEnd?.TokuiCd ?? 9999:0000}{condition.SelectedTokistEnd?.SitenCd ?? 9999:0000}");
                    command.Parameters.AddWithValue("@invoiceTypes", condition.InvoiceType.Option == InvoiceTypeOption.All ? "1-2" : (condition.InvoiceType.Option == InvoiceTypeOption.Liquidate ? "1" : "2"));
                    command.Parameters.AddWithValue("@companyId", condition.Company?.CompanyCd ?? 0);
                    command.Parameters.AddWithValue("@brandStart", condition.BranchStart?.EigyoCd ?? 0);
                    command.Parameters.AddWithValue("@brandEnd", condition.BranchEnd?.EigyoCd ?? int.MaxValue);
                    command.Parameters.AddWithValue("@futTumStart", condition.FutaiStart?.FutaiCdSeq ?? 0);
                    command.Parameters.AddWithValue("@futTumEnd", condition.FutaiEnd?.FutaiCdSeq ?? int.MaxValue);
                    command.Parameters.AddWithValue("@bookingTypes", string.Join('-', listReservation.Select(_=>_.YoyaKbnSeq)));
                    command.Parameters.AddWithValue("@ukeCdFrom", condition._ukeCdFrom == -1 ? "1" : condition.UkeCdFrom);
                    command.Parameters.AddWithValue("@ukeCdTo", condition._ukeCdTo == -1 ? int.MaxValue.ToString() : condition.UkeCdTo);
                    command.Parameters.AddWithValue("@futaiTypes", string.Join('-', request._searchCondition.FutaiFeeTypes.Where(_=> _ != null).Select(_=>_.FutGuiKbn)));
                    command.Parameters.AddWithValue("@tenantId", request._tenantId);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    await command.Connection.CloseAsync();

                    var data = MapTableToObjectHelper.ConvertDataTable<AccessoryFeeListReportData>(dt);

                    return await request._codeSyService.FilterTenantIdByListCodeSyu((tenantIds, codeSyus) =>
                    {
                        return Task.FromResult(
                            (from main in data.AsEnumerable()
                             join codeSeisan in _context.VpmCodeKb on new { CodeKbn = Convert.ToInt32(main.CodeKbnSeisan), CodeSyu = codeSyus[0], TenantCdSeq = tenantIds[0] } equals new { CodeKbn = Convert.ToInt32(codeSeisan.CodeKbn), codeSeisan.CodeSyu, codeSeisan.TenantCdSeq } into cs
                             from subCs in cs.DefaultIfEmpty()
                             let temp = main.CodeKbnNmSeisan = subCs?.CodeKbnNm ?? string.Empty
                             select main).ToList());
                    }, request._tenantId, new List<string> { "SEISANKBN" });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());

                    return new List<AccessoryFeeListReportData>();
                }
            }
        }
    }
}
