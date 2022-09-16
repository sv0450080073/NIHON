using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Linq;

namespace HassyaAllrightCloud.Infrastructure.Services
{
    public class AppSettingsService
    {
        private readonly IConfiguration _config;
        private readonly NavigationManager _navManager;
        private readonly IWebHostEnvironment _env;
        public AppSettingsService(IConfiguration config, NavigationManager navManager, IWebHostEnvironment env)
        {
            _config = config;
            _navManager = navManager;
            _env = env;
        }
        public string GetBaseUrl()
        {
            if (_env.IsDevelopment())
            {
                return _config.GetValue<string>("MySettings:BaseUrl");
            }
            else
            {
                var baseUrl = _config.GetValue<string>("MySettings:BaseUrl");
                var basePath = _navManager.ToAbsoluteUri(baseUrl).AbsoluteUri;
                return basePath.TrimEnd('/');
            }
        }
        public string GetNotificationServiceUrl()
        {
            return _config.GetValue<string>("MySettings:NotificationServiceUrl");
        }

        public string GetMasterUrl()
        {
            return _config.GetValue<string>("ServiceUrls:MasterMaintenanceService");
        }
    }
}
