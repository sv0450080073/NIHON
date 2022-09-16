using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.RegulationSetting;
using HassyaAllrightCloud.IService;
using HassyaAllrightCloud.IService.RegulationSetting;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;


namespace HassyaAllrightCloud.Pages
{
    public class RegulationSettingFormBase : ComponentBase
    {
        [Inject] protected IStringLocalizer<RegulationSettingForm> _lang { get; set; }
        [Inject] private IErrorHandlerService _errService { get; set; }
        [Inject] private ITransportationSummaryService _transportationSummaryService { get; set; }
        [Inject] private ILoadingService _loading { get; set; }
        [Parameter] public bool ShowPopup { get; set; }
        [Parameter] public EventCallback<bool> ShowPopupChanged { get; set; }
        [Parameter] public bool IsCreate { get; set; }
        [Parameter] public EventCallback<bool> OnTogglePopup { get; set; }
        [Parameter] public EventCallback<bool> OnReset { get; set; }
        [Inject] private IRegulationSettingService _service { get; set; }
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }
        protected RegulationSettingFormModel model { get; set; }
        protected Dictionary<string, string> LangDic = new Dictionary<string, string>();
        protected IEnumerable<CompanyListItem> companyList = new List<CompanyListItem>();
        protected List<EiygoItem> eiygoItems = new List<EiygoItem>();
        protected EditContext formContext { get; set; }
        public List<WorkHolidayType> WorkHolidayTypes { get; set; }
        public RegulationSettingItem SelectedItem { get; set; }

        public bool ReadOnly { get; set; } = true;
        public bool CompanyReadOnly { get; set; } = false;
        public WorkHolidayType DragingItem { get; set; }
        public WorkHolidayType DropItem { get; set; }
        public List<WorkHolidayType> SourceList { get; set; }
        public List<WorkHolidayType> ListData { get; set; }
        public List<WorkHolidayType> UnAssignList { get; set; } = new List<WorkHolidayType>();
        public List<WorkHolidayType> UnSpecificList { get; set; } = new List<WorkHolidayType>();

        public List<WorkHolidayType> WorkHolidayRow1 { get; set; } = new List<WorkHolidayType>();
        public List<WorkHolidayType> WorkHolidayRow2 { get; set; } = new List<WorkHolidayType>();
        public List<WorkHolidayType> WorkHolidayRow3 { get; set; } = new List<WorkHolidayType>();
        public List<WorkHolidayType> WorkHolidayRow4 { get; set; } = new List<WorkHolidayType>();
        public List<WorkHolidayType> WorkHolidayRow5 { get; set; } = new List<WorkHolidayType>();
        public List<WorkHolidayType> WorkHolidayRow6 { get; set; } = new List<WorkHolidayType>();
        public List<WorkHolidayType> WorkHolidayRow7 { get; set; } = new List<WorkHolidayType>();
        public List<WorkHolidayType> WorkHolidayRow8 { get; set; } = new List<WorkHolidayType>();
        public List<WorkHolidayType> WorkHolidayRow9 { get; set; } = new List<WorkHolidayType>();
        public List<WorkHolidayType> WorkHolidayRow10 { get; set; } = new List<WorkHolidayType>();
        public int FromSourceId { get; set; }
        public int ToSourceId { get; set; }
        protected override async Task OnParametersSetAsync()
        {
            ReadOnly = IsCreate;
            if(SelectedItem == null)
            {
                CompanyReadOnly = false;
                ResetListWorkType();
                model = new RegulationSettingFormModel();
                formContext = new EditContext(model);
            }
        }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                
                ListData = _service.GetWorkHolidayTypes(model?.Company != null ? model.Company.CompanyCdSeq : 0).Result;
                eiygoItems = _service.GetFormCharacters().Result;
                var dataLang = _lang.GetAllStrings();
                LangDic = dataLang.ToDictionary(l => l.Name, l => l.Value);
                companyList = await _transportationSummaryService.GetCompanyListItems(0);
                model = new RegulationSettingFormModel();
                formContext = new EditContext(model);
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
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JSRuntime.InvokeVoidAsync("setNumberRate", ".number-rate");
            await JSRuntime.InvokeAsync<string>("addMaxLength", "length", 10);
            await JSRuntime.InvokeAsync<string>("addMaxLength", "length", 2);
            await JSRuntime.InvokeAsync<string>("addMaxLength", "length", 60);
            await JSRuntime.InvokeAsync<string>("addMaxLength", "length", 5);
            await JSRuntime.InvokeAsync<string>("addMaxLength", "length", 30);
        }

        public void ClosePopup()
        {
            ShowPopup = false;
            StateHasChanged();
        }

        public void LoadData(RegulationSettingItem selectedItem)
        {
            CompanyReadOnly = true;
            SelectedItem = selectedItem;
            model = new RegulationSettingFormModel()
            {
                Company = companyList.Where(x => x.CompanyCd == Int32.Parse(SelectedItem.GridCompanyCode)).FirstOrDefault(),
                EditFormMonthlyProcess = SelectedItem.GetSyoKbn == 1 ? EditFormMonthlyProcess.TransferMonthly : EditFormMonthlyProcess.DonotTransferMonthly,
                EditFormBillForward = SelectedItem.SeiKrksKbn == 1 ? EditFormBillForward.CarryForward : EditFormBillForward.DonotCarryForward,
                EditFormDailyProcess = SelectedItem.DaySyoKbn == 1 ? EditFormDailyProcess.ProcessDaily : EditFormDailyProcess.DonotProcessDaily,
                EditFormLastUpdateStaffCode = SelectedItem.UpdSyainCd.ToString(),
                EditFormLastUpdateStaffName = "",
                EditFormLastUpdateDate = SelectedItem.UpdYmd.Insert(4, "/").Insert(7, "/"),
                EditFormLastUpdateTime = SelectedItem.UpdTime.Insert(2, ":").Insert(5, ":"),
                EditFormSaleClassification = SelectedItem.UriKbn == 1 ? EditFormSaleClassification.Start : EditFormSaleClassification.End,
                EditFormSaleChange = SelectedItem.UriHenKbn == 1 ? false : true,
                EditFormSalesChangeablePeriod = SelectedItem.UriHenKikan.ToString(),
                EditFormSalesChangeDateClassification = SelectedItem.UriMDKbn == 1 ? EditFormSalesChangeDateClassification.Day : EditFormSalesChangeDateClassification.Month,
                EditFormCheckZeroYen = SelectedItem.UriZeroChk == 1 ? EditFormCheckZeroYen.DonotCheck : SelectedItem.UriZeroChk == 2 ? EditFormCheckZeroYen.CannotChange : EditFormCheckZeroYen.MessageOnly,
                EditFormCancelClassification = SelectedItem.CanKbn == 1 ? false : true,
                EditFormCancellationPeriod = SelectedItem.CanKikan.ToString(),
                EditFormCancelDateClassification = SelectedItem.CanMDKbn == 1 ? EditFormSalesChangeDateClassification2.Day : EditFormSalesChangeDateClassification2.Month,
                EditFormFareByVehicle = SelectedItem.SyaUntKbn == 1 ? EditFormFareByVehicle.CaculateByTotal : EditFormFareByVehicle.CaculateByBusType,
                EditFormTaxFraction = SelectedItem.SyohiHasu == 1 ? EditFormTaxFraction.Cut : SelectedItem.SyohiHasu == 2 ? EditFormTaxFraction.Truncate : EditFormTaxFraction.Round,
                EditFormFeeFraction = SelectedItem.TesuHasu == 1 ? EditFormTaxFraction.Cut : SelectedItem.TesuHasu == 2 ? EditFormTaxFraction.Truncate : EditFormTaxFraction.Round,
                EditFormHiredBusFee = SelectedItem.YouTesuKbn == 1 ? EditFormHiredBusFee.Reservation : EditFormHiredBusFee.CarDestination,
                EditFormHiredBusDifferentClassification = SelectedItem.YouSagaKbn == 1 ? EditFormHiredBusDifferentClassification.AddToYourCompany : EditFormHiredBusDifferentClassification.DonotAdd,
                EditFormFareTaxDisplay = SelectedItem.UntZeiKbn == 1 ? EditFormFareTaxDisplay.FreeTax : SelectedItem.UntZeiKbn == 2 ? EditFormFareTaxDisplay.IncludedTax : EditFormFareTaxDisplay.OtherTax,
                EditFormLoadingGoodsTaxDisplay = SelectedItem.TumZeiKbn == 1 ? EditFormFareTaxDisplay.FreeTax : SelectedItem.TumZeiKbn == 2 ? EditFormFareTaxDisplay.IncludedTax : EditFormFareTaxDisplay.OtherTax,
                EditFormCancelRate1StartTime = SelectedItem.CanSKan1.ToString(),
                EditFormCancelRate1EndTime = SelectedItem.CanEKan1.ToString(),
                EditFormCancelRate1 = SelectedItem.CanRit1.ToString(),
                EditFormCancelRate2StartTime = SelectedItem.CanSKan2.ToString(),
                EditFormCancelRate2EndTime = SelectedItem.CanEKan2.ToString(),
                EditFormCancelRate2 = SelectedItem.CanRit2.ToString(),
                EditFormCancelRate3StartTime = SelectedItem.CanSKan3.ToString(),
                EditFormCancelRate3EndTime = SelectedItem.CanEKan3.ToString(),
                EditFormCancelRate3 = SelectedItem.CanRit3.ToString(),
                EditFormCancelRate4StartTime = SelectedItem.CanSKan4.ToString(),
                EditFormCancelRate4EndTime = SelectedItem.CanEKan4.ToString(),
                EditFormCancelRate4 = SelectedItem.CanRit4.ToString(),
                EditFormCancelRate5StartTime = SelectedItem.CanSKan5.ToString(),
                EditFormCancelRate5EndTime = SelectedItem.CanEKan5.ToString(),
                EditFormCancelRate5 = SelectedItem.CanRit5.ToString(),
                EditFormCancelRate6StartTime = SelectedItem.CanSKan6.ToString(),
                EditFormCancelRate6EndTime = SelectedItem.CanEKan6.ToString(),
                EditFormCancelRate6 = SelectedItem.CanRit6.ToString(),
                EditFormAutoTemporaryBus = SelectedItem.JKariKbn == 1 ? EditFormAutoTemporaryBus.Do : EditFormAutoTemporaryBus.Donot,
                EditFormPriority = SelectedItem.AutKarJyun == 1 ? EditFormPriority.SaleOfficeOrder : EditFormPriority.VehicleTypeOrder,
                EditFormAutoTemporaryBusDivision = SelectedItem.JKBunPat == 1 ? EditFormAutoTemporaryBusDivision.Standard : EditFormAutoTemporaryBusDivision.Daily,
                EditFormVehicleReplacement = SelectedItem.SyaIrePat == 1 ? EditFormVehicleReplacement.VehicleOnly : SelectedItem.SyaIrePat == 2 ? EditFormVehicleReplacement.VehicleDriver : SelectedItem.SyaIrePat == 3 ? EditFormVehicleReplacement.VehicleGuider : EditFormVehicleReplacement.All,
                EditFormCrewCompatibilityCheck = SelectedItem.JymAChkKbn == 1 ? EditFormCrewCompatibilityCheck.DonotCheck : EditFormCrewCompatibilityCheck.Check,
                EditFormAutoKoban = SelectedItem.AutKouKbn == 1 ? EditFormAutoKoban.No : SelectedItem.AutKouKbn == 2 ? EditFormAutoKoban.VehiclePersonAssignment : EditFormAutoKoban.AutomaticPoliceBox,
                EditFormCharacter1DisplayByBusType = eiygoItems.Where(x => x.CodeKbn == SelectedItem.SyaSenMjPtnKbn1.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColor1DisplayByBusType = SelectedItem.SyaSenMjPtnCol1.Insert(0, "#"),
                EditFormCharacter2DisplayByBusType = eiygoItems.Where(x => x.CodeKbn == SelectedItem.SyaSenMjPtnKbn2.ToString().PadLeft(2,'0')).FirstOrDefault(),
                EditFormColor2DisplayByBusType = SelectedItem.SyaSenMjPtnCol2.Insert(0, "#"),
                EditFormCharacter3DisplayByBusType = eiygoItems.Where(x => x.CodeKbn == SelectedItem.SyaSenMjPtnKbn3.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColor3DisplayByBusType = SelectedItem.SyaSenMjPtnCol3.Insert(0, "#"),
                EditFormCharacter4DisplayByBusType = eiygoItems.Where(x => x.CodeKbn == SelectedItem.SyaSenMjPtnKbn4.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColor4DisplayByBusType = SelectedItem.SyaSenMjPtnCol4.Insert(0, "#"),
                EditFormCharacter5DisplayByBusType = eiygoItems.Where(x => x.CodeKbn == SelectedItem.SyaSenMjPtnKbn5.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColor5DisplayByBusType = SelectedItem.SyaSenMjPtnCol5.Insert(0, "#"),
                EditFormCharacter1DisplayByCrew = eiygoItems.Where(x => x.CodeKbn == SelectedItem.JyoSenMjPtnKbn1.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColor1DisplayByCrew = SelectedItem.JyoSenMjPtnCol1.Insert(0, "#"),
                EditFormCharacter2DisplayByCrew = eiygoItems.Where(x => x.CodeKbn == SelectedItem.JyoSenMjPtnKbn2.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColor2DisplayByCrew = SelectedItem.JyoSenMjPtnCol2.Insert(0, "#"),
                EditFormCharacter3DisplayByCrew = eiygoItems.Where(x => x.CodeKbn == SelectedItem.JyoSenMjPtnKbn3.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColor3DisplayByCrew = SelectedItem.JyoSenMjPtnCol3.Insert(0, "#"),
                EditFormCharacter4DisplayByCrew = eiygoItems.Where(x => x.CodeKbn == SelectedItem.JyoSenMjPtnKbn4.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColor4DisplayByCrew = SelectedItem.JyoSenMjPtnCol4.Insert(0, "#"),
                EditFormCharacter5DisplayByCrew = eiygoItems.Where(x => x.CodeKbn == SelectedItem.JyoSenMjPtnKbn5.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColor5DisplayByCrew = SelectedItem.JyoSenMjPtnCol5.Insert(0, "#"),
                EditFormCharacterClassification1DisplayByBusType = eiygoItems.Where(x =>x.CodeKbn == SelectedItem.SyaSenInfoPtnKbn1.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColorClassification1DisplayByBusType = SelectedItem.SyaSenInfoPtnCol1.Insert(0 , "#"),
                EditFormCharacterClassification2DisplayByBusType = eiygoItems.Where(x => x.CodeKbn == SelectedItem.SyaSenInfoPtnKbn2.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColorClassification2DisplayByBusType = SelectedItem.SyaSenInfoPtnCol2.Insert(0, "#"),
                EditFormCharacterClassification3DisplayByBusType = eiygoItems.Where(x => x.CodeKbn == SelectedItem.SyaSenInfoPtnKbn3.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColorClassification3DisplayByBusType = SelectedItem.SyaSenInfoPtnCol3.Insert(0, "#"),
                EditFormCharacterClassification4DisplayByBusType = eiygoItems.Where(x => x.CodeKbn == SelectedItem.SyaSenInfoPtnKbn4.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColorClassification4DisplayByBusType = SelectedItem.SyaSenInfoPtnCol4.Insert(0, "#"),
                EditFormCharacterClassification5DisplayByBusType = eiygoItems.Where(x => x.CodeKbn == SelectedItem.SyaSenInfoPtnKbn5.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColorClassification5DisplayByBusType = SelectedItem.SyaSenInfoPtnCol5.Insert(0, "#"),
                EditFormCharacterClassification6DisplayByBusType = eiygoItems.Where(x => x.CodeKbn == SelectedItem.SyaSenInfoPtnKbn6.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColorClassification6DisplayByBusType = SelectedItem.SyaSenInfoPtnCol6.Insert(0, "#"),
                EditFormCharacterClassification7DisplayByBusType = eiygoItems.Where(x => x.CodeKbn == SelectedItem.SyaSenInfoPtnKbn7.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColorClassification7DisplayByBusType = SelectedItem.SyaSenInfoPtnCol7.Insert(0, "#"),
                EditFormCharacterClassification8DisplayByBusType = eiygoItems.Where(x => x.CodeKbn == SelectedItem.SyaSenInfoPtnKbn8.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColorClassification8DisplayByBusType = SelectedItem.SyaSenInfoPtnCol8.Insert(0, "#"),
                EditFormCharacterClassification9DisplayByBusType = eiygoItems.Where(x => x.CodeKbn == SelectedItem.SyaSenInfoPtnKbn9.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColorClassification9DisplayByBusType = SelectedItem.SyaSenInfoPtnCol9.Insert(0, "#"),
                EditFormCharacterClassification10DisplayByBusType = eiygoItems.Where(x => x.CodeKbn == SelectedItem.SyaSenInfoPtnKbn10.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColorClassification10DisplayByBusType = SelectedItem.SyaSenInfoPtnCol10.Insert(0, "#"),
                EditFormCharacterClassification11DisplayByBusType = eiygoItems.Where(x => x.CodeKbn == SelectedItem.SyaSenInfoPtnKbn11.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColorClassification11DisplayByBusType = SelectedItem.SyaSenInfoPtnCol11.Insert(0, "#"),
                EditFormCharacterClassification12DisplayByBusType = eiygoItems.Where(x => x.CodeKbn == SelectedItem.SyaSenInfoPtnKbn12.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColorClassification12DisplayByBusType = SelectedItem.SyaSenInfoPtnCol12.Insert(0, "#"),
                EditFormCharacterClassification13DisplayByBusType = eiygoItems.Where(x => x.CodeKbn == SelectedItem.SyaSenInfoPtnKbn13.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColorClassification13DisplayByBusType = SelectedItem.SyaSenInfoPtnCol13.Insert(0, "#"),
                EditFormCharacterClassification14DisplayByBusType = eiygoItems.Where(x => x.CodeKbn == SelectedItem.SyaSenInfoPtnKbn14.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColorClassification14DisplayByBusType = SelectedItem.SyaSenInfoPtnCol14.Insert(0, "#"),
                EditFormCharacterClassification15DisplayByBusType = eiygoItems.Where(x => x.CodeKbn == SelectedItem.SyaSenInfoPtnKbn15.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColorClassification15DisplayByBusType = SelectedItem.SyaSenInfoPtnCol15.Insert(0, "#"),

                EditFormCharacterClassification1DisplayByCrewType = eiygoItems.Where(x => x.CodeKbn == SelectedItem.JyoSenInfoPtnKbn1.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColorClassification1DisplayByCrewType = SelectedItem.JyoSenInfoPtnCol1.Insert(0, "#"),
                EditFormCharacterClassification2DisplayByCrewType = eiygoItems.Where(x => x.CodeKbn == SelectedItem.JyoSenInfoPtnKbn2.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColorClassification2DisplayByCrewType = SelectedItem.JyoSenInfoPtnCol2.Insert(0, "#"),
                EditFormCharacterClassification3DisplayByCrewType = eiygoItems.Where(x => x.CodeKbn == SelectedItem.JyoSenInfoPtnKbn3.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColorClassification3DisplayByCrewType = SelectedItem.JyoSenInfoPtnCol3.Insert(0, "#"),
                EditFormCharacterClassification4DisplayByCrewType = eiygoItems.Where(x => x.CodeKbn == SelectedItem.JyoSenInfoPtnKbn4.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColorClassification4DisplayByCrewType = SelectedItem.JyoSenInfoPtnCol4.Insert(0, "#"),
                EditFormCharacterClassification5DisplayByCrewType = eiygoItems.Where(x => x.CodeKbn == SelectedItem.JyoSenInfoPtnKbn5.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColorClassification5DisplayByCrewType = SelectedItem.JyoSenInfoPtnCol5.Insert(0, "#"),
                EditFormCharacterClassification6DisplayByCrewType = eiygoItems.Where(x => x.CodeKbn == SelectedItem.JyoSenInfoPtnKbn6.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColorClassification6DisplayByCrewType = SelectedItem.JyoSenInfoPtnCol6.Insert(0, "#"),
                EditFormCharacterClassification7DisplayByCrewType = eiygoItems.Where(x => x.CodeKbn == SelectedItem.JyoSenInfoPtnKbn7.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColorClassification7DisplayByCrewType = SelectedItem.JyoSenInfoPtnCol7.Insert(0, "#"),
                EditFormCharacterClassification8DisplayByCrewType = eiygoItems.Where(x => x.CodeKbn == SelectedItem.JyoSenInfoPtnKbn8.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColorClassification8DisplayByCrewType = SelectedItem.JyoSenInfoPtnCol8.Insert(0, "#"),
                EditFormCharacterClassification9DisplayByCrewType = eiygoItems.Where(x => x.CodeKbn == SelectedItem.JyoSenInfoPtnKbn9.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColorClassification9DisplayByCrewType = SelectedItem.JyoSenInfoPtnCol9.Insert(0, "#"),
                EditFormCharacterClassification10DisplayByCrewType = eiygoItems.Where(x => x.CodeKbn == SelectedItem.JyoSenInfoPtnKbn10.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColorClassification10DisplayByCrewType = SelectedItem.JyoSenInfoPtnCol10.Insert(0, "#"),
                EditFormCharacterClassification11DisplayByCrewType = eiygoItems.Where(x => x.CodeKbn == SelectedItem.JyoSenInfoPtnKbn11.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColorClassification11DisplayByCrewType = SelectedItem.JyoSenInfoPtnCol11.Insert(0, "#"),
                EditFormCharacterClassification12DisplayByCrewType = eiygoItems.Where(x => x.CodeKbn == SelectedItem.JyoSenInfoPtnKbn12.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColorClassification12DisplayByCrewType = SelectedItem.JyoSenInfoPtnCol12.Insert(0, "#"),
                EditFormCharacterClassification13DisplayByCrewType = eiygoItems.Where(x => x.CodeKbn == SelectedItem.JyoSenInfoPtnKbn13.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColorClassification13DisplayByCrewType = SelectedItem.JyoSenInfoPtnCol13.Insert(0, "#"),
                EditFormCharacterClassification14DisplayByCrewType = eiygoItems.Where(x => x.CodeKbn == SelectedItem.JyoSenInfoPtnKbn14.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColorClassification14DisplayByCrewType = SelectedItem.JyoSenInfoPtnCol14.Insert(0, "#"),
                EditFormCharacterClassification15DisplayByCrewType = eiygoItems.Where(x => x.CodeKbn == SelectedItem.JyoSenInfoPtnKbn15.ToString().PadLeft(2, '0')).FirstOrDefault(),
                EditFormColorClassification15DisplayByCrewType = SelectedItem.JyoSenInfoPtnCol15.Insert(0, "#"),
                EditFormBillComent1 = SelectedItem.SeiCom1,
                EditFormBillComent2 = SelectedItem.SeiCom2,
                EditFormBillComent3 = SelectedItem.SeiCom3,
                EditFormBillComent4 = SelectedItem.SeiCom4,
                EditFormBillComent5 = SelectedItem.SeiCom5,
                EditFormBillComent6 = SelectedItem.SeiCom6,
                EditFormReportClassification = SelectedItem.HoukoKbn == 1 ? EditFormReportClassification.Departure : EditFormReportClassification.Return,
                EditFormReportSummary = SelectedItem.HouZeiKbn == 1 ? EditFormReportSummary.Delivery : EditFormReportSummary.TaxExcluded,
                EditFormReportOutput = SelectedItem.HouOutKbn == 1 ? EditFormReportOutput.Tohoku : SelectedItem.HouOutKbn == 2 ? EditFormReportOutput.Kanto : SelectedItem.HouOutKbn == 3 ? EditFormReportOutput.Koshinetsu : EditFormReportOutput.Other,
                EditFormTransportationMiscellaneousIncome = SelectedItem.ZasyuKbn == 1 ? EditFormTransportationMiscellaneousIncome.Yes : EditFormTransportationMiscellaneousIncome.No,
                EditFormDisplayDetailSelection = SelectedItem.MeiShyKbn == 1 ? EditFormDisplayDetailSelection.Invert : EditFormDisplayDetailSelection.Frame,
                EditFormCurrentInvoice = SelectedItem.SeiGenFlg == 1 ? EditFormCurrentInvoice.Manage : EditFormCurrentInvoice.NoManagement,
                EditFormIncidentalType1Addition = SelectedItem.FutSF1Flg == 1 ? YesNoRadio.Yes : YesNoRadio.None,
                EditFormIncidentalType2Addition = SelectedItem.FutSF2Flg == 1 ? YesNoRadio.Yes : YesNoRadio.None,
                EditFormIncidentalType3Addition = SelectedItem.FutSF3Flg == 1 ? YesNoRadio.Yes : YesNoRadio.None,
                EditFormIncidentalType4Addition = SelectedItem.FutSF4Flg == 1 ? YesNoRadio.Yes : YesNoRadio.None,
                EditFormInitCopyProcessData = SelectedItem.KoteiCopyFlg == 1 ? YesNoRadio.Yes : YesNoRadio.None,
                EditFormInitCopyIncidentalData = SelectedItem.FutaiCopyFlg == 1 ? YesNoRadio.Yes : YesNoRadio.None,
                EditFormInitCopyLoadingGoodData = SelectedItem.TumiCopyFlg == 1 ? YesNoRadio.Yes : YesNoRadio.None,
                EditFormInitCopyArrangeData = SelectedItem.TehaiCopyFlg == 1 ? YesNoRadio.Yes : YesNoRadio.None,
                EditFormInitCopyBoardingPlaceData = SelectedItem.JoshaCopyFlg == 1 ? YesNoRadio.Yes : YesNoRadio.None,
                EditFormInitCopyReservationRemarkData = SelectedItem.YykCopyFlg == 1 ? YesNoRadio.Yes : YesNoRadio.None,
                EditFormInitCopyOperationDateRemarkData = SelectedItem.UkbCopyFlg == 1 ? YesNoRadio.Yes : YesNoRadio.None,
                EditFormInitTransferEstimateData = SelectedItem.QuotationTransfer == 1 ? YesNoRadio.Yes : YesNoRadio.None,
                JisKinKyuNm01 = SelectedItem.JisKinKyuNm01,
                JisKinKyuNm02 = SelectedItem.JisKinKyuNm02,
                JisKinKyuNm03 = SelectedItem.JisKinKyuNm03,
                JisKinKyuNm04 = SelectedItem.JisKinKyuNm04,
                JisKinKyuNm05 = SelectedItem.JisKinKyuNm05,
                JisKinKyuNm06 = SelectedItem.JisKinKyuNm06,
                JisKinKyuNm07 = SelectedItem.JisKinKyuNm07,
                JisKinKyuNm08 = SelectedItem.JisKinKyuNm08,
                JisKinKyuNm09 = SelectedItem.JisKinKyuNm09,
                JisKinKyuNm10 = SelectedItem.JisKinKyuNm10
            };
            formContext = new EditContext(model);
            UnAssignList = _service.GetWorkHolidayTypes(model?.Company != null ? model.Company.CompanyCdSeq : 0).Result;
            LoadSpecificData();
            StateHasChanged();
        }

        public void HandleDragStart(WorkHolidayType selectItem, List<WorkHolidayType> sourceList, int fromSourceId)
        {
            FromSourceId = fromSourceId;
            DragingItem = selectItem;
            SourceList = sourceList;
        }
        public async Task HandleDrop(int targerListId)
        {
            ToSourceId = targerListId;
            DropItem = ListData.FirstOrDefault(x => x.KinKyuCdSeq == DragingItem.KinKyuCdSeq);

            switch (targerListId)
            {
                case 1:
                    if (FromSourceId != ToSourceId)
                    {
                        WorkHolidayRow1.Add(DropItem);
                    }
                    break;
                case 2:
                    if (FromSourceId != ToSourceId)
                    {
                        WorkHolidayRow2.Add(DropItem);
                    }
                    break;
                case 3:
                    if (FromSourceId != ToSourceId)
                    {
                        WorkHolidayRow3.Add(DropItem);
                    }
                    break;
                case 4:
                    if (FromSourceId != ToSourceId)
                    {
                        WorkHolidayRow4.Add(DropItem);
                    }
                    break;
                case 5:
                    if (FromSourceId != ToSourceId)
                    {
                        WorkHolidayRow5.Add(DropItem);
                    }
                    break;
                case 6:
                    if (FromSourceId != ToSourceId)
                    {
                        WorkHolidayRow6.Add(DropItem);
                    }
                    break;
                case 7:
                    if (FromSourceId != ToSourceId)
                    {
                        WorkHolidayRow7.Add(DropItem);
                    }
                    break;
                case 8:
                    if (FromSourceId != ToSourceId)
                    {
                        WorkHolidayRow8.Add(DropItem);
                    }
                    break;
                case 9:
                    if (FromSourceId != ToSourceId)
                    {
                        WorkHolidayRow9.Add(DropItem);
                    }
                    break;
                case 10:
                    if(FromSourceId != ToSourceId)
                    {
                        WorkHolidayRow10.Add(DropItem);
                    }
                    break;
                case 11:
                    if(FromSourceId != ToSourceId)
                    {
                        UnAssignList.Add(DropItem);
                        UnAssignList = UnAssignList.OrderBy(x => x.KinKyuCd).ToList();
                    }
                    break;
            };
            RemoveFromSource();
            StateHasChanged();
        }

        private void RemoveFromSource()
        {
            if(FromSourceId != ToSourceId)
            {
                SourceList.Remove(DragingItem);
            }
        }

        public void SaveSetting()
        {
            var saved = _service.SaveKasSet(model).Result;
        }

        public void SaveJiskin()
        {
            var saveRow1 = _service.SaveJiskin(1, WorkHolidayRow1, model.Company.CompanyCdSeq).Result;
            var saveRow2 = _service.SaveJiskin(2, WorkHolidayRow2, model.Company.CompanyCdSeq).Result;
            var saveRow3 = _service.SaveJiskin(3, WorkHolidayRow3, model.Company.CompanyCdSeq).Result;
            var saveRow4 = _service.SaveJiskin(4, WorkHolidayRow4, model.Company.CompanyCdSeq).Result;
            var saveRow5 = _service.SaveJiskin(5, WorkHolidayRow5, model.Company.CompanyCdSeq).Result;
            var saveRow6 = _service.SaveJiskin(6, WorkHolidayRow6, model.Company.CompanyCdSeq).Result;
            var saveRow7 = _service.SaveJiskin(7, WorkHolidayRow7, model.Company.CompanyCdSeq).Result;
            var saveRow8 = _service.SaveJiskin(8, WorkHolidayRow8, model.Company.CompanyCdSeq).Result;
            var saveRow9 = _service.SaveJiskin(9, WorkHolidayRow9, model.Company.CompanyCdSeq).Result;
            var saveRow10 = _service.SaveJiskin(10, WorkHolidayRow10, model.Company.CompanyCdSeq).Result;
            LoadSpecificData();
            StateHasChanged();
        }
        public void LoadSpecificData()
        {
            UnSpecificList = _service.GetSpecificWorkHolidayTypes(model?.Company != null ? model.Company.CompanyCdSeq : 0).Result;
            
            WorkHolidayRow1 = UnSpecificList.Where(x => x.JisKinKyuCd == 1).ToList();
            WorkHolidayRow2 = UnSpecificList.Where(x => x.JisKinKyuCd == 2).ToList();
            WorkHolidayRow3 = UnSpecificList.Where(x => x.JisKinKyuCd == 3).ToList();
            WorkHolidayRow4 = UnSpecificList.Where(x => x.JisKinKyuCd == 4).ToList();
            WorkHolidayRow5 = UnSpecificList.Where(x => x.JisKinKyuCd == 5).ToList();
            WorkHolidayRow6 = UnSpecificList.Where(x => x.JisKinKyuCd == 6).ToList();
            WorkHolidayRow7 = UnSpecificList.Where(x => x.JisKinKyuCd == 7).ToList();
            WorkHolidayRow8 = UnSpecificList.Where(x => x.JisKinKyuCd == 8).ToList();
            WorkHolidayRow9 = UnSpecificList.Where(x => x.JisKinKyuCd == 9).ToList();
            WorkHolidayRow10 = UnSpecificList.Where(x => x.JisKinKyuCd == 10).ToList();
            StateHasChanged();
        }

        protected async Task CloseDialog()
        {
            try
            {
                SelectedItem = null;
                model = new RegulationSettingFormModel();
                await ShowPopupChanged.InvokeAsync(false);
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected async Task FormChanged(string propertyName, dynamic value)
        {
            try
            {
                if(propertyName == nameof(model.JisKinKyuNm10) || propertyName == nameof(model.JisKinKyuNm09) || propertyName == nameof(model.JisKinKyuNm08) || propertyName == nameof(model.JisKinKyuNm07) || propertyName == nameof(model.JisKinKyuNm06) || propertyName == nameof(model.JisKinKyuNm05) || propertyName == nameof(model.JisKinKyuNm04) || propertyName == nameof(model.JisKinKyuNm03) || propertyName == nameof(model.JisKinKyuNm02) || propertyName == nameof(model.JisKinKyuNm01))
                {
                    if(value.ToString().Length > 30)
                    {
                        value = value.ToString().Substring(0, 30);
                    }
                }
                if (propertyName == nameof(model.EditFormBillComent6) || propertyName == nameof(model.EditFormBillComent5) || propertyName == nameof(model.EditFormBillComent4) || propertyName == nameof(model.EditFormBillComent3) || propertyName == nameof(model.EditFormBillComent2) || propertyName == nameof(model.EditFormBillComent1))
                {
                    if (value.ToString().Length > 30)
                    {
                        value = value.ToString().Substring(0, 60);
                    }
                }
                if (propertyName == nameof(model.EditFormCancellationPeriod) || propertyName == nameof(model.EditFormSalesChangeablePeriod) || propertyName == nameof(model.EditFormCancelRate1StartTime) || propertyName == nameof(model.EditFormCancelRate1EndTime) || propertyName == nameof(model.EditFormCancelRate2StartTime) || propertyName == nameof(model.EditFormCancelRate2EndTime) || propertyName == nameof(model.EditFormCancelRate3StartTime) || propertyName == nameof(model.EditFormCancelRate3EndTime) || propertyName == nameof(model.EditFormCancelRate4StartTime) || propertyName == nameof(model.EditFormCancelRate4EndTime) || propertyName == nameof(model.EditFormCancelRate5StartTime) || propertyName == nameof(model.EditFormCancelRate5EndTime) || propertyName == nameof(model.EditFormCancelRate6StartTime) || propertyName == nameof(model.EditFormCancelRate6EndTime))
                {
                    if (value.ToString().Length > 2)
                    {
                        value = value.ToString().Substring(0, 2);
                    }
                }
                if (propertyName == nameof(model.EditFormCancelRate6) || propertyName == nameof(model.EditFormCancelRate5) || propertyName == nameof(model.EditFormCancelRate4) || propertyName == nameof(model.EditFormCancelRate3) || propertyName == nameof(model.EditFormCancelRate2) || propertyName == nameof(model.EditFormCancelRate1))
                {
                    if (value.ToString().Length > 5)
                    {
                        value = value.ToString().Substring(0, 5);
                    }
                }
                if (propertyName == nameof(model.EditFormSalesChangeablePeriod))
                {
                    if(!decimal.TryParse(value, out decimal number))
                    {
                        value = model.EditFormSalesChangeablePeriod;
                    }
                }
                if (propertyName == nameof(model.EditFormCancellationPeriod))
                {
                    if (!decimal.TryParse(value, out decimal number))
                    {
                        value = model.EditFormCancellationPeriod;
                    }
                }
                if (propertyName == nameof(model.EditFormCancelRate1StartTime))
                {
                    if (!int.TryParse(value, out int number))
                    {
                        value = model.EditFormCancelRate1StartTime;
                    }
                }
                if (propertyName == nameof(model.EditFormCancelRate1EndTime))
                {
                    if (!int.TryParse(value, out int number))
                    {
                        value = model.EditFormCancelRate1EndTime;
                    }
                }
                if (propertyName == nameof(model.EditFormCancelRate1))
                {
                    if (!decimal.TryParse(value, out decimal number))
                    {
                        value = model.EditFormCancelRate1;
                    }
                }
                if (propertyName == nameof(model.EditFormCancelRate2StartTime))
                {
                    if (!int.TryParse(value, out int number))
                    {
                        value = model.EditFormCancelRate2StartTime;
                    }
                }
                if (propertyName == nameof(model.EditFormCancelRate2EndTime))
                {
                    if (!int.TryParse(value, out int number))
                    {
                        value = model.EditFormCancelRate2EndTime;
                    }
                }
                if (propertyName == nameof(model.EditFormCancelRate2))
                {
                    if (!decimal.TryParse(value, out decimal number))
                    {
                        value = model.EditFormCancelRate2;
                    }
                }
                if (propertyName == nameof(model.EditFormCancelRate3StartTime))
                {
                    if (!int.TryParse(value, out int number))
                    {
                        value = model.EditFormCancelRate3StartTime;
                    }
                }
                if (propertyName == nameof(model.EditFormCancelRate3EndTime))
                {
                    if (!int.TryParse(value, out int number))
                    {
                        value = model.EditFormCancelRate3EndTime;
                    }
                }
                if (propertyName == nameof(model.EditFormCancelRate3))
                {
                    if (!decimal.TryParse(value, out decimal number))
                    {
                        value = model.EditFormCancelRate3;
                    }
                }
                if (propertyName == nameof(model.EditFormCancelRate4StartTime))
                {
                    if (!int.TryParse(value, out int number))
                    {
                        value = model.EditFormCancelRate4StartTime;
                    }
                }
                if (propertyName == nameof(model.EditFormCancelRate4EndTime))
                {
                    if (!int.TryParse(value, out int number))
                    {
                        value = model.EditFormCancelRate4EndTime;
                    }
                }
                if (propertyName == nameof(model.EditFormCancelRate4))
                {
                    if (!decimal.TryParse(value, out decimal number))
                    {
                        value = model.EditFormCancelRate4;
                    }
                }
                if (propertyName == nameof(model.EditFormCancelRate5StartTime))
                {
                    if (!int.TryParse(value, out int number))
                    {
                        value = model.EditFormCancelRate5StartTime;
                    }
                }
                if (propertyName == nameof(model.EditFormCancelRate5EndTime))
                {
                    if (!int.TryParse(value, out int number))
                    {
                        value = model.EditFormCancelRate5EndTime;
                    }
                }
                if (propertyName == nameof(model.EditFormCancelRate5))
                {
                    if (!decimal.TryParse(value, out decimal number))
                    {
                        value = model.EditFormCancelRate5;
                    }
                }
                if (propertyName == nameof(model.EditFormCancelRate6StartTime))
                {
                    if (!int.TryParse(value, out int number))
                    {
                        value = model.EditFormCancelRate6StartTime;
                    }
                }
                if (propertyName == nameof(model.EditFormCancelRate6EndTime))
                {
                    if (!int.TryParse(value, out int number))
                    {
                        value = model.EditFormCancelRate6EndTime;
                    }
                }
                if (propertyName == nameof(model.EditFormCancelRate6))
                {
                    if (!decimal.TryParse(value, out decimal number))
                    {
                        value = model.EditFormCancelRate6;
                    }
                }
                if (propertyName == nameof(model.EditFormMonthlyProcess))
                {
                    value = value.ToString() == "TransferMonthly" ? EditFormMonthlyProcess.TransferMonthly : EditFormMonthlyProcess.DonotTransferMonthly;
                }
                if (propertyName == nameof(model.EditFormBillForward))
                {
                    value = value.ToString() == "CarryForward" ? EditFormBillForward.CarryForward : EditFormBillForward.DonotCarryForward;
                }
                if (propertyName == nameof(model.EditFormDailyProcess))
                {
                    value = value.ToString() == "ProcessDaily" ? EditFormDailyProcess.ProcessDaily : EditFormDailyProcess.DonotProcessDaily;
                }
                if (propertyName == nameof(model.EditFormSaleClassification))
                {
                    value = value.ToString() == "Start" ? EditFormSaleClassification.Start : EditFormSaleClassification.End;
                }
                if (propertyName == nameof(model.EditFormSalesChangeDateClassification))
                {
                    value = value.ToString() == "Day" ? EditFormSalesChangeDateClassification.Day : EditFormSalesChangeDateClassification.Month;
                }
                if (propertyName == nameof(model.EditFormCheckZeroYen))
                {
                    value = value.ToString() == "DonotCheck" ? EditFormCheckZeroYen.DonotCheck : value.ToString() == "CannotChange" ? EditFormCheckZeroYen.CannotChange : EditFormCheckZeroYen.MessageOnly;
                }
                if (propertyName.ToString() == nameof(model.EditFormCancelDateClassification))
                {
                    value = value.ToString() == "Day" ? EditFormSalesChangeDateClassification.Day : EditFormSalesChangeDateClassification.Month;
                }
                if (propertyName == nameof(model.EditFormFareByVehicle))
                {
                    value = value.ToString() == "CaculateByTotal" ? EditFormFareByVehicle.CaculateByTotal : EditFormFareByVehicle.CaculateByBusType;
                }
                if (propertyName == nameof(model.EditFormTaxFraction) || propertyName == nameof(model.EditFormFeeFraction))
                {
                    value = value.ToString() == "Cut" ? EditFormTaxFraction.Cut : value.ToString() == "Truncate" ? EditFormTaxFraction.Truncate : EditFormTaxFraction.Round;
                }
                if (propertyName == nameof(model.EditFormHiredBusFee))
                {
                    value = value.ToString() == "Reservation" ? EditFormHiredBusFee.Reservation : EditFormHiredBusFee.CarDestination;
                }
                if (propertyName == nameof(model.EditFormHiredBusDifferentClassification))
                {
                    value = value.ToString() == "AddToYourCompany" ? EditFormHiredBusDifferentClassification.AddToYourCompany : EditFormHiredBusDifferentClassification.DonotAdd;
                }
                if (propertyName == nameof(model.EditFormFareTaxDisplay) || propertyName == nameof(model.EditFormLoadingGoodsTaxDisplay))
                {
                    value = value.ToString() == "FreeTax" ? EditFormFareTaxDisplay.FreeTax : value.ToString() == "IncludedTax" ? EditFormFareTaxDisplay.IncludedTax : EditFormFareTaxDisplay.OtherTax;
                }
                if (propertyName == nameof(model.EditFormAutoTemporaryBus))
                {
                    value = value.ToString() == "Do" ? EditFormAutoTemporaryBus.Do : EditFormAutoTemporaryBus.Donot;
                }
                if (propertyName == nameof(model.EditFormPriority))
                {
                    value = value.ToString() == "SaleOfficeOrder" ? EditFormPriority.SaleOfficeOrder : EditFormPriority.VehicleTypeOrder;
                }
                if (propertyName == nameof(model.EditFormAutoTemporaryBusDivision))
                {
                    value = value.ToString() == "Standard" ? EditFormAutoTemporaryBusDivision.Standard : EditFormAutoTemporaryBusDivision.Daily;
                }
                if (propertyName == nameof(model.EditFormVehicleReplacement))
                {
                    value = value.ToString() == "VehicleOnly" ? EditFormVehicleReplacement.VehicleOnly : value.ToString() == "All" ? EditFormVehicleReplacement.All : value.ToString() == "VehicleDriver" ? EditFormVehicleReplacement.VehicleDriver : EditFormVehicleReplacement.VehicleGuider;
                }
                if (propertyName == nameof(model.EditFormCrewCompatibilityCheck))
                {
                    value = value.ToString() == "DonotCheck" ? EditFormCrewCompatibilityCheck.DonotCheck : EditFormCrewCompatibilityCheck.Check;
                }
                if (propertyName == nameof(model.EditFormAutoKoban))
                {
                    value = value.ToString() == "No" ? EditFormAutoKoban.No : value.ToString() == "VehiclePersonAssignment" ? EditFormAutoKoban.VehiclePersonAssignment : EditFormAutoKoban.AutomaticPoliceBox;
                }
                if (propertyName == nameof(model.EditFormReportClassification))
                {
                    value = value.ToString() == "Departure" ? EditFormReportClassification.Departure : EditFormReportClassification.Return;
                }
                if (propertyName == nameof(model.EditFormReportSummary))
                {
                    value = value.ToString() == "Delivery" ? EditFormReportSummary.Delivery : EditFormReportSummary.TaxExcluded;
                }
                if (propertyName == nameof(model.EditFormReportOutput))
                {
                    value = value.ToString() == "Tohoku" ? EditFormReportOutput.Tohoku : value.ToString() == "Kanto" ? EditFormReportOutput.Kanto : value.ToString() == "Koshinetsu" ? EditFormReportOutput.Koshinetsu : EditFormReportOutput.Other;
                }
                if (propertyName == nameof(model.EditFormTransportationMiscellaneousIncome))
                {
                    value = value.ToString() == "Yes" ? EditFormTransportationMiscellaneousIncome.Yes : EditFormTransportationMiscellaneousIncome.No;
                }
                if (propertyName == nameof(model.EditFormDisplayDetailSelection))
                {
                    value = value.ToString() == "Invert" ? EditFormDisplayDetailSelection.Invert : EditFormDisplayDetailSelection.Frame;
                }
                if (propertyName == nameof(model.EditFormCurrentInvoice))
                {
                    value = value.ToString() == "NoManagement" ? EditFormCurrentInvoice.NoManagement : EditFormCurrentInvoice.Manage;
                }
                if (propertyName == nameof(model.EditFormInitTransferEstimateData) || propertyName == nameof(model.EditFormInitCopyOperationDateRemarkData) || propertyName == nameof(model.EditFormInitCopyReservationRemarkData) || propertyName == nameof(model.EditFormInitCopyBoardingPlaceData) || propertyName == nameof(model.EditFormInitCopyArrangeData) || propertyName == nameof(model.EditFormInitCopyLoadingGoodData) || propertyName == nameof(model.EditFormInitCopyIncidentalData) || propertyName == nameof(model.EditFormInitCopyProcessData) || propertyName == nameof(model.EditFormIncidentalType4Addition) || propertyName == nameof(model.EditFormIncidentalType1Addition) || propertyName == nameof(model.EditFormIncidentalType2Addition) || propertyName == nameof(model.EditFormIncidentalType3Addition))
                {
                    value = value.ToString() == "None" ? YesNoRadio.None : YesNoRadio.Yes;
                }
                var propertyInfo = model.GetType().GetProperty(propertyName);
                propertyInfo.SetValue(model, value);
                if(propertyName == nameof(model.Company))
                {
                    if (model.Company != null)
                    {
                        var isExistKasSet = _service.GetExitKasSet(model.Company.CompanyCdSeq).Result;
                        if (isExistKasSet)
                        {
                            ReadOnly = true;
                            model.IsDuplicateCompany = true;
                        }
                        else
                        {
                            CompanyReadOnly = true;
                            ReadOnly = false;
                            model.IsDuplicateCompany = false;
                            UnSpecificList = _service.GetSpecificWorkHolidayTypes(model?.Company != null ? model.Company.CompanyCdSeq : 0).Result;
                            UnAssignList = _service.GetWorkHolidayTypes(model?.Company != null ? model.Company.CompanyCdSeq : 0).Result;
                            LoadSpecificData();
                        }
                    }
                    else
                    {
                        ReadOnly = true;
                        model.IsDuplicateCompany = false;
                        ResetListWorkType();
                    }
                }
                InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected void TriggerTabChange()
        {
            StateHasChanged();
        }

        public async Task Save()
        {
            if (formContext.Validate())
            {
                _loading.ShowAsync();
                ShowPopupChanged.InvokeAsync(false);
                SaveSetting();
                SaveJiskin();
                SelectedItem = null;
                OnTogglePopup.InvokeAsync(true);
                _loading.HideAsync();
            }
        }

        public async Task ResetForm()
        {
            if (SelectedItem != null)
            {
                LoadData(SelectedItem);
                UnAssignList = _service.GetWorkHolidayTypes(SelectedItem != null ? SelectedItem.CompanyCdSeq : 0).Result;
                UnSpecificList = _service.GetSpecificWorkHolidayTypes(SelectedItem != null ? SelectedItem.CompanyCdSeq : 0).Result;

                WorkHolidayRow1 = UnSpecificList.Where(x => x.JisKinKyuCd == 1).ToList();
                WorkHolidayRow2 = UnSpecificList.Where(x => x.JisKinKyuCd == 2).ToList();
                WorkHolidayRow3 = UnSpecificList.Where(x => x.JisKinKyuCd == 3).ToList();
                WorkHolidayRow4 = UnSpecificList.Where(x => x.JisKinKyuCd == 4).ToList();
                WorkHolidayRow5 = UnSpecificList.Where(x => x.JisKinKyuCd == 5).ToList();
                WorkHolidayRow6 = UnSpecificList.Where(x => x.JisKinKyuCd == 6).ToList();
                WorkHolidayRow7 = UnSpecificList.Where(x => x.JisKinKyuCd == 7).ToList();
                WorkHolidayRow8 = UnSpecificList.Where(x => x.JisKinKyuCd == 8).ToList();
                WorkHolidayRow9 = UnSpecificList.Where(x => x.JisKinKyuCd == 9).ToList();
                WorkHolidayRow10 = UnSpecificList.Where(x => x.JisKinKyuCd == 10).ToList();
            }
            else
            { 
                CompanyReadOnly = false;
                ResetListWorkType();
            }
            
            await OnReset.InvokeAsync(true);
        }

        public void ResetListWorkType()
        {
            UnAssignList = new List<WorkHolidayType>();
            UnSpecificList = new List<WorkHolidayType>();
            WorkHolidayRow1 = new List<WorkHolidayType>();
            WorkHolidayRow2 = new List<WorkHolidayType>();
            WorkHolidayRow3 = new List<WorkHolidayType>();
            WorkHolidayRow4 = new List<WorkHolidayType>();
            WorkHolidayRow5 = new List<WorkHolidayType>();
            WorkHolidayRow6 = new List<WorkHolidayType>();
            WorkHolidayRow7 = new List<WorkHolidayType>();
            WorkHolidayRow8 = new List<WorkHolidayType>();
            WorkHolidayRow9 = new List<WorkHolidayType>();
            WorkHolidayRow10 = new List<WorkHolidayType>();
        }
    }
}
