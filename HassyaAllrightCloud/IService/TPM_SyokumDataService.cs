using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using HassyaAllrightCloud.Domain.Dto;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface ITPM_SyokumDataListService
    {
        Task<List<TPM_SyokumData>> GetDriver(DateTime date, int companyseq, int TenantCdSeq);
        Task<TPM_SyokumData> GetDriverbyid(int SyainCdSeq, int TenantCdSeq);
    }
    public class TPM_SyokumDataService : ITPM_SyokumDataListService
    {
        private readonly KobodbContext _dbContext;
        public TPM_SyokumDataService(KobodbContext context)
        {
            _dbContext = context;
        }
        
        /// <summary>
        /// get driver
        /// </summary>
        /// <param name="date"></param>
        /// <param name="companyseq"></param>
        /// <returns></returns>
        public async Task<List<TPM_SyokumData>> GetDriver(DateTime date, int companyseq,int TenantCdSeq)
        {
            string datetime = date.ToString("yyyyMMdd");
            int [] driverandguider = new int[4]  { 1,  2, 3, 4};
            return await (from TPM_KyoSHe in _dbContext.VpmKyoShe
                          join TPM_Eigyos in _dbContext.VpmEigyos on TPM_KyoSHe.EigyoCdSeq equals TPM_Eigyos.EigyoCdSeq into TPM_Eigyos_join
                          from TPM_Eigyos in TPM_Eigyos_join.DefaultIfEmpty()
                          join TPM_Company in _dbContext.VpmCompny on TPM_Eigyos.CompanyCdSeq equals TPM_Company.CompanyCdSeq into TPM_Company_join
                          from TPM_Company in TPM_Company_join.DefaultIfEmpty()
                          join TPM_Syokum in _dbContext.VpmSyokum on TPM_KyoSHe.SyokumuCdSeq equals TPM_Syokum.SyokumuCdSeq into TPM_Syokum_join
                          from TPM_Syokum in TPM_Syokum_join.DefaultIfEmpty()
                          join TPM_Syain in _dbContext.VpmSyain on TPM_KyoSHe.SyainCdSeq equals TPM_Syain.SyainCdSeq into TPM_Syain_join
                          from TPM_Syain in TPM_Syain_join.DefaultIfEmpty()
                          where
                            TPM_Eigyos.CompanyCdSeq == companyseq &&
                            string.Compare(TPM_KyoSHe.StaYmd, datetime) <= 0 &&
                            string.Compare(TPM_KyoSHe.EndYmd, datetime) >= 0 &&
                            TPM_Eigyos.SiyoKbn == 1 &&
                            TPM_Syokum.SiyoKbn == 1 &&
                            TPM_Company.TenantCdSeq==TenantCdSeq &&
                            driverandguider.Contains(TPM_Syokum.SyokumuKbn)
                          orderby
                            TPM_Eigyos.EigyoCdSeq,
                            TPM_KyoSHe.SyainCdSeq
                          select new TPM_SyokumData
                          {
                              Syokum_SyokumuCdSeq = TPM_Syokum.SyokumuCdSeq,
                              Syokum_SyokumuCd = TPM_Syokum.SyokumuCd,
                              Syokum_SyokumuNm = TPM_Syokum.SyokumuNm,
                              KyoSHe_SyainCdSeq = TPM_KyoSHe.SyainCdSeq,
                              Syain_SyainCd = TPM_Syain.SyainCd,
                              Syain_SyainNm = TPM_Syain.SyainNm,
                              Eigyos_EigyoCdSeq = TPM_Eigyos.EigyoCdSeq,
                              Eigyos_RyakuNm = TPM_Eigyos.RyakuNm,
                              SyokumuKbn = TPM_Syokum.SyokumuKbn
                          }).Distinct().ToListAsync();
        }
        
        /// <summary>
        /// get driver by staff id
        /// </summary>
        /// <param name="SyainCdSeq"></param>
        /// <returns></returns>
        public async Task<TPM_SyokumData> GetDriverbyid(int SyainCdSeq, int TenantCdSeq)
        {
            return await (from TPM_KyoSHe in _dbContext.VpmKyoShe
                          join TPM_Eigyos in _dbContext.VpmEigyos on TPM_KyoSHe.EigyoCdSeq equals TPM_Eigyos.EigyoCdSeq into TPM_Eigyos_join
                          from TPM_Eigyos in TPM_Eigyos_join.DefaultIfEmpty()
                          join TPM_Company in _dbContext.VpmCompny on TPM_Eigyos.CompanyCdSeq equals TPM_Company.CompanyCdSeq into TPM_Company_join
                          from TPM_Company in TPM_Company_join.DefaultIfEmpty()
                          join TPM_Syokum in _dbContext.VpmSyokum on TPM_KyoSHe.SyokumuCdSeq equals TPM_Syokum.SyokumuCdSeq into TPM_Syokum_join
                          from TPM_Syokum in TPM_Syokum_join.DefaultIfEmpty()
                          join TPM_Syain in _dbContext.VpmSyain on TPM_KyoSHe.SyainCdSeq equals TPM_Syain.SyainCdSeq into TPM_Syain_join
                          from TPM_Syain in TPM_Syain_join.DefaultIfEmpty()
                          where
                           TPM_KyoSHe.SyainCdSeq == SyainCdSeq &&
                            TPM_Eigyos.SiyoKbn == 1 &&
                            TPM_Syokum.SiyoKbn == 1 &&
                            TPM_Company.TenantCdSeq==TenantCdSeq
                          orderby
                            TPM_Eigyos.EigyoCdSeq,
                            TPM_KyoSHe.SyainCdSeq
                          select new TPM_SyokumData
                          {
                              Syokum_SyokumuCdSeq = TPM_Syokum.SyokumuCdSeq,
                              Syokum_SyokumuCd = TPM_Syokum.SyokumuCd,
                              Syokum_SyokumuNm = TPM_Syokum.SyokumuNm,
                              KyoSHe_SyainCdSeq = TPM_KyoSHe.SyainCdSeq,
                              Syain_SyainCd = TPM_Syain.SyainCd,
                              Syain_SyainNm = TPM_Syain.SyainNm,
                              Eigyos_EigyoCdSeq = TPM_Eigyos.EigyoCdSeq,
                              Eigyos_RyakuNm = TPM_Eigyos.RyakuNm
                          }).FirstAsync();
        }
    }
}
