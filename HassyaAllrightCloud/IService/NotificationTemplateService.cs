using HassyaAllrightCloud.Application.NotificationTemplate.Queries;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface INotificationTemplateService
    {
        Task<NotificationResult> SendApplication(int TenantCdSeq, NotificationSendMethod Method, CodeKbnForNotification CodeKbn, int ToStaffId, DateTime ReplyExpiredDateTime, Dictionary<string, string> DictionaryToBeReplace);
        Task<NotificationResult> SendMail(int TenantCdSeq, CodeKbnForNotification CodeKbn, List<string> EmailAdresses, Dictionary<string, string> DictionaryToBeReplace);
    }
    public class NotificationTemplateService : INotificationTemplateService
    {
        private IMediator mediatR;
        private INotificationToStaffService NotificationToStaffService;
        public NotificationTemplateService(IMediator mediatR, INotificationToStaffService notificationToStaffService)
        {
            this.mediatR = mediatR;
            this.NotificationToStaffService = notificationToStaffService;
        }

        public async Task<NotificationResult> SendApplication(int TenantCdSeq, NotificationSendMethod Method, CodeKbnForNotification CodeKbn, int ToStaffId, DateTime ReplyExpiredDateTime, Dictionary<string, string> DictionaryToBeReplace)
        {
            List<string> Contents = await this.CreateContentToSend(TenantCdSeq, Method, CodeKbn, DictionaryToBeReplace);
            return await this.NotificationToStaffService.SendNotificationToStaff((int)CodeKbn, Method, ToStaffId, ReplyExpiredDateTime, Contents[0], Contents[1], Contents[2]);
        }

        public async Task<NotificationResult> SendMail(int TenantCdSeq, CodeKbnForNotification CodeKbn, List<string> EmailAdresses, Dictionary<string, string> DictionaryToBeReplace)
        {
            List<string> Contents = await this.CreateContentToSend(TenantCdSeq, NotificationSendMethod.Mail, CodeKbn, DictionaryToBeReplace);
            return await this.NotificationToStaffService.SendMail(EmailAdresses, Contents[0], Contents[1]);
        }

        private string ReplaceContent(string Content, Dictionary<string, string> DictionaryToBeReplace)
        {
            string Result = Content;
            foreach (KeyValuePair<string, string> entry in DictionaryToBeReplace)
            {
                Result = Result.Replace(entry.Key, entry.Value);

            }
            return Result;
        }

        private async Task<List<string>> CreateContentToSend(int TenantCdSeq, NotificationSendMethod Method, CodeKbnForNotification CodeKbn, Dictionary<string, string> DictionaryToBeReplace)
        {
            List<string> Contents = new List<string>(new string[] { string.Empty, string.Empty, string.Empty });
            List<string> MailContent = new List<string>();
            List<string> LineContent = new List<string>();
            List<NotificationTemplateData> notiTemplateDatas = await mediatR.Send(new GetNotificationTemplateQuery() { TenantId = TenantCdSeq, Method = Method, CodeKbn = CodeKbn.ToString("D") });
            foreach (NotificationTemplateData notiTemplateData in notiTemplateDatas)
            {
                string AfterReplacedString = ReplaceContent(notiTemplateData.LineContent, DictionaryToBeReplace);
                if (notiTemplateData.ContentKbn == (int)NotificationContentKbn.Subject)
                {
                    Contents[0] = AfterReplacedString;
                }
                else
                {
                    if (notiTemplateData.NotiMethod == (int)NotificationSendMethod.Mail)
                    {
                        MailContent.Add(AfterReplacedString);
                    }
                    else
                    {
                        LineContent.Add(AfterReplacedString);
                    }
                }
            }
            Contents[1] = string.Join("<br>", MailContent);
            Contents[2] = string.Join(System.Environment.NewLine, LineContent);
            return Contents;
        }
    }
}
