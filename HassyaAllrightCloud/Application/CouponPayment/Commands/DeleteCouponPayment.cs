using HassyaAllrightCloud.Commons;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.CouponPayment.Commands
{
    public class DeleteCouponPayment : IRequest<bool>
    {
        public int CurrentUserId { get; set; }
        public int CurrentTenantId { get; set; }
        public CouponPaymentFormGridItem Item { get; set; }
        public CouponPaymentGridItem GridItem { get; set; }
        public class Handler : IRequestHandler<DeleteCouponPayment, bool>
        {
            private KobodbContext _context { get; set; }
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(DeleteCouponPayment request, CancellationToken cancellationToken)
            {
                using var tran = _context.Database.BeginTransaction();
                try
                {
                    if (request == null) return false;
                    await DeleteNyuSih(request.Item, request.CurrentUserId, cancellationToken);
                    await DeleteNyShmi(request.Item, request.CurrentUserId, cancellationToken);
                    await tran.CommitAsync(cancellationToken);
                    await AdditionalProcess.Process(_context, request.GridItem, request.CurrentTenantId, request.CurrentUserId, cancellationToken);
                    return true;
                }
                catch (Exception e)
                {
                    await tran.RollbackAsync(cancellationToken);
                    throw e;
                }
            }

            private async Task DeleteNyShmi(CouponPaymentFormGridItem item, int currentUserId, CancellationToken cancellationToken)
            {
                if (item == null) throw new Exception($"{nameof(DeleteNyShmi)} throw an error: item is null");
                var nyShmi = await _context.TkdNyShmi.FirstOrDefaultAsync(e => e.NyuSihRen == item.NyuSihRen && e.UkeNo == item.UkeNo, cancellationToken);
                if (nyShmi != null)
                {
                    nyShmi.HenKai++;
                    nyShmi.SiyoKbn = 2;
                    nyShmi.UpdYmd = DateTime.Now.ToString(DateTimeFormat.yyyyMMdd);
                    nyShmi.UpdTime = $"{DateTime.Now.Hour:00}{DateTime.Now.Minute:00}{DateTime.Now.Second:00}";
                    nyShmi.UpdSyainCd = currentUserId;
                    nyShmi.UpdPrgId = ScreenCode.CouponPaymentUpdPrgId;
                }
                var result = await _context.SaveChangesAsync(cancellationToken);
            }

            private async Task DeleteNyuSih(CouponPaymentFormGridItem item, int currentUserId, CancellationToken cancellationToken)
            {
                if (item == null) throw new Exception($"{nameof(DeleteNyuSih)} throw an error: item is null");
                var nyShmi = await _context.TkdNyShmi.FirstOrDefaultAsync(e => e.NyuSihRen == item.NyuSihRen && e.UkeNo == item.UkeNo, cancellationToken);
                if (nyShmi != null)
                {
                    nyShmi.HenKai++;
                    nyShmi.SiyoKbn = 2;
                    nyShmi.UpdYmd = DateTime.Now.ToString(DateTimeFormat.yyyyMMdd);
                    nyShmi.UpdTime = $"{DateTime.Now.Hour:00}{DateTime.Now.Minute:00}{DateTime.Now.Second:00}";
                    nyShmi.UpdSyainCd = currentUserId;
                    nyShmi.UpdPrgId = ScreenCode.CouponPaymentUpdPrgId;
                }
                var result = await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
