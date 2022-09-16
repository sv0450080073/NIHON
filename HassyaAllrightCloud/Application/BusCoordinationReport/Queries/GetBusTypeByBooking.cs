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
    public class GetBusTypeByBooking : IRequest<List<BusTypeDataReport>>
    {
        public string bookingParams { get; set; } = "";
        public int tenantID { get; set; } = 0;
        public class Handler : IRequestHandler<GetBusTypeByBooking, List<BusTypeDataReport>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }      
            public async Task<List<BusTypeDataReport>> Handle(GetBusTypeByBooking request, CancellationToken cancellationToken)
            {
               
                List<BusTypeDataReport> result = new List<BusTypeDataReport>();
                try
                {                  
                    var connection = _context.Database.GetDbConnection();
                    SqlCommand command = new SqlCommand();
                    command.Connection = (SqlConnection)connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Pro_GetBusType_R";
                    command.Parameters.AddWithValue("@ListBooking", request.bookingParams);
                    command.Parameters.AddWithValue("@TenantCdSeq", request.tenantID.ToString());
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    await command.Connection.CloseAsync();
                    result = MapTableToObjectHelper.ConvertDataTable<BusTypeDataReport>(dt);
                    return result;                
                }
                catch(Exception ex)
                {
                    return result;
                }
            }
        }
    }
}
