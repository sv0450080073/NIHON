using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BookingInputMultiCopy.Queries
{
    public class GetUnkobiByUkeNoQuery : IRequest<TkdUnkobi>
    {
        public readonly string _ukeNo;

        public GetUnkobiByUkeNoQuery(string ukeNo)
        {
            _ukeNo = ukeNo;
        }

        public class Handler : IRequestHandler<GetUnkobiByUkeNoQuery, TkdUnkobi>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<TkdUnkobi> Handle(GetUnkobiByUkeNoQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    return await _context.TkdUnkobi
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
