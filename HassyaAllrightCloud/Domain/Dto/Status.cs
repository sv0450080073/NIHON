using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class Status
    {
        public string id { get; set; }
        public string status { get; set; }
        public string Text
        {
            get
            {
                return status;
            }
        }
    }
}
