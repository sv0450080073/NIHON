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
    public class GetStaffByTenantIdQuery : IRequest<List<LoadStaff>>
    {
        private readonly int _tenantId;
        private readonly string _startDate;
        private readonly string _endDate;

        public GetStaffByTenantIdQuery(int tenantId, string startDate, string endDate)
        {
            _tenantId = tenantId;
            _startDate = startDate ?? throw new ArgumentNullException(startDate);
            _endDate = endDate ?? throw new ArgumentNullException(endDate); 
        }

        public class Handler : IRequestHandler<GetStaffByTenantIdQuery, List<LoadStaff>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetStaffByTenantIdQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetStaffByTenantIdQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<List<LoadStaff>> Handle(GetStaffByTenantIdQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    await Task.FromResult(0);
                    var result = ((from s in _context.VpmSyain
                                  join h in _context.VpmKyoShe on s.SyainCdSeq equals h.SyainCdSeq into grsh
                                  from sh in grsh.DefaultIfEmpty()
                                  join e in _context.VpmEigyos on sh.EigyoCdSeq equals e.EigyoCdSeq into grshe
                                  from she in grshe.DefaultIfEmpty()
                                  join c in _context.VpmCompny on she.CompanyCdSeq equals c.CompanyCdSeq into grshec
                                  from shec in grshec.DefaultIfEmpty()
                                  where shec.TenantCdSeq == request._tenantId
                                            && sh.StaYmd.CompareTo(request._startDate) <= 0 && sh.EndYmd.CompareTo(request._endDate) >= 0
                                  select new LoadStaff()
                                  {
                                      SyainCdSeq = s.SyainCdSeq,
                                      SyainCd = Convert.ToInt64(s.SyainCd).ToString("D10"),
                                      SyainNm = s.SyainNm,
                                      EigyoCdSeq = sh.EigyoCdSeq,
                                      EigyoCd = she.EigyoCd,
                                      TenkoNo = sh.TenkoNo,
                                      EndYmd = DateTime.ParseExact(sh.EndYmd, "yyyyMMdd", null),
                                      StaYmd = DateTime.ParseExact(sh.StaYmd, "yyyyMMdd", null)
                                  }).ToList().OrderBy(_ => _.SyainCd).ToList()).ToList();
                    return await Task.FromResult(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);

                    return await Task.FromResult(new List<LoadStaff>());
                }
            }
        }
    }
}
