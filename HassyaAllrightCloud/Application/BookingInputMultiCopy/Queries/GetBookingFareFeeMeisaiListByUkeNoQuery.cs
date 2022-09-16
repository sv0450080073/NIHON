using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BookingInputMultiCopy.Queries
{
    public class GetBookingFareFeeMeisaiListByUkeNoQuery : IRequest<List<TkdBookingMaxMinFareFeeCalcMeisai>>
    {
        public readonly string _ukeNo;

        public GetBookingFareFeeMeisaiListByUkeNoQuery(string ukeNo)
        {
            _ukeNo = ukeNo;
        }

        public class Handler : IRequestHandler<GetBookingFareFeeMeisaiListByUkeNoQuery, List<TkdBookingMaxMinFareFeeCalcMeisai>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<TkdBookingMaxMinFareFeeCalcMeisai>> Handle(GetBookingFareFeeMeisaiListByUkeNoQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    return await _context.TkdBookingMaxMinFareFeeCalcMeisai
                                .Where(x => x.UkeNo == request._ukeNo)
                                .ToListAsync();
                }
                catch (System.Exception)
                {
                    throw;
                }
            }
        }
    }
}
