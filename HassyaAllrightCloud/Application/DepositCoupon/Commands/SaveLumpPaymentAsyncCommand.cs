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
    public class SaveLumpPaymentAsyncCommand : IRequest<string>
    {
        public List<DepositCouponGrid> gridCheckDatas { get; set; } = new List<DepositCouponGrid>();
        public DepositCouponPayment depositCouponPayment { get; set; } = new DepositCouponPayment()
        {
            OffsetPaymentTables = new List<OffsetPaymentTable>() { new OffsetPaymentTable() }
        };
        public List<DepositPaymentHaitaCheck> depositPaymentHaitaChecks { get; set; }
        public readonly string couponCode = "07";
        public class Handler : IRequestHandler<SaveLumpPaymentAsyncCommand, string>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<SaveLumpPaymentAsyncCommand> _logger;

            public Handler(KobodbContext context, ILogger<SaveLumpPaymentAsyncCommand> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<string> Handle(SaveLumpPaymentAsyncCommand request, CancellationToken cancellationToken = default)
            {
                string errorMessage = null;
                if (!request.gridCheckDatas.Any() || request.depositCouponPayment == null)
                    return ErrorMessage.SAVEFAIL;
                using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbTran = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (request.depositCouponPayment.DepositMethod != request.couponCode)
                        {
                            errorMessage = await CreatePaymentNotCoupon(request);
                        }
                        else
                        {
                            errorMessage = await CreatePaymentCoupon(request);
                        }

                        if (errorMessage != null)
                            return errorMessage;

                        await _context.SaveChangesAsync();
                        await dbTran.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        //Rollback transaction if exception occurs  
                        dbTran.Rollback();
                        return ErrorMessage.SAVEFAIL;
                    }
                    return null;
                }
            }

            public async Task<string> CreatePaymentNotCoupon(SaveLumpPaymentAsyncCommand request)
            {
                var tkdNyShmis = new List<TkdNyShmi>();
                var tkdMishums = new List<TkdMishum>();
                var tkdFutTums = new List<TkdFutTum>();
                var tkdYykshoes = new List<TkdYyksho>();
                var claimModel = new HassyaAllrightCloud.Domain.Dto.ClaimModel();
                var tkdNyuSihData = _context.TkdNyuSih.Where(x => x.TenantCdSeq == claimModel.TenantID)
                .OrderByDescending(x => x.NyuSihTblSeq).FirstOrDefault();
                var count = 0;
                foreach (var item in request.gridCheckDatas)
                {
                    count += 1;
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
                        NyuSihG = request.depositCouponPayment.DepositMethod == "02" ? ((int)item.SeiKin - request.depositCouponPayment.TransferFee 
                        - request.depositCouponPayment.SponsorshipFund) : (int)item.SeiKin,
                        FuriTes = request.depositCouponPayment.DepositMethod == "02" ? request.depositCouponPayment.TransferFee : 0,
                        KyoRyoKin = request.depositCouponPayment.DepositMethod == "02" ? request.depositCouponPayment.SponsorshipFund : 0,
                        BankCd = request.depositCouponPayment.DepositMethod == "02" ? request.depositCouponPayment.DepositTransferBank.BankCd : string.Empty,
                        BankSitCd = request.depositCouponPayment.DepositMethod == "02" ? request.depositCouponPayment.DepositTransferBank.BankSitCd : string.Empty,
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
                        NyuSihTblSeq = tkdNyuSihData == null ? 1 : (tkdNyuSihData.NyuSihTblSeq + count),
                        TenantCdSeq = claimModel.TenantID
                    };
                    _context.Add(tkdNyuSih);
                    _context.SaveChanges();

                    var tkdNyShmiData = _context.TkdNyShmi.Where(x => x.UkeNo == item.UkeNo && x.TenantCdSeq == claimModel.TenantID)
                    .OrderByDescending(x => x.NyuSihRen).FirstOrDefault();
                    short nyuSihRen = (short)(tkdNyShmiData == null ? (short)1 : (tkdNyShmiData.NyuSihRen + (short)count));
                    var tkdNyShmi = new TkdNyShmi()
                    {
                        UkeNo = item.UkeNo,
                        NyuSihRen = nyuSihRen,
                        HenKai = 0,
                        NyuSihKbn = 1,
                        SeiFutSyu = item.SeiFutSyu,
                        UnkRen = item.FutuUnkRen,
                        YouTblSeq = 0,
                        KesG = request.depositCouponPayment.DepositMethod == "02" ? ((int)item.SeiKin -
                        request.depositCouponPayment.TransferFee - request.depositCouponPayment.SponsorshipFund)
                        : (int)item.SeiKin,
                        FurKesG = request.depositCouponPayment.DepositMethod == "02" ? request.depositCouponPayment.TransferFee : 0,
                        KyoKesG = request.depositCouponPayment.DepositMethod == "02" ? request.depositCouponPayment.SponsorshipFund : 0,
                        FutTumRen = item.FutTumRen,
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
                    tkdNyShmis.Add(tkdNyShmi);

                    var tkdMishum = _context.TkdMishum.Where(x => x.UkeNo == item.UkeNo && x.MisyuRen == item.MisyuRen).FirstOrDefault();
                    if (tkdMishum == null)
                        return ErrorMessage.SAVEFAIL;
                    //haita check
                    var tkdMishumUpdYmdTime = request.depositPaymentHaitaChecks.Where(x => x.UkeNo == item.UkeNo 
                    && x.MisyuRen == item.MisyuRen).FirstOrDefault()?.tkdMishumUpdYmdTime;
                    if ((tkdMishum.UpdYmd + tkdMishum.UpdTime) != tkdMishumUpdYmdTime)
                        return "BI_T011";

                    tkdMishum.HenKai += 1;
                    tkdMishum.NyuKinRui = (decimal)item.SeiKin;
                    //tkdMishum.CouKesRui = 0;
                    tkdMishum.UpdYmd = DateTime.Now.ToString(Commons.Constants.Formats.yyyyMMdd);
                    tkdMishum.UpdTime = DateTime.Now.ToString(Commons.Constants.Formats.HHmmss);
                    tkdMishum.UpdPrgId = "KK2300P";
                    tkdMishum.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    tkdMishums.Add(tkdMishum);

                    if (item.SeiFutSyu == 1 || item.SeiFutSyu == 7)
                    {
                        var tkdYyksho = _context.TkdYyksho.Where(x => x.UkeNo == item.UkeNo && x.TenantCdSeq == claimModel.TenantID).FirstOrDefault();
                        if (tkdYyksho != null) {
                            //haita check
                            var tkdYykshoUpdYmdTime = request.depositPaymentHaitaChecks.Where(x => x.UkeNo == item.UkeNo 
                            && x.TenantCdSeq == claimModel.TenantID).FirstOrDefault()?.tkdYykshoUpdYmdTime;
                            if ((tkdYyksho.UpdYmd + tkdYyksho.UpdTime) != tkdYykshoUpdYmdTime)
                                return "BI_T011";
                            tkdYyksho.HenKai += (short)1;
                            tkdYyksho.NyuKinKbn = request.depositCouponPayment.DepositMethod == "07" ? tkdYyksho.NyuKinKbn : (byte)2;
                            tkdYyksho.NcouKbn = tkdMishum.CouKesRui == 0 ? (byte)1 : (tkdMishum.CouKesRui >= item.SeiKin) ? (byte)2
                                : (tkdMishum.CouKesRui < item.SeiKin) ? (byte)3 : (byte)0;
                            tkdYyksho.UpdYmd = DateTime.Now.ToString(Commons.Constants.Formats.yyyyMMdd);
                            tkdYyksho.UpdTime = DateTime.Now.ToString(Commons.Constants.Formats.HHmmss);
                            tkdYyksho.UpdPrgId = "KK2300P";
                            tkdYyksho.UpdSyainCd = claimModel.SyainCdSeq;
                            tkdYykshoes.Add(tkdYyksho);
                        }
                    }
                    else
                    {
                        var tkdFutTum = _context.TkdFutTum.Where(x => x.UkeNo == item.UkeNo && x.UnkRen == item.FutuUnkRen
                        && x.FutTumRen == item.FutTumRen && x.FutTumKbn == (item.SeiFutSyu == (byte)6 ? (byte)2 : (byte)1)).FirstOrDefault();
                        if (tkdFutTum != null) {
                            //haita check
                            var tkdFutTumUpdYmdTime = request.depositPaymentHaitaChecks.Where(x => x.UkeNo == item.UkeNo && x.UnkRen == item.FutuUnkRen
                            && x.FutTumRen == item.FutTumRen && x.FutTumKbn == (item.SeiFutSyu == (byte)6 ? (byte)2 : (byte)1)).FirstOrDefault()?.tkdFutTumUpdYmdTime;
                            if ((tkdFutTum.UpdYmd + tkdFutTum.UpdTime) != tkdFutTumUpdYmdTime)
                                return "BI_T011";
                            tkdFutTum.HenKai += (short)1;
                            tkdFutTum.NyuKinKbn = request.depositCouponPayment.DepositMethod == "07" ? tkdFutTum.NyuKinKbn : (byte)2;
                            tkdFutTum.NcouKbn = tkdMishum.CouKesRui == 0 ? (byte)1 : (tkdMishum.CouKesRui >= item.SeiKin) ? (byte)2
                                : (tkdMishum.CouKesRui < item.SeiKin) ? (byte)3 : (byte)0;
                            tkdFutTum.UpdYmd = DateTime.Now.ToString(Commons.Constants.Formats.yyyyMMdd);
                            tkdFutTum.UpdTime = DateTime.Now.ToString(Commons.Constants.Formats.HHmmss);
                            tkdFutTum.UpdPrgId = "KK2300P";
                            tkdFutTum.UpdSyainCd = claimModel.SyainCdSeq;
                            tkdFutTums.Add(tkdFutTum);
                        }
                    }
                }
                _context.UpdateRange(tkdMishums);
                _context.UpdateRange(tkdFutTums);
                _context.UpdateRange(tkdYykshoes);
                await _context.AddRangeAsync(tkdNyShmis);
                return null;
            }

            public async Task<string> CreatePaymentCoupon(SaveLumpPaymentAsyncCommand request)
            {
                var tkdMishums = new List<TkdMishum>();
                var tkdFutTums = new List<TkdFutTum>();
                var tkdYykshoes = new List<TkdYyksho>();
                var customCouponTable = CreateCouponTableCustom(request);
                var claimModel = new HassyaAllrightCloud.Domain.Dto.ClaimModel();
                foreach (var item in request.gridCheckDatas.OrderByDescending(x => x.SeiKin).Select((value, i) => new { i, value }))
                {
                    var filterCustomCouponTable = customCouponTable.Where(x => x.IndexPayment == item.i).ToList();
                    var count = 1;

                    var tkdMishum = _context.TkdMishum.Where(x => x.UkeNo == item.value.UkeNo && x.MisyuRen == item.value.MisyuRen).FirstOrDefault();
                    if (tkdMishum == null)
                        return ErrorMessage.SAVEFAIL;
                    //haita check
                    var tkdMishumUpdYmdTime = request.depositPaymentHaitaChecks.Where(x => x.UkeNo == item.value.UkeNo 
                    && x.MisyuRen == item.value.MisyuRen).FirstOrDefault()?.tkdMishumUpdYmdTime;
                    if ((tkdMishum.UpdYmd + tkdMishum.UpdTime) != tkdMishumUpdYmdTime)
                        return "BI_T011";

                    tkdMishum.HenKai += 1;
                    //tkdMishum.NyuKinRui = (decimal)0;
                    tkdMishum.CouKesRui = filterCustomCouponTable.Sum(x => x.ApplicationAmount.GetValueOrDefault());
                    tkdMishum.UpdYmd = DateTime.Now.ToString(Commons.Constants.Formats.yyyyMMdd);
                    tkdMishum.UpdTime = DateTime.Now.ToString(Commons.Constants.Formats.HHmmss);
                    tkdMishum.UpdPrgId = "KK2300P";
                    tkdMishum.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    tkdMishums.Add(tkdMishum);

                    if (item.value.SeiFutSyu == 1 || item.value.SeiFutSyu == 7)
                    {
                        var tkdYyksho = _context.TkdYyksho.Where(x => x.UkeNo == item.value.UkeNo && x.TenantCdSeq == claimModel.TenantID).FirstOrDefault();
                        if (tkdYyksho != null) {
                            //haita check
                            var tkdYykshoUpdYmdTime = request.depositPaymentHaitaChecks.Where(x => x.UkeNo == item.value.UkeNo 
                            && x.TenantCdSeq == claimModel.TenantID).FirstOrDefault()?.tkdYykshoUpdYmdTime;
                            if ((tkdYyksho.UpdYmd + tkdYyksho.UpdTime) != tkdYykshoUpdYmdTime)
                                return "BI_T011";

                            tkdYyksho.HenKai += (short)1;
                            tkdYyksho.NyuKinKbn = request.depositCouponPayment.DepositMethod == "07" ? tkdYyksho.NyuKinKbn : (byte)2;
                            tkdYyksho.NcouKbn = tkdMishum.CouKesRui == 0 ? (byte)1 : (tkdMishum.CouKesRui >= item.value.SeiKin) ? (byte)2
                                : (tkdMishum.CouKesRui < item.value.SeiKin) ? (byte)3 : (byte)0;
                            tkdYyksho.UpdYmd = DateTime.Now.ToString(Commons.Constants.Formats.yyyyMMdd);
                            tkdYyksho.UpdTime = DateTime.Now.ToString(Commons.Constants.Formats.HHmmss);
                            tkdYyksho.UpdPrgId = "KK2300P";
                            tkdYyksho.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                            tkdYykshoes.Add(tkdYyksho);
                        }
                    }
                    else
                    {
                        var tkdFutTum = _context.TkdFutTum.Where(x => x.UkeNo == item.value.UkeNo && x.UnkRen == item.value.FutuUnkRen
                        && x.FutTumRen == item.value.FutTumRen && x.FutTumKbn == (item.value.SeiFutSyu == (byte)6 ? (byte)2 : (byte)1)).FirstOrDefault();
                        if (tkdFutTum != null) {
                            //haita check
                            var tkdFutTumUpdYmdTime = request.depositPaymentHaitaChecks.Where(x => x.UkeNo == item.value.UkeNo && x.UnkRen == item.value.FutuUnkRen
                            && x.FutTumRen == item.value.FutTumRen && x.FutTumKbn == (item.value.SeiFutSyu == (byte)6 ? (byte)2 : (byte)1)).FirstOrDefault()?.tkdFutTumUpdYmdTime;
                            if ((tkdFutTum.UpdYmd + tkdFutTum.UpdTime) != tkdFutTumUpdYmdTime)
                                return "BI_T011";

                            tkdFutTum.HenKai += (short)1;
                            tkdFutTum.NyuKinKbn = request.depositCouponPayment.DepositMethod == "07" ? tkdFutTum.NyuKinKbn : (byte)2;
                            tkdFutTum.NcouKbn = tkdMishum.CouKesRui == 0 ? (byte)1 : (tkdMishum.CouKesRui >= item.value.SeiKin) ? (byte)2
                                : (tkdMishum.CouKesRui < item.value.SeiKin) ? (byte)3 : (byte)0;
                            tkdFutTum.UpdYmd = DateTime.Now.ToString(Commons.Constants.Formats.yyyyMMdd);
                            tkdFutTum.UpdTime = DateTime.Now.ToString(Commons.Constants.Formats.HHmmss);
                            tkdFutTum.UpdPrgId = "KK2300P";
                            tkdFutTum.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                            tkdFutTums.Add(tkdFutTum);
                        }
                    }

                    var tkdNyShCuData = _context.TkdNyShCu.Where(x => x.UkeNo == item.value.UkeNo && x.TenantCdSeq == claimModel.TenantID)
                    .OrderByDescending(x => x.NyuSihCouRen).FirstOrDefault();
                    var nyuSihCouRen = tkdNyShCuData == null ? (short)0 : tkdNyShCuData.NyuSihCouRen;
                    var tkdCouponData = _context.TkdCoupon.Where(x => x.TenantCdSeq == claimModel.TenantID).OrderByDescending(x => x.CouTblSeq).FirstOrDefault();
                    var couTblSeq = tkdCouponData == null ? 0 : tkdCouponData.CouTblSeq;

                    foreach (var offsetPaymentTable in filterCustomCouponTable)
                    {
                        var tkdCoupon = new TkdCoupon()
                        {
                            HenKai = 0,
                            NyuSihKbn = 1,
                            SeiSihCdSeq = item.value.SeiCd,
                            SeiSihSitCdSeq = item.value.SeiSitenCd,
                            HakoYmd = offsetPaymentTable.DateOfIssue?.ToString(Commons.Constants.Formats.yyyyMMdd),
                            NyuSihEigSeq = int.Parse(request.depositCouponPayment.DepositOffice.Code),
                            CouNo = offsetPaymentTable.CouponNo,
                            CouGkin = (int)offsetPaymentTable.FaceValue,
                            CouZanFlg = offsetPaymentTable.FaceValue - offsetPaymentTable.ApplicationAmount.GetValueOrDefault() > (byte)0 ? (byte)0 : (byte)1,
                            CouZan = (int)offsetPaymentTable.FaceValue - offsetPaymentTable.ApplicationAmount.GetValueOrDefault(),
                            SiyoKbn = 1,
                            UpdYmd = DateTime.Now.ToString(Commons.Constants.Formats.yyyyMMdd),
                            UpdTime = DateTime.Now.ToString(Commons.Constants.Formats.HHmmss),
                            UpdPrgId = "KK2300P",
                            UpdSyainCd = claimModel.SyainCdSeq,
                            CouTblSeq = couTblSeq + count,
                            TenantCdSeq = claimModel.TenantID
                        };
                        _context.Add(tkdCoupon);
                        _context.SaveChanges();

                        var tkdNyShCu = new TkdNyShCu()
                        {
                            UkeNo = item.value.UkeNo,
                            NyuSihCouRen = (short)(nyuSihCouRen + (short)count),
                            HenKai = 0,
                            NyuSihKbn = 1,
                            SeiFutSyu = item.value.SeiFutSyu,
                            NyuKesiKbn = 1,
                            UnkRen = item.value.FutuUnkRen,
                            YouTblSeq = 0,
                            CouKesG = offsetPaymentTable.ApplicationAmount.GetValueOrDefault(),
                            FutTumRen = item.value.FutTumRen,
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
                _context.UpdateRange(tkdMishums);
                _context.UpdateRange(tkdFutTums);
                _context.UpdateRange(tkdYykshoes);
                return null;
            }

            public List<OffsetPaymentTable> CreateCouponTableCustom(SaveLumpPaymentAsyncCommand request)
            {
                bool isStop = false;
                var offsetTableCustom = new List<OffsetPaymentTable>();
                foreach(var gridCheckData in request.gridCheckDatas.OrderByDescending(x => x.SeiKin).Select((value, i) => new { i, value }))
                {
                    var remainCoupon = gridCheckData.value.SeiKin - gridCheckData.value.NyuKinRui - gridCheckData.value.CouKesRui;
                    foreach (var item in request.depositCouponPayment.OffsetPaymentTables.ToList())
                    {
                        remainCoupon = remainCoupon - item.ApplicationAmount.GetValueOrDefault();

                        if(remainCoupon >= 0)
                        {
                            offsetTableCustom.Add(new OffsetPaymentTable() {
                                ApplicationAmount = item.ApplicationAmount,
                                CouponNo = item.CouponNo,
                                DateOfIssue = item.DateOfIssue,
                                FaceValue = item.FaceValue,
                                IndexPayment = gridCheckData.i
                            });
                            request.depositCouponPayment.OffsetPaymentTables.Remove(item);
                        } else
                        {
                            var offsetTable = new OffsetPaymentTable()
                            {
                                ApplicationAmount = (int)Math.Abs(remainCoupon),
                                CouponNo = item.CouponNo,
                                DateOfIssue = item.DateOfIssue,
                                FaceValue = item.FaceValue,
                                IndexPayment = gridCheckData.i
                            };
                            var offsetTable2 = new OffsetPaymentTable()
                            {
                                ApplicationAmount = (int)(item.FaceValue + remainCoupon),
                                CouponNo = item.CouponNo,
                                DateOfIssue = item.DateOfIssue,
                                FaceValue = item.FaceValue,
                                IndexPayment = gridCheckData.i
                            };
                            offsetTableCustom.Add(offsetTable2);
                            isStop = gridCheckData.i == (request.gridCheckDatas.Count() - 1);
                            if (isStop)
                            {
                                break;
                            }
                            request.depositCouponPayment.OffsetPaymentTables.Remove(item);
                            request.depositCouponPayment.OffsetPaymentTables.Insert(0, offsetTable);
                            break;
                        }
                    }
                }
                return offsetTableCustom;
            }
        }
    }
}
