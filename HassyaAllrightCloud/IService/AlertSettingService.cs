using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Application.AlertSetting.Queries;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Services;
using HassyaAllrightCloud.Reports.DataSource;
using MediatR;
using Newtonsoft.Json;

namespace HassyaAllrightCloud.IService
{
    public interface IAlertSettingService
    {
        Task <List<AlertSetting>> GetAlertSettingAsync(List<int> alertCds, int tenantCdSeq, int syainCdSeq, int companyCdSeq);
        Task<List<ShowAlertSettingGrid>> GetShowAlertSettingAsync(int tenantCdSeq, int syainCdSeq);
        Task<string> SaveShowAlertSettingAsync(List<ShowAlertSettingGrid> showAlertSettingGrids);
    }

    public class AlertSettingService : IAlertSettingService
    {
        private IMediator mediatR;
        private readonly AppSettingsService _config;
        private readonly CustomHttpClient _httpClient;
        public AlertSettingService(IMediator mediatR, AppSettingsService config, CustomHttpClient httpClient)
        {
            this.mediatR = mediatR;
            _config = config;
            _httpClient = httpClient;
        }

        public async Task<List<AlertSetting>> GetAlertSettingAsync(List<int> alertCds, int tenantCdSeq, int syainCdSeq, int companyCdSeq)
        {
            return await mediatR.Send(new GetAlertSettingAsyncQuery { alertCds = alertCds, tenantCdSeq = tenantCdSeq, syainCdSeq = syainCdSeq, companyCdSeq = companyCdSeq });
        }

        public async Task<List<ShowAlertSettingGrid>> GetShowAlertSettingAsync(int tenantCdSeq, int syainCdSeq)
        {
            return await mediatR.Send(new GetShowAlertSettingAsyncQuery { tenantCdSeq = tenantCdSeq, syainCdSeq = syainCdSeq });
        }

        public async Task<string> SaveShowAlertSettingAsync(List<ShowAlertSettingGrid> showAlertSettingGrids)
        {
            string baseUrl = _config.GetMasterUrl();
            var client = new HttpClient();
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new System.Uri($"{baseUrl}/api/PersonalSetting/ShowAlertSetting"),
                Content = new StringContent(JsonConvert.SerializeObject(showAlertSettingGrids), System.Text.Encoding.UTF8, "application/json")
            };
            try
            {
                HttpResponseMessage response = await client.SendAsync(httpRequestMessage);
                string result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<string>(result);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
