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
    public class CheckOpenCouponBalancePopUpAsyncQuery : IRequest<bool>
    {
        public class Handler : IRequestHandler<CheckOpenCouponBalancePopUpAsyncQuery, bool>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(CheckOpenCouponBalancePopUpAsyncQuery request, CancellationToken cancellationToken = default)
            {
                return (from tkdCoupon in _context.TkdCoupon
                        where tkdCoupon.NyuSihKbn == (byte)1 
                        && tkdCoupon.SeiSihCdSeq == 1
                        && tkdCoupon.SeiSihSitCdSeq == 1
                        && tkdCoupon.CouZanFlg == (byte)1
                        && tkdCoupon.SiyoKbn == (byte)1
                        select tkdCoupon).Count() != 0;
            }
        }
    }
}
