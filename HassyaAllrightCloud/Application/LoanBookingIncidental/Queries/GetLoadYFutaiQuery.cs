using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.LoanBookingIncidental.Queries
{
    public class GetLoadYFutaiQuery : IRequest<List<LoadYFutai>>
    {
        private readonly int _tenantId;

        public GetLoadYFutaiQuery(int tenantId)
        {
            _tenantId = tenantId;
        }

        public class Handler : IRequestHandler<GetLoadYFutaiQuery, List<LoadYFutai>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            //Edited linq query on #7504
            public async Task<List<LoadYFutai>> Handle(GetLoadYFutaiQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await (from futai in _context.VpmFutai
                                        where futai.TenantCdSeq == request._tenantId && futai.SiyoKbn == 1//&& futai.FutGuiKbn != 5
                                        orderby futai.FutaiCd
                                        select new LoadYFutai
                                        {
                                            FutaiCd = futai.FutaiCd,
                                            FutaiCdSeq = futai.FutaiCdSeq,
                                            FutaiNm = futai.FutaiNm,
                                            FutGuiKbn = futai.FutGuiKbn
                                        }).ToListAsync();
                    
                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
