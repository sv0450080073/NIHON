namespace HassyaAllrightCloud.Domain.Dto
{
    public class BranchData
    {
        public int EigyoCdSeq { get; set; }
        public int EigyoCd { get; set; }
        public string EigyoNm { get; set; } = string.Empty;
        public string CompanyRyakuNm { get; set; } = string.Empty;
        public int CompanyCdSeq { get; set; }
        public int CompanyCd { get; set; }
        public string CompanyNm { get; set; } = string.Empty;

        /// <summary>
        /// Get branch data info.
        /// <para>Format => [CompanyCd : CompanyRyakuNm EigyoCd : EigyoNm]</para>
        /// <para>Format => [EigyoCd : EigyoNm]</para>
        /// <para>If EigyoCd is 0, return "すべて"</para>
        /// <para>Else if can not convert to any format will return empty string</para>
        /// </summary>
        public string BranchInfo
        {
            get
            {
                if((CompanyCd > 0 && !string.IsNullOrEmpty(CompanyRyakuNm)) 
                    && (EigyoCd > 0 && !string.IsNullOrEmpty(EigyoNm)))
                {
                    return $"{CompanyCd.ToString("D3")}：{CompanyRyakuNm} {EigyoCd.ToString("D4")}：{EigyoNm}";
                }
                else if(EigyoCd == 0)
                    return "すべて";

                return string.Empty;
            }
        }
    }
}
