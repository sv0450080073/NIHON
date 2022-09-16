using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Entities
{
    public partial class VpmTenantGroupDetailTokui
    {
        public int TenantGroupCdSeq { get; set; }
        public int TenantCdSeq { get; set; }
        public int SitenCdSeqTenantCdSeq { get; set; }
        public int TokuiSeq { get; set; }
        public int SitenCdSeq { get; set; }
        public string StaYmd { get; set; }
        public string EndYmd { get; set; }
        public byte SiyoKbn { get; set; }
        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
        public int UpdSyainCd { get; set; }
        public string UpdPrgId { get; set; }
    }
}
