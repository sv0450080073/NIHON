using HassyaAllrightCloud.Commons;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.CouponPayment.Queries
{
    public class GetCouponPayment : IRequest<(List<CouponPaymentGridItem>, CouponPaymentSummary, int)>
    {
        public bool IsGetSingle { get; set; }
        public CouponPaymentGridItem Item { get; set; }
        public CouponPaymentFormModel Model { get; set; }
        public class Handler : IRequestHandler<GetCouponPayment, (List<CouponPaymentGridItem>, CouponPaymentSummary, int)>
        {
            private readonly KobodbContext _kobodbContext;
            public Handler(KobodbContext kobodbContext)
            {
                _kobodbContext = kobodbContext;
            }

            public async Task<(List<CouponPaymentGridItem>, CouponPaymentSummary, int)> Handle(GetCouponPayment request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = new List<CouponPaymentGridItem>();
                    var summary = new CouponPaymentSummary();
                    var totalRows = 0;
                    if (request != null && request.Model != null)
                    {
                        var billAddress = request.Model.BillAddress;
                        var billAddressCode = billAddress != null ? $"{billAddress.GyosyaCd:000}{billAddress.TokuiCd:0000}{billAddress.SitenCd:0000}" : string.Empty;
                        var storedBuilder = _kobodbContext.LoadStoredProc("dbo.PK_dCouponPayment_R");
                        await storedBuilder
                            .AddParam("@StartIssuePeriod", request.Model.StartIssuePeriod?.ToString(DateTimeFormat.yyyyMMdd) ?? string.Empty)
                            .AddParam("@EndIssuePeriod", request.Model.EndIssuePeriod?.ToString(DateTimeFormat.yyyyMMdd) ?? string.Empty)
                            .AddParam("@BillAddress", billAddressCode)
                            .AddParam("@DepositOffice", request.Model.DepositOffice?.EigyoCd ?? 0)
                            .AddParam("@StartReservationClassificationSort", request.Model.StartReservationClassificationSort?.YoyaKbn ?? 0)
                            .AddParam("@EndReservationClassificationSort", request.Model.EndReservationClassificationSort?.YoyaKbn ?? 0)
                            .AddParam("@BillTypes", (request.Model.BillTypes != null && request.Model.BillTypes.Any()) ?
                            string.Join(',', request.Model.BillTypes.Select(e => e.CodeKbn)) : string.Empty)
                            .AddParam("@DepositOutputClassification", (int)(request.Model.DepositOutputClassification?.Value ?? 0))

                            .AddParam("@TenantCdSeq", request.Model.TenantCdSeq)

                            .AddParam("@IsGetSingle", request.IsGetSingle ? 1 : 0)
                            .AddParam("@IsFSuperMenu", request.Model.UkeNo != null && !request.IsGetSingle ? 1 : 0)
                            .AddParam("@UkeNo", request.Item?.UkeNo ?? request.Model?.UkeNo ?? string.Empty)
                            .AddParam("@NyuSihCouRen", request.Item?.NyuSihCouRen ?? 0)

                            .AddParam("@Skip", request.IsGetSingle ? 0 : request.Model.PageNum * request.Model.PageSize)
                            .AddParam("@Take", request.IsGetSingle ? 1 : request.Model.PageSize)

                            .AddParam("@TotalCount", out IOutParam<int> rowCount)
                            .AddParam("@TotalAllCouponApplicationAmount", out IOutParam<int> totalAppAmount)
                            .AddParam("@TotalAllCumulativeDeposit", out IOutParam<int> totalCummulativeDeposit)
                            .AddParam("@TotalAllUnpaidAmount", out IOutParam<int> totalUnpaidAmount)

                            .ExecAsync(async r =>
                            {
                                result = await r.ToListAsync<CouponPaymentGridItem>(cancellationToken);
                            });
                        summary.TotalAllCouponApplicationAmount = totalAppAmount?.Value ?? 0;
                        summary.TotalAllCumulativeDeposit = totalCummulativeDeposit?.Value ?? 0;
                        summary.TotalAllUnpaidAmount = totalUnpaidAmount?.Value ?? 0;
                        summary.TotalPageCouponApplicationAmount = result.Sum(e => e.CouKesG);
                        summary.TotalPageCumulativeDeposit = result.Sum(e => e.NyuKinRui);
                        summary.TotalPageUnpaidAmount = result.Sum(e => e.CouKesG - e.NyuKinRui);
                        totalRows = rowCount.Value;
                    }

                    return (result, summary, totalRows);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
}
