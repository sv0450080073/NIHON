using HassyaAllrightCloud.Commons.Constants;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class CompanyData
    {
        /// <summary>
        /// Mark this object is selected all item. Default values is <c>false</c>
        /// </summary>
        public bool IsSelectedAll { get; set; } = false;

        public int CompanyCdSeq { get; set; }
        public int CompanyCd { get; set; }
        public string CompanyNm { get; set; } = string.Empty;
        public string RyakuNm { get; set; } = string.Empty;
        public int TenantCdSeq { get; set; }

        /// <summary>
        /// Get text for company info follow format: <see cref="CompanyData.CompanyCd"/> : <see cref="CompanyData.CompanyNm"/>
        /// </summary>
        public string CompanyInfo
        {
            get
            {
                if (IsSelectedAll || CompanyCdSeq == 0)
                    return Constants.SelectedAll;

                return $"{CompanyCd:D5}：{CompanyNm}";
            }
        }
        public string DisplayName
        {
            get
            {
                return $"{CompanyCd:00000} : {RyakuNm}";
            }
        }
        public string Text
        {
            get
            {
                return string.Format("{0}{1}{2}", $"{CompanyCd.ToString("D5")}", RyakuNm.Equals(string.Empty) ? "" : " : ", RyakuNm);
            }
        }
        public string CompanyTextReport
        {
            get
            {
                if (CompanyCdSeq > 0)
                {
                    return $"{CompanyCd:00000} : {RyakuNm}";
                }
                return string.Empty;
            }
        }
    }
}
