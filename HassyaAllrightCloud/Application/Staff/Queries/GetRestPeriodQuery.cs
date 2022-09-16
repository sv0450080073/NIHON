using HassyaAllrightCloud.Commons;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.Data.SqlClient;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.Staff.Queries
{
    public class GetRestPeriodQuery : IRequest<List<RestPeriod>>
    {
        public TimeSearchParam SearchParam { get; set; }
        public class Handler : IRequestHandler<GetRestPeriodQuery, List<RestPeriod>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<RestPeriod>> Handle(GetRestPeriodQuery request, CancellationToken cancellationToken)
            {
                List<RestPeriod> result = new List<RestPeriod>();

                var sqlParam = new SqlParameter("@KobanTable", SqlDbType.Structured);
                sqlParam.Value = request.SearchParam.KobanTableType;
                sqlParam.TypeName = "KobanTableType";

                var date = DateTime.ParseExact(request.SearchParam.UnkYmd, DateTimeFormat.yyyyMMdd, CultureInfo.InvariantCulture).AddDays(-1).ToString(DateTimeFormat.yyyyMMdd);
                await _context.LoadStoredProc("dbo.PK_dGetRestPeriod_R")
                             .AddParam("@CompanyCdSeq", request.SearchParam.CompanyCdSeq)
                             .AddParam("@SyainCdSeq", request.SearchParam.SyainCdSeq)
                             .AddParam("@UnkYmd", date)
                             .AddParam("@DriverNaikinOnly", request.SearchParam.DriverNaikinOnly)
                             .AddParam("@DelUkeNo", request.SearchParam.DelUkeNo)
                             .AddParam("@DelUnkRen", request.SearchParam.DelUnkRen)
                             .AddParam("@DelTeiDanNo", request.SearchParam.DelTeiDanNo)
                             .AddParam("@DelBunkRen", request.SearchParam.DelBunkRen)
                             .AddParam(sqlParam)
                             .ExecAsync(async rows => result = await rows.ToListAsync<RestPeriod>());

                return result;
            }
        }
    }
}
