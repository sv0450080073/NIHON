using HassyaAllrightCloud.Application.SaleCompany.Queries;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface ITPM_CompnyDataListService
    {
        Task<List<CompanyChartData>> GetCompanybyBranch(int EigyoCdSeq, int TenantCdSeq);
        Task<List<CompanyChartData>> GetCompany(int tenantId);
        Task<List<CompanyChartData>> GetCompanyListBox(int tenantId);
        /// <summary>
        /// Get list company info by tenantID
        /// </summary>
        /// <param name="tenantId">TenantID wanna get</param>
        /// <returns>List of the company in tenant specified. Return empty list if not found or error during query.</returns>
        Task<List<CompanyData>> GetCompanyByTenantIdAsync(int tenantId);
        Task<VpmSyain> GetSyainNm(int syainCd);
        Task<VpmSyain> GetSyainNmBySyainCdSeq(int syainCdSeq);
    }
    public class TPM_CompnyDataService : ITPM_CompnyDataListService
    {
        private readonly KobodbContext _dbContext;
        private readonly IMediator _mediatR;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<TPM_CompnyDataService> _logger;

        public TPM_CompnyDataService(KobodbContext context,
            IMediator mediatR,
            IMemoryCache memoryCache,
            ILogger<TPM_CompnyDataService> logger)
        {
            _dbContext = context;
            _mediatR = mediatR;
            _memoryCache = memoryCache;
            _logger = logger;
        }
        /// <summary>
        /// get data company by branch
        /// </summary>
        /// <param name="EigyoCdSeq"></param>
        /// <returns></returns>
        public async Task<List<CompanyChartData>> GetCompanybyBranch(int EigyoCdSeq, int TenantCdSeq)
        {
            return await (from s in _dbContext.VpmCompny
                          where s.SiyoKbn == 1 && s.EigyoCdSeq == EigyoCdSeq && s.TenantCdSeq == TenantCdSeq
                          orderby s.CompanyCd ascending
                          select new CompanyChartData()
                          {
                              CompanyCdSeq = s.CompanyCdSeq,
                              CompanyCd = s.CompanyCd,
                              CompanyNm = s.CompanyNm,
                              EigyoCdSeq = s.EigyoCdSeq,
                              RyakuNm = s.RyakuNm
                          }).ToListAsync();
        }

        public async Task<List<CompanyChartData>> GetCompany(int tenantId)
        {
            try
            {
                return await (from s in _dbContext.VpmCompny
                              where s.SiyoKbn == 1 && s.TenantCdSeq == tenantId
                              orderby s.CompanyCd ascending
                              select new CompanyChartData()
                              {
                                  CompanyCdSeq = s.CompanyCdSeq,
                                  CompanyCd = s.CompanyCd,
                                  CompanyNm = s.CompanyNm,
                                  EigyoCdSeq = s.EigyoCdSeq,
                                  RyakuNm = s.RyakuNm
                              }).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogTrace(ex.ToString());

                return await Task.FromResult(new List<CompanyChartData>());
            }
        }

        public async Task<List<CompanyData>> GetCompanyByTenantIdAsync(int tenantId)
        {
            return await _memoryCache.GetOrCreateAsync($"AllCompanyByTenantIdAsync_{tenantId}", async e =>
            {
                e.SetOptions(new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                    TimeSpan.FromSeconds(10)
                });

                return await _mediatR.Send(new GetCompanyByTenantIdQuery(tenantId));
            });
        }

        public async Task<VpmSyain> GetSyainNm(int syainCd)
        {
            return await _dbContext.VpmSyain.FirstOrDefaultAsync(x => x.SyainCdSeq == syainCd);
        }

        public Task<List<CompanyChartData>> GetCompanyListBox(int tenantId)
        {
            var data = new List<CompanyChartData>();
            try
            {
                data= (from s in _dbContext.VpmCompny
                        where s.SiyoKbn == 1 && s.TenantCdSeq == tenantId
                        orderby s.CompanyCd ascending
                        select new CompanyChartData()
                        {
                            CompanyCdSeq = s.CompanyCdSeq,
                            CompanyCd = s.CompanyCd,
                            CompanyNm = s.CompanyNm,
                            EigyoCdSeq = s.EigyoCdSeq,
                            RyakuNm = s.RyakuNm
                        }).ToList();
                return Task.FromResult(data);
            }
            catch (Exception ex)
            {
                _logger.LogTrace(ex.ToString());
                return Task.FromResult(data);
            }
        }

        public async Task<VpmSyain> GetSyainNmBySyainCdSeq(int syainCdSeq)
        {
            var data = new VpmSyain();
            try
            {
                 data = _dbContext.VpmSyain.Where(x => x.SyainCdSeq == syainCdSeq).FirstOrDefault();
            }
           catch(Exception ex)
            {

            }
            return data;
        }
    }
}
