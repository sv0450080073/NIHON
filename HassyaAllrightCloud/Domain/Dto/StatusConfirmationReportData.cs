using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class StatusConfirmationReportData : StatusConfirmationExportData
    {
        public string UkeNo { get; set; }
        public string UnkRen { get; set; }
        public string SitenRyakuNm { get; set; }
        public string SitenFaxNo { get; set; }

        private string _tokuiRyakuNm;
        public string TokuiRyakuNm
        {
            get
            {
                return IsReplaceItem ? string.Empty : _tokuiRyakuNm;
            }
            set { _tokuiRyakuNm = value; }
        }

        /// <summary>
        /// Get date time start and arrived after fomatting as string
        /// </summary>
        public string DateTimeInProcess
        {
            get
            {
                if (IsEmptyObject || IsReplaceItem)
                    return string.Empty;
                try
                {
                    CultureInfo culture = new CultureInfo("ja-JP");
                    DateTime startDate = DateTime.ParseExact(UnkoYmdStr, "yyyyMMdd", culture);
                    string startTime = CommonUtil.ConvertMyTimeStrToDefaultFormat(HaiSTime);
                    DateTime endDate = DateTime.ParseExact(UnkoYmdEnd, "yyyyMMdd", culture);
                    string endTime = CommonUtil.ConvertMyTimeStrToDefaultFormat(TouChTime);

                    return $"{startDate.ToString("yy/MM/dd")}({culture.DateTimeFormat.GetDayName(startDate.DayOfWeek)}) {startTime}"
                        + " ～ "
                        + $"{endDate.ToString("yy/MM/dd")}({culture.DateTimeFormat.GetDayName(endDate.DayOfWeek)}) {endTime}";
                }
                catch { return string.Empty; }
            }
        }

        public string Night
        {
            get
            {
                if (IsEmptyObject || IsReplaceItem)
                    return string.Empty;
                try
                {
                    CultureInfo culture = new CultureInfo("ja-JP");
                    DateTime startDate = DateTime.ParseExact(UnkoYmdStr, "yyyyMMdd", culture);
                    DateTime endDate = DateTime.ParseExact(UnkoYmdEnd, "yyyyMMdd", culture);

                    int night = (endDate - startDate).Days;

                    if (night < 0)
                        return string.Empty;
                    if (night == 0)
                        return "日帰";

                    return $"{night} 泊";
                }
                catch { return string.Empty; }
            }
        }

        public string Date
        {
            get
            {
                if (IsEmptyObject || IsReplaceItem)
                    return string.Empty;
                try
                {
                    CultureInfo culture = new CultureInfo("ja-JP");
                    DateTime startDate = DateTime.ParseExact(UnkoYmdStr, "yyyyMMdd", culture);
                    DateTime endDate = DateTime.ParseExact(UnkoYmdEnd, "yyyyMMdd", culture);

                    int night = (endDate - startDate).Days;

                    if (night <= 0)
                        return string.Empty;

                    return $"{night + 1} 日";
                }
                catch { return string.Empty; }
            }
        }

        /// <summary>
        /// Get total number of day and night room hiring
        /// </summary>
        public string HireDate
        {
            get
            {
                if (IsEmptyObject || IsReplaceItem)
                    return string.Empty;
                try
                {
                    CultureInfo culture = new CultureInfo("ja-JP");
                    DateTime startDate = DateTime.ParseExact(UnkoYmdStr, "yyyyMMdd", culture);
                    DateTime endDate = DateTime.ParseExact(UnkoYmdEnd, "yyyyMMdd", culture);

                    int night = (endDate - startDate).Days;

                    if (night < 0)
                        return string.Empty;

                    return $"{night} 泊{Environment.NewLine}{night + 1} 日";
                }
                catch { return string.Empty; }
            }
        }

        public string UntKinTxt
        {
            get
            {
                if (IsEmptyObject || IsReplaceItem)
                    return string.Empty;

                long newValue = 0;
                long.TryParse(UntKin, out newValue);

                return newValue.ToString("N0") + " 円";
            }
        }

        public string ZeiRuiTxt
        {
            get
            {
                if (IsEmptyObject || IsReplaceItem)
                    return string.Empty;

                long newValue = 0;
                long.TryParse(ZeiRui, out newValue);

                return newValue.ToString("N0") + " 円";
            }
        }

        private string _zeiritsu;

        public string Zeiritsu
        {
            get
            {
                if (IsEmptyObject || IsReplaceItem)
                    return string.Empty;

                if (string.IsNullOrEmpty(_zeiritsu))
                    return "0.0 ％";
                return _zeiritsu + " ％";
            }
            set { _zeiritsu = value; }
        }

        private string _tesuRitu;
        public string TesuRitu
        {
            get
            {
                if (IsEmptyObject || IsReplaceItem)
                    return string.Empty;

                if (string.IsNullOrEmpty(_tesuRitu))
                    return "0.0 ％";
                return _tesuRitu + " ％";
            }
            set { _tesuRitu = value; }
        }

        public string TesuRyoGTxt
        {
            get
            {
                if (IsEmptyObject || IsReplaceItem)
                    return string.Empty;

                long newValue = 0;
                long.TryParse(TesuRyoG, out newValue);

                return newValue.ToString("N0") + " 円";
            }
        }

        public string GuitKinTxt
        {
            get
            {
                if (IsEmptyObject || IsReplaceItem)
                    return string.Empty;

                long newValue = 0;
                long.TryParse(GuitKin, out newValue);

                return newValue.ToString("N0") + " 円";
            }
        }


        private string _feeGuider;
        public string FeeGuider
        {
            get
            {
                if (IsEmptyObject || IsReplaceItem)
                    return string.Empty;

                long newValue = 0;
                long.TryParse(_feeGuider, out newValue);

                return newValue.ToString("N0") + " 円";
            }
            set { _feeGuider = value; }
        }

        public string TaxGuiderTxt
        {
            get
            {
                if (IsEmptyObject || IsReplaceItem)
                    return string.Empty;

                long newValue = 0;
                long.TryParse(TaxGuider, out newValue);

                return newValue.ToString("N0") + " 円";
            }
        }

        private string _totalAmount;
        public string TotalAmount
        {
            get
            {
                if (IsEmptyObject || IsReplaceItem)
                    return string.Empty;

                long newValue = 0;
                long.TryParse(_totalAmount, out newValue);

                return newValue.ToString("N0") + " 円";
            }
            set { _totalAmount = value; }
        }

        public string PassengerInfo
        {
            get
            {
                if (IsEmptyObject || IsReplaceItem)
                    return string.Empty;

                return $"{JyoSyaJin} 人{Environment.NewLine}+　{PlusJin} 人";
            }
        }

        public string GuiWNin { get; set; }
        public string HadGuide
        {
            get
            {
                if (IsEmptyObject || IsReplaceItem)
                    return string.Empty;

                int flag = 0;
                if (int.TryParse(GuiWNin, out flag))
                {
                    return flag == 0 ? string.Empty : " 有";
                }
                return string.Empty;
            }
        }

        public string ConfirmInfo
        {
            get
            {
                if (IsEmptyObject || IsReplaceItem)
                    return string.Empty;

                if (string.IsNullOrWhiteSpace(KaktYmd))
                {
                    return "未確定";
                }
                return "確定";
            }
        }

        public string ConfirmYmd
        {
            get
            {
                if (IsEmptyObject || IsReplaceItem)
                    return string.Empty;

                if (!string.IsNullOrWhiteSpace(KaktYmd))
                {
                    DateTime confirmDate = DateTime.ParseExact(UnkoYmdStr, "yyyyMMdd", new CultureInfo("ja-JP"));

                    return confirmDate.ToString("yy/MM/dd");
                }
                return string.Empty;
            }
        }

        public string SaikFlgTxt
        {
            get
            {
                if (IsEmptyObject || IsReplaceItem)
                    return string.Empty;

                if (SaikFlg == "1")
                    return "〇";
                return string.Empty;
            }
        }

        public string DaiSuFlgTxt
        {
            get
            {
                if (IsEmptyObject || IsReplaceItem)
                    return string.Empty;
                if (DaiSuFlg == "1")
                    return "〇";
                return string.Empty;
            }
        }

        public string KingFlgTxt
        {
            get
            {
                if (IsEmptyObject || IsReplaceItem)
                    return string.Empty;
                if (KingFlg == "1")
                    return "〇";
                return string.Empty;
            }
        }

        public string NitteiFlgTxt
        {
            get
            {
                if (IsEmptyObject || IsReplaceItem)
                    return string.Empty;
                if (NitteiFlg == "1")
                    return "〇";
                return string.Empty;
            }
        }

        public string CompanyName { get; set; }
        public string EigyosRyakuNm { get; set; }
        public string EigSyain { get; set; }
        public string TEL { get; set; }
        public string FAX { get; set; }
    }

    public abstract class StatusConfirmationExportData
    {
        /// <summary>
        /// Mark this object is empty and not show some field. Default values is <c>false</c>
        /// </summary>
        public bool IsEmptyObject { get; set; } = false;
        /// <summary>
        /// Mark this object is replace object some field will be hide
        /// </summary>
        public bool IsReplaceItem { get; set; } = false;

        public string UkeCd { get; set; }
        public string YoyaKbn { get; set; }
        public string YoyaKbnNm { get; set; }
        public string UkeYmd { get; set; }
        public string TokiskTokuiCd { get; set; }
        public string TokiskRyakuNm { get; set; }
        public string TokiStSitenCd { get; set; }
        public string TokiStRyakuNm { get; set; }
        public string TokuiTanNm { get; set; }
        public string SirTokiskTokuiCd { get; set; }
        public string SirTokiskRyakuNm { get; set; }
        public string SirTokiStSitenCd { get; set; }
        public string SirTokiStRyakuNm { get; set; }
        public string EigyoCd { get; set; }
        public string EigyoRyakuNm { get; set; }

        private string _yoyakuNm;
        public string YoyakuNm
        {
            get
            {
                return IsReplaceItem ? string.Empty : _yoyakuNm;
            }
            set { _yoyakuNm = value; }
        }
        public string UnkoYmdStr { get; set; }
        public string HaiSTime { get; set; }
        public string UnkoYmdEnd { get; set; }

        public string TouChTime { get; set; }

        private string _haiSNm;
        public string HaiSNm
        {
            get
            {
                return IsReplaceItem ? string.Empty : _haiSNm;
            }
            set { _haiSNm = value; }
        }

        private string _touNm;
        public string TouNm
        {
            get
            {
                return IsReplaceItem ? string.Empty : _touNm;
            }
            set { _touNm = value; }
        }

        private string _danTaNm;
        public string DanTaNm
        {
            get
            {
                return IsReplaceItem ? string.Empty : _danTaNm;
            }
            set { _danTaNm = value; }
        }

        private string _ikNm;
        public string IkNm
        {
            get
            {
                return IsReplaceItem ? string.Empty : _ikNm;
            }
            set { _ikNm = value; }
        }

        public string KanJNm { get; set; }
        public string SyaSyuCd { get; set; }

        public string SyaSyuNm { get; set; }
        public string KataKbn { get; set; }
        public string CodeKbnNm { get; set; }

        private string _syaSyuDai;
        public string SyaSyuDai
        {
            get
            {
                if (IsEmptyObject)
                    return string.Empty;

                if (string.IsNullOrEmpty(_syaSyuDai))
                    return "0 台";
                return _syaSyuDai + " 台";
            }
            set { _syaSyuDai = value; }
        }
        public string UntKin { get; set; }
        public string ZeiRui { get; set; }
        public string TesuRyoG { get; set; }
        public string JyoSyaJin { get; set; }
        public string PlusJin { get; set; }
        public string KaknKais { get; set; }
        public string KaktYmd { get; set; }
        public string KaknNin { get; set; }

        private string _bikoNm;
        public string BikoNm
        {
            get
            {
                if (IsReplaceItem)
                    return string.Empty;
                return _bikoNm;
            }
            set { _bikoNm = value; }
        }

        public string SaikFlg { get; set; }
        public string DaiSuFlg { get; set; }
        public string KingFlg { get; set; }
        public string NitteiFlg { get; set; }

        public string IntanSyainCd { get; set; }
        public string IntanSyainNm { get; set; }
        public string EigtanSyainCd { get; set; }
        public string EigtanSyainNm { get; set; }
        public string GuiSu { get; set; }
        public string UnitGuiderFee { get; set; }
        public string TaxGuider { get; set; }
        public string GuitKin { get; set; }
    }

    public class StatusConfirmationReportPaged
    {
        /// <summary>
        /// Get/Sets Name of the customer
        /// </summary>
        public string TokuiRyakuNm { get; set; }
        /// <summary>
        /// Get/Sets BranchName of the customer
        /// </summary>
        public string SitenRyakuNm { get; set; }
        /// <summary>
        /// VPM_Eigyos.EigyoNm
        /// </summary>
        public string EigyosRyakuNm { get; set; }

        public string TokuiFax
        {
            get
            {
                if (!PagedData.Any())
                    return string.Empty;
                return PagedData.First().SitenFaxNo; //mistake naming
            }
        }

        public string FAX
        {
            get
            {
                if (!PagedData.Any())
                    return string.Empty;
                return PagedData.First().FAX;
            }
        }

        public string CompanyName
        {
            get
            {
                if (!PagedData.Any())
                    return string.Empty;
                return PagedData.First().CompanyName;
            }
        }

        public string TEL
        {
            get
            {
                if (!PagedData.Any())
                    return string.Empty;
                return PagedData.First().TEL;
            }
        }

        public string EigSyain
        {
            get
            {
                if (!PagedData.Any())
                    return string.Empty;
                return PagedData.First().EigSyain;
            }
        }

        /// <summary>
        /// Datetime as string exported report
        /// </summary>
        public string ReportDate { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }

        /// <summary>
        /// Paged list show to report. One item per page
        /// </summary>
        public List<StatusConfirmationReportData> PagedData { get; set; }
    }

    public class StatusConfirmationPreviewReportParam
    {
        public List<BookingKeyData> SelectedList { get; set; }
        public int TenantId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ItemPerPage { get; set; } = 20;
        public PaperSize PaperSize { get; set; }
    }
}
