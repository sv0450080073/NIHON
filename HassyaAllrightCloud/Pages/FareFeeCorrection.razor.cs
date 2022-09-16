using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Globalization;

namespace HassyaAllrightCloud.Pages
{
    public class FareFeeCorrectionBase : ComponentBase
    {
        #region Inject
        [Inject]
        protected IJSRuntime _jSRuntime { get; set; }
        [Inject]
        protected IStringLocalizer<FareFeeCorrection> _lang { get; set; }
        [Inject]
        protected IFareFeeCorrectionService _service { get; set; }
        [Inject]
        protected IJSRuntime _jsRuntime { get; set; }
        [Inject]
        protected NavigationManager _navigationManager { get; set; }
        [Inject]
        protected IFilterCondition _filterConditionService { get; set; }
        [Inject]
        protected IErrorHandlerService errorModalService { get; set; }
        [Inject]
        protected IGenerateFilterValueDictionary _generateFilterValueDictionaryService { get; set; }
        [Parameter]
        public string UkeCdParam { get; set; }
        [Parameter] public EventCallback<bool> OnCloseFareFeeCorrection { get; set; }
        #endregion Inject

        #region Property
        protected Reservation reservations { get; set; } = new Reservation();
        protected List<CompanyValidate> comapnies { get; set; } = new List<CompanyValidate>();
        protected List<Vehicle> vehicles { get; set; } = new List<Vehicle>();
        protected FareFeeCorrectionModel formModel = new FareFeeCorrectionModel();
        protected List<string> ErrorMessage = new List<string>();
        protected List<VehicleAllocation> vehicleAllocations { get; set; } = new List<VehicleAllocation>();
        protected List<ReservationGrid> DataSource { get; set; }
        public List<ReservationChange> lstReservationChange { get; set; } = new List<ReservationChange>();
        protected List<ReservationGrid> DataSourceBeforeUpdate { get; set; }
        protected ReservationGrid oldRowClickItem { get; set; }
        protected EditContext editFormContext { get; set; }
        protected int activeTabIndex = 0;
        protected int ActiveTabIndex { get => activeTabIndex; set { activeTabIndex = value; StateHasChanged(); } }
        protected bool isFirstRender { get; set; }
        protected int ActiveL { get; set; }
        protected int ActiveV { get; set; }
        protected bool isDisableGroup1 { get; set; } = true;
        protected bool isDisableGroup2 { get; set; } = true;
        protected bool isEnableSave { get; set; }
        protected bool isEnableReflect { get; set; }
        protected bool IsUkeNoValid { get; set; } = true;
        protected int IndexItem { get; set; }
        private HaitaCheckModel _model { get; set; }
        #endregion Property
        protected override async Task OnInitializedAsync()
        {
            try
            {
                editFormContext = new EditContext(formModel);
                await InitData();
                _model = await _service.GetHaitaCheck(UkeCdParam);
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (isFirstRender)
            {
                await _jsRuntime.InvokeVoidAsync("EnterTab", ".farefeecorrection-form");
                await _jSRuntime.InvokeVoidAsync("setEventforCodeNumberFieldV2", ".code-number", true, 0);
                isFirstRender = false;
            }
            await InvokeAsync(StateHasChanged);
        }

        protected void UpdateFormValue(string propertyName, dynamic value)
        {
            try
            {
                SetValue(propertyName, value);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        private async void SetValue(string propertyName, dynamic value)
        {
            var propertyInfo = formModel.GetType().GetProperty(propertyName);
            propertyInfo.SetValue(formModel, value, null);
            var keyValueFilterPairs = _generateFilterValueDictionaryService.GenerateForFareFeeCorrection(formModel).Result;
            if (editFormContext.Validate())
                await _filterConditionService.SaveFilterCondtion(keyValueFilterPairs, FormFilterName.FareFeeCorrection, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq);
            await InvokeAsync(StateHasChanged);
        }

        protected void ChangeMode(MouseEventArgs e, int number, int mode)
        {
            try
            {
                if (mode == (int)ModeChangeV.ViewMode)
                {
                    ActiveV = number;
                    UpdateFormValue(nameof(FareFeeCorrectionModel.ActiveV), ActiveV);
                }    
                else if (mode == (int)ModeChangeV.LayoutMode)
                {
                    ActiveL = number;
                    UpdateFormValue(nameof(FareFeeCorrectionModel.ActiveL), ActiveL);
                }
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected async Task BtnReflect()
        {
            try
            {
                DataSource = DataSource.Select(x => new ReservationGrid()
                {
                    No = x.No,
                    UnkYmd = x.UnkYmd,
                    GoSya = x.GoSya,
                    Eigyo = x.Eigyo,
                    SyaRyo = x.SyaRyo,
                    CompanyCd = x.CompanyCd,
                    YouSha = x.YouSha,
                    UkeCompanyCdSeqCompanyCd = x.UkeCompanyCdSeqCompanyCd,
                    YouJitaFlg = x.YouJitaFlg,
                    SyaRyoUncTmp = oldRowClickItem.No == x.No ? int.Parse(formModel.SyaRyoUnc, NumberStyles.AllowThousands, new CultureInfo("en-au")) : x.SyaRyoUncTmp,
                    SyaRyoSyoTmp = oldRowClickItem.No == x.No ? int.Parse(formModel.SyaRyoSyo, NumberStyles.AllowThousands, new CultureInfo("en-au")) : x.SyaRyoSyoTmp,
                    SyaRyoTesTmp = oldRowClickItem.No == x.No ? int.Parse(formModel.SyaRyoTes, NumberStyles.AllowThousands, new CultureInfo("en-au")) : x.SyaRyoTesTmp,
                    YouUncTmp = oldRowClickItem.No == x.No ? int.Parse(formModel.YouUnc, NumberStyles.AllowThousands, new CultureInfo("en-au")) : x.YouUncTmp,
                    YouZeiTmp = oldRowClickItem.No == x.No ? int.Parse(formModel.YouZei, NumberStyles.AllowThousands, new CultureInfo("en-au")) : x.YouZeiTmp,
                    YouTesTmp = oldRowClickItem.No == x.No ? int.Parse(formModel.YouTes, NumberStyles.AllowThousands, new CultureInfo("en-au")) : x.YouTesTmp,
                    HaishaYmd = x.HaishaYmd,
                    TouYmd = x.TouYmd,
                    YouTblSeq = x.YouTblSeq,
                    UnkRen = x.UnkRen,
                    UkeNo = x.UkeNo,
                    TeiDanNo = x.TeiDanNo,
                    BunkRen = x.BunkRen,
                    UpdYmd = x.UpdYmd,
                    UpdTime = x.UpdTime,
                    IsExistVehicleData = true ? vehicles.Any() : false
                }).ToList();

                var reservationChangeExist = lstReservationChange.FirstOrDefault(x => x.UkeNo == oldRowClickItem.UkeNo && x.BunkRen == oldRowClickItem.BunkRen && x.TeiDanNo == oldRowClickItem.TeiDanNo);
                if (reservationChangeExist == null)
                    lstReservationChange.Add(new ReservationChange
                    {
                        UkeNo = oldRowClickItem.UkeNo,
                        BunkRen = oldRowClickItem.BunkRen,
                        TeiDanNo = oldRowClickItem.TeiDanNo,
                        UnkRen = oldRowClickItem.UnkRen,
                        SyaRyoSyo = int.Parse(formModel.SyaRyoSyo, NumberStyles.AllowThousands, new CultureInfo("en-au")),
                        SyaRyoTes = int.Parse(formModel.SyaRyoTes, NumberStyles.AllowThousands, new CultureInfo("en-au")),
                        SyaRyoUnc = int.Parse(formModel.SyaRyoUnc, NumberStyles.AllowThousands, new CultureInfo("en-au")),
                        YouTes = int.Parse(formModel.YouTes, NumberStyles.AllowThousands, new CultureInfo("en-au")),
                        YouUnc = int.Parse(formModel.YouUnc, NumberStyles.AllowThousands, new CultureInfo("en-au")),
                        YouZei = int.Parse(formModel.YouZei, NumberStyles.AllowThousands, new CultureInfo("en-au")),
                        UpdYmd = oldRowClickItem.UpdYmd,
                        UpdTime = oldRowClickItem.UpdTime
                    });
                else
                {
                    reservationChangeExist.UkeNo = oldRowClickItem.UkeNo;
                    reservationChangeExist.BunkRen = oldRowClickItem.BunkRen;
                    reservationChangeExist.TeiDanNo = oldRowClickItem.TeiDanNo;
                    reservationChangeExist.UnkRen = oldRowClickItem.UnkRen;
                    reservationChangeExist.SyaRyoSyo = int.Parse(formModel.SyaRyoSyo, NumberStyles.AllowThousands, new CultureInfo("en-au"));
                    reservationChangeExist.SyaRyoTes = int.Parse(formModel.SyaRyoTes, NumberStyles.AllowThousands, new CultureInfo("en-au"));
                    reservationChangeExist.SyaRyoUnc = int.Parse(formModel.SyaRyoUnc, NumberStyles.AllowThousands, new CultureInfo("en-au"));
                    reservationChangeExist.YouTes = int.Parse(formModel.YouTes, NumberStyles.AllowThousands, new CultureInfo("en-au"));
                    reservationChangeExist.YouUnc = int.Parse(formModel.YouUnc, NumberStyles.AllowThousands, new CultureInfo("en-au"));
                    reservationChangeExist.YouZei = int.Parse(formModel.YouZei, NumberStyles.AllowThousands, new CultureInfo("en-au"));
                    reservationChangeExist.UpdYmd = oldRowClickItem.UpdYmd;
                    reservationChangeExist.UpdTime = oldRowClickItem.UpdTime;
                }

                isDisableGroup1 = true;
                isDisableGroup2 = true;
                isEnableReflect = false;
                isEnableSave = true;
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected async Task BtnSave()
        {
            try
            {
                IsUkeNoValid = true;
                await ValidateAddMessage();

                if (DataSource.Sum(x => x.SyaRyoUnc) != DataSourceBeforeUpdate.Sum(x => x.SyaRyoUnc) && !ErrorMessage.Contains(Constants.ErrorMessage.FareFeeCorrection_BI_T002))
                    ErrorMessage.Add(Constants.ErrorMessage.FareFeeCorrection_BI_T002);
                else if (DataSource.Sum(x => x.SyaRyoUnc) == DataSourceBeforeUpdate.Sum(x => x.SyaRyoUnc) && ErrorMessage.Contains(Constants.ErrorMessage.FareFeeCorrection_BI_T002))
                    ErrorMessage.Remove(Constants.ErrorMessage.FareFeeCorrection_BI_T002);
                if (DataSource.Sum(x => x.SyaRyoSyo) != DataSourceBeforeUpdate.Sum(x => x.SyaRyoSyo) && !ErrorMessage.Contains(Constants.ErrorMessage.FareFeeCorrection_BI_T003))
                    ErrorMessage.Add(Constants.ErrorMessage.FareFeeCorrection_BI_T003);
                else if (DataSource.Sum(x => x.SyaRyoSyo) == DataSourceBeforeUpdate.Sum(x => x.SyaRyoSyo) && ErrorMessage.Contains(Constants.ErrorMessage.FareFeeCorrection_BI_T003))
                    ErrorMessage.Remove(Constants.ErrorMessage.FareFeeCorrection_BI_T003);
                if (DataSource.Sum(x => x.SyaRyoTes) != DataSourceBeforeUpdate.Sum(x => x.SyaRyoTes) && !ErrorMessage.Contains(Constants.ErrorMessage.FareFeeCorrection_BI_T004))
                    ErrorMessage.Add(Constants.ErrorMessage.FareFeeCorrection_BI_T004);
                else if (DataSource.Sum(x => x.SyaRyoTes) == DataSourceBeforeUpdate.Sum(x => x.SyaRyoTes) && ErrorMessage.Contains(Constants.ErrorMessage.FareFeeCorrection_BI_T004))
                    ErrorMessage.Remove(Constants.ErrorMessage.FareFeeCorrection_BI_T004);

                if (oldRowClickItem?.YouTblSeq != 0 && vehicles.Any() && oldRowClickItem?.YouTblSeq == vehicles.FirstOrDefault().YouTblSeq && oldRowClickItem?.UnkRen == vehicles.FirstOrDefault().UnkRen && oldRowClickItem.YouJitaFlg == 0)
                {
                    if (DataSource.Sum(x => x.YouUnc) != DataSourceBeforeUpdate.Sum(x => x.YouUnc) && !ErrorMessage.Contains(Constants.ErrorMessage.FareFeeCorrection_BI_T005))
                        ErrorMessage.Add(Constants.ErrorMessage.FareFeeCorrection_BI_T005);
                    else if (DataSource.Sum(x => x.YouUnc) == DataSourceBeforeUpdate.Sum(x => x.YouUnc) && ErrorMessage.Contains(Constants.ErrorMessage.FareFeeCorrection_BI_T005))
                        ErrorMessage.Remove(Constants.ErrorMessage.FareFeeCorrection_BI_T005);
                    if (DataSource.Sum(x => x.YouZei) != DataSourceBeforeUpdate.Sum(x => x.YouZei) && !ErrorMessage.Contains(Constants.ErrorMessage.FareFeeCorrection_BI_T006))
                        ErrorMessage.Add(Constants.ErrorMessage.FareFeeCorrection_BI_T006);
                    else if (DataSource.Sum(x => x.YouZei) == DataSourceBeforeUpdate.Sum(x => x.YouZei) && ErrorMessage.Contains(Constants.ErrorMessage.FareFeeCorrection_BI_T006))
                        ErrorMessage.Remove(Constants.ErrorMessage.FareFeeCorrection_BI_T006);
                    if (DataSource.Sum(x => x.YouTes) != DataSourceBeforeUpdate.Sum(x => x.YouTes) && !ErrorMessage.Contains(Constants.ErrorMessage.FareFeeCorrection_BI_T007))
                        ErrorMessage.Add(Constants.ErrorMessage.FareFeeCorrection_BI_T007);
                    else if (DataSource.Sum(x => x.YouTes) == DataSourceBeforeUpdate.Sum(x => x.YouTes) && ErrorMessage.Contains(Constants.ErrorMessage.FareFeeCorrection_BI_T007))
                        ErrorMessage.Remove(Constants.ErrorMessage.FareFeeCorrection_BI_T007);
                }

                if (!ErrorMessage.Any())
                {
                    var model = await _service.GetHaitaCheck(UkeCdParam);
                    IsUkeNoValid = model.YykshoUpdYmdTime == _model.YykshoUpdYmdTime
                                    && model.HaishaMaxUpdYmdTime == _model.HaishaMaxUpdYmdTime
                                    && model.YoushaMaxUpdYmdTime == _model.YoushaMaxUpdYmdTime;
                    if (!IsUkeNoValid) return;
                    if (oldRowClickItem == null)
                        oldRowClickItem = DataSource.FirstOrDefault();
                    oldRowClickItem.ReservationChange = lstReservationChange;
                    await _service.SaveOrUpdateVehicleAllocation(oldRowClickItem);
                    _model = await _service.GetHaitaCheck(UkeCdParam);
                    lstReservationChange = new List<ReservationChange>();
                    await OnCloseFareFeeCorrection.InvokeAsync(false);
                }
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected async Task InitData()
        {
            isEnableReflect = false;
            isEnableSave = true;
            isFirstRender = true;
            await ValidateAddMessage();
            int index = 1;
            DataSource = vehicleAllocations.Select(x => new ReservationGrid()
            {
                No = index++,
                UnkYmd = $"{DateTime.ParseExact(x.UHaiSYmd, Formats.yyyyMMdd, null).ToString(CommonConstants.FormatYMDWithSlash)} ～ {DateTime.ParseExact(x.UTouYmd, Formats.yyyyMMdd, null).ToString(CommonConstants.FormatYMDWithSlash)}",
                GoSya = $"{x.GoSya.Substring(x.GoSya.Trim().Length - 2)}　号車-{x.BunKSyuJyn.ToString("D3")}",
                Eigyo = ((x.YouTblSeq == 0 || (x.YouTblSeq != 0 && x.YouJitaFlg == 1)) && x.HaiSKbn == 2) ? x.HaiSSryCdSeqRyakuNm : (((x.YouTblSeq == 0 || (x.YouTblSeq != 0 && x.YouJitaFlg == 1)) && x.HaiSKbn != 2 && x.KSKbn == 2) ? x.KSSyaRSeqRyakuNm : ""),
                SyaRyo = ((x.YouTblSeq == 0 || (x.YouTblSeq != 0 && x.YouJitaFlg == 1)) && x.HaiSKbn == 2) ? x.HaiSSyaRCdSeqSyaRyoNm : (((x.YouTblSeq == 0 || (x.YouTblSeq != 0 && x.YouJitaFlg == 1)) && x.HaiSKbn != 2 && x.KSKbn == 2) ? x.KSSyaRCdSeqKariSyaRyoNm : ""),
                CompanyCd = ((x.YouTblSeq == 0 || (x.YouTblSeq != 0 && x.YouJitaFlg == 1)) && x.HaiSKbn == 2) ? x.HaiSSryCdSeqCompanyCd : (((x.YouTblSeq == 0 || (x.YouTblSeq != 0 && x.YouJitaFlg == 1)) && x.HaiSKbn != 2 && x.KSKbn == 2) ? x.KSSyaRSeqCompanyCd : 0),
                YouSha = (x.YouTblSeq != 0 && x.YouJitaFlg == 0) ? $"{x.YouCdSeqRyakuNm}　{x.YouSitCdSeqRyakuNm}" : "",
                UkeCompanyCdSeqCompanyCd = reservations?.UkeCompanyCdSeqCompanyCd ?? 0,
                YouJitaFlg = x.YouJitaFlg,
                SyaRyoUncTmp = x.SyaRyoUnc,
                SyaRyoSyoTmp = x.SyaRyoSyo,
                SyaRyoTesTmp = x.SyaRyoTes,
                YouUncTmp = x.YoushaUnc,
                YouZeiTmp = x.YoushaSyo,
                YouTesTmp = x.YoushaTes,
                HaishaYmd = DateTime.ParseExact(x.UHaiSYmd, Formats.yyyyMMdd, null).ToString(Formats.SlashyyyyMMdd),
                TouYmd = DateTime.ParseExact(x.UTouYmd, Formats.yyyyMMdd, null).ToString(Formats.SlashyyyyMMdd),
                YouTblSeq = x.YouTblSeq,
                UnkRen = x.UnkRen,
                UkeNo = x.UkeNo,
                TeiDanNo = x.TeiDanNo,
                BunkRen = x.BunkRen,
                UpdYmd = x.UpdYmd,
                UpdTime = x.UpdTime,
                IsExistVehicleData = true ? vehicles.Any() : false
            }).ToList();

            DataSourceBeforeUpdate = DataSource;
            IndexItem = 1;
            var firstRevesation = DataSource.FirstOrDefault();
            formModel = new FareFeeCorrectionModel
            {
                SyaRyoUnc = firstRevesation?.SyaRyoUnc.ToString("#,##0"),
                SyaRyoSyo = firstRevesation?.SyaRyoSyo.ToString("#,##0"),
                SyaRyoTes = firstRevesation?.SyaRyoTes.ToString("#,##0"),
                YouUnc = firstRevesation?.YouUnc.ToString("#,##0"),
                YouZei = firstRevesation?.YouZei.ToString("#,##0"),
                YouTes = firstRevesation?.YouTes.ToString("#,##0"),
                UntKin = reservations?.UntKin.ToString("#,##0"),
                ZeiKbn = reservations?.ZeiKbn == 1 ? "外税" : (reservations?.ZeiKbn == 2 ? "内税" : (reservations?.ZeiKbn == 3 ? "非課税" : "")),
                ZeiRitu = reservations?.Zeiritsu.ToString(),
                ZeiRuiGaku = reservations?.ZeiRui.ToString("#,##0"),
                TesuRyo = reservations?.TesuRitu.ToString(),
                TesuRyoGaku = reservations?.TesuRyoG.ToString("#,##0"),
            };
            var filterValues = _filterConditionService.GetFilterCondition(FormFilterName.FareFeeCorrection, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq).Result;
            if (filterValues.Any())
            {
                ActiveV = int.Parse(filterValues.FirstOrDefault(x => x.ItemNm == nameof(FaresUpperAndLowerLimitsFormSearch.ActiveV))?.JoInput ?? ((int)ViewMode.Medium).ToString());
                ActiveL = int.Parse(filterValues.FirstOrDefault(x => x.ItemNm == nameof(FaresUpperAndLowerLimitsFormSearch.ActiveL))?.JoInput ?? ((int)LayoutMode.Save).ToString());
            }
            else
            {
                ActiveV = (int)ViewMode.Medium;
                ActiveL = (int)LayoutMode.Save;
            }
            editFormContext = new EditContext(formModel);
        }

        protected async Task ValidateAddMessage()
        {
            comapnies = await _service.GetCompanyValidate();
            reservations = (await _service.GetReservationList(new ClaimModel().TenantID.ToString(), UkeCdParam)).FirstOrDefault();
            vehicleAllocations = await _service.GetVehicleAllocationList(new ClaimModel().TenantID.ToString(), UkeCdParam);
            vehicles = await _service.GetVehicle(new ClaimModel().TenantID.ToString(), UkeCdParam);
            if ((!comapnies.Any() || reservations == null) && !ErrorMessage.Contains(Constants.ErrorMessage.FareFeeCorrection_BI_T001))
                ErrorMessage.Add(Constants.ErrorMessage.FareFeeCorrection_BI_T001);
            else if ((comapnies.Any() && reservations != null) && ErrorMessage.Contains(Constants.ErrorMessage.FareFeeCorrection_BI_T001))
                ErrorMessage.Remove(Constants.ErrorMessage.FareFeeCorrection_BI_T001);
        }

        protected async Task OnRowClick(ReservationGrid item, MouseEventArgs e)
        {
            try
            {
                IndexItem = item.No;
                formModel.SyaRyoUnc = item.SyaRyoUnc.ToString("#,##0");
                formModel.SyaRyoSyo = item.SyaRyoSyo.ToString("#,##0");
                formModel.SyaRyoTes = item.SyaRyoTes.ToString("#,##0");
                formModel.YouUnc = item.YouUnc.ToString("#,##0");
                formModel.YouZei = item.YouZei.ToString("#,##0");
                formModel.YouTes = item.YouTes.ToString("#,##0");

                if (oldRowClickItem?.No != item?.No)
                {
                    isDisableGroup1 = true;
                    isDisableGroup2 = true;
                }
                isEnableReflect = false;
                isEnableSave = true;
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected async Task OnDblRowClick(ReservationGrid item, MouseEventArgs e)
        {
            isDisableGroup1 = false;
            if (item.YouTblSeq != 0)
                isDisableGroup2 = false;
            else
                isDisableGroup2 = true;
            isEnableReflect = true;
            isEnableSave = false;
            oldRowClickItem = item;
            await InvokeAsync(StateHasChanged);
        }
    }
}
