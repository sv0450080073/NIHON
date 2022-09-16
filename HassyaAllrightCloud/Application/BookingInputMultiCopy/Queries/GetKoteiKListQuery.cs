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
    public class GetKoteiKListQuery : IRequest<List<TkdKoteik>>
    {
        public readonly string _ukeNo;

        public GetKoteiKListQuery(string ukeNo)
        {
            _ukeNo = ukeNo;
        }

        public class Handler : IRequestHandler<GetKoteiKListQuery, List<TkdKoteik>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<TkdKoteik>> Handle(GetKoteiKListQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    return await _context.TkdKoteik
                                .Where(x => x.UkeNo == request._ukeNo && x.TeiDanNo == 0)
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
