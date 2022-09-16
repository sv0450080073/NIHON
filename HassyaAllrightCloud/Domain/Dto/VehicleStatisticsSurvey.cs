using HassyaAllrightCloud.Commons.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class VehicleStatisticsSurveySearchParam
    {
        public int OutputInstructionMode { get; set; } = (int)OutputInstruction.Preview;
        public DateTime ProcessingDate { get; set; } = DateTime.Now;
        public CompanyItem Company { get; set; }
        public EigyoItem EigyoFrom { get; set; }
        public EigyoItem EigyoTo { get; set; }
        public ShippingItem ShippingFrom { get; set; }
        public ShippingItem ShippingTo { get; set; }
        public int TenantCdSeq { get; set; } = new ClaimModel().TenantID;
    }

    public class VehicleStatisticsSurveyData
    {
        public byte UnsouKbn { get; set; }
        public string UnsouKbnNm { get; set; }
        public string UnsouKbnRyaku { get; set; }
        public byte NenryoKbn { get; set; }
        public string NenryoKbnNm { get; set; }
        public string NenryoKbnRyaku { get; set; }
        public int YusoJin { get; set; }
        public int NobeSumCnt { get; set; }
        public int NobeJitCnt { get; set; }
        public decimal JitSumKm { get; set; }
        public decimal JitJisaKm { get; set; }
        public decimal JitKisoKm { get; set; }
        public int UnkoCnt { get; set; }
        public int NobeRinCnt { get; set; }
        public int EndOfMonthCnt { get; set; }
        public int LastMonthYusoJin { get; set; }
    }

    public class VehicleStatisticsSurveyPDF
    {
        public string UnsouKbnNm { get; set; }
        public string ProcessingDate { get; set; }
        public int SumYosuJin { get; set; }
        public int SumNobeSumCnt { get; set; }
        public int SumNobeJitCnt { get; set; }
        public int SumJitSumKm { get; set; }
        public int SumJitJisaKm { get; set; }
        public int SumJitKisoKm { get; set; }
        public int SumUnkoCnt { get; set; }
        public int EndOfMonthCnt { get; set; }
        public int SumNobeRinCnt { get; set; }
        public string SumLastMonthYusoJin { get; set; }
        public string SumNobe { get; set; }
        public string SumJit { get; set; }
        public string SumUnko { get; set; }
        public byte Shipping { get; set; }
    }
}
