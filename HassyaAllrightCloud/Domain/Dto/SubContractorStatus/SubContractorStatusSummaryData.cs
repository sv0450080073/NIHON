namespace HassyaAllrightCloud.Domain.Dto.SubContractorStatus
{
    using System;

    public class SubContractorStatusSummaryData
    {
        private int _totalRecord = 0;
        public string TotalRecords
        {
            get => _totalRecord.ToString();
            set => int.TryParse(value, out _totalRecord);
        }

        public long _totalSyaRyoUnc = 0;
        public string TotalSyaRyoUnc
        {
            get => $"{_totalSyaRyoUnc:N0}";
            set => long.TryParse(value, out _totalSyaRyoUnc);
        }

        public long _totalZeiRui = 0;
        public string TotalZeiRui
        {
            get => String.Format("{0:N0}", _totalZeiRui);
            set => long.TryParse(value, out _totalZeiRui);
        }

        public long _totalTesuRyoG = 0;
        public string TotalTesuRyoG
        {
            get => String.Format("{0:N0}", _totalTesuRyoG);
            set => long.TryParse(value, out _totalTesuRyoG);
        }

        //
        public long _totalGuideFee = 0;
        public string TotalGuideFee
        {
            get => String.Format("{0:N0}", _totalGuideFee);
            set => long.TryParse(value, out _totalGuideFee);
        }

        public long _totalGuideTax = 0;
        public string TotalGuideTax
        {
            get => String.Format("{0:N0}", _totalGuideTax);
            set => long.TryParse(value, out _totalGuideTax);
        }

        public long _totalUnitGuiderFee = 0;
        public string TotalUnitGuiderFee
        {
            get => String.Format("{0:N0}", _totalUnitGuiderFee);
            set => long.TryParse(value, out _totalUnitGuiderFee);
        }

        //
        public long _totalIncidentalFee = 0;
        public string TotalIncidentalFee
        {
            get => String.Format("{0:N0}", _totalIncidentalFee);
            set => long.TryParse(value, out _totalIncidentalFee);
        }

        public long _totalIncidentalTax = 0;
        public string TotalIncidentalTax
        {
            get => String.Format("{0:N0}", _totalIncidentalTax);
            set => long.TryParse(value, out _totalIncidentalTax);
        }

        public long _totalIncidentalCharge = 0;
        public string TotalIncidentalCharge
        {
            get => String.Format("{0:N0}", _totalIncidentalCharge);
            set => long.TryParse(value, out _totalIncidentalCharge);
        }

        //
        public long _totalYoushaUnc = 0;
        public string TotalYoushaUnc
        {
            get => String.Format("{0:N0}", _totalYoushaUnc);
            set => long.TryParse(value, out _totalYoushaUnc);
        }

        public long _totalYoushaSyo = 0;
        public string TotalYoushaSyo
        {
            get => String.Format("{0:N0}", _totalYoushaSyo);
            set => long.TryParse(value, out _totalYoushaSyo);
        }

        public long _totalYoushaTes = 0;
        public string TotalYoushaTes
        {
            get => String.Format("{0:N0}", _totalYoushaTes);
            set => long.TryParse(value, out _totalYoushaTes);
        }

        //
        public long _totalYouFutTumGuiKin = 0;
        public string TotalYouFutTumGuiKin
        {
            get => String.Format("{0:N0}", _totalYouFutTumGuiKin);
            set => long.TryParse(value, out _totalYouFutTumGuiKin);
        }

        public long _totalYouFutTumGuiTax = 0;
        public string TotalYouFutTumGuiTax
        {
            get => String.Format("{0:N0}", _totalYouFutTumGuiTax);
            set => long.TryParse(value, out _totalYouFutTumGuiTax);
        }

        public long _totalYouFutTumGuiTes = 0;
        public string TotalYouFutTumGuiTes
        {
            get => String.Format("{0:N0}", _totalYouFutTumGuiTes);
            set => long.TryParse(value, out _totalYouFutTumGuiTes);
        }
        //
        public long _totalYouFutTumKin = 0;
        public string TotalYouFutTumKin
        {
            get => String.Format("{0:N0}", _totalYouFutTumKin);
            set => long.TryParse(value, out _totalYouFutTumKin);
        }

        public long _totalYouFutTumTax = 0;
        public string TotalYouFutTumTax
        {
            get => String.Format("{0:N0}", _totalYouFutTumTax);
            set => long.TryParse(value, out _totalYouFutTumTax);
        }

        public long _totalYouFutTumTes = 0;
        public string TotalYouFutTumTes
        {
            get => String.Format("{0:N0}", _totalYouFutTumTes);
            set => long.TryParse(value, out _totalYouFutTumTes);
        }

        //
        public string Profit
        {
            get
            {
                return @$"{(_totalSyaRyoUnc + _totalZeiRui + _totalGuideFee + _totalGuideTax + _totalIncidentalFee + _totalIncidentalTax 
                    - _totalYoushaUnc - _totalYoushaSyo - _totalYouFutTumGuiKin - _totalYouFutTumGuiTax - _totalYouFutTumKin - _totalYouFutTumTax):N0}";
            }
        }
    }
}
