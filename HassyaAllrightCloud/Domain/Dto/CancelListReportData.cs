using HassyaAllrightCloud.Commons.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class CancelListReportData : CancelListExportData
    {
        public string UkeCdText
        {
            get
            {
                if (IsEmptyObject || IsReplaceItem)
                    return string.Empty;

                if (int.TryParse(UkeCd, out int ukeCd))
                {
                    return ukeCd.ToString("D10");
                }

                return string.Empty;
            }
        }
        public string UkeYmdText
        {
            get
            {
                if (IsEmptyObject || IsReplaceItem)
                    return string.Empty;

                if (DateTime.TryParseExact(UkeYmd, "yyyyMMdd", null, DateTimeStyles.None, out DateTime date))
                {
                    return date.ToString("yyyy/MM/dd");
                }

                return string.Empty;
            }
        }

        public string CancelYmdText
        {
            get
            {
                if (IsEmptyObject || IsReplaceItem)
                    return string.Empty;

                if (DateTime.TryParseExact(CancelYmd, "yyyyMMdd", null, DateTimeStyles.None, out DateTime date))
                {
                    return date.ToString("yyyy/MM/dd");
                }

                return string.Empty;
            }
        }

        public string DriverGuiderInfo
        {
            get
            {
                if (IsEmptyObject || IsReplaceItem)
                    return string.Empty;

                return $"運　{DrvJin}人/ガ　{GuiSu}人";
            }
        }

        public string KaktYmdText
        {
            get
            {
                if (IsEmptyObject || IsReplaceItem)
                    return string.Empty;

                if (DateTime.TryParseExact(KaktYmd, "yyyyMMdd", null, DateTimeStyles.None, out DateTime date))
                {
                    return date.ToString("yyyy/MM/dd");
                }

                return string.Empty;
            }
        }
        public string Status
        {
            get
            {
                if (IsEmptyObject || IsReplaceItem)
                    return string.Empty;

                return string.IsNullOrWhiteSpace(KaktYmd) ? (HaiSkbn == 1 ? "確定前" : (HaiSkbn == 2 ? "配車済" : "一部配車済")) : "確定済";
            }
        }
        public int HaiSkbn { get; set; }
        public string FixedText { get; set; }

        public string CancelFeeText
        {
            get
            {
                if (IsEmptyObject || IsReplaceItem)
                    return string.Empty;

                if (int.TryParse(CancelFee, out int money))
                    return money.ToString("N0");
                return string.Empty;
            }
        }
        public string CancelTaxText
        {
            get
            {
                if (IsEmptyObject || IsReplaceItem)
                    return string.Empty;

                if (int.TryParse(CancelTax, out int money))
                    return money.ToString("N0");
                return string.Empty;
            }
        }

        public string HaiSDateTime
        {
            get
            {
                if (IsEmptyObject || IsReplaceItem)
                    return string.Empty;

                if (DateTime.TryParseExact(HaiSYmd, "yyyyMMdd", null, DateTimeStyles.None, out DateTime date))
                {
                    return date.ToString("yyyy/MM/dd") + " " + CommonUtil.ConvertMyTimeStrToDefaultFormat(HaiSTime);
                }

                return string.Empty;
            }
        }
        public string TouChDateTime
        {
            get
            {
                if (IsEmptyObject || IsReplaceItem)
                    return string.Empty;

                if (DateTime.TryParseExact(TouYmd, "yyyyMMdd", null, DateTimeStyles.None, out DateTime date))
                {
                    return date.ToString("yyyy/MM/dd") + " " + CommonUtil.ConvertMyTimeStrToDefaultFormat(HaiSTime);
                }

                return string.Empty;
            }
        }

        public string SyaSyuDaiText
        {
            get
            {
                if (IsEmptyObject)
                    return string.Empty;
                return SyaSyuDai + "台" ?? string.Empty;
            }
        }

        public string UntKinText
        {
            get
            {
                if (IsEmptyObject || IsReplaceItem)
                    return string.Empty;

                if (int.TryParse(UntKin, out int money))
                    return money.ToString("N0");
                return string.Empty;
            }
        }
        public string ZeiRuiText
        {
            get
            {
                if (IsEmptyObject || IsReplaceItem)
                    return string.Empty;

                if (int.TryParse(ZeiRui, out int money))
                    return money.ToString("N0");
                return string.Empty;
            }
        }
        public string TesuRyoGText
        {
            get
            {
                if (IsEmptyObject || IsReplaceItem)
                    return string.Empty;

                if (int.TryParse(TesuRyoG, out int money))
                    return money.ToString("N0");
                return string.Empty;
            }
        }
        public string SeiHatYmdText
        {
            get
            {
                if (IsEmptyObject || IsReplaceItem)
                    return string.Empty;
                if (DateTime.TryParseExact(SeiHatYmd, "yyyyMMdd", null, DateTimeStyles.None, out DateTime date))
                {
                    return date.ToString("yyyy/MM/dd");
                }

                return "未発行";
            }
        }

        public override bool Equals(object obj)
        {
            try
            {
                var o = ((CancelListReportData)obj);
                return this.UkeCd == o.UkeCd && this.UnkRen == o.UnkRen;
            }
            catch
            {
                return false;
            }
        }
        public override int GetHashCode()
        {
            //Get the ID hash code value
            int IDHashCode = this.UkeCd?.GetHashCode() ?? string.Empty.GetHashCode(); ;
            int UnkRenHashCode = this.UnkRen.GetHashCode();
            //Get the string HashCode Value
            return IDHashCode ^ UnkRenHashCode;
        }
    }

    public abstract class CancelListExportData
    {
        public bool IsEmptyObject { get; set; } = false;
        public bool IsReplaceItem { get; set; } = false;

        private int _gyosyaCd;
        public string GyosyaCd
        {
            get => _gyosyaCd.ToString("D3");
            set
            {
                int.TryParse(value, out _gyosyaCd);
            }
        }
        public string GyosyaNm { get; set; }

        private int _tokuTokuiCd;
        public string TokuTokuiCd
        {
            get => _tokuTokuiCd.ToString("D4");
            set
            {
                int.TryParse(value, out _tokuTokuiCd);
            }
        }
        public string TokuTokuiNm { get; set; }
        public string TokuRyakuNm { get; set; }

        private int _toshiSitenCd;
        public string ToshiSitenCd
        {
            get => _toshiSitenCd.ToString("D4");
            set
            {
                int.TryParse(value, out _toshiSitenCd);
            }
        }
        public string ToshiSitenNm { get; set; }
        public string ToshiRyakuNm { get; set; }
        public string TokuiTanStaff { get; set; }

        public string GosaCd { get; set; }
        public string GosaNm { get; set; }

        private int _tokiTokuiCd;
        public string TokiTokuiCd
        {
            get => _tokiTokuiCd.ToString("D4");
            set
            {
                int.TryParse(value, out _tokiTokuiCd);
            }
        }
        public string TokiTokuiNm { get; set; }
        public string TokiRyakuNm { get; set; }

        private int _toshitenCd;
        public string ToshitenCd
        {
            get => _toshitenCd.ToString("D4");
            set
            {
                int.TryParse(value, out _toshitenCd);
            }
        }
        public string ToshitenNm { get; set; }
        public string ToshitenRyakuNm { get; set; }

        public string CancelYmd { get; set; }

        private int _eigyoCd;
        public string EigyoCd
        {
            get => _eigyoCd.ToString("D5");
            set
            {
                int.TryParse(value, out _eigyoCd);
            }
        }
        public string CanTanEigyoRyakuNm { get; set; }
        public string EgosRyakuNm { get; set; }

        public int _canTanSyainCd;
        public string CanTanSyainCd
        {
            get => _canTanSyainCd.ToString("D10");
            set
            {
                int.TryParse(value, out _canTanSyainCd);
            }
        } 
        public string CanTanSyainNm { get; set; }

        public string YoyaNm { get; set; }
        public string CanRiy { get; set; }
        public string DrvJin { get; set; }
        public string GuiSu { get; set; }
        public string HaiIKbn { get; set; }

        public string KaktYmd { get; set; }
        public string KaknKais { get; set; }
        public string ShuSyaRyoUnc { get; set; }
        public string ZeiKbn { get; set; }//ZEIKBN code
        public string ZeiRyakuNm { get; set; }//ZEIKBN ryakunm

        public string Zeiritsu { get; set; }
        private string _zeiRui;
        public string ZeiRui
        {
            get => IsReplaceItem ? string.Empty : _zeiRui;
            set => _zeiRui = value;
        }
        private string _tesuRitu;
        public string TesuRitu
        {
            get => IsReplaceItem ? string.Empty : _tesuRitu;
            set => _tesuRitu = value;
        }
        public string TesuRyoG { get; set; }
        public string CanRit { get; set; }

        public string CancelFee { get; set; }
        public string CanZKbn { get; set; }//CANZKBN code
        public string CanZRyakuNm { get; set; }//CANZKBN ryakunm

        public string CanSyoR { get; set; }
        public string CancelTax { get; set; }

        public string HaiSYmd { get; set; }
        public string HaiSTime { get; set; }
        public string HaiSNm { get; set; }

        public string TouYmd { get; set; }
        public string TouChTime { get; set; }
        public string TouNm { get; set; }

        public string DanTaNm { get; set; }
        public string KanJNm { get; set; }
        public string IkNm { get; set; }

        public string SyaSyuCd { get; set; }
        public string SyaSyuNm { get; set; }
        public string KataCode { get; set; }//KATAKBN code
        public string KataKbn { get; set; }//KATAKBN ryakunm
        public string SyaSyuDai { get; set; }

        public string JyoSyaJin { get; set; }
        public string PlusJin { get; set; }
        public string SeiHatYmd { get; set; }

        private int _ukeEigyoCd;
        public string UkeEigyoCd
        {
            get => _ukeEigyoCd.ToString("D5");
            set
            {
                int.TryParse(value, out _ukeEigyoCd);
            }
        }
        public string UkeEigyoNm { get; set; }
        public string UkeEigyoRyakuNm { get; set; }

        private int _eigTanSyainCd;
        public string EigTanSyainCd
        {
            get => _eigTanSyainCd.ToString("D10");
            set
            {
                int.TryParse(value, out _eigTanSyainCd);
            }
        }
        public string EigTanSyainNm { get; set; }
        private int _inputTanSyainCd;
        public string InputTanSyainCd
        {
            get => _inputTanSyainCd.ToString("D10");
            set
            {
                int.TryParse(value, out _inputTanSyainCd);
            }
        }
        public string InputTanSyainNm { get; set; }

        public string YoyaCodeTokuiTanStaff { get; set; }
        public string YoyaKbnNm { get; set; }

        public string UkeYmd { get; set; }
        private int _ukeCd;
        public string UkeCd
        {
            get => _ukeCd.ToString("D10");
            set
            {
                int.TryParse(value, out _ukeCd);
            }
        }
        public int UnkRen { get; set; }

        private string _untKin { get; set; }
        public string UntKin
        {
            get => IsReplaceItem ? string.Empty : _untKin;
            set => _untKin = value;
        }
        public string SummZeiRui => ZeiRui;
        public string SummTesuRyoG => TesuRyoG;
        public string SummCanUnc => CancelFee;
        public string SummCanSyoG => CancelTax;

        public string TokuSiyoStaYmd { get; set; }
        public string TokuSiyoEndYmd { get; set; }
        public string ToshiSiyoStaYmd { get; set; }
        public string ToshiSiyoEndYmd { get; set; }
    }

    public class CancelListReportSearchParams
    {
        public int TenantId { get; set; }
        public int UserLoginId { get; set; }
        public List<BookingKeyData> BookingKeys { get; set; }
        public CancelListData SearchCondition { get; set; }
       
    }
    public class CancelListReportSearchParamsUri
    {
        public int TenantId { get; set; }
        public int UserLoginId { get; set; }
        public List<BookingKeyData> BookingKeys { get; set; }       
        public CancelListDataUri SearchCondition { get; set; }
    }


    public class CancelListReportPagedData
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
        public string CancelDateTypeDetail { get; set; }
        public string CustomerInfo { get; set; }
        public string CompanyInfo { get; set; }
        public string SortInfo { get; set; }
        public string PaggingTypeInfo { get; set; }
        public string BookingTypeInfo { get; set; }
        public string UkeCdInfo { get; set; }
        public string BranchInfo { get; set; }
        public string StaffInfo { get; set; }
        public string CancelStaffInfo { get; set; }
        public string CancelFeeInfo { get; set; }
        public List<CancelListReportData> ReportDatas { get; set; }

        public PageSummaryData PageSummary { get; set; }
        public PageSummaryData AllPageSummary { get; set; }

        public string PrintedStaffCD { get; set; }
        public string PrintedStaffName { get; set; }
    }
}
