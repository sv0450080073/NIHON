using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.Data.SqlClient;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.Staff.Queries
{
    public class GetHolidayQuery : IRequest<List<Holiday>>
    {
        public TimeSearchParam SearchParam { get; set; }
        public int Period { get; set; }
        public class Handler : IRequestHandler<GetHolidayQuery, List<Holiday>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<Holiday>> Handle(GetHolidayQuery request, CancellationToken cancellationToken)
            {
                List<Holiday> result = new List<Holiday>();

                string KaizenKijunYmd = _context.TkmKasSet.FirstOrDefault(_ => _.CompanyCdSeq == request.SearchParam.CompanyCdSeq)?.KaizenKijunYmd;
                if (string.IsNullOrEmpty(KaizenKijunYmd) || string.IsNullOrWhiteSpace(KaizenKijunYmd))
                {
                    return null;
                }

                var sqlParam = new SqlParameter("@KobanTable", SqlDbType.Structured);
                sqlParam.Value = request.SearchParam.KobanTableType;
                sqlParam.TypeName = "KobanTableType";

                await _context.LoadStoredProc("dbo.PK_dGetHoliday_R")
                             .AddParam("@CompanyCdSeq", request.SearchParam.CompanyCdSeq)
                             .AddParam("@SyainCdSeq", request.SearchParam.SyainCdSeq)
                             .AddParam("@UnkYmd", request.SearchParam.UnkYmd)
                             .AddParam("@Period", request.Period)
                             .AddParam("@Times", request.SearchParam.Times)
                             .AddParam("@RefDate", KaizenKijunYmd)
                             .AddParam("@DriverNaikinOnly", request.SearchParam.DriverNaikinOnly)
                             .AddParam("@DelUkeNo", request.SearchParam.DelUkeNo)
                             .AddParam("@DelUnkRen", request.SearchParam.DelUnkRen)
                             .AddParam("@DelTeiDanNo", request.SearchParam.DelTeiDanNo)
                             .AddParam("@DelBunkRen", request.SearchParam.DelBunkRen)
                             .AddParam(sqlParam)
                             .ExecAsync(async rows => result = await rows.ToListAsync<Holiday>());

                return result;
            }
        }
    }
}
