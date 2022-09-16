using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using StoredProcedureEFCore;

namespace HassyaAllrightCloud.Application.ReceivableList.Queries
{
    public class GetBusinessPlanReceivableList : IRequest<(List<BussinesPlanReceivableGridDataModel>, BusinessPlanReceivablePaymentSummary, int)>
    {
        public int TenantCdSeq { get; set; }
        public int CompanyCd { get; set; }
        public ReceivableFilterModel ReceivableFilterModel { get; set; }
        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public bool IsGetSingle { get; set; }
        public bool isReport { get; set; }
        public class Handler : IRequestHandler<GetBusinessPlanReceivableList, (List<BussinesPlanReceivableGridDataModel>, BusinessPlanReceivablePaymentSummary, int)>
        {
            private readonly KobodbContext _dbcontext;
            public Handler(KobodbContext context)
            {
                _dbcontext = context;
            }
            public async Task<(List<BussinesPlanReceivableGridDataModel>, BusinessPlanReceivablePaymentSummary, int)> Handle(GetBusinessPlanReceivableList request, CancellationToken cancellationToken)
            {
                var pagingData = new List<BussinesPlanReceivableGridDataModel>();
                var totalRows = 0;
                var summary = new BusinessPlanReceivablePaymentSummary();

                var startBillAddress = (request.ReceivableFilterModel.startCustomerComponentGyosyaData == null ? "000"
                    : request.ReceivableFilterModel.startCustomerComponentGyosyaData.GyosyaCd.ToString("D3")) +
                    (request.ReceivableFilterModel.startCustomerComponentTokiskData == null ? "0000"
                    : request.ReceivableFilterModel.startCustomerComponentTokiskData.TokuiCd.ToString("D4")) +
                    (request.ReceivableFilterModel.startCustomerComponentTokiStData == null ? "0000"
                    : request.ReceivableFilterModel.startCustomerComponentTokiStData.SitenCd.ToString("D4"));
                var endBillAddress = (request.ReceivableFilterModel.endCustomerComponentGyosyaData == null ? "999"
                    : request.ReceivableFilterModel.endCustomerComponentGyosyaData.GyosyaCd.ToString("D3")) +
                    (request.ReceivableFilterModel.endCustomerComponentTokiskData == null ? "9999"
                    : request.ReceivableFilterModel.endCustomerComponentTokiskData.TokuiCd.ToString("D4")) +
                    (request.ReceivableFilterModel.endCustomerComponentTokiStData == null ? "9999"
                    : request.ReceivableFilterModel.endCustomerComponentTokiStData.SitenCd.ToString("D4"));

                _dbcontext.LoadStoredProc("PK_dBusinessPlanReceivablePaging_R")
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
                          .AddParam("@UnpaidSelection", request.ReceivableFilterModel.UnpaidSelection != null ? request.ReceivableFilterModel.UnpaidSelection.IdValue.ToString() : string.Empty)
                          .AddParam("@SaleOfficeKbn", request.ReceivableFilterModel.SaleOfficeType != null ? request.ReceivableFilterModel.SaleOfficeType.SaleOfficeName : string.Empty)
                          .AddParam("@SaleOfficeSelectiton", request.ReceivableFilterModel.SelectedSaleBranchPayment != null ? request.ReceivableFilterModel.SelectedSaleBranchPayment.CEigyoCdSeq.ToString() : string.Empty)
                          .AddParam("@StartBillingAddress", startBillAddress)
                          .AddParam("@EndBillingAddress", endBillAddress)

                          .AddParam("Skip", request.IsGetSingle ? 0 : request.PageNum * request.PageSize)
                          .AddParam("Take", request.IsGetSingle ? 1 : request.PageSize)
                          .Exec(r => pagingData = r.ToList<BussinesPlanReceivableGridDataModel>());

                int count = (request.PageNum * request.PageSize) + 1;
                foreach (var item in pagingData)
                {
                    item.No = count;
                    count++;
                }
                var totalCountData = new List<BusinessPlanTotalDataModel>();
                _dbcontext.LoadStoredProc("PK_dBusinessPlanReceivableTotal_R")
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
                          .AddParam("@UnpaidSelection", request.ReceivableFilterModel.UnpaidSelection != null ? request.ReceivableFilterModel.UnpaidSelection.IdValue.ToString() : string.Empty)
                          .AddParam("@SaleOfficeKbn", request.ReceivableFilterModel.SaleOfficeType != null ? request.ReceivableFilterModel.SaleOfficeType.SaleOfficeName : string.Empty)
                          .AddParam("@SaleOfficeSelectiton", request.ReceivableFilterModel.SelectedSaleBranchPayment != null ? request.ReceivableFilterModel.SelectedSaleBranchPayment.CEigyoCdSeq.ToString() : string.Empty)
                          .AddParam("@StartBillingAddress", startBillAddress)
                          .AddParam("@EndBillingAddress", endBillAddress)
                          .AddParam("ROWCOUNT", out IOutParam<int> rowCountBP)
                          .Exec(r => totalCountData = r.ToList<BusinessPlanTotalDataModel>());
                totalRows = rowCountBP.Value;
                if (!request.isReport)
                {
                    summary.TotalFareSalesAmount = totalCountData.Sum(x => x.UnUriGakKin);
                    summary.TotalFareTaxAmount = totalCountData.Sum(x => x.UnSyaRyoSyo);
                    summary.TotalFareFeeAmount = totalCountData.Sum(x => x.UnSyaRyoTes);
                    summary.TotalFareDepositAmount = totalCountData.Sum(x => x.UnNyukinG);
                    summary.TotalFareUnpaidAmount = totalCountData.Sum(x => x.UnMisyuG);

                    summary.TotalGuideSalesAmount = totalCountData.Sum(x => x.GaUriGakKin);
                    summary.TotalGuideTaxAmount = totalCountData.Sum(x => x.GaSyaRyoSyo);
                    summary.TotalGuideFeeAmount = totalCountData.Sum(x => x.GaSyaRyoTes);
                    summary.TotalGuideDepositAmount = totalCountData.Sum(x => x.GaNyukinG);
                    summary.TotalGuideUnpaidAmount = totalCountData.Sum(x => x.GaMisyuG);

                    summary.TotalOtherSalesAmount = totalCountData.Sum(x => x.SoUriGakKin);
                    summary.TotalOtherTaxAmount = totalCountData.Sum(x => x.SoSyaRyoSyo);
                    summary.TotalOtherFeeAmount = totalCountData.Sum(x => x.SoSyaRyoTes);
                    summary.TotalOtherDepositAmount = totalCountData.Sum(x => x.SoNyukinG);
                    summary.TotalOtherUnpaidAmount = totalCountData.Sum(x => x.SoMisyuG);

                    summary.TotalCancelSalesAmount = totalCountData.Sum(x => x.CaUriGakKin);
                    summary.TotalCancelTaxAmount = totalCountData.Sum(x => x.CaSyaRyoSyo);
                    summary.TotalCancelFeeAmount = totalCountData.Sum(x => x.CaNyukinG);
                    summary.TotalCancelUnpaidAmount = totalCountData.Sum(x => x.CaMisyuG);
                    summary.TotalUnpaidSubTotal = totalCountData.Sum(x => x.MisyuG);

                    summary.PageFareSalesAmount = pagingData.Sum(x => x.UnUriGakKin);
                    summary.PageFareTaxAmount = pagingData.Sum(x => x.UnSyaRyoSyo);
                    summary.PageFareFeeAmount = pagingData.Sum(x => x.UnSyaRyoTes);
                    summary.PageFareDepositAmount = pagingData.Sum(x => x.UnNyukinG);
                    summary.PageFareUnpaidAmount = pagingData.Sum(x => x.UnMisyuG);

                    summary.PageGuideSalesAmount = pagingData.Sum(x => x.GaUriGakKin);
                    summary.PageGuideTaxAmount = pagingData.Sum(x => x.GaSyaRyoSyo);
                    summary.PageGuideFeeAmount = pagingData.Sum(x => x.GaSyaRyoTes);
                    summary.PageGuideDepositAmount = pagingData.Sum(x => x.GaNyukinG);
                    summary.PageGuideUnpaidAmount = pagingData.Sum(x => x.GaMisyuG);

                    summary.PageOtherSalesAmount = pagingData.Sum(x => x.SoUriGakKin);
                    summary.PageOtherTaxAmount = pagingData.Sum(x => x.SoSyaRyoSyo);
                    summary.PageOtherFeeAmount = pagingData.Sum(x => x.SoSyaRyoTes);
                    summary.PageOtherDepositAmount = pagingData.Sum(x => x.SoNyukinG);
                    summary.PageOtherUnpaidAmount = pagingData.Sum(x => x.SoMisyuG);

                    summary.PageCancelSalesAmount = pagingData.Sum(x => x.CaUriGakKin);
                    summary.PageCancelTaxAmount = pagingData.Sum(x => x.CaSyaRyoSyo);
                    summary.PageCancelFeeAmount = pagingData.Sum(x => x.CaNyukinG);
                    summary.PageCancelUnpaidAmount = pagingData.Sum(x => x.CaMisyuG);
                    summary.PageUnpaidSubTotal = pagingData.Sum(x => x.MisyuG);
                }
                return (pagingData, summary, totalRows);
            }
        }
    }
}
