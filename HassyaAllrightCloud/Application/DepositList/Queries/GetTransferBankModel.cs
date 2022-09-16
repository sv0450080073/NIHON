using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using System.Threading;
using HassyaAllrightCloud.Infrastructure.Persistence;

namespace HassyaAllrightCloud.Application.DepositList.Queries
{
    public class GetTransferBankModel : IRequest<List<TransferBankModel>>
    {
        public class Handler : IRequestHandler<GetTransferBankModel, List<TransferBankModel>>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext kobodbContext) => _dbContext = kobodbContext;

            public async Task<List<TransferBankModel>> Handle(GetTransferBankModel request, CancellationToken cancellationToken)
            {
                return (from bank in _dbContext.VpmBank
                        join bankst in _dbContext.VpmBankSt
                        on new { key1 = bank.BankCd, key2 = (byte)1 } equals new { key1 = bankst.BankCd, key2 = bankst.SiyoKbn } into bankbankst
                        from bankbankstTemp in bankbankst.DefaultIfEmpty()
                        where bank.SiyoKbn == 1
                        select new TransferBankModel()
                        {
                            Code = bank.BankCd + bankbankstTemp.BankSitCd,
                            Name = bank.BankCd + ":" + bank.BankNm +" "+bankbankstTemp.BankSitCd+":"+bankbankstTemp.BankSitNm
                        }).ToList();
            }
        }
    }
}
