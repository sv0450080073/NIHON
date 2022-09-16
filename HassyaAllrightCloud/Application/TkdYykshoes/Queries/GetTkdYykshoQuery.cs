using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.TkdYykshoes.Queries
{
    public class GetTkdYykshoQuery : IRequest<TkdYyksho>
    {
        public int Id { get; set; }
        public class Handler : IRequestHandler<GetTkdYykshoQuery, TkdYyksho>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<TkdYyksho> Handle(GetTkdYykshoQuery request, CancellationToken cancellationToken)
            {
                return await _context.TkdYyksho.FindAsync(request.Id);
            }
        }
    }
}
