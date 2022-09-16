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
    public class GetCompanyNameForSearchQuery : IRequest<string>
    {
        public int TenantCdSeq { get; set; }
        public int CompanyCdSeq { get; set; }
        public class Handler : IRequestHandler<GetCompanyNameForSearchQuery, string>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            /// <summary>
            /// Get company name for search
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<string> Handle(GetCompanyNameForSearchQuery request, CancellationToken cancellationToken)
            {
                var result = await (from c in _context.VpmCompny
                                    where c.TenantCdSeq == request.TenantCdSeq
                                            && c.CompanyCdSeq == request.CompanyCdSeq
                                    select $"{c.CompanyCd:00000} : {c.RyakuNm}").FirstOrDefaultAsync();
                return result;
            }
        }
    }
}
