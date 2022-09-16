using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.Staff.Queries
{
    public class GetListSyuTaikinCalculationTimeQuery : IRequest<List<SyuTaikinCalculationTimeItem>>
    {
        public int CompanyCdSeq { get; set; }
        public class Handler : IRequestHandler<GetListSyuTaikinCalculationTimeQuery, List<SyuTaikinCalculationTimeItem>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<SyuTaikinCalculationTimeItem>> Handle(GetListSyuTaikinCalculationTimeQuery request, CancellationToken cancellationToken)
            {
                List<SyuTaikinCalculationTimeItem> result = new List<SyuTaikinCalculationTimeItem>();

                try
                {
                    result = await _context.VpmSyuTaikinCalculationTime.Where(_ => _.CompanyCdSeq == request.CompanyCdSeq && _.SyugyoKbn == 1)
                                                                       .Select(_ => new SyuTaikinCalculationTimeItem()
                                                                       {
                                                                           KouZokPtnKbn = _.KouZokPtnKbn,
                                                                           SyukinCalculationTimeMinutes = _.SyukinCalculationTimeMinutes,
                                                                           TaikinCalculationTimeMinutes = _.TaikinCalculationTimeMinutes
                                                                       }).ToListAsync();
                    return result;
                }
                catch(Exception ex)
                {
                    // TODO: write log
                    throw ex;
                }
            }
        }
    }
}
