using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.Customer.Queries
{
    public class GetCustomersWithHaisFromToQuery : IRequest<List<LoadCustomerList>>
    {
        private readonly int _tenantId;
        private readonly string _haisDateFrom;
        private readonly string _haisDateTo;

        public GetCustomersWithHaisFromToQuery(int tenantId, string haisDateFrom, string haisDateTo)
        {
            _tenantId = tenantId;
            _haisDateFrom = haisDateFrom;
            _haisDateTo = haisDateTo;
        }

        public class Handler : IRequestHandler<GetCustomersWithHaisFromToQuery, List<LoadCustomerList>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetCustomersWithHaisFromToQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetCustomersWithHaisFromToQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<List<LoadCustomerList>> Handle(GetCustomersWithHaisFromToQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await (from TOKIST in _context.VpmTokiSt
                                        join TOKISK in _context.VpmTokisk on TOKIST.TokuiSeq equals TOKISK.TokuiSeq into gr
                                        from TOKISKGr in gr.DefaultIfEmpty()
                                        where ((string.Compare(TOKIST.SiyoStaYmd, request._haisDateFrom) >= 0 && string.Compare(TOKIST.SiyoStaYmd, request._haisDateTo) <= 0)
                                                || (string.Compare(TOKIST.SiyoEndYmd, request._haisDateFrom) >= 0 && string.Compare(TOKIST.SiyoEndYmd, request._haisDateTo) <= 0)
                                                || (string.Compare(TOKIST.SiyoStaYmd, request._haisDateFrom) <= 0 && string.Compare(TOKIST.SiyoEndYmd, request._haisDateTo) >= 0))
                                              && ((string.Compare(TOKISKGr.SiyoStaYmd, request._haisDateFrom) >= 0 && string.Compare(TOKISKGr.SiyoStaYmd, request._haisDateTo) <= 0)
                                                || (string.Compare(TOKISKGr.SiyoEndYmd, request._haisDateFrom) >= 0 && string.Compare(TOKISKGr.SiyoEndYmd, request._haisDateTo) <= 0)
                                                || (string.Compare(TOKISKGr.SiyoStaYmd, request._haisDateFrom) <= 0 && string.Compare(TOKISKGr.SiyoEndYmd, request._haisDateTo) >= 0))
                                              && TOKISKGr.TenantCdSeq == request._tenantId
                                        select new LoadCustomerList()
                                        {
                                            TokuiSeq = TOKISKGr.TokuiSeq,
                                            TokuiCd = TOKISKGr.TokuiCd,
                                            TOKISK_TokuiNm = TOKISKGr.TokuiNm,
                                            SitenCdSeq = TOKIST.SitenCdSeq,
                                            SitenCd = TOKIST.SitenCd,
                                            SitenRyakuNm = TOKIST.RyakuNm,
                                            SitenNm = TOKIST.SitenNm,
                                        }).Distinct().OrderBy(_ => _.TokuiCd).ThenBy(_ => _.SitenCd).ToListAsync();
                    return result;
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    throw;
                }
            }
        }
    }
}
