using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class RepairDivision
    {
        public int RepairCdSeq { get; set; }
        public int RepairCd { get; set; }
        public string RepairRyakuNm { get; set; }
        public string RepairText
        {
            get
            {
                if (RepairCdSeq == 0 || RepairCd == 0)
                    return string.Empty;
                return $"{RepairCd.ToString("D5")}：{RepairRyakuNm}";
            }
        }
    }
}
