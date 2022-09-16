using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.IService;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BookingIncidental.Queries
{
    public class GetLoadTsumiQuery : IRequest<List<LoadTsumi>>
    {
        public readonly int _tenantId;
        private readonly ITPM_CodeSyService _codeSyuService;

        public GetLoadTsumiQuery(ITPM_CodeSyService codeSyuService, int tenantId)
        {
            _tenantId = tenantId;
            _codeSyuService = codeSyuService ?? throw new ArgumentNullException(nameof(codeSyuService));
        }

        public class Handler : IRequestHandler<GetLoadTsumiQuery, List<LoadTsumi>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<LoadTsumi>> Handle(GetLoadTsumiQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await
                            request._codeSyuService.FilterTenantIdByCodeSyu(async (tenantId, codeSyu) => {
                                return await
                                     _context.VpmCodeKb.Where(s => s.CodeSyu == codeSyu && 
                                                                   s.SiyoKbn == 1 && 
                                                                   s.TenantCdSeq == tenantId)
                                                       .Select(s => new LoadTsumi()
                                                       {
                                                           CodeKbn = s.CodeKbn,
                                                           CodeKbnNm = s.CodeKbnNm,
                                                           RyakuNm = s.RyakuNm,
                                                           CodeKbnSeq = s.CodeKbnSeq
                                                       })
                                                       .OrderBy(s => s.CodeKbn)
                                                       .ToListAsync();
                            }, request._tenantId, "TUMIKCD");

                    return result ?? new List<LoadTsumi>();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
