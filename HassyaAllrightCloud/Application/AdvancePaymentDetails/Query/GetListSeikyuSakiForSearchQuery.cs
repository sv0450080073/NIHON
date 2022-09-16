using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.AdvancePaymentDetails.Query
{
    public class GetListSeikyuSakiForSearchQuery : IRequest<List<SeikyuSakiSearch>>
    {
        public int TenantCdSeq { get; set; }
        public class Handler : IRequestHandler<GetListSeikyuSakiForSearchQuery, List<SeikyuSakiSearch>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<SeikyuSakiSearch>> Handle(GetListSeikyuSakiForSearchQuery request, CancellationToken cancellationToken)
            {
                List<SeikyuSakiSearch> listSeikyuSaki = new List<SeikyuSakiSearch>();
                await _context.LoadStoredProc("dbo.PK_dSeikyuSaki_R")
                    .AddParam("@TenantCdSeq", request.TenantCdSeq)
                    .ExecAsync(async rows => listSeikyuSaki = await rows.ToListAsync<SeikyuSakiSearch>());
                return listSeikyuSaki;
            }
        }
    }
}
