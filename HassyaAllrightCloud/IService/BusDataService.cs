using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using HassyaAllrightCloud.Infrastructure.Persistence;
using System.Linq;
using HassyaAllrightCloud.Domain.Dto;

namespace HassyaAllrightCloud.IService
{
    public interface IBusDataListService
    {
        Task<List<BusInfoData>> Getbus(DateTime Busstartdate, int CurrentCompanyCdSeq, int[] CompanyCdSeqslt, int[] EigyoCdSeqslt, int TenantID);
        Task<List<BusInfoData>> Getallbus(int TenantID);
        Task<IEnumerable<BusInfoData>> Getbusrp(DateTime Busstartdate, int CurrentCompanyCdSeq, int[] CompanyCdSeqslt, int[] EigyoCdSeqslt, int TenantID);
    }
    public class BusDataService : IBusDataListService
    {
        private readonly KobodbContext _dbContext;

        public BusDataService(KobodbContext context)
        {
            _dbContext = context;
        }
        /// <summary>
        /// get all info data bus
        /// </summary>
        /// <returns></returns>
        public async Task<List<BusInfoData>> Getallbus(int TenantID)
        {
            return await (from e in _dbContext.VpmEigyos
                          join c in _dbContext.VpmCompny
                          on e.CompanyCdSeq equals c.CompanyCdSeq
                          join h in _dbContext.VpmHenSya
                          on e.EigyoCdSeq equals h.EigyoCdSeq
                          join sr in _dbContext.VpmSyaRyo
                          on h.SyaRyoCdSeq equals sr.SyaRyoCdSeq
                          join ss in _dbContext.VpmSyaSyu
                          on sr.SyaSyuCdSeq equals ss.SyaSyuCdSeq
                          where
                                e.SiyoKbn == 1 && c.TenantCdSeq == TenantID &&
                                ss.TenantCdSeq == TenantID
                          orderby h.EigyoCdSeq, ss.SyaSyuCd, sr.SyaRyoCdSeq ascending
                          select new BusInfoData()
                          {
                              EigyoCdSeq = e.EigyoCdSeq,
                              EigyoCd = e.EigyoCd,
                              RyakuNm = e.RyakuNm,
                              SyaRyoCdSeq = sr.SyaRyoCdSeq,
                              SyaRyoCd = sr.SyaRyoCd,
                              SyaRyoNm = sr.SyaRyoNm,
                              TeiCnt = sr.TeiCnt,
                              KariSyaRyoNm = sr.KariSyaRyoNm,
                              SyaSyuCdSeq = ss.SyaSyuCdSeq,
                              SyaSyuCd = ss.SyaSyuCd,
                              EigyoNm = e.EigyoNm,
                              SyaSyuNm = ss.SyaSyuNm,
                              CompanyCdSeq = c.CompanyCdSeq,
                              CompanyCd = c.CompanyCd,
                              CompanyNm = c.CompanyNm,
                              NinkaKbn = sr.NinkaKbn,
                              TenkoNo = h.TenkoNo,
                              id = h.SyaRyoCdSeq,
                              EndYmd = h.EndYmd,
                              StaYmd = h.StaYmd,
                              KataKbn = ss.KataKbn
                          }).ToListAsync();
        }
        /// <summary>
        /// get info data bus with parameter
        /// </summary>
        /// <param name="Busstartdate"></param>
        /// <param name="CurrentCompanyCdSeq"></param>
        /// <param name="CompanyCdSeqslt"></param>
        /// <param name="EigyoCdSeqslt"></param>
        /// <returns></returns>
        public async Task<List<BusInfoData>> Getbus(DateTime Busstartdate, int CurrentCompanyCdSeq, int[] CompanyCdSeqslt, int[] EigyoCdSeqslt,int TenantID)
        {
            int a = CompanyCdSeqslt.Count();
            int b = EigyoCdSeqslt.Count();
            string DateAsString = Busstartdate.ToString("yyyyMMdd");
            return await (from e in _dbContext.VpmEigyos
                          join c in _dbContext.VpmCompny
                          on e.CompanyCdSeq equals c.CompanyCdSeq
                          join h in _dbContext.VpmHenSya
                          on e.EigyoCdSeq equals h.EigyoCdSeq
                          join sr in _dbContext.VpmSyaRyo
                          on h.SyaRyoCdSeq equals sr.SyaRyoCdSeq
                          join ss in _dbContext.VpmSyaSyu
                          on sr.SyaSyuCdSeq equals ss.SyaSyuCdSeq
                          join locktable in _dbContext.TkdLockTable
                          on new { c.TenantCdSeq, e.EigyoCdSeq } equals new { locktable.TenantCdSeq, locktable.EigyoCdSeq } into locktableGr
                          from locktableSub in locktableGr.DefaultIfEmpty()
                          where
                                e.SiyoKbn == 1 &&
                                c.TenantCdSeq==TenantID &&
                                ss.TenantCdSeq==TenantID
                                &&
                                (
                                EigyoCdSeqslt.Count() > 0 ? EigyoCdSeqslt.Contains(e.EigyoCdSeq) : CompanyCdSeqslt.Contains(e.CompanyCdSeq))
                          orderby h.EigyoCdSeq, ss.SyaSyuCd, sr.SyaRyoCdSeq ascending
                          select new BusInfoData()
                          {
                              EigyoCdSeq = e.EigyoCdSeq,
                              EigyoCd = e.EigyoCd,
                              RyakuNm = e.RyakuNm,
                              SyaRyoCdSeq = sr.SyaRyoCdSeq,
                              SyaRyoCd = sr.SyaRyoCd,
                              SyaRyoNm = sr.SyaRyoNm,
                              TeiCnt = sr.TeiCnt,
                              KariSyaRyoNm = sr.KariSyaRyoNm,
                              SyaSyuCdSeq = ss.SyaSyuCdSeq,
                              SyaSyuCd = ss.SyaSyuCd,
                              EigyoNm = e.EigyoNm,
                              SyaSyuNm = ss.SyaSyuNm,
                              CompanyCdSeq = c.CompanyCdSeq,
                              CompanyCd = c.CompanyCd,
                              CompanyNm = c.CompanyNm,
                              NinkaKbn = sr.NinkaKbn,
                              TenkoNo = h.TenkoNo,
                              id = h.SyaRyoCdSeq,
                              EndYmd = h.EndYmd,
                              StaYmd = h.StaYmd,
                              KataKbn = ss.KataKbn,
                              LockYmd = locktableSub.LockYmd
                          }).ToListAsync();
        }
        /// <summary>
        /// get data bus repair
        /// </summary>
        /// <param name="Busstartdate"></param>
        /// <param name="CurrentCompanyCdSeq"></param>
        /// <param name="CompanyCdSeqslt"></param>
        /// <param name="EigyoCdSeqslt"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BusInfoData>> Getbusrp(DateTime Busstartdate, int CurrentCompanyCdSeq, int[] CompanyCdSeqslt, int[] EigyoCdSeqslt,int TenantID)
        {
            int a = CompanyCdSeqslt.Count();
            int b = EigyoCdSeqslt.Count();
            string DateAsString = Busstartdate.ToString("yyyyMMdd");
            return await (from e in _dbContext.VpmEigyos
                          join c in _dbContext.VpmCompny
                          on e.CompanyCdSeq equals c.CompanyCdSeq
                          join h in _dbContext.VpmHenSya
                          on e.EigyoCdSeq equals h.EigyoCdSeq
                          join sr in _dbContext.VpmSyaRyo
                          on h.SyaRyoCdSeq equals sr.SyaRyoCdSeq
                          join ss in _dbContext.VpmSyaSyu
                          on sr.SyaSyuCdSeq equals ss.SyaSyuCdSeq
                          where DateAsString.CompareTo(h.StaYmd) >= 0 &&
                                DateAsString.CompareTo(h.EndYmd) <= 0 &&
                                e.SiyoKbn == 1 &&
                                c.TenantCdSeq == TenantID &&
                                ss.TenantCdSeq == TenantID
                                &&
                                (
                                EigyoCdSeqslt.Count() > 0 ? EigyoCdSeqslt.Contains(e.EigyoCdSeq) : CompanyCdSeqslt.Contains(e.CompanyCdSeq))
                          orderby h.EigyoCdSeq, ss.SyaSyuCd, sr.SyaRyoCdSeq ascending
                          select new BusInfoData()
                          {
                              EigyoCdSeq = e.EigyoCdSeq,
                              EigyoCd = e.EigyoCd,
                              RyakuNm = e.RyakuNm,
                              SyaRyoCdSeq = sr.SyaRyoCdSeq,
                              SyaRyoCd = sr.SyaRyoCd,
                              SyaRyoNm = sr.SyaRyoNm,
                              TeiCnt = sr.TeiCnt,
                              KariSyaRyoNm = sr.KariSyaRyoNm,
                              SyaSyuCdSeq = ss.SyaSyuCdSeq,
                              NinkaKbn = sr.NinkaKbn,
                              SyaSyuCd = ss.SyaSyuCd,
                              EigyoNm = e.EigyoNm,
                              SyaSyuNm = ss.SyaSyuNm,
                              CompanyCdSeq = c.CompanyCdSeq,
                              CompanyCd = c.CompanyCd,
                              CompanyNm = c.CompanyNm,
                              TenkoNo = h.TenkoNo,
                              id = h.SyaRyoCdSeq,
                              EndYmd = h.EndYmd,
                              StaYmd = h.StaYmd,
                              KataKbn = ss.KataKbn
                          }).ToListAsync();
        }
    }
}
