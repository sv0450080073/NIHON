using HassyaAllrightCloud.Application.CouponPayment.Queries;
using HassyaAllrightCloud.Commons;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using HassyaAllrightCloud.Commons.Constants;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.CouponPayment.Commands
{
    public class AdditionalProcess
    {
        public static async Task Process(KobodbContext context, CouponPaymentGridItem gridItem, int currentTenant, int currentUserId, CancellationToken cancellationToken)
        {
            if (gridItem == null) throw new Exception("Grid item is empty");
            var currentYmd = DateTime.Now.ToString(DateTimeFormat.yyyyMMdd);
            var currentHms = $"{DateTime.Now.Hour:00}{DateTime.Now.Minute:00}{DateTime.Now.Second:00}";
            var total = context.TkdNyShmi.Where(e => e.UkeNo == gridItem.UkeNo &&
                                         e.UnkRen == gridItem.UnkRen &&
                                         e.SeiFutSyu == gridItem.SeiFutSyu &&
                                         e.FutTumRen == gridItem.FutTumRen && 
                                         e.YouTblSeq == gridItem.YouTblSeq && 
                                         e.SiyoKbn == 1 &&
                                         e.NyuSihKbn == 1).Sum(e => e.KesG + e.FurKesG + e.KyoKesG);

            #region Update mishum
            var mishum = await context.TkdMishum.FirstOrDefaultAsync(e => e.UkeNo == gridItem.UkeNo && e.SeiFutSyu == gridItem.SeiFutSyu, cancellationToken);
            if (mishum == null) throw new Exception($"{nameof(AdditionalProcess)} throw an error: mishum not found");
            mishum.HenKai++;
            mishum.NyuKinRui = total;
            mishum.UpdYmd = currentYmd;
            mishum.UpdTime = currentHms;
            mishum.UpdSyainCd = currentUserId;
            mishum.UpdPrgId = ScreenCode.CouponPaymentUpdPrgId;
            await context.SaveChangesAsync(cancellationToken);
            #endregion


            if (gridItem.SeiFutSyu == 1 || gridItem.SeiFutSyu == 7)
            {
                #region Update Yyksho
                var yyksho = await context.TkdYyksho.FirstOrDefaultAsync(e => e.UkeNo == gridItem.UkeNo, cancellationToken);
                if (yyksho == null) throw new Exception($"{ nameof(AdditionalProcess) } throw an error: yyksho not found");
                yyksho.HenKai++;
                yyksho.NyuKinKbn = (byte)(mishum.NyuKinRui == 0 ? 1 :
                                        mishum.NyuKinRui == mishum.SeiKin ? 2 :
                                        (mishum.NyuKinRui > 0 && mishum.NyuKinRui < mishum.SeiKin) ? 3 : 4);
                yyksho.UpdYmd = currentYmd;
                yyksho.UpdTime = currentHms;
                yyksho.UpdSyainCd = currentUserId;
                yyksho.UpdPrgId = ScreenCode.CouponPaymentUpdPrgId;
                await context.SaveChangesAsync(cancellationToken);
                #endregion
            }
            else
            {
                #region Update Futtum
                var futtum = await context.TkdFutTum.FirstOrDefaultAsync(e => e.UkeNo == gridItem.UkeNo &&
                e.UnkRen == gridItem.UnkRen && e.FutTumRen == gridItem.FutTumRen &&
                e.FutTumKbn == (byte)(gridItem.SeiFutSyu == 6 ? 2 : 1), cancellationToken);
                if (futtum == null) throw new Exception($"{ nameof(AdditionalProcess) } throw an error: futtum not found");
                futtum.HenKai++;
                futtum.NyuKinKbn = (byte)(mishum.NyuKinRui == 0 ? 1 :
                                       mishum.NyuKinRui == mishum.SeiKin ? 2 :
                                       (mishum.NyuKinRui > 0 && mishum.NyuKinRui < mishum.SeiKin) ? 3 : 4);
                futtum.UpdYmd = currentYmd;
                futtum.UpdTime = currentHms;
                futtum.UpdSyainCd = currentUserId;
                futtum.UpdPrgId = ScreenCode.CouponPaymentUpdPrgId;
                await context.SaveChangesAsync(cancellationToken);
                #endregion
            }
        }
    }
}
