using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class FileInfoData
    {
		public int TenantCdSeq { get; set; }
		public string UkeNo { get; set; }
		public short UnkRen { get; set; }
		public int FileNo { get; set; }
		public string FileNm { get; set; }
		public byte[] File { get; set; }
		public int FileSize { get; set; }
		public string FolderId { get; set; }
		public int SiyoKbn { get; set; }
		public string FileId { get; set; }
		public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
	}
}
