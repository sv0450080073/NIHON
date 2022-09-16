using Microsoft.EntityFrameworkCore;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;

namespace HassyaAllrightCloud.IService
{
    public interface ISaleBranchListService
    {
        Task<List<SaleBranchData>> Get(int CompanyId);
    }

    public class SaleBranchService : ISaleBranchListService
    {
        private readonly KobodbContext _dbContext;

        public SaleBranchService(KobodbContext context)
        {
            _dbContext = context;
        }
        public async Task<List<SaleBranchData>> Get(int CompanyId)
        {
            return await (from eigyos in _dbContext.VpmEigyos
                          where eigyos.CompanyCdSeq == CompanyId && eigyos.SiyoKbn == 1
                          orderby eigyos.EigyoCd, eigyos.EigyoCdSeq
                          select new SaleBranchData(eigyos)).ToListAsync();
        }
    }
}
