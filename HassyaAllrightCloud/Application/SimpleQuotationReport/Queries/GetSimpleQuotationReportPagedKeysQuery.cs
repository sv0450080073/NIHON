using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;

namespace HassyaAllrightCloud.Application.SimpleQuotationReport.Queries
{
    public class GetSimpleQuotationReportPagedKeysQuery : IRequest<List<BookingKeyData>>
    {
        private readonly SimpleQuotationData _filterData;

        public GetSimpleQuotationReportPagedKeysQuery(SimpleQuotationData filterData)
        {
            _filterData = filterData ?? throw new ArgumentNullException(nameof(SimpleQuotationData), "Cannot get report simple quotation with null filter data");
        }

        public class Handler : IRequestHandler<GetSimpleQuotationReportPagedKeysQuery, List<BookingKeyData>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<BookingKeyData>> Handle(GetSimpleQuotationReportPagedKeysQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var resultList = new List<BookingKeyData>();
                    var connection = _context.Database.GetDbConnection();
                    SqlCommand command = new SqlCommand();
                    command.Connection = (SqlConnection)connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Pro_GetSimpleQuotationPagedKey_R";
                    command.Parameters.AddWithValue("@tenantId", request._filterData.TenantId);
                    command.Parameters.AddWithValue("@startPickupDate", request._filterData.StartPickupDate?.ToString("yyyyMMdd") ?? string.Empty);
                    command.Parameters.AddWithValue("@endPickupDate", request._filterData.EndPickupDate?.ToString("yyyyMMdd") ?? string.Empty);
                    command.Parameters.AddWithValue("@startArrivalDate", request._filterData.StartArrivalDate?.ToString("yyyyMMdd") ?? string.Empty);
                    command.Parameters.AddWithValue("@endArrivalDate", request._filterData.EndArrivalDate?.ToString("yyyyMMdd") ?? string.Empty);
                    command.Parameters.AddWithValue("@startReservationClassification", GetValue(request._filterData.YoyakuFrom));
                    command.Parameters.AddWithValue("@endReservationClassification", GetValue(request._filterData.YoyakuTo));
                    command.Parameters.AddWithValue("@ukeCdFrom", Convert.ToDouble(string.IsNullOrWhiteSpace(request._filterData.UkeCdFrom) ? "0" : request._filterData.UkeCdFrom));
                    command.Parameters.AddWithValue("@ukeCdTo", Convert.ToDouble(string.IsNullOrWhiteSpace(request._filterData.UkeCdTo) ? "0" : request._filterData.UkeCdTo));
                    command.Parameters.AddWithValue("@branchStart", request._filterData.BranchStart?.EigyoCd ?? 0);
                    command.Parameters.AddWithValue("@branchEnd", request._filterData.BranchEnd?.EigyoCd ?? 0);
                    command.Parameters.AddWithValue("@startSupplier", GetCustomerCondition(request._filterData.GyosyaShiireSakiFrom, request._filterData));
                    command.Parameters.AddWithValue("@endSupplier", GetCustomerCondition(request._filterData.GyosyaShiireSakiTo, request._filterData, false));
                    await command.Connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var bookingKey = new BookingKeyData();
                            bookingKey.UkeNo = reader["UkeNo"].ToString();
                            bookingKey.UnkRen = Convert.ToInt32(reader["UnkRen"]);
                            resultList.Add(bookingKey);
                        }
                    }

                    await command.Connection.CloseAsync();
                    return resultList;
                }
                catch (Exception)
                {
                    throw;
                }

                object GetValue(object val)
                {
                    if (val is null)
                        return DBNull.Value;
                    return val;
                }

                object GetCustomerCondition(object gyosya, SimpleQuotationData condition, bool isFrom = true)
                {
                    if(gyosya is null)
                        return DBNull.Value;
                    if (isFrom)
                    {
                        return ((CustomerComponentGyosyaData)gyosya).GyosyaCd.ToString("D3")
                        + (condition.TokiskShiireSakiFrom == null ? "0000" : condition.TokiskShiireSakiFrom.TokuiCd.ToString("D4"))
                        + (condition.TokiStShiireSakiFrom == null ? "0000" : condition.TokiStShiireSakiFrom.SitenCd.ToString("D4"));
                    }
                    else
                    {
                        return ((CustomerComponentGyosyaData)gyosya).GyosyaCd.ToString("D3")
                        + (condition.TokiskShiireSakiTo == null ? "9999" : condition.TokiskShiireSakiTo.TokuiCd.ToString("D4"))
                        + (condition.TokiStShiireSakiTo == null ? "9999" : condition.TokiStShiireSakiTo.SitenCd.ToString("D4"));
                    }
                }
            }
        }
    }
}
