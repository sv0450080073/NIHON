using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.TransportActualResult.Queries
{
    public class GetCodeKb : IRequest<List<CodeKbDataItem>>
    {
        public int TenantId { get; set; }
        public string CodeSyu { get; set; }
        public class Handler : IRequestHandler<GetCodeKb, List<CodeKbDataItem>>
        {
            private readonly KobodbContext _kobodbContext;

            public Handler(KobodbContext kobodbContext)
            {
                _kobodbContext = kobodbContext;
            }

            /// <summary>
            /// Get Code Kb base on Current Tenant Id and CodeSyu = "UNSOUKBN"
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<List<CodeKbDataItem>> Handle(GetCodeKb request, CancellationToken cancellationToken)
            {
                var tenantId = _kobodbContext.VpmCodeKb.Where(e => e.CodeSyu == request.CodeSyu && e.SiyoKbn == 1 && e.TenantCdSeq == request.TenantId)
                    .Any() ? request.TenantId : 0;
                var query = _kobodbContext.VpmCodeKb.Where(e => e.CodeSyu == request.CodeSyu && e.SiyoKbn == 1 && e.TenantCdSeq == tenantId)
                    .Select(e => new CodeKbDataItem()
                    {
                        CodeKbn = e.CodeKbn,
                        CodeKbnNm = e.CodeKbnNm
                    });
                return await query.ToListAsync(cancellationToken);
            }
        }
    }
}
