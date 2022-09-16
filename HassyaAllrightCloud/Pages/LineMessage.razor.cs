using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Pages
{
    public class LineMessageBase : ComponentBase
    {
        [Inject]
        protected ILineService lineService { get; set; }
        public List<LineUser> LineUserList { get; set; }
        protected List<string> SelectedTokens = new List<string>();
        public string Message { get; set; }
        public bool IsShowPopup { get; set; }
        public string AlertMessage { get; set; }
        public string AlertIcon { get; set; } = "times";

        protected override void OnInitialized()
        {
            LineUserList = lineService.GetLineUsers().ToList();
        }

        protected async Task SendMessage()
        {
            if (!SelectedTokens.Any())
            {
                AlertMessage = "UserRequired";
            }
            else if (string.IsNullOrEmpty(Message))
            {
                AlertMessage = "MessageRequired";
            }
            else
            {
                if (await lineService.SendMessage(SelectedTokens, Message))
                {
                    AlertIcon = "info";
                    AlertMessage = "SuccessMessage";
                    Message = null;
                }
                else
                {
                    AlertMessage = "SendFail";
                }
            }
            IsShowPopup = true;
            StateHasChanged();
        }
    }
}
