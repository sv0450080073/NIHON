using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface ISupplierService
    {
        Task<List<SupplierData>> GetAll();
    }

    public class SupplierService : ISupplierService
    {
        private readonly KobodbContext _dbContext;
        private readonly ILogger<SupplierService> _logger;

        public SupplierService(KobodbContext dbContext, ILogger<SupplierService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<SupplierData>> GetAll()
        {
            var result = new List<SupplierData>();
            try
            {
                string DateAsString = DateTime.Today.ToString("yyyyMMdd");
                result = await (from t in _dbContext.VpmTokisk
                                join s in _dbContext.VpmTokiSt
                                on t.TokuiSeq equals s.TokuiSeq
                                where DateAsString.CompareTo(s.SiyoStaYmd) >= 0 &&
                                        DateAsString.CompareTo(s.SiyoEndYmd) <= 0
                                orderby t.TokuiCd ascending
                                select new SupplierData()
                                {
                                    SirCdSeq = t.TokuiSeq,
                                    SirSitenCdSeq = s.SitenCdSeq,
                                    TokuiCd = t.TokuiCd,
                                    SitenCd = s.SitenCd,
                                    RyakuNm = t.RyakuNm,
                                    SitenNm = s.SitenNm
                                }).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
            return result;
        }
    }
}
