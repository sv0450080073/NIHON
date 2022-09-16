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
    public class GetCustomersByTenantIdQuery : IRequest<List<LoadCustomerList>>
    {
        public int TenantId { get; set; }

        public GetCustomersByTenantIdQuery(int tenantId)
        {
            TenantId = tenantId;
        }

        public class Handler : IRequestHandler<GetCustomersByTenantIdQuery, List<LoadCustomerList>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetCustomersByTenantIdQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetCustomersByTenantIdQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<List<LoadCustomerList>> Handle(GetCustomersByTenantIdQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    string DateAsString = DateTime.Today.ToString("yyyyMMdd");
                    var result = await (from t in _context.VpmTokisk
                                        join s in _context.VpmTokiSt
                                        on t.TokuiSeq equals s.TokuiSeq
                                        into lj
                                        from subTo in lj.DefaultIfEmpty()
                                        join second in _context.VpmTokiSt
                                        on new { first = subTo.SeiSitenCdSeq, second = subTo.TokuiSeq } equals new { first = second.SitenCdSeq, second = second.TokuiSeq }
                                        into ls
                                        from secondTo in ls.DefaultIfEmpty()
                                        where DateAsString.CompareTo(subTo.SiyoStaYmd) >= 0 &&
                                              DateAsString.CompareTo(subTo.SiyoEndYmd) <= 0
                                              && t.TenantCdSeq == request.TenantId
                                        orderby t.TokuiCd ascending
                                        select new LoadCustomerList()
                                        {
                                            TokuiSeq = t.TokuiSeq,
                                            SitenCdSeq = subTo.SitenCdSeq,
                                            TokuiCd = t.TokuiCd,
                                            RyakuNm = t.RyakuNm,
                                            SitenCd = subTo.SitenCd,
                                            SitenNm = subTo.SitenNm,
                                            TesuRitu = subTo.TesuRitu,
                                            TesuRituGui = subTo.TesuRituGui,
                                            SimeD = secondTo.SimeD,
                                            SitenRyakuNm = subTo.RyakuNm,
                                            TokuiTel=subTo.TelNo,
                                            TokuiFax=subTo.FaxNo,
                                            TokuiMail=subTo.TokuiMail,
                                            TokuiTanNm=subTo.TokuiTanNm
                                        }).Distinct().ToListAsync();
                    return result;
                }
                catch(Exception ex)
                {
                    _logger.LogTrace(ex.ToString());

                    return new List<LoadCustomerList>();
                }
            }
        }
    }
}
