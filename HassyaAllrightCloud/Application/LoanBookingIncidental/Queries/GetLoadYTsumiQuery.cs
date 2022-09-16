using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HassyaAllrightCloud.IService;

namespace HassyaAllrightCloud.Application.LoanBookingIncidental.Queries
{
    public class GetLoadYTsumiQuery : IRequest<List<LoadYTsumi>>
    {
        public class Handler : IRequestHandler<GetLoadYTsumiQuery, List<LoadYTsumi>>
        {
            private readonly KobodbContext _context;
            private readonly ITPM_CodeSyService _codeSyuService;

            public Handler(KobodbContext context, ITPM_CodeSyService codeSyuService)
            {
                _context = context;
                _codeSyuService = codeSyuService;
            }

            public async Task<List<LoadYTsumi>> Handle(GetLoadYTsumiQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await
                        _codeSyuService.FilterTenantIdByCodeSyu((tenantId, codeSyu) =>
                        {
                            return 
                                (from tsumi in _context.VpmCodeKb
                                        where tsumi.CodeSyu == codeSyu && tsumi.SiyoKbn == 1 && tsumi.TenantCdSeq == tenantId
                                        select new LoadYTsumi
                                        {
                                            CodeKbn = tsumi.CodeKbn,
                                            CodeKbnNm = tsumi.CodeKbnNm,
                                            CodeKbnSeq = tsumi.CodeKbnSeq
                                        }).OrderBy(t => t.CodeKbn).ToListAsync();
                        }, new ClaimModel().TenantID, "TUMIKCD");
                    
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
