using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class CalendarSetModel
    {
        public int CalendarSeq { get; set; }
        public string CalendarName { get; set; }
        public int CompanyCdSeq { get; set; }
        public string CompanyNm { get; set; }
    }
}
