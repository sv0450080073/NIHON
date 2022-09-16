using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Pages
{
    public class JitHouReportPreviewBase : ComponentBase
    {
        [Parameter]
        public string searchString { get; set; }

        public string reportUrl { get; set; }

        protected override Task OnInitializedAsync()
        {
            reportUrl = $"{nameof(IMonthlyTransportationService)}?{searchString}";
            return base.OnInitializedAsync();
        }
    }
}
