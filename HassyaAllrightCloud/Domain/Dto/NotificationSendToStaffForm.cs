using System.Collections.Generic;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class NotificationSendToStaffForm
    {
        public int NotiContentKbn { get; set; }
        public int ToSyainCdSeq { get; set; }
        public string ControlNo { get; set; }
        public List<string> Tokens { get; set; }
        public string UrlCallBack { get; set; }
        public NotificationSendToStaffForm(int notiContentKbn, int toSyainCdSeq, string controlNo, List<string> tokens, string urlCallBack)
        {
            this.NotiContentKbn = notiContentKbn;
            this.ToSyainCdSeq = toSyainCdSeq;
            this.ControlNo = controlNo;
            this.Tokens = tokens;
            this.UrlCallBack = urlCallBack;
        }

        public NotificationSendToStaffForm(NotificationSendToStaffForm SendForm)
        {
            this.NotiContentKbn = SendForm.NotiContentKbn;
            this.ToSyainCdSeq = SendForm.ToSyainCdSeq;
            this.ControlNo = SendForm.ControlNo;
            this.Tokens = SendForm.Tokens;
            this.UrlCallBack = SendForm.UrlCallBack;
        }
    }
}
