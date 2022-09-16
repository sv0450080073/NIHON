using HassyaAllrightCloud.Commons.Constants;
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
    public class GetEigyoForSearchQuery : IRequest<List<EigyoStaffItem>>
    {
        public int CompanyCdSeq { get; set; }
        public class Handler : IRequestHandler<GetEigyoForSearchQuery, List<EigyoStaffItem>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<EigyoStaffItem>> Handle(GetEigyoForSearchQuery request, CancellationToken cancellationToken)
            {
                List<EigyoStaffItem> result = new List<EigyoStaffItem>();

                try
                {
                    result = await _context.VpmEigyos.Where(_ => _.SiyoKbn == (byte)CommonConstants.SiyoKbn && _.CompanyCdSeq == request.CompanyCdSeq)
                                                     .OrderBy(_ => _.EigyoCd)
                                                     .Select(_ => new EigyoStaffItem()
                                                     {
                                                         EigyoCdSeq = _.EigyoCdSeq,
                                                         EigyoCd = _.EigyoCd,
                                                         EigyoNm = _.EigyoNm,
                                                         RyakuNm = _.RyakuNm,
                                                         CalcuSyuTime = _.CalcuSyuTime,
                                                         CalcuTaiTime = _.CalcuTaiTime
                                                     }).ToListAsync();
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
