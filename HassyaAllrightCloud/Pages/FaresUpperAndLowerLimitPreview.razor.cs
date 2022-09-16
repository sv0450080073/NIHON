using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Pages
{
    public class FaresUpperAndLowerLimitPreviewBase : ComponentBase
    {
        [Parameter]
        public string searchString { get; set; }

        public string reportUrl { get; set; }

        protected override Task OnInitializedAsync()
        {
            reportUrl = $"{nameof(IFaresUpperAndLowerLimitsService)}?{searchString}";
            return base.OnInitializedAsync();
        }
    }
}
