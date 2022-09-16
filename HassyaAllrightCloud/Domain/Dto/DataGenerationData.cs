using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class DataGenerationData
    {
        public TPM_CodeKbCodeSyuData CodeKbnItem { get; set; }
        public TPM_GeneralOutPutData OutPutUseType { get; set; }
        public List<TPM_GeneralOutPutWhereData> OutPutWhereItem { get; set; }
        public string HeadingOutput { get; set; }
        public string Separator { get; set; }
        public string GroupType { get; set; }
        public string EnclosedCharacters { get; set; }
    }
}
