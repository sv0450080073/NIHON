using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class SaleOfficeModel
    {
        public int SaleOfficeKbn { get; set; }
        public string SaleOfficeName { get; set; }
        public string Text
        {
            get
            {
                return SaleOfficeName;
            }
        }
    }
}
