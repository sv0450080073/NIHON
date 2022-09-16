using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace HassyaAllrightCloud.Pages
{
    public class ConfirmReservationDetailMobileBase : ComponentBase
    {
        [Inject] public IStringLocalizer<ConfirmReservationDetailMobile> Lang { get; set; }
        [Inject] public IHyperDataService HyperDataService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] protected IErrorHandlerService errorModalService { get; set; }

        [Parameter] public VehicleSchedulerMobileData Data { get; set; }
        [Parameter] public EventCallback<bool> eventCallback { get; set; }

        public string Message { get; set; }
        public bool IsShow { get; set; }
        public MessageBoxType Type { get; set; }

        protected void OnNavigate()
        {
            NavigationManager.NavigateTo(string.Format("reservationmobile?UkeCd={0}", Data.UkeCd.ToString().PadLeft(10, '0')));
        }

        protected void OnCancel()
        {
            try
            {
                IsShow = true;
                Message = Lang["ConfirmMessage"];
                Type = MessageBoxType.Confirm;
                StateHasChanged();
            }
            catch(Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected async Task OnClosePopup(bool value)
        {
            try
            {
                IsShow = value;
                if (value)
                {
                    await HandleCancel();
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        private async Task HandleCancel()
        {
            var ReservertionData = await HyperDataService.GetDataReservationToCheck(new List<string>() { Data.UkeNo });
            if(ReservertionData.Count > 0)
            {
                await CheckReservationToBeCancel(ReservertionData[0]);
            }
        }

        private async Task CheckReservationToBeCancel(ReservationDataToCheck CheckData)
        {
            string Error = "";
            if (CheckData.CompanySeq != new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID)
            {
                Error = Lang["BI_T001"];
            }
            else if (CheckData.NippoKbn != 1)
            {
                Error = Lang["BI_T002"];
            }
            else if (CheckData.DepositClassificationStatus)
            {
                Error = Lang["BI_T003"];
            }
            else if (CheckData.PaymentClassificationStatus)
            {
                Error = Lang["BI_T004"];
            }
            else if (CheckData.ClosedStatus)
            {
                Error = Lang["BI_T005"];
            }

            if (!string.IsNullOrEmpty(Error))
            {
                Message = Error;
                Type = MessageBoxType.Error;
            }
            else
            {
                var result = await HyperDataService.CancelRevervation(new List<string>() { Data.UkeNo }, new List<ReservationDataToCheck>() { CheckData });
                if (result)
                {
                    Message = Lang["BI_T006"];
                    Type = MessageBoxType.Info;
                    Data.IsCancel = true;
                }
                else
                {
                    IsShow = false;
                }
            }
            StateHasChanged();
        }
    }
}
