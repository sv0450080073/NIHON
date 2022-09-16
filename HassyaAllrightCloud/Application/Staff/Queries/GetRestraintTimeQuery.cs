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
    public class GetRestraintTimeQuery : IRequest<List<RestraintTime>>
    {
        public TimeSearchParam SearchParam { get; set; }
        public int Period { get; set; }
        public class Handler : IRequestHandler<GetRestraintTimeQuery, List<RestraintTime>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<RestraintTime>> Handle(GetRestraintTimeQuery request, CancellationToken cancellationToken)
            {
                List<RestraintTime> result = new List<RestraintTime>();

                string KaizenKijunYmd = _context.TkmKasSet.FirstOrDefault(_ => _.CompanyCdSeq == request.SearchParam.CompanyCdSeq)?.KaizenKijunYmd;
                if (string.IsNullOrEmpty(KaizenKijunYmd) || string.IsNullOrWhiteSpace(KaizenKijunYmd))
                {
                    return null;
                }

                var sqlParam = new SqlParameter("@KobanTable", SqlDbType.Structured);
                sqlParam.Value = request.SearchParam.KobanTableType;
                sqlParam.TypeName = "KobanTableType";

                await _context.LoadStoredProc("dbo.PK_dGetListRestraintTime_R")
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
                                .ExecAsync(async rows => result = await rows.ToListAsync<RestraintTime>());

                return result;
            }
        }
    }
}
