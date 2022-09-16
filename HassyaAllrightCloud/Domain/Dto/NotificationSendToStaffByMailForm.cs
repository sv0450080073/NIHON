using System.Collections.Generic;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class NotificationSendToStaffByMailForm : NotificationSendToStaffForm
    {
        public string EmailSubject { get; set; }
        public string EmailContent { get; set; }
        public NotificationSendToStaffByMailForm(int notiContentKbn, int toSyainCdSeq, string controlNo, List<string> tokens, string urlCallBack) : base(notiContentKbn, toSyainCdSeq, controlNo, tokens, urlCallBack)
        {
        }

        public NotificationSendToStaffByMailForm(NotificationSendToStaffForm sendForm, string emailSubject, string emailContent) : base(sendForm)
        {
            this.EmailSubject = emailSubject;
            this.EmailContent = emailContent;
        }
    }
}
