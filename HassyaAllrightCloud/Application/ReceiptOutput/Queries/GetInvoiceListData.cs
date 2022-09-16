using DevExpress.Blazor.Internal;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.ReceiptOutput.Queries
{
    public class GetInvoiceListData : IRequest<List<Invoice>>
    {
        public ReceiptOutputFormSeachModel SearchModel { get; set; }
        public class Handler : IRequestHandler<GetInvoiceListData, List<Invoice>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<Invoice>> Handle(GetInvoiceListData request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = new List<InvoicesListResult>();
                    int index = 1;

                    var staInvoicingDate = request?.SearchModel?.StaInvoicingDate != null ? DateTime.ParseExact(request?.SearchModel?.StaInvoicingDate?.ToString(), Formats._yyyyMMdd, null).ToString(Formats.yyyyMMdd) : "";
                    var endInvoicingDate = request?.SearchModel?.EndInvoicingDate != null ? DateTime.ParseExact(request?.SearchModel?.EndInvoicingDate?.ToString(), Formats._yyyyMMdd, null).ToString(Formats.yyyyMMdd) : "";
                    var staInvoiceOutNum = !string.IsNullOrEmpty(request?.SearchModel?.StaInvoiceOutNum) ? string.Format("{0:D8}", Convert.ToInt32(request?.SearchModel?.StaInvoiceOutNum)) : "";
                    var staInvoiceSerNum = !string.IsNullOrEmpty(request?.SearchModel?.StaInvoiceSerNum) ? string.Format("{0:D4}", Convert.ToInt32(request?.SearchModel?.StaInvoiceSerNum)) : "";
                    var endInvoiceOutNum = !string.IsNullOrEmpty(request?.SearchModel?.EndInvoiceOutNum) ? string.Format("{0:D8}", Convert.ToInt32(request?.SearchModel?.EndInvoiceOutNum)) : "";
                    var endInvoiceSerNum = !string.IsNullOrEmpty(request?.SearchModel?.EndInvoiceSerNum) ? string.Format("{0:D8}", Convert.ToInt32(request?.SearchModel?.EndInvoiceSerNum)) : "";
                    var invoiceYearMonth = request?.SearchModel?.InvoiceYearMonth != null ? DateTime.ParseExact(request?.SearchModel?.InvoiceYearMonth?.ToString(), Formats._yyyyMMdd, null).ToString(Formats.yyyyMM) : "";

                    var storedBuilder = _context.LoadStoredProc("PK_SpInvoice_R");
                    await storedBuilder.AddParam("@EigyoCdSeq", request.SearchModel?.BillOffice?.EigyoCdSeq.ToString() ?? "")
                                       .AddParam("@SeikYm", invoiceYearMonth)
                                       .AddParam("@StaInvoicingDate", staInvoicingDate)
                                       .AddParam("@EndInvoicingDate", endInvoicingDate)
                                       .AddParam("@TenantCdSeq", new ClaimModel().TenantID)
                                       .AddParam("@StaInvoiceOutNum", staInvoiceOutNum)
                                       .AddParam("@StaInvoiceSerNum", staInvoiceSerNum)
                                       .AddParam("@EndInvoiceOutNum", endInvoiceOutNum)
                                       .AddParam("@EndInvoiceSerNum", endInvoiceSerNum)
                                       .AddParam("@GyosyaCd", request?.SearchModel?.BillAddressReceipt?.GyosyaCd.ToString() ?? "0")
                                       .AddParam("@TokuiCd", request?.SearchModel?.BillAddressReceipt?.TokuiCd.ToString() ?? "0")
                                       .AddParam("@SitenCd", request?.SearchModel?.BillAddressReceipt?.SitenCd.ToString() ?? "0")
                        .AddParam("@ROWCOUNT", out IOutParam<int> retParam).ExecAsync(async r => result = await r.ToListAsync<InvoicesListResult>(cancellationToken));

                    return result.Select(x => new Invoice
                    {
                        ListNo = index++,
                        ListInvoiceNo = $"{x.SeiOutSeq.ToString("D8")}-{x.SeiRen.ToString("D4")}",
                        ListBillingOffice = x.SeiEigEigyoNm,
                        ListBillingAddress = $"{x.TokuiNm}　{x.SitenNm}",
                        ListInvoiceYearMonth = !string.IsNullOrEmpty(x.SeikYm.Trim()) ? DateTime.ParseExact(x.SeikYm, Formats.yyyyMM, null).ToString(Formats._yyyyMM) : "",
                        PreviousCarryAmount = x.ZenKurG.ToString("#,##0"),
                        ThisAmount = x.KonUriG.ToString("#,##0"),
                        ThisTaxAmount = x.KonSyoG.ToString("#,##0"),
                        ThisFeeAmount = x.KonTesG.ToString("#,##0"),
                        ThisDeposit = x.KonNyuG.ToString("#,##0"),
                        ThisBillingAmount = x.KonSeiG.ToString("#,##0"),
                        InvoiceDate = !string.IsNullOrEmpty(x.SeiHatYmd.Trim()) ? DateTime.ParseExact(x.SeiHatYmd, Formats.yyyyMMdd, null).ToString(Formats.SlashyyyyMMdd) : "",
                        SeiOutSeq = x.SeiOutSeq,
                        SeiRen = x.SeiRen
                    }).ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
