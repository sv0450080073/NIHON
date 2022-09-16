using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.BillPrint;
using HassyaAllrightCloud.Domain.Dto.DepositCoupon;
using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Pages.Components.DepositCoupon
{
    public class CouponBase : ComponentBase
    {
        [Inject]
        protected IStringLocalizer<CouponBalance> Lang { get; set; }
        [Inject]
        protected IDepositCouponService depositCouponService { get; set; }
        [Inject]
        protected IErrorHandlerService errorModalService { get; set; }
        [Parameter]
        public bool isOpenCouponPopUp { get; set; }
        [Parameter]
        public EventCallback<bool> isOpenCouponPopUpChanged { get; set; }
        [Parameter]
        public OutDataTable outDataTable { get; set; }

        public List<CouponBalanceGrid> coupons { get; set; } = new List<CouponBalanceGrid>();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                coupons = await depositCouponService.GetCouponAsync(new ClaimModel().TenantID, outDataTable);
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
                isOpenCouponPopUp = false;
                isOpenCouponPopUpChanged.InvokeAsync(isOpenCouponPopUp);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }
    }
}
