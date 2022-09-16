using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using System.Threading;
using StoredProcedureEFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using HassyaAllrightCloud.Commons.Helpers;

namespace HassyaAllrightCloud.Application.DepositList.Queries
{
    public class GetDepositList : IRequest<(List<DepositDataGrid>, DepositListSummary, int)>
    {
        public int TenantCdSeq { get; set; }
        public int SeiKrksKbn { get; set; }
        public string SyoriYm { get; set; }
        public DepositListSearchModel depositListSearchModel { get; set; }
        public bool IsGetSingle { get; set; }
        public class Handler : IRequestHandler<GetDepositList, (List<DepositDataGrid>, DepositListSummary, int)>
        {
            private readonly KobodbContext _dbcontext;
            public Handler(KobodbContext context)
            {
                _dbcontext = context;
            }
            public async Task<(List<DepositDataGrid>, DepositListSummary, int)> Handle(GetDepositList request, CancellationToken cancellationToken)
            {
                var depositDataPaging = new List<DepositListModel>();
                var depositDataTotalFromDB = new List<DepositListModel>();
                var result = new List<DepositDataGrid>();
                var resultFinal = new List<DepositDataGrid>();
                var depositDataTotalDb = new List<DepositDataGrid>();
                var summary = new DepositListSummary();
                int totalRow;

                var startBillAddress = (request.depositListSearchModel.startCustomerComponentGyosyaData == null ? "000"
                    : request.depositListSearchModel.startCustomerComponentGyosyaData.GyosyaCd.ToString("D3")) +
                    (request.depositListSearchModel.startCustomerComponentTokiskData == null ? "0000"
                    : request.depositListSearchModel.startCustomerComponentTokiskData.TokuiCd.ToString("D4")) +
                    (request.depositListSearchModel.startCustomerComponentTokiStData == null ? "0000"
                    : request.depositListSearchModel.startCustomerComponentTokiStData.SitenCd.ToString("D4"));
                var endBillAddress = (request.depositListSearchModel.endCustomerComponentGyosyaData == null ? "999"
                    : request.depositListSearchModel.endCustomerComponentGyosyaData.GyosyaCd.ToString("D3")) +
                    (request.depositListSearchModel.endCustomerComponentTokiskData == null ? "9999"
                    : request.depositListSearchModel.endCustomerComponentTokiskData.TokuiCd.ToString("D4")) +
                    (request.depositListSearchModel.endCustomerComponentTokiStData == null ? "9999"
                    : request.depositListSearchModel.endCustomerComponentTokiStData.SitenCd.ToString("D4"));

                _dbcontext.LoadStoredProc("PK_dSelectDepositListPaging_R")
                          .AddParam("TenantCdSeq", request.TenantCdSeq)

                          .AddParam("@SalesOfficeKbn", request.depositListSearchModel.SaleOfficeType.SaleOfficeName)
                          .AddParam("@StartPaymentDate", request.depositListSearchModel.StartPaymentDate != null ? request.depositListSearchModel.StartPaymentDate.ToString().Substring(0, 10).Replace("/", string.Empty) : string.Empty)
                          .AddParam("@EndPaymentDate", request.depositListSearchModel.EndPaymentDate != null ? request.depositListSearchModel.EndPaymentDate.ToString().Substring(0, 10).Replace(" / ", string.Empty) : string.Empty)
                          .AddParam("@CompanyCdSeq", request.depositListSearchModel.CompanyData != null ? request.depositListSearchModel.CompanyData.CompanyCdSeq : -1)
                          .AddParam("@StartReceptionNumber", request.depositListSearchModel.StartReceiptNumber != null ? long.Parse(request.depositListSearchModel.StartReceiptNumber) : 0)
                          .AddParam("@EndReceptionNumber", request.depositListSearchModel.EndReceiptNumber != null ? long.Parse(request.depositListSearchModel.EndReceiptNumber) : 0)
                          .AddParam("@StartReservationCategory", request.depositListSearchModel.StartReservationClassification != null ? request.depositListSearchModel.StartReservationClassification.YoyaKbn : 0)
                          .AddParam("@EndReservationCategory", request.depositListSearchModel.EndReservationClassification != null ? request.depositListSearchModel.EndReservationClassification.YoyaKbn : 0)
                          .AddParam("@BillingTypeSelection", request.depositListSearchModel.BillingType != null ? request.depositListSearchModel.BillingType : string.Empty)
                          .AddParam("@StartTransferBank", request.depositListSearchModel.StartTransferBank != null ? request.depositListSearchModel.StartTransferBank.Code : string.Empty)
                          .AddParam("@EndTransferBank", request.depositListSearchModel.EndTransferBank != null ? request.depositListSearchModel.EndTransferBank.Code : string.Empty)
                          .AddParam("@PaymentMethod", request.depositListSearchModel.PaymentMethod != null ? request.depositListSearchModel.PaymentMethod : string.Empty)
                          .AddParam("@SalesOfficeSelection", request.depositListSearchModel.SelectedSaleBranchPayment.CEigyoCdSeq)

                          .AddParam("@CSeiGyosyaCd", request.depositListSearchModel.SelectedBillingAddressPayment.CSeiGyosyaCd)
                          .AddParam("@CSeiCd", request.depositListSearchModel.SelectedBillingAddressPayment.CSeiCd)
                          .AddParam("@CSeiSitenCd", request.depositListSearchModel.SelectedBillingAddressPayment.CSeiSitenCd)
                          .AddParam("@CSeiCdSeq", request.depositListSearchModel.SelectedBillingAddressPayment.CSeiCdSeq)
                          .AddParam("@CSeiSiyoStaYmd", request.depositListSearchModel.SelectedBillingAddressPayment.CSeiSiyoStaYmd != null ? request.depositListSearchModel.SelectedBillingAddressPayment.CSeiSiyoStaYmd : string.Empty)
                          .AddParam("@CSeiSiyoEndYmd", request.depositListSearchModel.SelectedBillingAddressPayment.CSeiSiyoEndYmd != null ? request.depositListSearchModel.SelectedBillingAddressPayment.CSeiSiyoEndYmd : string.Empty)
                          .AddParam("@CSeiSitenCdSeq", request.depositListSearchModel.SelectedBillingAddressPayment.CSeiSitenCdSeq)
                          .AddParam("@CSitSiyoStaYmd", request.depositListSearchModel.SelectedBillingAddressPayment.CSitSiyoStaYmd != null ? request.depositListSearchModel.SelectedBillingAddressPayment.CSitSiyoStaYmd : string.Empty)
                          .AddParam("@CSitSiyoEndYmd", request.depositListSearchModel.SelectedBillingAddressPayment.CSitSiyoEndYmd != null ? request.depositListSearchModel.SelectedBillingAddressPayment.CSitSiyoEndYmd : string.Empty)

                          .AddParam("@Skip", request.IsGetSingle ? 0 : request.depositListSearchModel.PageNum * request.depositListSearchModel.PageSize)
                          .AddParam("@Take", request.IsGetSingle ? 1 : request.depositListSearchModel.IsGetAll == true ? 100000 : request.depositListSearchModel.PageSize)

                         
                          .Exec(r => depositDataPaging = r.ToList<DepositListModel>());

                _dbcontext.LoadStoredProc("PK_dSelectDepositListTotal_R")
                          .AddParam("TenantCdSeq", request.TenantCdSeq)

                          .AddParam("@SalesOfficeKbn", request.depositListSearchModel.SaleOfficeType.SaleOfficeName)
                          .AddParam("@StartPaymentDate", request.depositListSearchModel.StartPaymentDate != null ? request.depositListSearchModel.StartPaymentDate.ToString().Substring(0, 10).Replace("/", string.Empty) : string.Empty)
                          .AddParam("@EndPaymentDate", request.depositListSearchModel.EndPaymentDate != null ? request.depositListSearchModel.EndPaymentDate.ToString().Substring(0, 10).Replace(" / ", string.Empty) : string.Empty)
                          .AddParam("@CompanyCdSeq", request.depositListSearchModel.CompanyData != null ? request.depositListSearchModel.CompanyData.CompanyCdSeq : -1)
                          .AddParam("@StartReceptionNumber", request.depositListSearchModel.StartReceiptNumber != null ? long.Parse(request.depositListSearchModel.StartReceiptNumber) : 0)
                          .AddParam("@EndReceptionNumber", request.depositListSearchModel.EndReceiptNumber != null ? long.Parse(request.depositListSearchModel.EndReceiptNumber) : 0)
                          .AddParam("@StartReservationCategory", request.depositListSearchModel.StartReservationClassification != null ? request.depositListSearchModel.StartReservationClassification.YoyaKbn : 0)
                          .AddParam("@EndReservationCategory", request.depositListSearchModel.EndReservationClassification != null ? request.depositListSearchModel.EndReservationClassification.YoyaKbn : 0)
                          .AddParam("@BillingTypeSelection", request.depositListSearchModel.BillingType != null ? request.depositListSearchModel.BillingType : string.Empty)
                          .AddParam("@StartTransferBank", request.depositListSearchModel.StartTransferBank != null ? request.depositListSearchModel.StartTransferBank.Code : string.Empty)
                          .AddParam("@EndTransferBank", request.depositListSearchModel.EndTransferBank != null ? request.depositListSearchModel.EndTransferBank.Code : string.Empty)
                          .AddParam("@PaymentMethod", request.depositListSearchModel.PaymentMethod != null ? request.depositListSearchModel.PaymentMethod : string.Empty)
                          .AddParam("@SalesOfficeSelection", request.depositListSearchModel.SelectedSaleBranchPayment.CEigyoCdSeq)

                          .AddParam("@CSeiGyosyaCd", request.depositListSearchModel.SelectedBillingAddressPayment.CSeiGyosyaCd)
                          .AddParam("@CSeiCd", request.depositListSearchModel.SelectedBillingAddressPayment.CSeiCd)
                          .AddParam("@CSeiSitenCd", request.depositListSearchModel.SelectedBillingAddressPayment.CSeiSitenCd)
                          .AddParam("@CSeiCdSeq", request.depositListSearchModel.SelectedBillingAddressPayment.CSeiCdSeq)
                          .AddParam("@CSeiSiyoStaYmd", request.depositListSearchModel.SelectedBillingAddressPayment.CSeiSiyoStaYmd != null ? request.depositListSearchModel.SelectedBillingAddressPayment.CSeiSiyoStaYmd : string.Empty)
                          .AddParam("@CSeiSiyoEndYmd", request.depositListSearchModel.SelectedBillingAddressPayment.CSeiSiyoEndYmd != null ? request.depositListSearchModel.SelectedBillingAddressPayment.CSeiSiyoEndYmd : string.Empty)
                          .AddParam("@CSeiSitenCdSeq", request.depositListSearchModel.SelectedBillingAddressPayment.CSeiSitenCdSeq)
                          .AddParam("@CSitSiyoStaYmd", request.depositListSearchModel.SelectedBillingAddressPayment.CSitSiyoStaYmd != null ? request.depositListSearchModel.SelectedBillingAddressPayment.CSitSiyoStaYmd : string.Empty)
                          .AddParam("@CSitSiyoEndYmd", request.depositListSearchModel.SelectedBillingAddressPayment.CSitSiyoEndYmd != null ? request.depositListSearchModel.SelectedBillingAddressPayment.CSitSiyoEndYmd : string.Empty)

                          .AddParam("ROWCOUNT", out IOutParam<int> rowCountTotal)
                          .Exec(r => depositDataTotalFromDB = r.ToList<DepositListModel>());
                totalRow = rowCountTotal.Value;


                resultFinal = CaculateFunction(request, depositDataPaging, result);
                var depositDataTotal = CaculateFunction(request, depositDataTotalFromDB, depositDataTotalDb);
                summary.PageAmount = result.Sum(x => x.Amount);
                summary.PageCumulativePayment = result.Sum(x => x.CumulativePayment);
                summary.PagePreviousReceiveAmount = result.Sum(x => x.PreviousReceiveAmount);
                summary.PageTransferFee = result.Sum(x => x.TransferFee);
                summary.TotalAmount = depositDataTotal.Sum(x => x.Amount);
                summary.TotalCumulativePayment = depositDataTotal.Sum(x => x.CumulativePayment);
                summary.TotalPreviousReceiveAmount = depositDataTotal.Sum(x => x.PreviousReceiveAmount);
                summary.TotalTransferFee = depositDataTotal.Sum(x => x.TransferFee);
                return (resultFinal, summary, totalRow);
            }

            private List<DepositDataGrid> CaculateFunction(GetDepositList request, List<DepositListModel> depositLists, List<DepositDataGrid> result)
            {
                int no = request.depositListSearchModel.PageNum * request.depositListSearchModel.PageSize;
                foreach (var item in depositLists)
                {
                    item.No = (no + 1);
                    if (item.NyuSihSyuS == "91")
                    {
                        item.Orther11 = item.EtcSyo1;
                        item.Orther12 = item.EtcSyo2;
                    }
                    else if (item.NyuSihSyuS == "92")
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
                        if ((!string.IsNullOrWhiteSpace(item.SeikYm) && !string.IsNullOrWhiteSpace(item.NyuSihYmd) && !string.IsNullOrWhiteSpace(item.SeiTaiYmd))
                            && DateTime.ParseExact(item.SeikYm, "yyyyMM", null) >= DateTime.ParseExact(item.NyuSihYmd.Substring(0, 6), "yyyyMM", null) || DateTime.ParseExact(item.NyuSihYmd, "yyyyMMdd", null) >= DateTime.ParseExact(item.SeiTaiYmd, "yyyyMMdd", null))
                        {
                            item.Amount = item.KesG;
                            item.TransferFee = item.FurKesG;
                            item.CumulativePayment = item.KesG + item.FurKesG;
                            item.PreviousReceiveAmount = 0;
                            item.Cash = item.NyuSihSyuS == "1" ? item.KesG : 0;
                            item.Another = item.NyuSihSyuS == "91" || item.NyuSihSyuS == "92" ? item.KesG : 0;
                            item.AdjustMoney = item.NyuSihSyuS == "6" ? item.KesG : 0;
                            item.Bill = item.NyuSihSyuS == "4" ? item.KesG : 0;
                            item.Compensation = item.NyuSihSyuS == "5" ? item.KesG : 0;
                            item.TransferAmount = item.NyuSihSyuS == "2" ? item.KesG : 0;
                        }
                        else
                        {
                            item.Amount = 0;
                            item.TransferFee = 0;
                            item.CumulativePayment = 0;
                            item.PreviousReceiveAmount = item.KesG + item.FurKesG;
                            item.Cash = 0;
                            item.Another = 0;
                            item.AdjustMoney = 0;
                            item.Bill = 0;
                            item.Compensation = item.NyuSihSyuS == "5" ? item.KesG : 0;
                            item.TransferAmount = 0;
                        }
                        
                    }
                    if (request.SeiKrksKbn == 2)
                    {
                        if ((!string.IsNullOrWhiteSpace(request.SyoriYm) && !string.IsNullOrWhiteSpace(item.NyuSihYmd) && !string.IsNullOrWhiteSpace(item.SeiTaiYmd)) && DateTime.ParseExact(request.SyoriYm, "yyyyMM", null) >= DateTime.ParseExact(item.NyuSihYmd.Substring(0, 6), "yyyyMM", null) || DateTime.ParseExact(item.NyuSihYmd, "yyyyMMdd", null) >= DateTime.ParseExact(item.SeiTaiYmd, "yyyyMMdd", null))
                        {
                            item.Amount = item.KesG;
                            item.TransferFee = item.FurKesG;
                            item.CumulativePayment = item.KesG + item.FurKesG;
                            item.PreviousReceiveAmount = 0;
                            item.Cash = item.NyuSihSyuS == "1" ? item.KesG : 0;
                            item.Another = item.NyuSihSyuS == "91" || item.NyuSihSyuS == "92" ? item.KesG : 0;
                            item.AdjustMoney = item.NyuSihSyuS == "6" ? item.KesG : 0;
                            item.Bill = item.NyuSihSyuS == "4" ? item.KesG : 0;
                            item.Compensation = item.NyuSihSyuS == "5" ? item.KesG : 0;
                            item.TransferAmount = item.NyuSihSyuS == "2" ? item.KesG : 0;
                        }
                        else
                        {
                            item.Amount = 0;
                            item.TransferFee = 0;
                            item.CumulativePayment = 0;
                            item.PreviousReceiveAmount = item.KesG + item.FurKesG;
                            item.Cash = 0;
                            item.Another = 0;
                            item.AdjustMoney = 0;
                            item.Bill = 0;
                            item.Compensation = 0;
                            item.TransferAmount = 0;
                        }
                        
                    }
                    result.Add(new DepositDataGrid()
                    {
                        No = item.No,
                        PaymentDate = item.NyuSihYmd,
                        UkeNo = item.UkeNo.Length >= 15 ? item.UkeNo.Substring(5) : item.UkeNo,
                        UkeNoFullString = item.UkeNo,
                        ReceptionOffice = item.UkeRyakuNm,
                        CustomerName = item.TokRyakuNm + " " + item.SitRyakuNm,
                        OperatingSerialNumber = string.IsNullOrEmpty(item.UnkRenS) || item.UnkRenS.Equals("0") ? string.Empty : item.UnkRenS.PadLeft(3, '0'),
                        GroupName = item.DantaNm,
                        DestinationName = item.IkNm,
                        DeliveryDate = item.HaiSYmd,
                        ArrivalDate = item.TouYmd,
                        BillingType = item.SeiFutSyuNm,
                        LoadingGoodName = item.FutTumNm,
                        PaymenMethod = item.NyuKinTejNm,
                        CouponFaceValue = item.CouGkin.ToString(),
                        CouponNo = item.CouNo.ToString(),
                        Transferbank = string.IsNullOrWhiteSpace(item.BankCd) ? string.Empty : item.BankCd + " " + item.BankRyak + " " + item.BankSitCd + " " + item.BankSitRyak,
                        CardApprovalNumber = item.CardSyo,
                        CardSlipNumber = item.CardDen,
                        PaperDueDate = item.TegataYmd,
                        PaperNumber = item.TegataNo,
                        Other11 = item.Orther11,
                        Other12 = item.Orther12,
                        Other21 = item.Orther21,
                        Other22 = item.Orther22,
                        DeliveryDateAndArrivalDate = item.HaiSYmd + item.TouYmd,
                        Amount = item.Amount,
                        CumulativePayment = item.CumulativePayment,
                        FutTumNm = item.FutTumNm,
                        GroupNameAndDestinationName = item.DantaNm + item.IkNm,
                        PreviousReceiveAmount = item.PreviousReceiveAmount,
                        TransferFee = item.TransferFee,
                        UkeNoAndReceptionOffice = item.UkeNo + item.UkeRyakuNm,
                        ServiceDate = string.IsNullOrWhiteSpace(item.HaiSYmd) && string.IsNullOrWhiteSpace(item.TouYmd) ? string.Empty : (item.HaiSYmd.Insert(4, "/").Insert(7, "/") + " ~ " + item.TouYmd.Insert(4, "/").Insert(7, "/")),
                        SaleOffice = item.EigyoRyak,
                        Division = item.SeiFutSyuNm,
                        Cash = item.Cash,
                        AdjustMoney = item.AdjustMoney,
                        Another = item.Another,
                        Bill = item.Bill,
                        Compensation = item.Compensation,
                        TransferAmount = item.TransferAmount,
                        BankNm = item.BankNm,
                        BankSitNm = item.BankSitNm,
                        YouKbn = item.YouKbn,
                        FutTumRen = item.FutTumRen,
                        FutuUnkRen = item.UnkRen,
                        SeiFutSyu = item.SeiFutSyu
                    });
                    no++;
                }
                return result;
            }
        }
    }
}
