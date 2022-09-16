using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using HassyaAllrightCloud.Pages.Components.InputFileComponent;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class FileSendData
    {
        public int Id { get; set; }
        public IFileListEntry File { get; set; }
    }
}
