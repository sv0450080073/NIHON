using HassyaAllrightCloud.Commons.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class NotificationResult
    {
        // 送受信管理番号
        public string ControlNo;
        // メール送信結果のコード
        public NotificationResultClassification SendResultKbn;
        // 送信のエラーメッセージ
        public string SendErrorMessage;

        // コンストラクター
        public NotificationResult()
        {
            ControlNo = string.Empty;
            SendResultKbn = NotificationResultClassification.Success;
            SendErrorMessage = string.Empty;
        }
    }
}
