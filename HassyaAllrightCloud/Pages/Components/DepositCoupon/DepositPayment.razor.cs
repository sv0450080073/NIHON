using BlazorContextMenu;
using HassyaAllrightCloud.Commons;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto.DepositCoupon;
using HassyaAllrightCloud.IService;
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

namespace HassyaAllrightCloud.Pages.Components.DepositCoupon
{
    public class DepositPaymentBase : ComponentBase
    {
        [Inject]
        protected IStringLocalizer<DepositPayment> Lang { get; set; }
        [Inject]
        protected IStringLocalizer<CommonMessages> CommonLang { get; set; }
        [Inject]
        protected IDepositCouponService depositCouponService { get; set; }
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }
        [Inject]
        protected IBlazorContextMenuService blazorContextMenuService { get; set; }
        [Inject]
        protected IErrorHandlerService errorModalService { get; set; }
        [Parameter]
        public bool isOpenDepositPaymentPopUp { get; set; }
        [Parameter]
        public EventCallback<bool> isOpenDepositPaymentPopUpChanged { get; set; }
        [Parameter]
        public DepositPaymentFilter depositPaymentFilter { get; set; } = new DepositPaymentFilter();
        [Parameter]
        public bool isUpdated { get; set; }
        [Parameter]
        public EventCallback<bool> isUpdatedChanged { get; set; }
        [Parameter]
        public DepositCouponFilter depositCouponFilter { get; set; }
        [Parameter]
        public DepositCouponGrid depositCouponGrid { get; set; } = new DepositCouponGrid();
        public bool isCreate { get; set; } = true;
        public bool checkCreate { get; set; }
        protected int NumberOfPage;
        protected int MaxPageCount = 5;
        protected int CurrentPage = 1;
        public string errorMessage { get; set; }
        public List<DepositMethod> depositMethods { get; set; } = new List<DepositMethod>();
        public List<DepositOffice> depositOffices { get; set; } = new List<DepositOffice>();
        public List<DepositTransferBank> depositTransferBanks { get; set; } = new List<DepositTransferBank>();
        public List<DepositPaymentGrid> depositPaymentGrids { get; set; } = new List<DepositPaymentGrid>();
        public DepositPaymentGrid selectedDepositPaymentGrid { get; set; } = new DepositPaymentGrid();
        public DepositCouponPayment depositCouponPayment { get; set; } = new DepositCouponPayment();
        public DepositPaymentTotal depositPaymentTotal { get; set; } = new DepositPaymentTotal();
        public DepositPaymentHaitaCheck depositPaymentHaitaCheck { get; set; } = new DepositPaymentHaitaCheck();
        public Dictionary<string, string> LangDic = new Dictionary<string, string>();
        public EditContext editContext;
        public int LastXClicked { get; set; }
        public int LastYClicked { get; set; }
        public int currentNo { get; set; }
        public bool IsValid { get; set; } = true;
        public bool isOpenCouponPaymentUpDatePopUp { get; set; }
        public bool isDeletedPayment { get; set; }
        public bool isLoading { get; set; } = true;
        protected override async Task OnInitializedAsync()
        {
            try
            {
                LangDic = Lang.GetAllStrings().ToDictionary(l => l.Name, l => l.Value);
                depositMethods = await depositCouponService.GetDepositMethodAsync();
                depositOffices = await depositCouponService.GetDepositOfficeAsync();
                //depositOffices.Insert(0, null);
                depositTransferBanks = await depositCouponService.GetDepositTransferBankAsync();
                //depositTransferBanks.Insert(0, null);
                InitDepositCouponPayment();
                await SelectPage(depositPaymentFilter.Offset);
                if(selectedDepositPaymentGrid != null)
                    depositPaymentHaitaCheck = await depositCouponService.GetDepositPaymentHaitaCheckAsync(depositCouponGrid, selectedDepositPaymentGrid);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public void InitDepositCouponPayment()
        {
            try
            {
                depositCouponPayment = new DepositCouponPayment()
                {
                    OffsetPaymentTables = new List<OffsetPaymentTable>() { new OffsetPaymentTable() }
                };
                depositPaymentFilter.UkeNo = depositCouponGrid.UkeNo;
                depositPaymentFilter.FutTumRen = depositCouponGrid.FutTumRen;
                depositPaymentFilter.FutuUnkRen = depositCouponGrid.FutuUnkRen;
                depositPaymentFilter.SeiFutSyu = depositCouponGrid.SeiFutSyu;
                depositPaymentFilter.Offset = 0;
                depositPaymentFilter.Limit = Common.LimitPage;
                depositCouponGrid.HaiSYmdString = string.IsNullOrWhiteSpace(depositCouponGrid.HaiSYmd) ? string.Empty
                    : string.Format("{0:yyyy/MM/dd（ddd）hh:mm}", DateTime.ParseExact(depositCouponGrid.HaiSYmd, "yyyyMMdd", CultureInfo.CurrentCulture));
                depositCouponGrid.TouYmdString = string.IsNullOrWhiteSpace(depositCouponGrid.TouYmd) ? string.Empty
                    : string.Format("{0:yyyy/MM/dd（ddd）hh:mm}", DateTime.ParseExact(depositCouponGrid.TouYmd, "yyyyMMdd", CultureInfo.CurrentCulture));
                depositCouponGrid.HaiSTimeString = string.IsNullOrWhiteSpace(depositCouponGrid.HaiSTime) ? string.Empty : CommonHelper.ConvertStringToTimeString(depositCouponGrid.HaiSTime).Substring(0, 5);
                depositCouponGrid.TouChTimeString = string.IsNullOrWhiteSpace(depositCouponGrid.TouChTime) ? string.Empty : CommonHelper.ConvertStringToTimeString(depositCouponGrid.TouChTime).Substring(0, 5);

                depositCouponPayment.BillAmount = string.Format("{0:#,0}", depositCouponGrid.SeiKin);
                depositCouponPayment.CumulativeDeposit = string.Format("{0:#,0}", depositCouponGrid.NyuKinRui);
                depositCouponPayment.CurrentDeposit = "0";
                depositCouponPayment.NumberOfCoupons = string.Format("{0:#,0}", depositPaymentGrids.Where(x => x.CouTblSeq != 0).Count());
                depositCouponPayment.SumCouponsApplied = string.Format("{0:#,0}", depositCouponGrid.CouKesRui);
                depositCouponPayment.CurrentApplied = "0";
                depositCouponPayment.CumulativeCouponsApplied = string.Format("{0:#,0}", depositCouponGrid.CouKesRui);
                depositCouponPayment.DepositMethod = depositMethods.FirstOrDefault()?.CodeKbn;
                depositCouponPayment.DepositType = 1;
                depositCouponPayment.DepositOffice = depositOffices.Any() ? depositOffices.FirstOrDefault() : new DepositOffice();
                depositCouponPayment.DepositTransferBank = depositTransferBanks.Any() ? depositTransferBanks.FirstOrDefault() : new DepositTransferBank();
                editContext = new EditContext(depositCouponPayment);
                UpdateUnpaidAmount();
                UpdateCumulativeCouponsApplied();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }
        
        public void ChangeValueForm(string ValueName, dynamic value, int? maxlength = null)
        {
            try
            {
                errorMessage = string.Empty;
                if (maxlength != null && value is string)
                {
                    value = ((string)value).TruncateWithMaxLength(maxlength.GetValueOrDefault());
                }
                if (ValueName == nameof(depositCouponPayment.DepositAmount))
                {
                    depositCouponPayment.CurrentDeposit = string.Format("{0:#,0}", ((int?)value).GetValueOrDefault());
                    UpdateUnpaidAmount();
                }

                var propertyInfo = depositCouponPayment.GetType().GetProperty(ValueName);
                propertyInfo.SetValue(depositCouponPayment, value, null);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected async Task ChangeValueTable(OffsetPaymentTable offsetPaymentTable, string ValueName, dynamic value, int? maxlength = null)
        {
            try
            {
                errorMessage = string.Empty;
                if (maxlength != null)
                {
                    value = ((string)value).TruncateWithMaxLength((int)maxlength);
                }

                depositCouponPayment.NumberOfCoupons = string.Format("{0:#,0}", depositCouponPayment.OffsetPaymentTables.Count() + depositPaymentGrids.Where(x => x.CouTblSeq != 0).Count());
                if (nameof(offsetPaymentTable.FaceValue) == ValueName)
                {
                    offsetPaymentTable.ApplicationAmount = Math.Min(((int?)value).GetValueOrDefault(),
                        (int)(long.Parse(depositCouponPayment.BillAmount, NumberStyles.Currency) - long.Parse(depositCouponPayment.SumCouponsApplied, NumberStyles.Currency)));
                    depositCouponPayment.CurrentApplied = string.Format("{0:#,0}", depositCouponPayment.OffsetPaymentTables.Sum(x => x.ApplicationAmount.GetValueOrDefault()));
                    depositCouponPayment.CumulativeCouponsApplied = string.Format("{0:#,0}", depositCouponPayment.OffsetPaymentTables.Sum(x => x.ApplicationAmount.GetValueOrDefault())
                        + long.Parse(depositCouponPayment.SumCouponsApplied, NumberStyles.Currency));
                }
                if (nameof(offsetPaymentTable.ApplicationAmount) == ValueName)
                {
                    depositCouponPayment.CurrentApplied = string.Format("{0:#,0}", ((int?)value).GetValueOrDefault());
                    depositCouponPayment.CumulativeCouponsApplied = string.Format("{0:#,0}", long.Parse(depositCouponPayment.SumCouponsApplied, NumberStyles.Currency)
                        + ((int?)value).GetValueOrDefault());
                    if (depositCouponPayment.OffsetPaymentTables.Any())
                    {
                        depositCouponPayment.OffsetPaymentTables.FirstOrDefault().BillAmount = depositCouponPayment.BillAmount;
                        depositCouponPayment.OffsetPaymentTables.FirstOrDefault().SumCouponsApplied = depositCouponPayment.SumCouponsApplied;
                    }
                }

                var propertyInfo = offsetPaymentTable.GetType().GetProperty(ValueName);
                propertyInfo.SetValue(offsetPaymentTable, value, null);

                depositCouponPayment.TotalApplicationAmount = depositCouponPayment.OffsetPaymentTables.Sum(x => x.ApplicationAmount.GetValueOrDefault());
                depositCouponPayment.TotalFaceValue = depositCouponPayment.OffsetPaymentTables.Sum(x => x.FaceValue.GetValueOrDefault());
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public void CloseModal()
        {
            try
            {
                isOpenDepositPaymentPopUp = false;
                isOpenDepositPaymentPopUpChanged.InvokeAsync(isOpenDepositPaymentPopUp);
                isUpdated = true;
                isUpdatedChanged.InvokeAsync(isUpdated);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public void LoadPaymentType(string code)
        {
            try
            {
                depositCouponPayment.DepositMethod = code;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public bool EnablePayment(string codes)
        {
            try
            {
                string[] codeList = codes.Split(',');
                return codeList.Any(x => x == depositCouponPayment.DepositMethod);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
                return false;
            }
        }

        public void AddRowOffsetPaymentTable(KeyboardEventArgs e, int lastIndex)
        {
            try
            {
                if (e.Code == "Enter" && depositCouponPayment.OffsetPaymentTables.Count() == (lastIndex + 1))
                {
                    depositCouponPayment.OffsetPaymentTables.Add(new OffsetPaymentTable());
                    StateHasChanged();
                }
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
                await InvokeAsync(StateHasChanged);
                errorMessage = string.Empty;
                depositPaymentFilter.Offset = index * Common.LimitPage;
                CurrentPage = index;
                if (index >= 0)
                {
                    depositPaymentGrids = await depositCouponService.GetDepositPaymentGridAsync(depositPaymentFilter);
                    NumberOfPage = (depositPaymentGrids.Count() + Common.LimitPage - 1) / Common.LimitPage;
                    depositCouponPayment.NumberOfCoupons = string.Format("{0:#,0}", depositPaymentGrids.Where(x => x.CouTblSeq != 0).Count());
                    depositCouponPayment.CumulativeDeposit = string.Format("{0:#,0}", depositPaymentGrids.Sum(x => x.KesG));
                    depositCouponPayment.SumCouponsApplied = string.Format("{0:#,0}", depositPaymentGrids.Sum(x => x.CouGkin));
                    UpdateUnpaidAmount();
                    UpdateCumulativeCouponsApplied();

                    if (depositPaymentGrids.Any())
                    {
                        NumberOfPage = (depositPaymentGrids.FirstOrDefault().CountNumber + Common.LimitPage - 1) / Common.LimitPage;
                        currentNo = 1 + CurrentPage * Common.LimitPage;
                        selectedDepositPaymentGrid = depositPaymentGrids.FirstOrDefault();
                        //total
                        depositPaymentTotal.TotalPageCash = Convert.ToInt64(depositPaymentGrids.Sum(x => x.NS_NyuSihSyu == 1 ? x.KesG : 0));
                        depositPaymentTotal.TotalPageTransfer = Convert.ToInt64(depositPaymentGrids.Sum(x => x.NS_NyuSihSyu == 2 ? x.KesG : 0));
                        depositPaymentTotal.TotalPageTransferFee = Convert.ToInt64(depositPaymentGrids.Sum(x => x.FurKesG));
                        depositPaymentTotal.TotalPageTransferSupport = Convert.ToInt64(depositPaymentGrids.Sum(x => x.KyoKesG));
                        depositPaymentTotal.TotalPageCard = Convert.ToInt64(depositPaymentGrids.Sum(x => x.NS_NyuSihSyu == 3 ? x.KesG : 0));
                        depositPaymentTotal.TotalPageCommercialPaper = Convert.ToInt64(depositPaymentGrids.Sum(x => x.NS_NyuSihSyu == 4 ? x.KesG : 0));
                        depositPaymentTotal.TotalPageOffset = Convert.ToInt64(depositPaymentGrids.Sum(x => x.NS_NyuSihSyu == 5 ? x.KesG : 0));
                        depositPaymentTotal.TotalPageAdjustment = Convert.ToInt64(depositPaymentGrids.Sum(x => x.NS_NyuSihSyu == 6 ? x.KesG : 0));
                        depositPaymentTotal.TotalPageOther1 = Convert.ToInt64(depositPaymentGrids.Sum(x => x.NS_NyuSihSyu == 91 ? x.KesG : 0));
                        depositPaymentTotal.TotalPageOther2 = Convert.ToInt64(depositPaymentGrids.Sum(x => x.NS_NyuSihSyu == 92 ? x.KesG : 0));
                        depositPaymentTotal.TotalPageTotalDeposit = Convert.ToInt64(depositPaymentGrids.Sum(x => x.KesG + x.FurKesG + x.KyoKesG));
                        depositPaymentTotal.TotalPageCouponAppliedAmount = Convert.ToInt64(depositPaymentGrids.Sum(x => x.CouKesG));
                        depositPaymentTotal.TotalAllCash = depositPaymentGrids.FirstOrDefault().TotalAllCash;
                        depositPaymentTotal.TotalAllTransfer = depositPaymentGrids.FirstOrDefault().TotalAllTransfer;
                        depositPaymentTotal.TotalAllTransferFee = depositPaymentGrids.FirstOrDefault().TotalAllTransferFee;
                        depositPaymentTotal.TotalAllTransferSupport = depositPaymentGrids.FirstOrDefault().TotalAllTransferSupport;
                        depositPaymentTotal.TotalAllCard = depositPaymentGrids.FirstOrDefault().TotalAllCard;
                        depositPaymentTotal.TotalAllCommercialPaper = depositPaymentGrids.FirstOrDefault().TotalAllCommercialPaper;
                        depositPaymentTotal.TotalAllOffset = depositPaymentGrids.FirstOrDefault().TotalAllOffset;
                        depositPaymentTotal.TotalAllAdjustment = depositPaymentGrids.FirstOrDefault().TotalAllAdjustment;
                        depositPaymentTotal.TotalAllOther1 = depositPaymentGrids.FirstOrDefault().TotalAllOther1;
                        depositPaymentTotal.TotalAllOther2 = depositPaymentGrids.FirstOrDefault().TotalAllOther2;
                        depositPaymentTotal.TotalAllTotalDeposit = depositPaymentGrids.FirstOrDefault().TotalAllTotalDeposit;
                        depositPaymentTotal.TotalAllCouponAppliedAmount = depositPaymentGrids.FirstOrDefault().TotalAllCouponAppliedAmount;
                        SelectPayment();
                    }
                }
                isLoading = false;
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public bool DisableCheckbox(string codeKbn)
        {
            try
            {
                return (!checkCreate ? (selectedDepositPaymentGrid == null ? isCreate : codeKbn == "07" ?
                    (selectedDepositPaymentGrid.NS_NyuSihSyu != 0) : (selectedDepositPaymentGrid.NS_NyuSihSyu == 0)) : isCreate) || isCreate;
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
                return false;
            }
        }

        protected int[] GetPagination()
        {
            try
            {
                if (NumberOfPage <= MaxPageCount)
                {
                    return Enumerable.Range(0, NumberOfPage).ToArray();
                }
                else
                {
                    int BeginIndex = ((int)Math.Floor(CurrentPage * 1.0 / MaxPageCount)) * MaxPageCount;
                    int Count = Math.Min(MaxPageCount, NumberOfPage - BeginIndex);
                    return Enumerable.Range(BeginIndex, Count).ToArray();
                }
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
                return new int[] { };
            }
        }

        public void UpdateUnpaidAmount()
        {
            try
            {
                depositCouponPayment.UnpaidAmount = string.Format("{0:#,0}", (long.Parse(depositCouponPayment.BillAmount, NumberStyles.Currency) - long.Parse(depositCouponPayment.CumulativeDeposit, NumberStyles.Currency)
                    - long.Parse(depositCouponPayment.CurrentDeposit, NumberStyles.Currency)));
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public void UpdateCumulativeCouponsApplied()
        {
            try
            {
                depositCouponPayment.CumulativeCouponsApplied = string.Format("{0:#,0}", (long.Parse(depositCouponPayment.SumCouponsApplied, NumberStyles.Currency)
                    + long.Parse(depositCouponPayment.CurrentApplied, NumberStyles.Currency)));
                if (long.Parse(depositCouponPayment.SumCouponsApplied, NumberStyles.Currency) > long.Parse(depositCouponPayment.BillAmount, NumberStyles.Currency))
                    depositCouponPayment.SumCouponsApplied = depositCouponPayment.BillAmount;
                if (long.Parse(depositCouponPayment.CumulativeCouponsApplied, NumberStyles.Currency) > long.Parse(depositCouponPayment.BillAmount, NumberStyles.Currency))
                    depositCouponPayment.CumulativeCouponsApplied = depositCouponPayment.BillAmount;
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public async Task HandleMouseDown(MouseEventArgs e, int index, int noIndex)
        {
            try
            {
                if (!e.ShiftKey && !e.CtrlKey && isCreate)
                {
                    selectedDepositPaymentGrid = depositPaymentGrids[index];
                    currentNo = noIndex;
                    LastXClicked = Convert.ToInt32(e.ClientX) + 10;
                    LastYClicked = Convert.ToInt32(e.ClientY) + 10;
                    SelectPayment();
                    //double click
                    if (e.Detail != 2)
                    {
                        await blazorContextMenuService.ShowMenu("gridRowClickMenu", LastXClicked, LastYClicked);
                    }
                }
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public void DbClick(int? index = null, int? noIndex = null)
        {
            try
            {
                if (isCreate)
                {
                    isCreate = false;
                    checkCreate = false;
                    selectedDepositPaymentGrid = index == null ? selectedDepositPaymentGrid : depositPaymentGrids[(int)index];
                    currentNo = noIndex == null ? currentNo : (int)noIndex;
                    if (depositCouponPayment.DepositMethod == "07")
                    {
                        depositCouponPayment.CurrentApplied = string.Format("{0:#,0}", selectedDepositPaymentGrid.CouKesG);
                        depositCouponPayment.SumCouponsApplied = string.Format("{0:#,0}", long.Parse(depositCouponPayment.SumCouponsApplied, NumberStyles.Currency)
                            - selectedDepositPaymentGrid.CouKesG, NumberStyles.Currency);
                        depositCouponPayment.IsEditOffsetTable = true;
                    }
                    else
                    {
                        depositCouponPayment.CurrentDeposit = depositCouponPayment.DepositAmount == null ? "0" : depositCouponPayment.DepositAmount.ToString();
                        depositCouponPayment.CumulativeDeposit = string.Format("{0:#,0}", long.Parse(depositCouponPayment.CumulativeDeposit, NumberStyles.Currency)
                            - depositCouponPayment.DepositAmount.GetValueOrDefault());
                    }
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public void OpenCouponPaymentUpDatePopUp(bool isDelete)
        {
            try
            {
                if(!editContext.Validate())
                {
                    StateHasChanged();
                    return;
                }

                if (selectedDepositPaymentGrid.CouTblSeq != 0 && selectedDepositPaymentGrid.NyuKesiKbn != 1)
                {
                    errorMessage = ErrorMessage.DCPBI_T010;
                    StateHasChanged();
                    return;
                }

                isOpenCouponPaymentUpDatePopUp = true;
                isDeletedPayment = isDelete;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public void EnabledUpCreatePayment(bool isEnable)
        {
            try
            {
                isCreate = isEnable;
                currentNo = isEnable ? (1 + CurrentPage * Common.LimitPage) : -1;
                checkCreate = !isEnable;
                depositCouponPayment.IsEditOffsetTable = isEnable;
                selectedDepositPaymentGrid = isEnable ? depositPaymentGrids.FirstOrDefault() : new DepositPaymentGrid();
                InitDepositCouponPayment();
                depositCouponPayment.NumberOfCoupons = string.Format("{0:#,0}", depositPaymentGrids.Where(x => x.CouTblSeq != 0).Count());
                depositCouponPayment.CumulativeDeposit = string.Format("{0:#,0}", depositPaymentGrids.Sum(x => x.KesG));
                depositCouponPayment.SumCouponsApplied = string.Format("{0:#,0}", depositPaymentGrids.Sum(x => x.CouGkin));
                UpdateUnpaidAmount();
                UpdateCumulativeCouponsApplied();
                if (isCreate)
                {
                    SelectPayment();
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public void SelectPayment()
        {
            try
            {
                if (selectedDepositPaymentGrid == null)
                    return;
                depositCouponPayment.DepositOffice = depositOffices.Where(x => long.Parse(x.Code) == selectedDepositPaymentGrid.NyuSihEigSeq).FirstOrDefault();
                depositCouponPayment.DepositMethod = depositMethods.Where(x => long.Parse(x.CodeKbn) == selectedDepositPaymentGrid.NS_NyuSihSyu).FirstOrDefault() == null ? depositCouponPayment.DepositMethod
                : depositMethods.Where(x => long.Parse(x.CodeKbn) == selectedDepositPaymentGrid.NS_NyuSihSyu).FirstOrDefault().CodeKbn;
                if (selectedDepositPaymentGrid.NS_NyuSihSyu == 0)
                {
                    depositCouponPayment.DepositMethod = "07";
                    depositCouponPayment.OffsetPaymentTables = new List<OffsetPaymentTable>();
                    depositCouponPayment.OffsetPaymentTables.Add(new OffsetPaymentTable()
                    {
                        DateOfIssue = string.IsNullOrWhiteSpace(selectedDepositPaymentGrid.NyuSihHakoYmd) ? (DateTime?)null
                        : DateTime.ParseExact(selectedDepositPaymentGrid.NyuSihHakoYmd, "yyyyMMdd", CultureInfo.CurrentCulture),
                        CouponNo = selectedDepositPaymentGrid.CouNo,
                        ApplicationAmount = selectedDepositPaymentGrid.CouKesG,
                        FaceValue = selectedDepositPaymentGrid.CouGkin
                    });
                    depositCouponPayment.TotalApplicationAmount = depositCouponPayment.OffsetPaymentTables.Sum(x => x.ApplicationAmount.GetValueOrDefault());
                    depositCouponPayment.TotalFaceValue = depositCouponPayment.OffsetPaymentTables.Sum(x => x.FaceValue.GetValueOrDefault());
                }
                else
                {
                    depositCouponPayment.DepositDate = string.IsNullOrWhiteSpace(selectedDepositPaymentGrid.NyuSihHakoYmd) ? (DateTime?)null
                        : DateTime.ParseExact(selectedDepositPaymentGrid.NyuSihHakoYmd, "yyyyMMdd", CultureInfo.CurrentCulture);
                    depositCouponPayment.DepositAmount = selectedDepositPaymentGrid.KesG + selectedDepositPaymentGrid.FurKesG + selectedDepositPaymentGrid.KyoKesG;
                }
                switch (depositCouponPayment.DepositMethod)
                {
                    case "02":
                        depositCouponPayment.DepositTransferBank = depositTransferBanks.Where(x => x.BankCd == selectedDepositPaymentGrid.NS_BankCd
                        && x.BankSitCd == selectedDepositPaymentGrid.NS_BankSitCd).FirstOrDefault();
                        depositCouponPayment.DepositType = selectedDepositPaymentGrid.NS_YokinSyu == 1 ? (byte)1 : (byte)2;
                        depositCouponPayment.TransferFee = selectedDepositPaymentGrid.FurKesG;
                        depositCouponPayment.SponsorshipFund = selectedDepositPaymentGrid.KyoKesG;
                        break;

                    case "03":
                        depositCouponPayment.CardApprovalNumber = string.IsNullOrWhiteSpace(selectedDepositPaymentGrid.NS_CardSyo) ? string.Empty : selectedDepositPaymentGrid.NS_CardSyo.Trim();
                        depositCouponPayment.CardSlipNumber = string.IsNullOrWhiteSpace(selectedDepositPaymentGrid.NS_CardDen) ? string.Empty : selectedDepositPaymentGrid.NS_CardDen.Trim();
                        break;

                    case "04":
                        depositCouponPayment.BillDate = string.IsNullOrWhiteSpace(selectedDepositPaymentGrid.NS_TegataYmd) ? (DateTime?)null
                            : DateTime.ParseExact(selectedDepositPaymentGrid.NS_TegataYmd, "yyyyMMdd", CultureInfo.CurrentCulture);
                        depositCouponPayment.BillNo = string.IsNullOrWhiteSpace(selectedDepositPaymentGrid.NS_TegataNo) ? string.Empty : selectedDepositPaymentGrid.NS_TegataNo.Trim();
                        break;

                    case "91":
                        depositCouponPayment.DetailedNameOfDepositMeans11 = string.IsNullOrWhiteSpace(selectedDepositPaymentGrid.NS_EtcSyo1) ? string.Empty : selectedDepositPaymentGrid.NS_EtcSyo1.Trim();
                        depositCouponPayment.DetailedNameOfDepositMeans12 = string.IsNullOrWhiteSpace(selectedDepositPaymentGrid.NS_EtcSyo2) ? string.Empty : selectedDepositPaymentGrid.NS_EtcSyo2.Trim();
                        break;

                    case "92":
                        depositCouponPayment.DetailedNameOfDepositMeans21 = string.IsNullOrWhiteSpace(selectedDepositPaymentGrid.NS_EtcSyo1) ? string.Empty : selectedDepositPaymentGrid.NS_EtcSyo1.Trim();
                        depositCouponPayment.DetailedNameOfDepositMeans22 = string.IsNullOrWhiteSpace(selectedDepositPaymentGrid.NS_EtcSyo2) ? string.Empty : selectedDepositPaymentGrid.NS_EtcSyo2.Trim();
                        break;
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public async Task SavePayment(bool isDeleted)
        {
            try
            {
                isLoading = true;
                await InvokeAsync(StateHasChanged);
                errorMessage = string.Empty;
                if (!editContext.Validate())
                {
                    isLoading = false;
                    await InvokeAsync(StateHasChanged);
                    return;
                }

                if (isDeleted)
                {
                    if (depositCouponPayment.DepositMethod != "07")
                    {
                        var cumulativeDepositNumber = (int)depositCouponGrid.NyuKinRui - (selectedDepositPaymentGrid.KesG + selectedDepositPaymentGrid.FurKesG + selectedDepositPaymentGrid.KyoKesG);
                        depositCouponPayment.CumulativeDeposit = string.Format("{0:#,0}", cumulativeDepositNumber);
                    }
                    else
                    {
                        var cumulativeCouponsAppliedNumber = (int)depositCouponGrid.CouKesRui - selectedDepositPaymentGrid.CouKesG;
                        depositCouponPayment.CumulativeCouponsApplied = string.Format("{0:#,0}", cumulativeCouponsAppliedNumber);
                    }
                    errorMessage = await depositCouponService.SavePaymentAsync(depositCouponGrid, depositCouponPayment, selectedDepositPaymentGrid, true, depositPaymentHaitaCheck);
                }
                else
                {
                    if (depositCouponPayment.DepositMethod != "07")
                    {
                        var cumulativeDepositNumber = (int)depositCouponGrid.NyuKinRui + depositCouponPayment.DepositAmount - (selectedDepositPaymentGrid.KesG + selectedDepositPaymentGrid.FurKesG + selectedDepositPaymentGrid.KyoKesG);
                        depositCouponPayment.CumulativeDeposit = string.Format("{0:#,0}", cumulativeDepositNumber);
                    }
                    errorMessage = await depositCouponService.SavePaymentAsync(depositCouponGrid, depositCouponPayment, checkCreate ? null : selectedDepositPaymentGrid, false, depositPaymentHaitaCheck);
                }
                if (string.IsNullOrWhiteSpace(errorMessage))
                {
                    isCreate = true;
                    isOpenCouponPaymentUpDatePopUp = false;
                    isDeletedPayment = false;
                    selectedDepositPaymentGrid = null;
                    InitDepositCouponPayment();
                    depositPaymentTotal = new DepositPaymentTotal();
                    await SelectPage(0);
                    if(selectedDepositPaymentGrid != null)
                        depositPaymentHaitaCheck = await depositCouponService.GetDepositPaymentHaitaCheckAsync(depositCouponGrid, selectedDepositPaymentGrid);
                }
                isLoading = false;
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }
    }
}
