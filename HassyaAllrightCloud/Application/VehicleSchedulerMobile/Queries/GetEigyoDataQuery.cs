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
    public class GetEigyoDataQuery : IRequest<VehicleSchedulerEigyoData>
    {
        public int SyaRyoCdSeq { get; set; }
        public class Handler : IRequestHandler<GetEigyoDataQuery, VehicleSchedulerEigyoData>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<VehicleSchedulerEigyoData> Handle(GetEigyoDataQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    VehicleSchedulerEigyoData result = null;

                    var connection = _context.Database.GetDbConnection();
                    SqlCommand command = new SqlCommand();
                    command.Connection = (SqlConnection)connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PK_dVehicleSchedulerMobileGetEigyo_R";

                    command.Parameters.AddWithValue("@SyaRyoCdSeq", request.SyaRyoCdSeq);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    var list = MapTableToObjectHelper.ConvertDataTable<VehicleSchedulerEigyoData>(dt);

                    await command.Connection.CloseAsync();
                    adapter.Dispose();

                    if (list.Count > 0) result = list[0];

                    return result;
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
