using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using HassyaAllrightCloud.Commons.Helpers;

namespace HassyaAllrightCloud.Application.BusCoordinationReport.Queries
{
    public class GetYousha : IRequest<List<YouShaDataReport>>
    {
        public string bookingParams { get; set; } = "";
        public int tenantID { get; set; } = 0;
        public string unkRenParams { get; set; }
        public class Handler : IRequestHandler<GetYousha, List<YouShaDataReport>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<YouShaDataReport>> Handle(GetYousha request, CancellationToken cancellationToken)
            {
                List<YouShaDataReport> result = new List<YouShaDataReport>();
                try
                {
                    
                    var connection = _context.Database.GetDbConnection();
                    SqlCommand command = new SqlCommand();
                    command.Connection = (SqlConnection)connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Pro_GetYousha_R";
                    command.Parameters.AddWithValue("@ListBooking", request.bookingParams);
                    command.Parameters.AddWithValue("@ListUnkRen", request.unkRenParams);
                    command.Parameters.AddWithValue("@TenantCdSeq", request.tenantID.ToString());
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    await command.Connection.CloseAsync();
                    result = MapTableToObjectHelper.ConvertDataTable<YouShaDataReport>(dt);
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
