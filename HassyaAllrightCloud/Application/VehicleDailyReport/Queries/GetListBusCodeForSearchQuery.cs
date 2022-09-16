using HassyaAllrightCloud.Domain.Dto;
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
    public class GetListBusCodeForSearchQuery : IRequest<List<BusCodeSearch>>
    {
        public class Handler : IRequestHandler<GetListBusCodeForSearchQuery, List<BusCodeSearch>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            /// <summary>
            /// Get list bus code for search
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<List<BusCodeSearch>> Handle(GetListBusCodeForSearchQuery request, CancellationToken cancellationToken)
            {
                var result = await (from s in _context.VpmSyaRyo
                                    orderby s.SyaRyoCd
                                    select new BusCodeSearch()
                                    {
                                        SyaRyoCdSeq = s.SyaRyoCdSeq,
                                        SyaRyoCd = s.SyaRyoCd,
                                        SyaRyoNm = s.SyaRyoNm
                                    }).ToListAsync();
                return result;
            }
        }
    }
}
