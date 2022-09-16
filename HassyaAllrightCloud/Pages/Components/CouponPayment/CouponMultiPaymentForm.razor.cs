using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using HassyaAllrightCloud.Commons.Constants;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace HassyaAllrightCloud.Pages.Components.CouponPayment
{
    public class CouponMultiPaymentFormBase : ComponentBase, IDisposable
    {
        [Inject] protected IStringLocalizer<CouponPaymentForm> _lang { get; set; }
        [Inject] protected ICouponPaymentService _couponService { get; set; }
        [Inject] private IJSRuntime _jSRuntime { get; set; }
        [Inject] private ILoadingService _loading { get; set; }
        [Inject] private IErrorHandlerService _errService { get; set; }
        [Parameter] public List<CouponPaymentGridItem> SelectedItems { get; set; }
        [Parameter] public CouponPaymentFormModel SearchModel { get; set; }
        [Parameter] public EventCallback<bool> CloseDialog { get; set; }
        [Parameter] public int Total { get; set; }
        protected CouponPaymentPopupFormModel Model { get; set; } = new CouponPaymentPopupFormModel();
        protected List<EigyoListItem> DepositOffices { get; set; } = new List<EigyoListItem>();
        protected List<BankTransferItem> BankTransferItems { get; set; } = new List<BankTransferItem>();
        private CancellationTokenSource source { get; set; } = new CancellationTokenSource();
        protected bool isCardSyoEmpty { get; set; }
        protected bool isCardDenEmpty { get; set; }
        protected bool isTegataNoEmpty { get; set; }
        private ClaimModel _currentUser { get; set; }
        private List<LastUpdatedYmdTimeMulti> _lastUpdatedYmdTimes { get; set; }
        protected bool isHaitaValid { get; set; } = true;
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                await _jSRuntime.InvokeVoidAsync("inputNumber", ".number", true);
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                _currentUser = new ClaimModel();
                DepositOffices = await _couponService.GetDepositOffice(_currentUser.TenantID, source.Token);
                BankTransferItems = await _couponService.GetBankTransferItems(source.Token);
                Model = InitFormModel();
                _lastUpdatedYmdTimes = await GetLatestUpdYmdTime();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        private async Task<List<LastUpdatedYmdTimeMulti>> GetLatestUpdYmdTime()
        {
            var temList = new List<LastUpdatedYmdTimeMulti>();
            foreach (var item in SelectedItems)
            {
                var temp = await _couponService.GetCommonLastUpdatedYmdTime(item, _currentUser.TenantID, source.Token);
                temList.Add(new LastUpdatedYmdTimeMulti() { LastUpdatedYmdTime = temp, UkeNo = item.UkeNo, NyuSihCouRen = item.NyuSihCouRen });
            }

            return temList;
        }

        private bool IsFormValid()
        {
            switch (Model.DepositMethod)
            {
                case DepositMethodEnum.Card:
                    isCardSyoEmpty = string.IsNullOrEmpty(Model.CardSyo?.Trim());
                    isCardDenEmpty = string.IsNullOrEmpty(Model.CardDen?.Trim());
                    isTegataNoEmpty = false;
                    break;
                case DepositMethodEnum.Bill:
                    isTegataNoEmpty = string.IsNullOrEmpty(Model.TegataNo?.Trim());
                    isCardSyoEmpty = false;
                    isCardDenEmpty = false;
                    break;
                default:
                    isTegataNoEmpty = false;
                    isCardSyoEmpty = false;
                    isCardDenEmpty = false;
                    break;
            }
            return !isCardSyoEmpty && !isCardDenEmpty && !isTegataNoEmpty;
        }

        private CouponPaymentPopupFormModel InitFormModel()
        {
            var model = new CouponPaymentPopupFormModel();
            model.BankTransfer = BankTransferItems.FirstOrDefault();
            model.CardDen = string.Empty;
            model.CardSyo = string.Empty;
            model.DepositType = DepositTypeEnum.Normal;
            model.EtcSyo1 = string.Empty;
            model.EtcSyo2 = string.Empty;
            model.SponsorshipFee = 0;
            model.TegataNo = string.Empty;
            model.TegataYmd = DateTime.Now;
            model.TransferFee = 0;
            model.DepositAmount = Total;
            model.DepositDate = DateTime.Now;
            model.DepositMethod = DepositMethodEnum.Cash;
            model.DepositOffice = DepositOffices.FirstOrDefault();
            return model;
        }

        protected string GetValidateClass(bool? isError)
        {
            return isError != null ? isError.Value ? "border-invalid" : "border-valid" : string.Empty;
        }

        protected async Task UpdateFormModel(string propertyName, dynamic value)
        {
            try
            {
                var propertyInfo = Model.GetType().GetProperty(propertyName);
                switch (propertyName)
                {
                    case nameof(CouponPaymentPopupFormModel.DepositType):
                        if (Enum.TryParse(value as string, out DepositTypeEnum type))
                            Model.DepositType = type;
                        break;
                    case nameof(CouponPaymentPopupFormModel.DepositMethod):
                        if (Enum.TryParse(value as string, out DepositMethodEnum method))
                            Model.DepositMethod = method;
                        break;
                    case nameof(CouponPaymentPopupFormModel.DepositAmount):
                    case nameof(CouponPaymentPopupFormModel.TransferFee):
                    case nameof(CouponPaymentPopupFormModel.SponsorshipFee):
                        var removeCommas = (value as string).Replace(",", "");
                        if (int.TryParse(removeCommas, out int val))
                            propertyInfo.SetValue(Model, val);
                        else
                            propertyInfo.SetValue(Model, 0);

                        break;
                    default:
                        propertyInfo.SetValue(Model, value);
                        break;
                }
                IsFormValid();
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected void Close()
        {
            try
            {
                CloseDialog.InvokeAsync(true);
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected async Task Save()
        {
            var isClosePopup = true;
            try
            {
                await _loading.ShowAsync();
                if (IsFormValid())
                {
                    var latestUpdYmdTimes = await GetLatestUpdYmdTime();
                    
                    foreach (var e in SelectedItems)
                    {
                        var ori = _lastUpdatedYmdTimes.FirstOrDefault(t => t.UkeNo == e.UkeNo && t.NyuSihCouRen == e.NyuSihCouRen);
                        var cur = latestUpdYmdTimes.FirstOrDefault(t => t.UkeNo == e.UkeNo && t.NyuSihCouRen == e.NyuSihCouRen);
                        if (ori == null || cur == null || !_couponService.CompareLatestUpdYmdTime(e.SeiFutSyu, ori.LastUpdatedYmdTime, cur.LastUpdatedYmdTime))
                        {
                            isHaitaValid = false;
                            isClosePopup = false;
                            return;
                        }
                    }

                    await _couponService.SaveMultiCouponPayment(SelectedItems, Model, _currentUser.SyainCdSeq, _currentUser.TenantID, source.Token);
                }
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
            finally
            {
                if(isClosePopup)
                    await CloseDialog.InvokeAsync(true);
                await _loading.HideAsync();
                StateHasChanged();
            }
        }

        public void Dispose()
        {
            source.Cancel();
        }
    }
}
