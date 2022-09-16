using HassyaAllrightCloud.Domain.Dto;


namespace HassyaAllrightCloud.Commons.Constants
{
    public static class Constants
    {
        public static class ErrorMessage
        {
            #region Booking Input error message constants
            public const string BookingTypeEmpty = "BI_T002.1";
            public const string SaleBranchEmpty = "BI_T002.2";
            public const string StaffEmpty = "BI_T002.3";
            public const string CustomerEmpty = "BI_T002.4";
            public const string BusEndDateTimeGreaterThanBusStartDateTime = "BI_T001";
            public const string BusTypeEmpty = "BI_T002.5";
            public const string UnitOfBusNotGreaterThanZero = "BI_T003.1";
            public const string DriverNotDivisibleIntoBus = "BI_T004";
            public const string DriverNotGreaterThanZero = "BI_T003.2";
            public const string UnitBusPriceNotInMinMaxRange = "BI_T005";
            public const string UnitBusFeeNotInMinMaxRange = "BI_T006";
            public const string GarageLeaveTimeLargerThanBusStartTime = "BI_T007";
            public const string DepartureStartTimeLowerThanBusStartTime = "BI_T008";
            public const string DepartureStartTimeLargerThanOrEqualBusEndTime = "BI_T010";
            public const string GarageReturnTimeLowerThanBusEndTime = "BI_T009";
            public const string MaxLengthIkNm = "BI_T011";
            public const string MaxLengthHaiSNm = "BI_T012";
            public const string MaxLengthTouNm = "BI_T013";
            public const string GarageLeaveDateError = "BI_T999";
            public const string GoStartDateError = "BI_T998";
            public const string GarageReturnDateError = "BI_T997";
            public const string MaxLengthBikoNm = "BI_T014";
            public const string CancelDateAfterOrEqualStartDate = "BI_T015";
            public const string SelectCancellationStaff = "BI_T016";
            public const string SelectReusedStaff = "BI_T017";
            public const string InputConfirmationRecipient = "BI_T018";
            public const string UkenoCdIsFull = "BI_T020";
            public const string DayBeforeAfterCannotDividedByDay = "E_T001";
            public const string PassengerEmpty = "BI_T019";
            public const string StartTimeWhenSplitOneDate = "BI_T021";
            public const string EndTimeWhenSplitOneDate = "BI_T022";
            public const string TokiskNull = "BI_TokiskNull";
            public const string TokiStNull = "BI_TokiStNull";
            public const string GyosyaNull = "BI_GyosyaNull";


            #endregion

            #region MinMax Settings error message constants
            public const string TranSportationOfficePlaceEmpty = "MX_T005";
            public const string UnitPriceNotInMinMaxRange = "MX_T001.1";
            public const string UnitFeeNotInMinMaxRange = "MX_T001.2";
            public const string BusInspectionStartDateGreaterThanEndDate = "MX_T002";
            public const string KmRunningLessThanExactKmRunning = "MX_T003";
            public const string TimeRunningLessThanExactTimeRunning = "MX_T004";
            public const string SumKmRunningOutOfRange = "MX_T006";
            public const string SumExactKmRunningOutOfRange = "MX_T007";
            #endregion

            #region BusAllocationValidator Settings error message constants
            public const string CheckStartTimeAndEndTimeCondition = "開始時間に終了時間の前を入力してください。";
            public const string CheckStartReceptionNumberAndEndReceptionNumberCondition = "開始受付番号に終了受付番号の以下を入力してください。";
            public const string CheckingTheStartAndEndReservationClassification = "開始予約区分に終了予約区分の以下を選択してください。";
            public const string CheckingTheDeliveryDateTimeAndDispatchTime = "配車日時は出庫日時の後に記入ください。";
            public const string CheckingTheDateAndtTimeOfReturnAndArrival = "帰庫日時は到着日時の後に記入ください";
            public const string CheckArrivalTimeWithDispatchTimeAndDepartureTime = "到着日時は出庫日時、配車日時の後に記入ください。";
            public const string CheckingTheReturnTimeTogetherWithTheDispatchTimeAndDepartureTime = "帰庫日時は出庫日時、配車日時の後に記入ください。";
            public const string CheckHaishaSymdandUnkobiSymd = "配車日時は運行日の配車日時以降で入力してください。";
            public const string CheckHaishaTouYmdandUnkobiTouYmd = "到着日時は運行日の到着日時以前で入力してください。";
            public const string CheckReservation = "BI_T017";
            public const string CheckSyuKoHaiShaWithSyuKoUnkoBi = "BI_T005";
            public const string CheckHaiSHaiShaWithHaiSUnkoBi = "BI_T006";
            public const string CheckTouChHaiShaWithTouChUnkoBi = "BI_T007";
            public const string CheckKikoHaiShaWithKikoUnkoBi = "BI_T008";
            public const string CheckSyuKoHaiShaWithSyuKoHaiSha = "BI_T019";
            public const string CheckKikoHaiShaWithTouChHaiSha = "BI_T020";
            public const string CheckSyuKoHaiSha = "BI_T012";
            public const string CheckHaiSHaiSha = "BI_T013";
            public const string CheckTouChHaiSha = "BI_T014";
            public const string CheckTouchHaiShaSubtractionHaiSHaiSha = "BI_T015";

            #endregion

            #region Home Error message constants
            public const string RecordNotFound = "該当するレコードがありません。";

            public const string NoticeDisplayKbnEmpty = "BI_T001";
            public const string NoticeContentEmpty = "BI_T002";

            public const string MenuLinkEmpty = "BI_T004";
            public const string MenuContentEmpty = "BI_T005";
            public const string MenuLinkMaxLength = "BI_T009";
            public const string MenuContentMaxLength = "BI_T010";

            public const string SiteLinkEmpty = "BI_T006";
            public const string SiteContentEmpty = "BI_T007";
            public const string SiteLinkMaxLength = "BI_T011";
            public const string SiteContentMaxLength = "BI_T012";

            public const string NoteContentEmpty = "BI_T008";

            #endregion

            #region Vehicle Daily Report error message constants
            public const string ScheduleYmdError = "BI_T001";
            public const string BusSaleBranchError = "BI_T002";
            public const string BusCodeError = "BI_T003";
            public const string ReceptionError = "BI_T004";
            public const string ReservationError = "BI_T005";
            public const string InputKiloError = "BI_T001";
            public const string MeterError = "BI_T003";
            public const string KoskuTimeError = "BI_T004";
            public const string JisTimeError = "BI_T005";
            #endregion

            #region REPORT Tenkokiroku
            public const string CompanyReport = "BI_T002";
            public const string BranchFromGreaterThanBranchToReport = "BI_T003";
            public const string BookingTypeFromGreaterThanBookingTypeToReport = "BI_T004";
            #endregion
            #region REPORT Tenkokiroku
            public const string CompairDate = "BI_T001";
            public const string CompairCategory = "BI_T002";
            public const string UkenoNull = "BI_T007";
            #endregion

            #region Report RepairList

            public const string StartDateTimeGreaterThanEndDateTimeReportRepair = "BI_T001";
            public const string BranchFromGreaterThanBranchToReportRepair = "BI_T002";
            public const string VehicleFromGreaterThanVehicleToReportRepair = "BI_T003";
            public const string RepairFromGreaterThanRepairToReportRepair = "BI_T004";
            #endregion
            #region Report BusTypeList
            public const string DestinationStartGreaterThanDestinationToReportBusTypeList = "BI_T004";
            public const string BranchFromGreaterThanBranchToReportBusTypeList = "BI_T0012";
            public const string BookingTypeFromGreaterThanBookingTypeToReportBusTypeList = "BI_T0015";
            public const string SaleStaffFromGreaterThanSaleStaffToReportBusTypeList = "BI_T0013";
            public const string VehicleTypeFromGreaterThanVehicleTypeToReportBusTypeList = "BI_T0011";


            #endregion


            #region BillPrint
            public const string InvalidInvoice = "BI_T001";
            public const string InvalidInvoiceYm = "BI_T002";
            public const string InvalidRcpNum = "BI_T003";
            public const string InvalidRsrCat = "BI_T004";
            public const string InvalidBillAdd = "BI_T005";
            public const string InvalidBillingOffice = "BI_T006";
            public const string InvalidIssueYmd = "BI_T007";
            public const string BillPrintNoData = "BI_T008";
            #endregion

            #region Booking input multi copy
            public const string CopyCustomerEmpty = "BI_T001";
            public const string CopyDestinationEmpty = "BI_T002";
            public const string CopyDispatchPlaceEmpty = "BI_T003";
            public const string CopyArrivePlaceEmpty = "BI_T004";
            public const string CopyJourneyTimeInvalid = "BI_T005";
            #endregion

            #region Booking Incidental
            public const string CannotDeleteIncidental = "BI_T001";
            public const string NotSelectedCustomerIncidental = "BI_T002";
            public const string NotMatchQuantityBusIncidental = "BI_T004";
            public const string NotSelectedIncidentalCode = "BI_T005";
            public const string NotSelectedLoadingCode = "BI_T006";
            public const string NotSelectedClearingCode = "BI_T007";
            #endregion

            #region Loan Booking Incidental
            public const string NotSelectedLoanIncidentalCode = "BI_T001";
            public const string NotSelectedYTsumiRequired = "BI_T008";
            public const string NotSelectedLoanClearingCode = "BI_T002";
            public const string NotMatchQuantityBusLoanIncidental = "BI_T007";
            public const string NotSelectedQuantity = "BI_T009";
            #endregion

            #region Booking Arrangement
            public const string NotselectedLocationName = "BI_T003";
            public const string NotInputTehaiName = "BI_T002";
            #endregion

            #region LockBooking
            public const string NotSelectedBranchToLock = "BI_T003";
            #endregion

            #region Report form  Buscoordination 
            public const string StartDateTimeGreaterThanEndDateTimeReport = "BI_T001";
            public const string BookingTypeToGreaterThanBookingTypeFromReportBusCoordina = "BookingTypeError";
            public const string CustomerToGreaterThanCustomerFromReport = "BI_T003";
            public const string CustomerTo01GreaterThanCustomerFrom01Report = "BI_T010";
            public const string BookingToGreaterThanBookingFromReport = "BI_T011";
            #endregion

            #region Transport Daily Report
            public const string UnkoDateError = "BI_T001";
            public const string SyuKbnError = "BI_T002";
            public const string EigyoError = "BI_T003";
            #endregion

            #region Status Confirmation constants

            public const string SC_StartDateLargerThanEndDate = "BI_T001";
            //public const string SC_EndDateEqualToStartDate = "BI_T002";
            public const string SC_BranchStartLargerThanBranchEnd = "BI_T002";
            public const string SC_CustomerStartLargerThanCustomerEnd = "BI_T005";
            public const string SC_CsvSeparatorIsEmpty = "BI_T0013";

            #endregion

            #region Advance Payment Details error message constants
            public const string BillingDesError = "BI_T001";
            #endregion

            #region AnnualTransportationRecord
            public const string AnnualTransportation_BI_T001 = "BI_T001";
            public const string AnnualTransportation_BI_T002 = "BI_T002";
            public const string AnnualTransportation_BI_T003 = "BI_T003";
            public const string AnnualTransportation_BI_T004 = "BI_T004";
            public const string AnnualTransportation_BI_T005 = "BI_T005";
            public const string AnnualTransportation_BI_T006 = "BI_T006";
            #endregion

            #region Vender Request constants

            public const string VR_UkeCdIsWrong = "BI_T002";
            public const string VR_ReservationIsWrong = "BI_T003";
            public const string VR_DateIsWrong = "BI_T004";
            public const string VR_CustomerIsWrong = "BI_T005";

            #endregion

            #region CancelList constants
            public const string CL_BookingTypeEmpty = "BI_T005";
            public const string CL_CompanyEmpty = "BI_T0014";
            public const string CL_CancelBookingTypeEmpty = "BI_T0011";
            public const string CL_BranchEmpty = "BI_T007";
            public const string CL_StaffEmpty = "BI_T008";
            public const string CL_CustomerOrSupplierEmpty = "BI_T006";
            public const string CL_EndDateEarlyThanStartDate = "BI_T001";
            public const string CL_UkeToLessThanUkeFrom = "BI_T004";
            public const string CL_BranchFromLargerThanBranchTo = "BI_T0012";
            public const string CL_BookingTypeFromLargerThanBookingTypeTo = "BI_T0015";
            public const string CL_StaffFromLargerThanStaffTo = "BI_T0013";
            public const string CL_CustomerFromLargerThanCustomerTo = "BI_T002";
            public const string CL_SupplierFromLargerThanSupplierTo = "BI_T003";
            public const string CL_CsvSeparatorIsEmpty = "BI_T009";
            #endregion

            #region DepositCoupon
            public const string InvalidDepositCouponBillPeriod = "BI_T001";
            public const string InvalidDepositCouponBillAddress = "BI_T002";
            public const string InvalidDepositCouponReservationClassification = "BI_T003";
            public const string InvalidDepositCoupon4 = "BI_T004";
            public const string InvalidDepositCouponNoData = "BI_T005";
            public const string InvalidDepositCoupon6 = "BI_T006";
            #endregion

            #region DepositCouponPayment
            public const string DCPDepositAmount = "BI_T002";
            public const string DCPCardApprovalNumber = "BI_T003";
            public const string DCPCardSlipNumber = "BI_T004";
            public const string DCPBillNo = "BI_T005";
            public const string DCPCouponNo = "BI_T006";
            public const string DCPFaceValue = "BI_T007";
            public const string DCPApplicationAmount = "BI_T008";
            public const string DCPValidApplicationAmount = "BI_T009";
            public const string DCPBI_T010 = "BI_T010";
            public const string DCPZeroApplicationAmount = "DCPZeroApplicationAmount";
            #endregion

            #region InvoiceIssueRelease
            public const string IIRBillIssueDate = "BI_T001";
            public const string IIRBillAddress = "BI_T002";
            public const string IIRNoData = "BI_T003";
            #endregion

            #region ETC
            public const string ETC_UsageYmdError = "UsageYmdError";
            public const string ETC_FutaiError = "FutaiError";
            public const string ETC_SeisanError = "SeisanError";
            public const string ETC_BI_T002 = "BI_T002";
            public const string ETC_BI_T003 = "BI_T003";
            public const string ETC_BI_T004 = "BI_T004";
            #endregion

            #region ReceiptOutput
            public const string ReceiptOutput_BI_T001 = "BI_T001";
            public const string ReceiptOutput_BI_T002 = "BI_T002";
            public const string ReceiptOutput_BI_T003 = "BI_T003";
            public const string ReceiptOutput_BI_T004 = "BI_T004";
            public const string ReceiptOutput_BI_T005 = "BI_T005";
            public const string ReceiptOutput_BI_T006 = "BI_T006";
            #endregion

            #region FaresUpperAndLowerLimits
            public const string FaresUpperAndLowerLimits_BI_T001 = "BI_T001";
            public const string FaresUpperAndLowerLimits_BI_T002 = "BI_T002";
            public const string FaresUpperAndLowerLimits_BI_T003 = "BI_T003";
            public const string FaresUpperAndLowerLimits_BI_T004 = "BI_T004";
            #endregion

            #region FareFeeCorrection
            public const string FareFeeCorrection_BI_T001 = "BI_T001";
            public const string FareFeeCorrection_BI_T002 = "BI_T002";
            public const string FareFeeCorrection_BI_T003 = "BI_T003";
            public const string FareFeeCorrection_BI_T004 = "BI_T004";
            public const string FareFeeCorrection_BI_T005 = "BI_T005";
            public const string FareFeeCorrection_BI_T006 = "BI_T006";
            public const string FareFeeCorrection_BI_T007 = "BI_T007";
            public const string FareFeeCorrection_BI_T008 = "BI_T008";
            #endregion

            #region AccessoryFeeList constants
            public const string AFL_BookingTypeEmpty = "BI_T005";
            public const string AFL_CompanyEmpty = "BI_T0014";
            public const string AFL_CancelBookingTypeEmpty = "BI_T0011";
            public const string AFL_BranchEmpty = "BI_T007";
            public const string AFL_CustomerEmpty = "BI_T006";
            public const string AFL_FutaiEmpty = "BI_T008";
            public const string AFL_EndDateEarlyThanStartDate = "BI_T001";
            public const string AFL_UkeToLessThanUkeFrom = "BI_T004";
            public const string AFL_BranchFromLargerThanBranchTo = "BI_T0012";
            public const string AFL_CustomerFromLargerThanCustomerTo = "BI_T002";
            public const string AFL_FutaiFromLargerThanFutaiTo = "BI_T0013";
            public const string AFL_CsvSeparatorIsEmpty = "BI_T009";
            public const string AFL_BookingTypeFromLargerThanBookingTypeTo = "BI_T015";
            #endregion

            public const string SAVEFAIL = "SaveFail";

            #region VehicleStatisticsSurvey
            public const string VehicleStatisticsSurvey_BI_T001 = "BI_T001";
            public const string VehicleStatisticsSurvey_BI_T002 = "BI_T002";
            #endregion

            #region DailyBatchCopy
            public const string DAILYBATCHCOPY_BI_T001 = "BI_T001";
            #endregion

            #region staffchart
            public const string Departurefromto = "BI_T007";
            public const string Arrivalfromto = "BI_T009";
            public const string Deliveryfromto = "BI_T008";
            public const string Returnfromto = "BI_T011";
            #endregion

            #region CustomField 
            public const string FieldEmpty = " を入力してください！"; //TODO change into error message code
            #endregion

            #region TransportationContract
            public const string EndDateGreaterThanStartDate = "BI_T001";
            #endregion

            #region EditReservationMobile
            public const string EditReservationMobile_BI_T001 = "BI_T001";
            public const string EditReservationMobile_BI_T002 = "BI_T002";
            public const string EditReservationMobile_BI_T003 = "BI_T003";
            public const string EditReservationMobile_BI_T004 = "BI_T004";
            public const string EditReservationMobile_BI_T007 = "BI_T007";
            #endregion

            #region SimpleQuotation
            public const string SQ_DateEndGreaterThanStart = "BI_T001";
            public const string SQ_CustomerEndGreaterThanStart = "BI_T002";
            public const string SQ_UkeCdEndGreaterThanStart = "BI_T004";
            public const string SQ_BranchEndGreaterThanStart = "BI_T009";
            public const string SQ_BookingTypeEndGreaterThanStart = "BI_T011";

            public const string SQ_BookingTypeGetErrorOrEmpty = "BI_T005";
            public const string SQ_CustomerGetErrorOrEmpty = "BI_T006";
            public const string SQ_BranchGetErrorOrEmpty = "BI_T007";
            public const string SQ_SubmitGetErrorOrEmpty = "BI_T008";
            #endregion

            #region QuotationWithJourney
            public const string QWJ_DateEndGreaterThanStart = "BI_T001";
            public const string QWJ_CustomerEndGreaterThanStart = "BI_T002";
            public const string QWJ_UkeCdEndGreaterThanStart = "BI_T004";
            public const string QWJ_BranchEndGreaterThanStart = "BI_T009";
            public const string QWJ_BookingTypeEndGreaterThanStart = "BI_T011";

            public const string QWJ_BookingTypeGetErrorOrEmpty = "BI_T005";
            public const string QWJ_CustomerGetErrorOrEmpty = "BI_T006";
            public const string QWJ_BranchGetErrorOrEmpty = "BI_T007";
            public const string QWJ_SubmitGetErrorOrEmpty = "BI_T008";
            #endregion

            #region SubContractorStatus
            public const string SCS_EndDateEarlyThanStartDate = "BI_T001";
            public const string SCS_CustomerFromLargerThanCustomerTo = "BI_T002";
            public const string SCS_UkeToLessThanUkeFrom = "BI_T003";
            public const string SCS_BookingTypeFromLargerThanBookingTypeTo = "BI_T013";
            public const string SCS_BranchFromLargerThanBranchTo = "BI_T004";
            public const string SCS_StaffFromLargerThanStaffTo = "BI_T005";
            public const string SCS_CompanyIsEmpty = "BI_T016";
            public const string SCS_CompanyIsRequired = "BI_T017";
            public const string SCS_CsvSeparatorIsEmpty = "BI_T018";
            #endregion
        }
        public static readonly TaxTypeList ForeignTax = new TaxTypeList { IdValue = 1, StringValue = "外税" };
        public static readonly TaxTypeList InTax = new TaxTypeList { IdValue = 2, StringValue = "内税" };
        public static readonly TaxTypeList NoTax = new TaxTypeList { IdValue = 3, StringValue = "非課税" };
        public static readonly byte SiyoKbn = 1;
        public static readonly int CompanyCdSeq = 1;
        public static readonly byte NoticeDisplayKbnAll = 1;
        public static readonly string BookingStatus = "01";
        public static readonly string CancelBookingStatus = "02";
        public static readonly string EstimateStatus = "03";
        public static readonly string CancelEstimateStatus = "04";
        public static readonly string WhiteColor = "ffffff";
        public static readonly string SubStoredFolderForBookingInput = "受付番号";

        public static readonly int CountryCdSeq = 2;
        /// <summary>
        /// Define maximum of km running in <see cref="Domain.Dto.MinMaxSettingFormData"/>
        /// </summary>
        public static int MaximumOfKmRunning = 9999;

        /// <summary>
        /// Define maximum of rate can be input.
        /// </summary>
        public const float MaximumOfRate = 99.9f;
        /// <summary>
        /// Define minimum of rate can be input.
        /// </summary>
        public const float MinimumOfRate = 0f;
        /// <summary>
        /// Define text for selected all options
        /// </summary>
        public static string SelectedAll = "すべて";

        public static readonly int SyainCdSeq = 1;
        //public static readonly byte NoticeDisplayKbnAll = 1;
        //public static readonly int CompanyCdSeq = 1;
        public static readonly string CodeKbBIKOCD = "BIKOCD";
        public static readonly string ETCUpdPrgId = "KO0300P";
        public static readonly string ETCTransferUpdPrgIdKJ1300P = "KJ1300P";
        public static readonly string ETCTransferUpdPrgIdPS7000P = "PS7000P";
        public static readonly string ETCTransferUpdPrgIdKK9210P = "KK9210P";
        public static readonly string FareFeeCorrectionPrgId = "KJ9200P";
        public struct Url
        {
            public const string MySetting = "MySetting";
        }

        public class BatchKobanInputConstants
        {
            public static readonly string CarAttend = "車輌点呼順";
            public static readonly string EmployeeAttend = "社員点呼順";
            public static readonly string EmployeeOrderCode = "社員コード順";
            public static readonly string OfficeOrder = "営業所コード順";
            public static readonly string TwoDay = "２日";
            public static readonly string FourteenDay = "１４日";
            public static readonly string ThirtyDay = "３１日";
        }

        public class BillIssuedClassificationCheckListConstants
        {
            public static readonly ComboboxFixField All = new ComboboxFixField { IdValue = 0, StringValue = "" };
            public static readonly ComboboxFixField Paid = new ComboboxFixField { IdValue = 1, StringValue = "" };
            public static readonly ComboboxFixField Unpaid = new ComboboxFixField { IdValue = 2, StringValue = "" };
        }
        public class BillTypeSortConstants
        {
            public static readonly ComboboxFixField BillOrder = new ComboboxFixField { IdValue = 0, StringValue = "" };
            public static readonly ComboboxFixField OrderByDay = new ComboboxFixField { IdValue = 1, StringValue = "" };
        }
        public class ReceivableUnpaidConstants
        {
            public static readonly ComboboxFixField Field0 = new ComboboxFixField { IdValue = 0, StringValue = "" };
            public static readonly ComboboxFixField Field1 = new ComboboxFixField { IdValue = 1, StringValue = "" };
            public static readonly ComboboxFixField Field2 = new ComboboxFixField { IdValue = 2, StringValue = "" };
            public static readonly ComboboxFixField Field3 = new ComboboxFixField { IdValue = 3, StringValue = "" };
        }
        public class BillTypePageConstants
        {
            public static readonly ComboboxFixField A4 = new ComboboxFixField { IdValue = 0, StringValue = "" };
            public static readonly ComboboxFixField A3 = new ComboboxFixField { IdValue = 1, StringValue = "" };
            public static readonly ComboboxFixField B4 = new ComboboxFixField { IdValue = 2, StringValue = "" };
        }
        public class DelimiterTypeOption
        {
            public static readonly ComboboxFixField Tab = new ComboboxFixField { IdValue = 0, StringValue = "" };
            public static readonly ComboboxFixField Semicolon = new ComboboxFixField { IdValue = 1, StringValue = "" };
            public static readonly ComboboxFixField Comma = new ComboboxFixField { IdValue = 2, StringValue = "" };
            public static readonly ComboboxFixField Space = new ComboboxFixField { IdValue = 3, StringValue = "" };
            public static readonly ComboboxFixField FixedLength = new ComboboxFixField { IdValue = 4, StringValue = "" };
            public static readonly ComboboxFixField OtherCharacters = new ComboboxFixField { IdValue = 5, StringValue = "" };
        }
        public class ShowHeaderOption
        {
            public static readonly ComboboxFixField OutWHeader = new ComboboxFixField { IdValue = 0, StringValue = "" };
            public static readonly ComboboxFixField OutNotHeader = new ComboboxFixField { IdValue = 1, StringValue = "" };
        }
        public class GroupTypeOption
        {
            public static readonly ComboboxFixField Active = new ComboboxFixField { IdValue = 0, StringValue = "" };
            public static readonly ComboboxFixField No = new ComboboxFixField { IdValue = 1, StringValue = "" };
        }
    }

    public class DepositList
    {
        public static readonly string PaymentOffice = "請求営業所";
        public static readonly string ReceprtionOffice = "受付営業所";
        public static readonly string Depositoffice = "入金営業所";
    }

    public class FormFilterName
    {
        public static readonly string SuperMenuReservation = "KJ5000F";
        public static readonly string SuperMenuVehicle = "KU5000F";
        public static readonly string HyperMenu = "HyperMenu";
        public static readonly string BillCheckList = "KS1700";
        public static readonly string InvoiceIssueRelease = "KS6000F";
        public static readonly string MonthlyTransportationResult = "KG1100";
        public static readonly string BillPrint = "KS2000";
        public static readonly string TransportationContract = "KJ5700";
        public static readonly string AdvancePaymentDetails = "KO0500F";
        public static readonly string FareFeeCorrection = "KJ9200P";
        public static readonly string AnnualTransportationRecord = "KG1200";
        public static readonly string ReceiptOutput = "KS2000";
        public static readonly string FaresUpperAndLowerLimits = "KPC0020";
        public static readonly string VehicleAvailabilityConfirmationMobile = "VAC0001";
		public static readonly string Staff = "KU0800";
		public static readonly string TransportDailyReport = "KU4200";
		public static readonly string ReceivableList = "KS1500";
		public static readonly string KashikiriSetting = "KM0000P";
        public static readonly string DepositList = "KS1400";
        public static readonly string LeaveManagement = "KU6000";
        public static readonly string BillingList = "KS1800F";
		public const string CouponPayment = "KS0300";
        public const string RevenueSummary = "KS1100";
        public const string TransportationSummary = "KG1000";
        public const string EtcImport = "KO0200";
        public const string EtcList = "KO0300";
        public const string TransportActualResult = "KG1600";
        public const string AttendanceReport = "KU5100";
        public const string AvailabilityCheck = "AvailabilityCheck";
        public static readonly string DepositCoupon = "KS0100F";
		public static readonly string VehicleStatisticsSurvey = "KG1300";
        public static readonly string DailyBatchCopy = "KJ3400";    
        public static readonly string StaffSchedule = "KU6100";
        public static readonly string ConpanyId = "CompanyId";
        public static readonly string Calendar = "Calendar";
        public static readonly string DateCommnet = "DateComment";
        public static readonly string BatchKobanInput = "KUH800";
        public static readonly string VehicleDailyReport = "KU1900";
    }

    public static class ScreenCode
    {
        public static readonly string CouponPaymentUpdPrgId = "KK2300P";
        public static readonly string CouponMultiPaymentUpdPrgId = "KK2400P";

        public const string PartnerBookingInputUpdPrgId = "KU1700";
    }

    public class CommonConstants
    {
        public const string FormatUpdateDbDate = "yyyyMMdd";
        public const string FormatUpdateDbTime = "HHmmss";

        public const string FormatYMD = "yyyyMMdd";
        public const string FormatYMDWithSlash = "yyyy/MM/dd（ddd）";
        public const string FormatYMDWithSlashFull = "yyyy/MM/dd dddd";
        public const string FormatHMS = "HHmmss";
        public const string FormatYMDHm = "yyyy/MM/dd HH:mm";
        public const string DefaultTime = "0000";
        public const string DefaultHHmmss = "000000";
        public const string Format2YMD = "yyMMdd";
        public const string FormatYMDHMSNoSeparated = "yyyyMMddHHmmss";
        public static readonly string leaveType = "leave";
        public static readonly string planType = "plan";
        public static readonly string jouneyType = "journey";
        public static readonly string currentScheduleview = "timelineWeek";
        public const int SiyoKbn = 1;
        public const string Format2YMDWithDOW = "yy/MM/dd（ddd）";
        public static int[] listPage = new int[] { 10, 25, 50, 100 };
        public const int pageNumber = 10;
        public const int pageSize = 10;
        public const string Format2YMDWithSlash = "yy/MM/dd";
        public const string FormatHms = "HHmmss";
        public const string FormatYMDHmWithoutSlash = "yyyyMMdd HHmm";
        public const string FormatYM = "yyyyMM";
        public const string DANTAICD = "DANTAICD";
        public const string KATAKBN = "KATAKBN";
    }
    public class StaffScheduleConstants
    {
        public const string Pending = "承認待ち";
        public const string Accept = "承認";
        public const string Refuse = "却下";
        public const string Day = "(終日)";
        public const string AcceptSt = "2:承認";
        public const string RefusetSt = "3:却下";
        public const string PendingSt = "1:承認待ち";
        public const string NoteName = "ShoRejBiko";
        public const string IsLeaving = "leaving";
        public const string IsWorking = "working";
        public const string To = "～";
        public const string Work = "勤務";
        public const string Holiday = "休日";
        public const string Board = "乗務";
        public const string BirthDay = "誕生日";
        public const string DataComment = "コメント";
        public const string HavePlan = "予定あり";
        public const string Absolute = "絶対";
        public const string Hope = "希望";
    }
    public class DepositListContants
    {
        public const string PaymentOfficeType = "請求営業所";
        public const string ReceptionOfficeType = "受付営業所";
        public const string DepositOfficeType = "入金営業所";
    }
    public class ScheduleYoteiType
    {
        public const string ScheduleMeeting = "schedulemeeting";
        public const string ScheduleLeave = "scheduleleave";
        public const string ScheduleJourney = "schedulejourney";
        public const string Trainning = "trainning";
    }
    public class FormatString
    {
        public const string FormatNumber = "#,##0";
        public const string FormatDecimalOnePlace = "#,##0.0";
        public const string FormatDecimalTwoPlace = "#,##0.00";
        public static readonly string yoteiType = "YOTEITYPE";
    }

    public class TokenChars
    {
        public const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    }

    public class BaseNamespace
    {
        public const string Report = "HassyaAllrightCloud.Reports.";
        public const string AdvancePaymentDetailsReportTemplate = "AdvancePaymentDetailsReportTemplate";
        public const string VehicleDailyReportTemplate = "VehicleDailyReportTemplate";
        public const string JitHouReport = "JitHouReport";
		public const string TransportDailyReportTemplate = "TransportDailyReport";
        public const string TransportContract = "HikiukeshoReport";
        public const string Buscoordination = "Busutehaisyo";
        public const string Venderrequestform = "YouShaIrai";
        public const string Busreports = "Haisyahyo";
        public const string Attendanceconfirmreport = "Tenkokiroku";
        public const string Attendancereport = "AttendanceReport";
        public const string Billprint = "PaymentRequestPreviewReport";
        public const string Receiptoutput = "ReceiptOutputReport";
        public const string NoReceiptoutput = "NoReceiptOutputReport";
        public const string JomukirokuboReport = "JomukirokuboReport";
        public const string UnkoushijishoReport = "UnkoushijishoReport";
        public const string UnkoushijishoBaseReport = "UnkoushijishoBaseReport";
    }

    public class PageSizeName
    {
        public const string A4 = "A4";
        public const string A3 = "A3";
        public const string B4 = "B4";
    }

    public static class BusCssClass
    {
        public const string SimezumiColor = "Simezumi-color";
        public const string NippozumiColor = "Nippozumi-color";
        public const string HaiinzumiColor = "Haiinzumi-color";
        public const string HaishazumiColor = "Haishazumi-color";
        public const string YoushazumiColor = "Youshazumi-color";
        public const string MikarishaColor = "Mikarisha-color";
        public const string KakuteiColor = "Kakutei-color";
        public const string KakuteichuColor = "Kakuteichu-color";
        public const string KarishaColor = "Karisha-color";
    }

    public class SuperMenuType1GridHeaderNameConstants
    {
        public const string GridName = "grdList";
        public const string No = "NO";
        public const string GridMark = "記号";
        public const string GridCustomer = "得意先";
        public const string GridGroup = "団体名";
        public const string GridDispatch = "配車";
        public const string GridBusType = "車種/台数";
        public const string GridFare = "運賃";
        public const string GridMaxAmount = "上限(料金)";
        public const string GridGuideFee = "ガイド料";
        public const string GridOtherCharge = "その他付帯";
        public const string GridYousyaNumber = "傭車台数";
        public const string GridYousyaFare = "傭車運賃";
        public const string GridYousyaGuideFee = "傭車ガイド料";
        public const string GridYousyaOtherCharge = "その他付帯";
        public const string GridBoardingPerson = "乗車人員";
        public const string GridBillingCategory = "請求区分";
        public const string GridBillingExportDate = "請求書出力年月日";
        public const string GridUnsoHikiukeshoExportDate = "運送引受書出力年月日";
        public const string GridRegistrationOffice = "受付営業所";
        public const string GridReservationClassification = "予約区分";
        public const string InputDisplay = "入力表示";

        public const string NoItemNm = "No";
        public const string MarkItemNm = "Mark";
        public const string CustomerItemNm = "Customer";
        public const string OrganizationItemNm = "Organization";
        public const string DispatchItemNm = "Dispatch";
        public const string CarTypeItemNm = "CarType";
        public const string FareItemNm = "Fare";
        public const string LimitItemNm = "Limit";
        public const string GuideFeeItemNm = "GuideFee";
        public const string IncidentalItemNm = "Incidental";
        public const string BusQuantityItemNm = "BusQuantity";
        public const string BusFareItemNm = "BusFare";
        public const string BusGuildFeeItemNm = "BusGuildFee";
        public const string BusIncidentalItemNm = "BusIncidental";
        public const string BusPersonItemNm = "BusPerson";
        public const string BillItemNm = "Bill";
        public const string BillOutputDateItemNm = "BillOutputDate";
        public const string DeliveryReceiptOutputDateItemNm = "DeliveryReceiptOutputDate";
        public const string ReceptionItemNm = "Reception";
        public const string ReservationItemNm = "Reservation";
        public const string InputDisplayItemNm = "InputDisplay";

    }
    public class BusAllocationGridGridHeaderNameConstants
    {
        public const string GridName = "grdBusAllocationLst";
        public const string no_colItemNm = "no_col";
        public const string receipt_numberItemNm = "receipt_number";
        public const string carItemNm = "car";
        public const string sales_officeItemNm = "sales_office";
        public const string car_number_modelItemNm = "car_number_model";
        public const string group_name_2ItemNm = "group_name_2";
        public const string departure_return_datetimeItemNm = "departure_return_datetime";
        public const string delivery_arrival_timeItemNm = "delivery_arrival_time";
        public const string depot_arrivalItemNm = "depot/arrival";
        public const string connection_dispatch_arrivalItemNm = "connection_dispatch/arrival";
        public const string tax_included_fareItemNm = "tax-included_fare"; 
        public const string fareItemNm = "fare"; 
        public const string sale_taxItemNm = "sale_tax";
        public const string feeItemNm = "fee";
        public const string depot_arrival_addressItemNm = "depot_arrival_address";
        public const string riding_plus_personnelItemNm = "riding_plus_personnel";
        public const string other_personnelItemNm = "other_personnel";
        public const string crew1ItemNm = "crew1";
        public const string crew2ItemNm = "crew2";
        public const string crew3ItemNm = "crew3";
        public const string destination_nameItemNm = "destination_name";
        public const string plate_noItemNm = "plate_no";
    }
    public class PartnerBookingInputGridHeaderNameConstants
    {
        public const string GridName = "grdPBookingInputLst";
        public const string no_colItemNm = "no_col";
        public const string Car = "Car";
        public const string MercenaryType = "MercenaryType";
        public const string SalesOffice = "SalesOffice";
        public const string MercenaryName = "MercenaryName";
        public const string Vehicle = "Vehicle";        
        public const string CarType = "CarType";
        public const string OrganizationName = "OrganizationName";
        public const string departure_return_datetimeItemNm = "departure_return_datetime";
        public const string Issue_Return = "Issue/Return";
        public const string Dispatch_Arrival = "Dispatch/Arrival";
        public const string Depot_Arrival = "Depot/Arrival";
        public const string FaresIncludingTax = "FaresIncludingTax";
        public const string Fare = "Fare";
        public const string ConsumptionTax = "ConsumptionTax";
        public const string FeeAmount = "FeeAmount";
        public const string Connection = "Connection";
        public const string DepotAddress = "DepotAddress";
        public const string BoardingPersonnel = "BoardingPersonnel";
        public const string PlusPersonnel = "PlusPersonnel";
        public const string OtherPersonnel = "OtherPersonnel";
        public const string DestinationName = "DestinationName";
       
    }


    public class StatusConfirmationGridHeaderNameConstants
    {
        public const string GridName = "StatusConfigGrid";
        public const string NoItemNm = "No";
        public const string TokuiSakiItemNm = "TokuiSaki";
        public const string ConfirmedTimeItemNm = "ConfirmedTime";
        public const string ConfirmedPersonItemNm = "ConfirmedPerson";
        public const string SaikouItemNm = "Saikou";
        public const string SumDaiItemNm = "SumDai";
        public const string AmmountItemNm = "Ammount";
        public const string ScheduleDateItemNm = "ScheduleDate";
        public const string DanTaiNameItemNm = "DanTaiName";
        public const string HaishaInfoItemNm = "HaishaInfo";
        public const string BusTypeInfoItemNm = "BusTypeInfo";
        public const string BusFeeItemNm = "BusFee";
        public const string GuideFeeItemNm = "GuideFee";
        public const string BranchOptionsItemNm = "BranchOptions";
        public const string BookingTypeItemNm = "BookingType";
        public const string GuideItemNm = "Guide";
        public const string KoteiItemNm = "Kotei";
        public const string TsuMiItemNm = "TsuMi";
        public const string TehaiItemNm = "Tehai";
        public const string FutaiItemNm = "Futai";
        public const string TokuiStaffItemNm = "TokuiStaff";
        public const string ConfirmedYmdItemNm = "ConfirmedYmd";
        public const string ConfirmedByItemNm = "ConfirmedBy";
        public const string KanjiNameItemNm = "KanjiName";
        public const string TouInfoItemNm = "TouInfo";
        public const string BusNameItemNm = "BusName";
        public const string BusTypeItemNm = "BusType";
        public const string DaisuItemNm = "Daisu";
        public const string ConsumptionTaxItemNm = "ConsumptionTax";
        public const string ConsumptionTaxGuideItemNm = "ConsumptionTaxGuide";
        public const string GuideTaxItemNm = "GuideTax";
        public const string ReceivedByItemNm = "ReceivedBy";
        public const string ReceivedYmdItemNm = "ReceivedYmd";
        public const string ShiireSakiItemNm = "ShiireSaki";
        public const string FixedYmdItemNm = "FixedYmd";
        public const string NoteContentItemNm = "NoteContent";
        public const string DestinationNameItemNm = "DestinationName";
        public const string PassengerInfoItemNm = "PassengerInfo";
        public const string FeeAmountItemNm = "FeeAmount";
        public const string FeeAmountGuideItemNm = "FeeAmountGuide";
        public const string GuideChargeItemNm = "GuideCharge";
        public const string InputByItemNm = "InputBy";
        public const string BookingNoItemNm = "BookingNo";
    }
    public class CancelListGridHeaderNameConstants
    {
        public const string GridName = "CancelListGrid";
        public const string no_colItemNm = "no_col";
        public const string customer_nameItemNm = "customer_name";
        public const string cancellation_dateItemNm = "cancellation_date";
        public const string booking_nameItemNm = "booking_name";
        public const string processing_divisionItemNm = "processing_division";
        public const string faresalesItemNm = "faresales";
        public const string CancelChargeItemNm = "CancelCharge";
        public const string vehicle_dispatchItemNm = "vehicle_dispatch";
        public const string organization_nameItemNm = "organization_name";
        public const string vehicle_typeItemNm = "vehicle_type";
        public const string number_of_unitsItemNm = "number_of_units";
        public const string boarding_personnelItemNm = "boarding_personnel";
        public const string invoiceItemNm = "invoice";
        public const string reception_committee_sales_officeItemNm = "reception_committee_sales_office";
        public const string BookingTypetypeItemNm = "BookingTypetype";
        public const string customer_contactItemNm = "customer_contact";
        public const string sales_officeItemNm = "sales_office";
        public const string reason_cancelItemNm = "reason_cancel";
        public const string consumption_taxItemNm = "consumption_tax";
        public const string salesItemNm = "sales";
        public const string secretary_nameItemNm = "secretary_name";
        public const string plus_peopleItemNm = "plus_people";
        public const string issue_dateItemNm = "issue_date";
        public const string StaffItemNm = "Staff";
        public const string UkeYmdItemNm = "UkeYmd";
        public const string supplier_nameItemNm = "supplier_name";
        public const string person_in_chargeItemNm = "person_in_charge";
        public const string stremptyItemNm = "strempty";
        public const string feeItemNm = "fee";
        public const string consumption_tax1ItemNm = "consumption_tax1";
        public const string destination_nameItemNm = "destination_name";
        public const string strempty1ItemNm = "strempty1";
        public const string strempty2ItemNm = "strempty2";
        public const string input_personItemNm = "input_person";
        public const string UkeCdItemNm = "UkeCd";
        public const string nulltextItemNm = "nulltext";
        public const string nulltext1ItemNm = "nulltext1";
        public const string nulltext2ItemNm = "nulltext2";

    }

    public class SubContractorStatusGridHeaderNameConstants
    {
        public const string GridName = "SubContractorStatus";
        public const string NoItemNm = "No";
        public const string AllServiceSchedule = "AllServiceSchedule";
        public const string Tokui = "Tokui";
        public const string Staff = "Staff";
        public const string Tel = "Tel";
        public const string DanTaNm = "DanTaNm";
        public const string IkNm = "IkNm";
        public const string SumDai = "SumDai";
        public const string SyaRyoUnc = "SyaRyoUnc";
        public const string Zeiritsu = "Zeiritsu";
        public const string SyaRyoSyo = "SyaRyoSyo";
        public const string TesuRitu = "TesuRitu";
        public const string SyaRyoTes = "SyaRyoTes";
        public const string FutTumGuiKin = "FutTumGuiKin";
        public const string FutTumGuiTax = "FutTumGuiTax";
        public const string FutTumGuiTes = "FutTumGuiTes";
        public const string FutTumKin = "FutTumKin";
        public const string FutTumTax = "FutTumTax";
        public const string FutTumTes = "FutTumTes";
        public const string CarDestination = "CarDestination";
        public const string TokuiNm = "TokuiNm";
        public const string SitenNm = "SitenNm";
        public const string Gyosya = "Gyosya";
        public const string HaiSInfo = "HaiSInfo";
        public const string TouChInfo = "TouChInfo";
        public const string YoushaUnc = "YoushaUnc";
        public const string YouZeiritsu = "YouZeiritsu";
        public const string YouTesuRitu = "YouTesuRitu";
        public const string YouFutTumGuiKin = "YouFutTumGuiKin";
        public const string YouFutTumGuiTax = "YouFutTumGuiTax";
        public const string YouFutTumGuiTes = "YouFutTumGuiTes";
        public const string YouFutTumKin = "YouFutTumKin";
        public const string YouFutTumTax = "YouFutTumTax";
        public const string YouFutTumTes = "YouFutTumTes";
        public const string JyoSyaJin = "JyoSyaJin";
        public const string PlusJin = "PlusJin";
        public const string UkeEigyosRyaku = "UkeEigyosRyaku";
        public const string YoyaKbn = "YoyaKbn";
        public const string UkeYmd = "UkeYmd";
    }
    public class CommonProgramId
    {
        public const string BookingUpdPrgId = "KJ5000F";
    }

    public class TransportDailyGridConstants
    {
        public const string GridName = "TransportGrid";
        public const string GridNameTotal = "TransportGridTotal";
        public const string GridNameTotal1 = "TransportGridTotal1";

        public const string No = "No";
        public const string CarNum = "CarNum";
        public const string Capacity = "Capacity";
        public const string Customer = "Customer";
        public const string Name = "Name";
        public const string DayAndNight = "DayAndNight";
        public const string Issue = "Issue";
        public const string Return = "Return";
        public const string FareFee = "FareFee";
        public const string NetIncome = "NetIncome";
        public const string Personel = "Personel";
        public const string Actual = "Actual";
        public const string Forwarding = "Forwarding";
        public const string Other = "Other";
        public const string Total = "Total";
        public const string Fuel1 = "Fuel1";
        public const string Fuel2 = "Fuel2";
        public const string Fuel3 = "Fuel3";
        public const string Crew = "Crew";

        public const string TotalList = "Total";
        public const string FareTotal = "FareTotal";
        public const string FeeTotal = "FeeTotal";
        public const string NetIncomeTotal = "NetIncomeTotal";
        public const string PersonelTotal = "PersonelTotal";
        public const string ActualTotal = "ActualTotal";
        public const string ForwardingTotal = "ForwardingTotal";
        public const string OtherKmTotal = "OtherKmTotal";
        public const string TotalKmTotal = "TotalKmTotal";
        public const string Fuel1Total = "Fuel1Total";
        public const string Fuel2Total = "Fuel2Total";
        public const string Fuel3Total = "Fuel3Total";

        public const string NumberOfVehicle = "NumberOfVehicle";
        public const string GroupBreakDown = "GroupBreakDown";
        public const string NumberOfService = "NumberOfService";
        public const string Reality = "Reality";
        public const string NumberOfActualVehicle = "NumberOfActualVehicle";
        public const string TempIncrease = "TempIncrease";
        public const string Vehicle = "Vehicle";
        public const string NumberOfGroup = "NumberOfGroup";
        public const string HeadOffice = "HeadOffice";
        public const string Mediator = "Mediator";
        public const string NumberOfTrip = "NumberOfTrip";
        public const string HeadOffice1 = "HeadOffice1";
        public const string Mediator1 = "Mediator1";
        public const string OutGoing = "OutGoing";
        public const string Night = "Night";
    }

    public class SubContractorGridConstants
    {

    }
}
