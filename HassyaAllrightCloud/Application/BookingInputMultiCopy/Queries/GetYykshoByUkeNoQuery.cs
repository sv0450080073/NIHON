using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BookingInputMultiCopy.Queries
{
    public class GetYykshoByUkeNoQuery : IRequest<TkdYyksho>
    {
        public readonly string _ukeNo;

        public GetYykshoByUkeNoQuery(string ukeNo)
        {
            _ukeNo = ukeNo;
        }

        public class Handler : IRequestHandler<GetYykshoByUkeNoQuery, TkdYyksho>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<TkdYyksho> Handle(GetYykshoByUkeNoQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    return await _context.TkdYyksho
                                .Where(x => x.UkeNo == request._ukeNo)
                                .SingleOrDefaultAsync();
                }
                catch (System.Exception)
                {
                    throw;
                }
            }
        }
    }
}
