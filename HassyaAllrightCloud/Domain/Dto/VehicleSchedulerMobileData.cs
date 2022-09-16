using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class VehicleSchedulerMobileData 
    {
        public string UkeNo { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Name { get; set; }
        public byte Type { get; set; }
        public string Status { get; set; }
        public string Ymd { get; set; }

        public string PopupYmd { get; set; }
        public string Customer { get; set; }
        public string DispatchNote { get; set; }

        public string EigyoName { get; set; }
        public string SyaRyoName { get; set; }

        public int UkeCd { get; set; }
        public bool IsCancel { get; set; } = false;
    }

    public class VehicleAllocationData
    {
        public string UkeNo { get; set; }
        public int HaiSSryCdSeq { get; set; }
        public string DanTaNm { get; set; }
        public byte KSKbn { get; set; }
        public string SyuKoYmd { get; set; }
        public string SyuKoTime { get; set; }
        public string KikYmd { get; set; }
        public string KikTime { get; set; }
        public int TokuiSeq { get; set; }
        public int SitenCdSeq { get; set; }
        public byte NinkaKbn { get; set; }
        public byte KataKbn { get; set; }
        public string TokiskNm { get; set; }
        public string TokiStNm { get; set; }
        public string BikoNm { get; set; }
        public int UkeCd { get; set; }
    }

    public class VehicleRepairData
    {
        public int ShuriCdSeq { get; set; }
        public int SyaRyoCdSeq { get; set; }
        public string ShuriSYmd { get; set; }
        public string ShuriSTime { get; set; }
        public string ShuriEYmd { get; set; }
        public string ShuriETime { get; set; }
        public string BikoNm { get; set; }
        public string SyaRyoNm { get; set; }
        public string SyuRiCd_CodeKbn { get; set; }
        public string SyuRiCd_CodeKbnNm { get; set; }
        public string SyuRiCd_RyakuNm { get; set; }
        public int EigyoCdSeq { get; set; }
        public int Eigyos_EigyoCd { get; set; }
        public string Eigyos_EigyoNm { get; set; }
        public string Eigyos_RyakuNm { get; set; }
    }

    public class VehicleSchedulerEigyoData
    {
        public int EigyoCdSeq { get; set; }
        public int EigyoCd { get; set; }
        public string EigyoName { get; set; }
    }

    public class VehicleSchedulerSyaRyoData
    {
        public int SyaRyoCdSeq { get; set; }
        public string SyaRyoNm { get; set; }
    }

    public class ReservationTokiskData : ICloneable
    {
        public int GyosyaCdSeq { get; set; }
        public short GyosyaCd { get; set; }
        public int TokuiSeq { get; set; }
        public short TokuiCd { get; set; }
        public int SitenCdSeq { get; set; }
        public short SitenCd { get; set; }
        public string Name { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class ReservationCodeKbnData : ICloneable
    {
        public int CodeKbnSeq { get; set; }
        public string CodeKbn { get; set; }
        public string CodeKbnNm { get; set; }
        public int JyoKyakuCdSeq { get; set; }
        public byte JyoKyakuCd { get; set; }
        public string JyoKyakuNm { get; set; }

        public string Text => string.Format("{0} : {1} {2} : {3}", CodeKbn.PadLeft(2, '0'), CodeKbnNm, JyoKyakuCd.ToString().PadLeft(2, '0'), JyoKyakuNm);

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class ReservationSyaSyuData
    {
        public int SyaSyuCdSeq { get; set; }
        public byte KataKbn { get; set; }
        public string KataNm { get; set; }
        public short SyaSyuCd { get; set; }
        public string SyaSyuNm { get; set; }

        public string Text => string.Format("{0} : {1} {2} : {3}", KataKbn, KataNm, SyaSyuCd, SyaSyuNm);
    }

    public class ReservationMobileData : ICloneable
    {
        public string UkeNo { get; set; }
        public int UkeCd { get; set; }
        public string KaktYmd { get; set; }

        public string Organization { get; set; }
        public ReservationTokiskData Tokisk { get; set; }
        public ReservationCodeKbnData CodeKb { get; set; }
        public DateTime DispatchDate { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        public string DispatchTime { get; set; } = "00:00";
        public DateTime ArrivalDate { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        public string ArrivalTime { get; set; } = "23:59";
        public List<ReservationMobileChildItemData> ListItems { get; set; } = new List<ReservationMobileChildItemData>();
        public string Note { get; set; }

        public int TokuiSeq { get; set; }
        public int SitenCdSeq { get; set; }
        public int DantaiCdSeq { get; set; }
        public int JyoKyakuCdSeq { get; set; }

        public object Clone()
        {
            var data = (ReservationMobileData)(this.MemberwiseClone());
            data.ListItems = new List<ReservationMobileChildItemData>();
            data.ListItems.AddRange(this.ListItems);
            if(this.Tokisk != null)
            {
                data.Tokisk = (ReservationTokiskData)(this.Tokisk.Clone());
            }
            else
            {
                data.Tokisk = null;
            }
            if(this.CodeKb != null)
            {
                data.CodeKb = (ReservationCodeKbnData)(this.CodeKb.Clone());
            }
            else
            {
                data.CodeKb = null;
            }
            return data;
        }
    }

    public class ReservationMobileChildItemData
    {
        public short SyaSyuRen { get; set; }
        public int SyaSyuCdSeq { get; set; }
        public int SyaSyuCd { get; set; }
        public byte KataKbn { get; set; }
        public int SyaSyuTan { get; set; }
        public int UnitGuiderPrice { get; set; }

        public ReservationSyaSyuData SyaSyu { get; set; }
        public string BusCount { get; set; } = "1";
        public string DriverCount { get; set; } = "1";
        public string GuiderCount { get; set; } = "0";

        public bool IsAddNew { get; set; } = true;
        public bool IsCanDelete { get; set; } = true;
        public bool IsDisable { get; set; } = false;
    }

    public class HenSyaData
    {
        public int EigyoCdSeq { get; set; }
        public int EigyoCd { get; set; }
        public string EigyoName { get; set; }
    }
}
