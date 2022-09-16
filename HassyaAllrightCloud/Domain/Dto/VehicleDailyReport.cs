using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class VehicleDailyReportModel
    {
        public int SyaRyoCdSeq { get; set; } // 車輛コード
        // 車号
        public string SyaryoNm { get; set; } // 車号
        public int SyaRyoCd { get; set; } // 車種コード
        public string SyaSyuNm { get; set; } // 車種コード名称
        public byte KataKbn { get; set; } // 型区分
        public string KataKbnRyakuNm { get; set; } // 型区分名
        // 運行年月日
        public string UnkYmd { get; set; }
        // 団体名
        public string DanTaNm { get; set; } // 団体名
        // 団体名２
        public string DanTaNm2 { get; set; } // 団体名２
        // 行先名
        public string IkNm { get; set; } // 行先名
        public short GyosyaCd { get; set; } // 得意先業者コード
        public short TokuiCd { get; set; } // 得意先コード
        public short SitenCd { get; set; } // 支店コード
        public string GyosyaNm { get; set; } // 得意先業者名
        public string TokuiNm { get; set; } // 得意先名
        public string SitenNm { get; set; } // 得意先支店名
        // 得意先
        public string TokuiRyakuNm { get; set; } // 得意先略名
        public string SitenRyakuNm { get; set; } // 得意先支店略名
        // 運行期間
        public string HaiSYmd { get; set; } // 配車年月日
        public string TouYmd { get; set; } // 到着年月日
        // 出庫/帰庫
        public string SyukoYmd { get; set; } // 出庫日付
        public string Haisha_SyukoTime { get; set; } // 出庫時間
        public string KikYmd { get; set; } // 帰庫日付
        public string Haisha_KikTime { get; set; } // 帰庫時間
        // 受付番号
        public string UkeNo { get; set; } // 受付番号
        public short UnkRen { get; set; } // 運行連番
        public byte YoyaKbn { get; set; } // 予約区分
        // 予約区分
        public string YoyaKbnNm { get; set; } // 予約区分名
        // 総走行キロ
        public decimal StMeter { get; set; } // 始メーター
        public decimal EndMeter { get; set; } // 終メーター
        // 実車キロ一般
        public decimal JisaIPKm { get; set; } // 実車_一般
        // 実車キロ高速
        public decimal JisaKSKm { get; set; } // 実車_高速
        // 回送キロ一般
        public decimal KisoIPKm { get; set; } // 回送_一般
        // 回送キロ高速
        public decimal KisoKOKm { get; set; } // 回送_高速
        // その他キロ
        public decimal OthKm { get; set; } // その他キロ

        public decimal TotalKm => EndMeter - StMeter; // 総走行キロ

        // 乗車人員
        public short JyoSyaJin { get; set; } // 乗車人員
        // プラス人員
        public short PlusJin { get; set; } // プラス人員
        // 燃料１
        public string NenryoCd1 { get; set; } // 燃料１コード
        public string NenryoNm1 { get; set; } // 燃料１名称
        public string NenryoRyakuNm1 { get; set; } // 燃料１略名
        public decimal Nenryo1 { get; set; } // 燃料１数量
        // 燃料２
        public string NenryoCd2 { get; set; } // 燃料２コード
        public string NenryoNm2 { get; set; } // 燃料２名称
        public string NenryoRyakuNm2 { get; set; } // 燃料２略名
        public decimal Nenryo2 { get; set; } // 燃料２数量
        // 燃料３
        public string NenryoCd3 { get; set; } // 燃料３コード
        public string NenryoNm3 { get; set; } // 燃料３名称
        public string NenryoRyakuNm3 { get; set; } // 燃料３略名
        public decimal Nenryo3 { get; set; } // 燃料３数量
        // 乗務員
        public string SyainCd1 { get; set; } // 乗務員１コード
        public string SyainNm1 { get; set; } // 乗務員１氏名
        public string SyainCd2 { get; set; } // 乗務員２コード
        public string SyainNm2 { get; set; } // 乗務員２氏名
        public string SyainCd3 { get; set; } // 乗務員３コード
        public string SyainNm3 { get; set; } // 乗務員３氏名
        public string SyainCd4 { get; set; } // 乗務員４コード
        public string SyainNm4 { get; set; } // 乗務員４氏名
        public string SyainCd5 { get; set; } // 乗務員５コード
        public string SyainNm5 { get; set; } // 乗務員５氏名

        public int EigyoCd { get; set; }
        public string Shabni_SyukoTime { get; set; }
        public string Shabni_KikTime { get; set; }
        // 拘束時間
        public string RestraintTime { get; set; }
        public byte UnkKai { get; set; }
        public short TeiDanNo { get; set; }
        public short BunkRen { get; set; }
        public int TenantCdSeq { get; set; } = new ClaimModel().TenantID;
        public string UkeYmd { get; set; }
        public string dayOfWeekTou { get; set; }
        public string dayOfWeekHai { get; set; }
        public int NenryoCd1Seq { get; set; }
        public int NenryoCd2Seq { get; set; }
        public int NenryoCd3Seq { get; set; }
    }

    public class VehicleDailyReportSearchParam : ICloneable
    {
        public byte OutputSetting { get; set; }
        public VehicleSearchDropdown OutputWithHeader { get; set; }
        public VehicleSearchDropdown KukuriKbn { get; set; }
        public VehicleSearchDropdown KugiriCharType { get; set; }
        public VehicleSearchDropdown PaperSize { get; set; }
        public DateTime? ScheduleYmdStart { get; set; } = null;
        public DateTime? ScheduleYmdEnd { get; set; } = null;
        public string Company { get; set; }
        public BusSaleBranchSearch selectedBusSaleStart { get; set; }
        public BusSaleBranchSearch selectedBusSaleEnd { get; set; }
        public BusCodeSearch selectedBusCodeStart { get; set; }
        public BusCodeSearch selectedBusCodeEnd { get; set; }
        public string ReceptionStart { get; set; } = string.Empty;
        public string ReceptionEnd { get; set; } = string.Empty;
        public ReservationClassComponentData selectedReservationStart { get; set; }
        public ReservationClassComponentData selectedReservationEnd { get; set; }
        public VehicleSearchDropdown OutputKbn { get; set; }
        public byte fontSize { get; set; } = (byte)ViewMode.Medium;

        public int TenantCdSeq { get; set; } = new ClaimModel().TenantID;
        public int CompanyCd { get; set; } = new ClaimModel().CompanyID;
        public int CompanyCdSeq { get; set; } = new ClaimModel().CompanyID;
        public string StrYmd { get; set; } = string.Empty;
        public string EndYmd { get; set; } = string.Empty;
        public int SyaRyoCdSeq { get; set; }
        public CurrentBus selectedCurrentBus { get; set; } = new CurrentBus();
        public string selectedUnkYmd { get; set; }

        // copy value of all properties into new object
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class VehicleDailyReportChildModel
    {
        // 合計
        public string Text { get; set; }
        // 運行回数
        public int NumberOfTrips { get; set; }
        // 乗車人員
        public int BoardingPersonnel { get; set; }
        // プラス人員
        public int PlusPersonnel { get; set; }
        // 実車キロ一般
        public decimal ActualKmGeneral { get; set; }
        // 実車キロ高速
        public decimal ActualKmHighSpeed { get; set; }
        // 回送キロ一般
        public decimal ForwardingKmGeneral { get; set; }
        // 回送キロ高速
        public decimal ForwardingKmHighSpeed { get; set; }
        // その他キロ
        public decimal OtherKm { get; set; }
        // 総走行キロ
        public decimal TotalMile { get; set; }
        // 燃料１
        public decimal Fuel1 { get; set; }
        // 燃料２
        public decimal Fuel2 { get; set; }
        // 燃料３
        public decimal Fuel3 { get; set; }
    }

    public class BusSaleBranchSearch
    {
        public int EigyoCdSeq { get; set; }
        public int EigyoCd { get; set; }
        public string RyakuNm { get; set; }
        public string Text => $"{EigyoCd.ToString().PadLeft(5, '0')} : {RyakuNm}";
    }

    public class BusCodeSearch
    {
        public int SyaRyoCdSeq { get; set; }
        public int SyaRyoCd { get; set; }
        public string SyaRyoNm { get; set; }
        public string Text => $"{SyaRyoCd.ToString().PadLeft(5, '0')} : {SyaRyoNm}";
    }

    public class ReservationSearch
    {
        public bool IsSelectedAll { get; set; } = false;
        public byte YoyaKbn { get; set; }
        public string PriorityNum { get; set; }
        public string YoyaKbnNm { get; set; }
        public string Text => $"{YoyaKbn} : {YoyaKbnNm}";
    }

    public class VehicleSearchDropdown
    {
        public byte Value { get; set; }
        public string Text { get; set; }
    }

    public class CurrentBus
    {
        public int SyaRyoCdSeq { get; set; }
        public int SyaRyoCd { get; set; }
        public string SyaRyoNm { get; set; }
        public string SyaSyuNm { get; set; }
        public string KataKbnRyakuNm { get; set; }
        public string NinkaKbnRyakuNm { get; set; }
        public string EigyoRyakuNm { get; set; }
        public string UnkYmd { get; set; }
        public string Text => $"{SyaRyoCd:00000} : {SyaRyoNm}";
    }

    public class VehicleDailyReportData
    {
        public string UkeNo { get; set; }
        public short UnkRen { get; set; }
        public short TeiDanNo { get; set; }
        public short BunkRen { get; set; }
        public string UnkYmd { get; set; }
        public short HenKai { get; set; }
        public string StMeter { get; set; } = "0.00";
        public string EndMeter { get; set; } = "0.00";
        public string JisaIPKm { get; set; } = "0.00";
        public string JisaKSKm { get; set; } = "0.00";
        public string KisoIPKm { get; set; } = "0.00";
        public string KisoKOKm { get; set; } = "0.00";
        public string OthKm { get; set; } = "0.00";
        public int NenryoCd1Seq { get; set; }
        public string Nenryo1 { get; set; } = "0.00";
        public int NenryoCd2Seq { get; set; }
        public string Nenryo2 { get; set; } = "0.00";
        public int NenryoCd3Seq { get; set; }
        public string Nenryo3 { get; set; } = "0.00";
        public string JyoSyaJin { get; set; } = "0";
        public string PlusJin { get; set; } = "0";
        public int NenryoDai { get; set; }
        public byte ZeiKbn { get; set; }
        public decimal Zeiritsu { get; set; }
        public int SyaRyoSyo { get; set; }
        public string SyuKoTime { get; set; } = "00:00";
        public string SyuPaTime { get; set; } = "00:00";
        public string KikTime { get; set; } = "00:00";
        public string TouChTime { get; set; } = "00:00";
        public string KoskuTime { get; set; } = "00:00";
        public string JisTime { get; set; } = "00:00";
        public string UnkKai { get; set; } = "1";
        public byte SiyoKbn { get; set; }

        public string TotalMile { get; set; } = "0.00";
        public DateTime? HaiSYmd { get; set; } = null;
        public DateTime? TouYmd { get; set; } = null;
        //public string HaiSYmd { get; set; }
        //public string TouYmd { get; set; }
        public string SyaRyoCd { get; set; }
        public string SyaRyoNm { get; set; }
        public string NippoKbn { get; set; }

        public string InspectionTime { get; set; } = "0200";
        public int FeeMaxAmount { get; set; }
        public int FeeMinAmount { get; set; }
        public int FareMaxAmount { get; set; }
        public int FareMinAmount { get; set; }
        public int FareFeeMaxAmount { get; set; }
        public int FareFeeMinAmount { get; set; }

        public bool isUpdate { get; set; } = true;
        public byte totalDays { get; set; }

        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
    }

    public class VehicleDailyReportPDF
    {
        public CurrentBus Bus { get; set; }
        public List<VehicleDailyReportModel> ListData { get; set; }
        public List<VehicleDailyReportChildModel> ListTotal { get; set; }
        public string CurrentDate { get; set; }
        public int PageNumber { get; set; }
        public int TotalPage { get; set; }
        public string StrYmd { get; set; }
        public string EndYmd { get; set; }
    }

    public class VehicleDailyReportHaisha
    {
        public string UkeNo { get; set; }
        public short UnkRen { get; set; }
        public short SyaSyuRen { get; set; }
        public short TeiDanNo { get; set; }
        public short BunkRen { get; set; }
        public int FeeMaxAmount { get; set; }
        public int FeeMinAmount { get; set; }
        public int TransportationPlaceCodeSeq { get; set; }
        public byte KataKbn { get; set; }
        public int BigVehicalMaxUnitPriceforKm { get; set; }
        public int BigVehicalMinUnitPriceforKm { get; set; }
        public int MedVehicalMaxUnitPriceforKm { get; set; }
        public int MedVehicalMinUnitPriceforKm { get; set; }
        public int SmallVehicalMaxUnitPriceforKm { get; set; }
        public int SmallVehicalMinUnitPriceforKm { get; set; }
        public int BigVehicalMaxUnitPriceforHour { get; set; }
        public int BigVehicalMinUnitPriceforHour { get; set; }
        public int MedVehicalMaxUnitPriceforHour { get; set; }
        public int MedVehicalMinUnitPriceforHour { get; set; }
        public int SmallVehicalMaxUnitPriceforHour { get; set; }
        public int SmallVehicalMinUnitPriceforHour { get; set; }

        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
    }

    public class HaitaCheckVehicleDailyReport
    {
        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
        public string HaishaUpdYmd { get; set; }
        public string HaishaUpdTime { get; set; }
    }
}
