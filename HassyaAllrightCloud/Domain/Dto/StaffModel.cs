using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class StaffModel
    {
        public int id { get; set; }
        public string SyainCd { get; set; }
        public string EmployeeName { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
        public string OneWeekWorkingHour { get; set; }
        public string FourWeekWorkingHour { get; set; }
        public string Employeeid { get; set; }
        public string Text
        {
            get
            {
                return SyainCd.Trim() + " "+EmployeeName.Trim()+" "+ OneWeekWorkingHour+" "+ FourWeekWorkingHour;
            }
        }
    }
}
