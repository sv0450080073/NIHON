using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace HassyaAllrightCloud.Application.BookingInput.Queries
{
    public class GetSettingStaffQuery : IRequest<IEnumerable<SettingStaff>>
    {
        public int TenantId { get; set; }
        public class Handler : IRequestHandler<GetSettingStaffQuery, IEnumerable<SettingStaff>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetSettingStaffQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetSettingStaffQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<IEnumerable<SettingStaff>> Handle(GetSettingStaffQuery request, CancellationToken cancellationToken)
            {
                var result = new List<SettingStaff>();
                try
                {
                    string dateAsString = DateTime.Today.ToString("yyyyMMdd");
                    result =  await (from s in _context.VpmSyain
                                    join h in _context.VpmKyoShe on s.SyainCdSeq equals h.SyainCdSeq into gbh from sgbh in gbh.DefaultIfEmpty()
                                    join e in _context.VpmEigyos on sgbh.EigyoCdSeq equals e.EigyoCdSeq into gbe from sgbe in gbe.DefaultIfEmpty()
                                    join c in _context.VpmCompny on sgbe.CompanyCdSeq equals c.CompanyCdSeq into gbc from sgbc in gbc.DefaultIfEmpty()
                                    join t in _context.VpmTenant on sgbc.TenantCdSeq equals t.TenantCdSeq into gbt from sgbt in gbt.DefaultIfEmpty()
                                    where sgbt.TenantCdSeq == request.TenantId
                                        && dateAsString.CompareTo(sgbh.StaYmd) >= 0
                                        && dateAsString.CompareTo(sgbh.EndYmd) <= 0
                                    select new SettingStaff()
                                    {
                                        EigyoCdSeq = sgbh.EigyoCdSeq,
                                        EndYmd = sgbh.EndYmd,
                                        StaYmd = sgbh.StaYmd,
                                        SyainCd = s.SyainCd,
                                        SyainCdSeq = s.SyainCdSeq,
                                        SyainNm = s.SyainNm,
                                        TenkoNo = sgbh.TenkoNo
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
