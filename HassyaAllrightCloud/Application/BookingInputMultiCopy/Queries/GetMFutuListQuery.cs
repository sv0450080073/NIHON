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
    public class GetMFutuListQuery : IRequest<List<TkdMfutTu>>
    {
        public readonly string _ukeNo;
        public readonly byte _futTumKbn;

        public GetMFutuListQuery(string ukeNo, byte futTumKbn)
        {
            _ukeNo = ukeNo;
            _futTumKbn = futTumKbn;
        }

        public class Handler : IRequestHandler<GetMFutuListQuery, List<TkdMfutTu>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<TkdMfutTu>> Handle(GetMFutuListQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    return await _context.TkdMfutTu
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
