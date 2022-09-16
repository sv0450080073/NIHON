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
    public class GetCommonListItems : IRequest<ReceiptOuputCommonItems>
    {
        public ReceiptOutputFormSeachModel SearchModel { get; set; }
        public class Handler : IRequestHandler<GetCommonListItems, ReceiptOuputCommonItems>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<ReceiptOuputCommonItems> Handle(GetCommonListItems request, CancellationToken cancellationToken)
            {
                var tenantCdSeq = new ClaimModel().TenantID;
                var billAddressReceipt = new List<BillAddressReceipt>();
                var billOffices = (from e in _context.VpmEigyos
                                   join c in _context.VpmCompny
                                   on e.CompanyCdSeq equals c.CompanyCdSeq
                                   where c.SiyoKbn == 1
                                         && c.TenantCdSeq == tenantCdSeq
                                         && e.SiyoKbn == 1
                                   orderby e.EigyoCd
                                   select new BillOfficeReceipt
                                   {
                                       EigyoCd = e.EigyoCd,
                                       RyakuNm = e.RyakuNm,
                                       EigyoCdSeq = e.EigyoCdSeq,
                                       CompanyCd = c.CompanyCd
                                   }).ToList();

                string DateAsString = DateTime.Today.ToString("yyyyMMdd");
                var billingAddressFromTo = (from Tokisk in _context.VpmTokisk
                                            join TokiSt in _context.VpmTokiSt on Tokisk.TokuiSeq equals TokiSt.TokuiSeq
                                            join Gyosya in _context.VpmGyosya on Tokisk.GyosyaCdSeq equals Gyosya.GyosyaCdSeq
                                            where
                                              DateAsString.CompareTo(TokiSt.SiyoStaYmd) >= 0 &&
                                              DateAsString.CompareTo(TokiSt.SiyoEndYmd) <= 0 &&
                                              DateAsString.CompareTo(Tokisk.SiyoStaYmd) >= 0 &&
                                              DateAsString.CompareTo(Tokisk.SiyoEndYmd) <= 0 &&
                                              Gyosya.SiyoKbn == CommonConstants.SiyoKbn
                                              && Tokisk.TenantCdSeq == tenantCdSeq
                                            orderby Gyosya.GyosyaCd, Tokisk.TokuiCd, TokiSt.SitenCd
                                            select new BillAddressReceiptFromTo
                                            {
                                                GyosyaCd = Gyosya.GyosyaCd,
                                                RyakuNm = Tokisk.RyakuNm,
                                                SitenCd = TokiSt.SitenCd,
                                                TokuiCd = Tokisk.TokuiCd,
                                                SitenNm = TokiSt.SitenNm,
                                                TokuiCdSeq = Tokisk.TokuiSeq,
                                                SitenCdSeq = TokiSt.SitenCdSeq,
                                            }).ToList();

                var invoiceYearMonth = request?.SearchModel?.InvoiceYearMonth != null ? DateTime.ParseExact(request?.SearchModel?.InvoiceYearMonth?.ToString(), Formats._yyyyMMdd, null).ToString(Formats.yyyyMM) : "";
                var staInvoicingDate = request?.SearchModel?.StaInvoicingDate != null ? DateTime.ParseExact(request?.SearchModel?.StaInvoicingDate?.ToString(), Formats._yyyyMMdd, null).ToString(Formats.yyyyMMdd) : "";
                var endInvoicingDate = request?.SearchModel?.EndInvoicingDate != null ? DateTime.ParseExact(request?.SearchModel?.EndInvoicingDate?.ToString(), Formats._yyyyMMdd, null).ToString(Formats.yyyyMMdd) : "";
                var staInvoiceOutNum = !string.IsNullOrEmpty(request?.SearchModel?.StaInvoiceOutNum) ? string.Format("{0:D8}", Convert.ToInt32(request?.SearchModel?.StaInvoiceOutNum)) : "";
                var staInvoiceSerNum = !string.IsNullOrEmpty(request?.SearchModel?.StaInvoiceSerNum) ? string.Format("{0:D4}", Convert.ToInt32(request?.SearchModel?.StaInvoiceSerNum)) : "";
                var endInvoiceOutNum = !string.IsNullOrEmpty(request?.SearchModel?.EndInvoiceOutNum) ? string.Format("{0:D8}", Convert.ToInt32(request?.SearchModel?.EndInvoiceOutNum)) : "";
                var endInvoiceSerNum = !string.IsNullOrEmpty(request?.SearchModel?.EndInvoiceSerNum) ? string.Format("{0:D8}", Convert.ToInt32(request?.SearchModel?.EndInvoiceSerNum)) : "";

                _context.LoadStoredProc("PK_SpBillAddressReceipt_R")
                                .AddParam("@EigyoCdSeq", request.SearchModel?.BillOffice?.EigyoCdSeq.ToString() ?? "")
                                .AddParam("@SeikYm", invoiceYearMonth)
                                .AddParam("@StaInvoicingDate", staInvoicingDate)
                                .AddParam("@EndInvoicingDate", endInvoicingDate)
                                .AddParam("@TenantCdSeq", tenantCdSeq)
                                .AddParam("@StaInvoiceOutNum", staInvoiceOutNum)
                                .AddParam("@StaInvoiceSerNum", staInvoiceSerNum)
                                .AddParam("@EndInvoiceOutNum", endInvoiceOutNum)
                                .AddParam("@EndInvoiceSerNum", endInvoiceSerNum)
                                //.AddParam("@StaBillingAddress", request?.SearchModel?.BillAddressFrom?.Code.ToString() ?? "")
                                //.AddParam("@EndBillingAddress", request?.SearchModel?.BillAddressTo?.Code.ToString() ?? "")
                                .AddParam("@GyosyaFrom", request?.SearchModel?.CustomerModelFrom?.SelectedGyosya?.GyosyaCd ?? 0)
                                .AddParam("@GyosyaTo", request?.SearchModel?.CustomerModelTo?.SelectedGyosya?.GyosyaCd ?? 999)
                                .AddParam("@TokuiFrom", request?.SearchModel?.CustomerModelFrom?.SelectedTokisk?.TokuiCd ?? 0)
                                .AddParam("@TokuiTo", request?.SearchModel?.CustomerModelTo?.SelectedTokisk?.TokuiCd ?? 9999)
                                .AddParam("@SitenFrom", request?.SearchModel?.CustomerModelFrom?.SelectedTokiSt?.SitenCd ?? 0)
                                .AddParam("@SitenTo", request?.SearchModel?.CustomerModelTo?.SelectedTokiSt?.SitenCd ?? 9999)
                                .AddParam("@ROWCOUNT", out IOutParam<int> rowCount)
                                .Exec(r => billAddressReceipt = r.ToList<BillAddressReceipt>());

                return new ReceiptOuputCommonItems()
                {
                    BillAddressReceiptFromTos = billingAddressFromTo,
                    BillOfficeReceipts = billOffices,
                    BillAddressReceipts = billAddressReceipt,
                };
            }
        }
    }
}
