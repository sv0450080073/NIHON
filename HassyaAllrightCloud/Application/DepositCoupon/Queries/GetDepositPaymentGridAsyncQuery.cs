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
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BillPrint.Queries
{
    public class GetDepositPaymentGridAsyncQuery : IRequest<List<DepositPaymentGrid>>
    {
        public DepositPaymentFilter depositPaymentFilter { get; set; }
        public class Handler : IRequestHandler<GetDepositPaymentGridAsyncQuery, List<DepositPaymentGrid>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<DepositPaymentGrid>> Handle(GetDepositPaymentGridAsyncQuery request, CancellationToken cancellationToken = default)
            {
                List<DepositPaymentGrid> rows = new List<DepositPaymentGrid>();

                _context.LoadStoredProc("PK_dDepositPaymentGrid_R")
                                .AddParam("@TenantCdSeq", new ClaimModel().TenantID)
                                .AddParam("@UkeNo", request.depositPaymentFilter.UkeNo)
                                .AddParam("@FutuUnkRen", request.depositPaymentFilter.FutuUnkRen)
                                .AddParam("@SeiFutSyu", request.depositPaymentFilter.SeiFutSyu)
                                .AddParam("@FutTumRen", request.depositPaymentFilter.FutTumRen)
                                .AddParam("@Offset", request.depositPaymentFilter.Offset)
                                .AddParam("@Limit", request.depositPaymentFilter.Limit)
                                .AddParam("@ROWCOUNT", out IOutParam<int> rowCount)
                                .Exec(r =>
                                {
                                    while (r.Read())
                                    {
                                        DepositPaymentGrid depositPaymentGrid = new DepositPaymentGrid();
                                        depositPaymentGrid.TotalAllAdjustment = (long)r["TotalAllAdjustment"];
                                        depositPaymentGrid.TotalAllCard = (long)r["TotalAllCard"];
                                        depositPaymentGrid.TotalAllCash = (long)r["TotalAllCash"];
                                        depositPaymentGrid.TotalAllCommercialPaper = (long)r["TotalAllCommercialPaper"];
                                        depositPaymentGrid.TotalAllCouponAppliedAmount = (long)r["TotalAllCouponAppliedAmount"];
                                        depositPaymentGrid.TotalAllOffset = (long)r["TotalAllOffset"];
                                        depositPaymentGrid.TotalAllOther1 = (long)r["TotalAllOther1"];
                                        depositPaymentGrid.TotalAllOther2 = (long)r["TotalAllOther2"];
                                        depositPaymentGrid.TotalAllTotalDeposit = (long)r["TotalAllTotalDeposit"];
                                        depositPaymentGrid.TotalAllTransfer = (long)r["TotalAllTransfer"];
                                        depositPaymentGrid.TotalAllTransferFee = (long)r["TotalAllTransferFee"];
                                        depositPaymentGrid.TotalAllTransferSupport = (long)r["TotalAllTransferSupport"];
                                        depositPaymentGrid.BankNm = (string)r["BankNm"];
                                        depositPaymentGrid.BankStNm = (string)r["BankStNm"];
                                        depositPaymentGrid.CouGkin = (int)r["CouGkin"];
                                        depositPaymentGrid.CouKesG = (int)r["CouKesG"];
                                        depositPaymentGrid.CouNo = (string)r["CouNo"];
                                        depositPaymentGrid.CountNumber = (int)r["CountNumber"];
                                        depositPaymentGrid.CouTblSeq = (int)r["CouTblSeq"];
                                        depositPaymentGrid.COU_NyuSihRen = (int)r["COU_NyuSihRen"];
                                        depositPaymentGrid.COU_UpdTime = (string)r["COU_UpdTime"];
                                        depositPaymentGrid.COU_UpdYmd = (string)r["COU_UpdYmd"];
                                        depositPaymentGrid.FurKesG = (int)r["FurKesG"];
                                        depositPaymentGrid.FutTumRen = (short)r["FutTumRen"];
                                        depositPaymentGrid.KesG = (int)r["KesG"];
                                        depositPaymentGrid.KyoKesG = (int)r["KyoKesG"];
                                        depositPaymentGrid.NSC_UpdTime = (string)r["NSC_UpdTime"];
                                        depositPaymentGrid.NSC_UpdYmd = (string)r["NSC_UpdYmd"];
                                        depositPaymentGrid.NS_BankCd = (string)r["NS_BankCd"];
                                        depositPaymentGrid.NS_BankSitCd = (string)r["NS_BankSitCd"];
                                        depositPaymentGrid.NS_CardDen = (string)r["NS_CardDen"];
                                        depositPaymentGrid.NS_CardSyo = (string)r["NS_CardSyo"];
                                        depositPaymentGrid.NS_EtcSyo1 = (string)r["NS_EtcSyo1"];
                                        depositPaymentGrid.NS_EtcSyo2 = (string)r["NS_EtcSyo2"];
                                        depositPaymentGrid.NS_FuriTes = (int)r["NS_FuriTes"];
                                        depositPaymentGrid.NS_KyoRyoKin = (int)r["NS_KyoRyoKin"];
                                        depositPaymentGrid.NS_NyuSihG = (int)r["NS_NyuSihG"];
                                        depositPaymentGrid.NS_NyuSihKbn = (int)r["NS_NyuSihKbn"];
                                        depositPaymentGrid.NS_NyuSihSyu = (int)r["NS_NyuSihSyu"];
                                        depositPaymentGrid.NS_NyuSihTblSeq = (int)r["NS_NyuSihTblSeq"];
                                        depositPaymentGrid.NS_TegataNo = (string)r["NS_TegataNo"];
                                        depositPaymentGrid.NS_TegataYmd = (string)r["NS_TegataYmd"];
                                        depositPaymentGrid.NS_UpdTime = (string)r["NS_UpdTime"];
                                        depositPaymentGrid.NS_UpdYmd = (string)r["NS_UpdYmd"];
                                        depositPaymentGrid.NS_YokinSyu = (int)r["NS_YokinSyu"];
                                        depositPaymentGrid.NyuKesiKbn = (byte)r["NyuKesiKbn"];
                                        depositPaymentGrid.NyuSihCouRen = (short)r["NyuSihCouRen"];
                                        depositPaymentGrid.NyuSihEigCd = (int)r["NyuSihEigCd"];
                                        depositPaymentGrid.NyuSihEigNm = (string)r["NyuSihEigNm"];
                                        depositPaymentGrid.NyuSihEigSeq = (int)r["NyuSihEigSeq"];
                                        depositPaymentGrid.NyuSihHakoYmd = (string)r["NyuSihHakoYmd"];
                                        depositPaymentGrid.NyuSihKbn = (byte)r["NyuSihKbn"];
                                        depositPaymentGrid.NyuSihRen = (int)r["NyuSihRen"];
                                        depositPaymentGrid.NyuSihTblSeq = (int)r["NyuSihTblSeq"];
                                        depositPaymentGrid.SeiFutSyu = (byte)r["SeiFutSyu"];
                                        depositPaymentGrid.SiyoKbn = (byte)r["SiyoKbn"];
                                        depositPaymentGrid.UkeNo = (string)r["UkeNo"];
                                        depositPaymentGrid.UnkRen = (short)r["UnkRen"];
                                        depositPaymentGrid.UpdTime = (string)r["UpdTime"];
                                        depositPaymentGrid.UpdYmd = (string)r["UpdYmd"];
                                        depositPaymentGrid.YokinSyuNm = (string)r["YokinSyuNm"];
                                        depositPaymentGrid.YouTblSeq = (int)r["YouTblSeq"];
                                        rows.Add(depositPaymentGrid);
                                    }
                                    r.Close();
                                });
                return rows;
            }
        }
    }
}
