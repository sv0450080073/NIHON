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
    public class GetLoadDouroListQuery : IRequest<List<LoadDouro>>
    {
        private readonly int _tenantId;
        private readonly ITPM_CodeSyService _codeSyuService;

        public GetLoadDouroListQuery(ITPM_CodeSyService codeSyuService, int tenantId)
        {
            _codeSyuService = codeSyuService ?? throw new ArgumentNullException(nameof(codeSyuService));
            _tenantId = tenantId;
        }

        public class Handler : IRequestHandler<GetLoadDouroListQuery, List<LoadDouro>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<LoadDouro>> Handle(GetLoadDouroListQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await
                            request._codeSyuService.FilterTenantIdByCodeSyu(async (tenantId, codeSyu) => {
                                return await
                                    (from codekb in _context.VpmCodeKb
                                     where codekb.TenantCdSeq == tenantId && 
                                           codekb.SiyoKbn == 1 && 
                                           codekb.CodeSyu == codeSyu
                                     join ryokin in _context.VpmRyokin
                                     on codekb.CodeKbnSeq equals ryokin.RoadCorporationKbn into gr
                                     from ryokin in gr.DefaultIfEmpty()
                                     select new LoadDouro()
                                     {
                                         CodeKbn = codekb.CodeKbn,
                                         CodeKbnSeq = codekb.CodeKbnSeq,
                                         CodeKbnName = codekb.CodeKbnNm
                                     }).Distinct().ToListAsync();
                            }, request._tenantId, "DOUROCD");

                    return result ?? new List<LoadDouro>();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
