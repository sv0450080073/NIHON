using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class CustomGroup
    {
        public int CusGrpSeq { get; set; }
        public string GrpNnm { get; set; }
        public string Text
        {
            get
            {
                return GrpNnm;
            }
        }
    }
}
