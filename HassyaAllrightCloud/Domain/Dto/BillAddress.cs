using System;
using HassyaAllrightCloud.Commons.Constants;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class BillAddress
    {
        /// <summary>
		/// Mark this object is selected all item. Default values is <c>false</c>
		/// </summary>
		public bool IsSelectedAll { get; set; } = false;

        public int TokuiSeq { get; set; } = -1;
        public int SitenCdSeq { get; set; }
        public int TokuiCd { get; set; }
        /// <summary>
        /// Specified for <see cref="VpmTokisk.RyakuNm"/>
        /// </summary>
        public string RyakuNm { get; set; }
        public int SitenCd { get; set; }
        public string SitenNm { get; set; }
        /// <summary>
        /// Specified for <see cref="VpmTokiSt.RyakuNm"/>
        /// </summary>
        public string SitenRyakuNm { get; set; }
        public decimal TesuRitu { get; set; } = 0;
        public decimal TesuRituGui { get; set; } = 0;
        public int GyoSysSeq { get; set; }
        public int GyoSyaCd { get; set; }
        public string GyoSyaNm { get; set; }
        public int SimeD { get; set; }
        public string TOKISK_TokuiNm { get; set; }
        public string Code { get => $"{GyoSyaCd:000}{TokuiCd:0000}{SitenCd:0000}"; }
        public string Text
        {
            get
            {
                if (TokuiSeq == -1) { return ""; }
                else { return String.Format("{0}-{1}-{2} : {3} {4}", GyoSyaCd.ToString("D3"), TokuiCd.ToString("D4"), SitenCd.ToString("D4"), RyakuNm, SitenNm); }
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
        public string TextReport => TokuiSeq == -1 && SitenCdSeq == 0 ? "すべて" : String.Format("{0}-{1}-{2} : {3} {4}", GyoSyaCd.ToString("D3"), TokuiCd.ToString("D4"), SitenCd.ToString("D4"), RyakuNm, SitenNm);


        /// <summary>
        /// Get customer info as format: [TokuiCd:TokiskRyakuNm SitenCd:SitenRyakuNm]
        /// <para>Return <see cref="String.Empty"/> if one of them is empty or default</para>
        /// </summary>
        public string CustomerInfo
        {
            get
            {
                if (IsSelectedAll || TokuiSeq == 0)
                    return Constants.SelectedAll;
                if (TokuiSeq == -1)
                    return string.Empty;
                return $"{TokuiCd.ToString("D4")} : {RyakuNm} {SitenCd.ToString("D4")} : {SitenRyakuNm}";
            }
        }
    }
}
