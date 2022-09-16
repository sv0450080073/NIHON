using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.IService;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BookingInput.Queries
{
    public class GetInvoiceTypeQuery : IRequest<IEnumerable<InvoiceType>>
    {
        public int TenantId { get; set; }
        public class Handler : IRequestHandler<GetInvoiceTypeQuery, IEnumerable<InvoiceType>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetInvoiceTypeQuery> _logger;
            private readonly ITPM_CodeSyService _codeSyuService;

            public Handler(KobodbContext context, ILogger<GetInvoiceTypeQuery> logger, ITPM_CodeSyService codeSyuService)
            {
                _context = context;
                _logger = logger;
                _codeSyuService = codeSyuService ?? throw new ArgumentNullException(nameof(codeSyuService));
            }

            public async Task<IEnumerable<InvoiceType>> Handle(GetInvoiceTypeQuery request, CancellationToken cancellationToken)
            {
                var result = new List<InvoiceType>();
                try
                {
                    result = await
                        _codeSyuService.FilterTenantIdByCodeSyu((tenantId, codeSyu) =>
                        {
                            return
                                (from s in _context.VpmCodeKb
                                 where s.CodeSyu.Contains(codeSyu) && s.TenantCdSeq == tenantId
                                 select new InvoiceType
                                 {
                                     CodeKbnSeq = s.CodeKbnSeq,
                                     CodeSyu = s.CodeSyu,
                                     CodeKbn = s.CodeKbn,
                                     CodeKbnNm = s.CodeKbnNm,
                                     RyakuNm = s.RyakuNm,
                                     CodeSeiKbn = s.CodeSeiKbn,
                                     SiyoKbn = s.SiyoKbn
                                 }).ToListAsync();
                        }, request.TenantId, "SEIKYUKBN");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return null;
                }
                return result;
            }
        }
    }
}
