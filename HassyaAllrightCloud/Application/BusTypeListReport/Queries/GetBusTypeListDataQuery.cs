using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.IService;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HassyaAllrightCloud.Commons.Helpers;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HassyaAllrightCloud.Application.BusTypeListReport.Queries
{
    public class GetBusTypeListDataQuery : IRequest<List<BusTypeItemDataReport>>
    {
        public BusTypeListData BusTypeListDataParam;
        public class Handler : IRequestHandler<GetBusTypeListDataQuery, List<BusTypeItemDataReport>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetBusTypeListDataQuery> _logger;
            public Handler(KobodbContext context, ILogger<GetBusTypeListDataQuery> logger)
            {
                _context = context;
                _logger = logger;
            }
            public async Task<List<BusTypeItemDataReport>> Handle(GetBusTypeListDataQuery request, CancellationToken cancellationToken)
            {
                List<BusTypeItemDataReport> result = new List<BusTypeItemDataReport>();
                try
                {
                    var param = request.BusTypeListDataParam;
                    int valueKataKbn = 0;
                    if (!int.TryParse(param.BusType.CodeKbn, out valueKataKbn))
                    {
                        valueKataKbn = 0;
                    }
                    var connection = _context.Database.GetDbConnection();
                    SqlCommand command = new SqlCommand();
                    command.Connection = (SqlConnection)connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Pro_GetBusTypeList_R";
                    command.Parameters.AddWithValue("@CompanyCdSeq", param.Company?.CompanyCdSeq??0);
                    command.Parameters.AddWithValue("@EigyoCdFrom", param.BranchStart?.EigyoCd ?? 0);
                    command.Parameters.AddWithValue("@EigyoCdTo", param.BranchEnd?.EigyoCd ?? 0);
                    command.Parameters.AddWithValue("@DateHenSyaFrom", param.StartDate.ToString("yyyyMMdd"));
                    command.Parameters.AddWithValue("@DateHenSyaTo", param.StartDate.AddDays(param.numberDay-1).ToString("yyyyMMdd"));
                    command.Parameters.AddWithValue("@SyaSyuCdFrom", param.VehicleFrom?.SyaSyuCd??0);
                    command.Parameters.AddWithValue("@SyaSyuCdTo", param.VehicleTo?.SyaSyuCd??0);
                    command.Parameters.AddWithValue("@KataKbn", valueKataKbn);
                    command.Parameters.AddWithValue("@TenantCdSeq", param.TenantCdSeq);
                    command.Parameters.AddWithValue("@TenantCdSeqByCodeSyu", param.TenantCdSeqByCodeSyu);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    await command.Connection.CloseAsync();
                    result = MapTableToObjectHelper.ConvertDataTable<BusTypeItemDataReport>(dt);
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
