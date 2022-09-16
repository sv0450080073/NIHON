using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using HassyaAllrightCloud.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using HassyaAllrightCloud.Domain.Dto.DepositCoupon;
using HassyaAllrightCloud.Domain.Dto;

namespace HassyaAllrightCloud.Pages.Components.DepositCoupon
{
    public class CouponBalanceBase : ComponentBase
    {
        [Inject]
        protected IStringLocalizer<CouponBalance> Lang { get; set; }
        [Inject]
        protected IDepositCouponService depositCouponService { get; set; }
        [Inject]
        protected IErrorHandlerService errorModalService { get; set; }
        [Inject]
        public IJSRuntime jSRuntime { get; set; }
        [Parameter]
        public bool isOpenCouponBalancePopUp { get; set; }
        [Parameter]
        public EventCallback<bool> isOpenCouponBalancePopUpChanged { get; set; }

        public List<CouponBalanceGrid> couponBalances { get; set; } = new List<CouponBalanceGrid>();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                couponBalances = await depositCouponService.GetCouponBalanceAsync(new ClaimModel().TenantID);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                 await jSRuntime.InvokeVoidAsync("onOffScroll", false);
                 await jSRuntime.InvokeVoidAsync("hidePageScroll");
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public void CloseModal()
        {
            try
            {
                isOpenCouponBalancePopUp = false;
                isOpenCouponBalancePopUpChanged.InvokeAsync(isOpenCouponBalancePopUp);
                jSRuntime.InvokeVoidAsync("onOffScroll");
                jSRuntime.InvokeVoidAsync("showPageScroll");
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }
    }
}
