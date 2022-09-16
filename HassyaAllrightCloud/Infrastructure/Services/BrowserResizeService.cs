using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace HassyaAllrightCloud.Infrastructure.Services
{
    public class BrowserResizeService
    {
        private readonly IJSRuntime JSRuntime;
        public BrowserResizeService(IJSRuntime js)
        {
            JSRuntime = js;
        }

        public static event Func<Task> OnResize;
        [JSInvokable]
        public static async Task OnBrowserResize()
        {
            await OnResize?.Invoke();
        }

        public static event Func<Task> OnCallbackWidth;
        [JSInvokable]
        public static async Task OnBrowserCallbackWidth()

        {
            await OnCallbackWidth?.Invoke();
        }

        public async Task<double> GetInnerWidth()
        {
            return await JSRuntime.InvokeAsync<double>("browserResize.getInnerWidth");
        }

        public async Task<double> GetCallbackInnerWidth()
        {
            return await JSRuntime.InvokeAsync<double>("browserResize.getCallbackInnerWidth");
        }

        public async Task<double> GetInnerWidthStaff()
        {
            return await JSRuntime.InvokeAsync<double>("browserResizeStaff.getInnerWidth");
        }

        public async Task<double> GetCallbackInnerWidthStaff()
        {
            return await JSRuntime.InvokeAsync<double>("browserResizeStaff.getCallbackInnerWidth");
        }
    }
}
