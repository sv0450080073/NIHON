using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using StoredProcedureEFCore;

namespace HassyaAllrightCloud.Application.DepositList.Queries
{
    public class GetDepositListCSVData : IRequest<List<DepositListCSVDataModel>>
    {
        public int TenantCdSeq { get; set; }
        public int SeiKrksKbn { get; set; }
        public string SyoriYm { get; set; }
        public DepositListSearchModel depositListSearchModel { get; set; }
        public class Handler : IRequestHandler<GetDepositListCSVData, List<DepositListCSVDataModel>>
        {
            private readonly KobodbContext _dbcontext;
            public Handler(KobodbContext context)
            {
                _dbcontext = context;
            }
            public async Task<List<DepositListCSVDataModel>> Handle(GetDepositListCSVData request, CancellationToken cancellationToken)
            {
                var depositDataFromDb = new List<DepositListModel>();
                var result = new List<DepositListCSVDataModel>();
                int no = 1;

                _dbcontext.LoadStoredProc("PK_dSelectDepositListPaging_R")
                          .AddParam("TenantCdSeq", request.TenantCdSeq)

                          .AddParam("@SalesOfficeKbn", request.depositListSearchModel.SaleOfficeType.SaleOfficeName)
                          .AddParam("@StartPaymentDate", request.depositListSearchModel.StartPaymentDate != null ? request.depositListSearchModel.StartPaymentDate.ToString().Substring(0, 10).Replace("/", string.Empty) : string.Empty)
                          .AddParam("@EndPaymentDate", request.depositListSearchModel.EndPaymentDate != null ? request.depositListSearchModel.EndPaymentDate.ToString().Substring(0, 10).Replace(" / ", string.Empty) : string.Empty)
                          .AddParam("@CompanyCdSeq", request.depositListSearchModel.CompanyData != null ? request.depositListSearchModel.CompanyData.CompanyCdSeq : -1)
                          .AddParam("@StartReceptionNumber", request.depositListSearchModel.StartReceiptNumber != null ? Int32.Parse(request.depositListSearchModel.StartReceiptNumber) : 0)
                          .AddParam("@EndReceptionNumber", request.depositListSearchModel.EndReceiptNumber != null ? Int32.Parse(request.depositListSearchModel.EndReceiptNumber) : 0)
                          .AddParam("@StartReservationCategory", request.depositListSearchModel.StartReservationClassification != null ? request.depositListSearchModel.StartReservationClassification.YoyaKbn : 0)
                          .AddParam("@EndReservationCategory", request.depositListSearchModel.EndReservationClassification != null ? request.depositListSearchModel.EndReservationClassification.YoyaKbn : 0)
                          .AddParam("@BillingTypeSelection", request.depositListSearchModel.BillingType != null ? request.depositListSearchModel.BillingType : string.Empty)
                          .AddParam("@StartTransferBank", request.depositListSearchModel.StartTransferBank != null ? request.depositListSearchModel.StartTransferBank.Code : string.Empty)
                          .AddParam("@EndTransferBank", request.depositListSearchModel.EndTransferBank != null ? request.depositListSearchModel.EndTransferBank.Code : string.Empty)
                          .AddParam("@PaymentMethod", request.depositListSearchModel.PaymentMethod != null ? request.depositListSearchModel.PaymentMethod : string.Empty)
                          .AddParam("@SalesOfficeSelection", request.depositListSearchModel.SelectedSaleBranchPayment != null ? request.depositListSearchModel.SelectedSaleBranchPayment.CEigyoCdSeq : 0)

                          .AddParam("@CSeiGyosyaCd", request.depositListSearchModel.SelectedBillingAddressPayment != null ? request.depositListSearchModel.SelectedBillingAddressPayment.CSeiGyosyaCd : "0")
                          .AddParam("@CSeiCd", request.depositListSearchModel.SelectedBillingAddressPayment != null ? request.depositListSearchModel.SelectedBillingAddressPayment.CSeiCd : "0")
                          .AddParam("@CSeiSitenCd", request.depositListSearchModel.SelectedBillingAddressPayment != null ? request.depositListSearchModel.SelectedBillingAddressPayment.CSeiSitenCd : "0")
                          .AddParam("@CSeiCdSeq", request.depositListSearchModel.SelectedBillingAddressPayment != null ? request.depositListSearchModel.SelectedBillingAddressPayment.CSeiCdSeq : "0")
                          .AddParam("@CSeiSiyoStaYmd", request.depositListSearchModel.SelectedBillingAddressPayment.CSeiSiyoStaYmd != null ? request.depositListSearchModel.SelectedBillingAddressPayment.CSeiSiyoStaYmd : string.Empty)
                          .AddParam("@CSeiSiyoEndYmd", request.depositListSearchModel.SelectedBillingAddressPayment.CSeiSiyoEndYmd != null ? request.depositListSearchModel.SelectedBillingAddressPayment.CSeiSiyoEndYmd : string.Empty)
                          .AddParam("@CSeiSitenCdSeq", request.depositListSearchModel.SelectedBillingAddressPayment.CSeiSitenCdSeq)
                          .AddParam("@CSitSiyoStaYmd", request.depositListSearchModel.SelectedBillingAddressPayment.CSitSiyoStaYmd != null ? request.depositListSearchModel.SelectedBillingAddressPayment.CSitSiyoStaYmd : string.Empty)
                          .AddParam("@CSitSiyoEndYmd", request.depositListSearchModel.SelectedBillingAddressPayment.CSitSiyoEndYmd != null ? request.depositListSearchModel.SelectedBillingAddressPayment.CSitSiyoEndYmd : string.Empty)

                          .AddParam("@Skip", 0)
                          .AddParam("@Take", 10000000)

                          .Exec(r => depositDataFromDb = r.ToList<DepositListModel>());
                foreach (var item in depositDataFromDb)
                {
                    item.No = no;
                    if (item.NyuSihSyuS == "91")
                    {
                        item.Orther11 = item.EtcSyo1;
                        item.Orther12 = item.EtcSyo2;
                    }
                    else
                    {
                        item.Orther12 = string.Empty;
                        item.Orther12 = string.Empty;
                    }
                    if (item.NyuSihSyuS == "92")
                    {
                        item.Orther21 = item.EtcSyo1;
                        item.Orther22 = item.EtcSyo2;
                    }
                    else
                    {
                        item.Orther21 = string.Empty;
                        item.Orther22 = string.Empty;
                    }
                    if (request.SeiKrksKbn == 1)
                    {
                        
                        if (item.SeikYm.CompareTo(item.NyuSihYmd.Substring(0, 6)) >= 0 || item.NyuSihYmd.CompareTo(item.SeiTaiYmd) >= 0)
                        {
                            item.Amount = item.KesG;
                            item.TransferFee = item.FurKesG;
                            item.CumulativePayment = item.KesG + item.FurKesG;
                            item.PreviousReceiveAmount = 0;
                        }
                        else
                        {
                            item.Amount = 0;
                            item.TransferFee = 0;
                            item.CumulativePayment = 0;
                            item.PreviousReceiveAmount = item.KesG + item.FurKesG;
                        }
                        item.Cash = item.NyuSihSyuS == "1" ? item.KesG : 0;
                        item.Another = item.NyuSihSyuS == "91" || item.NyuSihSyuS == "92" ? item.KesG : 0;
                        item.AdjustMoney = item.NyuSihSyuS == "6" ? item.KesG : 0;
                        item.Bill = item.NyuSihSyuS == "4" ? item.KesG : 0;
                        item.Compensation = item.NyuSihSyuS == "5" ? item.KesG : 0;
                        item.TransferAmount = item.NyuSihSyuS == "2" ? item.KesG : 0;
                    }
                    else if (request.SeiKrksKbn == 2)
                    {
                        if (request.SyoriYm.CompareTo(item.NyuSihYmd.Substring(0, 6)) >= 0 || item.NyuSihYmd.CompareTo(item.SeiTaiYmd) >= 0)
                        {
                            item.Amount = item.KesG;
                            item.TransferFee = item.FurKesG;
                            item.CumulativePayment = item.KesG + item.FurKesG;
                            item.PreviousReceiveAmount = 0;
                        }
                        else
                        {
                            item.Amount = 0;
                            item.TransferFee = 0;
                            item.CumulativePayment = 0;
                            item.PreviousReceiveAmount = item.KesG + item.FurKesG;
                        }
                        item.Cash = item.NyuSihSyuS == "1" ? item.KesG : 0;
                        item.Another = item.NyuSihSyuS == "91" || item.NyuSihSyuS == "92" ? item.KesG : 0;
                        item.AdjustMoney = item.NyuSihSyuS == "6" ? item.KesG : 0;
                        item.Bill = item.NyuSihSyuS == "4" ? item.KesG : 0;
                        item.Compensation = item.NyuSihSyuS == "5" ? item.KesG : 0;
                        item.TransferAmount = item.NyuSihSyuS == "2" ? item.KesG : 0;
                    }
                    else
                    {
                        item.Cash = 0;
                        item.Another = 0;
                        item.AdjustMoney = 0;
                        item.Bill = 0;
                        item.Compensation = 0;
                        item.TransferAmount = 0;
                    }
                    result.Add(new DepositListCSVDataModel()
                    {
                        EigyoCd = item.EigyoCd,
                        Cash = item.Cash,
                        NyuSihSyu = item.NyuSihSyuS,
                        TransferAmount = item.TransferAmount,
                        Compensation = item.Compensation,
                        Bill = item.Bill,
                        AdjustMoney = item.AdjustMoney,
                        Another = item.Another,
                        Amount = item.Amount,
                        BankCd = item.BankCd,
                        BankNm = item.BankNm,
                        BankRyak = item.BankRyak,
                        BankSitCd = item.BankSitCd,
                        BankSitNm = item.BankSitNm,
                        BankSitRyak = item.BankSitRyak,
                        CardDen = item.CardDen,
                        CardSyo = item.CardSyo,
                        CouGkin = item.CouGkin.ToString(),
                        CouNo = item.CouNo.ToString(),
                        CumulativeDeposit = item.CumulativePayment,
                        DanTaNm = item.DantaNm,
                        EigyoNm = item.EigyoNm,
                        EigyoRyak = item.EigyoRyak,
                        FutTumNm = item.FutTumNm,
                        GyosyaCd = item.GyosyaCd,
                        GyosyaNm = item.GyosyaNm,
                        HaiSYmd = item.HaiSYmd,
                        IkNm = item.IkNm,
                        NyuKinTejNm = item.NyuKinTejNm,
                        NyuSihYmd = item.NyuSihYmd,
                        Other11 = item.Orther11,
                        Other12 = item.Orther12,
                        Other21 = item.Orther21,
                        Other22 = item.Orther22,
                        PreviousPayment = item.PreviousReceiveAmount,
                        SeiCd = item.SeiCd,
                        SeiCdNm = item.SeiCdNm,
                        SeiFutSyu = item.SeiFutSyu,
                        SeiFutSyuNm = item.SeiFutSyuNm,
                        SeiGyosyaCd = item.SeiGyosyaCd,
                        SeiGyosyaCdNm = item.SeiGyosyaCdNm,
                        SeiRyakuNm = item.SeiRyakuNm,
                        SeiSitenCd = item.SeiSitenCd,
                        SeiSitenCdNm = item.SeiSitenCdNm,
                        SeiSitRyakuNm = item.SeiSitenCdNm,
                        SitenCd = item.SitenCd,
                        SitenNm = item.SitenNm,
                        SitRyakuNm = item.SitRyakuNm,
                        SSiyoEndYmd = item.SSiyoEndYmd,
                        SSiyoStaYmd = item.SSiyoStaYmd,
                        TegataNo = item.TegataNo,
                        TegataYmd = item.TegataYmd,
                        TokRyakuNm = item.TokRyakuNm,
                        TokuiCd = item.TokuiCd,
                        TokuiNm = item.TokuiNm,
                        TouYmd = item.TouYmd,
                        TransferFee = item.TransferFee,
                        TSiyoEndYmd = item.TSiyoEndYmd,
                        TSiyoStaYmd = item.TSiyoStaYmd,
                        UkeEigCd = item.UkeEigCd, 
                        UkeEigyoNm = item.UkeEigyoNm,
                        UkeNo = item.UkeNo,
                        UkeRyakuNm = item.UkeRyakuNm,
                        UnkRen = item.UnkRenS
                    });
                    no++;
                }
                return result;
            }
        }
    }
}
