namespace HassyaAllrightCloud.Commons.Constants
{

    public enum BusLineMode
    {
        View = 1,
        Edit = 2,
        Create = 3,
        Cut = 4,
        Repair = 5,
        Zoom = 6,
    }
    public enum ViewMode
    {
        Large = 1,
        Medium = 2,
        Small = 3,
    }

    public enum ModeChangeV
    {
        ViewMode = 1,
        LayoutMode = 2,
        OutputInstruction = 3
    }
    public enum LayoutMode
    {
        Save = 1,
        Init = 2,
    }
    public enum ReceiableGridMode
    {
        Detail = 1,
        BusinessPlan = 2
    }
    public enum TypeBillTotal
    {

        Choice = 1,
        PageTotal = 2,
        Cumulative = 3,
        All = 4,
    }
    public enum GroupMode
    {
        All = 1,
        Branch = 2,
        Company = 3,
    }
    public enum BusRunModeReport
    {
        HiGaeRi = 1,
        TomariDe = 2,
        HaKuChu = 3,
        HaKuKi = 4,
        YaKoDe = 5,
        YaKoKi = 6,
        YaKoChu = 7
    }
    public enum ShowMode
    {
        Screen = 1,
        Report = 2
    }
    public enum DayMode
    {
        OneDay = 1,
        ThreeDays = 2,
        Week = 3,
        Month = 4,
    }
    public enum TimeMode
    {
        Minute = 0,
        OneHour = 1,
        ThreeHours = 2,
        SixHours = 3,
        Day = 4,
    }
    public enum LineDrawMode
    {
        Normal = 1,
        RentalBus = 2,
        SpareBus = 3,
        All = 4,
    }
    public enum SortVehicleLineMode
    {
        Model_Branch = 1,
        Model_Vehicle = 2,
        Branch_Vehicle = 3,
        Branch_Model = 4,
        Rollcall = 5,
    }
    public enum SortVehicleNameMode
    {
        Model_Vehicle = 1,
        Branch_Model = 2,
        Branch_Temporary_Vehicle = 3,
        Branch_Vehicle = 4,
    }
    public enum DisplayLineMode
    {
        Custom = 1,
        Organization = 2,
        Destination = 3,
        Customer = 4,
        None = 5,
    }

    public enum ZoomMode
    {
        Zoom1d15 = 1,
        Zoom1d1h = 2,
        Zoom1d3h = 3,
        Zoom1d6h = 4,
        Zoom3d3h = 5,
        Zoom3d6h = 6,
        Zoom1w1d = 7,
        Zoom1m1d = 8,
    }
    // Use for Transportation Screen
    public enum PrintMode
    {
        Preview = 1,
        Print = 2,
        SaveAsExcel = 3,
        SaveAsPDF = 4,
    }

    public enum OutputUnit
    {
        EachBooking = 1,
        EachBusTypeBooking = 2,
    }

    public enum DateTypeContract
    {
        Dispatch = 1,
        Arrival = 2,
        Reservation = 3,
        Reception = 4,
    }

    public enum OutputSelection
    {
        All = 0,
        OnlyNotOutputAlready = 1,
    }

    public enum YearlyContract
    {
        Output = 0,
        NotOutput = 1
    }

    public enum StaffSortOrder
    {
        Earlier = 0,
        Rolling = 1,
        Work = 2,
        Job = 3
    }

    public enum StaffDisplay1
    {
        Assigned = 0,
        Employee = 1,
    }

    public enum StaffCrewSortOrder
    {
        EmployeeCodeOrder = 0,
        PreviousDayEndTime = 1,
        AscendingTimeForOneWeek = 2,
        FourWeeksLessTime = 3,
    }

    public enum StaffWorkSortOrder
    {
        Earlier = 0,
        Time = 1
    }

    public enum StaffDisplay2
    {
        Driver = 0,
        GuideDriver = 1,
        DriverInHouse = 2,
    }

    public enum Flag
    {
        Todo,
        NotTodo,
        Absense,
        Holiday,
        Work,
        Job,
    }

    public enum RepeatTypes
    {
        // 毎時 
        Hourly = 0,
        // 毎日
        Daily = 1,
        // 毎週
        Weekly = 2,
        // 毎月
        Monthly = 3,
        // 毎年
        Yearly = 4

    }

    public enum SuperMenyTypeDisplay
    {
        Reservation = 1,
        Vehicle = 2
    }

    public enum GraphTypeDisplay
    {
        GraphSaleStaffDayBar = 1,
        GraphSaleStaffMonthBar = 2,
        GraphCustomerDayBar = 3,
        GraphCustomerMonthBar = 4,
        GraphOrganizationBar = 5,
        GraphOrganizationPie = 6,
        GraphSaleDayLine = 7,
        GraphSaleMonthLine = 8,
        GraphSaleQuanDayBarLine = 9,
        GraphSaleQuanMonthBarLine = 10
    }

    public enum DateType
    {
        Dispatch = 0,
        Arrival = 1,
        Reservation = 2,
        Cancellation,
        VehicleDelivery,
        NextDay,
        Occurrence
    }

    public enum SuperMenuColorPattern
    {
        Payment = 1,
        Support = 2,
        Deposit = 3,
        Enter = 4,
        DailyReport = 5,
        Mercenary = 6,
        Dispatch = 7,
        Manning = 8,
        Allocation = 9,
        Confirmed = 10,
        Confirmation = 11,
        TemporaryBus = 12,
        TemporaryDistribution = 13,
        Unprovisioned = 14
    }
    public enum BillCheckListColorPattern
    {
        Deposited = 1,
        Coupon = 2,
        Some = 3,
        PartialEntry = 4,
        OverPayment = 5,
        NotPayment = 6,
    }
    public enum BillCheckListTypeTotalTable
    {
        Selector = 1,
        PageTotal = 2,
        Cumulative = 3,
    }

    public enum GraphPattern
    {
        LineSale = 0,
        BarSalePerStaff = 1,
        BarSalePerCustomer = 2,
        BarSalePerGroupClassification = 3,
        PieSalePerGroupClassification = 4,
        BarLineSaleQuan = 5,
    }

    public enum SiyoKbn
    {
        Usable = 1,
        NotUsable = 2
    }
    public enum OptionReport
    {
        Preview = 1,
        Download = 2,
        None = 3
    }
    public enum ConfirmStatus
    {
        NotFixed,
        Confirm,
        Fixed,
        Confirmed,
        UnConfirmed,
        Yes,
        No,
        Unknown
    }
    public enum NumberOfConfirmed
    {
        Unknown,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Other
    }

    public enum CallBackTabBookingState
    {
        False,
        True,
        FixedConfirm,
        UpdateBooking,
        DisableIndex,
        EnableIndex,
        UpdateConfirmTab,
        UpdateCancelTab,
        HaitaError
    }
    public enum MinMaxSettingState
    {
        None,
        ViewOnly
    }

    public enum BillIssuedClassification
    {
        All,
        Paid,
        Unpaid
    }
    public enum BillType
    {
        Fare,
        Incidental,
        TollFee,
        ArrangementFee,
        GuideFee,
        ShippingCharge,
        CancellationCharge
    }

    public enum Staffs_Type
    {
        Normal = 1,
        Unassigned = 2,
    }
    public enum Staffs_OrderbyList
    {
        DutyEmployee = 1,
        BranchEmployee = 2,
        BranchJob = 3,
        Rolling = 4,
    }

    public enum Staffs_OrderbyName
    {
        DutyEmployee = 1,
        BranchDuty = 2,
        BranchEmployee = 3,
    }

    public enum Staffs_Duties
    {
        All = 1,
        Driver = 2,
        Guide = 3,
        OfficeWork = 4,
    }

    public enum PaperSize
    {
        A4 = 1,
        A3 = 2,
        B4 = 3,
    }

    public enum SortCancel
    {
        Customer,
        CancellationDate,
        VehicleDeliveryDate
    }

    public enum PaymentRequestPrintMode
    {
        Print = 1,
        IndependPrint = 2,
        MultiPrint = 3,
        BillNumberChosenPrint = 4,
        Pdf = 5,
        Preview = 6
    }
    public enum OutputReportType
    {
        Preview,
        Print,
        ExportPdf,
        CSV,
        Excel
    }

    public enum IncidentalViewMode
    {
        All = 0,
        Futai = 1,
        Tsumi = 2,
    }

    public enum CSV_Group
    {
        /// <summary>
        /// Group by symbol: "
        /// </summary>
        QuotationMarks,
        WithoutQuotes,
        Empty,
    }

    public enum CSV_Header
    {
        IncludeHeader,
        WithoutHeader,
    }

    public enum CSV_Delimiter
    {
        Comma,
        Semicolon,
        Space,
        FixedLength,
        Other,
        Tab
    }

    public enum ViewBookingInput
    {
        All = 0,
        Futai = 1,
        Tsumi = 2,
    }

    public enum BookingMultiCopyType
    {
        Reservation,
        Vehicle
    }

    public enum OutputInstruction
    {
        Show = 1,
        Preview = 2,
        Pdf = 3,
        Print = 4,
        Csv = 5
    }

    public enum KataKbn
    {
        Big = 1,
        Medium = 2,
        Small = 3
    }

    public enum NenRyoKbn
    {
        LightOil = 1,
        Gasoline = 2,
        LPG = 3,
        GasTurbine = 4,
        Other = 5,
    }

    public enum ModeTab
    {
        Tab1Big = 1,
        Tab1Medium = 2,
        Tab1Small = 3,
        Tab2Big = 4,
        Tab2Medium = 5,
        Tab2Small = 6,
    }

    public enum RoundSettings
    {
        Ceiling = 1,
        Floor = 2,
        Round = 3
    }

    public enum RoundTaxAmountType
    {
        RoundUp = 1,
        Truncate = 2,
        Rounding = 3
    }
    public enum ModeNamesUnassigned
    {
        Big = 15,
        Medium = 20,
        Small = 26,
    }
    public enum NotificationResultClassification
    {
        Success = 1,
        Failed = 2
    }

    public enum NotificationSendMethod
    {
        Both = 0,
        Mail = 1,
        Line = 2
    }

    public enum CodeKbnForNotification
    {
        Application = 21
    }

    public enum NotificationContentKbn
    {
        Subject = 1,
        Content = 2
    }

    public enum BusAllocationSortOrder
    {
        DeliveryVehicle,
        DeliveryLadder,
        Vehicle,
        Rollcall,
        Delivery,
        Ladder,
    }

    public enum BookingDisableEditState
    {
        PaidOrCoupon,
        Locked
    }

    public enum FormEditState
    {
        None,
        Added,
        Edited,
        Deleted,
    }

    public enum ReportIdForSetting
    {
        // 立替明細書
        AdvancePaymentDetails = 1,
        VehicleDaily = 2,
        JitHouReport = 19,
		TransportDaily = 6,
        TransportContract = 12,
        Buscoordination = 23,
        Venderrequestform = 24,
        Busreports = 25,
        Attendanceconfirmreport = 26,
        Attendancereport = 13,
        Billprint = 3,
        Receiptoutput = 19,
        Operatinginstructionreport = 27
    }

    public enum FieldType
    {
        Text = 1,
        Number = 2,
        Date = 4,
        Time = 5,
        Combobox = 3,
        RadioButton = 6,
        Checkbox = 7,
    }

    public enum MyPopupIconType
    {
        Info,
        Warning,
        Error
    }


    public enum DateClassification
    {
        BackToGarageDate = 1,
        DipatchDate = 2,
        ArrivalDate = 3
    }

    public enum PlanningScope
    {
        OutOfRange = 1,
        InRange = 2
    }

    public enum ItemOutOfRange
    {
        BothOfItem = 1,
        RunningKilomet = 2,
        TotalTime = 3
    }

    public enum CauseInput
    {
        Input = 1,
        NotInput = 2
    }
    public enum ChooseCause
    {
        AllOfCause = 1,
        NeedEstimateAgain = 2,
        OtherCause = 3
    }

    public enum ModeChange
    {
        Range = 1,
        CauseInput = 2,
    }

    public enum MessageBoxType
    {
        Info,
        Warning,
        Error,
        Confirm
    }

    public enum ConfirmAction
    {
        Yes,
        No,
        OK,
        Cancel,
        All,
        None
    }

    public enum BreakReportPage
    {
        Customer,
        None
    }

    public enum ReportType
    {
        Detail,
        Summary,
    }

    public enum InvoiceTypeOption
    {
        Liquidate,
        NotSolve,
        All,
    }

    public enum PositionTooltip
    {
        top,
        bottom,
        left,
        right
    }

    public enum OutputOrientation
    {
        Horizontal,
        Vertical
    }

    public enum OwnCompanyType
    {
        Output,
        NoOutput
    }
    public enum GroupDivision
    {
        All,
        ParentCompanies,
        SubsidiaryCompanies
    }

    public enum PageBreak
    {
        Yes,
        No
    }

    public enum ContractorOutputOrder
    {
        Dispatch,
        Arrival
    }

    public enum ViewModeBusType
    {
        BusTypeNormal = 1,
        BusUnAsign = 2,
        BusHiring = 3,
        SumVehicle = 4,
        SumEmployee = 5
    }
    public enum BusMode
    {
        BusUnAsign = 1,
        BusHiring = 2,
        BusMormal=3,
        TableHeader = 4
    }
    public enum SumMode
    {
        SumBusUnAsign = 1,
        SumBusHiring = 2,
        SumBusNormal = 3,
        SumBusDriver = 4,
        SumBusGuiSu = 5,

    }

    public enum StaffPos
    {
        Driver = 1,
        GuiSu = 2
    }
    public enum OutPutWhere
    {
        UI_T01,
        UI_T02,
        UI_CA01,
        UI_CA02,
        UI_Co01,
        UI_Co02,
    }

public enum QuotationReportType
    {
        Simple,
        JourneyHorizontal,
        JourneyVertical
    }

    public enum ShowTime
    {
        IsShowTime,
        IsNotShowTime
    }
}
