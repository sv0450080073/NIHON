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

namespace HassyaAllrightCloud.Application.DepositList.Queries
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

                var data = _context.VpmYoyKbn.Where(x => x.SiyoKbn == 1 && x.TenantCdSeq == new ClaimModel().TenantID).ToList();

                foreach(var item in data)
                {
                    result.Add(new ReservationClassComponentData()
                    {
                        YoyaKbnSeq = item.YoyaKbnSeq,
                        YoyaKbn = item.YoyaKbn,
                        YoyaKbnNm = item.YoyaKbnNm
                    });
                }

                return result;
            }
        }
    }
}
