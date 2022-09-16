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

namespace HassyaAllrightCloud.Application.ETC.Queries
{
    public class GetListYoyakuDataQuery : IRequest<List<ETCYoyakuData>>
    {
        public ETCSearchParam SearchParam { get; set; }
        public class Handler : IRequestHandler<GetListYoyakuDataQuery, List<ETCYoyakuData>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<ETCYoyakuData>> Handle(GetListYoyakuDataQuery request, CancellationToken cancellationToken)
            {
                var searchParam = request.SearchParam;
                List<ETCYoyakuData> result = new List<ETCYoyakuData>();

                var connection = _context.Database.GetDbConnection();
                SqlCommand command = new SqlCommand();
                command.Connection = (SqlConnection)connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PK_dYoyakuInfos_R";

                command.Parameters.AddWithValue("@UnkYmdFrom", searchParam.ETCDateFrom == null ? string.Empty : searchParam.ETCDateFrom.Value.ToString(CommonConstants.FormatYMD));
                command.Parameters.AddWithValue("@UnkYmdTo", searchParam.ETCDateTo == null ? string.Empty : searchParam.ETCDateTo.Value.ToString(CommonConstants.FormatYMD));
                command.Parameters.AddWithValue("@KikoYmdFrom", searchParam.ReturnDateFrom == null ? string.Empty : searchParam.ReturnDateFrom.Value.ToString(CommonConstants.FormatYMD));
                command.Parameters.AddWithValue("@KikoYmdTo", searchParam.ReturnDateTo == null ? string.Empty : searchParam.ReturnDateTo.Value.ToString(CommonConstants.FormatYMD));
                command.Parameters.AddWithValue("@SyaryoCd", searchParam.SyaryoCd);
                command.Parameters.AddWithValue("@TenantCdSeq", searchParam.TenantCdSeq);
                command.Parameters.AddWithValue("@ScreenType", searchParam.ScreenType);

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                result = MapTableToObjectHelper.ConvertDataTable<ETCYoyakuData>(dt);

                await command.Connection.CloseAsync();
                adapter.Dispose();

                return result;
            }
        }
    }
}
