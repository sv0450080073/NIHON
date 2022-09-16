using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using StoredProcedureEFCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.CouponPayment.Queries
{
    public class GetNyShmi : IRequest<List<CouponPaymentFormGridItem>>
    {
        public CouponPaymentGridItem SelectedItem { get; set; }
        public int CurrentTenant { get; set; }
        public class Handler : IRequestHandler<GetNyShmi, List<CouponPaymentFormGridItem>>
        {
            KobodbContext _context { get; set; }
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<List<CouponPaymentFormGridItem>> Handle(GetNyShmi request, CancellationToken cancellationToken)
            {
                var result = new List<CouponPaymentFormGridItem>();
                if (request != null && request.SelectedItem != null)
                {
                    var storedBuilder = _context.LoadStoredProc("dbo.[PK_dNyShmi_R]");
                    await storedBuilder
                        .AddParam("@UkeNo", request.SelectedItem.UkeNo)
                        .AddParam("@UnkRen", request.SelectedItem.UnkRen)
                        .AddParam("@SeiFutSyu", request.SelectedItem.SeiFutSyu)
                        .AddParam("@FutTumRen", request.SelectedItem.FutTumRen)
                        .AddParam("@YouTblSeq", request.SelectedItem.YouTblSeq)
                        .AddParam("@TenantCdSeq", request.CurrentTenant)
                        .ExecAsync(async r =>
                        {
                            result = await r.ToListAsync<CouponPaymentFormGridItem>(cancellationToken);
                        });
                }
                return result;
            }
        }
    }
}
