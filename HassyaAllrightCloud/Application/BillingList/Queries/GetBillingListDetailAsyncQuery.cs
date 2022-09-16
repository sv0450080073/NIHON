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
    public class GetBillingListDetailAsyncQuery : IRequest<BillingListDetailResult>
    {
        public BillingListFilter billingListFilter { get; set; }
        public class Handler : IRequestHandler<GetBillingListDetailAsyncQuery, BillingListDetailResult>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<BillingListDetailResult> Handle(GetBillingListDetailAsyncQuery request, CancellationToken cancellationToken = default)
            {
                if (request.billingListFilter == null)
                    return new BillingListDetailResult();

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

                BillingListDetailResult BillingListDetailResult = new BillingListDetailResult();
                List<BillingListBusType> billingListBusTypes = new List<BillingListBusType>();
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
                await _context.LoadStoredProc("PK_dBillingListDetail_R")
                                .AddParam("@_TenantCdSeq", billingListFilterParam.TenantCdSeq)
                                .AddParam("@_BillDate", billingListFilterParam.BillDate)
                                .AddParam("@_CloseDate", billingListFilterParam.CloseDate)
                                .AddParam("@_StartBillAddress", billingListFilterParam.StartBillAddress)
                                .AddParam("@_EndBillAddress", billingListFilterParam.EndBillAddress)
                                .AddParam("@_BillOffice", billingListFilterParam.BillOffice)
                                .AddParam("@_StartReservationClassification", billingListFilterParam.StartReservationClassification)
                                .AddParam("@_EndReservationClassification", billingListFilterParam.EndReservationClassification)
                                .AddParam("@_BillTypes", billingListFilterParam.BillTypes)
                                .AddParam("@_StartReceiptNumber", billingListFilterParam.StartReceiptNumber)
                                .AddParam("@_EndReceiptNumber", billingListFilterParam.EndReceiptNumber)
                                .AddParam("@_StartBillClassification", billingListFilterParam.StartBillClassification)
                                .AddParam("@_EndBillClassification", billingListFilterParam.EndBillClassification)
                                .AddParam("@_BillIssuedClassification", billingListFilterParam.BillIssuedClassification)
                                .AddParam("@_BillTypeOrder", billingListFilterParam.BillTypeOrder)
                                .AddParam("@_GyosyaCd", billingListFilterParam.GyosyaCd)
                                .AddParam("@_TokuiCd", billingListFilterParam.TokuiCd)
                                .AddParam("@_SitenCd", billingListFilterParam.SitenCd)
                                .AddParam("@_TokuiNm", billingListFilterParam.TokuiNm)
                                .AddParam("@_Offset", billingListFilterParam.Offset)
                                .AddParam("@_Limit", billingListFilterParam.Limit)
                                .AddParam("ROWCOUNT", out IOutParam<int> rowCount)
                                .ExecAsync(async r => { 
                                    BillingListDetailResult.billingListDetailGrids = await r.ToListAsync<BillingListDetailGrid>();
                                    await r.NextResultAsync();
                                    BillingListDetailResult.billingListTotals = await r.ToListAsync<BillingListTotal>();
                                    await r.NextResultAsync();
                                    if (await r.ReadAsync())
                                        BillingListDetailResult.CountNumber = (int)r["CountNumber"];
                                    await r.CloseAsync();            
                                });

                string ukeNoList = string.Join(",", BillingListDetailResult.billingListDetailGrids.Select(x => x.UkeNo).Distinct());
                await _context.LoadStoredProc("PK_dBillingListDetailBusType_R")
                                .AddParam("@TenantCdSeq", new ClaimModel().TenantID)
                                .AddParam("@ListUkeNo", ukeNoList)
                                .AddParam("ROWCOUNT", out IOutParam<int> rowCount2)
                                .ExecAsync(async r => billingListBusTypes = await r.ToListAsync<BillingListBusType>());

                foreach (var item in BillingListDetailResult.billingListDetailGrids)
                {
                    if (item.SeiFutSyu == (byte)1)
                    {
                        item.DetailBusType = billingListBusTypes.Where(x => x.UkeNo == item.UkeNo).FirstOrDefault()?.SyaSyuCd_SyaSyuNm +
                            billingListBusTypes.Where(x => x.UkeNo == item.UkeNo).FirstOrDefault()?.KataKbn_RyakuNm;
                    }
                    
                }

                BillingListDetailResult.billingListTotals.Select(x => { x.Type = 3; return x; }).ToList();

                return BillingListDetailResult;
            }
        }
    }
}
