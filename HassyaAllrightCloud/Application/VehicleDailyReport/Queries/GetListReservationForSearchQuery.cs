using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HassyaAllrightCloud.Commons.Constants;
using Microsoft.EntityFrameworkCore;

namespace HassyaAllrightCloud.Application.VehicleDailyReport.Queries
{
    public class GetListReservationForSearchQuery : IRequest<List<ReservationSearch>>
    {
        public class Handler : IRequestHandler<GetListReservationForSearchQuery, List<ReservationSearch>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            /// <summary>
            /// Get list reservation for search
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<List<ReservationSearch>> Handle(GetListReservationForSearchQuery request, CancellationToken cancellationToken)
            {
                //var result = await (from y in _context.VpmYoyKbn.Where(_ => _.SiyoKbn == Constants.SiyoKbn)
                //                    select new ReservationSearch() 
                //                    { 
                //                        YoyaKbn = y.YoyaKbn,
                //                        YoyaKbnNm = y.YoyaKbnNm
                //                    }).ToListAsync();
                var result = (from y in _context.VpmYoyKbn.Where(_ => _.SiyoKbn == Constants.SiyoKbn).ToList()
                                    join yy in _context.VpmYoyaKbnSort.Where(_ => _.TenantCdSeq == new ClaimModel().TenantID).ToList()
                                    on y.YoyaKbnSeq equals yy.YoyaKbnSeq into temp
                                    from t in temp.DefaultIfEmpty()
                                    select new ReservationSearch()
                                    {
                                        PriorityNum = t == null ? string.Format("99{0}", y.YoyaKbn.ToString().PadLeft(2, '0')) : string.Format("{0}{1}", t.PriorityNum.ToString().PadLeft(2, '0'), y.YoyaKbn.ToString().PadLeft(2, '0')),
                                        YoyaKbnNm = y.YoyaKbnNm
                                    }).ToList();
                return result;
            }
        }
    }
}
