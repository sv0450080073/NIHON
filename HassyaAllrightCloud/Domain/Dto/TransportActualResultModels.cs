using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class TransportActualResultSearchModel
    {
        public CompanyListItem Company { get; set; }
        public EigyoListItem EigyoFrom { get; set; }
        public EigyoListItem EigyoTo{ get; set; }
        public OutputType OutputType { get; set; }
        public CodeKbDataItem ShippingFrom { get; set; }
        public CodeKbDataItem ShippingTo { get; set; }
        public DateTime? ProcessingYear { get; set; }
    }

    public class ReportSearchModel
    {
        public int ProcessingYear { get; set; }
        public int Company { get; set; }
        public int? EigyoFrom { get; set; }
        public int? EigyoTo { get; set; }
        public string ShippingFrom { get; set; }
        public string ShippingTo { get; set; }
        public int CurrentTenantId { get; set; }
        public Guid ReportId { get; set; }
    }

    public class CodeKbDataItem
    {
        public string CodeKbn { get; internal set; }
        public string CodeKbnNm { get; internal set; }
        public string DisplayName
        {
            get
            {
                return $"{CodeKbn.PadLeft(2,'0')} : {CodeKbnNm}";
            }
        }

    }

    public class TransportActualResultSPModel
    {
        public byte UnsouKbn { get; set; }
        public string UnsouKbnNm { get; set; }
        public string UnsouKbnRyaku { get; set; }
        public int TotalNobeJyoCnt { get; set; }
        public int TotalNobeRinCnt { get; set; }
        public int TotalNobeSumCnt { get; set; }
        public int TotalNobeJitCnt { get; set; }
        public decimal TotalJitJisaKm { get; set; }
        public decimal TotalJitKisoKm { get; set; }
        public decimal TotalJitSumKm { get; set; }
        public int TotalYusoJin { get; set; }
        public int TotalUnkoCnt { get; set; }
        public int TotalUnkoKikak1Cnt { get; set; }
        public int TotalUnkoKikak2Cnt { get; set; }
        public int TotalUnkoOthCnt { get; set; }
        public int TotalUnkoOthAllCnt { get; set; }
        public decimal TotalUnsoSyu { get; set; }
    }

    public class TransportActualResultReportData
    {
        public string UnsouKbnNm { get; set; }
        public string ProcessingYear { get; set; }
        public string BusinessYmd { get; set; }
        public string JigyoCarSumCnt { get; set; }
        public string NobeSumCnt { get; set; }
        public string NobeJitCnt { get; set; }
        public string JitSumKm { get; set; }
        public string JitJisaKm { get; set; }
        public string YusoJin { get; set; }
        public string UnkoCnt { get; set; }
        public string UnkoOthAllCnt { get; set; }
        public string UnsoSyu { get; set; }

    }

    public class HenSyaSearchModel
    {
        public string ProcessingYear { get; set; }
        public int? EigyoFrom { get; set; }
        public int? EigyoTo { get; set; }
    }
}
