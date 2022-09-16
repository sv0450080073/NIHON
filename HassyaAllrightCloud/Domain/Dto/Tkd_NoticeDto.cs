using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class Tkd_NoticeListDto
    {
        public int NoticeCdSeq { get; set; }
        public string NoticeContent { get; set; }
        public byte NoticeDisplayKbn { get; set; }
        public byte SiyoKbn { get; set; }
        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
        public int UpdSyainCd { get; set; }
        public string UpdPrgId { get; set; }
        public string SyainNm { get; set; }
        public bool isEdit { get; set; }
    }
    public class Tkd_NoticeDto
    {
        public int NoticeCdSeq { get; set; }
        public string NoticeContent { get; set; }
        public byte NoticeDisplayKbn { get; set; }
        public byte SiyoKbn { get; set; }
        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
        public int UpdSyainCd { get; set; }
        public string UpdPrgId { get; set; }
        public string SyainNm { get; set; }
    }

    public class NoticeDisplayKbnDto
    {
        public string CodeKbn { get; set; }
        public string RyakuNm { get; set; }
    }
}
