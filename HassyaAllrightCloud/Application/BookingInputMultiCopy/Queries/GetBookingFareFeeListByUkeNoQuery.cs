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
    public class GetBookingFareFeeListByUkeNoQuery : IRequest<List<TkdBookingMaxMinFareFeeCalc>>
    {
        public readonly string _ukeNo;

        public GetBookingFareFeeListByUkeNoQuery(string ukeNo)
        {
            _ukeNo = ukeNo;
        }

        public class Handler : IRequestHandler<GetBookingFareFeeListByUkeNoQuery, List<TkdBookingMaxMinFareFeeCalc>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<TkdBookingMaxMinFareFeeCalc>> Handle(GetBookingFareFeeListByUkeNoQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    return await _context.TkdBookingMaxMinFareFeeCalc
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
