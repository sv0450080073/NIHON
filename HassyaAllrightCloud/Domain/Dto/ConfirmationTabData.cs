using System;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class ConfirmationTabData
    {
        private short _kaknNin;

        //public string KaktYmd { get; set; } // yyyyMMdd
        public int FixDataNo { get; set; }
        public DateTime KaknYmd { get; set; } = DateTime.Today;
        public string KaknAit { get; set; }
        public string KaknNin
        {
            get
            {
                return _kaknNin.ToString();
            }
            set
            {
                Int16.TryParse(value, out _kaknNin);
            }
        }
        public bool SaikFlg { get; set; }
        public bool DaiSuFlg { get; set; }
        public bool KingFlg { get; set; }
        public bool NitteFlag { get; set; }

        public void SetData(ConfirmationTabData newData)
        {
            foreach (var item in this.GetType().GetProperties())
            {
                if (item.CanWrite && item.CanRead)
                    item.SetValue(this, newData.GetType().GetProperty(item.Name).GetValue(newData));
            }
        }
    }
}
