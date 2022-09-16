using Microsoft.EntityFrameworkCore;
using HassyaAllrightCloud.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using System;

namespace HassyaAllrightCloud.IService
{
    public interface ILocationListService
    {
        Task<List<LoadLocation>> GetDestination(int Tenant);
        Task<List<LoadLocation>> GetOrigin(int Tenant);
        Task<List<LoadLocation>> GetArea(int Tenant);
    }

    public class LocationService : ILocationListService
    {
        private readonly KobodbContext _dbContext;

        public LocationService(KobodbContext context)
        {
            _dbContext = context;
        }
        public async Task<List<LoadLocation>> GetDestination(int Tenant)
        {
            return await (from VpmBasyo in _dbContext.VpmBasyo
                          where VpmBasyo.SiyoIkiKbn == 1 && VpmBasyo.SiyoKbn == 1
                          from VpmCodeKb in _dbContext.VpmCodeKb
                          where VpmBasyo.BasyoKenCdSeq == VpmCodeKb.CodeKbnSeq && VpmCodeKb.SiyoKbn == 1 && VpmBasyo.TenantCdSeq == Tenant
                          orderby VpmCodeKb.CodeKbn, VpmBasyo.BasyoMapCd, VpmCodeKb.CodeKbnSeq, VpmBasyo.BasyoMapCdSeq
                          select new LoadLocation()
                          {
                              CodeKbn = VpmCodeKb.CodeKbn,
                              BasyoMapCd = VpmBasyo.BasyoMapCd,
                              CodeKbnNm = VpmCodeKb.CodeKbnNm,
                              BasyoNm = VpmBasyo.BasyoNm,
                              CodeKbnSeq = Convert.ToInt32(VpmCodeKb.CodeKbnSeq),
                              BasyoMapCdSeq = VpmBasyo.BasyoMapCdSeq,
                              BasyoKenCdSeq = VpmBasyo.BasyoKenCdSeq
                          }
                ).ToListAsync();
        }

        public async Task<List<LoadLocation>> GetOrigin(int Tenant)
        {
            return await (from VpmBasyo in _dbContext.VpmBasyo
                          where VpmBasyo.SiyoHseiKbn == 1 && VpmBasyo.SiyoKbn == 1
                          from VpmCodeKb in _dbContext.VpmCodeKb
                          where VpmBasyo.BasyoKenCdSeq == VpmCodeKb.CodeKbnSeq && VpmCodeKb.SiyoKbn == 1 && VpmBasyo.TenantCdSeq == Tenant
                          orderby VpmCodeKb.CodeKbn, VpmBasyo.BasyoMapCd, VpmCodeKb.CodeKbnSeq, VpmBasyo.BasyoMapCdSeq
                          select new LoadLocation()
                          {
                              CodeKbn = VpmCodeKb.CodeKbn,
                              BasyoMapCd = VpmBasyo.BasyoMapCd,
                              CodeKbnNm = VpmCodeKb.CodeKbnNm,
                              BasyoNm = VpmBasyo.BasyoNm,
                              CodeKbnSeq = Convert.ToInt32(VpmCodeKb.CodeKbnSeq),
                              BasyoMapCdSeq = VpmBasyo.BasyoMapCdSeq
                          }
                ).ToListAsync();
        }

        public async Task<List<LoadLocation>> GetArea(int Tenant)
        {
            return await (from VpmBasyo in _dbContext.VpmBasyo
                          where VpmBasyo.SiyoAreaKbn == 1 && VpmBasyo.SiyoKbn == 1
                          from VpmCodeKb in _dbContext.VpmCodeKb
                          where VpmBasyo.BasyoKenCdSeq == VpmCodeKb.CodeKbnSeq && VpmCodeKb.SiyoKbn == 1 && VpmBasyo.TenantCdSeq == Tenant
                          orderby VpmCodeKb.CodeKbn, VpmBasyo.BasyoMapCd, VpmCodeKb.CodeKbnSeq, VpmBasyo.BasyoMapCdSeq
                          select new LoadLocation()
                          {
                              CodeKbn = VpmCodeKb.CodeKbn,
                              BasyoMapCd = VpmBasyo.BasyoMapCd,
                              CodeKbnNm = VpmCodeKb.CodeKbnNm,
                              BasyoNm = VpmBasyo.BasyoNm,
                              CodeKbnSeq = Convert.ToInt32(VpmCodeKb.CodeKbnSeq),
                              BasyoMapCdSeq = VpmBasyo.BasyoMapCdSeq
                          }
                ).ToListAsync();
        }
    }
}
