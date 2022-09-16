using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.ETC.Queries
{
    public class GetListSyaRyoForSearchQuery : IRequest<List<ETCSyaRyo>>
    {
        public int TenantCdSeq { get; set; }
        public class Handler : IRequestHandler<GetListSyaRyoForSearchQuery, List<ETCSyaRyo>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<ETCSyaRyo>> Handle(GetListSyaRyoForSearchQuery request, CancellationToken cancellationToken)
            {
                var currentDate = DateTime.Now.ToString(CommonConstants.FormatYMD);

                var listSyaRyo = await (from s in _context.VpmSyaRyo
                                        join h in _context.VpmHenSya.Where(_ => _.EndYmd.CompareTo(currentDate) > -1)
                                        on s.SyaRyoCdSeq equals h.SyaRyoCdSeq
                                        join e in _context.VpmEigyos.Where(_ => _.SiyoKbn == Constants.SiyoKbn)
                                        on h.EigyoCdSeq equals e.EigyoCdSeq
                                        join c in _context.VpmCompny.Where(_ => _.SiyoKbn == Constants.SiyoKbn)
                                        on e.CompanyCdSeq equals c.CompanyCdSeq
                                        join t in _context.VpmTenant.Where(_ => _.SiyoKbn == Constants.SiyoKbn && _.TenantCdSeq == request.TenantCdSeq)
                                        on c.TenantCdSeq equals t.TenantCdSeq
                                        select new ETCSyaRyo()
                                        {
                                            CompanyCdSeq = c.CompanyCdSeq,
                                            EigyoCdSeq = e.EigyoCdSeq,
                                            SyaRyoCdSeq = s.SyaRyoCdSeq,
                                            SyaRyoCd = s.SyaRyoCd,
                                            SyaRyoNm = s.SyaRyoNm
                                        }).ToListAsync();

                return listSyaRyo;
            }
        }
    }
}
