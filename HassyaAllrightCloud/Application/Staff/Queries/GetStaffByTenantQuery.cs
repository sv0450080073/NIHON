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

namespace HassyaAllrightCloud.Application.Staff.Queries
{
    public class GetStaffByTenantQuery : IRequest<IEnumerable<LoadStaff>>
    {
        private readonly int _tenantId;

        public GetStaffByTenantQuery(int tenantId)
        {
            _tenantId = tenantId;
        }

        public class Handler : IRequestHandler<GetStaffByTenantQuery, IEnumerable<LoadStaff>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetStaffByTenantQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetStaffByTenantQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<IEnumerable<LoadStaff>> Handle(GetStaffByTenantQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    string todayString = DateTime.Today.ToString("yyyyMMdd");
                    return await (from s in _context.VpmSyain
                                  join h in _context.VpmKyoShe on s.SyainCdSeq equals h.SyainCdSeq into grsh
                                  from sh in grsh.DefaultIfEmpty()
                                  join e in _context.VpmEigyos on sh.EigyoCdSeq equals e.EigyoCdSeq into grshe
                                  from she in grshe.DefaultIfEmpty()
                                  join c in _context.VpmCompny on she.CompanyCdSeq equals c.CompanyCdSeq into grshec
                                  from shec in grshec.DefaultIfEmpty()
                                  join t in _context.VpmTenant on shec.TenantCdSeq equals t.TenantCdSeq into grshect
                                  from shect in grshect.DefaultIfEmpty()
                                  where shect.TenantCdSeq == request._tenantId
                                            && sh.StaYmd.CompareTo(todayString) <= 0 && sh.EndYmd.CompareTo(todayString) >= 0
                                    orderby she.EigyoCd, s.SyainCd
                                  select new LoadStaff()
                                  {
                                      SyainCdSeq = s.SyainCdSeq,
                                      SyainCd = s.SyainCd,
                                      SyainNm = s.SyainNm,
                                      EigyoCdSeq = sh.EigyoCdSeq,
                                      EigyoCd = she.EigyoCd,
                                      TenkoNo = sh.TenkoNo,
                                      EndYmd = DateTime.ParseExact(sh.EndYmd, "yyyyMMdd", null),
                                      StaYmd = DateTime.ParseExact(sh.StaYmd, "yyyyMMdd", null)
                                  }).ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    throw;
                }
            }
        }
    }
}
