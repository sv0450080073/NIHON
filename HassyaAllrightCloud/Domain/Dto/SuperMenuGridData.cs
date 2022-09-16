using System;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class BusInfo
    {
        // 車種
        public string BusType { get; set; }
        // 台数
        public int NumberOfBuses { get; set; }

    }
    public class SuperMenuReservationData
    {
        public int No { get; set; }
        public string UkeNo { get; set; }
        public int UnkRen { get; set; }
        public string EmptyString { get; set; }

        // 記号
        public string[] Symbol { get; set; }

        public string SymbolInString { get; set; }
        public string Y0 { get; set; }

        // 得意先
        public string Customer { get; set; }
        // 支店名
        public string Branch { get; set; }
        // 担当者
        public string PersonInCharge { get; set; }
        // 団体名
        public string Organization { get; set; }
        // 幹事名
        public string Secretary { get; set; }
        // 行先名
        public string Destination { get; set; }
        // 配車
        public string Dispatch { get; set; }
        // 到着
        public string Arrival { get; set; }
        // 運転手数
        public int NumberOfDrivers { get; set; }
        // ガイド数
        public int NumberOfGuides { get; set; }
        public string NumberOfDriverNumberOfGuideString { get; set; }
        // 車種 / 台数
        public List<BusInfo> BusesInfo { get; set; }
        // 運賃
        public int Fare { get; set; }
        // 税率
        public decimal TaxRate { get; set; }
        //
        public decimal FeeTaxRate { get; set; }
        // 消費税
        public int FareTax { get; set; }
        // 手数料
        public int FareFee { get; set; }
        // 自社運賃
        public int CompanyFare { get; set; }
        // 自社消費税
        public int CompanyFareTax { get; set; }
        // 自社手数料
        public int CompanyFareFee { get; set; }
        // 上限(料金)
        public int MaxFee { get; set; }
        // 下限(料金)
        public int MinFee { get; set; }
        // 指数
        public decimal UnitPriceIndex { get; set; }
        // ガイド料
        public int Guide { get; set; }
        // 消費税
        public int GuideTax { get; set; }
        // 手数料
        public int GuideFee { get; set; }
        // その他付帯
        public int Other { get; set; }
        // 消費税
        public int OtherTax { get; set; }
        // 手数料
        public int OtherFee { get; set; }
        // 傭車台数
        public int NumberOfHiredBus { get; set; }
        public string NumberOfHiredBusInString { get; set; }
        // 傭車運賃
        public int HiredBusFare { get; set; }
        // 消費税
        public int HiredBusFareTax { get; set; }
        // 手数料
        public int HiredBusFareFee { get; set; }
        // 傭車ガイド料
        public int HiredBusGuide { get; set; }
        // 消費税
        public int HiredBusGuideTax { get; set; }
        // 手数料
        public int HiredBusGuideFee { get; set; }
        // その他付帯
        public int HiredBusOther { get; set; }
        // 消費税
        public int HiredBusOtherTax { get; set; }
        // 手数料
        public int HiredBusOtherFee { get; set; }
        // 乗車人員
        public int Person { get; set; }
        public string PersonInString { get; set; }
        // プラス人員
        public int PersonPlus { get; set; }
        public string PersonPlusString { get; set; }
        // 請求区分>
        public string BillClassification { get; set; }
        // 請求年月日
        public string BillDate { get; set; }
        // 請求書出力年月日
        public string BillOutputDate { get; set; }
        // 運送引受書出力年月日
        public string DeliveryReceiptOutputDate { get; set; }
        // 受付営業所
        public string ReceptOffice { get; set; }
        // 営業担当
        public string SalesStaff { get; set; }
        // 入力担当
        public string InputStaff { get; set; }
        // 予約区分
        public string ReserveClassification { get; set; }
        // 受付日
        public string ReceptionDate { get; set; }
        // 受付番号
        public string ReceiptNumber { get; set; }
        // 行程
        public bool Journey { get; set; }
        public string JourneyString { get; set; }
        // 積込
        public bool Load { get; set; }
        public string LoadString { get; set; }
        // 手配
        public bool Arrange { get; set; }
        public string ArrangeString { get; set; }
        // 備考
        public bool Remarks { get; set; }
        public string RemarksString { get; set; }
        // 付帯
        public bool Incidental { get; set; }
        public string IncidentalString { get; set; }
        // 行のテキストの色の管理のためのプロパティ
        public byte SihKbn { get; set; }
        public byte SCouKbn { get; set; }
        public byte NyuKinKbn { get; set; }
        public byte NCouKbn { get; set; }
        public byte NippoKbn { get; set; }
        public byte YouKbn { get; set; }
        public byte HaiSKbn { get; set; }
        public byte HaiIKbn { get; set; }
        public short GuiWNin { get; set; }
        public string KaktYmd { get; set; }
        public byte KaknKais { get; set; }
        public byte KSKbn { get; set; }
        public byte KHinKbn { get; set; }
        public string HaiSYmd { get; set; }
        // Add some item for report
        public int DepartureFlag { get; set; }
        public int BusNumberFlag { get; set; }
        public int AmountFlag { get; set; }
        public int ScheduleFlag { get; set; }
        public byte ConfirmationCount { get; set; }
        public string ConfirmFlag { get; set; }
        public string Supplier { get; set; }
        public string KanjTel { get; set; }
        public string YykshoUpdYmdTime { get; set; }
        public string UnkobiFileUpdYmdTime { get; set; }
        public SuperMenuReservationData()
        {

        }
        public SuperMenuReservationData(SuperMenuReservationData data)
        {
            UkeNo = data.UkeNo;
            UnkRen = data.UnkRen;
            Symbol = data.Symbol;
            Customer = data.Customer;
            Branch = data.Branch;
            PersonInCharge = data.PersonInCharge;
            Organization = data.Organization;
            Secretary = data.Secretary;
            Destination = data.Destination;
            Dispatch = data.Dispatch;
            Arrival = data.Arrival;
            NumberOfDrivers = data.NumberOfDrivers;
            NumberOfGuides = data.NumberOfGuides;
            BusesInfo = data.BusesInfo;
            Fare = data.Fare;
            TaxRate = data.TaxRate;
            FeeTaxRate = data.FeeTaxRate;
            FareTax = data.FareTax;
            FareFee = data.FareFee;
            CompanyFare = data.CompanyFare;
            CompanyFareTax = data.CompanyFareTax;
            CompanyFareFee = data.CompanyFareFee;
            MaxFee = data.MaxFee;
            MinFee = data.MinFee;
            Guide = data.Guide;
            GuideTax = data.GuideTax;
            GuideFee = data.GuideFee;
            Other = data.Other;
            OtherTax = data.OtherTax;
            OtherFee = data.OtherFee;
            NumberOfHiredBus = data.NumberOfHiredBus;
            HiredBusFare = data.HiredBusFare;
            HiredBusFareTax = data.HiredBusFareTax;
            HiredBusFareFee = data.HiredBusFareFee;
            HiredBusGuide = data.HiredBusGuide;
            HiredBusGuideTax = data.HiredBusGuideTax;
            HiredBusGuideFee = data.HiredBusGuideFee;
            HiredBusOther = data.HiredBusOther;
            HiredBusOtherTax = data.HiredBusOtherTax;
            HiredBusOtherFee = data.HiredBusOtherFee;
            Person = data.Person;
            PersonPlus = data.PersonPlus;
            BillClassification = data.BillClassification;
            BillDate = data.BillDate;
            BillOutputDate = data.BillOutputDate;
            DeliveryReceiptOutputDate = data.DeliveryReceiptOutputDate;
            ReceptOffice = data.ReceptOffice;
            SalesStaff = data.SalesStaff;
            InputStaff = data.InputStaff;
            ReserveClassification = data.ReserveClassification;
            ReceptionDate = data.ReceptionDate;
            ReceiptNumber = data.ReceiptNumber;
            Journey = data.Journey;
            Load = data.Load;
            Arrange = data.Arrange;
            Remarks = data.Remarks;
            Incidental = data.Incidental;
            SihKbn = data.SihKbn;
            NyuKinKbn = data.NyuKinKbn;
            NCouKbn = data.NCouKbn;
            NippoKbn = data.NippoKbn;
            YouKbn = data.YouKbn;
            HaiSKbn = data.HaiSKbn;
            HaiIKbn = data.HaiIKbn;
            GuiWNin = data.GuiWNin;
            KaktYmd = data.KaktYmd;
            KaknKais = data.KaknKais;
            KSKbn = data.KSKbn;
            KHinKbn = data.KHinKbn;
            HaiSYmd = data.HaiSYmd;
            this.YykshoUpdYmdTime = data.YykshoUpdYmdTime;
            this.UnkobiFileUpdYmdTime = data.UnkobiFileUpdYmdTime;
    }
    }
    public class SuperMenuReservationTotalData
    {
        // 運賃
        public long? Fare { get; set; }
        // 消費税
        public long? FareTax { get; set; }
        // 税込額
        public long? TaxIncluded { get; set; }
        // 手数料
        public long? FareFee { get; set; }
        // ガイド料
        public long? Guide { get; set; }
        // ガイド消費税
        public long? GuideTax { get; set; }
        // ガイド手数料
        public long? GuideFee { get; set; }
        // 付帯料金
        public long? Other { get; set; }
        // 付帯消費税
        public long? OtherTax { get; set; }
        // 付帯手数料
        public long? OtherFee { get; set; }
        // 損益
        public long? ProfitLoss { get; set; }
    }
    public class SuperMenuReservationTotalGridData
    {
        // 受注額
        public SuperMenuReservationTotalData Order { get; set; }
        // 自社
        public SuperMenuReservationTotalData Company { get; set; }
        // 傭車
        public SuperMenuReservationTotalData Bus { get; set; }

        public SuperMenuReservationTotalGridData()
        {
            Order = new SuperMenuReservationTotalData();
            Company = new SuperMenuReservationTotalData();
            Bus = new SuperMenuReservationTotalData();
        }
    }
    public class SalePerTime
    {
        public DateTime Time { get; set; }
        public long Sale { get; set; }
        public int Count { get; set; }
    }
    public class SalePerStaff
    {
        public int StaffSeq { get; set; }
        public string StaffCd { get; set; }
        public string StaffName { get; set; }
        public long Sale { get; set; }
        public DateTime Time { get; set; }
    }
    public class SalePerCustomer
    {
        public int CustomerSeq { get; set; }
        public short CustomerCd { get; set; }
        public string CustomerName { get; set; }
        public int BranchSeq { get; set; }
        public short BranchCd { get; set; }
        public string BranchName { get; set; }
        public int GyosyaCdSeq { get; set; }
        public short GyosyaCd { get; set; }
        public long Sale { get; set; }
        public DateTime Time { get; set; }
        public string Key => $"{BranchCd:0000}{CustomerCd:0000}";
    }
    public class SalePerGroupClassification
    {
        public string GroupClassificationCd { get; set; }
        public string GroupClassificationName { get; set; }
        public long Sale { get; set; }
        public DateTime Time { get; set; }
    }
    public class HyperGraphData
    {
        // 得意先ＳＥＱ
        public int TokuiSeq { get; set; }
        // 得意先コード
        public short TokuiCd { get; set; }
        // 得意先略名
        public string TokuiRyakuNm { get; set; }
        // 支店コードＳＥＱ
        public int SitenCdSeq { get; set; }
        // 支店コード
        public short SitenCd { get; set; }
        // 支店略名
        public string SitenRyakuNm { get; set; }
        // 運賃
        public int SyaRyoUnc { get; set; }
        // 営業担当者コードＳＥＱ
        public int EigTanCdSeq { get; set; }
        // 営業担当者社員コード
        public string EigTanSyainCd { get; set; }
        // 営業担当者社員名
        public string EigTanSyainNm { get; set; }
        // 団体区分
        public string DantaiKbn { get; set; }
        // 団体区分略名
        public string DantaiKbnRyakuNm { get; set; }
        // 配車年月日
        public DateTime? HaiSYmd { get; set; }
        // 到着年月日
        public DateTime? TouYmd { get; set; }
        // 受付年月日
        public DateTime? UkeYmd { get; set; }
        public int GyosyaCdSeq { get; set; }
        public short GyosyaCd { get; set; }

    }
    public class ReservationDataToCheck
    {
        public string ReceiptNumber { get; set; }
        public int CompanySeq { get; set; }
        public byte NippoKbn { get; set; }
        public bool DepositClassificationStatus { get; set; }
        public bool PaymentClassificationStatus { get; set; }
        public bool ClosedStatus { get; set; }
        public string HaiSymd { get; set; }
    }
    public class UkeCount
    {
        public string UkeNo { get; set; }
        public int UkeNoCount { get; set; }
    }
    public class SuperMenuReservationReportPDF
    {
        public List<SuperMenuReservationData> ListData { get; set; }
        public int PageNumber { get; set; }
        public string CurrentDate { get; set; }
        public string OutputType { get; set; }
        public string DispatchDateFrom { get; set; }
        public string DispatchDateTo { get; set; }
        public string ArrivalDateFrom { get; set; }
        public string ArrivalDateTo { get; set; }
        public string ReceiptDateFrom { get; set; }
        public string ReceiptDateTo { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
    }
    public class SuperMenuReservationCsv
    {
        public string UkeNo { get; set; }
        public short UnkRen { get; set; }
        public byte YoyaKbn { get; set; }
        public string YoyaKbnNm { get; set; }
        public string UkeYmd { get; set; }
        public byte KaknKais { get; set; }
        public string KaktYmd { get; set; }
        public short TokGyosyaCd { get; set; }
        public short TokCd { get; set; }
        public short SitenCd { get; set; }
        public string TokGyosyaNm { get; set; }
        public string TokNm { get; set; }
        public string SitenNm { get; set; }
        public string TokRyakuNm { get; set; }
        public string SitenRyakuNm { get; set; }
        public string TokuiTanNm { get; set; }
        public string TokuiTel { get; set; }
        public string TokuiFax { get; set; }
        public string TokuiMail { get; set; }
        public short SirGyosyaCd { get; set; }
        public short SirCd { get; set; }
        public short SirSitenCd { get; set; }
        public string SirGyosyaNm { get; set; }
        public string SirNm { get; set; }
        public string SirSitenNm { get; set; }
        public string SirRyakuNm { get; set; }
        public string SirSitenRyakuNm { get; set; }
        public string DanTaNm { get; set; }
        public string KanJNm { get; set; }
        public string KanjJyus1 { get; set; }
        public string KanjJyus2 { get; set; }
        public string KanjTel { get; set; }
        public string KanjFax { get; set; }
        public string KanjKeiNo { get; set; }
        public string KanjMail { get; set; }
        public byte KanDMHFlg { get; set; }
        public string IkNm { get; set; }
        public string HaiSYmd { get; set; }
        public string HaiSTime { get; set; }
        public string HaiSBunCd { get; set; }
        public string HaiSBunNm { get; set; }
        public string HaiSBunRyakuNm { get; set; }
        public string HaiSCd { get; set; }
        public string HaiSNm { get; set; }
        public string TouYmd { get; set; }
        public string TouChTime { get; set; }
        public string TouChaBunCd { get; set; }
        public string TouChaBunNm { get; set; }
        public string TouChaBunRyakuNm { get; set; }
        public string TouChaCd { get; set; }
        public string TouNm { get; set; }
        public string SyuPaTime { get; set; }
        public short DrvJin { get; set; }
        public short GuiSu { get; set; }
        public short SyaSyuCd { get; set; }
        public string SyaSyuNm { get; set; }
        public byte KataKbn { get; set; }
        public string KataKbnRyakuNm { get; set; }
        public short SyaSyuDai { get; set; }
        public int SyaRyoUnc { get; set; }
        public int SyaRyoSyo { get; set; }
        public int SyaRyoTes { get; set; }
        public long Gui_UriGakKin_S { get; set; }
        public long Gui_SyaRyoSyo_S { get; set; }
        public long Gui_SyaRyoTes_S { get; set; }
        public long Oth_UriGakKin_S { get; set; }
        public long Oth_SyaRyoSyo_S { get; set; }
        public long Oth_SyaRyoTes_S { get; set; }
        public int YoushaUnc { get; set; }
        public int YouDai { get; set; }
        public int YoushaSyo { get; set; }
        public int YoushaTes { get; set; }
        public long YGui_HaseiKin_S { get; set; }
        public long YGui_SyaRyoSyo_S { get; set; }
        public long YGui_SyaRyoTes_S { get; set; }
        public long YOth_HaseiKin_S { get; set; }
        public long YOth_SyaRyoSyo_S { get; set; }
        public long YOth_SyaRyoTes_S { get; set; }
        public short JyoSyaJin { get; set; }
        public short PlusJin { get; set; }
        public string SeiKyuKbn { get; set; }
        public string SeiKyuKbnRyakuNm { get; set; }
        public string SeiTaiYmd { get; set; }
        public int UkeEigCd { get; set; }
        public string UkeEigNm { get; set; }
        public string UkeEigRyakuNm { get; set; }
        public string EigTanSyainCd { get; set; }
        public string EigTanSyainNm { get; set; }
        public string InputTanSyainCd { get; set; }
        public string InputTanSyainNm { get; set; }
        public byte UkeJyKbn { get; set; }
        public byte UnkoJKbn { get; set; }
        public byte SijJoKbn1 { get; set; }
        public byte SijJoKbn2 { get; set; }
        public byte SijJoKbn3 { get; set; }
        public byte SijJoKbn4 { get; set; }
        public byte SijJoKbn5 { get; set; }
        public string HasKenNm { get; set; }
        public string HasMapCd { get; set; }
        public string HasNm { get; set; }
        public string AreaKenNm { get; set; }
        public string AreaMapCd { get; set; }
        public string AreaNm { get; set; }
        public string DantaiCd { get; set; }
        public string DantaiCdNm { get; set; }
        public byte JyoKyakuCd { get; set; }
        public string JyoKyakuNm { get; set; }
        public string BikoNm { get; set; }
        public string YRep_AllSokoTime { get; set; }
        public string YRep_CheckTime { get; set; }
        public string YRep_AdjustTime { get; set; }
        public string YRep_ShinSoTime { get; set; }
        public decimal YRep_AllSokoKm { get; set; }
        public string YRep_JiSaTime { get; set; }
        public decimal YRep_JiSaKm { get; set; }
        public byte YRep_WaribikiKbn { get; set; }
        public string YRep_ChangeKoskTime { get; set; }
        public string YRep_ChangeShinTime { get; set; }
        public decimal YRep_ChangeSokoKm { get; set; }
        public byte YRep_ChangeFlg { get; set; }
        public byte YRep_SpecialFlg { get; set; }
        public int FareMaxAmount { get; set; }
        public int FareMinAmount { get; set; }
        public int FeeMaxAmount { get; set; }
        public int FeeMinAmount { get; set; }
        public int UnitPriceMaxAmount { get; set; }
        public int UnitPriceMinAmount { get; set; }
        public decimal UnitPriceIndex { get; set; }
        public int UExp_JituKm { get; set; }
        public int UExp_SouTotalKm { get; set; }
        public string UExp_JituTime { get; set; }
        public string UExp_SumTime { get; set; }
        public string UExp_ShinSoTime { get; set; }
        public string UExp_ChangeFlg { get; set; }
        public string UExp_SpecialFlg { get; set; }
        public string UExp_YearContractFlg { get; set; }
        public SuperMenuReservationCsv()
        {

        }
    }

    // Supper menu vehicle class model
    public class SuperMenuVehicleData
    {
        // NO
        public int No { get; set; }
        public string UkeNo { get; set; }
        // 記号
        public string[] Symbol { get; set; }
        // 得意先
        public string Customer { get; set; }
        // 支店名
        public string Branch { get; set; }
        // 担当者
        public string PersonInCharge { get; set; }
        // 団体名
        public string Organization { get; set; }
        // 団体名2
        public string Organization2 { get; set; }
        // 行先名
        public string Destination { get; set; }
        // 配車
        public string Dispatch { get; set; }
        // 到着
        public string Arrival { get; set; }
        // 運転手数
        public int NumberOfDrivers { get; set; }
        // ガイド数
        public int NumberOfGuides { get; set; }
        // 営業所
        public string OfficeAddress { get; set; }
        // 車号
        public string BusName { get; set; }
        // 号車
        public string BusNo { get; set; }
        // 乗務員名
        public string Crew { get; set; }
        // 運賃
        public int Fare { get; set; }
        // 消費税
        public int FareTax { get; set; }
        // 手数料
        public int FareFee { get; set; }
        // ガイド料
        public long Guide { get; set; }
        // 消費税
        public long GuideTax { get; set; }
        // 手数料
        public long GuideFee { get; set; }
        // その他付帯
        public long Other { get; set; }
        // 消費税
        public long OtherTax { get; set; }
        // 手数料
        public long OtherFee { get; set; }
        // 乗車人員
        public int Person { get; set; }
        // プラス人員
        public int PersonPlus { get; set; }
        // 出庫
        public string ExitingDate { get; set; }
        // 帰庫
        public string EnteringDate { get; set; }
        // 一般キロ
        public decimal InServiceKilo { get; set; }
        // 高速
        public decimal InServiceHighSpeed { get; set; }
        // 一般キロ
        public decimal ForwardingKilo { get; set; }
        // 高速
        public decimal ForwardingeHighSpeed { get; set; }
        // その他キロ
        public decimal OtherKilo { get; set; }
        // 総走行キロ
        public decimal TotalKilo { get; set; }
        // 名1
        public string Fuel1Name { get; set; }
        // 値1
        public decimal Fuel1Value { get; set; }
        // 名2
        public string Fuel2Name { get; set; }
        // 値2
        public decimal Fuel2Value { get; set; }
        // 名3
        public string Fuel3Name { get; set; }
        // 値3
        public decimal Fuel3Value { get; set; }
        
        // 受付営業所
        public string RegistrationOffice { get; set; }
        // 営業担当
        public string SalesStaff { get; set; }
        // 入力担当
        public string InputStaff { get; set; }
        // 予約区分
        public string ReserveClassification { get; set; }
        // 受付日
        public string ReceptionDate { get; set; }
        // 受付番号
        public string ReceiptNumber { get; set; }

        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
        // 行程
        public bool Journey { get; set; }
        // 積込
        public bool Load { get; set; }
        // 手配
        public bool Arrange { get; set; }
        // 備考
        public bool Remarks { get; set; }
        // 付帯
        public bool Incidental { get; set; }
        public string JourneyString { get; set; }
        // 積込
        public string LoadString { get; set; }
        // 手配
        public string ArrangeString { get; set; }
        // 備考
        public string RemarksString { get; set; }
        // 付帯
        public string IncidentalString { get; set; }
        public string NumberOfDriverString { get; set; }
        public string EmptyString { get; set; }
        public string SymbolInString { get; set; }
        public string PersonPlusString { get; set; }
        // 乗車人員
        public string PersonString { get; set; }
        // 行のテキストの色の管理のためのプロパティ
        public byte SihKbn { get; set; }
        public byte SCouKbn { get; set; }
        public byte NyuKinKbn { get; set; }
        public byte NCouKbn { get; set; }
        public byte NippoKbn { get; set; }
        public int YouKbn { get; set; }
        public byte HaiSKbn { get; set; }
        public byte HaiIKbn { get; set; }
        public short GuiWNin { get; set; }
        public string KaktYmd { get; set; }
        public byte KaknKais { get; set; }
        public byte KSKbn { get; set; }
        public byte KHinKbn { get; set; }
        public short UnkRen { get; set; }
        public short TeiDanNo { get; set; }
        public short BunkRen { get; set; }

        // For report
        // 車輌項目
        public string VehicleNm { get; set; }
        public int SyaRyoUnc { get; set; }
        public int SyaRyoSyo { get; set; }
        // 傭車項目
        public string YousyaNm { get; set; }
        public int YoushaUnc { get; set; }
        public int YoushaSyo { get; set; }
        //車種
        public string SyaSyuNm { get; set; }
        //
        public string HaiSYmdInfoUnkobi { get; set; }
        public int YouTblSeq { get; set; }
        public int NenryoCd1Seq { get; set; }
        public int NenryoCd2Seq { get; set; }
        public int NenryoCd3Seq { get; set; }
        public int SyaRyoCd { get; set; }
        public string SyaRyoNm { get; set; }
        public string HaiSYmd { get; set; }
        public string TouYmd { get; set; }
        public SuperMenuVehicleData()
        {

        }
        public SuperMenuVehicleData(SuperMenuVehicleData data)
        {
            UkeNo = data.UkeNo;
            Symbol = data.Symbol;
            Customer = data.Customer;
            Branch = data.Branch;
            PersonInCharge = data.PersonInCharge;
            Organization = data.Organization;
            Organization2 = data.Organization2;
            Destination = data.Destination;
            Dispatch = data.Dispatch;
            Arrival = data.Arrival;
            NumberOfDrivers = data.NumberOfDrivers;
            NumberOfGuides = data.NumberOfGuides;
            OfficeAddress = data.OfficeAddress;
            BusName = data.BusName;
            BusNo = data.BusNo;
            Fare = data.Fare;
            Crew = data.Crew;
            Fare = data.Fare;
            FareTax = data.FareTax;
            FareFee = data.FareFee;
            Guide = data.Guide;
            GuideTax = data.GuideTax;
            GuideFee = data.GuideFee;
            Other = data.Other;
            OtherTax = data.OtherTax;
            OtherFee = data.OtherFee;
            Person = data.Person;
            PersonPlus = data.PersonPlus;
            ExitingDate = data.ExitingDate;
            EnteringDate = data.EnteringDate;
            InServiceKilo = data.InServiceKilo;
            InServiceHighSpeed = data.InServiceHighSpeed;
            ForwardingKilo = data.ForwardingKilo;
            OtherKilo = data.OtherKilo;
            TotalKilo = data.TotalKilo;
            Fuel1Name = data.Fuel1Name;
            Fuel1Value = data.Fuel1Value;
            Fuel2Name = data.Fuel2Name;
            Fuel2Value = data.Fuel2Value;
            Fuel3Name = data.Fuel3Name;
            Fuel3Value = data.Fuel3Value;
            RegistrationOffice = data.RegistrationOffice;
            SalesStaff = data.SalesStaff;
            InputStaff = data.InputStaff;
            ReserveClassification = data.ReserveClassification;
            ReceptionDate = data.ReceptionDate;
            ReceiptNumber = data.ReceiptNumber;
            Journey = data.Journey;
            Load = data.Load;
            Arrange = data.Arrange;
            Remarks = data.Remarks;
            Incidental = data.Incidental;
            SihKbn = data.SihKbn;
            NyuKinKbn = data.NyuKinKbn;
            NCouKbn = data.NCouKbn;
            NippoKbn = data.NippoKbn;
            YouKbn = data.YouKbn;
            HaiSKbn = data.HaiSKbn;
            HaiIKbn = data.HaiIKbn;
            GuiWNin = data.GuiWNin;
            KaktYmd = data.KaktYmd;
            KaknKais = data.KaknKais;
            KSKbn = data.KSKbn;
            KHinKbn = data.KHinKbn;
            NumberOfDriverString = data.NumberOfDriverString;
            EmptyString = data.EmptyString;
            JourneyString = data.JourneyString;
            LoadString = data.LoadString;
            ArrangeString = data.ArrangeString;
            RemarksString = data.RemarksString;
            IncidentalString = data.IncidentalString;
            SymbolInString = data.SymbolInString;
            PersonString = data.PersonString;
            PersonPlusString = data.PersonPlusString;
            VehicleNm = data.VehicleNm;
            SyaRyoUnc = data.SyaRyoUnc;
            SyaRyoSyo = data.SyaRyoSyo;
            YousyaNm = data.YousyaNm;
            YoushaUnc = data.YoushaUnc;
            YoushaSyo = data.YoushaSyo;
            SyaSyuNm = data.SyaSyuNm;
            HaiSYmdInfoUnkobi = data.HaiSYmdInfoUnkobi;
            YouTblSeq = data.YouTblSeq;
            NenryoCd1Seq = data.NenryoCd1Seq;
            NenryoCd2Seq = data.NenryoCd2Seq;
            NenryoCd3Seq = data.NenryoCd3Seq;
            SyaRyoCd = data.SyaRyoCd;
            SyaRyoNm = data.SyaRyoNm;
        }
    }
    public class SuperMenuVehicleTotalData
    {
        // 運賃
        public long? Fare { get; set; }
        // 消費税
        public long? FareTax { get; set; }
        // 税込額
        public long? TaxIncluded { get; set; }
        // 手数料
        public long? FareFee { get; set; }
        // 付帯料金
        public long? Other { get; set; }
        // 付帯消費税
        public long? OtherTax { get; set; }
        // 付帯手数料
        public long? OtherFee { get; set; }
        // 損益
        public long? ProfitLoss { get; set; }
    }
    public class SuperMenuVehicleTotalGridData
    {
        // 受注額
        public SuperMenuVehicleTotalData Order { get; set; }
        // 自社
        public SuperMenuVehicleTotalData Company { get; set; }
        // 傭車
        public SuperMenuVehicleTotalData Bus { get; set; }

        public SuperMenuVehicleTotalGridData()
        {
            Order = new SuperMenuVehicleTotalData();
            Company = new SuperMenuVehicleTotalData();
            Bus = new SuperMenuVehicleTotalData();
        }
    }
    public class SuperMenuVehicleReportPDF
    {
        public List<SuperMenuVehicleData> ListData { get; set; }
        public int PageNumber { get; set; }
        public string CurrentDate { get; set; }
        public string OutputType { get; set; }
        public string DispatchDateFrom { get; set; }
        public string DispatchDateTo { get; set; }
        public string ArrivalDateFrom { get; set; }
        public string ArrivalDateTo { get; set; }
        public string ReceiptDateFrom { get; set; }
        public string ReceiptDateTo { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
    }
    public class SuperMenuVehicleCsv
    {
        public string UkeNo { get; set; }
        public short UnkRen { get; set; }
        public short TeiDanNo { get; set; }
        public short BunkRen { get; set; }
        public byte YoyaKbn { get; set; }
        public string YoyaKbnNm { get; set; }
        public string UkeYmd { get; set; }
        public short GyosyaCd { get; set; }
        public short TokuiCd { get; set; }
        public short SitenCd { get; set; }
        public string GyosyaNm { get; set; }
        public string TokuiNm { get; set; }
        public string SitenNm { get; set; }
        public string TokuiRyakuNm { get; set; }
        public string SitenRyakuNm { get; set; }
        public string TokuiTanNm { get; set; }
        public string DanTaNm { get; set; }
        public string DanTaNm2 { get; set; }
        public string IkNm { get; set; }
        public int SyaryoEigCd { get; set; }
        public string SyaryoEigyoNm { get; set; }
        public string SyaryoEigRyakuNm { get; set; }
        public int SyaRyoCd { get; set; }
        public string SyaRyoNm { get; set; }
        public string GoSya { get; set; }
        public short YouGyoCd { get; set; }
        public short YouCd { get; set; }
        public short YouSitCd { get; set; }
        public string YouGyoNm { get; set; }
        public string YouNm { get; set; }
        public string YouSitNm { get; set; }
        public string YouRyakuNm { get; set; }
        public string YouSitCdRyakuNm { get; set; }
        public string HaiSYmd { get; set; }
        public string HaiSTime { get; set; }
        public string HaiSNm { get; set; }
        public string HaiSJyuS { get; set; }
        public string HaiSJyuS2 { get; set; }
        public string TouYmd { get; set; }
        public string TouChTime { get; set; }
        public string TouNm { get; set; }
        public string TouJyusyo1 { get; set; }
        public string TouJyusyo2 { get; set; }
        public string SyuKoYmd { get; set; }
        public string SyuKoTime { get; set; }
        public int SyuEigCd { get; set; }
        public string SyuEigNm { get; set; }
        public string SyuEigRyakuNm { get; set; }
        public string KikYmd { get; set; }
        public string KikTime { get; set; }
        public int KikEigCd { get; set; }
        public string KikEigNm { get; set; }
        public string KikEigRyakuNm { get; set; }
        public string SyuPaTime { get; set; }
        public short DrvJin { get; set; }
        public short GuiSu { get; set; }
        public short SyaSyuCd { get; set; }
        public string SyaSyuNm { get; set; }
        public byte KataKbn { get; set; }
        public string KataNm { get; set; }
        public int SyaRyoUnc { get; set; }
        public int SyaRyoSyo { get; set; }
        public int SyaRyoTes { get; set; }
        public long Gui_UriGakKin_S { get; set; }
        public long Gui_SyaRyoSyo_S { get; set; }
        public long Gui_SyaRyoTes_S { get; set; }
        public long Oth_UriGakKin_S { get; set; }
        public long Oth_SyaRyoSyo_S { get; set; }
        public long Oth_SyaRyoTes_S { get; set; }
        public int YoushaUnc { get; set; }
        public int YoushaSyo { get; set; }
        public int YoushaTes { get; set; }
        public long YGui_HaseiKin_S { get; set; }
        public long YGui_SyaRyoSyo_S { get; set; }
        public long YGui_SyaRyoTes_S { get; set; }
        public long YOth_HaseiKin_S { get; set; }
        public long YOth_SyaRyoSyo_S { get; set; }
        public long YOth_SyaRyoTes_S { get; set; }
        public string SyainCd1 { get; set; }
        public string SyainNm1 { get; set; }
        public string SyainCd2 { get; set; }
        public string SyainNm2 { get; set; }
        public string SyainCd3 { get; set; }
        public string SyainNm3 { get; set; }
        public string SyainCd4 { get; set; }
        public string SyainNm4 { get; set; }
        public string SyainCd5 { get; set; }
        public string SyainNm5 { get; set; }
        public decimal JisaIPKm { get; set; }
        public decimal JisaKSKm { get; set; }
        public decimal KisoIPkm { get; set; }
        public decimal KisoKOKm { get; set; }
        public decimal OthKm { get; set; }
        public decimal TotalKilo { get; set; }
        public decimal Nenryo1 { get; set; }
        public string NenryoRyak1 { get; set; }
        public decimal Nenryo2 { get; set; }
        public string NenryoRyak2 { get; set; }
        public decimal Nenryo3 { get; set; }
        public string NenryoRyak3 { get; set; }
        public short JyoSyaJin { get; set; }
        public short PlusJin { get; set; }
        public string UkeJyKbn { get; set; }
        public string SijJoKbn1 { get; set; }
        public string SijJoKbn2 { get; set; }
        public string SijJoKbn3 { get; set; }
        public string SijJoKbn4 { get; set; }
        public string SijJoKbn5 { get; set; }
        public int UkeEigCd { get; set; }
        public string UkeEigNm { get; set; }
        public string UkeEigRyakuNm { get; set; }
        public string EigTanSyainCd { get; set; }
        public string EigTanSyainNm { get; set; }
        public string InputTanSyainCd { get; set; }
        public string InputTanSyainNm { get; set; }
        public string DantaiCd { get; set; }
        public string DantaiCdNm { get; set; }
        public byte JyokyakuCd { get; set; }
        public string JyokyakuNm { get; set; }
        public string CanFuYmd { get; set; }
        public string CanFuRiy { get; set; }
        public string CanFutanCd { get; set; }
        public string CanFutanNm { get; set; }
        
        public SuperMenuVehicleCsv()
        {

        }
    }

    public class HaiTaParam
    {
        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
        public string UkeNo { get; set; }
    }
}
