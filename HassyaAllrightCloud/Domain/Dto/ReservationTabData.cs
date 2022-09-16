using HassyaAllrightCloud.Commons.Extensions;
using System;
using static HassyaAllrightCloud.Commons.Helpers.BookingInputHelper;

namespace HassyaAllrightCloud.Domain.Dto
{
	public class DestinationData
	{
		public int CodeKbnSeq { get; set; }
		public string CodeKbn { get; set; }
		public string TpnCodeKbnRyakuNm { get; set; }
		public int BasyoMapCdSeq { get; set; } = 0; //use this to save
		public string BasyoMapCd { get; set; }
		public string VpmBasyoRyakuNm { get; set; }
		public int BasyoKenCdSeq { get; set; }
		public string Text
		{
			get
			{
				if (CodeKbnSeq == 0)
					return "";
				else if (String.IsNullOrEmpty(VpmBasyoRyakuNm))
					return $"{CodeKbn}:{TpnCodeKbnRyakuNm}";
				return $"{CodeKbn}:{TpnCodeKbnRyakuNm} {BasyoMapCd}:{VpmBasyoRyakuNm}";
			}
		}

		public string TextReport
		{
			get
			{
				if (CodeKbnSeq > 0)
				{
					return $"{CodeKbn} : {TpnCodeKbnRyakuNm} {BasyoMapCd} : {VpmBasyoRyakuNm}";
				}
				return string.Empty;
			}
		}


	}

	public class PlaceData
	{
		public int CodeKbnSeq { get; set; }
		public string CodeKbn { get; set; }
		public string TpnCodeKbnRyakuNm { get; set; }
		public int HaiSCdSeq { get; set; } = 0; //use this to save to 
		public string HaiSCd { get; set; }
		public string VpmHaichiRyakuNm { get; set; }
		public string Text
		{
			get
			{
				if (CodeKbnSeq == 0)
					return "";
				else if(String.IsNullOrEmpty(VpmHaichiRyakuNm))
					return $"{CodeKbn}:{TpnCodeKbnRyakuNm}";
				return $"{CodeKbn}:{TpnCodeKbnRyakuNm} {HaiSCd}:{VpmHaichiRyakuNm}";
			}
		}
	}

	public class ReservationTabData
	{
		public DateTime GarageLeaveDate { get; set; } = DateTime.Today;
		public DateTime GoDate { get; set; } = DateTime.Today;
		public MyTime SyuKoTime { get; set; } = new MyTime();
		//public string DepartureStartDate { get; set; }
		public MyTime SyuPatime { get; set; } = new MyTime();
		public DateTime GarageReturnDate { get; set; } = DateTime.Today;
		public MyTime KikTime { get; set; } = new MyTime();
		public DestinationData Destination { get; set; }
		public string IkNm { get; set; }
		//public string DestinationMap { get; set; }
		public PlaceData DespatchingPlace { get; set; }
		public string HaiSNm { get; set; }
        public string HaiSjyus1 { get; set; }
		public string HaiSjyus2 { get; set; }
		//public string DespatchingPlaceMap { get; set; }
		public PlaceData ArrivePlace { get; set; }
		public string TouNm { get; set; }
        public string TouJyusyo1 { get; set; }
		public string TouJyusyo2 { get; set; }
		//public string ArrivePlaceMap { get; set; }
		public TPM_CodeKbCodeSyuData MovementStatus { get; set; }
		public TPM_CodeKbCodeSyuData AcceptanceConditions { get; set; }
		public TPM_CodeKbCodeSyuData RainyMeasure { get; set; }
		public TPM_CodeKbCodeSyuData PaymentMethod { get; set; }
		public TPM_CodeKbCodeSyuData MovementForm { get; set; }
		public TPM_CodeKbCodeSyuData GuiderSetting { get; set; }
		public TPM_CodeKbCodeSyuData EstimateSetting { get; set; }

		public void SetData(ReservationTabData newData)
        {
			this.SimpleCloneProperties(newData);
		}
	}
}
