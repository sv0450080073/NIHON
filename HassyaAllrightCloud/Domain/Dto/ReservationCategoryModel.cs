using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class ReservationCategoryModel
    {
        public string PriorityNum { get; set; }
        public string YoyaKbnName { get; set; }
        public string Text
        {
            get
            {
                return YoyaKbnName;
            }
        }
    }
}
