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
    public class GetLoadNyuRyokinNameQuery : IRequest<List<LoadNyuRyokinName>>
    {
        public class Handler : IRequestHandler<GetLoadNyuRyokinNameQuery, List<LoadNyuRyokinName>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetLoadNyuRyokinNameQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetLoadNyuRyokinNameQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<List<LoadNyuRyokinName>> Handle(GetLoadNyuRyokinNameQuery request, CancellationToken cancellationToken)
            {
                var result = new List<LoadNyuRyokinName>();
                try
                {
                    string dateAsString = DateTime.Today.ToString("yyyyMMdd");
                    result = await (from s in _context.VpmRyokin
                                    where s.SiyoStaYmd.CompareTo(dateAsString) <= 0 && s.SiyoEndYmd.CompareTo(dateAsString) >= 0
                                    select new LoadNyuRyokinName()
                                    {
                                        RyakuNm = s.RyakuNm,
                                        RyokinCd = s.RyokinCd.Trim(),
                                        RyokinTikuCd = s.RyokinTikuCd.ToString(),
                                        DouroCdSeq = s.RoadCorporationKbn
                                    }).ToListAsync();
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
