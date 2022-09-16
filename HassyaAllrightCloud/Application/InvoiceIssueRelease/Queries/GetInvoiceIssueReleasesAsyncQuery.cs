using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.InvoiceIssueRelease;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using HassyaAllrightCloud.Commons.Extensions;

namespace HassyaAllrightCloud.Application.BillPrint.Queries
{
    public class GetInvoiceIssueReleasesAsyncQuery : IRequest<List<InvoiceIssueGrid>>
    {
        public InvoiceIssueFilter invoiceIssueFilter { get; set; }
        public class Handler : IRequestHandler<GetInvoiceIssueReleasesAsyncQuery, List<InvoiceIssueGrid>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<InvoiceIssueGrid>> Handle(GetInvoiceIssueReleasesAsyncQuery request, CancellationToken cancellationToken = default)
            {
                if (request.invoiceIssueFilter == null)
                {
                    return new List<InvoiceIssueGrid>();
                }
                List<InvoiceIssueGrid> rows = new List<InvoiceIssueGrid>();

                InvoiceIssueFilter invoiceIssueFilter = new InvoiceIssueFilter()
                {
                    StartBillAddressString = $"{(request.invoiceIssueFilter.SelectedGyosyaFrom?.GyosyaCd ?? 0):000}{(request.invoiceIssueFilter.SelectedTokiskFrom?.TokuiCd ?? 0):0000}{(request.invoiceIssueFilter.SelectedTokiStFrom?.SitenCd ?? 0):0000}",//request?.invoiceIssueFilter?.StartBillAddress?.GyoSyaCd.ToString("D3") + request?.invoiceIssueFilter?.StartBillAddress?.TokuiCd.ToString("D4") + request?.invoiceIssueFilter?.StartBillAddress?.SitenCd.ToString("D4"),
                    EndBillAddressString = $"{(request.invoiceIssueFilter.SelectedGyosyaTo?.GyosyaCd ?? 999):000}{(request.invoiceIssueFilter.SelectedTokiskTo?.TokuiCd ?? 9999):0000}{(request.invoiceIssueFilter.SelectedTokiStTo?.SitenCd ?? 9999):0000}",//request?.invoiceIssueFilter?.EndBillAddress?.GyoSyaCd.ToString("D3") + request?.invoiceIssueFilter?.EndBillAddress?.TokuiCd.ToString("D4") + request?.invoiceIssueFilter?.EndBillAddress?.SitenCd.ToString("D4"),
                    StartBillIssuedDateString = request?.invoiceIssueFilter?.StartBillIssuedDate == null ? null : ((DateTime)request?.invoiceIssueFilter?.StartBillIssuedDate).ToString("yyyyMMdd"),
                    EndBillIssuedDateString = request?.invoiceIssueFilter?.EndBillIssuedDate == null ? null : ((DateTime)request?.invoiceIssueFilter?.EndBillIssuedDate).ToString("yyyyMMdd"),
                    Offset = request.invoiceIssueFilter.Offset,
                    Limit = request.invoiceIssueFilter.Limit,
                    BillOutputSeq =  request.invoiceIssueFilter.BillOutputSeq,
                    BillSerialNumber = request.invoiceIssueFilter.BillSerialNumber
                };
                int tenantId = new ClaimModel().TenantID;
                await _context.LoadStoredProc("PK_dInvoiceIssueRelease_R")
                                .AddParam("@TenantCdSeq", tenantId)
                                .AddParam("@BillOutputSeq", invoiceIssueFilter.BillOutputSeq)
                                .AddParam("@BillSerialNumber", invoiceIssueFilter.BillSerialNumber)
                                .AddParam("@StartBillIssuedDate", invoiceIssueFilter.StartBillIssuedDateString)
                                .AddParam("@EndBillIssuedDate", invoiceIssueFilter.EndBillIssuedDateString)
                                .AddParam("@StartBillAddress", invoiceIssueFilter.StartBillAddressString)
                                .AddParam("@EndBillAddress", invoiceIssueFilter.EndBillAddressString)
                                .AddParam("@Offset", invoiceIssueFilter.Offset)
                                .AddParam("@Limit", invoiceIssueFilter.Limit)
                                .AddParam("ROWCOUNT", out IOutParam<int> rowCount)
                                .ExecAsync(async r => rows = await r.ToListAsync<InvoiceIssueGrid>());
                return rows;
            }
        }
    }
}