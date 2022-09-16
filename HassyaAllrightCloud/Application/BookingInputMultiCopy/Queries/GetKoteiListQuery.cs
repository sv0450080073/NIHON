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
    public class GetKoteiListQuery : IRequest<List<TkdKotei>>
    {
        public readonly string _ukeNo;

        public GetKoteiListQuery(string ukeNo)
        {
            _ukeNo = ukeNo;
        }

        public class Handler : IRequestHandler<GetKoteiListQuery, List<TkdKotei>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<TkdKotei>> Handle(GetKoteiListQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    return await _context.TkdKotei
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
