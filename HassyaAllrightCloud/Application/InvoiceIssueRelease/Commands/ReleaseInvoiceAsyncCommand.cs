using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.InvoiceIssueRelease;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BillPrint.Queries
{
    public class ReleaseInvoiceAsyncCommand : IRequest<string>
    {
        public List<InvoiceIssueGrid> invoiceIssueGrids { get; set; } = new List<InvoiceIssueGrid>();
        public class Handler : IRequestHandler<ReleaseInvoiceAsyncCommand, string>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<ReleaseInvoiceAsyncCommand> _logger;

            public Handler(KobodbContext context, ILogger<ReleaseInvoiceAsyncCommand> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<string> Handle(ReleaseInvoiceAsyncCommand request, CancellationToken cancellationToken = default)
            {
                var tkdSeiKyus = new List<TkdSeikyu>();
                foreach (var item in request.invoiceIssueGrids)
                {
                    var tkdSeikyu = _context.TkdSeikyu.Where(x => x.SeiOutSeq == item.SeiOutSeq && x.SeiRen == item.SeiRen && x.TenantCdSeq == new ClaimModel().TenantID).FirstOrDefault();
                    if (tkdSeikyu != null)
                    {
                        tkdSeikyu.SiyoKbn = 2;
                        tkdSeikyu.UpdYmd = DateTime.Now.ToString(Commons.Constants.Formats.yyyyMMdd);
                        tkdSeikyu.UpdTime = DateTime.Now.ToString(Commons.Constants.Formats.HHmmss);
                        tkdSeikyu.UpdPrgId = Commons.Constants.Common.UpdPrgId;
                        tkdSeikyu.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                        tkdSeiKyus.Add(tkdSeikyu);
                    }
                }
                _context.UpdateRange(tkdSeiKyus);
                _context.SaveChanges();
                return null;
            }
        }
    }
}
