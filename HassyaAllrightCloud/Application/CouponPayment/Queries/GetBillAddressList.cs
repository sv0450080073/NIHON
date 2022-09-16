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
    public class GetBillAddressList : IRequest<List<BillAddressItem>>
    {
        public CouponPaymentFormModel Model { get; set; }
        public class Handler : IRequestHandler<GetBillAddressList, List<BillAddressItem>>
        {
            private readonly KobodbContext _kobodbContext;
            public Handler(KobodbContext kobodbContext)
            {
                _kobodbContext = kobodbContext;
            }

            public async Task<List<BillAddressItem>> Handle(GetBillAddressList request, CancellationToken cancellationToken)
            {
                var result = new List<BillAddressItem>();
                var summary = new CouponPaymentSummary();
                if (request != null && request.Model != null)
                {
                    var startBillAddress = $"{(request.Model.SelectedGyosyaFrom?.GyosyaCd ?? 0):000}{(request.Model.SelectedTokiskFrom?.TokuiCd ?? 0):0000}{(request.Model.SelectedTokiStFrom?.SitenCd ?? 0):0000}";
                    var endBillAddress = $"{(request.Model.SelectedGyosyaTo?.GyosyaCd ?? 999):000}{(request.Model.SelectedTokiskTo?.TokuiCd ?? 9999):0000}{(request.Model.SelectedTokiStTo?.SitenCd ?? 9999):0000}";
                    var storedBuilder = _kobodbContext.LoadStoredProc("dbo.PK_dGetListPaymentAddress_R");
                    await storedBuilder
                        .AddParam("@StartIssuePeriod", request.Model.StartIssuePeriod?.ToString(DateTimeFormat.yyyyMMdd) ?? string.Empty)
                        .AddParam("@EndIssuePeriod", request.Model.EndIssuePeriod?.ToString(DateTimeFormat.yyyyMMdd) ?? string.Empty)
                        .AddParam("@StartBillAddress", startBillAddress)
                        .AddParam("@EndBillAddress", endBillAddress)
                        .AddParam("@DepositOffice", request.Model.DepositOffice?.EigyoCd ?? 0)
                        .AddParam("@StartReservationClassificationSort", request.Model.StartReservationClassificationSort?.YoyaKbn ?? 0)
                        .AddParam("@EndReservationClassificationSort", request.Model.EndReservationClassificationSort?.YoyaKbn ?? 0)
                        .AddParam("@BillTypes", (request.Model.BillTypes != null && request.Model.BillTypes.Any()) ?
                        string.Join(',', request.Model.BillTypes.Select(e => e.CodeKbn)) : string.Empty)
                        .AddParam("@DepositOutputClassification", (int)(request.Model.DepositOutputClassification?.Value ?? 0))
                        .AddParam("@TenantCdSeq", request.Model.TenantCdSeq)
                        .AddParam("@UkeNo", request.Model?.UkeNo ?? string.Empty)
                        .ExecAsync(async r =>
                        {
                            result = await r.ToListAsync<BillAddressItem>(cancellationToken);
                        });
                }

                return result;
            }
        }
    }
}
