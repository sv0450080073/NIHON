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

namespace HassyaAllrightCloud.Application.Staff.Queries
{
    public class GetNumberOfVehicleQuery : IRequest<List<NumberOfVehicle>>
    {
        public int CompanyCdSeq { get; set; }
        public string UnkYmd { get; set; }
        public class Handler : IRequestHandler<GetNumberOfVehicleQuery, List<NumberOfVehicle>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<NumberOfVehicle>> Handle(GetNumberOfVehicleQuery request, CancellationToken cancellationToken)
            {
                List<NumberOfVehicle> result = new List<NumberOfVehicle>();

                try
                {
                    var connection = _context.Database.GetDbConnection();
                    SqlCommand command = new SqlCommand();
                    command.Connection = (SqlConnection)connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PK_dNumberOfVehicle_R";

                    command.Parameters.AddWithValue("@CompanyCdSeq", request.CompanyCdSeq);
                    command.Parameters.AddWithValue("@UnkYmd", request.UnkYmd);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    result = MapTableToObjectHelper.ConvertDataTable<NumberOfVehicle>(dt);

                    await command.Connection.CloseAsync();
                    adapter.Dispose();
                    return result;
                }
                catch(Exception ex)
                {
                    // TODO: write log
                    throw ex;
                }
            }
        }
    }
}
