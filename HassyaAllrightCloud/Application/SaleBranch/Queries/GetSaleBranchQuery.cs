using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.SaleBranch.Queries
{
    public class GetSaleBranchQuery : IRequest<IEnumerable<LoadSaleBranchList>>
    {
        public int ID { get; set; }
        public class Handler : IRequestHandler<GetSaleBranchQuery, IEnumerable<LoadSaleBranchList>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<IEnumerable<LoadSaleBranchList>> Handle(GetSaleBranchQuery request, CancellationToken cancellationToken)
            {
                return await _context.VpmEigyos.Select(t => new LoadSaleBranchList
                {
                    EigyoCdSeq = t.EigyoCdSeq,
                    EigyoCd = t.EigyoCd,
                    RyakuNm = t.RyakuNm,
                    CompanyCdSeq = t.CompanyCdSeq
                }).Where(t => t.CompanyCdSeq == request.ID).ToListAsync();
            }
        }
    }
}
