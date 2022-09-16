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

namespace HassyaAllrightCloud.Application.VehicleSchedulerMobile.Queries
{
    public class GetListVehicleRepairQuery : IRequest<List<VehicleRepairData>>
    {
        public string StartYmd { get; set; }
        public string EndYmd { get; set; }
        public int SyaRyoCdSeq { get; set; }

        public class Handler : IRequestHandler<GetListVehicleRepairQuery, List<VehicleRepairData>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<VehicleRepairData>> Handle(GetListVehicleRepairQuery request, CancellationToken cancellationToken)
            {
                List<VehicleRepairData> result = new List<VehicleRepairData>();

                try
                {
                    var connection = _context.Database.GetDbConnection();
                    SqlCommand command = new SqlCommand();
                    command.Connection = (SqlConnection)connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PK_dVehicleRepair_R";

                    command.Parameters.AddWithValue("@StartYmd", request.StartYmd);
                    command.Parameters.AddWithValue("@EndYmd", request.EndYmd);
                    command.Parameters.AddWithValue("@SyaRyoCdSeq", request.SyaRyoCdSeq);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    result = MapTableToObjectHelper.ConvertDataTable<VehicleRepairData>(dt);

                    await command.Connection.CloseAsync();
                    adapter.Dispose();
                }
                catch(Exception ex)
                {
                    throw ex;
                }

                return result;
            }
        }
    }
}
