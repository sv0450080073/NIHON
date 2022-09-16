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
    public class GetMishumListByUkeNoQuery : IRequest<List<TkdMishum>>
    {
        public readonly string _ukeNo;

        public GetMishumListByUkeNoQuery(string ukeNo)
        {
            _ukeNo = ukeNo;
        }

        public class Handler : IRequestHandler<GetMishumListByUkeNoQuery, List<TkdMishum>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<TkdMishum>> Handle(GetMishumListByUkeNoQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    return await _context.TkdMishum
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
