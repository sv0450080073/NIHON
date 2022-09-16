using HassyaAllrightCloud.Commons.Extensions;
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

namespace HassyaAllrightCloud.Pages.Components.DepositCoupon
{
    public class LumpBase : ComponentBase
    {
        [Inject]
        protected IStringLocalizer<Lump> Lang { get; set; }
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }
        [Inject]
        protected IDepositCouponService depositCouponService { get; set; }
        [Parameter]
        public bool isOpenLumpPopUp { get; set; }
        [Parameter]
        public EventCallback<bool> isOpenLumpPopUpChanged { get; set; }
        [Parameter]
        public string statiticsDeposit { get; set; }
        [Parameter]
        public List<DepositCouponGrid> gridCheckDatas { get; set; }
        [Parameter]
        public bool isUpdated { get; set; }
        [Parameter]
        public EventCallback<bool> isUpdatedChanged { get; set; }
        [Parameter]
        public EventCallback<bool> isLumpUpdatedChanged { get; set; }
        [Inject]
        protected IErrorHandlerService errorModalService { get; set; }
        public EditContext editContext { get; set; }
        public string errorMessage { get; set; }
        public DepositCouponPayment depositCouponPayment { get; set; }
        public List<DepositPaymentHaitaCheck> depositPaymentHaitaChecks { get; set; } = new List<DepositPaymentHaitaCheck>();
        public List<DepositMethod> depositMethods { get; set; } = new List<DepositMethod>();
        public List<DepositOffice> depositOffices { get; set; } = new List<DepositOffice>();
        public List<DepositTransferBank> depositTransferBanks { get; set; } = new List<DepositTransferBank>();
        public Dictionary<string, string> LangDic = new Dictionary<string, string>();
        public bool isFirstRender { get; set; } = true;
        public int Test { get; set; }

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
                depositPaymentHaitaChecks = await depositCouponService.GetDepositPaymentHaitaCheckListAsync(gridCheckDatas);
            } catch(Exception ex)
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
                //depositCouponPayment.BillAmount = string.Format("{0:#,0}", depositCouponGrid.SeiKin);
                //depositCouponPayment.CumulativeDeposit = string.Format("{0:#,0}", depositCouponGrid.NyuKinRui);
                depositCouponPayment.CurrentDeposit = "0";
                //depositCouponPayment.NumberOfCoupons = string.Format("{0:#,0}", depositPaymentGrids.Where(x => x.CouTblSeq != 0).Count());
                //depositCouponPayment.SumCouponsApplied = string.Format("{0:#,0}", depositCouponGrid.CouKesRui);
                depositCouponPayment.CurrentApplied = "0";
                //depositCouponPayment.CumulativeCouponsApplied = string.Format("{0:#,0}", depositCouponGrid.CouKesRui);
                depositCouponPayment.DepositMethod = depositMethods.FirstOrDefault()?.CodeKbn;
                depositCouponPayment.DepositType = 1;
                depositCouponPayment.DepositOffice = depositOffices.Any() ? depositOffices.FirstOrDefault() : new DepositOffice();
                depositCouponPayment.DepositTransferBank = depositTransferBanks.Any() ? depositTransferBanks.FirstOrDefault() : new DepositTransferBank();
                depositCouponPayment.StatiticsDeposit = string.Format("{0:#,0}", long.Parse(statiticsDeposit));
                UpdateUnpaidAmount();
                UpdateCumulativeCouponsApplied();
                editContext = new EditContext(depositCouponPayment);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public void UpdateUnpaidAmount()
        {
            try
            {
                depositCouponPayment.UnpaidAmount = string.Format("{0:#,0}", (int.Parse(depositCouponPayment.BillAmount, NumberStyles.Currency) - int.Parse(depositCouponPayment.CumulativeDeposit, NumberStyles.Currency)
                    - int.Parse(depositCouponPayment.CurrentDeposit, NumberStyles.Currency)));
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
                depositCouponPayment.CumulativeCouponsApplied = string.Format("{0:#,0}", (int.Parse(depositCouponPayment.SumCouponsApplied, NumberStyles.Currency)
                    + int.Parse(depositCouponPayment.CurrentApplied, NumberStyles.Currency)));
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

        public void ChangeValueForm(string ValueName, dynamic value, int? maxlength = null)
        {
            try
            {
                errorMessage = string.Empty;
                if (maxlength != null && value is string)
                {
                    value = ((string)value).TruncateWithMaxLength(((string)value).Contains("-") ? ((int)maxlength + 1) : (int)maxlength);
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

                //depositCouponPayment.NumberOfCoupons = string.Format("{0:#,0}", depositCouponPayment.OffsetPaymentTables.Count() + depositPaymentGrids.Where(x => x.CouTblSeq != 0).Count());
                if (nameof(offsetPaymentTable.FaceValue) == ValueName)
                {
                    //depositCouponPayment.CurrentApplied = string.Format("{0:#,0}", depositCouponPayment.OffsetPaymentTables.Sum(x => int.Parse(x.ApplicationAmount, NumberStyles.Currency)));
                    offsetPaymentTable.ApplicationAmount = Math.Min(((int?)value).GetValueOrDefault(), int.Parse(depositCouponPayment.StatiticsDeposit, NumberStyles.Currency)
                    - depositCouponPayment.OffsetPaymentTables.SkipLast(1).Sum(x => x.ApplicationAmount.GetValueOrDefault()));
                    depositCouponPayment.CumulativeCouponsApplied = string.Format("{0:#,0}", depositCouponPayment.OffsetPaymentTables.Sum(x => x.ApplicationAmount.GetValueOrDefault())
                        + int.Parse(depositCouponPayment.SumCouponsApplied, NumberStyles.Currency));
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
        public async Task SavePayment()
        {
            try
            {
                errorMessage = string.Empty;
                if (!editContext.Validate())
                {
                    await InvokeAsync(StateHasChanged);
                    return;
                }

                errorMessage = await depositCouponService.SaveLumpPaymentAsync(gridCheckDatas, depositCouponPayment, depositPaymentHaitaChecks);
                if (string.IsNullOrWhiteSpace(errorMessage))
                {
                    CloseModal();
                    return;
                }
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
                isOpenLumpPopUp = false;
                isOpenLumpPopUpChanged.InvokeAsync(isOpenLumpPopUp);
                isUpdated = true;
                isUpdatedChanged.InvokeAsync(isUpdated);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }
    }
}
