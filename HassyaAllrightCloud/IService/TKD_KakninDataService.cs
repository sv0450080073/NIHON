using Microsoft.EntityFrameworkCore;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface ITKD_KakninDataListService
    {
        Task<List<TkdKaknin>> GetData();
    }
    public class TKD_KakninDataService : ITKD_KakninDataListService
    {
        private readonly KobodbContext _dbContext;

        public TKD_KakninDataService(KobodbContext context)
        {
            _dbContext = context;
        }
        public async Task<List<TkdKaknin>> GetData()
        {
            return await _dbContext.TkdKaknin.ToListAsync();
        }
    }
}
