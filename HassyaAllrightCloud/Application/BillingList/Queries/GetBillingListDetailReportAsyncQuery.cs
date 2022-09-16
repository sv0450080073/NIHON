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
    public class GetBillingListDetailReportAsyncQuery : IRequest<List<BillingListDetailReport>>
    {
        public BillingListFilter billingListFilter { get; set; }
        public class Handler : IRequestHandler<GetBillingListDetailReportAsyncQuery, List<BillingListDetailReport>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<BillingListDetailReport>> Handle(GetBillingListDetailReportAsyncQuery request, CancellationToken cancellationToken = default)
            {
                if(request.billingListFilter == null)
                    return new List<BillingListDetailReport>();
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

                List<BillingListDetailReport> billingListDetailReports = new List<BillingListDetailReport>();
                List<BillingListDetailGrid> billingListDetailGrids = new List<BillingListDetailGrid>();

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
                await _context.LoadStoredProc("PK_dBillingListDetailCsv_R")
                                .AddParam("@TenantCdSeq", billingListFilterParam.TenantCdSeq)
                                .AddParam("@BillDate", billingListFilterParam.BillDate)
                                .AddParam("@CloseDate", billingListFilterParam.CloseDate)
                                .AddParam("@StartBillAddress", billingListFilterParam.StartBillAddress)
                                .AddParam("@EndBillAddress", billingListFilterParam.EndBillAddress)
                                .AddParam("@BillOffice", billingListFilterParam.BillOffice)
                                .AddParam("@StartReservationClassification", billingListFilterParam.StartReservationClassification)
                                .AddParam("@EndReservationClassification", billingListFilterParam.EndReservationClassification)
                                .AddParam("@BillTypes", billingListFilterParam.BillTypes)
                                .AddParam("@StartReceiptNumber", billingListFilterParam.StartReceiptNumber)
                                .AddParam("@EndReceiptNumber", billingListFilterParam.EndReceiptNumber)
                                .AddParam("@StartBillClassification", billingListFilterParam.StartBillClassification)
                                .AddParam("@EndBillClassification", billingListFilterParam.EndBillClassification)
                                .AddParam("@BillIssuedClassification", billingListFilterParam.BillIssuedClassification)
                                .AddParam("@BillTypeOrder", billingListFilterParam.BillTypeOrder)
                                .AddParam("@GyosyaCd", billingListFilterParam.GyosyaCd)
                                .AddParam("@TokuiCd", billingListFilterParam.TokuiCd)
                                .AddParam("@SitenCd", billingListFilterParam.SitenCd)
                                .AddParam("@TokuiNm", billingListFilterParam.TokuiNm)
                                .AddParam("ROWCOUNT", out IOutParam<int> rowCount)
                                .ExecAsync(async r => billingListDetailGrids = await r.ToListAsync<BillingListDetailGrid>());

                if(billingListDetailGrids.Any())
                {
                    long count = 1;
                    short SeiGyosyaCd = 0;
                    short SeiCd = 0;
                    short SeiSitenCd = 0;
                    string SeiRyakuNm = string.Empty;
                    string SeiSitRyakuNm = string.Empty;
                    foreach (var item in billingListDetailGrids)
                    {
                        item.SeiHatYmd = string.IsNullOrWhiteSpace(item.SeiHatYmd) ? string.Empty : $"{item.SeiHatYmd.Substring(2, 2)}/{item.SeiHatYmd.Substring(4, 2)}/{item.SeiHatYmd.Substring(6, 2)}";
                        item.SeiTaiYmd = string.IsNullOrWhiteSpace(item.SeiTaiYmd) ? string.Empty : $"{item.SeiTaiYmd.Substring(2, 2)}/{item.SeiTaiYmd.Substring(4, 2)}/{item.SeiTaiYmd.Substring(6, 2)}";
                        item.HaiSYmd = string.IsNullOrWhiteSpace(item.HaiSYmd) ? string.Empty : $"{item.HaiSYmd.Substring(2, 2)}/{item.HaiSYmd.Substring(4, 2)}/{item.HaiSYmd.Substring(6, 2)}";
                        item.TouYmd = string.IsNullOrWhiteSpace(item.TouYmd) ? string.Empty : $"{item.TouYmd.Substring(2, 2)}/{item.TouYmd.Substring(4, 2)}/{item.TouYmd.Substring(6, 2)}";
                        item.HasYmd = string.IsNullOrWhiteSpace(item.HasYmd) ? string.Empty : $"{item.HasYmd.Substring(2, 2)}/{item.HasYmd.Substring(4, 2)}/{item.HasYmd.Substring(6, 2)}";
                        item.NyuKinYmd = string.IsNullOrWhiteSpace(item.NyuKinYmd) ? string.Empty : $"{item.NyuKinYmd.Substring(2, 2)}/{item.NyuKinYmd.Substring(4, 2)}/{item.NyuKinYmd.Substring(6, 2)}";

                        if (!(SeiGyosyaCd == item.SeiGyosyaCd && SeiCd == item.SeiCd && SeiSitenCd == item.SeiSitenCd 
                            && SeiRyakuNm == item.SeiRyakuNm && SeiSitRyakuNm == item.SeiSitRyakuNm))
                        {
                            BillingListDetailReport billingListDetailReport = new BillingListDetailReport() { billingListDetailGrids = new List<BillingListDetailGrid>() };
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
                            billingListDetailReport.mainInfo = new MainInfo()
                            {
                                OutputOrder = billingListFilterParam.BillTypeOrder,
                                BillOffice = request.billingListFilter.BillOffice?.Text,
                                BillAddress = $"{item.SeiGyosyaCd.ToString().PadLeft(3, '0')}-{item.SeiCd.ToString().PadLeft(4, '0')}-{item.SeiSitenCd.ToString().PadLeft(4, '0')} : {item.SeiRyakuNm} {item.SeiSitRyakuNm}",
                                BillDate = request.billingListFilter.BillDate == null ? string.Empty : ((DateTime)request.billingListFilter.BillDate).ToString("yyyy/MM"),
                                CloseDate = billingListFilterParam.CloseDate,
                                BillAddressCode = $"{startBillAddressCode} {endBillAddressCode}",
                                BillOfficeCode = request.billingListFilter.BillOffice?.Text,
                                BillIssuedClassification = request.billingListFilter.BillIssuedClassification?.StringValue,
                                TransferAmountOutputClassification = request.billingListFilter.TransferAmountOutputClassification?.Text,
                                UserCode = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq.ToString().PadLeft(10, '0'),
                                UserName = new HassyaAllrightCloud.Domain.Dto.ClaimModel().Name,
                                CurrentDate = $"{DateTime.Now.ToString("yyyy/MM/dd")}  {DateTime.Now.ToString("HH:mm")}"
                            };
                            SeiGyosyaCd = item.SeiGyosyaCd;
                            SeiCd = item.SeiCd;
                            SeiSitenCd = item.SeiSitenCd;
                            SeiRyakuNm = item.SeiRyakuNm;
                            SeiSitRyakuNm = item.SeiSitRyakuNm;
                            item.No = count;
                            count++;
                            billingListDetailReport.billingListDetailGrids.Add(item);
                            billingListDetailReports.Add(billingListDetailReport);
                        } else
                        {
                            BillingListDetailReport billingListDetailReport = billingListDetailReports.LastOrDefault();
                            if(billingListDetailReport != null)
                            {
                                item.No = count;
                                count++;
                                billingListDetailReport.billingListDetailGrids.Add(item);
                            }
                        }
                    }
                }
                return billingListDetailReports;
            }
        }
    }
}
