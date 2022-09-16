using System.Collections.Generic;
using System;
using System.Globalization;
using System.Linq;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class AccessoryFeeListReportData : AccessoryFeeListExportData
    {
        public string NoText { get { return IsEmptyObject || IsSumRow ? string.Empty : No.ToString(); } }
        
        public string HaiSYmdText
        {
            get
            {
                if(DateTime.TryParseExact(HaiSYmd, "yyyyMMdd", null, DateTimeStyles.None, out DateTime date))
                {
                    return date.ToString("yyyy/MM/dd");
                }
                return string.Empty;
            }
        }
        public string TouYmdText
        {
            get
            {
                if (DateTime.TryParseExact(TouYmd, "yyyyMMdd", null, DateTimeStyles.None, out DateTime date))
                {
                    return date.ToString("yyyy/MM/dd");
                }
                return string.Empty;
            }
        }
        public string HasYmdText
        {
            get
            {
                if (DateTime.TryParseExact(HasYmd, "yyyyMMdd", null, DateTimeStyles.None, out DateTime date))
                {
                    return date.ToString("yyyy/MM/dd");
                }
                return string.Empty;
            }
        }
        public string CodeKbnSeisan { get; set; }
        public string DeRyoText
        {
            get
            {
                if (IsSumRow)
                    return "合計";

                return DeRyoNm;
            }
        }
        public string SuryoText
        {
            get
            {
                if (IsSumRow)
                    return SumSuryo.ToString();
                if (IsEmptyObject)
                    return string.Empty;

                if(int.TryParse(Suryo, out int money))
                {
                    return money.ToString();
                }

                return string.Empty;
            }
        }
        public string TanKaText
        {
            get
            {
                if(int.TryParse(TanKa, out int tanka))
                {
                    return tanka.ToString("N0");
                }
                return string.Empty;
            }
        }
        public string SyaRyoTesText
        {
            get
            {
                if (IsSumRow)
                    return SumTesu.ToString("N0");
                if (IsEmptyObject)
                    return string.Empty;

                if (int.TryParse(SyaRyoTes, out int syaRyoTes))
                {
                    return syaRyoTes.ToString("N0");
                }
                return string.Empty;
            }
        }     
        public string SyaRyoSyoText
        {
            get
            {
                if (IsSumRow)
                    return SumSyohizei.ToString("N0");
                if (IsEmptyObject)
                    return string.Empty;

                if (int.TryParse(SyaRyoSyo, out int syaRyoSyo))
                {
                    return syaRyoSyo.ToString("N0");
                }
                return string.Empty;
            }
        }      
        public string UriGakKinText
        {
            get
            {
                if (IsSumRow)
                    return SumUriGakKin.ToString("N0");
                if (IsEmptyObject)
                    return string.Empty;

                if (int.TryParse(UriGakKin, out int money))
                {
                    return money.ToString("N0");
                }

                return string.Empty;
            }
        }

        public long SumSuryo { get; set; }
        public long SumUriGakKin { get; set; }
        public long SumSyohizei { get; set; }
        public long SumTesu { get; set; }
    }

    public abstract class AccessoryFeeListExportData
    {
        public bool IsSumRow { get; set; } = false;
        public bool IsEmptyObject { get; set; } = false;

        public int No { get; set; }

        private int _gyosyaCd;
        public string GyosyaCd
        {
            get => _gyosyaCd.ToString("D3");
            set
            {
                int.TryParse(value, out _gyosyaCd);
            }
        }

        private int _tokuTokuiCd;
        public string TokuTokuiCd
        {
            get => _tokuTokuiCd.ToString("D4");
            set
            {
                int.TryParse(value, out _tokuTokuiCd);
            }
        }

        private int _toshiSitenCd;
        public string ToshiSitenCd
        {
            get => _toshiSitenCd.ToString("D4");
            set
            {
                int.TryParse(value, out _toshiSitenCd);
            }
        }

        public string GyosyaNm { get; set; }
        public string TokuTokuiNm { get; set; }
        public string TokuRyakuNm { get; set; }
        public string ToshiSitenNm { get; set; }
        public string ToshiRyakuNm { get; set; }

        public string HaiSYmd { get; set; }
        public string TouYmd { get; set; }
        public string BranchName { get; set; }
        public string HasYmd { get; set; }
        public string DanTaNm { get; set; }

        private int _ukeCd;
        public string UkeCdText
        {
            get => (IsEmptyObject || IsSumRow) ? string.Empty : _ukeCd.ToString("D10");
            set
            {
                int.TryParse(value, out _ukeCd);
            }
        }

        public string SeisanNm { get; set; }
        public string FutTumNm { get; set; }
        public string CodeKbnNmSeisan { get; set; }
        public string IriRyoNm { get; set; }
        public string DeRyoNm { get; set; }
        public string Suryo { get; set; }
        public string TanKa { get; set; }
        public string SyaRyoTes { get; set; }
        public string SyaRyoSyo { get; set; }
        public string UriGakKin { get; set; }
        public string BikoNm { get; set; }
    }

    public class AccessoryFeeListReportSearchParams
    {
        public int TenantId { get; set; }
        public int UserLoginId { get; set; }

        public AccessoryFeeListData SearchCondition { get; set; }
    }

    public class AccessoryFeeListReportPagedData
    {
        public bool IsNormalPagging { get; set; } = true;
        public string TokuiSaki
        {
            get
            {
                if (IsNormalPagging)
                    return string.Empty;

                var obj = ReportDatas.FirstOrDefault();
                return $"得意先： {int.Parse(obj.TokuTokuiCd).ToString("D4")}-{int.Parse(obj.ToshiSitenCd).ToString("D4")}-{int.Parse(obj.GyosyaCd).ToString("D3")}: {obj.TokuRyakuNm} {obj.ToshiRyakuNm}";
            }
        }
        public string DateType { get; set; }
        public string DateTypeSearchInfo { get; set; }
        public string CustomerInfo { get; set; }
        public string CompanyInfo { get; set; }
        public string BranchInfo { get; set; }
        public string BookingTypeInfo { get; set; }
        public string UkeCdInfo { get; set; }
        public string InvoiceTypeInfo { get; set; }
        public string ExportTypeInfo { get; set; }
        public string PagingTypeInfo { get; set; }
        public string FutaiInfo { get; set; }
        public string FutaiTypeInfo { get; set; }

        public List<AccessoryFeeListReportData> ReportDatas { get; set; }

        public string PrintedStaffCompanyName { get; set; }
        public string PrintedStaffCD { get; set; }
        public string PrintedStaffName { get; set; }
    }
}
