using HassyaAllrightCloud.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using System.Collections.Generic;
using System;

namespace HassyaAllrightCloud.IService
{
    public interface IVPM_SyaRyoListService
    {
        Task<string> getSyaSyuNm(int SyaSyuCdSeq, int TenantCdSeq);
        Task<List<TPM_SyaRyoData>> GetSyaRyo();
        List<TPM_SyaRyoData> GetSyaRyoRepairList(int tenantCdSeq, string dateFrom, string dateTo);

    }
    public class VPM_SyaRyoService : IVPM_SyaRyoListService
    {
        private readonly KobodbContext _dbContext;
        public VPM_SyaRyoService(KobodbContext context)
        {
            _dbContext = context;
        }
        public async Task<string> getSyaSyuNm(int SyaSyuCdSeq, int TenantCdSeq)
        {
            try
            {
                return await _dbContext.VpmSyaSyu.Where(t => t.SyaSyuCdSeq == SyaSyuCdSeq && t.TenantCdSeq==TenantCdSeq).Select(t => t.SyaSyuNm).FirstAsync();
            }
            catch
            {
                return "";
            }

        }

        public async Task<List<TPM_SyaRyoData>> GetSyaRyo()
        {
            return await (from s in _dbContext.VpmSyaRyo
                          select new TPM_SyaRyoData()
                          {
                              SyaRyoCdSeq = s.SyaRyoCdSeq,
                              SyaRyoCd = s.SyaRyoCd,
                              SyaRyoNm = s.SyaRyoNm
                          }).ToListAsync();
        }

        public List<TPM_SyaRyoData> GetSyaRyoRepairList(int tenantCdSeq, string dateFrom , string dateTo)
        {
            var data = new List<TPM_SyaRyoData>();
            data = (from SYARYO in _dbContext.VpmSyaRyo
                    join SYASYU in _dbContext.VpmSyaSyu
                    on new { H1=SYARYO.SyaSyuCdSeq, H2= tenantCdSeq }
                    equals new { H1 = SYASYU.SyaSyuCdSeq, H2 = SYASYU.TenantCdSeq }
                    into SYASYU_join
                    from SYASYU in SYASYU_join.DefaultIfEmpty()
                    join HENSYA in _dbContext.VpmHenSya
                    on SYARYO.SyaRyoCdSeq equals HENSYA.SyaRyoCdSeq
                    into HENSYA_join
                    from HENSYA in HENSYA_join.DefaultIfEmpty()
                    join EIGYOS in _dbContext.VpmEigyos
                    on HENSYA.EigyoCdSeq equals EIGYOS.EigyoCdSeq
                    into EIGYOS_join
                    from EIGYOS in EIGYOS_join.DefaultIfEmpty()
                    join COMPNY in _dbContext.VpmCompny
                    on EIGYOS.CompanyCdSeq equals COMPNY.CompanyCdSeq
                    into COMPNY_join
                    from COMPNY in COMPNY_join.DefaultIfEmpty()
                    where COMPNY.TenantCdSeq == tenantCdSeq
                    && (HENSYA.StaYmd != null ? String.Compare(HENSYA.StaYmd, dateTo) <= 0 : HENSYA.StaYmd == null || HENSYA.StaYmd != null)
                   && (HENSYA.EndYmd != null ? String.Compare(HENSYA.EndYmd, dateFrom) >= 0 : HENSYA.EndYmd == null || HENSYA.EndYmd != null)
                    orderby SYARYO.SyaRyoCd
                    select new TPM_SyaRyoData()
                    {
                        SyaRyoCdSeq = SYARYO.SyaRyoCdSeq,
                        SyaRyoCd = SYARYO.SyaRyoCd,
                        SyaRyoNm = SYARYO.SyaRyoNm,
                        NinkaKbn = SYARYO.NinkaKbn
                    }).ToList();
            return data.Distinct().ToList();
        }
    }
}
