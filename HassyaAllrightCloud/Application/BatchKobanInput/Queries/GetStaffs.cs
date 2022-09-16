using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BatchKobanInput.Queries
{
    public class GetStaffs : IRequest<List<Staffs>>
    {
        public int TenantCdSeq { get; set; }
        public int CompanyCdSeq { get; set; }
        public BatchKobanInputFilterModel Model { get; set; }
        public class Handler : IRequestHandler<GetStaffs, List<Staffs>>
        {
            private readonly KobodbContext _dbcontext;

            public Handler(KobodbContext context)
            {
                _dbcontext = context;
            }

            public async Task<List<Staffs>> Handle(GetStaffs request, CancellationToken cancellationToken = default)
            {
                string dateAsString = DateTime.Today.ToString("yyyyMMdd");
                var res = from s in _dbcontext.VpmSyain
                          join k in _dbcontext.VpmKyoShe on s.SyainCdSeq equals k.SyainCdSeq
                          join e in _dbcontext.VpmEigyos on k.EigyoCdSeq equals e.EigyoCdSeq
                          join c in _dbcontext.VpmCompny on e.CompanyCdSeq equals c.CompanyCdSeq
                          join t in _dbcontext.VpmTenant on c.TenantCdSeq equals t.TenantCdSeq
                          where t.TenantCdSeq == request.TenantCdSeq
                          && c.CompanyCdSeq == request.CompanyCdSeq
                          && (request.Model.EigyoStart != null ? e.EigyoCd >= request.Model.EigyoStart.EigyoCd : true)
                          && (request.Model.EigyoEnd != null ? e.EigyoCd <= request.Model.EigyoEnd.EigyoCd : true)
                          && k.StaYmd.CompareTo(dateAsString) <= 0
                          && k.EndYmd.CompareTo(dateAsString) >= 0
                          orderby s.SyainCd
                          select new Staffs()
                          {
                              SyainCd = s.SyainCd,
                              Name = s.SyainNm
                          };

                return res.ToList();
            }
        }
    }
}
