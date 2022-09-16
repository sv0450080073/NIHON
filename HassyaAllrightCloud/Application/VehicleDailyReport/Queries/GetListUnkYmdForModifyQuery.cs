using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.VehicleDailyReport.Queries
{
    public class GetListUnkYmdForModifyQuery : IRequest<List<string>>
    {
        public string UkeNo { get; set; }
        public class Handler : IRequestHandler<GetListUnkYmdForModifyQuery, List<string>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            /// <summary>
            /// Get list unkymd for insert/update
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<List<string>> Handle(GetListUnkYmdForModifyQuery request, CancellationToken cancellationToken)
            {
                List<string> result = new List<string>();

                result = await _context.TkdShabni.Where(_ => _.UkeNo == request.UkeNo).Select(_ => _.UnkYmd.Substring(2).Insert(2, "/").Insert(5, "/")).Distinct().ToListAsync();

                return result;
            }
        }
    }
}
