using HassyaAllrightCloud.Domain.Dto;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Infrastructure.Services;
using HassyaAllrightCloud.Commons.Helpers;
using Microsoft.AspNetCore.Components.Forms;
using System.Globalization;
using System.Net.Http;
using HassyaAllrightCloud.IService;
using HassyaAllrightCloud.Pages.Components.Popup;

namespace HassyaAllrightCloud.Pages.Components.BookingTabs
{
    public class CancellationTabBase : ComponentBase
    {
        protected enum LoadType
        {
            FromBookingInput,
            FromBusSchedule
        }

        [Inject] public IStringLocalizer<BookingInputTab> Lang { get; set; }
        [Inject] public CustomHttpClient Http { get; set; }
        [Inject] public AppSettingsService AppSettingsService { get; set; }
        [Inject] public IHaitaCheckService HaitaCheckService { get; set; }

        [Parameter] public string UkeNo { get; set; }
        [Parameter] public EventCallback OnChange { get; set; }
        [Parameter] public EventCallback OnSubmit { get; set; }
        [Parameter] public BookingFormData BookingData { get; set; }
        [CascadingParameter] public EditContext FormContext { get; set; }

        protected bool IsLoading { get; set; } = true;
        protected List<SettingStaff> SettingStaffList { get; set; }
        protected CultureInfo CurrentCulture { get; set; }
        protected Dictionary<string, string> LangDic = new Dictionary<string, string>();

        private LoadType cancelLoadType;
        protected MyPopupModel MyPopup { get; set; }
        private string baseUrl = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            baseUrl = AppSettingsService.GetBaseUrl();
            LangDic = Lang.GetAllStrings().ToDictionary(l => l.Name, l => l.Value);
            CurrentCulture = new CultureInfo(LangDic["Culture"]);

            if(!string.IsNullOrWhiteSpace(UkeNo) && (BookingData is null || FormContext is null))
            {
                cancelLoadType = LoadType.FromBusSchedule;
                BookingData = await GetBookingFormDataAsync();
                if(BookingData != null)
                {
                    FormContext = new EditContext(BookingData);
                }
            }
            else
            {
                cancelLoadType = LoadType.FromBookingInput;
            }

            if (BookingData is null && FormContext is null)
            {
                // TODO: handle error, not loaded UkeCd, not pass booking, context
            }
            else
            {
                await SetupCancelTabAsync(BookingData);
                LoadCancelComboboxValue(BookingData);
            }

            IsLoading = false;
            MyPopup = new MyPopupModel();
        }

        private async Task<BookingFormData> GetBookingFormDataAsync()
        {
            try
            {
                var url = string.Format("{0}/api/BookingInput/{1:D5}", baseUrl, UkeNo);
                var booking = await Http.GetJsonAsync<BookingFormData>(url);
                return booking;
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected override void OnAfterRender(bool firstRender)
        {
            
        }

        public async Task SetupCancelTabAsync(BookingFormData booking)
        {
            if (booking.CancelTickedData.CancelTaxType.IdValue == 0)
            {
                booking.CancelTickedData.CancelTaxType.IdValue = Constants.ForeignTax.IdValue;
                booking.CancelTickedData.CancelTaxType.StringValue = Constants.ForeignTax.StringValue;
            }
            SettingStaffList = await Http.GetJsonAsync<List<SettingStaff>>(baseUrl + "/api/BookingInput/Cancellation/SettingStaff");

            if (booking.CancelTickedData.IsSetDefaultFee)
            {
                BookingData.CalculateCancelFee();
                BookingData.CalculateCancelTaxFee();
            }
        }

        public void LoadCancelComboboxValue(BookingFormData booking)
        {
            if (booking.CancelTickedData.CanceledSettingStaffData != null && SettingStaffList != null)
            {
                int syainCdSeq = booking.CancelTickedData.CanceledSettingStaffData.SyainCdSeq;
                booking.CancelTickedData.CanceledSettingStaffData = SettingStaffList.FirstOrDefault(d => d.SyainCdSeq == syainCdSeq);
            }
            if (booking.CancelTickedData.ReusedSettingStaffData != null && SettingStaffList != null)
            {
                int syainCdSeq = booking.CancelTickedData.ReusedSettingStaffData.SyainCdSeq;
                booking.CancelTickedData.ReusedSettingStaffData = SettingStaffList.FirstOrDefault(d => d.SyainCdSeq == syainCdSeq);
            }
        }

        protected bool IsEnableSubmitCancel()
        {
            return  !BookingData.IsDisableEdit
                    && FormContext.GetValidationMessages().Count() == 0
                    && FormContext.IsModified();
        }

        protected void CheckedCancelChanged(bool value)
        {
            BookingData.CancelTickedData.CancelStatus = value;
            if (value)
            {
                BookingData.CancelTickedData.CanceledSettingStaffData = SettingStaffList.Where(s => s.SyainCdSeq == BookingData.SelectedStaff.SyainCdSeq).SingleOrDefault();
                BookingData.CalculateCancelFee();
                BookingData.CalculateCancelTaxFee();
                FormContext.NotifyFieldChanged(FieldIdentifier.Create(() => BookingData.CancelTickedData.CancelStatus));
                Console.WriteLine("Check");
            }
            else
            {
                FormContext.MarkAsUnmodified();
                Console.WriteLine("Un-Check");
            }
            StateHasChanged();
            OnChange.InvokeAsync(null);
            FormContext.Validate();
        }

        protected void ReusedStatusChanged(ChangeEventArgs arg)
        {
            bool value = Boolean.Parse(arg.Value.ToString());
            ChangeReuseStatus(value);
        }

        protected void ChangeReuseStatus(bool value)
        {
            BookingData.CancelTickedData.ReusedStatus = value;
            if (value)
            {
                BookingData.CancelTickedData.ReusedSettingStaffData = SettingStaffList.Where(s => s.SyainCdSeq == BookingData.SelectedStaff.SyainCdSeq).SingleOrDefault();
            }
            FormContext.NotifyFieldChanged(FieldIdentifier.Create(() => BookingData.CancelTickedData.ReusedStatus));
            StateHasChanged();
            OnChange.InvokeAsync(null);
        }

        protected void SelectedCanceledSettingStaffDataChanged(SettingStaff newValue)
        {
            if (newValue != null)
            {
                BookingData.CancelTickedData.CanceledSettingStaffData = newValue;
                StateHasChanged();
                OnChange.InvokeAsync(null);
            }
        }

        protected void SelectedReusedSettingStaffDataChanged(SettingStaff newValue)
        {
            if (newValue != null)
            {
                BookingData.CancelTickedData.ReusedSettingStaffData = newValue;
                StateHasChanged();
                OnChange.InvokeAsync(null);
            }
        }

        protected void CanceledDateChanged(DateTime newValue)
        {
            if (newValue != null)
            {
                BookingData.CancelTickedData.CancelDate = newValue;
                StateHasChanged();
                OnChange.InvokeAsync(null);
            }
        }

        protected void ReusedDateChanged(DateTime newValue)
        {
            if (newValue != null)
            {
                BookingData.CancelTickedData.ReusedDate = newValue;
                StateHasChanged();
                OnChange.InvokeAsync(null);
            }
        }

        /// <summary>
        /// When InputRate value of <see cref="CancelTickedData.CancelFeeRate"/> changed.
        /// </summary>
        /// <param name="newRate">New rate value</param>
        protected void CancelFeeRateChanged(string newRate)
        {
            BookingData.CancelTickedData.CancelFeeRate = newRate;

            BookingData.CalculateCancelFee();
            BookingData.CalculateCancelTaxFee();
            StateHasChanged();
            OnChange.InvokeAsync(null);
        }

        protected void CancelFeeChanged(string newValue)
        {
            if (newValue != null)
            {
                if (CommonUtil.NumberTryParse(newValue, out int tmp))
                {
                    if (tmp >= 0)
                    {
                        BookingData.CancelTickedData.CancelFee = newValue;
                        BookingData.CalculateCancelTaxFee();
                        StateHasChanged();
                    }
                }
            }
            OnChange.InvokeAsync(null);
        }

        /// <summary>
        /// When InputRate value of <see cref="CancelTickedData.CancelTaxRate"/> changed.
        /// </summary>
        /// <param name="newRate">New rate value</param>
        protected void CancelTaxRateChanged(string newRate)
        {
            BookingData.CancelTickedData.CancelTaxRate = newRate;
            BookingData.CalculateCancelTaxFee();
            StateHasChanged();
            OnChange.InvokeAsync(null);
        }

        protected void CancelTaxFeeChanged(string newValue)
        {
            if (newValue != null)
            {
                if (CommonUtil.NumberTryParse(newValue, out int tmp))
                {
                    if (tmp >= 0)
                    {
                        BookingData.CancelTickedData.CancelTaxFee = newValue;
                        StateHasChanged();

                    }
                }
            }
            OnChange.InvokeAsync(null);
        }

        protected void CancelTaxTypeChanged(TaxTypeList newValue)
        {
            if (newValue != null)
            {
                BookingData.CancelTickedData.CancelTaxType = newValue;
                BookingData.CalculateCancelTaxFee();
                StateHasChanged();
            }
            OnChange.InvokeAsync(null);
        }

        protected void CancelReasonChanged(string newValue)
        {
            BookingData.CancelTickedData.CancelReason = newValue;
            StateHasChanged();
            OnChange.InvokeAsync(null);
        }

        protected void ReusedReasonChanged(string newValue)
        {
            BookingData.CancelTickedData.ReusedReason = newValue;
            StateHasChanged();
            OnChange.InvokeAsync(null);
        }

        protected async Task HandleSaveCancelData()
        {
            if (BookingData.IsDisableEdit == false)
            {
                if(cancelLoadType == LoadType.FromBookingInput)
                {
                    await OnSubmit.InvokeAsync(null).ContinueWith(t =>
                    {
                        LoadCancelComboboxValue(BookingData);
                        FormContext.MarkAsUnmodified();
                    });
                }
                else if(cancelLoadType == LoadType.FromBusSchedule)
                {
                    string ukeNo = string.Format("{0}", UkeNo);
                    List<HaitaModelCheck> HaitaModelsToCheck = new List<HaitaModelCheck>();
                    HaitaModelsToCheck.Add(new HaitaModelCheck()
                    {
                        TableName = "TKD_Yyksho",
                        CurrentUpdYmdTime = BookingData.YykshoUpdYmdTime,
                        PrimaryKeys = new List<PrimaryKeyToCheck>(new PrimaryKeyToCheck[] { new PrimaryKeyToCheck() { PrimaryKey = "UkeNo =", Value = "'" + UkeNo + "'" } })
                    });
                    bool isHaita = await HaitaCheckService.GetHaitaCheck(HaitaModelsToCheck);
                    if (!isHaita)
                    {
                        await OnSubmit.InvokeAsync(null).ContinueWith(t =>
                        {
                            FormContext.MarkAsUnmodified();
                        });
                        return;
                    }
                    StringContent content = Http.getStringContentFromObject(BookingData.CancelTickedData);
                    HttpResponseMessage response =
                        await Http.PutAsync($"{baseUrl}/api/BookingInput/updateCancel/{ukeNo}", content);
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Update cancel success");
                    }
                    else
                    {
                        Console.WriteLine("Update cancel fail");
                    }
                    var putData = await GetBookingFormDataAsync();
                    if (putData != null && putData?.CurrentBookingType?.YoyaKbnSeq != -1)
                    {
                        BookingData.CancelTickedData.SetData(putData.CancelTickedData);
                        LoadCancelComboboxValue(BookingData);
                        FormContext.MarkAsUnmodified();
                    }
                }
            }
        }
    }
}
