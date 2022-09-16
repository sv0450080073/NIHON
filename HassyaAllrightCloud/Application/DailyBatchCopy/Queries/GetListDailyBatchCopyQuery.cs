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

namespace HassyaAllrightCloud.Application.DailyBatchCopy.Queries
{
    public class GetListDailyBatchCopyQuery : IRequest<List<DailyBatchCopyData>>
    {
        public int TenantCdSeq { get; set; }
        public string UkeNo { get; set; }
        public class Handler : IRequestHandler<GetListDailyBatchCopyQuery, List<DailyBatchCopyData>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<DailyBatchCopyData>> Handle(GetListDailyBatchCopyQuery request, CancellationToken cancellationToken)
            {
                List<DailyBatchCopyData> result = new List<DailyBatchCopyData>();

                var connection = _context.Database.GetDbConnection();
                SqlCommand command = new SqlCommand();
                command.Connection = (SqlConnection)connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PK_dDailyBatchCopyList_R";

                command.Parameters.AddWithValue("@UkeNo", request.UkeNo);
                command.Parameters.AddWithValue("@TenantCdSeq", request.TenantCdSeq);

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                result = MapTableToObjectHelper.ConvertDataTable<DailyBatchCopyData>(dt);

                await command.Connection.CloseAsync();
                adapter.Dispose();

                return result;
            }
        }
    }
}
