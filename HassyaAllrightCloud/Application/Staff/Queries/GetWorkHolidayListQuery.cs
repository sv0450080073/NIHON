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
    public class GetWorkHolidayListQuery : IRequest<List<WorkHolidayItem>>
    {
        public int CompanyCdSeq { get; set; }
        public string UnkYmd { get; set; }
        public class Handler : IRequestHandler<GetWorkHolidayListQuery, List<WorkHolidayItem>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<WorkHolidayItem>> Handle(GetWorkHolidayListQuery request, CancellationToken cancellationToken)
            {
                List<WorkHolidayItem> result = new List<WorkHolidayItem>();

                try
                {
                    var connection = _context.Database.GetDbConnection();
                    SqlCommand command = new SqlCommand();
                    command.Connection = (SqlConnection)connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PK_dWorkHolidays_R";

                    command.Parameters.AddWithValue("@CompanyCdSeq", request.CompanyCdSeq);
                    command.Parameters.AddWithValue("@UnkYmd", request.UnkYmd);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    result = MapTableToObjectHelper.ConvertDataTable<WorkHolidayItem>(dt);

                    await command.Connection.CloseAsync();
                    adapter.Dispose();
                    return result;
                }
                catch (Exception ex)
                {
                    // TODO: write log
                    throw ex;
                }
            }
        }
    }
}
