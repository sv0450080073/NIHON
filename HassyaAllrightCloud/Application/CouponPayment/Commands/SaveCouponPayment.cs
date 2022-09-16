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

namespace HassyaAllrightCloud.Application.CouponPayment.Commands
{
    public class SaveCouponPayment : IRequest<bool>
    {
        public bool IsUpdate { get; set; }
        public int CurrentTenant { get; set; }
        public int CurrentUserId { get; set; }
        public CouponPaymentPopupFormModel Model { get; set; }
        public CouponPaymentGridItem GridItem { get; set; }
        public class Handler : IRequestHandler<SaveCouponPayment, bool>
        {
            private KobodbContext _context { get; set; }
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<bool> Handle(SaveCouponPayment request, CancellationToken cancellationToken)
            {
                using var transac = _context.Database.BeginTransaction();
                try
                {
                    if (request == null || request.Model == null || request.GridItem == null) return false;
                    var currentYmd = DateTime.Now.ToString(DateTimeFormat.yyyyMMdd);
                    var currentHms = $"{DateTime.Now.Hour:00}{DateTime.Now.Minute:00}{DateTime.Now.Second:00}";

                    if (request.IsUpdate)
                    {
                        var nyuSih = await _context.TkdNyuSih.FirstOrDefaultAsync(e => e.NyuSihTblSeq == request.Model.NyuSihTblSeq);
                        if (nyuSih == null) throw new Exception($"{nameof(SaveCouponPayment)} throw an error: nyuSih not found, NyuSihTblSeq = {request.Model.NyuSihTblSeq}");
                        nyuSih = MapUpdateTkdNyuSih(nyuSih, request.Model, currentYmd, currentHms, request.CurrentUserId);
                        await _context.SaveChangesAsync(cancellationToken);

                        var nyshmi = await _context.TkdNyShmi.FirstOrDefaultAsync(e => e.NyuSihRen == request.Model.NyuSihRen && e.UkeNo == request.Model.UkeNo);
                        if (nyshmi == null) throw new Exception($"{nameof(SaveCouponPayment)} throw an error: nyshmi not found, NyuSihRen = {request.Model.NyuSihRen} && UkeNo = {request.Model.UkeNo}");
                        nyshmi = MapUpdateTkdNyShmi(nyuSih, nyshmi, request.Model, currentYmd, currentHms, request.CurrentUserId);
                        await _context.SaveChangesAsync(cancellationToken);
                    }
                    else
                    {
                        var nyuSih = MapTkdNyuSih(request.Model, currentYmd, currentHms, request.CurrentUserId);
                        await _context.TkdNyuSih.AddAsync(nyuSih, cancellationToken);
                        await _context.SaveChangesAsync(cancellationToken);

                        var nyshmi = MapTkdNyShmi(request.Model, nyuSih, request.GridItem, currentYmd, currentHms, request.CurrentUserId);
                        await _context.TkdNyShmi.AddAsync(nyshmi, cancellationToken);
                        await _context.SaveChangesAsync(cancellationToken);
                    }

                    await AdditionalProcess.Process(_context, request.GridItem, request.CurrentTenant, request.CurrentUserId, cancellationToken);
                    
                    await transac.CommitAsync(cancellationToken);
                    return true;
                }
                catch(Exception ex)
                {
                    await transac.RollbackAsync(cancellationToken);
                    throw ex;
                }
                
            }

            private TkdNyShmi MapTkdNyShmi(CouponPaymentPopupFormModel model, TkdNyuSih nyuSih, CouponPaymentGridItem gridItem, string ymd, string hms, int userId)
            {
                var currentNyuSihRen = _context.TkdNyShmi.Where(e => e.UkeNo == gridItem.UkeNo).Select(e => e.NyuSihRen).OrderByDescending(e => e).FirstOrDefault();
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
                    KesG = model.DepositAmount,
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
                    UpdPrgId = ScreenCode.CouponPaymentUpdPrgId
                };

                switch (model.DepositMethod)
                {
                    case DepositMethodEnum.Transfer:
                        nyshmi.KesG = model.DepositAmount - model.TransferFee - model.SponsorshipFee;
                        nyshmi.FurKesG = model.TransferFee;
                        nyshmi.KyoKesG = model.SponsorshipFee;
                        break;
                }

                return nyshmi;
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
                    NyuSihSyu = 0,
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
                    UpdPrgId = ScreenCode.CouponPaymentUpdPrgId
                };
                switch (model.DepositMethod)
                {
                    case DepositMethodEnum.Cash:
                        nyuSih.NyuSihSyu = (byte)DepositMethodEnum.Cash;
                        break;
                    case DepositMethodEnum.Transfer:
                        nyuSih.NyuSihSyu = (byte)DepositMethodEnum.Transfer;
                        nyuSih.NyuSihG = model.DepositAmount - model.TransferFee - model.SponsorshipFee;
                        nyuSih.FuriTes = model.TransferFee;
                        nyuSih.KyoRyoKin = model.SponsorshipFee;
                        nyuSih.BankCd = model.BankTransfer.BankCd;
                        nyuSih.BankSitCd = model.BankTransfer.BankSitCd;
                        nyuSih.YokinSyu = (byte)model.DepositType;
                        break;
                    case DepositMethodEnum.Card:
                        nyuSih.NyuSihSyu = (byte)DepositMethodEnum.Card;
                        nyuSih.CardSyo = model.CardSyo;
                        nyuSih.CardDen = model.CardDen;
                        break;
                    case DepositMethodEnum.Bill:
                        nyuSih.NyuSihSyu = (byte)DepositMethodEnum.Bill;
                        nyuSih.TegataYmd = model.TegataYmd.ToString(DateTimeFormat.yyyyMMdd);
                        nyuSih.TegataNo = model.TegataNo;
                        break;
                    case DepositMethodEnum.Offset:
                        nyuSih.NyuSihSyu = (byte)DepositMethodEnum.Offset;
                        break;
                    case DepositMethodEnum.AdjustmentMoney:
                        nyuSih.NyuSihSyu = (byte)DepositMethodEnum.AdjustmentMoney;
                        break;
                    case DepositMethodEnum.DepositorAndOther1:
                    case DepositMethodEnum.DepositorAndOther2:
                        nyuSih.NyuSihSyu = (byte)model.DepositMethod;
                        nyuSih.EtcSyo1 = model.EtcSyo1;
                        nyuSih.EtcSyo2 = model.EtcSyo2;
                        break;
                }
                return nyuSih;
            }

            private TkdNyShmi MapUpdateTkdNyShmi(TkdNyuSih currentTkdNyuSih, TkdNyShmi current, CouponPaymentPopupFormModel model, string ymd, string hms, int userId)
            {
                current.HenKai++;
                current.KesG = model.DepositAmount;
                current.UpdYmd = ymd;
                current.UpdTime = hms;
                current.UpdSyainCd = userId;
                current.UpdPrgId = ScreenCode.CouponPaymentUpdPrgId;
                current.FurKesG = 0;
                current.KyoKesG = 0;
                switch (currentTkdNyuSih.NyuSihSyu)
                {
                    case (int)DepositMethodEnum.Transfer:
                        current.KesG = model.DepositAmount - model.TransferFee - model.SponsorshipFee;
                        current.FurKesG = model.TransferFee;
                        current.KyoKesG = model.SponsorshipFee;
                        break;
                }

                return current;
            }

            private TkdNyuSih MapUpdateTkdNyuSih(TkdNyuSih current, CouponPaymentPopupFormModel model, string ymd, string hms, int userId)
            {
                current.HenKai++;
                current.NyuSihYmd = model.DepositDate.ToString(DateTimeFormat.yyyyMMdd);
                current.NyuSihEigSeq = model.DepositOffice.EigyoCdSeq;
                current.CardSyo = string.Empty;
                current.CardDen = string.Empty;
                current.NyuSihG = model.DepositAmount;
                current.FuriTes = 0;
                current.KyoRyoKin = 0;
                current.BankCd = string.Empty;
                current.BankSitCd = string.Empty;
                current.YokinSyu = 0;
                current.TegataYmd = string.Empty;
                current.TegataNo = string.Empty;
                current.EtcSyo1 = string.Empty;
                current.EtcSyo2 = string.Empty;
                current.UpdYmd = ymd;
                current.UpdTime = hms;
                current.UpdSyainCd = userId;
                current.UpdPrgId = ScreenCode.CouponPaymentUpdPrgId;
                current.NyuSihSyu = (byte)model.DepositMethod;
                switch (model.DepositMethod)
                {
                    case DepositMethodEnum.Transfer:
                        current.NyuSihG = model.DepositAmount - model.TransferFee - model.SponsorshipFee;
                        current.FuriTes = model.TransferFee;
                        current.KyoRyoKin = model.SponsorshipFee;
                        current.BankCd = model.BankTransfer.BankCd;
                        current.BankSitCd = model.BankTransfer.BankSitCd;
                        current.YokinSyu = (byte)model.DepositType;
                        break;
                    case DepositMethodEnum.Card:
                        current.CardSyo = model.CardSyo;
                        current.CardDen = model.CardDen;
                        break;
                    case DepositMethodEnum.Bill:
                        current.TegataYmd = model.TegataYmd.ToString(DateTimeFormat.yyyyMMdd);
                        current.TegataNo = model.TegataNo;
                        break;
                    case DepositMethodEnum.DepositorAndOther1:
                    case DepositMethodEnum.DepositorAndOther2:
                        current.EtcSyo1 = model.EtcSyo1;
                        current.EtcSyo2 = model.EtcSyo2;
                        break;
                }
                return current;
            }
        }
    }
}
