namespace HassyaAllrightCloud.Domain.Dto.SubContractorStatus
{
    using HassyaAllrightCloud.Commons.Helpers;
    using System;

    public class SubContractorStatusReportData
    {
        private string GetOutput(string outPut, bool shouldShowInSumRow = true)
        {
            if (IsRowSumResult && shouldShowInSumRow)
                return outPut;
            if (IsRepeatItem || (IsRowSumResult && !shouldShowInSumRow))
                return string.Empty;

            return outPut;
        }

        public bool IsRowSumResult { get; set; } = false;

        public bool IsRepeatItem { get; set; }

        public bool IsEmptyObject { get; set; } = false;

        public string TokuiNm { get; set; }
        public string SitenNm { get; set; }

        public int SkTokuiCd { get; set; }      
        public int StSitenCd { get; set; }
        public int YouTokuiSeq { get; set; }
        public int YouSitenCdSeq { get; set; }

        public string UkeNo { get; set; }
        public int _ukeCd = 0;
        public string UkeCd 
        {
            get => GetOutput($"{_ukeCd:D10}", false);
            set => int.TryParse(value, out _ukeCd);
        }
        public int UnkRen { get; set; }

        public string DisplayHaiSYmd
        {
            get
            {
                if (IsRepeatItem || IsRowSumResult)
                    return string.Empty;

                DateTime.TryParseExact(HaiSYmd, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime date);
                return $"{date.ToString("yyyy/MM/dd")}";
            }
        }
        public string DisplayTouYmd
        {
            get
            {
                if (IsRepeatItem || IsRowSumResult)
                    return string.Empty;

                DateTime.TryParseExact(TouYmd, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime date);
                return $"～{date.ToString("yyyy/MM/dd")}";
            }
        }
        public string HaiSYmd { get; set; }
        public string TouYmd { get; set; }

        public string _tokiskRyakuNm;
        public string TokiskRyakuNm 
        { 
            get => GetOutput(_tokiskRyakuNm); 
            set => _tokiskRyakuNm = value; 
        }
        public string _tokiStRyakuNm;
        public string TokiStRyakuNm
        {
            get => GetOutput(_tokiStRyakuNm);
            set => _tokiStRyakuNm = value;
        }
        public string _tokuiTanNm;
        public string TokuiTanNm
        {
            get => GetOutput(_tokuiTanNm);
            set => _tokuiTanNm = value;
        }
        public string _tokuiTel;
        public string TokuiTel
        {
            get => GetOutput(_tokuiTel);
            set => _tokuiTel = value;
        }

        public string _danTaNm;
        public string DanTaNm
        {
            get => GetOutput(_danTaNm);
            set => _danTaNm = value;
        }
        public string _ikNm;
        public string IkNm
        {
            get => GetOutput(_ikNm);
            set => _ikNm = value;
        }

        public string U_HaiScheduleName 
        {
            get => GetOutput($"{CommonUtil.ConvertMyTimeStrToDefaultFormat(U_HaiSTime)} {U_HaiSNm}");
        }
        public string U_HaiScheduleCode 
        {
            get => GetOutput($"{CommonUtil.ConvertMyTimeStrToDefaultFormat(U_HaiSSetTime)} {U_HaiSBinNm} {U_HaiSKouKNm}");
        }
        public string U_TouScheduleName
        {
            get => GetOutput($"{CommonUtil.ConvertMyTimeStrToDefaultFormat(U_TouChTime)} {U_TouNm}");
        }
        public string U_TouScheduleCode
        {
            get => GetOutput($"{CommonUtil.ConvertMyTimeStrToDefaultFormat(U_TouSetTime)} {U_TouSBinNm} {U_TouSKouKNm}");
        }

        public string U_HaiSTime { get; set; }
        public string U_HaiSNm { get; set; }
        public string U_HaiSSetTime { get; set; }
        public string U_HaiSKouKNm { get; set; }
        private string _u_HaiSBinNm;
        public string U_HaiSBinNm
        {
            get => GetOutput(_u_HaiSBinNm);
            set => _u_HaiSBinNm = value;
        }
        public string U_TouChTime { get; set; }
        public string U_TouNm { get; set; }
        public string U_TouSetTime { get; set; }
        public string U_TouSKouKNm { get; set; }
        private string _u_TouSBinNm;
        public string U_TouSBinNm
        {
            get => GetOutput(_u_TouSBinNm);
            set => _u_TouSBinNm = value;
        }

        public int _u_JyoSyaJin;
        public string U_JyoSyaJin
        {
            get => GetOutput($"{_u_JyoSyaJin}人", false);
            set =>  int.TryParse(value, out _u_JyoSyaJin);
        }
        public int _u_PlusJin;
        public string U_PlusJin
        {
            get => GetOutput($"{_u_PlusJin}人", false);
            set => int.TryParse(value, out _u_PlusJin);
        }

        public int _totalNumber;
        public string TotalNumber
        {
            get => GetOutput($"{_totalNumber}台");
            set => int.TryParse(value, out _totalNumber);
        }

        public long _sumSyaRyoUnc;
        public string SumSyaRyoUnc
        {
            get => GetOutput($"{_sumSyaRyoUnc:N0}");
            set
            {
                long.TryParse(value, out _sumSyaRyoUnc);
                _sumTicket = _sumSyaRyoUnc + _sumZeiRui;
            }
        }
        public float _zeiritsu;
        public string Zeiritsu 
        {
            get => GetOutput($"{(_zeiritsu / 100):P1}", false);
            set => float.TryParse(value, out _zeiritsu);
        }
        public long _sumZeiRui;
        public string SumZeiRui
        {
            get => GetOutput($"{_sumZeiRui:N0}");
            set
            {
                long.TryParse(value, out _sumZeiRui);
                _sumTicket = _sumSyaRyoUnc + _sumZeiRui;
            }
        }
        public float _tesuRitu;
        public string TesuRitu
        {
            get => GetOutput($"{(_tesuRitu / 100):P1}", false);
            set => float.TryParse(value, out _tesuRitu);
        }
        public long _sumTesuRyoG;
        public string SumTesuRyoG
        {
            get => GetOutput($"{_sumTesuRyoG:N0}");
            set => long.TryParse(value, out _sumTesuRyoG);
        }
        public long _sumTicket;
        public string SumTicket
        {
            get => GetOutput($"{_sumTicket:N0}");
            set => long.TryParse(value, out _sumTicket);
        }

        public float _sumGuideFee;
        public string SumGuideFee
        {
            get => GetOutput($"{_sumGuideFee:N0}");
            set
            {
                float.TryParse(value, out _sumGuideFee);
                _totalGuider = _sumGuideFee + _sumGuideTax;
            }
        }
        public float _sumGuideTax;
        public string SumGuideTax
        {
            get => GetOutput($"{_sumGuideTax:N0}");
            set
            {
                float.TryParse(value, out _sumGuideTax);
                _totalGuider = _sumGuideFee + _sumGuideTax;
            }
        }
        public float _sumUnitGuiderFee;
        public string SumUnitGuiderFee
        {
            get => GetOutput($"{_sumUnitGuiderFee:N0}");
            set => float.TryParse(value, out _sumUnitGuiderFee);
        }
        public float _totalGuider;
        public string TotalGuider
        {
            get => GetOutput($"{_totalGuider:N0}");
            set => float.TryParse(value, out _totalGuider);
        }

        public long _incidentalFee;
        public string IncidentalFee
        {
            get => GetOutput($"{_incidentalFee:N0}");
            set
            {
                long.TryParse(value, out _incidentalFee);
                _totalIncidental = _incidentalFee + _incidentalTax;
            }
        }
        public long _incidentalTax;
        public string IncidentalTax
        {
            get => GetOutput($"{_incidentalTax:N0}");
            set
            {
                long.TryParse(value, out _incidentalTax);
                _totalIncidental = _incidentalFee + _incidentalTax;
            }
        }
        public long _incidentalCharge;
        public string IncidentalCharge
        {
            get => GetOutput($"{_incidentalCharge:N0}");
            set => long.TryParse(value, out _incidentalCharge);
        }
        public long _totalIncidental;
        public string TotalIncidental
        {
            get => GetOutput($"{_totalIncidental:N0}");
            set => long.TryParse(value, out _totalIncidental);
        }

        public string _youSkRyakuNm;
        public string YouSkRyakuNm
        { 
            get => _youSkRyakuNm;
            set => _youSkRyakuNm = value;
        }
        public string _youStRyakuNm;
        public string YouStRyakuNm
        {
            get => _youStRyakuNm;
            set => _youStRyakuNm = value;
        }

        public string HAISHA_GoSya { get; set; }

        public string DisplayH_HaiSYmd
        {
            get
            {
                if (IsRowSumResult)
                    return string.Empty;

                DateTime.TryParseExact(H_HaiSYmd, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime date);
                return $"{date.ToString("yyyy/MM/dd")}～";
            }
        }
        public string DisplayH_TouYmd
        {
            get
            {
                if (IsRowSumResult)
                    return string.Empty;

                DateTime.TryParseExact(H_TouYmd, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime date);
                return date.ToString("yyyy/MM/dd");
            }
        }
        public string H_HaiSYmd { get; set; }
        public string H_TouYmd { get; set; }

        public string H_HaiScheduleName
        {
            get =>  IsRowSumResult ? string.Empty : $"{CommonUtil.ConvertMyTimeStrToDefaultFormat(H_HaiSTime)} {H_HaiSNm}";
        }
        public string H_HaiScheduleCode
        {
            get =>  IsRowSumResult ? string.Empty : $"{CommonUtil.ConvertMyTimeStrToDefaultFormat(H_HaiSSetTime)} {H_HaiSKouKNm}";
        }
        public string H_TouScheduleName
        {
            get => IsRowSumResult ?  string.Empty : $"{CommonUtil.ConvertMyTimeStrToDefaultFormat(H_TouChTime)} {H_TouNm}";
        }
        public string H_TouScheduleCode
        {
            get => IsRowSumResult ? string.Empty : $"{CommonUtil.ConvertMyTimeStrToDefaultFormat(H_TouSetTime)} {H_TouSKouKNm}";
        }

        public string H_HaiSTime { get; set; }
        public string H_HaiSNm { get; set; }
        public string H_HaiSSetTime { get; set; }
        private string _h_HaiSBinNm;
        public string H_HaiSBinNm
        {
            get => IsRowSumResult ? string.Empty : _h_HaiSBinNm;
            set => _h_HaiSBinNm = value;
        }
        public string H_HaiSKouKNm { get; set; }
        public string H_TouChTime { get; set; }
        public string H_TouNm { get; set; }
        public string H_TouSetTime { get; set; }
        private string _h_TouBinNm;
        public string H_TouBinNm 
        { 
            get => IsRowSumResult ? string.Empty : _h_TouBinNm; 
            set => _h_TouBinNm = value; 
        }
        public string H_TouSKouKNm { get; set; }

        public long _youshaUnc;
        public string YoushaUnc 
        { 
            get => $"{_youshaUnc:N0}"; 
            set
            {
                long.TryParse(value, out _youshaUnc);
                _sumTicketLoan = _youshaUnc + _youshaSyo;
            }
        }
        public float _youZeiritsu;
        public string YouZeiritsu
        {
            get => IsRowSumResult ?  string.Empty : $"{(_youZeiritsu / 100):P1}";
            set => float.TryParse(value, out _youZeiritsu);
        }
        public long _youshaSyo;
        public string YoushaSyo
        {
            get => $"{_youshaSyo:N0}";
            set
            {
                long.TryParse(value, out _youshaSyo);
                _sumTicketLoan = _youshaUnc + _youshaSyo;
            }
        }
        public float _youTesuRitu;
        public string YouTesuRitu
        {
            get => IsRowSumResult ? string.Empty : $"{(_youTesuRitu / 100):P1}";
            set => float.TryParse(value, out _youTesuRitu);
        }
        public long _youshaTes;
        public string YoushaTes
        {
            get => $"{_youshaTes:N0}";
            set => long.TryParse(value, out _youshaTes);
        }
        public long _sumTicketLoan;
        public string SumTicketLoan
        {
            get => $"{_sumTicketLoan:N0}";
            set => long.TryParse(value, out _sumTicketLoan);
        }

        public float _youFutTumGuiKin;
        public string YouFutTumGuiKin
        {
            get => GetOutput($"{_youFutTumGuiKin:N0}");
            set => float.TryParse(value, out _youFutTumGuiKin);
        }
        public float _youFutTumGuiTax;
        public string YouFutTumGuiTax
        { 
            get => GetOutput($"{_youFutTumGuiTax:N0}");
            set => float.TryParse(value, out _youFutTumGuiTax);
        }
        public float _youFutTumGuiTes;
        public string YouFutTumGuiTes
        {
            get => GetOutput($"{_youFutTumGuiTes:N0}");
            set => float.TryParse(value, out _youFutTumGuiTes);
        }
        public float _totalYouFutTumGui;
        public string TotalYouFutTumGui
        {
            get => GetOutput($"{_totalYouFutTumGui:N0}");
            set => float.TryParse(value, out _totalYouFutTumGui);
        }

        public long _youFutTumKin;
        public string YouFutTumKin
        {
            get => GetOutput($"{_youFutTumKin:N0}");
            set => long.TryParse(value, out _youFutTumKin);
        }
        public long _youFutTumTax;
        public string YouFutTumTax
        {
            get => GetOutput($"{_youFutTumTax:N0}");
            set => long.TryParse(value, out _youFutTumTax);
        }
        public long _youFutTumTes;
        public string YouFutTumTes
        {
            get => GetOutput($"{_youFutTumTes:N0}");
            set => long.TryParse(value, out _youFutTumTes);
        }
        public long _totalYouFutTum;
        public string TotalYouFutTum
        {
            get => GetOutput($"{_totalYouFutTum:N0}");
            set => long.TryParse(value, out _totalYouFutTum);
        }

        //Display Field with row total
        public string DisplayTokiStRyakuNm
        {
            get
            {
                return IsRowSumResult ? RowTotalDisplayText : TokiStRyakuNm;
            }
        }
        public string DisplayBusScheduleInfo
        {
            get => IsRowSumResult ? $"傭車計  {TotalBusLoan}台" : H_HaiScheduleCode;
        }

        //Total
        public string RowTotalDisplayText { get; set; }
        public int TotalBusLoan { get; set; }

        public string TotalSumSyaRyoUnc { set => SumSyaRyoUnc = value; }
        public string TotalSumZeiRui { set => SumZeiRui = value; }
        public string TotalSumTesuRyoG { set => SumTesuRyoG = value; }
        public string TotalSumTicket { set => SumTicket = value; }

        public string TotalSumGuideFee { set => SumGuideFee = value; }
        public string TotalSumGuideTax { set => SumGuideTax = value; }
        public string TotalSumUnitGuiderFee { set => SumUnitGuiderFee = value; }
        public string TotalTotalGuider { set => TotalGuider = value; }
     
        public string TotalIncidentalFee { set => IncidentalFee = value; }
        public string TotalIncidentalTax {  set => IncidentalTax = value; }
        public string TotalIncidentalCharge { set => IncidentalCharge = value; }
        public string TotalTotalIncidental { set => TotalIncidental = value; }

        public string TotalYoushaUnc { set => YoushaUnc = value; }
        public string TotalYoushaSyo { set => YoushaSyo = value; }
        public string TotalYoushaTes { set => YoushaTes = value; }
                      
        public string TotalYouFutTumGuiKin { set => YouFutTumGuiKin = value; }
        public string TotalYouFutTumGuiTax { set => YouFutTumGuiTax = value; }
        public string TotalYouFutTumGuiTes { set => YouFutTumGuiTes = value; }
                      
        public string TotalYouFutTumKin { set => YouFutTumGuiKin = value; }
        public string TotalYouFutTumTax { set => YouFutTumGuiTax = value; }
        public string TotalYouFutTumTes { set => YouFutTumGuiTes = value; }
    }
}
