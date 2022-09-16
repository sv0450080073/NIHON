using HassyaAllrightCloud.Commons.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class FaresUpperAndLowerLimitsFormSearch
    {
        public DateClassification DateClassification { get; set; }
        public DateTime? OutputStartDate { get; set; } = null;
        public DateTime? OutputEndDate { get; set; } = null;
        public SaleOffice SaleOffice { get; set; }
        public int Range { get; set; }
        public ItemOutOfRange ItemOutOfRange { get; set; }
        public string ReservationNumberStart { get; set; }
        public string ReservationNumberEnd { get; set; }
        public SalePersonInCharge SalePersonInCharge { get; set; }
        public int CauseInput { get; set; }
        public ChooseCause ChooseCause { get; set; }
        public OutputInstruction OutputSetting { get; set; }
        public string FormSetting { get; set; }
        public string OutputWithHeader { get; set; }
        public string KukuriKbn { get; set; }
        public string KugiriCharType { get; set; }
        public int ActiveV { get; set; }
        public int ActiveL { get; set; }
    }

    public class SaleOffice
    {
        public int EigyoCdSeq { get; set; }
        public int EigyoCd { get; set; }
        public string RyakuNm { get; set; }
        public string Text => EigyoCd > 0 ? $"{string.Format("{0:D4}", EigyoCd)}：{RyakuNm}" : "";
    }

    public class SalePersonInCharge
    {
        public int SyainCdSeq { get; set; }
        public string SyainCd { get; set; }
        public string SyainNm { get; set; }
        public int EigyoCdSeq { get; set; }
        public string TenkoNo { get; set; }
        public string StaYmd { get; set; }
        public string EndYmd { get; set; }
        public string Text => $"{SyainCd} : {SyainNm}";
    }

    public class Cause
    {
        public string CodeSyu { get; set; }
        public short CodeKbn { get; set; }
        public string CodeKbnNm { get; set; }
    }

    public class CommonListCombobox
    {
        public List<SaleOffice> SaleOffices { get; set; }
        public List<SalePersonInCharge> SalePersonInCharges { get; set; }
    }

    public class FaresUpperAndLowerLimitObject
    {
        public string UkeNo { get; set; }
        public short UnkRen { get; set; }
        public short SyaSyuRen { get; set; }
        public short TeiDanNo { get; set; }
        public string SyainCd { get; set; }
        public int SyaRyoCdSeq { get; set; }
        public string SyaryoNm { get; set; }
        public int SyaRyoCd { get; set; }
        public string KariSyaRyoNm { get; set; }
        public short SyaSyuCd { get; set; }
        public string SyaSyuNm { get; set; }
        public byte KataKbn { get; set; }
        public string HaiSYmd { get; set; }
        public string TouYmd { get; set; }
        public string SyukoYmd { get; set; }
        public string KikYmd { get; set; }
        public short UpperLowerCauseKbn { get; set; }
        public string CodeKbnNm { get; set; }
        public int SeikyuGaku { get; set; }
        public decimal StMeter { get; set; }
        public decimal EndMeter { get; set; }
        public string KoskuTime { get; set; }
        public string InspectionTime { get; set; }
        public decimal RunningKmSum { get; set; }
        public string RestraintTimeSum { get; set; }
        public int JissekiSumMaxAmount { get; set; }
        public int JissekiSumMinAmount { get; set; }
        public int MitsumoriSumMaxAmount { get; set; }
        public int MitsumoriSumMinAmount { get; set; }
        public byte ChangeDriverFeeFlag { get; set; }
        public byte SpecialFlg { get; set; }
        public int DisabledPersonDiscount { get; set; }
        public int SchoolDiscount { get; set; }
        public int EigyoCdSeq { get; set; }
        public int EigyoCd { get; set; }
        public string EigyoRyakuNm { get; set; }
        public short BunkRen { get; set; }
        public string CauseNm { get; set; }
    }

    public class FaresUpperAndLowerLimitGrid
    {
        public int GridNo { get; set; }
        public string GridReservationNumber { get; set; }
        public string GridOperationYmd { get; set; }
        public string GridVehicleName { get; set; }
        public string GridPlan { get; set; }
        public string GridActualResult { get; set; }
        public string GridPlanMinAmount { get; set; }
        public string GridPlanMaxAmount { get; set; }
        public string GridActualMinAmount { get; set; }
        public string GridActualMaxAmount { get; set; }
        public string GridBillingAmount { get; set; }
        public string GridCause { get; set; }
        public string GridPlanRunningKilomet { get; set; }
        public string GridPlanTotalTime { get; set; }
        public string GridActualRunningKilomet { get; set; }
        public string GridActualTotalTime { get; set; }
        public string GridChangedDriver { get; set; }
        public string GridSpecialVehicle { get; set; }
        public string GridDisabledPersonDiscount { get; set; }
        public string GridSchoolDiscount { get; set; }
        public string CssClass { get; set; }
        public short UnkRenGrid { get; set; }
        public short TeiDanNoGrid { get; set; }
        public short SyaSyuRenGrid { get; set; }
        public short BunkRenGrid { get; set; }
    }

    public class FaresUpperAndLowerLimitCsv
    {
        public string GridReservationNumber { get; set; }
        public string DateDispatch { get; set; }
        public string DateArrival { get; set; }
        public string GridVehicleName { get; set; }
        public string GridPlanMinAmount { get; set; }
        public string GridPlanMaxAmount { get; set; }
        public string GridActualMinAmount { get; set; }
        public string GridActualMaxAmount { get; set; }
        public string GridBillingAmount { get; set; }
        public string GridCause { get; set; }
        public string GridPlanRunningKilomet { get; set; }
        public string GridPlanTotalTime { get; set; }
        public string GridActualRunningKilomet { get; set; }
        public string GridActualTotalTime { get; set; }
        public string GridChangedDriver { get; set; }
        public string GridSpecialVehicle { get; set; }
        public string GridDisabledPersonDiscount { get; set; }
        public string GridSchoolDiscount { get; set; }
    }

    public class FaresUpperAndLowerLimitReport
    {
        public List<FaresUpperAndLowerLimitGrid> FaresUpperAndLowerLimitGrid { get; set; }
        public string SaleOfficeName { get; set; }
        public string OutputStartEndDate { get; set; }
        public string CurrentDate { get; set; }
    }

    public partial class TkdMaxMinFareFeeCauseParam
    {
        public string UkeNo { get; set; }
        public short UnkRen { get; set; }
        public short TeiDanNo { get; set; }
        public short BunkRen { get; set; }
    }

    public enum FormSetting
    {
        [Description("A4")]
        A4 = 1,
        [Description("A3")]
        A3 = 2,
        [Description("B4")]
        B4 = 3
    }

    public enum OutputWithHeader
    {
        [Description("出力")]
        Output = 1,
        [Description("未出力")]
        NoOutput = 2,
    }

    public enum KukuriKbn
    {
        [Description("「”」で括る")]
        EncloseIn = 1,
        [Description("「”」で括らない")]
        NoEncloseIn = 2,
    }

    public enum KugiriCharType
    {
        [Description("タブ")]
        Tab = 1,
        [Description("セミコロン")]
        Semicolon = 2,
        [Description("カンマ")]
        Comma = 2,
    }
}
