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
    public class GetLoadSeisanCdQuery : IRequest<List<LoadSeisanCd>>
    {
        public class Handler : IRequestHandler<GetLoadSeisanCdQuery, List<LoadSeisanCd>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetLoadSeisanCdQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetLoadSeisanCdQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<List<LoadSeisanCd>> Handle(GetLoadSeisanCdQuery request, CancellationToken cancellationToken)
            {
                var result = new List<LoadSeisanCd>();
                try
                {
                    result = await _context.VpmSeisan.Where(s => s.SiyoKbn == 1 && s.TenantCdSeq == new ClaimModel().TenantID)
                        .Select(s => new LoadSeisanCd()
                        {
                            SeisanCd = s.SeisanCd,
                            RyakuNm = s.RyakuNm,
                            SeisanKbn = s.SeisanKbn,
                            SeisanCdSeq = s.SeisanCdSeq,
                        }).OrderBy(s => s.SeisanCd).ToListAsync();
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
