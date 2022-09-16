using Microsoft.EntityFrameworkCore;
using HassyaAllrightCloud.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using System;

namespace HassyaAllrightCloud.IService
{
    public interface IDispatchListService
    {
        Task<List<LoadDispatchArea>> Get(int Tenant);
    }

    public class DispatchService : IDispatchListService
    {
        private readonly KobodbContext _dbContext;

        public DispatchService(KobodbContext context)
        {
            _dbContext = context;
        }
        public async Task<List<LoadDispatchArea>> Get(int Tenant)
        {
            return await (from haichi in _dbContext.VpmHaichi
                          where haichi.SiyoKbn == 1
                          from codeKb in _dbContext.VpmCodeKb
                          where haichi.BunruiCdSeq == codeKb.CodeKbnSeq && codeKb.SiyoKbn == 1 && haichi.TenantCdSeq == Tenant
                          orderby codeKb.CodeKbn, haichi.HaiScd, codeKb.CodeKbnSeq, haichi.HaiScdSeq
                          select new LoadDispatchArea()
                          {
                              CodeKbn = codeKb.CodeKbn,
                              CodeKbnNm = codeKb.CodeKbnNm,
                              HaiSCd = haichi.HaiScd,
                              HaiSNm = haichi.HaiSnm,
                              CodeKbnSeq = Convert.ToInt32(codeKb.CodeKbnSeq),
                              HaiScdSeq = haichi.HaiScdSeq,
                              BunruiCdSeq = haichi.BunruiCdSeq
                          }
                ).ToListAsync();
        }
    }
}
