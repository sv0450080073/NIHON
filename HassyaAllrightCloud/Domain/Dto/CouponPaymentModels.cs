using HassyaAllrightCloud.Commons;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class CouponPaymentFormModel : Paging
    {
        public DateTime? StartIssuePeriod { get; set; }
        public DateTime? EndIssuePeriod { get; set; }
        public EigyoListItem DepositOffice { get; set; }
        public ReservationClassComponentData StartReservationClassificationSort { get; set; }
        public ReservationClassComponentData EndReservationClassificationSort { get; set; }
        public DepositOutputClassification DepositOutputClassification { get; set; }
        public List<CodeKbDataItem> BillTypes { get; set; }
        public BillAddressItem BillAddress { get; set; }
        public CustomerComponentGyosyaData SelectedGyosyaFrom { get; set; }
        public CustomerComponentTokiskData SelectedTokiskFrom { get; set; }
        public CustomerComponentTokiStData SelectedTokiStFrom { get; set; }
        public CustomerComponentGyosyaData SelectedGyosyaTo { get; set; }
        public CustomerComponentTokiskData SelectedTokiskTo { get; set; }
        public CustomerComponentTokiStData SelectedTokiStTo { get; set; }
        public int Total { get; set; }
        public int TenantCdSeq { get; set; }
        public string UkeNo { get; set; }
    }

    public class Paging
    {
        public int PageNum { get; set; }
        public int PageSize { get; set; }
    }

    public class CouponPaymentGridItem
    {
        public bool IsChecked { get; set; }
        public long No { get; set; }
        public string HakoYmd { get; set; }
        public string CouNo { get; set; }
        public string SeiTaiYmd { get; set; }
        public string UkeRyakuNm { get; set; }
        public string TokRyakuNm { get; set; }
        public string SitRyakuNm { get; set; }
        public string UkeNo { get; set; }
        public short UnkRen { get; set; }
        public byte SeiFutSyu { get; set; }
        public short FutTumRen { get; set; }
        public int YouTblSeq { get; set; }
        public string YoyaKbnNm { get; set; }
        public string DanTaNm { get; set; }
        public string IkNm { get; set; }
        public string HaiSYmd { get; set; }
        public string HaiSTime { get; set; }
        public DateTime? HaiS
        {
            get
            {
                if (HaiSYmd.IsNullOrEmptyOrWhiteSpace()) return null;
                if (DateTime.TryParseExact($"{HaiSYmd} {(HaiSTime.IsNullOrEmptyOrWhiteSpace() ? "000001" : HaiSTime)}",
                    $"{DateTimeFormat.yyyyMMdd} {DateTimeFormat.HHmmss}", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                {
                    return date;
                }
                else return null;
            }
        }
        public string TouYmd { get; set; }
        public string TouChTime { get; set; }
        public DateTime? Tou
        {
            get
            {
                if (TouYmd.IsNullOrEmptyOrWhiteSpace()) return null;
                if (DateTime.TryParseExact($"{TouYmd} {(TouChTime.IsNullOrEmptyOrWhiteSpace() ? "000001" : TouChTime)}",
                    $"{DateTimeFormat.yyyyMMdd} {DateTimeFormat.HHmmss}", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                {
                    return date;
                }
                else return null;
            }
        }
        public string SeiFutSyuNm { get; set; }
        public string FutTumNm { get; set; }
        public int CouKesG { get; set; }
        public string CouGkin { get; set; }
        public string LastNyuYmd { get; set; }
        public int NyuKinRui { get; set; }
        public short GyosyaCd { get; set; }
        public string GyosyaNm { get; set; }
        public short TokuiCd { get; set; }
        public short SitenCd { get; set; }
        public string SeiFutSyuCd { get; set; }
        public int GridUnpaidAmount { get => CouKesG - NyuKinRui; }
        public byte NyuKinKbn { get; set; }
        public short NyuSihCouRen { get; set; }
        public string SeiRyakuNm { get; set; }
        public string SeiSitRyakuNm { get; set; }
        public int CouTblSeq { get; set; }
    }

    public class GeneralInfo
    {
        public int TotalFare { get; set; }
        public int TotalIncidental { get; set; }
        public int TotalTollFee { get; set; }
        public int TotalArrangementFee { get; set; }
        public int TotalGuideFee { get; set; }
        public int LoadedGoods { get; set; }
        public int TotalCancelFee { get; set; }
        public int Total { get => TotalFare + TotalIncidental + TotalTollFee + TotalArrangementFee + TotalGuideFee + LoadedGoods + TotalCancelFee; }
    }

    public class DepositOutputClassification
    {
        public DepositOutputClassificationEnum Value { get; set; }
        public string DisplayName { get; set; }
    }
    public class CouponPaymentSummary
    {
        public int TotalPageCouponApplicationAmount { get; set; }
        public int TotalPageCumulativeDeposit { get; set; }
        public int TotalPageUnpaidAmount { get; set; }
        public int TotalAllCouponApplicationAmount { get; set; }
        public int TotalAllCumulativeDeposit { get; set; }
        public int TotalAllUnpaidAmount { get; set; }
    }

    public class BillAddressItem
    {
        public string DisplayName { get; set; }
        public short GyosyaCd { get; set; }
        public short TokuiCd { get; set; }
        public short SitenCd { get; set; }
    }

    public class YoyaKbnItem
    {
        public string DisplayName
        {
            get
            {
                return YoyaKbnNm;
            }
        }
        public byte YoyaKbn { get; internal set; }
        public string YoyaKbnNm { get; internal set; }
    }

    public enum DepositOutputClassificationEnum
    {
        ReceivableOnly,
        Deposited
    }

    public class LastUpdatedYmdTimeMulti
    {
        public string UkeNo { get; set; }
        public int NyuSihCouRen { get; set; }
        public CommonLastUpdatedYmdTime LastUpdatedYmdTime { get; set; }
        //public List<SpecLastUpdatedYmdTime> LastUpdatedYmdTimeList { get; set; }
    }

    public class CommonLastUpdatedYmdTime
    {
        public string MishumUpdYmd { get; set; }
        public string MishumUpdTime { get; set; }
        public string YykshoUpdYmd { get; set; }
        public string YykshoUpdTime { get; set; }
        public string FuttumUpdYmd { get; set; }
        public string FuttumUpdTime { get; set; }
    }

    public class SpecLastUpdatedYmdTime
    {
        public int NyuSihTblSeq { get; set; }
        public int NyuSihRen { get; set; }
        public string NyuSihUpdYmd { get; set; }
        public string NyuSihUpdTime { get; set; }
        public string NyShmiUpdYmd { get; set; }
        public string NyShmiUpdTime { get; set; }
    }
}
