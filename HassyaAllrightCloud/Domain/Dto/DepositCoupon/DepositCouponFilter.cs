using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto.DepositCoupon
{
    public class DepositCouponFilter
    {
        public DateTime? BillPeriodFrom { get; set; }
        public DateTime? BillPeriodTo { get; set; }
        public BillOfficeData BillOffice { get; set; }
        public CustomerComponentGyosyaData startCustomerComponentGyosyaData { get; set; }
        public CustomerComponentTokiskData startCustomerComponentTokiskData { get; set; }
        public CustomerComponentTokiStData startCustomerComponentTokiStData { get; set; }
        public CustomerComponentGyosyaData endCustomerComponentGyosyaData { get; set; }
        public CustomerComponentTokiskData endCustomerComponentTokiskData { get; set; }
        public CustomerComponentTokiStData endCustomerComponentTokiStData { get; set; }
        public ReservationClassComponentData StartReservationClassification { get; set; }
        public ReservationClassComponentData EndReservationClassification { get; set; }
        public List<int> BillTypes { get; set; }
        public bool itemFare { get; set; }
        public bool itemIncidental { get; set; }
        public bool itemTollFee { get; set; }
        public bool itemArrangementFee { get; set; }
        public bool itemGuideFee { get; set; }
        public bool itemLoaded { get; set; }
        public bool itemCancellationCharge { get; set; }
        public string DepositOutputClassification { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; } = Common.LimitPage;
        public string Code { get; set; }
        public string UkeCd { get; set; }
        public int ActiveV { get; set; }
    }

    public class DepositCouponFilterParam
    {
        public string BillPeriodFrom { get; set; }
        public string BillPeriodTo { get; set; }
        public int? BillOffice { get; set; }
        public string StartBillAddress { get; set; }
        public string EndBillAddress { get; set; }
        public int? StartReservationClassification { get; set; }
        public int? EndReservationClassification { get; set; }
        public string BillTypes { get; set; }
        public string DepositOutputClassification { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; } = Common.LimitPage;
        public short? GyosyaCd { get; set; }
        public short? TokuiCd { get; set; }
        public short? SitenCd { get; set; }
        public string TokuiNm { get; set; }
        public string UkeCd { get; set; }
    }

    public class DepositCouponCode
    {
        public short GyosyaCd { get; set; }
        public short TokuiCd { get; set; }
        public short SitenCd { get; set; }
        public string TokuiNm { get; set; }
        public string Code => $"{GyosyaCd.ToString("D3")}-{TokuiCd.ToString("D4")}-{SitenCd.ToString("D4")} : {TokuiNm}";

    }
}
