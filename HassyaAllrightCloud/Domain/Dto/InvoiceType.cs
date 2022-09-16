namespace HassyaAllrightCloud.Domain.Dto
{
    public class InvoiceType
    {
        public int CodeKbnSeq { get; set; }
        public string CodeSyu { get; set; }
        public string CodeKbn { get; set; }
        public string CodeKbnNm { get; set; }
        public string RyakuNm { get; set; }
        public string CodeSeiKbn { get; set; }
        public byte SiyoKbn { get; set; }
        public string Text
        {
            get
            {
                return string.Format("{0} : {1}", CodeKbn, CodeKbnNm);
            }
        }
    }
}
