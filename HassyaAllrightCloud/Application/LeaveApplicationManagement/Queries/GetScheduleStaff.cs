using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.LeaveApplicationManagement.Queries
{
    public class GetScheduleStaff : IRequest<IEnumerable<Staffs>>
    {
        public class Handler : IRequestHandler<GetScheduleStaff, IEnumerable<Staffs>>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext kobodbContext) => _dbContext = kobodbContext;


            public async Task<IEnumerable<Staffs>> Handle(GetScheduleStaff request, CancellationToken cancellationToken)
            {
                string DateAsString = DateTime.Today.ToString("yyyyMMdd");
                var result = (from s in _dbContext.VpmSyain
                              join k in _dbContext.VpmKyoShe
                              on new { key1 = s.SyainCdSeq, key2 = true, key3 = true } equals new { key1 = k.SyainCdSeq, key2 = k.StaYmd.CompareTo(DateAsString) <= 0, key3 = k.EndYmd.CompareTo(DateAsString) >= 0 } into ek
                              from ekTemp in ek.DefaultIfEmpty()
                              join e in _dbContext.VpmEigyos
                              on new { key1 = ekTemp.EigyoCdSeq, key2 = (byte)1 } equals new { key1 = e.EigyoCdSeq, key2 = e.SiyoKbn } into eek
                              from eekTemp in eek.DefaultIfEmpty()
                              join c in _dbContext.VpmCompny
                              on new { key1 = eekTemp.CompanyCdSeq, key2 = (byte)1, key3 = new ClaimModel().TenantID } equals new { key1 = c.CompanyCdSeq, key2 = c.SiyoKbn, key3 = c.TenantCdSeq } into eekc
                              from eekcTemp in eekc.DefaultIfEmpty()
                              where eekTemp.CompanyCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID
                              orderby s.SyainCd
                              select new Staffs()
                              {
                                  Seg = s.SyainCdSeq,
                                  Name = s.SyainCd + ":" + s.SyainNm,
                                  EigyoCdSeq = eekTemp.EigyoCdSeq
                              }).ToList();
                return result;
            }
        }
    }
}
