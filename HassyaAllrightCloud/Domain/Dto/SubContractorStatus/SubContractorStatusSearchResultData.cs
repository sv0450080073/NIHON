using HassyaAllrightCloud.Commons.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace HassyaAllrightCloud.Domain.Dto.SubContractorStatus
{
    public class BusScheduleInfo
    {
        public string VehicleDispatch
        {
            get
            {
                DateTime.TryParseExact(H_HaiSYmd, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime date);

                return $"{date.ToString("yyyy/MM/dd")} {CommonUtil.ConvertMyTimeStrToDefaultFormat(H_HaiSTime)} {H_HaiSNm}";
            }
        }
        public string VehicleDispatchOther
        {
            get
            {
                return $"{CommonUtil.ConvertMyTimeStrToDefaultFormat(H_HaiSSetTime)} {H_HaiSBinNm?.Trim()} {H_HaiSKouKNm}";
            }
        }
        public string ArrivalConnection
        {
            get
            {
                DateTime.TryParseExact(H_TouYmd, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime date);

                return $"{date.ToString("yyyy/MM/dd")} {CommonUtil.ConvertMyTimeStrToDefaultFormat(H_TouChTime)} {H_TouNm}";
            }
        }
        public string ArrivalConnectionOther
        {
            get
            {
                return $"{CommonUtil.ConvertMyTimeStrToDefaultFormat(H_TouSetTime)} {H_TouBinNm?.Trim()} {H_TouSKouKNm}";
            }
        }

        public string H_HaiSYmd { get; set; } //傭車_日程_配車年月日
        public string H_HaiSTime { get; set; } //傭車_日程_配車時間
        public string H_HaiSNm { get; set; } //傭車_配車地名
        public string H_HaiSSetTime { get; set; } //傭車_配車接続時間
        public string H_HaiSBinNm { get; set; } //傭車_便名
        public string H_HaiSKouKNm { get; set; } //傭車_交通名

        public string H_TouYmd { get; set; } //傭車_日程_到着年月日
        public string H_TouChTime { get; set; } //傭車_到着時間
        public string H_TouNm { get; set; } //傭車_到着地名
        public string H_TouSetTime { get; set; } //傭車_到着接続時間
        public string H_TouBinNm { get; set; } //傭車_到着便_便名
        public string H_TouSKouKNm { get; set; } //傭車_到着交通名
    }

    public class TaxFeeInfo
    {
        public long _youshaUnc = 0;
        public string YoushaUnc //傭車_運賃
        {
            get => _youshaUnc.ToString("N0");
            set => long.TryParse(value, out _youshaUnc);
        }

        private float _youZeiritsu = 0f;
        public string YouZeiritsu //傭車_税率
        {
            get => String.Format("{0:P1}", _youZeiritsu / 100);
            set => float.TryParse(value, out _youZeiritsu);
        }

        public long _youshaSyo = 0;
        public string YoushaSyo //傭車_消費税
        {
            get => _youshaSyo.ToString("N0");
            set => long.TryParse(value, out _youshaSyo);
        }

        private float _youTesuRitu = 0f;
        public string YouTesuRitu //傭車_手数率
        {
            get => String.Format("{0:P1}", _youTesuRitu / 100);
            set => float.TryParse(value, out _youTesuRitu);
        }

        public long _youshaTes = 0;
        public string YoushaTes //傭車_手数料
        {
            get => _youshaTes.ToString("N0");
            set => long.TryParse(value, out _youshaTes);
        }
    }
    public class SubContractorStatusSearchResultData
    {
        public int YouTokuiSeq { get; set; }
        public int YouSitenCdSeq { get; set; }

        public int UnkRen { get; set; }

        //No
        public int No { get; set; }

        //運行全日程
        private DateTime _haiSYmd;
        public string HaiSYmd
        {
            get
            {
                CultureInfo culture = new CultureInfo("ja-JP");
                return $"{_haiSYmd.ToString("yyyy/MM/dd")}({culture.DateTimeFormat.GetShortestDayName(_haiSYmd.DayOfWeek)})";
            }
            set => DateTime.TryParseExact(value, "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out _haiSYmd);
        }

        private DateTime _touYmd; //運行日_到着年月日
        public string TouYmd
        {
            get
            {
                CultureInfo culture = new CultureInfo("ja-JP");
                return $"{_touYmd.ToString("yyyy/MM/dd")}({culture.DateTimeFormat.GetShortestDayName(_touYmd.DayOfWeek)})";
            }
            set => DateTime.TryParseExact(value, "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out _touYmd);
        }

        //得意先, 担当, 電話
        public string TokuiNm { get; set; } //得意先名
        public string SitenNm { get; set; } //支店名
        private string _tokuiTanNm;
        public string TokuiTanNm
        {
            get => $"担当：{_tokuiTanNm}";
            set => _tokuiTanNm = value;
        } //予約_得意担当者
        private string _tokuiTel;
        public string TokuiTel
        {
            get => $"電話：{_tokuiTel}";
            set => _tokuiTel = value;
        } //予約_得意電話番号

        //団体名, 行き先名
        public string DanTaNm { get; set; } //運行日_団体名
        public string IkNm { get; set; } //運行日_行き先

        //総台数
        public int TotalNumber { get; set; }

        //運賃, 消費税, 手数料
        public long _sumSyaRyoUnc = 0;
        public string SumSyaRyoUnc //運賃
        {
            get => _sumSyaRyoUnc.ToString("N0");
            set => long.TryParse(value, out _sumSyaRyoUnc);
        }

        private float _zeiritsu = 0f;
        public string Zeiritsu //税率
        {
            get => $"{String.Format("{0:0,0.0}", _zeiritsu)}%";
            set => float.TryParse(value, out _zeiritsu);
        }

        public long _sumZeiRui = 0;
        public string SumZeiRui //消費税
        {
            get => $"{_sumZeiRui:N0}";
            set => long.TryParse(value, out _sumZeiRui);
        }

        private float _tesuRitu = 0f;
        public string TesuRitu//手数率
        {
            get => $"{String.Format("{0:0,0.0}", _tesuRitu)}%";
            set => float.TryParse(value, out _tesuRitu);
        }

        public long _sumTesuRyoG = 0;
        public string SumTesuRyoG//手数料
        {
            get => $"{_sumTesuRyoG:N0}";
            set => long.TryParse(value, out _sumTesuRyoG);
        }

        //ガイド料, 消費税, 手数料
        public float _sumGuideFee = 0f;
        public string SumGuideFee //ガイド手数料
        {

            get => _sumGuideFee.ToString("N0");
            set => float.TryParse(value, out _sumGuideFee);
        }

        public long _sumGuideTax = 0;
        public string SumGuideTax //ガイド消費税
        {
            get => _sumGuideTax.ToString("N0");
            set => long.TryParse(value, out _sumGuideTax);
        }

        public long _sumUnitGuiderFee = 0;
        public string SumUnitGuiderFee //ガイド料
        {
            get => _sumUnitGuiderFee.ToString("N0");
            set => long.TryParse(value, out _sumUnitGuiderFee);
        }

        //付帯料, 消費税, 手数料
        public long _incidentalFee = 0;
        public string IncidentalFee //付帯・積込品
        {
            get => _incidentalFee.ToString("N0");
            set => long.TryParse(value, out _incidentalFee);
        }

        public long _incidentalTax = 0;
        public string IncidentalTax //付帯・積込品_消費税
        {
            get => _incidentalTax.ToString("N0");
            set => long.TryParse(value, out _incidentalTax);
        }

        public long _incidentalCharge = 0;
        public string IncidentalCharge //付帯・積込品_手数料
        {
            get => _incidentalCharge.ToString("N0");
            set => long.TryParse(value, out _incidentalCharge);
        }

        //傭車先
        public string YouSkTokuiNm { get; set; } //傭車_得意先名
        public string YouStSitenNm { get; set; } //傭車_得意先支店名

        //号車
        public List<string> GoSyas { get; set; } = new List<string>();
        public string HAISHA_GoSya { get; set; } //傭車_号車

        // 配車・接続, 到着・接続
        public List<BusScheduleInfo> BusScheduleInfos { get; set; } = new List<BusScheduleInfo>();

        public string H_HaiSYmd { get; set; } //傭車_日程_配車年月日
        public string H_HaiSTime { get; set; } //傭車_日程_配車時間
        public string H_HaiSNm { get; set; } //傭車_配車地名
        public string H_HaiSSetTime { get; set; } //傭車_配車接続時間
        public string H_HaiSBinNm { get; set; } //傭車_便名
        public string H_HaiSKouKNm { get; set; } //傭車_交通名

        public string H_TouYmd { get; set; } //傭車_日程_到着年月日
        public string H_TouChTime { get; set; } //傭車_到着時間
        public string H_TouNm { get; set; } //傭車_到着地名
        public string H_TouSetTime { get; set; } //傭車_到着接続時間
        public string H_TouBinNm { get; set; } //傭車_到着便_便名
        public string H_TouSKouKNm { get; set; } //傭車_到着交通名

        //傭車運賃, 消費税, 手数料
        public List<TaxFeeInfo> TaxFeeInfos { get; set; } = new List<TaxFeeInfo>();

        public string YoushaUnc { get; set; }//傭車_運賃

        public string YouZeiritsu { get; set; } //傭車_税率

        public string YoushaSyo { get; set; } //傭車_消費税

        public string YouTesuRitu { get; set; } //傭車_手数率

        public string YoushaTes { get; set; } //傭車_手数料

        //傭車ガイド料, 消費税, 手数料
        public long _youFutTumGuiKin = 0;
        public string YouFutTumGuiKin //傭車_ガイド料
        {
            get => _youFutTumGuiKin.ToString("N0");
            set => long.TryParse(value, out _youFutTumGuiKin);
        }

        public long _youFutTumGuiTax = 0;
        public string YouFutTumGuiTax //傭車_ガイド_消費税
        {
            get => _youFutTumGuiTax.ToString("N0");
            set => long.TryParse(value, out _youFutTumGuiTax);
        }

        public long _youFutTumGuiTes = 0;
        public string YouFutTumGuiTes //傭車_ガイド_手数料
        {
            get => _youFutTumGuiTes.ToString("N0");
            set => long.TryParse(value, out _youFutTumGuiTes);
        }

        //傭車付帯料, 消費税, 手数料
        public long _youFutTumKin = 0;
        public string YouFutTumKin //傭車_付帯・積込品
        {
            get => _youFutTumKin.ToString("N0");
            set => long.TryParse(value, out _youFutTumKin);
        }

        public long _youFutTumTax = 0;
        public string YouFutTumTax //傭車_付帯・積込品_消費税
        {
            get => _youFutTumTax.ToString("N0");
            set => long.TryParse(value, out _youFutTumTax);
        }

        public long _youFutTumTes = 0;
        public string YouFutTumTes //傭車_付帯・積込品_手数料
        {
            get => _youFutTumTes.ToString("N0");
            set => long.TryParse(value, out _youFutTumTes);
        }

        //乗車人員, プラス人員
        public int _jyoSyaJin = 0;
        public string JyoSyaJin //傭車_配車乗車人員
        {
            get => $"{_jyoSyaJin}人";
            set => int.TryParse(value, out _jyoSyaJin);
        }

        public int _plusJin = 0;
        public string PlusJin //傭車_配車プラス人員
        {
            get => $"+{_plusJin}人";
            set => int.TryParse(value, out _plusJin);
        }

        //受付営業所, 予約区分, 受付日 / 番号
        public string UkeEigyosRyaku { get; set; }  //受付営業所略名
        public string YoyaKbn { get; set; }  //予約区分

        public DateTime _ukeYmd;  //受付日
        public string UkeYmd
        {
            get
            {
                return _ukeYmd.ToString("yyyy/MM/dd");
            }
            set => DateTime.TryParseExact(value, "yyyyMMdd", null, DateTimeStyles.None, out _ukeYmd);
        }

        public int _ukeCd;  //受付コード
        public string UkeCd
        {
            get
            {
                return $"{_ukeCd:D10}";
            }
            set => int.TryParse(value, out _ukeCd);
        }

        public override bool Equals(object obj)
        {
            try
            {
                var o = ((SubContractorStatusSearchResultData)obj);
                return this.UkeCd == o.UkeCd && this.UnkRen == o.UnkRen
                    && this.YouTokuiSeq == o.YouTokuiSeq && this.YouSitenCdSeq == o.YouSitenCdSeq;
            }
            catch
            {
                return false;
            }
        }
        public override int GetHashCode()
        {
            //Get the ID hash code value
            int IDHashCode = this.UkeCd.GetHashCode();
            int UnkRenHashCode = this.UnkRen.GetHashCode();
            int YouTokuiSeqHashCode = this.YouTokuiSeq.GetHashCode();
            int YouSitenCdSeqHashCode = this.YouSitenCdSeq.GetHashCode();
            //Get the string HashCode Value
            return IDHashCode ^ UnkRenHashCode ^ YouTokuiSeqHashCode ^ YouSitenCdSeqHashCode;
        }
    }
}
