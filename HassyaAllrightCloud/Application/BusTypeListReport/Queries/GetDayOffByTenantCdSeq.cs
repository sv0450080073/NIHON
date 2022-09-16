using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BusTypeListReport.Queries
{
    public class GetDayOffByTenantCdSeq : IRequest<List<CalendarReport>>
    {
        public int CountryCdSeq;
        public int TenantCdSeq;
        public List<BusTypeItemDataReport> CompanyList { get; set; }
        public class Handler : IRequestHandler<GetDayOffByTenantCdSeq, List<CalendarReport>>
        {         
            private readonly KobodbContext _context;
            private readonly ILogger<GetDayOffByTenantCdSeq> _logger;
            public Handler(KobodbContext context, ILogger<GetDayOffByTenantCdSeq> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<List<CalendarReport>> Handle(GetDayOffByTenantCdSeq request, CancellationToken cancellationToken)
            {
                var result = new List<CalendarReport>();
                try
                {
                    result = (from Calendar in _context.VpmCalend
                              where Calendar.CalenKbn == 3
                              && Calendar.CountryCdSeq == request.CountryCdSeq
                              && (Calendar.TenantCdSeq == request.TenantCdSeq 
                              || Calendar.TenantCdSeq == 0)
                              && (request.CompanyList.Select(x=>x.CompanyCdSeq).ToArray().Contains(Calendar.CompanyCdSeq)
                              || Calendar.CompanyCdSeq ==0)
                              select new CalendarReport()
                              {
                                  CalenCom = Calendar.CalenCom,
                                  CalenYmd = Calendar.CalenYmd,
                                   CountryCdSeq= Calendar.CountryCdSeq,
                                  CompanyCdSeq = Calendar.CompanyCdSeq,
                                  TenantCdSeq =  Calendar.TenantCdSeq
                              }).ToList();
                    return result;
                }
                catch(Exception ex)
                {
                    return result;
                }
            }
        }
    }
}
