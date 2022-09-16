using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.Infrastructure.Services;
using System.Net.Http;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface TKD_MishumDataListService
    {
        Task<HttpResponseMessage> InsertTKD_MishumAsync(TkdMishum tkdmishum);
    }
    public class TKD_MishumDataService : TKD_MishumDataListService
    {
        private readonly KobodbContext _dbContext;
        private readonly AppSettingsService _config;
        private readonly CustomHttpClient _httpClient;

        public TKD_MishumDataService(KobodbContext context, AppSettingsService config, CustomHttpClient httpClient)
        {
            _dbContext = context;
            _config = config;
            _httpClient = httpClient;
        }
        public async Task<HttpResponseMessage> InsertTKD_MishumAsync(TkdMishum tkdmishum)
        {
            string baseUrl = _config.GetBaseUrl();
            var client = new HttpClient();
            return await client.PostAsync($"{baseUrl}/api/TkdMishum", _httpClient.getStringContentFromObject(tkdmishum));
        }

    }
}
