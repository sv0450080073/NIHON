using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Pages.Components
{
    public class DeleteConfirmBase : ComponentBase
    {
        [Parameter] public bool IsShowPopup { get; set; }
        [Parameter] public EventCallback<bool> OnTogglePopup { get; set; }
        [Parameter] public string Header { get; set; }
        [Parameter] public string Message { get; set; }

        [Inject]
        public IStringLocalizer<DeleteConfirm> _lang { get; set; }
    }
}
