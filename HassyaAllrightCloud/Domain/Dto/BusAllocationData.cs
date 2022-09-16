using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class BusAllocationData
    {
        public class OutputOrderData
        {
            public int IdValue { get; set; }
            public string StringValue { get; set; }
        }
        public class BusAllocationSearch
        {
            public string DateSpecified { get; set; } = "配車日";
            public DateTime pickupDate { get; set; }      
            public BranchChartData BranchChart { get; set; } = new BranchChartData();

            public string bookingParam { get; set; } = "";
            public ReservationData ReservationClassification { get; set; } = new ReservationData();
            public int outputSortOrder { get; set; } = 0;
            public CompanyChartData CompanyChart { get; set; } = new CompanyChartData();
            public string UnprovisionedVehicle1 { get; set; } = "有";
            public ReservationData ReservationClassification1 { get; set; } = new ReservationData();
            public ReservationData ReservationClassification2 { get; set; } = new ReservationData();
            public CompanyChartData VehicleAffiliation1 { get; set; } = new CompanyChartData();
            public BranchChartData VehicleAffiliation2 { get; set; } = new BranchChartData();
            public BusAllocationData.OutputOrderData UnprovisionedVehicle2 { get; set; } = new BusAllocationData.OutputOrderData();
            public bool pickupTime { get; set; }
            public bool isgray { get; set; }
            public string startTime { get; set; }
            public string endTime { get; set; }
            public string bookingfrom { get; set; } = "0";
            public string bookingto { get; set; } = "2147483647";
            public string size { get; set; }
            public string HaiSKbn { get; set; }
            public List<ReservationClassComponentData> BookingTypes { get; set; } = new List<ReservationClassComponentData>();
            public ReservationClassComponentData BookingFrom { get; set; }
            public ReservationClassComponentData BookingTo { get; set; }
            public byte KSKbn { get; set; }

        }
        public class OutputOrderListData
        {
            public static List<OutputOrderData> outputOrderlst = new List<OutputOrderData>
        {
            new OutputOrderData { IdValue = 0, StringValue = " ", },
            new OutputOrderData { IdValue = 1, StringValue = "出庫・車両コード順"},
            new OutputOrderData { IdValue = 2, StringValue = "出庫・悌団順"},
            new OutputOrderData { IdValue = 3, StringValue = "車両コード順"},
            new OutputOrderData { IdValue = 4, StringValue = "車両点呼順"}
        };

        }
        // 年月日指定
        public static IEnumerable<string> DateSpecified = new List<string>() {
            "配車日","到着日"
        };
        // 未仮車状況
        public static IEnumerable<string> UnprovisionedVehicle1 = new List<string>() {
            "有","無"
        };
    }
    public class AssignedEmployee
    {
        public string HAIIN_UkeNo { get; set; }
        public short HAIIN_UnkRen { get; set; }
        public short HAIIN_TeiDanNo { get; set; }
        public short HAIIN_BunkRen { get; set; }
        public byte HAIIN_HaiInRen { get; set; }
        public int HAIIN_SyainCdSeq { get; set; }
        public string HAIIN_Syukinbasy { get; set; }
        public string HAIIN_TaiknBasy { get; set; }
        public int EIGYOS_EigyoCdSeq { get; set; } 
        public int EIGYOS_EigyoCd { get; set; } 
        public string EIGYOS_EigyoNm { get; set; } = "";
        public string SYAIN_SyainCd { get; set; } = "";
        public string SYAIN_SyainNm { get; set; } = "";
        public string Text => EIGYOS_EigyoCdSeq>0? $"{EIGYOS_EigyoCd.ToString("D5")} : {EIGYOS_EigyoNm}  {SYAIN_SyainCd.PadLeft(5,'0')}: {SYAIN_SyainNm}" : "";

    }
    public class BusAllocationDataGrid
    {
        public int Index { get; set; }
        public int RowID { get; set; } = 0;
        public string YYKSHO_UkeNo { get; set; }
        public int YYKSHO_UkeCd { get; set; }
        public string TOKISK_TokuiNm { get; set; }
        public string TOKIST_SitenNm { get; set; }
        public short HAISHA_UnkRen { get; set; }
        public short HAISHA_SyaSyuRen { get; set; }
        public short HAISHA_TeiDanNo { get; set; }
        public short HAISHA_BunkRen { get; set; }
        public string HAISHA_GoSya { get; set; }
        public int HAISHA_SyuEigCdSeq { get; set; }
        public string EIGYOS_RyakuNm { get; set; }
        public int HAISHA_KikEigSeq { get; set; }
        public int HAISHA_HaiSSryCdSeq { get; set; }
        public string SYARYO_SyaRyoNm { get; set; }
        public string SYASYU_SyaSyuNm { get; set; }
        public string HAISHA_DanTaNm2 { get; set; }
        public string HAISHA_SyuKoYmd { get; set; }
        public string HAISHA_SyuKoTime { get; set; }
        public string HAISHA_HaiSYmd { get; set; }
        public string HAISHA_HaiSTime { get; set; }
        public string HAISHA_SyuPaTime { get; set; }
        public int HAISHA_IkMapCdSeq { get; set; }
        public string HAISHA_IkNm { get; set; }
        public int HAISHA_HaiSCdSeq { get; set; }
        public string HAISHA_HaiSNm { get; set; }
        public string HAISHA_HaiSJyus1 { get; set; }
        public string HAISHA_HaiSJyus2 { get; set; }
        public string HAISHA_HaiSKigou { get; set; }
        public int HAISHA_HaiSKouKCdSeq { get; set; }
        public string HAISHA_HaiSKouKNm { get; set; }
        public int HAISHA_HaiSBinCdSeq { get; set; }
        public string HAISHA_HaiSBinNm { get; set; }
        public string HAISHA_HaiSSetTime { get; set; }
        public string HAISHA_TouYmd { get; set; }
        public string HAISHA_TouChTime { get; set; }
        public string HAISHA_KikYmd { get; set; }
        public string HAISHA_KikTime { get; set; }
        public int HAISHA_TouCdSeq { get; set; }
        public string HAISHA_TouNm { get; set; }
        public string HAISHA_TouJyusyo1 { get; set; }
        public string HAISHA_TouJyusyo2 { get; set; }
        public string HAISHA_TouKigou { get; set; }
        public int HAISHA_TouKouKCdSeq { get; set; }
        public string HAISHA_TouSKouKNm { get; set; }
        public int HAISHA_TouBinCdSeq { get; set; }
        public string HAISHA_TouBinNm { get; set; }
        public string HAISHA_TouSetTime { get; set; }
        public short HAISHA_JyoSyaJin { get; set; }
        public short HAISHA_PlusJin { get; set; }
        public short HAISHA_DrvJin { get; set; }
        public short HAISHA_GuiSu { get; set; }
        public byte HAISHA_OthJinKbn1 { get; set; }
        public string OTHERJIN1_RyakuNm { get; set; }
        public short HAISHA_OthJin1 { get; set; }
        public byte HAISHA_OthJinKbn2 { get; set; }
        public string OTHERJIN2_RyakuNm { get; set; }
        public short HAISHA_OthJin2 { get; set; }
        public byte HAISHA_KSKbn { get; set; }
        public byte HAISHA_HaiSKbn { get; set; }
        public byte HAISHA_HaiIKbn { get; set; }
        public byte HAISHA_NippoKbn { get; set; }
        public int HAISHA_YouTblSeq { get; set; }
        public int HAISHA_SyaRyoUncSyaRyoSyo { get; set; }
        public int HAISHA_SyaRyoUnc { get; set; }
        public int HAISHA_SyaRyoSyo { get; set; }
        public int HAISHA_SyaRyoTes { get; set; }
        public string HAISHA_PlatNo { get; set; }
        public string HAISHA_CustomItems1 { get; set; }
        public string HAISHA_CustomItems2 { get; set; }
        public string HAISHA_CustomItems3 { get; set; }
        public string HAISHA_CustomItems4 { get; set; }
        public string HAISHA_CustomItems5 { get; set; }
        public string HAISHA_CustomItems6 { get; set; }
        public string HAISHA_CustomItems7 { get; set; }
        public string HAISHA_CustomItems8 { get; set; }
        public string HAISHA_CustomItems9 { get; set; }
        public string HAISHA_CustomItems10 { get; set; }
        public string HAISHA_CustomItems11 { get; set; }
        public string HAISHA_CustomItems12 { get; set; }
        public string HAISHA_CustomItems13 { get; set; }
        public string HAISHA_CustomItems14 { get; set; }
        public string HAISHA_CustomItems15 { get; set; }
        public string HAISHA_CustomItems16 { get; set; }
        public string HAISHA_CustomItems17 { get; set; }
        public string HAISHA_CustomItems18 { get; set; }
        public string HAISHA_CustomItems19 { get; set; }
        public string HAISHA_CustomItems20 { get; set; }
        public int UNKOBI_UnkoJKbn { get; set; }
        public string UNKOBI_DanTaNm { get; set; }
        public string UNKOBI_HaiSYmd { get; set; }
        public string UNKOBI_HaiSTime { get; set; }
        public string UNKOBI_SyukoYmd { get; set; }
        public string UNKOBI_SyuKoTime { get; set; }
        public string UNKOBI_TouYmd { get; set; }
        public string UNKOBI_TouChTime { get; set; }
        public string UNKOBI_KikYmd { get; set; }
        public string UNKOBI_KikTime { get; set; }
        public string HENSYA_TenkoNo { get; set; }
        public string ColorClass { get; set; }
        public bool IsLock { get; set; } = false;
        public Dictionary<string, string> CustomData { get; set; } = new Dictionary<string, string>();
        public DateTime CheckSyuKoHaiSha
        {
            get
            {
                return ParseStringToDateTime(HAISHA_SyuKoYmd, HAISHA_SyuKoTime);
            }
        }
        public DateTime CheckHaiSHaiSha
        {
            get
            {
                return ParseStringToDateTime(HAISHA_HaiSYmd, HAISHA_HaiSTime);
            }
        }
        public DateTime CheckTouHaiSha
        {
            get
            {
                return ParseStringToDateTime(HAISHA_TouYmd, HAISHA_TouChTime);
            }
        }
        public DateTime CheckKikHaiSha
        {
            get
            {
                return ParseStringToDateTime(HAISHA_KikYmd, HAISHA_KikTime);
            }
        }
        private DateTime ParseStringToDateTime(string dateTime, string Time)
        {
            DateTime result;
            DateTime.TryParseExact(dateTime + Time, "yyyyMMddHHmm", new CultureInfo("ja-JP"), DateTimeStyles.None, out result);
            return result;
        }
        public int HAISHA_SyaRyoUncSyo { get => HAISHA_SyaRyoUnc + HAISHA_SyaRyoSyo; }
        public string StringEmpty { get; set; }
        public int SYARYO_SyainCdSeq{ get; set; }
        public byte screenKbn { get; set; }
        public int YYKSHO_YoyaKbnSeq { get; set; }
    }

    public class BusAllocationHaitaCheck
    {
        public string UkeNo { get; set; }
        public string YYKSHO_UpdYmdTime { get; set; }
        public string HAISHA_UpYmdTime { get; set; }
        public string UNKOBI_UpdYmdTime { get; set; }
        public string KOBAN_UpdYmdTime { get; set; }
        public string HAIIN_UpdYmdTime { get; set; }
    }

    public class BusAllocationDataUpdate
    {
        public string YYKSHO_UkeNo { get; set; } = "";
        public string YYKSHO_UpdYmd { get; set; }
        public string YYKSHO_UpdTime { get; set; }
        public short HAISHA_UnkRen { get; set; } = 0;
        public short HAISHA_SyaSyuRen { get; set; } = 0;
        public short HAISHA_TeiDanNo { get; set; } = 0;
        public short HAISHA_BunkRen { get; set; } = 0;
        public string HAISHA_UpYmd { get; set; }
        public string HAISHA_UpdTime { get; set; }
        public string UkenoCd { get; set; } = "";
        public string UnkobiStart { get; set; } = "";
        public string UnkobiEnd { get; set; } = "";
        public string UNKOBI_UpdYmd { get; set; }
        public string UNKOBI_UpdTime { get; set; }
        public string CustomerName { get; set; } = "";
        public string HAISHA_GoSya { get; set; } = "";
        public BusInfoData BusInfoData { get; set; } = new BusInfoData();
        public BranchChartData BranchChartData { get; set; } = new BranchChartData();
        public string DanTaNm2 { get; set; } = "";
        public TPM_CodeKbDataKenCD TPM_CodeKbDataKenCD { get; set; } = new TPM_CodeKbDataKenCD();
        public string IkNm { get; set; } = "";
        public DateTime SyuKoYmd { get; set; } = DateTime.Today;
        public string SyuKoTime { get; set; } = ""; 
        public DateTime HaiSYmd { get; set; } = DateTime.Today;
        public string HaiSTime { get; set; } = "";
        public string SyuPaTime { get; set; } = "";
        public TPM_CodeKbDataBunruiCD TPM_CodeKbDataBunruiCDStart { get; set; } = new TPM_CodeKbDataBunruiCD();
        public string HaiSNm { get; set; } = "";
        public string HaiSJyus1 { get; set; } = "";
        public string HaiSJyus2 { get; set; } = "";
        public string HaiSKigou { get; set; } = "";
        public TPM_CodeKbDataDepot TPM_CodeKbDataDepotStart { get; set; } = new TPM_CodeKbDataDepot();
        public string HaiSSetTime { get; set; } = "";
        public string HaisBinNm { get; set; } = "";
        public DateTime TouYmd { get; set; } = DateTime.Today;
        public string TouChTime { get; set; } = "";
        public DateTime KikYmd { get; set; } = DateTime.Today;
        public string KikTime { get; set; } = "";
        public TPM_CodeKbDataBunruiCD TPM_CodeKbDataBunruiCDEnd { get; set; } = new TPM_CodeKbDataBunruiCD();
        public string TouNm { get; set; } = "";
        public string TouJyusyo1 { get; set; } = "";
        public string TouJyusyo2 { get; set; } = "";
        public string TouKigou { get; set; } = "";
        public TPM_CodeKbDataDepot TPM_CodeKbDataDepotEnd { get; set; } = new TPM_CodeKbDataDepot();
        public string TouSetTime { get; set; } = "";       
        public string TouBinNm { get; set; } = "";
        public BranchChartData BranchChartDataKik { get; set; } = new BranchChartData();
        public short JyoSyaJin { get; set; } = 0;
        public short PlusJin { get; set; } = 0;
        public short DrvJin { get; set; } = 0;
        public short GuiSu { get; set; } = 0;
        public TPM_CodeKbDataOTHJINKBN TPM_CodeKbDataOTHJINKBN01 { get; set; } = new TPM_CodeKbDataOTHJINKBN();
        public short OthJin1 { get; set; }
        public TPM_CodeKbDataOTHJINKBN TPM_CodeKbDataOTHJINKBN02 { get; set; } = new TPM_CodeKbDataOTHJINKBN();
        public short OthJin2 { get; set; } = 0;
        public string PlatNo { get; set; } = "";   
        public DateTime CheckSyuKoUnkobi { get; set; } = DateTime.Today;
        public DateTime CheckHaiSUnkobi { get; set; } = DateTime.Today;
        public DateTime CheckTouUnkobi { get; set; } = DateTime.Today;
        public DateTime CheckKikUnkobi { get; set; } = DateTime.Today;
        public DateTime EndDate { get; set; } = DateTime.Today;
        public DateTime CheckSyuKoHaiSha
         {
             get
             {
                 return ParseTimeStringToDate(SyuKoYmd , SyuKoTime);
             }
         }       
         public DateTime CheckHaiSHaiSha
         {
             get
             {
                 return ParseTimeStringToDate(HaiSYmd, HaiSTime);
             }
         }                 
         public DateTime CheckTouHaiSha
         {
             get
             {
                return ParseTimeStringToDate(TouYmd, TouChTime);
             }
         }
         public DateTime CheckKikHaiSha
         {
             get
             {
                return ParseTimeStringToDate(KikYmd, KikTime);
             }
         }
        private DateTime ParseTimeStringToDate(DateTime dateTime, string Time)
        {
            DateTime result;
            DateTime.TryParseExact(dateTime.ToString("yyyyMMdd") + Time, "yyyyMMddHHmm", new CultureInfo("ja-JP"), DateTimeStyles.None, out result);
            return result;
        }
        public Dictionary<string, string> CustomData { get; set; } = new Dictionary<string, string>();
        public string HaiSKbn { get; set; }
        public byte screenKbn { get; set; }
        public TPM_SyokumData driverchartItem { get; set; } = new TPM_SyokumData();
        public List<Driverlst> Driverlstitem { get; set; } = new List<Driverlst>();
        public List<Driverlst> DriverAssigneds { get; set; }
        public int SYARYO_SyainCdSeq { get; set; }
        public byte SyokumuKbn { get; set; }
        public bool CheckDeleteDriver { get; set; }
        public string KOBAN_UpdYmd { get; set; }
        public string KOBAN_UpdTime { get; set; }
        public string HAIIN_UpdYmd { get; set; }
        public string HAIIN_UpdTime { get; set; }

        public int HAISHA_HaiSSryCdSeq { get; set; }
        public int HAISHA_YouTblSeq { get; set; }
        public byte HAISHA_HaiSKbn { get; set; }
        public byte HAISHA_HaiIKbn { get; set; }
        public int YYKSHO_YoyaKbnSeq { get; set; }
    }
    public class CarTypePopup
    {
        public string YYKSYU_UkeNo { get; set; } = "";
        public short YYKSYU_UnkRen { get; set; } 
        public short YYKSYU_SyaSyuRen { get; set; } 
        public int YYKSYU_SyaSyuCdSeq { get; set; } 
        public string SYASYU_SyaSyuNm { get; set; } = "";
        public byte YYKSYU_KataKbn { get; set; }
        public string KATAKBN_RyakuNm { get; set; } = "";
        public int YYKSYU_SyaSyuDai { get; set; }
    }
    public class StaffPosition
    {
        public int SyainCdSeq { get; set; }
        public int SyokumuCd { get; set; }

    }
    public class BusAllocationDataCopyPaste
    {
        public BusAllocationDataGrid DataSourceItem { get; set; } = new BusAllocationDataGrid();
        public List<BusAllocationDataGrid> DataUpdateList { get; set; } = new List<BusAllocationDataGrid>();

    }
    public class BusAllocationDataUpdateAll
    {
        public string YYKSHO_UkeNo { get; set; } = "";
        public short HAISHA_UnkRen { get; set; } = 0;
        public short HAISHA_SyaSyuRen { get; set; } = 0;
        public short HAISHA_TeiDanNo { get; set; } = 0;
        public short HAISHA_BunkRen { get; set; } = 0;
        public string UkenoCd { get; set; } = "";
        public string UnkobiStart { get; set; } = "";
        public string UnkobiEnd { get; set; } = "";
        public BusInfoData BusInfoData { get; set; } = new BusInfoData();
        public BranchChartData BranchChartData { get; set; } = new BranchChartData();
        public string DanTaNm2 { get; set; } = "";
        public TPM_CodeKbDataKenCD TPM_CodeKbDataKenCD { get; set; } = new TPM_CodeKbDataKenCD();
        public string IkNm { get; set; } = "";
        public DateTime SyuKoYmd { get; set; } = DateTime.Today;
        public string SyuKoTime { get; set; } = "";
        public DateTime HaiSYmd { get; set; } = DateTime.Today;
        public string HaiSTime { get; set; } = "";
        public string SyuPaTime { get; set; } = "";
        public TPM_CodeKbDataBunruiCD TPM_CodeKbDataBunruiCDStart { get; set; } = new TPM_CodeKbDataBunruiCD();
        public string HaiSNm { get; set; } = "";
        public string HaiSJyus1 { get; set; } = "";
        public string HaiSJyus2 { get; set; } = "";
        public string HaiSKigou { get; set; } = "";
        public TPM_CodeKbDataDepot TPM_CodeKbDataDepotStart { get; set; } = new TPM_CodeKbDataDepot();
        public string HaiSSetTime { get; set; } = "";
        public string HaisBinNm { get; set; } = "";
        public DateTime TouYmd { get; set; } = DateTime.Today;
        public string TouChTime { get; set; } = "";
        public DateTime KikYmd { get; set; } = DateTime.Today;
        public string KikTime { get; set; } = "";
        public TPM_CodeKbDataBunruiCD TPM_CodeKbDataBunruiCDEnd { get; set; } = new TPM_CodeKbDataBunruiCD();
        public string TouNm { get; set; } = "";
        public string TouJyusyo1 { get; set; } = "";
        public string TouJyusyo2 { get; set; } = "";
        public string TouKigou { get; set; } = "";
        public TPM_CodeKbDataDepot TPM_CodeKbDataDepotEnd { get; set; } = new TPM_CodeKbDataDepot();
        public string TouSetTime { get; set; } = "";
        public string TouBinNm { get; set; } = "";
        public BranchChartData BranchChartDataKik { get; set; } = new BranchChartData();
        public short JyoSyaJin { get; set; } = 0;
        public short PlusJin { get; set; } = 0;
        public short DrvJin { get; set; } = 0;
        public short GuiSu { get; set; } = 0;
        public TPM_CodeKbDataOTHJINKBN TPM_CodeKbDataOTHJINKBN01 { get; set; } = new TPM_CodeKbDataOTHJINKBN();
        public short OthJin1 { get; set; }
        public TPM_CodeKbDataOTHJINKBN TPM_CodeKbDataOTHJINKBN02 { get; set; } = new TPM_CodeKbDataOTHJINKBN();
        public short OthJin2 { get; set; } = 0;
        public string PlatNo { get; set; } = "";
        public DateTime CheckSyuKoUnkobi { get; set; } = DateTime.Today;
        public DateTime CheckHaiSUnkobi { get; set; } = DateTime.Today;
        public DateTime CheckTouUnkobi { get; set; } = DateTime.Today;
        public DateTime CheckKikUnkobi { get; set; } = DateTime.Today;
        public DateTime EndDate { get; set; } = DateTime.Today;
        public bool DisableSyukoDateTime { get; set; } = true;
        public bool DisableHaiSDateTime { get; set; } = true;
        public bool DisableSyaPaTime { get; set; } = true;
        public bool DisableTouDateTime { get; set; } = true;
        public bool DisableKikDateTime { get; set; } = true;
        public bool DisableHaiSSetTime { get; set; } = true;
        public bool DisableTouSetTime { get; set; } = true;
        public Dictionary<string, string> CustomData { get; set; } = new Dictionary<string, string>();
        public byte screenKbn { get; set; }
        public DateTime CheckSyuKoHaiSha
        {
            get
            {
                return ParseTimeStringToDate(SyuKoYmd, SyuKoTime);
            }
        }
        public DateTime CheckHaiSHaiSha
        {
            get
            {
                return ParseTimeStringToDate(HaiSYmd, HaiSTime);
            }
        }
        public DateTime CheckSyuPaHaiSha
        {
            get
            {
                return ParseTimeStringToDate(HaiSYmd, SyuPaTime);
            }
        }
        public DateTime CheckTouHaiSha
        {
            get
            {
                return ParseTimeStringToDate(TouYmd, TouChTime);
            }
        }
        public DateTime CheckKikHaiSha
        {
            get
            {
                return ParseTimeStringToDate(KikYmd, KikTime);
            }
        }
        public List<DateTimeHaiShaItem> DateTimeHaiShaList { get; set; } = new List<DateTimeHaiShaItem>();
        private DateTime ParseTimeStringToDate(DateTime dateTime, string Time)
        {
            DateTime result;
            DateTime.TryParseExact(dateTime.ToString("yyyyMMdd") + Time, "yyyyMMddHHmm", new CultureInfo("ja-JP"), DateTimeStyles.None, out result);
            return result;
        }
        public string HaiSKbn { get; set; }
    }
    public class BusAllocationDataEditSimultaneously
    {
        public BusAllocationDataUpdateAll DataSourceItem { get; set; } = new BusAllocationDataUpdateAll();
        public List<BusAllocationDataGrid> DataUpdateList { get; set; } = new List<BusAllocationDataGrid>();
    }
    public class DateTimeHaiShaItem
    {
        public DateTime CheckSyukoHaiSha { get; set; }
        public DateTime CheckHaiSHaiSha { get; set; }
        public DateTime CheckTouHaiSha { get; set; }
        public DateTime CheckKikHaiSha { get; set; }
        public DateTime CheckSyukoHaiShaForm { get; set; }
        public DateTime CheckHaiSHaiShaForm { get; set; }
        public DateTime CheckTouHaiShaForm { get; set; }
        public DateTime CheckKikHaiShaForm { get; set; }
        public bool DisableSyukoDateTime { get; set; } = true;
        public bool DisableHaiSDateTime { get; set; } = true;
        public bool DisableSyaPaTime { get; set; } = true;
        public bool DisableTouDateTime { get; set; } = true;
        public bool DisableKikDateTime { get; set; } = true;
        public bool DisableHaiSSetTime { get; set; } = true;
        public bool DisableTouSetTime { get; set; } = true;            
    } 



    
}
