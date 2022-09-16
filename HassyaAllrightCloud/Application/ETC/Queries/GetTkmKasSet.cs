using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.ETC.Queries
{
    public class GetTkmKasSet : IRequest<TkmKasSetModel>
    {
        public int CompanyId { get; set; }
        public class Handler : IRequestHandler<GetTkmKasSet, TkmKasSetModel>
        {
            KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<TkmKasSetModel> Handle(GetTkmKasSet request, CancellationToken cancellationToken)
            {
                return await _context.TkmKasSet.Select(e => new TkmKasSetModel() {
                    CompanyCdSeq = e.CompanyCdSeq,
                    FutTumCdSeq = e.FutTumCdSeq,
                    SeisanCdSeq = e.SeisanCdSeq
                }).FirstOrDefaultAsync(e => e.CompanyCdSeq == request.CompanyId);
            }
        }
    }
}
