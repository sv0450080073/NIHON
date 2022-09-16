using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BusCoordinationReport.Queries
{
    public class GetKyoSet : IRequest<List<KyoSetData>>
    {
        public int tenantID { get; set; } = 0;
        public class Handler : IRequestHandler<GetKyoSet, List<KyoSetData>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<KyoSetData>> Handle(GetKyoSet request, CancellationToken cancellationToken)
            {
                var data = new List<KyoSetData>();
                try
                {
                    data = (from VPM_KyoSet in _context.VpmKyoSet
                            select new KyoSetData()
                            {
                                SijJoKNm01 = VPM_KyoSet.SijJoKnm1,
                                SijJoKNm02 = VPM_KyoSet.SijJoKnm2,
                                SijJoKNm03 = VPM_KyoSet.SijJoKnm3,
                                SijJoKNm04 = VPM_KyoSet.SijJoKnm4,
                                SijJoKNm05 = VPM_KyoSet.SijJoKnm5,
                            }).ToList();
                    return data;
                }
                catch (Exception)
                {
                    return data;
                }

            }
        }

    }
}
