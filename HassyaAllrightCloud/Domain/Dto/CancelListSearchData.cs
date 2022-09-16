using HassyaAllrightCloud.Commons.Helpers;
using System;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class CancelListSearchData
    {
        public int RowID { get; set; } = 0;
        public bool IsChecked { get; set; } = false;
        public string UkeNo { get; set; }
        public int UkeCd { get; set; }
        public int UnkRen { get; set; }

        public int TokuiCd { get; set; }
        public string CanCanYmd { get; set; }

        public string TokuiSaki { get; set; }
        public string TokuiTanNm { get; set; }
        public string ShiireSaki { get; set; }

        //
        public string Tokui { get; set; }
        public string Shiire { get; set; }

        public string CancelYmd { get; set; }
        public string CancelYmdText
        {
            get
            {
                bool hadDate = DateTime.TryParseExact(CancelYmd, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime date);
                if (hadDate)
                {
                    return date.ToString("yyyy/MM/dd");
                }
                return string.Empty;
            }
        }
        public string Eigos { get; set; }
        public string InChargeStaff { get; set; }

        public string BookingName { get; set; }
        public string CancelReason { get; set; }

        public string FixedDate { get; set; }
        public string FixedDateText
        {
            get
            {
                bool hadDate = DateTime.TryParseExact(FixedDate, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime date);
                if (hadDate)
                {
                    return date.ToString("yyyy/MM/dd");
                }
                return string.Empty;
            }
        }
        public string Status { get; set; }
        public string Cancel { get; set; }

        public string BookingAmount { get; set; }
        public string BookingAmountText => int.Parse(BookingAmount).ToString("N0");
        public string TaxRate { get; set; }
        public string TaxRateText => TaxRate + "%";
        public string TaxAmount { get; set; }
        public string TaxAmountText => int.Parse(TaxAmount).ToString("N0");
        public string ChargeRate { get; set; }
        public string ChargeRateText => ChargeRate + "%";
        public string ChargeAmount { get; set; }
        public string ChargeAmountText => int.Parse(ChargeAmount).ToString("N0");

        public string CancelRate { get; set; }
        public string CancelRateText => CancelRate + "%";
        public string CancelFee { get; set; }
        public string CancelFeeText => int.Parse(CancelFee).ToString("N0");

        public string CancelTaxRate { get; set; }
        public string CancelTaxRateText => CancelTaxRate + "%";
        public string CancelTaxFee { get; set; }
        public string CancelTaxFeeText => int.Parse(CancelTaxFee).ToString("N0");
        public string HaiSYmd { get; set; }
        public string HaiSYmdText
        {
            get
            {
                bool hadDate = DateTime.TryParseExact(HaiSYmd, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime date);
                if (hadDate)
                {
                    return date.ToString("yyyy/MM/dd");
                }
                return string.Empty;
            }
        }

        public string HaiSTime { get; set; }
        public string HaiSTimeText => CommonUtil.ConvertMyTimeStrToDefaultFormat(HaiSTime);
        public string HaiSNm { get; set; }
        public string HaiSTimeTextGrid
        {
            get
            {
                return HaiSYmdText + " " + CommonUtil.ConvertMyTimeStrToDefaultFormat(HaiSTime) + " " + HaiSNm;
            }
        }
        public string TouYmd { get; set; }
        public string TouYmdText
        {
            get
            {
                bool hadDate = DateTime.TryParseExact(TouYmd, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime date);
                if (hadDate)
                {
                    return date.ToString("yyyy/MM/dd");
                }
                return string.Empty;
            }
        }
        public string TouChTime { get; set; }
        public string TouChTimeText => CommonUtil.ConvertMyTimeStrToDefaultFormat(TouChTime);
        public string TouNm { get; set; }
        public string TouChTimeTextGrid
        {
            get
            {
                return TouYmdText + " " + CommonUtil.ConvertMyTimeStrToDefaultFormat(TouChTime) + " " + TouNm;
            }
        }
        public string Driver { get; set; }
        public string Guide { get; set; }
        public string DGInfo
        {
            get
            {
                return $"運転手 {Driver}名   ガイド {Guide}名";
            }
        }
        public string DanTaNm { get; set; }
        public string KanJNm { get; set; }
        public string IkNm { get; set; }
        public string BusTypeName { get; set; }

        public string BusQuantity { get; set; }

        public string Passenger { get; set; }
        public string PlusJin { get; set; }

        public string InvoiceIssueDate { get; set; }
        public string InvoiceIssueDateText
        {
            get
            {
                bool hadDate = DateTime.TryParseExact(InvoiceIssueDate, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime date);
                if (hadDate)
                {
                    return date.ToString("yyyy/MM/dd");
                }
                return string.Empty;
            }
        }

        public string ReceivedBranch { get; set; }

        public string InChargeStaff2 { get; set; }

        public string InputBy { get; set; }

        public string BookingStatus { get; set; }

        public string BookingType { get; set; }

        public string UkeYmd { get; set; }
        public string UkeYmdText
        {
            get
            {
                bool hadDate = DateTime.TryParseExact(UkeYmd, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime date);
                if (hadDate)
                {
                    return date.ToString("yyyy/MM/dd");
                }
                return string.Empty;
            }
        }

        public List<BusViewData> BusViewDatas { get; set; } = new List<BusViewData>();

        public override bool Equals(object obj)
        {
            try
            {
                var o = ((CancelListSearchData)obj);
                return this.UkeNo == o.UkeNo && this.UnkRen == o.UnkRen;
            }
            catch
            {
                return false;
            }
        }
        public override int GetHashCode()
        {
            //Get the ID hash code value
            int IDHashCode = this.UkeNo.GetHashCode();
            int UnkRenHashCode = this.UnkRen.GetHashCode();
            //Get the string HashCode Value
            return IDHashCode ^ UnkRenHashCode;
        }
        public string EmptyValue { get; set; }
        public string BookingAmountTextGrid
        {
            get
            {
                return string.Format("<div class='d-flex justify-content-between'><span>{0}</span><span class='text-right'>{1}</span></div>", "", BookingAmountText);
            }
        }
        public string TaxAmountTextGrid
        {
            get
            {
                return string.Format("<div class='d-flex justify-content-between'><span>{0}</span><span class='text-right'>{1}</span></div>", TaxRateText, TaxAmountText);
            }
        }
        public string ChargeAmountTextGrid
        {
            get
            {
                return string.Format("<div class='d-flex justify-content-between'><span>{0}</span><span class='text-right'>{1}</span></div>", ChargeRateText, ChargeAmountText);
            }
        }
        public string CancelFeeTextGrid
        {
            get
            {
                return string.Format("<div class='d-flex justify-content-between'><span>{0}</span><span class='text-right'>{1}</span></div>", CancelRateText, CancelFeeText);
            }
        }
        public string CancelTaxFeeTextGrid
        {
            get
            {
                return string.Format("<div class='d-flex justify-content-between'><span>{0}</span><span class='text-right'>{1}</span></div>", CancelTaxRateText, CancelTaxFeeText);
            }
        }
        public string PassengerText
        {
            get
            {
                return Passenger+ " 人";
            }
        }
        public string PlusJinText
        {
            get
            {
                return PlusJin + " 人";
            }
        }
        public string BookingStatusText
        {
            get
            {
                return BookingType +" "+ BookingStatus;
            }
        }
        public string UkeCdText
        {
            get
            {
                return UkeCd.ToString("D10");
            }
        }

    }
}
