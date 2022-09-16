using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.RevenueSummary.Queries
{
    public class GetYoyaKbns : IRequest<IEnumerable<YoyaKbnDto>>
    {
        public int TenantId { get; set; }
        public class Handler : IRequestHandler<GetYoyaKbns, IEnumerable<YoyaKbnDto>>
        {
            private KobodbContext _kobodbContext;
            public Handler(KobodbContext kobodbContext)
            {
                _kobodbContext = kobodbContext;
            }
            public async Task<IEnumerable<YoyaKbnDto>> Handle(GetYoyaKbns request, CancellationToken cancellationToken)
            {
                var yoyaKbnSortList = await _kobodbContext.VpmYoyaKbnSort.Where(e => e.TenantCdSeq == request.TenantId).ToListAsync();
                var yoyKbnList = await _kobodbContext.VpmYoyKbn.Where(e => e.SiyoKbn == 1).ToListAsync();
                return (from yoyKbn in yoyKbnList
                        join yoyaKbnSort in yoyaKbnSortList
                       on yoyKbn.YoyaKbnSeq equals yoyaKbnSort.YoyaKbnSeq into result
                        from r in result.DefaultIfEmpty()
                        select new YoyaKbnDto()
                        {
                            YoyaKbn = yoyKbn.YoyaKbn,
                            YoyaKbnNm = yoyKbn.YoyaKbnNm,
                            YoyaKbnSeq = yoyKbn.YoyaKbnSeq,
                            PriorityNum = r == null ? $"99{yoyKbn.YoyaKbn:00}" : $"{r.PriorityNum:00}{yoyKbn.YoyaKbn:00}"
                        }).OrderBy(r => r.PriorityNum).ToList();
            }
        }
    }
}
