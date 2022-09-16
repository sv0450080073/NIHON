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

namespace HassyaAllrightCloud.Application.VehicleDailyReport.Queries
{
    public class GetVehicleDailyReportForEditQuery : IRequest<List<VehicleDailyReportData>>
    {
        public VehicleDailyReportModel searchParams { get; set; }
        public class Handler : IRequestHandler<GetVehicleDailyReportForEditQuery, List<VehicleDailyReportData>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            /// <summary>
            /// Get list vehicle daily report for update
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<List<VehicleDailyReportData>> Handle(GetVehicleDailyReportForEditQuery request, CancellationToken cancellationToken)
            {
                var searchParam = request.searchParams;
                List<VehicleDailyReportData> result = new List<VehicleDailyReportData>();

                var connection = _context.Database.GetDbConnection();
                SqlCommand command = new SqlCommand();
                command.Connection = (SqlConnection)connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PK_dVehicleDailyReportForEdit_R";

                command.Parameters.AddWithValue("@UkeNo", searchParam.UkeNo);
                command.Parameters.AddWithValue("@UnkRen", searchParam.UnkRen);
                command.Parameters.AddWithValue("@TeiDanNo", searchParam.TeiDanNo);
                command.Parameters.AddWithValue("@BunkRen", searchParam.BunkRen);
                command.Parameters.AddWithValue("@UnkYmd", searchParam.UnkYmd);
                //command.Parameters.AddWithValue("@TenantCdSeq", searchParam.TenantCdSeq);

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                result = MapTableToObjectHelper.ConvertDataTable<VehicleDailyReportData>(dt);

                await command.Connection.CloseAsync();
                adapter.Dispose();

                return result;
            }
        }
    }
}
