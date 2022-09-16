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
    public class GetClientDataQuery : IRequest<ClientData>
    {
        private readonly int _tenantCdSeq;
        private readonly int _tokuiSeq;
        private readonly int _sitenCdSeq;
    public GetClientDataQuery(int tenantCdSeq, int tokuiSeq, int sitenCdSeq)
    {
        _tenantCdSeq = tenantCdSeq;
        _tokuiSeq = tokuiSeq;
        _sitenCdSeq = sitenCdSeq;
    }
    public class Handler : IRequestHandler<GetClientDataQuery, ClientData>
    {
        private readonly KobodbContext _context;
        private readonly ILogger<GetClientDataQuery> _logger;

        public Handler(KobodbContext context, ILogger<GetClientDataQuery> logger)
        {
            _context = context;
            _logger = logger;
        }
            public async Task<ClientData> Handle(GetClientDataQuery request, CancellationToken cancellationToken)
            {
                return await (from Tokisk in _context.VpmTokisk
                              join Tokist in _context.VpmTokiSt on Tokisk.TokuiSeq equals Tokist.TokuiSeq into Tokist_join
                              from Tokist in Tokist_join.DefaultIfEmpty()
                              where
                                Tokisk.TokuiSeq == request._tokuiSeq &&
                                Tokisk.TenantCdSeq == request._tenantCdSeq &&
                                Tokist.SitenCdSeq == request._sitenCdSeq
                              select new ClientData
                              {
                                  TokuiCd = Tokisk.TokuiCd,
                                  TokuiNm = Tokisk.TokuiNm,
                                  SitenCd = Tokist.SitenCd,
                                  SitenNm = Tokist.SitenNm,
                                  TokuiMail=Tokist.TokuiMail
                              }).FirstOrDefaultAsync();
            }
        }
    }
}
