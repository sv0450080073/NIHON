using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IServiceOfficeService
    {
        Task<List<LoadServiceOffice>> Get(int TenantId);
        Task<List<LoadServiceOffice>> GetBranchReport(int TenantId);
    }

    public class ServiceOfficeService : IServiceOfficeService
    {
        private readonly KobodbContext _dbContext;
        public ServiceOfficeService(KobodbContext context)
        {
            _dbContext = context;
        }
        public async Task<List<LoadServiceOffice>> Get(int TenantId)
        {
            return await (
                from c in _dbContext.VpmCompny
                join e in _dbContext.VpmEigyos on c.CompanyCdSeq equals e.CompanyCdSeq
                where c.SiyoKbn == 1
                    && e.SiyoKbn == 1
                    && c.TenantCdSeq == TenantId
                orderby c.CompanyCd, e.EigyoCd
                select new LoadServiceOffice()
                {
                    CompanyCd = c.CompanyCd,
                    CompanyName = c.RyakuNm,
                    OfficeCd = e.EigyoCd,
                    OfficeName = e.RyakuNm,
                    OfficeCdSeq = e.EigyoCdSeq
                }).ToListAsync();
        }

        public async Task<List<LoadServiceOffice>> GetBranchReport(int TenantId)
        {
            return  (
               from c in _dbContext.VpmCompny
               join e in _dbContext.VpmEigyos 
               on c.CompanyCdSeq equals e.CompanyCdSeq
               where c.SiyoKbn == 1
                   && e.SiyoKbn == 1
                   && c.TenantCdSeq == TenantId
               orderby c.CompanyCdSeq, e.EigyoCdSeq
               select new LoadServiceOffice()
               {
                   CompanyCd = c.CompanyCd,
                   CompanyName = c.RyakuNm,
                   OfficeCdSeq =  e.EigyoCdSeq,
                   OfficeCd = e.EigyoCd,
                   OfficeName = e.RyakuNm
               }).ToList();

        }
    }
}
