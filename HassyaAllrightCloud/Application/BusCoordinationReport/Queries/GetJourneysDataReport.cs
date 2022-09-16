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
    public class GetJourneysDataReport : IRequest<List<JourneysDataReport>>
    {
        public string ukeno { get; set; } = "";
        public string unkRen { get; set; }
        public string syuKoYmd { get; set; } = "";
        public string kikYmd { get; set; } = "";
        public string teiDanNo { get; set; } = "";
        public class Handler : IRequestHandler<GetJourneysDataReport, List<JourneysDataReport>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<JourneysDataReport>> Handle(GetJourneysDataReport request, CancellationToken cancellationToken)
            {
                List<JourneysDataReport> result = new List<JourneysDataReport>();
                try
                {
                var connection = _context.Database.GetDbConnection();
                SqlCommand command = new SqlCommand();
                command.Connection = (SqlConnection)connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Pro_GetKouteiAndTeHai_R";
                command.Parameters.AddWithValue("@Ukeno", request.ukeno);
                command.Parameters.AddWithValue("@UnkRen", request.unkRen);
                command.Parameters.AddWithValue("@SyuKoYmd", request.syuKoYmd);
                command.Parameters.AddWithValue("@KikYmd", request.kikYmd); 
                command.Parameters.AddWithValue("@TeiDanNo", request.teiDanNo);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                await command.Connection.CloseAsync();
                result = MapTableToObjectHelper.ConvertDataTable<JourneysDataReport>(dt);
                return result;

                }
                catch(Exception ex )
                {
                    return result;
                }
            }
        }
    }
}
