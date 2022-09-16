using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using System.Threading;
using HassyaAllrightCloud.Infrastructure.Persistence;
using StoredProcedureEFCore;
using HassyaAllrightCloud.Commons.Constants;

namespace HassyaAllrightCloud.Application.ReceivableList.Queries
{
    public class GetReceivableList : IRequest<(List<ReceivableListDataModelFromQueryResult>, ReceivablePaymentSummary, int)>
    {
        public int TenantCdSeq { get; set; }
        public int CompanyCd { get; set; }
        public bool IsGetSingle { get; set; }
        public int PageSize { get; set; }
        public int PageNum { get; set; }
        public ReceivableFilterModel ReceivableFilterModel { get; set; }
        public class Handler : IRequestHandler<GetReceivableList, (List<ReceivableListDataModelFromQueryResult>, ReceivablePaymentSummary, int)>
        {
            private readonly KobodbContext _dbcontext;
            public Handler(KobodbContext context)
            {
                _dbcontext = context;
            }
            public async Task<(List<ReceivableListDataModelFromQueryResult>, ReceivablePaymentSummary, int)> Handle(GetReceivableList request, CancellationToken cancellationToken)
            {
                var result = new List<ReceivableListDataModelFromQueryResult>();
                var totalRows = 0;
                var summary = new ReceivablePaymentSummary();

                _dbcontext.LoadStoredProc("PK_dSelectedReceivableListReceivable_R")
                          .AddParam("@TenantCdSeq", request.TenantCdSeq)
                          .AddParam("@CompanyCdSeq", request.ReceivableFilterModel.CompanyData != null ? request.ReceivableFilterModel.CompanyData.CompanyCdSeq : -1)
                          .AddParam("@PaymentDate", request.ReceivableFilterModel.PaymentDate != null ? request.ReceivableFilterModel.PaymentDate.ToString().Substring(0, 10).Replace("/", string.Empty) : string.Empty)
                          .AddParam("@StartPaymentDate", request.ReceivableFilterModel.StartPaymentDate != null ? request.ReceivableFilterModel.StartPaymentDate.ToString().Substring(0, 10).Replace("/", string.Empty) : string.Empty)
                          .AddParam("@EndPaymentDate", request.ReceivableFilterModel.EndPaymentDate != null ? request.ReceivableFilterModel.EndPaymentDate.ToString().Substring(0, 10).Replace("/", string.Empty) : string.Empty)
                          .AddParam("@StartReceptionNumber", request.ReceivableFilterModel.StartReceiptNumber != null ? long.Parse(request.ReceivableFilterModel.StartReceiptNumber) : 0)
                          .AddParam("@EndReceptionNumber", request.ReceivableFilterModel.EndReceiptNumber != null ? long.Parse(request.ReceivableFilterModel.EndReceiptNumber) : 0)
                          .AddParam("@StartReservation", request.ReceivableFilterModel.StartReservationClassification != null ? request.ReceivableFilterModel.StartReservationClassification.YoyaKbn : 0)
                          .AddParam("@EndReservation", request.ReceivableFilterModel.EndReservationClassification != null ? request.ReceivableFilterModel.EndReservationClassification.YoyaKbn : 0)
                          .AddParam("@BillingType", request.ReceivableFilterModel.BillingType != null ? request.ReceivableFilterModel.BillingType : string.Empty)
                          .AddParam("@UnpaidSelection", request.ReceivableFilterModel.UnpaidSelection != null ? request.ReceivableFilterModel.UnpaidSelection.IdValue.ToString() : "0")
                          .AddParam("@SaleOfficeKbn", request.ReceivableFilterModel.SaleOfficeType != null ? request.ReceivableFilterModel.SaleOfficeType.SaleOfficeName : string.Empty)
                          .AddParam("@SaleOfficeSelectiton", request.ReceivableFilterModel.SelectedSaleBranchPayment != null ? request.ReceivableFilterModel.SelectedSaleBranchPayment.CEigyoCdSeq.ToString() : string.Empty)
                          .AddParam("@CSeiGyosyaCd", request.ReceivableFilterModel.SelectedBillingAddressPayment.CSeiGyosyaCd?.ToString())
                          .AddParam("@CSeiCd", request.ReceivableFilterModel.SelectedBillingAddressPayment.CSeiCd?.ToString())
                          .AddParam("@CSeiSitenCd", request.ReceivableFilterModel.SelectedBillingAddressPayment.CSeiSitenCd?.ToString())
                          .AddParam("@CSeiCdSeq", request.ReceivableFilterModel.SelectedBillingAddressPayment.CSeiCdSeq?.ToString())
                          .AddParam("@CSeiSiyoStaYmd", request.ReceivableFilterModel.SelectedBillingAddressPayment.CSeiSiyoStaYmd?.ToString())
                          .AddParam("@CSeiSiyoEndYmd", request.ReceivableFilterModel.SelectedBillingAddressPayment.CSeiSiyoEndYmd?.ToString())
                          .AddParam("@CSeiSitenCdSeq", request.ReceivableFilterModel.SelectedBillingAddressPayment.CSeiSitenCdSeq?.ToString())
                          .AddParam("@CSitSiyoStaYmd", request.ReceivableFilterModel.SelectedBillingAddressPayment.CSitSiyoStaYmd?.ToString())
                          .AddParam("@CSitSiyoEndYmd", request.ReceivableFilterModel.SelectedBillingAddressPayment.CSitSiyoEndYmd?.ToString())
                          .AddParam("@Skip", request.IsGetSingle ? 0 : request.PageNum * request.PageSize)
                          .AddParam("@Take", request.IsGetSingle ? 1 : request.PageSize)

                          .AddParam("@TotalCount", out IOutParam<int> rowCount)
                          .AddParam("@TotalAllTaxAmount", out IOutParam<long> totalAllTaxAmount)
                          .AddParam("@TotalAllSaleAmount", out IOutParam<long> totalAllSaleAmount)
                          .AddParam("@TotalAllFeeAmount", out IOutParam<long> totalAllFeeAmount)
                          .AddParam("@TotalAllBillingAmount", out IOutParam<long> totalAllBillingAmount)
                          .AddParam("@TotalAllDepositAmount", out IOutParam<long> totalAllDepositAmount)
                          .AddParam("@TotalAllUnpaidAmount", out IOutParam<long> totalAllUnpaidAmount)
                          .AddParam("@TotalAllCouponAmount", out IOutParam<long> totalAllCouponAmount)
                          .Exec(r => result = r.ToList<ReceivableListDataModelFromQueryResult>());
                totalRows = rowCount.Value;
                summary.TotalAllDepositAmount = totalAllDepositAmount.Value;
                summary.TotalAllBillingAmount = totalAllBillingAmount.Value;
                summary.TotalAllCouponAmount = totalAllCouponAmount.Value;
                summary.TotalAllFeeAmount = totalAllFeeAmount.Value;
                summary.TotalAllSaleAmount = totalAllSaleAmount.Value;
                summary.TotalAllUnpaidAmount = totalAllUnpaidAmount.Value;
                summary.TotalAllTaxAmount = totalAllTaxAmount.Value;

                summary.TotalPageDepositAmount = result.Sum(e => Convert.ToInt64(e.NyukinG));
                summary.TotalPageBillingAmount = result.Sum(e => Convert.ToInt64(e.SeiKin));
                summary.TotalPageCouponAmount = result.Sum(e => Convert.ToInt64(e.CouKesRui));
                summary.TotalPageFeeAmount = result.Sum(e => Convert.ToInt64(e.SyaRyoTes));
                summary.TotalPageSaleAmount = result.Sum(e => Convert.ToInt64(e.UriGakKin));
                summary.TotalPageUnpaidAmount = result.Sum(e => Convert.ToInt64(e.SeiKin) - Convert.ToInt64(e.NyukinG) - Convert.ToInt64(e.FuriTes));
                summary.TotalPageTaxAmount = result.Sum(e => Convert.ToInt64(e.SyaRyoSyo));

                return (result, summary, totalRows);
            }
        }
    }
}
