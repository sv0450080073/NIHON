using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class Branch
    {
        public int Seg { get; set; }
        public string Name { get; set; }
        public string Text
        {
            get { return Name; }
        }
    }
}
