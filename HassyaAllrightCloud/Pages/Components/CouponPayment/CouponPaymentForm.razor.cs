using HassyaAllrightCloud.Commons;
using HassyaAllrightCloud.Domain.Dto;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BlazorContextMenu;
using HassyaAllrightCloud.IService;
using HassyaAllrightCloud.Commons.Constants;
using System.Threading;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Commons.Helpers;
using Microsoft.JSInterop;
using SharedLibraries.UI.Models;

namespace HassyaAllrightCloud.Pages.Components.CouponPayment
{
    public class CouponPaymentFormBase : ComponentBase, IDisposable
    {
        [Inject] protected IStringLocalizer<CouponPaymentForm> _lang { get; set; }
        [Inject] protected IBlazorContextMenuService _contextMenu { get; set; }
        [Inject] protected ICouponPaymentService _couponService { get; set; }
        [Inject] protected ILoadingService _loading { get; set; }
        [Inject] private IErrorHandlerService _errService { get; set; }
        [Inject] private IJSRuntime _jSRuntime { get; set; }
        [Parameter] public CouponPaymentGridItem SelectedItem { get; set; }
        [Parameter] public CouponPaymentFormModel SearchModel { get; set; }
        [Parameter] public EventCallback<bool> CloseDialog { get; set; }
        protected HeaderTemplate Header { get; set; }
        protected HeaderTemplate SummaryTableHeader { get; set; }
        protected BodyTemplate Body { get; set; }
        protected BodyTemplate SummaryTableBody { get; set; }
        protected List<CouponPaymentFormGridItem> DataItems { get; set; } = new List<CouponPaymentFormGridItem>();
        protected List<CouponPaymentFormGridItem> DisplayDataItems { get; set; } = new List<CouponPaymentFormGridItem>();
        protected List<CouponPaymentFormGridItem> SelectedItems { get; set; } = new List<CouponPaymentFormGridItem>();
        protected List<CouponPaymentFormSummaryItem> SummaryDataItems { get; set; } = new List<CouponPaymentFormSummaryItem>();
        protected CouponPaymentPopupFormModel Model { get; set; } = new CouponPaymentPopupFormModel();
        protected List<EigyoListItem> DepositOffices { get; set; } = new List<EigyoListItem>();
        protected List<BankTransferItem> BankTransferItems { get; set; } = new List<BankTransferItem>();
        private CancellationTokenSource source { get; set; } = new CancellationTokenSource();
        protected Pagination pagination { get; set; }
        protected bool dataNotFound { get; set; }
        protected bool isDepositAmountEmpty { get; set; }
        protected bool isCardSyoEmpty { get; set; }
        protected bool isCardDenEmpty { get; set; }
        protected bool isTegataNoEmpty { get; set; }
        protected bool isError10 { get; set; }
        protected bool isSelectable { get; set; } = true;
        protected FormType formType { get; set; }
        protected int tempDepositAmount { get; set; }
        protected byte pageSize { get; set; } = Common.DefaultPageSize;
        private ClaimModel _currentUser;
        CommonLastUpdatedYmdTime _lastUpdYmdTime { get; set; } = new CommonLastUpdatedYmdTime();
        List<SpecLastUpdatedYmdTime> _lastUpdYmdTimeList { get; set; } = new List<SpecLastUpdatedYmdTime>();
        protected bool isHaitaValid { get; set; } = true;
        protected enum FormType
        {
            View,
            Edit,
            Create
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                if (firstRender)
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
                await _loading.ShowAsync();
                _currentUser = new ClaimModel();
                InitTable();
                InitSummaryTable();
                await GetSelectedItem();
                await InitControls();
                await LoadGridData(SelectedItem, false);
                InitDefaultFormModel();
                await _loading.HideAsync();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        private async Task GetSelectedItem()
        {
            var result = await _couponService.GetCouponPayment(SearchModel, true, SelectedItem, source.Token);
            SelectedItem = result.Item1?.FirstOrDefault();
            if (SelectedItem == null)
                await CloseDialog.InvokeAsync(true);
        }

        private void InitDefaultFormModel()
        {
            if (!dataNotFound)
            {
                var firstItem = DisplayDataItems.FirstOrDefault();

                Model = InitFormModel(firstItem);
                
                isHaitaValid = true;
            }
            else
                Model = InitFormModel();
        }

        private async Task InitControls()
        {
            DepositOffices = await _couponService.GetDepositOffice(_currentUser.TenantID, source.Token);
            BankTransferItems = await _couponService.GetBankTransferItems(source.Token);
        }

        private async Task LoadGridData(CouponPaymentGridItem selectedItem, bool showLoading = true)
        {
            if (showLoading)
                await _loading.ShowAsync();
            _lastUpdYmdTimeList.Clear();
            DataItems = await _couponService.GetCouponPaymentFormGridItem(_currentUser.TenantID, selectedItem, source.Token);
            // Get haita check
            _lastUpdYmdTime = await _couponService.GetCommonLastUpdatedYmdTime(selectedItem, _currentUser.TenantID, source.Token);
            if (DataItems != null && DataItems.Any())
            {
                foreach (var item in DataItems)
                {
                    var lastUpdYmdTime = await _couponService.GetSpecLastUpdatedYmdTime(selectedItem, item.NyuSihTblSeq, item.NyuSihRen, _currentUser.TenantID, source.Token);
                    lastUpdYmdTime.NyuSihTblSeq = item.NyuSihTblSeq;
                    lastUpdYmdTime.NyuSihRen = item.NyuSihRen;
                    _lastUpdYmdTimeList.Add(lastUpdYmdTime);
                    isHaitaValid = true;
                }
            }
            pagination.currentPage = 0;
            UpdateDisplayDataItems();
            if (showLoading)
                await _loading.HideAsync();
        }

        private void UpdateDisplayDataItems()
        {
            DisplayDataItems = DataItems.Skip(pagination.currentPage * pageSize).Take(pageSize).ToList();
            dataNotFound = !DisplayDataItems.Any();
            SummaryDataItems = new List<CouponPaymentFormSummaryItem>()
            {
                new CouponPaymentFormSummaryItem()
                {
                    TotalType = _lang["page_total_cell_data"],
                    TotalCash = DisplayDataItems.Sum(e => e.GridCash).AddCommas(),
                    TotalTransfer = DisplayDataItems.Sum(e => e.GridTransfer).AddCommas(),
                    TotalTransferFee = DisplayDataItems.Sum(e => e.FurKesG).AddCommas(),
                    TotalTransferSupport = DisplayDataItems.Sum(e => e.KyoKesG).AddCommas(),
                    TotalCard = DisplayDataItems.Sum(e => e.GridCard).AddCommas(),
                    TotalCommercialPaper = DisplayDataItems.Sum(e => e.GridCommercialPaper).AddCommas(),
                    TotalOffset = DisplayDataItems.Sum(e => e.GridOffset).AddCommas(),
                    TotalAdjustment = DisplayDataItems.Sum(e => e.GridAdjustment).AddCommas(),
                    TotalOther1 = DisplayDataItems.Sum(e => e.GridOther1).AddCommas(),
                    TotalOther2 = DisplayDataItems.Sum(e => e.GridOther2).AddCommas(),
                    TotalTotalDeposit = DisplayDataItems.Sum(e => e.GridTotalDeposit).AddCommas()
                },
                new CouponPaymentFormSummaryItem()
                {
                    TotalType = _lang["cumulative_cell_data"],
                    TotalCash = DataItems.Sum(e => e.GridCash).AddCommas(),
                    TotalTransfer = DataItems.Sum(e => e.GridTransfer).AddCommas(),
                    TotalTransferFee = DataItems.Sum(e => e.FurKesG).AddCommas(),
                    TotalTransferSupport = DataItems.Sum(e => e.KyoKesG).AddCommas(),
                    TotalCard = DataItems.Sum(e => e.GridCard).AddCommas(),
                    TotalCommercialPaper = DataItems.Sum(e => e.GridCommercialPaper).AddCommas(),
                    TotalOffset = DataItems.Sum(e => e.GridOffset).AddCommas(),
                    TotalAdjustment = DataItems.Sum(e => e.GridAdjustment).AddCommas(),
                    TotalOther1 = DataItems.Sum(e => e.GridOther1).AddCommas(),
                    TotalOther2 = DataItems.Sum(e => e.GridOther2).AddCommas(),
                    TotalTotalDeposit = DataItems.Sum(e => e.GridTotalDeposit).AddCommas()
                }
            };
        }

        private CouponPaymentPopupFormModel InitFormModel(CouponPaymentFormGridItem item = null)
        {
            var model = new CouponPaymentPopupFormModel();
            model.SelectedItem = SelectedItem;
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

            if (item == null)
            {
                model.DepositAmount = 0;
                model.DepositDate = DateTime.Now;
                model.DepositMethod = DepositMethodEnum.Cash;
                model.DepositOffice = DepositOffices.FirstOrDefault();
            }
            else
            {
                model.DepositAmount = item.GridTotalDeposit;
                model.DepositDate = string.IsNullOrEmpty(item.NyuSihHakoYmd.Trim()) ? DateTime.MinValue : DateTime.ParseExact(item.NyuSihHakoYmd, DateTimeFormat.yyyyMMdd, CultureInfo.InvariantCulture);
                model.DepositMethod = (DepositMethodEnum)item.NSNyuSihSyu;
                model.DepositOffice = DepositOffices.FirstOrDefault(e => e.EigyoCdSeq == item.NyuSihEigSeq);
                model.NyuSihTblSeq = item.NyuSihTblSeq;
                model.NyuSihRen = item.NyuSihRen;
                model.UkeNo = item.UkeNo;
                switch (model.DepositMethod)
                {
                    case DepositMethodEnum.Transfer:
                        model.TransferFee = item.FurKesG;
                        model.SponsorshipFee = item.KyoKesG;
                        model.BankTransfer = BankTransferItems.Find(e => e.BankCd == item.NSBankCd && e.BankSitCd == item.NSBankSitCd);
                        model.DepositType = item.NSYokinSyu == 1 ? DepositTypeEnum.Normal : DepositTypeEnum.Current;
                        break;
                    case DepositMethodEnum.Card:
                        model.CardSyo = item.NSCardSyo;
                        model.CardDen = item.NSCardDen;
                        break;
                    case DepositMethodEnum.Bill:
                        model.TegataYmd = string.IsNullOrEmpty(item.NSTegataYmd.Trim()) ? DateTime.MinValue : DateTime.ParseExact(item.NSTegataYmd, DateTimeFormat.yyyyMMdd, CultureInfo.InvariantCulture);
                        model.TegataNo = item.NSTegataYmd;
                        break;

                    case DepositMethodEnum.DepositorAndOther1:
                    case DepositMethodEnum.DepositorAndOther2:
                        model.EtcSyo1 = item.NSEtcSyo1;
                        model.EtcSyo2 = item.NSEtcSyo2;
                        break;
                }
                SelectedItems.Clear();
                SelectedItems.Add(item);
            }

            return model;
        }

        protected void RowDbClick(CouponPaymentFormGridItem item)
        {
            try
            {
                if (item != null)
                {
                    Model = InitFormModel(item);

                    SelectedItems.Clear();
                    SelectedItems.Add(item);
                    formType = FormType.Edit;
                    isSelectable = false;
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected string GetValidateClass(bool? isError) => isError != null ? isError.Value ? "border-invalid" : "border-valid" : string.Empty;

        protected async Task RowClick(RowClickEventArgs<CouponPaymentFormGridItem> args)
        {
            try
            {
                if (args.IsSelected && args.Event.Detail != 2)
                {
                    formType = FormType.View;
                    Model = InitFormModel(args.SelectedItem);
                    await _contextMenu.ShowMenu("gridRowClickMenu", Convert.ToInt32(args.Event.ClientX) + 5, Convert.ToInt32(args.Event.ClientY) + 5, args.SelectedItem);
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected void OnChangeItemPerPage(byte pageSize)
        {
            try
            {
                this.pageSize = pageSize;
                pagination.currentPage = 0;
                UpdateDisplayDataItems();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected void UpdateOnClick(ItemClickEventArgs e)
        {
            try
            {
                isSelectable = false;
                var item = e.Data as CouponPaymentFormGridItem;
                if (item.CouTblSeq != 0 && item.NyuKesiKbn != 1)
                    isError10 = true;
                else
                {
                    isError10 = false;
                    formType = FormType.Edit;
                    Model = InitFormModel(item);
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected async Task DeleteOnClick(ItemClickEventArgs e)
        {
            try
            {
                var item = (e.Data as CouponPaymentFormGridItem);
                if (item != null)
                {
                    await _loading.ShowAsync();
                    await _couponService.Delete(item, Model.SelectedItem, _currentUser.SyainCdSeq, source.Token, _currentUser.TenantID);
                    await GetSelectedItem();
                    await LoadGridData(Model.SelectedItem, false);

                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
            finally
            {
                await _loading.HideAsync();
            }
        }

        private void InitTable()
        {
            Header = new HeaderTemplate()
            {
                Rows = new List<RowHeaderTemplate>()
                {
                    new RowHeaderTemplate()
                    {
                        Columns = new List<ColumnHeaderTemplate>()
                        {
                            new ColumnHeaderTemplate() { ColName = _lang["no_col"], Width = 100 },
                            new ColumnHeaderTemplate() { ColName = _lang["payment_date_col"], Width = 100 },
                            new ColumnHeaderTemplate() { ColName = _lang["deposit_office_col"], Width = 100 },
                            new ColumnHeaderTemplate() { ColName = _lang["cash_col"], Width = 100 },
                            new ColumnHeaderTemplate() { ColName = _lang["transfer_col"], Width = 100 },
                            new ColumnHeaderTemplate() { ColName = _lang["transfer_fee_col"], Width = 100 },
                            new ColumnHeaderTemplate() { ColName = _lang["sponsorship_fund_col"], Width = 100 },
                            new ColumnHeaderTemplate() { ColName = _lang["card_col"], Width = 100 },
                            new ColumnHeaderTemplate() { ColName = _lang["bill_col"], Width = 100 },
                            new ColumnHeaderTemplate() { ColName = _lang["offset_col"], Width = 100 },
                            new ColumnHeaderTemplate() { ColName = _lang["adjustment_money_col"], Width = 100 },
                            new ColumnHeaderTemplate() { ColName = _lang["deposit_hand_1_col"], Width = 100 },
                            new ColumnHeaderTemplate() { ColName = _lang["deposit_hand_and_others_2_col"], Width = 100 },
                            new ColumnHeaderTemplate() { ColName = _lang["total_deposit_col"], Width = 100 },
                            new ColumnHeaderTemplate() { ColName = _lang["transfer_bank_col"], Width = 100 },
                            new ColumnHeaderTemplate() { ColName = _lang["deposit_type_col"], Width = 100 },
                            new ColumnHeaderTemplate() { ColName = _lang["card_approval_number_col"], Width = 100 },
                            new ColumnHeaderTemplate() { ColName = _lang["card_slip_number_col"], Width = 100 },
                            new ColumnHeaderTemplate() { ColName = _lang["bill_no_col"], Width = 100 }
                        }
                    }
                }
            };
            Body = new BodyTemplate()
            {
                Rows = new List<RowBodyTemplate>()
                {
                    new RowBodyTemplate()
                    {
                        Columns = new List<ColumnBodyTemplate>()
                        {
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentFormGridItem.No), AlignCol = AlignColEnum.Center},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentFormGridItem.NyuSihHakoYmd), CustomTextFormatDelegate = KoboGridHelper.FormatYYYYMMDDDelegate},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentFormGridItem.NyuSihEigNm)},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentFormGridItem.GridCash), CustomTextFormatDelegate = KoboGridHelper.AddCommasForNumber, AlignCol = AlignColEnum.Right},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentFormGridItem.GridTransfer), CustomTextFormatDelegate = KoboGridHelper.AddCommasForNumber, AlignCol = AlignColEnum.Right},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentFormGridItem.FurKesG), CustomTextFormatDelegate = KoboGridHelper.AddCommasForNumber, AlignCol = AlignColEnum.Right},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentFormGridItem.KyoKesG), CustomTextFormatDelegate = KoboGridHelper.AddCommasForNumber, AlignCol = AlignColEnum.Right},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentFormGridItem.GridCard), CustomTextFormatDelegate = KoboGridHelper.AddCommasForNumber, AlignCol = AlignColEnum.Right},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentFormGridItem.GridCommercialPaper), CustomTextFormatDelegate = KoboGridHelper.AddCommasForNumber, AlignCol = AlignColEnum.Right},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentFormGridItem.GridOffset), CustomTextFormatDelegate = KoboGridHelper.AddCommasForNumber, AlignCol = AlignColEnum.Right},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentFormGridItem.GridAdjustment), CustomTextFormatDelegate = KoboGridHelper.AddCommasForNumber, AlignCol = AlignColEnum.Right},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentFormGridItem.GridOther1), CustomTextFormatDelegate = KoboGridHelper.AddCommasForNumber, AlignCol = AlignColEnum.Right},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentFormGridItem.GridOther2), CustomTextFormatDelegate = KoboGridHelper.AddCommasForNumber, AlignCol = AlignColEnum.Right},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentFormGridItem.GridTotalDeposit), CustomTextFormatDelegate = KoboGridHelper.AddCommasForNumber, AlignCol = AlignColEnum.Right},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentFormGridItem.GridBank)},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentFormGridItem.YokinSyuNm)},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentFormGridItem.NSCardSyo)},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentFormGridItem.NSCardDen)},
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentFormGridItem.NSTegataNo)}
                        }
                    }
                }
            };
        }

        private void InitSummaryTable()
        {
            SummaryTableHeader = new HeaderTemplate()
            {
                Rows = new List<RowHeaderTemplate>()
                {
                    new RowHeaderTemplate()
                    {
                        Columns = new List<ColumnHeaderTemplate>()
                        {
                            new ColumnHeaderTemplate(){ ColName = string.Empty, Width = 130 },
                            new ColumnHeaderTemplate(){ ColName = _lang["cash_col"], Width = 130 },
                            new ColumnHeaderTemplate(){ ColName = _lang["transfer_col"], Width = 130 },
                            new ColumnHeaderTemplate(){ ColName = _lang["transfer_fee_col"], Width = 130 },
                            new ColumnHeaderTemplate(){ ColName = _lang["sponsorship_fund_col"], Width = 130 },
                            new ColumnHeaderTemplate(){ ColName = _lang["card_col"], Width = 130 },
                            new ColumnHeaderTemplate(){ ColName = _lang["bill_col"], Width = 130 },
                            new ColumnHeaderTemplate(){ ColName = _lang["offset_col"], Width = 130 },
                            new ColumnHeaderTemplate(){ ColName = _lang["adjustment_money_col"], Width = 130 },
                            new ColumnHeaderTemplate(){ ColName = _lang["deposit_hand_1_col"], Width = 130 },
                            new ColumnHeaderTemplate(){ ColName = _lang["deposit_hand_and_others_2_col"], Width = 130 },
                            new ColumnHeaderTemplate(){ ColName = _lang["total_deposit_col"], Width = 135 }
                        }
                    }
                }
            };
            SummaryTableBody = new BodyTemplate()
            {
                Rows = new List<RowBodyTemplate>()
                {
                    new RowBodyTemplate()
                    {
                        Columns = new List<ColumnBodyTemplate>()
                        {
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentFormSummaryItem.TotalType), AlignCol = AlignColEnum.Center },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentFormSummaryItem.TotalCash), AlignCol = AlignColEnum.Right },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentFormSummaryItem.TotalTransfer), AlignCol = AlignColEnum.Right },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentFormSummaryItem.TotalTransferFee), AlignCol = AlignColEnum.Right },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentFormSummaryItem.TotalTransferSupport), AlignCol = AlignColEnum.Right },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentFormSummaryItem.TotalCard), AlignCol = AlignColEnum.Right },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentFormSummaryItem.TotalCommercialPaper), AlignCol = AlignColEnum.Right },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentFormSummaryItem.TotalOffset), AlignCol = AlignColEnum.Right },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentFormSummaryItem.TotalAdjustment), AlignCol = AlignColEnum.Right },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentFormSummaryItem.TotalOther1), AlignCol = AlignColEnum.Right },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentFormSummaryItem.TotalOther2), AlignCol = AlignColEnum.Right },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentFormSummaryItem.TotalTotalDeposit), AlignCol = AlignColEnum.Right }
                        }
                    }
                }
            };
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

                        if (propertyName == nameof(CouponPaymentPopupFormModel.DepositAmount))
                            tempDepositAmount = Model.DepositAmount;
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

        protected void PageChanged(int pageNum)
        {
            try
            {
                UpdateDisplayDataItems();
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

        private bool IsFormValid()
        {
            isDepositAmountEmpty = Model.DepositAmount == 0;

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
            return !isDepositAmountEmpty && !isCardSyoEmpty && !isCardDenEmpty && !isTegataNoEmpty;
        }

        protected async Task SaveForm()
        {
            try
            {
                isHaitaValid = true;
                await _loading.ShowAsync();
                
                if (IsFormValid())
                {
                    var commonLastUpdYmdTime = await _couponService.GetCommonLastUpdatedYmdTime(Model.SelectedItem, _currentUser.TenantID, source.Token);

                    isHaitaValid = _couponService.CompareLatestUpdYmdTime(Model.SelectedItem.SeiFutSyu, _lastUpdYmdTime, commonLastUpdYmdTime);

                    if (formType == FormType.Edit)
                    {
                        var specLastUpdYmdTime = await _couponService.GetSpecLastUpdatedYmdTime(Model.SelectedItem, Model.NyuSihTblSeq, Model.NyuSihRen, _currentUser.TenantID, source.Token);
                        var slu = _lastUpdYmdTimeList.FirstOrDefault(e => e.NyuSihTblSeq == Model.NyuSihTblSeq && e.NyuSihRen == Model.NyuSihRen);
                       
                        isHaitaValid = isHaitaValid &&
                                        slu.NyShmiUpdYmd == specLastUpdYmdTime.NyShmiUpdYmd && slu.NyShmiUpdTime == specLastUpdYmdTime.NyShmiUpdTime &&
                                        slu.NyuSihUpdYmd == specLastUpdYmdTime.NyuSihUpdYmd && slu.NyuSihUpdTime == specLastUpdYmdTime.NyuSihUpdTime;
                    }

                    if (!isHaitaValid)
                    {
                        StateHasChanged();
                        return;
                    }

                    await _couponService.SaveCouponPayment(Model, formType == FormType.Edit, Model.SelectedItem, _currentUser.SyainCdSeq, _currentUser.TenantID, source.Token);
                    await GetSelectedItem();
                    await LoadGridData(Model.SelectedItem);
                    InitDefaultFormModel();
                    formType = FormType.View;
                }

                isSelectable = true;
                tempDepositAmount = 0;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
                await CloseDialog.InvokeAsync(true);
            }
            finally
            {
                await _loading.HideAsync();
            }
        }

        protected async Task Create()
        {
            try
            {
                formType = FormType.Create;
                isSelectable = false;
                Model = InitFormModel();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected void Cancel()
        {
            try
            {
                formType = FormType.View;
                isSelectable = true;
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        public void Dispose()
        {
            source.Cancel();
        }
    }
}
