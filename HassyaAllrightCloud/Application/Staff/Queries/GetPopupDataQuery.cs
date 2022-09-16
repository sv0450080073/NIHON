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
    public class GetPopupDataQuery : IRequest<List<PopupData>>
    {
        public class Handler : IRequestHandler<GetPopupDataQuery, List<PopupData>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<PopupData>> Handle(GetPopupDataQuery request, CancellationToken cancellationToken)
            {
                List<PopupData> result = new List<PopupData>();

                await _context.LoadStoredProc("dbo.PK_dGetRulePopupData_R")
                             .ExecAsync(async rows => result = await rows.ToListAsync<PopupData>());

                return result;
            }
        }
    }
}
