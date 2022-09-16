namespace HassyaAllrightCloud.Domain.Dto
{
    public class LoadLocation
    {
        public string CodeKbn { get; set; }
        public string CodeKbnNm { get; set; }
        public string BasyoMapCd { get; set; }
        public string BasyoNm { get; set; }
        public long CodeKbnSeq { get; set; }
        public int BasyoMapCdSeq { get; set; }
        public int BasyoKenCdSeq { get; set; }
        public string Text => $" {CodeKbnNm} {BasyoMapCd} : {BasyoNm}";
    }
}
