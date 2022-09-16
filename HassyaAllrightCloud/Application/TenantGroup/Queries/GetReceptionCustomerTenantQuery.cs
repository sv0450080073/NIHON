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
    public class GetReceptionCustomerTenantQuery : IRequest<TenantGroupData>
    {
        private readonly int _tenantCdSeq;
        private readonly int _tenantGroupCdSeq;
        private readonly int _sitenCdSeqTenantCdSeq;
        public GetReceptionCustomerTenantQuery(int tenantCdSeq, int tenantGroupCdSeq, int sitenCdSeqTenantCdSeq)
        {
            _tenantCdSeq = tenantCdSeq;
            _tenantGroupCdSeq = tenantGroupCdSeq;
            _sitenCdSeqTenantCdSeq = sitenCdSeqTenantCdSeq;
        }
        public class Handler : IRequestHandler<GetReceptionCustomerTenantQuery, TenantGroupData>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetReceptionCustomerTenantQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetReceptionCustomerTenantQuery> logger)
            {
                _context = context;
                _logger = logger;
            }
            public async Task<TenantGroupData> Handle(GetReceptionCustomerTenantQuery request, CancellationToken cancellationToken)
            {
                return await (from TGDTOKUI in _context.VpmTenantGroupDetailTokui
                              join TGDETAIL in _context.VpmTenantGroupDetail
                                    on new { TGDTOKUI.TenantGroupCdSeq, TenantCdSeq = TGDTOKUI.SitenCdSeqTenantCdSeq, SiyoKbn = 1 }
                                equals new { TGDETAIL.TenantGroupCdSeq, TGDETAIL.TenantCdSeq, SiyoKbn = (int)TGDETAIL.SiyoKbn } into TGDETAIL_join
                              from TGDETAIL in TGDETAIL_join.DefaultIfEmpty()
                              where
                                TGDTOKUI.TenantCdSeq == request._tenantCdSeq &&
                                TGDTOKUI.TenantGroupCdSeq == request._tenantGroupCdSeq &&
                                TGDTOKUI.SitenCdSeqTenantCdSeq == request._sitenCdSeqTenantCdSeq &&
                                TGDTOKUI.SiyoKbn == 1
                              select new TenantGroupData
                              {
                                  TenantGroupCdSeq = TGDTOKUI.TenantGroupCdSeq,
                                  TenantCdSeq = TGDTOKUI.TenantCdSeq,
                                  TokuiSeq = TGDTOKUI.TokuiSeq,
                                  SitenCdSeq = TGDTOKUI.SitenCdSeq,
                                  SitenCdSeqTenantCdSeq = TGDTOKUI.SitenCdSeqTenantCdSeq
                              }).FirstOrDefaultAsync();
            }
        }
    }
}
