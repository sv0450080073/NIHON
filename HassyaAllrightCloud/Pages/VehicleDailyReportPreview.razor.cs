using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Pages
{
    public class VehicleDailyReportPreviewBase : ComponentBase
    {
        [Parameter]
        public string searchString { get; set; }

        public string reportUrl { get; set; }

        protected override Task OnInitializedAsync()
        {
            reportUrl = $"{nameof(IVehicleDailyReportService)}?{searchString}";
            return base.OnInitializedAsync();
        }
    }
}
