using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto.BillPrint;
using HassyaAllrightCloud.Domain.Dto.DepositCoupon;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static HassyaAllrightCloud.Commons.Constants.Constants;

namespace HassyaAllrightCloud.Application.BillPrint.Queries
{
    public class SavePaymentAsyncCommand : IRequest<string>
    {
        public DepositCouponGrid depositCouponGrid { get; set; } = new DepositCouponGrid();
        public DepositCouponPayment depositCouponPayment { get; set; } = new DepositCouponPayment()
        {
            OffsetPaymentTables = new List<OffsetPaymentTable>() { new OffsetPaymentTable() }
        };
        public DepositPaymentGrid depositPaymentGrid { get; set; }
        public DepositPaymentHaitaCheck depositPaymentHaitaCheck { get; set; }
        public bool isDeleted { get; set; }
        public readonly string couponCode = "07";
        public class Handler : IRequestHandler<SavePaymentAsyncCommand, string>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<SavePaymentAsyncCommand> _logger;

            public Handler(KobodbContext context, ILogger<SavePaymentAsyncCommand> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<string> Handle(SavePaymentAsyncCommand request, CancellationToken cancellationToken = default)
            {
                string errorMessage = null;
                if (request.depositCouponGrid == null || request.depositCouponPayment == null)
                    return ErrorMessage.SAVEFAIL;
                using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbTran = _context.Database.BeginTransaction())
                {
                    try
                    {
                        //delete
                        if(request.isDeleted)
                            errorMessage = await DeletePayment(request);
                        else
                        {
                            //create
                            if (request.depositPaymentGrid == null)
                            {
                                if (request.depositCouponPayment.DepositMethod != request.couponCode)
                                    await CreatePaymentNotCoupon(request);
                                else
                                    await CreatePaymentCoupon(request);
                            } //update 
                            else
                            {
                                if (request.depositCouponPayment.DepositMethod != request.couponCode)
                                    errorMessage = await UpdatePaymentNotCoupon(request);
                                else
                                    errorMessage = await UpdatePaymentCoupon(request);
                            }
                        }

                        if(errorMessage == null)
                        {
                            errorMessage = await LastUpdate(request);
                            if(errorMessage == null)
                            {
                                await _context.SaveChangesAsync();
                                await dbTran.CommitAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        //Rollback transaction if exception occurs  
                        dbTran.Rollback();
                        return ErrorMessage.SAVEFAIL;
                    }
                    return errorMessage;
                }
            }

            public async Task CreatePaymentNotCoupon(SavePaymentAsyncCommand request)
            {
                var claimModel = new HassyaAllrightCloud.Domain.Dto.ClaimModel();
                var tkdNyuSihData = _context.TkdNyuSih.Where(x => x.TenantCdSeq == claimModel.TenantID)
                .OrderByDescending(x => x.NyuSihTblSeq).FirstOrDefault();
                var tkdNyuSih = new TkdNyuSih()
                {
                    HenKai = 0,
                    NyuSihKbn = 1,
                    NyuSihYmd = request.depositCouponPayment.DepositDate?.ToString(Commons.Constants.Formats.yyyyMMdd),
                    NyuSihEigSeq = int.Parse(request.depositCouponPayment.DepositOffice.Code),
                    NyuSihSyu = request.depositCouponPayment.DepositMethod == "01" ? (byte)1 : request.depositCouponPayment.DepositMethod == "02" ? (byte)2 :
                    request.depositCouponPayment.DepositMethod == "03" ? (byte)3 : request.depositCouponPayment.DepositMethod == "04" ? (byte)4 :
                    request.depositCouponPayment.DepositMethod == "05" ? (byte)5 : request.depositCouponPayment.DepositMethod == "06" ? (byte)6 :
                    request.depositCouponPayment.DepositMethod == "91" ? (byte)91 : request.depositCouponPayment.DepositMethod == "92" ? (byte)92 : (byte)0,
                    CardSyo = request.depositCouponPayment.DepositMethod == "03" ? request.depositCouponPayment.CardApprovalNumber : string.Empty,
                    CardDen = request.depositCouponPayment.DepositMethod == "03" ? request.depositCouponPayment.CardSlipNumber : string.Empty,
                    NyuSihG = request.depositCouponPayment.DepositMethod == "02" ? ((int)request.depositCouponPayment.DepositAmount -
                    request.depositCouponPayment.TransferFee - request.depositCouponPayment.SponsorshipFund)
                    : (int)request.depositCouponPayment.DepositAmount,
                    FuriTes = request.depositCouponPayment.DepositMethod == "02" ? request.depositCouponPayment.TransferFee : 0,
                    KyoRyoKin = request.depositCouponPayment.DepositMethod == "02" ? request.depositCouponPayment.SponsorshipFund : 0,
                    BankCd = request.depositCouponPayment.DepositMethod == "02" ? request.depositCouponPayment.DepositTransferBank?.BankCd : string.Empty,
                    BankSitCd = request.depositCouponPayment.DepositMethod == "02" ? request.depositCouponPayment.DepositTransferBank?.BankSitCd : string.Empty,
                    YokinSyu = request.depositCouponPayment.DepositMethod == "02" ? request.depositCouponPayment.DepositType : (byte)0,
                    TegataYmd = request.depositCouponPayment.DepositMethod == "04" ? request.depositCouponPayment.BillDate?.ToString(Commons.Constants.Formats.yyyyMMdd) : string.Empty,
                    TegataNo = request.depositCouponPayment.DepositMethod == "04" ? request.depositCouponPayment.BillNo : string.Empty,
                    EtcSyo1 = request.depositCouponPayment.DepositMethod == "91" ? request.depositCouponPayment.DetailedNameOfDepositMeans11
                    : request.depositCouponPayment.DepositMethod == "92" ? request.depositCouponPayment.DetailedNameOfDepositMeans21 : string.Empty,
                    EtcSyo2 = request.depositCouponPayment.DepositMethod == "91" ? request.depositCouponPayment.DetailedNameOfDepositMeans12
                    : request.depositCouponPayment.DepositMethod == "92" ? request.depositCouponPayment.DetailedNameOfDepositMeans22 : string.Empty,
                    SiyoKbn = 1,
                    UpdYmd = DateTime.Now.ToString(Commons.Constants.Formats.yyyyMMdd),
                    UpdTime = DateTime.Now.ToString(Commons.Constants.Formats.HHmmss),
                    UpdPrgId = "KK2300P",
                    UpdSyainCd = claimModel.SyainCdSeq,
                    TenantCdSeq = claimModel.TenantID,
                    NyuSihTblSeq = tkdNyuSihData == null ? 1 : (tkdNyuSihData.NyuSihTblSeq + 1)
                };
                await _context.AddAsync(tkdNyuSih);
                await _context.SaveChangesAsync();

                var tkdNyShmiData = _context.TkdNyShmi.Where(x => x.UkeNo == request.depositCouponGrid.UkeNo && x.TenantCdSeq == claimModel.TenantID)
                .OrderByDescending(x => x.NyuSihRen).FirstOrDefault();
                short nyuSihRen = (short)(tkdNyShmiData == null ? (short)1 : (tkdNyShmiData.NyuSihRen + (short)1));
                var tkdNyShmi = new TkdNyShmi()
                {
                    UkeNo = request.depositCouponGrid.UkeNo,
                    NyuSihRen = nyuSihRen,
                    HenKai = 0,
                    NyuSihKbn = 1,
                    SeiFutSyu = request.depositCouponGrid.SeiFutSyu,
                    UnkRen = request.depositCouponGrid.FutuUnkRen,
                    YouTblSeq = 0,
                    KesG = request.depositCouponPayment.DepositMethod == "02" ? ((int)request.depositCouponPayment.DepositAmount -
                    request.depositCouponPayment.TransferFee - request.depositCouponPayment.SponsorshipFund)
                    : (int)request.depositCouponPayment.DepositAmount,
                    FurKesG = request.depositCouponPayment.DepositMethod == "02" ? request.depositCouponPayment.TransferFee : 0,
                    KyoKesG = request.depositCouponPayment.DepositMethod == "02" ? request.depositCouponPayment.SponsorshipFund : 0,
                    FutTumRen = request.depositCouponGrid.FutTumRen,
                    NyuSihCouRen = 0,
                    NyuSihTblSeq = tkdNyuSih.NyuSihTblSeq,
                    CouTblSeq = 0,
                    SiyoKbn = 1,
                    UpdYmd = DateTime.Now.ToString(Commons.Constants.Formats.yyyyMMdd),
                    UpdTime = DateTime.Now.ToString(Commons.Constants.Formats.HHmmss),
                    UpdPrgId = "KK2300P",
                    UpdSyainCd = claimModel.SyainCdSeq,
                    TenantCdSeq = claimModel.TenantID
                };
                await _context.AddAsync(tkdNyShmi);
            }

            public async Task<string> UpdatePaymentNotCoupon(SavePaymentAsyncCommand request)
            {
                var claimModel = new HassyaAllrightCloud.Domain.Dto.ClaimModel();
                var tkdNyuSih = _context.TkdNyuSih.Where(x => x.NyuSihTblSeq == request.depositPaymentGrid.NS_NyuSihTblSeq 
                && x.TenantCdSeq == claimModel.TenantID).FirstOrDefault();
                if (tkdNyuSih != null) {
                    if ((tkdNyuSih.UpdYmd + tkdNyuSih.UpdTime) != request.depositPaymentHaitaCheck.tkdNyuSihUpdYmdTime)
                        return "BI_T011";

                    tkdNyuSih.HenKai += (short)1;
                    tkdNyuSih.NyuSihSyu = request.depositCouponPayment.DepositMethod == "01" ? (byte)1 : request.depositCouponPayment.DepositMethod == "02" ? (byte)2 :
                    request.depositCouponPayment.DepositMethod == "03" ? (byte)3 : request.depositCouponPayment.DepositMethod == "04" ? (byte)4 :
                    request.depositCouponPayment.DepositMethod == "05" ? (byte)5 : request.depositCouponPayment.DepositMethod == "06" ? (byte)6 :
                    request.depositCouponPayment.DepositMethod == "91" ? (byte)91 : request.depositCouponPayment.DepositMethod == "92" ? (byte)92 : (byte)0;
                    tkdNyuSih.NyuSihEigSeq = int.Parse(request.depositCouponPayment.DepositOffice.Code);
                    tkdNyuSih.NyuSihYmd = request.depositCouponPayment.DepositDate?.ToString(Commons.Constants.Formats.yyyyMMdd);
                    tkdNyuSih.CardSyo = request.depositCouponPayment.DepositMethod == "03" ? request.depositCouponPayment.CardApprovalNumber : string.Empty;
                    tkdNyuSih.CardDen = request.depositCouponPayment.DepositMethod == "03" ? request.depositCouponPayment.CardSlipNumber : string.Empty;
                    tkdNyuSih.NyuSihG = request.depositCouponPayment.DepositMethod == "02" ? ((int)request.depositCouponPayment.DepositAmount -
                    request.depositCouponPayment.TransferFee - request.depositCouponPayment.SponsorshipFund)
                    : (int)request.depositCouponPayment.DepositAmount;
                    tkdNyuSih.FuriTes = request.depositCouponPayment.DepositMethod == "02" ? request.depositCouponPayment.TransferFee : 0;
                    tkdNyuSih.KyoRyoKin = request.depositCouponPayment.DepositMethod == "02" ? request.depositCouponPayment.SponsorshipFund : 0;
                    tkdNyuSih.BankCd = request.depositCouponPayment.DepositMethod == "02" ? request.depositCouponPayment.DepositTransferBank.BankCd : string.Empty;
                    tkdNyuSih.BankSitCd = request.depositCouponPayment.DepositMethod == "02" ? request.depositCouponPayment.DepositTransferBank.BankSitCd : string.Empty;
                    tkdNyuSih.YokinSyu = request.depositCouponPayment.DepositMethod == "02" ? request.depositCouponPayment.DepositType : (byte)0;
                    tkdNyuSih.TegataYmd = request.depositCouponPayment.DepositMethod == "04" ? request.depositCouponPayment.BillDate?.ToString(Commons.Constants.Formats.yyyyMMdd) : string.Empty;
                    tkdNyuSih.TegataNo = request.depositCouponPayment.DepositMethod == "04" ? request.depositCouponPayment.BillNo : string.Empty;
                    tkdNyuSih.EtcSyo1 = request.depositCouponPayment.DepositMethod == "91" ? request.depositCouponPayment.DetailedNameOfDepositMeans11
                        : request.depositCouponPayment.DepositMethod == "92" ? request.depositCouponPayment.DetailedNameOfDepositMeans21 : string.Empty;
                    tkdNyuSih.EtcSyo2 = request.depositCouponPayment.DepositMethod == "91" ? request.depositCouponPayment.DetailedNameOfDepositMeans12
                        : request.depositCouponPayment.DepositMethod == "92" ? request.depositCouponPayment.DetailedNameOfDepositMeans22 : string.Empty;
                    tkdNyuSih.UpdYmd = DateTime.Now.ToString(Commons.Constants.Formats.yyyyMMdd);
                    tkdNyuSih.UpdTime = DateTime.Now.ToString(Commons.Constants.Formats.HHmmss);
                    tkdNyuSih.UpdPrgId = "KK2300P";
                    tkdNyuSih.UpdSyainCd = claimModel.SyainCdSeq;
                    _context.Update(tkdNyuSih);
                }

                var tkdNyShmi = _context.TkdNyShmi.Where(x => x.UkeNo == request.depositPaymentGrid.UkeNo 
                && x.NyuSihRen == request.depositPaymentGrid.NyuSihRen && x.TenantCdSeq == claimModel.TenantID).FirstOrDefault();
                if (tkdNyShmi != null) {
                    if((tkdNyShmi.UpdYmd + tkdNyShmi.UpdTime) != request.depositPaymentHaitaCheck.tkdNyShmiUpdYmdTime)
                        return "BI_T011";
                    tkdNyShmi.HenKai += (short)1;
                    tkdNyShmi.KesG = request.depositCouponPayment.DepositMethod == "02" ? ((int)request.depositCouponPayment.DepositAmount -
                    request.depositCouponPayment.TransferFee - request.depositCouponPayment.SponsorshipFund)
                    : (int)request.depositCouponPayment.DepositAmount;
                    tkdNyShmi.FurKesG = request.depositCouponPayment.DepositMethod == "02" ? request.depositCouponPayment.TransferFee : 0;
                    tkdNyShmi.KyoKesG = request.depositCouponPayment.DepositMethod == "02" ? request.depositCouponPayment.SponsorshipFund : 0;
                    tkdNyShmi.NyuSihCouRen = 0;
                    tkdNyShmi.UpdYmd = DateTime.Now.ToString(Commons.Constants.Formats.yyyyMMdd);
                    tkdNyShmi.UpdTime = DateTime.Now.ToString(Commons.Constants.Formats.HHmmss);
                    tkdNyShmi.UpdPrgId = "KK2300P";
                    tkdNyShmi.UpdSyainCd = claimModel.SyainCdSeq;
                    _context.Update(tkdNyShmi);
                }
                return null;
            }

            public async Task CreatePaymentCoupon(SavePaymentAsyncCommand request)
            {
                var claimModel = new HassyaAllrightCloud.Domain.Dto.ClaimModel();
                List<TkdNyShCu> tkdNyShCus = new List<TkdNyShCu>();
                var count = 1;
                var tkdCouponData = _context.TkdCoupon.Where(x => x.TenantCdSeq == claimModel.TenantID).OrderByDescending(x => x.CouTblSeq).FirstOrDefault();
                var couTblSeq = tkdCouponData == null ? 1 : (tkdCouponData.CouTblSeq + 1);
                var tkdNyShCuData = _context.TkdNyShCu.Where(x => x.UkeNo == request.depositCouponGrid.UkeNo && x.TenantCdSeq == claimModel.TenantID)
                .OrderByDescending(x => x.NyuSihCouRen).FirstOrDefault();
                var nyuSihCouRen = tkdNyShCuData == null ? (short)0 : tkdNyShCuData.NyuSihCouRen;
                //coupon
                foreach (var item in request.depositCouponPayment.OffsetPaymentTables)
                {
                    var tkdCoupon = new TkdCoupon()
                    {
                        HenKai = 0,
                        NyuSihKbn = 1,
                        SeiSihCdSeq = request.depositCouponGrid.SeiCd,
                        SeiSihSitCdSeq = request.depositCouponGrid.SeiSitenCd,
                        HakoYmd = item.DateOfIssue?.ToString(Commons.Constants.Formats.yyyyMMdd),
                        NyuSihEigSeq = int.Parse(request.depositCouponPayment.DepositOffice.Code),
                        CouNo = item.CouponNo,
                        CouGkin = (int)item.FaceValue,
                        CouZanFlg = int.Parse(request.depositCouponPayment.BillAmount, NumberStyles.Currency) - int.Parse(request.depositCouponPayment.SumCouponsApplied, NumberStyles.Currency)
                        - item.FaceValue > (byte)0 ? (byte)0 : (byte)1,
                        CouZan = Math.Max(0, (int)item.FaceValue + int.Parse(request.depositCouponPayment.SumCouponsApplied, NumberStyles.Currency)
                        - int.Parse(request.depositCouponPayment.BillAmount, NumberStyles.Currency)),
                        SiyoKbn = 1,
                        UpdYmd = DateTime.Now.ToString(Commons.Constants.Formats.yyyyMMdd),
                        UpdTime = DateTime.Now.ToString(Commons.Constants.Formats.HHmmss),
                        UpdPrgId = "KK2300P",
                        UpdSyainCd = claimModel.SyainCdSeq,
                        CouTblSeq = couTblSeq,
                        TenantCdSeq = claimModel.TenantID
                    };
                    _context.Add(tkdCoupon);
                    _context.SaveChanges();
                    var tkdNyShCu = new TkdNyShCu()
                    {
                        UkeNo = request.depositCouponGrid.UkeNo,
                        NyuSihCouRen = (short)(nyuSihCouRen + (short)count),
                        HenKai = 0,
                        NyuSihKbn = 1,
                        SeiFutSyu = request.depositCouponGrid.SeiFutSyu,
                        NyuKesiKbn = 1,
                        UnkRen = request.depositCouponGrid.FutuUnkRen,
                        YouTblSeq = 0,
                        CouKesG = item.ApplicationAmount.GetValueOrDefault(),
                        FutTumRen = request.depositCouponGrid.FutTumRen,
                        CouTblSeq = tkdCoupon.CouTblSeq,
                        SiyoKbn = 1,
                        UpdYmd = DateTime.Now.ToString(Commons.Constants.Formats.yyyyMMdd),
                        UpdTime = DateTime.Now.ToString(Commons.Constants.Formats.HHmmss),
                        UpdPrgId = "KK2300P",
                        UpdSyainCd = claimModel.SyainCdSeq,
                        TenantCdSeq = claimModel.TenantID
                    };
                    _context.Add(tkdNyShCu);
                    _context.SaveChanges();
                    count++;
                }
            }

            public async Task<string> UpdatePaymentCoupon(SavePaymentAsyncCommand request)
            {
                var offsetPaymentTable = request.depositCouponPayment.OffsetPaymentTables.FirstOrDefault();
                if (offsetPaymentTable == null)
                    return ErrorMessage.SAVEFAIL;
                var claimModel = new HassyaAllrightCloud.Domain.Dto.ClaimModel();
                var tkdCoupon = _context.TkdCoupon.Where(x => x.CouTblSeq == request.depositPaymentGrid.CouTblSeq && x.TenantCdSeq == claimModel.TenantID).FirstOrDefault();
                if (tkdCoupon != null) {
                    if ((tkdCoupon.UpdYmd + tkdCoupon.UpdTime) != request.depositPaymentHaitaCheck.tkdCouponUpdYmdTime)
                        return "BI_T011";
                    tkdCoupon.HenKai += 1;
                    tkdCoupon.HakoYmd = offsetPaymentTable.DateOfIssue?.ToString(Formats.yyyyMMdd);
                    tkdCoupon.NyuSihEigSeq = int.Parse(request.depositCouponPayment.DepositOffice.Code);
                    tkdCoupon.CouNo = offsetPaymentTable.CouponNo;
                    tkdCoupon.CouZan = tkdCoupon.CouZan - (request.depositCouponPayment.IsChangeOrDeleteCoupon ? 0
                        : (offsetPaymentTable.ApplicationAmount.GetValueOrDefault() - request.depositPaymentGrid.CouKesG));
                    tkdCoupon.CouGkin = offsetPaymentTable.ApplicationAmount.GetValueOrDefault() + tkdCoupon.CouZan;
                    tkdCoupon.CouZanFlg = request.depositCouponPayment.IsChangeOrDeleteCoupon ? tkdCoupon.CouZanFlg : (byte)1;
                    tkdCoupon.UpdYmd = DateTime.Now.ToString(Commons.Constants.Formats.yyyyMMdd);
                    tkdCoupon.UpdTime = DateTime.Now.ToString(Commons.Constants.Formats.HHmmss);
                    tkdCoupon.UpdPrgId = "KK2300P";
                    tkdCoupon.UpdSyainCd = claimModel.SyainCdSeq;
                    _context.Update(tkdCoupon);
                }

                var tkdNyShCu = _context.TkdNyShCu.Where(x => x.UkeNo == request.depositPaymentGrid.UkeNo 
                && x.NyuSihCouRen == request.depositPaymentGrid.NyuSihCouRen && x.TenantCdSeq == claimModel.TenantID).FirstOrDefault();
                if (tkdNyShCu != null) {
                    if ((tkdNyShCu.UpdYmd + tkdNyShCu.UpdTime) != request.depositPaymentHaitaCheck.tkdNyShCuUpdYmdTime)
                            return "BI_T011";
                    tkdNyShCu.HenKai += 1;
                    tkdNyShCu.CouKesG = offsetPaymentTable.ApplicationAmount.GetValueOrDefault();
                    tkdNyShCu.UpdYmd = DateTime.Now.ToString(Commons.Constants.Formats.yyyyMMdd);
                    tkdNyShCu.UpdTime = DateTime.Now.ToString(Commons.Constants.Formats.HHmmss);
                    tkdNyShCu.UpdPrgId = "KK2300P";
                    tkdNyShCu.UpdSyainCd = claimModel.SyainCdSeq;
                    _context.Update(tkdNyShCu);
                }
                return null;
            }

            public async Task<string> LastUpdate(SavePaymentAsyncCommand request)
            {
                var claimModel = new HassyaAllrightCloud.Domain.Dto.ClaimModel();
                var tkdMishum = _context.TkdMishum.Where(x => x.UkeNo == request.depositCouponGrid.UkeNo && x.MisyuRen == request.depositCouponGrid.MisyuRen).FirstOrDefault();
                if (tkdMishum == null)
                    return ErrorMessage.SAVEFAIL;
                //haita check
                if ((tkdMishum.UpdYmd + tkdMishum.UpdTime) != request.depositPaymentHaitaCheck.tkdMishumUpdYmdTime)
                    return "BI_T011";
                tkdMishum.HenKai += 1;
                tkdMishum.NyuKinRui = decimal.Parse(request.depositCouponPayment.CumulativeDeposit, NumberStyles.Currency);
                tkdMishum.CouKesRui = int.Parse(request.depositCouponPayment.CumulativeCouponsApplied, NumberStyles.Currency);
                tkdMishum.UpdYmd = DateTime.Now.ToString(Commons.Constants.Formats.yyyyMMdd);
                tkdMishum.UpdTime = DateTime.Now.ToString(Commons.Constants.Formats.HHmmss);
                tkdMishum.UpdPrgId = "KK2300P";
                tkdMishum.UpdSyainCd = claimModel.SyainCdSeq;
                _context.Update(tkdMishum);

                if (request.depositCouponGrid.SeiFutSyu == 1 || request.depositCouponGrid.SeiFutSyu == 7)
                {
                    var tkdYyksho = _context.TkdYyksho.Where(x => x.UkeNo == request.depositCouponGrid.UkeNo && x.TenantCdSeq == claimModel.TenantID).FirstOrDefault();
                    if (tkdYyksho != null) {
                        if (request.depositPaymentGrid != null)
                            if ((tkdYyksho.UpdYmd + tkdYyksho.UpdTime) != request.depositPaymentHaitaCheck.tkdYykshoUpdYmdTime)
                                return "BI_T011";
                        tkdYyksho.HenKai += (short)1;
                        tkdYyksho.NyuKinKbn = tkdMishum.NyuKinRui == (decimal)0 ? (byte)1 : ((int)tkdMishum.NyuKinRui == int.Parse(request.depositCouponPayment.BillAmount, NumberStyles.Currency)) ? (byte)2
                            : ((int)tkdMishum.NyuKinRui < int.Parse(request.depositCouponPayment.BillAmount, NumberStyles.Currency)) ? (byte)3
                            : ((int)tkdMishum.NyuKinRui > int.Parse(request.depositCouponPayment.BillAmount, NumberStyles.Currency)) ? (byte)4 : (byte)0;
                        tkdYyksho.NcouKbn = tkdMishum.CouKesRui == 0 ? (byte)1 : (tkdMishum.CouKesRui >= int.Parse(request.depositCouponPayment.BillAmount, NumberStyles.Currency)) ? (byte)2
                            : (tkdMishum.CouKesRui < int.Parse(request.depositCouponPayment.BillAmount, NumberStyles.Currency)) ? (byte)3 : (byte)0;
                        tkdYyksho.UpdYmd = DateTime.Now.ToString(Commons.Constants.Formats.yyyyMMdd);
                        tkdYyksho.UpdTime = DateTime.Now.ToString(Commons.Constants.Formats.HHmmss);
                        tkdYyksho.UpdPrgId = "KK2300P";
                        tkdYyksho.UpdSyainCd = claimModel.SyainCdSeq;
                        _context.Update(tkdYyksho);
                    }
                }
                else
                {
                    var tkdFutTum = _context.TkdFutTum.Where(x => x.UkeNo == request.depositCouponGrid.UkeNo && x.UnkRen == request.depositCouponGrid.FutuUnkRen
                    && x.FutTumRen == request.depositCouponGrid.FutTumRen && x.FutTumKbn == (request.depositCouponGrid.SeiFutSyu == (byte)6 ? (byte)2 : (byte)1)).FirstOrDefault();
                    if (tkdFutTum != null) {
                        if (request.depositPaymentGrid != null)
                            if ((tkdFutTum.UpdYmd + tkdFutTum.UpdTime) != request.depositPaymentHaitaCheck.tkdFutTumUpdYmdTime)
                                return "BI_T011";
                        tkdFutTum.HenKai += (short)1;
                        tkdFutTum.NyuKinKbn = tkdMishum.NyuKinRui == (decimal)0 ? (byte)1 : ((int)tkdMishum.NyuKinRui == int.Parse(request.depositCouponPayment.BillAmount, NumberStyles.Currency)) ? (byte)2
                            : ((int)tkdMishum.NyuKinRui < int.Parse(request.depositCouponPayment.BillAmount, NumberStyles.Currency)) ? (byte)3
                            : ((int)tkdMishum.NyuKinRui > int.Parse(request.depositCouponPayment.BillAmount, NumberStyles.Currency)) ? (byte)4 : (byte)0;
                        tkdFutTum.NcouKbn = tkdMishum.CouKesRui == 0 ? (byte)1 : (tkdMishum.CouKesRui >= request.depositCouponGrid.SeiKin) ? (byte)2
                            : (tkdMishum.CouKesRui < int.Parse(request.depositCouponPayment.BillAmount, NumberStyles.Currency)) ? (byte)3 : (byte)0;
                        tkdFutTum.UpdYmd = DateTime.Now.ToString(Commons.Constants.Formats.yyyyMMdd);
                        tkdFutTum.UpdTime = DateTime.Now.ToString(Commons.Constants.Formats.HHmmss);
                        tkdFutTum.UpdPrgId = "KK2300P";
                        tkdFutTum.UpdSyainCd = claimModel.SyainCdSeq;
                        _context.Update(tkdFutTum);
                    }
                }
                return null;
            }

            public async Task<string> DeletePayment(SavePaymentAsyncCommand request)
            {
                var claimModel = new HassyaAllrightCloud.Domain.Dto.ClaimModel();
                if (request.depositCouponPayment.DepositMethod != request.couponCode)
                {
                    var tkdNyuSih = _context.TkdNyuSih.Where(x => x.NyuSihTblSeq == request.depositPaymentGrid.NyuSihTblSeq 
                    && x.TenantCdSeq == claimModel.TenantID).FirstOrDefault();
                    if (tkdNyuSih != null)
                    {
                        tkdNyuSih.HenKai += 1;
                        tkdNyuSih.SiyoKbn = 2;
                        tkdNyuSih.UpdYmd = DateTime.Now.ToString(Commons.Constants.Formats.yyyyMMdd);
                        tkdNyuSih.UpdTime = DateTime.Now.ToString(Commons.Constants.Formats.HHmmss);
                        tkdNyuSih.UpdPrgId = "KK2300P";
                        tkdNyuSih.UpdSyainCd = claimModel.SyainCdSeq;
                        _context.Update(tkdNyuSih);
                    }
                    var tkdNyShmi = _context.TkdNyShmi.Where(x => x.UkeNo == request.depositPaymentGrid.UkeNo 
                    && x.NyuSihRen == request.depositPaymentGrid.NyuSihRen && x.TenantCdSeq == claimModel.TenantID).FirstOrDefault();
                    if (tkdNyShmi != null)
                    {
                        tkdNyShmi.HenKai += 1;
                        tkdNyShmi.SiyoKbn = 2;
                        tkdNyShmi.UpdYmd = DateTime.Now.ToString(Commons.Constants.Formats.yyyyMMdd);
                        tkdNyShmi.UpdTime = DateTime.Now.ToString(Commons.Constants.Formats.HHmmss);
                        tkdNyShmi.UpdPrgId = "KK2300P";
                        tkdNyShmi.UpdSyainCd = claimModel.SyainCdSeq;
                        _context.Update(tkdNyShmi);
                    }
                }
                else
                {
                    var tkdCoupon = _context.TkdCoupon.Where(x => x.CouTblSeq == request.depositPaymentGrid.CouTblSeq && x.TenantCdSeq == claimModel.TenantID).FirstOrDefault();
                    if (tkdCoupon != null)
                    {
                        tkdCoupon.HenKai += 1;
                        tkdCoupon.CouGkin = request.depositCouponPayment.IsChangeOrDeleteCoupon ? 0 : tkdCoupon.CouGkin;
                        tkdCoupon.CouZanFlg = request.depositCouponPayment.IsChangeOrDeleteCoupon ? tkdCoupon.CouZanFlg : (byte)1;
                        tkdCoupon.CouZan = request.depositCouponPayment.IsChangeOrDeleteCoupon ? 0 : tkdCoupon.CouZan;
                        tkdCoupon.SiyoKbn = 2;
                        tkdCoupon.UpdYmd = DateTime.Now.ToString(Commons.Constants.Formats.yyyyMMdd);
                        tkdCoupon.UpdTime = DateTime.Now.ToString(Commons.Constants.Formats.HHmmss);
                        tkdCoupon.UpdPrgId = "KK2300P";
                        tkdCoupon.UpdSyainCd = claimModel.SyainCdSeq;
                        _context.Update(tkdCoupon);
                    }
                    var tkdNyShCu = _context.TkdNyShCu.Where(x => x.UkeNo == request.depositPaymentGrid.UkeNo 
                    && x.NyuSihCouRen == request.depositPaymentGrid.NyuSihCouRen && x.TenantCdSeq == claimModel.TenantID).FirstOrDefault();
                    if (tkdNyShCu != null)
                    {
                        tkdNyShCu.HenKai += 1;
                        tkdNyShCu.SiyoKbn = 2;
                        tkdNyShCu.UpdYmd = DateTime.Now.ToString(Commons.Constants.Formats.yyyyMMdd);
                        tkdNyShCu.UpdTime = DateTime.Now.ToString(Commons.Constants.Formats.HHmmss);
                        tkdNyShCu.UpdPrgId = "KK2300P";
                        tkdNyShCu.UpdSyainCd = claimModel.SyainCdSeq;
                        _context.Update(tkdNyShCu);
                    }
                }
                return null;
            }
        }
    }
}
