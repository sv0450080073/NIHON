using System.Collections.Generic;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class NotificationSendToStaffByLineForm : NotificationSendToStaffForm
    {
        public string LineMessage { get; set; }
        public NotificationSendToStaffByLineForm(int notiContentKbn, int toSyainCdSeq, string controlNo, List<string> tokens, string urlCallBack) : base(notiContentKbn, toSyainCdSeq, controlNo, tokens, urlCallBack)
        {
        }

        public NotificationSendToStaffByLineForm(NotificationSendToStaffForm sendForm, string lineMessage) : base(sendForm)
        {
            this.LineMessage = lineMessage;
        }
    }
}
