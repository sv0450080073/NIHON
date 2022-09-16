using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Services;
using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace HassyaAllrightCloud.Pages.Components.BookingTabs
{
    public class BookingRemarkBase : ComponentBase
    {
        [Inject] public IStringLocalizer<BookingInputTab> Lang { get; set; }
        [Inject] public CustomHttpClient Http { get; set; }
        [Inject] public AppSettingsService AppSettingsService { get; set; }
        [Inject] public IBusBookingDataListService BusBookingDataService { get; set; }
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }
        [Parameter] public string UkeNo { get; set; }
        [Parameter] public short UnkRen { get; set; }
        [Parameter] public bool IsDisableEdit { get; set; }
        [Parameter] public bool IsUnkobiBikoNm { get; set; }
        [Parameter] public bool IsOpenEditUnkobiBikoNm { get; set; }
        [Parameter] public bool IsOpenEditYykshoBikoNm { get; set; }

        public string BikoNm { get; set; } = string.Empty;
        protected EditContext FormContext { get; set; }
        protected bool IsLoading { get; set; } = true;
        protected bool IsEnable { get; set; } = false;
        private string baseUrl = string.Empty;
        public string errorMessage { get; set; } = string.Empty;
        public BookingRemarkHaitaCheck bookingRemarkHaitaCheck { get; set; }
        protected override void OnAfterRender(bool firstRender)
        {
            JSRuntime.InvokeVoidAsync("focusout");
        }
        protected override async Task OnInitializedAsync()
        {
            baseUrl = AppSettingsService.GetBaseUrl();

            try
            {
                bookingRemarkHaitaCheck = await BusBookingDataService.GetBookingRemarkHaiCheck(UkeNo, UnkRen);
                string url = string.Format("{0}/api/BookingInput/GetBikoNm?ukeNo={1}&isUnkobi={2}&unkRen={3}", baseUrl, UkeNo, IsUnkobiBikoNm, UnkRen);
                BikoNm = BusBookingDataService.GetBikoNm(UkeNo, IsUnkobiBikoNm, UnkRen).Result;
                //BikoNm = await Http.GetStringAsync(url);
                FormContext = new EditContext(BikoNm);
            }
            catch (Exception)
            {
                // TODO: handle exception
            }

            IsLoading = false;
        }

        protected bool IsEnableSubmitButton()
        {
            if (IsDisableEdit) return false;
            return FormContext.IsModified();
        }
        protected void BikoNmChanged(ChangeEventArgs newValue)
        {
            BikoNm = newValue.Value.ToString();
            if (string.IsNullOrWhiteSpace(BikoNm) && IsEnable)
            {
                IsEnable = false;
                StateHasChanged();
            }
            if (!string.IsNullOrWhiteSpace(BikoNm) && !IsEnable)
            {
                IsEnable = true;
                StateHasChanged();
            }
        }

        protected async Task HandleSave()
        {
            try
            {
                //haita check
                var bookingRemarkHaitaCheckNew = await BusBookingDataService.GetBookingRemarkHaiCheck(UkeNo, UnkRen);
                if ((IsOpenEditUnkobiBikoNm && bookingRemarkHaitaCheckNew.UnkobiUpdYmdTIme != bookingRemarkHaitaCheck.UnkobiUpdYmdTIme) || (IsOpenEditYykshoBikoNm && bookingRemarkHaitaCheckNew.YykshoUpdYmdTIme != bookingRemarkHaitaCheck.YykshoUpdYmdTIme))
                    errorMessage = "CM_T002";
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    StateHasChanged();
                    return;
                }
                var url = string.Format("{0}/api/BookingInput/SaveBikoNm?ukeNo={1}&isUnkobi={2}&unkRen={3}", baseUrl, UkeNo, IsUnkobiBikoNm, UnkRen);
                await Http.PutJsonAsync(url, BikoNm);
                if (string.IsNullOrWhiteSpace(errorMessage))
                {
                    bookingRemarkHaitaCheck = await BusBookingDataService.GetBookingRemarkHaiCheck(UkeNo, UnkRen);
                }
                //url = string.Format("{0}/api/BookingInput/GetBikoNm?ukeNo={1}&isUnkobi={2}&unkRen={3}", baseUrl, UkeNo, IsUnkobiBikoNm, UnkRen);
                //BikoNm = await Http.GetStringAsync(url);
                BikoNm = BusBookingDataService.GetBikoNm(UkeNo, IsUnkobiBikoNm, UnkRen).Result;
                IsEnable = false;
                FormContext.MarkAsUnmodified();
                StateHasChanged();
            }
            catch (Exception)
            {
                // TODO: handle exception
            }

            StateHasChanged();
        }
    }
}
