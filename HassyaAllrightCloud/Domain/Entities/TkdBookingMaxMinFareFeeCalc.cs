// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Domain.Entities
{
    public partial class TkdBookingMaxMinFareFeeCalc
    {
        public string UkeNo { get; set; }
        public short UnkRen { get; set; }
        public short SyaSyuRen { get; set; }
        public int TransportationPlaceCodeSeq { get; set; }
        public byte KataKbn { get; set; }
        public byte ZeiKbn { get; set; }
        public decimal Zeiritsu { get; set; }
        public decimal RunningKmSum { get; set; }
        public decimal RunningKmCalc { get; set; }
        public string RestraintTimeSum { get; set; }
        public string RestraintTimeCalc { get; set; }
        public decimal ServiceKmSum { get; set; }
        public string ServiceTimeSum { get; set; }
        public string MidnightEarlyMorningTimeSum { get; set; }
        public string MidnightEarlyMorningTimeCalc { get; set; }
        public decimal ChangeDriverRunningKmSum { get; set; }
        public decimal ChangeDriverRunningKmCalc { get; set; }
        public string ChangeDriverRestraintTimeSum { get; set; }
        public string ChangeDriverRestraintTimeCalc { get; set; }
        public string ChangeDriverMidnightEarlyMorningTimeSum { get; set; }
        public string ChangeDriverMidnightEarlyMorningTimeCalc { get; set; }
        public byte WaribikiKbn { get; set; }
        public byte AnnualContractFlag { get; set; }
        public byte SpecialFlg { get; set; }
        public int FareMaxAmount { get; set; }
        public int FareMinAmount { get; set; }
        public int FeeMaxAmount { get; set; }
        public int FeeMinAmount { get; set; }
        public int UnitPriceMaxAmount { get; set; }
        public int UnitPriceMinAmount { get; set; }
        public int FareMaxAmountforKm { get; set; }
        public int FareMinAmountforKm { get; set; }
        public int FareMaxAmountforHour { get; set; }
        public int FareMinAmountforHour { get; set; }
        public int ChangeDriverFareMaxAmountforKm { get; set; }
        public int ChangeDriverFareMinAmountforKm { get; set; }
        public int ChangeDriverFareMaxAmountforHour { get; set; }
        public int ChangeDriverFareMinAmountforHour { get; set; }
        public int MidnightEarlyMorningFeeMaxAmount { get; set; }
        public int MidnightEarlyMorningFeeMinAmount { get; set; }
        public int SpecialVehicalFeeMaxAmount { get; set; }
        public int SpecialVehicalFeeMinAmount { get; set; }
        public decimal? FareIndex { get; set; }
        public decimal? FeeIndex { get; set; }
        public decimal? UnitPriceIndex { get; set; }
        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
        public int UpdSyainCd { get; set; }
        public string UpdPrgId { get; set; }
    }
}