using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DevExpress.Blazor.Internal;

namespace HassyaAllrightCloud.Application.BookingInput.Queries
{
    public class GetSupplierDataQuery : IRequest<IEnumerable<SupplierData>>
    {
        public int TenantId { get; set; }
        public class Handler : IRequestHandler<GetSupplierDataQuery, IEnumerable<SupplierData>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetSupplierDataQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetSupplierDataQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<IEnumerable<SupplierData>> Handle(GetSupplierDataQuery request, CancellationToken cancellationToken)
            {
                var result = new List<SupplierData>();
                try
                {
                    string dateAsString = DateTime.Today.ToString("yyyyMMdd");
                    result = await (from t in _context.VpmTokisk
                                    where t.TenantCdSeq == request.TenantId
                                    join s in _context.VpmTokiSt
                                        on t.TokuiSeq equals s.TokuiSeq
                                    where dateAsString.CompareTo(s.SiyoStaYmd) >= 0 &&
                                            dateAsString.CompareTo(s.SiyoEndYmd) <= 0
                                    orderby t.TokuiCd ascending
                                    select new SupplierData()
                                    {
                                        SirCdSeq = t.TokuiSeq,
                                        SirSitenCdSeq = s.SitenCdSeq,

                                        TokuiCd = t.TokuiCd,
                                        SitenCd = s.SitenCd,
                                        RyakuNm = t.RyakuNm,
                                        SitenNm = s.SitenNm
                                    }).ToListAsync();
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
