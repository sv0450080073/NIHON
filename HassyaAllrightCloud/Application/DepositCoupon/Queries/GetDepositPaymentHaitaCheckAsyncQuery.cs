using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
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
    public class GetDepositPaymentHaitaCheckAsyncQuery : IRequest<DepositPaymentHaitaCheck>
    {
        public DepositCouponGrid depositCouponGrid { get; set; }
        public DepositPaymentGrid depositPaymentGrid { get; set; }
        public class Handler : IRequestHandler<GetDepositPaymentHaitaCheckAsyncQuery, DepositPaymentHaitaCheck>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<DepositPaymentHaitaCheck> Handle(GetDepositPaymentHaitaCheckAsyncQuery request, CancellationToken cancellationToken = default)
            {
                var claimModel = new ClaimModel();
                var tkdNyuSih = new TkdNyuSih();
                var tkdNyShmi = new TkdNyShmi();
                var tkdCoupon = new TkdCoupon();
                var tkdNyShCu = new TkdNyShCu();
                var tkdMishum = new TkdMishum();
                var tkdYyksho = new TkdYyksho();
                var tkdFutTum = new TkdFutTum();

                if(request.depositPaymentGrid != null) {
                    tkdNyuSih = await _context.TkdNyuSih.Where(x => x.NyuSihTblSeq == request.depositPaymentGrid.NS_NyuSihTblSeq 
                    && x.TenantCdSeq == claimModel.TenantID).FirstOrDefaultAsync();
                    tkdNyShmi = await _context.TkdNyShmi.Where(x => x.UkeNo == request.depositPaymentGrid.UkeNo 
                    && x.NyuSihRen == request.depositPaymentGrid.NyuSihRen && x.TenantCdSeq == claimModel.TenantID).FirstOrDefaultAsync();
                    tkdCoupon = await _context.TkdCoupon.Where(x => x.CouTblSeq == request.depositPaymentGrid.CouTblSeq 
                    && x.TenantCdSeq == claimModel.TenantID).FirstOrDefaultAsync();                
                    tkdNyShCu = await _context.TkdNyShCu.Where(x => x.UkeNo == request.depositPaymentGrid.UkeNo 
                    && x.NyuSihCouRen == request.depositPaymentGrid.NyuSihCouRen && x.TenantCdSeq == claimModel.TenantID).FirstOrDefaultAsync();
                }
                if(request.depositCouponGrid != null) {
                    tkdMishum = await _context.TkdMishum.Where(x => x.UkeNo == request.depositCouponGrid.UkeNo && x.MisyuRen == request.depositCouponGrid.MisyuRen).FirstOrDefaultAsync();
                    tkdYyksho = await _context.TkdYyksho.Where(x => x.UkeNo == request.depositCouponGrid.UkeNo && x.TenantCdSeq == claimModel.TenantID).FirstOrDefaultAsync();
                    tkdFutTum = await _context.TkdFutTum.Where(x => x.UkeNo == request.depositCouponGrid.UkeNo && x.UnkRen == request.depositCouponGrid.FutuUnkRen
                    && x.FutTumRen == request.depositCouponGrid.FutTumRen && x.FutTumKbn == (request.depositCouponGrid.SeiFutSyu == (byte)6 ? (byte)2 : (byte)1)).FirstOrDefaultAsync();   
                }
                return new DepositPaymentHaitaCheck() {
                    tkdNyuSihUpdYmdTime = tkdNyuSih?.UpdYmd + tkdNyuSih?.UpdTime,
                    tkdNyShmiUpdYmdTime = tkdNyShmi?.UpdYmd + tkdNyShmi?.UpdTime,
                    tkdCouponUpdYmdTime = tkdCoupon?.UpdYmd + tkdCoupon?.UpdTime,
                    tkdNyShCuUpdYmdTime = tkdNyShCu?.UpdYmd + tkdNyShCu?.UpdTime,
                    tkdMishumUpdYmdTime = tkdMishum?.UpdYmd + tkdMishum?.UpdTime,
                    tkdYykshoUpdYmdTime = tkdYyksho?.UpdYmd + tkdYyksho?.UpdTime,
                    tkdFutTumUpdYmdTime = tkdFutTum?.UpdYmd + tkdFutTum?.UpdTime
                };    
            }
        }
    }
}
