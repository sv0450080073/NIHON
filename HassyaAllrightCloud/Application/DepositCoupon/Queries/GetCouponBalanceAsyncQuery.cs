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
    public class GetCouponBalanceAsyncQuery : IRequest<List<CouponBalanceGrid>>
    {
        public int tenantCdSeq { get; set; }
        public class Handler : IRequestHandler<GetCouponBalanceAsyncQuery, List<CouponBalanceGrid>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<CouponBalanceGrid>> Handle(GetCouponBalanceAsyncQuery request, CancellationToken cancellationToken = default)
            {
                return await (from tkdCoupon in _context.TkdCoupon
                        where tkdCoupon.NyuSihKbn == (byte)1
                        && tkdCoupon.SeiSihCdSeq == 1
                        && tkdCoupon.SeiSihSitCdSeq == 1
                        && tkdCoupon.CouZanFlg == (byte)1
                        && tkdCoupon.SiyoKbn == (byte)1
                        && tkdCoupon.TenantCdSeq == request.tenantCdSeq
                        orderby tkdCoupon.HakoYmd, tkdCoupon.CouTblSeq
                        select new CouponBalanceGrid
                        {
                            HakoYmd = tkdCoupon.HakoYmd,
                            CouGkin = tkdCoupon.CouGkin,
                            CouNo = tkdCoupon.CouNo,
                            CouZan = tkdCoupon.CouZan
                        }).ToListAsync();
            }
        }
    }
}
