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
    public class GetFutumListQuery : IRequest<List<TkdFutTum>>
    {
        public readonly string _ukeNo;
        public readonly byte _futTumKbn;

        public GetFutumListQuery(string ukeNo, byte futTumKbn)
        {
            _ukeNo = ukeNo;
            _futTumKbn = futTumKbn;
        }

        public class Handler : IRequestHandler<GetFutumListQuery, List<TkdFutTum>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<TkdFutTum>> Handle(GetFutumListQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    return await _context.TkdFutTum
                                .Where(x => x.UkeNo == request._ukeNo && x.FutTumKbn == request._futTumKbn)
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
