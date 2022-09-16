using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.BillPrint;
using HassyaAllrightCloud.Domain.Dto.DepositCoupon;
using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Pages.Components.DepositCoupon
{
    public class CharterInquiryBase : ComponentBase
    {
        [Inject]
        protected IStringLocalizer<CharterInquiry> Lang { get; set; }
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }
        [Inject]
        protected IErrorHandlerService errorModalService { get; set; }
        [Inject]
        protected IDepositCouponService depositCouponService { get; set; }
        [Parameter]
        public bool isOpenCharterInquiryPopUp { get; set; }
        [Parameter] 
        public EventCallback<bool> isOpenCharterInquiryPopUpChanged { get; set; }
        [Parameter]
        public OutDataTable outDataTable { get; set; } = new OutDataTable();
        public List<ChaterInquiryGrid> chaterInquiryGrids { get; set; } = new List<ChaterInquiryGrid>();
        public string paymentIncidentalType { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                chaterInquiryGrids = await depositCouponService.GetChaterInquiryAsync(new ClaimModel().TenantID, outDataTable);
                paymentIncidentalType = await depositCouponService.GetPaymentIncidentalTypeAsync(new ClaimModel().TenantID, outDataTable.SeiFutSyu);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public void CloseModal()
        {
            try {
                isOpenCharterInquiryPopUp = false;
                isOpenCharterInquiryPopUpChanged.InvokeAsync(isOpenCharterInquiryPopUp);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }
    }
}
