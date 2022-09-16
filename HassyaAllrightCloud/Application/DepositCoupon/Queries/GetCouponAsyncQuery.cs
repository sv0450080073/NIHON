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
    public class GetCouponAsyncQuery : IRequest<List<CouponBalanceGrid>>
    {
        public int tenantCdSeq { get; set; }
        public OutDataTable outDataTable { get; set; }
        public class Handler : IRequestHandler<GetCouponAsyncQuery, List<CouponBalanceGrid>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<CouponBalanceGrid>> Handle(GetCouponAsyncQuery request, CancellationToken cancellationToken = default)
            {
                return await (from tkdNyShCu in _context.TkdNyShCu
                        join tkdCoupon in _context.TkdCoupon
                        on new { CouTblSeq = tkdNyShCu.CouTblSeq, TenantCdSeq = request.tenantCdSeq }
                        equals new { CouTblSeq = tkdCoupon.CouTblSeq, TenantCdSeq = tkdCoupon.TenantCdSeq } into tkdCouponn
                        from tkdCoupon1 in tkdCouponn.DefaultIfEmpty()
                        where tkdNyShCu.NyuSihKbn == (byte)1
                        && tkdNyShCu.UkeNo == request.outDataTable.UkeNo
                        && tkdNyShCu.SeiFutSyu == request.outDataTable.SeiFutSyu
                        && tkdNyShCu.UnkRen == request.outDataTable.FutuUnkRen
                        && tkdNyShCu.FutTumRen == request.outDataTable.FutTumRen
                        && tkdNyShCu.SiyoKbn == (byte)1
                        orderby tkdCoupon1.HakoYmd
                        select new CouponBalanceGrid
                        {
                            HakoYmd = tkdCoupon1.HakoYmd ?? string.Empty,
                            CouGkin = tkdCoupon1.CouGkin,
                            CouNo = tkdCoupon1.CouNo ?? string.Empty,
                        }).ToListAsync();
            }
        }
    }
}
