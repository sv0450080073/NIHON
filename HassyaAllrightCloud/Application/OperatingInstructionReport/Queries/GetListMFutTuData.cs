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

namespace HassyaAllrightCloud.Application.OperatingInstructionReport.Queries
{
    public class GetListMFutTuData: IRequest<List<FuttumData>>
    {
        public string ukeNo { get; set; } = "";
        public short unkren { get; set; } = 0;
        public short bunkRen { get; set; } = 0;
        public short teiDanNo { get; set; } = 0;
        public byte mode { get; set; } = 1;
    }
    public class Handler : IRequestHandler<GetListMFutTuData, List<FuttumData>>
    {
        private readonly KobodbContext _context;
        public Handler(KobodbContext context)
        {
            _context = context;
        }

        public async Task<List<FuttumData>> Handle(GetListMFutTuData request, CancellationToken cancellationToken)
        {
            List<FuttumData> result = new List<FuttumData>();
            try
            {

                var connection = _context.Database.GetDbConnection();
                SqlCommand command = new SqlCommand();
                command.Connection = (SqlConnection)connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Pro_GetMFutTu_R";
                command.Parameters.AddWithValue("@UkeNo", request.ukeNo);
                command.Parameters.AddWithValue("@Unkren", request.unkren);
                command.Parameters.AddWithValue("@BunkRen", request.bunkRen);
                command.Parameters.AddWithValue("@TeiDanNo", request.teiDanNo);
                command.Parameters.AddWithValue("@Mode", request.mode);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                await command.Connection.CloseAsync();
                result = MapTableToObjectHelper.ConvertDataTable<FuttumData>(dt);
                return result;

            }
            catch (Exception)
            {
                return result;
            }
        }
    }
}
