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
    public class GetDepositTransferBankAsyncQuery : IRequest<List<DepositTransferBank>>
    {
        public class Handler : IRequestHandler<GetDepositTransferBankAsyncQuery, List<DepositTransferBank>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<DepositTransferBank>> Handle(GetDepositTransferBankAsyncQuery request, CancellationToken cancellationToken = default)
            {
                return (from vpmBank in _context.VpmBank
                        join vpmBankSt in _context.VpmBankSt
                        on vpmBank.BankCd equals vpmBankSt.BankCd into bst
                        from bstt in bst.DefaultIfEmpty()
                        where vpmBank.SiyoKbn == 1 && bstt.SiyoKbn == 1
                        orderby bstt.BankCd, bstt.BankSitCd
                        select new DepositTransferBank
                        {
                            BankCd = vpmBank.BankCd,
                            BankRyakuNm = vpmBank.RyakuNm,
                            BankSitCd = bstt.BankSitCd,
                            BankStRyakuNm = bstt.RyakuNm
                        }).ToList();
            }
        }
    }
}
