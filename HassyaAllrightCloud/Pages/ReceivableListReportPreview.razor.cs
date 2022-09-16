using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.IService;

namespace HassyaAllrightCloud.Pages
{
    public class ReceivableListReportPreviewBase : ComponentBase
    {
        [Parameter]
        public string searchString { get; set; }

        public string reportUrl { get; set; }

        protected override Task OnInitializedAsync()
        {
            reportUrl = $"{nameof(IReceivableListService)}?{searchString}";
            return base.OnInitializedAsync();
        }
    }
}
