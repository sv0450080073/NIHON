using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.BillingList;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BillPrint.Queries
{
    public class GetBillingListReportAsyncQuery : IRequest<List<BillingListReport>>
    {
        public BillingListFilter billingListFilter { get; set; }
        public class Handler : IRequestHandler<GetBillingListReportAsyncQuery, List<BillingListReport>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<BillingListReport>> Handle(GetBillingListReportAsyncQuery request, CancellationToken cancellationToken = default)
            {
                if(request.billingListFilter == null)
                    return new List<BillingListReport>();

                var startBillAddress = (request.billingListFilter.startCustomerComponentGyosyaData == null ? "000"
                    : request.billingListFilter.startCustomerComponentGyosyaData.GyosyaCd.ToString("D3")) +
                    (request.billingListFilter.startCustomerComponentTokiskData == null ? "0000"
                    : request.billingListFilter.startCustomerComponentTokiskData.TokuiCd.ToString("D4")) +
                    (request.billingListFilter.startCustomerComponentTokiStData == null ? "0000"
                    : request.billingListFilter.startCustomerComponentTokiStData.SitenCd.ToString("D4"));
                var endBillAddress = (request.billingListFilter.endCustomerComponentGyosyaData == null ? "999"
                    : request.billingListFilter.endCustomerComponentGyosyaData.GyosyaCd.ToString("D3")) +
                    (request.billingListFilter.endCustomerComponentTokiskData == null ? "9999"
                    : request.billingListFilter.endCustomerComponentTokiskData.TokuiCd.ToString("D4")) +
                    (request.billingListFilter.endCustomerComponentTokiStData == null ? "9999"
                    : request.billingListFilter.endCustomerComponentTokiStData.SitenCd.ToString("D4"));

                List<BillingListReport> billingListReports = new List<BillingListReport>();
                List<BillingListGrid> billingListGrids = new List<BillingListGrid>();

                BillingListFilterParam billingListFilterParam = new BillingListFilterParam()
                {
                    BillOffice = request.billingListFilter.BillOffice == null ? 0 : request.billingListFilter.BillOffice.EigyoCdSeq,
                    BillDate = request.billingListFilter.BillDate == null ? string.Empty : ((DateTime)request.billingListFilter.BillDate).ToString("yyyyMM"),
                    CloseDate = request.billingListFilter.CloseDate.GetValueOrDefault(),
                    BillTypes = request.billingListFilter.BillTypes == null ? string.Empty : string.Join(",", request.billingListFilter.BillTypes),
                    EndBillAddress = endBillAddress,
                    EndReservationClassification = Convert.ToByte(request.billingListFilter.EndReservationClassification?.YoyaKbn),
                    Limit = request.billingListFilter.Limit,
                    Offset = request.billingListFilter.Offset,
                    StartBillAddress = startBillAddress,
                    StartReservationClassification = Convert.ToByte(request.billingListFilter.StartReservationClassification?.YoyaKbn),
                    GyosyaCd = string.IsNullOrWhiteSpace(request.billingListFilter.Code) ? (short)0 : short.Parse(request.billingListFilter.Code.Substring(0, 3)),
                    TokuiCd = string.IsNullOrWhiteSpace(request.billingListFilter.Code) ? (short)0 : short.Parse(request.billingListFilter.Code.Substring(4, 4)),
                    SitenCd = string.IsNullOrWhiteSpace(request.billingListFilter.Code) ? (short)0 : short.Parse(request.billingListFilter.Code.Substring(9, 4)),
                    TokuiNm = string.IsNullOrWhiteSpace(request.billingListFilter.Code) ? string.Empty : request.billingListFilter.Code.Split(":")[1].Substring(1),
                    BillIssuedClassification = byte.Parse(request.billingListFilter.BillIssuedClassification?.IdValue.ToString()),
                    BillTypeOrder = request.billingListFilter.BillTypeOrder?.StringValue,
                    EndBillClassification = request.billingListFilter.EndBillClassification == null ? (long)0 : long.Parse(request.billingListFilter.EndBillClassification?.CodeKbn),
                    EndReceiptNumber = request.billingListFilter.EndReceiptNumber == null ? string.Empty : $"{new ClaimModel().TenantID.ToString("D5")}{request.billingListFilter.EndReceiptNumber.GetValueOrDefault().ToString("D10")}",
                    StartBillClassification = request.billingListFilter.StartBillClassification == null ? (long)0 : long.Parse(request.billingListFilter.StartBillClassification?.CodeKbn),
                    StartReceiptNumber = request.billingListFilter.StartReceiptNumber == null ? string.Empty : $"{new ClaimModel().TenantID.ToString("D5")}{request.billingListFilter.StartReceiptNumber.GetValueOrDefault().ToString("D10")}",
                    TenantCdSeq = new ClaimModel().TenantID
                };
                await _context.LoadStoredProc(request.billingListFilter.TransferAmountOutputClassification?.Code == 1 ?
                    "PK_dBillingListGridWithoutOutputClassificationCsv_R" : "PK_dBillingListGridWithOutputClassificationCsv_R")
                                .AddParam("@TenantCdSeq", billingListFilterParam.TenantCdSeq)
                                .AddParam("@BillDate", billingListFilterParam.BillDate)
                                .AddParam("@CloseDate", billingListFilterParam.CloseDate)
                                .AddParam("@StartBillAddress", billingListFilterParam.StartBillAddress)
                                .AddParam("@EndBillAddress", billingListFilterParam.EndBillAddress)
                                .AddParam("@BillOffice", billingListFilterParam.BillOffice)
                                .AddParam("@BillTypes", billingListFilterParam.BillTypes)
                                .AddParam("ROWCOUNT", out IOutParam<int> rowCount)
                                .ExecAsync(async r => billingListGrids = await r.ToListAsync<BillingListGrid>());

                if (billingListGrids.Any())
                {
                    BillingListReport billingListReport = new BillingListReport() { billingListGrids = new List<BillingListGrid>() };
                    var startBillAddressCode = String.Format("{0} : {1} {2} : {3}", 
                        Convert.ToInt32(request.billingListFilter.startCustomerComponentTokiskData?.TokuiCd).ToString("D4"),
                        request.billingListFilter.startCustomerComponentTokiskData?.RyakuNm,
                        Convert.ToInt32(request.billingListFilter.startCustomerComponentTokiStData?.SitenCd).ToString("D4"),
                        request.billingListFilter.startCustomerComponentTokiStData?.SitenNm);
                    var endBillAddressCode = String.Format("{0} : {1} {2} : {3}",
                        Convert.ToInt32(request.billingListFilter.endCustomerComponentTokiskData?.TokuiCd).ToString("D4"),
                        request.billingListFilter.endCustomerComponentTokiskData?.RyakuNm,
                        Convert.ToInt32(request.billingListFilter.endCustomerComponentTokiStData?.SitenCd).ToString("D4"),
                        request.billingListFilter.endCustomerComponentTokiStData?.SitenNm);
                    billingListReport.mainInfo = new MainInfo()
                    {
                        BillOfficeCode = request.billingListFilter.BillOffice?.Text,
                        BillDate = request.billingListFilter.BillDate == null ? string.Empty : ((DateTime)request.billingListFilter.BillDate).ToString("yyyy/MM"),
                        CloseDate = billingListFilterParam.CloseDate,
                        BillAddressCode = $"{startBillAddressCode} {endBillAddressCode}",
                        TransferAmountOutputClassification = request.billingListFilter.TransferAmountOutputClassification?.Text,
                        BillIssuedClassification = request.billingListFilter.BillIssuedClassification?.StringValue,
                        UserCode = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq.ToString().PadLeft(10, '0'),
                        UserName = new HassyaAllrightCloud.Domain.Dto.ClaimModel().Name,
                        CurrentDate = $"{DateTime.Now.ToString("yyyy/MM/dd")}  {DateTime.Now.ToString("HH:mm")}"
                    };
                    long count = 1;
                    foreach(var item in billingListGrids)
                    {
                        item.No = count;
                        count++;
                        billingListReport.billingListGrids.Add(item);
                    }

                    billingListReports.Add(billingListReport);
                }
                return billingListReports;
            }
        }
    }
}
