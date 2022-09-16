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
    public class GetDepositPaymentHaitaCheckListAsyncQuery : IRequest<List<DepositPaymentHaitaCheck>>
    {
        public List<DepositCouponGrid> depositCouponGrids { get; set; }
        public class Handler : IRequestHandler<GetDepositPaymentHaitaCheckListAsyncQuery, List<DepositPaymentHaitaCheck>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<DepositPaymentHaitaCheck>> Handle(GetDepositPaymentHaitaCheckListAsyncQuery request, CancellationToken cancellationToken = default)
            {
                if(request.depositCouponGrids == null || !request.depositCouponGrids.Any())
                    return new List<DepositPaymentHaitaCheck>();
                var depositPaymentHaitaChecks = new List<DepositPaymentHaitaCheck>();
                var claimModel = new ClaimModel();
                foreach(var item in request.depositCouponGrids) {
                    depositPaymentHaitaChecks.Add(GetDepositPaymentHaitaCheckItem(item, claimModel));
                }
                return depositPaymentHaitaChecks;
            }

            public DepositPaymentHaitaCheck GetDepositPaymentHaitaCheckItem(DepositCouponGrid depositCouponGrid, ClaimModel claimModel) {
                var tkdMishum = _context.TkdMishum.Where(x => x.UkeNo == depositCouponGrid.UkeNo && x.MisyuRen == depositCouponGrid.MisyuRen).FirstOrDefault();
                var tkdYyksho = _context.TkdYyksho.Where(x => x.UkeNo == depositCouponGrid.UkeNo && x.TenantCdSeq == claimModel.TenantID).FirstOrDefault();
                var tkdFutTum = _context.TkdFutTum.Where(x => x.UkeNo == depositCouponGrid.UkeNo && x.UnkRen == depositCouponGrid.FutuUnkRen
                && x.FutTumRen == depositCouponGrid.FutTumRen && x.FutTumKbn == (depositCouponGrid.SeiFutSyu == (byte)6 ? (byte)2 : (byte)1)).FirstOrDefault();
                return new DepositPaymentHaitaCheck() {
                    tkdMishumUpdYmdTime = tkdMishum?.UpdYmd + tkdMishum?.UpdTime,
                    tkdYykshoUpdYmdTime = tkdYyksho?.UpdYmd + tkdYyksho?.UpdTime,
                    tkdFutTumUpdYmdTime = tkdFutTum?.UpdYmd + tkdFutTum?.UpdTime,
                    UkeNo = depositCouponGrid.UkeNo,
                    MisyuRen = depositCouponGrid.MisyuRen,
                    TenantCdSeq = claimModel.TenantID,
                    UnkRen = Convert.ToInt16(tkdFutTum?.UnkRen),
                    FutTumRen = Convert.ToInt16(tkdFutTum?.FutTumRen),
                    FutTumKbn = Convert.ToByte(tkdFutTum?.FutTumKbn)
                };  
            }
        }
    }
}
