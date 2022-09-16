using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Services;
using HassyaAllrightCloud.IService;
using HassyaAllrightCloud.Pages.Components.Popup;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Pages.Components.BookingTabs
{
    public class ConfirmInputTabBase : ComponentBase
    {
        protected enum LoadType
        {
            FromBookingInput,
            FromBusSchedule
        }

        [Inject] public IStringLocalizer<BookingInputTab> Lang { get; set; }
        [Inject] public CustomHttpClient Http { get; set; }
        [Inject] public AppSettingsService AppSettingsService { get; set; }
        [Inject] public IConfirmationTabService ConfirmationTabService { get; set; }
        [Inject] public IHaitaCheckService HaitaCheckService { get; set; }

        [Parameter] public string UkeNo { get; set; }
        [Parameter] public EventCallback OnChange { get; set; }
        [Parameter] public EventCallback OnSubmit { get; set; }
        [Parameter] public EventCallback OnHaitaError { get; set; }
        [Parameter] public EventCallback<(ConfirmStatus status, bool success)> OnConfirmSuccess { get; set; }
        [Parameter] public BookingFormData BookingData { get; set; }
        [Parameter] public bool CopyMode { get; set; }
        [CascadingParameter] public EditContext FormContext { get; set; }

        protected MyPopupModel MyPopup { get; set; }
        protected ConfirmStatus ConfirmStatus { get; set; }
        protected bool IsLoading { get; set; } = true;
        protected List<SettingStaff> SettingStaffList { get; set; }
        protected CultureInfo CurrentCulture { get; set; }
        protected Dictionary<string, string> LangDic = new Dictionary<string, string>();

        private LoadType confirmLoadType;
        private int maxRow = 15;
        private string baseUrl = string.Empty;

        private string ok;
        private string popupTitleInfo;
        private string popupTitleError;
        private string bookingFixedSuccess;
        private string bookingUnFixedSuccess;
        private string updateResultSuccessMessage;
        private string updateResultErrorMessage;
        private string commonError;
        private void LocalizationInit()
        {
            popupTitleError = Lang["PopupTitleError"];
            popupTitleInfo = Lang["PopupTitleInfo"];
            ok = Lang["OK"];
            bookingFixedSuccess = Lang["BookingFixed"];
            bookingUnFixedSuccess = Lang["BookingUnFixed"];
            commonError = Lang["CommonError"];
            updateResultSuccessMessage = Lang["UpdateResultSuccessMessage"];
            updateResultErrorMessage = Lang["UpdateResultErrorMessage"];
        }

        protected override async Task OnInitializedAsync()
        {
            LocalizationInit();
            MyPopup = new MyPopupModel();
            baseUrl = AppSettingsService.GetBaseUrl();
            LangDic = Lang.GetAllStrings().ToDictionary(l => l.Name, l => l.Value);
            CurrentCulture = new CultureInfo(LangDic["Culture"]);

            if (!string.IsNullOrWhiteSpace(UkeNo) && (BookingData is null || FormContext is null))
            {
                confirmLoadType = LoadType.FromBusSchedule;
                BookingData = await GetBookingFormDataAsync();
                if (BookingData != null)
                {
                    FormContext = new EditContext(BookingData);
                }
            }
            else
            {
                confirmLoadType = LoadType.FromBookingInput;
            }

            if (BookingData is null || FormContext is null)
            {
                // TODO: handle error, not loaded UkeCd, not pass booking, context
            }
            else
            {
                SetupCOnfirmationTab(BookingData);
            }

            IsLoading = false;
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

        public void SetupCOnfirmationTab(BookingFormData booking)
        {
            if (booking != null)
            {
                if (string.IsNullOrEmpty(booking.KaktYmd?.Trim()) == false)
                {
                    ConfirmStatus = ConfirmStatus.Fixed;
                }
                else
                {
                    if (booking.ConfirmationTabDataList?.Count == 0)
                    {
                        ConfirmStatus = ConfirmStatus.NotFixed;
                    }
                    else
                    {
                        ConfirmStatus = ConfirmStatus.Confirm;
                    }
                }
            }
        }

        protected bool IsEnableSubmitButton()
        {
            return !BookingData.IsDisableEdit
                    && FormContext.GetValidationMessages().Count() == 0
                    && FormContext.IsModified();
        }

        protected void HandleAddConfirmationTab()
        {
            if (ConfirmStatus != ConfirmStatus.Fixed)
            {
                if (BookingData.ConfirmationTabDataList.Count == 0)
                {
                    BookingData.ConfirmationTabDataList.Add(new ConfirmationTabData() { FixDataNo = 1 });
                    FormContext.NotifyFieldChanged(FieldIdentifier.Create(() => BookingData.ConfirmationTabDataList));
                }
                else
                {
                    if (BookingData.ConfirmationTabDataList.Count < maxRow)
                    {
                        int max = BookingData.ConfirmationTabDataList.Max(t => t.FixDataNo);
                        BookingData.ConfirmationTabDataList.Add(new ConfirmationTabData() { FixDataNo = max + 1 });
                        FormContext.NotifyFieldChanged(FieldIdentifier.Create(() => BookingData.ConfirmationTabDataList));
                    }
                }
                OnChange.InvokeAsync(null);
            }
        }

        protected void HandleChangeKaknYmd(DateTime dateTime, int rowId)
        {
            BookingData.ConfirmationTabDataList.Single(t => t.FixDataNo == rowId).KaknYmd = dateTime;
            OnChange.InvokeAsync(null);
        }

        protected void HandleChangedKaknAit(string newValue, int rowId)
        {
            BookingData.ConfirmationTabDataList.Single(t => t.FixDataNo == rowId).KaknAit = newValue;
            StateHasChanged();
            OnChange.InvokeAsync(null);
        }

        protected void HandleChangedKaknNin(string newValue, int rowId)
        {
            BookingData.ConfirmationTabDataList.Single(t => t.FixDataNo == rowId).KaknNin = newValue;
            OnChange.InvokeAsync(null);
        }

        protected void HandleSaikFlgChanged(bool newValue, int rowId)
        {
            BookingData.ConfirmationTabDataList.Single(t => t.FixDataNo == rowId).SaikFlg = newValue;
            OnChange.InvokeAsync(null);
        }

        protected void HandleDaiSuFlgChanged(bool newValue, int rowId)
        {
            BookingData.ConfirmationTabDataList.Single(t => t.FixDataNo == rowId).DaiSuFlg = newValue;
            OnChange.InvokeAsync(null);
        }

        protected void HandleKingFlgChanged(bool newValue, int rowId)
        {
            BookingData.ConfirmationTabDataList.Single(t => t.FixDataNo == rowId).KingFlg = newValue;
            OnChange.InvokeAsync(null);
        }

        protected void HandleNitteChanged(bool newValue, int rowId)
        {
            BookingData.ConfirmationTabDataList.Single(t => t.FixDataNo == rowId).NitteFlag = newValue;
            OnChange.InvokeAsync(null);
        }

        protected async Task HandleConfirmSubmit()
        {
            if (UkeNo != null && UkeNo != "0" && BookingData.IsDisableEdit == false)
            {
                if (ConfirmStatus != ConfirmStatus.Fixed)
                {
                    try
                    {
                        bool isHaita = await HaitaCheck();
                        if (!isHaita)
                        {
                            await OnHaitaError.InvokeAsync(null).ContinueWith(t =>
                            {
                                FormContext.MarkAsUnmodified();
                            });
                            return;
                        }
                        string date = DateTime.Now.ToString("yyyyMMdd");
                        string YykshoUpdYmdTime = await ConfirmationTabService.UpdateConfirmStatus(ConfirmStatus.Fixed, UkeNo, date);
                        BookingData.KaktYmd = date;
                        BookingData.YykshoUpdYmdTime = YykshoUpdYmdTime;
                        ConfirmStatus = ConfirmStatus.Fixed;
                        if(confirmLoadType == LoadType.FromBookingInput)
                        {
                            await OnConfirmSuccess.InvokeAsync((status: ConfirmStatus, success: true));
                        }
                        else
                        {
                            MyPopup.Build()
                                    .WithTitle(popupTitleInfo)
                                    .WithBody(bookingFixedSuccess)
                                    .WithIcon(MyPopupIconType.Info)
                                    .AddButton(new MyPopupFooterButton(ok, () => MyPopup.Hide()))
                                    .Show();
                        }
                    }
                    catch (Exception)
                    {
                        if (confirmLoadType == LoadType.FromBookingInput)
                        {
                            await OnConfirmSuccess.InvokeAsync((status: ConfirmStatus, success: false));
                        }
                        else
                        {
                            MyPopup.Build()
                                    .WithTitle(popupTitleError)
                                    .WithBody(commonError)
                                    .WithIcon(MyPopupIconType.Info)
                                    .AddButton(new MyPopupFooterButton(ok, () => MyPopup.Hide()))
                                    .Show();
                        }
                    }
                }
                else
                {
                    try
                    {
                        bool isHaita = await HaitaCheck();
                        if (!isHaita)
                        {
                            await OnHaitaError.InvokeAsync(null).ContinueWith(t =>
                            {
                                FormContext.MarkAsUnmodified();
                            });
                            return;
                        }
                        var newStatus = BookingData.ConfirmationTabDataList?.Count == 0 ? ConfirmStatus.NotFixed : ConfirmStatus.Confirm;
                        string YykshoUpdYmdTime = await ConfirmationTabService.UpdateConfirmStatus(newStatus, UkeNo, null);
                        BookingData.KaktYmd = string.Empty;
                        BookingData.YykshoUpdYmdTime = YykshoUpdYmdTime;
                        ConfirmStatus = newStatus;
                        if (confirmLoadType == LoadType.FromBookingInput)
                        {
                            await OnConfirmSuccess.InvokeAsync((status: ConfirmStatus, success: true));
                        }
                        else
                        {
                            MyPopup.Build()
                                    .WithTitle(popupTitleInfo)
                                    .WithBody(bookingUnFixedSuccess)
                                    .WithIcon(MyPopupIconType.Info)
                                    .AddButton(new MyPopupFooterButton(ok, () => MyPopup.Hide()))
                                    .Show();
                        }
                    }
                    catch (Exception)
                    {
                        if (confirmLoadType == LoadType.FromBookingInput)
                        {
                            await OnConfirmSuccess.InvokeAsync((status: ConfirmStatus, success: false));
                        }
                        else
                        {
                            MyPopup.Build()
                                    .WithTitle(popupTitleError)
                                    .WithBody(commonError)
                                    .WithIcon(MyPopupIconType.Info)
                                    .AddButton(new MyPopupFooterButton(ok, () => MyPopup.Hide()))
                                    .Show();
                        }
                    }
                }
                await InvokeAsync(StateHasChanged);
            }
        }

        protected async Task HandleSaveConfirmationDataList()
        {
            if (BookingData.IsDisableEdit == false)
            {
                if (confirmLoadType == LoadType.FromBookingInput)
                {
                    await OnSubmit.InvokeAsync(null).ContinueWith(t =>
                    {
                        FormContext.MarkAsUnmodified();
                    });
                }
                else if (confirmLoadType == LoadType.FromBusSchedule)
                {
                    StringContent content = Http.getStringContentFromObject(BookingData.ConfirmationTabDataList);
                    HttpResponseMessage response =
                        await Http.PutAsync($"{baseUrl}/api/BookingInput/updateConfirmList/{UkeNo}", content);
                    if (response.IsSuccessStatusCode)
                    {
                        MyPopup.Build()
                                .WithTitle(popupTitleInfo)
                                .WithBody(updateResultSuccessMessage)
                                .WithIcon(MyPopupIconType.Info)
                                .AddButton(new MyPopupFooterButton(ok, () => MyPopup.Hide()))
                                .Show();
                    }
                    else
                    {
                        MyPopup.Build()
                                .WithTitle(popupTitleError)
                                .WithBody(updateResultErrorMessage)
                                .WithIcon(MyPopupIconType.Info)
                                .AddButton(new MyPopupFooterButton(ok, () => MyPopup.Hide()))
                                .Show();
                    }
                    var putData = await GetBookingFormDataAsync();
                    if (putData != null && putData?.CurrentBookingType?.YoyaKbnSeq != -1)
                    {
                        BookingData.ConfirmationTabDataList = putData.ConfirmationTabDataList;
                        FormContext.MarkAsUnmodified();
                    }
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        protected void HandleClosePopup()
        {
            MyPopup.Hide();
            StateHasChanged();
        }

        protected void HandleConfirmDelete(int rowId)
        {
            if (ConfirmStatus != ConfirmStatus.Fixed)
            {
                BookingData.ConfirmationTabDataList.RemoveAll(c => c.FixDataNo == rowId);
                FormContext.NotifyFieldChanged(FieldIdentifier.Create(() => BookingData.ConfirmationTabDataList));
                OnChange.InvokeAsync(null);
            }
        }

        private async Task<bool> HaitaCheck()
        {
            List<HaitaModelCheck> HaitaModelsToCheck = new List<HaitaModelCheck>();
            HaitaModelsToCheck.Add(new HaitaModelCheck()
            {
                TableName = "TKD_Yyksho",
                CurrentUpdYmdTime = BookingData.YykshoUpdYmdTime,
                PrimaryKeys = new List<PrimaryKeyToCheck>(new PrimaryKeyToCheck[] { new PrimaryKeyToCheck() { PrimaryKey = "UkeNo =", Value = "'" + UkeNo + "'"} })
            });
            return await HaitaCheckService.GetHaitaCheck(HaitaModelsToCheck);
        }
    }
}
