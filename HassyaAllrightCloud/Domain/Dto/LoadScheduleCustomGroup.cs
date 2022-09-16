using System.Collections.Generic;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class GroupScheduleWithMembers
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int MemberId { get; set; }
    }
    public class GroupScheduleInfo
    {
        public int CompanyId { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public List<int> MembersId { get; set; }
    }
}
