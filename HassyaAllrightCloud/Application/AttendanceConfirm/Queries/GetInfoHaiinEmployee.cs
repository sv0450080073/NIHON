using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using HassyaAllrightCloud.Commons.Helpers;

namespace HassyaAllrightCloud.Application.AttendanceConfirm.Queries
{
    public class GetInfoHaiinEmployee : IRequest<List<InfoHaiinEmployee>>
    {
        public string Ukeno { get; set; } = "";
        public string Unkren { get; set; } = "";
        public string TeiDanNo { get; set; } = "";
        public string BunkRen { get; set; } = "";
        public string HaiShaSyuKoYmd { get; set; } = "";
        public string TenantCdSeq { get; set; } = "";
        public class Handler : IRequestHandler<GetInfoHaiinEmployee, List<InfoHaiinEmployee>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<List<InfoHaiinEmployee>> Handle(GetInfoHaiinEmployee request, CancellationToken cancellationToken)
            {
                List<InfoHaiinEmployee> result = new List<InfoHaiinEmployee>();
                try
                {
                    var connection = _context.Database.GetDbConnection();
                    SqlCommand command = new SqlCommand();
                    command.Connection = (SqlConnection)connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Pro_GetInfoHaiinDrive_R";
                    command.Parameters.AddWithValue("@Ukeno", request.Ukeno ?? "");
                    command.Parameters.AddWithValue("@UnkRen", request.Unkren ?? "");
                    command.Parameters.AddWithValue("@TeiDanNo", request.TeiDanNo ?? "");
                    command.Parameters.AddWithValue("@BunkRen", request.BunkRen ?? "");
                    command.Parameters.AddWithValue("@HaiShaSyuKoYmd", request.HaiShaSyuKoYmd ?? "");
                    command.Parameters.AddWithValue("@TenantCdSeq", request.TenantCdSeq ?? "");
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    await command.Connection.CloseAsync();
                    result = MapTableToObjectHelper.ConvertDataTable<InfoHaiinEmployee>(dt);
                    return result;
                }
                catch (Exception ex)
                {
                    return result;
                }
            }
        }
    }
}
