using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using System;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class SupplierData
	{
		/// <summary>
		/// Mark this object is selected all item. Default values is <c>false</c>
		/// </summary>
		public bool IsSelectedAll { get; set; } = false;
		public int SirCdSeq { get; set; }
		public int SirSitenCdSeq { get; set; }
		public int TokuiCd { get; set; }
		public int SitenCd { get; set; }
		public string RyakuNm { get; set; }
		public string SitenNm { get; set; }
		public string Text
		{
			get
			{
				return String.Format("{0} : {1} {2} : {3}", TokuiCd.ToString("D4"), RyakuNm, SitenCd.ToString("D4"), SitenNm);
			}
		}
		public string TextReport => SirCdSeq == 0 ? "すべて" : $"{TokuiCd.ToString("D4")}：{RyakuNm} {SitenCd.ToString("D4")}:{SitenNm}";

		/// <summary>
		/// Get Supplier info as format: [TokuiCd:TokiskRyakuNm SitenCd:SitenRyakuNm]
		/// <para>Return <see cref="String.Empty"/> if one of them is empty or default</para>
		/// </summary>
		public string SupplierInfo
		{
			get
			{
				if (IsSelectedAll || SirCdSeq == 0)
					return Constants.SelectedAll ;
				if (SirCdSeq< 0 || string.IsNullOrEmpty(RyakuNm) || string.IsNullOrEmpty(SitenNm))
					return string.Empty;
				return $"{TokuiCd.ToString("D4")}:{RyakuNm} {SitenCd.ToString("D4")}:{SitenNm}";
			}
		}
	}

	public class SupervisorTabData
	{
		public string KanJNm { get; set; }
		public string KanjTel { get; set; }
		public string KanjFax { get; set; }
		public string KanjJyus1 { get; set; }
		public string KanjJyus2 { get; set; }
		public string KanjKeiNo { get; set; }
		public string KanjMail { get; set; }
		public bool KanDMHFlg { get; set; }
		public CustomerComponentGyosyaData customerComponentGyosyaData { get; set; } = new CustomerComponentGyosyaData();
		public CustomerComponentTokiskData customerComponentTokiskData { get; set; }
		public CustomerComponentTokiStData customerComponentTokiStData { get; set; }
		public string TokuiTanNm { get; set; }
		public string TokuiTel { get; set; }
		public string TokuiFax { get; set; }
		public string TokuiMail { get; set; }
		private int _jyoSyaJin;
		public string JyoSyaJin
		{
			get
			{
				return _jyoSyaJin.ToString();
			}
			set
			{
				Int32.TryParse(value, out _jyoSyaJin);
			}
		}
		public string JyoSyaJinDisplay
        {
            get
            {
				if (_jyoSyaJin == 0) return "00";
				return _jyoSyaJin.ToString("N0");
			}
        }
		private int _plusJin;
		public string PlusJin
		{
			get
			{
				return _plusJin.ToString();
			}
			set
			{
				Int32.TryParse(value, out _plusJin);
			}
		}
		public string PlusJinDisplay
        {
            get
            {
				return _plusJin.ToString("N0");
			}
        }
	}
}
