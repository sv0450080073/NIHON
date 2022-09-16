using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Commons.Extensions;
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
    /// This query execute Pro_VenderRequestSub_R sp to get root data for sub report
    /// </summary>
    public class GetVenderRequestDataSubReportQuery : IRequest<List<VenderRequestReportData>>
    {
        private readonly int _tenantId;
        private readonly IEnumerable<Tuple<string, int>> _bookingKeys;

        public GetVenderRequestDataSubReportQuery(int tenantId, IEnumerable<Tuple<string, int>> bookingKeys)
        {
            _tenantId = tenantId;
            _bookingKeys = bookingKeys ?? throw new ArgumentNullException(nameof(bookingKeys));
        }

        public class Handler : IRequestHandler<GetVenderRequestDataSubReportQuery, List<VenderRequestReportData>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetVenderRequestDataSubReportQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetVenderRequestDataSubReportQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<List<VenderRequestReportData>> Handle(GetVenderRequestDataSubReportQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var connection = _context.Database.GetDbConnection();
                    SqlCommand command = new SqlCommand();
                    command.Connection = (SqlConnection)connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Pro_VenderRequestSub_R";
                    command.Parameters.AddWithValue("@bookingKeys", request._bookingKeys.ToDataTable());
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
