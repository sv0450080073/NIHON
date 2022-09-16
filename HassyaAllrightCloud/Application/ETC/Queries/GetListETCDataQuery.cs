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
    public class GetListETCDataQuery : IRequest<List<ETCData>>
    {
        public ETCSearchParam SearchParam { get; set; }
        public class Hanlder : IRequestHandler<GetListETCDataQuery, List<ETCData>>
        {
            private readonly KobodbContext _context;
            public Hanlder(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<ETCData>> Handle(GetListETCDataQuery request, CancellationToken cancellationToken)
            {
                var searchParam = request.SearchParam;
                List<ETCData> result = new List<ETCData>();

                var connection = _context.Database.GetDbConnection();
                SqlCommand command = new SqlCommand();
                command.Connection = (SqlConnection)connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PK_dETCs_R";

                command.Parameters.AddWithValue("@SyaRyoCompanyCd", searchParam.SelectedCompany?.CompanyCdSeq ?? 0);
                command.Parameters.AddWithValue("@SyaRyoEigyoCd", searchParam.SelectedEigyo?.EigyoCd ?? 0);
                command.Parameters.AddWithValue("@SyaRyoCdFrom", searchParam.SelectedSyaRyoFrom?.SyaRyoCd ?? 0);
                command.Parameters.AddWithValue("@SyaRyoCdTo", searchParam.SelectedSyaRyoTo?.SyaRyoCd ?? 0);
                command.Parameters.AddWithValue("@UnkYmdFrom", searchParam.ETCDateFrom == null ? string.Empty : searchParam.ETCDateFrom.Value.ToString(CommonConstants.FormatYMD));
                command.Parameters.AddWithValue("@UnkYmdTo", searchParam.ETCDateTo == null ? string.Empty : searchParam.ETCDateTo.Value.ToString(CommonConstants.FormatYMD));
                command.Parameters.AddWithValue("@KikoYmdFrom", searchParam.ReturnDateFrom == null ? string.Empty : searchParam.ReturnDateFrom.Value.ToString(CommonConstants.FormatYMD));
                command.Parameters.AddWithValue("@KikoYmdTo", searchParam.ReturnDateTo == null ? string.Empty : searchParam.ReturnDateTo.Value.ToString(CommonConstants.FormatYMD));
                command.Parameters.AddWithValue("@SortOrder", searchParam.SelectedSortOrder?.Value ?? 0);
                command.Parameters.AddWithValue("@TensoKbn", searchParam.SelectedTensoKbn?.Value ?? 0);
                command.Parameters.AddWithValue("@ListFileName", searchParam.ListFileName == null ? string.Empty : string.Join(',', searchParam.ListFileName));
                command.Parameters.AddWithValue("@TenantCdSeq", searchParam.SelectedCompany == null ? 0 : searchParam.TenantCdSeq);

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                result = MapTableToObjectHelper.ConvertDataTable<ETCData>(dt);

                await command.Connection.CloseAsync();
                adapter.Dispose();

                return result;
            }
        }
    }
}
