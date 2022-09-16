using System;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class ItemBus
    {
        public string BookingId { get; set; }
        public short haUnkRen { get; set; }
        public short TeiDanNo { get; set; }
        public short BunkRen { get; set; }
        public short HenKai { get; set; }
        public string Id { get; set; }
        public int UkeCd { get; set; }
        public string BusLine { get; set; }
        public string Name { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Top { get; set; }
        public double Left { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public Int32 TimeStart { get; set; }
        public Int32 TimeEnd { get; set; }
        public object Tooltip { get; set; }
        public string ColorLine { get; set; }
        public string CCSStyle { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public int Order { get; set; }
        public string StartDateDefault { get; set; }
        public string EndDateDefault { get; set; }
        public Int32 TimeStartDefault { get; set; }
        public Int32 TimeEndDefault { get; set; }
        public double LeftDefault { get; set; }
        public double WidthDefault { get; set; }
        public string CCSStyleDefault { get; set; }
        public double MaxWidth { get; set; } = -1;
        public double MinLeft { get; set; } = -1;
        public string DanTaNm { get; set; }
        public string IkNm { get; set; }
        public string TokuiNm { get; set; }
        public string Text { get; set; }
        public string MinDate { get; set; }
        public int MinTime { get; set; }
        public string Maxdate { get; set; }
        public int MaxTime { get; set; }
        public bool AllowEdit { get; set; } = true;
        public bool AllowDrop { get; set; } = true;
        public bool AllowCut { get; set; } = true;
        public int BookingType { get; set; }
        public string HasYmd { get; set; }
        public string SihYotYmd { get; set; }
        public string SihYm { get; set; }
        public decimal Zeiritsu { get; set; }
        public string CodeKb_CodeKbn { get; set; }
        public int YykSyu_KataKbn { get; set; }
        public string SyaSyu_SyaSyuNm { get; set; }
        public string SyaSyu_SyaSyuNm_Haisha { get; set; }
        public string Tokisk_YouSRyakuNm { get; set; }
        public int Tokisk_SitenCdSeq { get; set; }
        public int KSKbn { get; set; }
        public int YouTblSeq { get; set; }
        public string BusLineType { get; set; }
        public string BusName { get; set; }
        public int BusVehicle { get; set; }
        public double TimeStartString => double.Parse(StartDateDefault + TimeStartDefault.ToString("D4"));
        public double TimeEndString => double.Parse(EndDateDefault + TimeEndDefault.ToString("D4"));
        public string TokiSk_RyakuNm { get; set; }
        public string TokiSt_RyakuNm { get; set; }
        public int Shuri_ShuriTblSeq { get; set; }
        public int SyaSyuRen { get; set; }
        public int SyaSyuCdSeq { get; set; }
        public int SyuEigCdSeq { get; set; }
        public int KikEigSeq { get; set; }
        public bool IsBusRepair { get; set; } = false;
        public int ShuriCdSeq { get; set; }
        public string BikoNm { get; set; }
        public int NumberDriver { get; set; } = 0;
        public int NumberGuider { get; set; } = 0;
        public int TotalNumberDriver { get; set; } = 0;
        public int TotalNumberGuider { get; set; } = 0;
        public int BranchId { get; set; }
        public int CompanyId { get; set; }
        public int BunKSyuJyn { get; set; }
        public int JyoSyaJin { get; set; }
        public bool CanShowMenu { get; set; } = true;
        public bool CanBeDeleted { get; set; } = true;
        public bool CanSimpledispatch { get; set; } = true;
        public Double minleftcreate { get; set; }
        public Double maxwidthcreate { get; set; }
        public bool IsShow { get; set; } = false;
        public string Unkobi_StartYmd { get; set; }
        //NinkaKbn
        public int Syasyu_KataKbn { get; set; }
        public int SyaRyo_NinKaKbn { get; set; }
        public string SeiTaiYmd { get; set; }
        public int HaiSKbn { get; set; }
        public int HaiSsryCdSeq { get; set; }
        public string SyuKoYmd { get; set; }
        public string KikYmd { get; set; }
        public byte NippoKbn { get; set; }
    }
}
