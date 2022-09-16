using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto.RegulationSetting
{
    public class RegulationSettingModel
    {
        public CompanyListItem CompanyFrom { get; set; }
        public CompanyListItem CompanyTo { get; set; }
    }

    public class RegulationSettingFormModel
    {
        public CompanyListItem Company { get; set; }
        public bool IsDuplicateCompany { get; set; } = false;

        // Tab 1
        public EditFormMonthlyProcess EditFormMonthlyProcess { get; set; } = EditFormMonthlyProcess.TransferMonthly;
        public EditFormBillForward EditFormBillForward { get; set; } = EditFormBillForward.CarryForward;
        public EditFormDailyProcess EditFormDailyProcess { get; set; } = EditFormDailyProcess.ProcessDaily;
        public string EditFormLastUpdateStaffCode { get; set; }
        public string EditFormLastUpdateStaffName { get; set; }
        public string EditFormLastUpdateDate { get; set; }
        public string EditFormLastUpdateTime { get; set; }

        // Tab 2
        public EditFormSaleClassification EditFormSaleClassification { get; set; } = EditFormSaleClassification.Start;
        public bool EditFormSaleChange { get; set; } = true;
        public string EditFormSalesChangeablePeriod { get; set; }
        public EditFormSalesChangeDateClassification EditFormSalesChangeDateClassification { get; set; } = EditFormSalesChangeDateClassification.Day;
        public EditFormCheckZeroYen EditFormCheckZeroYen { get; set; } = EditFormCheckZeroYen.DonotCheck;
        public bool EditFormCancelClassification { get; set; } = true;
        public string EditFormCancellationPeriod { get; set; }
        public EditFormSalesChangeDateClassification2 EditFormCancelDateClassification { get; set; } = EditFormSalesChangeDateClassification2.Day;
        public EditFormFareByVehicle EditFormFareByVehicle { get; set; } = EditFormFareByVehicle.CaculateByTotal;
        public EditFormTaxFraction EditFormTaxFraction { get; set; } = EditFormTaxFraction.Cut;
        public EditFormTaxFraction EditFormFeeFraction { get; set; } = EditFormTaxFraction.Cut;
        public EditFormHiredBusFee EditFormHiredBusFee { get; set; } = EditFormHiredBusFee.Reservation;
        public EditFormHiredBusDifferentClassification EditFormHiredBusDifferentClassification { get; set; } = EditFormHiredBusDifferentClassification.AddToYourCompany;
        public EditFormFareTaxDisplay EditFormFareTaxDisplay { get; set; } = EditFormFareTaxDisplay.FreeTax;
        public EditFormFareTaxDisplay EditFormLoadingGoodsTaxDisplay { get; set; } = EditFormFareTaxDisplay.FreeTax;

        public string EditFormCancelRate1StartTime { get; set; } = string.Empty;
        public string EditFormCancelRate2StartTime { get; set; } = string.Empty;
        public string EditFormCancelRate3StartTime { get; set; } = string.Empty;
        public string EditFormCancelRate4StartTime { get; set; } = string.Empty;
        public string EditFormCancelRate5StartTime { get; set; } = string.Empty;
        public string EditFormCancelRate6StartTime { get; set; } = string.Empty;

        public string EditFormCancelRate1EndTime { get; set; } = string.Empty;
        public string EditFormCancelRate2EndTime { get; set; } = string.Empty;
        public string EditFormCancelRate3EndTime { get; set; } = string.Empty;
        public string EditFormCancelRate4EndTime { get; set; } = string.Empty;
        public string EditFormCancelRate5EndTime { get; set; } = string.Empty;
        public string EditFormCancelRate6EndTime { get; set; } = string.Empty;

        public string EditFormCancelRate1 { get; set; } = string.Empty;
        public string EditFormCancelRate2 { get; set; } = string.Empty;
        public string EditFormCancelRate3 { get; set; } = string.Empty;
        public string EditFormCancelRate4 { get; set; } = string.Empty;
        public string EditFormCancelRate5 { get; set; } = string.Empty;
        public string EditFormCancelRate6 { get; set; } = string.Empty;

        // Tab 3
        public EditFormAutoTemporaryBus EditFormAutoTemporaryBus { get; set; } = EditFormAutoTemporaryBus.Do;
        public EditFormPriority EditFormPriority { get; set; } = EditFormPriority.SaleOfficeOrder;
        public EditFormAutoTemporaryBusDivision EditFormAutoTemporaryBusDivision { get; set; } = EditFormAutoTemporaryBusDivision.Standard;

        // Tab 4
        public EditFormVehicleReplacement EditFormVehicleReplacement { get; set; } = EditFormVehicleReplacement.VehicleOnly;
        public EditFormCrewCompatibilityCheck EditFormCrewCompatibilityCheck { get; set; } = EditFormCrewCompatibilityCheck.DonotCheck;
        public EditFormAutoKoban EditFormAutoKoban { get; set; } = EditFormAutoKoban.No;

        // Tab 5 
        public EiygoItem EditFormCharacter1DisplayByBusType { get; set; }
        public string EditFormColor1DisplayByBusType { get; set; } = "#ffffff";
        public EiygoItem EditFormCharacter2DisplayByBusType { get; set; }
        public string EditFormColor2DisplayByBusType { get; set; } = "#ffffff";
        public EiygoItem EditFormCharacter3DisplayByBusType { get; set; }
        public string EditFormColor3DisplayByBusType { get; set; } = "#ffffff";
        public EiygoItem EditFormCharacter4DisplayByBusType { get; set; }
        public string EditFormColor4DisplayByBusType { get; set; } = "#ffffff";
        public EiygoItem EditFormCharacter5DisplayByBusType { get; set; }
        public string EditFormColor5DisplayByBusType { get; set; } = "#ffffff";

        public EiygoItem EditFormCharacter1DisplayByCrew { get; set; }
        public string EditFormColor1DisplayByCrew { get; set; } = "#ffffff";
        public EiygoItem EditFormCharacter2DisplayByCrew { get; set; }
        public string EditFormColor2DisplayByCrew { get; set; } = "#ffffff";
        public EiygoItem EditFormCharacter3DisplayByCrew { get; set; }
        public string EditFormColor3DisplayByCrew { get; set; } = "#ffffff";
        public EiygoItem EditFormCharacter4DisplayByCrew { get; set; }
        public string EditFormColor4DisplayByCrew { get; set; } = "#ffffff";
        public EiygoItem EditFormCharacter5DisplayByCrew { get; set; }
        public string EditFormColor5DisplayByCrew { get; set; } = "#ffffff";

        // Tab 6 
        public EiygoItem EditFormCharacterClassification1DisplayByBusType { get; set; }
        public string EditFormColorClassification1DisplayByBusType { get; set; } = "#ffffff";
        public EiygoItem EditFormCharacterClassification2DisplayByBusType { get; set; }
        public string EditFormColorClassification2DisplayByBusType { get; set; } = "#ffffff";
        public EiygoItem EditFormCharacterClassification3DisplayByBusType { get; set; }
        public string EditFormColorClassification3DisplayByBusType { get; set; } = "#ffffff";
        public EiygoItem EditFormCharacterClassification4DisplayByBusType { get; set; }
        public string EditFormColorClassification4DisplayByBusType { get; set; } = "#ffffff";
        public EiygoItem EditFormCharacterClassification5DisplayByBusType { get; set; }
        public string EditFormColorClassification5DisplayByBusType { get; set; } = "#ffffff";
        public EiygoItem EditFormCharacterClassification6DisplayByBusType { get; set; }
        public string EditFormColorClassification6DisplayByBusType { get; set; } = "#ffffff";
        public EiygoItem EditFormCharacterClassification7DisplayByBusType { get; set; }
        public string EditFormColorClassification7DisplayByBusType { get; set; } = "#ffffff";
        public EiygoItem EditFormCharacterClassification8DisplayByBusType { get; set; }
        public string EditFormColorClassification8DisplayByBusType { get; set; } = "#ffffff";
        public EiygoItem EditFormCharacterClassification9DisplayByBusType { get; set; }
        public string EditFormColorClassification9DisplayByBusType { get; set; } = "#ffffff";
        public EiygoItem EditFormCharacterClassification10DisplayByBusType { get; set; }
        public string EditFormColorClassification10DisplayByBusType { get; set; } = "#ffffff";
        public EiygoItem EditFormCharacterClassification11DisplayByBusType { get; set; }
        public string EditFormColorClassification11DisplayByBusType { get; set; } = "#ffffff";
        public EiygoItem EditFormCharacterClassification12DisplayByBusType { get; set; }
        public string EditFormColorClassification12DisplayByBusType { get; set; } = "#ffffff";
        public EiygoItem EditFormCharacterClassification13DisplayByBusType { get; set; }
        public string EditFormColorClassification13DisplayByBusType { get; set; } = "#ffffff";
        public EiygoItem EditFormCharacterClassification14DisplayByBusType { get; set; }
        public string EditFormColorClassification14DisplayByBusType { get; set; } = "#ffffff";
        public EiygoItem EditFormCharacterClassification15DisplayByBusType { get; set; }
        public string EditFormColorClassification15DisplayByBusType { get; set; } = "#ffffff";

        public EiygoItem EditFormCharacterClassification1DisplayByCrewType { get; set; }
        public string EditFormColorClassification1DisplayByCrewType { get; set; } = "#ffffff";
        public EiygoItem EditFormCharacterClassification2DisplayByCrewType { get; set; }
        public string EditFormColorClassification2DisplayByCrewType { get; set; } = "#ffffff";
        public EiygoItem EditFormCharacterClassification3DisplayByCrewType { get; set; }
        public string EditFormColorClassification3DisplayByCrewType { get; set; } = "#ffffff";
        public EiygoItem EditFormCharacterClassification4DisplayByCrewType { get; set; }
        public string EditFormColorClassification4DisplayByCrewType { get; set; } = "#ffffff";
        public EiygoItem EditFormCharacterClassification5DisplayByCrewType { get; set; }
        public string EditFormColorClassification5DisplayByCrewType { get; set; } = "#ffffff";
        public EiygoItem EditFormCharacterClassification6DisplayByCrewType { get; set; }
        public string EditFormColorClassification6DisplayByCrewType { get; set; } = "#ffffff";
        public EiygoItem EditFormCharacterClassification7DisplayByCrewType { get; set; }
        public string EditFormColorClassification7DisplayByCrewType { get; set; } = "#ffffff";
        public EiygoItem EditFormCharacterClassification8DisplayByCrewType { get; set; }
        public string EditFormColorClassification8DisplayByCrewType { get; set; } = "#ffffff";
        public EiygoItem EditFormCharacterClassification9DisplayByCrewType { get; set; }
        public string EditFormColorClassification9DisplayByCrewType { get; set; } = "#ffffff";
        public EiygoItem EditFormCharacterClassification10DisplayByCrewType { get; set; }
        public string EditFormColorClassification10DisplayByCrewType { get; set; } = "#ffffff";
        public EiygoItem EditFormCharacterClassification11DisplayByCrewType { get; set; }
        public string EditFormColorClassification11DisplayByCrewType { get; set; } = "#ffffff";
        public EiygoItem EditFormCharacterClassification12DisplayByCrewType { get; set; }
        public string EditFormColorClassification12DisplayByCrewType { get; set; } = "#ffffff";
        public EiygoItem EditFormCharacterClassification13DisplayByCrewType { get; set; }
        public string EditFormColorClassification13DisplayByCrewType { get; set; } = "#ffffff";
        public EiygoItem EditFormCharacterClassification14DisplayByCrewType { get; set; }
        public string EditFormColorClassification14DisplayByCrewType { get; set; } = "#ffffff";
        public EiygoItem EditFormCharacterClassification15DisplayByCrewType { get; set; }
        public string EditFormColorClassification15DisplayByCrewType { get; set; } = "#ffffff";

        // Tab 7
        public string EditFormBillComent1 { get; set; } = string.Empty;
        public string EditFormBillComent2 { get; set; } = string.Empty;
        public string EditFormBillComent3 { get; set; } = string.Empty;
        public string EditFormBillComent4 { get; set; } = string.Empty;
        public string EditFormBillComent5 { get; set; } = string.Empty;
        public string EditFormBillComent6 { get; set; } = string.Empty;


        // Tab 8
        public EditFormReportClassification EditFormReportClassification { get; set; } = EditFormReportClassification.Departure;
        public EditFormReportSummary EditFormReportSummary { get; set; } = EditFormReportSummary.Delivery;
        public EditFormReportOutput EditFormReportOutput { get; set; } = EditFormReportOutput.Tohoku;
        public EditFormTransportationMiscellaneousIncome EditFormTransportationMiscellaneousIncome { get; set; } = EditFormTransportationMiscellaneousIncome.Yes;
        public EditFormDisplayDetailSelection EditFormDisplayDetailSelection { get; set; } = EditFormDisplayDetailSelection.Invert;
        public EditFormCurrentInvoice EditFormCurrentInvoice { get; set; } = EditFormCurrentInvoice.NoManagement;
        public YesNoRadio EditFormIncidentalType1Addition { get; set; } = YesNoRadio.None;
        public YesNoRadio EditFormIncidentalType2Addition { get; set; } = YesNoRadio.None;
        public YesNoRadio EditFormIncidentalType3Addition { get; set; } = YesNoRadio.None;
        public YesNoRadio EditFormIncidentalType4Addition { get; set; } = YesNoRadio.None;
        public YesNoRadio EditFormInitCopyProcessData { get; set; } = YesNoRadio.None;
        public YesNoRadio EditFormInitCopyIncidentalData { get; set; } = YesNoRadio.None;
        public YesNoRadio EditFormInitCopyLoadingGoodData { get; set; } = YesNoRadio.None;
        public YesNoRadio EditFormInitCopyArrangeData { get; set; } = YesNoRadio.None;
        public YesNoRadio EditFormInitCopyBoardingPlaceData { get; set; } = YesNoRadio.None;
        public YesNoRadio EditFormInitCopyReservationRemarkData { get; set; } = YesNoRadio.None;
        public YesNoRadio EditFormInitCopyOperationDateRemarkData { get; set; } = YesNoRadio.None;
        public YesNoRadio EditFormInitTransferEstimateData { get; set; } = YesNoRadio.None;

        //tab 9
        public string JisKinKyuNm01 { get; set; } = string.Empty;
        public string JisKinKyuNm02 { get; set; } = string.Empty;
        public string JisKinKyuNm03 { get; set; } = string.Empty;
        public string JisKinKyuNm04 { get; set; } = string.Empty;
        public string JisKinKyuNm05 { get; set; } = string.Empty;
        public string JisKinKyuNm06 { get; set; } = string.Empty;
        public string JisKinKyuNm07 { get; set; } = string.Empty;
        public string JisKinKyuNm08 { get; set; } = string.Empty;
        public string JisKinKyuNm09 { get; set; } = string.Empty;
        public string JisKinKyuNm10 { get; set; } = string.Empty;

        public List<WorkHolidayType> JisKinKyuList01 { get; set; }
        public List<WorkHolidayType> JisKinKyuList02 { get; set; }
        public List<WorkHolidayType> JisKinKyuList03 { get; set; }
        public List<WorkHolidayType> JisKinKyuList04 { get; set; }
        public List<WorkHolidayType> JisKinKyuList05 { get; set; }
        public List<WorkHolidayType> JisKinKyuList06 { get; set; }
        public List<WorkHolidayType> JisKinKyuList07 { get; set; }
        public List<WorkHolidayType> JisKinKyuList08 { get; set; }
        public List<WorkHolidayType> JisKinKyuList09 { get; set; }
        public List<WorkHolidayType> JisKinKyuList10 { get; set; }

    }
    public class EiygoItem
    {
        public int CodeKbnSeq { get; set; }
        public string CodeKbn { get; set; }
        public string CodeKbnNm { get; set; }
        public string DisplayName { get; set; }
    }
    public enum EditFormReportClassification { Departure = 1, Return = 2 }
    public enum EditFormReportSummary { Delivery = 1, TaxExcluded = 2 }
    public enum EditFormReportOutput { Tohoku = 1, Kanto = 2, Koshinetsu = 3, Other = 4 }
    public enum EditFormTransportationMiscellaneousIncome { Yes = 1, No = 2 }
    public enum EditFormDisplayDetailSelection { Invert = 1, Frame = 2 }
    public enum EditFormCurrentInvoice { NoManagement = 0, Manage = 1 }
    public enum YesNoRadio { None = 0, Yes = 1 }
    public enum EditFormAutoKoban
    {
        No = 1,
        VehiclePersonAssignment = 2,
        AutomaticPoliceBox = 3
    }
    public enum EditFormCrewCompatibilityCheck
    {
        DonotCheck = 1,
        Check = 2
    }
    public enum EditFormVehicleReplacement
    {
        VehicleOnly = 1,
        VehicleDriver = 2,
        VehicleGuider = 3,
        All = 4
    }
    public enum EditFormAutoTemporaryBusDivision
    {
        Standard = 1,
        Daily = 2
    }
    public enum EditFormPriority
    {
        SaleOfficeOrder = 1,
        VehicleTypeOrder = 2
    }
    public enum EditFormAutoTemporaryBus
    {
        Do = 1,
        Donot = 2
    }
    public enum EditFormFareTaxDisplay
    {
        FreeTax = 1,
        IncludedTax = 2,
        OtherTax = 3
    }
    public enum EditFormHiredBusDifferentClassification
    {
        AddToYourCompany = 1,
        DonotAdd = 2
    }
    public enum EditFormHiredBusFee
    {
        Reservation = 1,
        CarDestination = 2
    }

    public enum EditFormTaxFraction
    {
        Cut = 1,
        Truncate = 2,
        Round = 3
    }
    public enum EditFormFareByVehicle
    {
        CaculateByTotal = 1,
        CaculateByBusType = 2
    }
    public enum EditFormCheckZeroYen
    {
        DonotCheck = 1,
        CannotChange = 2,
        MessageOnly = 3
    }
    public enum EditFormSalesChangeDateClassification
    {
        Day = 1,
        Month = 2
    }
    public enum EditFormSalesChangeDateClassification2
    {
        Day = 1,
        Month = 2
    }
    public enum EditFormSaleClassification
    {
        Start = 1,
        End = 2
    }
    public enum EditFormMonthlyProcess
    {
        TransferMonthly = 1,
        DonotTransferMonthly = 2
    }

    public enum EditFormBillForward
    {
        CarryForward = 1,
        DonotCarryForward = 2
    }

    public enum EditFormDailyProcess
    {
        ProcessDaily = 1,
        DonotProcessDaily = 2
    }

    public class RegulationSettingItem
    {
        public int CompanyCdSeq { get; set; }
        public byte UriKbn { get; set; }
        public byte SyohiHasu { get; set; }
        public byte TesuHasu { get; set; }
        public byte HoukoKbn { get; set; }
        public byte HouZeiKbn { get; set; }
        public byte HouOutKbn { get; set; }
        public byte JKariKbn { get; set; }
        public byte AutKarJyun { get; set; }
        public byte JKBunPat { get; set; }
        public byte SyaIrePat { get; set; }
        public byte JymAChkKbn { get; set; }
        public byte UriHenKbn { get; set; }
        public byte UriMDKbn { get; set; }
        public byte UriHenKikan { get; set; }
        public byte UriZeroChk { get; set; }
        public byte CanKbn { get; set; }
        public byte CanMDKbn { get; set; }
        public byte CanKikan { get; set; }
        public byte CanWaitKbn { get; set; }
        public byte CanJidoKbn { get; set; }
        public byte YouTesuKbn { get; set; }
        public byte YouSagaKbn { get; set; }
        public byte SyaUntKbn { get; set; }
        public byte ZasyuKbn { get; set; }
        public byte FutSF1Flg { get; set; }
        public byte FutSF2Flg { get; set; }
        public byte FutSF3Flg { get; set; }
        public byte FutSF4Flg { get; set; }
        public byte SokoJunKbn { get; set; }
        public byte UntZeiKbn { get; set; }
        public byte TumZeiKbn { get; set; }
        public string ColKari { get; set; }
        public string ColKariH { get; set; }
        public string ColNin { get; set; }
        public string ColHai { get; set; }
        public string ColHaiin { get; set; }
        public string ColWari { get; set; }
        public string ColShiha { get; set; }
        public string ColKaku { get; set; }
        public string ColNip { get; set; }
        public string ColNyu { get; set; }
        public string ColYou { get; set; }
        public string ColMiKari { get; set; }
        public string ColIcKari { get; set; }
        public string ColIcKariH { get; set; }
        public string ColIcHai { get; set; }
        public string ColIcHaiin { get; set; }
        public string ColIcWari { get; set; }
        public string ColIcShiha { get; set; }
        public string ColIcNip { get; set; }
        public string ColIcNyu { get; set; }
        public string ColIcYou { get; set; }
        public string ColNCou { get; set; }
        public string ColIcNCou { get; set; }
        public string ColSCou { get; set; }
        public string ColIcSCou { get; set; }
        public string ColKYoy { get; set; }
        public string ColSelect { get; set; }
        public string ColKanyu { get; set; }
        public string ColKahar { get; set; }
        public byte SryHyjSyu { get; set; }
        public byte SryHyjHga { get; set; }
        public byte SryHyjTde { get; set; }
        public byte SryHyjTch { get; set; }
        public byte SryHyjTka { get; set; }
        public decimal CanRit1 { get; set; }
        public byte CanSKan1 { get; set; }
        public byte CanEKan1 { get; set; }
        public decimal CanRit2 { get; set; }
        public byte CanSKan2 { get; set; }
        public byte CanEKan2 { get; set; }
        public decimal CanRit3 { get; set; }
        public byte CanSKan3 { get; set; }
        public byte CanEKan3 { get; set; }
        public decimal CanRit4 { get; set; }
        public byte CanSKan4 { get; set; }
        public byte CanEKan4 { get; set; }
        public decimal CanRit5 { get; set; }
        public byte CanSKan5 { get; set; }
        public byte CanEKan5 { get; set; }
        public decimal CanRit6 { get; set; }
        public byte CanSKan6 { get; set; }
        public byte CanEKan6 { get; set; }
        public byte GetSyoKbn { get; set; }
        public byte SeiKrksKbn { get; set; }
        public byte DaySyoKbn { get; set; }
        public byte KouYouSet { get; set; }
        public byte AutKouKbn { get; set; }
        public byte SenHyoHi { get; set; }
        public byte SenYouDefFlg { get; set; }
        public byte SenMikDefFlg { get; set; }
        public byte KoteiCopyFlg { get; set; }
        public byte FutaiCopyFlg { get; set; }
        public byte TumiCopyFlg { get; set; }
        public byte TehaiCopyFlg { get; set; }
        public byte JoshaCopyFlg { get; set; }
        public byte YykCopyFlg { get; set; }
        public byte UkbCopyFlg { get; set; }
        public byte SeiGenFlg { get; set; }
        public byte MeiShyKbn { get; set; }
        public byte SyaSenMjPtnKbn1 { get; set; }
        public string SyaSenMjPtnCol1 { get; set; }
        public byte SyaSenMjPtnKbn2 { get; set; }
        public string SyaSenMjPtnCol2 { get; set; }
        public byte SyaSenMjPtnKbn3 { get; set; }
        public string SyaSenMjPtnCol3 { get; set; }
        public byte SyaSenMjPtnKbn4 { get; set; }
        public string SyaSenMjPtnCol4 { get; set; }
        public byte SyaSenMjPtnKbn5 { get; set; }
        public string SyaSenMjPtnCol5 { get; set; }
        public byte JyoSenMjPtnKbn1 { get; set; }
        public string JyoSenMjPtnCol1 { get; set; }
        public byte JyoSenMjPtnKbn2 { get; set; }
        public string JyoSenMjPtnCol2 { get; set; }
        public byte JyoSenMjPtnKbn3 { get; set; }
        public string JyoSenMjPtnCol3 { get; set; }
        public byte JyoSenMjPtnKbn4 { get; set; }
        public string JyoSenMjPtnCol4 { get; set; }
        public byte JyoSenMjPtnKbn5 { get; set; }
        public string JyoSenMjPtnCol5 { get; set; }
        public string SeiCom1 { get; set; }
        public string SeiCom2 { get; set; }
        public string SeiCom3 { get; set; }
        public string SeiCom4 { get; set; }
        public string SeiCom5 { get; set; }
        public string SeiCom6 { get; set; }
        public string JisKinKyuNm01 { get; set; }
        public string JisKinKyuNm02 { get; set; }
        public string JisKinKyuNm03 { get; set; }
        public string JisKinKyuNm04 { get; set; }
        public string JisKinKyuNm05 { get; set; }
        public string JisKinKyuNm06 { get; set; }
        public string JisKinKyuNm07 { get; set; }
        public string JisKinKyuNm08 { get; set; }
        public string JisKinKyuNm09 { get; set; }
        public string JisKinKyuNm10 { get; set; }
        public byte SyaSenInfoPtnKbn1 { get; set; }
        public string SyaSenInfoPtnCol1 { get; set; }
        public byte SyaSenInfoPtnKbn2 { get; set; }
        public string SyaSenInfoPtnCol2 { get; set; }
        public byte SyaSenInfoPtnKbn3 { get; set; }
        public string SyaSenInfoPtnCol3 { get; set; }
        public byte SyaSenInfoPtnKbn4 { get; set; }
        public string SyaSenInfoPtnCol4 { get; set; }
        public byte SyaSenInfoPtnKbn5 { get; set; }
        public string SyaSenInfoPtnCol5 { get; set; }
        public byte SyaSenInfoPtnKbn6 { get; set; }
        public string SyaSenInfoPtnCol6 { get; set; }
        public byte SyaSenInfoPtnKbn7 { get; set; }
        public string SyaSenInfoPtnCol7 { get; set; }
        public byte SyaSenInfoPtnKbn8 { get; set; }
        public string SyaSenInfoPtnCol8 { get; set; }
        public byte SyaSenInfoPtnKbn9 { get; set; }
        public string SyaSenInfoPtnCol9 { get; set; }
        public byte SyaSenInfoPtnKbn10 { get; set; }
        public string SyaSenInfoPtnCol10 { get; set; }
        public byte SyaSenInfoPtnKbn11 { get; set; }
        public string SyaSenInfoPtnCol11 { get; set; }
        public byte SyaSenInfoPtnKbn12 { get; set; }
        public string SyaSenInfoPtnCol12 { get; set; }
        public byte SyaSenInfoPtnKbn13 { get; set; }
        public string SyaSenInfoPtnCol13 { get; set; }
        public byte SyaSenInfoPtnKbn14 { get; set; }
        public string SyaSenInfoPtnCol14 { get; set; }
        public byte SyaSenInfoPtnKbn15 { get; set; }
        public string SyaSenInfoPtnCol15 { get; set; }
        public byte JyoSenInfoPtnKbn1 { get; set; }
        public string JyoSenInfoPtnCol1 { get; set; }
        public byte JyoSenInfoPtnKbn2 { get; set; }
        public string JyoSenInfoPtnCol2 { get; set; }
        public byte JyoSenInfoPtnKbn3 { get; set; }
        public string JyoSenInfoPtnCol3 { get; set; }
        public byte JyoSenInfoPtnKbn4 { get; set; }
        public string JyoSenInfoPtnCol4 { get; set; }
        public byte JyoSenInfoPtnKbn5 { get; set; }
        public string JyoSenInfoPtnCol5 { get; set; }
        public byte JyoSenInfoPtnKbn6 { get; set; }
        public string JyoSenInfoPtnCol6 { get; set; }
        public byte JyoSenInfoPtnKbn7 { get; set; }
        public string JyoSenInfoPtnCol7 { get; set; }
        public byte JyoSenInfoPtnKbn8 { get; set; }
        public string JyoSenInfoPtnCol8 { get; set; }
        public byte JyoSenInfoPtnKbn9 { get; set; }
        public string JyoSenInfoPtnCol9 { get; set; }
        public byte JyoSenInfoPtnKbn10 { get; set; }
        public string JyoSenInfoPtnCol10 { get; set; }
        public byte JyoSenInfoPtnKbn11 { get; set; }
        public string JyoSenInfoPtnCol11 { get; set; }
        public byte JyoSenInfoPtnKbn12 { get; set; }
        public string JyoSenInfoPtnCol12 { get; set; }
        public byte JyoSenInfoPtnKbn13 { get; set; }
        public string JyoSenInfoPtnCol13 { get; set; }
        public byte JyoSenInfoPtnKbn14 { get; set; }
        public string JyoSenInfoPtnCol14 { get; set; }
        public byte JyoSenInfoPtnKbn15 { get; set; }
        public string JyoSenInfoPtnCol15 { get; set; }
        public byte JyoSyaChkSiyoKbn { get; set; }
        public byte SenDayRenge { get; set; }
        public short SenDefWidth { get; set; }
        public string YykHaiSTime { get; set; }
        public string YykTouTime { get; set; }
        public byte TehaiAutoSet { get; set; }
        public byte GoSyaAutoSet { get; set; }
        public byte YoySyuKiTimeSiyoKbn { get; set; }
        public byte KarSyuKiTimeSiyoKbn { get; set; }
        public byte GuiAutoSet { get; set; }
        public byte DrvAutoSet { get; set; }
        public byte SenBackPtnKbn { get; set; }
        public string SenBackPtnCol { get; set; }
        public byte SenOBPtnKbn { get; set; }
        public string SenOBPtnCol { get; set; }
        public int FutTumCdSeq { get; set; }
        public byte ETCKinKbn { get; set; }
        public int SeisanCdSeq { get; set; }
        public int GuideFutTumCdSeq { get; set; }
        public byte SeiMuki { get; set; }
        public string ExpItem { get; set; }
        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
        public int UpdSyainCd { get; set; }
        public string UpdPrgID { get; set; }
        public byte QuotationTransfer { get; set; }
        public string GridNo { get; set; }
        public string GridCompanyCode { get; set; }
        public string GridCompanyName { get; set; }
        public string GridSaleClassification { get; set; }
        public string GridTaxFraction { get; set; }
        public string GridFeeFraction { get; set; }
        public string GridReportClassification { get; set; }
        public string GridReportSummary { get; set; }
        public string GridReportOutput { get; set; }
        public string GridAutoTemporaryBus { get; set; }
        public string GridAutoPriorty { get; set; }
        public string GridAutoTemporaryBusDivision { get; set; }
        public string GridVehicleReplacement { get; set; }
        public string GridCrewCompatibilityCheck { get; set; }
        public string GridSaleChange { get; set; }
        public string GridCheckZeroYen { get; set; }
        public string GridCancelClassification { get; set; }
        public string GridHiredBusFee { get; set; }
        public string GridHiredBusDifferentClassification { get; set; }
        public string GridFareByVehicle { get; set; }
        public string GridTransportationMiscellaneousIncome { get; set; }
        public string GridIncidentalType1Addition { get; set; }
        public string GridIncidentalType2Addition { get; set; }
        public string GridIncidentalType3Addition { get; set; }
        public string GridIncidentalType4Addition { get; set; }
        public string GridTravelOrder { get; set; }
        public string GridFareTaxDisplay { get; set; }
        public string GridLoadingGoodsTaxDisplay { get; set; }
        public string GridCancelRate1 { get; set; }
        public string GridCancelRate1StartTime { get; set; }
        public string GridCancelRate1EndTime { get; set; }
        public string GridCancelRate2 { get; set; }
        public string GridCancelRate2StartTime { get; set; }
        public string GridCancelRate2EndTime { get; set; }
        public string GridCancelRate3 { get; set; }
        public string GridCancelRate3StartTime { get; set; }
        public string GridCancelRate3EndTime { get; set; }
        public string GridCancelRate4 { get; set; }
        public string GridCancelRate4StartTime { get; set; }
        public string GridCancelRate4EndTime { get; set; }
        public string GridCancelRate5 { get; set; }
        public string GridCancelRate5StartTime { get; set; }
        public string GridCancelRate5EndTime { get; set; }
        public string GridCancelRate6 { get; set; }
        public string GridCancelRate6StartTime { get; set; }
        public string GridCancelRate6EndTime { get; set; }
        public string GridMonthlyProcess { get; set; }
        public string GridBillForward { get; set; }
        public string GridDailyProcess { get; set; }
        public string GridAutoKoban { get; set; }
        public string GridInitCopyProcessData { get; set; }
        public string GridInitCopyIncidentalData { get; set; }
        public string GridInitCopyLoadingGoodData { get; set; }
        public string GridInitCopyArrangeData { get; set; }
        public string GridInitCopyBoardingPlaceData { get; set; }
        public string GridInitCopyReservationRemarkData { get; set; }
        public string GridInitCopyOperationDateRemarkData { get; set; }
        public string GridInitTransferEstimateData { get; set; }
        public string GridCurrentInvoice { get; set; }
        public string GridDisplayDetailSelection { get; set; }
        public string GridCharacter1DisplayByBusType { get; set; }
        public string GridColor1DisplayByBusType { get; set; }
        public string GridCharacter2DisplayByBusType { get; set; }
        public string GridColor2DisplayByBusType { get; set; }
        public string GridCharacter3DisplayByBusType { get; set; }
        public string GridColor3DisplayByBusType { get; set; }
        public string GridCharacter4DisplayByBusType { get; set; }
        public string GridColor4DisplayByBusType { get; set; }
        public string GridCharacter5DisplayByBusType { get; set; }
        public string GridColor5DisplayByBusType { get; set; }
        public string GridCharacter1DisplayByCrew { get; set; }
        public string GridColor1DisplayByCrew { get; set; }
        public string GridCharacter2DisplayByCrew { get; set; }
        public string GridColor2DisplayByCrew { get; set; }
        public string GridCharacter3DisplayByCrew { get; set; }
        public string GridColor3DisplayByCrew { get; set; }
        public string GridCharacter4DisplayByCrew { get; set; }
        public string GridColor4DisplayByCrew { get; set; }
        public string GridCharacter5DisplayByCrew { get; set; }
        public string GridColor5DisplayByCrew { get; set; }
        public string GridBillComent1 { get; set; }
        public string GridBillComent2 { get; set; }
        public string GridBillComent3 { get; set; }
        public string GridBillComent4 { get; set; }
        public string GridBillComent5 { get; set; }
        public string GridBillComent6 { get; set; }
        public string GridAchievementTotalWorkHolidayTypeName1 { get; set; }
        public string GridAchievementTotalWorkHolidayTypeName2 { get; set; }
        public string GridAchievementTotalWorkHolidayTypeName3 { get; set; }
        public string GridAchievementTotalWorkHolidayTypeName4 { get; set; }
        public string GridAchievementTotalWorkHolidayTypeName5 { get; set; }
        public string GridAchievementTotalWorkHolidayTypeName6 { get; set; }
        public string GridAchievementTotalWorkHolidayTypeName7 { get; set; }
        public string GridAchievementTotalWorkHolidayTypeName8 { get; set; }
        public string GridAchievementTotalWorkHolidayTypeName9 { get; set; }
        public string GridAchievementTotalWorkHolidayTypeName10 { get; set; }
        public string GridCharacterClassification1DisplayByBusType { get; set; }
        public string GridColorClassification1DisplayByBusType { get; set; }
        public string GridCharacterClassification2DisplayByBusType { get; set; }
        public string GridColorClassification2DisplayByBusType { get; set; }
        public string GridCharacterClassification3DisplayByBusType { get; set; }
        public string GridColorClassification3DisplayByBusType { get; set; }
        public string GridCharacterClassification4DisplayByBusType { get; set; }
        public string GridColorClassification4DisplayByBusType { get; set; }
        public string GridCharacterClassification5DisplayByBusType { get; set; }
        public string GridColorClassification5DisplayByBusType { get; set; }
        public string GridCharacterClassification6DisplayByBusType { get; set; }
        public string GridColorClassification6DisplayByBusType { get; set; }
        public string GridCharacterClassification7DisplayByBusType { get; set; }
        public string GridColorClassification7DisplayByBusType { get; set; }
        public string GridCharacterClassification8DisplayByBusType { get; set; }
        public string GridColorClassification8DisplayByBusType { get; set; }
        public string GridCharacterClassification9DisplayByBusType { get; set; }
        public string GridColorClassification9DisplayByBusType { get; set; }
        public string GridCharacterClassification10DisplayByBusType { get; set; }
        public string GridColorClassification10DisplayByBusType { get; set; }
        public string GridCharacterClassification11DisplayByBusType { get; set; }
        public string GridColorClassification11DisplayByBusType { get; set; }
        public string GridCharacterClassification12DisplayByBusType { get; set; }
        public string GridColorClassification12DisplayByBusType { get; set; }
        public string GridCharacterClassification13DisplayByBusType { get; set; }
        public string GridColorClassification13DisplayByBusType { get; set; }
        public string GridCharacterClassification14DisplayByBusType { get; set; }
        public string GridColorClassification14DisplayByBusType { get; set; }
        public string GridCharacterClassification15DisplayByBusType { get; set; }
        public string GridColorClassification15DisplayByBusType { get; set; }
        public string GridCharacterClassification1DisplayByCrewType { get; set; }
        public string GridColorClassification1DisplayByCrewType { get; set; }
        public string GridCharacterClassification2DisplayByCrewType { get; set; }
        public string GridColorClassification2DisplayByCrewType { get; set; }
        public string GridCharacterClassification3DisplayByCrewType { get; set; }
        public string GridColorClassification3DisplayByCrewType { get; set; }
        public string GridCharacterClassification4DisplayByCrewType { get; set; }
        public string GridColorClassification4DisplayByCrewType { get; set; }
        public string GridCharacterClassification5DisplayByCrewType { get; set; }
        public string GridColorClassification5DisplayByCrewType { get; set; }
        public string GridCharacterClassification6DisplayByCrewType { get; set; }
        public string GridColorClassification6DisplayByCrewType { get; set; }
        public string GridCharacterClassification7DisplayByCrewType { get; set; }
        public string GridColorClassification7DisplayByCrewType { get; set; }
        public string GridCharacterClassification8DisplayByCrewType { get; set; }
        public string GridColorClassification8DisplayByCrewType { get; set; }
        public string GridCharacterClassification9DisplayByCrewType { get; set; }
        public string GridColorClassification9DisplayByCrewType { get; set; }
        public string GridCharacterClassification10DisplayByCrewType { get; set; }
        public string GridColorClassification10DisplayByCrewType { get; set; }
        public string GridCharacterClassification11DisplayByCrewType { get; set; }
        public string GridColorClassification11DisplayByCrewType { get; set; }
        public string GridCharacterClassification12DisplayByCrewType { get; set; }
        public string GridColorClassification12DisplayByCrewType { get; set; }
        public string GridCharacterClassification13DisplayByCrewType { get; set; }
        public string GridColorClassification13DisplayByCrewType { get; set; }
        public string GridCharacterClassification14DisplayByCrewType { get; set; }
        public string GridColorClassification14DisplayByCrewType { get; set; }
        public string GridCharacterClassification15DisplayByCrewType { get; set; }
        public string GridColorClassification15DisplayByCrewType { get; set; }
        public string GridExtendItem { get; set; }
        public string GridLastUpdateDate { get; set; }
        public string GridLastUpdateTime { get; set; }
        public string GridLastUpdateStaffCode { get; set; }
        public string GridLastUpdateStaffName { get; set; }
        public string GridLastUpdatePgId { get; set; }
    }

    public class SPModel
    {
        public int CompanyCdSeq { get; set; }
        public byte UriKbn { get; set; }
        public byte SyohiHasu { get; set; }
        public byte TesuHasu { get; set; }
        public byte HoukoKbn { get; set; }
        public byte HouZeiKbn { get; set; }
        public byte HouOutKbn { get; set; }
        public byte JKariKbn { get; set; }
        public byte AutKarJyun { get; set; }
        public byte JKBunPat { get; set; }
        public byte SyaIrePat { get; set; }
        public byte JymAChkKbn { get; set; }
        public byte UriHenKbn { get; set; }
        public byte UriMDKbn { get; set; }
        public byte UriHenKikan { get; set; }
        public byte UriZeroChk { get; set; }
        public byte CanKbn { get; set; }
        public byte CanMDKbn { get; set; }
        public byte CanKikan { get; set; }
        public byte CanWaitKbn { get; set; }
        public byte CanJidoKbn { get; set; }
        public byte YouTesuKbn { get; set; }
        public byte YouSagaKbn { get; set; }
        public byte SyaUntKbn { get; set; }
        public byte ZasyuKbn { get; set; }
        public byte FutSF1Flg { get; set; }
        public byte FutSF2Flg { get; set; }
        public byte FutSF3Flg { get; set; }
        public byte FutSF4Flg { get; set; }
        public byte SokoJunKbn { get; set; }
        public byte UntZeiKbn { get; set; }
        public byte TumZeiKbn { get; set; }
        public string ColKari { get; set; }
        public string ColKariH { get; set; }
        public string ColNin { get; set; }
        public string ColHai { get; set; }
        public string ColHaiin { get; set; }
        public string ColWari { get; set; }
        public string ColShiha { get; set; }
        public string ColKaku { get; set; }
        public string ColNip { get; set; }
        public string ColNyu { get; set; }
        public string ColYou { get; set; }
        public string ColMiKari { get; set; }
        public string ColIcKari { get; set; }
        public string ColIcKariH { get; set; }
        public string ColIcHai { get; set; }
        public string ColIcHaiin { get; set; }
        public string ColIcWari { get; set; }
        public string ColIcShiha { get; set; }
        public string ColIcNip { get; set; }
        public string ColIcNyu { get; set; }
        public string ColIcYou { get; set; }
        public string ColNCou { get; set; }
        public string ColIcNCou { get; set; }
        public string ColSCou { get; set; }
        public string ColIcSCou { get; set; }
        public string ColKYoy { get; set; }
        public string ColSelect { get; set; }
        public string ColKanyu { get; set; }
        public string ColKahar { get; set; }
        public byte SryHyjSyu { get; set; }
        public byte SryHyjHga { get; set; }
        public byte SryHyjTde { get; set; }
        public byte SryHyjTch { get; set; }
        public byte SryHyjTka { get; set; }
        public decimal CanRit1 { get; set; }
        public byte CanSKan1 { get; set; }
        public byte CanEKan1 { get; set; }
        public decimal CanRit2 { get; set; }
        public byte CanSKan2 { get; set; }
        public byte CanEKan2 { get; set; }
        public decimal CanRit3 { get; set; }
        public byte CanSKan3 { get; set; }
        public byte CanEKan3 { get; set; }
        public decimal CanRit4 { get; set; }
        public byte CanSKan4 { get; set; }
        public byte CanEKan4 { get; set; }
        public decimal CanRit5 { get; set; }
        public byte CanSKan5 { get; set; }
        public byte CanEKan5 { get; set; }
        public decimal CanRit6 { get; set; }
        public byte CanSKan6 { get; set; }
        public byte CanEKan6 { get; set; }
        public byte GetSyoKbn { get; set; }
        public byte SeiKrksKbn { get; set; }
        public byte DaySyoKbn { get; set; }
        public byte KouYouSet { get; set; }
        public byte AutKouKbn { get; set; }
        public byte SenHyoHi { get; set; }
        public byte SenYouDefFlg { get; set; }
        public byte SenMikDefFlg { get; set; }
        public byte KoteiCopyFlg { get; set; }
        public byte FutaiCopyFlg { get; set; }
        public byte TumiCopyFlg { get; set; }
        public byte TehaiCopyFlg { get; set; }
        public byte JoshaCopyFlg { get; set; }
        public byte YykCopyFlg { get; set; }
        public byte UkbCopyFlg { get; set; }
        public byte SeiGenFlg { get; set; }
        public byte MeiShyKbn { get; set; }
        public byte SyaSenMjPtnKbn1 { get; set; }
        public string SyaSenMjPtnCol1 { get; set; }
        public byte SyaSenMjPtnKbn2 { get; set; }
        public string SyaSenMjPtnCol2 { get; set; }
        public byte SyaSenMjPtnKbn3 { get; set; }
        public string SyaSenMjPtnCol3 { get; set; }
        public byte SyaSenMjPtnKbn4 { get; set; }
        public string SyaSenMjPtnCol4 { get; set; }
        public byte SyaSenMjPtnKbn5 { get; set; }
        public string SyaSenMjPtnCol5 { get; set; }
        public byte JyoSenMjPtnKbn1 { get; set; }
        public string JyoSenMjPtnCol1 { get; set; }
        public byte JyoSenMjPtnKbn2 { get; set; }
        public string JyoSenMjPtnCol2 { get; set; }
        public byte JyoSenMjPtnKbn3 { get; set; }
        public string JyoSenMjPtnCol3 { get; set; }
        public byte JyoSenMjPtnKbn4 { get; set; }
        public string JyoSenMjPtnCol4 { get; set; }
        public byte JyoSenMjPtnKbn5 { get; set; }
        public string JyoSenMjPtnCol5 { get; set; }
        public string SeiCom1 { get; set; }
        public string SeiCom2 { get; set; }
        public string SeiCom3 { get; set; }
        public string SeiCom4 { get; set; }
        public string SeiCom5 { get; set; }
        public string SeiCom6 { get; set; }
        public string JisKinKyuNm01 { get; set; }
        public string JisKinKyuNm02 { get; set; }
        public string JisKinKyuNm03 { get; set; }
        public string JisKinKyuNm04 { get; set; }
        public string JisKinKyuNm05 { get; set; }
        public string JisKinKyuNm06 { get; set; }
        public string JisKinKyuNm07 { get; set; }
        public string JisKinKyuNm08 { get; set; }
        public string JisKinKyuNm09 { get; set; }
        public string JisKinKyuNm10 { get; set; }
        public byte SyaSenInfoPtnKbn1 { get; set; }
        public string SyaSenInfoPtnCol1 { get; set; }
        public byte SyaSenInfoPtnKbn2 { get; set; }
        public string SyaSenInfoPtnCol2 { get; set; }
        public byte SyaSenInfoPtnKbn3 { get; set; }
        public string SyaSenInfoPtnCol3 { get; set; }
        public byte SyaSenInfoPtnKbn4 { get; set; }
        public string SyaSenInfoPtnCol4 { get; set; }
        public byte SyaSenInfoPtnKbn5 { get; set; }
        public string SyaSenInfoPtnCol5 { get; set; }
        public byte SyaSenInfoPtnKbn6 { get; set; }
        public string SyaSenInfoPtnCol6 { get; set; }
        public byte SyaSenInfoPtnKbn7 { get; set; }
        public string SyaSenInfoPtnCol7 { get; set; }
        public byte SyaSenInfoPtnKbn8 { get; set; }
        public string SyaSenInfoPtnCol8 { get; set; }
        public byte SyaSenInfoPtnKbn9 { get; set; }
        public string SyaSenInfoPtnCol9 { get; set; }
        public byte SyaSenInfoPtnKbn10 { get; set; }
        public string SyaSenInfoPtnCol10 { get; set; }
        public byte SyaSenInfoPtnKbn11 { get; set; }
        public string SyaSenInfoPtnCol11 { get; set; }
        public byte SyaSenInfoPtnKbn12 { get; set; }
        public string SyaSenInfoPtnCol12 { get; set; }
        public byte SyaSenInfoPtnKbn13 { get; set; }
        public string SyaSenInfoPtnCol13 { get; set; }
        public byte SyaSenInfoPtnKbn14 { get; set; }
        public string SyaSenInfoPtnCol14 { get; set; }
        public byte SyaSenInfoPtnKbn15 { get; set; }
        public string SyaSenInfoPtnCol15 { get; set; }
        public byte JyoSenInfoPtnKbn1 { get; set; }
        public string JyoSenInfoPtnCol1 { get; set; }
        public byte JyoSenInfoPtnKbn2 { get; set; }
        public string JyoSenInfoPtnCol2 { get; set; }
        public byte JyoSenInfoPtnKbn3 { get; set; }
        public string JyoSenInfoPtnCol3 { get; set; }
        public byte JyoSenInfoPtnKbn4 { get; set; }
        public string JyoSenInfoPtnCol4 { get; set; }
        public byte JyoSenInfoPtnKbn5 { get; set; }
        public string JyoSenInfoPtnCol5 { get; set; }
        public byte JyoSenInfoPtnKbn6 { get; set; }
        public string JyoSenInfoPtnCol6 { get; set; }
        public byte JyoSenInfoPtnKbn7 { get; set; }
        public string JyoSenInfoPtnCol7 { get; set; }
        public byte JyoSenInfoPtnKbn8 { get; set; }
        public string JyoSenInfoPtnCol8 { get; set; }
        public byte JyoSenInfoPtnKbn9 { get; set; }
        public string JyoSenInfoPtnCol9 { get; set; }
        public byte JyoSenInfoPtnKbn10 { get; set; }
        public string JyoSenInfoPtnCol10 { get; set; }
        public byte JyoSenInfoPtnKbn11 { get; set; }
        public string JyoSenInfoPtnCol11 { get; set; }
        public byte JyoSenInfoPtnKbn12 { get; set; }
        public string JyoSenInfoPtnCol12 { get; set; }
        public byte JyoSenInfoPtnKbn13 { get; set; }
        public string JyoSenInfoPtnCol13 { get; set; }
        public byte JyoSenInfoPtnKbn14 { get; set; }
        public string JyoSenInfoPtnCol14 { get; set; }
        public byte JyoSenInfoPtnKbn15 { get; set; }
        public string JyoSenInfoPtnCol15 { get; set; }
        public byte JyoSyaChkSiyoKbn { get; set; }
        public byte SenDayRenge { get; set; }
        public short SenDefWidth { get; set; }
        public string YykHaiSTime { get; set; }
        public string YykTouTime { get; set; }
        public byte TehaiAutoSet { get; set; }
        public byte GoSyaAutoSet { get; set; }
        public byte YoySyuKiTimeSiyoKbn { get; set; }
        public byte KarSyuKiTimeSiyoKbn { get; set; }
        public byte GuiAutoSet { get; set; }
        public byte DrvAutoSet { get; set; }
        public byte SenBackPtnKbn { get; set; }
        public string SenBackPtnCol { get; set; }
        public byte SenOBPtnKbn { get; set; }
        public string SenOBPtnCol { get; set; }
        public int FutTumCdSeq { get; set; }
        public byte ETCKinKbn { get; set; }
        public int SeisanCdSeq { get; set; }
        public int GuideFutTumCdSeq { get; set; }
        public byte SeiMuki { get; set; }
        public string ExpItem { get; set; }
        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
        public int UpdSyainCd { get; set; }
        public string UpdPrgID { get; set; }
        public byte QuotationTransfer { get; set; }


        // Custom select

        public int CompanyCdSeqCompanyCd { get; set; }
        public string CompanyCdSeqCompanyNm { get; set; }
        public string CompanyCdSeqRyakuNm { get; set; }
        public string SyainCd { get; set; }
        public string SyainNm { get; set; }
        public string UriKbnCodeKbnNm { get; set; }
        public string UriKbnRyakuNm { get; set; }
        public string SyohiHasuCodeKbnNm { get; set; }
        public string SyohiHasuRyakuNm { get; set; }
        public string TesuHasuCodeKbnNm { get; set; }
        public string TesuHasuRyakuNm { get; set; }
        public string HoukoKbnCodeKbnNm { get; set; }
        public string HoukoKbnRyakuNm { get; set; }
        public string HouZeiKbnCodeKbnNm { get; set; }
        public string HouZeiKbnRyakuNm { get; set; }
        public string JKariKbnCodeKbnNm { get; set; }
        public string JKariKbnRyakuNm { get; set; }
        public string AutKarJyunCodeKbnNm { get; set; }
        public string AutKarJyunRyakuNm { get; set; }
        public string JKBunPatCodeKbnNm { get; set; }
        public string JKBunPatRyakuNm { get; set; }
        public string SyaIrePatCodeKbnNm { get; set; }
        public string SyaIrePatRyakuNm { get; set; }
        public string JymAChkKbnCodeKbnNm { get; set; }
        public string JymAChkKbnRyakuNm { get; set; }
        public string UriHenKbnCodeKbnNm { get; set; }
        public string UriHenKbnRyakuNm { get; set; }
        public string UriMDKbnCodeKbnNm { get; set; }
        public string UriMDKbnRyakuNm { get; set; }
        public string UriZeroChkCodeKbnNm { get; set; }
        public string UriZeroChkRyakuNm { get; set; }
        public string CanKbnCodeKbnNm { get; set; }
        public string CanKbnRyakuNm { get; set; }
        public string CanMDKbnCodeKbnNm { get; set; }
        public string CanMDKbnRyakuNm { get; set; }
        public string CanWaitKbnCodeKbnNm { get; set; }
        public string CanWaitKbnRyakuNm { get; set; }
        public string CanJidoKbnCodeKbnNm { get; set; }
        public string CanJidoKbnRyakuNm { get; set; }
        public string YouTesuKbnCodeKbnNm { get; set; }
        public string YouTesuKbnRyakuNm { get; set; }
        public string YouSagaKbnCodeKbnNm { get; set; }
        public string YouSagaKbnRyakuNm { get; set; }
        public string SyaUntKbnCodeKbnNm { get; set; }
        public string SyaUntKbnRyakuNm { get; set; }
        public string ZasyuKbnCodeKbnNm { get; set; }
        public string ZasyuKbnRyakuNm { get; set; }
        public string FutSF1FlgCodeKbnNm { get; set; }
        public string FutSF1FlgRyakuNm { get; set; }
        public string FutSF2FlgCodeKbnNm { get; set; }
        public string FutSF2FlgRyakuNm { get; set; }
        public string FutSF3FlgCodeKbnNm { get; set; }
        public string FutSF3FlgRyakuNm { get; set; }
        public string FutSF4FlgCodeKbnNm { get; set; }
        public string FutSF4FlgRyakuNm { get; set; }
        public string SokoJunKbnCodeKbnNm { get; set; }
        public string SokoJunKbnRyakuNm { get; set; }
        public string UntZeiKbnCodeKbnNm { get; set; }
        public string UntZeiKbnRyakuNm { get; set; }
        public string TumZeiKbnCodeKbnNm { get; set; }
        public string TumZeiKbnRyakuNm { get; set; }
        public string GetSyoKbnCodeKbnNm { get; set; }
        public string GetSyoKbnRyakuNm { get; set; }
        public string SeiKrksKbnCodeKbnNm { get; set; }
        public string SeiKrksKbnRyakuNm { get; set; }
        public string DaySyoKbnCodeKbnNm { get; set; }
        public string DaySyoKbnRyakuNm { get; set; }
        public string KouYouSetCodeKbnNm { get; set; }
        public string KouYouSetRyakuNm { get; set; }
        public string AutKouKbnCodeKbnNm { get; set; }
        public string AutKouKbnRyakuNm { get; set; }
        public string SenYouDefFlgCodeKbnNm { get; set; }
        public string SenYouDefFlgRyakuNm { get; set; }
        public string SenMikDefFlgCodeKbnNm { get; set; }
        public string SenMikDefFlgRyakuNm { get; set; }
        public string KoteiCopyFlgCodeKbnNm { get; set; }
        public string KoteiCopyFlgRyakuNm { get; set; }
        public string FutaiCopyFlgCodeKbnNm { get; set; }
        public string FutaiCopyFlgRyakuNm { get; set; }
        public string TumiCopyFlgCodeKbnNm { get; set; }
        public string TumiCopyFlgRyakuNm { get; set; }
        public string TehaiCopyFlgCodeKbnNm { get; set; }
        public string TehaiCopyFlgRyakuNm { get; set; }
        public string JoshaCopyFlgCodeKbnNm { get; set; }
        public string JoshaCopyFlgRyakuNm { get; set; }
        public string YykCopyFlgCodeKbnNm { get; set; }
        public string YykCopyFlgRyakuNm { get; set; }
        public string UkbCopyFlgCodeKbnNm { get; set; }
        public string UkbCopyFlgRyakuNm { get; set; }
        public string SeiGenFlgCodeKbnNm { get; set; }
        public string SeiGenFlgRyakuNm { get; set; }
        public string HouOutKbnCodeKbnNm { get; set; }
        public string HouOutKbnRyakuNm { get; set; }
        public string MeiShyKbnCodeKbnNm { get; set; }
        public string MeiShyKbnRyakuNm { get; set; }
        public string SyaSenMjPtnKbn1CodeKbnNm { get; set; }
        public string SyaSenMjPtnKbn1RyakuNm { get; set; }
        public string SyaSenMjPtnKbn2CodeKbnNm { get; set; }
        public string SyaSenMjPtnKbn2RyakuNm { get; set; }
        public string SyaSenMjPtnKbn3CodeKbnNm { get; set; }
        public string SyaSenMjPtnKbn3RyakuNm { get; set; }
        public string SyaSenMjPtnKbn4CodeKbnNm { get; set; }
        public string SyaSenMjPtnKbn4RyakuNm { get; set; }
        public string SyaSenMjPtnKbn5CodeKbnNm { get; set; }
        public string SyaSenMjPtnKbn5RyakuNm { get; set; }
        public string JyoSenMjPtnKbn1CodeKbnNm { get; set; }
        public string JyoSenMjPtnKbn1RyakuNm { get; set; }
        public string JyoSenMjPtnKbn2CodeKbnNm { get; set; }
        public string JyoSenMjPtnKbn2RyakuNm { get; set; }
        public string JyoSenMjPtnKbn3CodeKbnNm { get; set; }
        public string JyoSenMjPtnKbn3RyakuNm { get; set; }
        public string JyoSenMjPtnKbn4CodeKbnNm { get; set; }
        public string JyoSenMjPtnKbn4RyakuNm { get; set; }
        public string JyoSenMjPtnKbn5CodeKbnNm { get; set; }
        public string JyoSenMjPtnKbn5RyakuNm { get; set; }
        public string JyoSyaChkSiyoKbnCodeKbnNm { get; set; }
        public string JyoSyaChkSiyoKbnRyakuNm { get; set; }
        public string GuiAutoSetCodeKbnNm { get; set; }
        public string GuiAutoSetRyakuNm { get; set; }
        public string DrvAutoSetCodeKbnNm { get; set; }
        public string DrvAutoSetRyakuNm { get; set; }
        public string SenOBPtnKbnCodeKbnNm { get; set; }
        public string SenOBPtnKbnRyakuNm { get; set; }
        public string SyaSenInfoPtnKbn1CodeKbnNm { get; set; }
        public string SyaSenInfoPtnKbn1RyakuNm { get; set; }
        public string SyaSenInfoPtnKbn2CodeKbnNm { get; set; }
        public string SyaSenInfoPtnKbn2RyakuNm { get; set; }
        public string SyaSenInfoPtnKbn3CodeKbnNm { get; set; }
        public string SyaSenInfoPtnKbn3RyakuNm { get; set; }
        public string SyaSenInfoPtnKbn4CodeKbnNm { get; set; }
        public string SyaSenInfoPtnKbn4RyakuNm { get; set; }
        public string SyaSenInfoPtnKbn5CodeKbnNm { get; set; }
        public string SyaSenInfoPtnKbn5RyakuNm { get; set; }
        public string SyaSenInfoPtnKbn6CodeKbnNm { get; set; }
        public string SyaSenInfoPtnKbn6RyakuNm { get; set; }
        public string SyaSenInfoPtnKbn7CodeKbnNm { get; set; }
        public string SyaSenInfoPtnKbn7RyakuNm { get; set; }
        public string SyaSenInfoPtnKbn8CodeKbnNm { get; set; }
        public string SyaSenInfoPtnKbn8RyakuNm { get; set; }
        public string SyaSenInfoPtnKbn9CodeKbnNm { get; set; }
        public string SyaSenInfoPtnKbn9RyakuNm { get; set; }
        public string SyaSenInfoPtnKbn10CodeKbnNm { get; set; }
        public string SyaSenInfoPtnKbn10RyakuNm { get; set; }
        public string SyaSenInfoPtnKbn11CodeKbnNm { get; set; }
        public string SyaSenInfoPtnKbn11RyakuNm { get; set; }
        public string SyaSenInfoPtnKbn12CodeKbnNm { get; set; }
        public string SyaSenInfoPtnKbn12RyakuNm { get; set; }
        public string SyaSenInfoPtnKbn13CodeKbnNm { get; set; }
        public string SyaSenInfoPtnKbn13RyakuNm { get; set; }
        public string SyaSenInfoPtnKbn14CodeKbnNm { get; set; }
        public string SyaSenInfoPtnKbn14RyakuNm { get; set; }
        public string SyaSenInfoPtnKbn15CodeKbnNm { get; set; }
        public string SyaSenInfoPtnKbn15RyakuNm { get; set; }
        public string JyoSenInfoPtnKbn1CodeKbnNm { get; set; }
        public string JyoSenInfoPtnKbn1RyakuNm { get; set; }
        public string JyoSenInfoPtnKbn2CodeKbnNm { get; set; }
        public string JyoSenInfoPtnKbn2RyakuNm { get; set; }
        public string JyoSenInfoPtnKbn3CodeKbnNm { get; set; }
        public string JyoSenInfoPtnKbn3RyakuNm { get; set; }
        public string JyoSenInfoPtnKbn4CodeKbnNm { get; set; }
        public string JyoSenInfoPtnKbn4RyakuNm { get; set; }
        public string JyoSenInfoPtnKbn5CodeKbnNm { get; set; }
        public string JyoSenInfoPtnKbn5RyakuNm { get; set; }
        public string JyoSenInfoPtnKbn6CodeKbnNm { get; set; }
        public string JyoSenInfoPtnKbn6RyakuNm { get; set; }
        public string JyoSenInfoPtnKbn7CodeKbnNm { get; set; }
        public string JyoSenInfoPtnKbn7RyakuNm { get; set; }
        public string JyoSenInfoPtnKbn8CodeKbnNm { get; set; }
        public string JyoSenInfoPtnKbn8RyakuNm { get; set; }
        public string JyoSenInfoPtnKbn9CodeKbnNm { get; set; }
        public string JyoSenInfoPtnKbn9RyakuNm { get; set; }
        public string JyoSenInfoPtnKbn10CodeKbnNm { get; set; }
        public string JyoSenInfoPtnKbn10RyakuNm { get; set; }
        public string JyoSenInfoPtnKbn11CodeKbnNm { get; set; }
        public string JyoSenInfoPtnKbn11RyakuNm { get; set; }
        public string JyoSenInfoPtnKbn12CodeKbnNm { get; set; }
        public string JyoSenInfoPtnKbn12RyakuNm { get; set; }
        public string JyoSenInfoPtnKbn13CodeKbnNm { get; set; }
        public string JyoSenInfoPtnKbn13RyakuNm { get; set; }
        public string JyoSenInfoPtnKbn14CodeKbnNm { get; set; }
        public string JyoSenInfoPtnKbn14RyakuNm { get; set; }
        public string JyoSenInfoPtnKbn15CodeKbnNm { get; set; }
        public string JyoSenInfoPtnKbn15RyakuNm { get; set; }
        public string SenBackPtnKbnCodeKbnNm { get; set; }
        public string SenBackPtnKbnRyakuNm { get; set; }
        public string ETCKinKbnCodeKbnNm { get; set; }
        public string ETCKinKbnRyakuNm { get; set; }
        public string TehaiAutoSetCodeKbnNm { get; set; }
        public string TehaiAutoSetRyakuNm { get; set; }
        public string GoSyaAutoSetCodeKbnNm { get; set; }
        public string GoSyaAutoSetRyakuNm { get; set; }
        public string YoySyuKiTimeSiyoKbnCodeKbnNm { get; set; }
        public string YoySyuKiTimeSiyoKbnRyakuNm { get; set; }
        public string KarSyuKiTimeSiyoKbnCodeKbnNm { get; set; }
        public string KarSyuKiTimeSiyoKbnRyakuNm { get; set; }
        public string SeiMukiCodeKbnNm { get; set; }
        public string SeiMukiRyakuNm { get; set; }
        public string QuotationTransferCodeKbnNm { get; set; }
        public string QuotationTransferRyakuNm { get; set; }
    }
}
