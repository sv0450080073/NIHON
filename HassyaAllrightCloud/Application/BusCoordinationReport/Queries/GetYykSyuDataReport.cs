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

namespace HassyaAllrightCloud.Application.BusCoordinationReport.Queries
{
    public class GetYykSyuDataReport : IRequest<List<YykSyuDataReport>>
    {
        public string bookingParams { get; set; } = "";
        public int tenantID { get; set; } = 0;
        public string unkRenParams { get; set; }
        public class Handler : IRequestHandler<GetYykSyuDataReport, List<YykSyuDataReport>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }       
            public async Task<List<YykSyuDataReport>> Handle(GetYykSyuDataReport request, CancellationToken cancellationToken)
            {
                List<YykSyuDataReport> result = new List<YykSyuDataReport>();
                try
                {
                var connection = _context.Database.GetDbConnection();
                SqlCommand command = new SqlCommand();
                command.Connection = (SqlConnection)connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Pro_GetYykSyu_R";
                command.Parameters.AddWithValue("@ListBooking", request.bookingParams);
                command.Parameters.AddWithValue("@ListUnkRen", request.unkRenParams);
                command.Parameters.AddWithValue("@TenantCdSeq", request.tenantID.ToString());
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                await command.Connection.CloseAsync();
                result = MapTableToObjectHelper.ConvertDataTable<YykSyuDataReport>(dt);
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
