using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.CouponPayment.Queries
{
    public class GetBankTransferItems : IRequest<List<BankTransferItem>>
    {
        public class Handler : IRequestHandler<GetBankTransferItems, List<BankTransferItem>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<List<BankTransferItem>> Handle(GetBankTransferItems request, CancellationToken cancellationToken)
            {
                return  await (from bank in _context.VpmBank.Where(e => e.SiyoKbn == 1)
                            join bankst in _context.VpmBankSt.Where(e => e.SiyoKbn == 1).DefaultIfEmpty()
                            on bank.BankCd equals bankst.BankCd
                            orderby bankst.BankCd, bankst.BankSitCd
                            select new BankTransferItem()
                            {
                                BankCd = bank.BankCd,
                                BankRyakuNm = bank.RyakuNm,
                                BankSitCd = bankst.BankSitCd,
                                BankStRyakuNm = bankst.RyakuNm
                            }).ToListAsync(cancellationToken);
            }
        }
    }
}
