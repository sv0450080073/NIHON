using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.CommonComponents.Queries
{
    public class GetListReservationClassQuery : IRequest<List<ReservationClassComponentData>>
    {
        public class Handler : IRequestHandler<GetListReservationClassQuery, List<ReservationClassComponentData>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<ReservationClassComponentData>> Handle(GetListReservationClassQuery request, CancellationToken cancellationToken)
            {
                List<ReservationClassComponentData> result = new List<ReservationClassComponentData>();

                result = await (from y in _context.VpmYoyKbn
                                where y.SiyoKbn == CommonConstants.SiyoKbn && y.TenantCdSeq == new ClaimModel().TenantID
                                select new ReservationClassComponentData()
                                {
                                    YoyaKbnSeq = y.YoyaKbnSeq,
                                    YoyaKbn = y.YoyaKbn,
                                    YoyaKbnNm = y.YoyaKbnNm
                                }).ToListAsync();

                return result;
            }
        }
    }
}
