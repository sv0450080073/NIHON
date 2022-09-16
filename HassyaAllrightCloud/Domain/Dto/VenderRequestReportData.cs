using HassyaAllrightCloud.Commons.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class VenderRequestReportData
    {
        /// <summary>
        /// Flag mark this object was generated after pagging
        /// </summary>
        public bool IsChildPage { get; set; } = false;
        /// <summary>
        /// Flag mark this object contain data for main report
        /// </summary>
        public bool IsMainReport { get; set; }

        public string TokiskTokuiNm { get; set; }
        public string TokistTokuiSitenNm { get; set; }
        public string YouskFax { get; set; }
        public string OutPutYmd { get; set; }

        //乗車日
        public string UkeNo { get; set; }
        public int UnkRen { get; set; }
        public int YouTblSeq { get; set; }
        public int BunkRen { get; set; }
        public int TeiDanNo { get; set; }

        public string DanTaNm { get; set; }
        public string DanTaNm2 { get; set; }
        public string DanTaNmInfo
        {
            get
            {
                return DanTaNm + DanTaNm2;
            }
        }

        public int TotalSyaSyuDai { get; set; }
        public string YouKataKbn { get; set; }
        public string YoushaZeiKbn { get; set; }

        //得意先
        public string TokuiNm { get; set; }
        public string TokuiSitenNm { get; set; }
        public string TokuiTel { get; set; }
        public string TokuiTanNm { get; set; }


        //連絡先
        public string KanjJyus1 { get; set; }
        public string KanjJyus2 { get; set; }
        public string KanjTel { get; set; }
        public string KanjNm { get; set; }

        //行　先
        public string IkNm { get; set; }

        //台数
        public int TotalBusRequired { get; set; }
        public int TotalBusBorrow { get; set; }
        public string BusNum { get; set; }
        public string GetBusUnitInfo { 
            get 
            {
                if (IsMainReport)
                {
                    return $"{TotalBusRequired.ToString("D2")}台中{TotalBusBorrow.ToString("D2")}台";
                }
                return $"{TotalBusRequired.ToString("D2")}台中{BusNum.Trim()}号車";
            } 
        }

        //配車項目
        public string HaiSKoukRyaku { get; set; }
        public string HaiSBinNm { get; set; }

        private string _haiSSetTime;
        public string HaiSSetTime 
        {
            get => _haiSSetTime;
            set
            {
                _haiSSetTime = CommonUtil.ConvertMyTimeStrToDefaultFormat(value);
            }
        }
        public string HaiSNm { get; set; }
        public string HaiSJyus1 { get; set; }
        public string HaiSJyus2 { get; set; }

        //到着項目
        public string TouChaKoukRyaku { get; set; }
        public string TouChaBinNm { get; set; }

        private string _touSetTime;
        public string TouSetTime
        {
            get => _touSetTime;
            set
            {
                _touSetTime = CommonUtil.ConvertMyTimeStrToDefaultFormat(value);
            }
        }
        public string TouNm { get; set; }
        public string TouJyus1 { get; set; }
        public string TouJyus2 { get; set; }

        public string Zeiritsu { get; set; }

        private string _zeikbnRyakuNm;
        public string ZeikbnRyakuNm
        {
            get
            {
                if (IsMainReport)
                    return _zeikbnRyakuNm;
                return string.Empty;
            }
            set
            {
                _zeikbnRyakuNm = value;
            }
        }

        private int _syaRyoSyo;
        public string SyaRyoSyo
        {
            get
            {
                if (!IsMainReport)
                    return string.Empty;
                return _syaRyoSyo.ToString("N0");
            }
            set
            {
                int.TryParse(value.Replace(",", string.Empty), out _syaRyoSyo);
            }
        }

        private int _syaRyoUnc;
        public string SyaRyoUnc
        {
            get
            {
                if (!IsMainReport)
                    return string.Empty;
                return _syaRyoUnc.ToString("N0");
            }
            set
            {
                int.TryParse(value.Replace(",", string.Empty), out _syaRyoUnc);
            }
        }

        private int _syaRyoTes;
        public string SyaRyoTes
        {
            get
            {
                if (!IsMainReport)
                    return string.Empty;
                return _syaRyoTes.ToString("N0");
            }
            set
            {
                int.TryParse(value.Replace(",", string.Empty), out _syaRyoTes);
            }
        }

        private int _totalPrice;
        public string TotalPrice
        {
            get
            {
                if (!IsMainReport)
                    return string.Empty;
                return _totalPrice.ToString("N0");
            }
            set
            {
                int.TryParse(value.Replace(",", string.Empty), out _totalPrice);
            }
        }

        public string _tesRitu;
        public string TesRitu
        {
            get
            {
                if (!IsMainReport)
                    return string.Empty;
                return _tesRitu + (_tesRitu.Contains("%") ? string.Empty : "%");
            }
            set
            {
                _tesRitu = value;
            }
        }

        //Bus loans type info
        public VenderRequestReportBusLoanInfo BusLoan1 { get; set; }
        public VenderRequestReportBusLoanInfo BusLoan2 { get; set; }
        public VenderRequestReportBusLoanInfo BusLoan3 { get; set; }
        public VenderRequestReportBusLoanInfo BusLoan4 { get; set; }
        public VenderRequestReportBusLoanInfo BusLoan5 { get; set; }

        public List<VenderRequestReportBusLoanInfo> BusLoanInfos { get; set; }

        public string JyoSyaJin { get; set; }
        public string PlusJin { get; set; }
        public string OthJinKbn1Ryaku { get; set; }
        public string OthJin1 { get; set; }
        public string OthJinKbn2Ryaku { get; set; }
        public string OthJin2 { get; set; }

        public string EigyoSNm { get; set; }
        public string EigyoSZipCd { get; set; }
        public string EigyoSJyuS1 { get; set; }
        public string EigyoSJyuS2 { get; set; }
        public string EigyoSTel { get; set; }
        public string EigyoSFax { get; set; }

        public string HaisYmd { get; set; }
        public string HaisYmdText
        {
            get
            {
                var date = DateTime.ParseExact(HaisYmd, "yyyyMMdd", CultureInfo.CurrentCulture);
                CultureInfo culture = new CultureInfo("ja-JP");
                return $"{date.ToString("yyyy年MM月dd日")} ({culture.DateTimeFormat.GetShortestDayName(date.DayOfWeek)})";
            }
        }

        public string TouYmd { get; set; }
        public string TouYmdText
        {
            get
            {
                var date = DateTime.ParseExact(TouYmd, "yyyyMMdd", CultureInfo.CurrentCulture);
                CultureInfo culture = new CultureInfo("ja-JP");
                return $"{date.ToString("yyyy年MM月dd日")} ({culture.DateTimeFormat.GetShortestDayName(date.DayOfWeek)})";
            }
        }

        public string SyaSyuDai { get; set; }

        private string _haiSTime;
        public string HaiSTime
        {
            get => _haiSTime;
            set
            {
                _haiSTime = CommonUtil.ConvertMyTimeStrToDefaultFormat(value);
            }
        }

        private string _syuPaTime;
        public string SyuPaTime 
        {
            get => _syuPaTime;
            set
            {
                _syuPaTime = CommonUtil.ConvertMyTimeStrToDefaultFormat(value);
            }
        }

        private string _touChTime;
        public string TouChTime 
        {
            get => _touChTime;
            set
            {
                _touChTime = CommonUtil.ConvertMyTimeStrToDefaultFormat(value);
            }
        }

        //指示条件
        public string SijJoKbnNm1 { get; set; }
        public string SijJoKbnNm2 { get; set; }
        public string SijJoKbnNm3 { get; set; }
        public string SijJoKbnNm4 { get; set; }
        public string SijJoKbnNm5 { get; set; }

        public string SijJokbn1Ryaku { get; set; }
        public string SijJokbn2Ryaku { get; set; }
        public string SijJokbn3Ryaku { get; set; }
        public string SijJokbn4Ryaku { get; set; }
        public string SijJokbn5Ryaku { get; set; }

        public string Biko { get; set; }

        public short TokuiCd { get; set; }
        public short SitenCd { get; set; }
        public short GyosyaCd { get; set; }
        public string GyosyaNm { get; set; }

        public YFuttuVenderRequestReportData YFut1 { get; set; }
        public YFuttuVenderRequestReportData YFut2 { get; set; }
        public YFuttuVenderRequestReportData YFut3 { get; set; }
        public YFuttuVenderRequestReportData YFut4 { get; set; }
        public YFuttuVenderRequestReportData YFut5 { get; set; }
        public YFuttuVenderRequestReportData YFut6 { get; set; }

        public YFuttuVenderRequestReportData YTum1 { get; set; }
        public YFuttuVenderRequestReportData YTum2 { get; set; }
        public YFuttuVenderRequestReportData YTum3 { get; set; }
        public YFuttuVenderRequestReportData YTum4 { get; set; }
        public YFuttuVenderRequestReportData YTum5 { get; set; }
        public YFuttuVenderRequestReportData YTum6 { get; set; }

        public List<YFuttuVenderRequestReportData> YFuts { get; set; } = new List<YFuttuVenderRequestReportData>();
        public List<YFuttuVenderRequestReportData> YTums { get; set; } = new List<YFuttuVenderRequestReportData>();

        public List<KoteiTehaiVenderRequestReport> KoteiTehais { get; set; } = new List<KoteiTehaiVenderRequestReport>(); 
    }

    public class YFuttuVenderRequestReportData
    {
        public string Date { get; set; }
        public string FutTumNm { get; set; }

        public string SeisanNm { get; set; }

        public string Tanka { get; set; }

        public string Suryo { get; set; }
        public string SuryoText
        {
            get => " x   " + int.Parse(Suryo).ToString("N0");
        }

        public string Kingaku { get; set; }
        public string KingakuText
        {
            get => " x   " + int.Parse(Kingaku.Split('.')[0].Replace(",","")).ToString("N0") + " 円";
        }

        public int FutTumKbn { get; set; }
    }

    public class VenderRequestReportBusLoanInfo
    {
        public bool IsMainReport { get; set; } = true;
        public string RyakuNm { get; set; }

        private int _syaSyuTan;
        public string SyaSyuTan 
        {
            get
            {
                if(IsMainReport)
                    return _syaSyuTan.ToString("N0");
                return string.Empty;
            }
            set
            {
                int.TryParse(value.Replace(",", string.Empty), out _syaSyuTan);
            }
        }

        public string SyaSyuDai;
        public string SyaSyuDaiText 
        {
            get
            {
                if (IsMainReport)
                    return $" x   {SyaSyuDai} 台";
                return $"1 台";
            }
        }

        private int _syaRyoUnc;
        public string SyaRyoUnc
        {
            get
            {
                if(IsMainReport)
                    return _syaRyoUnc.ToString("N0");
                return string.Empty;
            }
            set
            {
                int.TryParse(value.Replace(",", string.Empty), out _syaRyoUnc);
            }
        }
    }

    public class KoteiTehaiVenderRequestReport
    {
        public string UkeNo { get; set; }
        public int UnkRen { get; set; }
        public int TeiDanNo { get; set; }
        public string Date { get; set; }
        public string DisplayDate { 
            get 
            {
                if (string.IsNullOrEmpty(Date))
                    return string.Empty;
                return DateTime.ParseExact(Date, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("MM/dd");
            } 
        }
        public string Koutei { get; set; }
        public string TehaiNm { get; set; }
        public string TehaiTel { get; set; }
        public string TehaiDisplay { get; set; }
    }

}
