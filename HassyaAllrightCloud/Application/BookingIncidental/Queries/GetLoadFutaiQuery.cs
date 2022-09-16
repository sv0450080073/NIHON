using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HassyaAllrightCloud.Application.BookingIncidental.Queries
{
    public class GetLoadFutaiQuery : IRequest<List<LoadFutai>>
    {
        public readonly int _tenantId;

        public GetLoadFutaiQuery(int tenantId)
        {
            _tenantId = tenantId;
        }

        public class Handler : IRequestHandler<GetLoadFutaiQuery, List<LoadFutai>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetLoadFutaiQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetLoadFutaiQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<List<LoadFutai>> Handle(GetLoadFutaiQuery request, CancellationToken cancellationToken)
            {
                var result = new List<LoadFutai>();
                try
                {
                    result = await _context.VpmFutai.Where(s => s.SiyoKbn == 1 && s.FutGuiKbn != 5 && s.TenantCdSeq == request._tenantId)
                        .Select(s => new LoadFutai()
                        {
                            Futaicd = s.FutaiCd,
                            FutaiNm = s.FutaiNm,
                            RyakuNm = s.RyakuNm,
                            FutaiCdSeq = s.FutaiCdSeq,
                            ZeiHyoKbn = s.ZeiHyoKbn
                        }).OrderBy(s => s.Futaicd).ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return null;
                }
                return result;
            }
        }
    }
}
