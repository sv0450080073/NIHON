using HassyaAllrightCloud.Application.Customer.Queries;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;

namespace HassyaAllrightCloud.IService
{
    public interface ITPM_TokiskDataListService
    {
        Task<List<TokiskChartData>> getdata(string DateAsString);
    }
    public class TPM_TokiskDataService : ITPM_TokiskDataListService
    {
        private readonly KobodbContext _dbContext;

        public TPM_TokiskDataService(KobodbContext context)
        {
            _dbContext = context;
        }
        /// <summary>
        /// get data Tokisk
        /// </summary>
        /// <returns></returns>
        public async Task<List<TokiskChartData>> getdata(string DateAsString)
        {
            var tenantCdSeq = new ClaimModel().TenantID;
            return await (from Tokisk in _dbContext.VpmTokisk
                          join TokiSt in _dbContext.VpmTokiSt on Tokisk.TokuiSeq equals TokiSt.TokuiSeq into TokiSt_join
                          from TokiSt in TokiSt_join.DefaultIfEmpty()
                          where
                            DateAsString.CompareTo(TokiSt.SiyoStaYmd) >= 0 &&
                            DateAsString.CompareTo(TokiSt.SiyoEndYmd) <= 0 &&
                            DateAsString.CompareTo(Tokisk.SiyoStaYmd) >= 0 &&
                            DateAsString.CompareTo(Tokisk.SiyoEndYmd) <= 0
                            && Tokisk.TenantCdSeq ==tenantCdSeq
                          select new TokiskChartData
                          {
                              Tokisk_TokuiSeq = Tokisk.TokuiSeq,
                              Tokisk_TokuiCd = Tokisk.TokuiCd,
                              Tokisk_TokuiNm = Tokisk.TokuiNm,
                              Tokisk_RyakuNm = Tokisk.RyakuNm,
                              TokiSt_SitenCdSeq = TokiSt.SitenCdSeq,
                              TokiSt_SitenCd = TokiSt.SitenCd,
                              TokiSt_RyakuNm = TokiSt.RyakuNm,
                              TokiSt_TelNo = TokiSt.TelNo,
                              TokiSt_FaxNo = TokiSt.FaxNo,
                              TokiSt_TokuiTanNm = TokiSt.TokuiTanNm,
                              TokiSt_TokuiMail = TokiSt.TokuiMail,
                              TokiSt_TesuRitu = TokiSt.TesuRitu
                          }).ToListAsync();
        }
    }
}
