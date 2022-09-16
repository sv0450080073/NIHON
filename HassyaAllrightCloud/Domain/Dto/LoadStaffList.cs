using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Entities;
using System;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class LoadStaff
    {
        public int SyainCdSeq { get; set; }
        public string SyainCd { get; set; }
        public string SyainNm { get; set; }
        public int EigyoCdSeq { get; set; }
        public int EigyoCd { get; set; }
        public string TenkoNo { get; set; }
        public DateTime StaYmd { get; set; }
        public DateTime EndYmd { get; set; }
        public string CompanyName { get; set; }

        public string Text
        {
            get
            {
                return string.Format("{0} : {1}", SyainCd, SyainNm);
            }
        }
        /// <summary>
        /// Get staff data display text.
        /// <para>Format => [SyainCd : SyainNm]</para>
        /// <para>If SyainCdSeq is 0, return "すべて"</para>
        /// <para>Else if can not convert to any format will return empty string</para>
        public string StaffText
        {
            get
            {
                if(SyainCdSeq > 0 && !string.IsNullOrEmpty(SyainNm) && !string.IsNullOrEmpty(SyainCd))
                {
                    return string.Format("{0} : {1}", long.Parse(SyainCd).ToString("D10"), SyainNm);
                }

                if (SyainCdSeq == 0)
                    return Constants.SelectedAll;

                return string.Empty;
            }
        }
    }

    public class LoadStaffList
    {
        public int SyainCdSeq { get; set; } = -1;
        public string SyainCd { get; set; }
        public string SyainNm { get; set; } 
        public int EigyoCdSeq { get; set; }
        public string TenkoNo { get; set; }
        public string StaYmd { get; set; }
        public string EndYmd { get; set; }
        public int EigyoCd { get; set; }
        public string RyakuNm { get; set; }
        public int SyainCdReport { get; set; }
        public string CompanyName { get; set; }
        public string Text => SyainCdSeq == -1 ? "" : $"{SyainCd} : {SyainNm}";
        public string TextBranch => $"{EigyoCd.ToString("D5")} : {RyakuNm}";
        public string TextReport => SyainCdSeq==-1 ? "すべて" : $"{SyainCd} : {SyainNm}";
        public string StaffText
        {
            get
            {
                if (SyainCdSeq != -1)
                {
                    return $"{SyainCd.Trim().PadLeft(10,'0')} : {SyainNm}";
                }
                return string.Empty;
            }
        }
    }

    public class StaffsData
    {
        public int SyainCdSeq { get; set; } = -1;
        public string SyainCd { get; set; }
        public string Kana { get; set; }
        public string SyainNm { get; set; }
        public string KariSyainNm { get; set; }
        public string NyusyaYmd { get; set; }
        public string TaisyaYmd { get; set; }
        public string BirthYmd { get; set; }
        public byte Seibetsu { get; set; }
        public string ZipCd { get; set; }
        public string Jyus1 { get; set; }
        public string Jyus2 { get; set; }
        public string ApaManNm { get; set; }
        public string TelNo { get; set; }
        public string KeitaiNo { get; set; }
        public string MailAdd1 { get; set; }
        public string MailAdd2 { get; set; }
        public string MenkyoNo { get; set; }
        public string ExpItem { get; set; }
        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
        public int UpdSyainCd { get; set; }
        public string UpdPrgId { get; set; }
        public string Text => SyainCdSeq == -1 ? "" : $"{SyainCd} : {SyainNm}";
        public StaffsData()
        {

        }

        public StaffsData(VpmSyain VpmSyain)
        {
            this.SyainCdSeq = VpmSyain.SyainCdSeq;
            this.SyainCd = VpmSyain.SyainCd;
            this.Kana = VpmSyain.Kana;
            this.SyainNm = VpmSyain.SyainNm;
            this.KariSyainNm = VpmSyain.KariSyainNm;
            this.NyusyaYmd = VpmSyain.NyusyaYmd;
            this.TaisyaYmd = VpmSyain.TaisyaYmd;
            this.BirthYmd = VpmSyain.BirthYmd;
            this.Seibetsu = VpmSyain.Seibetsu;
            this.ZipCd = VpmSyain.ZipCd;
            this.Jyus1 = VpmSyain.Jyus1;
            this.Jyus2 = VpmSyain.Jyus2;
            this.ApaManNm = VpmSyain.ApaManNm;
            this.TelNo = VpmSyain.TelNo;
            this.KeitaiNo = VpmSyain.KeitaiNo;
            this.MailAdd1 = VpmSyain.MailAdd1;
            this.MailAdd2 = VpmSyain.MailAdd2;
            this.MenkyoNo = VpmSyain.MenkyoNo;
            this.ExpItem = VpmSyain.ExpItem;
            this.UpdYmd = VpmSyain.UpdYmd;
            this.UpdTime = VpmSyain.UpdTime;
            this.UpdSyainCd = VpmSyain.UpdSyainCd;
            this.UpdPrgId = VpmSyain.UpdPrgId;
        }
    }
}
