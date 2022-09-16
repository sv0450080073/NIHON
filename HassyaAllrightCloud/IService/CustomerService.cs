using HassyaAllrightCloud.Application.Customer.Queries;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface ICustomerListService
    {
        Task<List<LoadCustomerList>> Get(int Tenant);
        /// <summary>
        /// Get list customer info by tenantId
        /// </summary>
        /// <param name="tenantId">TenantID</param>
        /// <returns>List customer after filter by tenantId. Empty list if not found any customer matched</returns>
        Task<List<LoadCustomerList>> GetCustomerByTenantIdAsync(int tenantId);
        List<LoadCustomerList> GetCustomerByDateReport(int Tenant, string DateReport);
        Task<List<LoadCustomerList>> GetCustomerCancelListAsync(int tenantId, string fromDate, string toDate);
        Task<List<LoadCustomerList>> GetCustomerAccessoryFeeListAsync(int tenantId, string fromDate, string toDate);
        /// <summary>
        /// Get list customer info by tenantId and (from-to) HaisYmd
        /// </summary>
        /// <param name="tenantId">TenantCdSeq</param>
        /// <param name="haisDateFrom">HaisYmd From</param>
        /// <param name="haisDateTo">HaisYmd To</param>
        /// <returns>List customer after filter by tenantId. Empty list if not found any customer matched</returns>
        Task<List<LoadCustomerList>> GetCustomerByTenantIdAsync(int tenantId, string haisDateFrom, string haisDateTo);

    }

    public class CustomerService : ICustomerListService
    {
        private readonly KobodbContext _dbContext;
        private readonly IMediator _mediatR;
        private readonly IMemoryCache _memoryCache;

        public CustomerService(KobodbContext context, IMediator mediatR, IMemoryCache memoryCache)
        {
            _dbContext = context;
            _mediatR = mediatR;
            _memoryCache = memoryCache;
        }
        public async Task<List<LoadCustomerList>> Get(int Tenant)
        {
            string DateAsString = DateTime.Today.ToString("yyyyMMdd");
            return await (from t in _dbContext.VpmTokisk
                          where DateAsString.CompareTo(t.SiyoStaYmd) >= 0 && DateAsString.CompareTo(t.SiyoEndYmd) <= 0 && t.TenantCdSeq == Tenant
                          from s in _dbContext.VpmTokiSt
                          where t.TokuiSeq == s.TokuiSeq && DateAsString.CompareTo(s.SiyoStaYmd) >= 0 && DateAsString.CompareTo(s.SiyoEndYmd) <= 0
                          from g in _dbContext.VpmGyosya
                          where t.GyosyaCdSeq == g.GyosyaCdSeq && g.SiyoKbn == 1
                          orderby g.GyosyaCd, t.TokuiCd, s.SitenCd, g.GyosyaCdSeq, t.TokuiSeq, s.SitenCdSeq
                          select new LoadCustomerList()
                          {
                              TokuiSeq = t.TokuiSeq,
                              SitenCdSeq = s.SitenCdSeq,
                              TokuiCd = t.TokuiCd,
                              RyakuNm = t.RyakuNm,
                              SitenCd = s.SitenCd,
                              SitenNm = s.SitenNm,
                              TesuRitu = s.TesuRitu,
                              TesuRituGui = s.TesuRituGui,
                              GyoSysSeq = g.GyosyaCdSeq,
                              GyoSyaCd = g.GyosyaCd,
                              GyoSyaNm = g.GyosyaNm
                          }).ToListAsync();
        }

        public async Task<List<LoadCustomerList>> GetCustomerByTenantIdAsync(int tenantId)
        {
            return await _memoryCache.GetOrCreateAsync($"AllCustomerByTenantId_{tenantId}", async e =>
            {
                e.SetOptions(new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                    TimeSpan.FromSeconds(10)
                });

                return await _mediatR.Send(new GetCustomersByTenantIdQuery(tenantId));
            });
        }

        public async Task<List<LoadCustomerList>> GetCustomerCancelListAsync(int tenantId, string fromDate, string toDate)
        {
            return await _memoryCache.GetOrCreateAsync($"AllCustomerCancelList_{tenantId}_{fromDate}_{toDate}", async e =>
            {
                e.SetOptions(new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                    TimeSpan.FromSeconds(10)
                });
                return await _mediatR.Send(new GetCustomerCancelListQuery(tenantId, fromDate, toDate));
            });
        }

        public async Task<List<LoadCustomerList>> GetCustomerAccessoryFeeListAsync(int tenantId, string fromDate, string toDate)
        {
            return await _memoryCache.GetOrCreateAsync($"AllCustomerAccessoryFeeList_{tenantId}_{fromDate}_{toDate}", async e =>
            {
                e.SetOptions(new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                    TimeSpan.FromSeconds(10)
                });
                return await _mediatR.Send(new GetCustomerAccessoryFeeListQuery(tenantId, fromDate, toDate));
            });
        }

        public List<LoadCustomerList> GetCustomerByDateReport(int Tenant, string DateReport)
        {
            var data =  (from TOKIST in _dbContext.VpmTokiSt
                        join TOKISK in _dbContext.VpmTokisk
                        on TOKIST.TokuiSeq equals TOKISK.TokuiSeq
                         into TOKISK_join
                        from TOKISK in TOKISK_join.DefaultIfEmpty()
                        where
                         String.Compare(TOKIST.SiyoStaYmd, DateReport) <= 0
                          && String.Compare(TOKIST.SiyoEndYmd, DateReport) >= 0
                          && String.Compare(TOKISK.SiyoStaYmd, DateReport) <= 0
                          && String.Compare(TOKISK.SiyoEndYmd, DateReport) >= 0
                         && TOKISK.TenantCdSeq == Tenant
                         //orderby TOKISK.TokuiSeq, TOKIST.SitenCdSeq
                         orderby TOKISK.TokuiCd, TOKIST.SitenCd
                         select new LoadCustomerList()
                        {
                            TokuiSeq = TOKISK.TokuiSeq,
                            TokuiCd = TOKISK.TokuiCd,
                            TOKISK_TokuiNm = TOKISK.TokuiNm,
                            SitenCdSeq = TOKIST.SitenCdSeq,
                            SitenCd = TOKIST.SitenCd,
                            SitenNm = TOKIST.SitenNm
                        }).ToList();
            return data;
        }

        /// <summary>
        /// Get customer data list service
        /// </summary>
        /// <param name="tenantId">Current login tenant id</param>
        /// <param name="haisDateFrom">HaiSYmd from</param>
        /// <param name="haisDateTo">HaiSYmd to</param>
        /// <returns>Customer data list</returns>
        public async Task<List<LoadCustomerList>> GetCustomerByTenantIdAsync(int tenantId, string haisDateFrom, string haisDateTo)
        {
            try
            {
                var result = await _memoryCache.GetOrCreateAsync($"AllCustomerByTenantId_{tenantId}_{haisDateFrom}_{haisDateTo}", async e =>
                {
                    try
                    {
                        e.SetOptions(new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10) });
                        return await _mediatR.Send(new GetCustomersWithHaisFromToQuery(tenantId, haisDateFrom, haisDateTo));
                    }
                    catch(Exception)
                    {
                        throw;
                    }
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
