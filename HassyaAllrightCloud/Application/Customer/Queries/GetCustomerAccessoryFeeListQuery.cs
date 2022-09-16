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
    public class GetCustomerAccessoryFeeListQuery : IRequest<List<LoadCustomerList>>
    {
        private readonly int _tenantId;
        private readonly string _fromDate;
        private readonly string _toDate;

        public GetCustomerAccessoryFeeListQuery(int tenantId, string fromDate, string toDate)
        {
            _tenantId = tenantId;
            _fromDate = fromDate;
            _toDate = toDate;
        }

        public class Handler : IRequestHandler<GetCustomerAccessoryFeeListQuery, List<LoadCustomerList>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetCustomerAccessoryFeeListQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetCustomerAccessoryFeeListQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<List<LoadCustomerList>> Handle(GetCustomerAccessoryFeeListQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var data = await (from TOKIST in _context.VpmTokiSt
                                      join TOKISK in _context.VpmTokisk
                                      on TOKIST.TokuiSeq equals TOKISK.TokuiSeq into TOKISK_join
                                      from TOKISK in TOKISK_join.DefaultIfEmpty()
                                      where
                                          String.Compare(TOKIST.SiyoEndYmd, request._fromDate) >= 0
                                          && String.Compare(TOKIST.SiyoStaYmd, request._toDate) <= 0
                                          && String.Compare(TOKISK.SiyoEndYmd, request._fromDate) >= 0
                                          && String.Compare(TOKISK.SiyoStaYmd, request._toDate) <= 0
                                          && TOKISK.TenantCdSeq == request._tenantId
                                      orderby TOKISK.TokuiCd, TOKIST.SitenCd
                                      select new LoadCustomerList()
                                      {
                                          TokuiSeq = TOKISK.TokuiSeq,
                                          TokuiCd = TOKISK.TokuiCd,
                                          TOKISK_TokuiNm = TOKISK.TokuiNm,
                                          RyakuNm = TOKISK.RyakuNm,
                                          SitenCdSeq = TOKIST.SitenCdSeq,
                                          SitenCd = TOKIST.SitenCd,
                                          SitenRyakuNm = TOKIST.RyakuNm
                                      }).ToListAsync();
                    return data;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    return new List<LoadCustomerList>();
                }
            }
        }
    }
}