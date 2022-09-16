using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.YoyKbn.Queries
{
    public class GetTpmYoyKbnWithPriorityNumQuery: IRequest<IEnumerable<ReservationData>>
    {
        public class Handler : IRequestHandler<GetTpmYoyKbnWithPriorityNumQuery, IEnumerable<ReservationData>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<ReservationData>> Handle(GetTpmYoyKbnWithPriorityNumQuery request, CancellationToken cancellationToken)
            {
                return await (from VPM_YoyKbn in _context.VpmYoyKbn
                                    join VPM_YoyaKbnSort in _context.VpmYoyaKbnSort
                                    on new { VPM_YoyKbn.YoyaKbnSeq, T1 = new HassyaAllrightCloud.Domain.Dto.ClaimModel().TenantID }
                                    equals new { VPM_YoyaKbnSort.YoyaKbnSeq, T1 = VPM_YoyaKbnSort.TenantCdSeq }
                                    into VPM_YoyaKbnSort_join
                                    from VPM_YoyaKbnSort in VPM_YoyaKbnSort_join.DefaultIfEmpty()
                                    where VPM_YoyKbn.SiyoKbn == 1
                                    select new ReservationData()
                                    {
                                        YoyaKbnSeq = VPM_YoyKbn.YoyaKbnSeq,
                                        YoyaKbn = VPM_YoyKbn.YoyaKbn,
                                        YoyaKbnNm = VPM_YoyKbn.YoyaKbnNm,
#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                                        PriorityNum = VPM_YoyaKbnSort.PriorityNum != null ? VPM_YoyaKbnSort.PriorityNum.ToString("D2") : "99"
#pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                                        + VPM_YoyKbn.YoyaKbn.ToString("D2"),
                                        TenantCdSeq = VPM_YoyKbn.TenantCdSeq
                                    }).ToListAsync();
            }
        }
    }
}
