using HassyaAllrightCloud.Commons.Constants;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class TPM_GeneralOutPutData
    {
        public int OutPutCd { get; set; }
        public int OutPutUseType { get; set; }
        public string OutPutNm { get; set; }
        public string OutPutSQL { get; set; }
    }

    public class TPM_GeneralOutPutWhereData
    {
        public int ID { get; set; }
        public int OutPutCd { get; set; }
        public int OutWhereNo { get; set; }        
        public string WhereNm { get; set; }
        public int WhereControl { get; set; }
        public int WhereType { get; set; }
        public byte WhereRequiredFlg { get; set; }
        public int WhereLength { get; set; }
        public string WhereDefault1 { get; set; }
        public string WhereDefault2 { get; set; }
        public OutPutWhere OutPutWhereType { get; set; }        
    }
}
