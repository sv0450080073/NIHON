using System.Linq;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.VenderRequestReport.Queries
{
    /// <summary>
    /// This query execute Pro_VenderRequestMain_R sp to get root data for main report
    /// </summary>
    public class GetVenderRequestDataMainReportQuery : IRequest<List<VenderRequestReportData>>
    {
        private readonly int _tenantId;
        private readonly VenderRequestFormData _searchData;

        public GetVenderRequestDataMainReportQuery(int tenantId, VenderRequestFormData searchData)
        {
            _tenantId = tenantId;
            _searchData = searchData ?? throw new ArgumentNullException(nameof(searchData));
        }

        public class Handler : IRequestHandler<GetVenderRequestDataMainReportQuery, List<VenderRequestReportData>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetVenderRequestDataMainReportQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetVenderRequestDataMainReportQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<List<VenderRequestReportData>> Handle(GetVenderRequestDataMainReportQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var listReservation = request._searchData.BookingTypes;
                    if(request._searchData.BookingTypeStart != null)
                    {
                        listReservation = listReservation.Where(_ => _.YoyaKbn >= request._searchData.BookingTypeStart.YoyaKbn).ToList();
                    }
                    if(request._searchData.BookingTypeEnd != null)
                    {
                        listReservation = listReservation.Where(_ => _.YoyaKbn <= request._searchData.BookingTypeEnd.YoyaKbn).ToList();
                    }
                    var connection = _context.Database.GetDbConnection();
                    SqlCommand command = new SqlCommand();
                    command.Connection = (SqlConnection)connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Pro_VenderRequestMain_R";
                    command.Parameters.AddWithValue("@minUkeCd", request._searchData._ukeCdFrom == -1 ? "1" : request._searchData.UkeCdFrom);
                    command.Parameters.AddWithValue("@maxUkeCd", request._searchData._ukeCdTo == -1 ? int.MaxValue.ToString() : request._searchData.UkeCdTo);
                    command.Parameters.AddWithValue("@startDate", request._searchData.StartDate.ToString("yyyyMMdd"));
                    command.Parameters.AddWithValue("@endDate", request._searchData.EndDate.ToString("yyyyMMdd"));
                    command.Parameters.AddWithValue("@branchId", request._searchData.Branch?.EigyoCd ?? 0); 
                    command.Parameters.AddWithValue("@reservationList", listReservation == null ? DBNull.Value : (object)(string.Join('-', listReservation.Select(_ => _.YoyaKbnSeq))));
                    command.Parameters.AddWithValue("@customerStart", $"{request._searchData.SelectedGyosyaFrom?.GyosyaCd ?? 0 :000}{request._searchData.SelectedTokiskFrom?.TokuiCd ?? 0 :0000}{request._searchData.SelectedTokiStFrom?.SitenCd ?? 0 :0000}");
                    command.Parameters.AddWithValue("@customerEnd", $"{request._searchData.SelectedGyosyaTo?.GyosyaCd ?? 999 :000}{request._searchData.SelectedTokiskTo?.TokuiCd ?? 9999 :0000}{request._searchData.SelectedTokiStTo?.SitenCd ?? 9999 :0000}");
                    command.Parameters.AddWithValue("@tenantId", request._tenantId);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    await command.Connection.CloseAsync();

                    return MapTableToObjectHelper.ConvertDataTable<VenderRequestReportData>(dt);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());

                    return new List<VenderRequestReportData>();
                }
            }
        }
    }
}
