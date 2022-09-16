using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Entities;
using System;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class LoadCustomerList
    {
        /// <summary>
		/// Mark this object is selected all item. Default values is <c>false</c>
		/// </summary>
		public bool IsSelectedAll { get; set; } = false;

        public int TokuiSeq { get; set; } = -1;
        public int SitenCdSeq { get; set; }
        public int TokuiCd { get; set; }
        /// <summary>
        /// Stand for <see cref="VpmTokisk.RyakuNm"/>
        /// </summary>
        public string RyakuNm { get; set; }
        public int SitenCd { get; set; }
        public string SitenNm { get; set; }
        /// <summary>
        /// Stand for <see cref="VpmTokiSt.RyakuNm"/>
        /// </summary>
        public string SitenRyakuNm { get; set; }
        public decimal TesuRitu { get; set; } = 0;
        public byte TesKbn { get; set; } = 0;
        public decimal TesuRituGui { get; set; } = 0;
        public byte TesKbnGui { get; set; } = 0;
        public int GyoSysSeq { get; set; }
        public int GyoSyaCd { get; set; }
        public string GyoSyaNm { get; set; }
        public byte GyosyaKbn { get; set; }
        public int SimeD { get; set; }
        public string TOKISK_TokuiNm { get; set; }
        public int SirCdSeq { get => TokuiSeq; set => TokuiSeq = value; }
        public int SirSitenCdSeq { get => SitenCdSeq; set => SitenCdSeq = value; }
        public string Code { get => $"{GyoSyaCd:000}{TokuiCd:0000}{SitenCd:0000}"; }
        public string TokuiTel { get; set; }
        public string TokuiTanNm { get; set; }
        public string TokuiFax { get; set; }
        public string TokuiMail { get; set; }
        public string Text
        {
            get
            {
                if (TokuiSeq == -1) { return ""; }
                else { return String.Format("{0} : {1} {2} : {3}", TokuiCd.ToString("D4"), RyakuNm, SitenCd.ToString("D4"), SitenNm); }
            }
        }
        public string TextReport => TokuiSeq == -1 && SitenCdSeq == 0 ? "すべて" : $"{TokuiCd.ToString("D4")}：{TOKISK_TokuiNm} {SitenCd.ToString("D4")}:{SitenNm}";
        public string CustomerText
        {
            get
            {
                if (TokuiSeq != -1 && SitenCdSeq != 0 )
                {
                    return $"{TokuiCd.ToString("D4")}：{TOKISK_TokuiNm} {SitenCd.ToString("D4")}:{SitenNm}";
                }
                return string.Empty;
            }
        }
        public string TextCondition
        {
            get
            {
                if (TokuiSeq == -1) { return ""; }
                else { return String.Format("{0}{1}{2}", GyoSyaCd.ToString("D3"), TokuiCd.ToString("D4"), SitenCd.ToString("D4")); }
            }
        }
        


        /// <summary>
        /// Get customer info as format: [TokuiCd:TokiskRyakuNm SitenCd:SitenRyakuNm]
        /// </summary>
        public string CustomerInfo
        {
            get
            {
                if (IsSelectedAll)
                    return Constants.SelectedAll;
                return $"{TokuiCd.ToString("D4")} : {RyakuNm} {SitenCd.ToString("D4")} : {SitenRyakuNm}";
            }
        }
        public string CustomerCancelListInfo
        {
            get
            {
                if (IsSelectedAll || TokuiSeq == 0)
                    return Constants.SelectedAll;
                if (TokuiSeq == -1)
                    return string.Empty;
                return $"{TokuiCd:D4} : {TOKISK_TokuiNm} {SitenCd:D4} : {SitenRyakuNm}";
            }
        }

        /// <summary>
        /// Get customer info as format: [TokuiCd-SitenCd-GyosyaCd：Toku.RyakuNm　Toshi.RyakuNm]
        /// </summary>
        public string TextTSG
        {
            get => $"{TokuiCd:D4}-{SitenCd:D4}-{GyoSyaCd:D3}：{RyakuNm} {SitenRyakuNm}";
        }
    }
}
