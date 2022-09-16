using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Pages.Components;
using SharedLibraries.UI.Models;
using HassyaAllrightCloud.Commons;
using System.Globalization;
using HassyaAllrightCloud.Pages.Components.CommonComponents;

namespace HassyaAllrightCloud.Pages
{
    public class CouponPaymentBase : ComponentBase, IDisposable
    {
        [Inject] protected IStringLocalizer<CouponPayment> _lang { get; set; }
        [Inject] protected ILoadingService _loadingService { get; set; }
        [Inject] private ICouponPaymentService _couponService { get; set; }
        [Inject] private ICustomerListService _customerService { get; set; }
        [Inject] IJSRuntime _jSRuntime { get; set; }
        [Inject] protected IErrorHandlerService errorModalService { get; set; }
        [Inject] protected IFilterCondition _filterService { get; set; }
        [Inject] protected IRevenueSummaryService revenueSummaryService { get; set; }
        [Parameter] public string UkeCdParam { get; set; }
        protected List<CouponPaymentGridItem> DataItems { get; set; } = new List<CouponPaymentGridItem>();
        protected HeaderTemplate Header { get; set; }
        protected BodyTemplate Body { get; set; }
        protected EditContext editFormContext { get; set; }
        protected int gridSizeClass { get; set; } = (int)ViewMode.Medium;
        protected CouponPaymentFormModel Model { get; set; }
        protected List<EigyoListItem> DepositOffices { get; set; } = new List<EigyoListItem>();
        protected List<LoadCustomerList> BillAddresses { get; set; } = new List<LoadCustomerList>();
        protected IEnumerable<CodeKbDataItem> CodeKbDataItems { get; set; } = new List<CodeKbDataItem>();
        protected List<BillAddressItem> BillAddressItems { get; set; } = new List<BillAddressItem>();
        protected GeneralInfo TotalSelectedRow { get; set; } = new GeneralInfo();
        protected List<DepositOutputClassification> DepositOutputClassifications { get; set; } = new List<DepositOutputClassification>();
        protected List<CouponPaymentGridItem> CheckedItems { get; set; } = new List<CouponPaymentGridItem>();
        protected CouponPaymentSummary Summary { get; set; } = new CouponPaymentSummary();
        protected ShowCheckboxArgs<CouponPaymentGridItem> ShowCheckboxOptions { get; set; } = new ShowCheckboxArgs<CouponPaymentGridItem>()
        {
            RowIdentifier = (checkedItem, item) => checkedItem.UkeNo == item.UkeNo && checkedItem.NyuSihCouRen == item.NyuSihCouRen,
            Disable = (item) => item.NyuKinKbn == 2
        };
        private CancellationTokenSource source { get; set; }
        protected bool dataNotFound { get; set; }
        protected int totalRows { get; set; }
        protected bool showPopup { get; set; }
        protected bool showMultiPopup { get; set; }
        protected bool? isTotalError { get; set; }
        protected Pagination pagination { get; set; }
        protected Dictionary<string, string> LangDic = new Dictionary<string, string>();
        private readonly char _spliter = ',';
        private bool isFromFirstLoaded = false;
        private bool isToFirstLoaded = false;
        protected DefaultCustomerData defaultFrom = new DefaultCustomerData();
        protected DefaultCustomerData defaultTo = new DefaultCustomerData();
        protected bool isCustomerLoaded = false;
        protected CustomerComponent customerFrom;
        protected CustomerComponent customerTo;
        protected ReservationClassComponent reservationRef;
        private bool isFirstLoaded;
        protected List<CouponPaymentGridItem> SelectedItems = new List<CouponPaymentGridItem>();
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                await _jSRuntime.InvokeVoidAsync("inputNumber", ".number", true);
                await base.OnAfterRenderAsync(firstRender);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var dataLang = _lang.GetAllStrings();
                LangDic = dataLang.ToDictionary(l => l.Name, l => l.Value);
                source = new CancellationTokenSource();
                Model = new CouponPaymentFormModel()
                {
                    PageSize = Common.DefaultPageSize,
                    TenantCdSeq = new ClaimModel().TenantID,
                    BillTypes = new List<CodeKbDataItem>()
                };
                editFormContext = new EditContext(Model);

                await InitControls();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected void RowClick(RowClickEventArgs<CouponPaymentGridItem> args)
        {
            try
            {
                SelectedItems.Clear();
                if(args.IsSelected)
                    SelectedItems.Add(args.SelectedItem);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        private async Task InitControls()
        {
            Header = InitHeader();
            Body = InitBody();
            DepositOffices = await _couponService.GetDepositOffice(new ClaimModel().TenantID, source.Token);
            Model.DepositOffice = DepositOffices.FirstOrDefault();
            BillAddresses = await _customerService.Get(new ClaimModel().TenantID);
            DepositOutputClassifications = GetOutputClassification();
            CodeKbDataItems = await _couponService.GetCodeKb(new ClaimModel().TenantID);
            // NTLanAnh add 2020/12/10 change 2021/04/09
            if (UkeCdParam != null)
            {
                Model.DepositOffice = null;
                ObjectPram temp = EncryptHelper.DecryptFromUrl<ObjectPram>(UkeCdParam);
                Model.UkeNo = temp.key;
            }
            else
            {
                Model = await BuildSearchModel(Model);
            }
            isCustomerLoaded = true;
            //
        }
        protected async Task OnChangeItemPerPage(byte size)
        {
            try
            {
                Model.PageSize = size;
                Model.PageNum = pagination.currentPage = 0;
                await LoadData();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }
        private List<DepositOutputClassification> GetOutputClassification()
        {
            return new List<DepositOutputClassification>()
            {
                new DepositOutputClassification()
                {
                    DisplayName = _lang["receivable_only"],
                    Value = DepositOutputClassificationEnum.ReceivableOnly
                },
                new DepositOutputClassification()
                {
                    DisplayName = _lang["deposited_item"],
                    Value = DepositOutputClassificationEnum.Deposited
                }
            };
        }

        public static Func<object, string> CustomRowCss = (item) =>
        {
            var model = item as CouponPaymentGridItem;
            var cssClass = "";
            switch (model.NyuKinKbn)
            {
                case 2:
                    cssClass = "nyukinkbn-2";
                    break;
                case 3:
                    cssClass = "nyukinkbn-3";
                    break;
                case 4:
                    cssClass = "nyukinkbn-4";
                    break;
                default:
                    cssClass = "nyukinkbn-1";
                    break;
            }
            return cssClass;
        };
        private BodyTemplate InitBody()
        {
            return new BodyTemplate()
            {
                CustomCssDelegate = CustomRowCss,
                Rows = new List<RowBodyTemplate>()
                {
                    new RowBodyTemplate()
                    {
                        Columns = new List<ColumnBodyTemplate>()
                        {
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentGridItem.No), RowSpan = 2 },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentGridItem.HakoYmd), CustomTextFormatDelegate = KoboGridHelper.FormatYYYYMMDDDelegate, RowSpan = 2 },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentGridItem.CouNo), RowSpan = 2 },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentGridItem.SeiTaiYmd), CustomTextFormatDelegate = KoboGridHelper.FormatYYYYMMDDDelegate, RowSpan = 2 },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentGridItem.UkeRyakuNm), RowSpan = 2 },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentGridItem.TokRyakuNm) },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentGridItem.UkeNo), CustomTextFormatDelegate = KoboGridHelper.RemoveTenantCdFromUkeNo },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentGridItem.DanTaNm) },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentGridItem.HaiSYmd), CustomTextFormatDelegate = KoboGridHelper.FormatYYYYMMDDDelegate, },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentGridItem.SeiFutSyuNm) },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentGridItem.CouKesG), CustomTextFormatDelegate = KoboGridHelper.AddCommasForNumber, RowSpan = 2, AlignCol = AlignColEnum.Right },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentGridItem.CouGkin), RowSpan = 2 },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentGridItem.LastNyuYmd), CustomTextFormatDelegate = KoboGridHelper.FormatYYYYMMDDDelegate, RowSpan = 2 },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentGridItem.NyuKinRui), CustomTextFormatDelegate = KoboGridHelper.AddCommasForNumber, RowSpan = 2, AlignCol = AlignColEnum.Right },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentGridItem.GridUnpaidAmount), CustomTextFormatDelegate = KoboGridHelper.AddCommasForNumber, RowSpan = 2, AlignCol = AlignColEnum.Right }
                        }
                    },
                     new RowBodyTemplate()
                    {
                        Columns = new List<ColumnBodyTemplate>()
                        {
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentGridItem.SitRyakuNm) },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentGridItem.YoyaKbnNm) },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentGridItem.IkNm) },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentGridItem.TouYmd), CustomTextFormatDelegate = KoboGridHelper.FormatYYYYMMDDDelegate },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(CouponPaymentGridItem.FutTumNm) }
                        }
                    }
                }
            };
        }
        private HeaderTemplate InitHeader()
        {
            return new HeaderTemplate()
            {
                StickyCount = 3,
                Rows = new List<RowHeaderTemplate>()
                {
                    new RowHeaderTemplate()
                    {
                        Columns = new List<ColumnHeaderTemplate>()
                        {
                            new ColumnHeaderTemplate() { ColName = _lang["no_col"], RowSpan = 2,Width = 50 },
                            new ColumnHeaderTemplate() { ColName = _lang["date_of_issue_col"], RowSpan = 2,Width = 150 },
                            new ColumnHeaderTemplate() { ColName = _lang["coupon_no_col"], RowSpan = 2,Width = 150 },
                            new ColumnHeaderTemplate() { ColName = _lang["billing_date_col"], RowSpan = 2,Width = 150 },
                            new ColumnHeaderTemplate() { ColName = _lang["reception_office_col"], RowSpan = 2,Width = 150 },
                            new ColumnHeaderTemplate() { ColName = _lang["customer_col"],Width = 150 },
                            new ColumnHeaderTemplate() { ColName = _lang["receipt_number_col"],Width = 150 },
                            new ColumnHeaderTemplate() { ColName = _lang["group_name_col"],Width = 150 },
                            new ColumnHeaderTemplate() { ColName = _lang["date_of_dispatch_col"],Width = 150 },
                            new ColumnHeaderTemplate() { ColName = _lang["billing_incidental_type_col"],Width = 150 },
                            new ColumnHeaderTemplate() { ColName = _lang["coupon_application_amount_col"],RowSpan = 2,    Width = 150 },
                            new ColumnHeaderTemplate() { ColName = _lang["coupon_face_value_col"],RowSpan = 2,Width = 150 },
                            new ColumnHeaderTemplate() { ColName = _lang["last_deposit_date_col"],RowSpan = 2,Width = 150 },
                            new ColumnHeaderTemplate() { ColName = _lang["cumulative_deposit_col"],RowSpan = 2,Width = 150 },
                            new ColumnHeaderTemplate() { ColName = _lang["receivable_amount_col"],RowSpan = 2,Width = 150 }
                        }
                    },
                    new RowHeaderTemplate()
                    {
                        Columns = new List<ColumnHeaderTemplate>()
                        {
                            new ColumnHeaderTemplate() { ColName = _lang["branch_name_col"], Width = 150 },
                            new ColumnHeaderTemplate() { ColName = _lang["reservation_classification_col"], Width = 150 },
                            new ColumnHeaderTemplate() { ColName = _lang["destination_name_col"], Width = 150 },
                            new ColumnHeaderTemplate() { ColName = _lang["date_of_arrival_col"], Width = 150 },
                            new ColumnHeaderTemplate() { ColName = _lang["incidental_loading_product_name_col"], Width = 150 }
                        }
                    }
                }
            };
        }
        protected void TriggerTabChange()
        {
            StateHasChanged();
        }
        private async Task LoadData()
        {
            try
            {
                if (editFormContext.Validate())
                {
                    await _loadingService.ShowAsync();
                    SelectedItems.Clear();
                    (DataItems, Summary, totalRows) = await _couponService.GetCouponPayment(Model, false, null, source.Token);
                    dataNotFound = DataItems == null || DataItems.Count == 0;
                    if (DataItems != null && DataItems.Any())
                    {
                        SelectedItems.Add(DataItems.FirstOrDefault());
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                await _loadingService.HideAsync();
            }
        }
        private async Task SaveSearchModel(CouponPaymentFormModel model)
        {
            var filers = GetSearchFormModel(model);
            await _filterService.SaveFilterCondtion(filers, FormFilterName.CouponPayment, 0, new ClaimModel().SyainCdSeq);
        }
        private Dictionary<string, string> GetSearchFormModel(CouponPaymentFormModel model)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            if (model == null) return result;

            result.Add(nameof(CouponPaymentFormModel.StartIssuePeriod), model.StartIssuePeriod?.ToString(DateTimeFormat.yyyyMMdd) ?? string.Empty);
            result.Add(nameof(CouponPaymentFormModel.EndIssuePeriod), model.EndIssuePeriod?.ToString(DateTimeFormat.yyyyMMdd) ?? string.Empty);
            result.Add(nameof(CouponPaymentFormModel.DepositOffice), model.DepositOffice?.EigyoCdSeq.ToString() ?? 0.ToString());

            result.Add(nameof(CouponPaymentFormModel.SelectedGyosyaFrom), model.SelectedGyosyaFrom?.GyosyaCdSeq.ToString() ?? string.Empty);
            result.Add(nameof(CouponPaymentFormModel.SelectedTokiskFrom), model.SelectedTokiskFrom?.TokuiSeq.ToString() ?? string.Empty);
            result.Add(nameof(CouponPaymentFormModel.SelectedTokiStFrom), model.SelectedTokiStFrom?.SitenCdSeq.ToString() ?? string.Empty);
            result.Add(nameof(CouponPaymentFormModel.SelectedGyosyaTo), model.SelectedGyosyaTo?.GyosyaCdSeq.ToString() ?? string.Empty);
            result.Add(nameof(CouponPaymentFormModel.SelectedTokiskTo), model.SelectedTokiskTo?.TokuiSeq.ToString() ?? string.Empty);
            result.Add(nameof(CouponPaymentFormModel.SelectedTokiStTo), model.SelectedTokiStTo?.SitenCdSeq.ToString() ?? string.Empty);

            result.Add(nameof(CouponPaymentFormModel.StartReservationClassificationSort), model.StartReservationClassificationSort?.YoyaKbnSeq.ToString() ?? string.Empty);
            result.Add(nameof(CouponPaymentFormModel.EndReservationClassificationSort), model.EndReservationClassificationSort?.YoyaKbnSeq.ToString() ?? string.Empty);
            result.Add(nameof(CouponPaymentFormModel.DepositOutputClassification), model.DepositOutputClassification?.Value == null ? string.Empty : ((int)model.DepositOutputClassification.Value).ToString());
            result.Add(nameof(CouponPaymentFormModel.BillTypes), model.BillTypes == null ? string.Empty : string.Join(_spliter, model.BillTypes.Select(e => e.CodeKbn)));
            result.Add(nameof(gridSizeClass), gridSizeClass.ToString());

            return result;
        }
        protected async Task ChangeGridSize(ViewMode mode)
        {
            try
            {
                gridSizeClass = (int)mode;
                if (editFormContext.Validate())
                {
                    await SaveSearchModel(Model);
                }
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        private async Task<CouponPaymentFormModel> BuildSearchModel(CouponPaymentFormModel model)
        {
            var filters = await _filterService.GetFilterCondition(FormFilterName.CouponPayment, 0, new ClaimModel().SyainCdSeq);

            foreach (var item in filters)
            {
                if (item.ItemNm == nameof(gridSizeClass))
                {
                    gridSizeClass = int.Parse(item.JoInput);
                    continue;
                }
                var propertyInfo = model.GetType().GetProperty(item.ItemNm);

                if (propertyInfo != null && !string.IsNullOrEmpty(item.JoInput))
                {
                    switch (item.ItemNm)
                    {
                        case nameof(CouponPaymentFormModel.StartIssuePeriod):
                        case nameof(CouponPaymentFormModel.EndIssuePeriod):
                            propertyInfo.SetValue(model, DateTime.ParseExact(item.JoInput, DateTimeFormat.yyyyMMdd, CultureInfo.InvariantCulture));
                            break;
                        case nameof(CouponPaymentFormModel.DepositOffice):
                            var depoSeq = int.Parse(item.JoInput);
                            if (depoSeq != 0)
                            {
                                var selectedDepo = DepositOffices.FirstOrDefault(e => e.EigyoCdSeq == depoSeq);
                                if (selectedDepo != null)
                                    propertyInfo.SetValue(model, selectedDepo);
                            }
                            break;
                        case nameof(CouponPaymentFormModel.StartReservationClassificationSort):
                        case nameof(CouponPaymentFormModel.EndReservationClassificationSort):
                            if (int.TryParse(item.JoInput, out int result))
                            {
                                var selectedRe = reservationRef.ListReservationClass.FirstOrDefault(e => e.YoyaKbnSeq == result);
                                if (selectedRe != null)
                                    propertyInfo.SetValue(model, selectedRe);
                            }
                            break;
                        case nameof(CouponPaymentFormModel.DepositOutputClassification):
                            var classVal = (DepositOutputClassificationEnum)(int.Parse(item.JoInput));
                            var selectedClass = DepositOutputClassifications.FirstOrDefault(e => e.Value == classVal);
                            if (selectedClass != null)
                                propertyInfo.SetValue(model, selectedClass);
                            break;
                        case nameof(CouponPaymentFormModel.BillTypes):
                            var codes = item.JoInput.Split(_spliter);
                            var selectedTypes = CodeKbDataItems.Where(e => codes.Contains(e.CodeKbn)).ToList();
                            propertyInfo.SetValue(model, selectedTypes);
                            break;

                        case nameof(CouponPaymentFormModel.SelectedGyosyaFrom):
                            if (int.TryParse(item.JoInput, out var fgSeq))
                                defaultFrom.GyosyaCdSeq = fgSeq;
                            break;
                        case nameof(CouponPaymentFormModel.SelectedTokiskFrom):
                            if (int.TryParse(item.JoInput, out var ftSeq))
                                defaultFrom.TokiskCdSeq = ftSeq;
                            break;
                        case nameof(CouponPaymentFormModel.SelectedTokiStFrom):
                            if (int.TryParse(item.JoInput, out var fsSeq))
                                defaultFrom.TokiStCdSeq = fsSeq;
                            break;

                        case nameof(CouponPaymentFormModel.SelectedGyosyaTo):
                            if (int.TryParse(item.JoInput, out var tgSeq))
                                defaultTo.GyosyaCdSeq = tgSeq;
                            break;
                        case nameof(CouponPaymentFormModel.SelectedTokiskTo):
                            if (int.TryParse(item.JoInput, out var ttSeq))
                                defaultTo.TokiskCdSeq = ttSeq;
                            break;
                        case nameof(CouponPaymentFormModel.SelectedTokiStTo):
                            if (int.TryParse(item.JoInput, out var tsSeq))
                                defaultTo.TokiStCdSeq = tsSeq;
                            break;
                    }
                }
            }

            return model;
        }

        protected async Task CloseDialog()
        {
            try
            {
                showPopup = false;
                showMultiPopup = false;
                await LoadData();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        private async Task LoadBillAddress()
        {
            BillAddressItems = await _couponService.GetBillAddress(Model, source.Token);
            Model.BillAddress = BillAddressItems.FirstOrDefault();
        }

        protected void RowDbClick(CouponPaymentGridItem item)
        {
            showPopup = true;
            SelectedItems.Clear();
            SelectedItems.Add(item);
            StateHasChanged();
        }
        protected void UpdateData()
        {
            if (SelectedItems.Any())
            {
                showPopup = true;
                StateHasChanged();
            }
        }

        protected void UpdateMultiple()
        {
            try
            {
                isTotalError = TotalSelectedRow.Total != Model.Total;
                if (CheckedItems != null && CheckedItems.Any() && TotalSelectedRow.Total != 0 && isTotalError != null && !isTotalError.Value)
                    showMultiPopup = true;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected async Task UpdateFormModel(string propertyName, dynamic value)
        {
            try
            {
                switch (propertyName)
                {
                    case nameof(CouponPaymentFormModel.BillTypes):
                        if (Model.BillTypes == null)
                            Model.BillTypes = new List<CodeKbDataItem>();
                        if (Model.BillTypes.Contains(value))
                            Model.BillTypes.Remove(value);
                        else
                            Model.BillTypes.Add(value);
                        break;
                    case nameof(CouponPaymentFormModel.BillAddress):
                        Model.BillAddress = value;
                        break;
                    case nameof(CouponPaymentFormModel.Total):
                        var removeCommas = (value as string).Replace(",", "");
                        var pi = Model.GetType().GetProperty(propertyName);
                        if (int.TryParse(removeCommas, out int val))
                            pi.SetValue(Model, val);
                        else
                            pi.SetValue(Model, 0);
                        isTotalError = TotalSelectedRow.Total != val;
                        break;

                    default:
                        var propertyInfo = Model.GetType().GetProperty(propertyName);
                        propertyInfo.SetValue(Model, value);
                        break;
                }
                if (editFormContext.Validate() && isFromFirstLoaded && isToFirstLoaded &&
                    propertyName != nameof(CouponPaymentFormModel.SelectedGyosyaFrom) &&
                    propertyName != nameof(CouponPaymentFormModel.SelectedTokiskFrom) &&
                    propertyName != nameof(CouponPaymentFormModel.SelectedGyosyaTo) &&
                    propertyName != nameof(CouponPaymentFormModel.SelectedTokiskTo) &&
                    propertyName != nameof(CouponPaymentFormModel.Total))
                {
                    if (propertyName != nameof(CouponPaymentFormModel.BillAddress))
                    {
                        await LoadBillAddress();
                        CheckedItems = new List<CouponPaymentGridItem>();
                    }
                    await LoadData();
                    await SaveSearchModel(Model);
                }
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected async Task PageChanged(int pageNum)
        {
            try
            {
                Model.PageNum = pageNum;
                await LoadData();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected void CheckedChange(CheckedChangeEventArgs<CouponPaymentGridItem> args)
        {
            try
            {
                TotalSelectedRow = new GeneralInfo();
                if (args.CheckedItems != null && args.CheckedItems.Any())
                {
                    foreach (var item in args.CheckedItems)
                    {
                        switch (item.SeiFutSyuCd)
                        {
                            case "01":
                                TotalSelectedRow.TotalFare += item.GridUnpaidAmount;
                                break;
                            case "02":
                                TotalSelectedRow.TotalIncidental += item.GridUnpaidAmount;
                                break;
                            case "03":
                                TotalSelectedRow.TotalTollFee += item.GridUnpaidAmount;
                                break;
                            case "04":
                                TotalSelectedRow.TotalArrangementFee += item.GridUnpaidAmount;
                                break;
                            case "05":
                                TotalSelectedRow.TotalGuideFee += item.GridUnpaidAmount;
                                break;
                            case "06":
                                TotalSelectedRow.LoadedGoods += item.GridUnpaidAmount;
                                break;
                            case "07":
                                TotalSelectedRow.TotalCancelFee += item.GridUnpaidAmount;
                                break;
                        }
                    }
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected async Task ResetBtnOnClick()
        {
            try
            {
                Model.StartIssuePeriod = null;
                Model.EndIssuePeriod = null;
                Model.DepositOffice = DepositOffices.FirstOrDefault();
                Model.BillTypes = new List<CodeKbDataItem>();
                Model.StartReservationClassificationSort = null;
                Model.EndReservationClassificationSort = null;
                Model.DepositOutputClassification = null;
                Model.BillAddress = null;
                BillAddressItems = new List<BillAddressItem>();
                TotalSelectedRow = new GeneralInfo();
                DataItems = new List<CouponPaymentGridItem>();
                Summary = new CouponPaymentSummary();
                CheckedItems.Clear();
                gridSizeClass = (int)ViewMode.Medium;
                await _filterService.DeleteCustomFilerCondition(new ClaimModel().SyainCdSeq, 0, FormFilterName.CouponPayment);

                ResetCustomer();

                await LoadBillAddress();
                await LoadData();

                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }

        }

        private void ResetCustomer()
        {
            defaultFrom.GyosyaCdSeq = null;
            defaultTo.GyosyaCdSeq = null;
            Model.SelectedGyosyaFrom = Model.SelectedGyosyaTo = null;
            Model.SelectedTokiskFrom = Model.SelectedTokiskTo = null;
            Model.SelectedTokiStFrom = Model.SelectedTokiStTo = null;
            customerFrom.UpdateSelectedItems();
            customerTo.UpdateSelectedItems();
        }

        protected async Task BillAddressChanged(bool isPre)
        {
            try
            {
                if (isPre == true)
                {
                    var preIndex = _couponService.GetPreNextIndex(true, BillAddressItems, Model.BillAddress);
                    if (preIndex != -1)
                    {
                        Model.BillAddress = BillAddressItems[preIndex];
                        CheckedItems.Clear();
                        TotalSelectedRow = new GeneralInfo();
                        Model.PageNum = pagination.currentPage = 0;
                        await LoadData();
                    }
                }
                else
                {
                    var nextIndex = _couponService.GetPreNextIndex(false, BillAddressItems, Model.BillAddress);
                    if (nextIndex != -1)
                    {
                        Model.BillAddress = BillAddressItems[nextIndex];
                        CheckedItems.Clear();
                        TotalSelectedRow = new GeneralInfo();
                        Model.PageNum = pagination.currentPage = 0;
                        await LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected async Task FirstLoad(bool isFrom)
        {
            if (isFrom)
                isFromFirstLoaded = true;
            else
                isToFirstLoaded = true;
            if (isFromFirstLoaded && isToFirstLoaded && !isFirstLoaded)
            {
                isFirstLoaded = true;
                await LoadBillAddress();
                await LoadData();
                StateHasChanged();
            }
        }

        public void Dispose()
        {
            source.Cancel();
        }
    }
}
