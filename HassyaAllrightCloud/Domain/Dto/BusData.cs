namespace HassyaAllrightCloud.Domain.Dto
{
    public class BusCompanyType
    {
        public int BusBranchID { get; set; }
        public int BusCompanyID { get; set; }
        public string BusBranchName { get; set; }
        public string BusCompanyName { get; set; }
    }

    public class BusDataType
    {
        public string BusID { get; set; }
        public string BusName { get; set; }
        public int SitenCdSeq { get; set; }
        public int BusCompanyID { get; set; }
        public int BusBranchID { get; set; }
        public int BusVehicle { get; set; }
        public double BusHeight { get; set; }
        public string SyaSyuNm { get; set; }
        public string SyaRyoNm { get; set; }
        public string RyakuNm { get; set; }
        public string EigyoNm { get; set; }
        public string KariSyaRyoNm { get; set; }
        public int SyaSyuCd { get; set; }
        public int SyaSyuCdSeq { get; set; }
        public int EigyoCd { get; set; }
        public int SyaRyoCd { get; set; }
        public string TenkoNo { get; set; }
        public string BookingID { get; set; }
        public string StaYmd { get; set; }
        public string Text => $"{SyaRyoNm}　{SyaSyuNm}　{EigyoNm}";
        public string EndYmd { get; set; }
        public bool IsOutOfDate { get; set; }
        public bool IsHasBooking { get; set; }
        public bool IsActive { get; set; }
        public int NinkaKbn { get; set; }
        public int TeiCnt { get; set; }
        public int KataKbn { get; set; }
        public string LockYmd { get; set; }
    }
}
