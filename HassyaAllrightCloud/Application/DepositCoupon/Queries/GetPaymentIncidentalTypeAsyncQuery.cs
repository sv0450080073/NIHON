using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto.BillPrint;
using HassyaAllrightCloud.Domain.Dto.DepositCoupon;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BillPrint.Queries
{
    public class GetPaymentIncidentalTypeAsyncQuery : IRequest<string>
    {
        public int tenantCdSeq { get; set; }
        public byte seiFutSyu { get; set; }
        public class Handler : IRequestHandler<GetPaymentIncidentalTypeAsyncQuery, string>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<string> Handle(GetPaymentIncidentalTypeAsyncQuery request, CancellationToken cancellationToken = default)
            {
                var result = string.Empty;
                await _context.LoadStoredProc("PK_dPaymentIncidentalType_R")
                    .AddParam("@TenantCdSeq", request.tenantCdSeq)
                    .AddParam("@SeiFutSyu", request.seiFutSyu)
                    .AddParam("ROWCOUNT", out IOutParam<int> rowCount)
                    .ExecAsync(async r =>
                    {
                        if (await r.ReadAsync())
                        {
                            result = (string)r["CodeKbnNm"];
                        }
                        await r.CloseAsync();
                    });
                return result;
            }
        }
    }
}
