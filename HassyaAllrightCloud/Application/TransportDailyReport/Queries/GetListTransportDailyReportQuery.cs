using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.TransportDailyReport.Queries
{
    public class GetListTransportDailyReportQuery : IRequest<List<TransportDailyReportData>>
    {
        public TransportDailyReportSearchParams searchParams { get; set; }
        public class Handler : IRequestHandler<GetListTransportDailyReportQuery, List<TransportDailyReportData>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<List<TransportDailyReportData>> Handle(GetListTransportDailyReportQuery request, CancellationToken cancellationToken)
            {
                var searchParam = request.searchParams;
                List<TransportDailyReportData> result = new List<TransportDailyReportData>();

                var connection = _context.Database.GetDbConnection();
                SqlCommand command = new SqlCommand();
                command.Connection = (SqlConnection)connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PK_dTransportDailyReports_R";

                command.Parameters.AddWithValue("@OutStei", searchParam.OutputCategory);
                command.Parameters.AddWithValue("@UnkYmd", searchParam.selectedDate == null ? string.Empty : searchParam.selectedDate.ToString(CommonConstants.FormatYMD));
                command.Parameters.AddWithValue("@CompanyCd", searchParam.selectedCompany?.CompanyCd ?? 0);
                command.Parameters.AddWithValue("@StaEigyoCd", searchParam.selectedEigyoFrom?.EigyoCd ?? 0);
                command.Parameters.AddWithValue("@EndEigyoCd", searchParam.selectedEigyoTo?.EigyoCd ?? 0);
                command.Parameters.AddWithValue("@SyuKbn", searchParam.aggregation?.Value ?? 0);
                command.Parameters.AddWithValue("@TenantCdSeq", searchParam.TenantCdSeq);

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                result = MapTableToObjectHelper.ConvertDataTable<TransportDailyReportData>(dt);

                await command.Connection.CloseAsync();
                adapter.Dispose();

                return result;
            }
        }
    }
}
