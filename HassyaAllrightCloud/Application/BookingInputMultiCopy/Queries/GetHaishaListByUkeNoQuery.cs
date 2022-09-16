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
    public class GetHaishaListByUkeNoQuery : IRequest<List<TkdHaisha>>
    {
        public readonly string _ukeNo;

        public GetHaishaListByUkeNoQuery(string ukeNo)
        {
            _ukeNo = ukeNo;
        }

        public class Handler : IRequestHandler<GetHaishaListByUkeNoQuery, List<TkdHaisha>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<TkdHaisha>> Handle(GetHaishaListByUkeNoQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    return await _context.TkdHaisha
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
