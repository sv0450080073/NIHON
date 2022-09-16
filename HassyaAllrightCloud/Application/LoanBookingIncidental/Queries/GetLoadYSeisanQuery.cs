using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HassyaAllrightCloud.IService;

namespace HassyaAllrightCloud.Application.LoanBookingIncidental.Queries
{
    public class GetLoadYSeisanQuery : IRequest<List<LoadYSeisan>>
    {
        private readonly int _tenantId;

        public GetLoadYSeisanQuery(int tenantId)
        {
            _tenantId = tenantId;
        }

        public class Handler : IRequestHandler<GetLoadYSeisanQuery, List<LoadYSeisan>>
        {
            private readonly KobodbContext _context;
            private readonly ITPM_CodeSyService _codeSyuService;

            public Handler(KobodbContext context, ITPM_CodeSyService codeSyuService)
            {
                _context = context;
                _codeSyuService = codeSyuService;
            }

            public async Task<List<LoadYSeisan>> Handle(GetLoadYSeisanQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    //var result = await
                    //    _codeSyuService.FilterTenantIdByCodeSyu((tenantId, codeSyu) =>
                    //    {
                    //        return 
                    //            (from seisan in _context.VpmSeisan
                    //                    join codekb in (_context.VpmCodeKb.Where(c => c.TenantCdSeq == tenantId && c.CodeSyu == codeSyu))
                    //                        on new { a = seisan.SeisanKbn } equals new { a = Convert.ToByte(codekb.CodeKbn) }
                    //                    where seisan.SiyoKbn == 1
                    //                    select new LoadYSeisan
                    //                    {
                    //                        SeisanCd = seisan.SeisanCd,
                    //                        SeisanCdSeq = seisan.SeisanCdSeq,
                    //                        SeisanKbn = seisan.SeisanKbn,
                    //                        SeisanNm = seisan.SeisanNm,
                    //                        CodeKbRyakuNm = codekb.RyakuNm
                    //                    }).OrderBy(c => c.SeisanCd).ToListAsync();
                    //    }, request._tenantId, "SeisanKbn");
                    var _tenantId = new ClaimModel().TenantID;
                    var tenantId = _context.VpmCodeKb.Where(_ => _.CodeSyu == "SeisanKbn" && _.SiyoKbn == 1
                                   && _.TenantCdSeq == _tenantId).Count() == 0 ? 0 : _tenantId;

                    return await (from seisan in _context.VpmSeisan
                                  join codekb in _context.VpmCodeKb on "SeisanKbn" equals codekb.CodeSyu into codekbTpm
                                  from codekb in codekbTpm.DefaultIfEmpty()
                                  where codekb.CodeKbn == seisan.SeisanKbn.ToString() && seisan.SiyoKbn == 1
                                  && seisan.TenantCdSeq == _tenantId && codekb.TenantCdSeq == tenantId
                                  select new LoadYSeisan
                                  {
                                      SeisanCd = seisan.SeisanCd,
                                      SeisanCdSeq = seisan.SeisanCdSeq,
                                      SeisanKbn = seisan.SeisanKbn,
                                      SeisanNm = seisan.SeisanNm,
                                      CodeKbRyakuNm = codekb.RyakuNm
                                  }).OrderBy(c => c.SeisanCd).ToListAsync();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
