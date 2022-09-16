using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class FileStorageData
    {
        public int FileSendDataId { get; set; }
        public FileSendData FileSendData { get; set; }
        public string FileName { get; set; }
    }
}
