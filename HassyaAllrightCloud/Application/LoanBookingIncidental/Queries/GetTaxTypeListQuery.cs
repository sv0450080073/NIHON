using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HassyaAllrightCloud.IService;
using Microsoft.EntityFrameworkCore;

namespace HassyaAllrightCloud.Application.LoanBookingIncidental.Queries
{
    public class GetTaxTypeListQuery : IRequest<List<TaxTypeList>>
    {
        private readonly int _tenantId;

        public GetTaxTypeListQuery(int tenantId)
        {
            _tenantId = tenantId;
        }

        public class Handler : IRequestHandler<GetTaxTypeListQuery, List<TaxTypeList>>
        {
            private readonly KobodbContext _context;
            private readonly ITPM_CodeSyService _codeSyuService;

            public Handler(KobodbContext context, ITPM_CodeSyService codeSyuService)
            {
                _context = context;
                _codeSyuService = codeSyuService;
            }

            public async Task<List<TaxTypeList>> Handle(GetTaxTypeListQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await
                        _codeSyuService.FilterTenantIdByCodeSyu((tenantId, codeSyu) =>
                        {
                            return 
                                _context.VpmCodeKb
                                    .Where(c => c.CodeSyu == codeSyu && c.SiyoKbn == 1 && c.TenantCdSeq == tenantId)
                                    .Select(c => new TaxTypeList()
                                    {
                                        IdValue = int.Parse(c.CodeKbn),
                                        StringValue = c.RyakuNm
                                    }).ToListAsync();
                        }, request._tenantId, "FUTZEIKBN");

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
