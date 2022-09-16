using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class YoushaNoticeData
    {
        public int MotoTenantCdSeq { get; set; }
        public int MotoYouTblSeq { get; set; }
        public string MotoUkeNo { get; set; }
        public short MotoUnKRen { get; set; }
        public int MotoTokuiSeq { get; set; }
        public int MotoSitenCdSeq { get; set; }
        public int UkeTenantCdSeq { get; set; }
        public string TOKISK_RyakuNm { get; set; }
        public string TOKIST_RyakuNm { get; set; }
        public string DanTaNm { get; set; }
        public string HaiSYmd { get; set; }
        public string HaiSTime { get; set; }
        public string TouYmd { get; set; }
        public string TouChTime { get; set; }
        public short BigtypeNum { get; set; }
        public short MediumtypeNum { get; set; }
        public short SmalltypeNum { get; set; }
        public byte UnReadKbn { get; set; }
        public byte RegiterKbn { get; set; }
        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
        public int TypeNoti { get; set; }

    }
}
