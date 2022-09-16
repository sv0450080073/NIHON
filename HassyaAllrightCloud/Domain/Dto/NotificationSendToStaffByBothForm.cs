using System.Collections.Generic;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class NotificationSendToStaffByBothForm : NotificationSendToStaffForm
    {
        public string EmailSubject { get; set; }
        public string EmailContent { get; set; }
        public string LineMessage { get; set; }
        public NotificationSendToStaffByBothForm(int notiContentKbn, int toSyainCdSeq, string controlNo, List<string> tokens, string urlCallBack) : base(notiContentKbn, toSyainCdSeq, controlNo, tokens, urlCallBack)
        {
        }

        public NotificationSendToStaffByBothForm(NotificationSendToStaffForm sendForm, string emailSubject, string emailContent, string lineMessage) : base(sendForm)
        {
            this.EmailSubject = emailSubject;
            this.EmailContent = emailContent;
            this.LineMessage = lineMessage;
        }
    }
}
