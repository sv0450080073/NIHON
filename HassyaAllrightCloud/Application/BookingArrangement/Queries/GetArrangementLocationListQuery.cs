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

namespace HassyaAllrightCloud.Application.BookingArrangement.Queries
{
    public class GetArrangementLocationListQuery : IRequest<List<ArrangementLocation>>
    {
        private readonly string _ukeNo;
        private readonly int _tenantId;
        private readonly ITPM_CodeSyService _codeSyuService; // Please inject service

        public GetArrangementLocationListQuery(ITPM_CodeSyService codeSyuService, string ukeNo, int tenantId)
        {
            _ukeNo = ukeNo ?? throw new ArgumentNullException(nameof(ukeNo));
            _tenantId = tenantId;
            _codeSyuService = codeSyuService ?? throw new ArgumentNullException(nameof(codeSyuService));
        }

        public class Handler : IRequestHandler<GetArrangementLocationListQuery, List<ArrangementLocation>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<ArrangementLocation>> Handle(GetArrangementLocationListQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = new List<ArrangementLocation>();

                    result = await
                            request._codeSyuService.FilterTenantIdByCodeSyu(async (tenantId, codeSyu) => {
                                return await
                                    (from basho in _context.VpmBasyo
                                     where basho.SiyoKbn == 1 && basho.TenantCdSeq == new ClaimModel().TenantID
                                     select new ArrangementLocation()
                                     {
                                         BasyoKenCdSeq = basho.BasyoKenCdSeq,
                                         BasyoMapCd = basho.BasyoMapCd,
                                         BasyoMapCdSeq = basho.BasyoMapCdSeq,
                                         BasyoNm = basho.BasyoNm,
                                         BunruiCdSeq = basho.BunruiCdSeq
                                     }).ToListAsync();
                            }, request._tenantId, "BUNRUICD");

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
