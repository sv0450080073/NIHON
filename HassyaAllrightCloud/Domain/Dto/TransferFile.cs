using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    namespace FileSharing.Models
    {
        public class TransferFile
        {
            public string Name { get; set; }
            public byte[] Content { get; set; }
        }
    }

}
