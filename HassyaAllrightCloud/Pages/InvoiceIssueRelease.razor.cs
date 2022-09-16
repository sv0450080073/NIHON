using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.BillPrint;
using HassyaAllrightCloud.Domain.Dto.InvoiceIssueRelease;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.IService;
using HassyaAllrightCloud.Pages.Components;
using HassyaAllrightCloud.Pages.Components.CommonComponents;
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
    public class InvoiceIssueReleaseBase : ComponentBase
    {
        [CascadingParameter(Name = "ClaimModel")]
        protected ClaimModel ClaimModel { get; set; }
        #region Inject
        [Inject]
        protected IStringLocalizer<InvoiceIssueRelease> Lang { get; set; }
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }
        [Inject]
        protected ICustomerListService CustomerService { get; set; }
        [Inject]
        public IInvoiceIssueReleaseService _invoiceIssueReleaseService { get; set; }
        [Inject]
        protected IErrorHandlerService errorModalService { get; set; }
        [Inject]
        protected IFilterCondition FilterConditionService { get; set; }
        [Inject]
        IGenerateFilterValueDictionary GenerateFilterValueDictionaryService { get; set; }
        #endregion

        #region parameter        
        protected List<InvoiceIssueGrid> GridDatas { get; set; } = new List<InvoiceIssueGrid>();
        public List<InvoiceIssueGrid> GridCheckDatas { get; set; }
        protected List<LoadCustomerList> billAddressList = new List<LoadCustomerList>();
        public Dictionary<string, string> LangDic = new Dictionary<string, string>();
        public Dictionary<string, string> keyValueFilterPairs = new Dictionary<string, string>();
        protected Pagination paging = new Pagination();
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

        public InvoiceIssueFilter invoiceIssueFilter { get; set; }
        protected string dateFormat = "yyyy/MM/dd";
        public byte itemPerPage { get; set; } = 25;

        public int NumberOfPage { get; set; }
        protected int activeTabIndex = 0;
        protected int CurrentPage = 1;
        protected int syainCdSeq;
        protected bool IsValid = true;
        public string errorMessage { get; set; } = string.Empty;
        public EditContext formContext;
        protected bool isCustomerLoaded = false;
        protected CustomerComponent customerFrom;
        protected CustomerComponent customerTo;
        private bool isFromFirstLoaded = false;
        private bool isToFirstLoaded = false;
        protected DefaultCustomerData defaultFrom = new DefaultCustomerData();
        protected DefaultCustomerData defaultTo = new DefaultCustomerData();
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
                invoiceIssueFilter = new InvoiceIssueFilter();
                formContext = new EditContext(invoiceIssueFilter);
                LangDic = Lang.GetAllStrings().ToDictionary(l => l.Name, l => l.Value);
                invoiceIssueFilter.ActiveV = (int)ViewMode.Medium;
                GridCheckDatas = new List<InvoiceIssueGrid>();
                billAddressList = await CustomerService.Get(new ClaimModel().TenantID);
                List<TkdInpCon> filterValues = await FilterConditionService.GetFilterCondition(FormFilterName.InvoiceIssueRelease, 0, syainCdSeq);
                if (filterValues.Any())
                {
                    foreach (var item in filterValues)
                    {
                        var propertyInfo = invoiceIssueFilter.GetType().GetProperty(item.ItemNm);
                        switch(item.ItemNm)
                        {
                            case nameof(invoiceIssueFilter.BillOutputSeq):
                            case nameof(invoiceIssueFilter.BillSerialNumber):
                                propertyInfo.SetValue(invoiceIssueFilter, string.IsNullOrWhiteSpace(item.JoInput) ? (int?)null : Convert.ToInt32(item.JoInput), null);
                                break;
                            case nameof(invoiceIssueFilter.ActiveV):
                                propertyInfo.SetValue(invoiceIssueFilter, int.Parse(item.JoInput), null);
                                break;

                            //case nameof(invoiceIssueFilter.StartBillAddress):
                            //case nameof(invoiceIssueFilter.EndBillAddress):
                            //    LoadCustomerList billAddress = string.IsNullOrWhiteSpace(item.JoInput) ? null 
                            //        : billAddressList.Where(x => x.GyoSyaCd.ToString("D3") + x.TokuiCd.ToString("D4") + x.SitenCd.ToString("D4") == item.JoInput).FirstOrDefault();
                            //    propertyInfo.SetValue(invoiceIssueFilter, billAddress, null);
                            //    break;

                            case nameof(invoiceIssueFilter.StartBillIssuedDate):
                            case nameof(invoiceIssueFilter.EndBillIssuedDate):
                                var datetime = string.IsNullOrWhiteSpace(item.JoInput) ? (DateTime?)null : DateTime.ParseExact(item.JoInput, "yyyyMMdd",CultureInfo.InvariantCulture);
                                propertyInfo.SetValue(invoiceIssueFilter, datetime, null);
                                break;

                            case nameof(invoiceIssueFilter.SelectedGyosyaFrom):
                                if (int.TryParse(item.JoInput, out var fgSeq))
                                    defaultFrom.GyosyaCdSeq = fgSeq;
                                break;
                            case nameof(invoiceIssueFilter.SelectedTokiskFrom):
                                if (int.TryParse(item.JoInput, out var ftSeq))
                                    defaultFrom.TokiskCdSeq = ftSeq;
                                break;
                            case nameof(invoiceIssueFilter.SelectedTokiStFrom):
                                if (int.TryParse(item.JoInput, out var fsSeq))
                                    defaultFrom.TokiStCdSeq = fsSeq;
                                break;

                            case nameof(invoiceIssueFilter.SelectedGyosyaTo):
                                if (int.TryParse(item.JoInput, out var tgSeq))
                                    defaultTo.GyosyaCdSeq = tgSeq;
                                break;
                            case nameof(invoiceIssueFilter.SelectedTokiskTo):
                                if (int.TryParse(item.JoInput, out var ttSeq))
                                    defaultTo.TokiskCdSeq = ttSeq;
                                break;
                            case nameof(invoiceIssueFilter.SelectedTokiStTo):
                                if (int.TryParse(item.JoInput, out var tsSeq))
                                    defaultTo.TokiStCdSeq = tsSeq;
                                break;
                        }
                    }
                }
                isCustomerLoaded = true;
                billAddressList.Insert(0, null);
                isLoading = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                if (firstRender)
                {
                    //await JSRuntime.InvokeVoidAsync("loadPageScript", "billCheckListPage", "fadeToggleBillTitle");
                    await JSRuntime.InvokeVoidAsync("EnterTab", ".enterField", true);
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("EnterTab", ".enterField");
                }
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
                {
                    value = string.Empty;
                }
                if ((ValueName == nameof(invoiceIssueFilter.BillOutputSeq) || ValueName == nameof(invoiceIssueFilter.BillSerialNumber)) && value is string)
                    value = null;
                var propertyInfo = invoiceIssueFilter.GetType().GetProperty(ValueName);
                propertyInfo.SetValue(invoiceIssueFilter, value, null);
                if (formContext.Validate() && isFromFirstLoaded && isToFirstLoaded &&
                    ValueName != nameof(invoiceIssueFilter.SelectedGyosyaFrom) &&
                    ValueName != nameof(invoiceIssueFilter.SelectedTokiskFrom) &&
                    ValueName != nameof(invoiceIssueFilter.SelectedGyosyaTo) &&
                    ValueName != nameof(invoiceIssueFilter.SelectedTokiskTo))
                {
                    GridCheckDatas = new List<InvoiceIssueGrid>();
                    await SelectPage(0);
                }
                await InvokeAsync(StateHasChanged);
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
        protected void CheckedValueGridChanged(InvoiceIssueGrid data, bool newValue, bool isCheckAll)
        {
            try
            {
                var item = GridCheckDatas.Where(x => x.SeiOutSeq == data.SeiOutSeq && x.SeiRen == data.SeiRen).FirstOrDefault();
                if (item == null)
                {
                    GridCheckDatas.Add(data);
                }
                else
                {
                    if (item.Checked && data.Checked && isCheckAll && newValue)
                    {
                        item.Checked = (item != null) && newValue;
                        return;
                    }
                    item.Checked = (item != null) && newValue;
                }
                data.Checked = newValue;
                itemCheckAll = GridDatas.All(x => GridCheckDatas.Any(y => x.SeiOutSeq == x.SeiOutSeq && y.SeiRen == x.SeiRen && x.Checked));
                if (!isCheckAll)
                {
                    StateHasChanged();
                }
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
            foreach (var item in GridDatas.ToList())
            {
                CheckedValueGridChanged(item, newValue, true);
            }
            StateHasChanged();
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

        protected async Task SelectPage(int index)
        {
            try
            {
                isLoading = true;
                StateHasChanged();
                errorMessage = string.Empty;
                //param code != null : choose from code, else choose from form search
                if (index >= 0)
                {
                    CurrentPage = index;
                    invoiceIssueFilter.Offset = index * itemPerPage;
                    invoiceIssueFilter.Limit = itemPerPage;
                    GridDatas = await _invoiceIssueReleaseService.GetInvoiceIssueReleasesAsync(invoiceIssueFilter);
                    NumberOfPage = GridDatas.Any() ? GridDatas.FirstOrDefault().CountNumber : 0;
                    //calculate total
                    if (GridDatas.Any())
                    {
                        keyValueFilterPairs = await GenerateFilterValueDictionaryService.GenerateInvoiceIssueFilter(invoiceIssueFilter);
                        await FilterConditionService.SaveFilterCondtion(keyValueFilterPairs, FormFilterName.InvoiceIssueRelease, 0, syainCdSeq);
                        foreach (var item in GridDatas)
                        {
                            var data = GridCheckDatas.Where(x => x.SeiOutSeq == item.SeiOutSeq && x.SeiRen == item.SeiRen).FirstOrDefault();
                            if (data != null)
                            {
                                item.Checked = data.Checked;
                            }
                        }
                        itemCheckAll = GridDatas.All(x => GridCheckDatas.Any(y => (y.SeiOutSeq == x.SeiOutSeq) && (y.SeiRen == x.SeiRen) && x.Checked));
                    }
                    else
                    {
                        errorMessage = ErrorMessage.IIRNoData;
                    }
                }
                isLoading = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public async Task ReleaseInvoice()
        {
            try
            {
                errorMessage = await _invoiceIssueReleaseService.ReleaseInvoicesAsync(GridCheckDatas.Where(x => x.Checked).ToList());
                GridCheckDatas = new List<InvoiceIssueGrid>();
                itemCheckAll = false;
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    await InvokeAsync(StateHasChanged);
                    return;
                }
                await SelectPage(0);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public async Task SaveGridLayout(bool isDelete)
        {
            //errorMessage = isDelete ? 
        }

        protected async Task OnChangePage(int page)
        {
            await SelectPage(page);
        }

        protected void OnChangeItemPerPage(byte _itemPerPage)
        {
            itemPerPage = _itemPerPage;
            StateHasChanged();
        }

        public async Task OnReset()
        {
            ResetCustomer();
            await FilterConditionService.DeleteCustomFilerCondition(syainCdSeq, 0, FormFilterName.InvoiceIssueRelease);
            await OnInitializedAsync();
            await SelectPage(0);
        }

        private void ResetCustomer()
        {
            defaultFrom.GyosyaCdSeq = null;
            defaultTo.GyosyaCdSeq = null;
            invoiceIssueFilter.SelectedGyosyaFrom = invoiceIssueFilter.SelectedGyosyaTo = null;
            invoiceIssueFilter.SelectedTokiskFrom = invoiceIssueFilter.SelectedTokiskTo = null;
            invoiceIssueFilter.SelectedTokiStFrom = invoiceIssueFilter.SelectedTokiStTo = null;
            customerFrom.UpdateSelectedItems();
            customerTo.UpdateSelectedItems();
        }

        protected async Task FirstLoad(bool isFrom)
        {
            if (isFrom)
                isFromFirstLoaded = true;
            else
                isToFirstLoaded = true;
            if (isFromFirstLoaded && isToFirstLoaded)
            {
                await SelectPage(0);
                StateHasChanged();
            }
        }
        #endregion
    }
}
