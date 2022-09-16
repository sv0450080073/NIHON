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
    public class GetWorkLeaveListQuery : IRequest<List<WorkLeaveItem>>
    {
        public class Handler : IRequestHandler<GetWorkLeaveListQuery, List<WorkLeaveItem>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<WorkLeaveItem>> Handle(GetWorkLeaveListQuery request, CancellationToken cancellationToken)
            {
                List<WorkLeaveItem> result = new List<WorkLeaveItem>();

                try
                {
                    result = await _context.VpmKinKyu.Where(_ => _.SiyoKbn == (byte)CommonConstants.SiyoKbn && _.TenantCdSeq == new ClaimModel().TenantID)
                                                     .OrderBy(_ => _.KinKyuCd)
                                                     .Select(_ => new WorkLeaveItem()
                                                     {
                                                         KinKyuCdSeq = _.KinKyuCdSeq,
                                                         KinKyuCd = _.KinKyuCd,
                                                         KinKyuNm = _.KinKyuNm,
                                                         RyakuNm = _.RyakuNm,
                                                         KinKyuKbn = _.KinKyuKbn,
                                                         ColKinKyu = _.ColKinKyu,
                                                         KyuSyukinNm = _.KyuSyukinNm,
                                                         KyuSyukinRyaku = _.KyuSyukinRyaku,
                                                         DefaultSyukinTime = _.DefaultSyukinTime,
                                                         DefaultTaiknTime = _.DefaultTaiknTime
                                                     })
                                                     .ToListAsync();
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
