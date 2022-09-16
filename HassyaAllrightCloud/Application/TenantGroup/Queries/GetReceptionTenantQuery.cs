using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.IService;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.TenantGroup.Queries
{
    public class GetReceptionTenantQuery: IRequest<TenantGroupData>
    {
        private readonly int _tenantCdSeq;
        private readonly int _tokuiSeq;
        private readonly int _sitenCdSeq;
        public GetReceptionTenantQuery(int tenantCdSeq, int tokuiSeq, int sitenCdSeq)
        {
            _tenantCdSeq = tenantCdSeq;
            _tokuiSeq = tokuiSeq;
            _sitenCdSeq = sitenCdSeq;
        }
        public class Handler : IRequestHandler<GetReceptionTenantQuery, TenantGroupData>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetReceptionTenantQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetReceptionTenantQuery> logger)
            {
                _context = context;
                _logger = logger;
            }
            public async Task<TenantGroupData> Handle(GetReceptionTenantQuery request, CancellationToken cancellationToken)
            {
                return await (from TGDTOKUI in _context.VpmTenantGroupDetailTokui
                              join TGDETAIL in _context.VpmTenantGroupDetail
                                    on new { TGDTOKUI.TenantGroupCdSeq, TenantCdSeq = TGDTOKUI.SitenCdSeqTenantCdSeq, SiyoKbn = 1 }
                                equals new { TGDETAIL.TenantGroupCdSeq, TGDETAIL.TenantCdSeq, SiyoKbn = (int)TGDETAIL.SiyoKbn } into TGDETAIL_join
                              from TGDETAIL in TGDETAIL_join.DefaultIfEmpty()
                              where
                                TGDTOKUI.TenantCdSeq == request._tenantCdSeq &&
                                TGDTOKUI.TokuiSeq == request._tokuiSeq &&
                                TGDTOKUI.SitenCdSeq == request._sitenCdSeq &&
                                TGDTOKUI.SiyoKbn == 1
                              select new TenantGroupData
                              {
                                  TenantGroupCdSeq=TGDTOKUI.TenantGroupCdSeq,
                                  TenantCdSeq=TGDTOKUI.TenantCdSeq,
                                  TokuiSeq=TGDTOKUI.TokuiSeq,
                                  SitenCdSeq=TGDTOKUI.SitenCdSeq,
                                  SitenCdSeqTenantCdSeq=TGDTOKUI.SitenCdSeqTenantCdSeq
                              }).FirstOrDefaultAsync();
            }
        }
    }
}
