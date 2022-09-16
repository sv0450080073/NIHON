using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.ReceivableList.Queries
{
    public class GetSelectedPaymentAddress : IRequest<List<SelectedPaymentAddressModel>>
    {
        public int TenantCdSeq { get; set; }
        public int CompanyCd { get; set; }
        public ReceivableFilterModel ReceivableFilterModel { get; set; }
        public class Handler : IRequestHandler<GetSelectedPaymentAddress, List<SelectedPaymentAddressModel>>
        {
            private readonly KobodbContext _dbcontext;
            public Handler(KobodbContext context)
            {
                _dbcontext = context;
            }
            public async Task<List<SelectedPaymentAddressModel>> Handle(GetSelectedPaymentAddress request, CancellationToken cancellationToken)
            {
                var result = new List<SelectedPaymentAddressModel>();

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

                _dbcontext.LoadStoredProc("PK_dSelectedPaymentAddressReceivable_R")
                          .AddParam("@TenantCdSeq", request.TenantCdSeq)
                          .AddParam("@CompanyCdSeq", request.ReceivableFilterModel.CompanyData != null ? request.ReceivableFilterModel.CompanyData.CompanyCdSeq : -1)
                          .AddParam("@PaymentDate", request.ReceivableFilterModel.PaymentDate != null ? request.ReceivableFilterModel.PaymentDate.ToString().Substring(0, 10).Replace("/", string.Empty) : string.Empty)
                          .AddParam("@StartPaymentDate", request.ReceivableFilterModel.StartPaymentDate != null ? request.ReceivableFilterModel.StartPaymentDate.ToString().Substring(0, 10).Replace("/", string.Empty) : string.Empty)
                          .AddParam("@EndPaymentDate", request.ReceivableFilterModel.EndPaymentDate != null ? request.ReceivableFilterModel.EndPaymentDate.ToString().Substring(0, 10).Replace("/", string.Empty) : string.Empty)
                          .AddParam("@StartReceptionNumber", !string.IsNullOrWhiteSpace(request.ReceivableFilterModel.StartReceiptNumber) ? long.Parse(request.ReceivableFilterModel.StartReceiptNumber) : 0)
                          .AddParam("@EndReceptionNumber", !string.IsNullOrWhiteSpace(request.ReceivableFilterModel.EndReceiptNumber) ? long.Parse(request.ReceivableFilterModel.EndReceiptNumber) : 0)
                          .AddParam("@StartReservation", request.ReceivableFilterModel.StartReservationClassification != null ? request.ReceivableFilterModel.StartReservationClassification.YoyaKbn : 0)
                          .AddParam("@EndReservation", request.ReceivableFilterModel.EndReservationClassification != null ? request.ReceivableFilterModel.EndReservationClassification.YoyaKbn : 0)
                          .AddParam("@BillingType", request.ReceivableFilterModel.BillingType != null ? request.ReceivableFilterModel.BillingType : string.Empty)
                          .AddParam("@UnpaidSelection", request.ReceivableFilterModel.UnpaidSelection != null ? request.ReceivableFilterModel.UnpaidSelection.IdValue.ToString() : string.Empty)
                          .AddParam("@SaleOfficeKbn", request.ReceivableFilterModel.SaleOfficeType != null ? request.ReceivableFilterModel.SaleOfficeType.SaleOfficeName : string.Empty)
                          .AddParam("@SaleOfficeSelectiton", request.ReceivableFilterModel.SelectedSaleBranchPayment != null ? request.ReceivableFilterModel.SelectedSaleBranchPayment.CEigyoCdSeq.ToString() : string.Empty)
                          .AddParam("@StartBillingAddress", startBillAddress)
                          .AddParam("@EndBillingAddress", endBillAddress)

                          .AddParam("ROWCOUNT", out IOutParam<int> rowCount)
                          .Exec(r => result = r.ToList<SelectedPaymentAddressModel>());
                int id = 1;
                foreach (var item in result)
                {
                    item.Id = id;
                    id++;
                }
                return result;
            }
        }
    }
}
