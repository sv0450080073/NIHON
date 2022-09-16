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
    public class GetStaffByStaffIdQuery : IRequest<List<LoadStaff>>
    {
        private int _tenantId;
        private int _staffId;

        public GetStaffByStaffIdQuery(int staffId, int tenantId)
        {
            if (tenantId < 0)
                throw new ArgumentOutOfRangeException(nameof(tenantId));
            if (staffId < 0)
                throw new ArgumentOutOfRangeException(nameof(staffId));

            _tenantId = tenantId;
            _staffId = staffId;
        }

        public class Handler : IRequestHandler<GetStaffByStaffIdQuery, List<LoadStaff>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetStaffByStaffIdQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetStaffByStaffIdQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<List<LoadStaff>> Handle(GetStaffByStaffIdQuery request, CancellationToken cancellationToken)
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
                                            && s.SyainCdSeq == request._staffId
                                  select new LoadStaff()
                                  {
                                      SyainCdSeq = s.SyainCdSeq,
                                      SyainCd = s.SyainCd,
                                      SyainNm = s.SyainNm,
                                      EigyoCdSeq = sh.EigyoCdSeq,
                                      EigyoCd = she.EigyoCd,
                                      TenkoNo = sh.TenkoNo,
                                      EndYmd = DateTime.ParseExact(sh.EndYmd, "yyyyMMdd", null),
                                      StaYmd = DateTime.ParseExact(sh.StaYmd, "yyyyMMdd", null),
                                      CompanyName = shec.CompanyNm,
                                  }).ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());

                    return new List<LoadStaff>();
                }
                
            }
        }
    }
}
