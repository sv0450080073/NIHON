namespace HassyaAllrightCloud.Domain.Dto
{
    public class LoadServiceOffice
    {
        public int OfficeCdSeq { get; set; }
        public int CompanyCd { get; set; }
        public string CompanyName { get; set; }
        public int OfficeCd { get; set; }
        public string OfficeName { get; set; }       
        public string TextReport => OfficeCdSeq == 0 ? "すべて" : $"{OfficeCd.ToString("D4")}：{OfficeName} ";
        public string OfficeText
        {
            get
            {
                if (OfficeCdSeq != 0)
                {
                    return $"{OfficeCd.ToString("D4")}：{OfficeName} ";
                }
                return string.Empty;
            }
        }
    }
}
