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
    public class GetDepositCouponCodesAsyncQuery : IRequest<List<string>>
    {
        public DepositCouponFilter depositCouponFilter { get; set; }
        public class Handler : IRequestHandler<GetDepositCouponCodesAsyncQuery, List<string>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<string>> Handle(GetDepositCouponCodesAsyncQuery request, CancellationToken cancellationToken = default)
            {
                if (request.depositCouponFilter == null)
                {
                    return new List<string>();
                }
                List<DepositCouponCode> rows = null;
                DepositCouponResult depositCouponResult = new DepositCouponResult();
                var startBillAddress = (request.depositCouponFilter.startCustomerComponentGyosyaData == null ? "000"
                    : request.depositCouponFilter.startCustomerComponentGyosyaData.GyosyaCd.ToString("D3")) +
                    (request.depositCouponFilter.startCustomerComponentTokiskData == null ? "0000"
                    : request.depositCouponFilter.startCustomerComponentTokiskData.TokuiCd.ToString("D4")) +
                    (request.depositCouponFilter.startCustomerComponentTokiStData == null ? "0000"
                    : request.depositCouponFilter.startCustomerComponentTokiStData.SitenCd.ToString("D4"));
                var endBillAddress = (request.depositCouponFilter.endCustomerComponentGyosyaData == null ? "999"
                    : request.depositCouponFilter.endCustomerComponentGyosyaData.GyosyaCd.ToString("D3")) +
                    (request.depositCouponFilter.endCustomerComponentTokiskData == null ? "9999"
                    : request.depositCouponFilter.endCustomerComponentTokiskData.TokuiCd.ToString("D4")) +
                    (request.depositCouponFilter.endCustomerComponentTokiStData == null ? "9999"
                    : request.depositCouponFilter.endCustomerComponentTokiStData.SitenCd.ToString("D4"));
                DepositCouponFilterParam depositCouponFilterParam = new DepositCouponFilterParam()
                {
                    BillOffice = request.depositCouponFilter.BillOffice == null ? (int?)null : request.depositCouponFilter.BillOffice.EigyoCdSeq,
                    BillPeriodFrom = request.depositCouponFilter.BillPeriodFrom == null ? null : ((DateTime)request.depositCouponFilter.BillPeriodFrom).ToString("yyyyMMdd"),
                    BillPeriodTo = request.depositCouponFilter.BillPeriodTo == null ? null : ((DateTime)request.depositCouponFilter.BillPeriodTo).ToString("yyyyMMdd"),
                    BillTypes = !request.depositCouponFilter.BillTypes.Any() ? null : string.Join(",", request.depositCouponFilter.BillTypes),
                    EndBillAddress = endBillAddress,
                    EndReservationClassification = request.depositCouponFilter.EndReservationClassification == null ? (int?)null : request.depositCouponFilter.EndReservationClassification.YoyaKbn,
                    Limit = request.depositCouponFilter.Limit,
                    Offset = request.depositCouponFilter.Offset,
                    StartBillAddress = startBillAddress,
                    StartReservationClassification = request.depositCouponFilter.StartReservationClassification == null ? (int?)null : request.depositCouponFilter.StartReservationClassification.YoyaKbn,
                    DepositOutputClassification = request.depositCouponFilter.DepositOutputClassification ?? null,
                    UkeCd = request.depositCouponFilter.UkeCd ?? null
                };

                await _context.LoadStoredProc("PK_dDepositCouponCode_R")
                                .AddParam("@_TenantCdSeq", new ClaimModel().TenantID)
                                .AddParam("@_StartBillPeriod", depositCouponFilterParam.BillPeriodFrom)
                                .AddParam("@_EndBillPeriod", depositCouponFilterParam.BillPeriodTo)
                                .AddParam("@_StartBillAddress", depositCouponFilterParam.StartBillAddress)
                                .AddParam("@_EndBillAddress", depositCouponFilterParam.EndBillAddress)
                                .AddParam("@_BillOffice", depositCouponFilterParam.BillOffice)
                                .AddParam("@_StartReservationClassification", depositCouponFilterParam.StartReservationClassification)
                                .AddParam("@_EndReservationClassification", depositCouponFilterParam.EndReservationClassification)
                                .AddParam("@_BillTypes", depositCouponFilterParam.BillTypes)
                                .AddParam("@_DepositOutputClassification", depositCouponFilterParam.DepositOutputClassification)
                                .AddParam("@_UkeCd", depositCouponFilterParam.UkeCd)
                                .AddParam("ROWCOUNT", out IOutParam<int> rowCount)
                                .ExecAsync(async r => rows = await r.ToListAsync<DepositCouponCode>());
                return rows.Select(x => x.Code).ToList();
            }
        }
    }
}
