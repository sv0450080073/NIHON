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
    public class GetHaishaForCalculateAmountQuery : IRequest<List<VehicleDailyReportHaisha>>
    {
        public VehicleDailyReportModel searchParams { get; set; }
        public class Handler : IRequestHandler<GetHaishaForCalculateAmountQuery, List<VehicleDailyReportHaisha>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<VehicleDailyReportHaisha>> Handle(GetHaishaForCalculateAmountQuery request, CancellationToken cancellationToken)
            {
                var searchParam = request.searchParams;
                List<VehicleDailyReportHaisha> result = new List<VehicleDailyReportHaisha>();

                var connection = _context.Database.GetDbConnection();
                SqlCommand command = new SqlCommand();
                command.Connection = (SqlConnection)connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PK_dVehicleDailyReportHaisha_R";

                command.Parameters.AddWithValue("@UkeNo", searchParam.UkeNo);
                command.Parameters.AddWithValue("@UnkRen", searchParam.UnkRen);
                command.Parameters.AddWithValue("@TeiDanNo", searchParam.TeiDanNo);
                command.Parameters.AddWithValue("@BunkRen", searchParam.BunkRen);

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                result = MapTableToObjectHelper.ConvertDataTable<VehicleDailyReportHaisha>(dt);

                await command.Connection.CloseAsync();
                adapter.Dispose();

                return result;
            }
        }
    }
}
