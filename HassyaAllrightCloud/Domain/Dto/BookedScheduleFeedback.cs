using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class BookedScheduleFeedback
    {
        public string Title { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public DateTime StartDateDisplay { get; set; }
        public DateTime EndDateDisplay { get; set; }
        public string Creator { get; set; }
        public string Note { get; set; }
        public bool IsAccept { get; set; }
        public bool IsRefuse { get; set; }
        public bool IsPending { get; set; }
        public bool CanFeedback { get; set; }
        public List<ParticipantFbStatus> ParticipantFbStatuses { get; set; }
        public Dictionary<string, int> listStaffFB {get; set; }
        
    }
}
