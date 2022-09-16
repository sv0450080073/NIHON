using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.Staff.Queries
{
    public class GetPopupValueQuery : IRequest<List<PopupValue>>
    {
        public int TenantCdSeq { get; set; }
        public class Handler : IRequestHandler<GetPopupValueQuery, List<PopupValue>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<PopupValue>> Handle(GetPopupValueQuery request, CancellationToken cancellationToken)
            {
                List<PopupValue> result = new List<PopupValue>();

                await _context.LoadStoredProc("dbo.PK_dGetRulePopupValue_R")
                              .AddParam("@TenantCdSeq", request.TenantCdSeq)
                              .ExecAsync(async rows => result = await rows.ToListAsync<PopupValue>());

                return result;
            }
        }
    }
}
