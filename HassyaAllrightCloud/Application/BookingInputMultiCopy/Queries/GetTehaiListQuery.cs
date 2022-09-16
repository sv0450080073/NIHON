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
    public class GetTehaiListQuery : IRequest<List<TkdTehai>>
    {
        public readonly string _ukeNo;

        public GetTehaiListQuery(string ukeNo)
        {
            _ukeNo = ukeNo;
        }

        public class Handler : IRequestHandler<GetTehaiListQuery, List<TkdTehai>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<TkdTehai>> Handle(GetTehaiListQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    return await _context.TkdTehai
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
