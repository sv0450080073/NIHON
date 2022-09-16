using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class TenantGroupData
    {
        public int TenantGroupCdSeq { get; set; }
        public int TenantCdSeq { get; set; }
        public int TokuiSeq { get; set; }
        public int SitenCdSeq { get; set; }
        public int SitenCdSeqTenantCdSeq { get; set; }
    }
}
