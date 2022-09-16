using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.BillPrint;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BillPrint.Queries
{
    public class GetSeiFileIdAsyncQuery : IRequest<string>
    {
        public int seiOutSeq { get; set; }
        public int seiRen { get; set; }
        public class Handler : IRequestHandler<GetSeiFileIdAsyncQuery, string>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<string> Handle(GetSeiFileIdAsyncQuery request, CancellationToken cancellationToken = default)
            {
                return await _context.TkdSeikyu.Where(x => x.SeiOutSeq == request.seiOutSeq && x.SeiRen == request.seiRen).Select(x => x.SeiFileId).FirstOrDefaultAsync();
            }
        }
    }
}
