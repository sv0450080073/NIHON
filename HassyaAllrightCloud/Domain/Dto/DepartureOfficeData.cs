namespace HassyaAllrightCloud.Domain.Dto
{
    public class DepartureOfficeData
    {
        public int CompanyCdSeq { get; set; }
        public int CompanyCd { get; set; }
        public string CompanyNm { get; set; }
        public int EigyoCdSeq { get; set; }
        public int EigyoCd { get; set; }
        public string Com_RyakuNm { get; set; }
        public string RyakuNm { get; set; }
        public string EigyoNm { get; set; }
        public string Text
        {
            get
            {
                if(EigyoCdSeq == 0)
                {
                    return string.Empty;
                }
                else
                {
                    return string.Format("{0:D3}：{1}　{2:D5}：{3}", CompanyCd, Com_RyakuNm, EigyoCd, EigyoNm);
                }
            }
        }
        public string EigyoCodeName
        {
            get
            {
                if (EigyoCdSeq != 0)
                {
                    return $"{EigyoCd.ToString("D5")} : {EigyoNm}";
                }
                return string.Empty;
            }
        }
    }
}