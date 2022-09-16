using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BusCoordinationReport.Queries
{
    public class GetFutTum : IRequest<List<FutTumDataReport>>
    {
        public string bookingParams { get; set; } = "";
        public int futTumKbn { get; set; } = 0;
        public string unkRenParams { get; set; }
        public class Handler : IRequestHandler<GetFutTum, List<FutTumDataReport>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<FutTumDataReport>> Handle(GetFutTum request, CancellationToken cancellationToken)
            {
                List<FutTumDataReport> result = new List<FutTumDataReport>();
                try
                {
                   
                    var connection = _context.Database.GetDbConnection();
                    SqlCommand command = new SqlCommand();
                    command.Connection = (SqlConnection)connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Pro_GetFutTum_R";
                    command.Parameters.AddWithValue("@ListBooking", request.bookingParams);
                    command.Parameters.AddWithValue("@ListUnkRen", request.unkRenParams);
                    command.Parameters.AddWithValue("@FutTumKbn", request.futTumKbn.ToString());
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    await command.Connection.CloseAsync();
                    result = MapTableToObjectHelper.ConvertDataTable<FutTumDataReport>(dt);
                    return result;

                }
                catch(Exception)
                {
                    return result;
                }
            }
        }

    }
}
