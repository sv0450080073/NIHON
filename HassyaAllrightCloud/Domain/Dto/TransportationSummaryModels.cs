using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using System;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class SummaryTableResult
    {
        public string SyoriYm { get; set; }
        public int? CompanyCd { get; set; }
        public string CompanyNm { get; set; }
        public int? EigyoCd { get; set; }
        public string EigyoNm { get; set; }
        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
    }

    public class CompanyListItem
    {
        public int CompanyCd { get; internal set; }
        public string RyakuNm { get; internal set; }
        public int CompanyCdSeq { get; internal set; }
        public string DisplayName
        {
            get
            {
                return $"{CompanyCd.AddPaddingLeft(5)} : {RyakuNm}";
            }
        }

        public int TenantCdSeq { get; internal set; }
    }

    public class EigyoListItem
    {
        public int EigyoCdSeq { get; set; }
        public int EigyoCd { get; set; }
        public string RyakuNm { get; set; }
        public string DisplayName
        {
            get
            {
                return $"{EigyoCd.AddPaddingLeft(5)} : {RyakuNm}";
            }
        }
    }

    public class TransportationSummarySearchModel
    {
        public DateTime ProcessingDate { get; set; }
        public CompanyListItem Company { get; set; }
        public EigyoListItem EigyoFrom { get; set; }
        public EigyoListItem EigyoTo { get; set; }
        public int UnsoKbn { get; set; }

        public CustomerComponentGyosyaData SelectedGyosya { get; set; }
        public CustomerComponentTokiskData SelectedTokisk { get; set; }
        public CustomerComponentTokiStData SelectedTokiSt { get; set; }
        public ReservationClassComponentData SelectedReservationClass { get; set; }
    }

    public class TransportationSummaryItem
    {
        public string ProcessingDate { get; set; }
        public IEnumerable<Company> Companies { get; set; }
    }

    public class Company
    {
        public string CompanyName { get; set; }
        public IEnumerable<Eigyo> Eigyos { get; set; }
    }

    public class Eigyo
    {
        public string EigyoName { get; set; }
        public string UpdateDate { get; set; }
    }
}
