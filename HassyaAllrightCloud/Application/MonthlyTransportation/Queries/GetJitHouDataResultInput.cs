using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.TransportationSummary.Queries
{
    public class GetJitHouDataResultInput : IRequest<IEnumerable<TKDJitHouResultInput>>
    {
        public SearchParam SearchParam { get; set; }
        public class Handler : IRequestHandler<GetJitHouDataResultInput, IEnumerable<TKDJitHouResultInput>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<TKDJitHouResultInput>> Handle(GetJitHouDataResultInput request, CancellationToken cancellationToken)
            {
                try
                {
                    List<TKDJitHouResultInput> rows = null;
                    await _context.LoadStoredProc("dbo.PK_SpJitHouInput_R")
                              .AddParam("@StrDate", request?.SearchParam?.StrDate ?? "")
                              .AddParam("@CompanyCdSeq", request?.SearchParam?.CompanyCdSeq.ToString() ?? "")
                              .AddParam("@TenantCdSeq", new ClaimModel().TenantID.ToString() ?? "")
                              .AddParam("@StrEigyoCd", request?.SearchParam?.StrEigyoCd.ToString() ?? "")
                              .AddParam("@StrUnsouKbn", request?.SearchParam?.StrUnsouKbn.ToString() ?? "")
                              .AddParam("@ROWCOUNT", out IOutParam<int> retParam)
                          .ExecAsync(async r => rows = await r.ToListAsync<TKDJitHouResultInput>());
                    return rows;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
