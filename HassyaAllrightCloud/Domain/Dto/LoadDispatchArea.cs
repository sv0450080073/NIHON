namespace HassyaAllrightCloud.Domain.Dto
{
    public class LoadDispatchArea
    {
        public string CodeKbn { get; set; }
        public string CodeKbnNm { get; set; }
        public string HaiSCd { get; set; }
        public string HaiSNm { get; set; }
        public long CodeKbnSeq { get; set; }
        public int HaiScdSeq { get; set; }
        public int BunruiCdSeq { get; set; }
        public string Text => $" {CodeKbnNm} {HaiSCd} : {HaiSNm}";
    }
}
