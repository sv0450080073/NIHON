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
    public class GetYykSyuListByUkeNoQuery : IRequest<List<TkdYykSyu>>
    {
        public readonly string _ukeNo;

        public GetYykSyuListByUkeNoQuery(string ukeNo)
        {
            _ukeNo = ukeNo;
        }

        public class Handler : IRequestHandler<GetYykSyuListByUkeNoQuery, List<TkdYykSyu>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<TkdYykSyu>> Handle(GetYykSyuListByUkeNoQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    return await _context.TkdYykSyu
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
