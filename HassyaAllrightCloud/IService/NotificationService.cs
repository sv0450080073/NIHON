using HassyaAllrightCloud.Application.Notification.Commands;
using HassyaAllrightCloud.Application.NotificationReplyToken.Commands;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HassyaAllrightCloud.Infrastructure.Services;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Linq;

namespace HassyaAllrightCloud.IService
{
    
    public interface INotificationToStaffService
    {
        Task<NotificationResult> SendNotificationToStaff(int NotiContentKbn, NotificationSendMethod Method, int ToStaffId, DateTime ReplyExpiredDateTime, string EmailSubject, string EmailContent, string LineMessage);
        Task<NotificationResult> SendMail(List<string> EmailAdresses, string EmailSubject, string EmailContent);
        Task<bool> UpdateState(string ControlNo, NotificationSendMethod Method, string Token);

    }
    public class NotificationToStaffService : INotificationToStaffService
    {
        private IMediator mediatR;
        private readonly AppSettingsService config;
        public NotificationToStaffService(AppSettingsService config, IMediator mediatR, CustomHttpClient http)
        {
            this.mediatR = mediatR;
            this.config = config;
        }

        public async Task<NotificationResult> SendNotificationToStaff(int NotiContentKbn, NotificationSendMethod Method, int ToStaffId, DateTime ReplyExpiredDateTime, string EmailSubject, string EmailContent, string LineMessage)
        {
            NotificationResult Result = await mediatR.Send(new InsertNotificationCommand { toStaffId = ToStaffId });
            List<string> Tokens = await mediatR.Send(new InsertNotificationReplyTokenCommand { method = Method, replyExpiredDateTime = ReplyExpiredDateTime, controlNo = Result.ControlNo });
            bool IsSendSuccess = false;
            string NotificationServiceUrl = config.GetNotificationServiceUrl() + "/notification/";
            NotificationSendToStaffForm SendForm = new NotificationSendToStaffForm(NotiContentKbn, ToStaffId, Result.ControlNo, Tokens, config.GetBaseUrl());
            if (Method == NotificationSendMethod.Both)
            {
                NotificationServiceUrl += "SendToStaffByBoth";
                SendForm = new NotificationSendToStaffByBothForm(SendForm, EmailSubject, EmailContent, LineMessage);
            }
            else if (Method == NotificationSendMethod.Mail)
            {
                NotificationServiceUrl += "SendToStaffByMail";
                SendForm = new NotificationSendToStaffByMailForm(SendForm, EmailSubject, EmailContent);
            }
            else
            {
                NotificationServiceUrl += "SendToStaffByLine";
                SendForm = new NotificationSendToStaffByLineForm(SendForm, LineMessage);
            }
            List<string> Results = await CallNotificationApi(NotificationServiceUrl, SendForm);
            IsSendSuccess = Results != null && Results.Count(item => string.IsNullOrEmpty(item)) > 0;
            if (Results == null)
            {
                Results = new List<string>() {" "};
            }
            await mediatR.Send(new UpdateNotificationCommand { ControlNo = Result.ControlNo, SendMethod = Method, Results = Results });
            if (!IsSendSuccess)
            {
                Result.SendResultKbn = NotificationResultClassification.Failed;
                Result.SendErrorMessage = string.Join("", Results.ToArray());
            }
            return Result;
        }

        public async Task<NotificationResult> SendMail(List<string> EmailAdresses, string EmailSubject, string EmailContent)
        {
            NotificationResult Result = new NotificationResult();

            string NotificationServiceUrl = config.GetNotificationServiceUrl() + "/notification/sendMail";
            NotificationSendMailForm SendForm = new NotificationSendMailForm(EmailAdresses, EmailSubject, EmailContent);
            List<string> Results = await CallNotificationApi(NotificationServiceUrl, SendForm);
            bool IsSendSuccess = Results.Count(item => string.IsNullOrEmpty(item)) > 0;
            if (!IsSendSuccess)
            {
                Result.SendResultKbn = NotificationResultClassification.Failed;
                Result.SendErrorMessage = string.Join("", Results.ToArray());
            }
            return Result;
        }

        public async Task<bool> UpdateState(string ControlNo, NotificationSendMethod Method, string Token)
        {
            return await mediatR.Send(new UpdateNotificationReplyTokenCommand { method = Method, controlNo = ControlNo, token = Token });
        }
        private async Task<List<string>> CallNotificationApi(string url, dynamic SendForm)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(url),
                Content = new StringContent(JsonConvert.SerializeObject(SendForm), Encoding.UTF8, "application/json")
            };
            try
            {
                HttpResponseMessage response = await client.SendAsync(httpRequestMessage);
                string result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<string>>(result);
            }
            catch (Exception e)
            {
                return new List<string>(new string[] { e.Message });
            }
        }
    }
}
