using BlazorContextMenu;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.BillPrint;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using HassyaAllrightCloud.Domain.Dto.DepositCoupon;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.IService;
using HassyaAllrightCloud.IService.CommonComponents;
using HassyaAllrightCloud.Pages.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static HassyaAllrightCloud.Commons.Constants.Constants;

namespace HassyaAllrightCloud.Pages
{
    public class DepositCouponBase : ComponentBase
    {
        [CascadingParameter(Name = "ClaimModel")]
        protected ClaimModel ClaimModel { get; set; }
        #region Inject
        [Inject]
        protected IStringLocalizer<DepositCoupon> Lang { get; set; }
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }
        [Inject]
        protected IDepositCouponService depositCouponService { get; set; }
        [Inject]
        protected ITPM_YoyKbnDataListService YoyKbnDataListService { get; set; }
        [Inject]
        protected IBillCheckListService BillCheckListService { get; set; }
        [Inject]
        protected ICustomerListService CustomerService { get; set; }
        [Inject]
        protected NavigationManager navigationManager { get; set; }
        [Inject]
        protected IBlazorContextMenuService blazorContextMenuService { get; set; }
        [Inject]
        protected IErrorHandlerService errorModalService { get; set; }
        [Inject]
        protected IFilterCondition FilterConditionService { get; set; }
        [Inject]
        IGenerateFilterValueDictionary GenerateFilterValueDictionaryService { get; set; }
        [Inject] protected IReservationClassComponentService ReservationClassComponentService { get; set; }
        [Inject] protected ICustomerComponentService CustomerComponentService { get; set; }

        #endregion

        #region Parameter
        [Parameter]
        public string UkeCdParam { get; set; }
        #endregion

        #region Properties And Variable          
        protected DepositCouponResult depositCouponResult { get; set; } = new DepositCouponResult();
        public List<DepositCouponGrid> GridCheckDatas { get; set; }
        protected DepositCouponGrid LastRowCheckDatas { get; set; }
        protected int ActiveTabIndex
        {
            get => activeTabIndex;
            set
            {
                activeTabIndex = value;
                AdjustHeightWhenTabChanged();
            }
        }
        protected bool isLoading { get; set; } = true;
        protected bool itemCheckAll { get; set; } = false;
        protected DepositCouponFilter depositCouponFilter { get; set; }
        protected List<BillOfficeData> billOfficeList = new List<BillOfficeData>();
        protected List<string> codeList = new List<string>();
        protected List<string> depositOutputClassifications { get; set; } = new List<string>() { "未収のみ", "入金済み", "クーポン未入力の未収のみ" };
    public Dictionary<string, string> LangDic = new Dictionary<string, string>();
        Dictionary<string, string> keyValueFilterPairs = new Dictionary<string, string>();
        protected string dateFormat = "yyyy/MM/dd";
        protected int MaxPageCount = 5;
        protected int CurrentPage = 1;
        protected Pagination paging = new Pagination();
        public OutDataTable outDataTable { get; set; }
		public byte itemPerPage { get; set; } = 25;
        public int NumberOfPage { get; set; }
        protected int activeTabIndex = 0;
        protected bool IsNoData { get; set; }
        protected bool IsValid = true;
        public string errorMessage { get; set; } = string.Empty;
        public EditContext formContext;
        public int selectedPage { get; set; } = 0;
        public bool isOpenCharterInquiryPopUp { get; set; }
        public bool isOpenLumpPopUp { get; set; }
        public bool isOpenCouponBalancePopUp { get; set; }
        public bool isOpenCouponPopUp { get; set; }
        public bool isOpenDepositPaymentPopUp { get; set; }
        public bool isUpdated { get; set; }
        public bool dbClicked { get; set; }
        public int syainCdSeq { get; set; } = new ClaimModel().SyainCdSeq;
        public DepositCouponTotal depositCouponTotal { get; set; }
        public DepositCouponItemTotal depositCouponItemTotal = new DepositCouponItemTotal() { 
            ItemArrangementFee = "0",
            StatiticsDeposit = "0",
            ItemCancellation = "0",
            ItemLoaded = "0",
            ItemGuideFee = "0",
            ItemTollFee = "0",
            ItemFare = "0",
            ItemIncidental = "0",
            Total = "0"
        };
        public Components.DepositCoupon.DepositPaymentBase depositPayment = new Components.DepositCoupon.DepositPaymentBase();
        public Components.DepositCoupon.LumpBase lumpBase = new Components.DepositCoupon.LumpBase();
        public List<ReservationClassComponentData> ListReservationClass { get; set; } = new List<ReservationClassComponentData>();
        protected List<CustomerComponentGyosyaData> ListGyosya { get; set; } = new List<CustomerComponentGyosyaData>();
        protected List<CustomerComponentTokiskData> ListTokisk { get; set; } = new List<CustomerComponentTokiskData>();
        protected List<CustomerComponentTokiStData> ListTokiSt { get; set; } = new List<CustomerComponentTokiStData>();
        public Components.CommonComponents.CustomerComponent startCustomerComponent { get; set; }
        = new Components.CommonComponents.CustomerComponent();
        public Components.CommonComponents.CustomerComponent endCustomerComponent { get; set; }
        = new Components.CommonComponents.CustomerComponent();
        #endregion

        #region Function
        protected override async Task OnInitializedAsync()
        {
            try
            {
                if (ClaimModel != null)
                {
                    syainCdSeq = ClaimModel.SyainCdSeq;
                }
                LangDic = Lang.GetAllStrings().ToDictionary(l => l.Name, l => l.Value);
                GridCheckDatas = new List<DepositCouponGrid>();
                depositCouponResult.depositCouponTotal = new DepositCouponPageTotal();
                LastRowCheckDatas = new DepositCouponGrid();
                depositCouponFilter = new DepositCouponFilter() { BillTypes = new List<int>(), ActiveV = (int)ViewMode.Medium };
                depositCouponTotal = new DepositCouponTotal();
                formContext = new EditContext(depositCouponFilter);

                // Bill office
                billOfficeList = await BillCheckListService.GetBillOffice(new ClaimModel().TenantID);
                ListReservationClass = await ReservationClassComponentService.GetListReservationClass();
                ListGyosya = await CustomerComponentService.GetListGyosya();
                ListTokisk = await CustomerComponentService.GetListTokisk();
                ListTokiSt = await CustomerComponentService.GetListTokiSt();

                if (billOfficeList.Any())
                    depositCouponFilter.BillOffice = billOfficeList.FirstOrDefault();
                if (UkeCdParam != null)
                {
                    depositCouponFilter.BillOffice = null;
                    ObjectPram temp = EncryptHelper.DecryptFromUrl<ObjectPram>(UkeCdParam);
                    depositCouponFilter.UkeCd = temp.key;
                }
                else
                {
                    List<TkdInpCon> filterValues = await FilterConditionService.GetFilterCondition(FormFilterName.DepositCoupon, 0, syainCdSeq);
                    if (filterValues.Any())
                    {
                        foreach (var item in filterValues)
                        {
                            var propertyInfo = depositCouponFilter.GetType().GetProperty(item.ItemNm);
                            switch (item.ItemNm)
                            {
                                case nameof(depositCouponFilter.ActiveV):
                                    propertyInfo.SetValue(depositCouponFilter, int.Parse(item.JoInput), null);
                                    break;
                                case nameof(depositCouponFilter.DepositOutputClassification):
                                    var depositOutputClassification = depositOutputClassifications.Where(x => x == item.JoInput).FirstOrDefault();
                                    propertyInfo.SetValue(depositCouponFilter, depositOutputClassification, null);
                                    break;
                                case nameof(depositCouponFilter.startCustomerComponentGyosyaData):
                                case nameof(depositCouponFilter.endCustomerComponentGyosyaData):
                                    if(!string.IsNullOrWhiteSpace(item.JoInput))
                                    {
                                        var gyosya = ListGyosya.Where(x => x.GyosyaCdSeq == int.Parse(item.JoInput)).FirstOrDefault();
                                        propertyInfo.SetValue(depositCouponFilter, gyosya, null);
                                        if (item.ItemNm == nameof(depositCouponFilter.startCustomerComponentGyosyaData))
                                            startCustomerComponent.OnChangeDefaultGyosya(gyosya.GyosyaCdSeq);
                                        else
                                            endCustomerComponent.OnChangeDefaultGyosya(gyosya.GyosyaCdSeq);
                                    }
                                    break;
                                case nameof(depositCouponFilter.startCustomerComponentTokiskData):
                                case nameof(depositCouponFilter.endCustomerComponentTokiskData):
                                    if (!string.IsNullOrWhiteSpace(item.JoInput))
                                    {
                                        var tokisk = ListTokisk.Where(x => x.TokuiSeq == int.Parse(item.JoInput)).FirstOrDefault();
                                        propertyInfo.SetValue(depositCouponFilter, tokisk, null);
                                        if (item.ItemNm == nameof(depositCouponFilter.startCustomerComponentGyosyaData))
                                            startCustomerComponent.OnChangeDefaultTokisk(tokisk.TokuiSeq);
                                        else
                                            endCustomerComponent.OnChangeDefaultTokisk(tokisk.TokuiSeq);
                                    }
                                    break;
                                case nameof(depositCouponFilter.startCustomerComponentTokiStData):
                                case nameof(depositCouponFilter.endCustomerComponentTokiStData):
                                    if (!string.IsNullOrWhiteSpace(item.JoInput))
                                    {
                                        var tokist = ListTokiSt.Where(x => x.TokuiSeq.ToString() + x.SitenCdSeq.ToString() == item.JoInput).FirstOrDefault();
                                        propertyInfo.SetValue(depositCouponFilter, tokist, null);
                                        if (item.ItemNm == nameof(depositCouponFilter.startCustomerComponentGyosyaData))
                                            startCustomerComponent.OnChangeDefaultTokiSt(tokist.SitenCdSeq);
                                        else
                                            endCustomerComponent.OnChangeDefaultTokiSt(tokist.SitenCdSeq);
                                    }
                                    break;
                                case nameof(depositCouponFilter.BillPeriodFrom):
                                case nameof(depositCouponFilter.BillPeriodTo):
                                    var datetime = string.IsNullOrWhiteSpace(item.JoInput) ? (DateTime?)null : DateTime.ParseExact(item.JoInput, "yyyyMMdd", CultureInfo.InvariantCulture);
                                    propertyInfo.SetValue(depositCouponFilter, datetime, null);
                                    break;
                                case nameof(depositCouponFilter.BillTypes):
                                    var billTypes = string.IsNullOrWhiteSpace(item.JoInput) ? new List<int>() : item.JoInput.Split(",").Select(int.Parse).ToList();
                                    propertyInfo.SetValue(depositCouponFilter, billTypes, null);
                                    break;
                                case nameof(depositCouponFilter.BillOffice):
                                    if (!string.IsNullOrWhiteSpace(item.JoInput))
                                    {
                                        var billOffice = billOfficeList.Where(x => x.EigyoCd == item.JoInput).FirstOrDefault();
                                        propertyInfo.SetValue(depositCouponFilter, billOffice, null);
                                    }
                                    break;
                                case nameof(depositCouponFilter.StartReservationClassification):
                                case nameof(depositCouponFilter.EndReservationClassification):
                                    if (!string.IsNullOrWhiteSpace(item.JoInput))
                                    {
                                        var reservationClassification = ListReservationClass.Where(x => x.YoyaKbnSeq == int.Parse(item.JoInput)).FirstOrDefault();
                                        propertyInfo.SetValue(depositCouponFilter, reservationClassification, null);
                                    }
                                    break;
                                case nameof(depositCouponFilter.itemFare):
                                case nameof(depositCouponFilter.itemIncidental):
                                case nameof(depositCouponFilter.itemTollFee):
                                case nameof(depositCouponFilter.itemArrangementFee):
                                case nameof(depositCouponFilter.itemGuideFee):
                                case nameof(depositCouponFilter.itemLoaded):
                                case nameof(depositCouponFilter.itemCancellationCharge):
                                    var status = item.JoInput == "0" ? false : true; ;
                                    propertyInfo.SetValue(depositCouponFilter, status, null);
                                    break;
                            }
                        }
                    }
                }
                
                depositOutputClassifications.Insert(0, null);
                await startCustomerComponent.RenderDefault();
                await endCustomerComponent.RenderDefault();
                await SelectPage(0, null);
            } catch(Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                if (isUpdated)
                {
                    isUpdated = false;
                    await SelectPage(CurrentPage, depositCouponFilter.Code);
                    GridCheckDatas = GridCheckDatas.Select(x => { x.Checked = false; return x; }).ToList();
                    depositCouponResult.depositCouponGrids = depositCouponResult.depositCouponGrids.Select(x => { x.Checked = false; return x; }).ToList();
                    depositCouponItemTotal = new DepositCouponItemTotal()
                    {
                        ItemArrangementFee = "0",
                        StatiticsDeposit = "0",
                        ItemCancellation = "0",
                        ItemLoaded = "0",
                        ItemGuideFee = "0",
                        ItemTollFee = "0",
                        ItemFare = "0",
                        ItemIncidental = "0",
                        Total = "0"
                    };
                    itemCheckAll = false;
                    await InvokeAsync(StateHasChanged);
                }

                await JSRuntime.InvokeVoidAsync("setInputFilter", ".statiticsDepositColon", true, 9);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }
        /// <summary>
        /// change value combobox
        /// </summary>
        /// <param name="ValueName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected async Task ChangeValueForm(string ValueName, dynamic value)
        {
            try
            {
                if (value is string && string.IsNullOrEmpty(value))
                    value = null;

                var propertyInfo = depositCouponFilter.GetType().GetProperty(ValueName);
                propertyInfo.SetValue(depositCouponFilter, value, null);
                StateHasChanged();
                if (!formContext.Validate())
                    return;

                GridCheckDatas = new List<DepositCouponGrid>();
                LastRowCheckDatas = new DepositCouponGrid();
                depositCouponItemTotal = new DepositCouponItemTotal()
                {
                    ItemArrangementFee = "0",
                    StatiticsDeposit = "0",
                    ItemCancellation = "0",
                    ItemLoaded = "0",
                    ItemGuideFee = "0",
                    ItemTollFee = "0",
                    ItemFare = "0",
                    ItemIncidental = "0",
                    Total = "0"
                };
                if (ValueName == nameof(depositCouponFilter.startCustomerComponentGyosyaData) 
                    || ValueName == nameof(depositCouponFilter.startCustomerComponentTokiskData)
                    || ValueName == nameof(depositCouponFilter.endCustomerComponentGyosyaData)
                    || ValueName == nameof(depositCouponFilter.endCustomerComponentTokiskData))
                    return;
                await SelectPage(0, ValueName == nameof(depositCouponFilter.Code) ? value : null);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public async Task SaveFilterCondtionAsync()
        {
            keyValueFilterPairs = GenerateFilterValueDictionaryService.GenerateForDepositCoupon(depositCouponFilter);
            await FilterConditionService.SaveFilterCondtion(keyValueFilterPairs, FormFilterName.DepositCoupon, 0, syainCdSeq);
        }
        /// set value when check box item bill type change
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        protected async Task CheckedValueChanged(string fieldName, object newValue)
        {
            try
            {
                switch (fieldName)
                {
                    case nameof(depositCouponFilter.itemFare):
                        depositCouponFilter.itemFare = Convert.ToBoolean(newValue);
                        if (depositCouponFilter.itemFare)
                        {
                            depositCouponFilter.BillTypes.Add(1);
                        }
                        else
                        {
                            depositCouponFilter.BillTypes.Remove(1);
                        }
                        break;
                    case nameof(depositCouponFilter.itemIncidental):
                        depositCouponFilter.itemIncidental = Convert.ToBoolean(newValue);
                        if (depositCouponFilter.itemIncidental)
                        {
                            depositCouponFilter.BillTypes.Add(2);
                        }
                        else
                        {
                            depositCouponFilter.BillTypes.Remove(2);
                        }
                        break;
                    case nameof(depositCouponFilter.itemTollFee):
                        depositCouponFilter.itemTollFee = Convert.ToBoolean(newValue);
                        if (depositCouponFilter.itemTollFee)
                        {
                            depositCouponFilter.BillTypes.Add(3);
                        }
                        else
                        {
                            depositCouponFilter.BillTypes.Remove(3);
                        }
                        break;
                    case nameof(depositCouponFilter.itemArrangementFee):
                        depositCouponFilter.itemArrangementFee = Convert.ToBoolean(newValue);
                        if (depositCouponFilter.itemArrangementFee)
                        {
                            depositCouponFilter.BillTypes.Add(4);
                        }
                        else
                        {
                            depositCouponFilter.BillTypes.Remove(4);
                        }
                        break;
                    case nameof(depositCouponFilter.itemGuideFee):
                        depositCouponFilter.itemGuideFee = Convert.ToBoolean(newValue);
                        if (depositCouponFilter.itemGuideFee)
                        {
                            depositCouponFilter.BillTypes.Add(5);
                        }
                        else
                        {
                            depositCouponFilter.BillTypes.Remove(5);
                        }
                        break;
                    case nameof(depositCouponFilter.itemLoaded):
                        depositCouponFilter.itemLoaded = Convert.ToBoolean(newValue);
                        if (depositCouponFilter.itemLoaded)
                        {
                            depositCouponFilter.BillTypes.Add(6);
                        }
                        else
                        {
                            depositCouponFilter.BillTypes.Remove(6);
                        }
                        break;
                    default:
                        depositCouponFilter.itemCancellationCharge = Convert.ToBoolean(newValue);
                        if (depositCouponFilter.itemCancellationCharge)
                        {
                            depositCouponFilter.BillTypes.Add(7);
                        }
                        else
                        {
                            depositCouponFilter.BillTypes.Remove(7);
                        }
                        break;
                }
                if (formContext.Validate())
                {
                    await SaveFilterCondtionAsync();           
                    GridCheckDatas = new List<DepositCouponGrid>();
                    LastRowCheckDatas = new DepositCouponGrid();
                    await SelectPage(0, null);
                }
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }
        /// <summary>
        /// event when check box in grid change
        /// </summary>
        /// <param name="i"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        protected void CheckedValueGridChanged(DepositCouponGrid data, bool newValue, bool isCheckAll)
        {
            try
            {
                var item = GridCheckDatas.Where(x => x.UkeNo == data.UkeNo && x.MisyuRen == data.MisyuRen).FirstOrDefault();
                if (item == null)
                    GridCheckDatas.Add(data);
                else
                {
                    if (item.Checked && data.Checked && isCheckAll && newValue)
                    {
                        item.Checked = (item != null) && newValue;
                        return;
                    }
                    item.Checked = (item != null) && newValue;
                    GridCheckDatas.Remove(data);
                }
                data.Checked = newValue;
                itemCheckAll = depositCouponResult.depositCouponGrids.Where(x => x.NyuKinKbn != 2).All(x => GridCheckDatas.Any(y => x.UkeNo == x.UkeNo && y.MisyuRen == x.MisyuRen && x.Checked));
                switch (data.SeiFutSyuNm)
                {
                    case "運賃":
                        if (newValue)
                            depositCouponItemTotal.ItemFareNum += (long)(data.SeiKin - data.NyuKinRui);
                        else
                            depositCouponItemTotal.ItemFareNum -= (long)(data.SeiKin - data.NyuKinRui);
                        depositCouponItemTotal.ItemFare = string.Format("{0:#,0}", depositCouponItemTotal.ItemFareNum);
                        break;
                    case "付帯":
                        if (newValue)
                            depositCouponItemTotal.ItemIncidentalNum += (long)(data.SeiKin - data.NyuKinRui);
                        else
                            depositCouponItemTotal.ItemIncidentalNum -= (long)(data.SeiKin - data.NyuKinRui);
                        depositCouponItemTotal.ItemIncidental = string.Format("{0:#,0}", depositCouponItemTotal.ItemIncidentalNum);
                        break;
                    case "通行料":
                        if (newValue)
                            depositCouponItemTotal.ItemTollFeeNum += (long)(data.SeiKin - data.NyuKinRui);
                        else
                            depositCouponItemTotal.ItemTollFeeNum -= (long)(data.SeiKin - data.NyuKinRui);
                        depositCouponItemTotal.ItemTollFee = string.Format("{0:#,0}", depositCouponItemTotal.ItemTollFeeNum);
                        break;
                    case "手配料":
                        if (newValue)
                            depositCouponItemTotal.ItemArrangementFeeNum += (long)(data.SeiKin - data.NyuKinRui);
                        else
                            depositCouponItemTotal.ItemArrangementFeeNum -= (long)(data.SeiKin - data.NyuKinRui);
                        depositCouponItemTotal.ItemArrangementFee = string.Format("{0:#,0}", depositCouponItemTotal.ItemArrangementFeeNum);
                        break;
                    case "ガイド料":
                        if (newValue)
                            depositCouponItemTotal.ItemGuideFeeNum += (long)(data.SeiKin - data.NyuKinRui);
                        else
                            depositCouponItemTotal.ItemGuideFeeNum -= (long)(data.SeiKin - data.NyuKinRui);
                        depositCouponItemTotal.ItemGuideFee = string.Format("{0:#,0}", depositCouponItemTotal.ItemGuideFeeNum);
                        break;
                    case "積込品":
                        if (newValue)
                            depositCouponItemTotal.ItemLoadedNum += (long)(data.SeiKin - data.NyuKinRui);
                        else
                            depositCouponItemTotal.ItemLoadedNum -= (long)(data.SeiKin - data.NyuKinRui);
                        depositCouponItemTotal.ItemLoaded = string.Format("{0:#,0}", depositCouponItemTotal.ItemLoadedNum);
                        break;
                    case "キャンセル料":
                        if (newValue)
                            depositCouponItemTotal.ItemCancellationNum += (long)(data.SeiKin - data.NyuKinRui);
                        else
                            depositCouponItemTotal.ItemCancellationNum -= (long)(data.SeiKin - data.NyuKinRui);
                        depositCouponItemTotal.ItemCancellation = string.Format("{0:#,0}", depositCouponItemTotal.ItemCancellationNum);
                        break;
                }
                depositCouponItemTotal.TotalNum = depositCouponItemTotal.ItemFareNum + depositCouponItemTotal.ItemIncidentalNum + depositCouponItemTotal.ItemArrangementFeeNum +
                    depositCouponItemTotal.ItemGuideFeeNum + depositCouponItemTotal.ItemLoadedNum + depositCouponItemTotal.ItemTollFeeNum + depositCouponItemTotal.ItemCancellationNum;
                depositCouponItemTotal.Total = string.Format("{0:#,0}", depositCouponItemTotal.TotalNum);
                if (!isCheckAll)
                    StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }
        /// <summary>
        /// event when check all button in grid change
        /// </summary>
        /// <param name="newValue"></param>
        /// <returns></returns>
        protected void CheckedItemAllChanged(bool newValue)
        {
            try
            {
                foreach (var item in depositCouponResult.depositCouponGrids.Where(x => x.NyuKinKbn != 2).ToList())
                {
                    CheckedValueGridChanged(item, newValue, true);
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }
        protected async void AdjustHeightWhenTabChanged()
        {
            try
            {
                await Task.Run(() =>
                {
                    InvokeAsync(StateHasChanged).Wait();
                    JSRuntime.InvokeVoidAsync("loadPageScript", "billCheckListPage", "AdjustHeight");
                });
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        /// <summary>
        /// Get color for each row in grid check list data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected int GetColorPattern(DepositCouponGrid data)
        {
            try
            {
                if (data is null)
                {
                    return 0;
                }
                BillCheckListColorPattern result;
                if (data.NyuKinKbn == 2)
                {
                    result = BillCheckListColorPattern.Deposited;
                }
                else if (data.NCouKbn == 2)
                {
                    result = BillCheckListColorPattern.Coupon;
                }
                else if (data.NCouKbn == 3)
                {
                    result = BillCheckListColorPattern.Some;
                }
                else if (data.NyuKinKbn == 3)
                {
                    result = BillCheckListColorPattern.PartialEntry;
                }
                else if (data.NyuKinKbn == 4)
                {
                    result = BillCheckListColorPattern.OverPayment;
                }
                else
                {
                    result = BillCheckListColorPattern.NotPayment;
                }
                return (int)result;
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
                return 0;
            }
        }
        /// <summary>
        /// select data when select page chage
        /// </summary>
        /// <param name="index"></param>
        protected async Task SelectPage(int index, string code)
        {
            try
            {
                isLoading = true;
                await InvokeAsync(StateHasChanged);
                errorMessage = string.Empty;
                //param code != null : choose from code, else choose from form search
                if (index >= 0)
                {
                    depositCouponFilter.Code = code;
                    if (string.IsNullOrWhiteSpace(code))
                    {
                        codeList = await depositCouponService.GetDepositCouponCodesAsync(depositCouponFilter);
                        depositCouponFilter.Code = codeList.FirstOrDefault();
                    }
                    CurrentPage = index;
                    selectedPage = index;
                    depositCouponFilter.Offset = index * itemPerPage;
                    depositCouponFilter.Limit = itemPerPage;
                    depositCouponResult = await depositCouponService.GetDepositCouponsAsync(depositCouponFilter);
                    IsNoData = !depositCouponResult.depositCouponGrids.Any();
                    NumberOfPage = depositCouponResult.depositCouponGrids.Any() ? depositCouponResult.depositCouponTotal.CountNumber : 0;
                    //calculate total
                    if (depositCouponResult.depositCouponGrids.Any())
                    {
                        await SaveFilterCondtionAsync();
                        depositCouponTotal.TotalAllBillAmount = depositCouponResult.depositCouponTotal.TotalBillAmount;
                        depositCouponTotal.TotalAllCommissionAmount = depositCouponResult.depositCouponTotal.TotalCommissionAmount;
                        depositCouponTotal.TotalAllCumulativeDeposit = depositCouponResult.depositCouponTotal.TotalCumulativeDeposit;
                        depositCouponTotal.TotalAllSalesAmount = depositCouponResult.depositCouponTotal.TotalSaleAmount;
                        depositCouponTotal.TotalAllTaxAmount = depositCouponResult.depositCouponTotal.TotalTaxAmount;
                        depositCouponTotal.TotalAllTaxIncluded = depositCouponResult.depositCouponTotal.TotalTaxIncluded;
                        depositCouponTotal.TotalAllUnpaidAmount = depositCouponResult.depositCouponTotal.TotalUnpaidAmount;
                        depositCouponTotal.TotalPageBillAmount = depositCouponResult.depositCouponGrids.Sum(x => (long)x.SeiKin);
                        depositCouponTotal.TotalPageCommissionAmount = depositCouponResult.depositCouponGrids.Sum(x => (long)x.SyaRyoTes);
                        depositCouponTotal.TotalPageCumulativeDeposit = (long)depositCouponResult.depositCouponGrids.Sum(x => (long)x.NyuKinRui);
                        depositCouponTotal.TotalPageSalesAmount = depositCouponResult.depositCouponGrids.Sum(x => (long)x.UriGakKin);
                        depositCouponTotal.TotalPageTaxAmount = depositCouponResult.depositCouponGrids.Sum(x => (long)x.SyaRyoSyo);
                        depositCouponTotal.TotalPageTaxIncluded = depositCouponResult.depositCouponGrids.Sum(x => (long)x.UriGakKin + (long)x.SyaRyoSyo);
                        depositCouponTotal.TotalPageUnpaidAmount = (long)depositCouponResult.depositCouponGrids.Sum(x => (long)x.SeiKin - (long)x.NyuKinRui);
                        foreach (var item in depositCouponResult.depositCouponGrids.Where(x => x.NyuKinKbn != 2).ToList())
                        {
                            var data = GridCheckDatas.Where(x => x.UkeNo == item.UkeNo && x.MisyuRen == item.MisyuRen).FirstOrDefault();
                            if (data != null)
                            {
                                item.Checked = data.Checked;
                            }
                        }
                        itemCheckAll = depositCouponResult.depositCouponGrids.Where(x => x.NyuKinKbn != 2)
                            .All(x => GridCheckDatas.Any(y => (y.UkeNo == x.UkeNo) && (y.MisyuRen == x.MisyuRen) && x.Checked));
                        LastRowCheckDatas = depositCouponResult.depositCouponGrids.FirstOrDefault();
                        if (!GridCheckDatas.Any())
                            itemCheckAll = false;
                    }
                }
                isLoading = false;
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                isLoading = false;
                errorModalService.HandleError(ex);
            }
        }

        public async void OnClickButtonPre()
        {
            try
            {
                int idexPre = codeList.IndexOf(depositCouponFilter.Code) - 1;
                if (idexPre == 0)
                {
                    depositCouponFilter.Code = codeList[idexPre];
                }
                else
                {
                    if (idexPre > 0)
                    {
                        depositCouponFilter.Code = codeList[idexPre];
                    }
                }
                if (formContext.Validate())
                {
                    GridCheckDatas = new List<DepositCouponGrid>();
                    LastRowCheckDatas = new DepositCouponGrid();
                    depositCouponItemTotal = new DepositCouponItemTotal()
                    {
                        ItemArrangementFee = "0",
                        StatiticsDeposit = "0",
                        ItemCancellation = "0",
                        ItemLoaded = "0",
                        ItemGuideFee = "0",
                        ItemTollFee = "0",
                        ItemFare = "0",
                        ItemIncidental = "0",
                        Total = "0"
                    };
                    await SelectPage(0, depositCouponFilter.Code);
                }
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }
        /// <summary>
        /// event when click button next
        /// </summary>
        ///
        public async void OnClickButtonNext()
        {
            try
            {
                int idexNext = codeList.IndexOf(depositCouponFilter.Code) + 1;
                if (idexNext == (codeList.Count - 1))
                {
                    depositCouponFilter.Code = codeList[idexNext];
                }
                else
                {
                    if (idexNext < (codeList.Count - 1))
                    {
                        depositCouponFilter.Code = codeList[idexNext];
                    }
                }
                if (formContext.Validate())
                {
                    GridCheckDatas = new List<DepositCouponGrid>();
                    LastRowCheckDatas = new DepositCouponGrid();
                    depositCouponItemTotal = new DepositCouponItemTotal()
                    {
                        ItemArrangementFee = "0",
                        StatiticsDeposit = "0",
                        ItemCancellation = "0",
                        ItemLoaded = "0",
                        ItemGuideFee = "0",
                        ItemTollFee = "0",
                        ItemFare = "0",
                        ItemIncidental = "0",
                        Total = "0"
                    };
                    await SelectPage(0, depositCouponFilter.Code);
                }
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public async Task OpenPopUp(string name)
        {
            try
            {
                switch(name)
                {
                    case "CharterInquiry":
                        outDataTable = new OutDataTable()
                        {
                            FutTumRen = LastRowCheckDatas.FutTumRen,
                            FutuUnkRen = LastRowCheckDatas.FutuUnkRen,
                            SeiFutSyu = LastRowCheckDatas.SeiFutSyu,
                            UkeNo = LastRowCheckDatas.UkeNo
                        };
                        isOpenCharterInquiryPopUp = await depositCouponService.CheckOpenChaterInquiryPopUpAsync(outDataTable);
                        errorMessage = isOpenCharterInquiryPopUp ? string.Empty : ErrorMessage.InvalidDepositCouponNoData;
                        break;
                    case "Lump":
                        isOpenLumpPopUp = long.Parse(depositCouponItemTotal.StatiticsDeposit) == depositCouponItemTotal.TotalNum;
                        errorMessage = isOpenLumpPopUp ? string.Empty : ErrorMessage.InvalidDepositCoupon6;
                        break;
                    case "CouponBalance":
                        isOpenCouponBalancePopUp = await depositCouponService.CheckOpenCouponBalancePopUpAsync();
                        errorMessage = isOpenCouponBalancePopUp ? string.Empty : ErrorMessage.InvalidDepositCouponNoData;
                        break;
                    case "Coupon":
                        outDataTable = new OutDataTable()
                        {
                            FutTumRen = LastRowCheckDatas.FutTumRen,
                            FutuUnkRen = LastRowCheckDatas.FutuUnkRen,
                            SeiFutSyu = LastRowCheckDatas.SeiFutSyu,
                            UkeNo = LastRowCheckDatas.UkeNo
                        };
                        isOpenCouponPopUp = await depositCouponService.CheckOpenCouponPopUpAsync(outDataTable);
                        errorMessage = string.Empty;
                        break;
                    case "Individual":
                        isOpenDepositPaymentPopUp = true;
                        errorMessage = string.Empty;
                        break;
                }
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }
        public void UpdateValue(string propertyName, dynamic value)
        {
            try
            {
                errorMessage = string.Empty;
                if (string.IsNullOrWhiteSpace(value))
                {
                    value = "0";
                }
                int tmp;
                CommonUtil.NumberTryParse(value, out tmp);
                value = tmp.ToString();
                if (tmp.ToString().Length > 9)
                    value = tmp.ToString().Substring(0, 9);
                var propertyInfo = depositCouponItemTotal.GetType().GetProperty(propertyName);
                propertyInfo.SetValue(depositCouponItemTotal, value, null);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public void OpenPayDepositPopUp(DepositCouponGrid item)
        {
            try
            {
                dbClicked = false;
                LastRowCheckDatas = item;
                isOpenDepositPaymentPopUp = true;
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public async Task HandleMouseDown(MouseEventArgs e)
        {
            try
            {
                if (dbClicked)
                {
                    return;
                }
                dbClicked = true;
                await Task.Delay(500);
                if (!dbClicked) return;
                dbClicked = false;

                if (!e.ShiftKey && !e.CtrlKey)
                {
                    //double click
                    if (e.Detail != 2)
                    {
                        await blazorContextMenuService.ShowMenu("gridRowClickMenu1", Convert.ToInt32(e.ClientX) + 10, Convert.ToInt32(e.ClientY) + 10);
                    }
                }
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected async Task OnChangePage(int page)
        {
            try
            {
                await SelectPage(page, depositCouponFilter.Code);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected void OnChangeItemPerPage(byte _itemPerPage)
        {
            try
            {
                itemPerPage = _itemPerPage;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public async Task OnReset()
        {
            try
            {
                paging = new Pagination();
                await FilterConditionService.DeleteCustomFilerCondition(syainCdSeq, 0, FormFilterName.DepositCoupon);
                await OnInitializedAsync();
            } catch(Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }
        #endregion
    }
}
