using HassyaAllrightCloud.Commons.Constants;
using System;
using System.Reflection;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class CancelTickedData
    {
        private int _cancelFee;
        private int _cancelTaxFee;

        public int UriGakKin { get; set; }
        /// <summary>
        /// This property to confirm that CancelFee and CancelTaxFee will calculate by UriGakKin and Rate
        /// or get from database
        /// </summary>
        public bool IsSetDefaultFee { get; set; } = true;
        public byte Status { get; set; } = 1;

        public bool CancelStatus { get; set; }
        public bool ReusedStatus { get; set; }
        public string BusPriceIncludeTaxFee { get; set; }
        public SettingStaff CanceledSettingStaffData { get; set; }
        public string CancelReason { get; set; } = string.Empty;
        public DateTime CancelDate { get; set; } = DateTime.Today;

        private float _cancelFeeRate = 10f; // need update to TKD_Yyksho.TesuRitu
        /// <summary>
        /// Get/set cancel fee rate value as string.
        /// </summary>
        public string CancelFeeRate
        {
            get
            {
                return _cancelFeeRate.ToString();
            }
            set
            {
                if (float.TryParse(value, out float newValue))
                {
                    _cancelFeeRate = (float)Math.Truncate(newValue * 10) / 10;
                }
            }
        }

        public string CancelFee
        {
            get
            {
                return _cancelFee.ToString();
            }
            set
            {
                int.TryParse(value, out _cancelFee);
            }
        }
        public TaxTypeList CancelTaxType { get; set; } = new TaxTypeList();

        private float _cancelTaxRate { get; set; } = 10f;
        /// <summary>
        /// Get/set cancel tax rate value as string.
        /// </summary>
        public string CancelTaxRate
        {
            get
            {
                return _cancelTaxRate.ToString();
            }
            set
            {
                if (float.TryParse(value, out float newValue))
                {
                    _cancelTaxRate = (float)Math.Truncate(newValue * 10) / 10;
                }
            }
        }

        public string CancelTaxFee
        {
            get
            {
                return _cancelTaxFee.ToString();
            }
            set
            {
                int.TryParse(value, out _cancelTaxFee);
            }
        }
        public string CancelTotalFee
        {
            get
            {
                int cancelTotalFee = 0;
                if(CancelTaxType.IdValue == Constants.InTax.IdValue)
                {
                    cancelTotalFee = _cancelFee;
                }
                else
                {
                    cancelTotalFee = _cancelFee + _cancelTaxFee;
                }
                return cancelTotalFee.ToString();
            }
        }
        public DateTime ReusedDate { get; set; } = DateTime.Today;
        public SettingStaff ReusedSettingStaffData { get; set; }
        public string ReusedReason { get; set; } = string.Empty;

        public void SetData(CancelTickedData newData)
        {
            foreach (var item in this.GetType().GetProperties())
            {
                if(item.CanWrite && item.CanRead)
                    item.SetValue(this, newData.GetType().GetProperty(item.Name).GetValue(newData));
            }
        }
    }

    public class SettingStaff
    {
        public int SyainCdSeq { get; set; }
        public string SyainCd { get; set; }
        public string SyainNm { get; set; }
        public int EigyoCdSeq { get; set; }
        public string TenkoNo { get; set; }
        public string StaYmd { get; set; }
        public string EndYmd { get; set; }
        public string Text
        {
            get
            {
                return string.Format("{0} : {1}", SyainCd, SyainNm);
            }
        }
    }
}
