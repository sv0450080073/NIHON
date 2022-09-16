using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto.BillPrint;
using HassyaAllrightCloud.Domain.Dto.DepositCoupon;
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
    public class GetDepositMethodAsyncQuery : IRequest<List<DepositMethod>>
    {
        private const string NYUSIHSYU = "NYUSIHSYU";
        public class Handler : IRequestHandler<GetDepositMethodAsyncQuery, List<DepositMethod>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<DepositMethod>> Handle(GetDepositMethodAsyncQuery request, CancellationToken cancellationToken = default)
            {
                return (from vpmCodeKb in _context.VpmCodeKb
                        where vpmCodeKb.CodeSyu == NYUSIHSYU &&
                        vpmCodeKb.SiyoKbn == (byte)1
                        select new DepositMethod
                        {
                            CodeKbn = vpmCodeKb.CodeKbn,
                            CodeKbnNm = vpmCodeKb.CodeKbnNm
                        }).ToList();
            }
        }
    }
}
