using HassyaAllrightCloud.Commons.Constants;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class ConfigStaff
    {
        public int Group { get; set; }
        public int Time { get; set; }
        public int View { get; set; }
        public int Sort { get; set; }
        public int Display { get; set; }
        public int Display2 { get; set; }
        public int CrewSort { get; set; }
        public int WorkSort { get; set; }
        public EigyoStaffItem Eigyo { get; set; }
        public int CompanyCdSeq { get; set; } = new ClaimModel().CompanyID;
        public int TenantCdSeq { get; set; } = new ClaimModel().TenantID;
        public int ShowTimeGrid { get; set; }
        public int ShowTime { get; set; }
    }
}
