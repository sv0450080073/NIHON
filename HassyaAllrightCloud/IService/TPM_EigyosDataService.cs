using HassyaAllrightCloud.Application.SaleBranch.Queries;
using HassyaAllrightCloud.Domain.Dto;
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
    public interface ITPM_EigyosDataListService
    {
        Task<List<BranchChartData>> GetBranchbyCompany(int CompanyCdSeq, int TenantCdSeq);
        List<BranchChartData> GetBranchbyCompany1(int CompanyCdSeq, int TenantCdSeq);
        /// <summary>
        /// Get list branch data info by companyId and tenantId
        /// </summary>
        /// <param name="companyId">Company ID</param>
        /// <param name="tenantId">Tenant ID</param>
        /// <returns>List result branch data follow condition</returns>
        Task<List<LoadSaleBranch>> GetBranchDataByCompany(int companyId, int tenantId);
        /// <summary>
        /// Get list branches by multiple company id
        /// </summary>
        /// <param name="tenantId">Tenant id</param>
        /// <param name="companyIds">List of company</param>
        /// <returns>banches</returns>
        Task<List<LoadSaleBranch>> GetBranchDataByCompanies(int tenantId, List<int> companyIds);
        /// <summary>
        /// Get list branch data info by tenantId
        /// </summary>
        /// <param name="tenantId">Tenant ID</param>
        /// <returns>List result branch data follow condition</returns>
        Task<List<LoadSaleBranch>> GetBranchDataByTenantId(int tenantId);
        string Getbranchbyid(int id);
        int GetBranchbyBusId(int busid, string fromdate);

        Task<List<DepartureOfficeData>> GetAllBranchData(int TenantCdSeq);
        List<DepartureOfficeData> GetBranchDataByIdCompany(List<CompanyChartData> idCompany, int TenantCdSeq);
        List<BranchChartData> GetBranchbyTenantCdSeq(int TenantCdSeq);
    }
    public class TPM_EigyosDataService : ITPM_EigyosDataListService
    {
        private readonly KobodbContext _dbContext;
        private readonly ILogger<TPM_EigyosDataService> _logger;
        private readonly IMediator _mediatR;
        private readonly IMemoryCache _memoryCache;

        public TPM_EigyosDataService(KobodbContext context, IMediator mediatR, IMemoryCache memoryCache, ILogger<TPM_EigyosDataService> logger)
        {
            _dbContext = context;
            _logger = logger;
            _mediatR = mediatR;
            _memoryCache = memoryCache;
        }
        /// <summary>
        /// get branch by company
        /// </summary>
        /// <param name="CompanyCdSeq"></param>
        /// <returns></returns>
        public async Task<List<BranchChartData>> GetBranchbyCompany(int CompanyCdSeq, int TenantCdSeq)
        {
            return await (from s in _dbContext.VpmEigyos
                          join c in _dbContext.VpmCompny on s.CompanyCdSeq equals c.CompanyCdSeq
                          where s.SiyoKbn == 1 && s.CompanyCdSeq == CompanyCdSeq && c.TenantCdSeq == TenantCdSeq
                          orderby s.EigyoCd ascending
                          select new BranchChartData()
                          {
                              EigyoCdSeq = s.EigyoCdSeq,
                              EigyoCd = s.EigyoCd,
                              RyakuNm = s.RyakuNm,
                              CompanyNm = c.CompanyNm,
                              CompanyCdSeq = s.CompanyCdSeq,
                              Com_RyakuNm = c.RyakuNm

                          }).ToListAsync();
        }

        /// <summary>
        /// get company name by companyCdSeq
        /// </summary>
        /// <param name="companyCdSeq"></param>
        /// <returns></returns>
        public string GetcompanyName(int companyCdSeq, int TenantCdSeq)
        {
            return _dbContext.VpmCompny.Where(t => t.CompanyCdSeq == companyCdSeq && t.TenantCdSeq == TenantCdSeq).First().CompanyNm;
        }
        /// <summary>
        /// get data branch by company
        /// </summary>
        /// <param name="CompanyCdSeq"></param>
        /// <returns></returns>
        public List<BranchChartData> GetBranchbyCompany1(int CompanyCdSeq, int TenantCdSeq)
        {
            return (from s in _dbContext.VpmEigyos
                    join c in _dbContext.VpmCompny on s.CompanyCdSeq equals c.CompanyCdSeq
                    where s.SiyoKbn == 1 && s.CompanyCdSeq == CompanyCdSeq && c.TenantCdSeq == TenantCdSeq
                    orderby s.EigyoCd ascending
                    select new BranchChartData()
                    {
                        EigyoCdSeq = s.EigyoCdSeq,
                        EigyoCd = s.EigyoCd,
                        RyakuNm = s.RyakuNm,
                        CompanyNm = c.CompanyNm,
                        CompanyCdSeq = c.CompanyCdSeq,
                        Com_RyakuNm = c.RyakuNm
                    }).ToList();
        }
        
        public List<BranchChartData> GetBranchbyTenantCdSeq(int TenantCdSeq)
        {
            return (from s in _dbContext.VpmEigyos
                    join c in _dbContext.VpmCompny on s.CompanyCdSeq equals c.CompanyCdSeq
                    where s.SiyoKbn == 1 
                    && c.TenantCdSeq == TenantCdSeq
                    orderby s.EigyoCd ascending
                    select new BranchChartData()
                    {
                        EigyoCdSeq = s.EigyoCdSeq,
                        EigyoCd = s.EigyoCd,
                        RyakuNm = s.RyakuNm,
                        CompanyNm = c.CompanyNm,
                        CompanyCdSeq = c.CompanyCdSeq,
                        Com_RyakuNm = c.RyakuNm
                    }).ToList();
        }
        /// <summary>
        /// get branch name
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string Getbranchbyid(int id)
        {
            return _dbContext.VpmEigyos.Where(t => t.EigyoCdSeq == id).FirstOrDefault().RyakuNm;
        }
        /// <summary>
        /// get branch by busid
        /// </summary>
        /// <param name="busid"></param>
        /// <param name="fromdate"></param>
        /// <returns></returns>
        public int GetBranchbyBusId(int busid, string fromdate)
        {
            var tenantCdSeq = new ClaimModel().TenantID;
            try
            {
                return (from e in _dbContext.VpmEigyos
                        join c in _dbContext.VpmCompny
                        on e.CompanyCdSeq equals c.CompanyCdSeq
                        join h in _dbContext.VpmHenSya
                        on e.EigyoCdSeq equals h.EigyoCdSeq
                        join sr in _dbContext.VpmSyaRyo
                        on h.SyaRyoCdSeq equals sr.SyaRyoCdSeq
                        join ss in _dbContext.VpmSyaSyu
                        on sr.SyaSyuCdSeq equals ss.SyaSyuCdSeq
                        where fromdate.CompareTo(h.StaYmd) >= 0 &&
                              fromdate.CompareTo(h.EndYmd) < 0 &&
                              e.SiyoKbn == 1 && h.SyaRyoCdSeq == busid
                              && c.TenantCdSeq == tenantCdSeq
                        select h.EigyoCdSeq).First();
            }
            catch
            {
                return 0;
            }

        }

        //new M
        public async Task<List<DepartureOfficeData>> GetAllBranchData(int TenantCdSeq)
        {
            var data = (from VpmEigyos in _dbContext.VpmEigyos
                        join VpmCompny in _dbContext.VpmCompny
                        on VpmEigyos.CompanyCdSeq equals VpmCompny.CompanyCdSeq
                        into VpmCompny_join
                        from VpmCompny in VpmCompny_join.DefaultIfEmpty()
                        where VpmEigyos.SiyoKbn == 1
                        && VpmCompny.SiyoKbn == 1
                        && VpmCompny.TenantCdSeq == TenantCdSeq
                        orderby VpmEigyos.EigyoCd ascending
                        select new DepartureOfficeData()
                        {
                            CompanyCd = VpmCompny.CompanyCd,
                            Com_RyakuNm = VpmCompny.RyakuNm,
                            EigyoCd = VpmEigyos.EigyoCd,
                            EigyoNm = VpmEigyos.EigyoNm,
                            EigyoCdSeq = VpmEigyos.EigyoCdSeq,
                            CompanyCdSeq = VpmCompny.CompanyCdSeq,

                        }).ToList();
            return data;
        }

        public List<DepartureOfficeData> GetBranchDataByIdCompany(List<CompanyChartData> ListCompany, int TenantCdSeq)
        {
            int checkAllCompany = 1;//true
            checkAllCompany = ListCompany.Count > 1 && ListCompany.First().CompanyCdSeq == 0 ? 1 : 0;
            var data = (from VpmEigyos in _dbContext.VpmEigyos
                        join VpmCompny in _dbContext.VpmCompny
                        on VpmEigyos.CompanyCdSeq equals VpmCompny.CompanyCdSeq
                        into VpmCompny_join
                        from VpmCompny in VpmCompny_join.DefaultIfEmpty()
                        where VpmEigyos.SiyoKbn == 1
                        && VpmCompny.SiyoKbn == 1
                        && (checkAllCompany == 0 ? ListCompany.Select(x => x.CompanyCdSeq).ToArray().Contains(VpmCompny.CompanyCdSeq) : VpmCompny.CompanyCdSeq != null)
                        && VpmCompny.TenantCdSeq == TenantCdSeq

                        orderby VpmEigyos.EigyoCd ascending
                        select new DepartureOfficeData()
                        {
                            CompanyCd = VpmCompny.CompanyCd,
                            Com_RyakuNm = VpmCompny.RyakuNm,
                            EigyoCd = VpmEigyos.EigyoCd,
                            EigyoNm = VpmEigyos.EigyoNm,
                            EigyoCdSeq = VpmEigyos.EigyoCdSeq,
                            CompanyCdSeq = VpmCompny.CompanyCdSeq,

                        }).ToList();
            return data;
        }

        public async Task<List<LoadSaleBranch>> GetBranchDataByCompany(int companyId, int tenantId)
        {
            return await _memoryCache.GetOrCreateAsync($"AllBranchByCompany_{tenantId}_{companyId}", async e =>
            {
                e.SetOptions(new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                    TimeSpan.FromSeconds(10)
                });

                return await _mediatR.Send(new GetSaleBranchByCompanyQuery(companyId, tenantId));
            });
        }

        public async Task<List<LoadSaleBranch>> GetBranchDataByTenantId(int tenantId)
        {
            return await _memoryCache.GetOrCreateAsync($"AllBranchByTenantId_{tenantId}", async e =>
            {
                e.SetOptions(new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                    TimeSpan.FromSeconds(10)
                });

                return (await _mediatR.Send(new GetSaleBranchByTenantQuery(tenantId))).ToList();
            });
        }

        public async Task<List<LoadSaleBranch>> GetBranchDataByCompanies(int tenantId, List<int> companyIds)
        {
            return await _memoryCache.GetOrCreateAsync($"GetBranchDataByCompanies{tenantId}_{companyIds?.Count ?? -1}", async e =>
            {
                e.SetOptions(new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                    TimeSpan.FromSeconds(10)
                });

                return await _mediatR.Send(new GetSaleBranchByCompaniesQuery(tenantId, companyIds));
            });
        }
    }
}
