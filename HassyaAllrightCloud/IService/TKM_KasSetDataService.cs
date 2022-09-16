using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.Infrastructure.Services;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface ITKM_KasSetDataListService
    {
        Task<TkmKasSet> Get(int id);
    }
    public class TKM_KasSetDataService : ITKM_KasSetDataListService
    {
        private readonly KobodbContext _dbContext;
        private readonly AppSettingsService _config;
        private readonly CustomHttpClient _httpClient;

        public TKM_KasSetDataService(KobodbContext context, AppSettingsService config, CustomHttpClient httpClient)
        {
            _dbContext = context;
            _config = config;
            _httpClient = httpClient;
        }

        public async Task<TkmKasSet> Get(int id)
        {
            id = 1;
            var toDo = await _dbContext.TkmKasSet.FindAsync(id);
            return toDo;
        }
    }

}
