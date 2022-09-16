using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.IService;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.CancelListReport.Queries
{
    public class GetCancelListReportQuery : IRequest<List<CancelListReportData>>
    {
        private readonly List<BookingKeyData> _bookingKeys;
        private readonly int _tenantId;
        private readonly ITPM_CodeSyService _codeSyService;

        public GetCancelListReportQuery(List<BookingKeyData> bookingKeys, int tenantId, 
            ITPM_CodeSyService codeSyService)
        {
            _bookingKeys = bookingKeys;
            _tenantId = tenantId;
            _codeSyService = codeSyService;
        }

        public class Handler : IRequestHandler<GetCancelListReportQuery, List<CancelListReportData>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetCancelListReportQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetCancelListReportQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<List<CancelListReportData>> Handle(GetCancelListReportQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var connection = _context.Database.GetDbConnection();
                    SqlCommand command = new SqlCommand();
                    command.Connection = (SqlConnection)connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Pro_CancelList_R";
                    command.Parameters.AddWithValue("@bookingKeys", request._bookingKeys.Select(_ => new { _.UkeNo, _.UnkRen }).ToDataTable());
                    command.Parameters.AddWithValue("@tenantId", request._tenantId);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    await command.Connection.CloseAsync();

                    var data = MapTableToObjectHelper.ConvertDataTable<CancelListReportData>(dt);

                    return await request._codeSyService.FilterTenantIdByListCodeSyu((tenantIds, codeSyus) =>
                    {
                        return Task.FromResult(
                            (from main in data.AsEnumerable()
                             join codeKata in _context.VpmCodeKb on new { CodeKbn = Convert.ToInt32(main.KataCode), CodeSyu = codeSyus[0], TenantCdSeq = tenantIds[0] }  equals new { CodeKbn = Convert.ToInt32(codeKata.CodeKbn), codeKata.CodeSyu, codeKata.TenantCdSeq } into ck
                             from subCk in ck.DefaultIfEmpty()
                             let temp = main.KataKbn = subCk?.CodeKbnNm ?? string.Empty
                             join codeZei in _context.VpmCodeKb on new { CodeKbn = Convert.ToInt32(main.ZeiKbn), CodeSyu = codeSyus[1], TenantCdSeq = tenantIds[1] } equals new { CodeKbn = Convert.ToInt32(codeZei.CodeKbn), codeZei.CodeSyu, codeZei.TenantCdSeq } into cz
                             from subCz in cz.DefaultIfEmpty()
                             let temp1 = main.ZeiRyakuNm = subCz?.RyakuNm ?? string.Empty
                             join codeCan in _context.VpmCodeKb on new { CodeKbn = Convert.ToInt32(main.CanZKbn), CodeSyu = codeSyus[2], TenantCdSeq = tenantIds[2] } equals new { CodeKbn = Convert.ToInt32(codeCan.CodeKbn), codeCan.CodeSyu, codeCan.TenantCdSeq } into cc
                             from subCc in cc.DefaultIfEmpty()
                             let temp2 = main.CanZRyakuNm = subCc?.RyakuNm ?? string.Empty
                             select main).ToList());
                    }, request._tenantId, new List<string> { "KATAKBN", "ZEIKBN", "CANZKBN" });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());

                    return new List<CancelListReportData>();
                }
            }
        }
    }
}
