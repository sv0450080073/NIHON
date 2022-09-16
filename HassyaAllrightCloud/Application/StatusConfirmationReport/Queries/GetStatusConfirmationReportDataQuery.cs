using HassyaAllrightCloud.Domain.Dto;
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

namespace HassyaAllrightCloud.Application.StatusConfirmationReport.Queries
{
    public class GetStatusConfirmationReportDataQuery : IRequest<List<StatusConfirmationReportData>>
    {
        private readonly List<BookingKeyData> _selectedList;
        private int _tenantId = 1;

        public GetStatusConfirmationReportDataQuery(List<BookingKeyData> selectedList, int tenantId)
        {
            if(selectedList is null || selectedList.Contains(null))
                throw new ArgumentNullException("Value should not null", "selectedList");

            _selectedList = selectedList;
            _tenantId = tenantId;
        }

        public class Handler : IRequestHandler<GetStatusConfirmationReportDataQuery, List<StatusConfirmationReportData>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetStatusConfirmationReportDataQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetStatusConfirmationReportDataQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<List<StatusConfirmationReportData>> Handle(GetStatusConfirmationReportDataQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var connection = _context.Database.GetDbConnection();
                    SqlCommand command = new SqlCommand();
                    command.Connection = (SqlConnection)connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Pro_StatusConfirmation_R";
                    command.Parameters.AddWithValue("@selectedList", request._selectedList.Select(_=> new { _.UkeNo, _.UnkRen}).ToDataTable());
                    command.Parameters.AddWithValue("@tenantId", request._tenantId);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    await command.Connection.CloseAsync();

                    return MapTableToObjectHelper.ConvertDataTable<StatusConfirmationReportData>(dt);
                }
                catch(Exception ex)
                {
                    _logger.LogTrace(ex.ToString());

                    return await Task.FromResult(new List<StatusConfirmationReportData>());
                }
            }
        }
    }
}
