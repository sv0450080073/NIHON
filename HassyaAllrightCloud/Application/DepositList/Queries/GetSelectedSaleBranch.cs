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
    public class GetSelectedSaleBranch : IRequest<List<SelectedSaleBranchModel>>
    {
        public int TenantCdSeq { get; set; }
        public DepositListSearchModel depositListSearchModel { get; set; }
        public class Handler : IRequestHandler<GetSelectedSaleBranch, List<SelectedSaleBranchModel>>
        {
            private readonly KobodbContext _dbcontext;
            public Handler(KobodbContext context)
            {
                _dbcontext = context;
            }
            public async Task<List<SelectedSaleBranchModel>> Handle(GetSelectedSaleBranch request, CancellationToken cancellationToken)
            {
                var result = new List<SelectedSaleBranchModel>();

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

                _dbcontext.LoadStoredProc("PK_dSelectedSaleBranch_R")
                          .AddParam("@TenantCdSeq", request.TenantCdSeq)

                          .AddParam("@SalesOfficeKbn", request.depositListSearchModel.SaleOfficeType.SaleOfficeName)
                          .AddParam("@StartPaymentDate", request.depositListSearchModel.StartPaymentDate != null ? request.depositListSearchModel.StartPaymentDate.ToString().Substring(0, 10).Replace("/", string.Empty) :string.Empty)
                          .AddParam("@EndPaymentDate", request.depositListSearchModel.EndPaymentDate !=null ? request.depositListSearchModel.EndPaymentDate.ToString().Substring(0, 10).Replace(" / ", string.Empty) : string.Empty)
                          .AddParam("@CompanyCdSeq", request.depositListSearchModel.CompanyData != null ? request.depositListSearchModel.CompanyData.CompanyCdSeq : -1)
                          .AddParam("@StartReceptionNumber", request.depositListSearchModel.StartReceiptNumber != null ? long.Parse(request.depositListSearchModel.StartReceiptNumber) : 0)
                          .AddParam("@EndReceptionNumber", request.depositListSearchModel.EndReceiptNumber != null ? long.Parse(request.depositListSearchModel.EndReceiptNumber) : 0)
                          .AddParam("@StartReservationCategory", request.depositListSearchModel.StartReservationClassification != null ? request.depositListSearchModel.StartReservationClassification.YoyaKbn : 0)
                          .AddParam("@EndReservationCategory", request.depositListSearchModel.EndReservationClassification != null ? request.depositListSearchModel.EndReservationClassification.YoyaKbn : 0)
                          .AddParam("@BillingTypeSelection", request.depositListSearchModel.BillingType != null ? request.depositListSearchModel.BillingType : string.Empty)
                          .AddParam("@StartTransferBank", request.depositListSearchModel.StartTransferBank != null ? request.depositListSearchModel.StartTransferBank.Code : string.Empty)
                          .AddParam("@EndTransferBank", request.depositListSearchModel.EndTransferBank != null ? request.depositListSearchModel.EndTransferBank.Code : string.Empty)
                          .AddParam("@PaymentMethod", request.depositListSearchModel.PaymentMethod != null ? request.depositListSearchModel.PaymentMethod : string.Empty)
                          .AddParam("@StartSalesOffice", request.depositListSearchModel.StartSaleBranchList != null ? request.depositListSearchModel.StartSaleBranchList.EigyoCd : -1)
                          .AddParam("@EndSalesOffice", request.depositListSearchModel.EndSaleBranchList != null ? request.depositListSearchModel.EndSaleBranchList.EigyoCd : -1)
                          .AddParam("@StartBillingAddress", startBillAddress)
                          .AddParam("@EndBillingAddress", endBillAddress)
                          .AddParam("ROWCOUNT", out IOutParam<int> rowCount)
                          .Exec(r => result = r.ToList<SelectedSaleBranchModel>());
                int id = 1;
                foreach(var item in result)
                {
                    item.Id = id;
                    id++;
                }
                return result;
            }
        }
    }
}
