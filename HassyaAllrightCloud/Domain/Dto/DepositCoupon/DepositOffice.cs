using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto.DepositCoupon
{
    public class DepositOffice
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Text => $"{Code} : {Name}";
    }
}
