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
    public class GetListSyaRyoQuery : IRequest<List<VehicleSchedulerSyaRyoData>>
    {
        public int EigyoCdSeq { get; set; }
        public class Handler : IRequestHandler<GetListSyaRyoQuery, List<VehicleSchedulerSyaRyoData>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<VehicleSchedulerSyaRyoData>> Handle(GetListSyaRyoQuery request, CancellationToken cancellationToken)
            {
                List<VehicleSchedulerSyaRyoData> result;

                try
                {
                    var connection = _context.Database.GetDbConnection();
                    SqlCommand command = new SqlCommand();
                    command.Connection = (SqlConnection)connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PK_dVehicleSchedulerMobileGetSyaRyos_R";

                    command.Parameters.AddWithValue("@EigyoCdSeq", request.EigyoCdSeq);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    result = MapTableToObjectHelper.ConvertDataTable<VehicleSchedulerSyaRyoData>(dt);

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
