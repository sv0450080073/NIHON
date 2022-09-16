using System.Collections.Generic;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class NotificationSendMailForm
    {
        public List<string> EmailAdresses { get; set; }
        public string EmailSubject { get; set; }
        public string EmailContent { get; set; }
        public NotificationSendMailForm(List<string> emailAdresses, string emailSubject, string emailContent)
        {
            this.EmailAdresses = new List<string>(emailAdresses);
            this.EmailSubject = emailSubject;
            this.EmailContent = emailContent;
        }
    }
}
