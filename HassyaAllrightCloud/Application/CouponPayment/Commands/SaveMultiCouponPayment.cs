using HassyaAllrightCloud.Commons;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using HassyaAllrightCloud.Commons.Constants;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Application.CouponPayment.Commands
{
    public class SaveMultiCouponPayment : IRequest<bool>
    {
        public int CurrentTenant { get; set; }
        public int CurrentUserId { get; set; }
        public List<CouponPaymentGridItem> GridItems { get; set; }
        public CouponPaymentPopupFormModel Model { get; set; }
        public class Handler : IRequestHandler<SaveMultiCouponPayment, bool>
        {
            private KobodbContext _context { get; set; }
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<bool> Handle(SaveMultiCouponPayment request, CancellationToken cancellationToken)
            {
                using var transac = _context.Database.BeginTransaction();
                try
                {
                    if (request == null || request.Model == null || request.GridItems == null) return false;
                    var currentYmd = DateTime.Now.ToString(DateTimeFormat.yyyyMMdd);
                    var currentHms = $"{DateTime.Now.Hour:00}{DateTime.Now.Minute:00}{DateTime.Now.Second:00}";

                    var nyuSih = MapTkdNyuSih(request.Model, currentYmd, currentHms, request.CurrentUserId);
                    await _context.TkdNyuSih.AddAsync(nyuSih, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);

                    var maxItem = request.GridItems.OrderByDescending(e => e.CouKesG - e.NyuKinRui).FirstOrDefault();
                    foreach (var item in request.GridItems)
                    {
                        var isMax = item.UkeNo == maxItem.UkeNo && item.NyuSihCouRen == maxItem.NyuSihCouRen;
                        var nyshmi = MapTkdNyShmi(request.Model, nyuSih, item, currentYmd, currentHms, request.CurrentUserId, isMax);
                        await _context.TkdNyShmi.AddAsync(nyshmi, cancellationToken);
                        await _context.SaveChangesAsync(cancellationToken);

                        await AdditionalProcess.Process(_context, item, request.CurrentTenant, request.CurrentUserId, cancellationToken);
                    }

                    await transac.CommitAsync(cancellationToken);
                    return true;
                }
                catch (Exception ex)
                {
                    await transac.RollbackAsync(cancellationToken);
                    throw ex;
                }
            }

            private TkdNyuSih MapTkdNyuSih(CouponPaymentPopupFormModel model, string ymd, string hms, int userId)
            {
                var currentTenantId = new ClaimModel().TenantID;
                var max = _context.TkdNyuSih.Where(e => e.TenantCdSeq == currentTenantId).Max(e => e.NyuSihTblSeq);
                var nyuSih = new TkdNyuSih()
                {
                    NyuSihTblSeq = max + 1,
                    TenantCdSeq = currentTenantId,
                    HenKai = 0,
                    NyuSihKbn = 1,
                    NyuSihYmd = model.DepositDate.ToString(DateTimeFormat.yyyyMMdd),
                    NyuSihEigSeq = model.DepositOffice.EigyoCdSeq,
                    NyuSihSyu = (byte)model.DepositMethod,
                    CardSyo = string.Empty,
                    CardDen = string.Empty,
                    NyuSihG = model.DepositAmount,
                    FuriTes = 0,
                    KyoRyoKin = 0,
                    BankCd = string.Empty,
                    BankSitCd = string.Empty,
                    YokinSyu = 0,
                    TegataYmd = string.Empty,
                    TegataNo = string.Empty,
                    EtcSyo1 = string.Empty,
                    EtcSyo2 = string.Empty,
                    SiyoKbn = 1,
                    UpdYmd = ymd,
                    UpdTime = hms,
                    UpdSyainCd = userId,
                    UpdPrgId = ScreenCode.CouponMultiPaymentUpdPrgId
                };
                switch (model.DepositMethod)
                {
                    case DepositMethodEnum.Transfer:
                        nyuSih.NyuSihG = model.DepositAmount - model.TransferFee - model.SponsorshipFee;
                        nyuSih.FuriTes = model.TransferFee;
                        nyuSih.KyoRyoKin = model.SponsorshipFee;
                        nyuSih.BankCd = model.BankTransfer.BankCd;
                        nyuSih.BankSitCd = model.BankTransfer.BankSitCd;
                        nyuSih.YokinSyu = (byte)model.DepositType;
                        break;
                    case DepositMethodEnum.Card:
                        nyuSih.CardSyo = model.CardSyo;
                        nyuSih.CardDen = model.CardDen;
                        break;
                    case DepositMethodEnum.Bill:
                        nyuSih.TegataYmd = model.TegataYmd.ToString(DateTimeFormat.yyyyMMdd);
                        nyuSih.TegataNo = model.TegataNo;
                        break;
                    case DepositMethodEnum.DepositorAndOther1:
                    case DepositMethodEnum.DepositorAndOther2:
                        nyuSih.EtcSyo1 = model.EtcSyo1;
                        nyuSih.EtcSyo2 = model.EtcSyo2;
                        break;
                }
                return nyuSih;
            }

            private TkdNyShmi MapTkdNyShmi(CouponPaymentPopupFormModel model, TkdNyuSih nyuSih, CouponPaymentGridItem gridItem, 
                string ymd, string hms, int userId, bool isBiggest)
            {
                var currentNyuSihRen = _context.TkdNyShmi.Where(e => e.UkeNo == gridItem.UkeNo).Select(e => e.NyuSihRen)
                    .OrderByDescending(e => e).FirstOrDefault();
                var nyshmi = new TkdNyShmi()
                {
                    TenantCdSeq = new ClaimModel().TenantID,
                    UkeNo = gridItem.UkeNo,
                    NyuSihRen = ++currentNyuSihRen,
                    HenKai = 0,
                    NyuSihKbn = 1,
                    SeiFutSyu = gridItem.SeiFutSyu,
                    UnkRen = gridItem.UnkRen,
                    YouTblSeq = gridItem.YouTblSeq,
                    KesG = gridItem.CouKesG - gridItem.NyuKinRui,
                    FurKesG = 0,
                    KyoKesG = 0,
                    FutTumRen = gridItem.FutTumRen,
                    NyuSihCouRen = gridItem.NyuSihCouRen,
                    NyuSihTblSeq = nyuSih.NyuSihTblSeq,
                    CouTblSeq = gridItem.CouTblSeq,
                    SiyoKbn = 1,
                    UpdYmd = ymd,
                    UpdTime = hms,
                    UpdSyainCd = userId,
                    UpdPrgId = ScreenCode.CouponMultiPaymentUpdPrgId
                };

                switch (model.DepositMethod)
                {
                    case DepositMethodEnum.Transfer:
                        if (isBiggest)
                        {
                            nyshmi.KesG = gridItem.CouKesG - gridItem.NyuKinRui - model.TransferFee - model.SponsorshipFee;
                            nyshmi.FurKesG = model.TransferFee;
                            nyshmi.KyoKesG = model.SponsorshipFee;
                        }
                        break;
                }

                return nyshmi;
            }

        }
    }
}
