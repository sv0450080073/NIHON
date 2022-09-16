using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.VenderRequestReport.Queries
{
    public class GetTopOneKyoSetQuery : IRequest<VpmKyoSet>
    {
        public class Handler : IRequestHandler<GetTopOneKyoSetQuery, VpmKyoSet>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetTopOneKyoSetQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetTopOneKyoSetQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<VpmKyoSet> Handle(GetTopOneKyoSetQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    return await
                        _context.VpmKyoSet.AsNoTracking()
                                             .OrderBy(_ => _.SetteiCd)
                                             .Take(1)
                                             .FirstOrDefaultAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());

                    return null;
                }
            }
        }
    }
}
